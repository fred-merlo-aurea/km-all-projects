using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class ApplicationCode
    {
        public static bool Exists(int appCodeID, ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode codeType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 AppCodeID FROM ApplicationCode WHERE AppCodeID = @AppCodeID AND CodeType = @CodeType and ci.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@AppCodeID", appCodeID);
            cmd.Parameters.AddWithValue("@CodeType", Convert.ToInt32(codeType));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.ApplicationCode> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM ApplicationCode ORDER BY CodeType ASC, CodeValue ASC AND IsDeleted = 0";
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ApplicationCode> GetByCodeType(ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode ctype)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM ApplicationCode WHERE CodeType = @CodeType AND IsDeleted = 0 ORDER BY CodeType ASC, CodeValue ASC";
            cmd.Parameters.AddWithValue("@CodeType", ctype.ToString());
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.ApplicationCode GetByCodeID(int codeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM ApplicationCode WHERE CodeID = @CodeID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@CodeID", codeID);
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.ApplicationCode> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ApplicationCode> retList = new List<ECN_Framework_Entities.Communicator.ApplicationCode>();

            ECN_Framework_Entities.Communicator.ApplicationCode retItem = null;

            DataTable dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            if (dt != null && dt.Rows.Count <= 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    retItem = GetApplicationCode(dt.Rows[0]);
                    retList.Add(retItem);
                }
                
            }

            return retList;
        }

        private static ECN_Framework_Entities.Communicator.ApplicationCode Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ApplicationCode retItem = null;

            DataTable dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            if (dt != null && dt.Rows.Count <= 0)
            {
                retItem = GetApplicationCode(dt.Rows[0]);
            }

            return retItem;
        }

        private static ECN_Framework_Entities.Communicator.ApplicationCode GetApplicationCode(DataRow row)
        {
            ECN_Framework_Entities.Communicator.ApplicationCode ac = new ECN_Framework_Entities.Communicator.ApplicationCode();

            ac.AppCodeID = Convert.ToInt32(row["AppCodeID"].ToString());
            ac.CodeValue = row["CodeValue"].ToString();
            if (!DBNull.Value.Equals(row["IsDeleted"]))
                ac.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());
            ac.CodeType = GetApplicationCode(row["CodeType"].ToString());
            return ac;
        }

        private static ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode GetApplicationCode(string code)
        {
            ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode returnType;
            switch (code.Trim().ToLower())
            {
                case "CAMPAIGNITEMTYPE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.CAMPAIGNITEMTYPE;
                    break;
                case "MENUCODE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.MENUCODE;
                    break;
                case "BLASTSTATUSCODE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.BLASTSTATUSCODE;
                    break;
                case "BLASTTYPE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.BLASTTYPE;
                    break;
                case "CODETYPE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.CODETYPE;
                    break;
                case "TEMPLATESTYLECODE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.TEMPLATESTYLECODE;
                    break;
                case "FOLDERTYPE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.FOLDERTYPE;
                    break;
                case "CONTENTTYPECODE":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.CONTENTTYPECODE;
                    break;
                default:
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.UNKNOWN;
                    break;
            }
            return returnType;
        }
    }
}
