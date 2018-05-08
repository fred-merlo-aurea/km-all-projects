using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FilterSchedule
    {
        public static Entity.FilterSchedule Get(SqlCommand cmd)
        {
            Entity.FilterSchedule retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterSchedule();
                        DynamicBuilder<Entity.FilterSchedule> builder = DynamicBuilder<Entity.FilterSchedule>.CreateBuilder(rdr);
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
        public static List<Entity.FilterSchedule> GetList(SqlCommand cmd)
        {
            List<Entity.FilterSchedule> retList = new List<Entity.FilterSchedule>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FilterSchedule retItem = new Entity.FilterSchedule();
                        DynamicBuilder<Entity.FilterSchedule> builder = DynamicBuilder<Entity.FilterSchedule>.CreateBuilder(rdr);
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
        public static int Save(Entity.FilterSchedule x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSchedule_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleId", x.FilterScheduleId));
            cmd.Parameters.Add(new SqlParameter("@FilterId", x.FilterId));
            cmd.Parameters.Add(new SqlParameter("@ExportTypeId", x.ExportTypeId));
            cmd.Parameters.Add(new SqlParameter("@IsRecurring", x.IsRecurring));
            cmd.Parameters.Add(new SqlParameter("@RecurrenceTypeId", x.RecurrenceTypeId));
            cmd.Parameters.Add(new SqlParameter("@StartDate", (object)x.StartDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)x.StartTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EndDate", (object)x.EndDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunSunday", x.RunSunday));
            cmd.Parameters.Add(new SqlParameter("@RunMonday", x.RunMonday));
            cmd.Parameters.Add(new SqlParameter("@RunTuesday", x.RunTuesday));
            cmd.Parameters.Add(new SqlParameter("@RunWednesday", x.RunWednesday));
            cmd.Parameters.Add(new SqlParameter("@RunThursday", x.RunThursday));
            cmd.Parameters.Add(new SqlParameter("@RunFriday", x.RunFriday));
            cmd.Parameters.Add(new SqlParameter("@RunSaturday", x.RunSaturday));
            cmd.Parameters.Add(new SqlParameter("@MonthScheduleDay", x.MonthScheduleDay));
            cmd.Parameters.Add(new SqlParameter("@MonthLastDay", x.MonthLastDay));
            cmd.Parameters.Add(new SqlParameter("@EmailNotification", x.EmailNotification));
            cmd.Parameters.Add(new SqlParameter("@Server", x.Server));
            cmd.Parameters.Add(new SqlParameter("@UserName", x.UserName));
            cmd.Parameters.Add(new SqlParameter("@Password", x.Password));
            cmd.Parameters.Add(new SqlParameter("@Folder", x.Folder));
            cmd.Parameters.Add(new SqlParameter("@ExportFormatTypeId", x.ExportFormatTypeId));
            cmd.Parameters.Add(new SqlParameter("@FileName", x.FileName));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", x.IsDeleted));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID_Selected", x.FilterGroupID_Selected));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID_Suppressed", x.FilterGroupID_Suppressed));
            cmd.Parameters.Add(new SqlParameter("@FolderId", x.FolderId));
            cmd.Parameters.Add(new SqlParameter("@GroupId", x.GroupId));
            cmd.Parameters.Add(new SqlParameter("@OperatorsTypeId", x.OperatorsTypeId));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
