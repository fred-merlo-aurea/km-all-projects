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
    public class FilterScheduleIntegration
    {
        public FilterScheduleIntegration() { }

        #region Properties
        [DataMember]
        public int FilterScheduleID { get; set; }
        [DataMember]
        public string IntegrationParamName { get; set; }
        [DataMember]
        public string IntegrationParamValue { get; set; }
        #endregion

        #region Data
        public static List<FilterScheduleIntegration> getByFilterScheduleID(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID)
        {
            List<FilterScheduleIntegration> retList = new List<FilterScheduleIntegration>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterScheduleIntegration_Select_FilterScheduleID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterScheduleID));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterScheduleIntegration> builder = DynamicBuilder<FilterScheduleIntegration>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    FilterScheduleIntegration x = builder.Build(rdr);
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterScheduleIntegration fsi)
        {
            SqlCommand cmd = new SqlCommand("e_FilterScheduleIntegration_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", fsi.FilterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@IntegrationParamName", fsi.IntegrationParamName));
            cmd.Parameters.Add(new SqlParameter("@IntegrationParamValue", fsi.IntegrationParamValue));

            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterScheduleIntegration_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterScheduleID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
