using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ProductSubscriptionDetail
    {
        public static List<Entity.ProductSubscriptionDetail> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionDetail_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.ProductSubscriptionDetail> Select(int pubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PubSubscriptionDetail_Select_PubSubscriptionID";
            cmd.Parameters.AddWithValue("@pubSubscriptionID", pubSubscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        public static bool DeleteCodeSheetID(KMPlatform.Object.ClientConnections client, int codeSheetID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PubSubscriptionDetail_Delete_CodeSheetID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static List<Entity.ProductSubscriptionDetail> SelectPaging(int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptionDetail_Select_ProductID_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ProductID", productID);

            return GetList(cmd);
        }

        public static int SelectCount(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptionDetail_PubID_Count";
            cmd.Parameters.AddWithValue("@ProductID", productID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Save(KMPlatform.Object.ClientConnections client, Entity.ProductSubscriptionDetail x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionDetail_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", x.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@CodeSheetID", x.CodeSheetID));            
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ResponseOther", x.ResponseOther));

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                return true;
            else
                return false;
        }

        public static List<Entity.ProductSubscriptionDetail> ProductSubscriptionDetailUpdateBulkSql(KMPlatform.Object.ClientConnections client, List<Entity.ProductSubscriptionDetail> list)
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (Entity.ProductSubscriptionDetail x in list)
            {
                sbXML.AppendLine("<ProductSubscriptionDetail>");

                sbXML.AppendLine("<PubSubscriptionID>" + x.PubSubscriptionID.ToString() + "</PubSubscriptionID>");
                sbXML.AppendLine("<SubscriptionID>" + x.SubscriptionID.ToString() + "</SubscriptionID>");
                sbXML.AppendLine("<CodeSheetID>" + x.CodeSheetID.ToString() + "</CodeSheetID>");
                sbXML.AppendLine("<DateCreated>" + x.DateCreated.ToString() + "</DateCreated>");
                sbXML.AppendLine("<DateUpdated>" + x.DateUpdated.ToString() + "</DateUpdated>");
                sbXML.AppendLine("<CreatedByUserID>" + x.CreatedByUserID.ToString() + "</CreatedByUserID>");
                sbXML.AppendLine("<UpdatedByUserID>" + x.UpdatedByUserID.ToString() + "</UpdatedByUserID>");
                if (x.ResponseOther != null)
                    sbXML.AppendLine("<ResponseOther>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.ResponseOther.ToString()) + "</ResponseOther>");
                else
                    sbXML.AppendLine("<ResponseOther></ResponseOther>");

                sbXML.AppendLine("</ProductSubscriptionDetail>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionDetail_BulkUpdate";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            //cmd.Connection = conn;

            //int rowCount = Convert.ToInt32(DataFunctions.ExecuteNonQuery(cmd));
            //if (rowCount > 0)
            //    done = true;
            //else
            //    done = false;

            return GetList(cmd);
        }
        public static Entity.ProductSubscriptionDetail Get(SqlCommand cmd)
        {
            Entity.ProductSubscriptionDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProductSubscriptionDetail();
                        DynamicBuilder<Entity.ProductSubscriptionDetail> builder = DynamicBuilder<Entity.ProductSubscriptionDetail>.CreateBuilder(rdr);
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
        public static List<Entity.ProductSubscriptionDetail> GetList(SqlCommand cmd)
        {
            List<Entity.ProductSubscriptionDetail> retList = new List<Entity.ProductSubscriptionDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ProductSubscriptionDetail retItem = new Entity.ProductSubscriptionDetail();
                        DynamicBuilder<Entity.ProductSubscriptionDetail> builder = DynamicBuilder<Entity.ProductSubscriptionDetail>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                //string name = "Status";
                                //int index = rdr.GetOrdinal(name);
                                //if (!rdr.IsDBNull(index))
                                //{
                                //    FrameworkUAD.BusinessLogic.Enums.EmailStatus es = FrameworkUAD.BusinessLogic.Enums.GetEmailStatus(rdr[index].ToString());
                                //    retItem.Status = es;
                                //}

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
