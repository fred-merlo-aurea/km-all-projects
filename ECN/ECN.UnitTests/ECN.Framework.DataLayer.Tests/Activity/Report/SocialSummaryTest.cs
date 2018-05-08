using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using ECN_Framework_DataLayer.Activity.Report;
using ECN_Framework_DataLayer.Fakes;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests.Activity.Report
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class SocialSummaryTest
    {
        private const string SampleImagePath = "SampleImagePath";
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string SocialPageName = "social.aspx";
        private const string ReportImagePathColumn = "ReportImagePath";
        private const string ShareColumn = "Share";
        private const string ClickColumn = "Click";
        private const string SocialMediaIDColumn = "SocialMediaID";

        private IDisposable _shimObject;
        private SocialSummary _testEntity;
        private string _sqlCommandExecuted;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _testEntity = new SocialSummary();
            _sqlCommandExecuted = string.Empty;
        }

        [TearDown]
        public void CleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetSocialSummaryByBlastID_WithBlastIdAndCustomerId_ReturnsSocialSummaryList()
        {
            // Arrange
            const int BlastId = 1;
            const int CustomerId = 1;
            SetFakesForSocialSummary();

            // Act
            var socialSummaryList = SocialSummary.GetSocialSummaryByBlastID(blastID: BlastId, customerID: CustomerId);

            // Assert
            socialSummaryList.ShouldSatisfyAllConditions(
                () => _sqlCommandExecuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExecuted.ShouldContain("rpt_BlastActivitySocial_SocialSummary"),
                () => socialSummaryList.ShouldNotBeEmpty(),
                () => socialSummaryList.Count.ShouldBe(1),
                () => socialSummaryList[0].ID.ShouldBe(BlastId),
                () => socialSummaryList[0].IsBlastGroup.Value.ShouldBeFalse(),
                () => socialSummaryList[0].ReportImagePath.ShouldBe(SampleImagePath),
                () => socialSummaryList[0].Share.ShouldBe(1),
                () => socialSummaryList[0].Click.ShouldBe(1),
                () => socialSummaryList[0].ReportPath.ShouldContain(SocialPageName));
        }

        [Test]
        public void GetSocialSummaryByCampaignItemID_WithCampaignItemIdAndCustomerId_ReturnsSocialSummaryList()
        {
            // Arrange
            const int CampaignItemId = 1;
            const int CustomerId = 1;
            SetFakesForSocialSummary();
            var dataTable = GetDataTable();
            var duplicateRow = dataTable.Rows[0];
            dataTable.ImportRow(duplicateRow);
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, conn) =>
            {
                _sqlCommandExecuted = cmd.CommandText;
                return dataTable;
            };

            // Act
            var socialSummaryList = SocialSummary.GetSocialSummaryByCampaignItemID(campaignItemID: CampaignItemId, customerID: CustomerId);

            // Assert
            socialSummaryList.ShouldSatisfyAllConditions(
                () => _sqlCommandExecuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExecuted.ShouldContain("rpt_BlastActivitySocial_SocialSummary"),
                () => socialSummaryList.ShouldNotBeEmpty(),
                () => socialSummaryList.Count.ShouldBe(1),
                () => socialSummaryList[0].ID.ShouldBe(CampaignItemId),
                () => socialSummaryList[0].IsBlastGroup.Value.ShouldBeTrue(),
                () => socialSummaryList[0].ReportImagePath.ShouldBe(SampleImagePath),
                () => socialSummaryList[0].Share.ShouldBe(2),
                () => socialSummaryList[0].Click.ShouldBe(2),
                () => socialSummaryList[0].ReportPath.ShouldContain(SocialPageName));
        }

        private void SetFakesForSocialSummary()
        {
            var settings = new NameValueCollection();
            settings.Add(KMCommonApplicationKey, "1");
            ShimConfigurationManager.AppSettingsGet = () => settings;

            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, conn) =>
            {
               _sqlCommandExecuted = cmd.CommandText;
               return GetDataTable();
            };

            ShimEncryption.GetCurrentByApplicationIDInt32 = _ => new Encryption(true);
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(ReportImagePathColumn, typeof(string));
            dataTable.Columns.Add(ShareColumn, typeof(int));
            dataTable.Columns.Add(ClickColumn, typeof(int));
            dataTable.Columns.Add(SocialMediaIDColumn, typeof(int));
            
            var row = dataTable.NewRow();
            row[ReportImagePathColumn] = SampleImagePath;
            row[ShareColumn] = 1;
            row[ClickColumn] = 1;
            row[SocialMediaIDColumn] = 1;
            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}
