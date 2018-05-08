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
    public class BlastLink
    {
        private static readonly string _CacheName = "CACHE_BLASTLINK_";
        private static string _CacheRegion = "BlastLink";

        //public static bool Exists(int blastID, int customerID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "if exists (select top 1 bf.BlastID FROM BlastFields bf with (nolock) join Blast b with (nolock) on bf.BlastID = b.BlastID WHERE bf.BlastID = @BlastID and b.CustomerID = @CustomerID and bf.IsDeleted = 0) select 1 else select 0";
        //    cmd.Parameters.AddWithValue("@BlastID", blastID);
        //    cmd.Parameters.AddWithValue("@CustomerID", customerID);
        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        //}

        public static List<ECN_Framework_Entities.Communicator.BlastLink> GetByBlastID(int blastID)
        {
            string cacheKey = _CacheName + "_" + blastID.ToString();

            List<ECN_Framework_Entities.Communicator.BlastLink> retList = new List<ECN_Framework_Entities.Communicator.BlastLink>();
 
            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM BlastLinks with(nolock)  WHERE BlastID = @BlastID";
                cmd.Parameters.AddWithValue("@BlastID", blastID);

                retList = GetList(cmd);
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, _CacheRegion) == null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM BlastLinks with(nolock)  WHERE BlastID = @BlastID";
                cmd.Parameters.AddWithValue("@BlastID", blastID);

                retList = GetList(cmd);
                KM.Common.CacheUtil.AddToCache(cacheKey, retList, _CacheRegion);
            }
            else
            {
                retList = (List<ECN_Framework_Entities.Communicator.BlastLink>)KM.Common.CacheUtil.GetFromCache(cacheKey, _CacheRegion);
            }

            return retList;
        }

        public static Dictionary<string, int> GetDictionaryByBlastID(int blastID)
        {
            string cacheKey = _CacheName + "_" + blastID.ToString() + "_Dictionary";
            Dictionary<string, int> retDict = new Dictionary<string, int>(10000000);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM BlastLinks with(nolock)  WHERE BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                retDict = GetDictionary(cmd);
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, _CacheRegion) == null)
            {
                retDict = GetDictionary(cmd);
                KM.Common.CacheUtil.AddToCache(cacheKey, retDict, _CacheRegion);
            }
            else
            {
                retDict = (Dictionary<string, int>)KM.Common.CacheUtil.GetFromCache(cacheKey, _CacheRegion);
            }

            return retDict;
        }

        public static ECN_Framework_Entities.Communicator.BlastLink GetByBlastLinkID(int blastID, int blastLinkID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM BlastLinks with(nolock) WHERE BlastLinkID = @BlastLinkID and BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@BlastLinkID", blastLinkID);

            ECN_Framework_Entities.Communicator.BlastLink retItem = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                retItem = (ECN_Framework_Entities.Communicator.BlastLink)KM.Common.CacheUtil.GetFromCache(blastLinkID.ToString(), _CacheRegion);
                if (retItem == null)
                {
                    retItem = Get(cmd);
                    if(retItem != null)
                        KM.Common.CacheUtil.AddToCache(blastLinkID.ToString(), retItem, _CacheRegion);
                }
            }
            else
                retItem = Get(cmd);
            return retItem;
        }

        public static ECN_Framework_Entities.Communicator.BlastLink GetByLinkURL(int blastID, string linkURL)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM BlastLinks  with(nolock)  WHERE BlastID = @BlastID AND LinkURL = @LinkURL";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@LinkURL", linkURL);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.BlastLink GetByLinkURL_ECNID(int blastID, string linkURL, string ecn_id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT bl.* FROM BlastLinks bl with(nolock) " +
                                "join UniqueLink lu with(nolock) on bl.BlastLinkID = lu.BlastLinkID " + 
                                "WHERE bl.BlastID = @BlastID AND LinkURL = @LinkURL AND lu.UniqueID = @ECNID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@LinkURL", linkURL);
            cmd.Parameters.AddWithValue("@ECNID", ecn_id);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.BlastLink Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastLink retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Communicator.BlastLink();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastLink>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.BlastLink> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastLink> retList = new List<ECN_Framework_Entities.Communicator.BlastLink>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastLink retItem = new ECN_Framework_Entities.Communicator.BlastLink();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastLink>.CreateBuilder(rdr);
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

        private static Dictionary<string, int> GetDictionary(SqlCommand cmd)
        {
            Dictionary<string, int> retItem = new Dictionary<string, int>(10000000);

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    try
                    {
                        while (rdr.Read())
                        {
                            if (!retItem.ContainsKey(rdr["LinkURL"].ToString().ToLower().Trim()))
                                retItem.Add(rdr["LinkURL"].ToString().ToLower().Trim(), int.Parse(rdr["BlastLinkID"].ToString()));
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retItem;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.BlastLink link)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO BlastLinks (BlastID, LinkURL) VALUES (@BlastID, @LinkURL); SELECT @@IDENTITY;";
            cmd.Parameters.Add(new SqlParameter("@BlastID", link.BlastID));
            cmd.Parameters.Add(new SqlParameter("@LinkURL", link.LinkURL));

            RemoveCache(link.BlastID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        private static void RemoveCache(int blastID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                KM.Common.CacheUtil.RemoveFromCache(_CacheName + "_" + blastID.ToString(), _CacheRegion);
                KM.Common.CacheUtil.RemoveFromCache(_CacheName + "_" + blastID.ToString() + "_Dictionary", _CacheRegion);
            }
        }

        //public static void Delete(int blastID, int customerID, int userID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_BlastFields_Delete";
        //    cmd.Parameters.AddWithValue("@CustomerID", customerID);
        //    cmd.Parameters.AddWithValue("@BlastID", blastID);
        //    cmd.Parameters.AddWithValue("@UserID", userID);

        //    DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        //}
    }
}
