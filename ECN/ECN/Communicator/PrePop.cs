using System;
using System.Text;
using System.Collections;

namespace ecn.communicator.classes
{
	
	
	
	public class PrePop
	{
		//public enum ControlTypeEnum {TextBox, DropDown, CheckBox, RadioButton, TextArea, Hidden}

		private int _fieldID;
		private string _fieldname;
		private string _displayname;
		private bool _isRequired = false;
		private bool _prepopulate = false;
		private string _controltype;
		private string _datatype;
		private string _optionvalues;
		private string _selectedvalues;

		#region getters & setters

		public int FieldID 
		{
			get 
			{
				return (this._fieldID);
			}
			set 
			{
				this._fieldID = value;
			}
		}

		public string FieldName 
		{
			get 
			{
				return (this._fieldname);
			}
			set 
			{
				this._fieldname = value;
			}
		}

		public string DisplayName 
		{
			get 
			{
				return (this._displayname);
			}
			set 
			{
				this._displayname = value;
			}
		}

		public string ControlType 
		{
			get 
			{
				return (this._controltype);
			}
			set 
			{
				this._controltype = value;
			}
		}

		public string DataType 
		{
			get 
			{
				return (this._datatype);
			}
			set 
			{
				this._datatype = value;
			}
		}

		public string OptionValues 
		{
			get 
			{
				return (this._optionvalues);
			}
			set 
			{
				this._optionvalues = value;
			}
		}

		public string SelectedValues
		{
			get 
			{
				return (this._selectedvalues);
			}
			set 
			{
				this._selectedvalues = value;
			}
		}

		public bool IsRequired
		{
			get 
			{
				return (this._isRequired);
			}
			set 
			{
				this._isRequired = value;
			}
		}

		public bool bPrepopulate
		{
			get 
			{
				return (this._prepopulate);
			}
			set 
			{
				this._prepopulate = value;
			}
		}

		
		#endregion

		public PrePop(int fieldID, string fieldname, string displayname, string controltype, string datatype, bool isrequired, bool prepopulate, string optionvalues, string selectedvalues)
		{
			this.FieldID = fieldID;
			this.FieldName = fieldname;
			this.DisplayName = displayname;
			this.ControlType = controltype;
			this.DataType = datatype;
			this.IsRequired = isrequired;
			this.bPrepopulate = prepopulate;
			this.OptionValues = optionvalues;
			this.SelectedValues = selectedvalues;
		}


		public string getControlHTML()
		{
			if (this._fieldname.ToLower() == "state")
				return CreateStateDRPControl();
			if (ControlType.ToLower() == "dropdown")
				return CreateDRPControl();
			else if (ControlType.ToLower() == "text" || ControlType.ToLower() == "hidden")
				return CreateTEXTControl();
			else 
				return CreateOtherControl();
		}

		private string CreateTEXTControl()
		{
			//"onkeypress","return checkKeyPressForInteger(this, event)"
			string strkeypress = string.Empty;

			if (this._datatype.ToLower() == "number")
			{
				strkeypress= "onkeypress=\"return checkKeyPressForDecimal(this, event)\"";
			}
			
			if (this._controltype.ToLower() == "hidden")
			{
				return "<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\" value=\"" + this._optionvalues + "\" type=\"hidden\">";
			}
			else
			{
				if (this._prepopulate)
				{
					if (this._datatype.ToLower() == "date")
					{
						try
						{
							return "<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\"" + strkeypress + " size=\"25\" value=\"" + Convert.ToDateTime(this._selectedvalues).ToShortDateString() + "\" type=\"" + this._controltype + "\"" + " " + ((this._selectedvalues.Trim().ToString()!=string.Empty && this._fieldname.ToLower()=="e")?"readonly":"") + ">";
						}
						catch
						{
							return "<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\"" + strkeypress + " size=\"25\" value=\"\" type=\"" + this._controltype + "\"" + " " + ((this._selectedvalues.Trim().ToString()!=string.Empty && this._fieldname.ToLower()=="e")?"readonly":"") + ">";
						}
					}
					else
						return "<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\"" + strkeypress + " size=\"25\" value=\"" + this._selectedvalues + "\" type=\"" + this._controltype + "\"" + " " + ((this._selectedvalues.Trim().ToString()!=string.Empty && this._fieldname.ToLower()=="e")?"readonly":"") + ">";
				}
				else
					return "<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\"" + strkeypress + " size=\"25\" value=\"\" type=\"" + this._controltype + "\"" +  " " + ((this._selectedvalues.Trim().ToString()!=string.Empty && this._fieldname.ToLower()=="e")?"readonly":"") + ">";
			}
		}


