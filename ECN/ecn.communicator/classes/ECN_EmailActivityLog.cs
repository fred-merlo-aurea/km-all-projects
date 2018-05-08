using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

namespace ecn.communicator.classes
{
    public class ECN_EmailActivityLog
    {
        public ECN_EmailActivityLog()
        {
            EAID = -1;
            EmailID = -1;
            EmailAddress = string.Empty;
            BlastID = -1;
            ActionTypeCode = string.Empty;
            ActionDate = DateTime.MinValue;
            ActionValue = string.Empty;
            ActionNotes = string.Empty;
            Processed = string.Empty;
        }

        public int EAID { get; set; }
        public int EmailID { get; set; }
        public string EmailAddress { get; set; }
        public int BlastID { get; set; }
        public string ActionTypeCode { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionValue { get; set; }
        public string ActionNotes { get; set; }
        public string Processed { get; set; }

        #region DataAccess
        public static List<ECN_EmailActivityLog> GetByBlastID(int blastId)
        {
            List<ECN_EmailActivityLog> retList = new List<ECN_EmailActivityLog>();
            System.Text.StringBuilder sqlQuery = new System.Text.StringBuilder();
            sqlQuery.Append(" SELECT l.EAID,");
            sqlQuery.Append(" l.EmailID,");
            sqlQuery.Append(" e.EmailAddress,");
            sqlQuery.Append(" l.BlastID,");
            sqlQuery.Append(" l.ActionTypeCode,");
            sqlQuery.Append(" l.ActionDate,");
            sqlQuery.Append(" l.ActionValue,");
            sqlQuery.Append(" l.ActionNotes,");
            sqlQuery.Append(" l.Processed");
            sqlQuery.Append(" FROM EmailActivityLog l");
            sqlQuery.Append(" INNER JOIN ecn5_communicator..Emails e ON l.EmailID = e.EmailID");
            sqlQuery.Append(" WHERE l.BlastID = " + blastId);

            SqlDataReader rdr = null;
            SqlConnection conn = GetSqlConn();
            SqlCommand cmd = new SqlCommand(sqlQuery.ToString(), conn);

            conn.Open();
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ECN_EmailActivityLog item = new ECN_EmailActivityLog();
                int index;
                string name;

                name = "EAID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.EAID = Convert.ToInt32(rdr[index].ToString());

                name = "EmailID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.EmailID = Convert.ToInt32(rdr[index].ToString());

                name = "EmailAddress";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.EmailAddress = (string)rdr[index];

                name = "BlastID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.BlastID = Convert.ToInt32(rdr[index].ToString());

                name = "ActionTypeCode";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.ActionTypeCode = (string)rdr[index];

                name = "ActionDate";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.ActionDate = (DateTime)rdr[index];

                name = "ActionValue";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.ActionValue = (string)rdr[index];

                name = "ActionNotes";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.ActionNotes = (string)rdr[index];

                name = "Processed";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.Processed = (string)rdr[index];

                retList.Add(item);
            }

            rdr.Close();
            retList = FilterActivityLog(retList);

            return retList;
        }

        static List<ECN_EmailActivityLog> FilterActivityLog(List<ECN_EmailActivityLog> list)
        {
            List<ECN_EmailActivityLog> filteredList = new List<ECN_EmailActivityLog>();

            //1. get a seperate list for each email address
            //2. check ActionTypeCode of each one and decide whether to keep it or drop
            //3. add just one item to the filteredList for each email

            List<string> emailAddresses = new List<string>();
            foreach (ECN_EmailActivityLog eal in list)
            {
                if (!emailAddresses.Contains(eal.EmailAddress))
                    emailAddresses.Add(eal.EmailAddress);
            }

            foreach (string e in emailAddresses)
            {
                List<ECN_EmailActivityLog> byEmail = new List<ECN_EmailActivityLog>();
                foreach (ECN_EmailActivityLog eal in list)
                {
                    if (eal.EmailAddress.Equals(e))
                        byEmail.Add(eal);
                }

                bool hasTrueATC = false;
                bool hasFalseATC = false;
                int? trueItem = null;
                int? falseItem = null;
                int counter = 0;

                foreach (ECN_EmailActivityLog eal in byEmail)
                {
                    if (eal.ActionTypeCode.Equals("ABUSERPT_UNSUB") || eal.ActionTypeCode.Equals("bounce") || eal.ActionTypeCode.Equals("FEEDBACK_UNSUB")
                        || eal.ActionTypeCode.Equals("MASTSUP_UNSUB") || eal.ActionTypeCode.Equals("resend") || eal.ActionTypeCode.Equals("send")
                        || eal.ActionTypeCode.Equals("testsend"))
                        hasFalseATC = true;
                    if (eal.ActionTypeCode.Equals("click") || eal.ActionTypeCode.Equals("conversion") || eal.ActionTypeCode.Equals("conversionRevenue")
                        || eal.ActionTypeCode.Equals("open") || eal.ActionTypeCode.Equals("read") || eal.ActionTypeCode.Equals("refer")
                        || eal.ActionTypeCode.Equals("subscribe"))
                        hasTrueATC = true;

                    if (hasTrueATC && !trueItem.HasValue)
                    {
                        trueItem = counter;
                    }

                    if (hasFalseATC && !hasTrueATC && !falseItem.HasValue)
                    {
                        falseItem = counter;
                    }

                    counter++;
                }

                if (hasTrueATC)
                {
                    ECN_EmailActivityLog getEAL = byEmail[trueItem.Value];
                    filteredList.Add(getEAL);
                }
                else if (hasFalseATC)
                {
                    ECN_EmailActivityLog getEAL = byEmail[falseItem.Value];
                    filteredList.Add(getEAL);
                }
            }

            return filteredList;
        }

        static SqlConnection GetSqlConn()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["activity"].ToString());
            return conn;
        }

        #endregion
    }

    public enum ActionTypeCode : byte
    {
        //0=false, 1=true
        ABUSERPT_UNSUB = 0,
        bounce = 0,
        click = 1,
        conversion = 1,
        conversionRevenue = 1,
        FEEDBACK_UNSUB = 0,
        MASTSUP_UNSUB = 0,
        open = 1,
        read = 1,
        refer = 1,
        resend = 0,
        send = 0,
        subscribe = 1,
        testsend = 0
    }
}
