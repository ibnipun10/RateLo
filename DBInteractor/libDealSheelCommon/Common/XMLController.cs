using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace libDealSheelCommon.Common
{
    public class CXMLNode
    {
        public XMLNodeElements AppServer { get; set; }
        public XMLNodeElements DatabaseServer { get; set; }

        public XMLNodeElements FTPServer { get; set; }

    }

    public class XMLNodeElements
    {
        public string Server {get; set;}
        public string Port {get; set;}
        public string UserName { get; set; }
        public string Password {get; set;}
    }
    public class XMLController
    {
        public static CXMLNode PopulateXMLObject(string xmlFilePath)
        {
            CXMLNode objXmlNode = new CXMLNode();
            PropertyInfo[] props = objXmlNode.GetType().GetProperties();


            using(XmlReader xmlReader = XmlReader.Create(xmlFilePath))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {

                        foreach (PropertyInfo prop in props)
                        {
                            if (prop.Name.Equals(xmlReader.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                XMLNodeElements objXMlElemetns = new XMLNodeElements();

                                PropertyInfo[] elementsProp = objXMlElemetns.GetType().GetProperties();

                                foreach (PropertyInfo elemProp in elementsProp)
                                {
                                    string value = xmlReader.GetAttribute(elemProp.Name);
                                    elemProp.SetValue(objXMlElemetns, value, null);
                                    
                                }

                                prop.SetValue(objXmlNode, objXMlElemetns, null);
                                break;
                            }

                        }
                    }
                }
            }

            return objXmlNode;
        }
    }
}
