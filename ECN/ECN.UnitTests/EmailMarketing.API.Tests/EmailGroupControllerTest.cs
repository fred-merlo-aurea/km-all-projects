using System;
using System.Collections.Generic;
using System.Data;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Models.EmailGroup;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests
{
    [TestFixture]
    public class EmailGroupControllerTest
    {
        private IDisposable _shrimContext;

        [SetUp]
        public void Setup()
        {
            _shrimContext = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shrimContext.Dispose();
        }

        [Test]
        public void ManageSubscribersWithProfile_WhenEmailListNotEmpty_ReturnsFilledImportResults()
        {         
            // Arrange
            var profileList = new ManageProfile()
            {
                Profiles = new List<Profile>() { new Profile() }
            };

            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (a, b, c, d, q, w, e, r, t, y) =>
                {
                    return CreateActionsTable();
                };

            // Act         
            var importResults = EmailGroupController.ManageSubscribersWithProfile(0, 0, profileList, new KMPlatform.Entity.User());

            // Assert
            AssertThatDataTableMappedToImportResults(importResults);            
        }

        [Test]
        public void ManageSubscribersWithProfileWithDupes_WhenEmailListNotEmpty_ReturnsFilledImportResults()
        {
            // Arrange
            var profileList = new ManageProfile()
            {
                Profiles = new List<Profile>() { new Profile() }
            };

            ShimEmailGroup.ImportEmailsWithDupesUserInt32StringStringStringStringBooleanStringBooleanString =
                    (a, b, c, d, q, w, e, r, t, y) =>
                    {
                        return CreateActionsTable();
                    };

            // Act         
            var importResults = EmailGroupController.ManageSubscribersWithProfileWithDupes(0, 0, profileList, new KMPlatform.Entity.User(), string.Empty);

            // Assert
            AssertThatDataTableMappedToImportResults(importResults);
        }

        [Test]
        public void ManageSubscribers_WhenEmailListNotEmpty_ReturnsFilledImportResults()
        {
            // Arrange
            var profileList = new UpdateEmailAddressForGroup();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (a, b, c, d, q, w, e, r, t, y) =>
                {
                    return CreateActionsTable();
                };

            // Act         
            var importResults = EmailGroupController.ManageSubscribers(0, 0, profileList, new KMPlatform.Entity.User());

            // Assert
            AssertThatDataTableMappedToImportResults(importResults);            
        }

        private static void AssertThatDataTableMappedToImportResults(ImportResult importResults)
        {
            importResults.ShouldSatisfyAllConditions(
                  () => importResults.New.ShouldBe(1),
                  () => importResults.Updated.ShouldBe(2),
                  () => importResults.MasterSuppressed.ShouldBe(3),
                  () => importResults.Duplicate.ShouldBe(4),
                  () => importResults.Skipped.ShouldBe(5),
                  () => importResults.Total.ShouldBe(6));
        }

        private static DataTable CreateActionsTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Action");
            dataTable.Columns.Add("Counts");

            dataTable.Rows.Add("I", 1);
            dataTable.Rows.Add("U", 2);
            dataTable.Rows.Add("M", 3);
            dataTable.Rows.Add("D", 4);
            dataTable.Rows.Add("S", 5);
            dataTable.Rows.Add("T", 6);
            return dataTable;
        }
    }
}
