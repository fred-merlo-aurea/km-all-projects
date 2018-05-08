using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using ecn.common.classes;

namespace ecn.communicator.classes.Event {
	public class EventTimer : DatabaseAccessor {
		private Hashtable _sendTimeSchedule;
		private NameValueCollection _sendDateTimeSchedule;

		public EventTimer(int customerID) : this(string.Format("<Timer>" +
			"<schedule key={0}Monday{0} value={0}10:0:0{0}/>" +
			"<schedule key={0}tuesday{0} value={0}10:0:0{0}/>" +
			"<schedule key={0}wednesday{0} value={0}10:0:0{0}/>" +
			"<schedule key={0}Thursday{0} value={0}10:0:0{0}/>" +
			"<schedule key={0}friday{0} value={0}10:0:0{0}/>" +
			"<schedule key={0}saturday{0} value={0}monday{0}/>" +
			"<schedule key={0}sunday{0} value={0}monday{0}/>" +			
			"</Timer>", "\""), customerID) {
		}
	
		public EventTimer(string eventTimerXMLString, int customerID) {
			_customerID = customerID;
			SetEventTimer(eventTimerXMLString);
		}

		public void SetEventTimer(string eventTimerXMLString) {
			_sendDateTimeSchedule = new NameValueCollection();
			_sendTimeSchedule = new Hashtable();

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(eventTimerXMLString);	
			XmlNodeList timerNode = doc.GetElementsByTagName("Timer");
			if (timerNode.Count != 1) {
				throw new ArgumentException(string.Format("Invalid argument to create event timer: {0}", eventTimerXMLString));
			}

			foreach(XmlNode node in timerNode.Item(0)) {									
				AddSchedule(node.Attributes["key"].Value, node.Attributes["value"].Value);
			}

			string loop = DetectLoop();
			if (loop != string.Empty) {
				throw new ArgumentException(string.Format("A loop found: {0}", loop));
			}
		}


		private int _customerID;
		public int CustomerID {
			get {
				return (this._customerID);
			}			
		}


		public TimeSpan GetSendTime(DateTime dateInPlan) {		
			return GetSendTime(GetSendDate(dateInPlan).DayOfWeek.ToString());
		}

		public TimeSpan GetSendTime(string dayOfWeek) {
			return (TimeSpan) _sendTimeSchedule[dayOfWeek.ToLower()];
		}

		public DateTime GetSendDate(DateTime dateInPlan) {
			string loop = DetectLoop();
			if (loop != string.Empty) {
				throw new ApplicationException(string.Format("Loop found: {0}", loop));
			}

			string dayOfWeek = dateInPlan.DayOfWeek.ToString().ToLower();
			string preferedDate = GetPreferedDayOfWeek(dayOfWeek);
			if (preferedDate == dayOfWeek) {
				return new DateTime(dateInPlan.Year, dateInPlan.Month, dateInPlan.Day);
			}
			
			while (dateInPlan.DayOfWeek.ToString().ToLower() != preferedDate) {
			    dateInPlan = dateInPlan.AddDays(1);
			}
			return new DateTime(dateInPlan.Year, dateInPlan.Month, dateInPlan.Day);
		}

		public DateTime GetSendDateTime(DateTime dateInPlan) {
			DateTime targetDateTime = GetSendDate(dateInPlan);
			return targetDateTime.AddTicks(GetSendTime(dateInPlan).Ticks);
		}
		// Only go down to one level
		public string GetPreferedDayOfWeek(string key) {		
			string nextDate = _sendDateTimeSchedule[key];
			if (nextDate == null || nextDate == string.Empty) {				
				return key;
			}
			// return nextDate;
			return GetPreferedDayOfWeek(nextDate);			 
		}

		

		public void ChangeSchedule(string dayOfWeekInPlan, string schedule) {
			AddSchedule(dayOfWeekInPlan, schedule);
		}

		public string DetectLoop() {			
			foreach(string key in _sendDateTimeSchedule.Keys) {
				StringCollection log = new StringCollection();
				bool hasLoop = DetectLoop(log, key);
				if (hasLoop) {
					return GetLoopList(log);
				}
			}
			return string.Empty;
		}

