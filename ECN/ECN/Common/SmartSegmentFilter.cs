using System;

namespace ecn.common.classes
{	
	public class SmartSegmentFilter
	{
		public const int FilterID_Unclick = int.MaxValue;
		public const int FilterID_OpenAndUnclick = int.MaxValue - 1;
        public const int FilterID_Unopen = int.MaxValue - 2;
        public const int FilterID_Open = int.MaxValue - 4;
        public const int FilterID_Click = int.MaxValue - 5;
        public const int FilterID_Suppressed = int.MaxValue - 3;

		public SmartSegmentFilter(int filterID) {
			_filterID = filterID;
		}

		private int _filterID;
		public int FilterID {
			get {
				return (this._filterID);
			}
			set {
				this._filterID = value;
			}
		}

		public bool IsSmartSegmentFilter {
			get {
				if (_filterID == FilterID_Unclick || _filterID == FilterID_OpenAndUnclick || _filterID == FilterID_Unopen
                    || _filterID == FilterID_Open || _filterID == FilterID_Click || _filterID == FilterID_Suppressed
                    ) 
                {
					return true;
				}

				return false;
			}
		}

		//this method won't be used.. ! Use the one below.
		//-- ashok 9/02/05
//        public string GetWhereClause(int blastID) {
//            if (!IsSmartSegmentFilter) {
//                return string.Empty;
//            }

//            switch(_filterID) {
//                case FilterID_Unclick:
//                    return string.Format(@"JOIN (select o.EmailID from (select EmailID, 'send' from ecn_activity..BlastActivitySends where BlastID = {0}) o left join
//(Select EmailID, 'click' from ecn_activity..BlastActivityClicks where BlastID = {0}) c
//on o.EmailID = c.EmailID
//Where c.EmailID is null) el on Emails.EmailID = el.EmailID", blastID);
//                case FilterID_Unopen:
//                    return string.Format(@"JOIN (select s.EmailID from (select * from ecn_activity..BlastActivitySends where BlastID = {0}) s left join
//(select * from ecn_activity..BlastActivityOpens where BlastID = {0}) o on s.EmailID = o.EmailID 
//where o.EmailID is null) el on Emails.EmailID = el.EmailID", blastID);
//                case FilterID_OpenAndUnclick:
//                    return string.Format(@"JOIN (select o.EmailID from (select EmailID, 'open' from ecn_activity..BlastActivityOpens where BlastID = {0}) o left join
//(Select EmailID, 'click' from ecn_activity..BlastActivityClicks where BlastID = {0}) c
//on o.EmailID = c.EmailID
//Where c.EmailID is null) el on Emails.EmailID = el.EmailID", blastID);
//                default:
//                    return string.Empty;
//            }	
//        }

		//-- removed the joinClause & simplyfied in the Sproc by adding actionType & refBlastID params
		//-- ashok 9/02/05
		public string GetWhereClause() {
			if (!IsSmartSegmentFilter) {
				return string.Empty;
			}

			switch(_filterID) {
				case FilterID_Unclick:
					return "unclick";

				case FilterID_Unopen:
					return "unopen";

				case FilterID_OpenAndUnclick:
					return "unopen_unclick";

                case FilterID_Suppressed:
                    return "suppressed";

                case FilterID_Open:
                    return "open";

                case FilterID_Click:
                    return "click";

				default:
					return string.Empty;
			}			
		}
	}
}
