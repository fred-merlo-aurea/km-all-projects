using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.main.content
{
    public partial class dynamicTagEdit : System.Web.UI.Page
    {

        private DataTable DynamicTag_DT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["DynamicTag_DT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["DynamicTag_DT"] = value;
            }
        }

        private int getDynamicTagID()
        {
            if (Request.QueryString["DynamicTagID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["DynamicTagID"]);
            }
            else
                return -1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "dynamic tag";
            //Master.Heading = "Content/Messages > Dynamic Tags > Rules List";
            //Master.Heading = "Manage Content & Messages";
            //Master.HelpContent = "<b>Editing Content:</b><br/><div id='par1'><ul><li>If the Content you created contains a link, you may want to create a name for the link, so that it is more easily referenced in the reporting section. You can do this by clicking on the link/alias icon. For example if your link was www.knowledgemarketing.com, you might name the link “Homepage.”</li><li>To preview your Content in HTML, click on the <em>HTML</em> icon and your Content will appear in a browser page.</li><li>To preview your Content in Text format, click on the <em>Text</em> Icon.</li><li>To edit your content, click on the <em>pencil</em> and you will have full access to make any changes.</li></ul><b>Deleting Content:</b><br/><ul><li>To delete your Content, click on the <em>red X</em>.<br/><em class='note'>NOTE:  While Editor defaults to an HTML format to create content, you can also create content using straight Source code.  To enter source code, click the <em>Source checkbox</em> in the upper right hand corner of the editor.  This will refresh the editor screen and you will be able to enter your source code directly or copy and paste existing code into the editor.  When finished, remember to unclick <em>Source checkbox</em> to view your content and save your code.</li></ul></div>";
            Master.HelpTitle = "Content Manager";

            phError.Visible = false;
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicTag, KMPlatform.Enums.Access.Edit))
                {
                    contentExplorer1.enableSelectMode();
                    if (getDynamicTagID() > 0)
                    {
                        loadData(getDynamicTagID());
                        //lblHeadingDynamicTag.Text = "Edit Dynamic Tag";
                        Master.Heading = "Content/Messages > Dynamic Tags > Edit Dynamic Tag";
                    }
                    else
                    {
                        Master.Heading = "Content/Messages > Dynamic Tags > Create Dynamic Tag";
                        loadGrid();
                    }
                    loadRules();
                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        private void loadRules()
        {
            drpRule.Items.Clear();
            List<ECN_Framework_Entities.Communicator.Rule> ruleList = ECN_Framework_BusinessLayer.Communicator.Rule.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser, false);
            drpRule.DataSource = ruleList;
            drpRule.DataBind();
            drpRule.Items.Insert(0, "-Select-");
        }

        private void loadData(int DynamicTagID)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByDynamicTagID(DynamicTagID, Master.UserSession.CurrentUser, true);
            if (DynamicTag.DynamicTagRulesList != null)
            { 
                ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(DynamicTag.ContentID.Value, Master.UserSession.CurrentUser, false);
                  
                txtTag.Text = DynamicTag.Tag;
                txtContentTag.Text = content.ContentTitle;
                hfTagContentID.Value = content.ContentID.ToString();
                DataTable dt = new DataTable();
                DataColumn DynamicTagRuleID = new DataColumn("DynamicTagRuleID", typeof(string));
                dt.Columns.Add(DynamicTagRuleID);

                DataColumn RuleID = new DataColumn("RuleID", typeof(string));
                dt.Columns.Add(RuleID);

                DataColumn RuleName = new DataColumn("RuleName", typeof(string));
                dt.Columns.Add(RuleName);

                DataColumn ContentID = new DataColumn("ContentID", typeof(string));
                dt.Columns.Add(ContentID);

                DataColumn ContentTitle = new DataColumn("ContentTitle", typeof(string));
                dt.Columns.Add(ContentTitle);

                DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
                dt.Columns.Add(IsDeleted);
                
                DataColumn Priority = new DataColumn("Priority", typeof(int));
                dt.Columns.Add(Priority);

                dt.AcceptChanges();
                foreach (ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRule in DynamicTag.DynamicTagRulesList)
                {
                    content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(DynamicTagRule.ContentID.Value, Master.UserSession.CurrentUser, false);
                    DataRow dr = dt.NewRow();
                    dr["DynamicTagRuleID"] = DynamicTagRule.DynamicTagRuleID.ToString();
                    dr["RuleName"] = DynamicTagRule.Rule.RuleName;
                    dr["RuleID"] = DynamicTagRule.Rule.RuleID;
                    dr["ContentID"] = content.ContentID;
                    dr["ContentTitle"] = content.ContentTitle;
                    dr["IsDeleted"] = DynamicTagRule.IsDeleted;
                    dr["Priority"] = DynamicTagRule.Priority;
                    dt.Rows.Add(dr);
                }
                DynamicTag_DT = dt;
                hfRuleCount.Value = DynamicTag.DynamicTagRulesList.Count.ToString();
            }
            loadGrid();
        }

        private void loadGrid()
        {
            if (DynamicTag_DT != null)
            {
                var result = (from src in DynamicTag_DT.AsEnumerable()
                              orderby src.Field<int>("Priority")
                              where src.Field<bool>("IsDeleted") == false
                              select new
                              {
                                  DynamicTagRuleID = src.Field<string>("DynamicTagRuleID"),
                                  RuleName = src.Field<string>("RuleName"),
                                  ContentTitle = src.Field<string>("ContentTitle"),
                                  ContentID = src.Field<string>("ContentID"),
                                  Priority = src.Field<int>("Priority"),
                                  RuleID = src.Field<string>("RuleID")
                              }).ToList();
                rolRules.DataSource = result;
                rolRules.DataBind();
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("ContentSelected"))
                {
                    int contentID = contentExplorer1.selectedContentID;
                    ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID, Master.UserSession.CurrentUser, false);            
                    if (hfContentExplorerFromTag.Value.Equals("True"))
                    {
                        hfTagContentID.Value = contentID.ToString();
                        txtContentTag.Text = content.ContentTitle;
                    }
                    else
                    {
                        hfRuleContentID.Value = contentID.ToString();
                        txtContentRule.Text = content.ContentTitle;
                    }
                    contentExplorer1.Reset();

                    this.modalPopupcontentExplorer.Hide();
                }
            }
            catch { }
            return true;
        }

        protected void btncontentExplorer_Click(object sender, EventArgs e)
        {
            modalPopupcontentExplorer.Hide();
        }

        protected void btnAddRule_Click(object sender, EventArgs e)
        {
            if (hfRuleID.Value.Equals("0") == false && hfRuleContentID.Value.Equals("0") == false)
            {
                hfRuleCount.Value = (Convert.ToInt32(hfRuleCount.Value) + 1).ToString();
                if (DynamicTag_DT == null)
                {
                    DataTable dt = new DataTable();
                    DataColumn DynamicTagRuleID = new DataColumn("DynamicTagRuleID", typeof(string));
                    dt.Columns.Add(DynamicTagRuleID);

                    DataColumn RuleID = new DataColumn("RuleID", typeof(string));
                    dt.Columns.Add(RuleID);

                    DataColumn RuleName = new DataColumn("RuleName", typeof(string));
                    dt.Columns.Add(RuleName);

                    DataColumn Priority = new DataColumn("Priority", typeof(int));
                    dt.Columns.Add(Priority);

                    DataColumn ContentID = new DataColumn("ContentID", typeof(string));
                    dt.Columns.Add(ContentID);

                    DataColumn ContentTitle = new DataColumn("ContentTitle", typeof(string));
                    dt.Columns.Add(ContentTitle);

                    DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
                    dt.Columns.Add(IsDeleted);

                    dt.AcceptChanges();
                    DynamicTag_DT = dt;
                }

                DataRow dr = DynamicTag_DT.NewRow();
                dr["DynamicTagRuleID"] = Guid.NewGuid().ToString();
                dr["RuleName"] = txtRule.Text;
                dr["RuleID"] = Convert.ToInt32(hfRuleID.Value);
                dr["ContentTitle"] = txtContentRule.Text;
                dr["ContentID"] = hfRuleContentID.Value;
                dr["IsDeleted"] = false;
                dr["Priority"] = Convert.ToInt32(hfRuleCount.Value);
                DynamicTag_DT.Rows.Add(dr);
                loadGrid();
                txtRule.Text = "-No Rule Selected-";
                hfRuleContentID.Value = "0";
                txtContentRule.Text = "-No Content Selected-";
            }
        }

        protected void btnExistingContentTag_Click(object sender, EventArgs e)
        {
            hfContentExplorerFromTag.Value = "True";
            this.modalPopupcontentExplorer.Show();
        }

        protected void btnExistingContentRule_Click(object sender, EventArgs e)
        {
            hfContentExplorerFromTag.Value = "False";
            this.modalPopupcontentExplorer.Show();
        }

        protected void btnTagSave_Click(object sender, EventArgs e)
        {
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;           
            try
            {
                ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = new ECN_Framework_Entities.Communicator.DynamicTag();
                if (getDynamicTagID() > 0)
                {
                    DynamicTag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByDynamicTagID(getDynamicTagID(), currentUser, false);
                    DynamicTag.UpdatedUserID = currentUser.UserID;
                }
                else
                {
                    DynamicTag.CreatedUserID = currentUser.UserID;
                }
                DynamicTag.CustomerID = currentUser.CustomerID;
                DynamicTag.Tag = txtTag.Text;
                DynamicTag.ContentID = Convert.ToInt32(hfTagContentID.Value);
                ECN_Framework_BusinessLayer.Communicator.DynamicTag.Save(DynamicTag, currentUser);
                if (DynamicTag_DT != null)
                {                  
                    foreach (DataRow dr in DynamicTag_DT.AsEnumerable())
                    {
                        string isDeleted = dr["IsDeleted"].ToString();
                        ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRule = new ECN_Framework_Entities.Communicator.DynamicTagRule();
                        if (dr["DynamicTagRuleID"].ToString().Contains("-") && isDeleted.Equals("False"))
                        {
                            //New DynamicTagRules 
                            DynamicTagRule.RuleID = Convert.ToInt32(dr["RuleID"].ToString());
                            DynamicTagRule.DynamicTagID = DynamicTag.DynamicTagID;
                            DynamicTagRule.ContentID = Convert.ToInt32(dr["ContentID"].ToString());
                            DynamicTagRule.CreatedUserID = currentUser.UserID;
                            DynamicTagRule.Priority = Convert.ToInt32(dr["Priority"].ToString());
                            ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.Save(DynamicTagRule, currentUser);
                        }
                        else
                        {
                            //Existing DynamicTagRules that were not deleted
                            DynamicTagRule.DynamicTagRuleID = Convert.ToInt32(dr["DynamicTagRuleID"].ToString());
                            DynamicTagRule.RuleID = Convert.ToInt32(dr["RuleID"].ToString());
                            DynamicTagRule.DynamicTagID = DynamicTag.DynamicTagID;
                            DynamicTagRule.ContentID = Convert.ToInt32(dr["ContentID"].ToString());
                            DynamicTagRule.CreatedUserID = currentUser.UserID;
                            DynamicTagRule.Priority = Convert.ToInt32(dr["Priority"].ToString());
                            ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.Save(DynamicTagRule, currentUser);

                        }

                        //Existing DynamicTagRules that were deleted
                        if (isDeleted.Equals("True") && !dr["DynamicTagRuleID"].ToString().Contains("-") && getDynamicTagID() > 0)
                        {
                            ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.Delete(Convert.ToInt32(dr["DynamicTagRuleID"].ToString()), currentUser);
                        }
                    }
                }
                Response.Redirect("dynamicTagList.aspx");
            }
            catch (ECNException ex)
            {
                setECNError(ex);
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

        protected void rolRules_ItemReorder(object sender, AjaxControlToolkit.ReorderListItemReorderEventArgs e)
        {
            int NewIndex = e.NewIndex + 1;
            int OldIndex = e.OldIndex + 1;
            var ItemMoved_DynamicTagRuleID = (from src in DynamicTag_DT.AsEnumerable()
                                              where src.Field<int>("Priority") == OldIndex
                                                select new
                                                {
                                                    DynamicTagRuleID = src.Field<string>("DynamicTagRuleID")
                                                }).ToList();

            if (NewIndex < OldIndex)
            {
                foreach (DataRow dr in DynamicTag_DT.AsEnumerable())
                {
                    if (Convert.ToInt32(dr["Priority"]) >= NewIndex && Convert.ToInt32(dr["Priority"]) <= OldIndex)
                    {
                        dr["Priority"] = Convert.ToInt32(dr["Priority"]) + 1;
                    }
                }
            }
            else if (NewIndex > OldIndex)
            {
                foreach (DataRow dr in DynamicTag_DT.AsEnumerable())
                {
                    if (Convert.ToInt32(dr["Priority"]) <= NewIndex && Convert.ToInt32(dr["Priority"]) >= OldIndex)
                    {
                        dr["Priority"] = Convert.ToInt32(dr["Priority"]) - 1;
                    }
                }
            }

            foreach (DataRow dr in DynamicTag_DT.AsEnumerable())
            {
                if (dr["DynamicTagRuleID"].Equals(ItemMoved_DynamicTagRuleID[0].DynamicTagRuleID))
                {
                    dr["Priority"] = NewIndex;
                }
            }
            loadGrid();
        }
        
        protected void rolRules_DeleteCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            string DynamicTagRuleID = e.CommandArgument.ToString();
            int currentPriority = 0;
            foreach (DataRow dr in DynamicTag_DT.AsEnumerable())
            {
                if (dr["DynamicTagRuleID"].Equals(DynamicTagRuleID))
                {
                    dr["IsDeleted"] = true;
                    currentPriority = Convert.ToInt32(dr["Priority"]);
                    break;
                }
            }

            foreach (DataRow dr in DynamicTag_DT.AsEnumerable())
            {
                if (Convert.ToInt32(dr["Priority"]) > currentPriority)
                {
                    dr["Priority"] = Convert.ToInt32(dr["Priority"]) - 1;
                }
            }
            hfRuleCount.Value = (Convert.ToInt32(hfRuleCount.Value) - 1).ToString();
            loadGrid();
        }

        protected void CreateContent_Close(object sender, EventArgs e)
        {
            contentEditor1.Reset();
            hfContentEditorFromTag.Value = "";
            this.modalPopupCreateContent.Hide();
        }

        protected void CreateContent_Save(object sender, EventArgs e)
        {
            int contentID = contentEditor1.SaveContent();
            if (contentID > 0)
            {
                ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID, Master.UserSession.CurrentUser, false);
                if (hfContentEditorFromTag.Value.Equals("True"))
                {
                    hfTagContentID.Value = contentID.ToString();
                    txtContentTag.Text = content.ContentTitle;
                }
                else
                {
                    hfRuleContentID.Value = contentID.ToString();
                    txtContentRule.Text = content.ContentTitle;
                }
                contentEditor1.Reset();
                this.modalPopupCreateContent.Hide();
            }
        }

        protected void btnNewContentRule_Click(object sender, EventArgs e)
        {
            hfContentEditorFromTag.Value = "False";
            this.modalPopupCreateContent.Show();
        }

        protected void btnNewContentTag_Click(object sender, EventArgs e)
        {
            hfContentEditorFromTag.Value = "True";
            this.modalPopupCreateContent.Show();
        }
        
        protected void btnRuleEditorSave_Click(object sender, EventArgs e)
        {
            int ruleID=RuleEditor1.SaveRule();
            if (ruleID > 0)
            {
                hfRuleID.Value = ruleID.ToString();
                ECN_Framework_Entities.Communicator.Rule rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID(ruleID, Master.UserSession.CurrentUser, false);
                txtRule.Text = rule.RuleName;
                RuleEditor1.Reset();
                this.modalPopupCreateRule.Hide();
                loadRules();
                if (getDynamicTagID() > 0)
                {
                    loadData(getDynamicTagID());
                }

            }
        }

        protected void btnRuleEditorClose_Click(object sender, EventArgs e)
        {
            RuleEditor1.Reset();
            this.modalPopupCreateRule.Hide();
        }

        protected void btnNewRule_Click(object sender, EventArgs e)
        {
            this.modalPopupCreateRule.Show();
        }

        protected void btnExistingRule_Click(object sender, EventArgs e)
        {
            this.modalPopupExistingRule.Show();
        }

        protected void btnExistingRuleClose_Click(object sender, EventArgs e)
        {
            drpRule.SelectedValue = "-Select-";
            this.modalPopupExistingRule.Hide();
        }

        protected void btnExitingRuleOk_Click(object sender, EventArgs e)
        {
            if(!drpRule.SelectedValue.Equals("-Select-"))
            {
                int ruleID = Convert.ToInt32(drpRule.SelectedValue);
                hfRuleID.Value = ruleID.ToString();
                ECN_Framework_Entities.Communicator.Rule rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID(ruleID, Master.UserSession.CurrentUser, false);
                txtRule.Text = rule.RuleName;
                drpRule.SelectedValue = "-Select-";
                this.modalPopupExistingRule.Hide();
            }              
        }

        protected void rolRules_EditCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            
            RuleEditor1.LoadExisting(Convert.ToInt32(e.CommandArgument.ToString()));
            RuleEditor1.selectedRuleID = Convert.ToInt32(e.CommandArgument.ToString());
            
            this.modalPopupCreateRule.Show();
        }

        protected void imgbtnRuleEdit_Click1(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            
            RuleEditor1.LoadExisting(Convert.ToInt32(imgbtn.CommandArgument.ToString()));
            RuleEditor1.selectedRuleID = Convert.ToInt32(imgbtn.CommandArgument.ToString());

            this.modalPopupCreateRule.Show();
        }

        


        
    }
}