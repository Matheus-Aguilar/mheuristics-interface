using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heuristics
{
    public class ExportData : HeuristicsBase
    {
        public static void Export(string filename)
        {
            try
            {
                var writer = new StreamWriter(filename, false, Encoding.ASCII);

                writer.Write(JsonConvert.SerializeObject(new HeuristicsBase()));

                writer.Close();
            }
            catch
            {
            }
        }
    }
}
