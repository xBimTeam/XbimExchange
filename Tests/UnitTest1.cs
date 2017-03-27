using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Ifc;
using XbimExchanger.IfcToCOBieLiteUK;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.UtilityResource;
using IfcInterfaces = Xbim.Ifc4.Interfaces;

namespace Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void XbimExchangeIssues17()
        {
            var model = CreateandInitModel("xbimTest");
            PopulatePerson(model);
            var site = CreateSite(model, "default site");
            CreateBuilding(model, "default building", site);

            var fileName = Path.GetTempPath() + Guid.NewGuid() + ".xlsx";
            SaveCobieFile(model, fileName);
        }

        private static void PopulatePerson(IfcStore model)
        {
            using (var txn = model.BeginTransaction("make a person"))
            {

                var person = model.DefaultOwningUser.ThePerson;
                var telecomAddress =
                    model.Instances.New<IfcTelecomAddress>();
                var postalAddress = model.Instances.New<IfcPostalAddress>();
                person.FamilyName = "doe";
                person.GivenName = "john";
                person.MiddleNames.Add("simple");

                telecomAddress.ElectronicMailAddresses.Add("blahblah@google.com");
                telecomAddress.TelephoneNumbers.Add("555-1234");
                telecomAddress.FacsimileNumbers.Add("555-4321");
                telecomAddress.PagerNumber = "who has a pager anymore";
                telecomAddress.WWWHomePageURL = "www.google.com";

                postalAddress.AddressLines.Add("10 nowhere ave");
                postalAddress.AddressLines.Add("nowhereville");
                postalAddress.Country = "Antarctica";
                postalAddress.Description = "just some dude";
                postalAddress.InternalLocation = "Gall Bladder";
                postalAddress.PostalBox = "000000";
                postalAddress.PostalBox = "1";
                postalAddress.Purpose = IfcAddressTypeEnum.OFFICE;
                postalAddress.Region = "the top one";
                postalAddress.Town = "nowhereville";

                person.Addresses.Add(telecomAddress);
                person.Addresses.Add(postalAddress);
                txn.Commit();
            }
        }

        protected static IfcStore CreateandInitModel(string projectName = null)
        {
            IfcStore model = IfcStore.Create(new XbimEditorCredentials()
            {
                ApplicationFullName = "V6",
                ApplicationDevelopersName = "SoftTech",
                ApplicationIdentifier = "v6",
                ApplicationVersion = "0.1",
                EditorsFamilyName = "Bloggs",
                EditorsGivenName = "Joe",
                EditorsOrganisationName = "SoftTech"
            },
            Xbim.Common.Step21.IfcSchemaVersion.Ifc2X3, XbimStoreType.InMemoryModel); //create an empty model


            //Begin a transaction as all changes to a model are transacted
            using (var txn = model.BeginTransaction("Initialise Model"))
            {
                //do once only initialisation of model application and editor values
                model.DefaultOwningUser.ThePerson.GivenName = "n/a";
                model.DefaultOwningUser.ThePerson.FamilyName = "n/a";
                model.DefaultOwningUser.TheOrganization.Name = "n/a";
                model.DefaultOwningApplication.ApplicationIdentifier = "SoftTech";
                model.DefaultOwningApplication.ApplicationDeveloper.Name = "SoftTech Ltd.";
                model.DefaultOwningApplication.ApplicationFullName = "V6";
                model.DefaultOwningApplication.Version = "n/a";

                //set up a project and initialise the defaults

                var project = model.Instances.New<IfcProject>();
                //project.Initialize(Xbim.Common.ProjectUnits.SIUnitsUK);
                //project.SetOrChangeSiUnit(Xbim.Ifc2x3.MeasureResource.IfcUnitEnum.LENGTHUNIT
                //	, Xbim.Ifc2x3.MeasureResource.IfcSIUnitName.METRE,
                //	null);
                project.Initialize(Xbim.Common.ProjectUnits.SIUnitsUK);
                project.SetOrChangeConversionUnit(Xbim.Ifc2x3.MeasureResource.IfcUnitEnum.LENGTHUNIT, Xbim.Ifc2x3.MeasureResource.ConversionBasedUnit.Inch);
                project.Name = "testProject";
                project.OwnerHistory.OwningUser = model.DefaultOwningUser as IfcPersonAndOrganization;
                project.OwnerHistory.OwningApplication = model.DefaultOwningApplication as IfcApplication;

                //project.ModelContext.WorldCoordinateSystem = model._New<IfcAxis2Placement3D>(c =>
                //	{
                //		c.Axis = model._New<IfcDirection>(d => d.SetXYZ(0,0, -1));
                //		c.RefDirection = model._New<IfcDirection>(d => d.SetXYZ(0, 1, 0));
                //	}
                //	);
                txn.Commit();
                return model;
            }

        }

        public static IfcSite CreateSite(IfcStore model, string name, double x = 0, double y = 0, double z = 0)
        {
            using (var txn = model.BeginTransaction("Create Site"))
            {
                var site = model.Instances.New<IfcSite>();
                site.Name = name;
                site.OwnerHistory.OwningUser = model.DefaultOwningUser as Xbim.Ifc2x3.ActorResource.IfcPersonAndOrganization;
                site.OwnerHistory.OwningApplication = model.DefaultOwningApplication as Xbim.Ifc2x3.UtilityResource.IfcApplication;
                //building.ElevationOfRefHeight = elevHeight;
                site.CompositionType = IfcElementCompositionEnum.ELEMENT;


                IfcProject(model).AddSite(site);
                txn.Commit();
                return site;
            }
        }

        public static IfcBuilding CreateBuilding(IfcStore model, string name, IfcSite site = null)
        {
            using (var txn = model.BeginTransaction("Create Building"))
            {
                var building = model.Instances.New<IfcBuilding>();
                building.Name = name;
                building.OwnerHistory.OwningUser = model.DefaultOwningUser as Xbim.Ifc2x3.ActorResource.IfcPersonAndOrganization;
                building.OwnerHistory.OwningApplication = model.DefaultOwningApplication as Xbim.Ifc2x3.UtilityResource.IfcApplication;
                //building.ElevationOfRefHeight = elevHeight;
                building.CompositionType = IfcElementCompositionEnum.ELEMENT;


                if (site == null)
                {
                    IfcProject(model).AddBuilding(building);
                }
                else
                {
                    AddBuildingElement(site, building);
                }
                txn.Commit();
                return building;
            }
        }

        private static IfcProject IfcProject(IfcStore model)
        {
            return model.Instances.FirstOrDefault(c => c is IfcProject) as IfcProject;
        }

        public static void AddBuildingElement(
            IfcInterfaces.IIfcObjectDefinition site,
            IfcInterfaces.IIfcObjectDefinition building)
        {
            IEnumerable<IfcInterfaces.IIfcRelAggregates> decomposition = site.IsDecomposedBy;
            if (decomposition.Count() == 0) //none defined create the relationship
            {
                IfcInterfaces.IIfcRelAggregates relSub = site.Model.Instances.New<IfcRelAggregates>();
                relSub.RelatingObject = site;
                relSub.RelatedObjects.Add(building);
            }
            else
            {
                decomposition.First().RelatedObjects.Add(building);
            }
        }


        public static void SaveCobieFile(IfcStore model, string outputFile)
        {
            //Logger.LogCurrentMethod();
            var facilities = new List<Xbim.CobieLiteUk.Facility>();
            var exchange = new IfcToCOBieLiteUkExchanger(model, facilities);
            facilities = exchange.Convert();
            foreach (var facilityType in facilities)
            {
                var jsonFile = Path.ChangeExtension(outputFile, ".cobielite.json");

                // write json
                facilityType.WriteJson(jsonFile, true);
                Assert.IsTrue(File.Exists(jsonFile));

                // write xls
                var cobieFile = Path.ChangeExtension(outputFile, ".cobielite.xls");
                string message = "";
                facilityType.WriteCobie(cobieFile, out message);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine(message);
                }
                Assert.IsTrue(File.Exists(cobieFile));
                break;
            }
        }
    }
}

