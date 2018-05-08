using ECN_Framework_BusinessLayer.Communicator;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_Common.Objects;
using System.Diagnostics;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class QuickTestBlastTest
    {
        [Test]
        public void Validate_WithNoGroupIdGroupNameAndInValidEmailAddressAndCampaignArchivedCampaignItemId_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase1();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID, paramObj.EmailPreview,
                paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID, paramObj.GroupID,
                paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignItemName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            foreach (var item in ecnExp.ErrorList)
            {
                Debug.WriteLine($"() => ecnExp.ErrorList.ShouldContain(\"{item.ErrorMessage}\"),");
            }
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldNotBeEmpty(),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Configuration requires you to specify a group name")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: test@test.com")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: sample@test.com")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Campaign is archived")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignID and CampaignItemID")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Group name cannot be empty")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Preview functionality not enabled")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message is archived")));
        }

        [Test]
        public void Validate_WithAllowAdhocEmailsFalseLayoutExistFalseHasServiceFeatures_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase2();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID, paramObj.EmailPreview,
                paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID, 
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID,paramObj.CampaignID, paramObj.CampaignItemName, 
                paramObj.CampaignItemID, paramObj.CampaignItemName,paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Configuration does not allow using Ad-Hoc emails")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: test@test.com")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: sample@test.com")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemName must be less than or equal to 50 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Configuration does not allow Ad Hoc emails")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message does not exist")));
        }

        [Test]
        public void Validate_WithCreateAutoGroupAndNoGroupName_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase3();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                 paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo,
                 paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID, paramObj.GroupID, 
                 paramObj.GroupName, paramObj.BaseChannelID,paramObj.CampaignID, paramObj.CampaignItemName, paramObj.CampaignItemID,
                 paramObj.CampaignItemName,paramObj.CurrentUser, paramObj.QTB));
            
            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignItemName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemName must be less than or equal to 50 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply GroupID and GroupName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithGroupIdAndEmailAddress_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase4();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply GroupID and EmailAddresses")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithNoGroupIdAndEmailAddress_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase5();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Addresses cannot be empty")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Group not selected")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithWithAutoCreateGroupFalseAndNotExists_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase6();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Selected group does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithWithAutoCreateGroupFalseAndGroupisNotArchived_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase7();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Selected group is archived")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithGroupIdGreaterThanZero_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase8();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Selected group exceeds the test blast limit")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithAutoCreateGroupTrueAndGroupName_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase9();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Configuration does not allow specifying a group name")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Group not selected")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }

        [Test]
        public void Validate_WithEmailTokenCountGreaterThanBlastLimit_ThrowsECNExceptionWithErrorList()
        {
            // Arrange
            var paramObj = GetValidateTestCase10();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => QuickTestBlast.Validate(paramObj.EmailsToAdd, paramObj.LayoutID,
                paramObj.EmailPreview, paramObj.EmailFrom, paramObj.ReplyTo, paramObj.FromName, paramObj.EmailSubject, paramObj.CustomerID,
                paramObj.GroupID, paramObj.GroupName, paramObj.BaseChannelID, paramObj.CampaignID, paramObj.CampaignName, paramObj.CampaignItemID,
                paramObj.CampaignItemName, paramObj.CurrentUser, paramObj.QTB));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldNotBeEmpty();
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Emails count exceeds test blast limit")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: test@test.com")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Email Address: sample@test.com")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Cannot supply CampaignItemID and CampaignName")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CampaignItemID does not exist")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UT Error")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incomplete Envelope Information")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmail is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyTo is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject cannot be more than 255 characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Message Layout not selected")));
        }
    }
}
