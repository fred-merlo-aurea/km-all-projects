using System;
using System.Collections;
using System.Data;
using ecn.common.classes;
using System.Configuration;
using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;


namespace ecn.communicator.classes {
    
    /// A layout plan is a certain set of preconditions that must be met for a particular layout in order to generate
    /// a customer event. EventOrganizer handles the matching of criteria but this class sets up the criteria in the DB
    
	public class LayoutPlans: DatabaseAccessor { 
		int _customer_id;
		Layouts _layout;
		double _period;
		Blasts _blast;
		Groups _group;
		string _type;
		string _criteria;
		string _name;
		string _status;
        int _smartformID;

		
		/// gets a layout plan with a specific db id and sets up some objects for internal use
		
		/// <param name="input_id">Id of the layoutplan in the database</param>
		public LayoutPlans(int input_id):base(input_id) {
			DefaultSetup();
		}
		
		/// String version of the Int version
		
		/// <param name="input_id"> Ids in the db</param>
		public LayoutPlans(string input_id):base(input_id) {
			DefaultSetup();
		}
		
		/// Nullary constructor. Sets up some default objects.
		
		public LayoutPlans():base() { 
			DefaultSetup();
		}
		
		/// Helper fuction for creation. Makes objects that the system needs.
		
		private void DefaultSetup() {
			_blast = new Blasts();
			_layout = new Layouts();
			_group = new Groups();
			_period = 0.0;
		}

		
		/// Sets Customer ID on the object
		
		/// <param name="cid">DB customer id</param>
		public void CustomerID(int cid) {
			_customer_id = cid;
		}
		
		/// Sets our internal blast object to be the same as the one passed in.
		
		/// <param name="blast"> Blast to copy</param>
		public void Blast(Blasts blast) {
			_blast=blast;
		}

		
		/// Gets Blast from the Database.
		
