using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;

namespace ecn.communicator.main.events
{
    public partial class MessageTriggers : TriggerBase
    {
        protected System.Web.UI.WebControls.DataGrid LayoutActions;
        protected System.Web.UI.WebControls.DropDownList RestrictGroup;
        protected System.Web.UI.WebControls.LinkButton btnCount;
        protected System.Web.UI.HtmlControls.HtmlForm BlastPlanner;

        public override PlaceHolder PhError => phError;
        public override MasterPages.Communicator EcnMaster => Master;
        public override RadioButtonList NoopRadioList => NOOP_RadioList;

        public override TextBox NoopEmailFrom => NOOP_EmailFrom;
        public override TextBox NoopReplyTo => NOOP_ReplyTo;
        public override TextBox NoopEmailFromName => NOOP_EmailFromName;
        public override TextBox NoopPeriod => NOOP_Period;
        public override TextBox NoopTxtHours => NOOP_txtHours;
        public override TextBox NoopTxtMinutes => NOOP_txtMinutes;
        public override RequiredFieldValidator ValNoopEmailFrom => val_NOOP_EmailFrom;
        public override RequiredFieldValidator ValNoopReplyTo => val_NOOP_ReplyTo;
        public override RequiredFieldValidator ValNoopEmailFromName => val_NOOP_EmailFromName;
        public override RequiredFieldValidator ValNoopPeriod => val_NOOP_Period;
        public override RequiredFieldValidator ValNoopTxtHours => val_NOOP_txtHours;
        public override RequiredFieldValidator ValNoopTxtMinutes => val_NOOP_txtMinutes;
        public override RangeValidator RangeValNoopPeriod => rangeval_NOOP_Period;
        public override RangeValidator RangeValNoopTxtHours => rangeval_NOOP_txtHours;
        public override RangeValidator RangeValNoopTxtMinutes => rangeval_NOOP_txtMinutes;
        public override HiddenField EmailSubject => _emailSubject;
        public override HiddenField NoopEmailSubject => NOOP_EmailSubject;
        public override TextBox EmailFromName => _emailFromName;
        public override TextBox EmailFrom => _emailFrom;
        public override TextBox ReplyTo => _replyTo;
        public override Button CreateButton => _createButton;
        public override TextBox LayoutName => _layoutName;
        public override DropDownList Criteria => _criteria;
        public override TextBox Period => _period;
        public override TextBox TxtHours => _txtHours;
        public override TextBox TxtMinutes => _txtMinutes;

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void DoTwemoji()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "pageloaded();", true);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            PageLoad();

            Master.SubMenu = "message triggers";
            Master.Heading = "Message Triggers";
            Master.HelpContent = "<b>Blast setup</b><br />Schedule the emails to blast. <br /> <font color=#FF0000> Note: System prohibits sending the same Message to the same group within 7 days.</font><p><ul><li>Select the Message from the <i>Message</i> Dropdown list.</li><br /><br /><li>Select the group you want the emails to be sent from the <i>Groups</i> Dropdown list.</li><br /><br /><li>Enter the <i>From email address</i>, <i>From Name</i> and the <i>Subject</i> of the email.</li><br /><br /><li>If you want the Blast to be scheduled now, hit the <i>BlastNow!</i> button. If you want the blast to be scheduled for a later date, set the date and time from <i>Send Time</i> dropdown lists and click on <i>Create Schedule<i> button.</li></ul></p>";

