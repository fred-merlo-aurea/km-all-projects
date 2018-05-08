using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

using ecn.common.classes;

namespace ecn.communicator.classes {
    
    /// Basic class that wraps around the inserting and updating of blast for the blast editor and the default.aspx of
    /// the blast directory.
    
	public class Blasts: DatabaseAccessor,IBlastLike {
		
		/// Basic Int Constructor
		
		/// <param name="input_id">BlastID</param>
		public Blasts(int input_id):base(input_id) {
			DefaultSetup();
		}
		
		/// String Constructor
		
		/// <param name="input_id"></param>
		public Blasts(string input_id):base(input_id) {
			DefaultSetup();
		}
		
		/// Nullary Constructor
		
		public Blasts():base() { DefaultSetup(); }

		
		/// Helper constructor that sets up the basic needs for the class
		
		private void DefaultSetup() {
			_layout = new Layouts();
			_group = new Groups();
			_test_blast = "n";
		}

		int _customer_id;
		int _user_id;
        int? _updated_user_id;
        DateTime? _created_date;
        DateTime? _updated_date;
		string _subject;
		string _email_from;
		string _email_from_name;
		string _reply_to;
		string _status_code;
        string _node_id;
		string _blast_type;
		string _blast_Category_Code;
		string _blast_Frequency;
		string _send_time;
		string _test_blast;
		DateTime _send_time_real;
		string _end_time;
		Groups _group;
		Layouts _layout;
		int _filter_id;
        int? _smart_segment_id;
        int? _sample_id;
        //string _refsegment = string.Empty;
		string _blastSuppressionlist;
		bool _optoutPreference= false;
        bool _hasEmailPreview = false;

        /* for Email Personalization - Ashok 01/12/09*/
        string _dynamic_email_from;
        string _dynamic_email_from_name;
        string _dynamic_reply_to;

        //for scheduler
        int? _blastScheduleID = null;
        bool? _overrideIsAmount = null;
        int? _overrideAmount = null;


		// General Gets and sets from the DB

        //for scheduler
        public int? BlastScheduleID
        {
            get { return _blastScheduleID; }
            set { _blastScheduleID = value; }
        }
        public bool? OverrideIsAmount
        {
            get { return _overrideIsAmount; }
            set { _overrideIsAmount = value; }
        }
        public int? OverrideAmount
        {
            get { return _overrideAmount; }
            set { _overrideAmount = value; }
        }

		public void CustomerID(int cid) 
		{
			_customer_id = cid;
		}
		public int CustomerID() {

			if(ID() != 0) {
				try {
                    _customer_id = getCustomerIDFromBlastFromCache(ID());
				} catch {

				}
			}
			return _customer_id;
		}
		public void UserID(int uid) {
			_user_id = uid;
		}
        public void UpdatedUserID(int? uuid)
        {
            _updated_user_id = uuid;
        }
		public void Subject(string sbj) {
			_subject = sbj;
		}
		public string Subject() {
			return _subject;
		}

		public void EmailFrom(string eml_frm) {
			_email_from = eml_frm;
		}
		public string EmailFrom() {
			return _email_from;
		}

		public bool OptoutPreference 
		{
			get {return _optoutPreference;}
			set { _optoutPreference = value;}
		}

        public bool HasEmailPreview
        {
            get { return _hasEmailPreview; }
            set { _hasEmailPreview = value; }
        }

		public string BlastSuppressionlist 
		{
			get {return _blastSuppressionlist;}
			set { _blastSuppressionlist = value;}
		}

		public void EmailFromName(string eml_frm_nm) {
			_email_from_name = eml_frm_nm;
		}
		public string EmailFromName() {
			return _email_from_name;
		}

        public void ReplyTo(string rply) {
            _reply_to = rply;
        }
		public string ReplyTo() {
			return _reply_to;
		}
        public void Group(Groups gid) {
            _group = gid;
        }
        public void Layout(Layouts lid) {
            _layout = lid;
        }		
		public Layouts Layout() {
			return _layout;
		}
		public Layouts CurrentLayout {
			get { return _layout;}
		}
        public void FilterID(int fid) {
            _filter_id = fid;
        }
        public void SampleID(int? sid)
        {
            _sample_id = sid;
        }
        public void SmartSegmentID(int? ssid)
        {
            _smart_segment_id = ssid;
        }
        public void StatusCode (string code) {
            _status_code = code;
        }

