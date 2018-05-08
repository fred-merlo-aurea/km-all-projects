namespace ecn.communicator.main.SMSWizard.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ecn.communicator.classes;

	public partial class previewLbltext : System.Web.UI.UserControl, IWizard
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
			lblpreviewTxt.Attributes.Add("readonly","true");

			if (WizardID != 0)
			{
				ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);

				if (w != null)
				{
					lblMessageName.Text = w.WizardName;

					lblGroupName.Text = DataFunctions.ExecuteScalar("SELECT GroupName FROM Groups WHERE GroupID=" + w.GroupID).ToString();
					lblContent.Text = DataFunctions.ExecuteScalar("SELECT ContentTitle FROM Content WHERE ContentID=" + w.ContentID).ToString();

					LicenseCheck lc = new LicenseCheck();

                    if (w.FilterID > 0)
                    {
                        //lblFilter.Text = DataFunctions.ExecuteScalar("SELECT FilterName FROM Filters WHERE FilterID=" + w.FilterID).ToString();
                        lblReceipientCount.Text = lc.BlastCheck(Convert.ToInt32(sc.CustomerID()), w.GroupID, w.FilterID, 0, "");
                    }
                    else
                    {
                        //lblFilter.Text = "<NO FILTER>";
                        lblReceipientCount.Text = DataFunctions.ExecuteScalar("select count(eg.emailID) from emailgroups eg where groupID = " + w.GroupID).ToString();
                    }


					DataTable dt = DataFunctions.GetDataTable("SELECT * FROM Content WHERE ContentID=" + w.ContentID);

					if (dt.Rows.Count > 0)
					{
						DataRow dr = dt.Rows[0];
						lblpreviewTxt.Text = dr["ContentSMS"].ToString().Replace("%23","#");
					}
					else
					{
						ErrorMessage = "ERROR - Content does not exist.";
					}
				}	
			}
		}

		public bool Save() 
		{
			try
			{
				ecn.communicator.classes.Wizard w = new ecn.communicator.classes.Wizard();
				
				w.ID = WizardID;
				w.CompletedStep = 4;
				w.Save();
				return true;
			}
			catch
			{
				return false;
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
			this.ID = "previewLbltext";

		}
		#endregion
	}
}
