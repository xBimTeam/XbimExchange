using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingSystemViaIfcPropertyToSystem : XbimMappings<IfcStore, IModel, int, IIfcPropertySet, CobieSystem>
    {
        private MappingIfcElementToComponent _elementToComponent;

        protected MappingIfcElementToComponent ElementToComponent
        {
            get { return _elementToComponent ?? (_elementToComponent = Exchanger.GetOrCreateMappings<MappingIfcElementToComponent>()); }
        }

        protected override CobieSystem Mapping(IIfcPropertySet pSet, CobieSystem target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;

            //Add Assets
            var systemAssignments = helper.GetSystemAssignments(pSet);

            var elements = systemAssignments.OfType<IIfcElement>().ToList();
            var name = string.Empty;
            if (elements.Any())
            {
                name = GetSystemName(helper, elements);

                foreach (var element in elements)
                {
                    CobieComponent component;
                    if (ElementToComponent.GetOrCreateTargetObject(element.EntityLabel, out component))
                        ElementToComponent.AddMapping(element, component);
                    if (!target.Components.Contains(component))
                        target.Components.Add(component);
                }
            }

            target.ExternalObject = helper.GetExternalObject(pSet);
            target.ExternalId = helper.ExternalEntityIdentity(pSet);
            target.ExternalSystem = helper.GetExternalSystem(pSet);
            target.Name = string.IsNullOrEmpty(name) ? "Unknown" + pSet.EntityLabel : name;
            target.Description = string.IsNullOrEmpty(pSet.Description) ? name : pSet.Description.ToString();
            target.Created = helper.GetCreatedInfo(pSet);
            target.Categories.AddRange(helper.GetCategories(pSet));

            //Attributes, no attributes from PSet as Pset is the attributes, assume that component attributes are extracted by each component anyway
            //target.Attributes = helper.GetAttributes(pSet);

            //Documents
            helper.AddDocuments(target, pSet);

            //TODO: System Issues
           
            return target;
        }


        /// <summary>
        /// Get system name from a IfcObjectDefinition
        /// </summary>
        /// <param name="helper">CoBieLiteUkHelper</param>
        /// <param name="ifcObjects"></param>
        /// <returns></returns>
        private static string GetSystemName(COBieExpressHelper helper, IEnumerable<IIfcObjectDefinition> ifcObjects )
        {   var name = string.Empty;
            var propMaps = helper.GetPropMap("SystemMaps").ToList();
            if (!propMaps.Any()) return name;

            propMaps = propMaps.Concat(propMaps.ConvertAll(s => s.Split('.')[0] + ".System Classification")).ToList();
            var propNameOrder = propMaps.Select(s => s.Split('.')[1]).Distinct().ToList();
                    
            foreach (var atts in ifcObjects.Select(helper.GetAttributesObj))
            {
                if (atts != null)
                {
                    //get propery values as system name
                    var values = atts.Properties
                        .Where(prop => propMaps.Contains(prop.Key))
                        .Select(prop => prop.Value)
                        .OfType<IIfcPropertySingleValue>()
                        .Where(propSv => propSv.NominalValue != null && !string.IsNullOrEmpty(propSv.NominalValue.ToString()))
                        .Select(propSv => propSv.Name.ToString() + ":" + propSv.NominalValue.ToString())
                        .Distinct()
                        .OrderBy(s => propNameOrder.IndexOf(s.Split(':')[0]));
                    if (values.Any())
                    {
                        name = values.First();//string.Join(":", value);
                    }
                    else //no name so try proprty names
                    {
                        //Try and get the property names as system name
                        values = atts.Properties
                            .Where(prop => propMaps.Contains(prop.Key))
                            .Select(prop => prop.Value)
                            .OfType<IIfcPropertySingleValue>()
                            .Where(propSv => propSv.Name != null && !string.IsNullOrEmpty(propSv.Name.ToString()))
                            .Select(prop => prop.Name.ToString())
                            .Distinct()
                            .OrderBy(s => propNameOrder.IndexOf(s));
                        if (values.Any())
                        {
                            name = string.Join(":", values);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(name)) break; //exit loop if name can be constructed
            }
            return name;
        }


        public override CobieSystem CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieSystem>();
        }
    }
}
