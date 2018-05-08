using System;

namespace ecn.collector.classes {
	
	
	
	public class Answer {
		public Answer(int id, string val) {
			ID = id;
			AnswerValue = val;
		}

		public Answer(string val) : this(-1, val) {}

		private int _id = -1;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private string _answerValue;
		public string AnswerValue {
			get {
				return (this._answerValue);
			}
			set {
				this._answerValue = value;
			}
		}

		public override string ToString() {
			return string.Format("{0} - {1}", ID, AnswerValue);
		}

	}
}
