using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace WinFormsXMLSerializer
{
    public class XMLSerializer 
    {
        const string XML_FOLDER = "xml";

        public string XMLPath { get; set; }
        
        public XMLSerializer(string xmlFile) 
        {
            if (xmlFile == string.Empty)
                throw new Exception("Please specify a name for data.");
                      
            if (!xmlFile.Contains("."))
                xmlFile += ".xml";
            
            this.XMLPath = Path.Combine(GetXMLDirectory(), xmlFile);
        }
        
        public void SaveData(object IClass) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer((IClass.GetType()));

            using (StreamWriter writer = new StreamWriter(this.XMLPath)) 
                xmlSerializer.Serialize(writer, IClass);
        }

        public T LoadData<T>()
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(this.XMLPath, FileMode.Open,
                FileAccess.Read, FileShare.Read))

            return (T)xmlserializer.Deserialize(fs);
        }

        private string GetXMLDirectory()
        {
            string exeLoc = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exeLoc);
            string xmlDir = Path.Combine(exeDir, XML_FOLDER);

            if (!Directory.Exists(xmlDir))
                Directory.CreateDirectory(xmlDir);

            return xmlDir;
        }
    }
}