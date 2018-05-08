using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Core_AMS.Utilities;
using KM;
using KM.Common.Data;
using KM.Common.Functions;
using KMCommonDataFunctions = KM.Common.DataFunctions;

namespace FrameworkSubGen.DataAccess
{
    [Serializable]
    public class DataFunctions
    {
        #region Executes

        public static int Execute(SqlCommand cmd, string connectionStringName)
        {
            int success = 0;
            try
            {
                using (SqlConnection conn = KMCommonDataFunctions.GetSqlConnection(connectionStringName))
                {
                    cmd = KMCommonDataFunctions.MinDateCheck(cmd);
                    cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    success = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return success;
        }
        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int Execute(SqlCommand cmd)
        {
            int success = 0;
            if (cmd.Connection != null)
            {
                try
                {
                    cmd = KMCommonDataFunctions.MinDateCheck(cmd);
                    cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                    using (cmd)
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Connection.Open();
                        success = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    cmd.Dispose();
                }
            }
            return success;
        }

        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand command)
        {
            var result = KMCommonDataFunctions.ExecuteScalar(command, false);
            return result;
        }

        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        #endregion
        #region SQL Helpers
        public static string CleanSerializedXML(string dirtyXML)
        {
            string returnClean = dirtyXML.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkSubGen.Entity\"","").Replace(" i:nil=\"true\"","");
            return returnClean;
        }

        public static string CleanSerializedXML(string dirtyXML, string nameSpace)
        {
            string returnClean = dirtyXML.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkSubGen.Entity\"", "").Replace(" i:nil=\"true\"", "");
            return returnClean;
        }

        #endregion

        public static DataTable GetDataTableViaAdapter(SqlCommand cmd)
        {
            cmd = KMCommonDataFunctions.MinDateCheck(cmd);
            cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
            cmd.CommandTimeout = 0;

            SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Table Data");
            myAdapter.Fill(dt);

            return dt;
        }

        public static SqlConnection GetClientSqlConnection(KMPlatform.Entity.Client client)
        {
            return FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
        }

        public static SqlConnection GetClientSqlConnection(KMPlatform.Object.ClientConnections client)
        {
            return FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
        }
    }
}
