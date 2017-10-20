using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using Xbim.CobieLiteUk;
using Xbim.Common;
using Xbim.Common.Federation;
using Xbim.Common.Geometry;
using Xbim.Ifc;
using Xbim.Presentation;
using Xbim.Presentation.LayerStyling;

namespace XplorerPlugin.DPoW
{
    public class TrafficLightStyler : ILayerStyler
    {
        private enum LayerGroup
        {
            Red,
            Green,
            Blue,
            Amber,
            Null
        }

        private readonly IModel _model;
        private readonly MainWindow _window;

        public bool UseBlue { get; set; }

        public bool UseAmber { get; set; }

        private static WpfMeshGeometry3D PrepareMesh(XbimColour col)
        {
            var wpfMaterial = new WpfMaterial(col);
            return new WpfMeshGeometry3D(wpfMaterial, wpfMaterial);
        }

        public TrafficLightStyler(IModel model, MainWindow window)
        {
            _window = window;
            _model = model;
            UseAmber = false;
            UseBlue = true;
        }

        

        

        XbimScene<WpfMeshGeometry3D, WpfMaterial> ILayerStyler.BuildScene(IModel model, XbimMatrix3D modelTransform, ModelVisual3D opaqueShapes, ModelVisual3D transparentShapes, List<Type> exclude)
        {
            
            
            var excludedTypes = model.DefaultExclusions(exclude);
            var tmpOpaquesGroup = new Model3DGroup();
            var retScene = new XbimScene<WpfMeshGeometry3D, WpfMaterial>(model);
            
            
            if (_model.Instances == null || !_model.Instances.Any())
                return retScene;

            // define colours
            var colours = new Dictionary<LayerGroup, XbimColour>();
            colours.Add(LayerGroup.Green, new XbimColour("Green", 0.0, 1.0, 0.0, 0.5));
            colours.Add(LayerGroup.Red, new XbimColour("Red", 1.0, 0.0, 0.0, 0.5));
            colours.Add(LayerGroup.Amber, new XbimColour("Amber", .5, 0.5, .5, .8)); // actually grey

            // prepare meshes
            //

            var meshes = new List<WpfMeshGeometry3D>(); // this list gets added to the scene at the end.

            // this dictionary holds the list of meshes that are currently used, as they are filled (in size), 
            // new ones get replaced
            //
            var meshDic = new Dictionary<LayerGroup, WpfMeshGeometry3D>(); 
            foreach (var group in colours.Keys)
            {
                var newItem = PrepareMesh(colours[group]);
                meshDic.Add(group, newItem);
                meshes.Add(newItem);
            }
            
            foreach (var mesh in meshes)
            {
                mesh.WpfModel.SetValue(FrameworkElement.TagProperty, mesh);
                mesh.BeginUpdate();
                tmpOpaquesGroup.Children.Add(mesh);
            }

            using (var geomStore = model.GeometryStore)
            {
                using (var geomReader = geomStore.BeginRead())
                {
                    var shapeInstances = geomReader.ShapeInstances
                        .Where(s => s.RepresentationType == XbimGeometryRepresentationType.OpeningsAndAdditionsIncluded
                                    &&
                                    !excludedTypes.Contains(s.IfcTypeId));
                    foreach (var shapeInstance in shapeInstances)
                    {
                        var ent = _model.Instances[shapeInstance.IfcProductLabel];
                        var grp = GetLayerGroup(ent);
                        if (grp == LayerGroup.Null)
                            continue;
                        if (!UseAmber && grp == LayerGroup.Amber)
                            continue;

                        WpfMeshGeometry3D targetMergeMesh;
                        meshDic.TryGetValue(grp, out targetMergeMesh);
                            
                        if (targetMergeMesh == null)
                            continue;

                        // replace target mesh beyond suggested size
                        // https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/maximize-wpf-3d-performance
                        // 
                        if (targetMergeMesh.PositionCount > 20000
                            ||
                            targetMergeMesh.TriangleIndexCount > 60000
                        )
                        {
                            // end current mesh
                            targetMergeMesh.EndUpdate();
                            
                            // prepare new one and add to the list
                            var replace = PrepareMesh(colours[grp]);
                            meshes.Add(replace);

                            // swap mesh in dicionary and for this loop
                            meshDic[grp] = replace;
                            targetMergeMesh = replace;

                            // prepare in output
                            replace.WpfModel.SetValue(FrameworkElement.TagProperty, replace);
                            replace.BeginUpdate();
                            tmpOpaquesGroup.Children.Add(replace);
                        }

                        IXbimShapeGeometryData shapeGeom = geomReader.ShapeGeometry(shapeInstance.ShapeGeometryLabel);
                        if (shapeGeom.Format == (byte)XbimGeometryType.PolyhedronBinary)
                        {
                            var transform = XbimMatrix3D.Multiply(shapeInstance.Transformation,
                                modelTransform);
                            targetMergeMesh.Add(
                                shapeGeom.ShapeData,
                                shapeInstance.IfcTypeId,
                                shapeInstance.IfcProductLabel,
                                shapeInstance.InstanceLabel, transform,
                                (short)model.UserDefinedId);
                        }
                    }
                }
            }
            foreach (var mesh in meshDic.Values)
            {
                mesh.EndUpdate();
            }
            if (!tmpOpaquesGroup.Children.Any())
                return retScene;
            var mv = new ModelVisual3D { Content = tmpOpaquesGroup };
            opaqueShapes.Children.Add(mv);
            return retScene;
        }

        private LayerGroup GetLayerGroup(IPersistEntity ent)
        {
            var defaultRet = LayerGroup.Null;
            if (UseAmber)
                defaultRet = LayerGroup.Amber;

            var asset = _window.ResolveVerifiedAsset(ent);
            if (asset == null)
                return defaultRet;
            if (asset.Categories == null)
                return defaultRet;
            var cat = asset.Categories.FirstOrDefault(x => x.Classification == @"DPoW");
            if (cat == null)
                return defaultRet;

            switch (cat.Code)
            {
                case @"Passed":
                    return LayerGroup.Green;
                case @"Failed":
                    return LayerGroup.Red;
                default:
                    return defaultRet;
            }
        }

        void ILayerStyler.SetFederationEnvironment(IReferencedModel refModel)
        {
            
        }
    }
}
