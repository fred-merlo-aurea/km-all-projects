using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastFields
    {
        private static string _CacheRegion = "BlastFields";
        public static bool Exists(int blastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 bf.BlastID FROM BlastFields bf with (nolock) join Blast b with (nolock) on bf.BlastID = b.BlastID WHERE bf.BlastID = @BlastID and b.CustomerID = @CustomerID and bf.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.BlastFields GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT b.blastID, b.CustomerID, bf.Field1, bf.Field2, bf.Field3, bf.Field4, bf.Field5, bf.CreatedDate, bf.CreatedUserID, bf.IsDeleted, bf.UpdatedDate, bf.UpdatedUserID FROM Blast b with (nolock)left outer join BlastFields bf with(nolock) on bf.BlastID = b.BlastID  and bf.IsDeleted = 0 WHERE b.BlastID =  @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            ECN_Framework_Entities.Communicator.BlastFields retItem = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                retItem = (ECN_Framework_Entities.Communicator.BlastFields)KM.Common.CacheUtil.GetFromCache(blastID.ToString(), _CacheRegion);
                if (retItem == null)
                {
                    retItem = Get(cmd);
                    if (retItem != null)
                        KM.Common.CacheUtil.AddToCache(blastID.ToString(), retItem, _CacheRegion);
                }

                
            }
            else
            {
                retItem = Get(cmd);
            }
            if (string.IsNullOrEmpty(retItem.Field1) && string.IsNullOrEmpty(retItem.Field2) && string.IsNullOrEmpty(retItem.Field3) && string.IsNullOrEmpty(retItem.Field4) && string.IsNullOrEmpty(retItem.Field5))
            {
                return null;
            }
            else
            {
                return retItem;
            }
        }

        //public static List<ECN_Framework_Entities.Communicator.BlastFields> GetByGroup(int groupID, int campaignItemID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_BlastFields_Select_ByGroup";
        //    cmd.Parameters.AddWithValue("@GroupID", groupID);
        //    cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
        //    return GetList(cmd);
        //}

        //public static List<ECN_Framework_Entities.Communicator.BlastFields> GetByType(string type, int campaignItemID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_BlastFields_Select_ByType";
        //    cmd.Parameters.AddWithValue("@BlastType", type);
        //    cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
        //    return GetList(cmd);
        //}

        //public static List<ECN_Framework_Entities.Communicator.BlastFields> GetByCampaignItemID(int campaignItemID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_BlastFields_Select_ByCampaignItemID";
        //    cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
        //    return GetList(cmd);
        //}

        private static ECN_Framework_Entities.Communicator.BlastFields Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastFields retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Communicator.BlastFields();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastFields>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.BlastFields> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastFields> retList = new List<ECN_Framework_Entities.Communicator.BlastFields>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastFields retItem = new ECN_Framework_Entities.Communicator.BlastFields();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastFields>.CreateBuilder(rdr);
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

        public static void Save(ECN_Framework_Entities.Communicator.BlastFields fields)
        {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_BlastFields_Save";
                cmd.Parameters.Add(new SqlParameter("@BlastID", fields.BlastID));
                cmd.Parameters.Add(new SqlParameter("@Field1", fields.Field1));
                cmd.Parameters.Add(new SqlParameter("@Field2", fields.Field2));
                cmd.Parameters.Add(new SqlParameter("@Field3", fields.Field3));
                cmd.Parameters.Add(new SqlParameter("@Field4", fields.Field4));
                cmd.Parameters.Add(new SqlParameter("@Field5", fields.Field5));
                cmd.Parameters.Add(new SqlParameter("@UpdatedUserID", (object)fields.UpdatedUserID ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CreatedUserID", (object)fields.CreatedUserID ?? DBNull.Value));

                DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int blastID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastFields_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
