using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Functions;
using HtmlAgilityPack;
using KM.Common.Functions;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ecn.communicator.main.ECNWizard.Content
{
    public partial class contentEditor : System.Web.UI.UserControl
    {
        private bool createAsNew
        {
            get { return Convert.ToBoolean(ViewState["createAsNew"]); }
            set { ViewState["createAsNew"] = value; }
        }
        private bool IsValidationRequried
        {
            get { return Convert.ToBoolean(ViewState["IsValidationRequried"]); }
            set { ViewState["IsValidationRequried"] = value; }
        }
       
        public int selectedContentID
        {
            get
            {
                if (Request.QueryString["ContentID"] != null)
                    return Convert.ToInt32(Request.QueryString["ContentID"]);
                else if (ViewState["selectedContentID"] != null)
                    return (int)ViewState["selectedContentID"];
                else
                    return 0;
            }
            set
            {
                ViewState["selectedContentID"] = value;
            }
        }

        private int getContentID()
        {
            if (Request.QueryString["ContentID"] != null)
                return Convert.ToInt32(Request.QueryString["ContentID"].ToString());
            else if (ViewState["selectedContentID"] != null)
                return (int)ViewState["selectedContentID"];
            else
                return 0;
        }

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
            int contentID = getContentID();
            int customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            int userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;


            FCKeditor1.FullPage = false;
            FCKeditor2.FullPage = false;
            if (!Page.IsPostBack)
            {
                LoadFoldersDR(customerID);
                LoadUsersDR(userID, customerID);
                FCKeditor1.Text = "";
                FCKeditor2.Text = "";

                if (contentID > 0)
                {
                    loadContent(contentID, false);
                    setShowPane();
                }
                else
                {
                    //presetdrpUserID();
                    setShowPane();
                }
            }
            else
            {
                setShowPane();
            }
       
        }
       
        #region Form Prep
        private void presetdrpUserID()
        {
            drpUserID.ClearSelection();
            drpUserID.Items.FindByValue(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID.ToString()).Selected = true;
        }

        public void setShowPane()
        {
            switch (ddContentTypeCode.SelectedItem.Value)
            {
                case "html":
                    panelContentSource.Visible = true;
                    panelContentText.Visible = true;
                    panelContentURL.Visible = false;
                    panelContentFilePointer.Visible = false;
                    GetTextButton.Visible = true;
                    break;
                case "text":
                    panelContentSource.Visible = false;
                    panelContentText.Visible = true;
                    panelContentURL.Visible = false;
                    panelContentFilePointer.Visible = false;
                    GetTextButton.Visible = false;
                    break;
                case "file":
                    panelContentSource.Visible = false;
                    panelContentText.Visible = false;
                    panelContentURL.Visible = false;
                    panelContentFilePointer.Visible = true;
                    break;
                case "feed":
                    panelContentSource.Visible = false;
                    panelContentText.Visible = false;
                    panelContentURL.Visible = true;
                    panelContentFilePointer.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void checkDuplicateContent()
        {
            CreateAsNewTopButton.Visible = true;
        }

        public void LoadFoldersDR(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList =
            ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in folderList
                          orderby src.FolderName
                          select src).ToList();
            folderID.DataSource = result;
            folderID.DataBind();
            folderID.Items.Insert(0, "root");
            folderID.Items.FindByValue("root").Value = "0";
        }

        public void LoadUsersDR(int currentUserID, int currentCustomerID)
        {
            List<KMPlatform.Entity.User> userList =
            KMPlatform.BusinessLogic.User.GetByCustomerID(currentCustomerID);
            var result = (from src in userList
                          orderby src.UserName
                          select src).ToList();

            //filter out inactive users 
            List<KMPlatform.Entity.User> filteredResult = new List<KMPlatform.Entity.User>();
            foreach (var user in result)
            {
                if (user.IsActive)
                {
                    filteredResult.Add(user);
                }
            }
            if (filteredResult.Count(x => x.UserID == currentUserID) == 0)
            {
                filteredResult.Add(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }


            drpUserID.DataSource = filteredResult.OrderBy(x => x.UserName);
            drpUserID.DataBind();

            drpUserID.SelectedValue = currentUserID.ToString();
        }
        #endregion

        #region Data Load
        public void loadContent(int contentID, bool createAsNew)
        {
            try
            {
                ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);

                string htmlSrc = content.ContentSource;
                string mobileSrc = content.ContentMobile;
                htmlSrc = RegexUtilities.GetCleanUrlContent(htmlSrc);
                mobileSrc = mobileSrc.Replace("%23", "#");

                if (createAsNew)
                {
                    ContentTitle.Text = "";
                    drpUserID.ClearSelection();
                    drpUserID.Items.FindByValue(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID.ToString()).Selected = true;
                    htmlSrc = ClearContentOfIDs(htmlSrc);
                    mobileSrc = ClearContentOfIDs(mobileSrc);
                    LockedFlag.Checked = false;
                }
                else
                {
                    ContentTitle.Text = content.ContentTitle;
                    if (content.LockedFlag.ToString() == "Y")
                    {
                        LockedFlag.Checked = true;
                    }
                    try
                    {
                        drpUserID.ClearSelection();
                        drpUserID.Items.FindByValue(content.UpdatedUserID.HasValue ? content.UpdatedUserID.ToString() : content.CreatedUserID.ToString()).Selected = true;
                    }
                    catch
                    {
                        drpUserID.Items.Insert(0, new ListItem("[CORPORATE]", "0"));
                    }

                }
                rblContentType.ClearSelection();
                rblContentType.SelectedValue = content.UseWYSIWYGeditor.Value.ToString();
                rblContentType.Enabled = false;
                if (content.UseWYSIWYGeditor.Value)
                {
                    FCKeditor1.Visible = true;
                    FCKeditor2.Visible = true;
                    pnlTxtMobile.Visible = false;
                    pnlTxtSource.Visible = false;
                    //radEditor1.Visible = true;
                    //radEditor2.Visible = true;
                    //radEditor1.Text = htmlSrc;
                    //radEditor2.Text = mobileSrc;
                    FCKeditor1.Text = htmlSrc;
                    FCKeditor2.Text = mobileSrc;
                }
                else
                {
                    FCKeditor1.Visible = false;
                    FCKeditor2.Visible = false;
                    pnlTxtMobile.Visible = true;
                    pnlTxtSource.Visible = true;
                    txtEditorSource.Text = htmlSrc;
                    txtEditorMobile.Text = mobileSrc;

                }
                ddContentTypeCode.ClearSelection();
                ddContentTypeCode.Items.FindByValue(content.ContentTypeCode.ToLower()).Selected = true;
                folderID.ClearSelection();
                folderID.Items.FindByValue(content.FolderID.ToString()).Selected = true;                
                string cleanContent = RegexUtilities.GetCleanUrlContent(content.ContentText.ToString());
                ContentText.Text = cleanContent;
                ContentURL.Text = content.ContentURL.ToString();
                ContentFilePointer.Text = content.ContentFilePointer.ToString();
                checkDuplicateContent();

                
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        protected void btnConvertoInlineCSS_Click(object sender, EventArgs e)
        {
            bool UseWYSIWYGeditor = Convert.ToBoolean(rblContentType.SelectedValue);
            System.Text.StringBuilder csource = new StringBuilder();
            if (UseWYSIWYGeditor)
            {
                if (FCKeditor1.Text.ToString().Length > 0)
                {
                    csource.Append(ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor1.Text.ToString()));
                }
                else
                {
                    csource.Append(ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentText.Text));
                }
                this.FCKeditor1.Text = ECN_Framework_Common.Functions.StringFunctions.ConvertToInlineCSS(csource);
            }
            else
            {
                if (txtEditorSource.Text.ToString().Length > 0)
                {
                    csource.Append(ECN_Framework_Common.Functions.StringFunctions.CleanString(txtEditorSource.Text.ToString()));
                }
                else
                {
                    csource.Append(ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentText.Text));
                }
                this.txtEditorSource.Text = ECN_Framework_Common.Functions.StringFunctions.ConvertToInlineCSS(csource);
            }
        }

        public void GetTextFromHTML(object sender, System.EventArgs e)
        {
            bool UseWYSIWYGeditor = Convert.ToBoolean(rblContentType.SelectedValue);
            string htmlText = string.Empty;
            if (UseWYSIWYGeditor)
            {
                htmlText = FCKeditor1.Text;
            }
            else
            {
                htmlText = txtEditorSource.Text;
            }
            ContentText.Text = HtmlFunctions.StripTextFromHtml(htmlText);
        }

        public void Reset()
        {
            FCKeditor1.Text = "";
            //RadEditorMobile.Content = "";
            FCKeditor2.Text = "";
            ContentText.Text = "";
            ContentTitle.Text = "";
            LockedFlag.Checked = false;
            folderID.SelectedValue = "0";
            ContentURL.Text = "";

        }

        public int SaveContent()
        {
            try
            {
                
                bool UseWYSIWYGeditor = Convert.ToBoolean(rblContentType.SelectedValue);
                int contentID = createAsNew == false ? getContentID() : 0;
                string ctitle = ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentTitle.Text);
                string lockedFlagValue = LockedFlag.Checked ? "Y" : "N";
                string sharing = "N";
                string csource = string.Empty;
                string cmobile = string.Empty;
                if (UseWYSIWYGeditor)
                {
                    csource = ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor1.Text.ToString());
                    cmobile = ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor1.Text.ToString());
                    Regex tbodyReg = new Regex("<[/]?[\\s]*?tbody[\\s\\S]*?>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    csource = tbodyReg.Replace(csource, "");
                    cmobile = tbodyReg.Replace(cmobile, "");
                    
                }
                else
                {
                    csource = ECN_Framework_Common.Functions.StringFunctions.CleanString(txtEditorSource.Text.ToString());
                    cmobile = ECN_Framework_Common.Functions.StringFunctions.CleanString(txtEditorMobile.Text.ToString());
                }

                


                csource = CommonStringFunctions.ReplaceAnchor(csource);
                cmobile = CommonStringFunctions.ReplaceAnchor(cmobile);

                //Add code to go through html and assign ECN_ID to each individual link
                //csource = CreateUniqueLinkIDs(csource);
                //cmobile = CreateUniqueLinkIDs(cmobile);
                string ctext = ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentText.Text);
                ctext = CommonStringFunctions.ReplaceAnchor(ctext);

                if (ctext == string.Empty || ctext.Length == 0)
                {
                    if (UseWYSIWYGeditor)
                    {
                        var cleanText = StringFunctions.CleanString(FCKeditor1.Text.ToString());
                        ctext = HtmlFunctions.StripTextFromHtml(cleanText);
                    }
                    else
                    {
                        var cleanText = StringFunctions.CleanString(txtEditorSource.Text.ToString());
                        ctext = HtmlFunctions.StripTextFromHtml(cleanText);
                    }
                }

                ECN_Framework_Entities.Communicator.Content content;
                if (contentID > 0)
                {
                    content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                    if(content.LockedFlag.ToUpper() == "Y" && ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID != (content.UpdatedUserID.HasValue ? content.UpdatedUserID.Value : content.CreatedUserID.Value))
                    {
                        List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
                        errorList.Add(new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Content, ECN_Framework_Common.Objects.Enums.Method.Save, "You do not have permission to edit this content"));
                        throw new ECN_Framework_Common.Objects.ECNException(errorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.WebSite);
                    }
                    content.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    if (drpUserID.SelectedValue.Equals("0"))
                        content.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    else
                        content.UpdatedUserID = Convert.ToInt32(drpUserID.SelectedValue);
                }
                else
                {
                    content = new ECN_Framework_Entities.Communicator.Content();
                    content.IsValidated = false;
                    content.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                }
                content.ContentTitle = ctitle;
                content.ContentTypeCode = ddContentTypeCode.SelectedValue;
                content.LockedFlag = lockedFlagValue;
                content.FolderID = Convert.ToInt32(folderID.SelectedValue);
                content.ContentSource = csource;
                content.ContentMobile = cmobile;
                content.ContentText = ctext;
                content.ContentURL = ContentURL.Text;
                content.ContentFilePointer = ContentFilePointer.Text;
                content.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                content.Sharing = sharing;
                content.UseWYSIWYGeditor = UseWYSIWYGeditor;
                content.IsValidated = content.IsValidated.HasValue ? content.IsValidated : false;
                if ((bool)content.IsValidated || (IsValidationRequried) )
                    content.IsValidated = ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(csource);
                else
                    content.IsValidated = false;
                ECN_Framework_BusinessLayer.Communicator.Content.Save(content, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);


                return content.ContentID;
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
                return 0;
            }

        }

        private string RemoveBreaks(string link)
        {
            link = link.Replace("\n", "");
            link = link.Replace("\r", "");
            link = link.Replace("\r\n", "");
            return link;
        }

        //private string CreateUniqueLinkIDs(string content)
        //{
        //    StringBuilder retContent = new StringBuilder();
        //    RegexOptions ro =  RegexOptions.Singleline;
            
        //    MatchCollection mc = Regex.Matches(content, "<a[^>]*>(.*?)</a>", ro);
        //    for (int i = 0; i < mc.Count; i++)
        //    {
        //        string originalA = "";
        //        string toReplaceLater = "";
        //        originalA = mc[i].Value;
        //        try
        //        {
        //            int indexOfHREF = 0;
        //            if (!originalA.Trim().StartsWith("ECN") && !originalA.Trim().Contains("%%") && !originalA.Trim().Contains("ecn_id="))
        //            {
        //                indexOfHREF = originalA.IndexOf("href=\"") + 6;
        //                string url = originalA.Substring(indexOfHREF, originalA.IndexOf("\"", indexOfHREF) - indexOfHREF);
        //                Uri RedirectUri = new Uri(url);
        //                toReplaceLater = originalA.Insert(0, i.ToString());
        //                int firstindexOF = 0;
        //                firstindexOF = content.IndexOf(originalA);

        //                content = content.Remove(firstindexOF, originalA.Length);
        //                content = content.Insert(firstindexOF, originalA.Insert(indexOfHREF - 7, " ecn_id=\"" + Guid.NewGuid().ToString() + "\""));

        //            }
        //        }
        //        catch(Exception ex) 
        //        {
 
        //        }
        //    }

        //    RegexOptions roArea = RegexOptions.Singleline;

        //    MatchCollection mcArea = Regex.Matches(content, "<area[^>]*>(.*?)</area>", roArea);
        //    for (int i = 0; i < mc.Count; i++)
        //    {
        //        string originalA = "";
        //        string toReplaceLater = "";
        //        originalA = mc[i].Value;
        //        try
        //        {
        //            int indexOfHREF = 0;
        //            if (!originalA.Trim().StartsWith("ECN") && !originalA.Trim().Contains("%%") && !originalA.Trim().Contains("ecn_id="))
        //            {
        //                indexOfHREF = originalA.IndexOf("href=\"") + 6;
        //                string url = originalA.Substring(indexOfHREF, originalA.IndexOf("\"", indexOfHREF) - indexOfHREF);
        //                Uri RedirectUri = new Uri(url);
        //                toReplaceLater = originalA.Insert(0, i.ToString());
        //                int firstindexOF = 0;
        //                firstindexOF = content.IndexOf(originalA);

        //                content = content.Remove(firstindexOF, originalA.Length);
        //                content = content.Insert(firstindexOF, originalA.Insert(indexOfHREF - 7, " ecn_id=\"" + Guid.NewGuid().ToString() + "\""));

        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //    }

        //    #region html agility pack
        //    ////This will also validate the links
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("div");
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("span");
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("table");
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("tr");
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("td");
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("a");
        //    //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("br");

        //    //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        //    //doc.OptionCheckSyntax = false;
        //    //doc.OptionAutoCloseOnEnd = false;
        //    //doc.OptionFixNestedTags = false;
        //    //byte[] bytes = Encoding.UTF8.GetBytes(content);
        //    //MemoryStream ms = new MemoryStream(bytes);
        //    //doc.Load((Stream)ms);
        //    //HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");

        //    //if (nodes != null)
        //    //{
        //    //    foreach (var node in nodes)
        //    //    {
        //    //        HtmlAttribute attECNID = node.Attributes["ECN_ID"];
        //    //        if (attECNID == null)
        //    //        {
        //    //            //if link validation fails, do not add the ECN_ID as an attribute
        //    //            try
        //    //            {
        //    //                if (!node.Attributes["href"].Value.ToString().Trim().StartsWith("ECN") && !node.Attributes["href"].Value.ToString().Trim().Contains("%%"))
        //    //                {
        //    //                    Uri RedirectUri = new Uri(node.Attributes["href"].Value.ToString());
        //    //                }
        //    //                node.Attributes.Add("ECN_ID", Guid.NewGuid().ToString());
        //    //            }
        //    //            catch { }
        //    //        }


        //    //    }
        //    //}

        //    //nodes = doc.DocumentNode.SelectNodes("//area[@href]");
        //    //if (nodes != null)
        //    //{
        //    //    foreach (var node in nodes)
        //    //    {
        //    //        HtmlAttribute attECNID = node.Attributes["ECN_ID"];
        //    //        if (attECNID == null)
        //    //        {
        //    //            //if link validation fails, do not add the ECN_ID as an attribute
        //    //            try
        //    //            {
        //    //                if (!node.Attributes["href"].Value.ToString().Trim().StartsWith("ECN") && !node.Attributes["href"].Value.ToString().Trim().Contains("%%"))
        //    //                {
        //    //                    Uri RedirectUri = new Uri(node.Attributes["href"].Value.ToString());
        //    //                }
        //    //                node.Attributes.Add("ECN_ID", Guid.NewGuid().ToString());
        //    //            }
        //    //            catch { }
        //    //        }
        //    //    }
        //    //}

        //    //MemoryStream stream = new MemoryStream();
        //    //doc.Save(stream);
        //    //stream.Seek(0, System.IO.SeekOrigin.Begin);
        //    //StreamReader sr = new StreamReader(stream);
       
        //    //content = sr.ReadToEnd();
        //    //content = doc.DocumentNode.OuterHtml;
        //    #endregion
        //    return content;
        //}

        public void CreateAsNewInitialize(object sender, System.EventArgs e)
        {
            int requestContentID = getContentID();
            loadContent(requestContentID, true);
            createAsNew = true;
        }
        public void ValidateContentInitialize(object sender, System.EventArgs e)
        {
           try {
                IsValidationRequried = true;
                bool UseWYSIWYGeditor = Convert.ToBoolean(rblContentType.SelectedValue);
                string csource = string.Empty;
                if (UseWYSIWYGeditor)
                {
                    Regex tbodyReg = new Regex("<[/]?[\\s]*?tbody[\\s\\S]*?>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    csource = ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor1.Text.ToString());
                    csource = tbodyReg.Replace(csource, "");
                }
                else
                {
                    csource = ECN_Framework_Common.Functions.StringFunctions.CleanString(txtEditorSource.Text.ToString());
                }
                if(ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(CommonStringFunctions.ReplaceAnchor(csource)))
                {
                    upContentExplorer.Update();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Redit2", "toastrContentValidated();", true);
                    int contentID = createAsNew == false ? getContentID() : 0;
                    if (contentID > 0)
                        SaveContent();

                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
}
        protected void rblContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblContentType.SelectedValue.Equals("True"))
            {
                FCKeditor1.Visible = true;
                FCKeditor2.Visible = true;
                pnlTxtMobile.Visible = false;
                pnlTxtSource.Visible = false;
            }
            else
            {
                FCKeditor1.Visible = false;
                FCKeditor2.Visible = false;
                pnlTxtMobile.Visible = true;
                pnlTxtSource.Visible = true;
            }
        }

        protected void btnTxtMobilePreview_Click(object sender, EventArgs e)
        {
            if (!txtEditorMobile.Text.Equals(string.Empty))
            {
                lblPreview.Text = txtEditorMobile.Text;
                this.modalPopupPreview.Show();
            }
        }

        protected void btnTxtSourcePreview_Click(object sender, EventArgs e)
        {
            if (!txtEditorSource.Text.Equals(string.Empty))
            {

                lblPreview.Text = RegexUtilities.GetCleanUrlContent(txtEditorSource.Text);
                this.modalPopupPreview.Show();
            }
        }

        protected void btnClosePreview_Hide(object sender, EventArgs e)
        {
            this.modalPopupPreview.Hide();
        }

        private string ClearContentOfIDs(string originalHTML)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(originalHTML);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    HtmlAttribute attECNID = node.Attributes["ECN_ID"];
                    if (attECNID == null)
                    {
                        node.Attributes.Remove("ECN_ID");

                    }


                }
            }

            nodes = doc.DocumentNode.SelectNodes("//area[@href]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    HtmlAttribute attECNID = node.Attributes["ECN_ID"];
                    if (attECNID == null)
                    {
                        node.Attributes.Remove("ECN_ID");

                    }
                }
            }

            return doc.DocumentNode.OuterHtml;

        }
    }

    public class UniqueLinkClass
    {
        public string originalA { get; set; }
        public string modifiedA { get; set; }
        public string toReplaceLater { get; set; }
    }
}