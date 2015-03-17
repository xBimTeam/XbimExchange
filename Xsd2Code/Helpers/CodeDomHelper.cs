// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeDomHelper.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Code DOM manipulation helper methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Xsd2Code.Library.Helpers
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;

    /// <summary>
    /// Code DOM manipulation helper methods 
    /// </summary>
    internal static class CodeDomHelper
    {
        /// <summary>
        /// Add to CodeCommentStatementCollection summary documentation
        /// </summary>
        /// <param name="codeStatmentColl">Collection of CodeCommentStatement</param>
        /// <param name="comment">summary text</param>
        internal static void CreateSummaryComment(CodeCommentStatementCollection codeStatmentColl, string comment)
        {
            codeStatmentColl.Add(new CodeCommentStatement("<summary>", true));
            string[] lines = comment.Split(new[] { '\n' });
            foreach (string line in lines)
                codeStatmentColl.Add(new CodeCommentStatement(line.Trim(), true));
            codeStatmentColl.Add(new CodeCommentStatement("</summary>", true));
        }


        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="objectName">object name.</param>
        /// <param name="ctorParams">The c tor parameter.</param>
        /// <returns>return variable declaration</returns>
        internal static CodeVariableDeclarationStatement CreateObject(Type objectType, string objectName, params string[] ctorParams)
        {
            var ce = new List<CodeExpression>();

            foreach (var item in ctorParams)
                ce.Add(new CodeTypeReferenceExpression(item));

            return CreateObject(objectType, objectName, ce.ToArray());
        }

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="ctorParams">The ctor params.</param>
        /// <returns>return variable declaration</returns>
        internal static CodeVariableDeclarationStatement CreateObject(Type objectType, string objectName, params CodeExpression[] ctorParams)
        {
            return new CodeVariableDeclarationStatement(
                new CodeTypeReference(objectType),
                objectName,
                new CodeObjectCreateExpression(new CodeTypeReference(objectType), ctorParams));
        }

        /// <summary>
        /// Get CodeMethodInvokeExpression
        /// </summary>
        /// <param name="targetObject">Name of target object. Use this if empty</param>
        /// <param name="methodName">Name of method to invoke</param>
        /// <returns>CodeMethodInvokeExpression value</returns>
        internal static CodeMethodInvokeExpression GetInvokeMethod(string targetObject, string methodName)
        {
            return GetInvokeMethod(targetObject, methodName, null);
        }

        /// <summary>
        /// Get CodeMethodInvokeExpression
        /// </summary>
        /// <param name="targetObject">Name of target object. Use this if empty</param>
        /// <param name="methodName">Name of method to invoke</param>
        /// <param name="parameters">method params</param>
        /// <returns>CodeMethodInvokeExpression value</returns>
        internal static CodeMethodInvokeExpression GetInvokeMethod(string targetObject, string methodName, CodeExpression[] parameters)
        {
            var methodInvoke =
                parameters != null
                    ? new CodeMethodInvokeExpression(
                          new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(targetObject), methodName), parameters)
                    : new CodeMethodInvokeExpression(
                          new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(targetObject), methodName));

            return methodInvoke;
        }

        /// <summary>
        /// Getr return true statment
        /// </summary>
        /// <returns>statment of return code</returns>
        internal static CodeMethodReturnStatement GetReturnTrue()
        {
            return new CodeMethodReturnStatement(new CodePrimitiveExpression(true));
        }

        /// <summary>
        /// Get return false startment
        /// </summary>
        /// <returns>statment of return code</returns>
        internal static CodeMethodReturnStatement GetReturnFalse()
        {
            return new CodeMethodReturnStatement(new CodePrimitiveExpression(false));
        }

        /// <summary>
        /// Return catch statments
        /// </summary>
        /// <returns>CodeCatchClause statments</returns>
        internal static CodeCatchClause[] GetCatchClause()
        {
            var catchStatmanents = new CodeStatement[2];

            catchStatmanents[0] = new CodeAssignStatement(
                new CodeVariableReferenceExpression("exception"),
                new CodeVariableReferenceExpression("ex"));

            catchStatmanents[1] = GetReturnFalse();
            var catchClause = new CodeCatchClause(
                                                    "ex",
                                                    new CodeTypeReference(typeof(Exception)),
                                                    catchStatmanents);

            var catchClauses = new[] { catchClause };
            return catchClauses;
        }

        /// <summary>
        /// Gets the throw clause.
        /// </summary>
        /// <returns>return catch...throw statment</returns>
        internal static CodeCatchClause[] GetThrowClause()
        {
            var catchStatmanents = new CodeStatementCollection();
            catchStatmanents.Add(new CodeThrowExceptionStatement(new CodeVariableReferenceExpression("ex")));
            var catchClause = new CodeCatchClause(
                                                    "ex",
                                                    new CodeTypeReference(typeof(Exception)),
                                                    catchStatmanents.ToArray());

            var catchClauses = new[] { catchClause };
            return catchClauses;
        }

        /// <summary>
        /// Codes the STMT col to array.
        /// </summary>
        /// <param name="statmentCollection">The statment collection.</param>
        /// <returns>return CodeStmtColToArray</returns>
        internal static CodeStatement[] CodeStmtColToArray(CodeStatementCollection statmentCollection)
        {
            var tryFinallyStatmanents = new CodeStatement[statmentCollection.Count];
            statmentCollection.CopyTo(tryFinallyStatmanents, 0);
            return tryFinallyStatmanents;
        }

        /// <summary>
        /// Get return CodeCommentStatement comment
        /// </summary>
        /// <param name="text">Return text comment</param>
        /// <returns>return return comment statment</returns>
        internal static CodeCommentStatement GetReturnComment(string text)
        {
            var comments = new CodeCommentStatement(string.Format("<returns>{0}</returns>", text), true);
            return comments;
        }

        /// <summary>
        /// Get summary CodeCommentStatementCollection comment
        /// </summary>
        /// <param name="text">Summary text comment</param>
        /// <returns>CodeCommentStatementCollection comment</returns>
        internal static CodeCommentStatementCollection GetSummaryComment(string text)
        {
            var comments = new CodeCommentStatementCollection
                               {
                                   new CodeCommentStatement("<summary>", true),
                                   new CodeCommentStatement(text, true),
                                   new CodeCommentStatement("</summary>", true)
                               };
            return comments;
        }

        /// <summary>
        /// Get param comment statment
        /// </summary>
        /// <param name="paramName">Param Name</param>
        /// <param name="text">param summary</param>
        /// <returns>CodeCommentStatement param</returns>
        internal static CodeCommentStatement GetParamComment(string paramName, string text)
        {
            var comments = new CodeCommentStatement(string.Format("<param name=\"{0}\">{1}</param>", paramName, text), true);
            return comments;
        }

        /// <summary>
        /// Transform CodeStatementCollection into CodeStatement[]
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>array of CodeStatement</returns>
        internal static CodeStatement[] ToArray(this CodeStatementCollection collection)
        {
            CodeStatement[] cdst = null;
            if (collection != null)
            {
                cdst = new CodeStatement[collection.Count];
                collection.CopyTo(cdst, 0);
            }

            return cdst;
        }

        /// <summary>
        /// Gets the dispose.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <returns>return dispose CodeDom</returns>
        internal static CodeConditionStatement GetDispose(string objectName)
        {
            var statments = new CodeStatementCollection();
            statments.Add(GetInvokeMethod(objectName, "Dispose"));
            return
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeVariableReferenceExpression(objectName),
                        CodeBinaryOperatorType.IdentityInequality,
                        new CodePrimitiveExpression(null)),
                        statments.ToArray());
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The instance type.</param>
        /// <param name="ctorParams">The ctor params.</param>
        /// <returns>return code of new objectinstance</returns>
        internal static CodeObjectCreateExpression CreateInstance(Type type)
        {
            return new CodeObjectCreateExpression(new CodeTypeReference(type));
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The instance type.</param>
        /// <param name="ctorParams">The ctor params.</param>
        /// <returns>return code of new objectinstance</returns>
        internal static CodeObjectCreateExpression CreateInstance(Type type, params string[] ctorParams)
        {
            var ce = new List<CodeTypeReferenceExpression>();
            foreach (var item in ctorParams)
                ce.Add(new CodeTypeReferenceExpression(item));

            return CreateInstance(type, ce.ToArray());
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The instance type.</param>
        /// <param name="ctorParams">The ctor params.</param>
        /// <returns>return code of new objectinstance</returns>
        internal static CodeObjectCreateExpression CreateInstance(Type type, CodeTypeReferenceExpression[] ctorParams)
        {
            return new CodeObjectCreateExpression(new CodeTypeReference(type), ctorParams);
        }

        //DCM ADDED: Helpers to remove More CodeSnippetExpressions
        #region DCM Added 2010-01-20

        /// <summary>
        /// Get CodeMethodInvokeExpression
        /// </summary>
        /// <param name="targetObject">Name of target object. Use this if empty</param>
        /// <param name="methodName">Name of method to invoke</param>
        /// <param name="parameters">method params as variable argument array</param>
        /// <returns>CodeMethodInvokeExpression value</returns>
        /// <remarks>DCM ADDED for varArgs</remarks>
        internal static CodeMethodInvokeExpression GetInvokeMethodEx(string targetObject, string methodName, params CodeExpression[] parameters)
        {
            var methodInvoke =
                parameters != null
                    ? new CodeMethodInvokeExpression(
                          new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(targetObject), methodName), parameters)
                    : new CodeMethodInvokeExpression(
                          new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(targetObject), methodName));

            return methodInvoke;
        }

        /// <summary>
        /// Gets the Enum CodeFieldReferenceExpression.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        internal static CodeFieldReferenceExpression GetEnum(Type enumType, String fieldName)
        {
            return new CodeFieldReferenceExpression(
                 new CodeTypeReferenceExpression(enumType), fieldName);
        }

        /// <summary>
        /// Gets the enum as CodeFieldReferenceExpression.
        /// </summary>
        /// <param name="enumType">Type of the enum as string.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        internal static CodeFieldReferenceExpression GetEnum(String enumType, String fieldName)
        {
            return new CodeFieldReferenceExpression(
                 new CodeTypeReferenceExpression(enumType), fieldName);
        }

        /// <summary>
        /// Gets the Static Field CodeFieldReferenceExpression.
        /// </summary>
        /// <param name="classType">Type of the Class.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        internal static CodeFieldReferenceExpression GetStaticField(Type classType, String fieldName)
        {
            return new CodeFieldReferenceExpression(
                 new CodeTypeReferenceExpression(classType), fieldName);
        }

        /// <summary>
        /// Gets the Static Field as CodeFieldReferenceExpression.
        /// </summary>
        /// <param name="classType">Type of the Class as string.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        internal static CodeFieldReferenceExpression GetStaticField(String classType, String fieldName)
        {
            return new CodeFieldReferenceExpression(
                 new CodeTypeReferenceExpression(classType), fieldName);
        }

        /// <summary>
        /// Gets the object's named property.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        internal static CodePropertyReferenceExpression GetObjectProperty(string targetObject, string propertyName)
        {
            return new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(targetObject), propertyName);
        }

        /// <summary>
        /// Searches the CodeTypeDeclaration and Gets the name member method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal static CodeMemberMethod GetMemberMethod(CodeTypeDeclaration type, string name)
        {
            CodeMemberMethod result = null;

            foreach (CodeTypeMember member in type.Members)
            {
                if (member is CodeMemberMethod && member.Name.Equals(name))
                    result = member as CodeMemberMethod;
            }
            return result;
        }

        /// <summary>
        /// Creates the property changed method.
        /// </summary>
        /// <returns>CodeMemberMethod on Property Change handler</returns>
        /// <remarks>
        /// <code>
        ///  protected virtual  void OnPropertyChanged(string info) {
        ///      PropertyChangedEventHandler handler = PropertyChanged;
        ///      if (handler != null) {
        ///          handler(this, new PropertyChangedEventArgs(info));
        ///      }
        ///  }
        /// </code>
        /// </remarks>
        internal static CodeMemberMethod CreatePropertyChangedMethod()
        {
            const string paramName = "propertyName";
            const string variableName = "handler";

            var propertyChangedMethod = new CodeMemberMethod
            {
                Name = "OnPropertyChanged",
                Attributes = MemberAttributes.Public
            };
            propertyChangedMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(typeof(String)), paramName));

            if (GeneratorContext.GeneratorParams.TrackingChanges.Enabled && GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp)
            {
                propertyChangedMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "value"));
                // this.ChangeTracker.RecordCurrentValue(info, value);
                var changeTrackerParams = new CodeExpression[] { new CodeArgumentReferenceExpression(paramName), new CodeArgumentReferenceExpression(("value")) };
                var changeTrackerInvokeExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "ChangeTracker.RecordCurrentValue"), changeTrackerParams);
                propertyChangedMethod.Statements.Add(changeTrackerInvokeExpression);
            }

            //Declare temp variable holding the event
            var vardec = new CodeVariableDeclarationStatement(
                new CodeTypeReference(typeof(PropertyChangedEventHandler)), variableName);

            vardec.InitExpression = new CodeEventReferenceExpression(
                new CodeThisReferenceExpression(), "PropertyChanged");

            propertyChangedMethod.Statements.Add(vardec);

            //The part of the true, create the event and invoke it

            //var createArgs = new CodeObjectCreateExpression(
            //    new CodeTypeReference(typeof(PropertyChangedEventArgs)),
            //    new CodeArgumentReferenceExpression(paramName));

            var createArgs = CodeDomHelper.CreateInstance(typeof(PropertyChangedEventArgs), paramName);

            var raiseEvent = new CodeDelegateInvokeExpression(
                new CodeVariableReferenceExpression(variableName),
                new CodeThisReferenceExpression(), createArgs);

            //The Condition
            CodeExpression condition = new CodeBinaryOperatorExpression(
                new CodeVariableReferenceExpression(variableName),
                    CodeBinaryOperatorType.IdentityInequality,
                new CodePrimitiveExpression(null));

            //The if condition
            var ifTempIsNull = new CodeConditionStatement();
            ifTempIsNull.Condition = condition;
            ifTempIsNull.TrueStatements.Add(raiseEvent);

            propertyChangedMethod.Statements.Add(ifTempIsNull);
            return propertyChangedMethod;
        }

        #endregion

        /// <summary>
        /// Creates a basic property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="isXmlIgnone">if set to <c>true</c> [is XML ignone].</param>
        internal static void CreateBasicProperty(CodeTypeDeclaration type, string propertyName, Type propertyType, bool isXmlIgnone)
        {
            var field = new CodeMemberField()
                            {
                                Attributes = MemberAttributes.Final | MemberAttributes.Private,
                                Name = GetSpecifiedFieldName(propertyName),
                                Type = new CodeTypeReference(propertyType)
                            };

            type.Members.Add(field);

            var property = new CodeMemberProperty()
            {
                Attributes = MemberAttributes.Final | MemberAttributes.Public,
                Name = string.Format("{0}Specified", propertyName),
                HasGet = true,
                HasSet = true,
                Type = new CodeTypeReference(propertyType)
            };
            property.CustomAttributes.Add(new CodeAttributeDeclaration("XmlIgnore"));
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression(field.Name)));
            property.SetStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(field.Name), new CodeVariableReferenceExpression("value")));
            type.Members.Add(property);
        }

         internal static string GetSpecifiedFieldName(string propertyName)
         {
             return string.Format("{0}{1}FieldSpecified", propertyName.Substring(0, 1).ToLower(),
                                  propertyName.Substring(1, propertyName.Length - 1));
         }

        /// <summary>
        /// Finds the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="searchPropertyName">Name of the search property.</param>
        /// <returns></returns>
        internal static CodeMemberProperty FindProperty(CodeTypeDeclaration type, string searchPropertyName)
        {
            CodeMemberProperty specifiedProperty = null;
            foreach (CodeTypeMember member in type.Members)
            {
                var codeMemberProperty = member as CodeMemberProperty;
                if (codeMemberProperty != null)
                {
                    if (codeMemberProperty.Name == searchPropertyName)
                    {
                        specifiedProperty = codeMemberProperty;
                        break;
                    }
                }
            }
            return specifiedProperty;
        }

        /// <summary>
        /// Finds the field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="searchFieldName">Name of the search field.</param>
        /// <returns></returns>
        internal static CodeMemberField FindField(CodeTypeDeclaration type, string searchFieldName)
        {
            CodeMemberField specifiedProperty = null;
            foreach (CodeTypeMember member in type.Members)
            {
                var codeMember = member as CodeMemberField;
                if (codeMember != null)
                {
                    if (codeMember.Name == searchFieldName)
                    {
                        specifiedProperty = codeMember;
                        break;
                    }
                }
            }
            return specifiedProperty;
        }
    }
}
