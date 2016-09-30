using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace WinFormsXMLSerializer
{
    public class XmlSave
    {
        public static void SaveData(object IClass, string filename)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer((IClass.GetType()));
                using (StreamWriter writer = new StreamWriter(filename)) 
                    xmlSerializer.Serialize(writer, IClass);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class XmlLoad<T>
    {
        public static Type type;

        public XmlLoad()
        {
            type = typeof(T);
        }

        public T LoadData(string filename)
        {
            try
            {
                XmlSerializer xmlserializer = new XmlSerializer(type);
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    return (T)xmlserializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}