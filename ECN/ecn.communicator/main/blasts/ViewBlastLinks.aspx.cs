using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.blasts
{
    public partial class ViewBlastLinks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.SubMenu = "view blast links";
            Master.Heading = "Blasts > View Blast Links";
            //Master.Heading = "Manage Content & Messages";
            //Master.HelpContent = "<b>Editing Content:</b><br/><div id='par1'><ul><li>If the Content you created contains a link, you may want to create a name for the link, so that it is more easily referenced in the reporting section. You can do this by clicking on the link/alias icon. For example if your link was www.knowledgemarketing.com, you might name the link “Homepage.”</li><li>To preview your Content in HTML, click on the <em>HTML</em> icon and your Content will appear in a browser page.</li><li>To preview your Content in Text format, click on the <em>Text</em> Icon.</li><li>To edit your content, click on the <em>pencil</em> and you will have full access to make any changes.</li></ul><b>Deleting Content:</b><br/><ul><li>To delete your Content, click on the <em>red X</em>.<br/><em class='note'>NOTE:  While Editor defaults to an HTML format to create content, you can also create content using straight Source code.  To enter source code, click the <em>Source checkbox</em> in the upper right hand corner of the editor.  This will refresh the editor screen and you will be able to enter your source code directly or copy and paste existing code into the editor.  When finished, remember to unclick <em>Source checkbox</em> to view your content and save your code.</li></ul></div>";
            Master.HelpTitle = "View Blast Links";
            phError.Visible = false;


        }

        protected void rblBlastLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rblBlastLinks.SelectedValue.Equals("get"))
            {
                pnlGetBlastLinks.Visible = true;
                pnlLookupBlastLinks.Visible = false;
                pnlLookupResults.Visible = false;
            }
            else if (rblBlastLinks.SelectedValue.Equals("lookup"))
            {
                pnlGetBlastLinks.Visible = false;
                pnlLookupBlastLinks.Visible = true;
                pnlLookupResults.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if(rblBlastLinks.SelectedValue.Equals("get"))
            {
                if(!string.IsNullOrEmpty(txtGetBlastID.Text.Trim()))
                {
                    List<ECN_Framework_Entities.Communicator.BlastLink> lstLinks = ECN_Framework_BusinessLayer.Communicator.BlastLink.GetByBlastID(Convert.ToInt32(txtGetBlastID.Text.Trim()));
                    gvGetBlastLinks.Visible = true;
                    gvGetBlastLinks.DataSource = lstLinks;
                    gvGetBlastLinks.DataBind();
                }
                else
                {
                    throwECNException("Please enter a blast id to search for");
                    return;
                }
            }
            else if(rblBlastLinks.SelectedValue.Equals("lookup"))
            {
                try
                {
                    string toUse = txtLookupBlastLink.Text;
                    if(txtLookupBlastLink.Text.Contains("http://"))
                    {
                        toUse = toUse.Substring(toUse.IndexOf("Clicks/") + 7);
                    }
                    KM.Common.Entity.Encryption ecLink = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                    string unencrypted = KM.Common.Encryption.Base64Decrypt(System.Web.HttpUtility.UrlDecode(toUse), ecLink);

                    string[] parse = unencrypted.Split('&');
                    string b = "0";
                    string eid = "0";
                    string lid = "0";
                    string ulid = "0";
                    foreach (string s in parse)
                    {
                        string[] kvp = s.Split('=');
                        switch (kvp[0])
                        {
                            case "b":
                                b = kvp[1];
                                break;
                            case "e":
                                eid = kvp[1];
                                break;
                            case "lid":
                                lid = kvp[1];
                                break;
                            case "ulid":
                                ulid = kvp[1];
                                break;
                        }
                    }

                    lblBlastIDResult.Text = b;
                    lblUniqueLinkIDResult.Text = ulid;
                    lblEmailIDResult.Text = eid;
                    lblLinkIDResult.Text = lid;
                    lblOLinkResult.Text = txtLookupBlastLink.Text;

                    ECN_Framework_Entities.Communicator.BlastLink bl = ECN_Framework_BusinessLayer.Communicator.BlastLink.GetByBlastLinkID(Convert.ToInt32(b), Convert.ToInt32(lid));
                    lblELinkResult.Text = bl.LinkURL;
                    pnlLookupResults.Visible = true;
                }
                catch(Exception ex) 
                {
                    throwECNException(ex.Message);
                }
            }
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

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.LandingPage, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }
    }
}