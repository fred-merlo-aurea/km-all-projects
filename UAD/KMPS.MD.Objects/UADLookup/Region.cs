using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using System.Configuration;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Region
    {
        public Region() { }

        #region Properties
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string RegionName { get; set; }
        [DataMember]
        public string RegionCode { get; set; }
        [DataMember]
        public string ZipCodeRange { get; set; }
        [DataMember]
        public int ZipCodeRangeSortOrder { get; set; }
        [DataMember]
        public int RegionGroupID { get; set; }
        [DataMember]
        public int sort_order { get; set; }
        [DataMember]
        public int country_sort_order { get; set; }
        [DataMember]
        public string Country { get; set; }
        #endregion

        #region Data
        public static List<Region> GetAll()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Region> Regions = (List<Region>)CacheUtil.GetFromCache("UL_REGION", DatabaseName);

                if (Regions == null)
                {
                    Regions = GetData();

                    CacheUtil.AddToCache("UL_Region", Regions, DatabaseName);
                }

                return Regions;
            }
            else
            {
                return GetData();
            }
        }

        private static List<Region> GetData()
        {
            List<Region> retList = new List<Region>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select r.*, ShortName as Country from Region r with (nolock) join Country c with (nolock) on r.CountryID = c.CountryID order by RegionCode asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Region> builder = DynamicBuilder<Region>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Region x = builder.Build(rdr);

                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<Region> GetByCountry(string country)
        {
            return Region.GetAll().FindAll(x => (x.Country ?? null) == country);
        }

        public static List<Region> GetByCountryID(int countryID)
        {
            return Region.GetAll().FindAll(x => x.CountryID  == countryID);
        }

        public static List<Region> GetByRegionGroupID(int RegionGroupID)
        {
            List<Region> retList = new List<Region>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("Select RegionCode from Region with (nolock) where RegionGroupID = @RegionGroupID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@RegionGroupID", RegionGroupID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Region> builder = DynamicBuilder<Region>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Region x = builder.Build(rdr);

                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static Region GetByRegionCode(string RegionCode)
        {
            Region retItem = new Region();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from Region with (nolock) where  RegionCode = @RegionCode", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@RegionCode", RegionCode));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Region> builder = DynamicBuilder<Region>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retItem;
        }
        #endregion
    }
}
