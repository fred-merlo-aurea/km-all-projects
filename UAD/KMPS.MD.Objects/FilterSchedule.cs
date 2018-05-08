using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using KM.Common;
using KMPlatform.Object;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class FilterSchedule
    {
        public FilterSchedule() { }

        #region Properties
        [DataMember]
        public int FilterScheduleID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public Enums.ExportType ExportTypeID { get; set; }
        [DataMember]
        public bool IsRecurring { get; set; }
        [DataMember]
        public int? RecurrenceTypeID { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public bool RunSunday { get; set; }
        [DataMember]
        public bool RunMonday { get; set; }
        [DataMember]
        public bool RunTuesday { get; set; }
        [DataMember]
        public bool RunWednesday { get; set; }
        [DataMember]
        public bool RunThursday { get; set; }
        [DataMember]
        public bool RunFriday { get; set; }
        [DataMember]
        public bool RunSaturday { get; set; }
        [DataMember]
        public int? MonthScheduleDay { get; set; }
        [DataMember]
        public bool MonthLastDay { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public string EmailNotification { get; set; }
        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Folder { get; set; }
        [DataMember]
        public string ExportFormat { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string RecurrenceType { get; set; }
        [DataMember]
        public string FilterName { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public int? FolderID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string Operation { get; set; }
        [DataMember]
        public List<int> FilterGroupID_Selected { get; set; }
        [DataMember]
        public List<int> FilterGroupID_Suppressed { get; set; }
        [DataMember]
        public bool ShowHeader { get; set; }
        [DataMember]
        public string ExportName { get; set; }
        [DataMember]
        public string ExportNotes { get; set; }
        [DataMember]
        public int? FilterSegmentationID { get; set; }
        [DataMember]
        public string FilterSegmentationName { get; set; }
        [DataMember]
        public string SelectedOperation { get; set; }
        [DataMember]
        public string SuppressedOperation { get; set; }
        [DataMember]
        public string FileNameFormat { get; set; }
        #endregion

        #region Data
        public static bool ExistsByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterSchedule where IsDeleted  = 0 and FilterID = @FilterID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsByFilterSegmentationID(KMPlatform.Object.ClientConnections clientconnection, int filterSegmentationID)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterSchedule where IsDeleted  = 0 and FilterSegmentationID = @FilterSegmentationID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterSegmentationID", filterSegmentationID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsByFileName(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID, string fileName)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterSchedule where IsDeleted  = 0 and filterScheduleID <> @FilterScheduleID and FileName = @FileName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterScheduleID", filterScheduleID);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static List<FilterSchedule> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int BrandID, bool IsFilterSegmentation)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_BrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.Parameters.Add(new SqlParameter("@IsFilterSegmentation", IsFilterSegmentation));
            cmd.CommandTimeout = 0;

            return GetList(conn, cmd);
        }

        public static List<FilterSchedule> GetByBrandIDUserID(KMPlatform.Object.ClientConnections clientconnection, int BrandID, int UserID, bool IsFilterSegmentation)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_UserID_BrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.Parameters.Add(new SqlParameter("@IsFilterSegmentation", IsFilterSegmentation));
            cmd.CommandTimeout = 0;
            
            return GetList(conn, cmd);
        }

        private static List<int> GetFilterGroupID(string FilterGroupIDs)
        {
            return FilterGroupIDs.Split(',').Select(n => int.Parse(n)).ToList(); 
        }

        public static FilterSchedule GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterscheduleID)
        {
            FilterSchedule retItem = new FilterSchedule();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_FilterScheduleID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterscheduleID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterSchedule> builder = DynamicBuilder<FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                    if (retItem != null)
                    {
                        retItem.FilterGroupID_Selected = rdr["FilterGroupID_Selected"].ToString() == null || rdr["FilterGroupID_Selected"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Selected"].ToString());
                        retItem.FilterGroupID_Suppressed = rdr["FilterGroupID_Suppressed"].ToString() == null || rdr["FilterGroupID_Suppressed"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Suppressed"].ToString());
                    }
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

        public static List<FilterSchedule> GetScheduleByDateTime(KMPlatform.Object.ClientConnections clientconnection, string dt, string time)
        {
            List<FilterSchedule> retList = new List<FilterSchedule>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_BySchedule", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@time", time);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterSchedule> builder = DynamicBuilder<FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterSchedule x = builder.Build(rdr);
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
        public static Tuple<DataTable, string, DataTable, bool> Export(ClientConnections clientconnection, int filterscheduleID)
        {
            Guard.NotNull(clientconnection, nameof(clientconnection));

            var filterScheduleExport = new FilterScheduleExport(clientconnection);
            return filterScheduleExport.Export(filterscheduleID);
        }

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        public static List<FilterSchedule> GetByFilterSegmentationID(ClientConnections clientconnection, int filterSegmentationID)
        {
            List<FilterSchedule> retList = new List<FilterSchedule>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_FilterSegmentationID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FilterSegmentationID", filterSegmentationID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterSchedule> builder = DynamicBuilder<FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterSchedule x = builder.Build(rdr);
                    if (x != null)
                    {
                        x.FilterGroupID_Selected = rdr["FilterGroupID_Selected"].ToString() == null || rdr["FilterGroupID_Selected"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Selected"].ToString());
                        x.FilterGroupID_Suppressed = rdr["FilterGroupID_Suppressed"].ToString() == null || rdr["FilterGroupID_Suppressed"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Suppressed"].ToString());
                    }
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

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterSchedule filterSchedule)
        {
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterSchedule.FilterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterSchedule.FilterID));
            cmd.Parameters.Add(new SqlParameter("@ExportTypeID", filterSchedule.ExportTypeID));
            cmd.Parameters.Add(new SqlParameter("@IsRecurring", filterSchedule.IsRecurring));
            cmd.Parameters.Add(new SqlParameter("@RecurrenceTypeID", (object)filterSchedule.RecurrenceTypeID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StartDate", filterSchedule.StartDate));
            cmd.Parameters.Add(new SqlParameter("@StartTime", filterSchedule.StartTime));
            cmd.Parameters.Add(new SqlParameter("@EndDate", (object)filterSchedule.EndDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunSunday", filterSchedule.RunSunday));
            cmd.Parameters.Add(new SqlParameter("@RunMonday", filterSchedule.RunMonday));
            cmd.Parameters.Add(new SqlParameter("@RunTuesday", filterSchedule.RunTuesday));
            cmd.Parameters.Add(new SqlParameter("@RunWednesday", filterSchedule.RunWednesday));
            cmd.Parameters.Add(new SqlParameter("@RunThursday", filterSchedule.RunThursday));
            cmd.Parameters.Add(new SqlParameter("@RunFriday", filterSchedule.RunFriday));
            cmd.Parameters.Add(new SqlParameter("@RunSaturday", filterSchedule.RunSaturday));
            cmd.Parameters.Add(new SqlParameter("@MonthScheduleDay", (object)filterSchedule.MonthScheduleDay ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MonthLastDay", filterSchedule.MonthLastDay));
            if (filterSchedule.FilterScheduleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", filterSchedule.UpdatedBy));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", filterSchedule.CreatedBy));

            cmd.Parameters.Add(new SqlParameter("@EmailNotification", filterSchedule.EmailNotification));
            cmd.Parameters.Add(new SqlParameter("@Server", (object)filterSchedule.Server ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserName", (object)filterSchedule.UserName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Password", (object)filterSchedule.Password ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Folder", (object)filterSchedule.Folder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ExportFormat", (object)filterSchedule.ExportFormat ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FileName", (object)filterSchedule.FileName ?? DBNull.Value));
            //cmd.Parameters.Add(new SqlParameter("@Operation", filterSchedule.Operation));

            string fgID_Selected = string.Empty;
            string fgID_Suppressed = string.Empty;

            if (filterSchedule.FilterGroupID_Selected != null)
            {
                foreach (int i in filterSchedule.FilterGroupID_Selected)
                {
                    fgID_Selected += fgID_Selected == string.Empty ? i.ToString() : "," + i.ToString();
                }
            }

            if (filterSchedule.FilterGroupID_Suppressed != null)
            {
                 foreach (int i in filterSchedule.FilterGroupID_Suppressed)
                {
                    fgID_Suppressed += fgID_Suppressed == string.Empty ? i.ToString() : "," + i.ToString();
                }
            }

            cmd.Parameters.Add(new SqlParameter("@FilterGroupID_Selected", fgID_Selected));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID_Suppressed", fgID_Suppressed));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)filterSchedule.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FolderID", (object)filterSchedule.FolderID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)filterSchedule.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ShowHeader", filterSchedule.ShowHeader));
            cmd.Parameters.Add(new SqlParameter("@ExportName", filterSchedule.ExportName));
            cmd.Parameters.Add(new SqlParameter("@ExportNotes", (object)filterSchedule.ExportNotes ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterSegmentationID", (object)filterSchedule.FilterSegmentationID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SelectedOperation", (object)filterSchedule.SelectedOperation ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SuppressedOperation", (object)filterSchedule.SuppressedOperation ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FileNameFormat", (object)filterSchedule.FileNameFormat ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID, int userID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

        private static List<FilterSchedule> GetList(SqlConnection con, SqlCommand cmd)
        {
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                var builder = DynamicBuilder<FilterSchedule>.CreateBuilder(reader);
                var list = new List<FilterSchedule>();
                while (reader.Read())
                {
                    var filterSchedule = builder.Build(reader);
                    if (filterSchedule != null)
                    {
                        if (reader["FilterGroupID_Selected"] != null && reader["FilterGroupID_Selected"].ToString() != "")
                        {
                            filterSchedule.FilterGroupID_Selected = GetFilterGroupID(reader["FilterGroupID_Selected"].ToString());
                        }
                        if (reader["FilterGroupID_Suppressed"] != null && reader["FilterGroupID_Suppressed"].ToString() != "")
                        {
                            filterSchedule.FilterGroupID_Suppressed = GetFilterGroupID(reader["FilterGroupID_Suppressed"].ToString());
                        }
                        list.Add(filterSchedule);
                    }
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}