            if (Page.IsPostBack == false)
            {
                
                layoutExplorer.enableSelectMode();
                SetNoopControlStatus(false);

                BindList();

                DataTable dt = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetLinkAliasDR(CustomerId, 0, Master.UserSession.CurrentUser);
                DataView dvlist = new DataView(dt);
                dvlist.Sort = "Alias";
                _criteria.DataSource = dvlist;
                _criteria.DataTextField = "Alias";
                _criteria.DataValueField = "Link";
                _criteria.DataBind();

                _criteria.Items.Insert(0, new ListItem("Any Match", ""));
                _criteria.Items.FindByValue("").Selected = true;
            }
        }


        private int LayoutPlanID
        {
            get
            {
                if (Session["CurrentLayoutPlanID"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(Session["CurrentLayoutPlanID"]);
            }

            set { Session["CurrentLayoutPlanID"] = value; }
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


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            this.lstLayoutSummary.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.lstLayoutSummary_ItemCommand);

        }
        #endregion

        #region Event Handler
        protected void CreateButton_Click(object sender, System.EventArgs e)
        {
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(Convert.ToInt32(hfSelectedLayoutTrigger.Value), Master.UserSession.CurrentUser, false);
            
            TimeSpan ts = new TimeSpan(Convert.ToInt32(_period.Text), Convert.ToInt32(_txtHours.Text), Convert.ToInt32(_txtMinutes.Text), 0);

            if (_createButton.Text == "Update Trigger")
            {
                ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(LayoutPlanID, Master.UserSession.CurrentUser);
                ECN_Framework_BusinessLayer.Communicator.LayoutPlans.ClearCacheForLayoutPlan(layoutPlan);
                ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser, false);
                if (!ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(blast.BlastID, blast.CustomerID.Value))
                {

                    layoutPlan.LayoutID = Convert.ToInt32(hfSelectedLayoutTrigger.Value);
                    layoutPlan.ActionName = _layoutName.Text;
                    layoutPlan.Criteria = _criteria.SelectedItem.Value;
                    layoutPlan.Period = Convert.ToDecimal(ts.TotalDays);
                    layoutPlan.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    layoutPlan.GroupID = 0;

                    
                    try
                    {
                        GetBlastFromControls(blast);
                        ECN_Framework_BusinessLayer.Communicator.LayoutPlans.Save(layoutPlan, Master.UserSession.CurrentUser);
                    }
                    catch (ECN_Framework_Common.Objects.ECNException ex)
                    {
                        setECNError(ex);
                        return;
                    }


                    if (UpdateLayoutCampaign(blast, layoutPlan.BlastID.Value) == -1)
                        return;
                    blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser, false);

                    ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByRefTriggerID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser);

                    if (triggerPlan != null)
                    {
                        //there is a NOOP trigger associated already. 

                        if (NOOP_RadioList.SelectedValue == "Y" && !string.IsNullOrEmpty(hfSelectedLayoutNOOPReply.Value))
                        {
                            //Check to see if they want it still active (Radio Button = Y)
                            //If Yes, Update the NOOP trigger. 
                            TimeSpan noopTS = new TimeSpan(Convert.ToInt32(NOOP_Period.Text), Convert.ToInt32(NOOP_txtHours.Text), Convert.ToInt32(NOOP_txtMinutes.Text), 0);
                            triggerPlan.Period = Convert.ToDecimal(noopTS.TotalDays);
                            triggerPlan.ActionName = "NO OPEN on " + _layoutName.Text;
                            triggerPlan.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                            try
                            {
                                ECN_Framework_BusinessLayer.Communicator.TriggerPlans.Save(triggerPlan, Master.UserSession.CurrentUser);
                            }
                            catch (ECN_Framework_Common.Objects.ECNException ex)
                            {
                                setECNError(ex);
                                return;
                            }


                            ECN_Framework_Entities.Communicator.Blast trigblast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(triggerPlan.BlastID.Value, Master.UserSession.CurrentUser, false);

                            try
                            {
                                GetNOOPBlastFromControls(trigblast);
                            }
                            catch(ECN_Framework_Common.Objects.ECNException ecn)
                            {
                                setECNError(ecn);
                                return;
                            }
                            if (UpdateTriggerMessage(trigblast, triggerPlan.BlastID.Value) == -1)
                                return;
                            trigblast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(triggerPlan.BlastID.Value, Master.UserSession.CurrentUser, false);

                        }
                        else if(NOOP_RadioList.SelectedValue == "Y" && string.IsNullOrEmpty(hfSelectedLayoutNOOPReply.Value))
                        {
                            throwECNException("Please select a No Open follow up message");
                        }
                        else if (NOOP_RadioList.SelectedValue == "N")
                        {
                            ECN_Framework_BusinessLayer.Communicator.TriggerPlans.Delete(triggerPlan.TriggerPlanID, Master.UserSession.CurrentUser);
                        }

                    }
                    else
                    {
                        //there is NO NOOP trigger associated. Check to see if they want to Create one (Radio Button = Y)
                        //If Yes, Create one else don't do anything
                        if (NOOP_RadioList.SelectedValue == "Y" && !string.IsNullOrEmpty(hfSelectedLayoutNOOPReply.Value))
                        {
                            int blastID = layoutPlan.BlastID.Value;
                            TimeSpan noopTS = new TimeSpan(Convert.ToInt32(NOOP_Period.Text), Convert.ToInt32(NOOP_txtHours.Text), Convert.ToInt32(NOOP_txtMinutes.Text), 0);

                            ECN_Framework_Entities.Communicator.Blast newTrigBlast = new ECN_Framework_Entities.Communicator.Blast();
                            try
                            {
                                GetNOOPBlastFromControls(newTrigBlast);
                            }
                            catch(ECN_Framework_Common.Objects.ECNException ecn)
                            {
                                setECNError(ecn);
                                return;
                            }
                            int newBlastID = CreateTriggerMessage(newTrigBlast);
                            if (newBlastID == -1)
                                return;
                            newTrigBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(newBlastID, Master.UserSession.CurrentUser, false);
                            ECN_Framework_Entities.Communicator.EventOrganizer noopEventer = new ECN_Framework_Entities.Communicator.EventOrganizer();
                            noopEventer.CustomerID = CustomerId;
                            ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddNoOpenTriggerPlan(blastID, newTrigBlast.BlastID, 0, noopTS.TotalDays, "", "NO OPEN on " + _layoutName.Text.ToString(), Master.UserSession.CurrentUser);
                        }
                        else
                        {
                            throwECNException("Please select a No Open follow up message");
                        }
                    }
                }
                else
                {
                    throwECNException("Triggers have already been sent.  Cannot update.");
                    return;
                }
                ClearControls();
                //Server.Transfer(@".\MessageTriggers.aspx");
                Response.Redirect(Request.RawUrl, false);
            }
            else
            {
                if (!string.IsNullOrEmpty(hfSelectedLayoutTrigger.Value))
                {
                    if (!string.IsNullOrEmpty(hfSelectedLayoutReply.Value))
                    {
                        if(NOOP_RadioList.SelectedValue == "Y" && string.IsNullOrEmpty(hfSelectedLayoutNOOPReply.Value))
                        {
                            throwECNException("Please select a No Open follow up message");
                            return;
                        }
                        ECN_Framework_Entities.Communicator.Blast my_blast = new ECN_Framework_Entities.Communicator.Blast();
                        try
                        {
                            GetBlastFromControls(my_blast);
                        }
                        catch(ECN_Framework_Common.Objects.ECNException ecn)
                        {
                            setECNError(ecn);
                            return;
                        }
                        int newBlastID = CreateLayoutCampaign(my_blast);
                        if (newBlastID != -1)
                        {
                            my_blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(newBlastID, Master.UserSession.CurrentUser, false);

                            ECN_Framework_Entities.Communicator.EventOrganizer eventer = new ECN_Framework_Entities.Communicator.EventOrganizer();
                            eventer.CustomerID = CustomerId;
                            ECN_Framework_Entities.Communicator.LayoutPlans currentLayoutPlan;
                            if (EventType.SelectedItem.Value == "open")
                            {
                                currentLayoutPlan = ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddOpenPlan(layout.LayoutID, my_blast.BlastID, 0, _layoutName.Text, ts.TotalDays, _criteria.SelectedItem.Value, Master.UserSession.CurrentUser);
                            }
                            else
                            {
                                currentLayoutPlan = ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddClickPlan(layout.LayoutID, my_blast.BlastID, 0, _layoutName.Text, ts.TotalDays, _criteria.SelectedItem.Value, Master.UserSession.CurrentUser);
                            }

                            //Create a No Open Trigger if you select "YES"
                            if (NOOP_RadioList.SelectedValue == "Y" && !string.IsNullOrEmpty(hfSelectedLayoutNOOPReply.Value))
                            {

                                int blastID = currentLayoutPlan.BlastID.Value;
                                TimeSpan noopTS = new TimeSpan(Convert.ToInt32(NOOP_Period.Text), Convert.ToInt32(NOOP_txtHours.Text), Convert.ToInt32(NOOP_txtMinutes.Text), 0);

                                ECN_Framework_Entities.Communicator.Blast newTrigBlast = new ECN_Framework_Entities.Communicator.Blast();
                                try
                                {
                                    GetNOOPBlastFromControls(newTrigBlast);
                                }
                                catch(ECN_Framework_Common.Objects.ECNException ecn)
                                {
                                    setECNError(ecn);
                                    return;

                                }
                                

                                int newTriggerBlastID = CreateTriggerMessage(newTrigBlast);
                                if (newTriggerBlastID == -1)
                                    return;
                                newTrigBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(newTriggerBlastID, Master.UserSession.CurrentUser, false);
                                ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddNoOpenTriggerPlan(blastID, newTrigBlast.BlastID, 0, noopTS.TotalDays, "", "NO OPEN on " + _layoutName.Text.ToString(), Master.UserSession.CurrentUser);


                            }
                            else
                            {
                                throwECNException("Please select a No Open follow up message");
                            }


                            ClearControls();
                            Response.Redirect(Request.RawUrl, false);
                        }



                    }
                    else
                    {
                        throwECNException("Please select a follow up message");
                    }
                }
                else
                {
                    throwECNException("Please select a target message");
                }
            }
        }

        private int CreateLayoutCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder)
        {
            return CreateCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout);
        }

        private int UpdateLayoutCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder, int blastID)
        {
            return CreateCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout, blastID);
        }

        private int CreateTriggerMessage(ECN_Framework_Entities.Communicator.Blast blastHolder)
        {
            return CreateCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen);
        }

        private int UpdateTriggerMessage(ECN_Framework_Entities.Communicator.Blast blastHolder, int blastID)
        {
            return CreateCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen, blastID);
        }

        private int CreateCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType ciType, int? blastID = null)
        {
            try
            {
                ECN_Framework_Entities.Communicator.Campaign c = null;
                ECN_Framework_Entities.Communicator.CampaignItem ci = null;
                ECN_Framework_Entities.Communicator.CampaignItemBlast cib = null;
                if (blastID != null)
                {
                    c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByBlastID(blastID.Value, Master.UserSession.CurrentUser, true);
                    c.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    ci = c.ItemList[0];
                    ci.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    cib = ci.BlastList[0];
                    cib.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                }
                else
                {
                    c = new ECN_Framework_Entities.Communicator.Campaign();
                    c.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    ci = new ECN_Framework_Entities.Communicator.CampaignItem();
                    ci.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    cib = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    cib.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                }

                //campaign
                c.CustomerID = Master.UserSession.CurrentUser.CustomerID;

                if (ciType == ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen)
                {
                    c.CampaignName = blastHolder.CampaignItemName_NoOpenAction;
                }
                else
                {
                    c.CampaignName = blastHolder.CampaignItemName_TriggerAction;
                }
                //c.CampaignName = blastHolder.EmailSubject;
                ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, Master.UserSession.CurrentUser);

                //campaign item
                ci.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                if (ciType == ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen)
                {
                    ci.CampaignItemName = blastHolder.CampaignItemName_NoOpenAction;
                }
                else
                {
                    ci.CampaignItemName = blastHolder.CampaignItemName_TriggerAction;
                }
                //ci.CampaignItemName = blastHolder.EmailSubject;
                ci.CampaignID = c.CampaignID;
                ci.CampaignItemType = ciType.ToString();
                ci.FromEmail = blastHolder.EmailFrom;
                ci.FromName = blastHolder.EmailFromName;
                ci.ReplyTo = blastHolder.ReplyTo;
                ci.SendTime = DateTime.Now.AddSeconds(15);
                ci.IsHidden = false;
                ci.CampaignItemNameOriginal = blastHolder.EmailSubject;
                ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, Master.UserSession.CurrentUser);

                //campaign item blast
                cib.CampaignItemID = ci.CampaignItemID;
                cib.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                //cib.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                Regex regexBR = new Regex("</?br.*?>");
                cib.EmailSubject = regexBR.Replace(blastHolder.EmailSubject,"");
                cib.LayoutID = blastHolder.LayoutID;
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, Master.UserSession.CurrentUser);

                //create blast
                ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, Master.UserSession.CurrentUser);
                ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID(cib.CampaignItemBlastID, Master.UserSession.CurrentUser, false);

                return blast.BlastID;
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
                return -1;
            }
        }

        protected void EventType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (EventType.SelectedItem.Value == "open")
            {
                _criteria.Items.Clear();
                _criteria.Items.Insert(0, new ListItem("None", ""));
                _criteria.Items.FindByValue("").Selected = true;
            }
            if (EventType.SelectedItem.Value == "click")
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedLayoutTrigger.Value))
                {
                    DataTable dt = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetLinkAliasDR(CustomerId, Convert.ToInt32(hfSelectedLayoutTrigger.Value), Master.UserSession.CurrentUser);
                    DataView dvlist = new DataView(dt);
                    dvlist.Sort = "Alias";
                    _criteria.DataSource = dvlist;
                    _criteria.DataTextField = "Alias";
                    _criteria.DataValueField = "Link";
                    _criteria.DataBind();
                    _criteria.Items.Insert(0, new ListItem("Any Match", ""));
                    _criteria.Items.FindByValue("").Selected = true;
                }
            }
            if (EventType.SelectedItem.Value == "subscribe")
            {
                _criteria.Items.Clear();
                _criteria.Items.Insert(0, new ListItem("Any Match", ""));
                _criteria.Items.FindByValue("").Selected = true;
                _criteria.Items.Insert(1, new ListItem("Subscribe", "S"));
                _criteria.Items.Insert(1, new ListItem("UnSubscribe", "U"));
                _criteria.Items.Insert(1, new ListItem("Pending", "P"));
            }
            if (EventType.SelectedItem.Value == "refer")
            {
                _criteria.Items.Clear();
                _criteria.Items.Insert(0, new ListItem("None", ""));
                _criteria.Items.FindByValue("").Selected = true;
            }
            DoTwemoji();

        }



        protected void LayoutActions_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan =
            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(Convert.ToInt32(e.CommandArgument), Master.UserSession.CurrentUser);
            EventType.Enabled = true;
            switch (e.CommandName)
            {
                case "Edit":
                    LayoutPlanID = Convert.ToInt32(e.CommandArgument);
                    LoadLayoutPlan(Convert.ToInt32(e.CommandArgument));
                    _createButton.Text = "Update Trigger";
                    EventType.Enabled = false;
                    _setScrollBackField.Value = true.ToString();
                    break;
                case "Delete":
                    int layoutPlanID = Convert.ToInt32(e.CommandArgument);
                    if (layoutPlanID > 0)
                    {
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.Delete(layoutPlan.LayoutID.Value, layoutPlanID, Master.UserSession.CurrentUser);
                            ECN_Framework_BusinessLayer.Communicator.BlastSingle.DeleteForLayoutPlanID(layoutPlan.LayoutPlanID, Master.UserSession.CurrentUser);
                        }
                        catch (ECN_Framework_Common.Objects.ECNException ex)
                        {
                            setECNError(ex);
                            return;
                        }
                        ECN_Framework_Entities.Communicator.TriggerPlans tp = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByRefTriggerID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser);
                        if (tp != null)
                        {
                            try
                            {
                                ECN_Framework_BusinessLayer.Communicator.TriggerPlans.Delete(tp.TriggerPlanID, Master.UserSession.CurrentUser);
                                ECN_Framework_BusinessLayer.Communicator.BlastSingle.DeleteForTriggerPlan(tp.TriggerPlanID, tp.BlastID.Value, Master.UserSession.CurrentUser);
                            }
                            catch (ECN_Framework_Common.Objects.ECNException ex)
                            {
                                setECNError(ex);
                                return;
                            }
                        }

                        Response.Redirect("MessageTriggers.aspx");
                    }
                    break;
                default:
                    throw new ApplicationException(string.Format("Unknow command '{0}'."));
            }
            DoTwemoji();
        }

        protected void NOOP_RadioList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (NOOP_RadioList.SelectedValue == "N")
            {
                SetNoopControlStatus(false);
            }
            else
            {
                SetNoopControlStatus(true);
            }
            DoTwemoji();
        }

        protected void imgSelectLayoutTrigger_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            hfWhichLayout.Value = "Trigger";
            mpeLayoutExplorer.Show();
            DoTwemoji();
        }

        protected void imgSelectLayoutReply_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            hfWhichLayout.Value = "Reply";
            mpeLayoutExplorer.Show();
            DoTwemoji();
        }
        protected void imgSelectLayoutNOOPReply_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            hfWhichLayout.Value = "NOOPReply";
            mpeLayoutExplorer.Show();
            DoTwemoji();
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                if (sender is ECN_Framework_Entities.Communicator.Layout)
                {
                    ECN_Framework_Entities.Communicator.Layout layout = (ECN_Framework_Entities.Communicator.Layout)sender;
                    if (hfWhichLayout.Value.Equals("Trigger"))
                    {
                        hfSelectedLayoutTrigger.Value = layout.LayoutID.ToString();
                        lblSelectedLayoutTrigger.Text = layout.LayoutName;

                        if (EventType.SelectedItem.Value == "click")
                        {
                            var dt = BusinessCommunicator.LinkAlias.GetLinkAliasDR(CustomerId, Convert.ToInt32(hfSelectedLayoutTrigger.Value), Master.UserSession.CurrentUser);
                            DataView dvlist = new DataView(dt);
                            dvlist.Sort = "Alias";
                            _criteria.DataSource = dvlist;
                            _criteria.DataTextField = "Alias";
                            _criteria.DataValueField = "Link";
                            _criteria.DataBind();
                            _criteria.Items.Insert(0, new ListItem("Any Match", ""));
                            _criteria.Items.FindByValue("").Selected = true;
                        }
                    }
                    else if (hfWhichLayout.Value.Equals("Reply"))
                    {
                        hfSelectedLayoutReply.Value = layout.LayoutID.ToString();
                        lblSelectedLayoutReply.Text = layout.LayoutName;
                    }
                    else if (hfWhichLayout.Value.Equals("NOOPReply"))
                    {
                        hfSelectedLayoutNOOPReply.Value = layout.LayoutID.ToString();
                        lblSelectedLayoutNOOPReply.Text = layout.LayoutName;
                    }
                    mpeLayoutExplorer.Hide();
                    upMain.Update();
                    DoTwemoji();
                }
            }
            catch { }
            return true;
        }
        #endregion

        #region Private/Protected Methods

        private void throwECNException(string message)
        {
            ECN_Framework_Common.Objects.ECNError ecnError = new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.LayoutPlans, ECN_Framework_Common.Objects.Enums.Method.Get, message);
            List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECN_Framework_Common.Objects.ECNException(errorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.WebSite));
        }

        private void LoadLayoutPlan(int layoutPlanID)
        {
            LoadLayoutPlanBase(layoutPlanID);
            var layoutPlan =
                ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID, Master.UserSession.CurrentUser);
            _layoutName.Text = layoutPlan.ActionName;

            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser, false);

            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutPlan.LayoutID.Value, Master.UserSession.CurrentUser, false);
            
            if (layout != null)
            {
                hfSelectedLayoutTrigger.Value = layout.LayoutID.ToString();
                lblSelectedLayoutTrigger.Text = layout.LayoutName;
            }

            int eventTypeIndex = EventType.Items.IndexOf(EventType.Items.FindByValue(layoutPlan.EventType.ToLower()));
            if (eventTypeIndex != -1)
            {
                EventType.SelectedIndex = eventTypeIndex;
                EventType_SelectedIndexChanged(null, null);
            }

            int criteriaIndex = _criteria.Items.IndexOf(_criteria.Items.FindByValue(layoutPlan.Criteria));
            if (criteriaIndex != -1)
            {
                _criteria.SelectedIndex = criteriaIndex;
            }

            ECN_Framework_Entities.Communicator.Layout layoutReply = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(blast.LayoutID.Value, Master.UserSession.CurrentUser, false);

            if (layoutReply != null)
            {
                hfSelectedLayoutReply.Value = layoutReply.LayoutID.ToString();
                lblSelectedLayoutReply.Text = layoutReply.LayoutName;
            }

            ECN_Framework_Entities.Communicator.CampaignItem campaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(blast.BlastID, Master.UserSession.CurrentUser, false);
            txtCampaingItemNameTA.Text = campaignItem.CampaignItemName;

            //Load NO-OPEN Trigger Details
            ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByRefTriggerID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser);
            if (triggerPlan != null)
            {

                NOOP_RadioList.SelectedValue = "Y";
                SetNoopControlStatus(true);
                ECN_Framework_Entities.Communicator.Blast trigblast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(triggerPlan.BlastID.Value, Master.UserSession.CurrentUser, false);

                int trigFollowupIndex = 0;
                try
                {
                    ECN_Framework_Entities.Communicator.Layout layoutNOOPReply = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(trigblast.LayoutID.Value, Master.UserSession.CurrentUser, false);

                    if (layoutNOOPReply != null)
                    {
                        hfSelectedLayoutNOOPReply.Value = layoutNOOPReply.LayoutID.ToString();
                        lblSelectedLayoutNOOPReply.Text = layoutNOOPReply.LayoutName;
                    }
                }
                catch { }

                campaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(trigblast.BlastID, Master.UserSession.CurrentUser, false);
                txtCampaingItemNameNO.Text = campaignItem.CampaignItemName;
            }
        }

        private void ClearControls()
        {
            ClearControlsBase();
            hfSelectedLayoutTrigger.Value = "";
            lblSelectedLayoutTrigger.Text = "";
            txtCampaingItemNameTA.Text = "";
            txtCampaingItemNameNO.Text = "";
            EventType.Enabled = true;
            lblSelectedLayoutReply.Text = "";
            EventType.SelectedIndex = 0;
            EventType_SelectedIndexChanged(null, null);
            hfSelectedLayoutReply.Value = "";
            lblSelectedLayoutReply.Text = "";
            _period.Text = "0";
            _txtHours.Text = "0";
            _txtMinutes.Text = "0";
            ClearNoopControls();
        }

        protected override void ClearNoopControls()
        {
            hfSelectedLayoutNOOPReply.Value = "";
            lblSelectedLayoutNOOPReply.Text = "";

            NOOP_EmailFrom.Text = string.Empty;
            NOOP_ReplyTo.Text = string.Empty;
            NOOP_EmailFromName.Text = string.Empty;
            NOOP_EmailSubject.Value = string.Empty;
            NOOP_Period.Text = "0";
            NOOP_txtHours.Text = "0";
            NOOP_txtMinutes.Text = "0";
        }

        protected override void SetNoopControlStatus(bool status)
        {
            imgbtnNOOP.Enabled = status;
            base.SetNoopControlStatus(status);

            txtCampaingItemNameNO.Enabled = status;
            rvfCampaingItemNameNO.Enabled = status;
        }

        private void GetBlastFromControls(ECN_Framework_Entities.Communicator.Blast my_blast)
        {
            Regex regexBR = new Regex("</?br.*?>");

            string strSubject = regexBR.Replace(ECN_Framework_Common.Functions.StringFunctions.CleanString(CleanAndStripHTML(_emailSubject.Value)), "");

            GetBlastFromControlsBase(my_blast, strSubject);
            my_blast.LayoutID = Convert.ToInt32(hfSelectedLayoutReply.Value);
            my_blast.GroupID = Convert.ToInt32(hfSelectedLayoutReply.Value);

            my_blast.CampaignItemName_TriggerAction = txtCampaingItemNameTA.Text;
            my_blast.CampaignItemName_NoOpenAction = txtCampaingItemNameNO.Text;
        }

        private string CleanAndStripHTML(string dirty)
        {
            string retString = "";
            Regex htmlStrip = new Regex("<.*?>");
            retString = htmlStrip.Replace(dirty, "");
            retString = retString.Replace("&gt;", ">");
            retString = retString.Replace("&lt;", "<");
            retString = retString.Replace("&nbsp;", "");
            return retString;
        }


        private void GetNOOPBlastFromControls(ECN_Framework_Entities.Communicator.Blast trigBlast)
        {
            Regex regexBR = new Regex("</?br.*?>");

            string strSubject = regexBR.Replace(ECN_Framework_Common.Functions.StringFunctions.CleanString(CleanAndStripHTML(NOOP_EmailSubject.Value)), "");
            string strEmailFromName = ECN_Framework_Common.Functions.StringFunctions.CleanString(NOOP_EmailFromName.Text);
            string strEmailFrom = ECN_Framework_Common.Functions.StringFunctions.Remove(NOOP_EmailFrom.Text, ECN_Framework_Common.Functions.StringFunctions.NonDomain());

            trigBlast.CustomerID = CustomerId;
            trigBlast.CreatedUserID = UserId;
            trigBlast.EmailSubject = strSubject;
            trigBlast.EmailFrom = strEmailFrom;
            trigBlast.EmailFromName = strEmailFromName;
            trigBlast.ReplyTo = ECN_Framework_Common.Functions.StringFunctions.CleanString(NOOP_ReplyTo.Text);
            trigBlast.LayoutID = Convert.ToInt32(hfSelectedLayoutNOOPReply.Value);
            trigBlast.SendTime = DateTime.Now;

            trigBlast.CampaignItemName_TriggerAction = txtCampaingItemNameTA.Text;
            trigBlast.CampaignItemName_NoOpenAction = txtCampaingItemNameNO.Text;
        }

        protected void lstLayoutSummary_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            if (e.CommandName == "DrillDown")
            {
                lstLayoutSummary.SelectedIndex = e.Item.ItemIndex;
                BindList();
                ClearControls();

                NOOP_RadioList.SelectedValue = "N";
                ClearNoopControls();
                SetNoopControlStatus(false);
            }
            DoTwemoji();
        }

        private void BindList()
        {
            List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary> viewLayoutPlanSummary =
            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetCampaignLayoutPlanSummary(CustomerId, Master.UserSession.CurrentUser);
            lstLayoutSummary.DataSource = viewLayoutPlanSummary;
            lstLayoutSummary.DataBind();
        }

        #endregion

        protected List<ECN_Framework_Entities.Communicator.LayoutPlans> GetLayoutPlansForCampaign(int layoutID)
        {
            return ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutID(layoutID, Master.UserSession.CurrentUser, false);
        }

        protected void btnCloseLayoutExplorer_Click(object sender, EventArgs e)
        {
            mpeLayoutExplorer.Hide();
            
        }
    }
}