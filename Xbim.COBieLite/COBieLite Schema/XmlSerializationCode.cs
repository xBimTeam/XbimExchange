#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
[assembly:System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
#endif

[assembly:System.Xml.Serialization.XmlSerializerVersionAttribute(ParentAssemblyId=@"cfee5e55-2e1e-45e3-ae27-c9f50421f15f,", Version=@"4.0.0.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterFacilityType : System.Xml.Serialization.XmlSerializationWriter {

        public void Write96_Facility(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"Facility", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
                return;
            }
            TopLevelElement();
            Write95_FacilityType(@"Facility", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite", ((global::Xbim.COBieLite.FacilityType)o), false, false);
        }

        void Write95_FacilityType(string n, string ns, global::Xbim.COBieLite.FacilityType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.FacilityType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"FacilityType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"FacilityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityName));
            WriteElementString(@"FacilityCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityCategory));
            Write87_ProjectType(@"ProjectAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ProjectType)o.@ProjectAssignment), false, false);
            Write86_SiteType(@"SiteAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SiteType)o.@SiteAssignment), false, false);
            if (o.@FacilityDefaultLinearUnitSpecified) {
                WriteElementString(@"FacilityDefaultLinearUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write92_LinearUnitSimpleType(((global::Xbim.COBieLite.LinearUnitSimpleType)o.@FacilityDefaultLinearUnit)));
            }
            if (o.@FacilityDefaultAreaUnitSpecified) {
                WriteElementString(@"FacilityDefaultAreaUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write93_AreaUnitSimpleType(((global::Xbim.COBieLite.AreaUnitSimpleType)o.@FacilityDefaultAreaUnit)));
            }
            if (o.@FacilityDefaultVolumeUnitSpecified) {
                WriteElementString(@"FacilityDefaultVolumeUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write94_VolumeUnitSimpleType(((global::Xbim.COBieLite.VolumeUnitSimpleType)o.@FacilityDefaultVolumeUnit)));
            }
            if (o.@FacilityDefaultCurrencyUnitSpecified) {
                WriteElementString(@"FacilityDefaultCurrencyUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write75_CurrencyUnitSimpleType(((global::Xbim.COBieLite.CurrencyUnitSimpleType)o.@FacilityDefaultCurrencyUnit)));
            }
            WriteElementString(@"FacilityDefaultMeasurementStandard", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityDefaultMeasurementStandard));
            WriteElementString(@"FacilityDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityDescription));
            WriteElementString(@"FacilityDeliverablePhaseName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityDeliverablePhaseName));
            Write49_FloorCollectionType(@"Floors", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.FloorCollectionType)o.@Floors), false, false);
            Write65_ZoneCollectionType(@"Zones", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ZoneCollectionType)o.@Zones), false, false);
            Write62_AssetTypeCollectionType(@"AssetTypes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssetTypeCollectionType)o.@AssetTypes), false, false);
            Write68_SystemCollectionType(@"Systems", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SystemCollectionType)o.@Systems), false, false);
            Write71_ConnectionCollectionType(@"Connections", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ConnectionCollectionType)o.@Connections), false, false);
            Write29_ContactCollectionType(@"Contacts", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactCollectionType)o.@Contacts), false, false);
            Write79_AttributeCollectionType(@"FacilityAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@FacilityAttributes), false, false);
            Write27_DocumentCollectionType(@"FacilityDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@FacilityDocuments), false, false);
            Write25_IssueCollectionType(@"FacilityIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@FacilityIssues), false, false);
            WriteEndElement(o);
        }

        void Write25_IssueCollectionType(string n, string ns, global::Xbim.COBieLite.IssueCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.IssueCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"IssueCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.IssueType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.IssueType>)o.@Issue;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write24_IssueType(@"Issue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write24_IssueType(string n, string ns, global::Xbim.COBieLite.IssueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.IssueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"IssueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"IssueName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueName));
            WriteElementString(@"IssueCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueCategory));
            WriteElementString(@"IssueRiskText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueRiskText));
            WriteElementString(@"IssueSeverityText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueSeverityText));
            WriteElementString(@"IssueImpactText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueImpactText));
            WriteElementString(@"IssueDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueDescription));
            Write23_ContactKeyType(@"ContactAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactKeyType)o.@ContactAssignment), false, false);
            WriteElementString(@"IssueMitigationDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueMitigationDescription));
            WriteEndElement(o);
        }

        void Write23_ContactKeyType(string n, string ns, global::Xbim.COBieLite.ContactKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ContactKeyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactKeyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"ContactEmailReference", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactEmailReference));
            WriteEndElement(o);
        }

        void Write27_DocumentCollectionType(string n, string ns, global::Xbim.COBieLite.DocumentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.DocumentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.DocumentType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.DocumentType>)o.@Document;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write26_DocumentType(@"Document", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write26_DocumentType(string n, string ns, global::Xbim.COBieLite.DocumentType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.DocumentType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"DocumentName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentName));
            WriteElementString(@"DocumentCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentCategory));
            WriteElementString(@"DocumentDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentDescription));
            WriteElementString(@"DocumentURI", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentURI));
            WriteElementString(@"DocumentReferenceURI", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentReferenceURI));
            Write79_AttributeCollectionType(@"DocumentAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@DocumentAttributes), false, false);
            Write25_IssueCollectionType(@"DocumentIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@DocumentIssues), false, false);
            WriteEndElement(o);
        }

        void Write79_AttributeCollectionType(string n, string ns, global::Xbim.COBieLite.AttributeCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.AttributeType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AttributeType>)o.@Attribute;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write78_AttributeType(@"Attribute", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write78_AttributeType(string n, string ns, global::Xbim.COBieLite.AttributeType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteAttribute(@"propertySetName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@propertySetName));
            WriteAttribute(@"propertySetExternalIdentifier", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@propertySetExternalIdentifier));
            WriteElementString(@"AttributeName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AttributeName));
            WriteElementString(@"AttributeCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AttributeCategory));
            WriteElementString(@"AttributeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AttributeDescription));
            Write77_AttributeValueType(@"AttributeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeValueType)o.@AttributeValue), false, false);
            Write25_IssueCollectionType(@"AttributeIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@AttributeIssues), false, false);
            WriteEndElement(o);
        }

        void Write77_AttributeValueType(string n, string ns, global::Xbim.COBieLite.AttributeValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeDecimalValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLite.AttributeDecimalValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLite.AttributeDecimalValueType", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeDecimalValue");
                    Write38_AttributeDecimalValueType(@"AttributeDecimalValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeDecimalValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeStringValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLite.AttributeStringValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLite.AttributeStringValueType", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeStringValue");
                    Write41_AttributeStringValueType(@"AttributeStringValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeStringValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeIntegerValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLite.AttributeIntegerValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLite.AttributeIntegerValueType", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeIntegerValue");
                    Write35_AttributeIntegerValueType(@"AttributeIntegerValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeIntegerValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeBooleanValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLite.BooleanValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLite.BooleanValueType", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeBooleanValue");
                    Write37_BooleanValueType(@"AttributeBooleanValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.BooleanValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeDateValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::System.DateTime)) throw CreateMismatchChoiceException(@"System.DateTime", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeDateValue");
                    WriteElementStringRaw(@"AttributeDateValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDate(((global::System.DateTime)o.@Item)));
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeTimeValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::System.DateTime)) throw CreateMismatchChoiceException(@"System.DateTime", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeTimeValue");
                    WriteElementStringRaw(@"AttributeTimeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromTime(((global::System.DateTime)o.@Item)));
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeDateTimeValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::System.DateTime)) throw CreateMismatchChoiceException(@"System.DateTime", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeDateTimeValue");
                    WriteElementStringRaw(@"AttributeDateTimeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDateTime(((global::System.DateTime)o.@Item)));
                }
                else if (o.@ItemElementName == Xbim.COBieLite.ItemChoiceType.@AttributeMonetaryValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLite.AttributeMonetaryValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLite.AttributeMonetaryValueType", @"ItemElementName", @"Xbim.COBieLite.ItemChoiceType.@AttributeMonetaryValue");
                    Write76_AttributeMonetaryValueType(@"AttributeMonetaryValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeMonetaryValueType)o.@Item), false, false);
                }
                else  if ((object)(o.@Item) != null){
                    throw CreateUnknownTypeException(o.@Item);
                }
            }
            WriteEndElement(o);
        }

        void Write76_AttributeMonetaryValueType(string n, string ns, global::Xbim.COBieLite.AttributeMonetaryValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeMonetaryValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeMonetaryValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementStringRaw(@"MonetaryValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Decimal)((global::System.Decimal)o.@MonetaryValue)));
            WriteElementString(@"MonetaryUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write75_CurrencyUnitSimpleType(((global::Xbim.COBieLite.CurrencyUnitSimpleType)o.@MonetaryUnit)));
            WriteEndElement(o);
        }

        string Write75_CurrencyUnitSimpleType(global::Xbim.COBieLite.CurrencyUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLite.CurrencyUnitSimpleType.@GBP: s = @"British Pounds"; break;
                case global::Xbim.COBieLite.CurrencyUnitSimpleType.@USD: s = @"US Dollars"; break;
                case global::Xbim.COBieLite.CurrencyUnitSimpleType.@EUR: s = @"European Union Euro"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLite.CurrencyUnitSimpleType");
            }
            return s;
        }

        void Write37_BooleanValueType(string n, string ns, global::Xbim.COBieLite.BooleanValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.BooleanValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"BooleanValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            if (o.@BooleanValueSpecified) {
                WriteElementStringRaw(@"BooleanValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@BooleanValue)));
            }
            WriteEndElement(o);
        }

        void Write35_AttributeIntegerValueType(string n, string ns, global::Xbim.COBieLite.AttributeIntegerValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeIntegerValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeIntegerValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            WriteElementStringRaw(@"IntegerValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@IntegerValue)));
            WriteElementStringRaw(@"MinValueInteger", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@MinValueInteger)));
            WriteElementStringRaw(@"MaxValueInteger", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@MaxValueInteger)));
            WriteEndElement(o);
        }

        void Write41_AttributeStringValueType(string n, string ns, global::Xbim.COBieLite.AttributeStringValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeStringValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeStringValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            WriteElementString(@"StringValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@StringValue));
            Write40_AllowedValueCollectionType(@"AllowedValues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AllowedValueCollectionType)o.@AllowedValues), false, false);
            WriteEndElement(o);
        }

        void Write40_AllowedValueCollectionType(string n, string ns, global::Xbim.COBieLite.AllowedValueCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AllowedValueCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AllowedValueCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::System.String> a = (global::System.Collections.Generic.List<global::System.String>)o.@AttributeAllowedValue;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        WriteElementString(@"AttributeAllowedValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)a[ia]));
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write38_AttributeDecimalValueType(string n, string ns, global::Xbim.COBieLite.AttributeDecimalValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AttributeDecimalValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeDecimalValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            if (o.@DecimalValueSpecified) {
                WriteElementStringRaw(@"DecimalValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@DecimalValue)));
            }
            if (o.@MinValueDecimalSpecified) {
                WriteElementStringRaw(@"MinValueDecimal", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@MinValueDecimal)));
            }
            if (o.@MaxValueDecimalSpecified) {
                WriteElementStringRaw(@"MaxValueDecimal", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@MaxValueDecimal)));
            }
            WriteEndElement(o);
        }

        void Write29_ContactCollectionType(string n, string ns, global::Xbim.COBieLite.ContactCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ContactCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactType>)o.@Contact;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write28_ContactType(@"Contact", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write28_ContactType(string n, string ns, global::Xbim.COBieLite.ContactType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ContactType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ContactEmail", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactEmail));
            WriteElementString(@"ContactCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactCategory));
            WriteElementString(@"ContactCompanyName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactCompanyName));
            WriteElementString(@"ContactPhoneNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactPhoneNumber));
            WriteElementString(@"ContactDepartmentName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactDepartmentName));
            WriteElementString(@"ContactGivenName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactGivenName));
            WriteElementString(@"ContactFamilyName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactFamilyName));
            WriteElementString(@"ContactStreet", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactStreet));
            WriteElementString(@"ContactPostalBoxNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactPostalBoxNumber));
            WriteElementString(@"ContactTownName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactTownName));
            WriteElementString(@"ContactRegionCode", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactRegionCode));
            WriteElementString(@"ContactCountryName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactCountryName));
            WriteElementString(@"ContactPostalCode", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactPostalCode));
            WriteElementString(@"ContactURL", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ContactURL));
            Write79_AttributeCollectionType(@"ContactAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@ContactAttributes), false, false);
            Write27_DocumentCollectionType(@"ContactDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@ContactDocuments), false, false);
            Write25_IssueCollectionType(@"ContactIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@ContactIssues), false, false);
            WriteEndElement(o);
        }

        void Write71_ConnectionCollectionType(string n, string ns, global::Xbim.COBieLite.ConnectionCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ConnectionCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ConnectionCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.ConnectionType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ConnectionType>)o.@Connection;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write70_ConnectionType(@"Connection", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ConnectionType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write70_ConnectionType(string n, string ns, global::Xbim.COBieLite.ConnectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ConnectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ConnectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ConnectionName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionName));
            WriteElementString(@"ConnectionCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionCategory));
            WriteElementString(@"ConnectionAsset1Name", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionAsset1Name));
            WriteElementString(@"ConnectionAsset1PortName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionAsset1PortName));
            WriteElementString(@"ConnectionAsset2Name", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionAsset2Name));
            WriteElementString(@"ConnectionAsset2PortName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionAsset2PortName));
            WriteElementString(@"ConnectionDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ConnectionDescription));
            Write79_AttributeCollectionType(@"ConnectionAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@ConnectionAttributes), false, false);
            Write27_DocumentCollectionType(@"ConnectionDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@ConnectionDocuments), false, false);
            Write25_IssueCollectionType(@"ConnectionIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@ConnectionIssues), false, false);
            WriteEndElement(o);
        }

        void Write68_SystemCollectionType(string n, string ns, global::Xbim.COBieLite.SystemCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SystemCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemType>)o.@System;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write67_SystemType(@"System", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SystemType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write67_SystemType(string n, string ns, global::Xbim.COBieLite.SystemType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SystemType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemName));
            WriteElementString(@"SystemCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemCategory));
            WriteElementString(@"SystemDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemDescription));
            Write79_AttributeCollectionType(@"SystemAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@SystemAttributes), false, false);
            Write27_DocumentCollectionType(@"SystemDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@SystemDocuments), false, false);
            Write25_IssueCollectionType(@"SystemIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@SystemIssues), false, false);
            WriteEndElement(o);
        }

        void Write62_AssetTypeCollectionType(string n, string ns, global::Xbim.COBieLite.AssetTypeCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AssetTypeCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetTypeCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetTypeInfoType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetTypeInfoType>)o.@AssetType;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write61_AssetTypeInfoType(@"AssetType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssetTypeInfoType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write61_AssetTypeInfoType(string n, string ns, global::Xbim.COBieLite.AssetTypeInfoType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AssetTypeInfoType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetTypeInfoType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"AssetTypeName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeName));
            WriteElementString(@"AssetTypeCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeCategory));
            WriteElementString(@"AssetTypeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeDescription));
            if (o.@AssetTypeAccountingCategorySpecified) {
                WriteElementString(@"AssetTypeAccountingCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write51_AssetPortabilitySimpleType(((global::Xbim.COBieLite.AssetPortabilitySimpleType)o.@AssetTypeAccountingCategory)));
            }
            WriteElementString(@"AssetTypeModelNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeModelNumber));
            Write43_DecimalValueType(@"AssetTypeReplacementCostValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@AssetTypeReplacementCostValue), false, false);
            Write36_IntegerValueType(@"AssetTypeExpectedLifeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IntegerValueType)o.@AssetTypeExpectedLifeValue), false, false);
            Write43_DecimalValueType(@"AssetTypeNominalLength", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@AssetTypeNominalLength), false, false);
            Write43_DecimalValueType(@"AssetTypeNominalWidth", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@AssetTypeNominalWidth), false, false);
            Write43_DecimalValueType(@"AssetTypeNominalHeight", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@AssetTypeNominalHeight), false, false);
            WriteElementString(@"AssetTypeAccessibilityText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeAccessibilityText));
            WriteElementString(@"AssetTypeCodePerformance", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeCodePerformance));
            WriteElementString(@"AssetTypeColorCode", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeColorCode));
            WriteElementString(@"AssetTypeConstituentsDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeConstituentsDescription));
            WriteElementString(@"AssetTypeFeaturesDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeFeaturesDescription));
            WriteElementString(@"AssetTypeFinishDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeFinishDescription));
            WriteElementString(@"AssetTypeGradeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeGradeDescription));
            WriteElementString(@"AssetTypeMaterialDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeMaterialDescription));
            WriteElementString(@"AssetTypeShapeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeShapeDescription));
            WriteElementString(@"AssetTypeSizeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeSizeDescription));
            WriteElementString(@"AssetTypeSustainabilityPerformanceDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeSustainabilityPerformanceDescription));
            Write33_AssetCollectionType(@"Assets", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssetCollectionType)o.@Assets), false, false);
            Write83_Item(@"AssetTypeManufacturerContactAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactAssignmentCollectionType)o.@AssetTypeManufacturerContactAssignments), false, false);
            Write53_WarrantyCollectionType(@"Warranties", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.WarrantyCollectionType)o.@Warranties), false, false);
            Write54_SpareCollectionType(@"Spares", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SpareCollectionType)o.@Spares), false, false);
            Write60_JobCollectionType(@"Jobs", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.JobCollectionType)o.@Jobs), false, false);
            Write81_Item(@"AssemblyAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssemblyAssignmentCollectionType)o.@AssemblyAssignments), false, false);
            Write79_AttributeCollectionType(@"AssetTypeAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@AssetTypeAttributes), false, false);
            Write27_DocumentCollectionType(@"AssetTypeDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@AssetTypeDocuments), false, false);
            Write25_IssueCollectionType(@"AssetTypeIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@AssetTypeIssues), false, false);
            WriteEndElement(o);
        }

        void Write81_Item(string n, string ns, global::Xbim.COBieLite.AssemblyAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AssemblyAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssemblyAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.AssemblyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AssemblyType>)o.@AssemblyAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write80_AssemblyType(@"AssemblyAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssemblyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write80_AssemblyType(string n, string ns, global::Xbim.COBieLite.AssemblyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AssemblyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssemblyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"AssemblyName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyName));
            WriteElementString(@"AssemblyParentName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyParentName));
            WriteElementString(@"AssemblyCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyCategory));
            WriteElementString(@"AssemblyDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyDescription));
            Write79_AttributeCollectionType(@"AssemblyAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@AssemblyAttributes), false, false);
            Write27_DocumentCollectionType(@"AssemblyDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@AssemblyDocuments), false, false);
            Write25_IssueCollectionType(@"AssemblyIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@AssemblyIssues), false, false);
            WriteEndElement(o);
        }

        void Write60_JobCollectionType(string n, string ns, global::Xbim.COBieLite.JobCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.JobCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"JobCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.JobType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.JobType>)o.@Job;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write59_JobType(@"Job", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.JobType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write59_JobType(string n, string ns, global::Xbim.COBieLite.JobType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.JobType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"JobType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"JobName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobName));
            WriteElementString(@"JobTaskID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobTaskID));
            WriteElementString(@"JobCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobCategory));
            WriteElementString(@"JobStatus", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobStatus));
            WriteElementString(@"JobDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobDescription));
            Write36_IntegerValueType(@"JobDuration", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IntegerValueType)o.@JobDuration), false, false);
            if (o.@JobStartDateSpecified) {
                WriteElementStringRaw(@"JobStartDate", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDate(((global::System.DateTime)o.@JobStartDate)));
            }
            WriteElementString(@"JobStartConditionDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobStartConditionDescription));
            Write43_DecimalValueType(@"JobFrequencyValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@JobFrequencyValue), false, false);
            WriteElementString(@"JobPriorTaskID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobPriorTaskID));
            Write58_ResourceCollectionType(@"Resources", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ResourceCollectionType)o.@Resources), false, false);
            Write79_AttributeCollectionType(@"JobAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@JobAttributes), false, false);
            Write27_DocumentCollectionType(@"JobDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@JobDocuments), false, false);
            Write25_IssueCollectionType(@"JobIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@JobIssues), false, false);
            WriteEndElement(o);
        }

        void Write58_ResourceCollectionType(string n, string ns, global::Xbim.COBieLite.ResourceCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ResourceCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ResourceCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.ResourceType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ResourceType>)o.@Resource;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write57_ResourceType(@"Resource", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ResourceType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write57_ResourceType(string n, string ns, global::Xbim.COBieLite.ResourceType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ResourceType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ResourceType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ResourceName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ResourceName));
            WriteElementString(@"ResourceCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ResourceCategory));
            WriteElementString(@"ResourceDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ResourceDescription));
            Write79_AttributeCollectionType(@"ResourceAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@ResourceAttributes), false, false);
            Write27_DocumentCollectionType(@"ResourceDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@ResourceDocuments), false, false);
            Write25_IssueCollectionType(@"ResourceIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@ResourceIssues), false, false);
            WriteEndElement(o);
        }

        void Write43_DecimalValueType(string n, string ns, global::Xbim.COBieLite.DecimalValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.DecimalValueType)) {
                }
                else if (t == typeof(global::Xbim.COBieLite.AttributeDecimalValueType)) {
                    Write38_AttributeDecimalValueType(n, ns,(global::Xbim.COBieLite.AttributeDecimalValueType)o, isNullable, true);
                    return;
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DecimalValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            if (o.@DecimalValueSpecified) {
                WriteElementStringRaw(@"DecimalValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@DecimalValue)));
            }
            WriteEndElement(o);
        }

        void Write36_IntegerValueType(string n, string ns, global::Xbim.COBieLite.IntegerValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.IntegerValueType)) {
                }
                else if (t == typeof(global::Xbim.COBieLite.AttributeIntegerValueType)) {
                    Write35_AttributeIntegerValueType(n, ns,(global::Xbim.COBieLite.AttributeIntegerValueType)o, isNullable, true);
                    return;
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"IntegerValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            WriteElementStringRaw(@"IntegerValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@IntegerValue)));
            WriteEndElement(o);
        }

        void Write54_SpareCollectionType(string n, string ns, global::Xbim.COBieLite.SpareCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SpareCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpareCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.SpareType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SpareType>)o.@Spare;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write84_SpareType(@"Spare", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SpareType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write84_SpareType(string n, string ns, global::Xbim.COBieLite.SpareType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SpareType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpareType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SpareName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareName));
            WriteElementString(@"SpareCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareCategory));
            WriteElementString(@"SpareDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareDescription));
            WriteElementString(@"SpareSetNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareSetNumber));
            WriteElementString(@"SparePartNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SparePartNumber));
            Write83_Item(@"SpareSupplierContactAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactAssignmentCollectionType)o.@SpareSupplierContactAssignments), false, false);
            Write79_AttributeCollectionType(@"SpareAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@SpareAttributes), false, false);
            Write27_DocumentCollectionType(@"SpareDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@SpareDocuments), false, false);
            Write25_IssueCollectionType(@"SpareIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@SpareIssues), false, false);
            WriteEndElement(o);
        }

        void Write83_Item(string n, string ns, global::Xbim.COBieLite.ContactAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ContactAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactKeyType>)o.@ContactAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write23_ContactKeyType(@"ContactAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write53_WarrantyCollectionType(string n, string ns, global::Xbim.COBieLite.WarrantyCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.WarrantyCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"WarrantyCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.WarrantyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.WarrantyType>)o.@Warranty;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write52_WarrantyType(@"Warranty", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.WarrantyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write52_WarrantyType(string n, string ns, global::Xbim.COBieLite.WarrantyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.WarrantyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"WarrantyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"WarrantyName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@WarrantyName));
            WriteElementString(@"WarrantyCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@WarrantyCategory));
            Write36_IntegerValueType(@"WarrantyDuration", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IntegerValueType)o.@WarrantyDuration), false, false);
            Write83_Item(@"WarrantyGaurantorContactAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ContactAssignmentCollectionType)o.@WarrantyGaurantorContactAssignments), false, false);
            Write79_AttributeCollectionType(@"WarrantyAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@WarrantyAttributes), false, false);
            Write27_DocumentCollectionType(@"WarrantyDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@WarrantyDocuments), false, false);
            Write25_IssueCollectionType(@"WarrantyIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@WarrantyIssues), false, false);
            WriteEndElement(o);
        }

        void Write33_AssetCollectionType(string n, string ns, global::Xbim.COBieLite.AssetCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AssetCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetInfoType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetInfoType>)o.@Asset;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write32_AssetInfoType(@"Asset", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssetInfoType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write32_AssetInfoType(string n, string ns, global::Xbim.COBieLite.AssetInfoType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.AssetInfoType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetInfoType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"AssetName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetName));
            Write14_SpaceAssignmentCollectionType(@"AssetSpaceAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SpaceAssignmentCollectionType)o.@AssetSpaceAssignments), false, false);
            WriteElementString(@"AssetDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetDescription));
            WriteElementString(@"AssetSerialNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetSerialNumber));
            if (o.@AssetInstallationDateSpecified) {
                WriteElementStringRaw(@"AssetInstallationDate", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDate(((global::System.DateTime)o.@AssetInstallationDate)));
            }
            WriteElementString(@"AssetInstalledModelNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetInstalledModelNumber));
            if (o.@AssetWarrantyStartDateSpecified) {
                WriteElementStringRaw(@"AssetWarrantyStartDate", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDate(((global::System.DateTime)o.@AssetWarrantyStartDate)));
            }
            WriteElementString(@"AssetStartDate", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetStartDate));
            WriteElementString(@"AssetTagNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTagNumber));
            WriteElementString(@"AssetBarCode", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetBarCode));
            WriteElementString(@"AssetIdentifier", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetIdentifier));
            WriteElementString(@"AssetLocationDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetLocationDescription));
            Write31_SystemAssignmentCollectionType(@"AssetSystemAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SystemAssignmentCollectionType)o.@AssetSystemAssignments), false, false);
            Write81_Item(@"AssemblyAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AssemblyAssignmentCollectionType)o.@AssemblyAssignments), false, false);
            Write79_AttributeCollectionType(@"AssetAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@AssetAttributes), false, false);
            Write27_DocumentCollectionType(@"AssetDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@AssetDocuments), false, false);
            Write25_IssueCollectionType(@"AssetIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@AssetIssues), false, false);
            WriteEndElement(o);
        }

        void Write31_SystemAssignmentCollectionType(string n, string ns, global::Xbim.COBieLite.SystemAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SystemAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemKeyType>)o.@SystemAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write17_SystemKeyType(@"SystemAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SystemKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write17_SystemKeyType(string n, string ns, global::Xbim.COBieLite.SystemKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SystemKeyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemKeyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteAttribute(@"externalIDReference", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalIDReference));
            WriteElementString(@"SystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemName));
            WriteElementString(@"SystemCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemCategory));
            WriteEndElement(o);
        }

        void Write14_SpaceAssignmentCollectionType(string n, string ns, global::Xbim.COBieLite.SpaceAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SpaceAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceKeyType>)o.@SpaceAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write6_SpaceKeyType(@"SpaceAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SpaceKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write6_SpaceKeyType(string n, string ns, global::Xbim.COBieLite.SpaceKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SpaceKeyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceKeyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"FloorName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorName));
            WriteElementString(@"SpaceName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceName));
            WriteEndElement(o);
        }

        string Write51_AssetPortabilitySimpleType(global::Xbim.COBieLite.AssetPortabilitySimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLite.AssetPortabilitySimpleType.@Fixed: s = @"Fixed"; break;
                case global::Xbim.COBieLite.AssetPortabilitySimpleType.@Moveable: s = @"Moveable"; break;
                case global::Xbim.COBieLite.AssetPortabilitySimpleType.@Item: s = @""; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLite.AssetPortabilitySimpleType");
            }
            return s;
        }

        void Write65_ZoneCollectionType(string n, string ns, global::Xbim.COBieLite.ZoneCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ZoneCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneType>)o.@Zone;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write64_ZoneType(@"Zone", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ZoneType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write64_ZoneType(string n, string ns, global::Xbim.COBieLite.ZoneType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ZoneType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ZoneName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneName));
            WriteElementString(@"ZoneCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneCategory));
            WriteElementString(@"ZoneDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneDescription));
            Write79_AttributeCollectionType(@"ZoneAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@ZoneAttributes), false, false);
            Write27_DocumentCollectionType(@"ZoneDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@ZoneDocuments), false, false);
            Write25_IssueCollectionType(@"ZoneIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@ZoneIssues), false, false);
            WriteEndElement(o);
        }

        void Write49_FloorCollectionType(string n, string ns, global::Xbim.COBieLite.FloorCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.FloorCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"FloorCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.FloorType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.FloorType>)o.@Floor;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write48_FloorType(@"Floor", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.FloorType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write48_FloorType(string n, string ns, global::Xbim.COBieLite.FloorType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.FloorType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"FloorType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"FloorName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorName));
            WriteElementString(@"FloorCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorCategory));
            WriteElementString(@"FloorDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorDescription));
            Write43_DecimalValueType(@"FloorElevationValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@FloorElevationValue), false, false);
            Write43_DecimalValueType(@"FloorHeightValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@FloorHeightValue), false, false);
            Write46_SpaceCollectionType(@"Spaces", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SpaceCollectionType)o.@Spaces), false, false);
            Write79_AttributeCollectionType(@"FloorAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@FloorAttributes), false, false);
            Write27_DocumentCollectionType(@"FloorDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@FloorDocuments), false, false);
            Write25_IssueCollectionType(@"FloorIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@FloorIssues), false, false);
            WriteEndElement(o);
        }

        void Write46_SpaceCollectionType(string n, string ns, global::Xbim.COBieLite.SpaceCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SpaceCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceType>)o.@Space;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write45_SpaceType(@"Space", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.SpaceType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write45_SpaceType(string n, string ns, global::Xbim.COBieLite.SpaceType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SpaceType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SpaceName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceName));
            WriteElementString(@"SpaceCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceCategory));
            WriteElementString(@"SpaceDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceDescription));
            WriteElementString(@"SpaceSignageName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceSignageName));
            Write43_DecimalValueType(@"SpaceUsableHeightValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@SpaceUsableHeightValue), false, false);
            Write43_DecimalValueType(@"SpaceGrossAreaValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@SpaceGrossAreaValue), false, false);
            Write43_DecimalValueType(@"SpaceNetAreaValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DecimalValueType)o.@SpaceNetAreaValue), false, false);
            Write44_ZoneAssignmentCollectionType(@"SpaceZoneAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ZoneAssignmentCollectionType)o.@SpaceZoneAssignments), false, false);
            Write79_AttributeCollectionType(@"SpaceAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.AttributeCollectionType)o.@SpaceAttributes), false, false);
            Write27_DocumentCollectionType(@"SpaceDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.DocumentCollectionType)o.@SpaceDocuments), false, false);
            Write25_IssueCollectionType(@"SpaceIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.IssueCollectionType)o.@SpaceIssues), false, false);
            WriteEndElement(o);
        }

        void Write44_ZoneAssignmentCollectionType(string n, string ns, global::Xbim.COBieLite.ZoneAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ZoneAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneKeyType>)o.@ZoneAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write20_ZoneKeyType(@"ZoneAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLite.ZoneKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write20_ZoneKeyType(string n, string ns, global::Xbim.COBieLite.ZoneKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ZoneKeyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneKeyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteAttribute(@"externalIDReference", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalIDReference));
            WriteElementString(@"ZoneName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneName));
            WriteElementString(@"ZoneCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneCategory));
            WriteEndElement(o);
        }

        string Write94_VolumeUnitSimpleType(global::Xbim.COBieLite.VolumeUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLite.VolumeUnitSimpleType.@cubiccentimeters: s = @"cubic centimeters"; break;
                case global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicfeet: s = @"cubic feet"; break;
                case global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicinches: s = @"cubic inches"; break;
                case global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicmeters: s = @"cubic meters"; break;
                case global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicmillimeters: s = @"cubic millimeters"; break;
                case global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicyards: s = @"cubic yards"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLite.VolumeUnitSimpleType");
            }
            return s;
        }

        string Write93_AreaUnitSimpleType(global::Xbim.COBieLite.AreaUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squarecentimeters: s = @"square centimeters"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squarefeet: s = @"square feet"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squareinches: s = @"square inches"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squarekilometers: s = @"square kilometers"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squaremeters: s = @"square meters"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squaremiles: s = @"square miles"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squaremillimeters: s = @"square millimeters"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@squareyards: s = @"square yards"; break;
                case global::Xbim.COBieLite.AreaUnitSimpleType.@Item: s = @""; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLite.AreaUnitSimpleType");
            }
            return s;
        }

        string Write92_LinearUnitSimpleType(global::Xbim.COBieLite.LinearUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLite.LinearUnitSimpleType.@centimeters: s = @"centimeters"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@feet: s = @"feet"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@inches: s = @"inches"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@kilometers: s = @"kilometers"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@meters: s = @"meters"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@miles: s = @"miles"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@millimeters: s = @"millimeters"; break;
                case global::Xbim.COBieLite.LinearUnitSimpleType.@yards: s = @"yards"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLite.LinearUnitSimpleType");
            }
            return s;
        }

        void Write86_SiteType(string n, string ns, global::Xbim.COBieLite.SiteType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.SiteType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SiteType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SiteName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SiteName));
            WriteElementString(@"SiteDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SiteDescription));
            WriteEndElement(o);
        }

        void Write87_ProjectType(string n, string ns, global::Xbim.COBieLite.ProjectType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLite.ProjectType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ProjectType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ProjectName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ProjectName));
            WriteElementString(@"ProjectDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ProjectDescription));
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReaderFacilityType : System.Xml.Serialization.XmlSerializationReader {

        public object Read96_Facility() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_Facility && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read95_FacilityType(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Facility");
            }
            return (object)o;
        }

        global::Xbim.COBieLite.FacilityType Read95_FacilityType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_FacilityType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.FacilityType o;
            o = new global::Xbim.COBieLite.FacilityType();
            bool[] paramsRead = new bool[23];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id8_FacilityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FacilityName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id9_FacilityCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FacilityCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id10_ProjectAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ProjectAssignment = Read87_ProjectType(false, true);
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id11_SiteAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SiteAssignment = Read86_SiteType(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id12_FacilityDefaultLinearUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityDefaultLinearUnitSpecified = true;
                        {
                            o.@FacilityDefaultLinearUnit = Read92_LinearUnitSimpleType(Reader.ReadElementString());
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id13_FacilityDefaultAreaUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityDefaultAreaUnitSpecified = true;
                        {
                            o.@FacilityDefaultAreaUnit = Read93_AreaUnitSimpleType(Reader.ReadElementString());
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id14_FacilityDefaultVolumeUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityDefaultVolumeUnitSpecified = true;
                        {
                            o.@FacilityDefaultVolumeUnit = Read94_VolumeUnitSimpleType(Reader.ReadElementString());
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id15_FacilityDefaultCurrencyUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityDefaultCurrencyUnitSpecified = true;
                        {
                            o.@FacilityDefaultCurrencyUnit = Read75_CurrencyUnitSimpleType(Reader.ReadElementString());
                        }
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id16_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FacilityDefaultMeasurementStandard = Reader.ReadElementString();
                        }
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id17_FacilityDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FacilityDescription = Reader.ReadElementString();
                        }
                        paramsRead[12] = true;
                    }
                    else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id18_FacilityDeliverablePhaseName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FacilityDeliverablePhaseName = Reader.ReadElementString();
                        }
                        paramsRead[13] = true;
                    }
                    else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id19_Floors && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Floors = Read49_FloorCollectionType(false, true);
                        paramsRead[14] = true;
                    }
                    else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id20_Zones && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Zones = Read65_ZoneCollectionType(false, true);
                        paramsRead[15] = true;
                    }
                    else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id21_AssetTypes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypes = Read62_AssetTypeCollectionType(false, true);
                        paramsRead[16] = true;
                    }
                    else if (!paramsRead[17] && ((object) Reader.LocalName == (object)id22_Systems && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Systems = Read68_SystemCollectionType(false, true);
                        paramsRead[17] = true;
                    }
                    else if (!paramsRead[18] && ((object) Reader.LocalName == (object)id23_Connections && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Connections = Read71_ConnectionCollectionType(false, true);
                        paramsRead[18] = true;
                    }
                    else if (!paramsRead[19] && ((object) Reader.LocalName == (object)id24_Contacts && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Contacts = Read29_ContactCollectionType(false, true);
                        paramsRead[19] = true;
                    }
                    else if (!paramsRead[20] && ((object) Reader.LocalName == (object)id25_FacilityAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[20] = true;
                    }
                    else if (!paramsRead[21] && ((object) Reader.LocalName == (object)id26_FacilityDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[21] = true;
                    }
                    else if (!paramsRead[22] && ((object) Reader.LocalName == (object)id27_FacilityIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FacilityIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[22] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ProjectAssignment, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SiteAssignment, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultLinearUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultAreaUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultVolumeUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultCurrencyUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultMeasurementStandard, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDeliverablePhaseName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Floors, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Zones, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Systems, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Connections, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Contacts, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ProjectAssignment, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SiteAssignment, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultLinearUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultAreaUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultVolumeUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultCurrencyUnit, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDefaultMeasurementStandard, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDeliverablePhaseName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Floors, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Zones, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Systems, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Connections, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Contacts, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FacilityIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.IssueCollectionType Read25_IssueCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id28_IssueCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.IssueCollectionType o;
            o = new global::Xbim.COBieLite.IssueCollectionType();
            if ((object)(o.@Issue) == null) o.@Issue = new global::System.Collections.Generic.List<global::Xbim.COBieLite.IssueType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.IssueType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.IssueType>)o.@Issue;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id29_Issue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read24_IssueType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Issue");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Issue");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.IssueType Read24_IssueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id30_IssueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.IssueType o;
            o = new global::Xbim.COBieLite.IssueType();
            bool[] paramsRead = new bool[11];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id31_IssueName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id32_IssueCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id33_IssueRiskText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueRiskText = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id34_IssueSeverityText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueSeverityText = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id35_IssueImpactText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueImpactText = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id36_IssueDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueDescription = Reader.ReadElementString();
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id37_ContactAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ContactAssignment = Read23_ContactKeyType(false, true);
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id38_IssueMitigationDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IssueMitigationDescription = Reader.ReadElementString();
                        }
                        paramsRead[10] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueRiskText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueSeverityText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueImpactText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactAssignment, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueMitigationDescription");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueRiskText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueSeverityText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueImpactText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactAssignment, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IssueMitigationDescription");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ContactKeyType Read23_ContactKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id39_ContactKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ContactKeyType o;
            o = new global::Xbim.COBieLite.ContactKeyType();
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations3 = 0;
            int readerCount3 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id40_ContactEmailReference && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactEmailReference = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactEmailReference");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactEmailReference");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations3, ref readerCount3);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.DocumentCollectionType Read27_DocumentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id41_DocumentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.DocumentCollectionType o;
            o = new global::Xbim.COBieLite.DocumentCollectionType();
            if ((object)(o.@Document) == null) o.@Document = new global::System.Collections.Generic.List<global::Xbim.COBieLite.DocumentType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.DocumentType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.DocumentType>)o.@Document;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations4 = 0;
            int readerCount4 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id42_Document && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read26_DocumentType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Document");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Document");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.DocumentType Read26_DocumentType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id43_DocumentType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.DocumentType o;
            o = new global::Xbim.COBieLite.DocumentType();
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations5 = 0;
            int readerCount5 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id44_DocumentName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@DocumentName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id45_DocumentCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@DocumentCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id46_DocumentDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@DocumentDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id47_DocumentURI && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@DocumentURI = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id48_DocumentReferenceURI && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@DocumentReferenceURI = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id49_DocumentAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@DocumentAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id50_DocumentIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@DocumentIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[9] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentURI, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentReferenceURI, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentURI, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentReferenceURI, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DocumentIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations5, ref readerCount5);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AttributeCollectionType Read79_AttributeCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id51_AttributeCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeCollectionType o;
            o = new global::Xbim.COBieLite.AttributeCollectionType();
            if ((object)(o.@Attribute) == null) o.@Attribute = new global::System.Collections.Generic.List<global::Xbim.COBieLite.AttributeType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.AttributeType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AttributeType>)o.@Attribute;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id52_Attribute && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read78_AttributeType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Attribute");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Attribute");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AttributeType Read78_AttributeType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id53_AttributeType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeType o;
            o = new global::Xbim.COBieLite.AttributeType();
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id54_propertySetName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@propertySetName = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id55_propertySetExternalIdentifier && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@propertySetExternalIdentifier = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:propertySetName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:propertySetExternalIdentifier");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations7 = 0;
            int readerCount7 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id56_AttributeName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AttributeName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id57_AttributeCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AttributeCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id58_AttributeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AttributeDescription = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id59_AttributeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AttributeValue = Read77_AttributeValueType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id60_AttributeIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AttributeIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[9] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations7, ref readerCount7);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AttributeValueType Read77_AttributeValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id61_AttributeValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeValueType o;
            o = new global::Xbim.COBieLite.AttributeValueType();
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id62_AttributeTimeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@Item = ToTime(Reader.ReadElementString());
                        }
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeTimeValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id63_AttributeStringValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Item = Read41_AttributeStringValueType(false, true);
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeStringValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id64_AttributeBooleanValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Item = Read37_BooleanValueType(false, true);
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeBooleanValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id65_AttributeDateTimeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@Item = ToDateTime(Reader.ReadElementString());
                        }
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeDateTimeValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id66_AttributeDateValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@Item = ToDate(Reader.ReadElementString());
                        }
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeDateValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id67_AttributeDecimalValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Item = Read38_AttributeDecimalValueType(false, true);
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeDecimalValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id68_AttributeIntegerValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Item = Read35_AttributeIntegerValueType(false, true);
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeIntegerValue;
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[0] && ((object) Reader.LocalName == (object)id69_AttributeMonetaryValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Item = Read76_AttributeMonetaryValueType(false, true);
                        o.@ItemElementName = global::Xbim.COBieLite.ItemChoiceType.@AttributeMonetaryValue;
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeTimeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeStringValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeBooleanValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDateTimeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDateValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDecimalValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeIntegerValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeMonetaryValue");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeTimeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeStringValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeBooleanValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDateTimeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDateValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeDecimalValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeIntegerValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeMonetaryValue");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AttributeMonetaryValueType Read76_AttributeMonetaryValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id70_AttributeMonetaryValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeMonetaryValueType o;
            o = new global::Xbim.COBieLite.AttributeMonetaryValueType();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id71_MonetaryValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@MonetaryValue = System.Xml.XmlConvert.ToDecimal(Reader.ReadElementString());
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id72_MonetaryUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@MonetaryUnit = Read75_CurrencyUnitSimpleType(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:MonetaryValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MonetaryUnit");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:MonetaryValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MonetaryUnit");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.CurrencyUnitSimpleType Read75_CurrencyUnitSimpleType(string s) {
            switch (s) {
                case @"British Pounds": return global::Xbim.COBieLite.CurrencyUnitSimpleType.@GBP;
                case @"US Dollars": return global::Xbim.COBieLite.CurrencyUnitSimpleType.@USD;
                case @"European Union Euro": return global::Xbim.COBieLite.CurrencyUnitSimpleType.@EUR;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLite.CurrencyUnitSimpleType));
            }
        }

        global::Xbim.COBieLite.AttributeIntegerValueType Read35_AttributeIntegerValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id73_AttributeIntegerValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeIntegerValueType o;
            o = new global::Xbim.COBieLite.AttributeIntegerValueType();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations10 = 0;
            int readerCount10 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id74_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@UnitName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id75_IntegerValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IntegerValue = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id76_MinValueInteger && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@MinValueInteger = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id77_MaxValueInteger && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@MaxValueInteger = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                        }
                        paramsRead[3] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IntegerValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MinValueInteger, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MaxValueInteger");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IntegerValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MinValueInteger, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MaxValueInteger");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations10, ref readerCount10);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AttributeDecimalValueType Read38_AttributeDecimalValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id78_AttributeDecimalValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeDecimalValueType o;
            o = new global::Xbim.COBieLite.AttributeDecimalValueType();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations11 = 0;
            int readerCount11 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id74_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@UnitName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id79_DecimalValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@DecimalValueSpecified = true;
                        {
                            o.@DecimalValue = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id80_MinValueDecimal && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@MinValueDecimalSpecified = true;
                        {
                            o.@MinValueDecimal = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id81_MaxValueDecimal && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@MaxValueDecimalSpecified = true;
                        {
                            o.@MaxValueDecimal = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                        }
                        paramsRead[3] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DecimalValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MinValueDecimal, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MaxValueDecimal");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DecimalValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MinValueDecimal, http://docs.buildingsmartalliance.org/nbims03/cobie/core:MaxValueDecimal");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations11, ref readerCount11);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.BooleanValueType Read37_BooleanValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id82_BooleanValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.BooleanValueType o;
            o = new global::Xbim.COBieLite.BooleanValueType();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations12 = 0;
            int readerCount12 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id74_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@UnitName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id83_BooleanValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@BooleanValueSpecified = true;
                        {
                            o.@BooleanValue = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:BooleanValue");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:BooleanValue");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations12, ref readerCount12);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AttributeStringValueType Read41_AttributeStringValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id84_AttributeStringValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AttributeStringValueType o;
            o = new global::Xbim.COBieLite.AttributeStringValueType();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations13 = 0;
            int readerCount13 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id74_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@UnitName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id85_StringValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@StringValue = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id86_AllowedValues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AllowedValues = Read40_AllowedValueCollectionType(false, true);
                        paramsRead[2] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:StringValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AllowedValues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:StringValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AllowedValues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations13, ref readerCount13);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AllowedValueCollectionType Read40_AllowedValueCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id87_AllowedValueCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AllowedValueCollectionType o;
            o = new global::Xbim.COBieLite.AllowedValueCollectionType();
            if ((object)(o.@AttributeAllowedValue) == null) o.@AttributeAllowedValue = new global::System.Collections.Generic.List<global::System.String>();
            global::System.Collections.Generic.List<global::System.String> a_0 = (global::System.Collections.Generic.List<global::System.String>)o.@AttributeAllowedValue;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id88_AttributeAllowedValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            a_0.Add(Reader.ReadElementString());
                        }
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeAllowedValue");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AttributeAllowedValue");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ContactCollectionType Read29_ContactCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id89_ContactCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ContactCollectionType o;
            o = new global::Xbim.COBieLite.ContactCollectionType();
            if ((object)(o.@Contact) == null) o.@Contact = new global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactType>)o.@Contact;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id90_Contact && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read28_ContactType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Contact");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Contact");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ContactType Read28_ContactType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id91_ContactType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ContactType o;
            o = new global::Xbim.COBieLite.ContactType();
            bool[] paramsRead = new bool[20];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations16 = 0;
            int readerCount16 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id92_ContactEmail && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactEmail = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id93_ContactCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id94_ContactCompanyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactCompanyName = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id95_ContactPhoneNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactPhoneNumber = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id96_ContactDepartmentName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactDepartmentName = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id97_ContactGivenName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactGivenName = Reader.ReadElementString();
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id98_ContactFamilyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactFamilyName = Reader.ReadElementString();
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id99_ContactStreet && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactStreet = Reader.ReadElementString();
                        }
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id100_ContactPostalBoxNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactPostalBoxNumber = Reader.ReadElementString();
                        }
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id101_ContactTownName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactTownName = Reader.ReadElementString();
                        }
                        paramsRead[12] = true;
                    }
                    else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id102_ContactRegionCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactRegionCode = Reader.ReadElementString();
                        }
                        paramsRead[13] = true;
                    }
                    else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id103_ContactCountryName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactCountryName = Reader.ReadElementString();
                        }
                        paramsRead[14] = true;
                    }
                    else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id104_ContactPostalCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactPostalCode = Reader.ReadElementString();
                        }
                        paramsRead[15] = true;
                    }
                    else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id105_ContactURL && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ContactURL = Reader.ReadElementString();
                        }
                        paramsRead[16] = true;
                    }
                    else if (!paramsRead[17] && ((object) Reader.LocalName == (object)id106_ContactAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ContactAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[17] = true;
                    }
                    else if (!paramsRead[18] && ((object) Reader.LocalName == (object)id107_ContactDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ContactDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[18] = true;
                    }
                    else if (!paramsRead[19] && ((object) Reader.LocalName == (object)id108_ContactIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ContactIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[19] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactEmail, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactCompanyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactPhoneNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactDepartmentName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactGivenName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactFamilyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactStreet, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactPostalBoxNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactTownName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactRegionCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactCountryName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactPostalCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactURL, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactEmail, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactCompanyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactPhoneNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactDepartmentName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactGivenName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactFamilyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactStreet, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactPostalBoxNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactTownName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactRegionCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactCountryName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactPostalCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactURL, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations16, ref readerCount16);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ConnectionCollectionType Read71_ConnectionCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id109_ConnectionCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ConnectionCollectionType o;
            o = new global::Xbim.COBieLite.ConnectionCollectionType();
            if ((object)(o.@Connection) == null) o.@Connection = new global::System.Collections.Generic.List<global::Xbim.COBieLite.ConnectionType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.ConnectionType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ConnectionType>)o.@Connection;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations17 = 0;
            int readerCount17 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id110_Connection && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read70_ConnectionType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Connection");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Connection");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations17, ref readerCount17);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ConnectionType Read70_ConnectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id111_ConnectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ConnectionType o;
            o = new global::Xbim.COBieLite.ConnectionType();
            bool[] paramsRead = new bool[13];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations18 = 0;
            int readerCount18 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id112_ConnectionName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id113_ConnectionCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id114_ConnectionAsset1Name && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionAsset1Name = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id115_ConnectionAsset1PortName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionAsset1PortName = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id116_ConnectionAsset2Name && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionAsset2Name = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id117_ConnectionAsset2PortName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionAsset2PortName = Reader.ReadElementString();
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id118_ConnectionDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ConnectionDescription = Reader.ReadElementString();
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id119_ConnectionAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ConnectionAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id120_ConnectionDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ConnectionDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id121_ConnectionIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ConnectionIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[12] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset1Name, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset1PortName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset2Name, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset2PortName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset1Name, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset1PortName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset2Name, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAsset2PortName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ConnectionIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations18, ref readerCount18);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SystemCollectionType Read68_SystemCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id122_SystemCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SystemCollectionType o;
            o = new global::Xbim.COBieLite.SystemCollectionType();
            if ((object)(o.@System) == null) o.@System = new global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemType>)o.@System;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations19 = 0;
            int readerCount19 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id123_System && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read67_SystemType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:System");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:System");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations19, ref readerCount19);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SystemType Read67_SystemType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id124_SystemType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SystemType o;
            o = new global::Xbim.COBieLite.SystemType();
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations20 = 0;
            int readerCount20 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id125_SystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SystemName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id126_SystemCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SystemCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id127_SystemDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SystemDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id128_SystemAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SystemAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id129_SystemDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SystemDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id130_SystemIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SystemIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations20, ref readerCount20);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssetTypeCollectionType Read62_AssetTypeCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id131_AssetTypeCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AssetTypeCollectionType o;
            o = new global::Xbim.COBieLite.AssetTypeCollectionType();
            if ((object)(o.@AssetType) == null) o.@AssetType = new global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetTypeInfoType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetTypeInfoType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetTypeInfoType>)o.@AssetType;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations21 = 0;
            int readerCount21 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id132_AssetType && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read61_AssetTypeInfoType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetType");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetType");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations21, ref readerCount21);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssetTypeInfoType Read61_AssetTypeInfoType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id133_AssetTypeInfoType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AssetTypeInfoType o;
            o = new global::Xbim.COBieLite.AssetTypeInfoType();
            bool[] paramsRead = new bool[33];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations22 = 0;
            int readerCount22 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id134_AssetTypeName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id135_AssetTypeCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id136_AssetTypeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id137_AssetTypeAccountingCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeAccountingCategorySpecified = true;
                        {
                            o.@AssetTypeAccountingCategory = Read51_AssetPortabilitySimpleType(Reader.ReadElementString());
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id138_AssetTypeModelNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeModelNumber = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id139_AssetTypeReplacementCostValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeReplacementCostValue = Read43_DecimalValueType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id140_AssetTypeExpectedLifeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeExpectedLifeValue = Read36_IntegerValueType(false, true);
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id141_AssetTypeNominalLength && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeNominalLength = Read43_DecimalValueType(false, true);
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id142_AssetTypeNominalWidth && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeNominalWidth = Read43_DecimalValueType(false, true);
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id143_AssetTypeNominalHeight && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeNominalHeight = Read43_DecimalValueType(false, true);
                        paramsRead[12] = true;
                    }
                    else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id144_AssetTypeAccessibilityText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeAccessibilityText = Reader.ReadElementString();
                        }
                        paramsRead[13] = true;
                    }
                    else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id145_AssetTypeCodePerformance && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeCodePerformance = Reader.ReadElementString();
                        }
                        paramsRead[14] = true;
                    }
                    else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id146_AssetTypeColorCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeColorCode = Reader.ReadElementString();
                        }
                        paramsRead[15] = true;
                    }
                    else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id147_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeConstituentsDescription = Reader.ReadElementString();
                        }
                        paramsRead[16] = true;
                    }
                    else if (!paramsRead[17] && ((object) Reader.LocalName == (object)id148_AssetTypeFeaturesDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeFeaturesDescription = Reader.ReadElementString();
                        }
                        paramsRead[17] = true;
                    }
                    else if (!paramsRead[18] && ((object) Reader.LocalName == (object)id149_AssetTypeFinishDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeFinishDescription = Reader.ReadElementString();
                        }
                        paramsRead[18] = true;
                    }
                    else if (!paramsRead[19] && ((object) Reader.LocalName == (object)id150_AssetTypeGradeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeGradeDescription = Reader.ReadElementString();
                        }
                        paramsRead[19] = true;
                    }
                    else if (!paramsRead[20] && ((object) Reader.LocalName == (object)id151_AssetTypeMaterialDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeMaterialDescription = Reader.ReadElementString();
                        }
                        paramsRead[20] = true;
                    }
                    else if (!paramsRead[21] && ((object) Reader.LocalName == (object)id152_AssetTypeShapeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeShapeDescription = Reader.ReadElementString();
                        }
                        paramsRead[21] = true;
                    }
                    else if (!paramsRead[22] && ((object) Reader.LocalName == (object)id153_AssetTypeSizeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeSizeDescription = Reader.ReadElementString();
                        }
                        paramsRead[22] = true;
                    }
                    else if (!paramsRead[23] && ((object) Reader.LocalName == (object)id154_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTypeSustainabilityPerformanceDescription = Reader.ReadElementString();
                        }
                        paramsRead[23] = true;
                    }
                    else if (!paramsRead[24] && ((object) Reader.LocalName == (object)id155_Assets && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Assets = Read33_AssetCollectionType(false, true);
                        paramsRead[24] = true;
                    }
                    else if (!paramsRead[25] && ((object) Reader.LocalName == (object)id156_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeManufacturerContactAssignments = Read83_Item(false, true);
                        paramsRead[25] = true;
                    }
                    else if (!paramsRead[26] && ((object) Reader.LocalName == (object)id157_Warranties && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Warranties = Read53_WarrantyCollectionType(false, true);
                        paramsRead[26] = true;
                    }
                    else if (!paramsRead[27] && ((object) Reader.LocalName == (object)id158_Spares && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Spares = Read54_SpareCollectionType(false, true);
                        paramsRead[27] = true;
                    }
                    else if (!paramsRead[28] && ((object) Reader.LocalName == (object)id159_Jobs && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Jobs = Read60_JobCollectionType(false, true);
                        paramsRead[28] = true;
                    }
                    else if (!paramsRead[29] && ((object) Reader.LocalName == (object)id160_AssemblyAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssemblyAssignments = Read81_Item(false, true);
                        paramsRead[29] = true;
                    }
                    else if (!paramsRead[30] && ((object) Reader.LocalName == (object)id161_AssetTypeAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[30] = true;
                    }
                    else if (!paramsRead[31] && ((object) Reader.LocalName == (object)id162_AssetTypeDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[31] = true;
                    }
                    else if (!paramsRead[32] && ((object) Reader.LocalName == (object)id163_AssetTypeIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetTypeIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[32] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeAccountingCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeModelNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeReplacementCostValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeExpectedLifeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeNominalLength, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeNominalWidth, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeNominalHeight, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeAccessibilityText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeCodePerformance, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeColorCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeConstituentsDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeFeaturesDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeFinishDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeGradeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeMaterialDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeShapeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeSizeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeSustainabilityPerformanceDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Assets, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeManufacturerContactAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Warranties, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Spares, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Jobs, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeAccountingCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeModelNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeReplacementCostValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeExpectedLifeValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeNominalLength, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeNominalWidth, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeNominalHeight, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeAccessibilityText, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeCodePerformance, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeColorCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeConstituentsDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeFeaturesDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeFinishDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeGradeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeMaterialDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeShapeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeSizeDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeSustainabilityPerformanceDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Assets, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeManufacturerContactAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Warranties, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Spares, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Jobs, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTypeIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations22, ref readerCount22);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssemblyAssignmentCollectionType Read81_Item(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id164_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AssemblyAssignmentCollectionType o;
            o = new global::Xbim.COBieLite.AssemblyAssignmentCollectionType();
            if ((object)(o.@AssemblyAssignment) == null) o.@AssemblyAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLite.AssemblyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.AssemblyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AssemblyType>)o.@AssemblyAssignment;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations23 = 0;
            int readerCount23 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id165_AssemblyAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read80_AssemblyType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAssignment");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAssignment");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations23, ref readerCount23);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssemblyType Read80_AssemblyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id166_AssemblyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AssemblyType o;
            o = new global::Xbim.COBieLite.AssemblyType();
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations24 = 0;
            int readerCount24 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id167_AssemblyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssemblyName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id168_AssemblyParentName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssemblyParentName = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id169_AssemblyCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssemblyCategory = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id170_AssemblyDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssemblyDescription = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id171_AssemblyAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssemblyAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id172_AssemblyDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssemblyDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id173_AssemblyIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssemblyIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[9] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyParentName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyParentName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations24, ref readerCount24);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.JobCollectionType Read60_JobCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id174_JobCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.JobCollectionType o;
            o = new global::Xbim.COBieLite.JobCollectionType();
            if ((object)(o.@Job) == null) o.@Job = new global::System.Collections.Generic.List<global::Xbim.COBieLite.JobType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.JobType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.JobType>)o.@Job;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations25 = 0;
            int readerCount25 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id175_Job && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read59_JobType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Job");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Job");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations25, ref readerCount25);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.JobType Read59_JobType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id176_JobType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.JobType o;
            o = new global::Xbim.COBieLite.JobType();
            bool[] paramsRead = new bool[17];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations26 = 0;
            int readerCount26 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id177_JobName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id178_JobTaskID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobTaskID = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id179_JobCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobCategory = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id180_JobStatus && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobStatus = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id181_JobDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobDescription = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id182_JobDuration && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@JobDuration = Read36_IntegerValueType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id183_JobStartDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@JobStartDateSpecified = true;
                        {
                            o.@JobStartDate = ToDate(Reader.ReadElementString());
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id184_JobStartConditionDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobStartConditionDescription = Reader.ReadElementString();
                        }
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id185_JobFrequencyValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@JobFrequencyValue = Read43_DecimalValueType(false, true);
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id186_JobPriorTaskID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@JobPriorTaskID = Reader.ReadElementString();
                        }
                        paramsRead[12] = true;
                    }
                    else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id187_Resources && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Resources = Read58_ResourceCollectionType(false, true);
                        paramsRead[13] = true;
                    }
                    else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id188_JobAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@JobAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[14] = true;
                    }
                    else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id189_JobDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@JobDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[15] = true;
                    }
                    else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id190_JobIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@JobIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[16] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobTaskID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobStatus, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobDuration, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobStartDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobStartConditionDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobFrequencyValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobPriorTaskID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Resources, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobTaskID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobStatus, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobDuration, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobStartDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobStartConditionDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobFrequencyValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobPriorTaskID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Resources, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:JobIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations26, ref readerCount26);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ResourceCollectionType Read58_ResourceCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id191_ResourceCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ResourceCollectionType o;
            o = new global::Xbim.COBieLite.ResourceCollectionType();
            if ((object)(o.@Resource) == null) o.@Resource = new global::System.Collections.Generic.List<global::Xbim.COBieLite.ResourceType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.ResourceType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ResourceType>)o.@Resource;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations27 = 0;
            int readerCount27 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id192_Resource && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read57_ResourceType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Resource");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Resource");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations27, ref readerCount27);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ResourceType Read57_ResourceType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id193_ResourceType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ResourceType o;
            o = new global::Xbim.COBieLite.ResourceType();
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations28 = 0;
            int readerCount28 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id194_ResourceName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ResourceName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id195_ResourceCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ResourceCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id196_ResourceDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ResourceDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id197_ResourceAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ResourceAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id198_ResourceDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ResourceDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id199_ResourceIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ResourceIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ResourceIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations28, ref readerCount28);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.DecimalValueType Read43_DecimalValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id200_DecimalValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id78_AttributeDecimalValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item))
                return Read38_AttributeDecimalValueType(isNullable, false);
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.DecimalValueType o;
            o = new global::Xbim.COBieLite.DecimalValueType();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations29 = 0;
            int readerCount29 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id74_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@UnitName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id79_DecimalValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@DecimalValueSpecified = true;
                        {
                            o.@DecimalValue = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DecimalValue");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:DecimalValue");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations29, ref readerCount29);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.IntegerValueType Read36_IntegerValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id201_IntegerValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id73_AttributeIntegerValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item))
                return Read35_AttributeIntegerValueType(isNullable, false);
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.IntegerValueType o;
            o = new global::Xbim.COBieLite.IntegerValueType();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations30 = 0;
            int readerCount30 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id74_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@UnitName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id75_IntegerValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@IntegerValue = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IntegerValue");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:UnitName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:IntegerValue");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations30, ref readerCount30);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SpareCollectionType Read54_SpareCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id202_SpareCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SpareCollectionType o;
            o = new global::Xbim.COBieLite.SpareCollectionType();
            if ((object)(o.@Spare) == null) o.@Spare = new global::System.Collections.Generic.List<global::Xbim.COBieLite.SpareType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.SpareType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SpareType>)o.@Spare;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations31 = 0;
            int readerCount31 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id203_Spare && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read84_SpareType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Spare");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Spare");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations31, ref readerCount31);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SpareType Read84_SpareType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id204_SpareType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SpareType o;
            o = new global::Xbim.COBieLite.SpareType();
            bool[] paramsRead = new bool[12];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations32 = 0;
            int readerCount32 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id205_SpareName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpareName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id206_SpareCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpareCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id207_SpareDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpareDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id208_SpareSetNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpareSetNumber = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id209_SparePartNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SparePartNumber = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id210_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpareSupplierContactAssignments = Read83_Item(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id211_SpareAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpareAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id212_SpareDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpareDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id213_SpareIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpareIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[11] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareSetNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SparePartNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareSupplierContactAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareSetNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SparePartNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareSupplierContactAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpareIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations32, ref readerCount32);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ContactAssignmentCollectionType Read83_Item(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id214_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ContactAssignmentCollectionType o;
            o = new global::Xbim.COBieLite.ContactAssignmentCollectionType();
            if ((object)(o.@ContactAssignment) == null) o.@ContactAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ContactKeyType>)o.@ContactAssignment;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations33 = 0;
            int readerCount33 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id37_ContactAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read23_ContactKeyType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactAssignment");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ContactAssignment");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations33, ref readerCount33);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.WarrantyCollectionType Read53_WarrantyCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id215_WarrantyCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.WarrantyCollectionType o;
            o = new global::Xbim.COBieLite.WarrantyCollectionType();
            if ((object)(o.@Warranty) == null) o.@Warranty = new global::System.Collections.Generic.List<global::Xbim.COBieLite.WarrantyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.WarrantyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.WarrantyType>)o.@Warranty;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations34 = 0;
            int readerCount34 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id216_Warranty && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read52_WarrantyType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Warranty");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Warranty");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations34, ref readerCount34);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.WarrantyType Read52_WarrantyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id217_WarrantyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.WarrantyType o;
            o = new global::Xbim.COBieLite.WarrantyType();
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations35 = 0;
            int readerCount35 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id218_WarrantyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@WarrantyName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id219_WarrantyCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@WarrantyCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id220_WarrantyDuration && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@WarrantyDuration = Read36_IntegerValueType(false, true);
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id221_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@WarrantyGaurantorContactAssignments = Read83_Item(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id222_WarrantyAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@WarrantyAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id223_WarrantyDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@WarrantyDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id224_WarrantyIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@WarrantyIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[9] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyDuration, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyGaurantorContactAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyDuration, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyGaurantorContactAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:WarrantyIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations35, ref readerCount35);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssetCollectionType Read33_AssetCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id225_AssetCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AssetCollectionType o;
            o = new global::Xbim.COBieLite.AssetCollectionType();
            if ((object)(o.@Asset) == null) o.@Asset = new global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetInfoType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetInfoType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.AssetInfoType>)o.@Asset;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations36 = 0;
            int readerCount36 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id226_Asset && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read32_AssetInfoType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Asset");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Asset");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations36, ref readerCount36);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssetInfoType Read32_AssetInfoType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id227_AssetInfoType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.AssetInfoType o;
            o = new global::Xbim.COBieLite.AssetInfoType();
            bool[] paramsRead = new bool[20];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations37 = 0;
            int readerCount37 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id228_AssetName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id229_AssetSpaceAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetSpaceAssignments = Read14_SpaceAssignmentCollectionType(false, true);
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id230_AssetDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id231_AssetSerialNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetSerialNumber = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id232_AssetInstallationDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetInstallationDateSpecified = true;
                        {
                            o.@AssetInstallationDate = ToDate(Reader.ReadElementString());
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id233_AssetInstalledModelNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetInstalledModelNumber = Reader.ReadElementString();
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id234_AssetWarrantyStartDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetWarrantyStartDateSpecified = true;
                        {
                            o.@AssetWarrantyStartDate = ToDate(Reader.ReadElementString());
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id235_AssetStartDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetStartDate = Reader.ReadElementString();
                        }
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id236_AssetTagNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetTagNumber = Reader.ReadElementString();
                        }
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id237_AssetBarCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetBarCode = Reader.ReadElementString();
                        }
                        paramsRead[12] = true;
                    }
                    else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id238_AssetIdentifier && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetIdentifier = Reader.ReadElementString();
                        }
                        paramsRead[13] = true;
                    }
                    else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id239_AssetLocationDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@AssetLocationDescription = Reader.ReadElementString();
                        }
                        paramsRead[14] = true;
                    }
                    else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id240_AssetSystemAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetSystemAssignments = Read31_SystemAssignmentCollectionType(false, true);
                        paramsRead[15] = true;
                    }
                    else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id160_AssemblyAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssemblyAssignments = Read81_Item(false, true);
                        paramsRead[16] = true;
                    }
                    else if (!paramsRead[17] && ((object) Reader.LocalName == (object)id241_AssetAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[17] = true;
                    }
                    else if (!paramsRead[18] && ((object) Reader.LocalName == (object)id242_AssetDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[18] = true;
                    }
                    else if (!paramsRead[19] && ((object) Reader.LocalName == (object)id243_AssetIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@AssetIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[19] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetSpaceAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetSerialNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetInstallationDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetInstalledModelNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetWarrantyStartDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetStartDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTagNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetBarCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetIdentifier, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetLocationDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetSystemAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetSpaceAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetSerialNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetInstallationDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetInstalledModelNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetWarrantyStartDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetStartDate, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetTagNumber, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetBarCode, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetIdentifier, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetLocationDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetSystemAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssemblyAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:AssetIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations37, ref readerCount37);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SystemAssignmentCollectionType Read31_SystemAssignmentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id244_SystemAssignmentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SystemAssignmentCollectionType o;
            o = new global::Xbim.COBieLite.SystemAssignmentCollectionType();
            if ((object)(o.@SystemAssignment) == null) o.@SystemAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SystemKeyType>)o.@SystemAssignment;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations38 = 0;
            int readerCount38 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id245_SystemAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read17_SystemKeyType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemAssignment");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemAssignment");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations38, ref readerCount38);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SystemKeyType Read17_SystemKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id246_SystemKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SystemKeyType o;
            o = new global::Xbim.COBieLite.SystemKeyType();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id247_externalIDReference && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalIDReference = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalIDReference");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations39 = 0;
            int readerCount39 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id125_SystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SystemName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id126_SystemCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SystemCategory = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemCategory");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SystemCategory");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations39, ref readerCount39);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SpaceAssignmentCollectionType Read14_SpaceAssignmentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id248_SpaceAssignmentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SpaceAssignmentCollectionType o;
            o = new global::Xbim.COBieLite.SpaceAssignmentCollectionType();
            if ((object)(o.@SpaceAssignment) == null) o.@SpaceAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceKeyType>)o.@SpaceAssignment;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations40 = 0;
            int readerCount40 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id249_SpaceAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read6_SpaceKeyType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceAssignment");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceAssignment");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations40, ref readerCount40);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SpaceKeyType Read6_SpaceKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id250_SpaceKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SpaceKeyType o;
            o = new global::Xbim.COBieLite.SpaceKeyType();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations41 = 0;
            int readerCount41 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id251_FloorName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FloorName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id252_SpaceName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpaceName = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceName");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceName");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations41, ref readerCount41);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.AssetPortabilitySimpleType Read51_AssetPortabilitySimpleType(string s) {
            switch (s) {
                case @"Fixed": return global::Xbim.COBieLite.AssetPortabilitySimpleType.@Fixed;
                case @"Moveable": return global::Xbim.COBieLite.AssetPortabilitySimpleType.@Moveable;
                case @"": return global::Xbim.COBieLite.AssetPortabilitySimpleType.@Item;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLite.AssetPortabilitySimpleType));
            }
        }

        global::Xbim.COBieLite.ZoneCollectionType Read65_ZoneCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id253_ZoneCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ZoneCollectionType o;
            o = new global::Xbim.COBieLite.ZoneCollectionType();
            if ((object)(o.@Zone) == null) o.@Zone = new global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneType>)o.@Zone;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations42 = 0;
            int readerCount42 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id254_Zone && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read64_ZoneType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Zone");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Zone");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations42, ref readerCount42);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ZoneType Read64_ZoneType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id255_ZoneType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ZoneType o;
            o = new global::Xbim.COBieLite.ZoneType();
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations43 = 0;
            int readerCount43 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id256_ZoneName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ZoneName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id257_ZoneCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ZoneCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id258_ZoneDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ZoneDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id259_ZoneAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ZoneAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id260_ZoneDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ZoneDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id261_ZoneIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@ZoneIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations43, ref readerCount43);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.FloorCollectionType Read49_FloorCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id262_FloorCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.FloorCollectionType o;
            o = new global::Xbim.COBieLite.FloorCollectionType();
            if ((object)(o.@Floor) == null) o.@Floor = new global::System.Collections.Generic.List<global::Xbim.COBieLite.FloorType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.FloorType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.FloorType>)o.@Floor;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations44 = 0;
            int readerCount44 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id263_Floor && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read48_FloorType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Floor");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Floor");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations44, ref readerCount44);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.FloorType Read48_FloorType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id264_FloorType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.FloorType o;
            o = new global::Xbim.COBieLite.FloorType();
            bool[] paramsRead = new bool[12];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations45 = 0;
            int readerCount45 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id251_FloorName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FloorName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id265_FloorCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FloorCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id266_FloorDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@FloorDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id267_FloorElevationValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FloorElevationValue = Read43_DecimalValueType(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id268_FloorHeightValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FloorHeightValue = Read43_DecimalValueType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id269_Spaces && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@Spaces = Read46_SpaceCollectionType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id270_FloorAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FloorAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id271_FloorDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FloorDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id272_FloorIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@FloorIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[11] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorElevationValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorHeightValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Spaces, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorElevationValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorHeightValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:Spaces, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:FloorIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations45, ref readerCount45);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SpaceCollectionType Read46_SpaceCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id273_SpaceCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SpaceCollectionType o;
            o = new global::Xbim.COBieLite.SpaceCollectionType();
            if ((object)(o.@Space) == null) o.@Space = new global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.SpaceType>)o.@Space;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations46 = 0;
            int readerCount46 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id274_Space && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read45_SpaceType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Space");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:Space");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations46, ref readerCount46);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.SpaceType Read45_SpaceType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id275_SpaceType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SpaceType o;
            o = new global::Xbim.COBieLite.SpaceType();
            bool[] paramsRead = new bool[14];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations47 = 0;
            int readerCount47 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id252_SpaceName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpaceName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id276_SpaceCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpaceCategory = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id277_SpaceDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpaceDescription = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id278_SpaceSignageName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SpaceSignageName = Reader.ReadElementString();
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id279_SpaceUsableHeightValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceUsableHeightValue = Read43_DecimalValueType(false, true);
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id280_SpaceGrossAreaValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceGrossAreaValue = Read43_DecimalValueType(false, true);
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id281_SpaceNetAreaValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceNetAreaValue = Read43_DecimalValueType(false, true);
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id282_SpaceZoneAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceZoneAssignments = Read44_ZoneAssignmentCollectionType(false, true);
                        paramsRead[10] = true;
                    }
                    else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id283_SpaceAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceAttributes = Read79_AttributeCollectionType(false, true);
                        paramsRead[11] = true;
                    }
                    else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id284_SpaceDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceDocuments = Read27_DocumentCollectionType(false, true);
                        paramsRead[12] = true;
                    }
                    else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id285_SpaceIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        o.@SpaceIssues = Read25_IssueCollectionType(false, true);
                        paramsRead[13] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceSignageName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceUsableHeightValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceGrossAreaValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceNetAreaValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceZoneAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceIssues");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceCategory, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceDescription, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceSignageName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceUsableHeightValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceGrossAreaValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceNetAreaValue, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceZoneAssignments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceAttributes, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceDocuments, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SpaceIssues");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations47, ref readerCount47);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ZoneAssignmentCollectionType Read44_ZoneAssignmentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id286_ZoneAssignmentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ZoneAssignmentCollectionType o;
            o = new global::Xbim.COBieLite.ZoneAssignmentCollectionType();
            if ((object)(o.@ZoneAssignment) == null) o.@ZoneAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLite.ZoneKeyType>)o.@ZoneAssignment;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations48 = 0;
            int readerCount48 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id287_ZoneAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read20_ZoneKeyType(false, true));
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneAssignment");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneAssignment");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations48, ref readerCount48);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ZoneKeyType Read20_ZoneKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id288_ZoneKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ZoneKeyType o;
            o = new global::Xbim.COBieLite.ZoneKeyType();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id247_externalIDReference && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalIDReference = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalIDReference");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations49 = 0;
            int readerCount49 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id256_ZoneName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ZoneName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id257_ZoneCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ZoneCategory = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneCategory");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ZoneCategory");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations49, ref readerCount49);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.VolumeUnitSimpleType Read94_VolumeUnitSimpleType(string s) {
            switch (s) {
                case @"cubic centimeters": return global::Xbim.COBieLite.VolumeUnitSimpleType.@cubiccentimeters;
                case @"cubic feet": return global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicfeet;
                case @"cubic inches": return global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicinches;
                case @"cubic meters": return global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicmeters;
                case @"cubic millimeters": return global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicmillimeters;
                case @"cubic yards": return global::Xbim.COBieLite.VolumeUnitSimpleType.@cubicyards;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLite.VolumeUnitSimpleType));
            }
        }

        global::Xbim.COBieLite.AreaUnitSimpleType Read93_AreaUnitSimpleType(string s) {
            switch (s) {
                case @"square centimeters": return global::Xbim.COBieLite.AreaUnitSimpleType.@squarecentimeters;
                case @"square feet": return global::Xbim.COBieLite.AreaUnitSimpleType.@squarefeet;
                case @"square inches": return global::Xbim.COBieLite.AreaUnitSimpleType.@squareinches;
                case @"square kilometers": return global::Xbim.COBieLite.AreaUnitSimpleType.@squarekilometers;
                case @"square meters": return global::Xbim.COBieLite.AreaUnitSimpleType.@squaremeters;
                case @"square miles": return global::Xbim.COBieLite.AreaUnitSimpleType.@squaremiles;
                case @"square millimeters": return global::Xbim.COBieLite.AreaUnitSimpleType.@squaremillimeters;
                case @"square yards": return global::Xbim.COBieLite.AreaUnitSimpleType.@squareyards;
                case @"": return global::Xbim.COBieLite.AreaUnitSimpleType.@Item;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLite.AreaUnitSimpleType));
            }
        }

        global::Xbim.COBieLite.LinearUnitSimpleType Read92_LinearUnitSimpleType(string s) {
            switch (s) {
                case @"centimeters": return global::Xbim.COBieLite.LinearUnitSimpleType.@centimeters;
                case @"feet": return global::Xbim.COBieLite.LinearUnitSimpleType.@feet;
                case @"inches": return global::Xbim.COBieLite.LinearUnitSimpleType.@inches;
                case @"kilometers": return global::Xbim.COBieLite.LinearUnitSimpleType.@kilometers;
                case @"meters": return global::Xbim.COBieLite.LinearUnitSimpleType.@meters;
                case @"miles": return global::Xbim.COBieLite.LinearUnitSimpleType.@miles;
                case @"millimeters": return global::Xbim.COBieLite.LinearUnitSimpleType.@millimeters;
                case @"yards": return global::Xbim.COBieLite.LinearUnitSimpleType.@yards;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLite.LinearUnitSimpleType));
            }
        }

        global::Xbim.COBieLite.SiteType Read86_SiteType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id289_SiteType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.SiteType o;
            o = new global::Xbim.COBieLite.SiteType();
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations50 = 0;
            int readerCount50 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id290_SiteName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SiteName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id291_SiteDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@SiteDescription = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SiteName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SiteDescription");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:SiteName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:SiteDescription");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations50, ref readerCount50);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLite.ProjectType Read87_ProjectType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id292_ProjectType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLite.ProjectType o;
            o = new global::Xbim.COBieLite.ProjectType();
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_externalEntityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalEntityName = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id7_externalSystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@externalSystemName = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations51 = 0;
            int readerCount51 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[3] && ((object) Reader.LocalName == (object)id293_ProjectName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ProjectName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id294_ProjectDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                        {
                            o.@ProjectDescription = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ProjectName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ProjectDescription");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:ProjectName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:ProjectDescription");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations51, ref readerCount51);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks() {
        }

        string id219_WarrantyCategory;
        string id236_AssetTagNumber;
        string id162_AssetTypeDocuments;
        string id191_ResourceCollectionType;
        string id223_WarrantyDocuments;
        string id146_AssetTypeColorCode;
        string id197_ResourceAttributes;
        string id84_AttributeStringValueType;
        string id34_IssueSeverityText;
        string id213_SpareIssues;
        string id292_ProjectType;
        string id130_SystemIssues;
        string id111_ConnectionType;
        string id58_AttributeDescription;
        string id88_AttributeAllowedValue;
        string id259_ZoneAttributes;
        string id66_AttributeDateValue;
        string id288_ZoneKeyType;
        string id80_MinValueDecimal;
        string id199_ResourceIssues;
        string id1_Facility;
        string id69_AttributeMonetaryValue;
        string id5_Item;
        string id232_AssetInstallationDate;
        string id24_Contacts;
        string id110_Connection;
        string id220_WarrantyDuration;
        string id214_Item;
        string id172_AssemblyDocuments;
        string id145_AssetTypeCodePerformance;
        string id210_Item;
        string id188_JobAttributes;
        string id184_JobStartConditionDescription;
        string id49_DocumentAttributes;
        string id225_AssetCollectionType;
        string id207_SpareDescription;
        string id222_WarrantyAttributes;
        string id51_AttributeCollectionType;
        string id153_AssetTypeSizeDescription;
        string id57_AttributeCategory;
        string id132_AssetType;
        string id157_Warranties;
        string id193_ResourceType;
        string id101_ContactTownName;
        string id134_AssetTypeName;
        string id181_JobDescription;
        string id234_AssetWarrantyStartDate;
        string id285_SpaceIssues;
        string id251_FloorName;
        string id280_SpaceGrossAreaValue;
        string id283_SpaceAttributes;
        string id156_Item;
        string id60_AttributeIssues;
        string id71_MonetaryValue;
        string id155_Assets;
        string id32_IssueCategory;
        string id26_FacilityDocuments;
        string id233_AssetInstalledModelNumber;
        string id242_AssetDocuments;
        string id97_ContactGivenName;
        string id174_JobCollectionType;
        string id208_SpareSetNumber;
        string id70_AttributeMonetaryValueType;
        string id89_ContactCollectionType;
        string id182_JobDuration;
        string id53_AttributeType;
        string id3_FacilityType;
        string id143_AssetTypeNominalHeight;
        string id173_AssemblyIssues;
        string id246_SystemKeyType;
        string id215_WarrantyCollectionType;
        string id231_AssetSerialNumber;
        string id62_AttributeTimeValue;
        string id82_BooleanValueType;
        string id263_Floor;
        string id8_FacilityName;
        string id29_Issue;
        string id142_AssetTypeNominalWidth;
        string id108_ContactIssues;
        string id133_AssetTypeInfoType;
        string id224_WarrantyIssues;
        string id180_JobStatus;
        string id59_AttributeValue;
        string id90_Contact;
        string id190_JobIssues;
        string id275_SpaceType;
        string id72_MonetaryUnit;
        string id106_ContactAttributes;
        string id39_ContactKeyType;
        string id171_AssemblyAttributes;
        string id286_ZoneAssignmentCollectionType;
        string id253_ZoneCollectionType;
        string id15_FacilityDefaultCurrencyUnit;
        string id144_AssetTypeAccessibilityText;
        string id229_AssetSpaceAssignments;
        string id28_IssueCollectionType;
        string id95_ContactPhoneNumber;
        string id154_Item;
        string id291_SiteDescription;
        string id128_SystemAttributes;
        string id126_SystemCategory;
        string id105_ContactURL;
        string id86_AllowedValues;
        string id25_FacilityAttributes;
        string id40_ContactEmailReference;
        string id122_SystemCollectionType;
        string id14_FacilityDefaultVolumeUnit;
        string id117_ConnectionAsset2PortName;
        string id13_FacilityDefaultAreaUnit;
        string id150_AssetTypeGradeDescription;
        string id245_SystemAssignment;
        string id48_DocumentReferenceURI;
        string id61_AttributeValueType;
        string id284_SpaceDocuments;
        string id16_Item;
        string id209_SparePartNumber;
        string id7_externalSystemName;
        string id139_AssetTypeReplacementCostValue;
        string id216_Warranty;
        string id265_FloorCategory;
        string id159_Jobs;
        string id79_DecimalValue;
        string id211_SpareAttributes;
        string id64_AttributeBooleanValue;
        string id137_AssetTypeAccountingCategory;
        string id121_ConnectionIssues;
        string id252_SpaceName;
        string id228_AssetName;
        string id204_SpareType;
        string id281_SpaceNetAreaValue;
        string id185_JobFrequencyValue;
        string id278_SpaceSignageName;
        string id178_JobTaskID;
        string id118_ConnectionDescription;
        string id78_AttributeDecimalValueType;
        string id267_FloorElevationValue;
        string id23_Connections;
        string id85_StringValue;
        string id261_ZoneIssues;
        string id293_ProjectName;
        string id166_AssemblyType;
        string id260_ZoneDocuments;
        string id258_ZoneDescription;
        string id92_ContactEmail;
        string id22_Systems;
        string id274_Space;
        string id87_AllowedValueCollectionType;
        string id38_IssueMitigationDescription;
        string id167_AssemblyName;
        string id73_AttributeIntegerValueType;
        string id36_IssueDescription;
        string id94_ContactCompanyName;
        string id147_Item;
        string id127_SystemDescription;
        string id125_SystemName;
        string id176_JobType;
        string id239_AssetLocationDescription;
        string id244_SystemAssignmentCollectionType;
        string id74_UnitName;
        string id238_AssetIdentifier;
        string id81_MaxValueDecimal;
        string id282_SpaceZoneAssignments;
        string id256_ZoneName;
        string id289_SiteType;
        string id273_SpaceCollectionType;
        string id164_Item;
        string id183_JobStartDate;
        string id198_ResourceDocuments;
        string id119_ConnectionAttributes;
        string id124_SystemType;
        string id141_AssetTypeNominalLength;
        string id221_Item;
        string id257_ZoneCategory;
        string id37_ContactAssignment;
        string id230_AssetDescription;
        string id237_AssetBarCode;
        string id271_FloorDocuments;
        string id93_ContactCategory;
        string id52_Attribute;
        string id19_Floors;
        string id107_ContactDocuments;
        string id262_FloorCollectionType;
        string id91_ContactType;
        string id140_AssetTypeExpectedLifeValue;
        string id46_DocumentDescription;
        string id151_AssetTypeMaterialDescription;
        string id243_AssetIssues;
        string id35_IssueImpactText;
        string id264_FloorType;
        string id203_Spare;
        string id99_ContactStreet;
        string id160_AssemblyAssignments;
        string id268_FloorHeightValue;
        string id114_ConnectionAsset1Name;
        string id12_FacilityDefaultLinearUnit;
        string id98_ContactFamilyName;
        string id112_ConnectionName;
        string id170_AssemblyDescription;
        string id18_FacilityDeliverablePhaseName;
        string id116_ConnectionAsset2Name;
        string id65_AttributeDateTimeValue;
        string id41_DocumentCollectionType;
        string id175_Job;
        string id54_propertySetName;
        string id129_SystemDocuments;
        string id250_SpaceKeyType;
        string id21_AssetTypes;
        string id270_FloorAttributes;
        string id187_Resources;
        string id77_MaxValueInteger;
        string id201_IntegerValueType;
        string id277_SpaceDescription;
        string id27_FacilityIssues;
        string id272_FloorIssues;
        string id96_ContactDepartmentName;
        string id290_SiteName;
        string id192_Resource;
        string id55_propertySetExternalIdentifier;
        string id100_ContactPostalBoxNumber;
        string id6_externalID;
        string id169_AssemblyCategory;
        string id266_FloorDescription;
        string id30_IssueType;
        string id42_Document;
        string id218_WarrantyName;
        string id104_ContactPostalCode;
        string id287_ZoneAssignment;
        string id131_AssetTypeCollectionType;
        string id279_SpaceUsableHeightValue;
        string id33_IssueRiskText;
        string id249_SpaceAssignment;
        string id113_ConnectionCategory;
        string id109_ConnectionCollectionType;
        string id120_ConnectionDocuments;
        string id56_AttributeName;
        string id235_AssetStartDate;
        string id136_AssetTypeDescription;
        string id202_SpareCollectionType;
        string id165_AssemblyAssignment;
        string id138_AssetTypeModelNumber;
        string id103_ContactCountryName;
        string id189_JobDocuments;
        string id135_AssetTypeCategory;
        string id63_AttributeStringValue;
        string id4_externalEntityName;
        string id2_Item;
        string id115_ConnectionAsset1PortName;
        string id212_SpareDocuments;
        string id177_JobName;
        string id269_Spaces;
        string id10_ProjectAssignment;
        string id240_AssetSystemAssignments;
        string id11_SiteAssignment;
        string id168_AssemblyParentName;
        string id200_DecimalValueType;
        string id227_AssetInfoType;
        string id43_DocumentType;
        string id67_AttributeDecimalValue;
        string id158_Spares;
        string id68_AttributeIntegerValue;
        string id179_JobCategory;
        string id205_SpareName;
        string id123_System;
        string id50_DocumentIssues;
        string id217_WarrantyType;
        string id196_ResourceDescription;
        string id17_FacilityDescription;
        string id149_AssetTypeFinishDescription;
        string id294_ProjectDescription;
        string id31_IssueName;
        string id45_DocumentCategory;
        string id206_SpareCategory;
        string id47_DocumentURI;
        string id9_FacilityCategory;
        string id226_Asset;
        string id276_SpaceCategory;
        string id152_AssetTypeShapeDescription;
        string id241_AssetAttributes;
        string id76_MinValueInteger;
        string id186_JobPriorTaskID;
        string id148_AssetTypeFeaturesDescription;
        string id254_Zone;
        string id247_externalIDReference;
        string id163_AssetTypeIssues;
        string id161_AssetTypeAttributes;
        string id75_IntegerValue;
        string id20_Zones;
        string id255_ZoneType;
        string id83_BooleanValue;
        string id194_ResourceName;
        string id195_ResourceCategory;
        string id102_ContactRegionCode;
        string id248_SpaceAssignmentCollectionType;
        string id44_DocumentName;

        protected override void InitIDs() {
            id219_WarrantyCategory = Reader.NameTable.Add(@"WarrantyCategory");
            id236_AssetTagNumber = Reader.NameTable.Add(@"AssetTagNumber");
            id162_AssetTypeDocuments = Reader.NameTable.Add(@"AssetTypeDocuments");
            id191_ResourceCollectionType = Reader.NameTable.Add(@"ResourceCollectionType");
            id223_WarrantyDocuments = Reader.NameTable.Add(@"WarrantyDocuments");
            id146_AssetTypeColorCode = Reader.NameTable.Add(@"AssetTypeColorCode");
            id197_ResourceAttributes = Reader.NameTable.Add(@"ResourceAttributes");
            id84_AttributeStringValueType = Reader.NameTable.Add(@"AttributeStringValueType");
            id34_IssueSeverityText = Reader.NameTable.Add(@"IssueSeverityText");
            id213_SpareIssues = Reader.NameTable.Add(@"SpareIssues");
            id292_ProjectType = Reader.NameTable.Add(@"ProjectType");
            id130_SystemIssues = Reader.NameTable.Add(@"SystemIssues");
            id111_ConnectionType = Reader.NameTable.Add(@"ConnectionType");
            id58_AttributeDescription = Reader.NameTable.Add(@"AttributeDescription");
            id88_AttributeAllowedValue = Reader.NameTable.Add(@"AttributeAllowedValue");
            id259_ZoneAttributes = Reader.NameTable.Add(@"ZoneAttributes");
            id66_AttributeDateValue = Reader.NameTable.Add(@"AttributeDateValue");
            id288_ZoneKeyType = Reader.NameTable.Add(@"ZoneKeyType");
            id80_MinValueDecimal = Reader.NameTable.Add(@"MinValueDecimal");
            id199_ResourceIssues = Reader.NameTable.Add(@"ResourceIssues");
            id1_Facility = Reader.NameTable.Add(@"Facility");
            id69_AttributeMonetaryValue = Reader.NameTable.Add(@"AttributeMonetaryValue");
            id5_Item = Reader.NameTable.Add(@"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            id232_AssetInstallationDate = Reader.NameTable.Add(@"AssetInstallationDate");
            id24_Contacts = Reader.NameTable.Add(@"Contacts");
            id110_Connection = Reader.NameTable.Add(@"Connection");
            id220_WarrantyDuration = Reader.NameTable.Add(@"WarrantyDuration");
            id214_Item = Reader.NameTable.Add(@"ContactAssignmentCollectionType");
            id172_AssemblyDocuments = Reader.NameTable.Add(@"AssemblyDocuments");
            id145_AssetTypeCodePerformance = Reader.NameTable.Add(@"AssetTypeCodePerformance");
            id210_Item = Reader.NameTable.Add(@"SpareSupplierContactAssignments");
            id188_JobAttributes = Reader.NameTable.Add(@"JobAttributes");
            id184_JobStartConditionDescription = Reader.NameTable.Add(@"JobStartConditionDescription");
            id49_DocumentAttributes = Reader.NameTable.Add(@"DocumentAttributes");
            id225_AssetCollectionType = Reader.NameTable.Add(@"AssetCollectionType");
            id207_SpareDescription = Reader.NameTable.Add(@"SpareDescription");
            id222_WarrantyAttributes = Reader.NameTable.Add(@"WarrantyAttributes");
            id51_AttributeCollectionType = Reader.NameTable.Add(@"AttributeCollectionType");
            id153_AssetTypeSizeDescription = Reader.NameTable.Add(@"AssetTypeSizeDescription");
            id57_AttributeCategory = Reader.NameTable.Add(@"AttributeCategory");
            id132_AssetType = Reader.NameTable.Add(@"AssetType");
            id157_Warranties = Reader.NameTable.Add(@"Warranties");
            id193_ResourceType = Reader.NameTable.Add(@"ResourceType");
            id101_ContactTownName = Reader.NameTable.Add(@"ContactTownName");
            id134_AssetTypeName = Reader.NameTable.Add(@"AssetTypeName");
            id181_JobDescription = Reader.NameTable.Add(@"JobDescription");
            id234_AssetWarrantyStartDate = Reader.NameTable.Add(@"AssetWarrantyStartDate");
            id285_SpaceIssues = Reader.NameTable.Add(@"SpaceIssues");
            id251_FloorName = Reader.NameTable.Add(@"FloorName");
            id280_SpaceGrossAreaValue = Reader.NameTable.Add(@"SpaceGrossAreaValue");
            id283_SpaceAttributes = Reader.NameTable.Add(@"SpaceAttributes");
            id156_Item = Reader.NameTable.Add(@"AssetTypeManufacturerContactAssignments");
            id60_AttributeIssues = Reader.NameTable.Add(@"AttributeIssues");
            id71_MonetaryValue = Reader.NameTable.Add(@"MonetaryValue");
            id155_Assets = Reader.NameTable.Add(@"Assets");
            id32_IssueCategory = Reader.NameTable.Add(@"IssueCategory");
            id26_FacilityDocuments = Reader.NameTable.Add(@"FacilityDocuments");
            id233_AssetInstalledModelNumber = Reader.NameTable.Add(@"AssetInstalledModelNumber");
            id242_AssetDocuments = Reader.NameTable.Add(@"AssetDocuments");
            id97_ContactGivenName = Reader.NameTable.Add(@"ContactGivenName");
            id174_JobCollectionType = Reader.NameTable.Add(@"JobCollectionType");
            id208_SpareSetNumber = Reader.NameTable.Add(@"SpareSetNumber");
            id70_AttributeMonetaryValueType = Reader.NameTable.Add(@"AttributeMonetaryValueType");
            id89_ContactCollectionType = Reader.NameTable.Add(@"ContactCollectionType");
            id182_JobDuration = Reader.NameTable.Add(@"JobDuration");
            id53_AttributeType = Reader.NameTable.Add(@"AttributeType");
            id3_FacilityType = Reader.NameTable.Add(@"FacilityType");
            id143_AssetTypeNominalHeight = Reader.NameTable.Add(@"AssetTypeNominalHeight");
            id173_AssemblyIssues = Reader.NameTable.Add(@"AssemblyIssues");
            id246_SystemKeyType = Reader.NameTable.Add(@"SystemKeyType");
            id215_WarrantyCollectionType = Reader.NameTable.Add(@"WarrantyCollectionType");
            id231_AssetSerialNumber = Reader.NameTable.Add(@"AssetSerialNumber");
            id62_AttributeTimeValue = Reader.NameTable.Add(@"AttributeTimeValue");
            id82_BooleanValueType = Reader.NameTable.Add(@"BooleanValueType");
            id263_Floor = Reader.NameTable.Add(@"Floor");
            id8_FacilityName = Reader.NameTable.Add(@"FacilityName");
            id29_Issue = Reader.NameTable.Add(@"Issue");
            id142_AssetTypeNominalWidth = Reader.NameTable.Add(@"AssetTypeNominalWidth");
            id108_ContactIssues = Reader.NameTable.Add(@"ContactIssues");
            id133_AssetTypeInfoType = Reader.NameTable.Add(@"AssetTypeInfoType");
            id224_WarrantyIssues = Reader.NameTable.Add(@"WarrantyIssues");
            id180_JobStatus = Reader.NameTable.Add(@"JobStatus");
            id59_AttributeValue = Reader.NameTable.Add(@"AttributeValue");
            id90_Contact = Reader.NameTable.Add(@"Contact");
            id190_JobIssues = Reader.NameTable.Add(@"JobIssues");
            id275_SpaceType = Reader.NameTable.Add(@"SpaceType");
            id72_MonetaryUnit = Reader.NameTable.Add(@"MonetaryUnit");
            id106_ContactAttributes = Reader.NameTable.Add(@"ContactAttributes");
            id39_ContactKeyType = Reader.NameTable.Add(@"ContactKeyType");
            id171_AssemblyAttributes = Reader.NameTable.Add(@"AssemblyAttributes");
            id286_ZoneAssignmentCollectionType = Reader.NameTable.Add(@"ZoneAssignmentCollectionType");
            id253_ZoneCollectionType = Reader.NameTable.Add(@"ZoneCollectionType");
            id15_FacilityDefaultCurrencyUnit = Reader.NameTable.Add(@"FacilityDefaultCurrencyUnit");
            id144_AssetTypeAccessibilityText = Reader.NameTable.Add(@"AssetTypeAccessibilityText");
            id229_AssetSpaceAssignments = Reader.NameTable.Add(@"AssetSpaceAssignments");
            id28_IssueCollectionType = Reader.NameTable.Add(@"IssueCollectionType");
            id95_ContactPhoneNumber = Reader.NameTable.Add(@"ContactPhoneNumber");
            id154_Item = Reader.NameTable.Add(@"AssetTypeSustainabilityPerformanceDescription");
            id291_SiteDescription = Reader.NameTable.Add(@"SiteDescription");
            id128_SystemAttributes = Reader.NameTable.Add(@"SystemAttributes");
            id126_SystemCategory = Reader.NameTable.Add(@"SystemCategory");
            id105_ContactURL = Reader.NameTable.Add(@"ContactURL");
            id86_AllowedValues = Reader.NameTable.Add(@"AllowedValues");
            id25_FacilityAttributes = Reader.NameTable.Add(@"FacilityAttributes");
            id40_ContactEmailReference = Reader.NameTable.Add(@"ContactEmailReference");
            id122_SystemCollectionType = Reader.NameTable.Add(@"SystemCollectionType");
            id14_FacilityDefaultVolumeUnit = Reader.NameTable.Add(@"FacilityDefaultVolumeUnit");
            id117_ConnectionAsset2PortName = Reader.NameTable.Add(@"ConnectionAsset2PortName");
            id13_FacilityDefaultAreaUnit = Reader.NameTable.Add(@"FacilityDefaultAreaUnit");
            id150_AssetTypeGradeDescription = Reader.NameTable.Add(@"AssetTypeGradeDescription");
            id245_SystemAssignment = Reader.NameTable.Add(@"SystemAssignment");
            id48_DocumentReferenceURI = Reader.NameTable.Add(@"DocumentReferenceURI");
            id61_AttributeValueType = Reader.NameTable.Add(@"AttributeValueType");
            id284_SpaceDocuments = Reader.NameTable.Add(@"SpaceDocuments");
            id16_Item = Reader.NameTable.Add(@"FacilityDefaultMeasurementStandard");
            id209_SparePartNumber = Reader.NameTable.Add(@"SparePartNumber");
            id7_externalSystemName = Reader.NameTable.Add(@"externalSystemName");
            id139_AssetTypeReplacementCostValue = Reader.NameTable.Add(@"AssetTypeReplacementCostValue");
            id216_Warranty = Reader.NameTable.Add(@"Warranty");
            id265_FloorCategory = Reader.NameTable.Add(@"FloorCategory");
            id159_Jobs = Reader.NameTable.Add(@"Jobs");
            id79_DecimalValue = Reader.NameTable.Add(@"DecimalValue");
            id211_SpareAttributes = Reader.NameTable.Add(@"SpareAttributes");
            id64_AttributeBooleanValue = Reader.NameTable.Add(@"AttributeBooleanValue");
            id137_AssetTypeAccountingCategory = Reader.NameTable.Add(@"AssetTypeAccountingCategory");
            id121_ConnectionIssues = Reader.NameTable.Add(@"ConnectionIssues");
            id252_SpaceName = Reader.NameTable.Add(@"SpaceName");
            id228_AssetName = Reader.NameTable.Add(@"AssetName");
            id204_SpareType = Reader.NameTable.Add(@"SpareType");
            id281_SpaceNetAreaValue = Reader.NameTable.Add(@"SpaceNetAreaValue");
            id185_JobFrequencyValue = Reader.NameTable.Add(@"JobFrequencyValue");
            id278_SpaceSignageName = Reader.NameTable.Add(@"SpaceSignageName");
            id178_JobTaskID = Reader.NameTable.Add(@"JobTaskID");
            id118_ConnectionDescription = Reader.NameTable.Add(@"ConnectionDescription");
            id78_AttributeDecimalValueType = Reader.NameTable.Add(@"AttributeDecimalValueType");
            id267_FloorElevationValue = Reader.NameTable.Add(@"FloorElevationValue");
            id23_Connections = Reader.NameTable.Add(@"Connections");
            id85_StringValue = Reader.NameTable.Add(@"StringValue");
            id261_ZoneIssues = Reader.NameTable.Add(@"ZoneIssues");
            id293_ProjectName = Reader.NameTable.Add(@"ProjectName");
            id166_AssemblyType = Reader.NameTable.Add(@"AssemblyType");
            id260_ZoneDocuments = Reader.NameTable.Add(@"ZoneDocuments");
            id258_ZoneDescription = Reader.NameTable.Add(@"ZoneDescription");
            id92_ContactEmail = Reader.NameTable.Add(@"ContactEmail");
            id22_Systems = Reader.NameTable.Add(@"Systems");
            id274_Space = Reader.NameTable.Add(@"Space");
            id87_AllowedValueCollectionType = Reader.NameTable.Add(@"AllowedValueCollectionType");
            id38_IssueMitigationDescription = Reader.NameTable.Add(@"IssueMitigationDescription");
            id167_AssemblyName = Reader.NameTable.Add(@"AssemblyName");
            id73_AttributeIntegerValueType = Reader.NameTable.Add(@"AttributeIntegerValueType");
            id36_IssueDescription = Reader.NameTable.Add(@"IssueDescription");
            id94_ContactCompanyName = Reader.NameTable.Add(@"ContactCompanyName");
            id147_Item = Reader.NameTable.Add(@"AssetTypeConstituentsDescription");
            id127_SystemDescription = Reader.NameTable.Add(@"SystemDescription");
            id125_SystemName = Reader.NameTable.Add(@"SystemName");
            id176_JobType = Reader.NameTable.Add(@"JobType");
            id239_AssetLocationDescription = Reader.NameTable.Add(@"AssetLocationDescription");
            id244_SystemAssignmentCollectionType = Reader.NameTable.Add(@"SystemAssignmentCollectionType");
            id74_UnitName = Reader.NameTable.Add(@"UnitName");
            id238_AssetIdentifier = Reader.NameTable.Add(@"AssetIdentifier");
            id81_MaxValueDecimal = Reader.NameTable.Add(@"MaxValueDecimal");
            id282_SpaceZoneAssignments = Reader.NameTable.Add(@"SpaceZoneAssignments");
            id256_ZoneName = Reader.NameTable.Add(@"ZoneName");
            id289_SiteType = Reader.NameTable.Add(@"SiteType");
            id273_SpaceCollectionType = Reader.NameTable.Add(@"SpaceCollectionType");
            id164_Item = Reader.NameTable.Add(@"AssemblyAssignmentCollectionType");
            id183_JobStartDate = Reader.NameTable.Add(@"JobStartDate");
            id198_ResourceDocuments = Reader.NameTable.Add(@"ResourceDocuments");
            id119_ConnectionAttributes = Reader.NameTable.Add(@"ConnectionAttributes");
            id124_SystemType = Reader.NameTable.Add(@"SystemType");
            id141_AssetTypeNominalLength = Reader.NameTable.Add(@"AssetTypeNominalLength");
            id221_Item = Reader.NameTable.Add(@"WarrantyGaurantorContactAssignments");
            id257_ZoneCategory = Reader.NameTable.Add(@"ZoneCategory");
            id37_ContactAssignment = Reader.NameTable.Add(@"ContactAssignment");
            id230_AssetDescription = Reader.NameTable.Add(@"AssetDescription");
            id237_AssetBarCode = Reader.NameTable.Add(@"AssetBarCode");
            id271_FloorDocuments = Reader.NameTable.Add(@"FloorDocuments");
            id93_ContactCategory = Reader.NameTable.Add(@"ContactCategory");
            id52_Attribute = Reader.NameTable.Add(@"Attribute");
            id19_Floors = Reader.NameTable.Add(@"Floors");
            id107_ContactDocuments = Reader.NameTable.Add(@"ContactDocuments");
            id262_FloorCollectionType = Reader.NameTable.Add(@"FloorCollectionType");
            id91_ContactType = Reader.NameTable.Add(@"ContactType");
            id140_AssetTypeExpectedLifeValue = Reader.NameTable.Add(@"AssetTypeExpectedLifeValue");
            id46_DocumentDescription = Reader.NameTable.Add(@"DocumentDescription");
            id151_AssetTypeMaterialDescription = Reader.NameTable.Add(@"AssetTypeMaterialDescription");
            id243_AssetIssues = Reader.NameTable.Add(@"AssetIssues");
            id35_IssueImpactText = Reader.NameTable.Add(@"IssueImpactText");
            id264_FloorType = Reader.NameTable.Add(@"FloorType");
            id203_Spare = Reader.NameTable.Add(@"Spare");
            id99_ContactStreet = Reader.NameTable.Add(@"ContactStreet");
            id160_AssemblyAssignments = Reader.NameTable.Add(@"AssemblyAssignments");
            id268_FloorHeightValue = Reader.NameTable.Add(@"FloorHeightValue");
            id114_ConnectionAsset1Name = Reader.NameTable.Add(@"ConnectionAsset1Name");
            id12_FacilityDefaultLinearUnit = Reader.NameTable.Add(@"FacilityDefaultLinearUnit");
            id98_ContactFamilyName = Reader.NameTable.Add(@"ContactFamilyName");
            id112_ConnectionName = Reader.NameTable.Add(@"ConnectionName");
            id170_AssemblyDescription = Reader.NameTable.Add(@"AssemblyDescription");
            id18_FacilityDeliverablePhaseName = Reader.NameTable.Add(@"FacilityDeliverablePhaseName");
            id116_ConnectionAsset2Name = Reader.NameTable.Add(@"ConnectionAsset2Name");
            id65_AttributeDateTimeValue = Reader.NameTable.Add(@"AttributeDateTimeValue");
            id41_DocumentCollectionType = Reader.NameTable.Add(@"DocumentCollectionType");
            id175_Job = Reader.NameTable.Add(@"Job");
            id54_propertySetName = Reader.NameTable.Add(@"propertySetName");
            id129_SystemDocuments = Reader.NameTable.Add(@"SystemDocuments");
            id250_SpaceKeyType = Reader.NameTable.Add(@"SpaceKeyType");
            id21_AssetTypes = Reader.NameTable.Add(@"AssetTypes");
            id270_FloorAttributes = Reader.NameTable.Add(@"FloorAttributes");
            id187_Resources = Reader.NameTable.Add(@"Resources");
            id77_MaxValueInteger = Reader.NameTable.Add(@"MaxValueInteger");
            id201_IntegerValueType = Reader.NameTable.Add(@"IntegerValueType");
            id277_SpaceDescription = Reader.NameTable.Add(@"SpaceDescription");
            id27_FacilityIssues = Reader.NameTable.Add(@"FacilityIssues");
            id272_FloorIssues = Reader.NameTable.Add(@"FloorIssues");
            id96_ContactDepartmentName = Reader.NameTable.Add(@"ContactDepartmentName");
            id290_SiteName = Reader.NameTable.Add(@"SiteName");
            id192_Resource = Reader.NameTable.Add(@"Resource");
            id55_propertySetExternalIdentifier = Reader.NameTable.Add(@"propertySetExternalIdentifier");
            id100_ContactPostalBoxNumber = Reader.NameTable.Add(@"ContactPostalBoxNumber");
            id6_externalID = Reader.NameTable.Add(@"externalID");
            id169_AssemblyCategory = Reader.NameTable.Add(@"AssemblyCategory");
            id266_FloorDescription = Reader.NameTable.Add(@"FloorDescription");
            id30_IssueType = Reader.NameTable.Add(@"IssueType");
            id42_Document = Reader.NameTable.Add(@"Document");
            id218_WarrantyName = Reader.NameTable.Add(@"WarrantyName");
            id104_ContactPostalCode = Reader.NameTable.Add(@"ContactPostalCode");
            id287_ZoneAssignment = Reader.NameTable.Add(@"ZoneAssignment");
            id131_AssetTypeCollectionType = Reader.NameTable.Add(@"AssetTypeCollectionType");
            id279_SpaceUsableHeightValue = Reader.NameTable.Add(@"SpaceUsableHeightValue");
            id33_IssueRiskText = Reader.NameTable.Add(@"IssueRiskText");
            id249_SpaceAssignment = Reader.NameTable.Add(@"SpaceAssignment");
            id113_ConnectionCategory = Reader.NameTable.Add(@"ConnectionCategory");
            id109_ConnectionCollectionType = Reader.NameTable.Add(@"ConnectionCollectionType");
            id120_ConnectionDocuments = Reader.NameTable.Add(@"ConnectionDocuments");
            id56_AttributeName = Reader.NameTable.Add(@"AttributeName");
            id235_AssetStartDate = Reader.NameTable.Add(@"AssetStartDate");
            id136_AssetTypeDescription = Reader.NameTable.Add(@"AssetTypeDescription");
            id202_SpareCollectionType = Reader.NameTable.Add(@"SpareCollectionType");
            id165_AssemblyAssignment = Reader.NameTable.Add(@"AssemblyAssignment");
            id138_AssetTypeModelNumber = Reader.NameTable.Add(@"AssetTypeModelNumber");
            id103_ContactCountryName = Reader.NameTable.Add(@"ContactCountryName");
            id189_JobDocuments = Reader.NameTable.Add(@"JobDocuments");
            id135_AssetTypeCategory = Reader.NameTable.Add(@"AssetTypeCategory");
            id63_AttributeStringValue = Reader.NameTable.Add(@"AttributeStringValue");
            id4_externalEntityName = Reader.NameTable.Add(@"externalEntityName");
            id2_Item = Reader.NameTable.Add(@"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            id115_ConnectionAsset1PortName = Reader.NameTable.Add(@"ConnectionAsset1PortName");
            id212_SpareDocuments = Reader.NameTable.Add(@"SpareDocuments");
            id177_JobName = Reader.NameTable.Add(@"JobName");
            id269_Spaces = Reader.NameTable.Add(@"Spaces");
            id10_ProjectAssignment = Reader.NameTable.Add(@"ProjectAssignment");
            id240_AssetSystemAssignments = Reader.NameTable.Add(@"AssetSystemAssignments");
            id11_SiteAssignment = Reader.NameTable.Add(@"SiteAssignment");
            id168_AssemblyParentName = Reader.NameTable.Add(@"AssemblyParentName");
            id200_DecimalValueType = Reader.NameTable.Add(@"DecimalValueType");
            id227_AssetInfoType = Reader.NameTable.Add(@"AssetInfoType");
            id43_DocumentType = Reader.NameTable.Add(@"DocumentType");
            id67_AttributeDecimalValue = Reader.NameTable.Add(@"AttributeDecimalValue");
            id158_Spares = Reader.NameTable.Add(@"Spares");
            id68_AttributeIntegerValue = Reader.NameTable.Add(@"AttributeIntegerValue");
            id179_JobCategory = Reader.NameTable.Add(@"JobCategory");
            id205_SpareName = Reader.NameTable.Add(@"SpareName");
            id123_System = Reader.NameTable.Add(@"System");
            id50_DocumentIssues = Reader.NameTable.Add(@"DocumentIssues");
            id217_WarrantyType = Reader.NameTable.Add(@"WarrantyType");
            id196_ResourceDescription = Reader.NameTable.Add(@"ResourceDescription");
            id17_FacilityDescription = Reader.NameTable.Add(@"FacilityDescription");
            id149_AssetTypeFinishDescription = Reader.NameTable.Add(@"AssetTypeFinishDescription");
            id294_ProjectDescription = Reader.NameTable.Add(@"ProjectDescription");
            id31_IssueName = Reader.NameTable.Add(@"IssueName");
            id45_DocumentCategory = Reader.NameTable.Add(@"DocumentCategory");
            id206_SpareCategory = Reader.NameTable.Add(@"SpareCategory");
            id47_DocumentURI = Reader.NameTable.Add(@"DocumentURI");
            id9_FacilityCategory = Reader.NameTable.Add(@"FacilityCategory");
            id226_Asset = Reader.NameTable.Add(@"Asset");
            id276_SpaceCategory = Reader.NameTable.Add(@"SpaceCategory");
            id152_AssetTypeShapeDescription = Reader.NameTable.Add(@"AssetTypeShapeDescription");
            id241_AssetAttributes = Reader.NameTable.Add(@"AssetAttributes");
            id76_MinValueInteger = Reader.NameTable.Add(@"MinValueInteger");
            id186_JobPriorTaskID = Reader.NameTable.Add(@"JobPriorTaskID");
            id148_AssetTypeFeaturesDescription = Reader.NameTable.Add(@"AssetTypeFeaturesDescription");
            id254_Zone = Reader.NameTable.Add(@"Zone");
            id247_externalIDReference = Reader.NameTable.Add(@"externalIDReference");
            id163_AssetTypeIssues = Reader.NameTable.Add(@"AssetTypeIssues");
            id161_AssetTypeAttributes = Reader.NameTable.Add(@"AssetTypeAttributes");
            id75_IntegerValue = Reader.NameTable.Add(@"IntegerValue");
            id20_Zones = Reader.NameTable.Add(@"Zones");
            id255_ZoneType = Reader.NameTable.Add(@"ZoneType");
            id83_BooleanValue = Reader.NameTable.Add(@"BooleanValue");
            id194_ResourceName = Reader.NameTable.Add(@"ResourceName");
            id195_ResourceCategory = Reader.NameTable.Add(@"ResourceCategory");
            id102_ContactRegionCode = Reader.NameTable.Add(@"ContactRegionCode");
            id248_SpaceAssignmentCollectionType = Reader.NameTable.Add(@"SpaceAssignmentCollectionType");
            id44_DocumentName = Reader.NameTable.Add(@"DocumentName");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReaderFacilityType();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriterFacilityType();
        }
    }

    public sealed class FacilityTypeSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"Facility", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterFacilityType)writer).Write96_Facility(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderFacilityType)reader).Read96_Facility();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderFacilityType(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterFacilityType(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Xbim.COBieLite.FacilityType:http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Facility:False:"] = @"Read96_Facility";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Xbim.COBieLite.FacilityType:http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Facility:False:"] = @"Write96_Facility";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Xbim.COBieLite.FacilityType:http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Facility:False:", new FacilityTypeSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Xbim.COBieLite.FacilityType)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Xbim.COBieLite.FacilityType)) return new FacilityTypeSerializer();
            return null;
        }
    }
}
