using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class UniqueLink
    {
        private static readonly string _CacheName = "CACHE_UNIQUELINK_";
        private static string _CacheRegion = "UNIQUELINK";

        public static int Save(ECN_Framework_Entities.Communicator.UniqueLink ul)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UniqueLink_Save";
            cmd.Parameters.AddWithValue("@BlastLinkID", ul.BlastLinkID);
            cmd.Parameters.AddWithValue("@BlastID", ul.BlastID);
            cmd.Parameters.AddWithValue("@UniqueID", ul.UniqueID);

            RemoveCache(ul.BlastID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.UniqueLink> GetByBlastID(int blastID)
        {
            string cacheKey = _CacheName + "_" + blastID.ToString();
            List<ECN_Framework_Entities.Communicator.UniqueLink> retList = new List<ECN_Framework_Entities.Communicator.UniqueLink>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UniqueLink_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                retList = GetList(cmd);
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, _CacheRegion) == null)
            {
                retList = GetList(cmd);
                KM.Common.CacheUtil.AddToCache(cacheKey, retList, _CacheRegion);
            }
            else
            {
                retList = (List<ECN_Framework_Entities.Communicator.UniqueLink>)KM.Common.CacheUtil.GetFromCache(cacheKey, _CacheRegion);
            }
            return retList;
        }

        public static Dictionary<string, int> GetDictionaryByBlastID(int blastID)
        {
            string cacheKey = _CacheName + "_" + blastID.ToString() + "_Dictionary";
            Dictionary<string, int> retDict = new Dictionary<string, int>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UniqueLink_Select_BlastID";
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


        public static ECN_Framework_Entities.Communicator.UniqueLink GetByBlastID_UniqueID(int BlastID, string UniqueID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UniqueLink_Select_BlastID_UniqueID";
            cmd.Parameters.AddWithValue("@BlastID", BlastID);
            cmd.Parameters.AddWithValue("@UniqueID", UniqueID);

            return Get(cmd);
        }

        private static void RemoveCache(int blastID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                KM.Common.CacheUtil.RemoveFromCache(_CacheName + "_" + blastID.ToString(), _CacheRegion);
                KM.Common.CacheUtil.RemoveFromCache(_CacheName + "_" + blastID.ToString() + "_Dictionary", _CacheRegion);
            }
        }

        private static ECN_Framework_Entities.Communicator.UniqueLink Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.UniqueLink retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Communicator.UniqueLink();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UniqueLink>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.UniqueLink> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.UniqueLink> retList = new List<ECN_Framework_Entities.Communicator.UniqueLink>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.UniqueLink retItem = new ECN_Framework_Entities.Communicator.UniqueLink();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UniqueLink>.CreateBuilder(rdr);
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
            Dictionary<string, int> retList = new Dictionary<string, int>();
            ECN_Framework_Entities.Communicator.UniqueLink ul = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ul = new ECN_Framework_Entities.Communicator.UniqueLink();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UniqueLink>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ul = builder.Build(rdr);
                        if (!retList.ContainsKey(ul.UniqueID.ToLower().Trim()))
                            retList.Add(ul.UniqueID.ToLower().Trim(), ul.UniqueLinkID);
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
