using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class DynamicTag
    {
        public static int Save(ECN_Framework_Entities.Communicator.DynamicTag DynamicTag)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Save";
            cmd.Parameters.Add(new SqlParameter("@DynamicTagID", (object)DynamicTag.DynamicTagID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", DynamicTag.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@Tag", DynamicTag.Tag));
            cmd.Parameters.Add(new SqlParameter("@ContentID", DynamicTag.ContentID));
            if (DynamicTag.DynamicTagID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)DynamicTag.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)DynamicTag.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool Exists(string Tag, int CustomerID, int DynamicTagID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 DynamicTagID FROM DynamicTag  WITH (NOLOCK) WHERE Tag=@Tag and CustomerID = @CustomerID and IsDeleted = 0 and DynamicTagID<>@DynamicTagID) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@DynamicTagID", DynamicTagID);
            cmd.Parameters.AddWithValue("@Tag", Tag);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static void Delete(int DynamicTagID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Delete";
            cmd.Parameters.AddWithValue("@DynamicTagID", DynamicTagID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.DynamicTag> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }
        public static List<ECN_Framework_Entities.Communicator.DynamicTag> GetByContentID(int ContentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Select_ContentID";
            cmd.Parameters.AddWithValue("@ContentID", ContentID);
            return GetList(cmd);
        }
        public static ECN_Framework_Entities.Communicator.DynamicTag GetByDynamicTagID(int DynamicTagID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Select_DynamicTagID";
            cmd.Parameters.AddWithValue("@DynamicTagID", DynamicTagID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.DynamicTag Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.DynamicTag retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.DynamicTag();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DynamicTag>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.DynamicTag> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTag> retList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.DynamicTag retItem = new ECN_Framework_Entities.Communicator.DynamicTag();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DynamicTag>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static ECN_Framework_Entities.Communicator.DynamicTag GetByTag(string Tag, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Select_Tag";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Tag", Tag);
            return Get(cmd);
        }

        public static bool IsUsedInContent(string Tag, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 ContentID FROM [Content]  WITH (NOLOCK) WHERE CustomerID=@CustomerID and ContentSource like '%ECN.DynamicTag." + Tag + ".ECN.DynamicTag%' and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ContentUsedInDynamicTag(int ContentID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTag_Exists_ContentID";
            cmd.Parameters.AddWithValue("@ContentID", ContentID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
    }
}
