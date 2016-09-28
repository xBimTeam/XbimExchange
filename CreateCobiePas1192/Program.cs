using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.CobieLiteUk;
using Xbim.Ifc;
using XbimExchanger.IfcToCOBieLiteUK;

namespace CreateCobiePas1192
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var m = IfcStore.Open("Lakeside_Restaurant.ifc"))
            {               
                var facilities = new List<Facility>();
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities);
                facilities = ifcToCoBieLiteUkExchanger.Convert();

                var facilityType = facilities.FirstOrDefault();
                if(facilityType!=null)
                {
                    //write the cobie data in json format
                    facilityType.WriteJson("Cobie.json", true);
                    //write the cobie data in xml format
                    facilityType.WriteXml("Cobie.xml", true);
                    //write the cobie data in spreadsheet format                 
                    string errMsg;
                    facilityType.WriteCobie("Cobie.xls", out errMsg);
                }
                   
                  
            }

        }
    }
}
