using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Microsoft.Win32;
using ComboBox = ArcGIS.Desktop.Framework.Contracts.ComboBox;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;




namespace LayerList
{
    /// <summary>
    /// Represents the ComboBox
    /// </summary>
    internal class ComboBox_LayerList : ComboBox
    {
        Dictionary<string, string> layerNameAndPath = new Dictionary<string, string>();
        private bool _isInitialized;
        //private static string FILE_NAME = @"C:\Work\GIS\data\shpList.txt";
        /// <summary>
        /// Combo Box constructor
        /// </summary>
        public ComboBox_LayerList()
        {
            AddLayersToMap addLayersToMap = AddLayersToMap.Current;
            if (addLayersToMap == null) return;
            addLayersToMap.ComboBox_LayerList = this;
            UpdateCombo();
        }

        /// <summary>
        /// Updates the combo box with all the items.
        /// </summary>

        private void UpdateCombo()
        {
            // TODO – customize this method to populate the combobox with your desired items  
            if (_isInitialized)
                SelectedItem = ItemCollection.FirstOrDefault(); //set the default item in the comboBox


            if (!_isInitialized)
            {
                Clear();

                //Add items to the combobox
                string FILE_NAME = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\shpList.txt";

                //OpenFileDialog file_choose = new OpenFileDialog();

                //private void fileButton


                if (!File.Exists(FILE_NAME))
                {
                    MessageBox.Show(FILE_NAME+" cannot be found.");
                    return; 
                }
                string[] lines = File.ReadAllLines(FILE_NAME);
                foreach (string line in lines)
                {
                    if (line.Length>=5 && line.Contains(","))
                    {
                        string[] content = line.Split(',');
                        Add(new ComboBoxItem(content[0].Trim()));
                        layerNameAndPath[content[0].Trim()] = content[1].Trim();
                    }
                    
                }
                //for (int i = 0; i < 6; i++)
                //{
                //    string name = string.Format("Item {0}", i);
                //    Add(new ComboBoxItem(name));
                //}
                _isInitialized = true;
            }

            Enabled = true; //enables the ComboBox
            SelectedItem = ItemCollection.FirstOrDefault(); //set the default item in the comboBox

        }

        /// <summary>
        /// The on comboBox selection change event. 
        /// </summary>
        /// <param name="item">The newly selected combo box item</param>
        protected override void OnSelectionChange(ComboBoxItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.Text))
                return;
            //if (item == null)
            //    return;

            //if (string.IsNullOrEmpty(item.Text))
            //    return;

            // TODO  Code behavior when selection changes.   
            Button1 btn = new Button1();
            //btn.AddLayer(item.Text);

            if (item.Text != "LayerName")
            {
                if (File.Exists(layerNameAndPath[item.Text]))
                {
                    btn.AddLayer(layerNameAndPath[item.Text]);//get the path with dictionary key
                }
                else MessageBox.Show(layerNameAndPath[item.Text] + " not found or inaccessible.");
            }


            //string FILE_NAME = @"C:\Work\GIS\data\shpList.txt";
            //string[] lines = File.ReadAllLines(FILE_NAME);//.Where(x =>!string.IsNullOrWhiteSpace(x));
            //foreach (string line in lines)
            //{
            //    string[] content = line.Split(',');
            //    //Add(new ComboBoxItem(content[0]));
            //    if (item.Text != "LayerName" && item.Text == content[0])
            //    {
            //        btn.AddLayer(content[1]);
            //    }
            //}

        }

        //private String[] LayerNamePath()
        //{
        //    string[] content = new string[] { };
        //    string FILE_NAME = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\shpList.txt";
        //    string[] lines = File.ReadAllLines(FILE_NAME);//.Where(x =>!string.IsNullOrWhiteSpace(x));
        //    foreach (string line in lines)
        //    {
        //        content = line.Split(',');
        //        //Add(new ComboBoxItem(content[0]));
        //        return content; 

        //    }
        //    return content; 
        //}

    }
}
