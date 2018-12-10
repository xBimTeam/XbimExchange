using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Ifc;
using XbimExchanger.IfcToCOBieLiteUK;

namespace CreateCobiePas1192
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            using (var m = IfcStore.Open("Lakeside_Restaurant.ifc"))
            {               
                var facilities = new List<Facility>();
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities);
                facilities = ifcToCoBieLiteUkExchanger.Convert();

                var facilityType = facilities.FirstOrDefault();
                if (facilityType == null)
                    return;
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