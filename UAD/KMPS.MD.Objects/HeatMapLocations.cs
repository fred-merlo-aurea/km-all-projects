using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class HeatMapLocations
    {
        public HeatMapLocations() { }

        #region Properties
        [DataMember]
        public int LocationID { get; set; }
        [DataMember]
        public string LocationName  { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Radius { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        #endregion

        #region Data
        public static List<HeatMapLocations> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<HeatMapLocations> retList = new List<HeatMapLocations>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select h.*, b.brandID from HeatMapLocations h left outer join brand b on h.brandID = b.BrandID  where isnull(b.IsDeleted,0) = 0 order by LocationName asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<HeatMapLocations> builder = DynamicBuilder<HeatMapLocations>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    HeatMapLocations x = builder.Build(rdr);
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

        public static List<HeatMapLocations> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<HeatMapLocations> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static DataTable GetSelectedSubscribers(KMPlatform.Object.ClientConnections clientconnection, string xmlLocations)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSelectedSubscriberCount", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@xmlLocations", xmlLocations);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static StringBuilder GetSelectedSubscribersQueriesByGL(KMPlatform.Object.ClientConnections clientconnection, double minLat, double maxLat, double minLon, double maxLon, double Latitude, double Longitude, int RadiusMax, int brandID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberQueriesByGL", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MinLat", minLat);
            cmd.Parameters.AddWithValue("@MaxLat", maxLat);
            cmd.Parameters.AddWithValue("@MinLon", minLon);
            cmd.Parameters.AddWithValue("@MaxLon", maxLon);
            cmd.Parameters.AddWithValue("@Latitude", Latitude);
            cmd.Parameters.AddWithValue("@Longitude", Longitude);
            cmd.Parameters.AddWithValue("@RadiusMax", RadiusMax);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            StringBuilder Queries = new StringBuilder();
            Queries.Append("<xml><Queries>");
            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1, dt.Rows[0][0].ToString()));
            Queries.Append("</Queries><Results>");
            Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
            Queries.Append("</Results></xml>");
            return Queries;
        }

        public static StringBuilder GetSelectedSubscribersQueries(StringBuilder latitude, StringBuilder longitude)
        {
            String query = "select distinct 1, s.SubscriptionID as 'SubscriberID' from Subscriptions s  where latitude in (" + latitude + ") and longitude in (" + longitude + ")"; 
            
            StringBuilder Queries = new StringBuilder();
            Queries.Append("<xml><Queries>");
            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1, query));
            Queries.Append("</Queries><Results>");
            Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
            Queries.Append("</Results></xml>");
            return Queries;
        }

        #endregion

        #region CRUD
        public static int SaveLocation(KMPlatform.Object.ClientConnections clientconnection, HeatMapLocations h)
        {
            SqlCommand cmd = new SqlCommand("sp_SaveHeatMapLocations");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@address", h.Address);
            cmd.Parameters.AddWithValue("@city", h.City);
            cmd.Parameters.AddWithValue("@state", h.State);
            cmd.Parameters.AddWithValue("@zip", h.Zip);
            cmd.Parameters.AddWithValue("@radius", h.Radius);
            cmd.Parameters.AddWithValue("@location_name", h.LocationName);
            cmd.Parameters.AddWithValue("@user", h.CreatedBy);
            cmd.Parameters.AddWithValue("@brandID", h.BrandID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void DeleteLocation(KMPlatform.Object.ClientConnections clientconnection, int LocationID)
        {
            SqlCommand cmd = new SqlCommand("sp_DelHeatMapLocations");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@location_id", LocationID);
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
