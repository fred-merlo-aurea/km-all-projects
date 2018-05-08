using System;
using System.Linq;
using ServiceStack.Text;
using System.Data;
using System.IO;

namespace Core_AMS.Utilities
{
    public class JsonFunctions
    {
        public string ToJson<T>(T x)
        {
            string jsonString = JsonSerializer.SerializeToString<T>(x);
            return jsonString;
        }
        public T FromJson<T>(string jsonString)
        {
            var fromJson = JsonSerializer.DeserializeFromString<T>(jsonString);
            return fromJson;
        }        

        public DataTable CreateDataTableFromJsonFile(FileInfo file)
        {
            string json = "";
            StreamReader streamReader = new StreamReader(file.FullName, System.Text.Encoding.ASCII);
            json = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
            streamReader.Close();

            DataTable dtbl = new DataTable();
            dtbl = ConvertJSONToDataTable(json);
            
            return dtbl;
        }
        public int GetRecordCount(FileInfo file)
        {
            string json = "";
            StreamReader streamReader = new StreamReader(file.FullName, System.Text.Encoding.ASCII);
            json = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
            streamReader.Close();
            JsonArrayObjects aObj = JsonArrayObjects.Parse(json);

            return aObj.Count;
        }


        public DataTable ConvertJSONToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();

            #region Get Entire File in String
            string json = jsonString;
            #endregion

            #region Set Up to Get Columns and Rows
            JsonArrayObjects obj = JsonArrayObjects.Parse(json);

            #region Headers
            var firstRow = obj.First();
            int columnOrder = 0;
            foreach (var i in firstRow)
            {
                try
                {
                    if (!dt.Columns.Contains(i.Key.ToUpper()))
                    {
                        dt.Columns.Add(i.Key.ToString());
                        columnOrder++;
                    }
                }
                catch 
                {
                        
                }
            }
            #endregion
            #region Rows
            foreach (var rowData in obj)
            {
                try
                {
                    DataRow drData = dt.NewRow();                    

                    foreach (var item in rowData)
                    {
                        try
                        {
                            drData[item.Key.ToString()] = item.Value;
                        }
                        catch (Exception)
                        {
                                
                        }
                    }

                    dt.Rows.Add(drData);
                }
                catch
                {
                        
                }
            }
            #endregion
            #endregion

            return dt;
        }        
    }
}