		private string CreateStateDRPControl()
		{
			StringBuilder sbHTML = new StringBuilder("<select id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\"" + " >");
			sbHTML.Append("<option value=\"\"></option>");

			string State = "AK|AL|AR|AZ|CA|CO|CT|DC|DE|FL|GA|HI|IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VA|WA|WI|WV|WY";
			string[] optionValues = State.Split('|');

			if (this._prepopulate)
			{
				for (int j=0;j<optionValues.Length;j++)
				{
					if (optionValues[j].ToLower().Equals(this._selectedvalues.ToLower()))
						sbHTML.Append("<option value=\"" + optionValues[j] + "\" selected>" + optionValues[j] + "</option>");
					else
						sbHTML.Append("<option value=\"" + optionValues[j] + "\">" + optionValues[j] + "</option>");
				}
			}
			else
			{
				for (int j=0;j<optionValues.Length;j++)
					sbHTML.Append("<option value=\"" + optionValues[j] + "\">" + optionValues[j] + "</option>");
			}

			sbHTML.Append("</select>");

			return sbHTML.ToString();
		}

		private string CreateDRPControl()
		{
			StringBuilder sbHTML = new StringBuilder("<select id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\"" + " >");
			sbHTML.Append("<option value=\"\"></option>");

			string[] optionValues = this.OptionValues.Split('|');

			if (this._prepopulate)
			{
				for (int j=0;j<optionValues.Length;j++)
				{
					if (optionValues[j].ToLower().Equals(this._selectedvalues.ToLower()))
						sbHTML.Append("<option value=\"" + optionValues[j] + "\" selected>" + optionValues[j] + "</option>");
					else
						sbHTML.Append("<option value=\"" + optionValues[j] + "\">" + optionValues[j] + "</option>");
				}
			}
			else
			{
				for (int j=0;j<optionValues.Length;j++)
					sbHTML.Append("<option value=\"" + optionValues[j] + "\">" + optionValues[j] + "</option>");
			}

			sbHTML.Append("</select>");

			return sbHTML.ToString();
		}

		private string CreateOtherControl()
		{
			StringBuilder sbHTML= new StringBuilder("");

			string ctype = this._controltype.ToLower()=="checkbox"?"checkbox":"radio";
			string[] optionValues = this.OptionValues.Split('|');

			if (this._prepopulate)
			{
				Hashtable hselectedValues= new Hashtable();
				string[] SelectedValues = this._selectedvalues.Split('|');

				for (int i=0;i<SelectedValues.Length;i++)
					if (!hselectedValues.Contains(SelectedValues[i].ToLower()))
						hselectedValues.Add(SelectedValues[i].ToLower(), SelectedValues[i].ToLower());				

				for (int j=0;j<optionValues.Length;j++)
					if (hselectedValues.Contains(optionValues[j].ToLower()))
						sbHTML.Append("<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\" type=\"" + ctype +"\" value=\"" + optionValues[j] + "\"" +  " checked>" + optionValues[j] + "<BR>");
					else
						sbHTML.Append("<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\" type=\"" + ctype +"\" value=\"" + optionValues[j] + "\"" +  "\">" + optionValues[j] + "<BR>");
			}
			else
			{
				for (int j=0;j<optionValues.Length;j++)
						sbHTML.Append("<input id=\"" + this._fieldname + "\" name=\"" + this._fieldname + "\" type=\"" + ctype +"\" value=\"" + optionValues[j] + "\"" +  "\">" + optionValues[j] + "<BR>");

			}

			return sbHTML.ToString();
		}

	}
}
