using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using ecn.communicator.classes;

namespace ecn.accounts.classes
{
    public class Lead : DatabaseAccessor
    {
        #region Properties
        private int _emailID;
        public int EmailID
        {
            get
            {
                return (this._emailID);
            }
            set
            {
                this._emailID = value;
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return (this._firstName);
            }
            set
            {
                this._firstName = value;
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return (this._lastName);
            }
            set
            {
                this._lastName = value;
            }
        }

        private string _company;
        public string Company
        {
            get
            {
                return (this._company);
            }
            set
            {
                this._company = value;
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get
            {
                return (this._emailAddress);
            }
            set
            {
                this._emailAddress = value;
            }
        }

        private string _phone;
        public string Phone
        {
            get
            {
                return (this._phone);
            }
            set
            {
                this._phone = value;
            }
        }

        private DateTime _sendDate;
        public DateTime SendDate
        {
            get
            {
                return (this._sendDate);
            }
            set
            {
                this._sendDate = value;
            }
        }

        private DateTime _demoSignUpDate;
        public DateTime DemoSignUpDate
        {
            get
            {
                return (this._demoSignUpDate);
            }
            set
            {
                this._demoSignUpDate = value;
            }
        }


        private DateTime _demoDate;
        public DateTime DemoDate
        {
            get
            {
                return (this._demoDate);
            }
            set
            {
                this._demoDate = value;
            }
        }


        private int _staffID;
        public int StaffID
        {
            get
            {
                return (this._staffID);
            }
            set
            {
                this._staffID = value;
            }
        }

        private DateTime _openDate;
        public DateTime OpenDate
        {
            get
            {
                return (this._openDate);
            }
            set
            {
                this._openDate = value;
            }
        }

        private DateTime _clickDate;
        public DateTime ClickDate
        {
            get
            {
                return (this._clickDate);
            }
            set
            {
                this._clickDate = value;
            }
        }


        private string _surveyAnswer1;
        public string SurveyAnswer1
        {
            get
            {
                return (this._surveyAnswer1);
            }
            set
            {
                this._surveyAnswer1 = value;
            }
        }

        private string _surveyAnswer2;
        public string SurveyAnswer2
        {
            get
            {
                return (this._surveyAnswer2);
            }
            set
            {
                this._surveyAnswer2 = value;
            }
        }

        private string _surveyAnswer3;
        public string SurveyAnswer3
        {
            get
            {
                return (this._surveyAnswer3);
            }
            set
            {
                this._surveyAnswer3 = value;
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return (this._status);
            }
            set
            {
                this._status = value;
            }
        }


        public bool HasDemo
        {
            get { return DemoDate != DateTime.MinValue; }
        }
        #endregion

        // Lead are stored in the email profile
        public Lead GetLeadByEmailID(int emailID)
        {
            SqlConnection conn = GetDbConnection("communicator");
            string sql = string.Format("select * from Emails where emailID = {0}", emailID);
            DataTable dt = DataFunctions.GetDataTable(sql, conn.ConnectionString);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return GetLead(dt.Rows[0]);
        }

        public ArrayList GetLeadsByStafffID(int staffID)
        {
            ArrayList leads = new ArrayList();
            SqlConnection conn = GetDbConnection("communicator");
            string sql = string.Format("select * from Emails e join EmailGroups g on e.EmailID = g.EmailID where g.GroupID = {0} and e.User6 = '{1}'", LeadConfig.DemoInviteGroupID, staffID);
            DataTable dt = DataFunctions.GetDataTable(sql, conn.ConnectionString);
            foreach (DataRow row in dt.Rows)
            {
                leads.Add(GetLead(row));
            }
            return leads;
        }

        public ArrayList GetLeadsByStafffIDAndDateRange(int staffID, DateTime start, DateTime end)
        {
            ArrayList leads = new ArrayList();
            SqlConnection conn = GetDbConnection("communicator");
            string sql = string.Format("select * from Emails e join EmailGroups g on e.EmailID = g.EmailID where g.GroupID = {0} and e.User6 = '{1}' and convert(datetime, UserEvent1Date) > '{2}' and convert(datetime, UserEvent1Date) < '{3}'", LeadConfig.DemoInviteGroupID, staffID, start.ToShortDateString(), end.ToShortDateString());
            DataTable dt = DataFunctions.GetDataTable(sql, conn.ConnectionString);
            foreach (DataRow row in dt.Rows)
            {
                leads.Add(GetLead(row));
            }
            return leads;
        }

        public ArrayList GetLeadsWithDemo(DateTime start, DateTime end)
        {
            ArrayList leads = new ArrayList();
            SqlConnection conn = GetDbConnection("communicator");
            string sql = string.Format("select * from Emails e join EmailGroups eg on e.EmailID = eg.EmailID where eg.GroupID = {0} and UserEvent2Date is not null and convert(datetime, UserEvent2Date) > '{1}' and convert(datetime, UserEvent2Date) < '{2}'", LeadConfig.DemoInviteGroupID, start.ToShortDateString(), end.ToShortDateString());
            DataTable dt = DataFunctions.GetDataTable(sql, conn.ConnectionString);
            foreach (DataRow row in dt.Rows)
            {
                leads.Add(GetLead(row));
            }
            return leads;
        }

        public ArrayList GetLeadsWithDemoSetupInfo(DateTime start, DateTime end)
        {
            ArrayList leads = new ArrayList();
            SqlConnection conn = GetDbConnection("communicator");
            string sql = string.Format("select * from Emails e join EmailGroups eg on e.EmailID = eg.EmailID where eg.GroupID = {0} and UserEvent2Date is not null and convert(datetime, UserEvent2Date) > '{1}' and convert(datetime, UserEvent2Date) < '{2}' and User2 <> ''", LeadConfig.DemoInviteGroupID, start.ToShortDateString(), end.ToShortDateString());
            DataTable dt = DataFunctions.GetDataTable(sql, conn.ConnectionString);
            foreach (DataRow row in dt.Rows)
            {
                leads.Add(GetLead(row));
            }
            return leads;
        }

        private Lead GetLead(DataRow row)
        {
            Lead lead = new Lead();
            lead.EmailID = Convert.ToInt32(row["EmailID"]);
            lead.EmailAddress = Convert.ToString(row["EmailAddress"]);
            lead.SendDate = row["UserEvent1Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["UserEvent1Date"]);
            lead.DemoDate = row["UserEvent2Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["UserEvent2Date"]);
            lead.DemoSignUpDate = row["User5"] == DBNull.Value || Convert.ToString(row["User5"]).Trim() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(row["User5"]);
            lead.FirstName = Convert.ToString(row["FirstName"]);
            lead.LastName = Convert.ToString(row["LastName"]);
            lead.Company = Convert.ToString(row["Company"]);
            lead.Phone = Convert.ToString(row["Voice"]);
            lead.StaffID = Convert.ToInt32(row["User6"]);
            lead.Status = Convert.ToString(row["UserEvent2"]);
            lead.OpenDate = GetOpenDate(lead.EmailID);
            lead.ClickDate = GetClickDate(lead.EmailID);
            return lead;
        }

        private DateTime GetOpenDate(int emailID)
        {
            return GetActionDate(emailID, "open");
        }

        private DateTime GetClickDate(int emailID)
        {
            return GetActionDate(emailID, "click");
        }

        private DateTime GetActionDate(int emailID, string action)
        {
            string sql = string.Empty;
            switch (action.ToLower())
            {
                case "click":
                    sql = string.Format("select top 1 ClickTime from BlastActivityClicks where EmailID = {0} and BlastID = {1}", emailID, LeadConfig.BlastID);
                    break;
                case "open":
                    sql = string.Format("select top 1 OpenTime from BlastActivityOpens where EmailID = {0} and BlastID = {1}", emailID, LeadConfig.BlastID);
                    break;
                default:
                    break;
            }
            //string sql = string.Format("select top 1 ActionDate from EmailActivityLog where EmailID = {0} and BlastID = {1} and ActionTypeCode = '{2}'", emailID, LeadConfig.BlastID,action);
            object date = DataFunctions.ExecuteScalar("activity", sql);
            if (date == DBNull.Value)
            {
                return DateTime.MinValue;
            }
            return Convert.ToDateTime(date);
        }

    }

    #region Lead Comparers
    public class LeadCompanyComparer : IComparer
    {
        bool _isDesc;
        public LeadCompanyComparer(bool isDesc)
        {
            _isDesc = isDesc;
        }
        #region IComparer Members

        public int Compare(object x, object y)
        {
            Lead l1 = x as Lead;
            Lead l2 = y as Lead;

            if (l1 == null || l2 == null)
            {
                return 0;
            }
            return !_isDesc ? l1.Company.CompareTo(l2.Company) : l2.Company.CompareTo(l1.Company);
        }
        #endregion
    }

    public class LeadSendDateComparer : IComparer
    {
        bool _isDesc;
        public LeadSendDateComparer(bool isDesc)
        {
            _isDesc = isDesc;
        }
        #region IComparer Members

        public int Compare(object x, object y)
        {
            Lead l1 = x as Lead;
            Lead l2 = y as Lead;

            if (l1 == null || l2 == null)
            {
                return 0;
            }
            return !_isDesc ? l1.SendDate.CompareTo(l2.SendDate) : l2.SendDate.CompareTo(l1.SendDate);
        }
        #endregion
    }

    public class LeadOpenDateComparer : IComparer
    {
        bool _isDesc;
        public LeadOpenDateComparer(bool isDesc)
        {
            _isDesc = isDesc;
        }
        #region IComparer Members

        public int Compare(object x, object y)
        {
            Lead l1 = x as Lead;
            Lead l2 = y as Lead;

            if (l1 == null || l2 == null)
            {
                return 0;
            }
            return !_isDesc ? l1.OpenDate.CompareTo(l2.OpenDate) : l2.OpenDate.CompareTo(l1.OpenDate);
        }
        #endregion
    }

    public class LeadClickDateComparer : IComparer
    {
        bool _isDesc;
        public LeadClickDateComparer(bool isDesc)
        {
            _isDesc = isDesc;
        }
        #region IComparer Members

        public int Compare(object x, object y)
        {
            Lead l1 = x as Lead;
            Lead l2 = y as Lead;

            if (l1 == null || l2 == null)
            {
                return 0;
            }
            return !_isDesc ? l1.ClickDate.CompareTo(l2.ClickDate) : l2.ClickDate.CompareTo(l1.ClickDate);
        }
        #endregion
    }

    public class LeadDemoDateComparer : IComparer
    {
        bool _isDesc;
        public LeadDemoDateComparer(bool isDesc)
        {
            _isDesc = isDesc;
        }
        #region IComparer Members

        public int Compare(object x, object y)
        {
            Lead l1 = x as Lead;
            Lead l2 = y as Lead;

            if (l1 == null || l2 == null)
            {
                return 0;
            }
            return !_isDesc ? l1.DemoDate.CompareTo(l2.DemoDate) : l2.DemoDate.CompareTo(l1.DemoDate);
        }
        #endregion
    }
    #endregion
}
