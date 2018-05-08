using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class CustomField
    {
        private const string ClassName = "FrameworkSubGen.DataAccess.CustomField";
        private const string CommandTextSaveBulkXml = "e_CustomField_SaveBulkXml";

        public static bool SaveBulkXml(string xml)
        {
            return DataAccessBase.SaveBulkXml(xml, CommandTextSaveBulkXml, ClassName);
        }

        public static List<Entity.CustomField> Select(int account_id)
        {
            List<Entity.CustomField> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomField_Select";
            cmd.Parameters.AddWithValue("@account_id", account_id);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.CustomField Get(SqlCommand cmd)
        {
            Entity.CustomField retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.CustomField();
                        DynamicBuilder<Entity.CustomField> builder = DynamicBuilder<Entity.CustomField>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.CustomField> GetList(SqlCommand cmd)
        {
            List<Entity.CustomField> retList = new List<Entity.CustomField>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CustomField retItem = new Entity.CustomField();
                        DynamicBuilder<Entity.CustomField> builder = DynamicBuilder<Entity.CustomField>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                if (rdr["type"].ToString() == Entity.Enums.HtmlFieldType.checkbox.ToString())
                                    retItem.type = Entity.Enums.HtmlFieldType.checkbox;
                                else if (rdr["type"].ToString() == Entity.Enums.HtmlFieldType.radio.ToString())
                                    retItem.type = Entity.Enums.HtmlFieldType.radio;
                                else if (rdr["type"].ToString() == Entity.Enums.HtmlFieldType.select.ToString())
                                    retItem.type = Entity.Enums.HtmlFieldType.select;
                                else if (rdr["type"].ToString() == Entity.Enums.HtmlFieldType.text.ToString())
                                    retItem.type = Entity.Enums.HtmlFieldType.text;
                                else if (rdr["type"].ToString() == Entity.Enums.HtmlFieldType.textarea.ToString())
                                    retItem.type = Entity.Enums.HtmlFieldType.textarea;

                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
    }
}
