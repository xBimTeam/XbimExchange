using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using Xbim.COBie.Resources;
using System.Diagnostics;
using Xbim.IO;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Kernel;
using Xbim.COBie.Contracts;

namespace Xbim.COBie
{
    [Serializable()]
    public class COBieSheet<T> : ICOBieSheet<T> where T : COBieRow
    {
        

        #region Private Fields
        private Dictionary<int, COBieColumn> _columns;
        private COBieErrorCollection _errors = new COBieErrorCollection();
        private Dictionary<string, HashSet<string>> _indices;
        private ErrorRowIndexBase _errorRowIdx; //report error row based with row stating at one(excel), or two (data table)
        #endregion


        #region Properties
        public string SheetName { get; private set; }
        public List<T> Rows { get; private set; }
        public List<T> RowsRemoved { //rows removed by merge rules
            get {
                if (_RowsRemoved == null) { _RowsRemoved = new List<T>(); }
                return _RowsRemoved; 
            } 
            private set { 
                _RowsRemoved = value; 
            } 
        } 
        private List<T> _RowsRemoved; //rows removed by merge rules

        public Dictionary<int, COBieColumn> Columns { get { return _columns; } }
        public Dictionary<string, HashSet<string>> Indices 
        {  
            get
            {
                //check we have built it
                if (_indices.Count() == 0)
                {
                    BuildIndices();
                }
                return _indices; 
            } 
        }
        public COBieErrorCollection Errors  { get { return _errors; } }
        public IEnumerable<COBieColumn> KeyColumns
        {
            get
            {
                return Columns.Where(c => COBieKeyType.CompoundKey.Equals(c.Value.KeyType)
                                          || COBieKeyType.CompoundKey_ForeignKey.Equals(c.Value.KeyType)
                                          || COBieKeyType.PrimaryKey.Equals(c.Value.KeyType)).Select(c => c.Value);
            }
        }
        public IEnumerable<COBieColumn> ForeignKeyColumns
        {
            get 
            { 
                return Columns.Where(c => COBieKeyType.ForeignKey.Equals(c.Value.KeyType) 
                                          || COBieKeyType.CompoundKey_ForeignKey.Equals(c.Value.KeyType)).Select(c => c.Value); 
            }
        } 

        public IEnumerable<T> RemovedRows
        {
            get
            {
                return RowsRemoved.AsEnumerable();
            }
        }
        public T this[int index] { get { return Rows[index]; } }

        public int RowCount { get { return Rows.Count; } }

        public object SheetMetaData { get; set; }

        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sheetName">Sheet name for this sheet object</param>
        public COBieSheet(string sheetName)
        {
            Rows = new List<T>();
            RowsRemoved = new List<T>();
            _indices = new Dictionary<string, HashSet<string>>();
            SheetName = sheetName;
            PropertyInfo[]  properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.GetSetMethod() != null).ToArray();
            _columns = new Dictionary<int, COBieColumn>();
            // add column info 
            foreach (PropertyInfo propInfo in properties)
            {
                object[] attrs = propInfo.GetCustomAttributes(typeof(COBieAttributes), true);
                if (attrs != null && attrs.Length > 0)
                {
                    COBieAttributes attr = (COBieAttributes)attrs[0];
                    List<string> aliases = GetAliases(propInfo);
                    _columns.Add(attr.Order, new COBieColumn(propInfo, attr, aliases));
                }
            }
            
        }

        #region Methods
        
        /// <summary>
        /// Create a COBieRow of the correct type for this sheet, not it is not added to the Rows list
        /// </summary>
        /// <returns>Correct COBieRow type for this COBieSheet</returns>
        public T AddNewRow()
        {
           Object[] args = {this};
           AddRow((T)Activator.CreateInstance(typeof(T), args));
           return Rows.Last();
        }

        public void AddRow(T cOBieRow)
        {
            cOBieRow.RowNumber = Rows.Count() + 1;
            Rows.Add(cOBieRow);
        }

        public void AddRow(COBieRow cOBieRow)
        {
            cOBieRow.RowNumber = Rows.Count() + 1;
            Rows.Add((T)cOBieRow);
        }

