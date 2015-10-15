using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbim.FilterHelper;

namespace Xbim.Client
{
    public partial class PropertyMapDlg : Form
    {

        public COBiePropertyMapping  PropertyMaps { get; set; }
        public PropertyMapDlg(COBiePropertyMapping maps)
        {
            InitializeComponent();
            PropertyMaps = maps;
            SetMappings();
        }

        /// <summary>
        /// Set the tabs up
        /// </summary>
        private void SetMappings()
        {
            foreach (string item in PropertyMaps.SectionKeys)
            {
                string removeStr = "PropertyMaps";
                TabPage page = new System.Windows.Forms.TabPage(item.Substring(0, item.Length - removeStr.Length ));
                page.Name = item;
                page.Controls.Add(new PropertyMapTab(GetPaths(item)));
                tabControl.TabPages.Add(page);
            }
        }

        /// <summary>
        /// Get the data specific to the section required
        /// </summary>
        /// <param name="sectionKey">string, section key</param>
        /// <returns>List of AttributePaths</returns>
        private List<AttributePaths> GetPaths(string sectionKey)
        {
            switch (sectionKey)
            {
                case "CommonPropertyMaps":
                    return PropertyMaps.CommonPaths;
                case "SparePropertyMaps":
                    return PropertyMaps.SparePaths;
                case "SpacePropertyMaps":
                    return PropertyMaps.SpacePaths;
                case "FloorPropertyMaps":
                    return PropertyMaps.FloorPaths;
                case "AssetPropertyMaps":
                    return PropertyMaps.AssetPaths;
                case "AssetTypePropertyMaps":
                    return PropertyMaps.AssetTypePaths;
                case "SystemPropertyMaps":
                    return PropertyMaps.PSetsAsSystem;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (TabPage item in tabControl.TabPages)
            {
                var tabName = item.Name;
                var propMapTabCtr = item.Controls[0];
                if (propMapTabCtr is PropertyMapTab)
                {
                    var mapsTab = (PropertyMapTab)propMapTabCtr;
                    switch (tabName)
                    {
                        case "CommonPropertyMaps":
                            PropertyMaps.CommonPaths = mapsTab.PropPaths;
                            break;
                        case "SparePropertyMaps":
                           PropertyMaps.SparePaths = mapsTab.PropPaths;
                            break;                         
                        case "SpacePropertyMaps":
                            PropertyMaps.SpacePaths = mapsTab.PropPaths;
                            break;
                        case "FloorPropertyMaps":
                            PropertyMaps.FloorPaths = mapsTab.PropPaths;
                            break;
                        case "AssetPropertyMaps":
                            PropertyMaps.AssetPaths = mapsTab.PropPaths;
                            break;
                        case "AssetTypePropertyMaps":
                            PropertyMaps.AssetTypePaths = mapsTab.PropPaths;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Reset to defaults
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            var filename = PropertyMaps.ConfigFile; //save config file name before we set temp file name
            FileInfo tempConfig = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp");
            ((COBieLiteGeneratorDlg)this.Owner).CreateDefaultAppConfig(tempConfig);

            if (tempConfig.Exists)
            {
                PropertyMaps = new COBiePropertyMapping(tempConfig);
                PropertyMaps.ConfigFile = filename; //set back to correct file
                foreach (TabPage item in tabControl.TabPages)
                {
                    var propMapTabCtr = item.Controls[0];
                    if (propMapTabCtr is PropertyMapTab)
                    {
                        ((PropertyMapTab)propMapTabCtr).ReSet(GetPaths(item.Name));
                    }
                }
                tempConfig.Delete();
            }
            
        }
    }
}
