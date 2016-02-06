using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal abstract class MappingIfcObjectToAsset<TIfc, TCobie> : XbimMappings<IfcStore, IModel, int, TIfc, TCobie>
        where TIfc : IIfcObjectDefinition
        where TCobie : CobieAsset
    {
        private COBieExpressHelper _helper;

        public COBieExpressHelper Helper
        {
            get { return _helper ?? (_helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper); }
        }


        protected override TCobie Mapping(TIfc source, TCobie target)
        {
            target.ExternalObject = Helper.GetExternalObject(source);
            target.ExternalId = Helper.ExternalEntityIdentity(source);
            target.AltExternalId = source.GlobalId;
            target.ExternalSystem = Helper.GetExternalSystem(source);
            target.Name = source.Name;
            target.Description = source.Description;
            target.Created = Helper.GetCreatedInfo(source);
            
            //Classification
            target.Categories.AddRange(Helper.GetCategories(source));
            
            //Attributes
            target.Attributes.AddRange(Helper.GetAttributes(source));

            //Documents
            Helper.AddDocuments(target, source);

            return target;
        }
    }
}