        public void NodeID(string nodeID)
        {
            _node_id = nodeID;
        }

        public void BlastType(string type) {
            _blast_type = type;
        }
		public void BlastCodeID(string blastCodeID) {
			_blast_Category_Code = blastCodeID;
		}
		public void BlastFrequency(string blastFreq) {
			_blast_Frequency = blastFreq;
		}
        public void SendTime(string time) {
            _send_time = time;
        }
        public DateTime SendTime() {
            return  _send_time_real;
        }

        public void CreatedDate(DateTime? createdDate)
        {
            _created_date = createdDate;
        }

        public void UpdatedDate(DateTime? updatedDate)
        {
            _updated_date = updatedDate;
        }

        public void TestBlast(string is_test) 
        {
            _test_blast = is_test;
        }

        /* for Email Personalization - Ashok 01/12/09*/
        public void DynamicEmailFromName(string dynamic_eml_frm_nm) {
            _dynamic_email_from_name = dynamic_eml_frm_nm;
        }
        public void DynamicEmailFrom(string dynamic_eml_frm) {
            _dynamic_email_from = dynamic_eml_frm;
        }
        public void DynamicReplyTo(string dynamic_rplyTo) {
            _dynamic_reply_to = dynamic_rplyTo;
        }

        private int getCustomerIDFromBlastFromCache(int BlastID)
        {
            Dictionary<int, int> dCustomerIDforBlast = new Dictionary<int, int>();
            int CustomerID = 0;

            if (System.Web.HttpContext.Current == null)
            {
                return getCustomerIDFromBlast(BlastID);
            }
            else
            {
                if (System.Web.HttpContext.Current.Cache["BlastCustomerIDs"] == null)
                {
                    CustomerID = getCustomerIDFromBlast(BlastID);

                    dCustomerIDforBlast.Add(BlastID, CustomerID);

                    System.Web.HttpContext.Current.Cache.Add("BlastCustomerIDs", dCustomerIDforBlast, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    dCustomerIDforBlast = (Dictionary<int, int>)System.Web.HttpContext.Current.Cache["BlastCustomerIDs"];

                    if (dCustomerIDforBlast.ContainsKey(BlastID))
                    {
                        CustomerID = dCustomerIDforBlast[BlastID];
                    }
                    else
                    {
                        CustomerID = getCustomerIDFromBlast(BlastID);

                        if (!dCustomerIDforBlast.ContainsKey(BlastID))
                        {
                            dCustomerIDforBlast.Add(BlastID, CustomerID);
                        }
                    }
                }
                return CustomerID;
            }
        }

        private int getCustomerIDFromBlast(int BlastID)
        {
            try
            {
                if (BlastID > 0)
                    return Convert.ToInt32(DataFunctions.ExecuteScalar("Select CustomerID from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'"));
                else
                    return 0;
            }
            catch
            { 
                return 0;
            }
        }

        public int SuccessTotal() {
            int SuccessTotal = 0;
            if(ID() != 0) {
                try {
                    SuccessTotal = Convert.ToInt32(DataFunctions.ExecuteScalar("Select SuccessTotal from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'"));
                } catch {

                }
            }
            return SuccessTotal;
        }
        public int AttemptTotal() 
        {
            int AttemptTotal = 0;
            if(ID() != 0) 
            {
                try 
                {
                    AttemptTotal = Convert.ToInt32(DataFunctions.ExecuteScalar("Select AttemptTotal from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'"));
                } 
                catch 
                {

                }
            }
            return AttemptTotal;
        }
        public int SendTotal() 
        {
            int SendTotal = 0;
            if(ID() != 0) 
            {
                try 
                {
                    SendTotal = Convert.ToInt32(DataFunctions.ExecuteScalar("Select SendTotal from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'"));
                } 
                catch 
                {

                }
            }
            return SendTotal;
        }

        //public string RefSegment
        //{
        //    get
        //    {
        //        if (_refsegment == "" && ID() > 0)
        //        {
        //            _refsegment = Convert.ToString(DataFunctions.ExecuteScalar("SELECT RefSegment from Blasts where BlastID = " + ID().ToString()));
        //        }
        //        return (this._refsegment);
        //    }
        //    set
        //    {
        //        this._refsegment = value;
        //    }
        //}


		private string _refBlastID="-1";
		public string RefBlastID {
			get {
				if (_refBlastID == "-1" && ID() > 0) {
                    _refBlastID = Convert.ToString(DataFunctions.ExecuteScalar("SELECT RefBlastID from Blast where BlastID = " + ID().ToString() + " and StatusCode <> 'Deleted'"));
				}
				return (this._refBlastID);
			}
			set {
				this._refBlastID = value;
			}
		}

        public void SetActive() 
        {
            DataFunctions.Execute("Update Blast set StatusCode='Active' where BlastID=" + ID());
        }
		
		/// Creates a sample blast

        //public void CreateSampleBlast()
        //{

        //    StatusCode("Pending");
        //    BlastType("Sample");
        //    Create();
        //}

		
		/// Crates a champion type blast

        //public void CreateChampionBlast()
        //{

        //    StatusCode("Pending");
        //    BlastType("Champion");
        //    Create();
        //}

		
		/// Creates a master cample blast that links sample blasts

        //public void CreateMasterSampleBlast()
        //{

        //    StatusCode("System");
        //    BlastType("mastersample");
        //    Create();
        //}

		
		/// Creates a layout plan blast.

        //public void CreateLayoutPlanBlast()
        //{

        //    StatusCode("system");
        //    BlastType("layout");
        //    Create();
        //}

		
		/// Creates a layout plan blast.

        //public void CreateTriggerPlanBlast()
        //{

        //    StatusCode("system");
        //    BlastType("noopen");
        //    Create();
        //}

		
		/// The usual type of "blastnow" blasts.

        //public void CreateRegularBlast()
        //{
        //    StatusCode("pending");
        //    BlastType("html");
        //    Create();
        //}

        //public void CreateRegularBlastDripTest(string nodeID)
        //{
        //    StatusCode("dripTest");
        //    BlastType("html");
        //    NodeID(nodeID);
        //    Create();
        //}

		/// The usual type of "blastnow" blasts  & a blast Type Specified.
        // Added by Ashok 11/18/08 for Canon TS changes.
        //public void CreateRegularBlast(string blastType)
        //{
        //    //SendTime(DateTime.Now.ToString());
        //    StatusCode("pending");
        //    BlastType(blastType);
        //    Create();
        //}

        

		
		/// Usual type of blasts with no time set.

        //public void CreateBlast()
        //{
        //    StatusCode("pending");
        //    BlastType("html");
        //    Create();
        //}

        /// Usual type of blasts with no time set & a blast Type Specified.
        // Added by Ashok 11/18/08 for Canon TS changes.

        //public void CreateBlast(string blastType)
        //{
        //    StatusCode("pending");
        //    BlastType(blastType);
        //    Create();
        //}
		
		/// Autoresponder type blast.

        //public void CreateAutoResponder()
        //{
        //    StatusCode("system");
        //    BlastType("autoresponder");
        //    Create();
        //}


		
		/// Resets all of the variables in the object from the ID() using the database.
		
        public void ResetFromDb() {
            DataTable dt = DataFunctions.GetDataTable("Select * from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'");
            DataRow dr = dt.Rows[0];
            _customer_id				= (int)dr["CustomerID"];
            _user_id = (int)dr["CreatedUserID"];
            if (!DBNull.Value.Equals(dr["UpdatedUserID"]))
                _updated_user_id = Convert.ToInt32(dr["UpdatedUserID"]);
            else
                _updated_user_id = null;
            if (!DBNull.Value.Equals(dr["CreatedDate"]))
                _created_date = Convert.ToDateTime(dr["CreatedDate"]);
            else
                _created_date = null;
            if (!DBNull.Value.Equals(dr["UpdatedDate"]))
                _updated_date = Convert.ToDateTime(dr["UpdatedDate"]);
            else
                _updated_date = null;
            if (!DBNull.Value.Equals(dr["SampleID"]))
                _sample_id = Convert.ToInt32(dr["SampleID"]);
            else
                _sample_id = null;
            if (!DBNull.Value.Equals(dr["SmartSegmentID"]))
                _smart_segment_id = Convert.ToInt32(dr["SmartSegmentID"]);
            else
                _smart_segment_id = null;
            _subject						= (string)dr["EmailSubject"];
            _email_from					= (string)dr["EmailFrom"];
            _email_from_name		= (string)dr["EmailFromName"];
            _send_time					= dr["SendTime"].ToString();
            _send_time_real			= (DateTime) dr["SendTime"];
            _end_time					= dr["FinishTime"].ToString();
            _status_code				= (string)dr["StatusCode"];
            _blast_type					= (string)dr["BlastType"];
			_blast_Category_Code	= dr["CodeID"].ToString();
			_blast_Frequency			= (string)dr["BlastFrequency"];
            int groupID = 0;
            int.TryParse(dr["GroupID"].ToString(), out groupID);
            if(groupID > 0)
                _group = new Groups(groupID);
            int layoutID = 0;
            int.TryParse(dr["LayoutID"].ToString(), out layoutID);
            if (layoutID > 0)
                _layout = new Layouts(layoutID);
            int filterID = 0;
            int.TryParse(dr["FilterID"].ToString(), out filterID);
            if (filterID > 0)
                _filter_id = filterID;
            _reply_to					= (string)dr["ReplyTo"];
			_optoutPreference =		dr["AddOptOuts_to_MS"] == DBNull.Value?false:Convert.ToBoolean(dr["AddOptOuts_to_MS"]);
			_blastSuppressionlist = dr["BlastSuppression"] == DBNull.Value?string.Empty:dr["BlastSuppression"].ToString();
            _hasEmailPreview = dr["HasEmailPreview"] == DBNull.Value ? false : Convert.ToBoolean(dr["HasEmailPreview"]);

			_subject						= _subject.Replace("'", "''");
			_email_from					= _email_from.Replace("'", "''");
			_email_from_name		= _email_from_name.Replace("'", "''");
            //for scheduler
            if (!DBNull.Value.Equals(dr["BlastScheduleID"]))
                _blastScheduleID = (int)dr["BlastScheduleID"];
            else
                _blastScheduleID = null;
            if (!DBNull.Value.Equals(dr["OverrideAmount"]))
                _overrideAmount = (int)dr["OverrideAmount"];
            else
                _overrideAmount = null;
            if (!DBNull.Value.Equals(dr["OverrideIsAmount"]))
                _overrideIsAmount = (bool)dr["OverrideIsAmount"];
            else
                _overrideIsAmount = null;

            /* for Email Personalization - Ashok 01/12/09*/
            _dynamic_email_from = dr["DynamicFromEmail"].ToString();
            _dynamic_email_from_name = dr["DynamicFromName"].ToString();
            _dynamic_reply_to = dr["DynamicReplyToEmail"].ToString();
        }

        
		
		/// Add successes for a single blast for reporting.
		
        public void Inc() 
        {
            int success = SuccessTotal() + 1;
            int send = SendTotal() + 1;
            int attempt = AttemptTotal() + 1;

            DataFunctions.Execute("Update Blast set SuccessTotal = " + success + " , SendTotal = " + send + " , AttemptTotal = " + attempt + " WHERE BlastID = " + ID());
        }

		
		/// Determines what the click percentages is for this particular blast		
		/// <returns>Click percentage on this blast</returns>
		public Decimal GetClickPercent() {
            string sql_click = " SELECT COUNT(DISTINCT EmailID) FROM BlastActivityClicks WHERE BlastID = " + ID().ToString() + " and StatusCode <> 'Deleted'";

            string sqlbouncesunique = " SELECT COUNT(DISTINCT EmailID) FROM BlastActivityBounces WHERE BlastID = " + ID().ToString() + " and StatusCode <> 'Deleted'";

            Decimal bouncesuniquecount = Convert.ToDecimal(DataFunctions.ExecuteScalar(sqlbouncesunique));
            int click_count = Convert.ToInt32(DataFunctions.ExecuteScalar(sql_click));
            int total_send = Convert.ToInt32(Convert.ToDecimal(SuccessTotal()) - bouncesuniquecount);
            Decimal my_percent= 0;
            if(total_send > 0) {
                my_percent = Convert.ToDecimal(click_count) / Convert.ToDecimal(total_send);
            }
            //throw new Exception("click_count= " + click_count + " total_send = " + total_send + " my_percent = " + my_percent.ToString());
            return my_percent;
        }

		
		/// Get a SQL datarow on the db values of this blast.
		
		/// <returns>Datarow of Select* on blast </returns>
        public DataRow GetBlastRow() {
            string sqlBlastQuery=
                " SELECT * "+
                " FROM Blast "+
                " WHERE BlastID=" + ID() + " and StatusCode <> 'Deleted'" + " ";
            DataTable dt = DataFunctions.GetDataTable(sqlBlastQuery);
            return dt.Rows[0];
        }

		
		/// Finds the number of sends for this sample.
		
		/// <returns></returns>
        //public int SampleCount() {
        //    int count = 0;
        //    if(ID() != 0) {
        //        try {
        //            count = Convert.ToInt32(DataFunctions.ExecuteScalar("Select OverrideAmount from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'"));
        //        } catch {

        //        }
        //    }
        //    return count;
        //}

		
		/// Returns our sample for this blast.
		
		/// <returns>Sample object or null</returns>
        //public Samples Sample() {
        //    object potential_sample = DataFunctions.ExecuteScalar("Select SampleID from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted'");
        //    if(potential_sample != null) {
        //        return new Samples((int) potential_sample); 
        //    } else {
        //        return null;
        //    }
        //}

		
		/// Returns this blast's champion.
		
		/// <returns>Champion Sample object or null</returns>
        //public Samples ChampionSample() 
        //{
        //    object potential_sample = DataFunctions.ExecuteScalar("Select SampleID from Blast where BlastID=" + ID() + " and StatusCode <> 'Deleted' and BlastType = 'Champion'");
        //    if(potential_sample != null) 
        //    {
        //        return new Samples((int) potential_sample); 
        //    } 
        //    else 
        //    {
        //        return null;
        //    }
        //}

		
		/// Given a sample, register this blast as the champion of that sample
		
		/// <param name="sample">Sample object to make this blast champion of</param>
        //public void RegisterChampion(Samples sample) 
        //{
        //    DataFunctions.Execute("Insert into ChampionBlasts (BlastID,SampleID) Values ( " + ID() + "  , " + sample.ID() + " )" );
        //}

		public static Blasts GetBlastByID(int blastID) {
			string sqlQuery=
				" SELECT * "+
				" FROM Blast "+
                " WHERE BlastID=" + blastID + " and StatusCode <> 'Deleted'" + " ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			if (dt.Rows.Count == 0) {
				return null;
			}
			
			Blasts blast = new Blasts();
			blast.ID(blastID);
			blast.Subject(dt.Rows[0]["EmailSubject"].ToString());
			blast.EmailFromName(dt.Rows[0]["EmailFromName"].ToString());
			blast.ReplyTo(dt.Rows[0]["ReplyTo"].ToString());
			blast.EmailFrom(dt.Rows[0]["EmailFrom"].ToString());
			blast.SendTime(dt.Rows[0]["SendTime"].ToString());

            if (!DBNull.Value.Equals(dt.Rows[0]["GroupID"]))
                blast.Group(new Groups(Convert.ToInt32(dt.Rows[0]["GroupID"].ToString())));
            else
                blast.Group(new Groups(Convert.ToInt32("0")));

			//blast.Group(new Groups(Convert.ToInt32(dt.Rows[0]["GroupID"].ToString())));
			blast.Layout(new Layouts(Convert.ToInt32(dt.Rows[0]["LayoutID"].ToString())));
			blast.OptoutPreference = dt.Rows[0]["AddOptOuts_to_MS"] == DBNull.Value?false:Convert.ToBoolean(dt.Rows[0]["AddOptOuts_to_MS"]);
			blast.BlastSuppressionlist = dt.Rows[0]["BlastSuppression"] == DBNull.Value?string.Empty:dt.Rows[0]["BlastSuppression"].ToString();
            //for scheduler
            if (!DBNull.Value.Equals(dt.Rows[0]["BlastScheduleID"]))
                blast.BlastScheduleID = (int)dt.Rows[0]["BlastScheduleID"];
            else
                blast.BlastScheduleID = null;
            if (!DBNull.Value.Equals(dt.Rows[0]["OverrideAmount"]))
                blast.OverrideAmount = (int)dt.Rows[0]["OverrideAmount"];
            else
                blast.OverrideAmount = null;
            if (!DBNull.Value.Equals(dt.Rows[0]["OverrideIsAmount"]))
                blast.OverrideIsAmount = (bool)dt.Rows[0]["OverrideIsAmount"];
            else
                blast.OverrideIsAmount = null;

			return blast;
		}

