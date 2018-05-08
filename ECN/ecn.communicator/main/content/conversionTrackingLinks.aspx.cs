using System;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.contentmanager 
{
	public partial class conversionTrackingLinks : ECN_Framework.WebPageHelper
    {

		ArrayList allLinks = new ArrayList();
		ArrayList linksToAlias = new ArrayList();

		public event EventHandler OnLinkItemAdded;
		int linksAdded = 1;
		int maxSortOrder = 0;

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }
        
		protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT; 
            Master.SubMenu = "new content";
            Master.Heading = "Conversion Tracking Links";
            Master.HelpContent = "";
            Master.HelpTitle = "Content Manager";	

            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking))
            {
				int layoutID = getLayoutID();
                List<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinks = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID, Master.UserSession.CurrentUser);
				if(!(Page.IsPostBack))
                {
                    if (conversionLinks.Count == 0)
                    {
						InsertConversionLinks();
					}
                    conversionLinks = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID, Master.UserSession.CurrentUser);

                    if (conversionLinks.Count > 0)
                    {
						msglabel.Visible = false;
						UpdateLinksBTN.Visible = true;
                        BuildConversionLinksGrid(layoutID);
					}
                    else
                    {
						msglabel.Visible = true;
						UpdateLinksBTN.Visible = false;
						msglabel.Text = "<br><br>There are no Conversion Tracking links that could be identified";
					}

                    if (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking, KMPlatform.Enums.Access.Edit))
                    {
                        ConversionTrackingLinksList.ShowFooter = true;
                        UpdateLinksBTN.Visible = true;
                    }
                    else
                    {
                        ConversionTrackingLinksList.ShowFooter = false;
                        UpdateLinksBTN.Visible = false;
                    }
				}
			}
            else
            {
				msglabel.Visible = true;
				UpdateLinksBTN.Visible = false;
				msglabel.Text = "<br><br>Conversion Tracking is an advanced ECN feature that allows for increased marketing intelligence through extended link tracking - a perfect extension to general reporting of clicks and opens. Conversion tracking uses specific URLs in your campaign to discover not only which links were clicked, but which generated a completed order page or any other action you desire.<br><br>If you are interested in this feature, please contact our Customer Service Department at 1-866-844-6275.";				
			}
		}

		#region getters
		private int getLayoutID()
        {
			int layoutID	= 0;
            if(Request.QueryString["layoutID"]!=null)
            {
				layoutID	= Convert.ToInt32(Request.QueryString["layoutID"].ToString());
			}
            else
            {
				layoutID = 0;
			}		
			return layoutID;
		}
		#endregion

		private void BuildConversionLinksGrid(int layoutID)
        {
            List<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinks = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID, Master.UserSession.CurrentUser);

            if (conversionLinks.Count > 0)
            {
                ConversionTrackingLinksList.DataSource = conversionLinks;
				ConversionTrackingLinksList.DataBind();
			}
		}

		private void InsertConversionLinks()
        {
            ECN_Framework_Entities.Communicator.Layout layout= ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(Convert.ToInt32(getLayoutID()), Master.UserSession.CurrentUser, true);
            if (layout.ContentSlot1 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot1.ToString());
            }
            if (layout.ContentSlot2 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot2.ToString());
            }
            if (layout.ContentSlot3 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot3.ToString());
            }
            if (layout.ContentSlot4 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot4.ToString());
            }
            if (layout.ContentSlot5 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot5.ToString());
            }
            if (layout.ContentSlot6 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot6.ToString());
            }
            if (layout.ContentSlot7 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot7.ToString());
            }
            if (layout.ContentSlot8 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot8.ToString());
            }
            if (layout.ContentSlot9 > 0)
            {
                InsertLinksFromContent(layout.ContentSlot9.ToString());
            }
		}

		protected void UpdateLinksBTN_Click(object sender, System.EventArgs e)
        {
            List<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinksList = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(getLayoutID(), Master.UserSession.CurrentUser);

            foreach (ECN_Framework_Entities.Communicator.ConversionLinks conversionLinks in conversionLinksList)
            {
                maxSortOrder = conversionLinks.SortOrder.Value;
			}

            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(Convert.ToInt32(getLayoutID()), Master.UserSession.CurrentUser, true);
            if (layout.ContentSlot1 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot1.ToString());
            }
            if (layout.ContentSlot2 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot2.ToString());
            }
            if (layout.ContentSlot3 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot3.ToString());
            }
            if (layout.ContentSlot4 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot4.ToString());
            }
            if (layout.ContentSlot5 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot5.ToString());
            }
            if (layout.ContentSlot6 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot6.ToString());
            }
            if (layout.ContentSlot7 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot7.ToString());
            }
            if (layout.ContentSlot8 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot8.ToString());
            }
            if (layout.ContentSlot9 > 0)
            {
                CheckLinksFromContent(layout.ContentSlot9.ToString());
            }

            BuildConversionLinksGrid(getLayoutID());
		}

        private void CheckLinksFromContent(String contentID)
        {
            try
            {
                ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(Convert.ToInt32(contentID), Master.UserSession.CurrentUser, true);
                List<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinksList = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(getLayoutID(), Master.UserSession.CurrentUser);

                string contentSource = content.ContentSource;

                DataTable newDT = new DataTable();
                newDT.Columns.Add(new DataColumn("Link"));
                newDT.Columns.Add(new DataColumn("SortOrder"));
                DataRow newDR;

                Regex r;
                Match m;
                r = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
                string linkFound = "";
                for (m = r.Match(contentSource); m.Success; m = m.NextMatch())
                {
                    newDR = newDT.NewRow();
                    linkFound = m.Groups[1].ToString();
                    linkFound = ECN_Framework_Common.Functions.StringFunctions.Replace(linkFound, "&amp;", "&");
                    if (linkFound.Length > 1)
                    {
                        linkFound = linkFound.Replace("%%ConversionTrkCDE%%", "");
                        IEnumerable<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinksSelect = conversionLinksList.Where(x => x.LinkURL == linkFound);
                        if (conversionLinksSelect.Count() == 0)
                        {
                            maxSortOrder = maxSortOrder + 1;
                            newDR[0] = linkFound.ToString();
                            newDR[1] = maxSortOrder;
                            newDT.Rows.Add(newDR);
                        }
                    }
                }
                foreach (DataRow dr in newDT.Rows)
                {
                    ECN_Framework_Entities.Communicator.ConversionLinks conversionLinks = new ECN_Framework_Entities.Communicator.ConversionLinks();
                    conversionLinks.LayoutID = getLayoutID();
                    conversionLinks.IsActive = "Y";
                    conversionLinks.LinkURL = dr["Link"].ToString();
                    conversionLinks.SortOrder = Convert.ToInt32(dr["SortOrder"].ToString());
                    conversionLinks.LinkParams = "";
                    conversionLinks.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    conversionLinks.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Save(conversionLinks, Master.UserSession.CurrentUser);
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

		private void InsertLinksFromContent(String contentID) 
        {
            try
            {
                ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(Convert.ToInt32(contentID), Master.UserSession.CurrentUser, false);

                string contentSource = content.ContentSource;
			
			    DataTable newDT = new DataTable();
			    newDT.Columns.Add(new DataColumn("Link"));
			    newDT.Columns.Add(new DataColumn("SortOrder"));
			    DataRow newDR;
			
			    Regex r;
			    Match m;
			    r = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))",
				    RegexOptions.IgnoreCase|RegexOptions.Compiled);
			    string linkFound	= "";
			    for (m = r.Match(contentSource); m.Success; m = m.NextMatch()) {
				    newDR		= newDT.NewRow();
				    linkFound	= m.Groups[1].ToString();
				    linkFound	= ECN_Framework_Common.Functions.StringFunctions.Replace(linkFound,"&amp;","&");
				    if(linkFound.Length > 1)
                    {
					    linkFound	= linkFound.Replace("%%ConversionTrkCDE%%", "");
					    newDR[0]	= linkFound.ToString();
					    newDR[1]	= linksAdded++;
					    newDT.Rows.Add(newDR);
				    }
			    }

                foreach (DataRow dr in newDT.Rows)
                {
                    ECN_Framework_Entities.Communicator.ConversionLinks conversionLinks = new ECN_Framework_Entities.Communicator.ConversionLinks();
                    conversionLinks.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    conversionLinks.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    conversionLinks.LayoutID = getLayoutID();
                    conversionLinks.IsActive = "Y";
                    conversionLinks.LinkName = " ";
                    conversionLinks.LinkURL = dr["Link"].ToString();
                    conversionLinks.SortOrder = Convert.ToInt32(dr["SortOrder"].ToString());
                    conversionLinks.LinkParams = "";
                    ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Save(conversionLinks, Master.UserSession.CurrentUser);
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}

		#region DataList Events
		private void ConversionTrackingLinksList_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e) 
        {			
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton delete = e.Item.FindControl("ConversionLinkDelete") as LinkButton;
                LinkButton edit = e.Item.FindControl("ConversionLinkEdit") as LinkButton;


                if (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking, KMPlatform.Enums.Access.Edit))
                {
                    edit.Visible = true;
                    return;
                }
                else
                {
                    edit.Visible = false;
                    return;
                }

                if (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking, KMPlatform.Enums.Access.Delete))
                {
                    delete.Attributes.Add("onclick", "return confirm('LINK ID: " + (int)ConversionTrackingLinksList.DataKeys[(int)e.Item.ItemIndex] + " - Are you sure that you want to delete this Tracking Link ?')");
                    return;
                }
                else
                {
                    delete.Visible = false;
                    return;
                }
			}

			if (e.Item.ItemType == ListItemType.EditItem) 
            {
                
				TextBox Edit_LinkNameTXT		= e.Item.FindControl("Edit_LinkNameTXT") as TextBox;
				TextBox Edit_LinkURLTXT		= e.Item.FindControl("Edit_LinkURLTXT") as TextBox;
				CheckBox Edit_IsActiveCHKBX	= e.Item.FindControl("Edit_IsActiveCHKBX") as CheckBox;
				TextBox Edit_SortOrderTXT	= e.Item.FindControl("Edit_SortOrderTXT") as TextBox;
                List<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinksList = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(getLayoutID(), Master.UserSession.CurrentUser);
                List<ECN_Framework_Entities.Communicator.ConversionLinks> conversionLinksSelect = (from src in conversionLinksList
                                                                                                   where src.LinkID == (int)ConversionTrackingLinksList.DataKeys[(int)e.Item.ItemIndex]
                                                                                                   select src).ToList();
                Edit_LinkNameTXT.Text = conversionLinksSelect[0].LinkName.ToString();
                Edit_LinkURLTXT.Text = conversionLinksSelect[0].LinkURL.ToString();
                string active = conversionLinksSelect[0].IsActive.ToString();
				if(active.Equals("Y"))
                {
					Edit_IsActiveCHKBX.Checked = true;
				}
                else
                {
					Edit_IsActiveCHKBX.Checked = false;
				}
                Edit_SortOrderTXT.Text = conversionLinksSelect[0].SortOrder.ToString();
			}
		}

		private void ConversionTrackingLinksList_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e) 
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Edit":
                        ConversionTrackingLinksList.EditItemIndex = e.Item.ItemIndex;
                        break;
                    case "Update":
                        ECN_Framework_Entities.Communicator.ConversionLinks conversionLinks = ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLinkID((int)ConversionTrackingLinksList.DataKeys[(int)e.Item.ItemIndex], Master.UserSession.CurrentUser, true);
               
                        TextBox Edit_LinkNameTXT = e.Item.FindControl("Edit_LinkNameTXT") as TextBox;
                        TextBox Edit_LinkURLTXT = e.Item.FindControl("Edit_LinkURLTXT") as TextBox;
                        CheckBox Edit_IsActiveCHKBX = e.Item.FindControl("Edit_IsActiveCHKBX") as CheckBox;
                        TextBox Edit_SortOrderTXT = e.Item.FindControl("Edit_SortOrderTXT") as TextBox;
                        string name = Edit_LinkNameTXT.Text.ToString().Replace("'", "");
                        string url = Edit_LinkURLTXT.Text.ToString().Replace("'", "");
                        string active = "N";
                        if (Edit_IsActiveCHKBX.Checked)
                        {
                            active = "Y";
                        }
                        conversionLinks.LinkURL = url;
                        conversionLinks.LinkParams = "";
                        conversionLinks.LinkName = name;
                        conversionLinks.IsActive = active;
                        conversionLinks.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                        conversionLinks.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                        conversionLinks.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                        conversionLinks.SortOrder = Convert.ToInt32(Edit_SortOrderTXT.Text.ToString().Replace("'", ""));
                        ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Save(conversionLinks, Master.UserSession.CurrentUser);

                        ConversionTrackingLinksList.EditItemIndex = -1;
                        FireItemChangeEvent();
                        break;
                    case "Cancel":
                        ConversionTrackingLinksList.EditItemIndex = -1;
                        break;
                    case "Delete":
                        DeleteLink((int)ConversionTrackingLinksList.DataKeys[(int)e.Item.ItemIndex], getLayoutID());
                        FireItemChangeEvent();
                        break;
                }
                BuildConversionLinksGrid(getLayoutID());
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }

		}

		protected void ConversionLinkAdd_Click(object sender, System.EventArgs e) 
        {	
            try
            {
			    LinkButton btn = sender as LinkButton;
			    TextBox Add_LinkNameTXT			= btn.Parent.FindControl("Add_LinkNameTXT") as TextBox;
			    TextBox Add_LinkURLTXT			= btn.Parent.FindControl("Add_LinkURLTXT") as TextBox;
			    CheckBox Add_IsActiveCHKBX		= btn.Parent.FindControl("Add_IsActiveCHKBX") as CheckBox;
			    TextBox Add_SortOrderTXT		= btn.Parent.FindControl("Add_SortOrderTXT") as TextBox;
			    string name		= Add_LinkNameTXT.Text.ToString().Replace("'","");
			    string url			= Add_LinkURLTXT.Text.ToString().Replace("'","");
			    string parms	= ""; 
			    string active	= "N";
			    if(Add_IsActiveCHKBX.Checked)
                {
				    active	= "Y";
			    }
			    string order	= Add_SortOrderTXT.Text.ToString().Replace("'","");
                if (string.IsNullOrEmpty(order))
                {
                    order = (ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByLayoutID(getLayoutID(), Master.UserSession.CurrentUser).Max(x => x.SortOrder).Value + 1).ToString();
                }
                ECN_Framework_Entities.Communicator.ConversionLinks conversionLinks = new ECN_Framework_Entities.Communicator.ConversionLinks();
                conversionLinks.LayoutID = getLayoutID();
                conversionLinks.LinkURL = url;
                conversionLinks.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                conversionLinks.LinkParams = parms;
                conversionLinks.LinkName = name;
                conversionLinks.IsActive = active;
                conversionLinks.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                conversionLinks.SortOrder = Int32.Parse(order);
                ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Save(conversionLinks, Master.UserSession.CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}

		private void DeleteLink(int linkID, int layoutID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Delete(layoutID, linkID, Master.UserSession.CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}

		private void FireItemChangeEvent() 
        {
			if (OnLinkItemAdded!=null) 
            {
				OnLinkItemAdded(this, null);
			}
		}
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    
			this.ConversionTrackingLinksList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.ConversionTrackingLinksList_ItemCommand);
			this.ConversionTrackingLinksList.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.ConversionTrackingLinksList_ItemDataBound);

		}
		#endregion

	}
}
