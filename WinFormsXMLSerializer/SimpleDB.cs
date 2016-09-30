using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.IO;

namespace WinFormsXMLSerializer
{
    class SimpleDB
    {
        const string DATATABLE_DIR = "db";

        public string FilePath { get; set; }
        public DataSet Ds { get; set; }

        public SimpleDB(DataSet ds = null, string p_filePath = "")
        {
            this.Ds = null;

            if (ds != null)
                this.Ds = ds;

            string exeLoc = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exeLoc);
            string fileData = Path.GetFileNameWithoutExtension(exeLoc) + "_Data.xml";

            if (p_filePath.Length > 0)
                fileData = p_filePath;

            this.FilePath = Path.Combine(exeDir, fileData);
        }

        public void Save()
        {
            try
            {
                using (this.Ds)
                    Ds.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Load()
        {
            // save the empty dataset the first time
            if (!File.Exists(this.FilePath))
                this.Save();

            try
            {
                Ds.ReadXml(FilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveTable(DataTable dt)
        {
            string saveTableFile = FilterTableName(dt.TableName);
            string saveFilePath = Path.Combine(GetDatatableDirectory(), saveTableFile) + ".xml";

            try
            {
                using (dt)
                    dt.WriteXml(saveFilePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadTable(ref DataTable dt)
        {
            string loadTableFile = FilterTableName(dt.TableName);
            string loadFilePath = Path.Combine(GetDatatableDirectory(), loadTableFile) + ".xml";

            // save the empty datatable the first time
            if (!File.Exists(loadFilePath))
                this.SaveTable(dt);

            try
            {
                dt.ReadXml(loadFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string FilterTableName(string tableName)
        {
            const char REPLACE_KAR = '_';
            var filteredName = new StringBuilder();

            foreach (char c in tableName.ToCharArray())
                filteredName.Append(Char.IsLetterOrDigit(c) ? c : REPLACE_KAR);

            return filteredName.ToString().ToLower();
        }

        private string GetDatatableDirectory()
        {
            string exeLoc = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exeLoc);

            string dtDir = Path.Combine(exeDir, DATATABLE_DIR);

            if (!Directory.Exists(dtDir))
                Directory.CreateDirectory(dtDir);

            return dtDir;
        }

        public int Insert(ref DataTable dt, Dictionary<string, object> dictValues)
        {
            var insertRow = dt.Rows.Add();

            // Will not error out if column name <> dictionary property value:
            foreach (DataColumn dc in dt.Columns)
                if (dictValues.ContainsKey(dc.ColumnName))
                    insertRow[dc.ColumnName] = dictValues[dc.ColumnName];

            return (int)insertRow[0];
        }

        public void Update(ref DataTable dt, Dictionary<string, object> dictValues)
        {
            var keyColumnName = dt.Columns[0].ColumnName;
            if (!dictValues.ContainsKey(keyColumnName))
                return;

            var updateRow = dt.Rows.Find(new object[] { (int)dictValues[keyColumnName] });

            if (updateRow == null)
                return;

            // Will not error out if column name <> dictionary property value:
            foreach (DataColumn dc in dt.Columns)
                if (dictValues.ContainsKey(dc.ColumnName)
                    && dc.ColumnName != keyColumnName)  // don't update key!
                    updateRow[dc.ColumnName] = dictValues[dc.ColumnName];
        }

        public void Delete(ref DataTable dt, int id)
        {
            var deleteRow = dt.Rows.Find(id);

            if (deleteRow != null)
                dt.Rows.Remove(deleteRow);
        }

        public DataTable CreateIdentityColumn(ref DataTable dt)
        {
            DataColumn keyColumn = dt.Columns.Add("Id", typeof(Int32));
            keyColumn.AutoIncrement = true;
            keyColumn.AutoIncrementSeed = 1;
            keyColumn.AutoIncrementStep = 1;
            keyColumn.ReadOnly = true;

            // Set the primary key to the identity column
            dt.PrimaryKey = new DataColumn[] { keyColumn };

            return dt;
        }
    }
}
