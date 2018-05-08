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
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ecn.communicator.main.events
{

    public partial class GroupTriggers : TriggerBase
    {       
       
        protected System.Web.UI.WebControls.DataGrid LayoutActions;
        protected System.Web.UI.WebControls.DropDownList RestrictGroup;
        protected System.Web.UI.HtmlControls.HtmlForm BlastPlanner;

        public override PlaceHolder PhError => phError;
        public override MasterPages.Communicator EcnMaster => Master;
        public override RadioButtonList NoopRadioList => NOOP_RadioList;

        public override TextBox NoopEmailFrom => NOOP_EmailFrom;
        public override TextBox NoopReplyTo => NOOP_ReplyTo;
        public override TextBox NoopEmailFromName => NOOP_EmailFrom;
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            PageLoad();

            Master.SubMenu = "group triggers";
            Master.Heading = "Group Triggers";
            Master.HelpContent = "<b>Blast setup</b><br />Schedule the emails to blast. <br /> <font color=#FF0000> Note: System prohibits sending the same campaign to the same group within 7 days.</font><p><ul><li>Select the campaign from the <i>Campaign</i> Dropdown list.</li><br /><br /><li>Select the group you want the emails to be sent from the <i>Groups</i> Dropdown list.</li><br /><br /><li>Enter the <i>From email address</i>, <i>From Name</i> and the <i>Subject</i> of the email.</li><br /><br /><li>If you want the Blast to be scheduled now, hit the <i>BlastNow!</i> button. If you want the blast to be scheduled for a later date, set the date and time from <i>Send Time</i> dropdown lists and click on <i>Create Schedule<i> button.</li></ul></p>";

			if (Page.IsPostBack==false)
            {                				
				_triggerCampaign.DataTextField = "GroupName";
				_triggerCampaign.DataValueField = "GroupID";
                _triggerCampaign.DataSource = ECN_Framework_BusinessLayer.Communicator.Group.GetGroupDR(CustomerId, UserId, Master.UserSession.CurrentUser);
				_triggerCampaign.DataBind();
				_triggerCampaign.Items.Insert(0,new ListItem("Reply On All", "0"));
				_triggerCampaign.Items.FindByValue("0").Selected = true;

                _replyCampaign.DataSource = ECN_Framework_BusinessLayer.Communicator.Layout.GetLayoutDR(CustomerId, UserId, Master.UserSession.CurrentUser);
				_replyCampaign.DataBind();

                NOOP_ReplyCampaign.DataSource =  ECN_Framework_BusinessLayer.Communicator.Layout.GetLayoutDR(CustomerId, UserId, Master.UserSession.CurrentUser);
				NOOP_ReplyCampaign.DataBind();

                SetNoopControlStatus(false);

                BindList();

                Criteria.Items.Insert(0,new ListItem("Any Match", ""));
				Criteria.Items.FindByValue("").Selected = true;
				Criteria.Items.Insert(1,new ListItem("Subscribe", "S"));
				Criteria.Items.Insert(1,new ListItem("UnSubscribe", "U"));
				Criteria.Items.Insert(1,new ListItem("Pending", "P"));                
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
			set 
            {
                Session["CurrentLayoutPlanID"] = value;
            }
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

		#region Event Handlers

        protected void CreateButton_Click(object sender, System.EventArgs e)
        {           
            try
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(_triggerCampaign.SelectedItem.Value), Master.UserSession.CurrentUser);

                TimeSpan ts = new TimeSpan(Convert.ToInt32(_period.Text), Convert.ToInt32(_txtHours.Text), Convert.ToInt32(_txtMinutes.Text), 0);

                if (_createButton.Text == "Update Trigger")
                {
                    ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(LayoutPlanID, Master.UserSession.CurrentUser);
                    layoutPlan.GroupID = group.GroupID;
                    layoutPlan.ActionName = _layoutName.Text;
                    layoutPlan.Period = Convert.ToDecimal(ts.TotalDays);
                    layoutPlan.Criteria = Criteria.SelectedItem.Value;
                    layoutPlan.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    layoutPlan.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    ECN_Framework_BusinessLayer.Communicator.LayoutPlans.Save(layoutPlan, Master.UserSession.CurrentUser);
                    ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser, false);
                    GetBlastFromControls(blast);

                    UpdateGroupCampaign(blast, layoutPlan.BlastID.Value);
                    blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser, false);
                    ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByRefTriggerID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser);
                    if (triggerPlan != null)
                    {
                        //there is a NOOP trigger associated already. 

                        if (NOOP_RadioList.SelectedValue == "Y")
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
                            }

                            ECN_Framework_Entities.Communicator.Blast trigblast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(triggerPlan.BlastID.Value, Master.UserSession.CurrentUser, false);
                            GetNOOPBlastFromControls(trigblast);

                            UpdateTriggerCampaign(trigblast, triggerPlan.BlastID.Value);
                            trigblast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(triggerPlan.BlastID.Value, Master.UserSession.CurrentUser, false);
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.TriggerPlans.Delete(triggerPlan.TriggerPlanID, Master.UserSession.CurrentUser);
                        }
                    }
                    else
                    {
                        //there is NO NOOP trigger associated. Check to see if they want to Create one (Radio Button = Y)
                        //If Yes, Create one else don't do anything
                        if (NOOP_RadioList.SelectedValue == "Y")
                        {
                            int blastID = layoutPlan.BlastID.Value;
                            TimeSpan noopTS = new TimeSpan(Convert.ToInt32(NOOP_Period.Text), Convert.ToInt32(NOOP_txtHours.Text), Convert.ToInt32(NOOP_txtMinutes.Text), 0);

                            ECN_Framework_Entities.Communicator.Blast newTrigBlast = new ECN_Framework_Entities.Communicator.Blast();
                            GetNOOPBlastFromControls(newTrigBlast);

                            int newBlastID = CreateTriggerCampaign(newTrigBlast);
                            newTrigBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(newBlastID, Master.UserSession.CurrentUser, false);
                            ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddNoOpenTriggerPlan(blastID, newTrigBlast.BlastID, null, noopTS.TotalDays, "", "NO OPEN on " + _layoutName.Text.ToString(), Master.UserSession.CurrentUser);

                        }
                    }
                    ClearControls();
                    Server.Transfer(@".\GroupTriggers.aspx");
                }
                else
                {
                    ECN_Framework_Entities.Communicator.Blast my_blast = new ECN_Framework_Entities.Communicator.Blast();
                    GetBlastFromControls(my_blast);
                    int newBlastID = CreateGroupCampaign(my_blast);
                    if (newBlastID > 0)
                    {
                        my_blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(newBlastID, Master.UserSession.CurrentUser, false);

                        ECN_Framework_Entities.Communicator.EventOrganizer eventer = new ECN_Framework_Entities.Communicator.EventOrganizer();
                        eventer.CustomerID = CustomerId;
                        ECN_Framework_Entities.Communicator.LayoutPlans currentLayoutPlan = ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddSubscribePlan(null, my_blast.BlastID, group.GroupID, _layoutName.Text, ts.TotalDays, Criteria.SelectedItem.Value, Master.UserSession.CurrentUser);

                        //Create a No Open Trigger if you select "YES"
                        if (NOOP_RadioList.SelectedValue == "Y")
                        {
                            int blastID = currentLayoutPlan.BlastID.Value;
                            TimeSpan noopTS = new TimeSpan(Convert.ToInt32(NOOP_Period.Text), Convert.ToInt32(NOOP_txtHours.Text), Convert.ToInt32(NOOP_txtMinutes.Text), 0);

                            ECN_Framework_Entities.Communicator.Blast newTrigBlast = new ECN_Framework_Entities.Communicator.Blast();
                            GetNOOPBlastFromControls(newTrigBlast);
                            int newTriggerBlastID = CreateTriggerCampaign(newTrigBlast);
                            newTrigBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(newTriggerBlastID, Master.UserSession.CurrentUser, false);
                            ECN_Framework_BusinessLayer.Communicator.EventOrganizer.AddNoOpenTriggerPlan(blastID, newTrigBlast.BlastID, 0, noopTS.TotalDays, "", "NO OPEN on " + _layoutName.Text.ToString(), Master.UserSession.CurrentUser);
                        }

                        ClearControls();
                        Server.Transfer(@".\GroupTriggers.aspx");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
                return;
            }
		}

        private int CreateGroupCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder)
        {
            return CreateCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout);
        }

        private int UpdateGroupCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder, int blastID)
        {
            return CreateCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout, blastID);
        }

        private int CreateTriggerCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder)
        {
            return CreateTriggerCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen);
        }

        private int UpdateTriggerCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder, int blastID)
        {
            return CreateTriggerCampaign(blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen, blastID);
        }

        private int CreateCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType ciType, int? blastID = null)
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
            c.CampaignName = blastHolder.EmailSubject;
            ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, Master.UserSession.CurrentUser);

            //campaign item
            ci.CustomerID = Master.UserSession.CurrentUser.CustomerID;
            ci.CampaignItemName = blastHolder.EmailSubject;
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
            cib.EmailSubject = blastHolder.EmailSubject;
            cib.LayoutID = blastHolder.LayoutID;
            cib.GroupID = Convert.ToInt32(_triggerCampaign.SelectedItem.Value);
            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, Master.UserSession.CurrentUser);

            //create blast
            ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, Master.UserSession.CurrentUser);
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID(cib.CampaignItemBlastID, Master.UserSession.CurrentUser, false);
            return blast.BlastID;
        }

        private int CreateTriggerCampaign(ECN_Framework_Entities.Communicator.Blast blastHolder, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType ciType, int? blastID = null)
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
            c.CampaignName = blastHolder.EmailSubject;
            ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, Master.UserSession.CurrentUser);

            //campaign item
            ci.CustomerID = Master.UserSession.CurrentUser.CustomerID;
            ci.CampaignItemName = blastHolder.EmailSubject;
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
            cib.EmailSubject = blastHolder.EmailSubject;
            cib.LayoutID = blastHolder.LayoutID;
            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, Master.UserSession.CurrentUser);

            //create blast
            ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, Master.UserSession.CurrentUser);
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID(cib.CampaignItemBlastID, Master.UserSession.CurrentUser, false);
            return blast.BlastID;
        }

        protected void EventType_SelectedIndexChanged(object sender, System.EventArgs e) 
        {           
            Criteria.Items.Clear();
            Criteria.Items.Insert(0,new ListItem("Any Match", ""));
            Criteria.Items.FindByValue("").Selected = true;
            Criteria.Items.Insert(1,new ListItem("Subscribe", "S"));
            Criteria.Items.Insert(1,new ListItem("UnSubscribe", "U"));
            Criteria.Items.Insert(1,new ListItem("Pending", "P"));
        }

        protected void TriggerCampaign_SelectedIndexChanged(object sender, System.EventArgs e)
        {
          
        }		

		protected void LayoutActions_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
        {
            ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan =
            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(Convert.ToInt32(e.CommandArgument), Master.UserSession.CurrentUser);

			switch (e.CommandName)
            {
				case "Edit":
					LayoutPlanID = Convert.ToInt32(e.CommandArgument);
					LoadLayoutPlan(LayoutPlanID);
					_createButton.Text = "Update Trigger";
					break;
				case "Activate":
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            if (layoutPlan.Status.Equals("Y"))
                                layoutPlan.Status = "N";
                            else
                                layoutPlan.Status = "Y";
                            layoutPlan.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                       
                            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.Save(layoutPlan, Master.UserSession.CurrentUser);
                     
                            ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByRefTriggerID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser);

                            if (triggerPlan.Status.Equals("Y"))
                                triggerPlan.Status = "N";
                            else
                                triggerPlan.Status = "Y";
                            triggerPlan.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                            ECN_Framework_BusinessLayer.Communicator.TriggerPlans.Save(triggerPlan, Master.UserSession.CurrentUser);
                            BindList();
                            scope.Complete();
                        }
                        catch (ECN_Framework_Common.Objects.ECNException ex)
                        {
                            setECNError(ex);
                            return;
                        }
                    }
					break;
				case "Delete":
					int layoutPlanID = Convert.ToInt32(e.CommandArgument);
					if (layoutPlanID >0)
                    {
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.Delete(layoutPlanID, Master.UserSession.CurrentUser);
                        }
                        catch (ECN_Framework_Common.Objects.ECNException ex)
                        {
                            setECNError(ex);
                            return;
                        }
						Response.Redirect("GroupTriggers.aspx");					
					}
					break;
				case "Copy":
					LoadLayoutPlan(Convert.ToInt32(e.CommandArgument));
					_createButton.Text = "Create Trigger";
					break;
				default:
					throw new ApplicationException(string.Format("Unknow command '{0}'."));
			}
		}


        protected List<ECN_Framework_Entities.Communicator.LayoutPlans> GetLayoutPlansForGroup(int groupID)
        {
            return ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByGroupID(groupID,Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
        }

		protected void NOOP_RadioList_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
			if(NOOP_RadioList.SelectedValue == "N")
            {
				SetNoopControlStatus(false);
			}
            else
            {
				SetNoopControlStatus(true);
			}
		}
		#endregion

		#region Private/Protected Methods		

		private void LoadLayoutPlan(int layoutPlanID)
        {
            LoadLayoutPlanBase(LayoutPlanID);
            var layoutPlan=
                ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID, Master.UserSession.CurrentUser);
            _layoutName.Text = layoutPlan.ActionName;

            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser, false);

            int triggerIndex = _triggerCampaign.Items.IndexOf(_triggerCampaign.Items.FindByValue(layoutPlan.GroupID.ToString()));
			if (triggerIndex != -1) 
            {
				_triggerCampaign.SelectedIndex = triggerIndex;
			}

            int criteriaIndex = Criteria.Items.IndexOf(Criteria.Items.FindByValue(layoutPlan.Criteria));
			if (criteriaIndex != -1) {
				Criteria.SelectedIndex = criteriaIndex;
			}

            int followupIndex = _replyCampaign.Items.IndexOf(_replyCampaign.Items.FindByValue(layoutPlan.LayoutID.ToString()));
			if (followupIndex != -1) {
				_replyCampaign.SelectedIndex = followupIndex;				
			}

            ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByRefTriggerID(layoutPlan.BlastID.Value, Master.UserSession.CurrentUser);
         
			if(triggerPlan!=null)
            {
				NOOP_RadioList.SelectedValue = "Y";
				SetNoopControlStatus(true);
                ECN_Framework_Entities.Communicator.Blast trigblast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(triggerPlan.BlastID.Value, Master.UserSession.CurrentUser, false); 

				int trigFollowupIndex = 0;
				try
                {
					trigFollowupIndex = NOOP_ReplyCampaign.Items.IndexOf(NOOP_ReplyCampaign.Items.FindByValue(trigblast.LayoutID.ToString()));
					if (trigFollowupIndex != -1) 
                    {
						NOOP_ReplyCampaign.SelectedIndex = trigFollowupIndex;			
					}
				}
                catch
                {
                }
			}
		}	

		private void ClearControls()
		{
		    ClearControlsBase();
			_triggerCampaign.SelectedIndex = 0;					
			_replyCampaign.SelectedIndex = 0;
			ClearNoopControls();
		}

        protected override void ClearNoopControls()
        {
			NOOP_ReplyCampaign.SelectedIndex = 0;
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
			NOOP_ReplyCampaign.Enabled = status;
            base.SetNoopControlStatus(status);
        }

        private void GetBlastFromControls(ECN_Framework_Entities.Communicator.Blast my_blast)
        {
            Regex regexBR = new Regex("</?br.*?>");

            string strSubject = regexBR.Replace(ECN_Framework_Common.Functions.StringFunctions.CleanString(_emailSubject.Value), "").Replace("&nbsp;", "").Trim();

            GetBlastFromControlsBase(my_blast, strSubject);
            my_blast.LayoutID = Convert.ToInt32(_replyCampaign.SelectedItem.Value);
            my_blast.GroupID = Convert.ToInt32(_triggerCampaign.SelectedItem.Value);
        }

        private void GetNOOPBlastFromControls(ECN_Framework_Entities.Communicator.Blast trigBlast) 
        {
            Regex regexBR = new Regex("</?br.*?>");

			string strSubject	= regexBR.Replace(ECN_Framework_Common.Functions.StringFunctions.CleanString(NOOP_EmailSubject.Value), "").Replace("&nbsp;","").Trim();
			string strEmailFromName=ECN_Framework_Common.Functions.StringFunctions.CleanString(NOOP_EmailFromName.Text);
            string strEmailFrom = ECN_Framework_Common.Functions.StringFunctions.Remove(NOOP_EmailFrom.Text, ECN_Framework_Common.Functions.StringFunctions.NonDomain());

            trigBlast.CustomerID = CustomerId;
            trigBlast.CreatedUserID = UserId;
            trigBlast.EmailSubject = strSubject;
            trigBlast.EmailFrom = strEmailFrom;
            trigBlast.EmailFromName = strEmailFromName;
			trigBlast.ReplyTo=ECN_Framework_Common.Functions.StringFunctions.CleanString(NOOP_ReplyTo.Text);
            trigBlast.LayoutID = Convert.ToInt32(NOOP_ReplyCampaign.SelectedItem.Value);
            trigBlast.SendTime = DateTime.Now;
        }

		private void BindList() 
        {
            List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary> viewLayoutPlanSummary =
            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetGroupLayoutPlanSummary(CustomerId, Master.UserSession.CurrentUser);
            lstLayoutSummary.DataSource = viewLayoutPlanSummary;
			lstLayoutSummary.DataBind();
		}

		private void lstLayoutSummary_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
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
		}
		#endregion
	}
}
