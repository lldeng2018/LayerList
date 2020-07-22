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
        public Dictionary<string, string> layerNameAndPath = new Dictionary<string, string>();
        private bool _isInitialized;
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

        public void UpdateCombo()
        {
            // TODO – customize this method to populate the combobox with your desired items  
            if (_isInitialized)
                SelectedItem = ItemCollection.FirstOrDefault(); //set the default item in the comboBox


            if (!_isInitialized)
            {
                ClearLists();
                //Add items to the combobox
                //string FILE_NAME = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\shpList.txt";
                string FILE_NAME = @"W:\Ames\LayerFiles\coa_LAYERS_list_W.txt";

                if (File.Exists(FILE_NAME))
                {
                    readText(FILE_NAME);
                }
                else {
                    MessageBox.Show(FILE_NAME + " cannot be found. Please search for your file using the OpenTextFile button");
                    return;
                }
                _isInitialized = true;
            }
         
            Enabled = true; //enables the ComboBox
            SelectedItem = ItemCollection.FirstOrDefault(); //set the default item in the comboBox

        }

        public void OpenFile() {
            string filename = string.Empty;
            OpenItemDialog oid = new OpenItemDialog {
                Title = "Open a text file",
                Filter = ItemFilters.textFiles,
                MultiSelect = true
            };
            bool? ok = oid.ShowDialog();

            if (ok == true)
            {
                IEnumerable<Item> selected = oid.Items;

                filename = selected.First().Path;

                AddLayersToMap addLayersToMap = AddLayersToMap.Current;

                Button1 button = addLayersToMap.button; //// get the instance of the current one, do not create a new Button1
                //Button1 b1 = new Button1(); 
                if (button != null)
                {
                    button.FILE_NAME = filename;
                    if (button.Enabled == false)
                    {
                        button.Enabled = true;
                    }
                }
                readText(filename);
            }
            else
            {
                MessageBox.Show("No file opened");
                return;
            }

        }

        public void readText(string FILE_NAME)
        {
            ClearLists();
            if (FILE_NAME != "")
            {
                string[] lines = File.ReadAllLines(FILE_NAME);
                if (lines.ToList().Count() == 0)
                {
                    MessageBox.Show(FILE_NAME + " is empty.");
                    return;
                }
                foreach (string line in lines)
                {
                    if (line.Contains(','))
                    {
                        string[] content = line.Split(',');
                        string layerName = content[0].Trim();
                        Add(new ComboBoxItem(layerName));
                        layerNameAndPath[layerName] = content[1].Trim();
                    }
                    else
                    {
                        string FName = Path.GetFileName(line.Trim());
                        FName = Path.GetFileNameWithoutExtension(FName);
                        Add(new ComboBoxItem(FName));
                        layerNameAndPath[FName] = line.Trim();
                    }
                }
                MessageBox.Show(this.ItemCollection.Count() + " layers added to layer list from " + FILE_NAME);
                _isInitialized = true;
            }
            else {
                MessageBox.Show("No file found");
                return;
            }
        }

        /// <summary>
        /// The on comboBox selection change event. 
        /// </summary>
        /// <param name="item">The newly selected combo box item</param>
        protected override void OnSelectionChange(ComboBoxItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.Text))
                return;

            // TODO  Code behavior when selection changes.   
            AddLayersToMap addLayersToMap = AddLayersToMap.Current;

            Button1 btn = addLayersToMap.button ?? new Button1();

            if (item.Text != "LayerName")
            {
                if (File.Exists(layerNameAndPath[item.Text]))
                {
                    btn.AddLayer(layerNameAndPath[item.Text]);//get the path with dictionary key
                }
                else System.Windows.MessageBox.Show(string.Format("{0} doesn't exist or is not accessible", layerNameAndPath[item.Text]));
            }
        }
        private void ClearLists() {
            Clear(); //clear comboBox
            layerNameAndPath.Clear(); //clear dictionary
        }

    }
}
