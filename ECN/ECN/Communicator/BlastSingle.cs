using System;
using ecn.common.classes;

namespace ecn.communicator.classes
{	
	public class BlastSingle
	{
		public BlastSingle(int blastID, int emailID, DateTime sendTime) : this(blastID, emailID, sendTime, 0) {}

		public BlastSingle(int blastID, int emailID, DateTime sendTime, int layoutPlanID) {
			_blastID = blastID;
			_emailID = emailID;
			_sendTime = sendTime;
			_layoutPlanID = layoutPlanID;
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

		private int _emailID;
		public int EmailID {
			get {
				return (this._emailID);
			}
			set {
				this._emailID = value;
			}
		}

		private DateTime _sendTime;
		public DateTime SendTime {
			get {
				return (this._sendTime);
			}
			set {
				this._sendTime = value;
			}
		}

		private bool _isProcessed;
		public bool IsProcessed {
			get {
				return (this._isProcessed);
			}
			set {
				this._isProcessed = value;
			}
		}

		private int _layoutPlanID;
		public int LayoutPlanID {
			get {
				return (this._layoutPlanID);
			}
			set {
				this._layoutPlanID = value;
			}
		}

		// TODO: for now insert is the only operation needed.
		public void Save() {
			DataFunctions.ExecuteScalar("communicator",string.Format(@"INSERT INTO BlastSingles (BlastID, EmailID, Sendtime, Processed, LayoutPlanID) 
VALUES ({0},{1},'{2}','{3}',{4});", BlastID, EmailID, SendTime, IsProcessed?"y":"n",LayoutPlanID));
		}        
	}
}
