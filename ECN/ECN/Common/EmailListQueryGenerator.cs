using System;

namespace ecn.common.classes
{
	
	
	
	public class EmailListQueryGenerator
	{
		public EmailListQueryGenerator(int groupID) : this(groupID, -1, null) {}
		public EmailListQueryGenerator(int groupID, int blastID, string bounceDomain)
		{
			_groupID = groupID;
			_blastID = blastID;
			_bounceDomain = bounceDomain;
		}

		private int _groupID;
		public int GroupID {
			get {
				return (this._groupID);
			}
			set {
				this._groupID = value;
			}
		}

		private int _blastID;
		public int BlastID {
			get {
				return (this._blastID);
			}
			set {
				this._blastID = value;
			}
		}

		private string _bounceDomain;
		public string BounceDomain {
			get {
				return (this._bounceDomain);
			}
			set {
				this._bounceDomain = value;
			}
		}

		public string GetSelectQuery(string filterWhereClause) {
			return string.Format("SELECT {0}{1}", SelectFields, GetFromWhereClauses(filterWhereClause));
		}

		public string GetCountQuery(string filterWhereClause) {
			return string.Format("SELECT count(*){0}", GetFromWhereClauses(filterWhereClause));
		}

		private string SelectFields {
			get { 
				if (BlastID == -1 || BounceDomain == null || BounceDomain.Trim() == string.Empty) {
					throw new InvalidOperationException("Without blastID, bounce domain, select fields can't be generated.");
				}
				return string.Format(@"e.* , eg.GroupID, eg.FormatTypeCode, eg.SubscribeTypeCode,  
'bounce_'+LTRIM(STR(e.EmailID))+'-{0}@{1}' AS BounceAddress", BlastID, BounceDomain);
			}
		}
		
		private string GetFromWhereClauses(string filterWhereClause) {
			if (filterWhereClause == null || filterWhereClause.Trim() == string.Empty) {
				return string.Format(@"
FROM Emails e, EmailGroups eg 
WHERE eg.groupID = {0} AND eg.SubscribeTypeCode='S' AND eg.emailID = e.emailID",GroupID);
			}
			if (!HasUDF(filterWhereClause)) {
				return string.Format(@"
FROM Emails e, EmailGroups eg 
WHERE eg.groupID = {0} AND eg.SubscribeTypeCode='S' AND eg.emailID = e.emailID AND ({1})",GroupID, filterWhereClause);
			}

			return string.Format(@"
FROM Emails e, EmailGroups eg, GroupDataFields g, EmailDataValues v
WHERE eg.groupID = {0} AND eg.SubscribeTypeCode='S' 
	AND eg.emailID = e.emailID
	AND e.EmailID = v.EmailID AND v.GroupDataFieldsID = g.GroupDataFieldsID 
	AND ({1})",GroupID, filterWhereClause);
		}
		

		private bool HasUDF(string filterWhereClause) {
			int index = filterWhereClause.IndexOf("g.ShortName=");
			if (index >= 0) {
				return true;
			}

			return false;
		}
	}
}
