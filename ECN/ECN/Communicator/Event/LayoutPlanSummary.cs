using System;
using System.Collections;
using System.Data;
using ecn.common.classes;


namespace ecn.communicator.classes.Event
{
	public class LayoutPlanSummary {
		public LayoutPlanSummary(int id, string name, int ruleCount) {
			ID = id;
			Name = name;
			RuleCount = ruleCount;
		}

		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private string _name;
		public string Name {
			get {
				return (this._name);
			}
			set {
				this._name = value;
			}
		}

		private int _ruleCount;
		public int RuleCount {
			get {
				return (this._ruleCount);
			}
			set {
				this._ruleCount = value;
			}
		}

		public static ArrayList GetGroupLayoutPlanSummary(int customerID) {			
			/*string sql = string.Format(@"select g.GroupID, g.GroupName, l.RuleCount from Groups g join 
(SELECT GroupID, count(LayoutPlanID) as RuleCount from LayoutPlans where CustomerID = {0} and GroupID > 0 
	Group by GroupID
) l on g.GroupID = l.GroupID
order by g.GroupName", customerID);
			*/
			string sql =	string.Format(@" SELECT g.GroupID, g.GroupName, COUNT(g.GroupID) AS 'RuleCount' "+
								" FROM LayoutPlans lp JOIN Groups g ON lp.GroupID = g.GroupID "+
								" WHERE g.CustomerID = {0} and lp.IsDeleted = 0 "+
								" GROUP BY g.GroupID, g.GroupName ORDER BY g.GroupName ", customerID);

			ArrayList summaries = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable(sql);
			foreach(DataRow row in dt.Rows) {				
				summaries.Add(new LayoutPlanSummary(Convert.ToInt32(row["GroupID"]), Convert.ToString(row["GroupName"]), Convert.ToInt32(row["RuleCount"])));
			}
			return summaries;
		}

		public static ArrayList GetCampaignLayoutPlanSummary(int customerID) {
			/*string sql = string.Format(@"select l.LayoutID, l.LayoutName, lp.RuleCount from Layouts l join 
(SELECT LayoutID, count(LayoutPlanID) as RuleCount from LayoutPlans where CustomerID = {0} and GroupID = 0 
	Group by LayoutID
) lp on lp.LayoutID = l.LayoutID
order by l.LayoutName", customerID);
			*/
			string sql = string.Format(@"SELECT l.LayoutID, l.LayoutName, COUNT(l.LayoutID) AS 'RuleCount' 
FROM LayoutPlans lp JOIN Layout l ON lp.LayoutID = l.LayoutID 
WHERE l.CustomerID = {0} and l.IsDeleted = 0 and lp.IsDeleted = 0
GROUP BY l.LayoutID, l.LayoutName ORDER BY l.LayoutName", customerID);

			ArrayList summaries = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable(sql);
			foreach(DataRow row in dt.Rows) {				
				summaries.Add(new LayoutPlanSummary(Convert.ToInt32(row["LayoutID"]), Convert.ToString(row["LayoutName"]), Convert.ToInt32(row["RuleCount"])));
			}
			return summaries;
		}
	}
}
