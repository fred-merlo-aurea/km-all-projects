using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ecn.common.classes;
using System.Configuration;
using System.Web.Security;
using ecn.showcare.wizard.main;

namespace ecn.showcare.wizard
{
	/// <summary>
	/// Summary description for ChooseTemplate.
	/// </summary>
	public class ChooseTemplate : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl templatesDiv;
		protected System.Web.UI.HtmlControls.HtmlTable templatesTable;
		protected System.Web.UI.HtmlControls.HtmlInputImage submit;
		protected SecurityCheck sc = new SecurityCheck();
		protected Step1 step1;
		protected Steps steps = new Steps();
		protected string cid = "";
		protected string uid = "";
		protected System.Web.UI.WebControls.Label lblLstName;
		protected System.Web.UI.WebControls.Label lblRecords;
		protected System.Web.UI.WebControls.HyperLink lnkRecords;
		protected string gid = "";

		protected void GetInfoFromTicket () 
		{
			// the first cookie is SessionID and the second one is the one that we set in the login page. Get that one.
			HttpCookie hc= null;
			for(int i=0;i<Request.Cookies.Count;i++)
			{
				if (Request.Cookies[i].Name.ToUpper() == ".ASPXAUTH")
				{
					hc = Request.Cookies[i];
				}
			}
			if (hc != null)
			{
				// When we set it up, we ecrypted it, so get a encrypted cookie string
				string strToDecrypt = hc.Value;
				// The cookie string is nothing but encrypted FormsAuthenticationTicket. So use Decrypt to get the ticket back
				FormsAuthenticationTicket fat = FormsAuthentication.Decrypt(strToDecrypt);
				// Get the UserData, thats where CustomerID, GroupID and UserID is in the comma seperated form
				string []allData = fat.UserData.Split(',');
				cid = allData[0];							// [0] --> CustomerID
				uid = fat.Name;							// ticket-name --> UserID
				gid = allData[allData.Length-1];	// [last-element] --> GroupID
			}
		}
		/* ========== FLOW ==========
		 * 1) Populate Templates Radio Button List
		 * 1.5) When user clicks on image popup HtmlTemplate image
		 * 2) Before SUBMIT check if Template is selected
		 *     a) if not display and error message (may be write a javascript)
		 *     b) if passes all the checks, then put the TemplateID in the session and goto next step
		 */
		private void Page_Load(object sender, System.EventArgs e)
		{
			GetInfoFromTicket();

			// You want to create step1 object one and only one time
			if (steps.LastVisited == steps.Current)	{	// It means this is the first time user is visiting this page, ever
				step1 = new Step1();
			} else {
				step1 = (Step1) Session["step1"];
			}
			//Display GroupName and Total subscriberin the group
			lblLstName.Text = DataFunctions.ExecuteScalar("select groupName from groups where groupID = " + gid).ToString();
			lnkRecords.Text = DataFunctions.ExecuteScalar("select count(distinct EmailID) from emailgroups where groupID = " + gid + " and subscribeTypeCode = 'S'").ToString();
			lnkRecords.NavigateUrl="javascript:void(0);";
			lnkRecords.Attributes.Add("onclick","javascript:window.open('list.aspx', 'List', 'width=550,height=500,resizable=yes,scrollbars=yes,status=yes')");

			// Populate Templates RadioButtonList
			BuildRBList();
			
			if (!IsPostBack && (steps.LastVisited > steps.Current)) { // That means user is coming back from the next step. So populate whatever you can
				ShowValues();
			}
			// Add later:	When user clicks on "next step" add the email addresses received into the DB
		}

		private void ShowValues () {
			step1 = (Step1) Session["step1"];
			HtmlInputRadioButton rb = (HtmlInputRadioButton) templatesDiv.FindControl("rblTemplates"+step1.TemplateID);
			rb.Checked = true;
		}

