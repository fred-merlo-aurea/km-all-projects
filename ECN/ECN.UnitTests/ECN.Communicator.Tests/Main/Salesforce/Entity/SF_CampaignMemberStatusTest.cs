using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Moq;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.Salesforce.Entity;
using ECN.TestHelpers;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Interfaces;
using SF_Utilities = ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    /// Unit test for <see cref="SF_CampaignMemberStatus"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_CampaignMemberStatusTest
    {
        private const string DefaultCulture = "en-US";

        private List<SF_CampaignMemberStatus> _memberStatuses;
        private DateTime _lastModifiedDate;
        private static CultureInfo _previousCulture;

        /// <summary>
        ///     Setup up <see cref="_memberStatuses"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _memberStatuses = new List<SF_CampaignMemberStatus>
            {
                new SF_CampaignMemberStatus(),
                new SF_CampaignMemberStatus(),
                new SF_CampaignMemberStatus()
            };

            _lastModifiedDate = new DateTime(2018, 1, 18, 14, 55, 30);
        }

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DefaultCulture);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            var exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_CampaignMemberStatus).CallMethod("ConvertJsonList", new object[] { null }));

            // Assert
            exp.InnerException.ShouldBeOfType<NullReferenceException>();
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyMemberList()
        {
            // Arrange
            var sfMemberStatus = new List<string>();

            // Act
            var result =
                typeof(SF_CampaignMemberStatus).CallMethod("ConvertJsonList", new object[] { sfMemberStatus }) as
                    List<SF_CampaignMemberStatus>;

            // Assert
            result.ShouldBeEmpty();
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyMemberStatusList()
        {
            // Arrange
            _lastModifiedDate = new DateTime(0001, 1, 1);
            var sfMemberStatus = SfMemberStatusJsonWithNonNullValues;
            var expectedList = SfMemberStatusListWithNonNullValues;

            // Act
            var result =
                typeof(SF_CampaignMemberStatus).CallMethod("ConvertJsonList", new object[] { sfMemberStatus }) as
                    List<SF_CampaignMemberStatus>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(1),
                () => result.IsListContentMatched(expectedList).ShouldBeTrue());
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyMemberStatusList()
        {
            // Arrange
            _lastModifiedDate = new DateTime(0001, 1, 1);
            var sfMemberStatus = SfMemberStatusJsonWithNullValues;
            var expectedList = GetSfMemberStatusListWithNullValues();

            // Act
            var result =
                typeof(SF_CampaignMemberStatus).CallMethod("ConvertJsonList", new object[] { sfMemberStatus }) as
                    List<SF_CampaignMemberStatus>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(1),
                () => result.IsListContentMatched(expectedList).ShouldBeTrue());
        }

        [Test]
        public void Insert_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            var token = "accessToken";
            var entity = new SF_CampaignMemberStatus();
            var sfUtilities = new Mock<ISFUtilities>();
            sfUtilities.Setup(x => x.Insert(token, entity, SF_Utilities.SFObject.CampaignMemberStatus)).Returns(true);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_CampaignMemberStatus.Insert(token, entity);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Insert(
                    It.Is<string>(t => t == token),
                    It.Is<SalesForceBase>(sf => sf == entity),
                    It.Is<SF_Utilities.SFObject>(sf => sf == SF_Utilities.SFObject.CampaignMemberStatus)),
                Times.Once);
        }

        private static List<string> SfMemberStatusJsonWithNonNullValues => new List<string>
        {
            " \"Id\": \"Id\" ,",
            " \"CampaignId\": \"CampaignId\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"CreatedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"HasResponded\": \"true\" ,",
            " \"IsDefault\": \"true\" ,",
            " \"SortOrder\": \"1\" ,",
            " \"Label\": \"Label\" ,",
            " \"SystemModstamp\": \"01/24/2018 12:51:30.000+0000\" ,",
            " \"LastModifiedDate\": \"01/18/2018 14:55:30.000+0000\" ,"
        };

        private List<SF_CampaignMemberStatus> SfMemberStatusListWithNonNullValues => new List<SF_CampaignMemberStatus>
        {
            new SF_CampaignMemberStatus
            {
                Id = "Id",
                CampaignId = "CampaignId",
                CreatedById = "CreatedById",
                CreatedDate = new DateTime(2018, 1, 26, 15, 52, 31),
                IsDeleted = true,
                LastModifiedById = "LastModifiedById",
                LastModifiedDate = _lastModifiedDate,
                SystemModstamp = new DateTime(2018, 1, 24, 12, 51, 30),
                Label = "Label",
                HasResponded = true,
                IsDefault = true,
                SortOrder = 1
            }
        };

        private static List<string> SfMemberStatusJsonWithNullValues => new List<string>
        {
            " \"Id\": null",
            " \"CampaignId\":  null",
            " \"CreatedById\":  null",
            " \"CreatedDate\":  \"01/01/1900 00:00:00\"",
            " \"IsDeleted\":  false",
            " \"LastModifiedById\":  null",
            " \"HasResponded\":  false",
            " \"IsDefault\":  false",
            " \"SortOrder\":  0",
            " \"Label\":  null",
            " \"SystemModstamp\":  \"01/01/1900 00:00:00\"",
            " \"LastModifiedDate\":  \"01/01/1900 00:00:00\""
        };

        private List<SF_CampaignMemberStatus> GetSfMemberStatusListWithNullValues()
        {
            return new List<SF_CampaignMemberStatus>
            {
                new SF_CampaignMemberStatus
                {
                    Id = "null",
                    CampaignId = string.Empty,
                    CreatedById = string.Empty,
                    CreatedDate = new DateTime(1900, 1, 1),
                    IsDeleted = false,
                    LastModifiedById = string.Empty,
                    LastModifiedDate = _lastModifiedDate,
                    HasResponded = false,
                    SystemModstamp = new DateTime(1900, 1, 1),
                    SortOrder = 0,
                    IsDefault = false,
                    Label = string.Empty
                }
            };
        }
    }
}