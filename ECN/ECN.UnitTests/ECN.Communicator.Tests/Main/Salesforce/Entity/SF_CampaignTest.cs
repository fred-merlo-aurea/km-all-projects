﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_Campaign"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_CampaignTest
    {
        private List<SF_Campaign> campaigns;
        private static CultureInfo previousCulture;

        /// <summary>
        ///     Setup up <see cref="campaigns"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => campaigns = new List<SF_Campaign>
        {
            new SF_Campaign(),
            new SF_Campaign(),
            new SF_Campaign()
        };

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            TargetInvocationException exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_Campaign).CallMethod("ConvertJsonList", new object[] {null}));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyCampaignList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (List<SF_Campaign>) typeof(SF_Campaign).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyCampaignList()
        {
            // Arrange
            List<string> json = SfCampaignJsonWithNonNullValues;
            List<SF_Campaign> expectedList = SfCampaignListWithNonNullValues;

            // Act
            var result =
                (List<SF_Campaign>) typeof(SF_Campaign).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyCampaignList()
        {
            // Arrange
            List<string> json = SfCampaignJsonWithNullValues;

            List<SF_Campaign> expectedList = SfCampaignListWithNullValues;

            // Act
            var result =
                (List<SF_Campaign>) typeof(SF_Campaign).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        private static List<string> SfCampaignJsonWithNonNullValues => new List<string>
        {
            " \"IsActive\": \"true\" ,",
            " \"ActualCost\": \"200\" ,",
            " \"BudgetedCost\": \"100\" ,",
            " \"Id\": \"Id\" ,",
            " \"CampaignMemberRecordTypeId\": \"CampaignMemberRecordTypeId\" ,",
            " \"Name\": \"Name\" ,",
            " \"OwnerId\": \"OwnerId\" ,",
            " \"NumberOfConvertedLeads\": \"10\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"CreatedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"Description\": \"Description\" ,",
            " \"EndDate\": \"01/25/2018 15:50:31.000+0000\" ,",
            " \"ExpectedResponse\": \"40\" ,",
            " \"ExpectedRevenue\": \"1000.0\" ,",
            " \"LastActivityDate\": \"01/22/2018 14:55:30.000+0000\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"LastModifiedDate\": \"01/18/2018 14:55:30.000+0000\" ,",
            " \"LastReferencedDate\": \"01/20/2018 14:55:30.000+0000\" ,",
            " \"LastViewedDate\": \"01/21/2018 14:55:30.000+0000\" ,",
            " \"NumberSent\": \"30\" ,",
            " \"NumberOfOpportunities\": \"35\" ,",
            " \"NumberOfWonOpportunities\": \"38\" ,",
            " \"ParentId\": \"ParentId\" ,",
            " \"StartDate\": \"01/19/2018 14:55:30.000+0000\" ,",
            " \"Status\": \"Status\" ,",
            " \"SystemModstamp\": \"01/24/2018 12:51:30.000+0000\" ,",
            " \"NumberOfContacts\": \"230\" ,",
            " \"NumberOfLeads\": \"235\" ,",
            " \"NumberOfResponses\": \"120\" ,",
            " \"AmountAllOpportunities\": \"20000\" ,",
            " \"AmountWonOpportunities\": \"15000\" ,",
            " \"Type\": \"Type\" ,"
        };

        private static List<SF_Campaign> SfCampaignListWithNonNullValues => new List<SF_Campaign>
        {
            new SF_Campaign
            {
                IsActive = true,
                ActualCost = 200,
                BudgetedCost = 100,
                Id = "Id",
                CampaignMemberRecordTypeId = "CampaignMemberRecordTypeId",
                Name = "Name",
                OwnerId = "OwnerId",
                NumberOfConvertedLeads = 10,
                CreatedById = "CreatedById",
                CreatedDate = new DateTime(2018, 1, 26, 15, 52, 31),
                IsDeleted = true,
                Description = "Description",
                EndDate = new DateTime(2018, 1, 25, 15, 50, 31),
                ExpectedResponse = 40,
                ExpectedRevenue = 1000.0,
                LastActivityDate = new DateTime(2018, 1, 22, 14, 55, 30),
                LastModifiedById = "LastModifiedById",
                LastModifiedDate = new DateTime(2018, 1, 18, 14, 55, 30),
                LastReferencedDate = new DateTime(2018, 1, 20, 14, 55, 30),
                LastViewedDate = new DateTime(2018, 1, 21, 14, 55, 30),
                NumberSent = 30,
                NumberOfOpportunities = 35,
                NumberOfWonOpportunities = 38,
                ParentId = "ParentId",
                StartDate = new DateTime(2018, 1, 19, 14, 55, 30),
                Status = "Status",
                SystemModstamp = new DateTime(2018, 1, 24, 12, 51, 30),
                NumberOfContacts = 230,
                NumberOfLeads = 235,
                NumberOfResponses = 120,
                AmountAllOpportunities = 20000,
                AmountWonOpportunities = 15000,
                Type = "Type"
            }
        };

        private static List<string> SfCampaignJsonWithNullValues => new List<string>
        {
            " \"IsActive\": false",
            " \"ActualCost\": null",
            " \"ActualCost\": ",
            " \"BudgetedCost\": null",
            " \"BudgetedCost\": ",
            " \"Id\": null",
            " \"CampaignMemberRecordTypeId\": null",
            " \"Name\": null",
            " \"OwnerId\": null",
            " \"NumberOfConvertedLeads\": 0",
            " \"CreatedById\": null",
            " \"CreatedDate\": null",
            " \"IsDeleted\": false",
            " \"Description\": null",
            " \"EndDate\": null",
            " \"ExpectedResponse\": 0",
            " \"ExpectedRevenue\": null",
            " \"ExpectedRevenue\": Null",
            " \"LastActivityDate\": null",
            " \"LastModifiedById\": null",
            " \"LastModifiedDate\": null",
            " \"LastReferencedDate\": null",
            " \"LastViewedDate\": null",
            " \"NumberSent\": null",
            " \"NumberOfOpportunities\": 0",
            " \"NumberOfWonOpportunities\": 0",
            " \"ParentId\": null",
            " \"StartDate\": null",
            " \"Status\": null",
            " \"SystemModstamp\": null",
            " \"NumberOfContacts\": 0",
            " \"NumberOfLeads\": 0",
            " \"NumberOfResponses\": 0",
            " \"AmountAllOpportunities\": null",
            " \"AmountWonOpportunities\": null",
            " \"Type\": null"
        };

        private static List<SF_Campaign> SfCampaignListWithNullValues => new List<SF_Campaign>
        {
            new SF_Campaign
            {
                IsActive = false,
                ActualCost = 0.0,
                BudgetedCost = 0.0,
                Id = string.Empty,
                CampaignMemberRecordTypeId = string.Empty,
                Name = string.Empty,
                OwnerId = string.Empty,
                NumberOfConvertedLeads = 0,
                CreatedById = string.Empty,
                CreatedDate = new DateTime(1900, 1, 1),
                IsDeleted = false,
                Description = string.Empty,
                EndDate = new DateTime(1900, 1, 1),
                ExpectedResponse = 0,
                ExpectedRevenue = 0.0,
                LastActivityDate = new DateTime(1900, 1, 1),
                LastModifiedById = string.Empty,
                LastReferencedDate = new DateTime(1900, 1, 1),
                LastModifiedDate = new DateTime(1900, 1, 1),
                LastViewedDate = new DateTime(1900, 1, 1),
                NumberSent = 0,
                NumberOfOpportunities = 0,
                NumberOfWonOpportunities = 0,
                ParentId = string.Empty,
                StartDate = new DateTime(1900, 1, 1),
                Status = string.Empty,
                SystemModstamp = new DateTime(1900, 1, 1),
                NumberOfContacts = 0,
                NumberOfLeads = 0,
                NumberOfResponses = 0,
                AmountAllOpportunities = 0.0,
                AmountWonOpportunities = 0.0,
                Type = string.Empty
            }
        };
    }
}