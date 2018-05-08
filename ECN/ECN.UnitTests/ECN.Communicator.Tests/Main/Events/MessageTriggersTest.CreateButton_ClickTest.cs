using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.events.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;
using commEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.Events
{
    public partial class MessageTriggersTest
    {
        private const string LayoutName = "layoutName";
        private const string CriteriaValue = "criteriaValue";
        private const string HfSelectedLayoutTriggerValue = "100";

        [Test]
        public void CreateButton_Click_UpdateNotActive_LayoutPlansSaveCalled()
        {
            // Arrange
            var layoutTimeSpan = new TimeSpan(2, 5, 3, 0);
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var userId = 200;
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", layoutTimeSpan: layoutTimeSpan, userId: userId, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: new commEntities.BlastSMS() { CustomerID = 1 }, updateLayoutCampaignValue: -1, hfSelectedLayoutNOOPReply: string.Empty, noopRadioValue: string.Empty, triggerPlan: null);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            layoutPlan.ShouldSatisfyAllConditions(
                () => layoutPlan.LayoutID.ShouldBe(int.Parse(HfSelectedLayoutTriggerValue)),
                () => layoutPlan.ActionName.ShouldBe(LayoutName),
                () => layoutPlan.Criteria.ShouldBe(CriteriaValue),
                () => layoutPlan.Period.ShouldBe(Convert.ToDecimal(layoutTimeSpan.TotalDays)),
                () => layoutPlan.UpdatedUserID.ShouldBe(userId),
                () => layoutPlan.GroupID.ShouldBe(0));
            _layoutPlanSaveMethodCallCount.ShouldBe(1);
        }

        [Test]
        public void CreateButton_Click_UpdateNotActiveSaveError_Error()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var ecnErros = new List<ECNError>();
            ecnErros.Add(new ECNError(Enums.Entity.BlastAB, Enums.Method.Save, "errorMsg"));
            var ecnException = new ECNException(ecnErros);
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: new commEntities.BlastSMS() { CustomerID = 1 }, saveECNException: ecnException, updateLayoutCampaignValue: -1, hfSelectedLayoutNOOPReply: string.Empty, noopRadioValue: string.Empty, triggerPlan: null);
            var error = "<br/>" + Enums.Entity.BlastAB + ": " + "errorMsg";

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [TestCase(-1)]
        [TestCase(1)]
        public void CreateButton_Click_UpdateNotActiveWithTriggerNOOPYesNOOPReply_NoErrors(int updateTriggerMessageMethodValue)
        {
            // Arrange
            var triggerTimeSpan = new TimeSpan(5, 3, 1, 0);
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var userId = 200;
            var blast = new commEntities.BlastSMS() { CustomerID = 1 };
            var triggerPlans = new commEntities.TriggerPlans() { BlastID = 0 };
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: userId, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: triggerPlans, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", triggerTimeSpan: triggerTimeSpan, updateTriggerMessageValue: updateTriggerMessageMethodValue);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            triggerPlans.ShouldSatisfyAllConditions(
                () => triggerPlans.Period.ShouldBe(Convert.ToDecimal(triggerTimeSpan.TotalDays)),
                () => triggerPlans.ActionName.ShouldBe("NO OPEN on " + LayoutName),
                () => triggerPlans.UpdatedUserID.ShouldBe(userId));
            _triggerPlanSaveMethodCallCount.ShouldBe(1);
            _phError.Visible.ShouldBeFalse();
        }

        [Test]
        public void CreateButton_Click_UpdateNotActiveWithTriggerNOOPYesNOOPReplySaveError_Error()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { CustomerID = 1 };
            var triggerPlans = new commEntities.TriggerPlans() { BlastID = 0 };
            var ecnErros = new List<ECNError>();
            ecnErros.Add(new ECNError(Enums.Entity.BlastAB, Enums.Method.Save, "errorMsg"));
            var ecnException = new ECNException(ecnErros);
            var error = "<br/>" + Enums.Entity.BlastAB + ": " + "errorMsg";
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: triggerPlans, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1, triggerPlanSaveException: ecnException);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [Test]
        public void CreateButton_Click_UpdateNotActiveWithTriggerNOOPYesNOOPReplyGetNOOPBlastError_Error()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { CustomerID = 1 };
            var triggerPlans = new commEntities.TriggerPlans() { BlastID = 0 };
            var ecnErros = new List<ECNError>();
            ecnErros.Add(new ECNError(Enums.Entity.BlastAB, Enums.Method.Save, "GetNOOPBlast_Error"));
            var ecnException = new ECNException(ecnErros);
            var error = "<br/>" + Enums.Entity.BlastAB + ": " + "GetNOOPBlast_Error";
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: triggerPlans, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);
            _shimMessageTriggers.GetNOOPBlastFromControlsBlast = (b) => throw ecnException;

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [Test]
        public void CreateButton_Click_UpdateNotActiveWithTriggerNOOPYesNullNOOPReply_Error()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { CustomerID = 1 };
            var triggerPlans = new commEntities.TriggerPlans() { BlastID = 0 };
            var error = "<br/>" + Enums.Entity.LayoutPlans + ": " + "Please select a No Open follow up message";
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: triggerPlans, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: string.Empty, updateTriggerMessageValue: 1);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [Test]
        public void CreateButton_Click_UpdateNotActiveWithTriggerNOOPNo_DeleteCalled()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { CustomerID = 1 };
            var triggerPlans = new commEntities.TriggerPlans() { BlastID = 0 };
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: triggerPlans, noopRadioValue: "N", hfSelectedLayoutNOOPReply: string.Empty, updateTriggerMessageValue: 1);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _triggerPlanDeleteMethodCallCount.ShouldBe(1);
        }

        [Test]
        public void CreateButton_Click_UpdateActiveWithTrigger_Errors()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { CustomerID = 1 };
            var triggerPlans = new commEntities.TriggerPlans() { BlastID = 0 };
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: triggerPlans, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1, activeOrSentResult: true);
            var error = "<br/>" + Enums.Entity.LayoutPlans + ": " + "Triggers have already been sent.  Cannot update.";

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [TestCase(1)]
        [TestCase(-1)]
        public void CreateButton_Click_UpdateNotActiveNoTriggerNOOPYesNOOPReply_SaveCalled(int blastId)
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { BlastID = blastId, CustomerID = 1 };
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeFalse();
            var saveCallCount = blastId > -1 ? 1 : 0;
            if (blastId > -1)
            {
                _triggerPlanSaveMethodCallCount.ShouldBe(saveCallCount);
            }
        }

        [Test]
        public void CreateButton_Click_UpdateNotActiveNoTriggerNOOPYesNOOPReplyGetNOOPBlastError_Error()
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var ecnErros = new List<ECNError>();
            ecnErros.Add(new ECNError(Enums.Entity.BlastAB, Enums.Method.Save, "GetNOOPBlast_Error"));
            var ecnException = new ECNException(ecnErros);
            _shimMessageTriggers.GetNOOPBlastFromControlsBlast = (b) => throw ecnException;
            var error = "<br/>" + Enums.Entity.BlastAB + ": " + "GetNOOPBlast_Error";
            var blast = new commEntities.BlastSMS() { BlastID = 1, CustomerID = 1 };
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [TestCase("Y", "")]
        [TestCase("N", "10")]
        public void CreateButton_Click_UpdateNotActiveNoTriggerWrongSelectedValue_Error(string noopValue, string layoutNOOPReplyValue)
        {
            // Arrange
            var layoutPlan = new commEntities.LayoutPlans() { BlastID = 0 };
            var blast = new commEntities.BlastSMS() { BlastID = 1, CustomerID = 1 };
            var error = "<br/>" + Enums.Entity.LayoutPlans + ": " + "Please select a No Open follow up message";
            InitTest_CreateButton_Click(createBtnText: "Update Trigger", userId: 0, layoutPlaneId: 0, layoutPlans: layoutPlan, blastAbstract: blast, triggerPlan: null, noopRadioValue: noopValue, hfSelectedLayoutNOOPReply: layoutNOOPReplyValue, updateTriggerMessageValue: 1);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _triggerPlanSaveMethodCallCount.ShouldBe(0);
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [Test]
        public void CreateButton_Click_NotUpdateNoLayoutReply_Error()
        {
            // Arrange
            var error = "<br/>" + Enums.Entity.LayoutPlans + ": " + "Please select a follow up message";
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: null, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10");
            _messageTriggersPrivateObject.SetField("hfSelectedLayoutReply", BindingFlags.Instance | BindingFlags.NonPublic, new HiddenField());

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }
        [Test]
        public void CreateButton_Click_NotUpdateNOOPYesWithoutNOOPReply_Error()
        {
            // Arrange
            var error = "<br/>" + Enums.Entity.LayoutPlans + ": " + "Please select a No Open follow up message";
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: null, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: string.Empty);

            // Act 
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [TestCase("subscribe")]
        [TestCase("open")]
        public void CreateButton_Click_NotUpdateNOOPYes_SaveCalledNoError(string eventTypeItem)
        {
            // Arrange
            var blast = new commEntities.BlastSMS() { CustomerID = 1, BlastID = 1 };
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: blast, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1, eventTypeSelectedItem: eventTypeItem);

            // Act
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _triggerPlanSaveMethodCallCount.ShouldBe(1);
            _phError.Visible.ShouldBeFalse();
        }

        [Test]
        public void CreateButton_Click_NotUpdateNOOPYesInvalidTriggerBlastId_SkipSaveNoError()
        {
            // Arrange
            var blast = new commEntities.BlastSMS() { CustomerID = 1, BlastID = 1 };
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: blast, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);
            ShimMessageTriggers.AllInstances.CreateTriggerMessageBlast = (m, b) => -1;

            // Act
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _triggerPlanSaveMethodCallCount.ShouldBe(0);
            _phError.Visible.ShouldBeFalse();
        }

        [Test]
        public void CreateButton_Click_NotUpdateNOOPYesGetBlastError_Error()
        {
            // Arrange
            var blast = new commEntities.BlastSMS() { CustomerID = 1, BlastID = 1 };
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: blast, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);
            var ecnErros = new List<ECNError>();
            ecnErros.Add(new ECNError(Enums.Entity.BlastAB, Enums.Method.Get, "GetBlastFromControlsBlast_Error"));
            var ecnException = new ECNException(ecnErros);
            var error = "<br/>" + Enums.Entity.BlastAB + ": " + "GetBlastFromControlsBlast_Error";
            ShimMessageTriggers.AllInstances.GetBlastFromControlsBlast = (m, b) => throw ecnException;

            // Act
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _triggerPlanSaveMethodCallCount.ShouldBe(0);
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [Test]
        public void CreateButton_Click_NotUpdateNOOPYesGetNOOPBlastError_Error()
        {
            // Arrange
            var blast = new commEntities.BlastSMS() { CustomerID = 1, BlastID = 1 };
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: blast, triggerPlan: null, noopRadioValue: "Y", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);
            var ecnErros = new List<ECNError>();
            ecnErros.Add(new ECNError(Enums.Entity.BlastAB, Enums.Method.Get, "GetNOOPBlastFromControlsBlast_Error"));
            var ecnException = new ECNException(ecnErros);
            var error = "<br/>" + Enums.Entity.BlastAB + ": " + "GetNOOPBlastFromControlsBlast_Error";
            ShimMessageTriggers.AllInstances.GetNOOPBlastFromControlsBlast = (m, b) => throw ecnException;

            // Act
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _triggerPlanSaveMethodCallCount.ShouldBe(0);
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        [Test]
        public void CreateButton_Click_NotUpdateNOOPNO_Error()
        {
            // Arrange
            var blast = new commEntities.BlastSMS() { CustomerID = 1, BlastID = 1 };
            InitTest_CreateButton_Click(createBtnText: "NotUpdate", userId: 0, layoutPlaneId: 0, layoutPlans: null, blastAbstract: blast, triggerPlan: null, noopRadioValue: "N", hfSelectedLayoutNOOPReply: "10", updateTriggerMessageValue: 1);
            var error = "<br/>" + Enums.Entity.LayoutPlans + ": " + "Please select a No Open follow up message";

            // Act
            _messageTriggersPrivateObject.Invoke("CreateButton_Click", new object[] { null, null });

            // Assert
            _triggerPlanSaveMethodCallCount.ShouldBe(0);
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(error);
        }

        private void InitTest_CreateButton_Click(string createBtnText, int userId, int layoutPlaneId, commEntities.LayoutPlans layoutPlans, commEntities.BlastAbstract blastAbstract, commEntities.TriggerPlans triggerPlan, string noopRadioValue, string hfSelectedLayoutNOOPReply, TimeSpan layoutTimeSpan = default(TimeSpan), TimeSpan triggerTimeSpan = default(TimeSpan), ECNException saveECNException = null, int updateLayoutCampaignValue = 10, ECNException triggerPlanSaveException = null, int updateTriggerMessageValue = -1, bool activeOrSentResult = false, string eventTypeSelectedItem = "subscribe")
        {
            InitMainForTest_CreateButton_Click(createBtnText: createBtnText, userId: userId, layoutTimeSpan: layoutTimeSpan);
            InitCommonForTest_CreateButton_Click(layoutPlaneId: layoutPlaneId, layoutPlans: layoutPlans, blastAbstract: blastAbstract, triggerPlan: triggerPlan, noopRadioValue: noopRadioValue, hfSelectedLayoutNOOPReply: hfSelectedLayoutNOOPReply, triggerTimeSpan: triggerTimeSpan, saveECNException: saveECNException, updateLayoutCampaignValue: updateLayoutCampaignValue, triggerPlanSaveException: triggerPlanSaveException, updateTriggerMessageValue: updateTriggerMessageValue, activeOrSentResult: activeOrSentResult, eventTypeSelectedItem: eventTypeSelectedItem);
        }
        private void InitMainForTest_CreateButton_Click(string createBtnText, int userId, TimeSpan layoutTimeSpan = default(TimeSpan))
        {
            SetDefaultMembers();
            SetPageProperties(userId);
            _messageTriggersPrivateObject.SetField("hfSelectedLayoutTrigger", BindingFlags.Instance | BindingFlags.NonPublic, new HiddenField() { Value = HfSelectedLayoutTriggerValue });
            _messageTriggersPrivateObject.SetField("_createButton", BindingFlags.Instance | BindingFlags.NonPublic, new Button() { Text = createBtnText });
            _messageTriggersPrivateObject.SetField("_period", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = layoutTimeSpan.Days.ToString() });
            _messageTriggersPrivateObject.SetField("_txtHours", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = layoutTimeSpan.Hours.ToString() });
            _messageTriggersPrivateObject.SetField("_txtMinutes", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = layoutTimeSpan.Minutes.ToString() });
        }
        private void InitCommonForTest_CreateButton_Click(int layoutPlaneId, commEntities.LayoutPlans layoutPlans, commEntities.BlastAbstract blastAbstract, commEntities.TriggerPlans triggerPlan, string noopRadioValue, string hfSelectedLayoutNOOPReply, TimeSpan triggerTimeSpan = default(TimeSpan), ECNException saveECNException = null, int updateLayoutCampaignValue = 10, ECNException triggerPlanSaveException = null, int updateTriggerMessageValue = -1, bool activeOrSentResult = false, string eventTypeSelectedItem = "subscribe")
        {
            var criteriaDropDownList = new DropDownList();
            criteriaDropDownList.Items.Add(new ListItem() { Value = CriteriaValue, Selected = true });
            var noopRadioLst = new RadioButtonList();
            noopRadioLst.Items.Add(noopRadioValue);
            noopRadioLst.SelectedIndex = 0;
            var hfSelectedLayoutNOOPReplay = new HiddenField() { Value = hfSelectedLayoutNOOPReply };
            var eventTypeDropDown = new DropDownList();
            eventTypeDropDown.Items.Add(eventTypeSelectedItem);
            eventTypeDropDown.SelectedIndex = 0;
            ShimMessageTriggers.AllInstances.LayoutPlanIDGet = (s) => layoutPlaneId;
            ShimMessageTriggers.AllInstances.UpdateLayoutCampaignBlastInt32 = (m, b, i) => updateLayoutCampaignValue;
            _shimMessageTriggers.UpdateTriggerMessageBlastInt32 = (b, id) => updateTriggerMessageValue;
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (id, user) => layoutPlans;
            ShimLayoutPlans.SaveLayoutPlansUser = (l, user) =>
            {
                _layoutPlanSaveMethodCallCount++;
                if (saveECNException != null) throw saveECNException;
                return 0;
            };
            ShimBlast.GetByBlastIDInt32UserBoolean = (id, user, b) => blastAbstract;
            ShimBlast.GetByCampaignItemBlastIDInt32UserBoolean = (c, u, b) => blastAbstract;
            ShimBlast.ActiveOrSentInt32Int32 = (bId, cId) => activeOrSentResult;
            ShimTriggerPlans.GetByRefTriggerIDInt32User = (i, u) => triggerPlan;
            ShimTriggerPlans.SaveTriggerPlansUser = (t, u) =>
            {
                _triggerPlanSaveMethodCallCount++;
                if (triggerPlanSaveException != null)
                {
                    throw triggerPlanSaveException;
                }
                return 0;
            };
            _messageTriggersPrivateObject.SetField("_criteria", BindingFlags.Instance | BindingFlags.NonPublic, criteriaDropDownList);
            _messageTriggersPrivateObject.SetField("_layoutName", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = LayoutName });
            _messageTriggersPrivateObject.SetField("NOOP_RadioList", BindingFlags.Instance | BindingFlags.NonPublic, noopRadioLst);
            _messageTriggersPrivateObject.SetField("hfSelectedLayoutNOOPReply", BindingFlags.Instance | BindingFlags.NonPublic, hfSelectedLayoutNOOPReplay);
            _messageTriggersPrivateObject.SetField("NOOP_Period", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = triggerTimeSpan.Days.ToString() });
            _messageTriggersPrivateObject.SetField("NOOP_txtHours", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = triggerTimeSpan.Hours.ToString() });
            _messageTriggersPrivateObject.SetField("NOOP_txtMinutes", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = triggerTimeSpan.Minutes.ToString() });
            _messageTriggersPrivateObject.SetField("EventType", BindingFlags.Instance | BindingFlags.NonPublic, eventTypeDropDown);
        }
        private void SetDefaultMembers()
        {
            ShimLayout.GetByLayoutIDInt32UserBoolean = (id, user, getChildren) => new Layout() { LayoutID = 0 };
            ShimCampaign.SaveCampaignUser = (c, u) => 0;
            ShimCampaignItem.SaveCampaignItemUser = (c, u) => 0;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (c, u, b) => 0;
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (c, u, b) => { };
            ShimLayoutPlans.ClearCacheForLayoutPlanLayoutPlans = (p) => { };
            _messageTriggersPrivateObject.SetField("_emailSubject", BindingFlags.Instance | BindingFlags.NonPublic, new HiddenField() { Value = "_emailSubject" });
            _messageTriggersPrivateObject.SetField("hfSelectedLayoutReply", BindingFlags.Instance | BindingFlags.NonPublic, new HiddenField() { Value = "100" });
            _messageTriggersPrivateObject.SetField("_emailFromName", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = "EmailFromName" });
            _messageTriggersPrivateObject.SetField("_emailFrom", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = "EmailFrom" });
            _messageTriggersPrivateObject.SetField("_replyTo", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = "ReplyTo" });
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameTA", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = "txtCampaingItemNameTA" });
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameNO", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox() { Text = "txtCampaingItemNameNO" });
            _messageTriggersPrivateObject.SetField("_layoutName", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
            _messageTriggersPrivateObject.SetField("lblSelectedLayoutTrigger", BindingFlags.Instance | BindingFlags.NonPublic, new Label());
            _messageTriggersPrivateObject.SetField("lblSelectedLayoutReply", BindingFlags.Instance | BindingFlags.NonPublic, new Label());
            _messageTriggersPrivateObject.SetField("_criteria", BindingFlags.Instance | BindingFlags.NonPublic, new DropDownList());
            _messageTriggersPrivateObject.SetField("lblSelectedLayoutNOOPReply", BindingFlags.Instance | BindingFlags.NonPublic, new Label());
            _messageTriggersPrivateObject.SetField("NOOP_EmailFrom", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
            _messageTriggersPrivateObject.SetField("NOOP_ReplyTo", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
            _messageTriggersPrivateObject.SetField("NOOP_EmailFromName", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
            _messageTriggersPrivateObject.SetField("NOOP_EmailSubject", BindingFlags.Instance | BindingFlags.NonPublic, new HiddenField());
            _messageTriggersPrivateObject.SetField("NOOP_Period", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
            _messageTriggersPrivateObject.SetField("NOOP_txtHours", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
            _messageTriggersPrivateObject.SetField("NOOP_txtMinutes", BindingFlags.Instance | BindingFlags.NonPublic, new TextBox());
        }
        private void SetPageProperties(int userId)
        {
            var shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new KMPlatform.Entity.User() { UserID = userId };
            ShimCommunicator.AllInstances.UserSessionGet = (c) => shimECNSession;
            ShimMessageTriggers.AllInstances.MasterGet = (mt) => new ecn.communicator.MasterPages.Communicator();
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (h, u, b) => { };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
        }
    }
}
