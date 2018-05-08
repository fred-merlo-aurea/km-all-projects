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
    public class Country
    {
        public Country() { }

        #region Properties
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public int PhonePrefix { get; set; }
        [DataMember]
        public string Continent { get; set; }
        [DataMember]
        public string ContinentCode { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string Alpha2 { get; set; }
        [DataMember]
        public string Apha3 { get; set; }
        [DataMember]
        public string ISOCountryCode { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        #endregion

        #region Data
        public static List<Country> GetAll()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Country> Country = (List<Country>)CacheUtil.GetFromCache("UL_COUNTRY", DatabaseName);

                if (Country == null)
                {
                    Country = GetData();

                    CacheUtil.AddToCache("UL_COUNTRY", Country, DatabaseName);
                }

                return Country;
            }
            else
            {
                return GetData();
            }
        }

        private static List<Country> GetData()
        {
            List<Country> retList = new List<Country>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select CountryID, ShortName from Country with (nolock) order by SortOrder asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Country> builder = DynamicBuilder<Country>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Country x = builder.Build(rdr);
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

         public static List<Country> GetSelectedCountries()
        {
            return GetAll().Where(x => x.CountryID != 3 & x.CountryID != 4).ToList();
        }

        public static List<Country> GetArea()
        {
            List<Country> retList = new List<Country>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select distinct Area from Country with (nolock) where isnull(Area,'') <> '' order by Area asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Country> builder = DynamicBuilder<Country>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Country x = builder.Build(rdr);
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

        public static List<Country> GetByArea(string Area)
        {
            List<Country> retList = new List<Country>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select CountryID from Country with (nolock) where Area = @Area", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@Area", Area));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Country> builder = DynamicBuilder<Country>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Country x = builder.Build(rdr);
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

        #endregion
    }
}
