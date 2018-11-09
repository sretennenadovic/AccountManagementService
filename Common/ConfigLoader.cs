using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public static class ConfigLoader
    {
        public static int Config(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            return Int32.Parse(doc.SelectSingleNode("Time").InnerText);
        }

        public static int[] ServerAnalizeParamsRead(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode xmlNode = doc.SelectSingleNode("params");

            int[] retVal = new int[3];

            retVal[0] = Int32.Parse(xmlNode.SelectSingleNode("A").InnerText);
            retVal[1] = Int32.Parse(xmlNode.SelectSingleNode("B").InnerText);
            retVal[2] = Int32.Parse(xmlNode.SelectSingleNode("C").InnerText);

            return retVal;
        }
    }
}
