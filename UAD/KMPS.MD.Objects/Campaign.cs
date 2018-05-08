using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class Plans
    {
        public Plans() { }
        #region Properties
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Count { get; set; }
        public int BrandID { get; set; }
        #endregion

        #region Data
        public static List<Plans> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Plans> retList = new List<Plans>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select p.planID, PlanName, count(distinct subscriptionID) as count from plans p left join planfilter pf on p.planID = pf.planID  left join planfilterdetails pfd on pfd.planfilterID = pf.planfilterID group by p.planID, PlanName order by p.planID", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Plans> builder = DynamicBuilder<Plans>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Plans x = builder.Build(rdr);
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

        public static int PlanExists(KMPlatform.Object.ClientConnections clientconnection, string planname)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Plans WHERE PlanName=@PlanName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PlanName", planname));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion

        #region CRUD
        public static int Insert(KMPlatform.Object.ClientConnections clientconnection, string planname, int UserID)
        {
            SqlCommand cmd = new SqlCommand("Insert Into plans (PlanName, AddedBy, DateAdded, UpdatedBy, DateUpdated) values (@PlanName, @AddedBy, @DateAdded, @UpdatedBy, @DateUpdated); select @@identity");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PlanName", planname));
            cmd.Parameters.Add(new SqlParameter("@AddedBy", UserID));
            cmd.Parameters.Add(new SqlParameter("@DateAdded", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@UpdatedBy", UserID));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", DateTime.Now));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int planID)
        {
            SqlCommand cmd = new SqlCommand("delete from planfilterdetails where planfilterID in (select planfilterID from PlanFilter where PlanID = @PlanID); delete from planfilter where planID = @PlanID; delete from Plans where PlanID = @PlanID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PlanID", planID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}