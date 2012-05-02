using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using Plupload.Net.Model;

namespace Plupload.Net.Utils
{
    /// <summary>
    /// a serialization util class for common serialization and deserialization operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XMLSerializer<T> where T : new()
    {
        /// <summary>
        /// deserialize an object/pluploaconfiguration from default configuration path
        /// </summary>
        /// <returns>a deserialized pluploadconfiguration</returns>
        public static T Load()
        {
            if (File.Exists(PluploadConfiguration.PluploadConfigPath))
                return Load(PluploadConfiguration.PluploadConfigPath);

            return default(T);
        }

        /// <summary>
        /// deserializes an object from the specific path given by configurationPath
        /// </summary>
        /// <param name="configurationPath">a path of the xml configuration</param>
        /// <returns>a deserialized object instance</returns>
        public static T Load(string configurationPath)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(configurationPath);

            return Load(xdoc.DocumentElement);
        }

        /// <summary>
        /// deserializes an object using the given xmlnod
        /// </summary>
        /// <param name="node">a node wich contains the xml to be deserialized</param>
        /// <returns>an deserialized instance of an object</returns>
        public static T Load(XmlNode node)
        {
            using (XmlReader xmlReader = new XmlNodeReader(node))
            {
                Type t = typeof(T);

                XmlSerializer ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(xmlReader);
            }
        }

        /// <summary>
        /// serializes an object to an xmlNode
        /// </summary>
        /// <param name="obj">an instance of an object</param>
        /// <returns>an xml node</returns>
        public static XmlNode ToXMLNode(object obj)
        {
            string xml = ToXML(obj, Encoding.UTF8);

            xml = xml.Replace("\r\n", "&#10;").Replace("\n", "&#10;");

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            return xdoc;
        }

        /// <summary>
        /// serialized an object instance to xml string using specific encoding
        /// </summary>
        /// <param name="obj">an instance of an object to be serialized</param>
        /// <param name="encoding">a specific encoding for serialization</param>
        /// <returns>a serialized xml string</returns>
        public static string ToXML(object obj, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings xws = new XmlWriterSettings()
                {
                    Encoding = encoding,
                    Indent = false,
                    OmitXmlDeclaration = true,
                    NamespaceHandling = NamespaceHandling.OmitDuplicates,
                    NewLineOnAttributes = true
                };

                using (XmlWriter xw = XmlWriter.Create(ms, xws))
                {
                    XmlSerializer ser = new XmlSerializer(obj.GetType(), string.Empty);
                    ser.Serialize(xw, obj);

                    string result = encoding.GetString(ms.ToArray());

                    return result.Substring(1);  //Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
                }
            }
        }
    }
}
