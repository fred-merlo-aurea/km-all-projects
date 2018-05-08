using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.Common.Fakes;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.common.classes.Fakes;
using ecn.communicator.blastsmanager;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using MasterPages = ecn.communicator.MasterPages;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="reportsGraphical.LoadFormData"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ReportGraphicalTest : BasePageTests
    {
        private IDisposable _context;
        private reportsGraphical _page;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _page = new reportsGraphical();
            InitializePage(_page);

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SetupFakes()
        {
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                [ConnString] = TestConnectionString,
                [ChartsTempDirectory] = TempDirectory
            };

            ShimDataFunctions.GetDataTableString = script => GetGroupNameDataTable();

            SetupSqlFakes();
        }

        [Test]
        public void LoadFormData_WhenCalled_VerifyLabelValues()
        {
            // Arrange, Act
            _privateObject.Invoke(LoadFormData, BlastId);

            // Assert
            VerifyLabels();
        }

        private void VerifyLabels()
        {
            _page.ShouldSatisfyAllConditions(
                () => Get<Label>(Campaign).Text.ShouldBe(LayoutNameValue),
                () => Get<Label>(DesignCostLbl).Text.ShouldBe(DesignCostValue),
                () => Get<Label>(EmailFrom).Text.ShouldBe($"{EmailFromNameValue}<br>&lt;{EmailFromValue}&gt;"),
                () => Get<Label>(EmailSubject).Text.ShouldBe(EmailSubjectValue),
                () => Get<Label>(GroupTo).Text.ShouldBe(GroupNameValue),
                () => Get<Label>(Filter).Text.ShouldBe(FilterNameValue),
                () => Get<Label>(OtherCostLbl).Text.ShouldBe(OtherCostValue),
                () => Get<Label>(RoiSetupFees).Text.ShouldBe(string.Empty),
                () => Get<Label>(RoiTotalConversion).Text.ShouldBe(ConversionRate),
                () => Get<Label>(RoiTotalResponse).Text.ShouldBe(ConversionRate),
                () => Get<Label>(ResponsesLbl).Text.ShouldBe(ConversionRate),
                () => Get<Label>(EmailsSentLbl).Text.ShouldBe(EmailSentCount),
                () => Get<Label>(RoiEmailsSentLbl).Text.ShouldBe(EmailSentCount),
                () => Get<Label>(HardBouncesUnique).Text.ShouldBe(HsuBoucesUniqueRatio),
                () => Get<Label>(SoftBouncesUnique).Text.ShouldBe(HsuBoucesUniqueRatio),
                () => Get<Label>(UnknownBouncesUnique).Text.ShouldBe(HsuBoucesUniqueRatio),
                () => Get<Label>(BouncesUnique).Text.ShouldBe(BouncesUniqueRatio),
                () => Get<Label>(ClicksUnique).Text.ShouldBe(CofrsBouncesUniqueRatio),
                () => Get<Label>(OpensUnique).Text.ShouldBe(CofrsBouncesUniqueRatio),
                () => Get<Label>(ForwardsUnique).Text.ShouldBe(CofrsBouncesUniqueRatio),
                () => Get<Label>(ResendsUnique).Text.ShouldBe(CofrsBouncesUniqueRatio),
                () => Get<Label>(SubscribesUnique).Text.ShouldBe(CofrsBouncesUniqueRatio),
                () => Get<Label>(Successful).Text.ShouldBe(SuccessRatio),
                () => Get<Label>(HardBouncesPercentage).Text.ShouldBe(HsuBouncesPercentage),
                () => Get<Label>(SoftBouncesPercentage).Text.ShouldBe(HsuBouncesPercentage),
                () => Get<Label>(UnknownBouncesPercentage).Text.ShouldBe(HsuBouncesPercentage),
                () => Get<Label>(BouncesPercentage).Text.ShouldBe(BouncePercentage),
                () => Get<Label>(SuccessfulPercentage).Text.ShouldBe(SuccessPercentage),
                () => Get<Label>(ClicksPercentage).Text.ShouldBe(CofrsBouncesPercentage),
                () => Get<Label>(ForwardsPercentage).Text.ShouldBe(CofrsBouncesPercentage),
                () => Get<Label>(OpensPercentage).Text.ShouldBe(CofrsBouncesPercentage),
                () => Get<Label>(ResendsPercentage).Text.ShouldBe(CofrsBouncesPercentage),
                () => Get<Label>(SubscribesPercentage).Text.ShouldBe(CofrsBouncesPercentage),
                () => Get<Label>(PerEmailChargeLbl).Text.ShouldBe(PerEmailCharge),
                () => Get<Label>(PerClickLbl).Text.ShouldBe(PerClick),
                () => Get<Label>(PerResponseLbl).Text.ShouldBe(PerClick),
                () => Get<Label>(RoiPerClick).Text.ShouldBe(PerClick),
                () => Get<Label>(RoiPerResponse).Text.ShouldBe(PerClick),
                () => Get<Label>(InboundCostLbl).Text.ShouldBe(TotalInboundCost),
                () => Get<Label>(OutboundCostLbl).Text.ShouldBe(TotalOutboundCost),
                () => Get<Label>(RoiTotalInvestment).Text.ShouldBe(TotalCost),
                () => Get<Label>(TotalSetupLbl).Text.ShouldBe(TotalCost),
                () => Get<Label>(SendTime).Text.ShouldBe(SendTimeValue),
                () => Get<Label>(SetupSetupCostLbl).Text.ShouldBe(SetupCostValue));
        }

        private void SetupSqlFakes()
        {
            ShimSqlConnection.ConstructorString = (_, __) => { };
            ShimSqlCommand.ConstructorStringSqlConnection = (_, __, ___) => { };
            ShimSqlDataAdapter.ConstructorSqlCommand = (_, __) => { };
            ShimSqlConnection.AllInstances.Open = _ => { };
            ShimSqlConnection.AllInstances.Close = _ => { };

            ShimDbDataAdapter.AllInstances.FillDataSetString = (_, dataSet, srcTable) =>
            {
                if (srcTable.Equals(ProcGetGraphicalBlastReportData))
                {
                    dataSet.Tables.Add(GetGraphicalBlastReportDataTable());
                }
                else if (srcTable.Equals(ProcGetGraphicalBounceBlastReportData))
                {
                    dataSet.Tables.Add(GetGraphicalBlastBounceReportDataTable());
                }
                return -1;
            };

            ShimPage.AllInstances.MasterGet = _ => new MasterPages.Communicator();
            ShimECNSession.CurrentSession = () =>
            {
                ECNSession session = typeof(ECNSession).CreateInstance();
                session.CurrentUser = new User
                {
                    CustomerID = CustomerId
                };

                return session;
            };
        }
    }
}
