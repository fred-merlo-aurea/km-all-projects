using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.contentmanager
{

    public partial class linkAlias : ECN_Framework.WebPageHelper
    {

        int contentID = 0;
        ArrayList allLinks = new ArrayList();
        ArrayList linksToAlias = new ArrayList();
        List<ECN_Framework_Entities.Communicator.LinkAlias> contentLinkAliasesList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
        List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> linkOwnerIndexList = new List<ECN_Framework_Entities.Communicator.LinkOwnerIndex>();
        List<ECN_Framework_Entities.Communicator.Code> codeList = new List<ECN_Framework_Entities.Communicator.Code>();

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
            Master.Heading = "Alias Links";
            Master.HelpContent = "<strong>Add / Edit Content</strong><p>To create a new content for the email: <br/><br/><ul><li>Enter a title for the content in the <em>Title</em> field and select the <em>Type</em> of content html / text.</li><br/><br/><li>Using the HTML enabled Wysiwyg editor create the content.</li><br/><br/><li>select the owner from the <em>Owner</em> dropdown list and check the<em> Locked</em> check box to lock the content.</li><br/><br/><li>Hit the Create button to create the content.</li></ul></p>";
            Master.HelpTitle = "Content Manager";

            if (Request.QueryString["contentID"] != null)
            {
                contentID = Int32.Parse(Request.QueryString["contentID"].ToString());
            }
            else
            {
                contentID = 0;
            }

            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
            {
                loadContentLinkAliasesDT(contentID);

                linkOwnerIndexList = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
                codeList = ECN_Framework_BusinessLayer.Communicator.Code.GetByCustomerAndCategory(ECN_Framework_Common.Objects.Communicator.Enums.CodeType.LINKTYPE, Master.UserSession.CurrentUser);
                if (!(Page.IsPostBack))
                    loadNewLinkAliasGrid(contentID);
                else
                {
                    if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner, KMPlatform.Enums.Access.Edit))
                        saveLinkAlias();
                    else
                    {
                        SaveButton.Visible = false;
                    }
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        #region Load Datasets
        private void loadContentLinkAliasesDT(int contentID)
        {
            contentLinkAliasesList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID(contentID, Master.UserSession.CurrentUser, true);
        }

       
        #endregion

        private void loadNewLinkAliasGrid(int contentID)
        {
            referenceLabel.Text = "<a href=\"#\" onClick=\"window.open('contentPreview.aspx?ContentID=" + contentID + "&type=html&requestFrom=linkAlias','ReferContent', 'left=100,top=100,height=550,width=600,resizable=yes,scrollbars=yes,status=yes')\">Refer to Content</a>";
            ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID,Master.UserSession.CurrentUser, true);
    
            DataTable newDT = new DataTable();
            newDT.Columns.Add(new DataColumn("<center>Link</center>"));
            newDT.Columns.Add(new DataColumn("<center>Link Alias</center>"));

            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
            {
                newDT.Columns.Add(new DataColumn("<center>Link Owner / Link Type</center>"));
            }
            DataRow newDR;

            int i = 0;
            int linkNumber = 0;
            string linkFound = "";
            string aliasProperties = "";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content.ContentSource);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");
            System.Collections.Generic.List<string> linkList = new System.Collections.Generic.List<string>();
            System.Collections.Generic.List<string> titleList = new System.Collections.Generic.List<string>();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string title = "";
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attTitle = node.Attributes["title"];
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["href"];
                    if (attTitle != null)
                    {
                        title = attTitle.Value;
                    }
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                    titleList.Add(title);
                }
            }

            for (int aLoop = 0; aLoop < linkList.Count; aLoop++)
            {
                linkFound = linkList[aLoop];

                newDR = newDT.NewRow();
                linkFound = ECN_Framework_Common.Functions.StringFunctions.Replace(linkFound, "&amp;", "&");

                //since we replace the Anchors while creating the Content, we need to put it back in place while doing the link Alias. - ashok 11/20/07
                linkFound = RegexUtilities.GetCleanUrlContent(linkFound);

                string linkfnd = Server.UrlEncode(linkFound);
                if ((linkFound.Length > 1) && !(checkLinkExistsinArray(linkFound)))
                {
                    newDR[0] = "<input type=text class='formfield' name='hiddenLink" + linkNumber + "' value=\"" + linkFound.ToString() + "\" maxlength=200 size=65>";
                    aliasProperties = getLinkAliasProperties(linkFound, titleList[aLoop]);

                    string linkAlias = aliasProperties.Split('|')[0];
                    string linkOwnerID = aliasProperties.Split('|')[1];
                    string linkTypeID = aliasProperties.Split('|')[2];

                    //Link Alias Names with Link Active Status
                    if (linkAlias.Length > 0)
                        newDR[1] = "&nbsp;<input type=text class='formfield' name='alias" + linkNumber + "' maxlength=200 size=55 value=\"" + linkAlias + "\">&nbsp;";
                    else
                        newDR[1] = "&nbsp;<input type=text class='formfield' name='alias" + linkNumber + "' maxlength=200 size=55 value=''>&nbsp;";

                    if (linkAlias.Equals("[404] PAGE NOT FOUND"))
                        newDR[1] += "&nbsp;<sub><img src='no-link-warning-small.jpg' border=0 alt='[404] PAGE NOT FOUND. Page cannot be located on the website.'></sub>";
                    else if (linkAlias.Length == 0)
                        newDR[1] += "&nbsp;<sub><img src='no-link-warning-small.jpg' border=0 alt='Bad [OR] not a valid URL. Valid URL should start with a http:// or https:// '></sub>";

                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
                    {
                        //Link Owner & Link Type
                        newDR[2] = loadLinkOwnerDDown(linkOwnerID, linkNumber);
                        newDR[2] += "&nbsp;";
                        newDR[2] += loadLinkTypeDDown(linkTypeID, linkNumber);
                    }

                    newDT.Rows.Add(newDR);
                    linkNumber++;
                }
                i++;
            }
            TotalLinks.Text = linkNumber.ToString();

            NewLinkAliasGrid.DataSource = new DataView(newDT);
            NewLinkAliasGrid.CurrentPageIndex = 0;
            NewLinkAliasGrid.PageSize = 9999;
            NewLinkAliasGrid.DataBind();
        }

        #region Load Link Owner & Type Dropdowns
        private string loadLinkOwnerDDown(string linkOwnerID, int linkNumber)
        {
            string ddHtml = "<select name='aliasLinkOwner" + linkNumber + "' class='formfield' >";
            string selected = "";
            bool anySelected = false;
            var result = (from src in linkOwnerIndexList
                          orderby src.LinkOwnerName
                          select src).ToList();

            foreach (ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwnerIndex in result)
            {
                if (linkOwnerIndex.LinkOwnerIndexID.ToString().Equals(linkOwnerID))
                {
                    selected = "selected";
                    anySelected = true;
                }
                else
                {
                    selected = "";
                }
                ddHtml += "<option value=" + linkOwnerIndex.LinkOwnerIndexID.ToString() + " " + selected + ">" + linkOwnerIndex.LinkOwnerName.ToString() + "</option>";              
            }
            
            if (anySelected)
            {
                ddHtml += "<option value=0></option>";
            }
            else
            {
                ddHtml += "<option value=0 selected></option>";
            }
            ddHtml += "</select>";

            return ddHtml;
        }

        private string loadLinkTypeDDown(string linkTypeID, int linkNumber)
        {
            string ddHtml = "<select name='aliasLinkType" + linkNumber + "' class='formfield' style='padding-top:3px'>";
            string selected = "false";
            bool anySelected = false;
            var result = (from src in codeList
                          orderby src.CodeDisplay
                          select src).ToList();
            foreach (ECN_Framework_Entities.Communicator.Code code in result)
            {
                if (code.CodeID.ToString().Equals(linkTypeID))
                {
                    selected = "selected";
                    anySelected = true;
                }
                else
                {
                    selected = "";
                }
                ddHtml += "<option value=" + code.CodeID.ToString() + " " + selected + ">" + code.CodeDisplay.ToString() + "</option>";
             
            }

            if (anySelected)
            {
                ddHtml += "<option value=0></option>";
            }
            else
            {
                ddHtml += "<option value=0 selected></option>";
            }
            ddHtml += "</select>";
            return ddHtml;
        }
        #endregion

        #region Check for Link Duplication
        private bool checkLinkExistsinArray(string linkFound)
        {
            bool exists = true;
            if (allLinks.Contains(linkFound))
            {
                exists = true;
            }
            else
            {
                exists = false;
                if (linkFound.Length > 1)
                {
                    allLinks.Add(linkFound);
                }
            }
            return exists;
        }
        #endregion

        private string getLinkAliasProperties(string linkFound, string title)
        {
            string aliasName = "", linkOwnerID = "", linkTypeID = "";
            try
            {
                try
                {                
                    foreach(ECN_Framework_Entities.Communicator.LinkAlias linkAlias in contentLinkAliasesList.Where(x=>x.Link==linkFound))
                    {
                        aliasName = linkAlias.Alias;
                        linkOwnerID = linkAlias.LinkOwnerID.ToString();
                        linkTypeID = linkAlias.LinkTypeID.ToString();
                    }
                }
                catch
                {
                    aliasName = "";
                }
               
                if (!(aliasName.ToString().Trim().Length > 0))
                {
                    if (title.Trim().Length > 0)
                    {
                        aliasName = title.Trim();
                    }
                    else
                    {
                        aliasName = ECN_Framework_Common.Functions.URLFunctions.getWebPageTitle(linkFound);
                    }
                }
            }
            catch
            {
                aliasName = "";
            }

            linkOwnerID = linkOwnerID.Length > 0 ? linkOwnerID : "0";
            linkTypeID = linkTypeID.Length > 0 ? linkTypeID : "0";

            return aliasName + "|" + linkOwnerID + "|" + linkTypeID;
        }

        public void saveLinkAlias()
        {
            string requestAlias = "";
            string requestLink = "", requestLinkOwnerID = "", requestLinkTypeID = "";
            string dbAlias = "";
            ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID, Master.UserSession.CurrentUser, true);                    
            try
            {
                for (int i = 0; i < Convert.ToInt32(TotalLinks.Text.ToString()); i++)
                {
                    requestAlias = Request["alias" + i].ToString();
                    requestAlias = requestAlias.Replace("'", string.Empty);
                    requestLink = Request["hiddenLink" + i].ToString();

                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
                    {
                        requestLinkOwnerID = Request["aliasLinkOwner" + i].ToString();
                        requestLinkTypeID = Request["aliasLinkType" + i].ToString();
                    }
                    else
                    {
                        requestLinkOwnerID =null;
                        requestLinkTypeID = null;
                    }

                    try
                    {
                        dbAlias = "";
                        foreach (ECN_Framework_Entities.Communicator.LinkAlias linkAlias in contentLinkAliasesList.Where(x => x.Link == requestLink))
                        {
                            dbAlias = linkAlias.Alias;
                        }
                    }
                    catch
                    {
                        dbAlias = "";
                    }               
                    if (dbAlias.Equals("") && requestAlias != string.Empty)
                    {
                        ECN_Framework_Entities.Communicator.LinkAlias linkAlias = new ECN_Framework_Entities.Communicator.LinkAlias();
                        linkAlias.ContentID = contentID;
                        linkAlias.Link = requestLink;
                        linkAlias.Alias = (requestAlias.Length > 200 ? requestAlias.Substring(1, 200) : requestAlias);
                        if (requestLinkOwnerID != null && requestLinkOwnerID != null)
                        {
                            if (requestLinkOwnerID.Trim().Length > 0 && Convert.ToInt32(requestLinkOwnerID.Trim()) > 0)
                                linkAlias.LinkOwnerID = Convert.ToInt32(requestLinkOwnerID);
                            if (requestLinkTypeID.Trim().Length > 0 && Convert.ToInt32(requestLinkTypeID.Trim()) > 0)
                                linkAlias.LinkTypeID = Convert.ToInt32(requestLinkTypeID);
                        }
                        linkAlias.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                        linkAlias.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkAlias.Save(linkAlias, Master.UserSession.CurrentUser);
                    }
                    else if (dbAlias != string.Empty && requestAlias == string.Empty)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkAlias.Delete(contentID, requestLink, Master.UserSession.CurrentUser);
                    }
                    else if (dbAlias != string.Empty && requestAlias != string.Empty)
                    {
                        foreach (ECN_Framework_Entities.Communicator.LinkAlias linkAlias in content.AliasList.Where(x => x.Link == requestLink))
                        {
                            linkAlias.Alias = (requestAlias.Length > 200 ? requestAlias.Substring(1, 200) : requestAlias);
                            linkAlias.LinkOwnerID = Convert.ToInt32(requestLinkOwnerID);
                            linkAlias.LinkTypeID = Convert.ToInt32(requestLinkTypeID);
                            linkAlias.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                            if(linkAlias.CreatedUserID==null)
                                linkAlias.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                            linkAlias.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                            ECN_Framework_BusinessLayer.Communicator.LinkAlias.Save(linkAlias, Master.UserSession.CurrentUser);
                        }
                    }                          
                }
                Response.Redirect("default.aspx");   
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
          
        }

        protected void NewLinkAliasGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Cells[0].Width = new Unit(44, UnitType.Percentage);
            e.Item.Cells[1].Width = new Unit(30, UnitType.Percentage);
            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
            {
                e.Item.Cells[2].Width = new Unit(26, UnitType.Percentage);
                e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}