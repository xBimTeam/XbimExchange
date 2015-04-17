using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace COBieLiteUK.Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new[]
            {
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Ext01.fixed.xlsx",
                @"C:\Users\mxfm2\Downloads\Bad Cobie\Ext01.xlsx",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Ext01.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Struc.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Site.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\INT02.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Int01.xls"
            };
            foreach (var file in files)
            {
                var dir = Path.GetDirectoryName(file);
                var name = Path.GetFileNameWithoutExtension(file);
                var newFile = Path.Combine(dir ?? "", name + ".fixed.xlsx");
                Debug.WriteLine(name ?? "");

                using (var log = File.CreateText(Path.Combine(dir ?? "", name + ".fixed.txt")))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    string msg;
                    var facility = Facility.ReadCobie(file, out msg);
                    stopWatch.Stop();

                    if (!String.IsNullOrEmpty(msg))
                        log.WriteLine(msg);
                    Debug.WriteLine("Reading COBie: " + stopWatch.ElapsedMilliseconds);

                    stopWatch.Reset();
                    stopWatch.Start();
                    //facility.ValidateUK2012(log, true);
                    stopWatch.Stop();

                    Debug.WriteLine("Validating COBie: " + stopWatch.ElapsedMilliseconds);


                    stopWatch.Reset();
                    stopWatch.Start();
                    //Debug.Write(msg);
                    //Debug.Write(log.ToString());    
                    facility.WriteCobie(newFile, out  msg);
                    stopWatch.Stop();
                    if (!String.IsNullOrEmpty(msg))
                        log.WriteLine(msg);
                    Debug.WriteLine("Writing COBie: " + stopWatch.ElapsedMilliseconds);
                    log.Close();
                }
            }
        }
    }
}