        /// <summary>
        /// Add COBieRow to the the Removed Rows list
        /// </summary>
        /// <param name="cOBieRow"></param>
        public void AddRemovedRow(T cOBieRow)
        {
            cOBieRow.RowNumber = RowsRemoved.Count() + 1;
            RowsRemoved.Add(cOBieRow);
        }

        public void AddRemovedRow(COBieRow cOBieRow)
        {
            cOBieRow.RowNumber = RowsRemoved.Count() + 1;
            RowsRemoved.Add((T)cOBieRow);
        }
        
        

        /// <summary>
        /// Get the alias attribute name values and add to a list of strings
        /// </summary>
        /// <param name="propInfo">PropertyInfo for the column field</param>
        /// <returns>List of strings</returns>
        private List<string> GetAliases(PropertyInfo propInfo)
        {
            object[] attrs = propInfo.GetCustomAttributes(typeof(COBieAliasAttribute), true);

            if (attrs != null && attrs.Length > 0)
                return attrs.Cast<COBieAliasAttribute>().Select(s => s.Name).ToList<string>();
            else 
                return new List<string>();
        }

        /// <summary>
        /// Set the initial hash code for each row in the sheet, i.e when the workbook is created
        /// </summary>
        public void SetRowsHashCode()
        {
            foreach (COBieRow row in Rows)
            {
                //SetThe initial has vale for each row
                row.SetInitialRowHash();
            }
        }

        private HashSet<string> RowHashs = new HashSet<string>();
        /// <summary>
        /// See if passed in hash code exists in the sheet
        /// </summary>
        /// <param name="hash">string, hash code to test</param>
        /// <param name="addHash">add passed in hash to RowHash, i.e if you are going to add th row to the sheet if result of function is false</param>
        /// <returns>bool</returns>
        public bool HasMergeHashCode(string hash, bool addHash)
        {
            if (RowHashs.Count == 0)
            {
                foreach (COBieRow row in Rows)
                {
                    RowHashs.Add(row.RowMergeHashValue);
                }
            }
            if (RowHashs.Contains(hash))
            {
                return true;
            }
            else
            {
                if (addHash) RowHashs.Add(hash);
                return false;
            }
        }

