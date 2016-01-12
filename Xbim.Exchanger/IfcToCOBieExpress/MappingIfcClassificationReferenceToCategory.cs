using System;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcClassificationReferenceToCategory: XbimMappings<IfcStore, IModel, string, IIfcClassificationReference, CobieCategory>
    {
        private MappingIfcClassificationToCobieClassification _classificationMapping;

        private MappingIfcClassificationToCobieClassification ClassificationMapping
        {
            get { return _classificationMapping ?? (_classificationMapping = Exchanger.GetOrCreateMappings<MappingIfcClassificationToCobieClassification>()); }
        }

        public override CobieCategory CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieCategory>();
        }

        protected override CobieCategory Mapping(IIfcClassificationReference source, CobieCategory target)
        {
            if (source.Identification.HasValue && source.Name.HasValue &&
                    string.CompareOrdinal(source.Identification, source.Name) == 0)
            {
                var strRef = source.Identification.Value.ToString();
                var parts = strRef.Split(new[] { ':', ';', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                    target.Description = parts[1];
                if (parts.Length > 0)
                    target.Value = parts[0];
            }
            else
            {
                target.Value = source.Identification;
                target.Description = source.Name;
            }
            target.Value = source.Identification;
            target.Description = source.Name;

            var ifcClassification = source.ReferencedSource as IIfcClassification;
            if (ifcClassification == null) return target;

            CobieClassification cls;
            if (ClassificationMapping.GetOrCreateTargetObject(ifcClassification.Name, out cls))
                ClassificationMapping.AddMapping(ifcClassification, cls);
            target.Classification = cls;
            return target;
        }
    }
}