        public static DataSet GetBlastReportDetails( int ID ,string IsBlastGroup , string ReportType , string FilterType , string ISP , int PageNo, int PageSize, string UDFName, string UDFData)
        {
            SqlCommand cmd = new SqlCommand("spGetBlastGroupReportWithSuppressed");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
            cmd.Parameters["@ID"].Value = ID;

            cmd.Parameters.Add(new SqlParameter("@IsBlastGroup", SqlDbType.VarChar));
            cmd.Parameters["@IsBlastGroup"].Value = IsBlastGroup;	

			cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
            cmd.Parameters["@ReportType"].Value = ReportType;		

			cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
            cmd.Parameters["@FilterType"].Value = FilterType;

			cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
            cmd.Parameters["@ISP"].Value = ISP;	

			cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int));
            cmd.Parameters["@PageNo"].Value = PageNo;	

			cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
            cmd.Parameters["@PageSize"].Value = PageSize;

            cmd.Parameters.Add(new SqlParameter("@UDFname", SqlDbType.VarChar));
            cmd.Parameters["@UDFname"].Value = UDFName;

            cmd.Parameters.Add(new SqlParameter("@UDFdata", SqlDbType.VarChar));
            cmd.Parameters["@UDFdata"].Value = UDFData;

            return DataFunctions.GetDataSet("activity", cmd); 
        }

        public static DataTable DownloadBlastReportDetails(int ID, bool IsBlastGroup, string ReportType, string FilterType, string ISP)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            if (IsBlastGroup)
            {
                cmd.CommandText = "spDownloadBlastGroupDetails";

                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                cmd.Parameters["@ID"].Value = ID;

                cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
                cmd.Parameters["@ReportType"].Value = ReportType;

                cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
                cmd.Parameters["@FilterType"].Value = FilterType;

                cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
                cmd.Parameters["@ISP"].Value = ISP;
            }
            else
            {
                cmd.CommandText = "spDownloadBlastEmails";

                cmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.VarChar));
                cmd.Parameters["@blastID"].Value = ID;

                cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
                cmd.Parameters["@ReportType"].Value = ReportType;

                cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
                cmd.Parameters["@FilterType"].Value = FilterType;

                cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
                cmd.Parameters["@ISP"].Value = ISP;
            }

            return DataFunctions.GetDataTable("activity", cmd);
        }

        //public void Update() {
        //    if (ID() <=0) {
        //        throw new ApplicationException(string.Format("Can't update blast with ID '{0}'.", ID()));
        //    }

        //    string sqlquery=
        //        " UPDATE Blast SET "+
        //        " EmailSubject='"+ Subject() +"', "+
        //        " EmailFrom='"+ EmailFrom() +"', "+
        //        " EmailFromName='"+ EmailFromName() +"', "+				
        //        " GroupID='"+ _group.ID() +"', "+
        //        " LayoutID="+ _layout.ID() +", "+
        //        " ReplyTo= '"+ ReplyTo() + "', " +
        //       //" RefSegment= '" + RefSegment + "', " +
        //        " RefBlastID =" + RefBlastID + ","+
        //        " FilterID = "+ _filter_id +", "+
        //        " AddOptOuts_to_MS = "+ (_optoutPreference?"1":"0") + ", " +
        //        " HasEmailPreview = " + (_hasEmailPreview ? "1" : "0") + ", " +
        //        " BlastSuppression = '"+  _blastSuppressionlist +"' "+
        //        " WHERE BlastID="+ ID() +" ";
        //    DataFunctions.Execute(sqlquery);
        //}
    }
}
