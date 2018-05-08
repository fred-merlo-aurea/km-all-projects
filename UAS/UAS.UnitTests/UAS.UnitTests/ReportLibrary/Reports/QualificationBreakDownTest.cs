using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using FrameworkServices.Fakes;
using KMPlatform.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ReportLibrary.Reports.Fakes;
using FrameworkObject = FrameworkUAS.Object;
using FrameworkObjectFakes = FrameworkUAS.Object.Fakes;
using FrameworkService = FrameworkUAS.Service;
using IReports = UAD_WS.Interface.IReports;
using QualificationBreakDown = ReportLibrary.Reports.QualificationBreakDown;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    /// <summary>
    ///     Unit tests for <see cref="QualificationBreakDown"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class QualificationBreakDownTest
    {
        private IDisposable _shimContext;
        private PrivateObject _qualificationBreakDownPrivateObject;
        private QualificationBreakDown _qualificationBreakDownInstance;
        private ShimQualificationBreakDown _shimQualificationBreakDown;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            ShimUser.AllInstances.CurrentClientGet = (u) => new ShimClient();
            ShimServiceClient<IReports>.ConstructorString = (client, endPoint) => { };
            FrameworkObjectFakes.ShimAppData.Constructor = (app) => { };
            FrameworkObjectFakes.ShimAppData.ConstructorString = (app, parent) => { };
            FrameworkObjectFakes.ShimAppData.myAppDataGet = () => new FrameworkObjectFakes.ShimAppData();
            FrameworkObjectFakes.ShimAppData.AllInstances.AuthorizedUserGet = (app) => new FrameworkObjectFakes.ShimUserAuthorization(new FrameworkObject.UserAuthorization());
            InitMocks();
            _qualificationBreakDownInstance = new QualificationBreakDown();
            _shimQualificationBreakDown = new ShimQualificationBreakDown(_qualificationBreakDownInstance);
            _qualificationBreakDownPrivateObject = new PrivateObject(_qualificationBreakDownInstance);
        }

        private void InitMocks()
        {
            var ireportMock = new Mock<IReports>();
            ireportMock.Setup(x => x.GetIssueDates(It.IsAny<Guid>(), It.IsAny<KMPlatform.Object.ClientConnections>(), It.IsAny<int>()))
                .Returns(new FrameworkService.Response<DataTable>(CreateIssueDatesDataTable()));
            ireportMock.Setup(x => x.SelectQSourceBreakdown(
                It.IsAny<Guid>(), 
                It.IsAny<KMPlatform.Object.ClientConnections>(), 
                It.IsAny<int>(), 
                It.IsAny<bool>(), 
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<int>())).Returns(new FrameworkService.Response<DataTable>(CreateSourceBreakdDownTable()));
            ShimServiceClient<IReports>.AllInstances.ProxyGet = (client) => ireportMock.Object;
        }

        private DataTable CreateIssueDatesDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("MonthDayStartDate");
            dataTable.Columns.Add("MonthDayEndDate");
            var row = dataTable.NewRow();
            row[0] = DateTime.Now.ToString();
            row[1] = DateTime.Now.ToString();
            dataTable.Rows.Add(row);
            return dataTable;
        }

        private DataTable CreateSourceBreakdDownTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Filters");
            dataTable.Columns.Add("AdHocFilters");
            dataTable.Columns.Add("ProductID");
            dataTable.Columns.Add("IssueID");
            dataTable.Columns.Add("IncludeAddRemove");
            var row = dataTable.NewRow();
            row[0] = "Filters";
            row[1] = "AdHocFilters";
            row[2] = "ProductID";
            row[3] = "IssueID";
            row[4] = "IncludeAddRemove";
            dataTable.Rows.Add(row);
            return dataTable;
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }
    }
}
