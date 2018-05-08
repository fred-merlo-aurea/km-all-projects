using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Entity.Fakes;
using ecn.communicator.main.Salesforce.SF_Pages;
using ecn.communicator.main.Salesforce.SF_Pages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Activity;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.Salesforce.SF_Pages.SF_CampaignActivity"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_CampaignActivityTest : PageHelper
    {
        private PrivateObject _testObject;
        private Page _testPage;
        private IDisposable _shimObject;
        private string _errorMessage;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            CreateMasterPage();
            CreateTestObjects();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Page_Load_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
        }

        [Test]
        public void Page_Load_WithoutLogin_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = false;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null })); 
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("You must first log into Salesforce to use this page");
        }

        [Test]
        public void btnSyncCampaign_Click_Success()
        {
            // Arrange 
            InitializeSyncFakes();
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.SelectedValue = "1";
            ddlECNCampItem.SelectedValue = "1";
            ShimActivityResults.AllInstances.SyncSuccessGet = (p) => true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncCampaign_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("lblBounceTotal") as Label).Text.ShouldBe("0");
            (_testObject.GetFieldOrProperty("lblClickTotal") as Label).Text.ShouldBe("0");
            (_testObject.GetFieldOrProperty("lblOpenTotal") as Label).Text.ShouldBe("0");
        }

        [Test]
        public void btnSyncCampaign_Click_Exception()
        {
            // Arrange 
            InitializeSyncFakes();
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.SelectedValue = "1";
            ddlECNCampItem.SelectedValue = "1";
            ShimSF_CampaignActivity.AllInstances.TransferOpensListOfBlastActivityOpensListOfSF_CampaignMemberInt32ActivityResults =
               (p1, p2, p3, p4, p5) => throw new Exception("Test Exception");

            // Act
            _testObject.Invoke("btnSyncCampaign_Click", new object[] { null, null });
           
            // Assert
            _errorMessage.ShouldBe("SF_CampaignActivity.TransferActivity");
        }

        [Test]
        public void btnSyncCampaign_Click_Exception_WithMessage()
        {
            // Arrange 
            InitializeSyncFakes();
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.SelectedValue = "1";
            ddlECNCampItem.SelectedValue = "1";
            ShimSF_CampaignActivity.AllInstances.TransferOpensListOfBlastActivityOpensListOfSF_CampaignMemberInt32ActivityResults =
               (p1, p2, p3, p4, p5) => throw new Exception("Test Exception");
            ShimActivityResults.AllInstances.ErrorMessageGet = (p) => "Test Error";

            // Act
            _testObject.Invoke("btnSyncCampaign_Click", new object[] { null, null });

            // Assert
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Test Error");
        }

        [Test]
        public void btnSyncCampaign_Click_NoSalesForce_Error()
        {
            // Arrange 
            InitializeSyncFakes();
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.SelectedValue = "0";
            ddlECNCampItem.SelectedValue = "1";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncCampaign_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please select a Salesforce campaign to sync to");
        }

        [Test]
        public void btnSyncCampaign_Click_NoECN_Error()
        {
            // Arrange 
            InitializeSyncFakes();
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.SelectedValue = "1";
            ddlECNCampItem.SelectedValue = "0";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncCampaign_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please select an ECN campaign item to sync from");
        }

        [Test]
        public void btnSyncCampaign_Click_NoCampaing_Error()
        {
            // Arrange 
            InitializeSyncFakes();
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.SelectedValue = "0";
            ddlECNCampItem.SelectedValue = "0";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncCampaign_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please select campaigns to sync");
        }
        [Test]
        public void TransferUnsubs_Success()
        {
            // Arrange 
            var listOpens = new List<BlastActivityUnSubscribes> {
                new BlastActivityUnSubscribes { EmailID = 1, UnsubscribeCodeID = 1 },
                new BlastActivityUnSubscribes { EmailID = 2, UnsubscribeCodeID = 1 },
                new BlastActivityUnSubscribes { EmailID = 3, UnsubscribeCodeID = 1 },
                new BlastActivityUnSubscribes { EmailID = 4, UnsubscribeCodeID = 1 },
                new BlastActivityUnSubscribes { EmailID = 5, UnsubscribeCodeID = 2 },
                new BlastActivityUnSubscribes { EmailID = 6, UnsubscribeCodeID = 2 }  };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1", Status = "subscribed" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2", Status = "subscribed" },
                new SF_CampaignMember { Id = "3", ContactId = "3", LeadId = "3", Status = "subscribed" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 3, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 3, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 4, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 4, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 5, DataValue = "3" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 5, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 6, DataValue = "3" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 6, DataValue = "contact"} };
            ShimSF_Lead.OptOutStringString = (p1, p2) => true;

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferUnsubs", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.UnsubTotal.ShouldBe(4),
                () => ar.MSTotal.ShouldBe(2));
        }

        [Test]
        public void TransferUnsubs_NullMasterList_Error()
        {
            // Arrange 
            var listOpens = new List<BlastActivityUnSubscribes> {
                new BlastActivityUnSubscribes { EmailID = 1, UnsubscribeCodeID = 1 },
                new BlastActivityUnSubscribes { EmailID = 2, UnsubscribeCodeID = 1 },
                new BlastActivityUnSubscribes { EmailID = 3, UnsubscribeCodeID = 2 }  };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 3, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 3, DataValue = "contact"}};
            _testObject.SetFieldOrProperty("MasterList", null);
            var numberOfCatchedExceptions = 0;
            ShimSF_Utilities.LogExceptionException = (ex) => numberOfCatchedExceptions++;

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferUnsubs", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.UnsubTotal.ShouldBe(2),
                () => ar.MSTotal.ShouldBe(1));
            numberOfCatchedExceptions.ShouldBe(3);
        }

        [Test]
        public void TransferUnsubs_TypeError()
        {
            // Arrange 
            var listOpens = new List<BlastActivityUnSubscribes> { };
            var listCampMember = new List<SF_CampaignMember> { };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> { };

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferUnsubs", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeFalse(),
                () => ar.UnsubTotal.ShouldBe(0),
                () => ar.ErrorMessage.ShouldBe("Could not sync email activity.  Required Salesforce UDFs are missing"));
        }

        [Test]
        public void TransferOpens_Success()
        {
            // Arrange 
            var listOpens = new List<BlastActivityOpens> {
                new BlastActivityOpens {EmailID = 1 },
                new BlastActivityOpens { EmailID = 2 },
                new BlastActivityOpens { EmailID = 3 },
                new BlastActivityOpens { EmailID = 4 } };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 3, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 3, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 4, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 4, DataValue = "lead"} };

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferOpens", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.OpenTotal.ShouldBe(4));
        }

        [Test]
        public void TransferOpens_NullMasterList_Error()
        {
            // Arrange 
            var listOpens = new List<BlastActivityOpens> {
                new BlastActivityOpens {EmailID = 1 },
                new BlastActivityOpens { EmailID = 2 } };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "lead"}};
            _testObject.SetFieldOrProperty("MasterList", null);
            var numberOfCatchedExceptions = 0;
            ShimSF_Utilities.LogExceptionException = (ex) => numberOfCatchedExceptions++;

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferOpens", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.OpenTotal.ShouldBe(2));
            numberOfCatchedExceptions.ShouldBe(2);
        }

        [Test]
        public void TransferOpens_TypeError()
        {
            // Arrange 
            var listOpens = new List<BlastActivityOpens> {};
            var listCampMember = new List<SF_CampaignMember> {};
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> { };

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferOpens", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeFalse(),
                () => ar.OpenTotal.ShouldBe(0),
                () => ar.ErrorMessage.ShouldBe("Could not sync email activity.  Required Salesforce UDFs are missing"));
        }
        
        [Test]
        public void TransferClicks_Success()
        {
            // Arrange 
            var listOpens = new List<BlastActivityClicks> {
                new BlastActivityClicks {EmailID = 1 },
                new BlastActivityClicks { EmailID = 2 },
                new BlastActivityClicks { EmailID = 3 },
                new BlastActivityClicks { EmailID = 4 } };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 3, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 3, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 4, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 4, DataValue = "lead"} };

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferClicks", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.ClickTotal.ShouldBe(4));
        }

        [Test]
        public void TransferClicks_NullMasterList_Error()
        {
            // Arrange 
            var listOpens = new List<BlastActivityClicks> {
                new BlastActivityClicks {EmailID = 1 },
                new BlastActivityClicks { EmailID = 2 } };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "lead"}};
            _testObject.SetFieldOrProperty("MasterList", null);
            var numberOfCatchedExceptions = 0;
            ShimSF_Utilities.LogExceptionException = (ex) => numberOfCatchedExceptions++;

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferClicks", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.ClickTotal.ShouldBe(2));
            numberOfCatchedExceptions.ShouldBe(2);
        }

        [Test]
        public void TransferClicks_TypeError()
        {
            // Arrange 
            var listOpens = new List<BlastActivityClicks> { };
            var listCampMember = new List<SF_CampaignMember> { };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> { };

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferClicks", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeFalse(),
                () => ar.ClickTotal.ShouldBe(0),
                () => ar.ErrorMessage.ShouldBe("Could not sync email activity.  Required Salesforce UDFs are missing"));
        }

        [Test]
        public void TransferBounces_Success()
        {
            // Arrange 
            var listOpens = new List<BlastActivityBounces> {
                new BlastActivityBounces { EmailID = 1, BounceCode = "Hardbounce"},
                new BlastActivityBounces { EmailID = 2, BounceCode = "Hardbounce" },
                new BlastActivityBounces { EmailID = 3, BounceCode = "Hardbounce" },
                new BlastActivityBounces { EmailID = 4, BounceCode = "Hardbounce" },
                new BlastActivityBounces { EmailID = 5, BounceCode = "Softbounce"},
                new BlastActivityBounces { EmailID = 6, BounceCode = "Softbounce" },
                new BlastActivityBounces { EmailID = 7, BounceCode = "Softbounce" },
                new BlastActivityBounces { EmailID = 8, BounceCode = "Softbounce" } };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" },
                new SF_CampaignMember { Id = "3", ContactId = "3", LeadId = "3" },
                new SF_CampaignMember { Id = "4", ContactId = "4", LeadId = "4" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 3, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 3, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 4, DataValue = "2" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 4, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 5, DataValue = "3" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 5, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 6, DataValue = "3" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 6, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 7, DataValue = "4" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 7, DataValue = "lead"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 8, DataValue = "4" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 8, DataValue = "lead"}};

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferBounces", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.BounceTotal.ShouldBe(8));
        }

        [Test]
        public void TransferBounces_NullMasterList_Error()
        {
            // Arrange 
            var listOpens = new List<BlastActivityBounces> {
                new BlastActivityBounces {EmailID = 1, BounceCode = "Hardbounce" },
                new BlastActivityBounces { EmailID = 2, BounceCode = "Hardbounce" } };
            var listCampMember = new List<SF_CampaignMember> {
                new SF_CampaignMember { Id = "1", ContactId = "1", LeadId = "1" },
                new SF_CampaignMember { Id = "2", ContactId = "2", LeadId = "2" } };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields{ ShortName = "sfid", GroupDataFieldsID = 1},
                new GroupDataFields{  ShortName = "sftype", GroupDataFieldsID = 2} };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> {
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 1, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 1, DataValue = "contact"},
                new EmailDataValues { GroupDataFieldsID = 1, EmailID = 2, DataValue = "1" },
                new EmailDataValues { GroupDataFieldsID = 2, EmailID = 2, DataValue = "lead"}};
            _testObject.SetFieldOrProperty("MasterList", null);
            var numberOfCatchedExceptions = 0;
            ShimSF_Utilities.LogExceptionException = (ex) => numberOfCatchedExceptions++;

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferBounces", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeTrue(),
                () => ar.BounceTotal.ShouldBe(2));
            numberOfCatchedExceptions.ShouldBe(2);
        }

        [Test]
        public void TransferBounces_TypeError()
        {
            // Arrange 
            var listOpens = new List<BlastActivityBounces> { };
            var listCampMember = new List<SF_CampaignMember> { };
            var activityResult = new ActivityResults { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { };
            ShimEmailDataValues.GetByGroupIDInt32User = (p1, p2) => new List<EmailDataValues> { };

            // Act, Assert
            var ar = Should.NotThrow(() => _testObject.Invoke("TransferBounces", new object[] { listOpens, listCampMember, 1, activityResult })) as ActivityResults;
            ar.ShouldSatisfyAllConditions(
                () => ar.ShouldNotBeNull(),
                () => ar.SyncSuccess.ShouldBeFalse(),
                () => ar.BounceTotal.ShouldBe(0),
                () => ar.ErrorMessage.ShouldBe("Could not sync email activity.  Required Salesforce UDFs are missing"));
        }

        [Test]
        public void MemberStatusCheck_Success()
        {
            // Arrange 
            ShimSF_CampaignMemberStatus.GetListStringString = (p1, p2) => new List<SF_CampaignMemberStatus> { new SF_CampaignMemberStatus { } };
            ShimSF_CampaignMemberStatus.GetAllString = (p) => new List<SF_CampaignMemberStatus> { };
            var numberOfInserts = 0;
            ShimSF_CampaignMemberStatus.InsertStringSF_CampaignMemberStatus = (p1, p2) => { numberOfInserts++; return true; };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("MemberStatusCheck", new object[] { "1" })) ;
            numberOfInserts.ShouldBe(6);
        }

        private void CreateTestObjects()
        {
            _testPage = new SF_CampaignActivity();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            var ddlECNCampItem = _testObject.GetFieldOrProperty("ddlECNCampItem") as DropDownList;
            ddlSFCampaign.Items.Add("0");
            ddlSFCampaign.Items.Add("1");
            ddlECNCampItem.Items.Add("0");
            ddlECNCampItem.Items.Add("1");
        }

        private void InitializeSyncFakes()
        {
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (p1, p2, p3, p4, p5, p6) => { _errorMessage = p2; return 0; };
            ShimSF_CampaignMember.GetListStringString = (p1, p2) => new List<SF_CampaignMember> { };
            ShimSF_CampaignMemberStatus.GetAllString = (p) => new List<SF_CampaignMemberStatus> { };
            ShimSF_CampaignActivity.AllInstances.MemberStatusCheckString = (p1, p2) =>
            {
                _testObject.SetFieldOrProperty("MasterList", new Dictionary<string, string> { { "1", "1" }, { "2", "2" } });
                _testObject.SetFieldOrProperty("ToMasterSuppressList", new Dictionary<string, string> { { "1", "1" }, { "2", "2" } });
            };
            ShimBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) => new List<BlastAbstract> { new BlastChampion { BlastID = 1, GroupID = 1 } };
            ShimBlastActivityUnSubscribes.GetByBlastIDInt32 = (p) => new List<BlastActivityUnSubscribes> { };
            ShimBlastActivityOpens.GetByBlastIDInt32 = (p) => new List<BlastActivityOpens> { };
            ShimBlastActivityClicks.GetByBlastIDInt32 = (p) => new List<BlastActivityClicks> { };
            ShimBlastActivityBounces.GetByBlastIDInt32 = (p) => new List<BlastActivityBounces> { };
            ShimSF_CampaignActivity.AllInstances.TransferOpensListOfBlastActivityOpensListOfSF_CampaignMemberInt32ActivityResults =
                (p1, p2, p3, p4, p5) => new ActivityResults { SyncSuccess = true };
            ShimSF_CampaignActivity.AllInstances.TransferUnsubsListOfBlastActivityUnSubscribesListOfSF_CampaignMemberInt32ActivityResults =
                (p1, p2, p3, p4, p5) => new ActivityResults { SyncSuccess = true };
            ShimSF_CampaignActivity.AllInstances.TransferClicksListOfBlastActivityClicksListOfSF_CampaignMemberInt32ActivityResults =
                (p1, p2, p3, p4, p5) => new ActivityResults { SyncSuccess = true };
            ShimSF_CampaignActivity.AllInstances.TransferBouncesListOfBlastActivityBouncesListOfSF_CampaignMemberInt32ActivityResults =
                (p1, p2, p3, p4, p5) => new ActivityResults { SyncSuccess = true };
            ShimSF_Job.CreateStringStringSF_UtilitiesSFObject = (p1, p2, p3) => string.Empty;
            ShimSF_CampaignMember.GetXMLForUpdateJobDictionaryOfStringString = (p) => string.Empty;
            ShimSF_Job.AddBatchStringStringString = (p1, p2, p3) => string.Empty;
            ShimSF_Job.CloseStringString = (p1, p2) => true;
            ShimSF_Job.GetBatchStateStringStringString = (p1, p2, p3) => true;
            ShimSF_Job.GetBatchResultsStringStringString = (p1, p2, p3) => new Dictionary<string, int> { };
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection { {"KMCommom_Application", "1" } };
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
        }
    }
}
