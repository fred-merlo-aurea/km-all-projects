using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace ecn.publisher.main.Edition
{
    public partial class SelectTemplate : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        private int EditionID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["EditionID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        string EditionName = string.Empty;
        string EditionURL = string.Empty;
        string PublicationType = string.Empty;
        string CustomerID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION;
            Master.SubMenu = "Edition List";
            Master.Heading = "Select Digital Edition Email Template";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            phError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                if (EditionID > 0)
                {
                    ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(EditionID, Master.UserSession.CurrentUser);

                    ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, Master.UserSession.CurrentUser);

                    string subdomain = pb.PublicationCode;

                    EditionURL = (subdomain != string.Empty ? "http://" + subdomain.ToLower() + ".ecndigitaledition.com/magazine.aspx?eid=" + EditionID + "&e=%%emailid%%&b=%%blastid%%" : "http://www.ecndigitaledition.com/magazine.aspx?eid=" + EditionID + "&e=%%emailid%%&b=%%blastid%%");

                    CustomerID = ed.CustomerID.ToString();
                    EditionName = ed.EditionName;
                    PublicationType = pb.PublicationType;

                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath(ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + "/templates/DEEmail_Templates.xml"));

                    grdDETemplate.DataSource = ds;
                    grdDETemplate.DataBind();
                }
            }
        }

        protected void grdDETemplate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTemplate = (Label)e.Row.FindControl("lblTemplate");
                string strHTML = lblTemplate.Text;
                lblTemplate.Text = ReplaceCodeSnippet(strHTML);

            }
        }

        private string ReplaceCodeSnippet(string s)
        {
            string ImgPath = ConfigurationManager.AppSettings["ImagePath"] + "/ecn.images/customers/" + CustomerID + "/publisher/" + EditionID.ToString() + '/';

            s = s.Replace("%%EditionName%%", EditionName);
            s = s.Replace("%%cover%%", ImgPath + "150/1.png");
            s = s.Replace("%%DigitalEditionlink%%", EditionURL);
            s = s.Replace("%%publicationtype%%", PublicationType);

            return s.ToString();
        }

        protected void btnSelect_Command(object sender, CommandEventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();                       
                ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(EditionID, Master.UserSession.CurrentUser);
                ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, Master.UserSession.CurrentUser);
                string subdomain = pb.PublicationCode;
                EditionURL = (subdomain != string.Empty ? "http://" + subdomain.ToLower() + ".ecndigitaledition.com/magazine.aspx?eid=" + EditionID + "&e=%%emailid%%&b=%%blastid%%" : "http://www.ecndigitaledition.com/magazine.aspx?eid=" + EditionID + "&e=%%emailid%%&b=%%blastid%%");

                CustomerID = ed.CustomerID.ToString();
                EditionName = ed.EditionName;
                PublicationType = pb.PublicationType;

                ECN_Framework_Entities.Communicator.Template Template = ECN_Framework_BusinessLayer.Communicator.Template.GetByNumberOfSlots(1, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                ECN_Framework_Entities.Communicator.Layout layout= new ECN_Framework_Entities.Communicator.Layout();
                layout.LayoutName=ed.EditionName + " - DE Campaign "+DateTime.Now.ToString();
                layout.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                layout.FolderID = 0;
                layout.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                layout.TemplateID = Template.TemplateID;
                layout.ContentSlot1 = CreateContent(getHTMLcontentFromXML(e.CommandArgument.ToString()), getTEXTcontentFromXML(e.CommandArgument.ToString()));
                ECN_Framework_BusinessLayer.Communicator.Layout.Save(layout, Master.UserSession.CurrentUser);

                ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignName("Marketing Campaign",Master.UserSession.CurrentUser, false);
                if (c == null || c.CampaignID <= 0)
                {
                    c = new ECN_Framework_Entities.Communicator.Campaign();
                    c.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                    c.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    c.CampaignName = "Marketing Campaign";
                    ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, Master.UserSession.CurrentUser);
                }
                ci.CampaignID = c.CampaignID;
                ci.CampaignItemName = ed.EditionName + " - DE Campaign "+DateTime.Now.ToString();
                ci.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                ci.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                ci.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                ci.CampaignItemNameOriginal = ed.EditionName + " - DE Campaign "+DateTime.Now.ToString();
                ci.IsHidden = true;
                ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
                ci.CompletedStep = 1;
                ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
               if(ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ReportingBlastFields))
                   ci.BlastField1=ed.EditionName + " - DE Campaign";
               ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, Master.UserSession.CurrentUser);
               Response.Redirect("/ecn.communicator/main/ecnwizard/wizardSetup.aspx?campaignItemid=" + ci.CampaignItemID + "&CampaignItemType=Regular&PrePopLayoutID=" + layout.LayoutID, true);

               
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

         }

        private string getHTMLcontentFromXML(string ID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath(ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + "/templates/DEEmail_Templates.xml"));
                return doc.SelectSingleNode("//Template[ID='" + ID + "']/Html").InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string getTEXTcontentFromXML(string ID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath(ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + "/templates/DEEmail_Templates.xml"));
                return doc.SelectSingleNode("//Template[ID='" + ID + "']/Text").InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }

        public int CreateContent(string strHTML, string strTEXT)
        {
            int contentID = 0;
            string csource = ReplaceAnchor(ECN_Framework_Common.Functions.StringFunctions.CleanString(ReplaceCodeSnippet(strHTML)));
            string ctext = ReplaceAnchor(ECN_Framework_Common.Functions.StringFunctions.CleanString(ReplaceCodeSnippet(strTEXT)));

            string cTitle = EditionName + " - Blast Content "+DateTime.Now ;

            ECN_Framework_Entities.Communicator.Content content = new ECN_Framework_Entities.Communicator.Content();
            content.ContentTitle = cTitle;
            content.ContentTypeCode = "HTML";
            content.LockedFlag = "N";
            content.CreatedUserID = Master.UserSession.CurrentUser.UserID;
            content.FolderID = 0;
            content.ContentSource = csource;
            content.ContentMobile = csource;
            content.ContentText = ctext;
            content.ContentURL = "";
            content.ContentFilePointer = "";
            content.CustomerID = Master.UserSession.CurrentUser.CustomerID;
            content.Sharing = "N";
            contentID = ECN_Framework_BusinessLayer.Communicator.Content.Save(content, Master.UserSession.CurrentUser);
            return contentID;
        }

        private string ReplaceAnchor(string str)
        {
            Regex regExp_href_tags = new Regex("\\w+:\\/\\/[\\w@][\\w.:@]+\\/?[\\w\\.?=%&=\\-@/$#,]*");
            MatchCollection link_Collection = regExp_href_tags.Matches(str);
            foreach (Match m in link_Collection)
            {
                foreach (Group g in m.Groups)
                    if (g.Value.Length > 0 && g.Value.IndexOf("#") > 0)
                        str = str.Replace(g.Value, g.Value.Replace("#", "%23"));
            }
            return str;
        }
    }
}
