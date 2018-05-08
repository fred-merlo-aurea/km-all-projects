using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.SessionState;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace ecn.communicator.contentmanager
{

    public partial class contentPreview : System.Web.UI.Page
    {
        #region Properties
        public string type
        {
            get
            {
                if (Session["CP_Type"] != null)
                    return Session["CP_Type"].ToString();
                else
                    return "html";
            }
            set
            {
                Session["CP_Type"] = value;
            }
        }

        public string requestFrom
        {
            get
            {
                if (Session["CP_RequestFrom"] != null)
                    return Session["CP_RequestFrom"].ToString();
                else
                    return "";
            }
            set
            {
                Session["CP_RequestFrom"] = value;
            }
        }

        public string contentSel
        {
            get
            {
                if (Session["CP_ContentSel"] != null)
                    return Session["CP_ContentSel"].ToString();
                else
                    return "ContentSource";
            }
            set
            {
                Session["CP_ContentSel"] = value;
            }
        }

        public string body
        {
            get
            {
                if (Session["CP_Body"] != null)
                    return Session["CP_Body"].ToString();
                else
                    return "";
            }
            set
            {
                Session["CP_Body"] = value;
            }
        }

        public int ContentID
        {
            get
            {
                if (Session["CP_ContentID"] != null)
                    return (int)Session["CP_ContentID"];
                else
                    return 0;
            }
            set
            {
                Session["CP_ContentID"] = value;
            }
        }

        public MasterContent MainMasterContent
        {
            get
            {
                if (Session["MainMasterContent"] != null)
                    return (MasterContent)Session["MainMasterContent"];
                else
                    return new MasterContent();
            }
            set
            {
                Session["MainMasterContent"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Properties
                    if (Request.QueryString["contentID"] != null)
                    {
                        int conID = 0;
                        int.TryParse(Request.QueryString["contentID"].ToString(), out conID);
                        ContentID = conID;
                    }
                    else
                    {
                        ContentID = 0;
                    }

                    if (Request.QueryString["type"] != null)
                    {
                        type = Request.QueryString["type"].ToString();
                    }
                    else
                    {
                        type = "html";
                        contentSel = "ContentSource";
                        LabelPreview.Visible = true;
                        TextPreview.Visible = false;
                    }

                    if (Request.QueryString["requestFrom"] != null)
                    {
                        requestFrom = Request.QueryString["requestFrom"].ToString();
                    }
                    else
                    {
                        requestFrom = "";
                    }

                    if (type.Equals("text"))
                    {
                        contentSel = "ContentText";
                        LabelPreview.Visible = false;
                        TextPreview.Visible = true;
                    }
                    else
                    {
                        TextPreview.Visible = false;
                        contentSel = "ContentSource";
                    }
                    #endregion

                    ShowContent();
                }
            }
            catch (Exception ex)
            {
                System.Text.StringBuilder sbLog = new System.Text.StringBuilder();
                sbLog.AppendLine("**********************");
                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
                sbLog.AppendLine("Method: ImportUnsubscribe - Subscription loop");
                sbLog.AppendLine("-- Message --");
                if (ex.Message != null)
                    sbLog.AppendLine(ex.Message);
                sbLog.AppendLine("-- InnerException --");
                if (ex.InnerException != null)
                    sbLog.AppendLine(ex.InnerException.ToString());
                sbLog.AppendLine("-- Stack Trace --");
                if (ex.StackTrace != null)
                    sbLog.AppendLine(ex.StackTrace);
                sbLog.AppendLine("**********************");

                TextPreview.Text = sbLog.ToString();
            }
        }

        private void ShowContent()
        {
            KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            bool getChildren = true;
            ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(Convert.ToInt32(ContentID), user, getChildren);

            if (content != null && getChildren)
            {
                //find and attach any dynamic tags
                System.Collections.Generic.List<string> toParse = new List<string>();
                toParse.Add(content.ContentSource); //Only the HTML version
                List<string> tagNameList = ECN_Framework_BusinessLayer.Communicator.Content.GetTags(toParse);
                List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                if (tagNameList.Count > 0)
                {                    
                    ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                    foreach (string tagName in tagNameList)
                    {
                        tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag(tagName, user, getChildren);
                        tagList.Add(tag);
                    }                    
                }
                content.DynamicTagList = tagList;
            }

            if (content.DynamicTagList == null || content.DynamicTagList.Count == 0)
            {
                pnlSideBar.Visible = false;

                if (contentSel.Equals("ContentText"))
                {
                    body = content.ContentText;
                }
                else if (contentSel.Equals("ContentSource"))
                {
                    body = BuildHtmlContent(content);//content.ContentSource;
                }

                if (type.Equals("html"))
                {
                    if (requestFrom.Equals("linkAlias"))
                    {
                        LabelPreview.Text = ShowLinksonMouseOver(body);
                    }
                    else
                    {
                        body = RegexUtilities.GetCleanUrlContent(body);
                        LabelPreview.Text = body;
                    }
                }
                else if (type.Equals("text"))
                {
                    
                    TextPreview.Text = body;
                }
            }
            else
            {
                try
                {
                    pnlSideBar.Visible = true;
                    rptSideBar.DataSource = content.DynamicTagList;
                    rptSideBar.DataMember = "Tag";
                    rptSideBar.DataBind();

                    body = BuildHtmlContent(content);
                    body = RegexUtilities.GetCleanUrlContent(body);
                    LabelPreview.Text = body;
                }
                catch (Exception ex)
                {
                    System.Text.StringBuilder sbLog = new System.Text.StringBuilder();
                    sbLog.AppendLine("**********************");
                    sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
                    sbLog.AppendLine("Method: ImportUnsubscribe - Subscription loop");
                    sbLog.AppendLine("-- Message --");
                    if (ex.Message != null)
                        sbLog.AppendLine(ex.Message);
                    sbLog.AppendLine("-- InnerException --");
                    if (ex.InnerException != null)
                        sbLog.AppendLine(ex.InnerException.ToString());
                    sbLog.AppendLine("-- Stack Trace --");
                    if (ex.StackTrace != null)
                        sbLog.AppendLine(ex.StackTrace);
                    sbLog.AppendLine("**********************");

                    TextPreview.Text = sbLog.ToString();
                }
            }
        }

        private string ShowLinksonMouseOver(string strText)
        {

            Regex r;
            Match m;
            string body = "<table><tr><td><font color='#FF0000' size='3' face='verdana'><b>Move your mouse over the links to view URL</b></font></td></tr></table><br>";
            body += strText.ToString();
            r = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            for (m = r.Match(body); m.Success; m = m.NextMatch())
            {
                if (m.Groups[1].Length > 1)
                {
                    body = StringFunctions.Replace(body, m.Groups[1].ToString(), m.Groups[1].ToString() + " \" onmouseover=\"javascript:alert('" + m.Groups[1] + "');");
                }
            }
            return body;
        }

        private void UpdateVisiblity(int _conID)
        {
            int padding = 36;

            foreach (MasterContent child in MainMasterContent.ChildList)
            {
                //<div id='xxx' style='display:visible|none;'>
                //none will be padded with 3 spaces so length can be the same
                if (child.SiblingContentIDs.Contains(_conID))
                {
                    int childIndexTABLE = body.IndexOf("<div id='" + child.ContentID.ToString());
                    int childLengthTABLE = padding + child.ContentID.ToString().Length;
                    string currentChildHtmlTABLE = body.Substring(childIndexTABLE, childLengthTABLE);
                    string newChildHtmlTABLE = "<div id='" + child.ContentID.ToString() + "' style='display:   none;'>";
                    body = body.Replace(currentChildHtmlTABLE, newChildHtmlTABLE);

                    //set this sibling html block to display:visible
                    int thisSibIndexTABLE = body.IndexOf("<div id='" + _conID.ToString());
                    int thisSibLengthTABLE = padding + _conID.ToString().Length;
                    string thisSibHtmlTABLE = body.Substring(thisSibIndexTABLE, thisSibLengthTABLE);
                    string thisSibNewHtmlTABLE = "<div id='" + _conID.ToString() + "' style='display:visible;'>";
                    body = body.Replace(thisSibHtmlTABLE, thisSibNewHtmlTABLE);

                    //set all other sibling display blocks to display:none
                    foreach (int sibContentID in child.SiblingContentIDs)
                    {
                        if (sibContentID != _conID)
                        {
                            //set this sibling html block to display:   none
                            int xSibIndexTABLE = body.IndexOf("<div id='" + sibContentID.ToString());
                            int xSibLengthTABLE = padding + sibContentID.ToString().Length;
                            string xSibHtmlTABLE = body.Substring(xSibIndexTABLE, xSibLengthTABLE);
                            string xSibNewHtmlTABLE = "<div id='" + sibContentID.ToString() + "' style='display:   none;'>";
                            body = body.Replace(xSibHtmlTABLE, xSibNewHtmlTABLE);
                        }
                    }
                }
                else if (child.ContentID == _conID)
                {
                    //the default value
                    //set child html block to display:none
                    int childIndexTR = body.IndexOf("<div id='" + child.ContentID.ToString());
                    int childLengthTR = padding + child.ContentID.ToString().Length;
                    string currentChildHtmlTR = body.Substring(childIndexTR, childLengthTR);
                    string newChildHtmlTR = "<div id='" + child.ContentID.ToString() + "' style='display:visible;'>";
                    body = body.Replace(currentChildHtmlTR, newChildHtmlTR);

                    //set all other sibling display blocks to display:none
                    foreach (int sibContentID in child.SiblingContentIDs)
                    {
                        //set this sibling html block to display:   none
                        int xSibIndexTR = body.IndexOf("<div id='" + sibContentID.ToString());
                        int xSibLengthTR = padding + sibContentID.ToString().Length;
                        string xSibHtmlTR = body.Substring(xSibIndexTR, xSibLengthTR);
                        string xSibNewHtmlTR = "<div id='" + sibContentID.ToString() + "' style='display:   none;'>";
                        body = body.Replace(xSibHtmlTR, xSibNewHtmlTR);
                    }
                }
            }
        }
        protected void ddlTagRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            //the contentID here will be that of MainContentMaster.Child.Sibling
            //need to loop thru MainContentMaster.Children then loop thru each childs siblings looking for the contentID

            System.Web.UI.WebControls.DropDownList ddlTagRule = (System.Web.UI.WebControls.DropDownList)sender;
            string[] conIDstring = ddlTagRule.SelectedValue.ToString().Split(':');
            int _conID = 0;
            int.TryParse(conIDstring[1], out _conID);

            ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(Convert.ToInt32(_conID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);

            if (contentSel.Equals("ContentText"))
            {
                body = content.ContentText;
            }
            else if (contentSel.Equals("ContentSource"))
            {
                UpdateVisiblity(_conID);
            }
            type = content.ContentTypeCode;
            if (type.Equals("html"))
            {
                if (requestFrom.Equals("linkAlias"))
                {
                    LabelPreview.Text = ShowLinksonMouseOver(body);
                }
                else
                {
                    LabelPreview.Text = RegexUtilities.GetCleanUrlContent(body);
                }
            }
            else if (type.Equals("text"))
            {
                TextPreview.Text = RegexUtilities.GetCleanUrlContent(body);
            }
        }

        /// <summary>
        /// not used now
        /// </summary>
        /// <param name="conID"></param>
        private void ShowDefaultContent(int conID)
        {
            ContentID = conID;

            ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(Convert.ToInt32(ContentID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);

            if (contentSel.Equals("ContentText"))
            {
                body = content.ContentText;
            }
            else if (contentSel.Equals("ContentSource"))
            {
                body = BuildHtmlContent(content); // content.ContentSource;
            }
            type = content.ContentTypeCode;
            if (type.Equals("html"))
            {
                if (requestFrom.Equals("linkAlias"))
                {
                    LabelPreview.Text = ShowLinksonMouseOver(body);
                }
                else
                {
                    LabelPreview.Text = RegexUtilities.GetCleanUrlContent(body);
                }
            }
            else if (type.Equals("text"))
            {
                TextPreview.Text = RegexUtilities.GetCleanUrlContent(body);
            }
        }
        protected void rptSideBar_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            ECN_Framework_Entities.Communicator.DynamicTag dt = (ECN_Framework_Entities.Communicator.DynamicTag)e.Item.DataItem;

            System.Web.UI.WebControls.Label lbTag = (System.Web.UI.WebControls.Label)e.Item.FindControl("lbTag");
            lbTag.Text = dt.Tag;
            System.Web.UI.WebControls.DropDownList ddlTagRule = (System.Web.UI.WebControls.DropDownList)e.Item.FindControl("ddlTagRule");
            int index = 1;
            foreach (ECN_Framework_Entities.Communicator.DynamicTagRule dtr in dt.DynamicTagRulesList)
            {
                ddlTagRule.Items.Add(new System.Web.UI.WebControls.ListItem(dtr.Rule.RuleName, index++ + ":" + dtr.ContentID.ToString()));
            }
            ddlTagRule.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Default", "0:" + dt.ContentID.ToString()));            
        }



        private string BuildHtmlContent(int contentID, bool isVisible, ref MasterContent child)
        {
            string visible = "visible";
            if (isVisible == false)
                visible = "   none";
            ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(Convert.ToInt32(contentID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
            child.Content = content;
            string html = "<div id='" + content.ContentID.ToString() + "' style='display:" + visible + ";'>" + content.ContentSource + "</div>";
                        
            ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed.Replace(ref html,
                            ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID,
                            false, null); // content is HTML, do NOT search for cached RSS info by BlastID

            if (content.DynamicTagList.Count > 0)
            {
                child.HasChildren = true;
                child.ChildContentIDs = new List<int>();
                child.ChildList = new List<MasterContent>();

                foreach (var dt in content.DynamicTagList)
                {
                    MasterContent subChild = new MasterContent();
                    subChild.IsMaster = false;
                    subChild.ContentID = dt.ContentID.Value;
                    subChild.IsVisible = true;
                    subChild.SiblingContentIDs = new List<int>();
                    subChild.SiblingContentList = new List<MasterContent>();
                    if (dt.DynamicTagRulesList.Count > 0)
                        subChild.HasSiblings = true;

                    System.Text.StringBuilder dtContent = new System.Text.StringBuilder();
                    dtContent.Append(BuildHtmlContent(dt.ContentID.Value, true, ref subChild));

                    //this loop is for the child's Siblings
                    foreach (var x in dt.DynamicTagRulesList)
                    {
                        MasterContent sib = new MasterContent();
                        sib.IsMaster = false;
                        sib.ContentID = x.ContentID.Value;
                        sib.IsVisible = false;
                        sib.SiblingContentIDs = new List<int>();
                        sib.SiblingContentList = new List<MasterContent>();
                        sib.ChildList = new List<MasterContent>();

                        dtContent.AppendLine(BuildHtmlContent(x.ContentID.Value, false, ref sib));
                        subChild.SiblingContentIDs.Add(sib.ContentID);
                        subChild.SiblingContentList.Add(sib);
                    }

                    child.ChildContentIDs.Add(subChild.ContentID);
                    child.ChildList.Add(subChild);
                    html = html.Replace("ECN.DynamicTag." + dt.Tag + ".ECN.DynamicTag", dtContent.ToString());
                }
            }
            return html;
        }

        private string BuildHtmlContent(ECN_Framework_Entities.Communicator.Content content)
        {
            MasterContent mc = new MasterContent();
            mc.IsMaster = true;
            mc.Content = content;
            mc.ContentID = content.ContentID;
            mc.IsVisible = true;
            mc.HasChildren = false;
            mc.HasSiblings = false;

            string html = content.ContentSource;

            //html = BuildHtmlContent_RssFeed(html);
            ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed.Replace(ref html,
                            ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID,
                            false, null); // content is HTML, do NOT search for cached RSS info by BlastID

            if (content.DynamicTagList.Count > 0)
            {
                mc.HasChildren = true;
                mc.ChildContentIDs = new List<int>();
                mc.ChildList = new List<MasterContent>();

                //this loop is for the Children of the main contentID
                foreach (var dt in content.DynamicTagList)
                {
                    MasterContent child = new MasterContent();
                    child.IsMaster = false;
                    child.ContentID = dt.ContentID.Value;
                    child.IsVisible = true;
                    child.SiblingContentIDs = new List<int>();
                    child.SiblingContentList = new List<MasterContent>();
                    if (dt.DynamicTagRulesList.Count > 0)
                        child.HasSiblings = true;

                    System.Text.StringBuilder dtContent = new System.Text.StringBuilder();
                    dtContent.Append(BuildHtmlContent(dt.ContentID.Value, true, ref child));

                    //this loop is for the child's Siblings
                    foreach (var x in dt.DynamicTagRulesList)
                    {
                        if (dt.ContentID.Value != x.ContentID.Value)
                        {
                            MasterContent sib = new MasterContent();
                            sib.IsMaster = false;
                            sib.ContentID = x.ContentID.Value;
                            sib.IsVisible = false;
                            sib.SiblingContentIDs = new List<int>();
                            sib.SiblingContentList = new List<MasterContent>();
                            sib.ChildList = new List<MasterContent>();

                            dtContent.AppendLine(BuildHtmlContent(x.ContentID.Value, false, ref sib));
                            child.SiblingContentIDs.Add(sib.ContentID);
                            child.SiblingContentList.Add(sib);
                        }
                    }

                    mc.ChildContentIDs.Add(child.ContentID);
                    mc.ChildList.Add(child);
                    html = html.Replace("ECN.DynamicTag." + dt.Tag + ".ECN.DynamicTag", dtContent.ToString());
                }
            }
            
            mc.HtmlBody = html;
            MainMasterContent = mc;
            return html;
        }

        #region rss feed handling

        Regex ecnRssFeedRegex = new Regex(@"ECN.RSSFEED\.(.*?)\.ECN.RSSFEED", RegexOptions.Singleline);
        MatchEvaluator ecnRssFeedMatchEvaluator = new MatchEvaluator(
            (Match match) =>
            {
                if (1 > match.Groups.Count || null == match.Groups[1] || String.IsNullOrEmpty(match.Groups[1].Value))
                {
                    return ""; // feed does not exist!
                }
                string feedName = match.Groups[1].Value;

                ECN_Framework_Entities.Communicator.RSSFeed rss = ECN_Framework_BusinessLayer.Communicator.RSSFeed.
                    GetByFeedName(feedName, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);

                if (rss == null)
                {
                    return "";
                }

                StringBuilder sbRSSFeed = new StringBuilder();
                StringBuilder sbRSSFeedText = new StringBuilder();
                XmlReader reader = XmlReader.Create(rss.URL);
                System.ServiceModel.Syndication.SyndicationFeed sf = System.ServiceModel.Syndication.SyndicationFeed.Load(reader);
                if (rss.StoriesToShow.HasValue)
                {
                    int index = 0;
                    foreach (System.ServiceModel.Syndication.SyndicationItem si in sf.Items.OrderByDescending(x => x.PublishDate))
                    {
                        if (index++ < rss.StoriesToShow.Value)
                        {
                            string summary = si.Summary != null ? si.Summary.Text : "";
                            sbRSSFeed.Append("<a href='" + si.Id + "'>" + si.Title.Text + "</a><br />");
                            sbRSSFeed.Append("<span>" + summary + "</span><br />");

                            sbRSSFeedText.Append("<" + si.Id + "\n");
                            sbRSSFeedText.Append(summary + "\n");
                        }
                        else
                            break;
                    }

                }
                else
                {
                    int index = 0;
                    foreach (System.ServiceModel.Syndication.SyndicationItem si in sf.Items.OrderByDescending(x => x.PublishDate))
                    {
                        if (index < 10)
                        {
                            string summary = si.Summary != null ? si.Summary.Text : "";
                            sbRSSFeed.Append("<a href='" + si.Id + "'>" + si.Title.Text + "</a><br />");
                            sbRSSFeed.Append("<span>" + summary + "</span><br />");
                            sbRSSFeedText.Append("<" + si.Id + "\n");
                            sbRSSFeedText.Append(summary + "\n");
                        }
                        else
                            break;
                    }
                }

                return sbRSSFeed.ToString();
            });

        string BuildHtmlContent_RssFeed(string html)
        {
            if (String.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            return ecnRssFeedRegex.Replace(html, ecnRssFeedMatchEvaluator);
        }
        #endregion RSS feed handling
    }

    public class MasterContent
    {
        public MasterContent() { }
        public int ContentID { get; set; }
        public bool IsMaster { get; set; } //will only be true for querystring contentID - wont have Sibling Objects
        public string HtmlBody { get; set; }
        public bool IsVisible { get; set; }
        public ECN_Framework_Entities.Communicator.Content Content { get; set; }
        public List<int> SiblingContentIDs { get; set; }
        public List<MasterContent> SiblingContentList { get; set; }
        public List<int> ChildContentIDs { get; set; }
        public List<MasterContent> ChildList { get; set; }
        public bool HasChildren { get; set; }
        public bool HasSiblings { get; set; }
    }
}
