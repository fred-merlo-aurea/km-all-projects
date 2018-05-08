using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class WizardTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int wizardID = -1;
            string wizardName = string.Empty;
            string emailSubject = string.Empty;
            string fromName = string.Empty;
            string fromEmail = string.Empty;
            string replyTo = string.Empty;
            int userID = -1;
            string statusCode = string.Empty;
            string cardcvNumber = string.Empty;
            string cardExpiration = string.Empty;
            string cardHolderName = string.Empty;
            string cardNumber = string.Empty;
            string cardType = string.Empty;
            string transactionNo = string.Empty;
            string multiGroupIDs = string.Empty;
            string suppressionGroupIDs = string.Empty;
            string blastType = string.Empty;
            string emailSubject2 = string.Empty;
            string dynamicFromEmail = string.Empty;
            string dynamicFromName = string.Empty;
            string dynamicReplyToEmail = string.Empty;        

            // Act
            Wizard wizard = new Wizard();    

            // Assert
            wizard.WizardID.ShouldBe(wizardID);
            wizard.IsNewMessage.ShouldBeNull();
            wizard.WizardName.ShouldBe(wizardName);
            wizard.EmailSubject.ShouldBe(emailSubject);
            wizard.FromName.ShouldBe(fromName);
            wizard.FromEmail.ShouldBe(fromEmail);
            wizard.ReplyTo.ShouldBe(replyTo);
            wizard.UserID.ShouldBe(userID);
            wizard.GroupID.ShouldBeNull();
            wizard.ContentID.ShouldBeNull();
            wizard.LayoutID.ShouldBeNull();
            wizard.BlastID.ShouldBeNull();
            wizard.FilterID.ShouldBeNull();
            wizard.StatusCode.ShouldBe(statusCode);
            wizard.CompletedStep.ShouldBeNull();
            wizard.CreatedDate.ShouldBeNull();
            wizard.UpdatedDate.ShouldBeNull();
            wizard.CardcvNumber.ShouldBe(cardcvNumber);
            wizard.CardExpiration.ShouldBe(cardExpiration);
            wizard.CardHolderName.ShouldBe(cardHolderName);
            wizard.CardNumber.ShouldBe(cardNumber);
            wizard.CardType.ShouldBe(cardType);
            wizard.TransactionNo.ShouldBe(transactionNo);
            wizard.MultiGroupIDs.ShouldBe(multiGroupIDs);
            wizard.SuppressionGroupIDs.ShouldBe(suppressionGroupIDs);
            wizard.PageWatchID.ShouldBeNull();
            wizard.BlastType.ShouldBe(blastType);
            wizard.EmailSubject2.ShouldBe(emailSubject2);
            wizard.ContentID2.ShouldBeNull();
            wizard.LayoutID2.ShouldBeNull();
            wizard.SampleWizardID.ShouldBeNull();
            wizard.RefBlastID.ShouldBeNull();
            wizard.CampaignID.ShouldBeNull();
            wizard.DynamicFromEmail.ShouldBe(dynamicFromEmail);
            wizard.DynamicFromName.ShouldBe(dynamicFromName);
            wizard.DynamicReplyToEmail.ShouldBe(dynamicReplyToEmail);
            wizard.CreatedUserID.ShouldBeNull();
            wizard.UpdatedUserID.ShouldBeNull();
            wizard.IsDeleted.ShouldBeNull();
        }
    }
}