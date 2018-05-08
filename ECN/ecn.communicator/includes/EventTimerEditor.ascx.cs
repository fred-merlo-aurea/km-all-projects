namespace ecn.communicator.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ecn.communicator.classes.Event;
    using System.IO;

	
	///		Summary description for EventTimerEditor.
	
	public partial class EventTimerEditor : System.Web.UI.UserControl
	{


		string[] days = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};

	
		protected void Page_Load(object sender, System.EventArgs e)
		{				
			if (!IsPostBack) {				
				Timer.Load();
				LoadControl(Timer);
			}
		}		

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion

		public EventTimer Timer {
			get { 
				object t = Session["EventTimer"];
				if (t == null) {
					Session["EventTimer"] = new EventTimer(CustomerID);
				}
				return (EventTimer) Session["EventTimer"];
			}
		}

		public int CustomerID {
			get {
				ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();				
				return Convert.ToInt32( sc.CustomerID() );
			}
		}

		public int UserID {
			get {
				ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();				
				return Convert.ToInt32( sc.UserID() );
			}
		}
		
		public string GetScheduleInXml() {
			StringBuilder xml = new StringBuilder();
			xml.Append("<Timer>");
			foreach(string day in days) {
				xml.Append(GetScheduleInXml(day));
			}
			xml.Append("</Timer>");
			return xml.ToString();
		}


		public string ScheduleConfigWebPath {
			get {
				ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
                string DataWebPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID());

                if (!Directory.Exists(DataWebPath))
                    Directory.CreateDirectory(DataWebPath);
                
                return DataWebPath;		

			}
		}

		protected void RadioButton_CheckedChanged(object sender, System.EventArgs e) {
			RadioButton rdoButton = sender as RadioButton;
			if (rdoButton == null) {
				return;
			}

			string dayOfWeek = string.Empty;
			foreach(string day in days) {
				if (rdoButton.ID.IndexOf(day) != -1) {
					dayOfWeek = day;
					break;
				}
			}
			if (dayOfWeek == string.Empty) {
				throw new ApplicationException(string.Format("Can't find the target day of week for radio button {0}.", rdoButton.ID));
			}

			if (rdoButton.Checked) {
				if (IsSendTimeRadioButton(rdoButton, dayOfWeek)) {
					Control ctrl = FindControl(GetNextSendDayRadioButtonName(dayOfWeek));
					
					RadioButton rdo = ctrl as RadioButton;								
					rdo.Checked = false;
				
					SetSendTimeControls(dayOfWeek, true);
					SetNextSendDayControls(dayOfWeek, false);
					return;
				}
				
				Control sendTimeCtrl = FindControl(GetSendTimeRadioButtonName(dayOfWeek));					
				RadioButton rdoSendTime = sendTimeCtrl as RadioButton;								
				rdoSendTime.Checked = false;	
				SetNextSendDayControls(dayOfWeek, true);
				SetSendTimeControls(dayOfWeek, false);
			}		
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e) {
			lblErrorMessage.Visible = false;
			
			try {
				Timer.SetEventTimer(GetScheduleInXml());
			} catch (ArgumentException ex) {
				lblErrorMessage.Visible = true;
				lblErrorMessage.Text = ex.Message;
				return;
			}

			Timer.Save(UserID);
			
		}


		#region Private Methods
		private void LoadControl(EventTimer timer) {
			foreach(string dayOfWeek in days) {
				if (timer.HasNextDate(dayOfWeek)) {
					Control ctrl = FindControl(GetNextSendDayRadioButtonName(dayOfWeek));					
					RadioButton rdo = ctrl as RadioButton;								
					rdo.Checked = true;
					RadioButton_CheckedChanged(rdo, null);
					

					Control ddlNextSendDayCtl = FindControl(string.Format("ddl{0}", dayOfWeek));
					DropDownList ddlNextSendDay = ddlNextSendDayCtl as DropDownList;
					ddlNextSendDay.SelectedIndex = ddlNextSendDay.Items.IndexOf(ddlNextSendDay.Items.FindByValue(timer.GetPreferedDayOfWeek(dayOfWeek)));					
					continue;
				}

				Control sendTimeCtrl = FindControl(GetSendTimeRadioButtonName(dayOfWeek));					
				RadioButton rdoSendTime = sendTimeCtrl as RadioButton;
				rdoSendTime.Checked = true;
				RadioButton_CheckedChanged(rdoSendTime, null);

				TimeSpan ts = timer.GetSendTime(dayOfWeek);
				Control txtHourCtrl = FindControl(string.Format("txt{0}Hour", dayOfWeek));
				Control txtMinuteCtrl = FindControl(string.Format("txt{0}Minute", dayOfWeek));
				Control txtSecondCtrl = FindControl(string.Format("txt{0}Second", dayOfWeek));

				TextBox txtHour = txtHourCtrl as TextBox;
				TextBox txtMinute = txtMinuteCtrl as TextBox;
				TextBox txtSecond = txtSecondCtrl as TextBox;
				
				txtHour.Text = ts.Hours.ToString();
				txtMinute.Text = ts.Minutes.ToString();
				txtSecond.Text = ts.Seconds.ToString();				
			}
		}
		private string GetScheduleInXml(string dayOfWeek) {
			string val=string.Empty;
			Control sentTimeControl = FindControl(GetSendTimeRadioButtonName(dayOfWeek));
			RadioButton rdoSendTime = sentTimeControl as RadioButton;
			
			if (rdoSendTime.Checked) {
				Control txtHourCtrl = FindControl(string.Format("txt{0}Hour", dayOfWeek));
				Control txtMinuteCtrl = FindControl(string.Format("txt{0}Minute", dayOfWeek));
				Control txtSecondCtrl = FindControl(string.Format("txt{0}Second", dayOfWeek));

				TextBox txtHour = txtHourCtrl as TextBox;
				TextBox txtMinute = txtMinuteCtrl as TextBox;
				TextBox txtSecond = txtSecondCtrl as TextBox;

				val = string.Format("{0}:{1}:{2}", txtHour.Text, txtMinute.Text, txtSecond.Text);
			} else {				
				Control ddlNextSendDayCtl = FindControl(string.Format("ddl{0}", dayOfWeek));
				DropDownList ddlNextSendDay = ddlNextSendDayCtl as DropDownList;
				val = ddlNextSendDay.SelectedValue;
			}

			return string.Format("<schedule key={0}{1}{0} value={0}{2}{0}/>","\"", dayOfWeek, val);
		}	
		
		private string GetSendTimeRadioButtonName(string dayOfWeek) {
			return string.Format("rdo{0}SendTime", dayOfWeek);
		}

		private string GetNextSendDayRadioButtonName(string dayOfWeek) {
			return string.Format("rdo{0}NextSendDay", dayOfWeek);
		}
	
		private bool IsSendTimeRadioButton(RadioButton button, string dayOfWeek) {
			return button.ID == GetSendTimeRadioButtonName(dayOfWeek);
		}

		private void SetSendTimeControls(string dayOfWeek, bool isChecked) {			
			Control txtHourCtrl = FindControl(string.Format("txt{0}Hour", dayOfWeek));
			Control txtMinuteCtrl = FindControl(string.Format("txt{0}Minute", dayOfWeek));
			Control txtSecondCtrl = FindControl(string.Format("txt{0}Second", dayOfWeek));

			TextBox txtHour = txtHourCtrl as TextBox;
			TextBox txtMinute = txtMinuteCtrl as TextBox;
			TextBox txtSecond = txtSecondCtrl as TextBox;

			txtHour.Enabled = txtMinute.Enabled = txtSecond.Enabled = isChecked;
		}

		private void SetNextSendDayControls(string dayOfWeek, bool isChecked) {
			Control ddlNextSendDayCtl = FindControl(string.Format("ddl{0}", dayOfWeek));
			DropDownList ddlNextSendDay = ddlNextSendDayCtl as DropDownList;

			ddlNextSendDay.Enabled = isChecked;
		}

		
		#endregion

		protected void chkShowTestWindow_CheckedChanged(object sender, System.EventArgs e) {
			tblTestPanel.Visible = chkShowTestWindow.Checked;

			if (!tblTestPanel.Visible) {
				return;
			}		
	
			lblToday.Text = string.Format("If an event is triggereed in {0}", DateTime.Now.ToShortDateString());
		}

		protected void btnTest_OnClick(object sender, System.EventArgs e) {
			lblErrorMessage.Visible = false;
			EventTimer timer;
			try {
				timer = new EventTimer(GetScheduleInXml(), CustomerID);
			} catch (ArgumentException ex) {
				lblErrorMessage.Visible = true;
				lblErrorMessage.Text = ex.Message;
				return;
			}

			lblToday.Text = string.Format("If an event is triggereed in {0}", DateTime.Now.ToShortDateString());
			DateTime targetDate = DateTime.Now.AddDays(Convert.ToInt32(txtAfter.Text));
			lblResult.Text = string.Format("After {4} days, it will be {3}. And {3} will be a '{0}'. According to the rule, it will send in {1} at {2}",
				targetDate.DayOfWeek.ToString(),
				timer.GetSendDate(targetDate).ToShortDateString(),
				timer.GetSendTime(targetDate).ToString(),
				targetDate.ToShortDateString(),
				txtAfter.Text);
		}
	}
}
