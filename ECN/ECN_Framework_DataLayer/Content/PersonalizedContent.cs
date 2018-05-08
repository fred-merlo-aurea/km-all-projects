using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace ECN_Framework_DataLayer.Content
{
    public class PersonalizedContent
    {
        public static void UpdateProcessed(ECN_Framework_Entities.Content.PersonalizedContent pc)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_Update_Processed";

            cmd.Parameters.AddWithValue("@PersonalizedContentID", pc.PersonalizedContentID);
            cmd.Parameters.AddWithValue("@HTMLContent", pc.HTMLContent);
            cmd.Parameters.AddWithValue("@TEXTContent", pc.TEXTContent);
            cmd.Parameters.AddWithValue("@IsProcessed", pc.IsProcessed);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Content.ToString()).ToString();
        }

        public static ECN_Framework_Entities.Content.PersonalizedContent GetByBlastID_EmailAddress(Int64 BlastID, string EmailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_Select_BlastID_EmailAddress";
            cmd.Parameters.Add(new SqlParameter("@BlastID", BlastID));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", EmailAddress));
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Content.PersonalizedContent GetByPersonalizedContentID(Int64 PersonalizedContentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_Select_PersonalizedContentID";
            cmd.Parameters.Add(new SqlParameter("@PersonalizedContentID", PersonalizedContentID));
            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Content.PersonalizedContent pc)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_Save";
            cmd.Parameters.Add(new SqlParameter("@PersonalizedContentID", pc.PersonalizedContentID));
            cmd.Parameters.AddWithValue("@BlastID", pc.BlastID);
            cmd.Parameters.AddWithValue("@EmailAddress", pc.EmailAddress);
            cmd.Parameters.AddWithValue("@EmailSubject", pc.EmailSubject);
            cmd.Parameters.AddWithValue("@HTMLContent", pc.HTMLContent);
            cmd.Parameters.AddWithValue("@TEXTContent", pc.TEXTContent);
            cmd.Parameters.AddWithValue("@IsValid", pc.IsValid);
            cmd.Parameters.AddWithValue("@IsProcessed", pc.IsProcessed);
            cmd.Parameters.AddWithValue("@IsDeleted", pc.IsDeleted);
            if (pc.PersonalizedContentID > 0)
                cmd.Parameters.AddWithValue("@UpdatedUserID", pc.UpdatedUserID);
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", pc.CreatedUserID);
            
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Content.ToString()).ToString());
        }

        public static void MarkAsFailed(long pcID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_MarkAsFailed";
            cmd.Parameters.Add(new SqlParameter("@PersonalizedContentID", pcID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Content.ToString());
        }

        public static Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> GetNotProcessed()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_Select_Top100_NonProcessed";

            return GetDictionary(cmd);
        }

        public static Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> GetDictionaryByPersonalizedContentIDs(string xmlPersonalizedContentIDs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PersonalizedContent_Select_PersonalizedContentIDs";
            cmd.Parameters.Add(new SqlParameter("@PersonalizedContentIDs", xmlPersonalizedContentIDs));

            return GetDictionary(cmd);
        }

        private static Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> GetDictionary(SqlCommand cmd)
        {
            Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> retDictionary = null;
            ECN_Framework_Entities.Content.PersonalizedContent pc = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Content.ToString()))
            {
                if (rdr != null)
                {
                    pc = new ECN_Framework_Entities.Content.PersonalizedContent();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Content.PersonalizedContent>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        pc = builder.Build(rdr);

                        if (retDictionary == null)
                            retDictionary = new Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent>();

                        retDictionary.Add(pc.PersonalizedContentID, pc);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retDictionary;
        }

        

        private static ECN_Framework_Entities.Content.PersonalizedContent Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Content.PersonalizedContent retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Content.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Content.PersonalizedContent();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Content.PersonalizedContent>.CreateBuilder(rdr);
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

    }
}
