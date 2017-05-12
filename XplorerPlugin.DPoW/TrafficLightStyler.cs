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
            var matRed = new WpfMaterial();
            matRed.CreateMaterial(col);
            var mg = new WpfMeshGeometry3D(matRed, matRed);
            return mg;
        }

        public TrafficLightStyler(IModel model, MainWindow window)
        {
            _window = window;
            _model = model;
            UseAmber = false;
            UseBlue = true;
        }
        
        XbimColour _colourPass = new XbimColour("Green", 0.0, 1.0, 0.0, 0.5);
        XbimColour _colourFail = new XbimColour("Red", 1.0, 0.0, 0.0, 0.5);
        XbimColour _colourWarning = new XbimColour("Amber", 1.0, 0.64, 0.0, 0.5);
        XbimColour _colourNa = new XbimColour("Blue", 0.0, 0.0, 1.0, 0.5);

        XbimScene<WpfMeshGeometry3D, WpfMaterial> ILayerStyler.BuildScene(IModel model, XbimMatrix3D modelTransform, ModelVisual3D opaqueShapes, ModelVisual3D transparentShapes, List<Type> exclude)
        {
            var excludedTypes = model.DefaultExclusions(exclude);
            var tmpOpaquesGroup = new Model3DGroup();
            var retScene = new XbimScene<WpfMeshGeometry3D, WpfMaterial>(model);
            var meshes = new List<WpfMeshGeometry3D>();
            
            if (_model.Instances == null || !_model.Instances.Any())
                return retScene;

            var red = PrepareMesh(_colourFail); meshes.Add(red);
            var green = PrepareMesh(_colourPass); meshes.Add(green);
            var blue = PrepareMesh(_colourNa); meshes.Add(blue);
            var amber = PrepareMesh(_colourWarning); meshes.Add(amber);

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
                        if (!UseBlue && grp == LayerGroup.Blue)
                            continue;
                        if (!UseAmber && grp == LayerGroup.Amber)
                            continue;

                        WpfMeshGeometry3D targetMergeMesh = null;

                        switch (grp)
                        {
                            case LayerGroup.Red:
                                targetMergeMesh = red;
                                break;
                            case LayerGroup.Green:
                                targetMergeMesh = green;
                                break;
                            case LayerGroup.Blue:
                                targetMergeMesh = blue;
                                break;
                            case LayerGroup.Amber:
                                targetMergeMesh = amber;
                                break;
                        }
                        if (targetMergeMesh == null)
                            continue;

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
            foreach (var mesh in meshes)
            {
                mesh.EndUpdate();
            }
            if (!tmpOpaquesGroup.Children.Any())
                return retScene;
            var mv = new ModelVisual3D { Content = tmpOpaquesGroup };
            opaqueShapes.Children.Add(mv);
            // no transparents are present
            //if (tmpTransparentsGroup.Children.Any())
            //{
            //    var mv = new ModelVisual3D { Content = tmpTransparentsGroup };
            //    transparentShapes.Children.Add(mv);
            //}
            return retScene;
        }

        private LayerGroup GetLayerGroup(IPersistEntity ent)
        {
            var asset = _window.ResolveAsset(ent);
            if (asset == null)
                return LayerGroup.Null;
            if (asset.Categories == null)
                return LayerGroup.Null;
            var cat = asset.Categories.FirstOrDefault(x => x.Classification == @"DPoW");
            if (cat == null)
                return LayerGroup.Null;

            switch (cat.Code)
            {
                case @"Passed":
                    return LayerGroup.Green;
                case @"Failed":
                    return LayerGroup.Red;
                default:
                    return LayerGroup.Blue;
            }
        }

        public void SetColors(XbimColour pass, XbimColour fail, XbimColour warning, XbimColour nonApplicable)
        {
            _colourPass = pass;
            _colourFail = fail;
            _colourWarning = warning;
            _colourNa = nonApplicable;
        }

        void ILayerStyler.SetFederationEnvironment(IReferencedModel refModel)
        {
            
        }
    }
}