		private void BuildRBList() {
			//"SELECT TemplateID, '<img src='+TemplateImage+'>' as TemplateImage "+
			//TemplateStyleCode='wizard' AND 
			string selectQuery =	
				"SELECT TemplateID, TemplateImage, TemplateName, sortOrder as TemplateOrder  "+
				"FROM Templates "+
				"WHERE ChannelID="+ConfigurationManager.AppSettings["ChannelID"]+" AND TemplateStyleCode='wizard' and  activeflag='Y' order by TemplateOrder";//+sc.ChannelID();
			DataTable dt = DataFunctions.GetDataTable(selectQuery);

			// Find how many table rows are required if we were to show 5 columns
			int nRows = dt.Rows.Count;
			const int nColumns = 4;

			// Check if number of rows are divisible by 5, coz if they are then we get exact number of rows required. 
			// Otherwise we get the number with one row less, so we gotta add one more row
			nRows = ((nRows%nColumns)==0) ? (nRows/nColumns) : ( Convert.ToInt32(nRows/nColumns) + 1);
			
			// Create n number of rows and add individual cell to it.
			for (int i=0; i<nRows; i++) {
				HtmlTableRow htr = new HtmlTableRow();
				htr.Attributes.Add("runat","server");	
				// Create n number of columns / cells
				for (int j=0; j<nColumns; j++) {
					try {
						HtmlTableCell htc = new HtmlTableCell();
						htc.Attributes.Add("runat","server");
						htc.Attributes.Add("style", "padding-bottom: 10px;");
						// Add RadioButton Control to it.
						HtmlInputRadioButton hirb = new HtmlInputRadioButton();
						hirb.Name = "rblTemplates";																								// Set Name property
						hirb.Attributes.Add("runat", "server");																					// Set runat=server
						hirb.Value = dt.Rows[(i*nColumns)+j]["TemplateID"].ToString();										// Set the value to TemplateID
						hirb.ID = "rblTemplates"+hirb.Value;																		// Set ID property

						// check if Template has already been selected when user visited last time...
						HtmlAnchor ha = new HtmlAnchor();
						HtmlImage hi = new HtmlImage();
						//ha.Attributes.Add("class","lbOn");
						//ha.Attributes.Add("href","../HtmlTemplates/"+hirb.Value+".jpg");
						ha.Attributes.Add("target","_blank");
						//ha.Attributes.Add("rel","lightbox");
						
						hi.Attributes.Add("name",hirb.Value);//hii.Name = hirb.Value;//dt.Rows[(i*nColumns)+j]["TemplateID"].ToString();											// Set Name property
						//hi.ID = "img"+hirb.Value;																								// Set ID property
						hi.Attributes.Add("src",dt.Rows[(i*nColumns)+j]["TemplateImage"].ToString());			// Set src property
						hi.Style["cursor"] = "hand";																								// Set style cusrsor:hand
						hi.Attributes.Add("border","0");																						// Set Image border width to 0
						// Set OnClick event of image to open a new window with a preview of template image
						//hi.Attributes.Add("onClick","javascript:window.open('../HtmlTemplates/"+hirb.Value+".jpg','Preview','heigth=800,width=800,location=no,menubar=no,scrollbars=no,status=no,toolbar=no')");
						hi.Attributes.Add("onClick","javascript:window.open('" + dt.Rows[(i*nColumns)+j]["TemplateImage"].ToString().Replace("thumb","preview") +"','Preview','width=600,height=550,location=no,menubar=no,scrollbars=no,status=no,toolbar=no')");
						ha.Controls.Add(hi);
						// Add Image tag with source property set
/*						HtmlInputImage hii = new HtmlInputImage();
						hii.Attributes.Add("runat","server");																					// Set runat=server
						hii.Attributes.Add("name",hirb.Value);//hii.Name = hirb.Value;//dt.Rows[(i*nColumns)+j]["TemplateID"].ToString();											// Set Name property
						hii.ID = "img"+hirb.Value;																										// Set ID property
						hii.Attributes.Add("src",dt.Rows[(i*nColumns)+j]["TemplateImage"].ToString());				// Set src property
						hii.Style["cursor"] = "hand";																								// Set style cusrsor:hand
						hii.Attributes.Add("Class","lbOn");
						//hii.Attributes.Add("onClick","javascript:__doPostBack('ShowHtmlTemplate','"+hii.Name+"')");	// Set PostBack for onclick of image
*/						//hii.Attributes.Add("onClick","javascript:window.open('../HtmlTemplates/"+hirb.Value+".jpg','Preview','heigth=800,width=800,location=no,menubar=no,scrollbars=no,status=no,toolbar=no')");

/*						System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
						img.Attributes.Add("src", dt.Rows[(i*nColumns)+j]["TemplateImage"].ToString());			// Set Src property
						img.Style["cursor"] = "hand";																								// Set cursor style
						img.Attributes.Add("onClick","javascript:__doPostBack('ShowHtmlTemplate','"+hirb.Value+"')");	// Set PostBack for onclick of image
						//img.Attributes.Add("runat","server");																					// Set runat=server
*/
						//Add the RadioButton and Image to Cell
						htc.Attributes.Add("align","center");
						htc.Attributes.Add("valign","top");
						htc.Controls.Add(hirb);
						htc.Controls.Add(new LiteralControl("<BR>"));
						htc.Controls.Add(ha);
						if (i < nRows-1) {
							htc.Controls.Add(new LiteralControl("<BR>"));
							htc.Controls.Add(new LiteralControl("<BR>"));
						}

						// Add this cell to the row
						htr.Cells.Add(htc);
					} catch {
						break;
					}
				}

				// Add this row to the table
				templatesTable.Rows.Add(htr);
			}

		}

		private void ShowHtmlTemplate (Hashtable ht) {
			string key = ht["__EVENTTARGET"].ToString();
			string val = ht["__EVENTARGUMENT"].ToString();
			string path = "../HtmlTemplates/"+val+"jpg";
			Response.Write("window.open('"+path+"','Preview','channelmode=yes,heigth=700,width=800,location=no,menubar=no,resizable=no,scrollbars=no,status=no,toolbar=no')");
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.submit.ServerClick += new System.Web.UI.ImageClickEventHandler(this.submit_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void submit_ServerClick(object sender, System.Web.UI.ImageClickEventArgs e) {
			// Get the value of selected Template from the query string
            Hashtable ht = ecn.showcare.wizard.main.Wizard.ParseUrl(Request.RawUrl);
			step1.TemplateID = ht["rblTemplates"].ToString();
			step1.CustomerID = cid;
			step1.GroupID = gid;
			step1.UserID = uid;

			string selectQuery = "SELECT  TemplateName FROM Templates WHERE TemplateID=" + step1.TemplateID;
			if (DataFunctions.ExecuteScalar(selectQuery).ToString().ToLower() == "custom_template")
				step1.IsCustomHeader = true;
			else
				step1.IsCustomHeader = false;

			step1.ToSession();														// Save Step1 to session
			steps.increamentCurrentStep();									// Increament current step too
			Response.Redirect("UpdateMessage.aspx");
		}
	}
}