		public string GetScheduleInXml() {
			return string.Format(@"<Timer>{0}{1}{2}{3}{4}{5}{6}</Timer>",
				GetScheduleInXml("monday"),
				GetScheduleInXml("tuesday"),
				GetScheduleInXml("wednesday"),
				GetScheduleInXml("thursday"),
				GetScheduleInXml("friday"),
				GetScheduleInXml("saturday"),
				GetScheduleInXml("sunday"));
		}
	
		public bool HasNextDate(string dayOfWeek) {
			return _sendDateTimeSchedule[dayOfWeek]!=null;
		}

		#region Data Access methods
		const string tableName = "LayoutPlanSettings";
		public void Save(int modifiedUserID) {
			string sql;
			if (ID() == 0) {
				sql = string.Format(@"INSERT INTO {0} (CustomerID, ScheduleXmlString, ModifiedUserID) values
					({1}, '{2}', {3});select @@identity;", tableName, CustomerID, GetScheduleInXml(), modifiedUserID);
				this.ID(Convert.ToInt32(DataFunctions.ExecuteScalar(sql)));
			} else {
				sql = string.Format(@"Update {0} set ScheduleXmlString = '{1}', ModifiedUserID={2}, ModifiedDate='{3}';", 
					tableName,GetScheduleInXml(), modifiedUserID, DateTime.Now.ToString());
				DataFunctions.Execute(sql);
			}			
		}

		public void Load() {
			DataTable dt = DataFunctions.GetDataTable(string.Format("select * from {0} where CustomerID ={1};", tableName, CustomerID));
			if (dt.Rows.Count == 0) {
				return;
			}

			ID(Convert.ToInt32(dt.Rows[0]["SettingID"]));
			SetEventTimer(Convert.ToString(dt.Rows[0]["ScheduleXmlString"]));
		}


		#endregion

		#region Private Methods
		private bool DetectLoop(StringCollection loopLog, string key) {
			foreach(string visitedKey in loopLog) {
				if (visitedKey == key) {					
					loopLog.Add(key);
					return true;
				}
			}
			
			string nextDate = _sendDateTimeSchedule[key];
			if (nextDate == null || nextDate == string.Empty) {				
				return false;
			}			

			loopLog.Add(key);
			return DetectLoop(loopLog, nextDate);			 
		}

		private string GetLoopList(StringCollection loopLog) {
			StringBuilder log = new StringBuilder();
			for(int i=0; i< loopLog.Count; i++) {
				log.Append(loopLog[i]);
				if (i != loopLog.Count -1) {
					log.Append("->");
				}
			}			

			return log.ToString();
		}

		private void AddSchedule(string dayOfWeek, string schedule) {
			string[] timeSpans = schedule.Split(':');
			string key = dayOfWeek.ToLower();
			schedule = schedule.ToLower();
			if (timeSpans.Length == 3) {				
				TimeSpan ts = new TimeSpan(0, Convert.ToInt32(timeSpans[0]), Convert.ToInt32(timeSpans[1]), Convert.ToInt32(timeSpans[2]));

				_sendDateTimeSchedule.Remove(key);

				if (_sendTimeSchedule[key] == null) {
					_sendTimeSchedule.Add(key, ts);
					return;
				}		
				
				_sendTimeSchedule[key] = ts;     
           		return;		
			}
			
			_sendTimeSchedule.Remove(key);

			if (_sendDateTimeSchedule[key] == null) {
				_sendDateTimeSchedule.Add(key, schedule);
				return;
			}
			
			_sendDateTimeSchedule[key] = schedule;
		}
		private string GetScheduleInXml(string dayOfWeek) {			
			if (_sendDateTimeSchedule[dayOfWeek] != null) {				
				return string.Format("<schedule key={0}{1}{0} value={0}{2}{0}/>", "\"", dayOfWeek, _sendDateTimeSchedule[dayOfWeek]);
			}

			if (_sendTimeSchedule[dayOfWeek] != null) {
				TimeSpan ts = (TimeSpan) _sendTimeSchedule[dayOfWeek];
				return string.Format("<schedule key={0}{1}{0} value={0}{2}:{3}:{4}{0}/>", "\"", dayOfWeek, ts.Hours,ts.Minutes, ts.Seconds);
			}
			
			throw new ApplicationException("No schedule found for " + dayOfWeek);
		}
		#endregion
	}
}
