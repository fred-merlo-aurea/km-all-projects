using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using System.Web.UI;
using System.Linq;

namespace ecn.communicator.main.content
{
    public partial class ruleList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "rules list";
            Master.Heading = "Content/Messages > Dynamic Tags > Rules List";
            //Master.Heading = "Manage Content & Messages";
            //Master.HelpContent = "<b>Editing Content:</b><br/><div id='par1'><ul><li>If the Content you created contains a link, you may want to create a name for the link, so that it is more easily referenced in the reporting section. You can do this by clicking on the link/alias icon. For example if your link was www.knowledgemarketing.com, you might name the link “Homepage.”</li><li>To preview your Content in HTML, click on the <em>HTML</em> icon and your Content will appear in a browser page.</li><li>To preview your Content in Text format, click on the <em>Text</em> Icon.</li><li>To edit your content, click on the <em>pencil</em> and you will have full access to make any changes.</li></ul><b>Deleting Content:</b><br/><ul><li>To delete your Content, click on the <em>red X</em>.<br/><em class='note'>NOTE:  While Editor defaults to an HTML format to create content, you can also create content using straight Source code.  To enter source code, click the <em>Source checkbox</em> in the upper right hand corner of the editor.  This will refresh the editor screen and you will be able to enter your source code directly or copy and paste existing code into the editor.  When finished, remember to unclick <em>Source checkbox</em> to view your content and save your code.</li></ul></div>";
            Master.HelpTitle = "Content Manager";
            if (!IsPostBack)
            {
                loadRules();
            }
        }

        private void loadRules()
        {
            List<ECN_Framework_Entities.Communicator.Rule> RuleList = ECN_Framework_BusinessLayer.Communicator.Rule.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser, false);
            var result = (from src in RuleList
                          orderby src.RuleName
                          select src).ToList();
            gvRule.DataSource = result;
            gvRule.DataBind();
        }

        protected void gvRule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string RuleID = e.CommandArgument.ToString();
            if (e.CommandName == "RuleDelete")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.Rule.Delete(Convert.ToInt32(RuleID), Master.UserSession.CurrentUser);
                    loadRules();
                }
                catch (ECNException ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errormsg", "alert('"+ ex.ErrorList[0].ErrorMessage +"');", true);
    
                }              
            }
        }

        protected void btnAddRule_Click(object sender, EventArgs e)
        {
            Response.Redirect("RuleEdit.aspx");
        }
    }
}