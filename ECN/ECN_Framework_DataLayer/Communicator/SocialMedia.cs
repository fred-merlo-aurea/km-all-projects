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
    public class SocialMedia
    {
        public static bool Exists(int socialMediaID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 SocialMediaID FROM SocialMedia WHERE SocialMediaID = @SocialMediaID AND canpublish = 'true' and isactive = 'true') SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@SocialMediaID", socialMediaID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.SocialMedia GetSocialMediaByID(int socialMediaID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from socialmedia where socialmediaid = @socialmediaid";
            cmd.Parameters.AddWithValue("@SocialMediaID", socialMediaID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMedia> GetSocialMediaCanShare()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from socialmedia where canshare = 'true' and isactive = 'true'";
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMedia> GetSocialMediaCanPublish()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from socialmedia where canpublish = 'true' and isactive = 'true'";
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.SocialMedia Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SocialMedia retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SocialMedia();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SocialMedia>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.SocialMedia> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SocialMedia> retList = new List<ECN_Framework_Entities.Communicator.SocialMedia>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SocialMedia retItem = new ECN_Framework_Entities.Communicator.SocialMedia();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SocialMedia>.CreateBuilder(rdr);
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
