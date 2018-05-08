using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.content
{
    public partial class dynamicTagList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "dynamic tag list";
            Master.Heading = "Content/Messages > Dynamic Tags";
            //Master.Heading = "Manage Content & Messages";
            //Master.HelpContent = "<b>Editing Content:</b><br/><div id='par1'><ul><li>If the Content you created contains a link, you may want to create a name for the link, so that it is more easily referenced in the reporting section. You can do this by clicking on the link/alias icon. For example if your link was www.knowledgemarketing.com, you might name the link “Homepage.”</li><li>To preview your Content in HTML, click on the <em>HTML</em> icon and your Content will appear in a browser page.</li><li>To preview your Content in Text format, click on the <em>Text</em> Icon.</li><li>To edit your content, click on the <em>pencil</em> and you will have full access to make any changes.</li></ul><b>Deleting Content:</b><br/><ul><li>To delete your Content, click on the <em>red X</em>.<br/><em class='note'>NOTE:  While Editor defaults to an HTML format to create content, you can also create content using straight Source code.  To enter source code, click the <em>Source checkbox</em> in the upper right hand corner of the editor.  This will refresh the editor screen and you will be able to enter your source code directly or copy and paste existing code into the editor.  When finished, remember to unclick <em>Source checkbox</em> to view your content and save your code.</li></ul></div>";
            Master.HelpTitle = "Content Manager";

            if (!IsPostBack)
            {
                KMPlatform.Entity.User currentUser = Master.UserSession.CurrentUser;
                if (KMPlatform.BusinessLogic.Client.HasServiceFeature(Master.UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicTag))
                {
                    if (KM.Platform.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicTag, KMPlatform.Enums.Access.View))
                    {
                        if(KM.Platform.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicTag, KMPlatform.Enums.Access.Edit))
                        {
                            gvDynamicTag.Columns[3].Visible = true;
                            btnAddDynamicTag.Visible = true;
                            btnManageRules.Visible = true;
                        }
                        else
                        {
                            gvDynamicTag.Columns[3].Visible = false;
                            btnAddDynamicTag.Visible = false;
                            btnManageRules.Visible = false;
                        }
                        if (KM.Platform.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicTag, KMPlatform.Enums.Access.Delete))
                        {
                            gvDynamicTag.Columns[4].Visible = true;
                        }
                        else
                            gvDynamicTag.Columns[4].Visible = false;

                        loadDynamicTags();
                    }
                    else
                    {
                        throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                    }
                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
                }
            }
        }

        private void loadDynamicTags()
        {
            List<ECN_Framework_Entities.Communicator.DynamicTag> DynamicTagList = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser, false);
            var result = (from src in DynamicTagList
                          orderby src.Tag
                          select src).ToList();
            gvDynamicTag.DataSource = result;
            gvDynamicTag.DataBind();
        }

        protected void gvDynamicTag_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string DynamicTagID = e.CommandArgument.ToString();
            if (e.CommandName == "DynamicTagDelete")
            {   
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.DynamicTag.Delete(Convert.ToInt32(DynamicTagID), Master.UserSession.CurrentUser);
                    loadDynamicTags();
                }
                catch (ECNException ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errormsg", "alert('"+ ex.ErrorList[0].ErrorMessage +"');", true);    
                }     
            }
        }

        protected void btnAddDynamicTag_Click(object sender, EventArgs e)
        {
            Response.Redirect("DynamicTagEdit.aspx");
        }
        
        protected void btnManageRules_Click(object sender, EventArgs e)
        {
            Response.Redirect("rulelist.aspx");
        }
    }
}