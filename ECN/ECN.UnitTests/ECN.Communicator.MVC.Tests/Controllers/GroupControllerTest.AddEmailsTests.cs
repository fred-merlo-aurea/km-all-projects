using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Fakes;
using System.Web.Mvc;
using NUnit.Framework;
using ecn.communicator.mvc.Controllers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ecn.communicator.mvc.Infrastructure.Fakes;
using Shouldly;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        private const string SubscriptionTypeS = "S";
        private const string SubscriptionTypeU = "U";
        private const string ActionTypeT = "T";
        private const string ActionTypeI = "I";
        private const string ActionTypeU = "U";
        private const string ActionTypeD = "D";
        private const string ActionTypeS = "S";
        private const string ActionTypeM = "M";
        private const string ActionCount2 = "2";
        private const string ActionCount3 = "3";
        private const string GroupLabelA = "A";
        private const string GroupLabelM = "M";
        private const string DummyEmailAddresses = "dummy1@dummy.com,dummy2@dummy.com,";
        private const string ResultStringInvalidEmailAddress = "Invalid email address:";
        private const string ResultStringSuccessfullyAdded = "Total Records in the File</td><td>5";
        private const string ResultStringNoGroup = "Please select a Group.";
        private const string ResultStringNoData = "0 rows updated/inserted";
        private const string HttpStatusOk = "200";
        private const string HttpStatusInternalServerError = "500";
        private const int SampleGroupID = 1;
        private const int SampleCustomerID = 1;
        private const string SampleColumAction = "Action";
        private const string SampleColumCounts = "Counts";
        private const int SampleDataTableGroupId = 2;

        [Test]
        public void AddEmails_GroupLabelDeafault_SubscriptionTypeU_ReturnJson()
        {
            InitilizeAddEmailsTests();
            var controller = new GroupController();
            var result = controller.AddEmails(String.Empty, SampleGroupID, SampleCustomerID, SubscriptionTypeU, String.Empty, DummyEmailAddresses) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringSuccessfullyAdded));
        }

        [Test]
        public void AddEmails_GroupLabelA_SubscriptionTypeS_ReturnJson()
        {
            InitilizeAddEmailsTests();
            var controller = new GroupController();
            var result = controller.AddEmails(GroupLabelA, SampleGroupID, SampleCustomerID, SubscriptionTypeS, String.Empty, DummyEmailAddresses) as JsonResult;
            
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0],HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringSuccessfullyAdded));
        }

        [Test]
        public void AddEmails_GroupLabelM_SubscriptionTypeU_ReturnJson()
        {
            InitilizeAddEmailsTests();
            var controller = new GroupController();
            var result = controller.AddEmails(GroupLabelM, SampleGroupID, SampleCustomerID, SubscriptionTypeU, String.Empty, DummyEmailAddresses) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringSuccessfullyAdded));
        }

        [Test]
        public void AddEmails_GroupLabelDeafault_SubscriptionTypeU_InvalidEmailAddress_ReturnJson()
        {
            InitilizeAddEmailsTests();
            ShimEmail.IsValidEmailAddressString = (p) => false;
            var controller = new GroupController();
            var result = controller.AddEmails(String.Empty, SampleGroupID, SampleCustomerID, SubscriptionTypeU, String.Empty, DummyEmailAddresses) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringInvalidEmailAddress));
        }

        [Test]
        public void AddEmails_GroupLabelDeafault_SubscriptionTypeS_InvalidEmailAddress_ReturnJson()
        {
            InitilizeAddEmailsTests();
            ShimEmail.IsValidEmailAddressString = (p) => false;
            var controller = new GroupController();
            var result = controller.AddEmails(String.Empty, SampleGroupID, SampleCustomerID, SubscriptionTypeS, String.Empty, DummyEmailAddresses) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringInvalidEmailAddress));
        }

        [Test]
        public void AddEmails_GroupLabelDeafault_SubscriptionTypeU_NoData_ReturnJson()
        {
            InitilizeAddEmailsTests();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => new DataTable();
            var controller = new GroupController();
            var result = controller.AddEmails(String.Empty, SampleGroupID, SampleCustomerID, SubscriptionTypeU, String.Empty, DummyEmailAddresses) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringNoData));
        }

        [Test]
        public void AddEmails_GroupLabelDeafault_SubscriptionTypeU_NoEmailToImport_ReturnJson()
        {
            InitilizeAddEmailsTests();
            var controller = new GroupController();
            var result = controller.AddEmails(String.Empty, SampleGroupID, SampleCustomerID, SubscriptionTypeU, String.Empty, String.Empty) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringNoData));
        }

        [Test]
        public void AddEmails_GroupLabelDeafault_SubscriptionTypeU_NoGroup_ReturnJson()
        {
            InitilizeAddEmailsTests();
            ShimGroup.GetByGroupIDInt32User = (x1, x2) => null;
            var controller = new GroupController();
            var result = controller.AddEmails(String.Empty, SampleGroupID, SampleCustomerID, SubscriptionTypeU, String.Empty, String.Empty) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.AreEqual(jsonResult[1],ResultStringNoGroup);
        }

        private void InitilizeAddEmailsTests()
        {
            ShimHttpContext.CurrentGet = () => new ShimHttpContext { };
            ShimGroup.GetByCustomerIDInt32UserString = (x1, x2, x3) => new List<Group>
            {
                new Group
                {
                    GroupID = SampleDataTableGroupId
                }
            };

            ShimGroup.GetMasterSuppressionGroupInt32User = (x1, x2) => new Group {  };
            ShimGroup.GetByGroupIDInt32User = (x1, x2) => new Group
            {
                GroupID = SampleDataTableGroupId
            };

            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x1, x2, x3) => new EmailGroup { };
            ShimEmail.IsValidEmailAddressString = (p) => true;
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => new DataTable
                {
                    Columns = { SampleColumAction, SampleColumCounts },
                    Rows =
                    {
                        { ActionTypeT, ActionCount2 },
                        { ActionTypeT, ActionCount3 },
                        { ActionTypeI, ActionCount3 },
                        { ActionTypeU, ActionCount3 },
                        { ActionTypeD, ActionCount3 },
                        { ActionTypeS, ActionCount3 },
                        { ActionTypeM, ActionCount3 }
                    }
                };

            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
        }
    }
}
