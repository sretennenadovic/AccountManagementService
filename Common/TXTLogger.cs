using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class TXTLogger
    {
        public static void LogData(string message)
        {
            var parts = message.Split(',');

            if (!File.Exists("AuditClientTXTLog"))
            {
                TextWriter tw = new StreamWriter("AuditClientTXTLog");
                tw.WriteLine(string.Format("Name={0},Time={1},Method={2},AccountNumber={3},Sum={4},Status={5}", parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]));
                tw.Close();
            }
            else if (File.Exists("AuditClientTXTLog"))
            {
                using (var tw = new StreamWriter("AuditClientTXTLog", true))
                {
                    tw.WriteLine(string.Format("Name={0},Time={1},Method={2},AccountNumber={3},Sum={4},Status={5}", parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]));
                }
            }
        }
    }
}
