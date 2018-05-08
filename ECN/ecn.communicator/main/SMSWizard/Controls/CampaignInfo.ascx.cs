namespace ecn.communicator.main.SMSWizard.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.communicator.classes;
	using ecn.common.classes;
	using System.Configuration;
	using aspNetMX;
	using aspNetEmail;

	public partial class CampaignInfo : System.Web.UI.UserControl, IWizard
	{
		
		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
		int _wizardID = 0;

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
            if (WizardID != 0)
            {
                try
                {
                    ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);

                    txtMessageName.Text = w.WizardName;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                txtMessageName.Text = "Text Message " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"); //2009-04-27-hh-mm-ss";
            }
		}

     


		public bool Save() 
		{
			if (Page.IsValid)
			{
				try
				{
					
                    ecn.communicator.classes.Wizard w = new ecn.communicator.classes.Wizard();
					
					if (Convert.ToInt32(DataFunctions.ExecuteScalar("select count(wizardID) from wizard where wizardname = '" + txtMessageName.Text.Replace("'", "''") + "' and userID=" + sc.UserID() + " and wizardID <> " + WizardID)) > 0) 
					{
						ErrorMessage = "ERROR - Message <font color='#000000'>\""+ txtMessageName.Text +"\"</font> already exists.  Please enter a different name.";
						return false;
					}
					w.ID = WizardID;
					w.WizardName = txtMessageName.Text;
                    w.EmailSubject = "";
					w.FromName = "";
                    w.FromEmail = "";
                    w.ReplyTo = "";
					w.UserID = Convert.ToInt32(sc.UserID());
					w.CompletedStep = 1;
                    w.BlastType = "sms";

					WizardID = w.Save();
					return true;
                  
				}
				catch (Exception ex)
				{
					ErrorMessage = ex.Message;
				}
				
			}
			return false;
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
	}
}
