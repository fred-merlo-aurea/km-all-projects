namespace ecn.wizard.wizard
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
	using System.Text;
	using System.Xml;
	using System.Web.Mail;
	
	using System.Text.RegularExpressions;
	using System.Data.SqlClient;
	
	using ecn.common.classes;
	using ecn.communicator.classes;
	using ecn.wizard.Component;


	/// <summary>
	///		Summary description for CreditCardSend.
	/// </summary>
	public partial class CreditCardSend : ecn.wizard.MasterControl, IWizard
	{

		protected string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
		
		public void Initialize() 
		{
			// Display charges at the top label control
			//delete EmailID from EmailGroups for that Group.	
			string sqlQuery	= " exec sp_EmailWizardRate " + ChannelID +  ", " + WizardSession.GroupID;
			try
			{
				WizardSession.Amount = Convert.ToDecimal(DataFunctions.ExecuteScalar("accounts", sqlQuery));
				lblAmount.Text = "$" + WizardSession.Amount.ToString();
			}			
			catch
			{
				throw;
			}
			
		}

		public bool Save() 
		{
			SaveWizard sWizard = new SaveWizard(ChannelID, CustomerID, UserID);

			if (Page.IsValid)
			{
				try
				{
					if (PerformChecks())
					{
						sWizard.CardHolderName = Name.Value;
						sWizard.CardType = cardType.SelectedValue;
						sWizard.CardNumber = cardNumber.Value;
						sWizard.CardExpMonth = month.Value;
						sWizard.CardExpYear = year.Value;
						sWizard.CardVerificationNumber = cvNumber.Value;

						sWizard.Save();
						return true;
					}
				}
				catch(Exception ex)
				{
					ShowMessage(ex.Message);
					return false;
				}
			}

			return false;
		}



		/// <summary>
		/// Performs a check to see if all the information required to create a Cotnent or Layout is present or not.
		/// If any of the information requied to create Content or Layout is not avaibale, 
		/// then this method simply displays an error message, until the required information is avaiable.
		/// </summary>
		/// <returns>
		/// True:	If all the required information is present
		/// False: If any of the piece of information is not found.
		/// </returns>
		private bool PerformChecks() 
		{
			string msg = ConfigurationManager.AppSettings["createerrmsg"];

			// Errors for unknown reasons. Mainly because there is no session contents available to successfully process datastore.
			if (WizardSession.GroupID == -1 || WizardSession.TemplateID == -1) 
			{
				msg = msg.Replace("%%errmsg%%", "Could not find information about selected Group and Template.");
				ShowMessage(msg);
				return false;
			}

			if (WizardSession.Name == string.Empty || WizardSession.ContentSource == string.Empty) 
			{
				msg = msg.Replace("%%errmsg%%", "Could not find information regarding Message.");
				ShowMessage(msg);

				return false;
			}

			return true;
		}
		

		/// <summary>
		/// Shows appropriate message for credit-card transaction
		/// </summary>
		/// <param name="type">Type of message to show
		/// f:	Failure Message
		/// s:	Success Message
		/// </param>
		private void ShowMessage(string ErrorMessage) 
		{
			Label lbl = new Label();
			
			lbl.Text = ErrorMessage;
			lbl.ForeColor = Color.Red;
			divMsg.Controls.Add(lbl);
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
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			if (cardType.SelectedIndex == 2) 
			{		// Amex selected. Make CV size 4, and, image.
				cvNumber.MaxLength = 4;	// Max length is 4.
			}
		}
	}
}
