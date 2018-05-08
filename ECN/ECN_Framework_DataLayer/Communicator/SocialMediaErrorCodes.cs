using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    public class SocialMediaErrorCodes
    {
        public static ECN_Framework_Entities.Communicator.SocialMediaErrorCodes GetByErrorCode(int errorCode, int mediaType, bool repostCodes)
        {
            SqlCommand cmd = new SqlCommand();
            if (repostCodes)
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "e_SocialMediaErrorCodes_Select_ByErrorCode";
                cmd.Parameters.AddWithValue("@errorCode", errorCode.ToString());
                cmd.Parameters.AddWithValue("@mediaType", mediaType.ToString());
                return Get(cmd);    
            }
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaErrorCodes_Select_ByErrorCode_LinkedIn";
            cmd.Parameters.AddWithValue("@errorCode", errorCode.ToString());
            cmd.Parameters.AddWithValue("@mediaType", mediaType.ToString());
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.SocialMediaErrorCodes Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SocialMediaErrorCodes retItem = new ECN_Framework_Entities.Communicator.SocialMediaErrorCodes();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SocialMediaErrorCodes();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes>.CreateBuilder(rdr);
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
        
        public static List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes> GetByMediaType(int mediaType)
        {
            SqlCommand cmd = new SqlCommand();
            if (mediaType != 3)
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "e_SocialMediaErrorCodes_Select_ByErrorCode";
                cmd.Parameters.AddWithValue("@mediaType", mediaType.ToString());
                return GetList(cmd);
            }
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaErrorCodes_Select_ByErrorCode_LinkedIn";
            cmd.Parameters.AddWithValue("@mediaType", mediaType.ToString());
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes> retList = new List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SocialMediaErrorCodes retItem = new ECN_Framework_Entities.Communicator.SocialMediaErrorCodes();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes>.CreateBuilder(rdr);
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
