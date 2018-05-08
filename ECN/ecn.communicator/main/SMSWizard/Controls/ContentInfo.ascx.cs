namespace ecn.communicator.main.SMSWizard.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Data.SqlClient;

	using ecn.common.classes;
	using ecn.communicator.classes;

	
	///		Summary description for ContentInfo.
	
	public partial class ContentInfo : System.Web.UI.UserControl, IWizard
	{
		int _wizardID = 0;

		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
	
      
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
			int SelectedFolderID = 0;

			if (WizardID != 0)
			{
				try
				{
					ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);
                    txtContentTitle.Text = w.WizardName;
                    if (w.ContentID > 0)
                    {
                        SelectedFolderID = Convert.ToInt32(DataFunctions.ExecuteScalar("Select isnull(FolderID ,0) as folderID from content where contentID = " + w.ContentID));
                        plExistingContent.Visible = true;
                        rbExistingContent.Checked = true;
                        plNewContent.Visible = false;
                        rbNewContent.Checked = false;
                    }
                    
//					else
//					{
//						plExistingContent.Visible = false;
//						rbExistingContent.Checked = false;
//						plNewContent.Visible = true;
//						rbNewContent.Checked = true;					
//					}

					LoadFoldersDR(SelectedFolderID);
					LoadContentDR(SelectedFolderID, w.ContentID);
                    LoadWelcomeMessage(es.CurrentCustomer.CustomerID);
				
                    if (KM.Platform.User.IsSystemAdministrator(es.CurrentUser) || KM.Platform.User.IsChannelAdministrator(es.CurrentUser) || KM.Platform.User.IsAdministrator(es.CurrentUser))
					//if(!(sc.CheckChannelAdmin() || sc.CheckSysAdmin() || sc.CheckAdmin()))
					{
						//if (!(es.CurrentCustomer.HasPermission("ecn.communicator", "contentpriv") && es.HasPermission("ecn.communicator","writecontent")))
                        if (!(KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit)))
						{
							plCreate.Visible = false;
							ContentText.Attributes.Add("readonly","true");

						}
						else
							plCreate.Visible = true;
					}
					else
						plCreate.Visible = true;
				}
				catch (Exception ex)
				{
					ErrorMessage = ex.Message;
				}
			}
		}
		

		private void LoadFoldersDR(int FolderID)
		{
			drpFolder.DataSource = DataLists.GetFoldersDR(sc.CustomerID().ToString(), "CNT");; 
			drpFolder.DataBind();
			drpFolder.Items.Insert(0, new ListItem("root", "0"));
			try
			{
				drpFolder.Items.FindByValue(FolderID.ToString()).Selected = true;
			}
			catch{}
			drpFolder1.DataSource = DataLists.GetFoldersDR(sc.CustomerID().ToString(), "CNT");;
			drpFolder1.DataBind();
			drpFolder1.Items.Insert(0, new ListItem("root", "0"));
			drpFolder1.Items.FindByValue("0").Selected = true;

		}

		private void LoadContentDR(int FolderID, int ContentID)
		{
			string sqlquery="";
			sqlquery=
				//" SELECT ContentID, ContentTitle, ContentTypeCode, LockedFlag "+
				" SELECT ContentID as ContentID, REPLACE (c.ContentTitle, '[CORPORATE]', '<font color=''FF0000''>[CORPORATE]</FONT> ') as 'ContentTitle'"+
				" FROM Content c WHERE folderID = " + FolderID + " and c.UserID="+ sc.UserID() + " order by contenttitle asc ";

			drpContent.DataSource = DataFunctions.GetDataTable(sqlquery, ConfigurationManager.AppSettings["com"].ToString());
			drpContent.DataTextField = "ContentTitle";
			drpContent.DataValueField = "ContentID";
			drpContent.DataBind();

			drpContent.Items.Insert(0, new ListItem("----- Select Content -----",""));

			if (ContentID > 0)
			{
				drpContent.ClearSelection();
				try
				{
					drpContent.Items.FindByValue(ContentID.ToString()).Selected = true;
				}
				catch{}
				LoadContentDetails(ContentID);
			}
		}

		private void LoadContentDetails(int ContentID)
		{
			ContentID = Convert.ToInt32(ContentID);

			try
			{
				if (ContentID != 0 )
				{
					String sqlQuery= " SELECT * FROM Content WHERE ContentID="+ContentID+" ";
					DataTable dt = DataFunctions.GetDataTable(sqlQuery);
					DataRow dr = dt.Rows[0];
					ContentText.Text= dr["ContentSMS"].ToString().Replace("%23","#");
				}		
			}
			catch{}
		}

        private void LoadWelcomeMessage(int CustomerID)
        {
            try
            {
                if (CustomerID != 0)
                {
                    SqlCommand cmd  = new SqlCommand(" SELECT TextPowerWelcomeMsg FROM Customer WHERE CustomerID=" + CustomerID + " ");
                    DataTable dt = DataFunctions.GetDataTable("accounts", cmd);
                    DataRow dr = dt.Rows[0];
                    WelcomeText.Text = dr["TextPowerWelcomeMsg"].ToString();
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        private void UpdateWelcomeMessage(int CustomerID)
        {
            try
            {
                if (WelcomeText.Text.Length > 0)
                {
                    SqlCommand sqlquery = new SqlCommand(
                        " UPDATE Customer SET " +
                        " TextPowerWelcomeMsg='" + WelcomeText.Text + "'" +
                        " WHERE CustomerID=" + CustomerID);
                    DataFunctions.Execute("accounts", sqlquery);
                }
                else
                {
                    throw new Exception("Auto Welcome Message cannot be empty");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public bool Save() 
		{
			int ContentID = 0;

			if (Page.IsValid)
			{
				try
				{
					if (ContentText.Text.ToString().Trim() == string.Empty)
					{
						ErrorMessage = "ERROR - Please enter Content before you proceed.";
						return false;
					}

                    if (WelcomeText.Text.ToString().Trim() == string.Empty)
                    {
                        ErrorMessage = "ERROR - Please enter Welcome message before you proceed.";
                        return false;
                    }


                    string[] toValidate = { ContentText.Text };
                    if(!ValidateCodeSnippetsPreSave(toValidate))
                    {
                        ErrorMessage = "ERROR - Content has invalid code snippets.";
						return false;
                    }

					if (plNewContent.Visible)
						ContentID = CreateContent();
					else
					{
						ContentID = Convert.ToInt32(drpContent.SelectedValue);
												
						//if((sc.CheckChannelAdmin() || sc.CheckSysAdmin() || sc.CheckAdmin()) || (es.HasPermission("ecn.communicator", "contentpriv") && es.HasPermission("ecn.communicator","writecontent")))
                        if(KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
						{
							UpdateContent();
                            UpdateWelcomeMessage(es.CurrentCustomer.CustomerID);
						}

					}
					ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);
				
					w.ID = WizardID;
					w.ContentID = ContentID;
					w.LayoutID = SaveLayout(w.LayoutID, w.WizardName, w.ContentID);
					w.CompletedStep = 3;
					w.Save();
			
					if (!ValidateCodeSnippet(w.GroupID, w.LayoutID))
						return false;
					else
						return true;
				}
				catch (Exception ex)
				{
					ErrorMessage = "ERROR - " + ex.Message;
				}
			}
			return false;
		}

		private int SaveLayout(int LayoutID, string MessageName, int ContentID) 
		{
			string sqlquery = "";
			
		
			if (LayoutID == 0)
			{
				sqlquery =  " INSERT INTO Layouts ( "+
					" LayoutName, CustomerID, UserID, FolderID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9,"+
					" TemplateID, ModifyDate, DisplayAddress, TableOptions "+
					" ) VALUES ( '" + MessageName.Replace("'", "''") + "' ," + sc.CustomerID() + "," + sc.UserID() + ",0," + ContentID + ",0,0,0,0,0,0,0,0, " + getTemplateID() + ",getdate(), '" + GetCustomerAddress().Replace("'", "''") + "','');SELECT @@IDENTITY";
			}
			else
			{
				sqlquery =  " Update Layouts  "+
					" Set LayoutName = '" + MessageName.Replace("'", "''") + "', ContentSlot1 = " + ContentID + ", TableOptions = '', ModifyDate = getdate() " +
					" Where LayoutID = " + LayoutID + "; select " + LayoutID;
			}
			try
			{
				LayoutID = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			} 

			return LayoutID;
		}

		private bool ValidateCodeSnippet(int GroupID, int LayoutID)
		{
			bool bSuccess = true;

			try
			{			
				SqlCommand cmd = new SqlCommand("sp_ValidateCodeSnippet");
				cmd.CommandTimeout	= 0;
				cmd.CommandType  = CommandType.StoredProcedure;
			
				cmd.Parameters.Add(new SqlParameter("@layoutID", SqlDbType.VarChar,100));
				cmd.Parameters["@layoutID"].Value = LayoutID;
				cmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.VarChar,100));
				cmd.Parameters["@groupID"].Value = GroupID;

				DataFunctions.Execute(cmd);
			}
			catch(SqlException ex)
			{
				bSuccess=false;
				ErrorMessage= "ERROR - " + ex.Message.ToString();
			}		

			return bSuccess;
		}

		private int getTemplateID()
		{
			string selectQuery = "select top 1 TemplateID from templates where slotstotal = 1 and activeflag = 'Y' and templatestylecode = 'newsletter' and ChannelID in (select BaseChannelID from "+ConfigurationManager.AppSettings["accountsdb"]+"dbo.customer where customerID = " + sc.CustomerID() + ")";
			return Convert.ToInt32(DataFunctions.ExecuteScalar(selectQuery));
		}
		
		private string GetCustomerAddress() 
		{
			string displayaddr = "";

			string selectQuery = "SELECT Address, City, State, Zip FROM "+ConfigurationManager.AppSettings["accountsdb"]+".dbo.Customer WHERE CustomerID=" + sc.CustomerID();

			DataRow dr = (DataFunctions.GetDataTable(selectQuery)).Rows[0];
			displayaddr = DataFunctions.CleanString((dr["Address"].ToString()+", "+dr["City"].ToString()+", "+dr["State"].ToString()+" - "+dr["Zip"].ToString()));

			return displayaddr;
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

		protected void drpContent_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (drpContent.SelectedValue != "")
			{
				LoadContentDetails(Convert.ToInt32(drpContent.SelectedValue));
			}
			else 
			{
				ContentText.Text = "";
			}
		}

		public int CreateContent() 
		{
			int contentID = 0;
			string ctext=string.Empty;

			if(!(checkContentExists(txtContentTitle.Text.Trim())))
			{
            

				if (ContentText.Text.Length>0) 
					ctext=DataFunctions.CleanString(ContentText.Text);				

				string cTitle = DataFunctions.CleanString(txtContentTitle.Text.Trim().ToString());

				string sqlquery=
					" INSERT INTO Content ( "+
					" ContentTitle, ContentTypeCode, LockedFlag, UserID, FolderID, "+
					" ContentSource, ContentText,ContentSMS,  ContentMobile, ContentURL, ContentFilePointer, "+
					" CustomerID, ModifyDate, Sharing "+
					" ) VALUES ( "+
					" '"+cTitle+"', 'SMS', 'N', "+ sc.UserID() +", " + drpFolder1.SelectedValue + ", "+
                    " '','', '" + ReplaceAnchor(ctext) + "', '', '', '', " +
					" "+sc.CustomerID()+", getdate(), 'N');SELECT @@IDENTITY ";

				try
				{
					contentID = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));
				}
				catch(Exception ex)
				{
					throw new Exception("Error Occured when creating Content : "+ex.ToString());
				}
			}
			else
			{
				throw new Exception("Content title <font color='#000000'>\""+txtContentTitle.Text+"\"</font> already exists. Please enter a different title.");
			}
			
			return contentID;
		}

        private bool ValidateCodeSnippetsPreSave(string[] toValidate)
        {
            foreach (string s in toValidate)
            {
                //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
                System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
                if (MatchList.Count > 0)
                {
                    if ((MatchList.Count % 2) != 0)
                    {
                        return false;
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
                        if ((MatchList.Count / 2) > MatchListGood.Count)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

		private bool checkContentExists(string contentTitle)
		{
			bool exists = false;	

			if(Convert.ToInt32(DataFunctions.ExecuteScalar("SELECT count(ContentID) FROM Content WHERE customerID=" + sc.CustomerID() + " and ContentTitle = '" + contentTitle.Replace("'","''") +"'")) > 0)
				exists = true;

			return exists;
		}

		public void UpdateContent() 
		{      

			string ctext=DataFunctions.CleanString(ContentText.Text);

			string sqlquery=
				" UPDATE Content SET "+
				" ContentSMS='"+ReplaceAnchor(ctext)+"', "+
				" ModifyDate=getdate() "+
				" WHERE ContentID=" + drpContent.SelectedValue;
				
			DataFunctions.Execute(sqlquery);
		}
        
        private string ReplaceAnchor(string str)
        {
            Regex regExp_href_tags = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection link_Collection = regExp_href_tags.Matches(str);
            foreach (Match m in link_Collection)
            {
                foreach (Group g in m.Groups)
                    if (g.Value.Length > 0 && g.Value.IndexOf("#") > 10)
                        str = str.Replace(g.Value, g.Value.Replace("#", "%23"));
            }
            return str;
        }		

		private string StripHTML(string html)
		{
			try
			{
				string result;

				Regex regExp_A_tags		= new Regex("<\\s*a\\s*.*href\\s*=\\s*(?:(?:\\\"(?<url>[^\\\"]*)\\\")|(?<url>[^\\s]* )|(?:(?:\\'(?<url>[^\\']*)\\')))>");
			
				MatchCollection A_tags_Collection = regExp_A_tags.Matches(html); 
				foreach(Match m in A_tags_Collection) 
				{ 
					foreach(Group g in m.Groups) 
					{ 
						if(g.Value.Length > 0)
						{
							html = html.Replace(m.Value.Trim().ToString(), "[URL: "+g.Value.Trim().ToString()+"] ");
						}
					} 
					html = html.Replace("[URL: [URL:","[URL:");
					html = html.Replace("<a [URL:","[URL:");
					html = html.Replace("] ]","]");	

				}

				// Remove HTML Development formatting
				result = html.Replace("\r", string.Empty);
				result = html.Replace("\n", string.Empty);

				// Remove step-formatting
				result = result.Replace("\t", string.Empty);
				// Remove repeating speces becuase browsers ignore them
				result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");

				// Remove the header (prepare first by clearing attributes)
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*head([^>])*>","<head>",  System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*head( )*>)","</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)",string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// remove all scripts (prepare first by clearing attributes)
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*script([^>])*>","<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*script( )*>)","</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				//result = System.Text.RegularExpressions.Regex.Replace(result, 
				//         @"(<script>)([^(<script>\.</script>)])*(</script>)",
				//         string.Empty, 
				//         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"(<script>).*(</script>)",string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        
				// remove all styles (prepare first by clearing attributes)
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*style([^>])*>","<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*style( )*>)","</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)",string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// insert tabs in spaces of <td> tags
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*td([^>])*>","\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// insert line breaks in places of <BR> and <LI> tags
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*br( )*>","\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*li( )*>","\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// insert line paragraphs (double line breaks) in place
				// if <P>, <DIV> and <TR> tags
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*div([^>])*>","\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*tr([^>])*>","\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*p([^>])*>","\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// Remove remaining tags like <a>, links, images,
				// comments etc - anything thats enclosed inside < >
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]*>",string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// replace special characters:
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&nbsp;"," ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&bull;"," * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);    
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&lsaquo;","<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&rsaquo;",">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&trade;","(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&frasl;","/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"<","<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @">",">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&copy;","(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);        
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&reg;","(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);    
				// Remove all others. More can be added, see
				// http://hotwired.lycos.com/webmonkey/reference/special_characters/
				result = System.Text.RegularExpressions.Regex.Replace(result, @"&(.{2,6});", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);    

				// for testng
				//System.Text.RegularExpressions.Regex.Replace(result, 
				//       this.txtRegex.Text,string.Empty, 
				//       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				// make line breaking consistent
				result = result.Replace("\n", "\r");

				// Remove extra line breaks and tabs:
				// replace over 2 breaks with 2 and over 4 tabs with 4. 
				// Prepare first to remove any whitespaces inbetween
				// the escaped characters and remove redundant tabs inbetween linebreaks
				result = System.Text.RegularExpressions.Regex.Replace(result, 
					"(\r)( )+(\r)","\r\r", 
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, 
					"(\t)( )+(\t)","\t\t", 
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, 
					"(\t)( )+(\r)","\t\r", 
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				result = System.Text.RegularExpressions.Regex.Replace(result, 
					"(\r)( )+(\t)","\r\t", 
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Remove redundant tabs
				result = System.Text.RegularExpressions.Regex.Replace(result, 
					"(\r)(\t)+(\r)","\r\r", 
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Remove multible tabs followind a linebreak with just one tab
				result = System.Text.RegularExpressions.Regex.Replace(result, 
					"(\r)(\t)+","\r\t", 
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				// Initial replacement target string for linebreaks
				string breaks = "\r\r\r";
				// Initial replacement target string for tabs
				string tabs = "\t\t\t\t\t";
				for (int index=0; index<result.Length; index++)
				{
					result = result.Replace(breaks, "\r\r");
					result = result.Replace(tabs, "\t\t\t\t");
					breaks = breaks + "\r";    
					tabs = tabs + "\t";
				}

				
				//REPLACE '[URL:' AND ']' TO '<' AND '>' respectively. By doing this, the text links won't break.
				result = result.Replace("[URL: ", "[URL: <");
				result = result.Replace("]", "> ]");


				// Thats it.
				return result;

			}
			catch
			{
				return string.Empty;
			}
		}

		protected void drpFolder_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			LoadContentDR(Convert.ToInt32(drpFolder.SelectedValue), 0);
		}

		protected void rbExistingContent_CheckedChanged(object sender, System.EventArgs e)
		{
			plExistingContent.Visible=true;
			plNewContent.Visible = false;
			Initialize();
		}

		protected void rbNewContent_CheckedChanged(object sender, System.EventArgs e)
		{
			plExistingContent.Visible=false;
			plNewContent.Visible = true;
			ContentText.Text = "";
            if (WizardID > 0)
            {
                ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);
                if (w.PageWatchID > 0)
                {
                    string url = PageWatch.GetPageWatchURL(w.PageWatchID);
                    ContentText.Text = "Content has changed on the following page: " + url;
                }
            }
		}
    }
}
