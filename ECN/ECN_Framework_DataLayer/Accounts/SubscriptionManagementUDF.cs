using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class SubscriptionManagementUDF
    {
        public static List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> GetBySMGID(int SMGID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementUDF_Select_SMGID";
            cmd.Parameters.AddWithValue("@SMGID", SMGID);

            return GetList(cmd);
        }

        public static void Delete(int SMGID, int UserID,int? SMGUDFID = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementUDF_Delete";
            cmd.Parameters.AddWithValue("@SMGID", SMGID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            if (SMGUDFID != null)
            {
                cmd.Parameters.AddWithValue("@SMGUDFID", SMGUDFID.Value);
            }

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }



        public static bool Exists(int SMID, int SMGID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementUDF_Exists_SMID_SMGID";
            cmd.Parameters.AddWithValue("@SMID", SMID);
            cmd.Parameters.AddWithValue("@SMGID", SMGID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static void Save(ECN_Framework_Entities.Accounts.SubsriptionManagementUDF smUDF)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementUDF_Save";

            cmd.Parameters.AddWithValue("@SMUDFID", smUDF.SubscriptionManagementUDFID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", smUDF.GroupDataFieldsID);
            cmd.Parameters.AddWithValue("@IsDeleted", smUDF.IsDeleted);
            cmd.Parameters.AddWithValue("@StaticValue", smUDF.StaticValue);
            cmd.Parameters.AddWithValue("@SubscriptionManagementGroupID", smUDF.SubscriptionManagementGroupID);

            if (smUDF.SubscriptionManagementUDFID > 0)
            {
                cmd.Parameters.AddWithValue("@UpdatedUserID", smUDF.UpdatedUserID);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedUserID", smUDF.CreatedUserID);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            }

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());

        }

        private static List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> retList = new List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.SubsriptionManagementUDF retItem = new ECN_Framework_Entities.Accounts.SubsriptionManagementUDF();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF>.CreateBuilder(rdr);
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
    }
}
