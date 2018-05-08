using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    [Serializable]
    public class PublicationSequence
    {
        public static List<Entity.PublicationSequence> SelectPublisher(int publisherID)
        {
            List<Entity.PublicationSequence> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PublicationSequence_Select_PublisherID";
            cmd.Parameters.AddWithValue("@PublisherID", publisherID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.PublicationSequence> SelectPublicationID(int publicationID)
        {
            List<Entity.PublicationSequence> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PublicationSequence_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static int GetNextSequenceID(int publicationID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PublicationSequence_Select_NextSeqID_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        private static Entity.PublicationSequence Get(SqlCommand cmd)
        {
            Entity.PublicationSequence retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.PublicationSequence();
                        DynamicBuilder<Entity.PublicationSequence> builder = DynamicBuilder<Entity.PublicationSequence>.CreateBuilder(rdr);
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
        private static List<Entity.PublicationSequence> GetList(SqlCommand cmd)
        {
            List<Entity.PublicationSequence> retList = new List<Entity.PublicationSequence>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.PublicationSequence retItem = new Entity.PublicationSequence();
                        DynamicBuilder<Entity.PublicationSequence> builder = DynamicBuilder<Entity.PublicationSequence>.CreateBuilder(rdr);
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
