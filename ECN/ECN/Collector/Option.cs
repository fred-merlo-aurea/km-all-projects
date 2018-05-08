using System;

namespace ecn.collector.classes {
	public class Option {

		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private string _optionText;
		public string OptionText {
			get {
				return (this._optionText);
			}
			set {
				this._optionText = value;
			}
		}

		private string _optionValue;
		public string OptionValue {
			get {
				return (this._optionValue);
			}
			set {
				this._optionValue = value;
			}
		}			

		private int _branchToPageID=0;
		public int BranchToPageID
		{
			get 
			{
				return (this._branchToPageID);
			}
			set 
			{
				this._branchToPageID = value;
			}
		}

        private int _score = 0;
        public int Score
        {
            get
            {
                return (this._score);
            }
            set
            {
                this._score = value;
            }
        }

		public Option(int id, string text, string val, int branchToPageID, int score) {
			ID = id;
			OptionText = text;
			OptionValue = val;
            BranchToPageID = branchToPageID;
            Score = score;
		}
	}
}
