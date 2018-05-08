using System;

namespace ecn.accounts.classes
{	
	public abstract class LeadConfig
	{
		public static int DemoInviteGroupID {
			get { return 886;}		
		}

		public static int CustomerID {
			get { return 142;}
		}		

		public static int MaxAllowedDemoNumber {
			get { return 9;}
		}

		public static int DemoSurveyID {
			get { return 158;}
		}

		#region Blast IDs
		// Invite Blast ID
		public static int BlastID {
			get { return 1113;}
		}

		public static int ThankyouBlastID {
			get { return 1192;}
		}

		public static int AbsentBlastsID {
			get { return 1873;}
		}
		#endregion
	}
}
