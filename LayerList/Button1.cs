using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
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

namespace LayerList
{
    internal class Button1 : Button
    {
        public string FILE_NAME = string.Empty;
        protected override void OnClick()
        {
            //string uriShp = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\Mainland.shp";
            //AddLayer(uriShp);
            AddLayersToMap addLayersToMap = AddLayersToMap.Current;
            addLayersToMap.button = this;
            GetLayers();
        }

        public void GetLayers()
        {
            //string FILE_NAME = @"W:\Ames\LayerFiles\coa_LAYERS_list_W.txt"; 
            if (FILE_NAME == "")
            {
                FILE_NAME = @"C:\Work\GIS\data\shpList1.txt";
            }
            if (File.Exists(FILE_NAME))
            {
                string[] lines = File.ReadAllLines(FILE_NAME);//.Where(x =>!string.IsNullOrWhiteSpace(x));
                foreach (string line in lines)
                {                   
                    if (line.EndsWith(".lyr") || line.EndsWith(".shp"))
                    {
                        if (line.Contains(","))
                        {
                            if (line.Split(',')[0] != "LayerName")
                            {
                                String filePath = line.Split(',')[1].Trim();
                                if (File.Exists(filePath))
                                {
                                    AddLayer(filePath);
                                }
                                else MessageBox.Show(filePath + " does not exist or is inaccessible.");

                            }
                            else
                            {
                                string fPath = line.Trim();
                                if (File.Exists(fPath))
                                {
                                    AddLayer(fPath);
                                }
                                else MessageBox.Show(string.Format("{0} doesn't exist or is not accessible", fPath));
                            }
                        }  
                    }
                }
            }
            else
            {
                MessageBox.Show(FILE_NAME + " is not accessible");
                this.Enabled = false;
                return;
            }
        }

        public Task<Layer> AddLayer(string uri)
        {
            return QueuedTask.Run(() =>
            {
                Map map = MapView.Active.Map;
                return LayerFactory.Instance.CreateLayer(new Uri(uri.Trim()), map);
            });
        }
    }
}
