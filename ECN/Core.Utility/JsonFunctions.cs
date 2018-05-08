using System;
using System.Linq;
//using ServiceStack.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Core.Utilities
{
    public class JsonFunctions
    {
        public string ToJson<T>(T x)
        {
            throw new NotImplementedException(); // TODO
            //string jsonString = JsonSerializer.SerializeToString<T>(x);
            //return jsonString;
        }
        public T FromJson<T>(string jsonString)
        {
            throw new NotImplementedException(); // TODO
            //var fromJson = JsonSerializer.DeserializeFromString<T>(jsonString);
            //return fromJson;
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
            throw new NotImplementedException(); // TODO
            //string json = "";
            //StreamReader streamReader = new StreamReader(file.FullName, System.Text.Encoding.ASCII);
            //json = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
            //streamReader.Close();
            //JsonArrayObjects aObj = JsonArrayObjects.Parse(json);

            //return aObj.Count;
        }

        public DataTable ConvertJSONToDataTable(string jsonString)
        {
            throw new NotImplementedException(); // TODO
            //DataTable dt = new DataTable();

            //#region Get Entire File in String
            //string json = jsonString;
            //#endregion

            //#region Set Up to Get Columns and Rows
            //JsonArrayObjects obj = JsonArrayObjects.Parse(json);

            //#region Headers
            //var firstRow = obj.First();
            //int columnOrder = 0;
            //foreach (var i in firstRow)
            //{
            //    try
            //    {
            //        if (!dt.Columns.Contains(i.Key.ToUpper()))
            //        {
            //            dt.Columns.Add(i.Key.ToString());
            //            columnOrder++;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
                        
            //    }
            //}
            //#endregion
            //#region Rows
            //foreach (var rowData in obj)
            //{
            //    try
            //    {
            //        DataRow drData = dt.NewRow();                    

            //        foreach (var item in rowData)
            //        {
            //            try
            //            {
            //                drData[item.Key.ToString()] = item.Value;
            //            }
            //            catch (Exception ex)
            //            {
                                
            //            }
            //        }

            //        dt.Rows.Add(drData);
            //    }
            //    catch (Exception ex)
            //    {
                        
            //    }
            //}
            //#endregion
            //#endregion

            //return dt;
        }        
    }
}
