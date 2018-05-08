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
    public class BaseChannel
    {
        private static string _CacheRegion = "Basechannel";

        public static List<ECN_Framework_Entities.Accounts.BaseChannel> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> retItem = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BaseChannel_Select";

            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                retItem = (List<ECN_Framework_Entities.Accounts.BaseChannel>)KM.Common.CacheUtil.GetFromCache("ALL", _CacheRegion);
                if (retItem == null)
                {
                    retItem = GetList(cmd);
                    KM.Common.CacheUtil.AddToCache("ALL", retItem, _CacheRegion);
                }
            }
            else
                retItem = GetList(cmd);

            return retItem;
        }

        public static bool Exists(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BaseChannel_Exists_ByID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string Name, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BaseChannel_Exists_ByName";
            cmd.Parameters.AddWithValue("@BaseChannelName", Name);
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        private static List<ECN_Framework_Entities.Accounts.BaseChannel> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> retList = new List<ECN_Framework_Entities.Accounts.BaseChannel>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.BaseChannel retItem = new ECN_Framework_Entities.Accounts.BaseChannel();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.BaseChannel>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            string sal = rdr["Salutation"].ToString();
                            retItem.Salutation = sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr : sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms : ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
                            retItem.ChannelTypeCode = GetChannelTypeCode(rdr["ChannelType"].ToString());

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

        private static ECN_Framework_Common.Objects.Accounts.Enums.ChannelType GetChannelTypeCode(string channelType)
        {
            ECN_Framework_Common.Objects.Accounts.Enums.ChannelType returnType;
            switch (channelType.Trim().ToLower())
            {
                case "charity":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Charity;
                    break;
                case "marketing":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Marketing;
                    break;
                case "other":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Other;
                    break;
                case "publishing":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Publishing;
                    break;
                case "Retail":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Retail;
                    break;
                case "tradeshow":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Tradeshow;
                    break;
                default:
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Unknown;
                    break;
            }
            return returnType;
        }


        public static int Save(ECN_Framework_Entities.Accounts.BaseChannel basechannel)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BaseChannel_Save";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", basechannel.BaseChannelID));
            cmd.Parameters.Add(new SqlParameter("@PlatformClientGroupID", basechannel.PlatformClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelName", basechannel.BaseChannelName));
            cmd.Parameters.Add(new SqlParameter("@ChannelPartnerType", (int)basechannel.ChannelPartnerType));
            cmd.Parameters.Add(new SqlParameter("@RatesXML", basechannel.RatesXml));
            cmd.Parameters.Add(new SqlParameter("@Salutation", basechannel.Salutation.ToString()));
            cmd.Parameters.Add(new SqlParameter("@ContactName", basechannel.ContactName)); //Contact.FirstName + " " + basechannel.Contact.LastName
            cmd.Parameters.Add(new SqlParameter("@ContactTitle", basechannel.ContactTitle));
            cmd.Parameters.Add(new SqlParameter("@Phone", basechannel.Phone));
            cmd.Parameters.Add(new SqlParameter("@Fax", basechannel.Fax));
            cmd.Parameters.Add(new SqlParameter("@Email", basechannel.Email));
            cmd.Parameters.Add(new SqlParameter("@Address", basechannel.Address));
            cmd.Parameters.Add(new SqlParameter("@City", basechannel.City));
            cmd.Parameters.Add(new SqlParameter("@State", basechannel.State));
            cmd.Parameters.Add(new SqlParameter("@Country", basechannel.Country));
            cmd.Parameters.Add(new SqlParameter("@Zip", basechannel.Zip));
            cmd.Parameters.Add(new SqlParameter("@BounceDomain", basechannel.BounceDomain));
            cmd.Parameters.Add(new SqlParameter("@EmailThreshold", basechannel.EmailThreshold));
            cmd.Parameters.Add(new SqlParameter("@ChannelURL", basechannel.ChannelURL));
            cmd.Parameters.Add(new SqlParameter("@WebAddress", basechannel.WebAddress));
            cmd.Parameters.Add(new SqlParameter("@ChannelType", basechannel.ChannelType));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelGuid", (object)basechannel.BaseChannelGuid ?? DBNull.Value));
            if (basechannel.MSCustomerID != null)
                cmd.Parameters.Add(new SqlParameter("@MSCustomerID", basechannel.MSCustomerID));
            if (basechannel.BaseChannelID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)basechannel.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)basechannel.CreatedUserID ?? DBNull.Value));


            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }


        public static void Delete(int basechannelID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BaseChannel_Delete";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", basechannelID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

         public static ECN_Framework_Entities.Accounts.BaseChannel GetByDomain(string SubDomain)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BaseChannel_Select_SubDomain";
            cmd.Parameters.Add(new SqlParameter("@SubDomain", SubDomain));
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.BaseChannel Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.BaseChannel retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.BaseChannel();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.BaseChannel>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        
                        retItem = builder.Build(rdr);
                        string sal = rdr["Salutation"].ToString();
                        retItem.Salutation = sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr : sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms : ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
    }
}
