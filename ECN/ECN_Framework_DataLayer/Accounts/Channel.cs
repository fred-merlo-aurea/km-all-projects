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
    public class Channel
    {
        public static List<ECN_Framework_Entities.Accounts.Channel> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.Channel> retItemList = new List<ECN_Framework_Entities.Accounts.Channel>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Channel_Select_All";
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.Channel GetByProductTypeAndID(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode productTypeCode, int baseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Channel> retItemList = new List<ECN_Framework_Entities.Accounts.Channel>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Channel_Select_ProductTypeCode_BaseChannelID";
            cmd.Parameters.AddWithValue("@ProductTypeCode", productTypeCode.ToString());
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID.ToString());
            return GetList(cmd).First();
        }

        
        public static List<ECN_Framework_Entities.Accounts.Channel> GetByBaseChannelID(int baseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Channel> retItemList = new List<ECN_Framework_Entities.Accounts.Channel>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Channel_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID.ToString());

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.Channel GetByChannelID(int ChannelID)
        {
            ECN_Framework_Entities.Accounts.Channel retItemList = new ECN_Framework_Entities.Accounts.Channel();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Channel_Select_ChannelID";
            cmd.Parameters.AddWithValue("@ChannelID", ChannelID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.Channel Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.Channel retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.Channel();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Channel>.CreateBuilder(rdr);
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


        private static List<ECN_Framework_Entities.Accounts.Channel> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.Channel> retList = new List<ECN_Framework_Entities.Accounts.Channel>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.Channel retItem = new ECN_Framework_Entities.Accounts.Channel();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Channel>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        if (retItem != null)
                        {
                            retItem.ChannelTypeCodeID = GetChannelTypeCode(retItem.ChannelTypeCode);

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

        private static ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode GetChannelTypeCode(string ChannelTypeCode)
        {
            return (ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode)Enum.Parse(typeof(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode), ChannelTypeCode);        
        }

        public static int Save(ECN_Framework_Entities.Accounts.Channel channel)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Channel_Save";
            cmd.Parameters.Add(new SqlParameter("@ChannelID", channel.ChannelID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", channel.BaseChannelID));
            cmd.Parameters.Add(new SqlParameter("@ChannelName", channel.ChannelName));
            cmd.Parameters.Add(new SqlParameter("@AssetsPath", channel.AssetsPath));
            cmd.Parameters.Add(new SqlParameter("@VirtualPath", channel.VirtualPath));
            cmd.Parameters.Add(new SqlParameter("@HeaderSource", channel.HeaderSource));
            cmd.Parameters.Add(new SqlParameter("@FooterSource", channel.FooterSource));
            cmd.Parameters.Add(new SqlParameter("@ChannelTypeCode", channel.ChannelTypeCode));
            cmd.Parameters.Add(new SqlParameter("@Active", channel.Active));
            cmd.Parameters.Add(new SqlParameter("@MailingIP", channel.MailingIP));
            cmd.Parameters.Add(new SqlParameter("@PickupPath", channel.PickupPath));
            if (channel.ChannelID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)channel.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)channel.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

    }
}
