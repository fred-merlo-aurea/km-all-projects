using System;
using ecn.common.classes;
using System.Data;
using System.Data.SqlClient;

namespace ecn.communicator.classes
{
    public class SmartFormActivityLog : DatabaseAccessor 
    {
        /// Nullary constructor		
        public SmartFormActivityLog():base() 
        { 
        }
		
		/// ID constructor		
		/// <param name="input_id">SALID</param>
        public SmartFormActivityLog(int input_id):base(input_id) 
        {
        
        }		

        #region Private Properties

        private int _SALID;
        private int _SFID;
        private int _CustomerID;
        private int _GroupID;
        private int _EmailID;
        private string _EmailType;
        private string _EmailTo;
        private string _EmailFrom;
        private string _EmailSubject;
        private DateTime _SendTime;

        #endregion

        #region Public Field

        public int SALID
        {
            get
            {
                return (_SALID);
            }
        }
        public int SFID
        {
            get
            {
                return (_SFID);
            }
            set
            {
                _SFID = value;
            }
        }
        public int CustomerID
        {
            get
            {
                return (_CustomerID);
            }
            set
            {
                _CustomerID = value;
            }
        }
        public int GroupID
        {
            get
            {
                return (_GroupID);
            }
            set
            {
                _GroupID = value;
            }
        }
        public int EmailID
        {
            get
            {
                return (_EmailID);
            }
            set
            {
                _EmailID = value;
            }
        }
        public string EmailType
        {
            get
            {
                return (_EmailType);
            }
            set
            {
                _EmailType = value;
            }
        }
        public string EmailTo
        {
            get
            {
                return (_EmailTo);
            }
            set
            {
                _EmailTo = value;
            }
        }
        public string EmailFrom
        {
            get
            {
                return (_EmailFrom);
            }
            set
            {
                _EmailFrom = value;
            }
        }
        public string EmailSubject
        {
            get
            {
                return (_EmailSubject);
            }
            set
            {
                _EmailSubject = value;
            }
        }
        public DateTime SendTime
        {
            get
            {
                return (_SendTime);
            }
        }

        #endregion

        #region Public Methods

        public static SmartFormActivityLog GetLogByID(int salID)
        {
            if (salID <= 0)
            {
                return null;
            }

            DataTable dt = DataFunctions.GetDataTable("SELECT * from SmartFormActivityLog where SALID = " + salID, DatabaseAccessor.con_communicator);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            SmartFormActivityLog smartFormActivityLog = new SmartFormActivityLog(salID);
            DataRow row = dt.Rows[0];
            smartFormActivityLog._SALID = Convert.ToInt32(row["SALID"]);
            smartFormActivityLog._SFID = Convert.ToInt32(row["SFID"]);
            smartFormActivityLog._CustomerID = Convert.ToInt32(row["CustomerID"]);
            smartFormActivityLog._GroupID = Convert.ToInt32(row["GroupID"]);
            smartFormActivityLog._EmailID = Convert.ToInt32(row["EmailID"]);
            smartFormActivityLog._EmailType = Convert.ToString(row["EmailType"]);
            smartFormActivityLog._EmailTo = Convert.ToString(row["EmailTo"]);
            smartFormActivityLog._EmailFrom = Convert.ToString(row["EmailFrom"]);
            smartFormActivityLog._EmailSubject = Convert.ToString(row["EmailSubject"]);
            smartFormActivityLog._SendTime = Convert.ToDateTime(row["SendTime"]);

            return smartFormActivityLog;
        }

        public void Insert()
        {
            string insertSql = "Insert INTO SmartFormActivityLog (SFID, CustomerID, GroupID, EmailID, EmailType, EmailTo, EmailFrom, EmailSubject) VALUES (@SFID, @CustomerID, @GroupID, @EmailID, @EmailType, @EmailTo, @EmailFrom, @EmailSubject);SELECT @@IDENTITY";
            SqlConnection conn = GetDbConnection("communicator");
            SqlCommand insert_cmd = null;

            try
            {
                insert_cmd = new SqlCommand(insertSql, conn);
                insert_cmd.Parameters.Add(new SqlParameter("@SFID", SqlDbType.Int)).Value = _SFID;
                insert_cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int)).Value = _CustomerID;
                insert_cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = _GroupID;
                insert_cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int)).Value = _EmailID;
                insert_cmd.Parameters.Add(new SqlParameter("@EmailType", SqlDbType.VarChar, 10)).Value = _EmailType;
                insert_cmd.Parameters.Add(new SqlParameter("@EmailTo", SqlDbType.VarChar, 500)).Value = _EmailTo;
                insert_cmd.Parameters.Add(new SqlParameter("@EmailFrom", SqlDbType.VarChar, 500)).Value = _EmailFrom;
                insert_cmd.Parameters.Add(new SqlParameter("@EmailSubject", SqlDbType.VarChar, 500)).Value = _EmailSubject;
                conn.Open();
                insert_cmd.Prepare();
                int new_id = Convert.ToInt32(insert_cmd.ExecuteScalar());
                ID(new_id);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                if (insert_cmd != null)
                {
                    insert_cmd.Dispose();
                }
            }
            return;
        }

        #endregion

        #region Private Functions


        #endregion


    }
}
