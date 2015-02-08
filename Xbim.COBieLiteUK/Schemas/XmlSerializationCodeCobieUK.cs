#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
[assembly:System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
#endif
[assembly:System.Xml.Serialization.XmlSerializerVersionAttribute(ParentAssemblyId=@"73d3c3c1-8173-43da-ade9-78136269fec4,", Version=@"4.0.0.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterProjectType : System.Xml.Serialization.XmlSerializationWriter {

        public void Write99_Project(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"Project", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
                return;
            }
            TopLevelElement();
            Write98_ProjectType(@"Project", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite", ((global::Xbim.COBieLiteUK.ProjectType)o), false, false);
        }

        void Write98_ProjectType(string n, string ns, global::Xbim.COBieLiteUK.ProjectType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ProjectType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ProjectTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteAttribute(@"externalID", @"", ((global::System.String)o.@externalID1));
            WriteElementString(@"ProjectName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ProjectName));
            WriteElementString(@"ProjectDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ProjectDescription));
            WriteElementString(@"ProjectAddress", @"", ((global::System.String)o.@ProjectAddress));
            WriteElementString(@"ProjectFunction", @"", ((global::System.String)o.@ProjectFunction));
            WriteElementString(@"ProjectProcurement", @"", ((global::System.String)o.@ProjectProcurement));
            Write45_DecimalValueType(@"ProjectFunctionalUnit", @"", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@ProjectFunctionalUnit), false, false);
            Write45_DecimalValueType(@"ProjectExpectedLifeCycle", @"", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@ProjectExpectedLifeCycle), false, false);
            Write45_DecimalValueType(@"ProjectCost", @"", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@ProjectCost), false, false);
            WriteElementStringRaw(@"ProjectConstructionStart", @"", FromDate(((global::System.DateTime)o.@ProjectConstructionStart)));
            WriteElementStringRaw(@"ProjectConstructionEnd", @"", FromDate(((global::System.DateTime)o.@ProjectConstructionEnd)));
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType>)((global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType>)o.@ProjectStages);
                if (a != null){
                    WriteStartElement(@"ProjectStages", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write97_ProjectStageType(@"ProjectStage", @"", ((global::Xbim.COBieLiteUK.ProjectStageType)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write97_ProjectStageType(string n, string ns, global::Xbim.COBieLiteUK.ProjectStageType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ProjectStageType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ProjectStageType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalID", @"", ((global::System.String)o.@externalID));
            WriteElementString(@"ProjectStageName", @"", ((global::System.String)o.@ProjectStageName));
            WriteElementString(@"ProjectStageDescription", @"", ((global::System.String)o.@ProjectStageDescription));
            WriteElementString(@"ProjectStageCode", @"", ((global::System.String)o.@ProjectStageCode));
            WriteElementStringRaw(@"ProjectStageStartDate", @"", FromDate(((global::System.DateTime)o.@ProjectStageStartDate)));
            WriteElementStringRaw(@"ProjectStageEndDate", @"", FromDate(((global::System.DateTime)o.@ProjectStageEndDate)));
            Write45_DecimalValueType(@"ProjectStageCost", @"", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@ProjectStageCost), false, false);
            WriteElementString(@"ProjectStageEnvironmentalAssesmentRating", @"", ((global::System.String)o.@ProjectStageEnvironmentalAssesmentRating));
            Write78_AttributeCollectionType(@"ProjectStageAttributes", @"", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@ProjectStageAttributes), false, false);
            Write89_FacilityType(@"Facility", @"", ((global::Xbim.COBieLiteUK.FacilityType)o.@Facility), false, false);
            Write96_ProjectStageKeyType(@"ProjectStagePrevious", @"", ((global::Xbim.COBieLiteUK.ProjectStageKeyType)o.@ProjectStagePrevious), false, false);
            Write96_ProjectStageKeyType(@"ProjectStageNext", @"", ((global::Xbim.COBieLiteUK.ProjectStageKeyType)o.@ProjectStageNext), false, false);
            WriteEndElement(o);
        }

        void Write96_ProjectStageKeyType(string n, string ns, global::Xbim.COBieLiteUK.ProjectStageKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ProjectStageKeyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ProjectStageKeyType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalIDRefer", @"", ((global::System.String)o.@externalIDRefer));
            WriteEndElement(o);
        }

        void Write89_FacilityType(string n, string ns, global::Xbim.COBieLiteUK.FacilityType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.FacilityType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"FacilityTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"FacilityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityName));
            WriteElementString(@"FacilityCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityCategory));
            Write95_ProjectTypeBase(@"ProjectAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ProjectTypeBase)o.@ProjectAssignment), false, false);
            Write85_SiteType(@"SiteAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SiteType)o.@SiteAssignment), false, false);
            if (o.@FacilityDefaultLinearUnitSpecified) {
                WriteElementString(@"FacilityDefaultLinearUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write86_LinearUnitSimpleType(((global::Xbim.COBieLiteUK.LinearUnitSimpleType)o.@FacilityDefaultLinearUnit)));
            }
            if (o.@FacilityDefaultAreaUnitSpecified) {
                WriteElementString(@"FacilityDefaultAreaUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write87_AreaUnitSimpleType(((global::Xbim.COBieLiteUK.AreaUnitSimpleType)o.@FacilityDefaultAreaUnit)));
            }
            if (o.@FacilityDefaultVolumeUnitSpecified) {
                WriteElementString(@"FacilityDefaultVolumeUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write88_VolumeUnitSimpleType(((global::Xbim.COBieLiteUK.VolumeUnitSimpleType)o.@FacilityDefaultVolumeUnit)));
            }
            if (o.@FacilityDefaultCurrencyUnitSpecified) {
                WriteElementString(@"FacilityDefaultCurrencyUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write74_CurrencyUnitSimpleType(((global::Xbim.COBieLiteUK.CurrencyUnitSimpleType)o.@FacilityDefaultCurrencyUnit)));
            }
            WriteElementString(@"FacilityDefaultMeasurementStandard", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityDefaultMeasurementStandard));
            WriteElementString(@"FacilityDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityDescription));
            WriteElementString(@"FacilityDeliverablePhaseName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FacilityDeliverablePhaseName));
            Write65_FloorCollectionType(@"Floors", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.FloorCollectionType)o.@Floors), false, false);
            Write34_ZoneCollectionType(@"Zones", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ZoneCollectionType)o.@Zones), false, false);
            Write59_AssetTypeCollectionType(@"AssetTypes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssetTypeCollectionType)o.@AssetTypes), false, false);
            Write30_SystemCollectionType(@"Systems", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SystemCollectionType)o.@Systems), false, false);
            Write70_ConnectionCollectionType(@"Connections", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ConnectionCollectionType)o.@Connections), false, false);
            Write68_ContactCollectionType(@"Contacts", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactCollectionType)o.@Contacts), false, false);
            Write78_AttributeCollectionType(@"FacilityAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@FacilityAttributes), false, false);
            Write28_DocumentCollectionType(@"FacilityDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@FacilityDocuments), false, false);
            Write26_IssueCollectionType(@"FacilityIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@FacilityIssues), false, false);
            WriteEndElement(o);
        }

        void Write26_IssueCollectionType(string n, string ns, global::Xbim.COBieLiteUK.IssueCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.IssueCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"IssueCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.IssueType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.IssueType>)o.@Issue;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write25_IssueType(@"Issue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write25_IssueType(string n, string ns, global::Xbim.COBieLiteUK.IssueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.IssueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"IssueTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"IssueName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueName));
            WriteElementString(@"IssueCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueCategory));
            WriteElementString(@"IssueRiskText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueRiskText));
            WriteElementString(@"IssueSeverityText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueSeverityText));
            WriteElementString(@"IssueImpactText", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueImpactText));
            WriteElementString(@"IssueDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueDescription));
            Write2_ContactKeyType(@"ContactAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactKeyType)o.@ContactAssignment), false, false);
            WriteElementString(@"IssueMitigationDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@IssueMitigationDescription));
            WriteEndElement(o);
        }

        void Write2_ContactKeyType(string n, string ns, global::Xbim.COBieLiteUK.ContactKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ContactKeyType)) {
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

        void Write28_DocumentCollectionType(string n, string ns, global::Xbim.COBieLiteUK.DocumentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.DocumentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.DocumentType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.DocumentType>)o.@Document;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write27_DocumentType(@"Document", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write27_DocumentType(string n, string ns, global::Xbim.COBieLiteUK.DocumentType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.DocumentType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"DocumentName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentName));
            WriteElementString(@"DocumentCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentCategory));
            WriteElementString(@"DocumentDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentDescription));
            WriteElementString(@"DocumentURI", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentURI));
            WriteElementString(@"DocumentReferenceURI", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@DocumentReferenceURI));
            Write78_AttributeCollectionType(@"DocumentAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@DocumentAttributes), false, false);
            Write26_IssueCollectionType(@"DocumentIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@DocumentIssues), false, false);
            WriteEndElement(o);
        }

        void Write78_AttributeCollectionType(string n, string ns, global::Xbim.COBieLiteUK.AttributeCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AttributeType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AttributeType>)o.@Attribute;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write77_AttributeType(@"Attribute", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write77_AttributeType(string n, string ns, global::Xbim.COBieLiteUK.AttributeType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteAttribute(@"propertySetExternalIdentifier", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@propertySetExternalIdentifier));
            WriteAttribute(@"propertySetName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@propertySetName));
            WriteElementString(@"AttributeName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AttributeName));
            WriteElementString(@"AttributeCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AttributeCategory));
            WriteElementString(@"AttributeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AttributeDescription));
            Write76_AttributeValueType(@"AttributeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeValueType)o.@AttributeValue), false, false);
            Write26_IssueCollectionType(@"AttributeIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@AttributeIssues), false, false);
            WriteEndElement(o);
        }

        void Write76_AttributeValueType(string n, string ns, global::Xbim.COBieLiteUK.AttributeValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeDecimalValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLiteUK.AttributeDecimalValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLiteUK.AttributeDecimalValueType", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeDecimalValue");
                    Write43_AttributeDecimalValueType(@"AttributeDecimalValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeDecimalValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeStringValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLiteUK.AttributeStringValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLiteUK.AttributeStringValueType", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeStringValue");
                    Write39_AttributeStringValueType(@"AttributeStringValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeStringValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeIntegerValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLiteUK.AttributeIntegerValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLiteUK.AttributeIntegerValueType", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeIntegerValue");
                    Write40_AttributeIntegerValueType(@"AttributeIntegerValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeIntegerValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeBooleanValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLiteUK.BooleanValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLiteUK.BooleanValueType", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeBooleanValue");
                    Write42_BooleanValueType(@"AttributeBooleanValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.BooleanValueType)o.@Item), false, false);
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeDateTimeValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::System.DateTime)) throw CreateMismatchChoiceException(@"System.DateTime", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeDateTimeValue");
                    WriteElementStringRaw(@"AttributeDateTimeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDateTime(((global::System.DateTime)o.@Item)));
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeTimeValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::System.DateTime)) throw CreateMismatchChoiceException(@"System.DateTime", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeTimeValue");
                    WriteElementStringRaw(@"AttributeTimeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromTime(((global::System.DateTime)o.@Item)));
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeDateValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::System.DateTime)) throw CreateMismatchChoiceException(@"System.DateTime", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeDateValue");
                    WriteElementStringRaw(@"AttributeDateValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDate(((global::System.DateTime)o.@Item)));
                }
                else if (o.@ItemElementName == Xbim.COBieLiteUK.ItemChoiceType.@AttributeMonetaryValue && ((object)(o.@Item) != null)) {
                    if (((object)o.@Item) != null && !(o.@Item is global::Xbim.COBieLiteUK.AttributeMonetaryValueType)) throw CreateMismatchChoiceException(@"Xbim.COBieLiteUK.AttributeMonetaryValueType", @"ItemElementName", @"Xbim.COBieLiteUK.ItemChoiceType.@AttributeMonetaryValue");
                    Write75_AttributeMonetaryValueType(@"AttributeMonetaryValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeMonetaryValueType)o.@Item), false, false);
                }
                else  if ((object)(o.@Item) != null){
                    throw CreateUnknownTypeException(o.@Item);
                }
            }
            WriteEndElement(o);
        }

        void Write75_AttributeMonetaryValueType(string n, string ns, global::Xbim.COBieLiteUK.AttributeMonetaryValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeMonetaryValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeMonetaryValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementStringRaw(@"MonetaryValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", System.Xml.XmlConvert.ToString((global::System.Decimal)((global::System.Decimal)o.@MonetaryValue)));
            WriteElementString(@"MonetaryUnit", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write74_CurrencyUnitSimpleType(((global::Xbim.COBieLiteUK.CurrencyUnitSimpleType)o.@MonetaryUnit)));
            WriteEndElement(o);
        }

        string Write74_CurrencyUnitSimpleType(global::Xbim.COBieLiteUK.CurrencyUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@BritishPounds: s = @"British Pounds"; break;
                case global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@Dollars: s = @"Dollars"; break;
                case global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@Euros: s = @"Euros"; break;
                case global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@Item: s = @""; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLiteUK.CurrencyUnitSimpleType");
            }
            return s;
        }

        void Write42_BooleanValueType(string n, string ns, global::Xbim.COBieLiteUK.BooleanValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.BooleanValueType)) {
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

        void Write40_AttributeIntegerValueType(string n, string ns, global::Xbim.COBieLiteUK.AttributeIntegerValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeIntegerValueType)) {
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

        void Write39_AttributeStringValueType(string n, string ns, global::Xbim.COBieLiteUK.AttributeStringValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeStringValueType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AttributeStringValueType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            WriteElementString(@"UnitName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@UnitName));
            WriteElementString(@"StringValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@StringValue));
            Write38_AllowedValueCollectionType(@"AllowedValues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AllowedValueCollectionType)o.@AllowedValues), false, false);
            WriteEndElement(o);
        }

        void Write38_AllowedValueCollectionType(string n, string ns, global::Xbim.COBieLiteUK.AllowedValueCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AllowedValueCollectionType)) {
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

        void Write43_AttributeDecimalValueType(string n, string ns, global::Xbim.COBieLiteUK.AttributeDecimalValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AttributeDecimalValueType)) {
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

        void Write68_ContactCollectionType(string n, string ns, global::Xbim.COBieLiteUK.ContactCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ContactCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactType>)o.@Contact;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write67_ContactType(@"Contact", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write67_ContactType(string n, string ns, global::Xbim.COBieLiteUK.ContactType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ContactType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
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
            Write78_AttributeCollectionType(@"ContactAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@ContactAttributes), false, false);
            Write28_DocumentCollectionType(@"ContactDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@ContactDocuments), false, false);
            Write26_IssueCollectionType(@"ContactIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@ContactIssues), false, false);
            WriteEndElement(o);
        }

        void Write70_ConnectionCollectionType(string n, string ns, global::Xbim.COBieLiteUK.ConnectionCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ConnectionCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ConnectionCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ConnectionType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ConnectionType>)o.@Connection;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write69_ConnectionType(@"Connection", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ConnectionType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write69_ConnectionType(string n, string ns, global::Xbim.COBieLiteUK.ConnectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ConnectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ConnectionTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
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
            Write78_AttributeCollectionType(@"ConnectionAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@ConnectionAttributes), false, false);
            Write28_DocumentCollectionType(@"ConnectionDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@ConnectionDocuments), false, false);
            Write26_IssueCollectionType(@"ConnectionIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@ConnectionIssues), false, false);
            WriteEndElement(o);
        }

        void Write30_SystemCollectionType(string n, string ns, global::Xbim.COBieLiteUK.SystemCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SystemCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemType>)o.@System;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write29_SystemType(@"System", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SystemType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write29_SystemType(string n, string ns, global::Xbim.COBieLiteUK.SystemType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SystemType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemName));
            WriteElementString(@"SystemCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemCategory));
            WriteElementString(@"SystemDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SystemDescription));
            Write78_AttributeCollectionType(@"SystemAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@SystemAttributes), false, false);
            Write28_DocumentCollectionType(@"SystemDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@SystemDocuments), false, false);
            Write26_IssueCollectionType(@"SystemIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@SystemIssues), false, false);
            WriteEndElement(o);
        }

        void Write59_AssetTypeCollectionType(string n, string ns, global::Xbim.COBieLiteUK.AssetTypeCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AssetTypeCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetTypeCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetTypeInfoType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetTypeInfoType>)o.@AssetType;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write58_AssetTypeInfoType(@"AssetType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssetTypeInfoType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write58_AssetTypeInfoType(string n, string ns, global::Xbim.COBieLiteUK.AssetTypeInfoType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AssetTypeInfoType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetTypeInfoTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"AssetTypeName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeName));
            WriteElementString(@"AssetTypeCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeCategory));
            WriteElementString(@"AssetTypeDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeDescription));
            if (o.@AssetTypeAccountingCategorySpecified) {
                WriteElementString(@"AssetTypeAccountingCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", Write36_AssetPortabilitySimpleType(((global::Xbim.COBieLiteUK.AssetPortabilitySimpleType)o.@AssetTypeAccountingCategory)));
            }
            WriteElementString(@"AssetTypeModelNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetTypeModelNumber));
            Write45_DecimalValueType(@"AssetTypeReplacementCostValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@AssetTypeReplacementCostValue), false, false);
            Write41_IntegerValueType(@"AssetTypeExpectedLifeValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IntegerValueType)o.@AssetTypeExpectedLifeValue), false, false);
            Write45_DecimalValueType(@"AssetTypeNominalLength", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@AssetTypeNominalLength), false, false);
            Write45_DecimalValueType(@"AssetTypeNominalWidth", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@AssetTypeNominalWidth), false, false);
            Write45_DecimalValueType(@"AssetTypeNominalHeight", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@AssetTypeNominalHeight), false, false);
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
            Write49_AssetCollectionType(@"Assets", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssetCollectionType)o.@Assets), false, false);
            Write82_Item(@"AssetTypeManufacturerContactAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactAssignmentCollectionType)o.@AssetTypeManufacturerContactAssignments), false, false);
            Write51_WarrantyCollectionType(@"Warranties", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.WarrantyCollectionType)o.@Warranties), false, false);
            Write31_SpareCollectionType(@"Spares", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SpareCollectionType)o.@Spares), false, false);
            Write57_JobCollectionType(@"Jobs", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.JobCollectionType)o.@Jobs), false, false);
            Write80_Item(@"AssemblyAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType)o.@AssemblyAssignments), false, false);
            Write78_AttributeCollectionType(@"AssetTypeAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@AssetTypeAttributes), false, false);
            Write28_DocumentCollectionType(@"AssetTypeDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@AssetTypeDocuments), false, false);
            Write26_IssueCollectionType(@"AssetTypeIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@AssetTypeIssues), false, false);
            WriteEndElement(o);
        }

        void Write80_Item(string n, string ns, global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssemblyAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssemblyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssemblyType>)o.@AssemblyAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write79_AssemblyType(@"AssemblyAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssemblyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write79_AssemblyType(string n, string ns, global::Xbim.COBieLiteUK.AssemblyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AssemblyType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssemblyTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"AssemblyName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyName));
            WriteElementString(@"AssemblyParentName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyParentName));
            WriteElementString(@"AssemblyCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyCategory));
            WriteElementString(@"AssemblyDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssemblyDescription));
            Write78_AttributeCollectionType(@"AssemblyAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@AssemblyAttributes), false, false);
            Write28_DocumentCollectionType(@"AssemblyDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@AssemblyDocuments), false, false);
            Write26_IssueCollectionType(@"AssemblyIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@AssemblyIssues), false, false);
            WriteEndElement(o);
        }

        void Write57_JobCollectionType(string n, string ns, global::Xbim.COBieLiteUK.JobCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.JobCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"JobCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.JobType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.JobType>)o.@Job;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write56_JobType(@"Job", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.JobType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write56_JobType(string n, string ns, global::Xbim.COBieLiteUK.JobType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.JobType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"JobTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"JobName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobName));
            WriteElementString(@"JobTaskID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobTaskID));
            WriteElementString(@"JobCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobCategory));
            WriteElementString(@"JobStatus", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobStatus));
            WriteElementString(@"JobDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobDescription));
            Write41_IntegerValueType(@"JobDuration", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IntegerValueType)o.@JobDuration), false, false);
            if (o.@JobStartDateSpecified) {
                WriteElementStringRaw(@"JobStartDate", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", FromDate(((global::System.DateTime)o.@JobStartDate)));
            }
            WriteElementString(@"JobStartConditionDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobStartConditionDescription));
            Write45_DecimalValueType(@"JobFrequencyValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@JobFrequencyValue), false, false);
            WriteElementString(@"JobPriorTaskID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@JobPriorTaskID));
            Write55_ResourceCollectionType(@"Resources", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ResourceCollectionType)o.@Resources), false, false);
            Write78_AttributeCollectionType(@"JobAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@JobAttributes), false, false);
            Write28_DocumentCollectionType(@"JobDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@JobDocuments), false, false);
            Write26_IssueCollectionType(@"JobIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@JobIssues), false, false);
            WriteEndElement(o);
        }

        void Write55_ResourceCollectionType(string n, string ns, global::Xbim.COBieLiteUK.ResourceCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ResourceCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ResourceCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ResourceType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ResourceType>)o.@Resource;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write54_ResourceType(@"Resource", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ResourceType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write54_ResourceType(string n, string ns, global::Xbim.COBieLiteUK.ResourceType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ResourceType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ResourceTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ResourceName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ResourceName));
            WriteElementString(@"ResourceCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ResourceCategory));
            WriteElementString(@"ResourceDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ResourceDescription));
            Write78_AttributeCollectionType(@"ResourceAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@ResourceAttributes), false, false);
            Write28_DocumentCollectionType(@"ResourceDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@ResourceDocuments), false, false);
            Write26_IssueCollectionType(@"ResourceIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@ResourceIssues), false, false);
            WriteEndElement(o);
        }

        void Write45_DecimalValueType(string n, string ns, global::Xbim.COBieLiteUK.DecimalValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.DecimalValueType)) {
                }
                else if (t == typeof(global::Xbim.COBieLiteUK.AttributeDecimalValueType)) {
                    Write43_AttributeDecimalValueType(n, ns,(global::Xbim.COBieLiteUK.AttributeDecimalValueType)o, isNullable, true);
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

        void Write41_IntegerValueType(string n, string ns, global::Xbim.COBieLiteUK.IntegerValueType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.IntegerValueType)) {
                }
                else if (t == typeof(global::Xbim.COBieLiteUK.AttributeIntegerValueType)) {
                    Write40_AttributeIntegerValueType(n, ns,(global::Xbim.COBieLiteUK.AttributeIntegerValueType)o, isNullable, true);
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

        void Write31_SpareCollectionType(string n, string ns, global::Xbim.COBieLiteUK.SpareCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SpareCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpareCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpareType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpareType>)o.@Spare;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write83_SpareType(@"Spare", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SpareType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write83_SpareType(string n, string ns, global::Xbim.COBieLiteUK.SpareType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SpareType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpareTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SpareName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareName));
            WriteElementString(@"SpareCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareCategory));
            WriteElementString(@"SpareDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareDescription));
            WriteElementString(@"SpareSetNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpareSetNumber));
            WriteElementString(@"SparePartNumber", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SparePartNumber));
            Write82_Item(@"SpareSupplierContactAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactAssignmentCollectionType)o.@SpareSupplierContactAssignments), false, false);
            Write78_AttributeCollectionType(@"SpareAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@SpareAttributes), false, false);
            Write28_DocumentCollectionType(@"SpareDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@SpareDocuments), false, false);
            Write26_IssueCollectionType(@"SpareIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@SpareIssues), false, false);
            WriteEndElement(o);
        }

        void Write82_Item(string n, string ns, global::Xbim.COBieLiteUK.ContactAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ContactAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ContactAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactKeyType>)o.@ContactAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write2_ContactKeyType(@"ContactAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write51_WarrantyCollectionType(string n, string ns, global::Xbim.COBieLiteUK.WarrantyCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.WarrantyCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"WarrantyCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.WarrantyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.WarrantyType>)o.@Warranty;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write50_WarrantyType(@"Warranty", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.WarrantyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write50_WarrantyType(string n, string ns, global::Xbim.COBieLiteUK.WarrantyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.WarrantyType)) {
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
            Write41_IntegerValueType(@"WarrantyDuration", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IntegerValueType)o.@WarrantyDuration), false, false);
            Write82_Item(@"WarrantyGaurantorContactAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ContactAssignmentCollectionType)o.@WarrantyGaurantorContactAssignments), false, false);
            Write78_AttributeCollectionType(@"WarrantyAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@WarrantyAttributes), false, false);
            Write28_DocumentCollectionType(@"WarrantyDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@WarrantyDocuments), false, false);
            Write26_IssueCollectionType(@"WarrantyIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@WarrantyIssues), false, false);
            WriteEndElement(o);
        }

        void Write49_AssetCollectionType(string n, string ns, global::Xbim.COBieLiteUK.AssetCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AssetCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetInfoType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetInfoType>)o.@Asset;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write48_AssetInfoType(@"Asset", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssetInfoType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write48_AssetInfoType(string n, string ns, global::Xbim.COBieLiteUK.AssetInfoType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.AssetInfoType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetInfoTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"AssetName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@AssetName));
            Write16_SpaceAssignmentCollectionType(@"AssetSpaceAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SpaceAssignmentCollectionType)o.@AssetSpaceAssignments), false, false);
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
            Write47_SystemAssignmentCollectionType(@"AssetSystemAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SystemAssignmentCollectionType)o.@AssetSystemAssignments), false, false);
            Write80_Item(@"AssemblyAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType)o.@AssemblyAssignments), false, false);
            Write78_AttributeCollectionType(@"AssetAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@AssetAttributes), false, false);
            Write28_DocumentCollectionType(@"AssetDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@AssetDocuments), false, false);
            Write26_IssueCollectionType(@"AssetIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@AssetIssues), false, false);
            WriteEndElement(o);
        }

        void Write47_SystemAssignmentCollectionType(string n, string ns, global::Xbim.COBieLiteUK.SystemAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SystemAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SystemAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemKeyType>)o.@SystemAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write9_SystemKeyType(@"SystemAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SystemKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write9_SystemKeyType(string n, string ns, global::Xbim.COBieLiteUK.SystemKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SystemKeyType)) {
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

        void Write16_SpaceAssignmentCollectionType(string n, string ns, global::Xbim.COBieLiteUK.SpaceAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SpaceAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceKeyType>)o.@SpaceAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write12_SpaceKeyType(@"SpaceAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SpaceKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write12_SpaceKeyType(string n, string ns, global::Xbim.COBieLiteUK.SpaceKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SpaceKeyType)) {
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

        string Write36_AssetPortabilitySimpleType(global::Xbim.COBieLiteUK.AssetPortabilitySimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLiteUK.AssetPortabilitySimpleType.@Fixed: s = @"Fixed"; break;
                case global::Xbim.COBieLiteUK.AssetPortabilitySimpleType.@Moveable: s = @"Moveable"; break;
                case global::Xbim.COBieLiteUK.AssetPortabilitySimpleType.@Item: s = @""; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLiteUK.AssetPortabilitySimpleType");
            }
            return s;
        }

        void Write34_ZoneCollectionType(string n, string ns, global::Xbim.COBieLiteUK.ZoneCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ZoneCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneType>)o.@Zone;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write33_ZoneType(@"Zone", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ZoneType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write33_ZoneType(string n, string ns, global::Xbim.COBieLiteUK.ZoneType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ZoneType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"ZoneName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneName));
            WriteElementString(@"ZoneCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneCategory));
            WriteElementString(@"ZoneDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@ZoneDescription));
            Write78_AttributeCollectionType(@"ZoneAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@ZoneAttributes), false, false);
            Write28_DocumentCollectionType(@"ZoneDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@ZoneDocuments), false, false);
            Write26_IssueCollectionType(@"ZoneIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@ZoneIssues), false, false);
            WriteEndElement(o);
        }

        void Write65_FloorCollectionType(string n, string ns, global::Xbim.COBieLiteUK.FloorCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.FloorCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"FloorCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.FloorType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.FloorType>)o.@Floor;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write64_FloorType(@"Floor", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.FloorType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write64_FloorType(string n, string ns, global::Xbim.COBieLiteUK.FloorType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.FloorType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"FloorTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"FloorName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorName));
            WriteElementString(@"FloorCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorCategory));
            WriteElementString(@"FloorDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@FloorDescription));
            Write45_DecimalValueType(@"FloorElevationValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@FloorElevationValue), false, false);
            Write45_DecimalValueType(@"FloorHeightValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@FloorHeightValue), false, false);
            Write62_SpaceCollectionType(@"Spaces", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SpaceCollectionType)o.@Spaces), false, false);
            Write78_AttributeCollectionType(@"FloorAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@FloorAttributes), false, false);
            Write28_DocumentCollectionType(@"FloorDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@FloorDocuments), false, false);
            Write26_IssueCollectionType(@"FloorIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@FloorIssues), false, false);
            WriteEndElement(o);
        }

        void Write62_SpaceCollectionType(string n, string ns, global::Xbim.COBieLiteUK.SpaceCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SpaceCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceType>)o.@Space;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write61_SpaceType(@"Space", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.SpaceType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write61_SpaceType(string n, string ns, global::Xbim.COBieLiteUK.SpaceType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SpaceType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"SpaceTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            WriteAttribute(@"externalEntityName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalEntityName));
            WriteAttribute(@"externalID", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalID));
            WriteAttribute(@"externalSystemName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@externalSystemName));
            WriteElementString(@"SpaceName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceName));
            WriteElementString(@"SpaceCategory", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceCategory));
            WriteElementString(@"SpaceDescription", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceDescription));
            WriteElementString(@"SpaceSignageName", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::System.String)o.@SpaceSignageName));
            Write45_DecimalValueType(@"SpaceUsableHeightValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@SpaceUsableHeightValue), false, false);
            Write45_DecimalValueType(@"SpaceGrossAreaValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@SpaceGrossAreaValue), false, false);
            Write45_DecimalValueType(@"SpaceNetAreaValue", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DecimalValueType)o.@SpaceNetAreaValue), false, false);
            Write20_ZoneAssignmentCollectionType(@"SpaceZoneAssignments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ZoneAssignmentCollectionType)o.@SpaceZoneAssignments), false, false);
            Write78_AttributeCollectionType(@"SpaceAttributes", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.AttributeCollectionType)o.@SpaceAttributes), false, false);
            Write28_DocumentCollectionType(@"SpaceDocuments", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.DocumentCollectionType)o.@SpaceDocuments), false, false);
            Write26_IssueCollectionType(@"SpaceIssues", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.IssueCollectionType)o.@SpaceIssues), false, false);
            WriteEndElement(o);
        }

        void Write20_ZoneAssignmentCollectionType(string n, string ns, global::Xbim.COBieLiteUK.ZoneAssignmentCollectionType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ZoneAssignmentCollectionType)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ZoneAssignmentCollectionType", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            {
                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneKeyType> a = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneKeyType>)o.@ZoneAssignment;
                if (a != null) {
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
                        Write19_ZoneKeyType(@"ZoneAssignment", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core", ((global::Xbim.COBieLiteUK.ZoneKeyType)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write19_ZoneKeyType(string n, string ns, global::Xbim.COBieLiteUK.ZoneKeyType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ZoneKeyType)) {
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

        string Write88_VolumeUnitSimpleType(global::Xbim.COBieLiteUK.VolumeUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubiccentimeters: s = @"cubic centimeters"; break;
                case global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicfeet: s = @"cubic feet"; break;
                case global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicinches: s = @"cubic inches"; break;
                case global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicmeters: s = @"cubic meters"; break;
                case global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicmillimeters: s = @"cubic millimeters"; break;
                case global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicyards: s = @"cubic yards"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLiteUK.VolumeUnitSimpleType");
            }
            return s;
        }

        string Write87_AreaUnitSimpleType(global::Xbim.COBieLiteUK.AreaUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squarecentimeters: s = @"square centimeters"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squarefeet: s = @"square feet"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squareinches: s = @"square inches"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squarekilometers: s = @"square kilometers"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squaremeters: s = @"square meters"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squaremiles: s = @"square miles"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squaremillimeters: s = @"square millimeters"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squareyards: s = @"square yards"; break;
                case global::Xbim.COBieLiteUK.AreaUnitSimpleType.@Item: s = @""; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLiteUK.AreaUnitSimpleType");
            }
            return s;
        }

        string Write86_LinearUnitSimpleType(global::Xbim.COBieLiteUK.LinearUnitSimpleType v) {
            string s = null;
            switch (v) {
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@centimeters: s = @"centimeters"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@feet: s = @"feet"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@inches: s = @"inches"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@kilometers: s = @"kilometers"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@meters: s = @"meters"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@miles: s = @"miles"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@millimeters: s = @"millimeters"; break;
                case global::Xbim.COBieLiteUK.LinearUnitSimpleType.@yards: s = @"yards"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Xbim.COBieLiteUK.LinearUnitSimpleType");
            }
            return s;
        }

        void Write85_SiteType(string n, string ns, global::Xbim.COBieLiteUK.SiteType o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.SiteType)) {
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

        void Write95_ProjectTypeBase(string n, string ns, global::Xbim.COBieLiteUK.ProjectTypeBase o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Xbim.COBieLiteUK.ProjectTypeBase)) {
                }
                else if (t == typeof(global::Xbim.COBieLiteUK.ProjectType)) {
                    Write98_ProjectType(n, ns,(global::Xbim.COBieLiteUK.ProjectType)o, isNullable, true);
                    return;
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ProjectTypeBase", @"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
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

    public class XmlSerializationReaderProjectType : System.Xml.Serialization.XmlSerializationReader {

        public object Read99_Project() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_Project && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read98_ProjectType(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Project");
            }
            return (object)o;
        }

        global::Xbim.COBieLiteUK.ProjectType Read98_ProjectType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_ProjectTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ProjectType o;
            o = new global::Xbim.COBieLiteUK.ProjectType();
            if ((object)(o.@ProjectStages) == null) o.@ProjectStages = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType> a_13 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType>)o.@ProjectStages;
            bool[] paramsRead = new bool[15];
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
                else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id8_Item)) {
                    o.@externalID1 = Reader.Value;
                    paramsRead[14] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName, :externalID");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            int state = 0;
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id9_ProjectName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ProjectName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id10_ProjectDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ProjectDescription = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id11_ProjectAddress && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectAddress = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id12_ProjectFunction && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectFunction = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id13_ProjectProcurement && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectProcurement = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id14_ProjectFunctionalUnit && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectFunctionalUnit = Read45_DecimalValueType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id15_ProjectExpectedLifeCycle && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectExpectedLifeCycle = Read45_DecimalValueType(false, true);
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id16_ProjectCost && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectCost = Read45_DecimalValueType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id17_ProjectConstructionStart && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectConstructionStart = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id18_ProjectConstructionEnd && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectConstructionEnd = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id19_ProjectStages && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            if (!ReadNull()) {
                                if ((object)(o.@ProjectStages) == null) o.@ProjectStages = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType>();
                                global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType> a_13_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ProjectStageType>)o.@ProjectStages;
                                if ((Reader.IsEmptyElement)) {
                                    Reader.Skip();
                                }
                                else {
                                    Reader.ReadStartElement();
                                    Reader.MoveToContent();
                                    int whileIterations1 = 0;
                                    int readerCount1 = ReaderCount;
                                    while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                        if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                            if (((object) Reader.LocalName == (object)id20_ProjectStage && (object) Reader.NamespaceURI == (object)id8_Item)) {
                                                if ((object)(a_13_0) == null) Reader.Skip(); else a_13_0.Add(Read97_ProjectStageType(false, true));
                                            }
                                            else {
                                                UnknownNode(null, @":ProjectStage");
                                            }
                                        }
                                        else {
                                            UnknownNode(null, @":ProjectStage");
                                        }
                                        Reader.MoveToContent();
                                        CheckReaderCount(ref whileIterations1, ref readerCount1);
                                    }
                                ReadEndElement();
                                }
                            }
                        }
                        else {
                            state = 11;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ProjectStageType Read97_ProjectStageType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id21_ProjectStageType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ProjectStageType o;
            o = new global::Xbim.COBieLiteUK.ProjectStageType();
            bool[] paramsRead = new bool[12];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[11] && ((object) Reader.LocalName == (object)id6_externalID && (object) Reader.NamespaceURI == (object)id8_Item)) {
                    o.@externalID = Reader.Value;
                    paramsRead[11] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":externalID");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            int state = 0;
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id22_ProjectStageName && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectStageName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id23_ProjectStageDescription && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectStageDescription = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id24_ProjectStageCode && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectStageCode = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id25_ProjectStageStartDate && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectStageStartDate = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id26_ProjectStageEndDate && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectStageEndDate = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id27_ProjectStageCost && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectStageCost = Read45_DecimalValueType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id28_Item && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            {
                                o.@ProjectStageEnvironmentalAssesmentRating = Reader.ReadElementString();
                            }
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id29_ProjectStageAttributes && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectStageAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id30_Facility && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@Facility = Read89_FacilityType(false, true);
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id31_ProjectStagePrevious && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectStagePrevious = Read96_ProjectStageKeyType(false, true);
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id32_ProjectStageNext && (object) Reader.NamespaceURI == (object)id8_Item)) {
                            o.@ProjectStageNext = Read96_ProjectStageKeyType(false, true);
                        }
                        state = 11;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ProjectStageKeyType Read96_ProjectStageKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id33_ProjectStageKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ProjectStageKeyType o;
            o = new global::Xbim.COBieLiteUK.ProjectStageKeyType();
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id34_externalIDRefer && (object) Reader.NamespaceURI == (object)id8_Item)) {
                    o.@externalIDRefer = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":externalIDRefer");
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
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations3, ref readerCount3);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.FacilityType Read89_FacilityType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id35_FacilityTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.FacilityType o;
            o = new global::Xbim.COBieLiteUK.FacilityType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations4 = 0;
            int readerCount4 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id36_FacilityName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FacilityName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id37_FacilityCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FacilityCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id38_ProjectAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ProjectAssignment = Read95_ProjectTypeBase(false, true);
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id39_SiteAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SiteAssignment = Read85_SiteType(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id40_FacilityDefaultLinearUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityDefaultLinearUnitSpecified = true;
                            {
                                o.@FacilityDefaultLinearUnit = Read86_LinearUnitSimpleType(Reader.ReadElementString());
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id41_FacilityDefaultAreaUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityDefaultAreaUnitSpecified = true;
                            {
                                o.@FacilityDefaultAreaUnit = Read87_AreaUnitSimpleType(Reader.ReadElementString());
                            }
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id42_FacilityDefaultVolumeUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityDefaultVolumeUnitSpecified = true;
                            {
                                o.@FacilityDefaultVolumeUnit = Read88_VolumeUnitSimpleType(Reader.ReadElementString());
                            }
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id43_FacilityDefaultCurrencyUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityDefaultCurrencyUnitSpecified = true;
                            {
                                o.@FacilityDefaultCurrencyUnit = Read74_CurrencyUnitSimpleType(Reader.ReadElementString());
                            }
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id44_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FacilityDefaultMeasurementStandard = Reader.ReadElementString();
                            }
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id45_FacilityDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FacilityDescription = Reader.ReadElementString();
                            }
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id46_FacilityDeliverablePhaseName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FacilityDeliverablePhaseName = Reader.ReadElementString();
                            }
                        }
                        state = 11;
                        break;
                    case 11:
                        if (((object) Reader.LocalName == (object)id47_Floors && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Floors = Read65_FloorCollectionType(false, true);
                        }
                        state = 12;
                        break;
                    case 12:
                        if (((object) Reader.LocalName == (object)id48_Zones && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Zones = Read34_ZoneCollectionType(false, true);
                        }
                        state = 13;
                        break;
                    case 13:
                        if (((object) Reader.LocalName == (object)id49_AssetTypes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypes = Read59_AssetTypeCollectionType(false, true);
                        }
                        state = 14;
                        break;
                    case 14:
                        if (((object) Reader.LocalName == (object)id50_Systems && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Systems = Read30_SystemCollectionType(false, true);
                        }
                        state = 15;
                        break;
                    case 15:
                        if (((object) Reader.LocalName == (object)id51_Connections && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Connections = Read70_ConnectionCollectionType(false, true);
                        }
                        state = 16;
                        break;
                    case 16:
                        if (((object) Reader.LocalName == (object)id52_Contacts && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Contacts = Read68_ContactCollectionType(false, true);
                        }
                        state = 17;
                        break;
                    case 17:
                        if (((object) Reader.LocalName == (object)id53_FacilityAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 18;
                        break;
                    case 18:
                        if (((object) Reader.LocalName == (object)id54_FacilityDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 19;
                        break;
                    case 19:
                        if (((object) Reader.LocalName == (object)id55_FacilityIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FacilityIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 20;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.IssueCollectionType Read26_IssueCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id56_IssueCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.IssueCollectionType o;
            o = new global::Xbim.COBieLiteUK.IssueCollectionType();
            if ((object)(o.@Issue) == null) o.@Issue = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.IssueType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.IssueType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.IssueType>)o.@Issue;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations5 = 0;
            int readerCount5 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id57_Issue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read25_IssueType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations5, ref readerCount5);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.IssueType Read25_IssueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id58_IssueTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.IssueType o;
            o = new global::Xbim.COBieLiteUK.IssueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id59_IssueName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id60_IssueCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id61_IssueRiskText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueRiskText = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id62_IssueSeverityText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueSeverityText = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id63_IssueImpactText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueImpactText = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id64_IssueDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueDescription = Reader.ReadElementString();
                            }
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id65_ContactAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ContactAssignment = Read2_ContactKeyType(false, true);
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id66_IssueMitigationDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IssueMitigationDescription = Reader.ReadElementString();
                            }
                        }
                        state = 8;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ContactKeyType Read2_ContactKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id67_ContactKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ContactKeyType o;
            o = new global::Xbim.COBieLiteUK.ContactKeyType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations7 = 0;
            int readerCount7 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id68_ContactEmailReference && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactEmailReference = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations7, ref readerCount7);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.DocumentCollectionType Read28_DocumentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id69_DocumentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.DocumentCollectionType o;
            o = new global::Xbim.COBieLiteUK.DocumentCollectionType();
            if ((object)(o.@Document) == null) o.@Document = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.DocumentType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.DocumentType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.DocumentType>)o.@Document;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id70_Document && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read27_DocumentType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.DocumentType Read27_DocumentType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id71_DocumentTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.DocumentType o;
            o = new global::Xbim.COBieLiteUK.DocumentType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id72_DocumentName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@DocumentName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id73_DocumentCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@DocumentCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id74_DocumentDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@DocumentDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id75_DocumentURI && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@DocumentURI = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id76_DocumentReferenceURI && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@DocumentReferenceURI = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id77_DocumentAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@DocumentAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id78_DocumentIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@DocumentIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 7;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AttributeCollectionType Read78_AttributeCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id79_AttributeCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeCollectionType o;
            o = new global::Xbim.COBieLiteUK.AttributeCollectionType();
            if ((object)(o.@Attribute) == null) o.@Attribute = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AttributeType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AttributeType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AttributeType>)o.@Attribute;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations10 = 0;
            int readerCount10 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id80_Attribute && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read77_AttributeType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations10, ref readerCount10);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AttributeType Read77_AttributeType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id81_AttributeTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeType o;
            o = new global::Xbim.COBieLiteUK.AttributeType();
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
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id82_propertySetExternalIdentifier && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@propertySetExternalIdentifier = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id83_propertySetName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                    o.@propertySetName = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalEntityName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalID, http://docs.buildingsmartalliance.org/nbims03/cobie/core:externalSystemName, http://docs.buildingsmartalliance.org/nbims03/cobie/core:propertySetExternalIdentifier, http://docs.buildingsmartalliance.org/nbims03/cobie/core:propertySetName");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            int state = 0;
            Reader.MoveToContent();
            int whileIterations11 = 0;
            int readerCount11 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id84_AttributeName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AttributeName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id85_AttributeCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AttributeCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id86_AttributeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AttributeDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id87_AttributeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AttributeValue = Read76_AttributeValueType(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id88_AttributeIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AttributeIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 5;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations11, ref readerCount11);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AttributeValueType Read76_AttributeValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id89_AttributeValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeValueType o;
            o = new global::Xbim.COBieLiteUK.AttributeValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations12 = 0;
            int readerCount12 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id90_AttributeIntegerValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Item = Read40_AttributeIntegerValueType(false, true);
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeIntegerValue;
                        }
                        else if (((object) Reader.LocalName == (object)id91_AttributeDateValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@Item = ToDate(Reader.ReadElementString());
                            }
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeDateValue;
                        }
                        else if (((object) Reader.LocalName == (object)id92_AttributeDecimalValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Item = Read43_AttributeDecimalValueType(false, true);
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeDecimalValue;
                        }
                        else if (((object) Reader.LocalName == (object)id93_AttributeDateTimeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@Item = ToDateTime(Reader.ReadElementString());
                            }
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeDateTimeValue;
                        }
                        else if (((object) Reader.LocalName == (object)id94_AttributeStringValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Item = Read39_AttributeStringValueType(false, true);
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeStringValue;
                        }
                        else if (((object) Reader.LocalName == (object)id95_AttributeTimeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@Item = ToTime(Reader.ReadElementString());
                            }
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeTimeValue;
                        }
                        else if (((object) Reader.LocalName == (object)id96_AttributeMonetaryValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Item = Read75_AttributeMonetaryValueType(false, true);
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeMonetaryValue;
                        }
                        else if (((object) Reader.LocalName == (object)id97_AttributeBooleanValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Item = Read42_BooleanValueType(false, true);
                            o.@ItemElementName = global::Xbim.COBieLiteUK.ItemChoiceType.@AttributeBooleanValue;
                        }
                        state = 1;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations12, ref readerCount12);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.BooleanValueType Read42_BooleanValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id98_BooleanValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.BooleanValueType o;
            o = new global::Xbim.COBieLiteUK.BooleanValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations13 = 0;
            int readerCount13 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id99_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@UnitName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id100_BooleanValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@BooleanValueSpecified = true;
                            {
                                o.@BooleanValue = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations13, ref readerCount13);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AttributeMonetaryValueType Read75_AttributeMonetaryValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id101_AttributeMonetaryValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeMonetaryValueType o;
            o = new global::Xbim.COBieLiteUK.AttributeMonetaryValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id102_MonetaryValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@MonetaryValue = System.Xml.XmlConvert.ToDecimal(Reader.ReadElementString());
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id103_MonetaryUnit && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@MonetaryUnit = Read74_CurrencyUnitSimpleType(Reader.ReadElementString());
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.CurrencyUnitSimpleType Read74_CurrencyUnitSimpleType(string s) {
            switch (s) {
                case @"British Pounds": return global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@BritishPounds;
                case @"Dollars": return global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@Dollars;
                case @"Euros": return global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@Euros;
                case @"": return global::Xbim.COBieLiteUK.CurrencyUnitSimpleType.@Item;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLiteUK.CurrencyUnitSimpleType));
            }
        }

        global::Xbim.COBieLiteUK.AttributeStringValueType Read39_AttributeStringValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id104_AttributeStringValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeStringValueType o;
            o = new global::Xbim.COBieLiteUK.AttributeStringValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id99_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@UnitName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id105_StringValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@StringValue = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id106_AllowedValues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AllowedValues = Read38_AllowedValueCollectionType(false, true);
                        }
                        state = 3;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AllowedValueCollectionType Read38_AllowedValueCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id107_AllowedValueCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AllowedValueCollectionType o;
            o = new global::Xbim.COBieLiteUK.AllowedValueCollectionType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations16 = 0;
            int readerCount16 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id108_AttributeAllowedValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                a_0.Add(Reader.ReadElementString());
                            }
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations16, ref readerCount16);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AttributeDecimalValueType Read43_AttributeDecimalValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id109_AttributeDecimalValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeDecimalValueType o;
            o = new global::Xbim.COBieLiteUK.AttributeDecimalValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations17 = 0;
            int readerCount17 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id99_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@UnitName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id110_DecimalValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@DecimalValueSpecified = true;
                            {
                                o.@DecimalValue = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id111_MinValueDecimal && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@MinValueDecimalSpecified = true;
                            {
                                o.@MinValueDecimal = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id112_MaxValueDecimal && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@MaxValueDecimalSpecified = true;
                            {
                                o.@MaxValueDecimal = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                            }
                        }
                        state = 4;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations17, ref readerCount17);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AttributeIntegerValueType Read40_AttributeIntegerValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id113_AttributeIntegerValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AttributeIntegerValueType o;
            o = new global::Xbim.COBieLiteUK.AttributeIntegerValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations18 = 0;
            int readerCount18 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id99_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@UnitName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id114_IntegerValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IntegerValue = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id115_MinValueInteger && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@MinValueInteger = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id116_MaxValueInteger && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@MaxValueInteger = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                            }
                        }
                        state = 4;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations18, ref readerCount18);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ContactCollectionType Read68_ContactCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id117_ContactCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ContactCollectionType o;
            o = new global::Xbim.COBieLiteUK.ContactCollectionType();
            if ((object)(o.@Contact) == null) o.@Contact = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactType>)o.@Contact;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations19 = 0;
            int readerCount19 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id118_Contact && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read67_ContactType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations19, ref readerCount19);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ContactType Read67_ContactType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id119_ContactTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ContactType o;
            o = new global::Xbim.COBieLiteUK.ContactType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations20 = 0;
            int readerCount20 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id120_ContactEmail && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactEmail = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id121_ContactCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id122_ContactCompanyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactCompanyName = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id123_ContactPhoneNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactPhoneNumber = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id124_ContactDepartmentName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactDepartmentName = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id125_ContactGivenName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactGivenName = Reader.ReadElementString();
                            }
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id126_ContactFamilyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactFamilyName = Reader.ReadElementString();
                            }
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id127_ContactStreet && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactStreet = Reader.ReadElementString();
                            }
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id128_ContactPostalBoxNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactPostalBoxNumber = Reader.ReadElementString();
                            }
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id129_ContactTownName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactTownName = Reader.ReadElementString();
                            }
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id130_ContactRegionCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactRegionCode = Reader.ReadElementString();
                            }
                        }
                        state = 11;
                        break;
                    case 11:
                        if (((object) Reader.LocalName == (object)id131_ContactCountryName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactCountryName = Reader.ReadElementString();
                            }
                        }
                        state = 12;
                        break;
                    case 12:
                        if (((object) Reader.LocalName == (object)id132_ContactPostalCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactPostalCode = Reader.ReadElementString();
                            }
                        }
                        state = 13;
                        break;
                    case 13:
                        if (((object) Reader.LocalName == (object)id133_ContactURL && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ContactURL = Reader.ReadElementString();
                            }
                        }
                        state = 14;
                        break;
                    case 14:
                        if (((object) Reader.LocalName == (object)id134_ContactAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ContactAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 15;
                        break;
                    case 15:
                        if (((object) Reader.LocalName == (object)id135_ContactDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ContactDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 16;
                        break;
                    case 16:
                        if (((object) Reader.LocalName == (object)id136_ContactIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ContactIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 17;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations20, ref readerCount20);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ConnectionCollectionType Read70_ConnectionCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id137_ConnectionCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ConnectionCollectionType o;
            o = new global::Xbim.COBieLiteUK.ConnectionCollectionType();
            if ((object)(o.@Connection) == null) o.@Connection = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ConnectionType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ConnectionType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ConnectionType>)o.@Connection;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations21 = 0;
            int readerCount21 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id138_Connection && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read69_ConnectionType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations21, ref readerCount21);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ConnectionType Read69_ConnectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id139_ConnectionTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ConnectionType o;
            o = new global::Xbim.COBieLiteUK.ConnectionType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations22 = 0;
            int readerCount22 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id140_ConnectionName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id141_ConnectionCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id142_ConnectionAsset1Name && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionAsset1Name = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id143_ConnectionAsset1PortName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionAsset1PortName = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id144_ConnectionAsset2Name && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionAsset2Name = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id145_ConnectionAsset2PortName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionAsset2PortName = Reader.ReadElementString();
                            }
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id146_ConnectionDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ConnectionDescription = Reader.ReadElementString();
                            }
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id147_ConnectionAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ConnectionAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id148_ConnectionDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ConnectionDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id149_ConnectionIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ConnectionIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 10;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations22, ref readerCount22);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SystemCollectionType Read30_SystemCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id150_SystemCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SystemCollectionType o;
            o = new global::Xbim.COBieLiteUK.SystemCollectionType();
            if ((object)(o.@System) == null) o.@System = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemType>)o.@System;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations23 = 0;
            int readerCount23 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id151_System && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read29_SystemType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations23, ref readerCount23);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SystemType Read29_SystemType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id152_SystemTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SystemType o;
            o = new global::Xbim.COBieLiteUK.SystemType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations24 = 0;
            int readerCount24 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id153_SystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SystemName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id154_SystemCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SystemCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id155_SystemDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SystemDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id156_SystemAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SystemAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id157_SystemDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SystemDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id158_SystemIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SystemIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations24, ref readerCount24);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssetTypeCollectionType Read59_AssetTypeCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id159_AssetTypeCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AssetTypeCollectionType o;
            o = new global::Xbim.COBieLiteUK.AssetTypeCollectionType();
            if ((object)(o.@AssetType) == null) o.@AssetType = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetTypeInfoType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetTypeInfoType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetTypeInfoType>)o.@AssetType;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations25 = 0;
            int readerCount25 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id160_AssetType && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read58_AssetTypeInfoType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations25, ref readerCount25);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssetTypeInfoType Read58_AssetTypeInfoType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id161_AssetTypeInfoTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AssetTypeInfoType o;
            o = new global::Xbim.COBieLiteUK.AssetTypeInfoType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations26 = 0;
            int readerCount26 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id162_AssetTypeName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id163_AssetTypeCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id164_AssetTypeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id165_AssetTypeAccountingCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeAccountingCategorySpecified = true;
                            {
                                o.@AssetTypeAccountingCategory = Read36_AssetPortabilitySimpleType(Reader.ReadElementString());
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id166_AssetTypeModelNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeModelNumber = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id167_AssetTypeReplacementCostValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeReplacementCostValue = Read45_DecimalValueType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id168_AssetTypeExpectedLifeValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeExpectedLifeValue = Read41_IntegerValueType(false, true);
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id169_AssetTypeNominalLength && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeNominalLength = Read45_DecimalValueType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id170_AssetTypeNominalWidth && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeNominalWidth = Read45_DecimalValueType(false, true);
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id171_AssetTypeNominalHeight && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeNominalHeight = Read45_DecimalValueType(false, true);
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id172_AssetTypeAccessibilityText && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeAccessibilityText = Reader.ReadElementString();
                            }
                        }
                        state = 11;
                        break;
                    case 11:
                        if (((object) Reader.LocalName == (object)id173_AssetTypeCodePerformance && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeCodePerformance = Reader.ReadElementString();
                            }
                        }
                        state = 12;
                        break;
                    case 12:
                        if (((object) Reader.LocalName == (object)id174_AssetTypeColorCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeColorCode = Reader.ReadElementString();
                            }
                        }
                        state = 13;
                        break;
                    case 13:
                        if (((object) Reader.LocalName == (object)id175_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeConstituentsDescription = Reader.ReadElementString();
                            }
                        }
                        state = 14;
                        break;
                    case 14:
                        if (((object) Reader.LocalName == (object)id176_AssetTypeFeaturesDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeFeaturesDescription = Reader.ReadElementString();
                            }
                        }
                        state = 15;
                        break;
                    case 15:
                        if (((object) Reader.LocalName == (object)id177_AssetTypeFinishDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeFinishDescription = Reader.ReadElementString();
                            }
                        }
                        state = 16;
                        break;
                    case 16:
                        if (((object) Reader.LocalName == (object)id178_AssetTypeGradeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeGradeDescription = Reader.ReadElementString();
                            }
                        }
                        state = 17;
                        break;
                    case 17:
                        if (((object) Reader.LocalName == (object)id179_AssetTypeMaterialDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeMaterialDescription = Reader.ReadElementString();
                            }
                        }
                        state = 18;
                        break;
                    case 18:
                        if (((object) Reader.LocalName == (object)id180_AssetTypeShapeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeShapeDescription = Reader.ReadElementString();
                            }
                        }
                        state = 19;
                        break;
                    case 19:
                        if (((object) Reader.LocalName == (object)id181_AssetTypeSizeDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeSizeDescription = Reader.ReadElementString();
                            }
                        }
                        state = 20;
                        break;
                    case 20:
                        if (((object) Reader.LocalName == (object)id182_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTypeSustainabilityPerformanceDescription = Reader.ReadElementString();
                            }
                        }
                        state = 21;
                        break;
                    case 21:
                        if (((object) Reader.LocalName == (object)id183_Assets && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Assets = Read49_AssetCollectionType(false, true);
                        }
                        state = 22;
                        break;
                    case 22:
                        if (((object) Reader.LocalName == (object)id184_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeManufacturerContactAssignments = Read82_Item(false, true);
                        }
                        state = 23;
                        break;
                    case 23:
                        if (((object) Reader.LocalName == (object)id185_Warranties && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Warranties = Read51_WarrantyCollectionType(false, true);
                        }
                        state = 24;
                        break;
                    case 24:
                        if (((object) Reader.LocalName == (object)id186_Spares && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Spares = Read31_SpareCollectionType(false, true);
                        }
                        state = 25;
                        break;
                    case 25:
                        if (((object) Reader.LocalName == (object)id187_Jobs && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Jobs = Read57_JobCollectionType(false, true);
                        }
                        state = 26;
                        break;
                    case 26:
                        if (((object) Reader.LocalName == (object)id188_AssemblyAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssemblyAssignments = Read80_Item(false, true);
                        }
                        state = 27;
                        break;
                    case 27:
                        if (((object) Reader.LocalName == (object)id189_AssetTypeAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 28;
                        break;
                    case 28:
                        if (((object) Reader.LocalName == (object)id190_AssetTypeDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 29;
                        break;
                    case 29:
                        if (((object) Reader.LocalName == (object)id191_AssetTypeIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetTypeIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 30;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations26, ref readerCount26);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType Read80_Item(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id192_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType o;
            o = new global::Xbim.COBieLiteUK.AssemblyAssignmentCollectionType();
            if ((object)(o.@AssemblyAssignment) == null) o.@AssemblyAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssemblyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssemblyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssemblyType>)o.@AssemblyAssignment;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations27 = 0;
            int readerCount27 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id193_AssemblyAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read79_AssemblyType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations27, ref readerCount27);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssemblyType Read79_AssemblyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id194_AssemblyTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AssemblyType o;
            o = new global::Xbim.COBieLiteUK.AssemblyType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations28 = 0;
            int readerCount28 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id195_AssemblyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssemblyName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id196_AssemblyParentName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssemblyParentName = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id197_AssemblyCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssemblyCategory = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id198_AssemblyDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssemblyDescription = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id199_AssemblyAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssemblyAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id200_AssemblyDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssemblyDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id201_AssemblyIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssemblyIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 7;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations28, ref readerCount28);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.JobCollectionType Read57_JobCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id202_JobCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.JobCollectionType o;
            o = new global::Xbim.COBieLiteUK.JobCollectionType();
            if ((object)(o.@Job) == null) o.@Job = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.JobType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.JobType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.JobType>)o.@Job;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations29 = 0;
            int readerCount29 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id203_Job && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read56_JobType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations29, ref readerCount29);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.JobType Read56_JobType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id204_JobTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.JobType o;
            o = new global::Xbim.COBieLiteUK.JobType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations30 = 0;
            int readerCount30 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id205_JobName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id206_JobTaskID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobTaskID = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id207_JobCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobCategory = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id208_JobStatus && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobStatus = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id209_JobDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobDescription = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id210_JobDuration && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@JobDuration = Read41_IntegerValueType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id211_JobStartDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@JobStartDateSpecified = true;
                            {
                                o.@JobStartDate = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id212_JobStartConditionDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobStartConditionDescription = Reader.ReadElementString();
                            }
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id213_JobFrequencyValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@JobFrequencyValue = Read45_DecimalValueType(false, true);
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id214_JobPriorTaskID && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@JobPriorTaskID = Reader.ReadElementString();
                            }
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id215_Resources && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Resources = Read55_ResourceCollectionType(false, true);
                        }
                        state = 11;
                        break;
                    case 11:
                        if (((object) Reader.LocalName == (object)id216_JobAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@JobAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 12;
                        break;
                    case 12:
                        if (((object) Reader.LocalName == (object)id217_JobDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@JobDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 13;
                        break;
                    case 13:
                        if (((object) Reader.LocalName == (object)id218_JobIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@JobIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 14;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations30, ref readerCount30);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ResourceCollectionType Read55_ResourceCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id219_ResourceCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ResourceCollectionType o;
            o = new global::Xbim.COBieLiteUK.ResourceCollectionType();
            if ((object)(o.@Resource) == null) o.@Resource = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ResourceType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ResourceType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ResourceType>)o.@Resource;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations31 = 0;
            int readerCount31 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id220_Resource && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read54_ResourceType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations31, ref readerCount31);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ResourceType Read54_ResourceType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id221_ResourceTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ResourceType o;
            o = new global::Xbim.COBieLiteUK.ResourceType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations32 = 0;
            int readerCount32 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id222_ResourceName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ResourceName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id223_ResourceCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ResourceCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id224_ResourceDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ResourceDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id225_ResourceAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ResourceAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id226_ResourceDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ResourceDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id227_ResourceIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ResourceIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations32, ref readerCount32);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.DecimalValueType Read45_DecimalValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id228_DecimalValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id109_AttributeDecimalValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item))
                return Read43_AttributeDecimalValueType(isNullable, false);
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.DecimalValueType o;
            o = new global::Xbim.COBieLiteUK.DecimalValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations33 = 0;
            int readerCount33 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id99_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@UnitName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id110_DecimalValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@DecimalValueSpecified = true;
                            {
                                o.@DecimalValue = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations33, ref readerCount33);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.IntegerValueType Read41_IntegerValueType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id229_IntegerValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id113_AttributeIntegerValueType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item))
                return Read40_AttributeIntegerValueType(isNullable, false);
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.IntegerValueType o;
            o = new global::Xbim.COBieLiteUK.IntegerValueType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations34 = 0;
            int readerCount34 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id99_UnitName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@UnitName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id114_IntegerValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@IntegerValue = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations34, ref readerCount34);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SpareCollectionType Read31_SpareCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id230_SpareCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SpareCollectionType o;
            o = new global::Xbim.COBieLiteUK.SpareCollectionType();
            if ((object)(o.@Spare) == null) o.@Spare = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpareType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpareType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpareType>)o.@Spare;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations35 = 0;
            int readerCount35 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id231_Spare && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read83_SpareType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations35, ref readerCount35);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SpareType Read83_SpareType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id232_SpareTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SpareType o;
            o = new global::Xbim.COBieLiteUK.SpareType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations36 = 0;
            int readerCount36 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id233_SpareName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpareName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id234_SpareCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpareCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id235_SpareDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpareDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id236_SpareSetNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpareSetNumber = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id237_SparePartNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SparePartNumber = Reader.ReadElementString();
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id238_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpareSupplierContactAssignments = Read82_Item(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id239_SpareAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpareAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id240_SpareDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpareDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id241_SpareIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpareIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 9;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations36, ref readerCount36);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ContactAssignmentCollectionType Read82_Item(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id242_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ContactAssignmentCollectionType o;
            o = new global::Xbim.COBieLiteUK.ContactAssignmentCollectionType();
            if ((object)(o.@ContactAssignment) == null) o.@ContactAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ContactKeyType>)o.@ContactAssignment;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations37 = 0;
            int readerCount37 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id65_ContactAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read2_ContactKeyType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations37, ref readerCount37);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.WarrantyCollectionType Read51_WarrantyCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id243_WarrantyCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.WarrantyCollectionType o;
            o = new global::Xbim.COBieLiteUK.WarrantyCollectionType();
            if ((object)(o.@Warranty) == null) o.@Warranty = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.WarrantyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.WarrantyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.WarrantyType>)o.@Warranty;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations38 = 0;
            int readerCount38 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id244_Warranty && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read50_WarrantyType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations38, ref readerCount38);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.WarrantyType Read50_WarrantyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id245_WarrantyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.WarrantyType o;
            o = new global::Xbim.COBieLiteUK.WarrantyType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations39 = 0;
            int readerCount39 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id246_WarrantyName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@WarrantyName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id247_WarrantyCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@WarrantyCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id248_WarrantyDuration && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@WarrantyDuration = Read41_IntegerValueType(false, true);
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id249_Item && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@WarrantyGaurantorContactAssignments = Read82_Item(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id250_WarrantyAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@WarrantyAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id251_WarrantyDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@WarrantyDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id252_WarrantyIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@WarrantyIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 7;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations39, ref readerCount39);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssetCollectionType Read49_AssetCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id253_AssetCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AssetCollectionType o;
            o = new global::Xbim.COBieLiteUK.AssetCollectionType();
            if ((object)(o.@Asset) == null) o.@Asset = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetInfoType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetInfoType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.AssetInfoType>)o.@Asset;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations40 = 0;
            int readerCount40 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id254_Asset && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read48_AssetInfoType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations40, ref readerCount40);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssetInfoType Read48_AssetInfoType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id255_AssetInfoTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.AssetInfoType o;
            o = new global::Xbim.COBieLiteUK.AssetInfoType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations41 = 0;
            int readerCount41 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id256_AssetName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id257_AssetSpaceAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetSpaceAssignments = Read16_SpaceAssignmentCollectionType(false, true);
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id258_AssetDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id259_AssetSerialNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetSerialNumber = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id260_AssetInstallationDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetInstallationDateSpecified = true;
                            {
                                o.@AssetInstallationDate = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id261_AssetInstalledModelNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetInstalledModelNumber = Reader.ReadElementString();
                            }
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id262_AssetWarrantyStartDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetWarrantyStartDateSpecified = true;
                            {
                                o.@AssetWarrantyStartDate = ToDate(Reader.ReadElementString());
                            }
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id263_AssetStartDate && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetStartDate = Reader.ReadElementString();
                            }
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id264_AssetTagNumber && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetTagNumber = Reader.ReadElementString();
                            }
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id265_AssetBarCode && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetBarCode = Reader.ReadElementString();
                            }
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id266_AssetIdentifier && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetIdentifier = Reader.ReadElementString();
                            }
                        }
                        state = 11;
                        break;
                    case 11:
                        if (((object) Reader.LocalName == (object)id267_AssetLocationDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@AssetLocationDescription = Reader.ReadElementString();
                            }
                        }
                        state = 12;
                        break;
                    case 12:
                        if (((object) Reader.LocalName == (object)id268_AssetSystemAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetSystemAssignments = Read47_SystemAssignmentCollectionType(false, true);
                        }
                        state = 13;
                        break;
                    case 13:
                        if (((object) Reader.LocalName == (object)id188_AssemblyAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssemblyAssignments = Read80_Item(false, true);
                        }
                        state = 14;
                        break;
                    case 14:
                        if (((object) Reader.LocalName == (object)id269_AssetAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 15;
                        break;
                    case 15:
                        if (((object) Reader.LocalName == (object)id270_AssetDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 16;
                        break;
                    case 16:
                        if (((object) Reader.LocalName == (object)id271_AssetIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@AssetIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 17;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations41, ref readerCount41);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SystemAssignmentCollectionType Read47_SystemAssignmentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id272_SystemAssignmentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SystemAssignmentCollectionType o;
            o = new global::Xbim.COBieLiteUK.SystemAssignmentCollectionType();
            if ((object)(o.@SystemAssignment) == null) o.@SystemAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SystemKeyType>)o.@SystemAssignment;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations42 = 0;
            int readerCount42 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id273_SystemAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read9_SystemKeyType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations42, ref readerCount42);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SystemKeyType Read9_SystemKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id274_SystemKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SystemKeyType o;
            o = new global::Xbim.COBieLiteUK.SystemKeyType();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id275_externalIDReference && (object) Reader.NamespaceURI == (object)id5_Item)) {
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations43 = 0;
            int readerCount43 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id153_SystemName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SystemName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id154_SystemCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SystemCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations43, ref readerCount43);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SpaceAssignmentCollectionType Read16_SpaceAssignmentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id276_SpaceAssignmentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SpaceAssignmentCollectionType o;
            o = new global::Xbim.COBieLiteUK.SpaceAssignmentCollectionType();
            if ((object)(o.@SpaceAssignment) == null) o.@SpaceAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceKeyType>)o.@SpaceAssignment;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations44 = 0;
            int readerCount44 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id277_SpaceAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read12_SpaceKeyType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations44, ref readerCount44);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SpaceKeyType Read12_SpaceKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id278_SpaceKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SpaceKeyType o;
            o = new global::Xbim.COBieLiteUK.SpaceKeyType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations45 = 0;
            int readerCount45 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id279_FloorName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FloorName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id280_SpaceName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpaceName = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations45, ref readerCount45);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.AssetPortabilitySimpleType Read36_AssetPortabilitySimpleType(string s) {
            switch (s) {
                case @"Fixed": return global::Xbim.COBieLiteUK.AssetPortabilitySimpleType.@Fixed;
                case @"Moveable": return global::Xbim.COBieLiteUK.AssetPortabilitySimpleType.@Moveable;
                case @"": return global::Xbim.COBieLiteUK.AssetPortabilitySimpleType.@Item;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLiteUK.AssetPortabilitySimpleType));
            }
        }

        global::Xbim.COBieLiteUK.ZoneCollectionType Read34_ZoneCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id281_ZoneCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ZoneCollectionType o;
            o = new global::Xbim.COBieLiteUK.ZoneCollectionType();
            if ((object)(o.@Zone) == null) o.@Zone = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneType>)o.@Zone;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations46 = 0;
            int readerCount46 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id282_Zone && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read33_ZoneType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations46, ref readerCount46);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ZoneType Read33_ZoneType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id283_ZoneTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ZoneType o;
            o = new global::Xbim.COBieLiteUK.ZoneType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations47 = 0;
            int readerCount47 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id284_ZoneName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ZoneName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id285_ZoneCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ZoneCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id286_ZoneDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ZoneDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id287_ZoneAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ZoneAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id288_ZoneDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ZoneDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id289_ZoneIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@ZoneIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations47, ref readerCount47);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.FloorCollectionType Read65_FloorCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id290_FloorCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.FloorCollectionType o;
            o = new global::Xbim.COBieLiteUK.FloorCollectionType();
            if ((object)(o.@Floor) == null) o.@Floor = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.FloorType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.FloorType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.FloorType>)o.@Floor;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations48 = 0;
            int readerCount48 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id291_Floor && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read64_FloorType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations48, ref readerCount48);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.FloorType Read64_FloorType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id292_FloorTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.FloorType o;
            o = new global::Xbim.COBieLiteUK.FloorType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations49 = 0;
            int readerCount49 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id279_FloorName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FloorName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id293_FloorCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FloorCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id294_FloorDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@FloorDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id295_FloorElevationValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FloorElevationValue = Read45_DecimalValueType(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id296_FloorHeightValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FloorHeightValue = Read45_DecimalValueType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id297_Spaces && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@Spaces = Read62_SpaceCollectionType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id298_FloorAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FloorAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id299_FloorDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FloorDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id300_FloorIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@FloorIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 9;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations49, ref readerCount49);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SpaceCollectionType Read62_SpaceCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id301_SpaceCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SpaceCollectionType o;
            o = new global::Xbim.COBieLiteUK.SpaceCollectionType();
            if ((object)(o.@Space) == null) o.@Space = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.SpaceType>)o.@Space;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations50 = 0;
            int readerCount50 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id302_Space && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read61_SpaceType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations50, ref readerCount50);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.SpaceType Read61_SpaceType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id303_SpaceTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SpaceType o;
            o = new global::Xbim.COBieLiteUK.SpaceType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations51 = 0;
            int readerCount51 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id280_SpaceName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpaceName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id304_SpaceCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpaceCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id305_SpaceDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpaceDescription = Reader.ReadElementString();
                            }
                        }
                        state = 3;
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id306_SpaceSignageName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SpaceSignageName = Reader.ReadElementString();
                            }
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id307_SpaceUsableHeightValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceUsableHeightValue = Read45_DecimalValueType(false, true);
                        }
                        state = 5;
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id308_SpaceGrossAreaValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceGrossAreaValue = Read45_DecimalValueType(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id309_SpaceNetAreaValue && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceNetAreaValue = Read45_DecimalValueType(false, true);
                        }
                        state = 7;
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id310_SpaceZoneAssignments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceZoneAssignments = Read20_ZoneAssignmentCollectionType(false, true);
                        }
                        state = 8;
                        break;
                    case 8:
                        if (((object) Reader.LocalName == (object)id311_SpaceAttributes && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceAttributes = Read78_AttributeCollectionType(false, true);
                        }
                        state = 9;
                        break;
                    case 9:
                        if (((object) Reader.LocalName == (object)id312_SpaceDocuments && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceDocuments = Read28_DocumentCollectionType(false, true);
                        }
                        state = 10;
                        break;
                    case 10:
                        if (((object) Reader.LocalName == (object)id313_SpaceIssues && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            o.@SpaceIssues = Read26_IssueCollectionType(false, true);
                        }
                        state = 11;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations51, ref readerCount51);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ZoneAssignmentCollectionType Read20_ZoneAssignmentCollectionType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id314_ZoneAssignmentCollectionType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ZoneAssignmentCollectionType o;
            o = new global::Xbim.COBieLiteUK.ZoneAssignmentCollectionType();
            if ((object)(o.@ZoneAssignment) == null) o.@ZoneAssignment = new global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneKeyType>();
            global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneKeyType> a_0 = (global::System.Collections.Generic.List<global::Xbim.COBieLiteUK.ZoneKeyType>)o.@ZoneAssignment;
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations52 = 0;
            int readerCount52 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id315_ZoneAssignment && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            if ((object)(a_0) == null) Reader.Skip(); else a_0.Add(Read19_ZoneKeyType(false, true));
                        }
                        else {
                            state = 1;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations52, ref readerCount52);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ZoneKeyType Read19_ZoneKeyType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id316_ZoneKeyType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ZoneKeyType o;
            o = new global::Xbim.COBieLiteUK.ZoneKeyType();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id275_externalIDReference && (object) Reader.NamespaceURI == (object)id5_Item)) {
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations53 = 0;
            int readerCount53 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id284_ZoneName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ZoneName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id285_ZoneCategory && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ZoneCategory = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations53, ref readerCount53);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.VolumeUnitSimpleType Read88_VolumeUnitSimpleType(string s) {
            switch (s) {
                case @"cubic centimeters": return global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubiccentimeters;
                case @"cubic feet": return global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicfeet;
                case @"cubic inches": return global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicinches;
                case @"cubic meters": return global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicmeters;
                case @"cubic millimeters": return global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicmillimeters;
                case @"cubic yards": return global::Xbim.COBieLiteUK.VolumeUnitSimpleType.@cubicyards;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLiteUK.VolumeUnitSimpleType));
            }
        }

        global::Xbim.COBieLiteUK.AreaUnitSimpleType Read87_AreaUnitSimpleType(string s) {
            switch (s) {
                case @"square centimeters": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squarecentimeters;
                case @"square feet": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squarefeet;
                case @"square inches": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squareinches;
                case @"square kilometers": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squarekilometers;
                case @"square meters": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squaremeters;
                case @"square miles": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squaremiles;
                case @"square millimeters": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squaremillimeters;
                case @"square yards": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@squareyards;
                case @"": return global::Xbim.COBieLiteUK.AreaUnitSimpleType.@Item;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLiteUK.AreaUnitSimpleType));
            }
        }

        global::Xbim.COBieLiteUK.LinearUnitSimpleType Read86_LinearUnitSimpleType(string s) {
            switch (s) {
                case @"centimeters": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@centimeters;
                case @"feet": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@feet;
                case @"inches": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@inches;
                case @"kilometers": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@kilometers;
                case @"meters": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@meters;
                case @"miles": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@miles;
                case @"millimeters": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@millimeters;
                case @"yards": return global::Xbim.COBieLiteUK.LinearUnitSimpleType.@yards;
                default: throw CreateUnknownConstantException(s, typeof(global::Xbim.COBieLiteUK.LinearUnitSimpleType));
            }
        }

        global::Xbim.COBieLiteUK.SiteType Read85_SiteType(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id317_SiteType && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.SiteType o;
            o = new global::Xbim.COBieLiteUK.SiteType();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations54 = 0;
            int readerCount54 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id318_SiteName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SiteName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id319_SiteDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@SiteDescription = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations54, ref readerCount54);
            }
            ReadEndElement();
            return o;
        }

        global::Xbim.COBieLiteUK.ProjectTypeBase Read95_ProjectTypeBase(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_ProjectTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id5_Item)) {
            }
            else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_ProjectTypeBase && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                return Read98_ProjectType(isNullable, false);
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Xbim.COBieLiteUK.ProjectTypeBase o;
            o = new global::Xbim.COBieLiteUK.ProjectTypeBase();
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
            int state = 0;
            Reader.MoveToContent();
            int whileIterations55 = 0;
            int readerCount55 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id9_ProjectName && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ProjectName = Reader.ReadElementString();
                            }
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id10_ProjectDescription && (object) Reader.NamespaceURI == (object)id5_Item)) {
                            {
                                o.@ProjectDescription = Reader.ReadElementString();
                            }
                        }
                        state = 2;
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations55, ref readerCount55);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks() {
        }

        string id279_FloorName;
        string id100_BooleanValue;
        string id208_JobStatus;
        string id129_ContactTownName;
        string id68_ContactEmailReference;
        string id318_SiteName;
        string id131_ContactCountryName;
        string id294_FloorDescription;
        string id269_AssetAttributes;
        string id1_Project;
        string id119_ContactTypeBase;
        string id52_Contacts;
        string id9_ProjectName;
        string id148_ConnectionDocuments;
        string id304_SpaceCategory;
        string id38_ProjectAssignment;
        string id48_Zones;
        string id253_AssetCollectionType;
        string id55_FacilityIssues;
        string id243_WarrantyCollectionType;
        string id195_AssemblyName;
        string id102_MonetaryValue;
        string id81_AttributeTypeBase;
        string id298_FloorAttributes;
        string id18_ProjectConstructionEnd;
        string id128_ContactPostalBoxNumber;
        string id57_Issue;
        string id299_FloorDocuments;
        string id153_SystemName;
        string id163_AssetTypeCategory;
        string id151_System;
        string id295_FloorElevationValue;
        string id259_AssetSerialNumber;
        string id117_ContactCollectionType;
        string id241_SpareIssues;
        string id127_ContactStreet;
        string id240_SpareDocuments;
        string id37_FacilityCategory;
        string id31_ProjectStagePrevious;
        string id140_ConnectionName;
        string id42_FacilityDefaultVolumeUnit;
        string id138_Connection;
        string id224_ResourceDescription;
        string id287_ZoneAttributes;
        string id181_AssetTypeSizeDescription;
        string id113_AttributeIntegerValueType;
        string id80_Attribute;
        string id227_ResourceIssues;
        string id132_ContactPostalCode;
        string id307_SpaceUsableHeightValue;
        string id214_JobPriorTaskID;
        string id99_UnitName;
        string id95_AttributeTimeValue;
        string id43_FacilityDefaultCurrencyUnit;
        string id77_DocumentAttributes;
        string id211_JobStartDate;
        string id115_MinValueInteger;
        string id203_Job;
        string id3_ProjectTypeBase;
        string id49_AssetTypes;
        string id178_AssetTypeGradeDescription;
        string id281_ZoneCollectionType;
        string id311_SpaceAttributes;
        string id174_AssetTypeColorCode;
        string id12_ProjectFunction;
        string id219_ResourceCollectionType;
        string id7_externalSystemName;
        string id109_AttributeDecimalValueType;
        string id67_ContactKeyType;
        string id276_SpaceAssignmentCollectionType;
        string id104_AttributeStringValueType;
        string id167_AssetTypeReplacementCostValue;
        string id217_JobDocuments;
        string id47_Floors;
        string id124_ContactDepartmentName;
        string id177_AssetTypeFinishDescription;
        string id149_ConnectionIssues;
        string id168_AssetTypeExpectedLifeValue;
        string id5_Item;
        string id154_SystemCategory;
        string id274_SystemKeyType;
        string id136_ContactIssues;
        string id293_FloorCategory;
        string id242_Item;
        string id107_AllowedValueCollectionType;
        string id231_Spare;
        string id312_SpaceDocuments;
        string id260_AssetInstallationDate;
        string id170_AssetTypeNominalWidth;
        string id46_FacilityDeliverablePhaseName;
        string id284_ZoneName;
        string id24_ProjectStageCode;
        string id300_FloorIssues;
        string id202_JobCollectionType;
        string id160_AssetType;
        string id157_SystemDocuments;
        string id244_Warranty;
        string id278_SpaceKeyType;
        string id50_Systems;
        string id209_JobDescription;
        string id29_ProjectStageAttributes;
        string id197_AssemblyCategory;
        string id92_AttributeDecimalValue;
        string id289_ZoneIssues;
        string id247_WarrantyCategory;
        string id114_IntegerValue;
        string id4_externalEntityName;
        string id64_IssueDescription;
        string id257_AssetSpaceAssignments;
        string id74_DocumentDescription;
        string id268_AssetSystemAssignments;
        string id283_ZoneTypeBase;
        string id248_WarrantyDuration;
        string id166_AssetTypeModelNumber;
        string id212_JobStartConditionDescription;
        string id277_SpaceAssignment;
        string id23_ProjectStageDescription;
        string id310_SpaceZoneAssignments;
        string id61_IssueRiskText;
        string id246_WarrantyName;
        string id90_AttributeIntegerValue;
        string id86_AttributeDescription;
        string id45_FacilityDescription;
        string id112_MaxValueDecimal;
        string id62_IssueSeverityText;
        string id137_ConnectionCollectionType;
        string id205_JobName;
        string id51_Connections;
        string id280_SpaceName;
        string id36_FacilityName;
        string id236_SpareSetNumber;
        string id6_externalID;
        string id266_AssetIdentifier;
        string id161_AssetTypeInfoTypeBase;
        string id192_Item;
        string id169_AssetTypeNominalLength;
        string id309_SpaceNetAreaValue;
        string id179_AssetTypeMaterialDescription;
        string id171_AssetTypeNominalHeight;
        string id204_JobTypeBase;
        string id264_AssetTagNumber;
        string id87_AttributeValue;
        string id186_Spares;
        string id183_Assets;
        string id175_Item;
        string id27_ProjectStageCost;
        string id182_Item;
        string id16_ProjectCost;
        string id165_AssetTypeAccountingCategory;
        string id162_AssetTypeName;
        string id126_ContactFamilyName;
        string id44_Item;
        string id313_SpaceIssues;
        string id110_DecimalValue;
        string id91_AttributeDateValue;
        string id238_Item;
        string id147_ConnectionAttributes;
        string id305_SpaceDescription;
        string id88_AttributeIssues;
        string id66_IssueMitigationDescription;
        string id282_Zone;
        string id198_AssemblyDescription;
        string id56_IssueCollectionType;
        string id122_ContactCompanyName;
        string id98_BooleanValueType;
        string id53_FacilityAttributes;
        string id134_ContactAttributes;
        string id21_ProjectStageType;
        string id30_Facility;
        string id116_MaxValueInteger;
        string id301_SpaceCollectionType;
        string id118_Contact;
        string id265_AssetBarCode;
        string id83_propertySetName;
        string id189_AssetTypeAttributes;
        string id256_AssetName;
        string id188_AssemblyAssignments;
        string id28_Item;
        string id150_SystemCollectionType;
        string id314_ZoneAssignmentCollectionType;
        string id235_SpareDescription;
        string id75_DocumentURI;
        string id207_JobCategory;
        string id292_FloorTypeBase;
        string id286_ZoneDescription;
        string id210_JobDuration;
        string id89_AttributeValueType;
        string id252_WarrantyIssues;
        string id290_FloorCollectionType;
        string id285_ZoneCategory;
        string id201_AssemblyIssues;
        string id221_ResourceTypeBase;
        string id85_AttributeCategory;
        string id200_AssemblyDocuments;
        string id19_ProjectStages;
        string id185_Warranties;
        string id8_Item;
        string id84_AttributeName;
        string id155_SystemDescription;
        string id71_DocumentTypeBase;
        string id319_SiteDescription;
        string id143_ConnectionAsset1PortName;
        string id105_StringValue;
        string id33_ProjectStageKeyType;
        string id245_WarrantyType;
        string id234_SpareCategory;
        string id73_DocumentCategory;
        string id315_ZoneAssignment;
        string id164_AssetTypeDescription;
        string id273_SystemAssignment;
        string id34_externalIDRefer;
        string id190_AssetTypeDocuments;
        string id249_Item;
        string id141_ConnectionCategory;
        string id63_IssueImpactText;
        string id25_ProjectStageStartDate;
        string id32_ProjectStageNext;
        string id239_SpareAttributes;
        string id22_ProjectStageName;
        string id296_FloorHeightValue;
        string id206_JobTaskID;
        string id158_SystemIssues;
        string id187_Jobs;
        string id270_AssetDocuments;
        string id159_AssetTypeCollectionType;
        string id2_Item;
        string id180_AssetTypeShapeDescription;
        string id254_Asset;
        string id173_AssetTypeCodePerformance;
        string id101_AttributeMonetaryValueType;
        string id133_ContactURL;
        string id103_MonetaryUnit;
        string id272_SystemAssignmentCollectionType;
        string id93_AttributeDateTimeValue;
        string id225_ResourceAttributes;
        string id108_AttributeAllowedValue;
        string id156_SystemAttributes;
        string id196_AssemblyParentName;
        string id194_AssemblyTypeBase;
        string id11_ProjectAddress;
        string id191_AssetTypeIssues;
        string id275_externalIDReference;
        string id263_AssetStartDate;
        string id96_AttributeMonetaryValue;
        string id35_FacilityTypeBase;
        string id308_SpaceGrossAreaValue;
        string id152_SystemTypeBase;
        string id65_ContactAssignment;
        string id176_AssetTypeFeaturesDescription;
        string id288_ZoneDocuments;
        string id291_Floor;
        string id13_ProjectProcurement;
        string id317_SiteType;
        string id15_ProjectExpectedLifeCycle;
        string id184_Item;
        string id228_DecimalValueType;
        string id258_AssetDescription;
        string id251_WarrantyDocuments;
        string id130_ContactRegionCode;
        string id120_ContactEmail;
        string id72_DocumentName;
        string id142_ConnectionAsset1Name;
        string id261_AssetInstalledModelNumber;
        string id20_ProjectStage;
        string id123_ContactPhoneNumber;
        string id58_IssueTypeBase;
        string id262_AssetWarrantyStartDate;
        string id193_AssemblyAssignment;
        string id106_AllowedValues;
        string id255_AssetInfoTypeBase;
        string id10_ProjectDescription;
        string id14_ProjectFunctionalUnit;
        string id79_AttributeCollectionType;
        string id94_AttributeStringValue;
        string id306_SpaceSignageName;
        string id78_DocumentIssues;
        string id59_IssueName;
        string id69_DocumentCollectionType;
        string id17_ProjectConstructionStart;
        string id39_SiteAssignment;
        string id232_SpareTypeBase;
        string id316_ZoneKeyType;
        string id60_IssueCategory;
        string id222_ResourceName;
        string id297_Spaces;
        string id230_SpareCollectionType;
        string id250_WarrantyAttributes;
        string id220_Resource;
        string id41_FacilityDefaultAreaUnit;
        string id271_AssetIssues;
        string id135_ContactDocuments;
        string id82_propertySetExternalIdentifier;
        string id26_ProjectStageEndDate;
        string id54_FacilityDocuments;
        string id215_Resources;
        string id125_ContactGivenName;
        string id229_IntegerValueType;
        string id111_MinValueDecimal;
        string id139_ConnectionTypeBase;
        string id233_SpareName;
        string id199_AssemblyAttributes;
        string id226_ResourceDocuments;
        string id76_DocumentReferenceURI;
        string id70_Document;
        string id146_ConnectionDescription;
        string id216_JobAttributes;
        string id302_Space;
        string id213_JobFrequencyValue;
        string id237_SparePartNumber;
        string id97_AttributeBooleanValue;
        string id267_AssetLocationDescription;
        string id40_FacilityDefaultLinearUnit;
        string id303_SpaceTypeBase;
        string id223_ResourceCategory;
        string id218_JobIssues;
        string id121_ContactCategory;
        string id172_AssetTypeAccessibilityText;
        string id145_ConnectionAsset2PortName;
        string id144_ConnectionAsset2Name;

        protected override void InitIDs() {
            id279_FloorName = Reader.NameTable.Add(@"FloorName");
            id100_BooleanValue = Reader.NameTable.Add(@"BooleanValue");
            id208_JobStatus = Reader.NameTable.Add(@"JobStatus");
            id129_ContactTownName = Reader.NameTable.Add(@"ContactTownName");
            id68_ContactEmailReference = Reader.NameTable.Add(@"ContactEmailReference");
            id318_SiteName = Reader.NameTable.Add(@"SiteName");
            id131_ContactCountryName = Reader.NameTable.Add(@"ContactCountryName");
            id294_FloorDescription = Reader.NameTable.Add(@"FloorDescription");
            id269_AssetAttributes = Reader.NameTable.Add(@"AssetAttributes");
            id1_Project = Reader.NameTable.Add(@"Project");
            id119_ContactTypeBase = Reader.NameTable.Add(@"ContactTypeBase");
            id52_Contacts = Reader.NameTable.Add(@"Contacts");
            id9_ProjectName = Reader.NameTable.Add(@"ProjectName");
            id148_ConnectionDocuments = Reader.NameTable.Add(@"ConnectionDocuments");
            id304_SpaceCategory = Reader.NameTable.Add(@"SpaceCategory");
            id38_ProjectAssignment = Reader.NameTable.Add(@"ProjectAssignment");
            id48_Zones = Reader.NameTable.Add(@"Zones");
            id253_AssetCollectionType = Reader.NameTable.Add(@"AssetCollectionType");
            id55_FacilityIssues = Reader.NameTable.Add(@"FacilityIssues");
            id243_WarrantyCollectionType = Reader.NameTable.Add(@"WarrantyCollectionType");
            id195_AssemblyName = Reader.NameTable.Add(@"AssemblyName");
            id102_MonetaryValue = Reader.NameTable.Add(@"MonetaryValue");
            id81_AttributeTypeBase = Reader.NameTable.Add(@"AttributeTypeBase");
            id298_FloorAttributes = Reader.NameTable.Add(@"FloorAttributes");
            id18_ProjectConstructionEnd = Reader.NameTable.Add(@"ProjectConstructionEnd");
            id128_ContactPostalBoxNumber = Reader.NameTable.Add(@"ContactPostalBoxNumber");
            id57_Issue = Reader.NameTable.Add(@"Issue");
            id299_FloorDocuments = Reader.NameTable.Add(@"FloorDocuments");
            id153_SystemName = Reader.NameTable.Add(@"SystemName");
            id163_AssetTypeCategory = Reader.NameTable.Add(@"AssetTypeCategory");
            id151_System = Reader.NameTable.Add(@"System");
            id295_FloorElevationValue = Reader.NameTable.Add(@"FloorElevationValue");
            id259_AssetSerialNumber = Reader.NameTable.Add(@"AssetSerialNumber");
            id117_ContactCollectionType = Reader.NameTable.Add(@"ContactCollectionType");
            id241_SpareIssues = Reader.NameTable.Add(@"SpareIssues");
            id127_ContactStreet = Reader.NameTable.Add(@"ContactStreet");
            id240_SpareDocuments = Reader.NameTable.Add(@"SpareDocuments");
            id37_FacilityCategory = Reader.NameTable.Add(@"FacilityCategory");
            id31_ProjectStagePrevious = Reader.NameTable.Add(@"ProjectStagePrevious");
            id140_ConnectionName = Reader.NameTable.Add(@"ConnectionName");
            id42_FacilityDefaultVolumeUnit = Reader.NameTable.Add(@"FacilityDefaultVolumeUnit");
            id138_Connection = Reader.NameTable.Add(@"Connection");
            id224_ResourceDescription = Reader.NameTable.Add(@"ResourceDescription");
            id287_ZoneAttributes = Reader.NameTable.Add(@"ZoneAttributes");
            id181_AssetTypeSizeDescription = Reader.NameTable.Add(@"AssetTypeSizeDescription");
            id113_AttributeIntegerValueType = Reader.NameTable.Add(@"AttributeIntegerValueType");
            id80_Attribute = Reader.NameTable.Add(@"Attribute");
            id227_ResourceIssues = Reader.NameTable.Add(@"ResourceIssues");
            id132_ContactPostalCode = Reader.NameTable.Add(@"ContactPostalCode");
            id307_SpaceUsableHeightValue = Reader.NameTable.Add(@"SpaceUsableHeightValue");
            id214_JobPriorTaskID = Reader.NameTable.Add(@"JobPriorTaskID");
            id99_UnitName = Reader.NameTable.Add(@"UnitName");
            id95_AttributeTimeValue = Reader.NameTable.Add(@"AttributeTimeValue");
            id43_FacilityDefaultCurrencyUnit = Reader.NameTable.Add(@"FacilityDefaultCurrencyUnit");
            id77_DocumentAttributes = Reader.NameTable.Add(@"DocumentAttributes");
            id211_JobStartDate = Reader.NameTable.Add(@"JobStartDate");
            id115_MinValueInteger = Reader.NameTable.Add(@"MinValueInteger");
            id203_Job = Reader.NameTable.Add(@"Job");
            id3_ProjectTypeBase = Reader.NameTable.Add(@"ProjectTypeBase");
            id49_AssetTypes = Reader.NameTable.Add(@"AssetTypes");
            id178_AssetTypeGradeDescription = Reader.NameTable.Add(@"AssetTypeGradeDescription");
            id281_ZoneCollectionType = Reader.NameTable.Add(@"ZoneCollectionType");
            id311_SpaceAttributes = Reader.NameTable.Add(@"SpaceAttributes");
            id174_AssetTypeColorCode = Reader.NameTable.Add(@"AssetTypeColorCode");
            id12_ProjectFunction = Reader.NameTable.Add(@"ProjectFunction");
            id219_ResourceCollectionType = Reader.NameTable.Add(@"ResourceCollectionType");
            id7_externalSystemName = Reader.NameTable.Add(@"externalSystemName");
            id109_AttributeDecimalValueType = Reader.NameTable.Add(@"AttributeDecimalValueType");
            id67_ContactKeyType = Reader.NameTable.Add(@"ContactKeyType");
            id276_SpaceAssignmentCollectionType = Reader.NameTable.Add(@"SpaceAssignmentCollectionType");
            id104_AttributeStringValueType = Reader.NameTable.Add(@"AttributeStringValueType");
            id167_AssetTypeReplacementCostValue = Reader.NameTable.Add(@"AssetTypeReplacementCostValue");
            id217_JobDocuments = Reader.NameTable.Add(@"JobDocuments");
            id47_Floors = Reader.NameTable.Add(@"Floors");
            id124_ContactDepartmentName = Reader.NameTable.Add(@"ContactDepartmentName");
            id177_AssetTypeFinishDescription = Reader.NameTable.Add(@"AssetTypeFinishDescription");
            id149_ConnectionIssues = Reader.NameTable.Add(@"ConnectionIssues");
            id168_AssetTypeExpectedLifeValue = Reader.NameTable.Add(@"AssetTypeExpectedLifeValue");
            id5_Item = Reader.NameTable.Add(@"http://docs.buildingsmartalliance.org/nbims03/cobie/core");
            id154_SystemCategory = Reader.NameTable.Add(@"SystemCategory");
            id274_SystemKeyType = Reader.NameTable.Add(@"SystemKeyType");
            id136_ContactIssues = Reader.NameTable.Add(@"ContactIssues");
            id293_FloorCategory = Reader.NameTable.Add(@"FloorCategory");
            id242_Item = Reader.NameTable.Add(@"ContactAssignmentCollectionType");
            id107_AllowedValueCollectionType = Reader.NameTable.Add(@"AllowedValueCollectionType");
            id231_Spare = Reader.NameTable.Add(@"Spare");
            id312_SpaceDocuments = Reader.NameTable.Add(@"SpaceDocuments");
            id260_AssetInstallationDate = Reader.NameTable.Add(@"AssetInstallationDate");
            id170_AssetTypeNominalWidth = Reader.NameTable.Add(@"AssetTypeNominalWidth");
            id46_FacilityDeliverablePhaseName = Reader.NameTable.Add(@"FacilityDeliverablePhaseName");
            id284_ZoneName = Reader.NameTable.Add(@"ZoneName");
            id24_ProjectStageCode = Reader.NameTable.Add(@"ProjectStageCode");
            id300_FloorIssues = Reader.NameTable.Add(@"FloorIssues");
            id202_JobCollectionType = Reader.NameTable.Add(@"JobCollectionType");
            id160_AssetType = Reader.NameTable.Add(@"AssetType");
            id157_SystemDocuments = Reader.NameTable.Add(@"SystemDocuments");
            id244_Warranty = Reader.NameTable.Add(@"Warranty");
            id278_SpaceKeyType = Reader.NameTable.Add(@"SpaceKeyType");
            id50_Systems = Reader.NameTable.Add(@"Systems");
            id209_JobDescription = Reader.NameTable.Add(@"JobDescription");
            id29_ProjectStageAttributes = Reader.NameTable.Add(@"ProjectStageAttributes");
            id197_AssemblyCategory = Reader.NameTable.Add(@"AssemblyCategory");
            id92_AttributeDecimalValue = Reader.NameTable.Add(@"AttributeDecimalValue");
            id289_ZoneIssues = Reader.NameTable.Add(@"ZoneIssues");
            id247_WarrantyCategory = Reader.NameTable.Add(@"WarrantyCategory");
            id114_IntegerValue = Reader.NameTable.Add(@"IntegerValue");
            id4_externalEntityName = Reader.NameTable.Add(@"externalEntityName");
            id64_IssueDescription = Reader.NameTable.Add(@"IssueDescription");
            id257_AssetSpaceAssignments = Reader.NameTable.Add(@"AssetSpaceAssignments");
            id74_DocumentDescription = Reader.NameTable.Add(@"DocumentDescription");
            id268_AssetSystemAssignments = Reader.NameTable.Add(@"AssetSystemAssignments");
            id283_ZoneTypeBase = Reader.NameTable.Add(@"ZoneTypeBase");
            id248_WarrantyDuration = Reader.NameTable.Add(@"WarrantyDuration");
            id166_AssetTypeModelNumber = Reader.NameTable.Add(@"AssetTypeModelNumber");
            id212_JobStartConditionDescription = Reader.NameTable.Add(@"JobStartConditionDescription");
            id277_SpaceAssignment = Reader.NameTable.Add(@"SpaceAssignment");
            id23_ProjectStageDescription = Reader.NameTable.Add(@"ProjectStageDescription");
            id310_SpaceZoneAssignments = Reader.NameTable.Add(@"SpaceZoneAssignments");
            id61_IssueRiskText = Reader.NameTable.Add(@"IssueRiskText");
            id246_WarrantyName = Reader.NameTable.Add(@"WarrantyName");
            id90_AttributeIntegerValue = Reader.NameTable.Add(@"AttributeIntegerValue");
            id86_AttributeDescription = Reader.NameTable.Add(@"AttributeDescription");
            id45_FacilityDescription = Reader.NameTable.Add(@"FacilityDescription");
            id112_MaxValueDecimal = Reader.NameTable.Add(@"MaxValueDecimal");
            id62_IssueSeverityText = Reader.NameTable.Add(@"IssueSeverityText");
            id137_ConnectionCollectionType = Reader.NameTable.Add(@"ConnectionCollectionType");
            id205_JobName = Reader.NameTable.Add(@"JobName");
            id51_Connections = Reader.NameTable.Add(@"Connections");
            id280_SpaceName = Reader.NameTable.Add(@"SpaceName");
            id36_FacilityName = Reader.NameTable.Add(@"FacilityName");
            id236_SpareSetNumber = Reader.NameTable.Add(@"SpareSetNumber");
            id6_externalID = Reader.NameTable.Add(@"externalID");
            id266_AssetIdentifier = Reader.NameTable.Add(@"AssetIdentifier");
            id161_AssetTypeInfoTypeBase = Reader.NameTable.Add(@"AssetTypeInfoTypeBase");
            id192_Item = Reader.NameTable.Add(@"AssemblyAssignmentCollectionType");
            id169_AssetTypeNominalLength = Reader.NameTable.Add(@"AssetTypeNominalLength");
            id309_SpaceNetAreaValue = Reader.NameTable.Add(@"SpaceNetAreaValue");
            id179_AssetTypeMaterialDescription = Reader.NameTable.Add(@"AssetTypeMaterialDescription");
            id171_AssetTypeNominalHeight = Reader.NameTable.Add(@"AssetTypeNominalHeight");
            id204_JobTypeBase = Reader.NameTable.Add(@"JobTypeBase");
            id264_AssetTagNumber = Reader.NameTable.Add(@"AssetTagNumber");
            id87_AttributeValue = Reader.NameTable.Add(@"AttributeValue");
            id186_Spares = Reader.NameTable.Add(@"Spares");
            id183_Assets = Reader.NameTable.Add(@"Assets");
            id175_Item = Reader.NameTable.Add(@"AssetTypeConstituentsDescription");
            id27_ProjectStageCost = Reader.NameTable.Add(@"ProjectStageCost");
            id182_Item = Reader.NameTable.Add(@"AssetTypeSustainabilityPerformanceDescription");
            id16_ProjectCost = Reader.NameTable.Add(@"ProjectCost");
            id165_AssetTypeAccountingCategory = Reader.NameTable.Add(@"AssetTypeAccountingCategory");
            id162_AssetTypeName = Reader.NameTable.Add(@"AssetTypeName");
            id126_ContactFamilyName = Reader.NameTable.Add(@"ContactFamilyName");
            id44_Item = Reader.NameTable.Add(@"FacilityDefaultMeasurementStandard");
            id313_SpaceIssues = Reader.NameTable.Add(@"SpaceIssues");
            id110_DecimalValue = Reader.NameTable.Add(@"DecimalValue");
            id91_AttributeDateValue = Reader.NameTable.Add(@"AttributeDateValue");
            id238_Item = Reader.NameTable.Add(@"SpareSupplierContactAssignments");
            id147_ConnectionAttributes = Reader.NameTable.Add(@"ConnectionAttributes");
            id305_SpaceDescription = Reader.NameTable.Add(@"SpaceDescription");
            id88_AttributeIssues = Reader.NameTable.Add(@"AttributeIssues");
            id66_IssueMitigationDescription = Reader.NameTable.Add(@"IssueMitigationDescription");
            id282_Zone = Reader.NameTable.Add(@"Zone");
            id198_AssemblyDescription = Reader.NameTable.Add(@"AssemblyDescription");
            id56_IssueCollectionType = Reader.NameTable.Add(@"IssueCollectionType");
            id122_ContactCompanyName = Reader.NameTable.Add(@"ContactCompanyName");
            id98_BooleanValueType = Reader.NameTable.Add(@"BooleanValueType");
            id53_FacilityAttributes = Reader.NameTable.Add(@"FacilityAttributes");
            id134_ContactAttributes = Reader.NameTable.Add(@"ContactAttributes");
            id21_ProjectStageType = Reader.NameTable.Add(@"ProjectStageType");
            id30_Facility = Reader.NameTable.Add(@"Facility");
            id116_MaxValueInteger = Reader.NameTable.Add(@"MaxValueInteger");
            id301_SpaceCollectionType = Reader.NameTable.Add(@"SpaceCollectionType");
            id118_Contact = Reader.NameTable.Add(@"Contact");
            id265_AssetBarCode = Reader.NameTable.Add(@"AssetBarCode");
            id83_propertySetName = Reader.NameTable.Add(@"propertySetName");
            id189_AssetTypeAttributes = Reader.NameTable.Add(@"AssetTypeAttributes");
            id256_AssetName = Reader.NameTable.Add(@"AssetName");
            id188_AssemblyAssignments = Reader.NameTable.Add(@"AssemblyAssignments");
            id28_Item = Reader.NameTable.Add(@"ProjectStageEnvironmentalAssesmentRating");
            id150_SystemCollectionType = Reader.NameTable.Add(@"SystemCollectionType");
            id314_ZoneAssignmentCollectionType = Reader.NameTable.Add(@"ZoneAssignmentCollectionType");
            id235_SpareDescription = Reader.NameTable.Add(@"SpareDescription");
            id75_DocumentURI = Reader.NameTable.Add(@"DocumentURI");
            id207_JobCategory = Reader.NameTable.Add(@"JobCategory");
            id292_FloorTypeBase = Reader.NameTable.Add(@"FloorTypeBase");
            id286_ZoneDescription = Reader.NameTable.Add(@"ZoneDescription");
            id210_JobDuration = Reader.NameTable.Add(@"JobDuration");
            id89_AttributeValueType = Reader.NameTable.Add(@"AttributeValueType");
            id252_WarrantyIssues = Reader.NameTable.Add(@"WarrantyIssues");
            id290_FloorCollectionType = Reader.NameTable.Add(@"FloorCollectionType");
            id285_ZoneCategory = Reader.NameTable.Add(@"ZoneCategory");
            id201_AssemblyIssues = Reader.NameTable.Add(@"AssemblyIssues");
            id221_ResourceTypeBase = Reader.NameTable.Add(@"ResourceTypeBase");
            id85_AttributeCategory = Reader.NameTable.Add(@"AttributeCategory");
            id200_AssemblyDocuments = Reader.NameTable.Add(@"AssemblyDocuments");
            id19_ProjectStages = Reader.NameTable.Add(@"ProjectStages");
            id185_Warranties = Reader.NameTable.Add(@"Warranties");
            id8_Item = Reader.NameTable.Add(@"");
            id84_AttributeName = Reader.NameTable.Add(@"AttributeName");
            id155_SystemDescription = Reader.NameTable.Add(@"SystemDescription");
            id71_DocumentTypeBase = Reader.NameTable.Add(@"DocumentTypeBase");
            id319_SiteDescription = Reader.NameTable.Add(@"SiteDescription");
            id143_ConnectionAsset1PortName = Reader.NameTable.Add(@"ConnectionAsset1PortName");
            id105_StringValue = Reader.NameTable.Add(@"StringValue");
            id33_ProjectStageKeyType = Reader.NameTable.Add(@"ProjectStageKeyType");
            id245_WarrantyType = Reader.NameTable.Add(@"WarrantyType");
            id234_SpareCategory = Reader.NameTable.Add(@"SpareCategory");
            id73_DocumentCategory = Reader.NameTable.Add(@"DocumentCategory");
            id315_ZoneAssignment = Reader.NameTable.Add(@"ZoneAssignment");
            id164_AssetTypeDescription = Reader.NameTable.Add(@"AssetTypeDescription");
            id273_SystemAssignment = Reader.NameTable.Add(@"SystemAssignment");
            id34_externalIDRefer = Reader.NameTable.Add(@"externalIDRefer");
            id190_AssetTypeDocuments = Reader.NameTable.Add(@"AssetTypeDocuments");
            id249_Item = Reader.NameTable.Add(@"WarrantyGaurantorContactAssignments");
            id141_ConnectionCategory = Reader.NameTable.Add(@"ConnectionCategory");
            id63_IssueImpactText = Reader.NameTable.Add(@"IssueImpactText");
            id25_ProjectStageStartDate = Reader.NameTable.Add(@"ProjectStageStartDate");
            id32_ProjectStageNext = Reader.NameTable.Add(@"ProjectStageNext");
            id239_SpareAttributes = Reader.NameTable.Add(@"SpareAttributes");
            id22_ProjectStageName = Reader.NameTable.Add(@"ProjectStageName");
            id296_FloorHeightValue = Reader.NameTable.Add(@"FloorHeightValue");
            id206_JobTaskID = Reader.NameTable.Add(@"JobTaskID");
            id158_SystemIssues = Reader.NameTable.Add(@"SystemIssues");
            id187_Jobs = Reader.NameTable.Add(@"Jobs");
            id270_AssetDocuments = Reader.NameTable.Add(@"AssetDocuments");
            id159_AssetTypeCollectionType = Reader.NameTable.Add(@"AssetTypeCollectionType");
            id2_Item = Reader.NameTable.Add(@"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
            id180_AssetTypeShapeDescription = Reader.NameTable.Add(@"AssetTypeShapeDescription");
            id254_Asset = Reader.NameTable.Add(@"Asset");
            id173_AssetTypeCodePerformance = Reader.NameTable.Add(@"AssetTypeCodePerformance");
            id101_AttributeMonetaryValueType = Reader.NameTable.Add(@"AttributeMonetaryValueType");
            id133_ContactURL = Reader.NameTable.Add(@"ContactURL");
            id103_MonetaryUnit = Reader.NameTable.Add(@"MonetaryUnit");
            id272_SystemAssignmentCollectionType = Reader.NameTable.Add(@"SystemAssignmentCollectionType");
            id93_AttributeDateTimeValue = Reader.NameTable.Add(@"AttributeDateTimeValue");
            id225_ResourceAttributes = Reader.NameTable.Add(@"ResourceAttributes");
            id108_AttributeAllowedValue = Reader.NameTable.Add(@"AttributeAllowedValue");
            id156_SystemAttributes = Reader.NameTable.Add(@"SystemAttributes");
            id196_AssemblyParentName = Reader.NameTable.Add(@"AssemblyParentName");
            id194_AssemblyTypeBase = Reader.NameTable.Add(@"AssemblyTypeBase");
            id11_ProjectAddress = Reader.NameTable.Add(@"ProjectAddress");
            id191_AssetTypeIssues = Reader.NameTable.Add(@"AssetTypeIssues");
            id275_externalIDReference = Reader.NameTable.Add(@"externalIDReference");
            id263_AssetStartDate = Reader.NameTable.Add(@"AssetStartDate");
            id96_AttributeMonetaryValue = Reader.NameTable.Add(@"AttributeMonetaryValue");
            id35_FacilityTypeBase = Reader.NameTable.Add(@"FacilityTypeBase");
            id308_SpaceGrossAreaValue = Reader.NameTable.Add(@"SpaceGrossAreaValue");
            id152_SystemTypeBase = Reader.NameTable.Add(@"SystemTypeBase");
            id65_ContactAssignment = Reader.NameTable.Add(@"ContactAssignment");
            id176_AssetTypeFeaturesDescription = Reader.NameTable.Add(@"AssetTypeFeaturesDescription");
            id288_ZoneDocuments = Reader.NameTable.Add(@"ZoneDocuments");
            id291_Floor = Reader.NameTable.Add(@"Floor");
            id13_ProjectProcurement = Reader.NameTable.Add(@"ProjectProcurement");
            id317_SiteType = Reader.NameTable.Add(@"SiteType");
            id15_ProjectExpectedLifeCycle = Reader.NameTable.Add(@"ProjectExpectedLifeCycle");
            id184_Item = Reader.NameTable.Add(@"AssetTypeManufacturerContactAssignments");
            id228_DecimalValueType = Reader.NameTable.Add(@"DecimalValueType");
            id258_AssetDescription = Reader.NameTable.Add(@"AssetDescription");
            id251_WarrantyDocuments = Reader.NameTable.Add(@"WarrantyDocuments");
            id130_ContactRegionCode = Reader.NameTable.Add(@"ContactRegionCode");
            id120_ContactEmail = Reader.NameTable.Add(@"ContactEmail");
            id72_DocumentName = Reader.NameTable.Add(@"DocumentName");
            id142_ConnectionAsset1Name = Reader.NameTable.Add(@"ConnectionAsset1Name");
            id261_AssetInstalledModelNumber = Reader.NameTable.Add(@"AssetInstalledModelNumber");
            id20_ProjectStage = Reader.NameTable.Add(@"ProjectStage");
            id123_ContactPhoneNumber = Reader.NameTable.Add(@"ContactPhoneNumber");
            id58_IssueTypeBase = Reader.NameTable.Add(@"IssueTypeBase");
            id262_AssetWarrantyStartDate = Reader.NameTable.Add(@"AssetWarrantyStartDate");
            id193_AssemblyAssignment = Reader.NameTable.Add(@"AssemblyAssignment");
            id106_AllowedValues = Reader.NameTable.Add(@"AllowedValues");
            id255_AssetInfoTypeBase = Reader.NameTable.Add(@"AssetInfoTypeBase");
            id10_ProjectDescription = Reader.NameTable.Add(@"ProjectDescription");
            id14_ProjectFunctionalUnit = Reader.NameTable.Add(@"ProjectFunctionalUnit");
            id79_AttributeCollectionType = Reader.NameTable.Add(@"AttributeCollectionType");
            id94_AttributeStringValue = Reader.NameTable.Add(@"AttributeStringValue");
            id306_SpaceSignageName = Reader.NameTable.Add(@"SpaceSignageName");
            id78_DocumentIssues = Reader.NameTable.Add(@"DocumentIssues");
            id59_IssueName = Reader.NameTable.Add(@"IssueName");
            id69_DocumentCollectionType = Reader.NameTable.Add(@"DocumentCollectionType");
            id17_ProjectConstructionStart = Reader.NameTable.Add(@"ProjectConstructionStart");
            id39_SiteAssignment = Reader.NameTable.Add(@"SiteAssignment");
            id232_SpareTypeBase = Reader.NameTable.Add(@"SpareTypeBase");
            id316_ZoneKeyType = Reader.NameTable.Add(@"ZoneKeyType");
            id60_IssueCategory = Reader.NameTable.Add(@"IssueCategory");
            id222_ResourceName = Reader.NameTable.Add(@"ResourceName");
            id297_Spaces = Reader.NameTable.Add(@"Spaces");
            id230_SpareCollectionType = Reader.NameTable.Add(@"SpareCollectionType");
            id250_WarrantyAttributes = Reader.NameTable.Add(@"WarrantyAttributes");
            id220_Resource = Reader.NameTable.Add(@"Resource");
            id41_FacilityDefaultAreaUnit = Reader.NameTable.Add(@"FacilityDefaultAreaUnit");
            id271_AssetIssues = Reader.NameTable.Add(@"AssetIssues");
            id135_ContactDocuments = Reader.NameTable.Add(@"ContactDocuments");
            id82_propertySetExternalIdentifier = Reader.NameTable.Add(@"propertySetExternalIdentifier");
            id26_ProjectStageEndDate = Reader.NameTable.Add(@"ProjectStageEndDate");
            id54_FacilityDocuments = Reader.NameTable.Add(@"FacilityDocuments");
            id215_Resources = Reader.NameTable.Add(@"Resources");
            id125_ContactGivenName = Reader.NameTable.Add(@"ContactGivenName");
            id229_IntegerValueType = Reader.NameTable.Add(@"IntegerValueType");
            id111_MinValueDecimal = Reader.NameTable.Add(@"MinValueDecimal");
            id139_ConnectionTypeBase = Reader.NameTable.Add(@"ConnectionTypeBase");
            id233_SpareName = Reader.NameTable.Add(@"SpareName");
            id199_AssemblyAttributes = Reader.NameTable.Add(@"AssemblyAttributes");
            id226_ResourceDocuments = Reader.NameTable.Add(@"ResourceDocuments");
            id76_DocumentReferenceURI = Reader.NameTable.Add(@"DocumentReferenceURI");
            id70_Document = Reader.NameTable.Add(@"Document");
            id146_ConnectionDescription = Reader.NameTable.Add(@"ConnectionDescription");
            id216_JobAttributes = Reader.NameTable.Add(@"JobAttributes");
            id302_Space = Reader.NameTable.Add(@"Space");
            id213_JobFrequencyValue = Reader.NameTable.Add(@"JobFrequencyValue");
            id237_SparePartNumber = Reader.NameTable.Add(@"SparePartNumber");
            id97_AttributeBooleanValue = Reader.NameTable.Add(@"AttributeBooleanValue");
            id267_AssetLocationDescription = Reader.NameTable.Add(@"AssetLocationDescription");
            id40_FacilityDefaultLinearUnit = Reader.NameTable.Add(@"FacilityDefaultLinearUnit");
            id303_SpaceTypeBase = Reader.NameTable.Add(@"SpaceTypeBase");
            id223_ResourceCategory = Reader.NameTable.Add(@"ResourceCategory");
            id218_JobIssues = Reader.NameTable.Add(@"JobIssues");
            id121_ContactCategory = Reader.NameTable.Add(@"ContactCategory");
            id172_AssetTypeAccessibilityText = Reader.NameTable.Add(@"AssetTypeAccessibilityText");
            id145_ConnectionAsset2PortName = Reader.NameTable.Add(@"ConnectionAsset2PortName");
            id144_ConnectionAsset2Name = Reader.NameTable.Add(@"ConnectionAsset2Name");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReaderProjectType();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriterProjectType();
        }
    }

    public sealed class ProjectTypeSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"Project", @"http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterProjectType)writer).Write99_Project(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderProjectType)reader).Read99_Project();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderProjectType(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterProjectType(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Xbim.COBieLiteUK.ProjectType:http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Project:False:"] = @"Read99_Project";
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
                    _tmp[@"Xbim.COBieLiteUK.ProjectType:http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Project:False:"] = @"Write99_Project";
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
                    _tmp.Add(@"Xbim.COBieLiteUK.ProjectType:http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite:Project:False:", new ProjectTypeSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Xbim.COBieLiteUK.ProjectType)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Xbim.COBieLiteUK.ProjectType)) return new ProjectTypeSerializer();
            return null;
        }
    }
}
