using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class CrossTabReport
    {
        #region Properties
        [DataMember]
        public int CrossTabReportID { get; set; }
        [DataMember]
        public string CrossTabReportName { get; set; }
        [DataMember]
        public string Row { get; set; }
        [DataMember]
        public string Column { get; set; }
        [DataMember]
        public string ViewType { get; set; }
        [DataMember]
        public Enums.ViewType View_Type { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string PubName { get; set; }
        [DataMember]
        public string RowDisplayName { get; set; }
        [DataMember]
        public string ColumnDisplayName { get; set; }
        #endregion

        #region Data
        public static bool ExistsByCrossTabReportName(KMPlatform.Object.ClientConnections clientconnection, int crossTabReportID, string crossTabReportName)
        {
            SqlCommand cmd = new SqlCommand("e_CrossTabReport_Exists_ByCrossTabReportName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CrossTabReportID", crossTabReportID);
            cmd.Parameters.AddWithValue("@CrossTabReportName", crossTabReportName);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static List<CrossTabReport> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<CrossTabReport> lctr = (List<CrossTabReport>)CacheUtil.GetFromCache("CROSSTABREPORT", DatabaseName);

                if (lctr == null)
                {
                    lctr = GetData(clientconnection);

                    CacheUtil.AddToCache("CROSSTABREPORT", lctr, DatabaseName);
                }

                return lctr;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<CrossTabReport> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<CrossTabReport> retList = new List<CrossTabReport>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_CrossTabReport_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CrossTabReport> builder = DynamicBuilder<CrossTabReport>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    CrossTabReport x = builder.Build(rdr);
                    x.View_Type = GetViewType(x.ViewType);
                    x.BrandID = x.BrandID == null ? 0 : x.BrandID;
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

        private static Enums.ViewType GetViewType(string ViewType)
        {
            return (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), ViewType, true);
        }

        public static List<CrossTabReport> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<CrossTabReport> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static CrossTabReport GetByID(KMPlatform.Object.ClientConnections clientconnection, int crossTabReportID)
        {
            return GetAll(clientconnection).Find(x => x.CrossTabReportID == crossTabReportID);
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("CROSSTABREPORT", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("CROSSTABREPORT", DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, CrossTabReport r)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_CrossTabReport_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CrossTabReportID", r.CrossTabReportID));
            cmd.Parameters.Add(new SqlParameter("@CrossTabReportName", r.CrossTabReportName));
            cmd.Parameters.Add(new SqlParameter("@Row", r.Row));
            cmd.Parameters.Add(new SqlParameter("@Column", r.Column));
            cmd.Parameters.Add(new SqlParameter("@ViewType", r.View_Type.ToString()));
            cmd.Parameters.Add(new SqlParameter("@PubID", (object)r.PubID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BrandID", (object)r.BrandID ?? DBNull.Value));;
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", (object)r.IsDeleted ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedUserID", (object)r.UpdatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", (object)r.UpdatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", (object)r.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedDate", (object)r.CreatedDate ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int crossTabReportID, int userID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_CrossTabReport_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CrossTabReportID", crossTabReportID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
