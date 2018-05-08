using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common.Data;
//using System.Text;
//using System.Threading.Tasks;

using KMPlatform.Object;

namespace KMPlatform.DataAccess
{
    public class SecurityGroupPermission
    {
        public static List<Entity.SecurityGroupPermission> Select(int securityGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupPermission_Select_SecurityGroupID";
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", securityGroupID));
            return cmd.GetList<Entity.SecurityGroupPermission>();
        }

        public static List<Entity.SecurityGroupPermission.Permission> GetPermissions(int securityGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupPermission_Select_Permissions_SecurityGroupID";
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", securityGroupID));
            return cmd.GetList<Entity.SecurityGroupPermission.Permission>();
        }
        
        public static int Save(Entity.SecurityGroupPermission x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupPermission_Save";
            if(x.SecurityGroupPermissionID > 0)
                cmd.Parameters.Add(new SqlParameter("@SecurityGroupPermissionID", x.SecurityGroupPermissionID));
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", x.SecurityGroupID));
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureAccessMapID", x.ServiceFeatureAccessMapID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