		/// <returns>Returns this layout plans's blast if any</returns>
		public Blasts Blast() {
			Blasts my_blast = new Blasts();
			if(ID() != 0) {
				my_blast.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "Select BlastID from LayoutPlans where LayoutPlanID=" + ID() + " and IsDeleted = 0 ")));
				return my_blast;
			} 
			return null;
		}
		
		/// Updates the layout object
		
		/// <param name="lid">General layout object to attach</param>
		public void Layout(Layouts lid) {
			_layout=lid;
		}
		public Layouts Layout() {
			return _layout;
		}
	

		
		/// Updates our group (if any)
		
		/// <param name="group"> Group to use, ID 0 allowed</param>
        public void Group(Groups group) 
        {
            _group=group;
        }	

		
		/// Gets our group for this plan from the database
		
		/// <returns>Group that attaches to layoutplan or groupid 0</returns>
        public Groups Group() 
        {
			if (_group == null || _group.ID() == 0 ) {            
				if(ID() != 0) {
                    _group.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "Select GroupID from LayoutPlans where LayoutPlanID=" + ID() + " and IsDeleted = 0 ")));              
				}
			}            
            return _group;
        }
		
		/// Set the criteria we will use to match this event
		
		/// <param name="crit"></param>
        public void Criteria(string crit) 
        {
            _criteria=crit;
        }

	    
	    /// returns the criteria we set in the db
	    
	    /// <returns>string to match</returns>
        public string Criteria() 
        {

            if(ID() != 0) 
            {
                try 
                {
                    _criteria = DataFunctions.ExecuteScalar("communicator", "Select Criteria from LayoutPlans where LayoutPlanID=" + ID() + " and IsDeleted = 0 ").ToString();
                } 
                catch 
                {

                }
            }
            return _criteria;
        }

		
		/// The name of this type of action
		
		/// <param name="name">action name</param>
        public void ActionName(string name) 
        {
            _name = name;
        }

		
		/// How long in days it will happen after the event
		
		/// <param name="per"> period in days</param>
        public void Period(double per) {
            _period = per;
        }
		
		/// Get the period from the DB
		
		/// <returns> period from the db</returns>
        public double Period() {

            if(ID() != 0) {
                try {
                    _period = Convert.ToDouble(DataFunctions.ExecuteScalar("communicator", "Select Period from LayoutPlans where LayoutPlanID=" + ID() + " and IsDeleted = 0 "));
                } catch {

                }
            }
            return _period;
        }

		
		/// Type of event (such as click, open, subscribe... etc) Should be an enumerated type
		
		/// <param name="type"> event type, needs enumeration</param>
        public void EventType(string type) {
            _type= type;
        }
		public string EventType() {
			return _type;
		}

		#region Wrapper Properties
		public int BlastID 
		{
			get {
				return Blast().ID();
			}
		}

		public int LayoutPlanID {
			get {
				return ID();
			}
		}
		public string Name {
			get { return _name;}
		}

		private string _layoutName;
		public string LayoutName {
			get {
				return (this._layoutName);
			}
			set {
				this._layoutName = value;
			}
		}

		private string _responseLayoutName;
		public string ResponseLayoutName {
			get {
				return (this._responseLayoutName);
			}
			set {
				this._responseLayoutName = value;
			}
		}	
	
		private string _noopRptLnk;
		public string NOOPRptLnk {
			get {
				return (this._noopRptLnk);
			}
			set {
				this._noopRptLnk = value;
			}
		}

		public string Status 
		{
			get 
			{
				return (this._status==string.Empty?"Y":_status);
			}
			set 
			{
				this._status = value;
			}
		}

        public int SmartformID
        {
            get
            {
                return (this._smartformID);
            }
            set
            {
                this._smartformID = value;
            }
        }

		public string EventRuleText {
			get {				
				if (Group().ID() >0) {
					return GroupEventRuleText;
				}	
		
				return CampaignEventRuleText;
			}
		}

		private string GroupEventRuleText {
			get {
				switch(_type.ToLower()) {
					case "subscribe":
					case "unsubscribe":
						return string.Format("'{0}' to group[{3}] will trigger campaign[{1}] {2}.",
							_type, ResponseLayoutName, GetFriendlySendDateTime(Period()), Group().Name);
					default:
						return string.Format("'{0}' any compaign send to group[{3}] will trigger campaign[{1}] {2}.",
							_type, ResponseLayoutName, GetFriendlySendDateTime(Period()), Group().Name);
				}				
			}
		}

		private string CampaignEventRuleText {
			get {
				string creteria = Criteria();
				if (creteria.Trim() == string.Empty) {
					return string.Format("'{0}' on it will trigger campaign[{2}] {3}.",
						_type, 
						LayoutName,
						ResponseLayoutName, 
						GetFriendlySendDateTime(Period()));					
				}


				return string.Format("'{0}' on [{4}] will trigger campaign[{2}] {3}.",
					_type, 
					LayoutName,
					ResponseLayoutName, 
					GetFriendlySendDateTime(Period()),
					creteria);
			}
		}

		private string GetFriendlySendDateTime(double period) {
			TimeSpan ts = TimeSpan.FromDays(period);

			System.Text.StringBuilder ret = new System.Text.StringBuilder();
			if (ts.Days > 0) {
				ret.Append(string.Format("{0} days", ts.Days));
			}

			if (ts.Hours > 0) {
				if (ret.Length > 0) {
					ret.Append(",");
				}
				ret.Append(string.Format("{0} hours", ts.Hours));
			}
			
			if (ts.Minutes > 0) {
				if (ret.Length > 0) {
					ret.Append(",");
				}
				ret.Append(string.Format("{0} minutes", ts.Minutes));
			}		
			
			return ret.Length > 0? "in " + ret.ToString(): "immediately";
		}
		
		#endregion

		
		/// Given what variables have been set try to find a plan that mache and update ourself to it if we can find one.
		
        public void GetLayoutPlan() {
            // Try Group Specific
            if( _group.ID() != 0) 
            {
                try 
                {
                    ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "SELECT LayoutPlanID FROM LayoutPlans where CustomerID = " + _customer_id + " AND  GroupID = " + _group.ID()
                        + " AND EventType='" + _type + "'" + " and IsDeleted = 0 ")));
                } 
                catch 
                {
                    ID(0);
                }

				if (ID() != 0) {
					return;
				}
            }
            // Layout specific
            if(_layout.ID() != 0) 
            {
                try 
                {
                    ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "SELECT LayoutPlanID FROM LayoutPlans where CustomerID = " + _customer_id + " AND  LayoutID = " + _layout.ID()
                        + " AND EventType='" + _type + "' AND Criteria ='" + _criteria + "'" + " and IsDeleted = 0 ")));
                } 
                catch 
                {
                    ID(0);
                }
				if (ID() != 0) {
					return;
				}
            }
            // Check for a default plan if we cannot find a specific one.

                try {
                    ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "SELECT LayoutPlanID FROM LayoutPlans where CustomerID = " + _customer_id + " AND LayoutID = 0 AND GroupID = 0"
                    + " AND EventType='" + _type + "'" + " and IsDeleted = 0 ")));
                } 
                catch 
                {
                    ID(0);
                }

        }

		
		
		/// Update the improtant variables in the db
		
        public void Update() {
            DataFunctions.ExecuteScalar("communicator", "Update LayoutPlans set Period = " + _period + ", BlastID=" + _blast.ID() + ", ActionName ='" + _name + "', Criteria='" + _criteria + "', GroupID = " + Group().ID().ToString() + ", LayoutID = " + Layout().ID().ToString()
               + " WHERE LayoutPlanID = " + ID());
        }
		
		/// Create a new layout plan based on our internal variables
		
        public int Create() {
			int currentLayoutPlanID = 0;
			string createLayoutPlanSQL = "INSERT INTO LayoutPlans "+
				" (LayoutID, Period, BlastID, EventType, Criteria,ActionName,CustomerID,GroupID, Status, IsDeleted) "+
				" VALUES "+
				" ( "+ _layout.ID() +", "+ _period +", "+ _blast.ID() +" , '"+_type+"' , '"+_criteria+"' , '"+_name+"' , "+ _customer_id+ "," + _group.ID() + ",'" + _status + "', 0 ); SELECT @@IDENTITY";

            currentLayoutPlanID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", createLayoutPlanSQL));

			return currentLayoutPlanID;
        }
    
		
		/// Display helper for the managing object
		
		/// <returns> the table of all of our layoutplans </returns>
        public DataTable GetLayoutPlansGrid() 
        {
            string sqlquery=
                " SELECT lp.*, l.LayoutName, r.LayoutName as ResponseName, r.LayoutID AS ResponseLayoutID , g.GroupName from LayoutPlans lp, Layout l, Layout r , Groups g where " +
                " lp.CustomerID = "+_customer_id +" AND l.LayoutID =* lp.LayoutID AND r.LayoutID = (SELECT LayoutID from Blast where BlastID = lp.BlastID and StatusCode <> 'Deleted') AND g.GroupID =*lp.GroupID";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, ConfigurationManager.AppSettings["com"].ToString());
            return dt;
        }


        #region Static Methods

        public static ArrayList GetGroupLayoutPlans(int customerID, int groupID, string type, int SmartFormID)
        {
            ArrayList layoutPlans = new ArrayList();
            if (groupID <= 0)
            {
                return layoutPlans;
            }

            if (System.Web.HttpContext.Current == null)
            {
                DataTable groupPlans = DataFunctions.GetDataTable("SELECT * FROM LayoutPlans where CustomerID = " + customerID + " AND  GroupID = " + groupID
                + " AND EventType='" + type + "' and isnull(smartformID,0)= " + SmartFormID + " and IsDeleted = 0 ", ConfigurationManager.AppSettings["com"].ToString());

                foreach (DataRow row in groupPlans.Rows)
                {
                    layoutPlans.Add(GetLayoutPlan(row));
                }
            }
            else
            {
                List<ECN_Framework_Entities.Communicator.LayoutPlans> lLayoutPlans = getdata();

                if (lLayoutPlans.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.LayoutPlans> query = lLayoutPlans.FindAll(x => x.Status.ToUpper() == "Y" && x.CustomerID.Value == customerID && x.EventType.ToUpper() == type.ToUpper() && x.GroupID.Value == groupID && x.SmartFormID.Value == SmartFormID);

                    foreach (ECN_Framework_Entities.Communicator.LayoutPlans lp in query)
                    {
                        layoutPlans.Add(GetLayoutPlan(lp));
                    }
                }
            }
            return layoutPlans;
        }

        public static ArrayList GetCampaignLayoutPlans(int customerID, int layoutID, string type)
        {
            ArrayList layoutPlans = new ArrayList();
            if (layoutID <= 0)
            {
                return layoutPlans;
            }

            if (System.Web.HttpContext.Current == null)
            {
                DataTable groupPlans = DataFunctions.GetDataTable("SELECT * FROM LayoutPlans where Isnull(Status,'Y') = 'Y' and CustomerID = " + customerID + " AND  LayoutID = " + layoutID
                + " AND EventType='" + type + "'" + " and IsDeleted = 0 ", ConfigurationManager.AppSettings["com"].ToString());
                foreach (DataRow row in groupPlans.Rows)
                {
                    layoutPlans.Add(GetLayoutPlan(row));
                }
            }
            else
            {

                List<ECN_Framework_Entities.Communicator.LayoutPlans> lLayoutPlans = getdata();

                if (lLayoutPlans.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.LayoutPlans> query = lLayoutPlans.FindAll(x => x.Status.ToUpper() == "Y" && x.CustomerID.Value == customerID && x.EventType.ToUpper() == type.ToUpper() && x.LayoutID == layoutID && x.IsDeleted == false);

                    foreach (ECN_Framework_Entities.Communicator.LayoutPlans lp in query)
                    {
                        layoutPlans.Add(GetLayoutPlan(lp));
                    }
                }
            }


            return layoutPlans;
        }

        private static List<ECN_Framework_Entities.Communicator.LayoutPlans> getdata()
        {

            List<ECN_Framework_Entities.Communicator.LayoutPlans> lLayoutPlans = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();
            if (System.Web.HttpContext.Current.Cache["cache_layoutplans"] == null)
            {
                DataTable dt = DataFunctions.GetDataTable("select LayoutPlanID, isnull(LayoutID,0) as LayoutID, EventType, isnull(BlastID,0) as BlastID, Period, Criteria, isnull(l.CustomerID,0) as CustomerID, ActionName, isnull(l.GroupID,0) as GroupID, isnull(l.SmartFormID,0) as SmartFormID from layoutplans l join ecn5_accounts..Customer c on l.CustomerID = c.CustomerID  where  isnull(Status,'Y') = 'Y' and l.IsDeleted = 0", ConfigurationManager.AppSettings["com"].ToString());

                foreach (DataRow row in dt.Rows)
                {
                    ECN_Framework_Entities.Communicator.LayoutPlans plan = new ECN_Framework_Entities.Communicator.LayoutPlans();
                    
                    plan.LayoutPlanID = int.Parse(row["LayoutPlanID"].ToString());
                    plan.LayoutID = int.Parse(row["LayoutID"].ToString());
                    plan.EventType = Convert.ToString(row["EventType"]);
                    plan.CustomerID = int.Parse(row["CustomerID"].ToString());
                    plan.BlastID = int.Parse(row["BlastID"].ToString());
                    plan.GroupID = int.Parse(row["GroupID"].ToString());
                    plan.Period = Convert.ToDecimal(row["Period"].ToString());
                    plan.Criteria = Convert.ToString(row["Criteria"]);
                    plan.ActionName = Convert.ToString(row["ActionName"]);
                    plan.Status = "Y";
                    plan.SmartFormID = int.Parse(row["SmartformID"].ToString());
                    plan.IsDeleted = false;

                    lLayoutPlans.Add(plan);
                }

                System.Web.HttpContext.Current.Cache.Add("cache_layoutplans", lLayoutPlans, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            }
            else
            {
                lLayoutPlans = (List<ECN_Framework_Entities.Communicator.LayoutPlans>)System.Web.HttpContext.Current.Cache["cache_layoutplans"];
            }

            return lLayoutPlans;
        }


		public static ArrayList GetLayouPlansByCampaignID(int customerID, int layoutID) {
			/*string sqlquery= " SELECT lp.*, l.LayoutName, r.LayoutName as ResponseName, r.LayoutID AS ResponseLayoutID , g.GroupName from LayoutPlans lp, Layouts l, Layouts r , Groups g where " +
                " lp.CustomerID = "+ customerID + " AND lp.LayoutID = " + layoutID + " AND lp.GroupID = 0 AND l.LayoutID =* lp.LayoutID AND r.LayoutID = (SELECT LayoutID from Blasts where BlastID = lp.BlastID) AND g.GroupID =*lp.GroupID";
			*/
			string sqlquery =	" SELECT lp.LayoutPlanID, lp.LayoutID, lp.EventType, lp.BlastID, lp.Period, lp.Criteria, lp.CustomerID, lp.GroupID, isnull(lp.Status,'Y') as Status, "+
										" l.LayoutName, lr.LayoutName AS 'ResponseName', l.LayoutID AS 'ResponseLayoutID', NULL AS 'GroupName',"+
										" CASE "+
 										" 	WHEN tp.BlastID IS NULL THEN lp.ActionName "+
										" 	ELSE (lp.ActionName+'&nbsp;&nbsp;'+'<sub><img src=/ecn.images/images/icon-no-open.gif alt=''Indicates a NO OPEN Trigger'' border=0></sub>') "+
										" 	END AS 'ActionName', "+
										" CASE "+
										" 	WHEN tb.BlastID IS NULL THEN ''"+
										" 	ELSE '<sub><a href=../blasts/reports.aspx?BlastID='+Convert(varchar,tb.BlastID)+'><img src=/ecn.images/images/icon-reports.gif alt=''View NO OPEN Trigger Reporting'' border=0></a></sub>' "+
										" 	END AS 'NOOPRptLnk' "+
										" FROM layoutplans lp "+
										" JOIN Layout l on l.LayoutID = lp.LayoutID and l.IsDeleted = 0 "+
										" JOIN Blast b on lp.BlastID = b.BlastID and b.StatusCode <> 'Deleted' "+
                                        " JOIN Layout lr on b.LayoutID = lr.LayoutID and lr.IsDeleted = 0 " +
										" LEFT OUTER JOIN TriggerPlans tp on tp.RefTriggerID = lp.BlastID and tp.IsDeleted = 0 "+
                                        " LEFT JOIN Blast tb on tp.BlastID = tb.BlastID and tb.StatusCode <> 'Deleted' " +
										" WHERE lp.layoutID = "+layoutID +
                                        " AND lp.CustomerID = " + customerID + " and lp.IsDeleted = 0 ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, ConfigurationManager.AppSettings["com"].ToString());
			ArrayList layoutPlans = new ArrayList();
			foreach(DataRow row in dt.Rows) {				
				layoutPlans.Add(GetLayoutPlanWithAdditionalProperties(row));
			}
			return layoutPlans;
		}


		public static ArrayList GetLayouPlansByGroupID(int customerID, int groupID) {
			/*string sqlquery= " SELECT lp.*, l.LayoutName, r.LayoutName as ResponseName, r.LayoutID AS ResponseLayoutID , g.GroupName from LayoutPlans lp, Layouts l, Layouts r , Groups g where " +
				" lp.CustomerID = "+ customerID +" AND lp.GroupID = " + groupID.ToString() + " AND l.LayoutID =* lp.LayoutID AND r.LayoutID = (SELECT LayoutID from Blasts where BlastID = lp.BlastID) AND g.GroupID =*lp.GroupID";
			*/
			string sqlquery =	" SELECT lp.LayoutPlanID, lp.LayoutID, lp.EventType, lp.BlastID, lp.Period, lp.Criteria, lp.CustomerID, lp.GroupID, isnull(lp.Status,'Y') as Status,"+
										" NULL AS 'LayoutName', lr.LayoutName AS 'ResponseName', lr.LayoutID AS 'ResponseLayoutID', g.GroupName as 'GroupName', "+
										" CASE "+
 										" 	WHEN tp.BlastID IS NULL THEN lp.ActionName "+
 										" 	ELSE (lp.ActionName+'&nbsp;&nbsp;'+'<sub><img src=/ecn.images/images/icon-no-open.gif alt=''Indicates a NO OPEN Trigger'' border=0></sub>') "+
 										" 	END AS 'ActionName' ,  "+
										" CASE  "+
  										" 	WHEN tb.BlastID IS NULL THEN '' "+
  										" 	ELSE '<sub><a href=../blasts/reports.aspx?BlastID='+Convert(varchar,tb.BlastID)+'><img src=/ecn.images/images/icon-reports.gif alt=''View NO OPEN Trigger Reporting'' border=0></a></sub>'  "+
  										" 	END AS 'NOOPRptLnk' "+
										" FROM layoutplans lp "+
										" JOIN Groups g on g.GroupID = lp.GroupID "+
                                        " JOIN Blast b on lp.BlastID = b.BlastID and b.StatusCode <> 'Deleted' " +
                                        " JOIN Layout lr on lr.LayoutID = b.LayoutID and lr.IsDeleted = 0 " +
                                        " LEFT OUTER JOIN TriggerPlans tp on tp.RefTriggerID = lp.BlastID and tp.IsDeleted = 0 " +
                                        " LEFT JOIN Blast tb on tp.BlastID = tb.BlastID and tb.StatusCode <> 'Deleted' " +
										" WHERE lp.GroupID = "+groupID+
                                        " AND lp.CustomerID = " + customerID + " and lp.IsDeleted = 0 ";

            DataTable dt = DataFunctions.GetDataTable(sqlquery, ConfigurationManager.AppSettings["com"].ToString());
			ArrayList layoutPlans = new ArrayList();
			foreach(DataRow row in dt.Rows) {				
				layoutPlans.Add(GetLayoutPlanWithAdditionalProperties(row));
			}
			return layoutPlans;
		}


		public static LayoutPlans GetLayoutPlanByID(int layoutPlanID) {
            string sqlquery = " SELECT lp.*, l.LayoutName, r.LayoutName as ResponseName, r.LayoutID AS ResponseLayoutID , g.GroupName, '' as 'NOOPRptLnk' " +
                        " from LayoutPlans lp " +
                        " left outer join Layout l on lp.LayoutID = l.LayoutID and l.IsDeleted = 0 " +
                        " left outer join Groups g on lp.GroupID = g.GroupID " +
                        " left outer join Blast b on lp.BlastID = b.BlastID and b.StatusCode <> 'Deleted' " +
                        " left outer join Layout r on b.LayoutID = r.LayoutID and r.IsDeleted = 0 " +
                    " where  lp.LayoutPlanID = " + layoutPlanID.ToString() + " and lp.IsDeleted = 0 ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, ConfigurationManager.AppSettings["com"].ToString());			
			if (dt.Rows.Count == 0) {
				return null;
			}
				
			return GetLayoutPlanWithAdditionalProperties(dt.Rows[0]);
		}

        private static LayoutPlans GetLayoutPlanWithAdditionalProperties(DataRow row) {
			LayoutPlans plan = new LayoutPlans();
			plan.ID(Convert.ToInt32(row["LayoutPlanID"]));
			plan.CustomerID(Convert.ToInt32(row["CustomerID"]));			
			plan.ActionName(Convert.ToString(row["ActionName"]));
			plan.EventType(Convert.ToString(row["EventType"]));
			plan.Group(new Groups(Convert.ToInt32(row["GroupID"])));	
			plan.Blast(new Blasts(Convert.ToInt32(row["BlastID"])));
			plan.Layout(new Layouts(Convert.ToInt32(row["LayoutID"])));
			plan.LayoutName = Convert.ToString(row["LayoutName"]);
			plan.ResponseLayoutName = Convert.ToString(row["ResponseName"]);
			plan.NOOPRptLnk = Convert.ToString(row["NOOPRptLnk"]);
			plan.Status = row["Status"].ToString();
			return plan;
		}


		private static LayoutPlans GetLayoutPlan(DataRow row) {
			LayoutPlans plan = new LayoutPlans();
			plan.ID(Convert.ToInt32(row["LayoutPlanID"]));
			plan.CustomerID(Convert.ToInt32(row["CustomerID"]));			
			plan.ActionName(Convert.ToString(row["ActionName"]));
			plan.EventType(Convert.ToString(row["EventType"]));
			plan.Group(new Groups(Convert.ToInt32(row["GroupID"])));	
			plan.Blast(new Blasts(Convert.ToInt32(row["BlastID"])));
			plan.Layout(new Layouts(Convert.ToInt32(row["LayoutID"])));	
			plan.Status = row["Status"].ToString();
			return plan;
		}

        private static LayoutPlans GetLayoutPlan(ECN_Framework_Entities.Communicator.LayoutPlans lp)
        {
            LayoutPlans plan = new LayoutPlans();
            plan.ID(lp.LayoutPlanID);
            plan.CustomerID(lp.CustomerID.Value);
            plan.ActionName(lp.ActionName);
            plan.EventType(lp.EventType);
            plan.Group(new Groups(lp.GroupID.Value));
            plan.Blast(new Blasts(lp.BlastID.Value));
            plan.Layout(new Layouts(lp.LayoutID.Value));
            plan.Status = lp.Status;
            return plan;
        }
		#endregion
	}
}
