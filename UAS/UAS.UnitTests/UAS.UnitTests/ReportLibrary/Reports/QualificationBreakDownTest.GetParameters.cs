using System;
using System.Collections.Generic;
using System.Data;
using FrameworkServices.Fakes;
using KMPlatform.DataAccess.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Reporting.Processing.Fakes;
using UAS_WS.Interface;
using KmPlatformBusinessFakes = KMPlatform.BusinessLogic.Fakes;
using ReportFakes = ReportLibrary.Reports.Fakes;
using TelerikProcessing = Telerik.Reporting.Processing;
using UASService = UAS_WS.Service;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    public partial class QualificationBreakDownTest
    {
        private bool _errorLogged;
        private DataTable _boundDataTable;

        [Test]
        public void GetParameters_GetSourceException_ErrorLogged()
        {
            // Arrange
            InitTestGetParameters(out TelerikProcessing.Table telerikTable, getSourceError: true);

            // Act
            _qualificationBreakDownPrivateObject.Invoke("GetParameters", new object[] { telerikTable, EventArgs.Empty });

            // Assert
            _errorLogged.ShouldBeTrue();
        }

        [Test]
        public void GetParameters_NoErrorAndDataBound()
        {
            // Arrange
            InitTestGetParameters(out TelerikProcessing.Table telerikTable);

            // Act
            _qualificationBreakDownPrivateObject.Invoke("GetParameters", new object[] { telerikTable, EventArgs.Empty });

            // Assert
            _errorLogged.ShouldSatisfyAllConditions(
                () => _errorLogged.ShouldBeFalse(),
                () => _boundDataTable.ShouldNotBeNull());
        }

        private void InitTestGetParameters(out TelerikProcessing.Table telerikTable, bool getSourceError = false)
        {
            _errorLogged = false;
            _boundDataTable = null;
            if (getSourceError)
            {
                ReportFakes.ShimReportUtilities.GetQSourceBreakDownStringStringInt32BooleanInt32 = (filters, hoc, issueID, include, id) => throw new Exception();
            }
            ShimDataItem.AllInstances.DataSourceSetObject = (dataItem, data) =>
            {
                _boundDataTable = data as DataTable;
            };
            telerikTable = new ShimTable();
            ShimProcessingElement.AllInstances.ReportGet = (element) => new ShimReport();
            ShimReport.AllInstances.ParametersGet = (report) => CreateTableParametersForGetParameters();
            ShimServiceClient<IApplicationLog>.ConstructorString = (client, endPoint) => { };
            ShimServiceClient<IApplicationLog>.AllInstances.ProxyGet = (client) => new UASService.ApplicationLog();
            ShimClient.GetSqlCommand = (cmd) => new KMPlatform.Entity.Client();
            ShimUser.GetSqlCommand = (cmd) => new KMPlatform.Entity.User() { IsActive = true };
            KmPlatformBusinessFakes.ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (applog, ex, sourceMethod, app, note, clientId, subject) =>
                {
                    _errorLogged = true;
                    return 1;
                };
        }

        private Dictionary<string, TelerikProcessing.Parameter> CreateTableParametersForGetParameters()
        {
            var parametersDic = new Dictionary<string, TelerikProcessing.Parameter>();
            parametersDic.Add("Filters", new TelerikProcessing.Parameter()
            {
                Type = "System.String",
                Value = "Filters"
            });
            parametersDic.Add("AdHocFilters", new TelerikProcessing.Parameter()
            {
                Type = "System.String",
                Value = "AdHocFilters"
            });
            parametersDic.Add("ProductID", new TelerikProcessing.Parameter()
            {
                Type = "System.String",
                Value = "5"
            });
            parametersDic.Add("IssueID", new TelerikProcessing.Parameter()
            {
                Type = "System.String",
                Value = "10"
            });
            parametersDic.Add("IncludeAddRemove", new TelerikProcessing.Parameter()
            {
                Type = "System.String",
                Value = true.ToString()
            });
            return parametersDic;
        }
    }
}
