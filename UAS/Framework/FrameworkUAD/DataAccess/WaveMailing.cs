using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class WaveMailing
    {
        public static List<Entity.WaveMailing> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.WaveMailing> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WaveMailing_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        private static List<Entity.WaveMailing> GetList(SqlCommand cmd)
        {
            List<Entity.WaveMailing> retList = new List<Entity.WaveMailing>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.WaveMailing retItem = new Entity.WaveMailing();
                        DynamicBuilder<Entity.WaveMailing> builder = DynamicBuilder<Entity.WaveMailing>.CreateBuilder(rdr);
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

        public static int Save(Entity.WaveMailing x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WaveMailing_Save";
            cmd.Parameters.Add(new SqlParameter("@WaveMailingID", x.WaveMailingID));
            cmd.Parameters.Add(new SqlParameter("@IssueID", x.IssueID));
            cmd.Parameters.Add(new SqlParameter("@WaveMailingName", x.WaveMailingName));
            cmd.Parameters.Add(new SqlParameter("@WaveNumber", x.WaveNumber));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@DateSubmittedToPrinter", (object)x.DateSubmittedToPrinter ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubmittedToPrinterByUserID", (object)x.SubmittedToPrinterByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
