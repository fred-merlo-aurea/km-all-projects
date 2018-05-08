using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class PublicationTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int publicationID = -1;
            string publicationName = string.Empty;
            string publicationType = string.Empty;
            string publicationCode = string.Empty;
            int customerID = -1;
            string contactAddress1 = string.Empty;
            string contactAddress2 = string.Empty;
            string contactEmail = string.Empty;
            string contactPhone = string.Empty;
            string logoLink = string.Empty;
            string logoURL = string.Empty;
            string contactFormLink = string.Empty;
            string subscriptionFormLink = string.Empty;        

            // Act
            Publication publication = new Publication();    

            // Assert
            publication.PublicationID.ShouldBe(publicationID);
            publication.PublicationName.ShouldBe(publicationName);
            publication.PublicationType.ShouldBe(publicationType);
            publication.PublicationCode.ShouldBe(publicationCode);
            publication.CustomerID.ShouldBe(customerID);
            publication.GroupID.ShouldBeNull();
            publication.Active.ShouldBeTrue();
            publication.CreatedDate.ShouldBeNull();
            publication.UpdatedDate.ShouldBeNull();
            publication.ContactAddress1.ShouldBe(contactAddress1);
            publication.ContactAddress2.ShouldBe(contactAddress2);
            publication.ContactEmail.ShouldBe(contactEmail);
            publication.ContactPhone.ShouldBe(contactPhone);
            publication.EnableSubscription.ShouldBeNull();
            publication.LogoLink.ShouldBe(logoLink);
            publication.LogoURL.ShouldBe(logoURL);
            publication.SubscriptionOption.ShouldBeNull();
            publication.CreatedUserID.ShouldBeNull();
            publication.ContactFormLink.ShouldBe(contactFormLink);
            publication.SubscriptionFormLink.ShouldBe(subscriptionFormLink);
            publication.CategoryID.ShouldBeNull();
            publication.Circulation.ShouldBeNull();
            publication.FrequencyID.ShouldBeNull();
            publication.UpdatedUserID.ShouldBeNull();
            publication.IsDeleted.ShouldBeNull();
        }
    }
}