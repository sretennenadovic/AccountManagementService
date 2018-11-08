using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Common
{
    public static class XMLLogger
    {
        public static void LogData(string message)
        {

            var parts = message.Split(',');
            if (!File.Exists("AuditClientXMLLog"))
            {
                using (XmlWriter writter = XmlWriter.Create("AuditClientXMLLog"))
                {

                    writter.WriteStartDocument();
                    writter.WriteStartElement("Logs");
                    writter.WriteStartElement("Log");

                    writter.WriteElementString("Name", parts[0]);
                    writter.WriteElementString("Time", parts[1]);
                    writter.WriteElementString("Method", parts[2]);
                    writter.WriteElementString("AccountNumber", parts[3]);
                    writter.WriteElementString("Sum", parts[4]);
                    writter.WriteElementString("Status", parts[5]);

                    writter.WriteEndElement();
                    writter.WriteEndElement();
                    writter.WriteEndDocument();
                    writter.Flush();
                    writter.Close();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load("AuditClientXMLLog");
                XElement root = xDocument.Element("Logs");
                IEnumerable<XElement> rows = root.Descendants("Log");
                XElement lastRow = rows.Last();
                lastRow.AddAfterSelf(
                   new XElement("Log",
                   new XElement("Name", parts[0]),
                   new XElement("Time", parts[1]),
                   new XElement("Method", parts[2]),
                   new XElement("AccountNumber", parts[3]),
                   new XElement("Sum", parts[4]),
                   new XElement("Status", parts[5])));
                xDocument.Save("AuditClientXMLLog");
            }
        }
    }
}
