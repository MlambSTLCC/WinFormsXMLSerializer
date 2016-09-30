using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WinFormsXMLSerializer
{
    class DirectoryLocator
    {
        public DirectoryLocator()
        {
        }

        private string GetSerializeDirectory(string serializeSubDir = "db")
        {
            string fullDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (serializeSubDir.Length > 0)
                fullDir = Path.Combine(fullDir, serializeSubDir);

            if (!Directory.Exists(fullDir))
                Directory.CreateDirectory(fullDir);

            return fullDir;
        }
    }
}
