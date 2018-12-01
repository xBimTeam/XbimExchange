using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"TestFiles\Ifc\")]
    public class COBieDocumentTests
    {
        /// <summary>
        /// Proof on concept
        /// </summary>
        [TestMethod]
        public void TestMethod()
        {
            var doclist = new[] {
                new {RelatingDocument = "Doc1", RelatedObjects = new List<string>() { "Ent1", "Ent2", "Ent3", "Ent4", "Ent5" } },
                new {RelatingDocument = "Doc1", RelatedObjects = new List<string>() { "Ent1", "Ent6", "Ent7" } },
                new {RelatingDocument = "Doc1", RelatedObjects = new List<string>() { "Ent8", "Ent9" } },
                new {RelatingDocument = "Doc2", RelatedObjects = new List<string>() { "Ent1", "Ent6", "Ent7" } },
                new {RelatingDocument = "Doc3", RelatedObjects = new List<string>() { "Ent1", "Ent6" } },
                new {RelatingDocument = "Doc4", RelatedObjects = new List<string>() { "Ent1", "Ent2", "Ent7" } },
                new {RelatingDocument = "Doc5", RelatedObjects = new List<string>() { "Ent1", "Ent2", "Ent3" } },
                new {RelatingDocument = "Doc1", RelatedObjects = new List<string>() { "Ent10", "Ent11" } },
                new {RelatingDocument = "Doc2", RelatedObjects = new List<string>() { "Ent10", "Ent11" } },
            }.ToList();

            Dictionary<string, List<string>> dic = null;
            //check for duplicates
            var dups = doclist.GroupBy(d => d.RelatingDocument).SelectMany(grp => grp.Skip(1));
            //combine object lists to single key value, merge lists
            var dupsMerge = dups.GroupBy(d => d.RelatingDocument).Select(p => new { x = p.Key, y = p.SelectMany(c => c.RelatedObjects) });

            if (dupsMerge.Any())
            {
                //remove the duplicates and convert to dictionary
                dic = doclist.Except(dups).ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects);

                //add the duplicate doc referenced object lists to the original doc
                foreach (var item in dupsMerge)
                {
                    dic[item.x] = dic[item.x].Union(item.y).ToList();
                }
            }
            else
            {
                //no duplicates, so just convert to dictionary
                dic = doclist.ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects);
            }

            //reverse lookup to entity to list of documents
            var newDic = dic
                            .SelectMany(pair => pair.Value
                            .Select(val => new { Key = val, Value = pair.Key }))
                            .GroupBy(item => item.Key)
                            .ToDictionary(gr => gr.Key, gr => gr.Select(item => item.Value));

            Assert.AreEqual(newDic["Ent1"].Count(), 5);
            Assert.AreEqual(newDic["Ent2"].Count(), 3);
            Assert.AreEqual(newDic["Ent3"].Count(), 2);
            Assert.AreEqual(newDic["Ent4"].Count(), 1);
            Assert.AreEqual(newDic["Ent5"].Count(), 1);
            Assert.AreEqual(newDic["Ent6"].Count(), 3);
            Assert.AreEqual(newDic["Ent7"].Count(), 3);
            Assert.AreEqual(newDic["Ent8"].Count(), 1);
            Assert.AreEqual(newDic["Ent9"].Count(), 1);
            Assert.AreEqual(newDic["Ent10"].Count(), 2);
            Assert.AreEqual(newDic["Ent11"].Count(), 2);
        }

        [TestMethod]
        public void TestDocExtraction()
        {
            var ifcFile = "Clinic-Handover-v12.ifc";
            var xbimFile = Path.ChangeExtension(ifcFile, "xbim");
            using (var _model = IfcStore.Open(ifcFile))
            {
                var ifcRelAssociatesDocuments = _model.Instances.OfType<IfcRelAssociatesDocument>();

                var dups = ifcRelAssociatesDocuments.GroupBy(d => d.RelatingDocument).SelectMany(grp => grp.Skip(1));
                Dictionary<IfcDocumentSelect, List<IfcRoot>> docToObjs = null;
                if (dups.Any())
                {
                    var dupsMerge = dups.GroupBy(d => d.RelatingDocument).Select(p => new { x = p.Key, y = p.SelectMany(c => c.RelatedObjects) });
                    //remove the duplicates and convert to dictionary
                    docToObjs = ifcRelAssociatesDocuments.Except(dups).ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects.ToList());

                    //add the duplicate doc referenced object lists to the original doc
                    foreach (var item in dupsMerge)
                    {
                        docToObjs[item.x] = docToObjs[item.x].Union(item.y).ToList();
                    }
                }
                else
                {
                    //no duplicates, so just convert to dictionary
                    docToObjs = ifcRelAssociatesDocuments.ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects.ToList());
                }

                //Lets set up some children documents
                using (var txn = _model.BeginTransaction("Add Documents"))
                {
                    foreach (var item in docToObjs)
                    {
                        var doc1 = item.Key as IfcDocumentInformation;
                        if (doc1 != null)
                        {
                            var docRelChild1 = _model.Instances.New<IfcDocumentInformationRelationship>();
                            docRelChild1.RelatingDocument = doc1;
                            //add child docs
                            var childDocA = CreateDocInfo(_model, "ChildDoc1a", @"c:\TestDir\Dir1");
                            var childDocB = CreateDocInfo(_model, "ChildDoc1b", @"c:\TestDir\Dir1");
                            var childDocC = CreateDocInfo(_model, "ChildDoc1c", @"c:\TestDir\Dir1");
                            docRelChild1.RelatedDocuments.Add(childDocA);
                            docRelChild1.RelatedDocuments.Add(childDocB);
                            docRelChild1.RelatedDocuments.Add(childDocC);

                            //add another layer
                            var docRelChild2 = _model.Instances.New<IfcDocumentInformationRelationship>();
                            docRelChild2.RelatingDocument = childDocA;
                            var childDoc2D = CreateDocInfo(_model, "ChildDoc2d", @"c:\TestDir\Dir1\Dir2");
                            var childDoc2E = CreateDocInfo(_model, "ChildDoc2e", @"c:\TestDir\Dir1\Dir2");
                            docRelChild2.RelatedDocuments.Add(childDoc2D);
                            docRelChild2.RelatedDocuments.Add(childDoc2E);

                            //add another layer
                            var docRelChild3 = _model.Instances.New<IfcDocumentInformationRelationship>();
                            docRelChild3.RelatingDocument = childDoc2D;
                            var childDoc3F = CreateDocInfo(_model, "ChildDoc3f", @"c:\TestDir\Dir1\Dir2\Dir3");
                            docRelChild3.RelatedDocuments.Add(childDoc3F);

                        }
                    }

                    CreateDocInfo(_model, "orphanDocA", @"c:");


                    var orphanDocB = _model.Instances.New<IfcDocumentReference>();
                    orphanDocB.Name = "orphanDocB";
                    orphanDocB.Location = @"x:";
                    txn.Commit();

                    _model.SaveAs("Clinic-Handover_ChildDocs.ifc", StorageType.Ifc);
                }


                //Get all documents information objects held in model
                var docAllInfos = _model.Instances.OfType<IfcDocumentInformation>();
                //Get the child document relationships
                var childDocRels = _model.Instances.OfType<IfcDocumentInformationRelationship>();



                //get the already attached to entity documents 
                var docInfosAttached = docToObjs.Select(dictionary => dictionary.Key).OfType<IfcDocumentInformation>();

                //see if we have any documents not attached to IfcRoot objects, but could be attached as children documents to a parent document
                var docInfosNotAttached = docAllInfos.Except(docInfosAttached);

                List<IfcDocumentInformation> docChildren = docInfosAttached.ToList(); //first check on docs attached to IfcRoot Objects
                int idx = 0;
                do
                {
                    //get the relationships that are attached to the docs already associated with an IfcRoot object on first pass, then associated with all children, drilling down until nothing found
                    docChildren = childDocRels.Where(docRel => docChildren.Contains(docRel.RelatingDocument)).SelectMany(docRel => docRel.RelatedDocuments).ToList(); //docs that are children to attached entity docs, drilling down
                    docInfosNotAttached = docInfosNotAttached.Except(docChildren); //attached by association to the root parent document, so remove from none attached document list


                } while (docChildren.Any() && (++idx < 100)); //assume that docs are not embedded deeper than 100

                Assert.IsTrue(docInfosNotAttached.Count() == 1);


                //get all the doc reference objects held in the model
                var docAllRefs = _model.Instances.OfType<IfcDocumentReference>();
                //get all attached document references
                var docRefsAttached = docToObjs.Select(dictionary => dictionary.Key).OfType<IfcDocumentReference>();
                //checked on direct attached to object document references
                var docRefsNotAttached = docAllRefs.Except(docRefsAttached).ToList();

                //Check for document references held in the IfcDocumentInformation objects
                var docRefsAttachedDocInfo = docAllInfos.Where(docInfo => docInfo.DocumentReferences != null).SelectMany(docInfo => docInfo.DocumentReferences);
                //remove from Not Attached list
                docRefsNotAttached = docRefsNotAttached.Except(docRefsAttachedDocInfo).ToList();

                Assert.IsTrue(docRefsNotAttached.Count() == 1);

                //reverse lookup to entity to list of documents
                var newDic = docToObjs
                                .SelectMany(pair => pair.Value
                                .Select(val => new { Key = val, Value = pair.Key }))
                                .GroupBy(item => item.Key)
                                .ToLookup(gr => gr.Key, gr => gr.Select(item => item.Value));

                foreach (var group in newDic)
                {
                    foreach (var item in group)
                    {
                        Assert.IsTrue(item.Count() > 0);
                        //foreach (var doc in item)
                        //{
                        //    if (doc is IfcDocumentInformation)
                        //    {
                        //        Debug.WriteLine("Doc {0}", ((IfcDocumentInformation)doc).Name);
                        //    }
                        //    else
                        //    {
                        //        Debug.WriteLine("Doc {0}", ((IfcDocumentReference)doc).Name);
                        //    }

                        //} 
                    }
                }

            }
        }

        private IfcDocumentInformation CreateDocInfo(IModel model, string name, string location)
        {
            var docinfo = model.Instances.New<IfcDocumentInformation>();
            docinfo.Name = name;
            var docRef = model.Instances.New<IfcDocumentReference>();
            docRef.Location = location;
            docRef.Name = "Ref" + name + ".txt";
            docinfo.DocumentReferences.Add(docRef);
            return docinfo;
        }
        
    }
}