        /// <summary>
        /// Build Indexed dictionaries of values in each Keyed Columns.
        /// </summary>
        /// <remarks>Permits optimised validation</remarks>
        public void BuildIndices()
        {
            // Add Indices first. We may have no rows of data, but should still have indices
            foreach (COBieColumn column in KeyColumns)
            {
                if (!_indices.ContainsKey(column.ColumnName)) //no column key so add to dictionary
                    _indices.Add(column.ColumnName, new HashSet<string>());
            }

            foreach (COBieRow row in Rows)
            {
                foreach (COBieColumn cobieColumn in KeyColumns)
                {
                    string columnName = cobieColumn.ColumnName;
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        string rowValue = row[columnName].CellValue.Trim();
                        if (rowValue != null)
                        {
                            if (!string.IsNullOrEmpty(rowValue))
                            {
                                if (!_indices[columnName].Contains(rowValue)) //add value to HashSet, if not existing
                                    _indices[columnName].Add(rowValue);
                            } 
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validate the sheet
        /// </summary>
        /// <param name="workbook"></param>
        public void Validate(COBieWorkbook workbook, ErrorRowIndexBase errorRowIdx, ICOBieSheetValidationTemplate SheetValidator)
        {
            _errorRowIdx = errorRowIdx; //set the index for error reporting on rows
            _errors.Clear();

            ValidatePrimaryKeysUnique(SheetValidator);
            ValidateFields(SheetValidator);
            ValidateForeignKeys(workbook, SheetValidator);
        }

        /// <summary>
        /// Validate component sheet for merge types depending on the role of the model this worksheet was built from
        /// </summary>
        /// <param name="model">model the cobie file was generated from</param>
        /// <param name="fileRoles">the file roles</param>
        public List<string> ValidateComponentMerge(XbimModel model, COBieMergeRoles fileRoles) 
        {
            List<string> typeObjectGlobalId = new List<string>();
            List<string> typeObjectGlobalIdKeep = new List<string>();
            //RowsRemoved.Clear();
            if (fileRoles != COBieMergeRoles.Unknown) //if role is a single value of unknown then do no merging
            {
                COBieColumn colExtObj = Columns.Where(c => c.Value.ColumnName == "ExtObject").Select(c => c.Value).FirstOrDefault();
                COBieColumn colExtId = Columns.Where(c => c.Value.ColumnName == "ExtIdentifier").Select(c => c.Value).FirstOrDefault();
                
                List<IfcElement> elements = model.InstancesLocal.OfType<IfcElement>().ToList(); //get all IfcElements, 
                
                List<T> RemainRows = new List<T>();
                if (colExtObj != null)
                {
                    FilterValuesOnMerge mergeHelper = new FilterValuesOnMerge();

                    for (int i = 0; i < Rows.Count; i++)
                    {
                        COBieRow row = Rows[i];//.ElementAt(i);
                        COBieCell cell = row[colExtObj.ColumnOrder];
                        string extObject = cell.CellValue;
                        if (mergeHelper.Merge(extObject)) //object can be tested on
                        {
                            COBieCell cellExtId = row[colExtId.ColumnOrder];
                            string extId = cellExtId.CellValue;

                            IfcElement IfcElement = elements.Where(ie => ie.GlobalId.ToString() == extId).FirstOrDefault();
                            if (IfcElement != null)
                            {
                                //we need to remove the ObjectType from the type sheet
                                IfcRelDefinesByType elementDefinesByType = IfcElement.IsDefinedBy.OfType<IfcRelDefinesByType>().FirstOrDefault(); //should be only one
                                IfcTypeObject elementType = null;
                                if (elementDefinesByType != null)
                                    elementType = elementDefinesByType.RelatingType;

                                if (mergeHelper.Merge(IfcElement, fileRoles))
                                {
                                    RemainRows.Add((T)row);
                                    if ((elementType != null) && (!typeObjectGlobalIdKeep.Contains(elementType.GlobalId)))
                                        typeObjectGlobalIdKeep.Add(elementType.GlobalId);
                                }
                                else
                                {
                                    RowsRemoved.Add((T)row);
                                    if ((elementType != null) && (!typeObjectGlobalId.Contains(elementType.GlobalId)))
                                        typeObjectGlobalId.Add(elementType.GlobalId);
                                }
                            }
                            else
                                RemainRows.Add((T)row); //cannot evaluate IfcType so keep
                        }
                    }
                    Rows = RemainRows;
                } 
            }
            
            typeObjectGlobalId.RemoveAll(Id => typeObjectGlobalIdKeep.Contains(Id)); //ensure we remove any type we have kept from the type object GlobalId list (ones to remove)
            return typeObjectGlobalId;
        }


        /// <summary>
        /// Validate type sheet for merge types depending on the role of the model this worksheet was built from
        /// </summary>
        ///<param name="GlobalIds">List of GlobalId's</param>
        ///<returns>Number of rows removed</returns>
        public int ValidateTypeMerge(List<string> GlobalIds)
        {
            COBieColumn colExtId = Columns.Where(c => c.Value.ColumnName == "ExtIdentifier").Select(c => c.Value).FirstOrDefault();
            List<T> RemainRows = new List<T>();
            if (colExtId != null)
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    COBieRow row = Rows[i];
                    COBieCell cell = row[colExtId.ColumnOrder];
                    string extId = cell.CellValue;
                    if (GlobalIds.Contains(extId))
                        RowsRemoved.Add((T)row);
                    else
                        RemainRows.Add((T)row);
                }
                Rows = RemainRows;
            }
            return RowsRemoved.Count;
        }



        /// <summary>
        ///  Validate attribute sheet for merge types depending on the role of the model this worksheet was built from
        /// </summary>
        /// <param name="keys">string list holding the sheetname and name property concatenated together</param>
        /// <returns>Number removed</returns>
        public int ValidateAttributeMerge(List<string> keys)
        {
            COBieColumn colSheet = Columns.Where(c => c.Value.ColumnName == "SheetName").Select(c => c.Value).FirstOrDefault();
            COBieColumn colName = Columns.Where(c => c.Value.ColumnName == "RowName").Select(c => c.Value).FirstOrDefault();
            List<T> RemainRows = new List<T>();
            if ((colSheet != null) && (colName != null))
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    COBieRow row = Rows[i];
                    COBieCell cellSheet = row[colSheet.ColumnOrder];
                    string sheetName = cellSheet.CellValue;
                    COBieCell cellName = row[colName.ColumnOrder];
                    string rowName = cellName.CellValue;
                    if (keys.Contains(sheetName + rowName))
                        RowsRemoved.Add((T)row);
                    else
                        RemainRows.Add((T)row);
                }
                Rows = RemainRows;
            }
            return RowsRemoved.Count;
        }

  
        /// <summary>
        ///  Validate system sheet for merge types in ComponentName, depending on the role of the model this worksheet was built from
        /// </summary>
        /// <param name="names">string list holding the name properties removed from the component sheet</param>
        /// <returns>Number removed</returns>
        public int ValidateSystemMerge(List<string> names)
        {
            COBieColumn compName = Columns.Where(c => c.Value.ColumnName == "ComponentNames").Select(c => c.Value).FirstOrDefault();
            List<T> RemainRows = new List<T>();
            if (compName != null)
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    COBieRow row = Rows[i];
                    COBieCell cell = row[compName.ColumnOrder];
                    string componentName = cell.CellValue;
                    if (names.Contains(componentName))
                        RowsRemoved.Add((T)row);
                    else
                        RemainRows.Add((T)row);
                }
                Rows = RemainRows;
            }
            return RowsRemoved.Count;
        }

        /// <summary>
        /// Validate the existence of the Foreign Key value on the referencing sheet, if not add error
        /// </summary>
        /// <param name="context">COBieContext object holding global values for this model</param>
        private void ValidateForeignKeys(COBieWorkbook workbook, ICOBieSheetValidationTemplate SheetValidator)
        {
            int rowIndex = 1;
            foreach (COBieRow row in Rows)
            {
                foreach (COBieColumn foreignKeyColumn in ForeignKeyColumns)
                {
                    // TODO: COBieColumn should own the relationship rather than creating a new one each time.
                    COBieColumnRelationship cobieReference = new COBieColumnRelationship(workbook, foreignKeyColumn);

                    if (SheetValidator != null && SheetValidator.IsRequired.ContainsKey(foreignKeyColumn.ColumnOrder) &&
                        SheetValidator.IsRequired[foreignKeyColumn.ColumnOrder])
                    {
                        if (!string.IsNullOrEmpty(foreignKeyColumn.ReferenceColumnName))
                        {

                            COBieCell cell = row[foreignKeyColumn.ColumnOrder];

                            string foreignKeyValue = cell.CellValue;

                            // Don't validate nulls. Will be reported by the Foreign Key null value check, so just skip here 
                            if (!string.IsNullOrEmpty(foreignKeyValue))
                            {
                                bool isValid = false;

                                bool isPickList = (cobieReference.SheetName == Constants.WORKSHEET_PICKLISTS);

                                if (isPickList)
                                {
                                    isValid = PickListMatch(cobieReference, cell);
                                }
                                else
                                {
                                    isValid = ForeignKeyMatch(cobieReference, cell);
                                }
                                //report no match
                                if (!isValid)
                                {
                                    string errorDescription = BuildErrorMessage(cobieReference, isPickList);

                                    COBieError.ErrorTypes errorType = isPickList == true
                                        ? COBieError.ErrorTypes.PickList_Violation
                                        : COBieError.ErrorTypes.ForeignKey_Violation;

                                    COBieError error = new COBieError(SheetName, foreignKeyColumn.ColumnName,
                                        errorDescription, errorType,
                                        COBieError.ErrorLevels.Error, row.InitialRowHashValue,
                                        foreignKeyColumn.ColumnOrder, rowIndex);
                                    _errors.Add(error);
                                }
                            }
                        }
                    }
                }
                rowIndex++;
            }

        }

        private static string BuildErrorMessage(COBieColumnRelationship cobieReference, bool isPickList)
        {
            string errFieldName = cobieReference.ColumnName;
            //get the correct Pick list column name depending on template for the category columns only, for now

            if (isPickList && (cobieReference.ColumnName.Contains("Category")))
                errFieldName = ErrorDescription.ResourceManager.GetString(cobieReference.ColumnName.Replace("-", "")); //strip out the "-" to get the resource, (resource did not like the '-' in the name)
            if (string.IsNullOrEmpty(errFieldName)) //if resource not found then reset back to field name
                errFieldName = cobieReference.ColumnName;

            string errorDescription = String.Format(ErrorDescription.PickList_Violation, cobieReference.SheetName, errFieldName);
            return errorDescription;
        }

        /// <summary>
        /// Match the Foreign Key with the primary key field
        /// </summary>
        /// <param name="reference">The COBie Index to cross reference</param>
        /// <param name="cell">The COBie Cell to validate</param>
        /// <returns>bool</returns>
        private bool ForeignKeyMatch(COBieColumnRelationship reference, COBieCell cell)
        {
            if(reference.HasKeyMatch(cell.CellValue))
                return true;

            if (cell.COBieColumn.AllowsMultipleValues == true)
            {
                foreach (string value in cell.CellValues)
                {
                    if (!reference.HasKeyMatch(value))
                        return false;
                }
                return true;
            }

            return false;

        }

        
       
        /// <summary>
        /// Match either side of a : delimited string or all of the string including the delimiter
        /// </summary>
        /// <param name="hashSet">List of strings</param>
        /// <param name="foreignKeyValue">string to match</param>
        /// <returns>true if a match, false if none</returns>
        private bool PickListMatch(COBieColumnRelationship reference, COBieCell cell)
        {
            if (cell.CellValue == Constants.DEFAULT_STRING) 
                return false;

            if (reference.HasKeyMatch(cell.CellValue))
                return true;

            // There are no current cases where PickLists can have Many to Many mappings - only One to Many. So don't worry about MultipleValues.

            // Due to the way some Categories/Classifications in Pick lists are compound keys (e.g. 11-11 11 14: Exhibition Hall ... where the code and name are stored separately in IFC)
            // we need to special case partial matches, since we may have the code, name, or code:name (perhaps with differing white space)

            if (cell.CellValue.Contains(":")) //assume category split
            {
                return reference.HasPartialMatch(cell.CellValue, ':');  
            }
            

            return false;
        }


        /// <summary>
        /// Validate the row columns against the attributes set for each column 
        /// </summary>
        public void ValidateFields(ICOBieSheetValidationTemplate SheetValidator)
        {
            int r = 0;
            foreach(T row in Rows)
            {
                r++;
                for(int col = 0 ; col < row.RowCount ; col++)
                {
                    
                    var cell = row[col];
                    COBieAttributeState state = cell.COBieColumn.AttributeState;
                    var errorLevel = COBieError.ErrorLevels.Warning;

                    if (SheetValidator != null && SheetValidator.IsRequired.ContainsKey(col) && SheetValidator.IsRequired[col])
                    {
                        // Override required state based on validation template
                        if ((SheetValidator.IsRequired.ContainsKey(col) && SheetValidator.IsRequired[col]))
                        {
                            errorLevel = COBieError.ErrorLevels.Error;
                            switch (state)
                            {
                                case COBieAttributeState.Required_IfSpecified:
                                    state = COBieAttributeState.Required_Information;
                                    break;

                                case COBieAttributeState.Required_System_IfSpecified:
                                    state = COBieAttributeState.Required_System;
                                    break;

                                case COBieAttributeState.Required_PrimaryKey_IfSpecified:
                                    state = COBieAttributeState.Required_PrimaryKey;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    COBieError errNull = GetCobieFieldNullError(row[col], state, errorLevel, SheetName, r, col, row.InitialRowHashValue);
                    if (errNull != null)
                    {
                        _errors.Add(errNull);
                    }
                    else //passed null check so check format
                    {
                        errNull = GetCOBieFieldOutOfBoundsError(row[col], state, errorLevel, SheetName, r, col, row.InitialRowHashValue);
                        if (errNull != null)
                        {
                            _errors.Add(errNull);
                        }
                        errNull = GetCOBieFieldFormatError(row[col], state, errorLevel, SheetName, r, col, row.InitialRowHashValue);
                        if (errNull != null)
                        {
                            _errors.Add(errNull);
                        }
                    }
                }
             
            }
        }

        /// <summary>
        /// Validate the Primary Keys only exist once in the sheet 
        /// </summary>
        private void ValidatePrimaryKeysUnique(ICOBieSheetValidationTemplate SheetValidator)
        {
        var dupes = Rows
                    .Select((v, i) => new { row = v, index = i }) //get COBieRow and its index in the list
                    .GroupBy(r => r.row.GetPrimaryKeyValue.ToLower().Trim(), (key, group) => new {rowkey = key, rows = group }) //group by the primary key value(s) joint as a delimited string
                    .Where(grp => grp.rows.Count() > 1); 
                 

            List<string> keyColList = new List<string>();
            foreach (COBieColumn col in KeyColumns)
            {
                keyColList.Add(col.ColumnName);
            }
            string keyCols = string.Join(",", keyColList);

            //set the index for the reported error row numbers
            int errorRowInc = 2; //default for rows starting at row two - ErrorRowIndexBase.Two
            if (_errorRowIdx == ErrorRowIndexBase.RowOne) //if error row starting sow set the the row numbered one
            {
                errorRowInc = 1; 
            }

            foreach (var dupe in dupes)
            {
                List<string> indexList = new List<string>();
                
                foreach (var row in dupe.rows)
                {
                    if (SheetValidator != null && SheetValidator.IsRequired.ContainsKey(row.index) && SheetValidator.IsRequired[row.index]){
                        indexList.Add((row.index + errorRowInc).ToString());
                    }
                }
                string rowIndexList = string.Join(",", indexList);
                foreach (var row in dupe.rows)
                {
                    if (SheetValidator != null && SheetValidator.IsRequired.ContainsKey(row.index) &&
                        SheetValidator.IsRequired[row.index])
                    {
                        string errorDescription = String.Format(ErrorDescription.PrimaryKey_Violation, keyCols,
                            rowIndexList);
                        COBieError error = new COBieError(SheetName, keyCols, errorDescription,
                            COBieError.ErrorTypes.PrimaryKey_Violation,
                            COBieError.ErrorLevels.Error, row.row.InitialRowHashValue, KeyColumns.First().ColumnOrder,
                            (row.index + 1));
                        _errors.Add(error);
                    }

                }
                
                
            }

        }
        /// <summary>
        /// Validating of the column COBieAttributeState attributes for null or n/a values
        /// </summary>
        /// <param name="cell">COBieCell</param>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <param name="initialRowHash">Initial row hash value</param>
        /// <returns>COBieError or null</returns>
        private COBieError GetCobieFieldNullError(COBieCell cell, COBieAttributeState state, COBieError.ErrorLevels errorLevel, string sheetName, int row, int col, string initialRowHash)
        {
            COBieError err = new COBieError(sheetName, cell.COBieColumn.ColumnName, "", COBieError.ErrorTypes.None, COBieError.ErrorLevels.None, initialRowHash, col, row);
      
            if ((string.IsNullOrEmpty(cell.CellValue)) ||
                (cell.CellValue == Constants.DEFAULT_STRING)
                )
            {
                switch (state)
                {
                    case COBieAttributeState.Required_PrimaryKey:
                    case COBieAttributeState.Required_CompoundKeyPart:
                        err.ErrorDescription = ErrorDescription.PrimaryKey_Violation;
                        err.ErrorType = COBieError.ErrorTypes.PrimaryKey_Violation;
                        err.ErrorLevel = errorLevel;
                        break;
                    case COBieAttributeState.Required_Information:
                    case COBieAttributeState.Required_System:
                        switch (cell.COBieColumn.AllowedType)
                        {
                            case COBieAllowedType.AlphaNumeric:
                                err.ErrorDescription = ErrorDescription.AlphaNumeric_Value_Expected;
                                err.ErrorType = COBieError.ErrorTypes.AlphaNumeric_Value_Expected;
                                break;
                            case COBieAllowedType.Email:
                                err.ErrorDescription = ErrorDescription.Email_Value_Expected;
                                err.ErrorType = COBieError.ErrorTypes.Email_Value_Expected;
                                break;
                            case COBieAllowedType.ISODateTime:
                            case COBieAllowedType.ISODate:
                                err.ErrorDescription = ErrorDescription.ISODate_Value_Expected;
                                err.ErrorType = COBieError.ErrorTypes.ISODate_Value_Expected;
                                break;
                            case COBieAllowedType.Numeric:
                                err.ErrorDescription = ErrorDescription.Numeric_Value_Expected;
                                err.ErrorType = COBieError.ErrorTypes.Numeric_Value_Expected;
                                break;
                            case COBieAllowedType.AnyType:
                            case COBieAllowedType.Text:
                                err.ErrorDescription = ErrorDescription.Text_Value_Expected;
                                err.ErrorType = COBieError.ErrorTypes.Text_Value_Expected;
                                break;
                            default:
                                err.ErrorDescription = ErrorDescription.Text_Value_Expected;
                                err.ErrorType = COBieError.ErrorTypes.Text_Value_Expected;
                                break;
                        }
                        err.ErrorLevel = errorLevel;
                        //err.ErrorLevel = COBieError.ErrorLevels.Warning; //set as a warning
                        break;
                    case COBieAttributeState.Required_Reference_PrimaryKey:
                    case COBieAttributeState.Required_Reference_PickList:
                    case COBieAttributeState.Required_Reference_ForeignKey:
                        err.ErrorDescription = ErrorDescription.Null_ForeignKey_Value;
                        err.ErrorType = COBieError.ErrorTypes.Null_ForeignKey_Value;
                        err.ErrorLevel = errorLevel;
                        break;

                    case COBieAttributeState.Required_IfSpecified:
                    case COBieAttributeState.Required_System_IfSpecified:
                    case COBieAttributeState.Required_PrimaryKey_IfSpecified:
                        if (cell.CellValue == Constants.DEFAULT_STRING)
                            return null; //if a required value but not required in validation template then do not class as error
                        break;
                    default:
                        return null;
                }
                if (err.ErrorType != COBieError.ErrorTypes.None)
                {
                    err.FieldValue = cell.CellValue;
                    return err;
                }
            }
           return null;
        }

        /// <summary>
        /// check for Field format Error in passed cell
        /// </summary>
        /// <param name="cell">COBieCell</param>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <param name="initialRowHash">Initial row hash value</param>
        /// <returns>COBieError or null</returns>
        public COBieError GetCOBieFieldFormatError(COBieCell cell, COBieAttributeState state, COBieError.ErrorLevels errorLevel, string sheetName, int row, int col, string initialRowHash)
        {
            COBieError err = new COBieError(sheetName, cell.COBieColumn.ColumnName, "", COBieError.ErrorTypes.None, COBieError.ErrorLevels.None, initialRowHash, col, row);
      

            int maxLength = cell.COBieColumn.ColumnLength;
            COBieAllowedType allowedType = cell.COBieColumn.AllowedType;
                        
            if ((state == COBieAttributeState.Required_IfSpecified) ||
                (state == COBieAttributeState.Required_System) ||
                (state == COBieAttributeState.Required_System_IfSpecified)
                ) //if a required value but marked as n/a then do not class as error
            {
                if (cell.CellValue == Constants.DEFAULT_STRING)
                    return null;
            }

            //check cell.COBieColumn.AllowedType for format errors
            switch (allowedType)
            {
                case COBieAllowedType.AlphaNumeric:
                    if (!cell.IsAlphaNumeric())
                    {
                        err.ErrorDescription = ErrorDescription.AlphaNumeric_Value_Expected;
                        err.ErrorType = COBieError.ErrorTypes.AlphaNumeric_Value_Expected;
                    }
                    break;
                case COBieAllowedType.Email:
                    if (!cell.IsEmailAddress())
                    {
                        err.ErrorDescription = ErrorDescription.ISODate_Value_Expected;
                        err.ErrorType = COBieError.ErrorTypes.ISODate_Value_Expected;
                    }
                    break;
                case COBieAllowedType.ISODate:
                case COBieAllowedType.ISODateTime:
                    if (!cell.IsDateTime())
                    {
                        err.ErrorDescription = ErrorDescription.ISODate_Value_Expected;
                        err.ErrorType = COBieError.ErrorTypes.ISODate_Value_Expected;
                    }
                    break;
                case COBieAllowedType.Numeric:
                    if (!cell.IsNumeric())
                    {
                        err.ErrorDescription = ErrorDescription.Numeric_Value_Expected;
                        err.ErrorType = COBieError.ErrorTypes.Numeric_Value_Expected;
                    }
                    break;
                case COBieAllowedType.AnyType:
                case COBieAllowedType.Text:
                    if (!cell.IsText())
                    {
                        err.ErrorDescription = ErrorDescription.Text_Value_Expected;
                        err.ErrorType = COBieError.ErrorTypes.Text_Value_Expected;
                    }
                    break;
                default: 
                    break;
            }
            if (err.ErrorType != COBieError.ErrorTypes.None)
            {
                err.FieldValue = cell.CellValue;
                if (err.ErrorLevel == COBieError.ErrorLevels.None) //if set above, just in case we do set above
                    err.ErrorLevel = errorLevel;
                return err;
            } 
            return null;
        }

        /// <summary>
        /// Check for Field format length
        /// </summary>
        /// <param name="cell">COBieCell</param>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <param name="initialRowHash">Initial row hash value</param>
        /// <returns>COBieError or null</returns>
        public COBieError GetCOBieFieldOutOfBoundsError(COBieCell cell, COBieAttributeState state, COBieError.ErrorLevels errorLevel, string sheetName, int row, int col, string initialRowHash)
        {
            COBieError err = new COBieError(sheetName, cell.COBieColumn.ColumnName, "", COBieError.ErrorTypes.None, COBieError.ErrorLevels.None, initialRowHash, col, row);

            int maxLength = cell.COBieColumn.ColumnLength;

            if (cell.CellValue.Length > maxLength)
            {
                err.ErrorDescription = String.Format(ErrorDescription.Value_Out_of_Bounds, maxLength);
                err.ErrorType = COBieError.ErrorTypes.Value_Out_of_Bounds;
                err.ErrorLevel = errorLevel; // GetErroLevel(cell.COBieColumn.AttributeState); 
                err.FieldValue = cell.CellValue;
                return err;
            }
            return null;
        }

        /// <summary>
        /// Get ErrorLevel base on the COBieAttributeState Attribute
        /// </summary>
        /// <param name="state">COBieAttributeState</param>
        /// <returns>COBieError.ErrorLevels enumeration</returns>
        public COBieError.ErrorLevels GetErrorLevel(COBieAttributeState state)
        {
            switch (state)
            {
                case COBieAttributeState.Required_PrimaryKey:
                case COBieAttributeState.Required_CompoundKeyPart:
                case COBieAttributeState.Required_Reference_ForeignKey:
                case COBieAttributeState.Required_Reference_PrimaryKey:
                case COBieAttributeState.Required_Reference_PickList:
                case COBieAttributeState.Required_Information:
                    return COBieError.ErrorLevels.Error;
                case COBieAttributeState.Required_System:
                case COBieAttributeState.Required_IfSpecified:
                case COBieAttributeState.Required_System_IfSpecified:
                case COBieAttributeState.Required_PrimaryKey_IfSpecified:
                    return COBieError.ErrorLevels.Warning;
                default:
                    break;
            }
            return COBieError.ErrorLevels.None;
        }
        #endregion



        internal void OrderBy(Func<T, String> func)
        {
            Rows = Rows.OrderBy(func).ToList();
            for (var i = 0; i < Rows.Count(); i++)
            {
                Rows[i].RowNumber = i + 1;
            }
        }
    } 

}
