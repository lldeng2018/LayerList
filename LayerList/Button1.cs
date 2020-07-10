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
        protected override void OnClick()
        {
            string uriShp = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\Mainland.shp";
            //AddLayer(uriShp);
            GetLayers();
        }

        public void GetLayers()
        {
            //string FILE_NAME = @"W:\Ames\LayerFiles\coa_LAYERS_list_W.txt"; 
            string FILE_NAME = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\shpList.txt";
            MapView mv = MapView.Active;
            Map map = mv.Map;
            if (File.Exists(FILE_NAME))
            {

                string[] lines = File.ReadAllLines(FILE_NAME);//.Where(x =>!string.IsNullOrWhiteSpace(x));
                foreach (string line in lines)
                {                   
                    if (line.EndsWith(".lyr") || line.EndsWith(".shp"))
                    {
                        //Layer newLayer = LayerFactory.Instance.CreateLayer(new Uri(line), map);
                        //string[] path = line.Split(',');
                        if (line.Length >= 5 && line.Contains(","))
                        {
                            if (line.Split(',')[0]!= "LayerName")
                            {
                                AddLayer(line.Split(',')[1]);
                            }                                                          
                        }
                           
                    }
                    
                    //else
                    //{
                    //    MessageBox.Show(line + " is not a valid file, cannot add to map. ");
                    //    //return; 
                    //}
                }
            }
            else
            {

                string uriShp = @"C:\ArcGISWebApp\Ames_Sources\Shp_test\Mainland.shp";
                Layer lyr = LayerFactory.Instance.CreateLayer(new Uri(uriShp), map);
            }
        }

        public Task<Layer> AddLayer(string uri)
        {
            return QueuedTask.Run(() =>
            {
                Map map = MapView.Active.Map;
                return LayerFactory.Instance.CreateLayer(new Uri(uri), map);
            });
        }
    }
}
