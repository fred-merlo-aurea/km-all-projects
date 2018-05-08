using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class PubResponseEmail
    {
        public int PREID { get; set; }
        public int PFID { get; set; }
        public NotificatonFor NotificatonFor { get; set; }
        public string Response_FromName { get; set; }
        public bool SendUserEmail { get; set; }

        public string Response_FromEmail { get; set; }
        public string Response_UserMsgSubject { get; set; }
        public string Response_UserMsgBody { get; set; }

        public bool SendAdminEmail { get; set; }
        public string Response_AdminEmail { get; set; }
        public string Response_AdminMsgSubject { get; set; }
        public string Response_AdminMsgBody { get; set; }

        public bool SendNQRespEmail { get; set; } 
        public string Response_UserMsgNQRespSub { get; set; }
        public string Response_UserMsgNQRespMsgBody { get; set; }      

        public PubResponseEmail()
        {
            PREID = 0;
            PFID= 0;

            NotificatonFor = NotificatonFor.Other;

            Response_FromName = string.Empty;
            SendUserEmail = false;

            Response_FromEmail = string.Empty;
            Response_UserMsgSubject = string.Empty;
            Response_UserMsgBody = string.Empty;

            SendAdminEmail = false;
            Response_AdminEmail = string.Empty;
            Response_AdminMsgSubject = string.Empty;
            Response_AdminMsgBody = string.Empty;

            SendNQRespEmail = false; 
            
        }

        public static Dictionary<NotificatonFor, PubResponseEmail> GetByPFID(int PFID)
        {
            Dictionary<NotificatonFor, PubResponseEmail> dResponseEmail = new Dictionary<NotificatonFor, PubResponseEmail>();

            SqlCommand cmd = new SqlCommand(string.Format("select * from PubResponseEmails  with (NOLOCK) where PFID = {0}", PFID));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable(cmd);

            foreach (DataRow dr in dt.Rows)
            {
                PubResponseEmail re = new PubResponseEmail();

                re.PREID = Convert.ToInt32(dr["PREID"]);
                re.PFID = Convert.ToInt32(dr["PFID"]);

                re.NotificatonFor = (NotificatonFor)Enum.Parse(typeof(NotificatonFor), dr["ResponseType"].ToString(), true);

                re.SendUserEmail = dr.IsNull("SendUserEmail") ? false : Convert.ToBoolean(dr["SendUserEmail"]);

                re.Response_FromName = dr["Response_FromName"].ToString().Trim();
                re.Response_FromEmail = dr["Response_FromEmail"].ToString().Trim();
                re.Response_UserMsgSubject = dr["Response_UserMsgSubject"].ToString().Trim();
                re.Response_UserMsgBody = dr["Response_UserMsgBody"].ToString().Trim();

                re.SendAdminEmail = dr.IsNull("SendAdminEmail") ? false : Convert.ToBoolean(dr["SendAdminEmail"]);
                re.Response_AdminEmail = dr["Response_AdminEmail"].ToString().Trim();
                re.Response_AdminMsgSubject = dr["Response_AdminMsgSubject"].ToString().Trim();
                re.Response_AdminMsgBody = dr["Response_AdminMsgBody"].ToString().Trim();

                re.SendNQRespEmail = dr.IsNull("SendNQRespEmail") ? false : Convert.ToBoolean(dr["SendNQRespEmail"].ToString());
                re.Response_UserMsgNQRespSub = dr["Response_UserMsgNQRespSub"].ToString().Trim();
                re.Response_UserMsgNQRespMsgBody = dr["Response_UserMsgNQRespMsgBody"].ToString().Trim();

                dResponseEmail.Add(re.NotificatonFor, re);
            }
            return dResponseEmail;
        }
    }
}
