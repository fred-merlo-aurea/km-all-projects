using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_Entities.Salesforce.Convertors;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity.Converters
{
    [TestFixture]
    public class ContactConverterTest
    {
        [Test]
        public void ConvertJsonItem_SetupJsonWithNonNullValues_VerifyContact()
        {
            // Arrange
            var json = SfContactJsonWithNonNullValues;
            var converter = new ContactConverter();
            var expected = SfContactListWithNonNullValues.First();

            // Act
            var result = converter.Convert<SF_Contact>(json).FirstOrDefault();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsContentMatched(expected));
        }

        [Test]
        public void ConvertJsonItem_SetupJsonWithNullValues_VerifyContact()
        {
            // Arrange
            var json = SfContactJsonWithNullValues;
            var converter = new ContactConverter();
            var expected = SfContactListWithNullValues.First();

            // Act
            var result = converter.Convert<SF_Contact>(json).FirstOrDefault();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(), 
                () => result.IsContentMatched(expected));
        }

        private static List<string> SfContactJsonWithNonNullValues => new List<string>
        {
            " \"done\": \"true\" ,",
            " \"nextRecordsUrl\": \"nextRecordsUrl\" ,",
            " \"Id\": \"Id\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"MasterRecordId\": \"MasterRecordId\" ,",
            " \"AccountId\": \"AccountId\" ,",
            " \"Email\": \"Email\" ,",
            " \"Fax\": \"Fax\" ,",
            " \"FirstName\": \"FirstName\" ,",
            " \"Salutation\": \"Salutation\" ,",
            " \"Name\": \"Name\" ,",
            " \"OtherCity\": \"OtherCity\" ,",
            " \"OtherState\": \"OtherState\" ,",
            " \"OtherCountry\": \"OtherCountry\" ,",
            " \"OtherPostalCode\": \"OtherPostalCode\" ,",
            " \"OtherStreet\": \"OtherStreet\" ,",
            " \"OtherLatitude\": \"10.0\" ,",
            " \"OtherLongitude\": \"20.0\" ,",
            " \"AssistantName\": \"AssistantName\" ,",
            " \"AssistantPhone\": \"AssistantPhone\" ,",
            " \"LeadSource\": \"LeadSource\" ,",
            " \"BirthDate\": \"01/25/2018 15:50:31.000+0000\" ,",
            " \"Description\": \"Description\" ,",
            " \"OwnerId\": \"OwnerId\" ,",
            " \"HomePhone\": \"HomePhone\" ,",
            " \"LastName\": \"LastName\" ,",
            " \"MailingCity\": \"MailingCity\" ,",
            " \"MailingState\": \"MailingState\" ,",
            " \"MailingCountry\": \"MailingCountry\" ,",
            " \"MailingPostalCode\": \"MailingPostalCode\" ,",
            " \"MailingStreet\": \"MailingStreet\" ,",
            " \"MailingLatitude\": \"35.00\" ,",
            " \"MailingLongitude\": \"-40.00\" ,",
            " \"MobilePhone\": \"MobilePhone\" ,",
            " \"Phone\": \"Phone\" ,",
            " \"Title\": \"Title\" ,",
            " \"Department\": \"Department\" ,",
            " \"HasOptedOutOfEmail\": \"true\" ,",
            " \"DoNotCall\": \"true\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"CreatedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"SystemModstamp\": \"01/15/2018 15:52:31.000+0000\" ,",
            " \"LastActivityDate\": \"01/22/2018 14:55:30.000+0000\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"LastModifiedDate\": \"01/18/2018 14:55:30.000+0000\" ,",
            " \"LastCURequestDate\": \"01/20/2018 14:55:30.000+0000\" ,",
            " \"LastViewedDate\": \"01/21/2018 14:55:30.000+0000\" ,",
            " \"LastReferencedDate\": \"01/17/2018 14:55:30.000+0000\" ,",
            " \"EmailBounceReason\": \"EmailBounceReason\" ,",
            " \"EmailBounceDate\": \"01/19/2018 14:55:30.000+0000\" ,",
            " \"JigsawContactId\": \"JigsawContactId\" ,",
            " \"Master_Suppressed__c\": \"true\" ,"
        };

        private static List<SF_Contact> SfContactListWithNonNullValues => new List<SF_Contact>
        {
            new SF_Contact
            {
                Id = "Id",
                IsDeleted = true,
                MasterRecordId = "MasterRecordId",
                AccountId = "AccountId",
                Email = "Email",
                Fax = "Fax",
                FirstName = "FirstName",
                Salutation = "Salutation",
                Name = "Name",
                OtherCity = "OtherCity",
                OtherState = "OtherState",
                OtherCountry = "OtherCountry",
                OtherPostalCode = "OtherPostalCode",
                OtherStreet = "OtherStreet",
                OtherLatitude = 10.0,
                OtherLongitude = 20.0,
                AssistantName = "AssistantName",
                AssistantPhone = "AssistantPhone",
                LeadSource = "LeadSource",
                BirthDate = new DateTime(2018, 1, 25, 15, 50, 31),
                Description = "Description",
                OwnerId = "OwnerId",
                HomePhone = "HomePhone",
                LastName = "LastName",
                MailingCity = "MailingCity",
                MailingState = "MailingState",
                MailingCountry = "MailingCountry",
                MailingPostalCode = "MailingPostalCode",
                MailingStreet = "MailingStreet",
                MailingLatitude = 35.00,
                MailingLongitude = -40.00,
                MobilePhone = "MobilePhone",
                Phone = "Phone",
                Title = "Title",
                Department = "Department",
                HasOptedOutOfEmail = true,
                DoNotCall = true,
                CreatedById = "CreatedById",
                CreatedDate = new DateTime(2018, 1, 26, 15, 52, 31),
                SystemModstamp = new DateTime(2018, 1, 15, 15, 52, 31),
                LastActivityDate = new DateTime(2018, 1, 22, 14, 55, 30),
                LastModifiedById = "LastModifiedById",
                LastModifiedDate = new DateTime(2018, 1, 18, 14, 55, 30),
                LastCURequestDate = new DateTime(2018, 1, 20, 14, 55, 30),
                LastViewedDate = new DateTime(2018, 1, 21, 14, 55, 30),
                LastReferencedDate = new DateTime(2018, 1, 17, 14, 55, 30),
                EmailBouncedReason = "EmailBounceReason",
                EmailBouncedDate = new DateTime(2018, 1, 19, 14, 55, 30),
                JigsawContactId = "JigsawContactId",
                Master_Suppressed__c = true
            }
        };

        private List<string> SfContactJsonWithNullValues => new List<string>
        {
            " \"done\": null",
            " \"nextRecordsUrl\": null",
            " \"Id\": null",
            " \"IsDeleted\": false",
            " \"MasterRecordId\": null",
            " \"AccountId\": null",
            " \"Email\": null",
            " \"Fax\": null",
            " \"FirstName\": null",
            " \"Salutation\": null",
            " \"Name\": null",
            " \"OtherCity\": null",
            " \"OtherState\": null",
            " \"OtherCountry\": null",
            " \"OtherPostalCode\": null",
            " \"OtherStreet\": null",
            " \"OtherLatitude\": null",
            " \"OtherLongitude\": null",
            " \"AssistantName\": null",
            " \"AssistantPhone\": null",
            " \"LeadSource\": null",
            " \"BirthDate\": null",
            " \"Description\": null",
            " \"OwnerId\": null",
            " \"HomePhone\": null",
            " \"LastName\": null",
            " \"MailingCity\": null",
            " \"MailingState\": null",
            " \"MailingCountry\": null",
            " \"MailingPostalCode\": null",
            " \"MailingStreet\": null",
            " \"MailingLatitude\": null",
            " \"MailingLongitude\": null",
            " \"MobilePhone\": null",
            " \"Phone\": null",
            " \"Title\": null",
            " \"Department\": null",
            " \"HasOptedOutOfEmail\": false",
            " \"DoNotCall\": false",
            " \"CreatedById\": null",
            " \"CreatedDate\": null",
            " \"SystemModstamp\": null",
            " \"LastActivityDate\": null",
            " \"LastModifiedById\": null",
            " \"LastModifiedDate\": null",
            " \"LastCURequestDate\": null",
            " \"LastViewedDate\": null",
            " \"LastReferencedDate\": null",
            " \"EmailBounceReason\": null",
            " \"EmailBounceDate\": null",
            " \"JigsawContactId\": null",
            " \"Master_Suppressed__c\": false"
        };

        private static List<SF_Contact> SfContactListWithNullValues => new List<SF_Contact>
        {
            new SF_Contact
            {
                Id = "null",
                IsDeleted = false,
                MasterRecordId = string.Empty,
                AccountId = string.Empty,
                Email = string.Empty,
                Fax = string.Empty,
                FirstName = string.Empty,
                Salutation = string.Empty,
                Name = string.Empty,
                OtherCity = string.Empty,
                OtherState = string.Empty,
                OtherCountry = string.Empty,
                OtherPostalCode = string.Empty,
                OtherStreet = string.Empty,
                OtherLatitude = 0.0,
                OtherLongitude = 0.0,
                AssistantName = string.Empty,
                AssistantPhone = string.Empty,
                LeadSource = string.Empty,
                BirthDate = new DateTime(1900, 1, 1),
                Description = string.Empty,
                OwnerId = string.Empty,
                HomePhone = string.Empty,
                LastName = string.Empty,
                MailingCity = string.Empty,
                MailingState = string.Empty,
                MailingCountry = string.Empty,
                MailingPostalCode = string.Empty,
                MailingStreet = string.Empty,
                MailingLatitude = 0.0,
                MailingLongitude = 0.0,
                MobilePhone = string.Empty,
                Phone = string.Empty,
                Title = string.Empty,
                Department = string.Empty,
                HasOptedOutOfEmail = false,
                DoNotCall = false,
                CreatedById = string.Empty,
                CreatedDate = new DateTime(1900, 1, 1),
                SystemModstamp = new DateTime(1900, 1, 1),
                LastActivityDate = new DateTime(1900, 1, 1),
                LastModifiedById = string.Empty,
                LastModifiedDate = new DateTime(1900, 1, 1),
                LastCURequestDate = new DateTime(1900, 1, 1),
                LastViewedDate = new DateTime(1900, 1, 1),
                LastReferencedDate = new DateTime(1900, 1, 1),
                EmailBouncedReason = string.Empty,
                EmailBouncedDate = new DateTime(1900, 1, 1),
                JigsawContactId = string.Empty,
                Master_Suppressed__c = false,
            }
        };
    }
}
