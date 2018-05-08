using System;

namespace ecn.collector.classes {

	public class Gridstatement {
		public Gridstatement(int id, string statement ) {
			GridStatementID = id;
			Statement = statement;
		}

		private int _gridStatementID;
		public int GridStatementID {
			get {
				return (this._gridStatementID);
			}
			set {
				this._gridStatementID = value;
			}
		}

		private string _statement;
		public string Statement {
			get {
				return (this._statement);
			}
			set {
				this._statement = value;
			}
		}
	}
}
