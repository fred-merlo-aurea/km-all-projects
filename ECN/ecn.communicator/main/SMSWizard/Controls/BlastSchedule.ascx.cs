namespace ecn.communicator.main.SMSWizard.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Globalization;
	using System.Data.SqlClient;
	using System.Configuration;
	using System.Text.RegularExpressions;
    using System.Text;

	using ecn.common.classes;
	using ecn.communicator.classes;

	public partial class BlastSchedule : System.Web.UI.UserControl, IWizard
	{

		int _wizardID = 0;
		
		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		public int WizardID
		{
			set 
			{
				_wizardID = value;
			}
			get 
			{
				return _wizardID;
			}
		}
		
		string _errormessage = string.Empty;
		public string ErrorMessage
		{
			set 
			{
				_errormessage = value;
			}
			get 
			{
				return _errormessage;
			}
		}

		public void Initialize() 
		{
			phMessage.Visible = false;
			btnSendNow.Attributes.Add("onclick","javascript:return confirm('Are you sure you want to send this Message?');");
		

			btnSendNow.Attributes.Add("onmouseover","this.src='/ecn.images/images/sendNowBtn_h.gif';");
			btnSendNow.Attributes.Add("onmouseout","this.src='/ecn.images/images/sendNowBtn.gif';");
            
            //Check Licenses
            SetLicenseInfo();
		}

        public void SetLicenseInfo()
        {
            string Current = "0";
            string Used = "0";
            string Available = "0";
            string thisblast = string.Empty;

            LicenseCheck lc = new LicenseCheck();

            if (!lc.IsFreeTrial(sc.CustomerID().ToString()))
            {
                Current = lc.Current(sc.CustomerID().ToString(), "smsblock10k");
                Used = lc.Used(sc.CustomerID().ToString(), "smsblock10k");
                Available = lc.Available(sc.CustomerID().ToString(), "smsblock10k");
            }


            if (Current != "UNLIMITED")
            {
                if (Current == "NO LICENSE" || Used == "NO LICENSE" || Available == "NO LICENSE")
                {
                    ErrorMessage = "ERROR - You do not have license or your license is expired.  please contact Customer Service at 1-866-844-6275.";
                     btnSendNow.Visible = false;
                }
                else if (Available != "N/A")
                {
                    if (WizardID != 0)
                    {
                        ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);

                        if (w != null)
                            thisblast = lc.BlastCheck(Convert.ToInt32(sc.CustomerID()), w.GroupID, w.FilterID, 0, "");
                    }

                    if (Convert.ToInt32(Available) == 0 || (Convert.ToInt32(Available) - Convert.ToInt32(thisblast)) < 0)
                    {
                        ErrorMessage = "ERROR - You do not have license or your license is expired.  please contact Customer Service at 1-866-844-6275.";
                         btnSendNow.Visible = false;
                    }
                }
            }
        }
        
		public bool Save()
		{
			phMessage.Visible = false;
			return true;
		}

		public string getFormattedTime(string time) 
		{
			DateTimeFormatInfo myDTFI = new CultureInfo( "en-US", false ).DateTimeFormat;
			DateTime dateTime = DateTime.Parse(time); 

			string hr = dateTime.ToString("%h");
			string mn = dateTime.ToString("%m");
			string tt = dateTime.ToString("tt");

			string formattedTime = hr+":"+mn+":00 "+tt;

			return formattedTime;
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
		
//		this.btnTestMessage.Click += new System.Web.UI.ImageClickEventHandler(this.btnTestMessage_Click);
//		this.btnSchedule.Click += new System.Web.UI.ImageClickEventHandler(this.btnSchedule_Click);
//		this.rbScheduleOnce.CheckedChanged += new System.EventHandler(this.rbScheduleOnce_CheckedChanged);
//		this.rbScheduleDaily.CheckedChanged += new System.EventHandler(this.rbScheduleDaily_CheckedChanged);
//		this.rbScheduleWeekly.CheckedChanged += new System.EventHandler(this.rbScheduleWeekly_CheckedChanged);
//		this.rbScheduleMonthly.CheckedChanged += new System.EventHandler(this.rbScheduleMonthly_CheckedChanged);
//		this.btnSendNow.Click += new System.Web.UI.ImageClickEventHandler(this.btnSendNow_Click);
//		this.drpFolder.SelectedIndexChanged += new System.EventHandler(this.drpFolder_SelectedIndexChanged);


		private void InitializeComponent()
		{
			this.btnSendNow.Click += new System.Web.UI.ImageClickEventHandler(this.btnSendNow_Click);
		}
		#endregion		
		
		private void btnSendNow_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			phMessage.Visible = false;
			ecn.communicator.classes.Wizard objWizard = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);

			if (objWizard != null)
			{                

				if (objWizard.BlastID == 0)
				{
			
					try
					{
                        string errorMessage = string.Empty;
                        if (!BlastContent.ValidateBlastContent(objWizard.GroupID.ToString(), objWizard.LayoutID.ToString(), DataFunctions.CleanString(objWizard.EmailSubject), ref errorMessage))
                        {
                            ErrorMessage = errorMessage;
                            //ErrorMessage = "ERROR - Bad code snippets in content.";
                            return;
                        }

						Blasts objBlast = new Blasts();

						objBlast.CustomerID(Convert.ToInt32(sc.CustomerID()));
						objBlast.UserID(Convert.ToInt32(sc.UserID()));
						objBlast.Subject(DataFunctions.CleanString(objWizard.EmailSubject));
						objBlast.EmailFrom(StringFunctions.Remove(objWizard.FromEmail,StringFunctions.NonDomain()));
						objBlast.EmailFromName(DataFunctions.CleanString(objWizard.FromName));
						objBlast.ReplyTo(DataFunctions.CleanString(objWizard.ReplyTo));
						objBlast.Layout(new Layouts(objWizard.LayoutID)); 
						objBlast.FilterID(objWizard.FilterID);
						objBlast.RefBlastID = "-1";
						objBlast.Group(new Groups(objWizard.GroupID));
						objBlast.BlastCodeID("0");
						objBlast.TestBlast("n");

						// Setup the Default Blast
						objBlast.BlastFrequency("ONETIME");
						objBlast.SendTime(DateTime.Now.ToString());
                        //wgh - commented out for now as CreateRegularBlast is no longer valid
						//objBlast.CreateRegularBlast("sms");
						objWizard.BlastID = objBlast.ID();
						objWizard.CompletedStep= 5;
                        objWizard.Status = "completed";
						objWizard.Save();
                                            

						Response.Redirect("default.aspx");
						
					}
					catch(Exception ex)
					{
						ErrorMessage = "ERROR - " + ex.Message;
					}
				}
				else
				{
					ErrorMessage = "ERROR - Blast has already been sent or scheduled";
				}
			}
		}

	}
}
