using System;
using System.Collections;
using System.Data;
using System.Text;
using ecn.common.classes;

namespace ecn.communicator.classes
{
	
	
	
	public class GroupDataField
	{
		// When create new GroupDataField
        public GroupDataField(int groupID, string shortName, string longName, int surveyID, string ispublic) : this(-1, groupID, shortName, longName, surveyID, ispublic) { }

		// When Load from database
        public GroupDataField(int id, int groupID, string shortName, string longName, int surveyID, string ispublic) : this(id, groupID, shortName, longName, surveyID, -1, ispublic)
        {			
		}	

		public GroupDataField(int id, int groupID, string shortName, string longName, int surveyID, int dataFieldSetID, string ispublic) {
			ID = id;
			GroupID = groupID;
			ShortName = shortName;
			LongName = longName;
			SurveyID = surveyID;
			DatafieldSetID = dataFieldSetID;
            IsPublic = ispublic;
		}
		
		#region Properites
		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
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

		private string _shortName;
		public string ShortName {
			get {
				return (this._shortName);
			}
			set {
				this._shortName = value;
			}
		}

		private string _longName;
		public string LongName {
			get {
				return (this._longName);
			}
			set {
				this._longName = value;
			}
		}

		private int _surveyID;
		public int SurveyID {
			get {
				return (this._surveyID);
			}
			set {
				this._surveyID = value;
			}
		}

		private int _datafieldSetID = -1;
		public int DatafieldSetID {
			get {
				return (this._datafieldSetID);
			}
			set {
				this._datafieldSetID = value;
			}
		}

        private string _ispublic;
        public string IsPublic
        {
            get
            {
                return (this._ispublic);
            }
            set
            {
                this._ispublic = value;
            }
        }


		public bool SupportHistory {
			get { return _datafieldSetID != -1;}
		}

		#endregion

		#region Static Methods
		public static ArrayList GetGroupDataFieldsByGroupID(int groupID) {
			string sqlstmt=
				" SELECT * FROM GroupDatafields "+
				" WHERE GroupID="+ groupID;
                        
			DataTable emailstable = DataFunctions.GetDataTable(sqlstmt);
			
			StringBuilder sqlState = new StringBuilder();
            
			ArrayList fields = new ArrayList();
			foreach(DataRow dr in emailstable.Rows) {
				GroupDataField gf = new GroupDataField(
				Convert.ToInt32(dr["GroupDataFieldsID"]), Convert.ToInt32(dr["GroupID"]), 				
				Convert.ToString(dr["ShortName"]), Convert.ToString(dr["LongName"]), 
					dr["SurveyID"] is DBNull?-1:Convert.ToInt32(dr["SurveyID"]), Convert.ToString(dr["IsPublic"]));
                gf.DatafieldSetID = dr["DatafieldSetID"] is DBNull ? -1 : Convert.ToInt32(dr["DatafieldSetID"]);
				fields.Add(gf);
			}
			return fields;
		}

		public static GroupDataField GetGroupDataFieldsByShortName(ArrayList groupDataFields, string shortName) {
			foreach(GroupDataField field in groupDataFields) {
				if (field.ShortName == shortName) {
					return field;
				}
			}
			return null;
		}
		#endregion
	}
}
