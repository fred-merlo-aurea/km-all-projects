using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class FilterExportField
    {
        public FilterExportField() { }

        #region Properties
        [DataMember]
        public int FilterExportFieldID { get; set; }
        [DataMember]
        public int FilterScheduleID { get; set; }
        [DataMember]
        public string ExportColumn { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string MappingField { get; set; }
        [DataMember]
        public bool IsCustomValue { get; set; }
        [DataMember]
        public string CustomValue { get; set; }
        [DataMember]
        public bool IsDescription { get; set; }
        [DataMember]
        public string FieldCase { get; set; }
        #endregion

        #region Data
        public static List<FilterExportField> getByFilterScheduleID(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID)
        {
            List<FilterExportField> retList = new List<FilterExportField>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterExportField where FilterScheduleID = @FilterScheduleID", conn);
            cmd.Parameters.AddWithValue("@FilterScheduleID", filterScheduleID);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterExportField> builder = DynamicBuilder<FilterExportField>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterExportField x = builder.Build(rdr);
                    if (x.IsDescription)
                        x.ExportColumn = x.ExportColumn + "_Description";
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

        public static List<FilterExportField> getDisplayName(KMPlatform.Object.ClientConnections clientconnection, int @FilterScheduleID)
        {
            List<FilterExportField> retList = new List<FilterExportField>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterExportField_GetDisplayName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", @FilterScheduleID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterExportField> builder = DynamicBuilder<FilterExportField>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterExportField x = builder.Build(rdr);
                    if (x.IsDescription)
                        x.DisplayName = x.DisplayName + "_Description";
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterExportField fef)
        {
            SqlCommand cmd = new SqlCommand("e_FilterExportField_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", fef.FilterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@ExportColumn", fef.ExportColumn));
            cmd.Parameters.Add(new SqlParameter("@MappingField", fef.MappingField));
            cmd.Parameters.Add(new SqlParameter("@IsCustomValue", fef.IsCustomValue));
            cmd.Parameters.Add(new SqlParameter("@CustomValue", fef.CustomValue));
            cmd.Parameters.Add(new SqlParameter("@IsDescription", fef.IsDescription));
            cmd.Parameters.Add(new SqlParameter("@FieldCase", (object)fef.FieldCase ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterExportField_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterScheduleID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

    }
}