using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SmartFormsHistoryTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int smartFormID = -1;
            string subscriptionGroupIDs = string.Empty;
            string smartFormName = string.Empty;
            string smartFormHTML = string.Empty;
            string smartFormFields = string.Empty;
            string response_UserMsgSubject = string.Empty;
            string response_UserMsgBody = string.Empty;
            string response_UserScreen = string.Empty;
            string response_FromEmail = string.Empty;
            string response_AdminEmail = string.Empty;
            string response_AdminMsgSubject = string.Empty;
            string response_AdminMsgBody = string.Empty;
            string type = string.Empty;        

            // Act
            SmartFormsHistory smartFormsHistory = new SmartFormsHistory();    

            // Assert
            smartFormsHistory.SmartFormID.ShouldBe(smartFormID);
            smartFormsHistory.GroupID.ShouldBeNull();
            smartFormsHistory.SubscriptionGroupIDs.ShouldBe(subscriptionGroupIDs);
            smartFormsHistory.SmartFormName.ShouldBe(smartFormName);
            smartFormsHistory.SmartFormHTML.ShouldBe(smartFormHTML);
            smartFormsHistory.SmartFormFields.ShouldBe(smartFormFields);
            smartFormsHistory.Response_UserMsgSubject.ShouldBe(response_UserMsgSubject);
            smartFormsHistory.Response_UserMsgBody.ShouldBe(response_UserMsgBody);
            smartFormsHistory.Response_UserScreen.ShouldBe(response_UserScreen);
            smartFormsHistory.Response_FromEmail.ShouldBe(response_FromEmail);
            smartFormsHistory.Response_AdminEmail.ShouldBe(response_AdminEmail);
            smartFormsHistory.Response_AdminMsgSubject.ShouldBe(response_AdminMsgSubject);
            smartFormsHistory.Response_AdminMsgBody.ShouldBe(response_AdminMsgBody);
            smartFormsHistory.Type.ShouldBe(type);
            smartFormsHistory.DoubleOptIn.ShouldBeNull();
            smartFormsHistory.CreatedUserID.ShouldBeNull();
            smartFormsHistory.CreatedDate.ShouldBeNull();
            smartFormsHistory.UpdatedUserID.ShouldBeNull();
            smartFormsHistory.UpdatedDate.ShouldBeNull();
            smartFormsHistory.IsDeleted.ShouldBeNull();
            smartFormsHistory.CustomerID.ShouldBeNull();
        }
    }
}