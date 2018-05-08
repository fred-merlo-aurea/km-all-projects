using System;
using System.Configuration;
using System.Collections;
using System.Data;
using ecn.common.classes;

namespace ecn.communicator.classes {
	
	
	
	public class TriggerPlans {

		#region Getters & Setters
		private static int _triggerPlanID	= 0;
		private static int _refTriggerID		= 0;
		private static string _eventType	= "";
		private static int _blastID				= 0;
		private static double _period		= 0.0;
		private static string _criteria		= "";
		private static int _customerID		= 0;
		private static string _actionName= "";
		private static int _groupID			= 0;
		private static string _status = string.Empty;
		public int TriggerPlanID{
			get{	return _triggerPlanID;	}
			set{	_triggerPlanID = value;	}
		} 

		public int RefTriggerID{
			get{	return _refTriggerID;	}
			set{	_refTriggerID = value;	}
		} 

		public string EventType{
			get{	return _eventType;	}
			set{	_eventType = value;	}
		}

		public int BlastID{
			get{	return _blastID;	}
			set{	_blastID = value;	}
		}

		public double Period{
			get{	return _period;	}
			set{	_period = value;	}
		}

		public string Criteria{
			get{	return _criteria;	}
			set{	_criteria = value;	}
		} 

		public int CustomerID{
			get{	return _customerID;	}
			set{	_customerID = value;	}
		}

		public string ActionName{
			get{	return _actionName;	}
			set{	_actionName = value;	}
		} 

		public string Status
		{
			get{	return _status==string.Empty?"Y":_status;	}
			set{	_status = value;	}
		} 

		public int GroupID{
			get{	return _groupID;	}
			set{	_groupID = value;	}
		}

		#endregion

		public TriggerPlans() {}

		public TriggerPlans(int refTriggerrID) {
            string selectTrigSQL = "SELECT * FROM TriggerPlans WHERE RefTriggerID = " + refTriggerrID + " and IsDeleted = 0 ";

			DataTable dt = DataFunctions.GetDataTable(selectTrigSQL, ConfigurationManager.AppSettings["com"]);
			if(dt.Rows.Count > 0){
				foreach(DataRow dr in dt.Rows){
					_triggerPlanID	= Convert.ToInt32(dr["TriggerPlanID"].ToString());	
					_refTriggerID	= Convert.ToInt32(dr["RefTriggerID"].ToString());		
					_eventType		= dr["EventType"].ToString();	
					_blastID			= Convert.ToInt32(dr["BlastID"].ToString());		
					_period			= Convert.ToDouble(dr["Period"].ToString());	
					_criteria			= dr["Criteria"].ToString();	
					_customerID	= Convert.ToInt32(dr["CustomerID"].ToString());	
					_actionName	= dr["ActionName"].ToString();	
					_groupID			= Convert.ToInt32(dr["GroupID"].ToString());	
					_status = dr["Status"].ToString();
				}
			}else{
				_triggerPlanID	= 0;	
				_refTriggerID	= 0;		
				_eventType		= "";	
				_blastID			= 0;		
				_period			= 0;	
				_criteria			= "";	
				_customerID	= 0;	
				_actionName	= "";	
				_groupID			= 0;	
				_status = string.Empty;
			}

		}
	
		public void updateTriggerPlan(int trigPlanID){
			string updateTrigSQL = "UPDATE TriggerPlans SET Period = "+Period+", ActionName = '"+ActionName+"' WHERE TriggerPlanID = "+trigPlanID;
			DataFunctions.ExecuteScalar(updateTrigSQL);
		}

		public void deleteTriggerPlan(int trigPlanID){
            string updateTrigSQL = "UPDATE TriggerPlans SET IsDeleted = 1 WHERE TriggerPlanID = " + trigPlanID;
			DataFunctions.ExecuteScalar(updateTrigSQL);
		}

		public int createTriggerPlan(){
			string insertTrigSQL = "INSERT INTO TriggerPlans (RefTriggerID, EventType, BlastID, Period, Criteria, CustomerID, ActionName, GroupID, Status, IsDeleted) "+
				" VALUES ( " +
				RefTriggerID +", 'noopen', "+BlastID +", "+Period+", '', "+CustomerID+ ", '"+ActionName+"', "+GroupID+", '"+Status+"', 0 ); SELECT @@IDENTITY";
			int newTrigID = 0;
			try{
				newTrigID = Convert.ToInt32(DataFunctions.ExecuteScalar(insertTrigSQL).ToString());
			}catch{}

			return newTrigID;
		}
	}
}
