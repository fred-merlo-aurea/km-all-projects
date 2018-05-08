using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ReportLibrary.Reports;
using ReportLibrary.Reports.Fakes;
using Shouldly;
using Telerik.Reporting.Processing.Fakes;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using Telerik.Reporting;
using UAS.UnitTests.ReportLibrary.Common;
using FrameworkUASService = FrameworkUAS.Service;
using KmObject = KMPlatform.Object;
using static FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ReportLibrary
{
    /// <summary>
    /// Unit test for <see cref="DemoXQualification"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DemoXQualificationTest
    {
        private const string GetParameters = "GetParameters";
        private const string Main = "Main";
        private const string Fields = "Fields";
        private const string IssueDates = "IssueDates";
        private const string Crosstab1 = "crosstab1";
        private const string GeoTable = "GeoTable";
        private const string Total = "Total";
        private const string FieldsCopies = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";

        private DemoXQualification _demoXQualification;
        private PrivateObject _privateObject;
        protected IDisposable _shimObject;
        private bool LogCriticalError = false;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
            ShimReportUtilities.GetIssueDatesInt32 = (x) =>
            {
                return CreateIssueDates();
            };
            ShimReportUtilities.GetResponsesInt32 = (x) =>
              {
                  return new List<ResponseGroup>
                  {
                    new ResponseGroup()
                  };
              };
            ShimReportUtilities.GetDemoXQualDataStringStringInt32StringBooleanInt32 =
                (filters, adHocFilters, issueID, row, includeReportGroups, productID) =>
                {
                    return new DataTable();
                };
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void InitializeComponent_ReportUtilitiesDebugIsTrue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = true;

            // Act
            CreateClassObject();

            // Assert
            AssertMethodResult(_demoXQualification);
            var main = Get<SqlDataSource>(Main);
            var fields = Get<SqlDataSource>(Fields);
            var issueDates = Get<SqlDataSource>(IssueDates);
            var crosstab1 = Get<Crosstab>(Crosstab1);
            main.ShouldNotBeNull();
            fields.ShouldNotBeNull();
            issueDates.ShouldNotBeNull();
            crosstab1.ShouldNotBeNull();
            main.ShouldSatisfyAllConditions(
             () => main.CommandTimeout.ShouldBe(180),
             () => main.Parameters.Count.ShouldBe(6),
             () => main.ProviderName.ShouldBe("System.Data.SqlClient"),
             () => main.SelectCommand.ShouldBe("dbo.rpt_DemoXQualification"),
             () => main.SelectCommandType.ShouldBe(SqlDataSourceCommandType.StoredProcedure),
             () => main.Site.ShouldBeNull(),
             () => main.Name.ShouldBe("Main")
            );
            fields.ShouldSatisfyAllConditions(
             () => fields.CalculatedFields.Count.ShouldBe(0),
             () => fields.CommandTimeout.ShouldBe(30),
             () => fields.Name.ShouldBe("Fields"),
             () => fields.SelectCommand.ShouldBe("dbo.o_GetDemosAndProfileFields"),
             () => fields.Site.ShouldBeNull()
            );
            issueDates.ShouldSatisfyAllConditions(
                () => issueDates.CalculatedFields.Count.ShouldBe(0),
                () => issueDates.CommandTimeout.ShouldBe(30),
                () => issueDates.Container.ShouldBeNull(),
                () => issueDates.Parameters.Count.ShouldBe(0),
                () => issueDates.ProviderName.ShouldBe("System.Data.SqlClient"),
                () => issueDates.SelectCommandType.ShouldBe(SqlDataSourceCommandType.Text)
            );
            crosstab1.ShouldSatisfyAllConditions(
                () => crosstab1.ConditionalFormatting.Count.ShouldBe(0),
                () => crosstab1.DesignMode.ShouldBeFalse(),
                () => crosstab1.Items.Count.ShouldBe(25),
                () => crosstab1.Report.ShouldNotBeNull(),
                () => crosstab1.Report.Items.Count.ShouldBe(3),
                () => crosstab1.Report.ReportParameters.Count.ShouldBe(10),
               () => crosstab1.Report.StyleSheet.Count.ShouldBe(8),
               () => crosstab1.Report.Visible.ShouldBeTrue()
            );
        }

        [Test]
        public void InitializeComponent_LoadDefaultControlValue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            var result = _demoXQualification;

            // Act
            CreateClassObject();

            // Assert
            AssertMethodResult(result);
        }

        [Test]
        public void GetParameters_ReportParametersIsNull_LogCriticalErrorForApplication()
        {
            // Arrange
            CreateClassObject();
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();
            var mock = new Mock<Telerik.Reporting.Processing.DataItem>();
            Telerik.Reporting.Processing.Report report = new ShimReport();
            ShimProcessingElement.AllInstances.ReportGet = (x) => report;
            var parameters = new object[] { mock.Object, EventArgs.Empty };

            // Act
            _privateObject.Invoke(GetParameters, parameters);

            // Assert
            LogCriticalError.ShouldBeTrue();
        }

        private void CreateAuthorizedUser()
        {
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
            {
                return new UserAuthorization
                {
                    User = new User
                    {
                        CurrentClient = new Client
                        {
                            ClientConnections = new KmObject.ClientConnections()
                        }
                    }
                };
            };
        }

        private void LogCriticalErrorForApplication()
        {
            ShimServiceClient.UAS_ApplicationLogClient = () =>
            {
                return new ShimServiceClient<IApplicationLog>
                {
                    ProxyGet = () =>
                    {
                        return new StubIApplicationLog()
                        {
                            LogCriticalErrorGuidStringStringEnumsApplicationsStringInt32 =
                                (accessKey, ex, sourceMethod, application, note, clientId) =>
                                {
                                    LogCriticalError = true;
                                    return new FrameworkUASService.Response<int>
                                    {
                                        Message = string.Empty,
                                        ProcessCode = string.Empty,
                                        Result = 1,
                                        Status = ServiceResponseStatusTypes.Error
                                    };
                                }
                        };
                    }
                };
            };
        }
        private DataTable CreateIssueDates()
        {
            var dataTable = new DataTable();
            var dataColumn = new System.Data.DataColumn("MonthDayStartDate", typeof(DateTime));
            dataTable.Columns.Add(dataColumn);
            dataColumn = new System.Data.DataColumn("MonthDayEndDate", typeof(DateTime));
            dataTable.Columns.Add(dataColumn);
            dataTable.Rows.Add(new Object[] { DateTime.Now, DateTime.Now });
            dataTable.Rows.Add(new Object[] { DateTime.Now, DateTime.Now });
            dataTable.Rows.Add(new Object[] { DateTime.Now, DateTime.Now });
            dataTable.Rows.Add(new Object[] { DateTime.Now, DateTime.Now });
            dataTable.Rows.Add(new Object[] { DateTime.Now, DateTime.Now });
            dataTable.Rows.Add(new Object[] { DateTime.Now, DateTime.Now });
            return dataTable;
        }

        private object GetTextbox(string tbName)
        {
            var textBox = (TextBox)_privateObject.GetFieldOrProperty(tbName);
            return textBox.Value;
        }
        private T Get<T>(string propName)
        {
            return (T)_privateObject.GetFieldOrProperty(propName);
        }

        private void CreateClassObject()
        {
            _demoXQualification = new DemoXQualification();
            _privateObject = new PrivateObject(_demoXQualification);
        }

        private void AssertMethodResult(DemoXQualification result)
        {
            GetTextbox(CommonHelper.TextBox23).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox30).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox7).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox3).ShouldBe(Total);
            GetTextbox(CommonHelper.TextBox4).ShouldBe(Total);
            GetTextbox(CommonHelper.TextBox32).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox33).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox34).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox35).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox8).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox9).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox10).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox11).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox12).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox13).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox1).ShouldBe("= Fields.Demo7");
            GetTextbox(CommonHelper.TextBox2).ShouldBe("= Fields.Year");
            GetTextbox(CommonHelper.TextBox5).ShouldBe("= Fields.Row");
            GetTextbox(CommonHelper.TextBox6).ShouldBe("= Parameters.Row.Value + \" Total\"");
            GetTextbox(CommonHelper.TextBox14).ShouldBe("Total Respondents");
            GetTextbox(CommonHelper.TextBox17).ShouldBe("= Max(Fields.colTotalUniqueRespondents)");
            GetTextbox(CommonHelper.TextBox18).ShouldBe("Issue Start Date:");
            GetTextbox(CommonHelper.TextBox19).ShouldBe("= Fields.GroupDisplay");
            GetTextbox(CommonHelper.TextBox20).ShouldBe("= Parameters.YearStart.Value");
            GetTextbox(CommonHelper.TextBox21).ShouldBe("= Parameters.YearEnd.Value");
            GetTextbox(CommonHelper.TextBox22).ShouldBe("Issue End Date:");
            GetTextbox(CommonHelper.TextBox24).ShouldBe("=Now()");
            GetTextbox(CommonHelper.TextBox25).ShouldBe("As of Date: ");
            GetTextbox(CommonHelper.TextBox26).ShouldBe("=Parameters.IssueName");
            GetTextbox(CommonHelper.TextBox27).ShouldBe("Issue:");
            GetTextbox(CommonHelper.TextBox28).ShouldBe("= Parameters.ProductName.Value");
            GetTextbox(CommonHelper.TextBox29).ShouldBe("For Product: ");
            GetTextbox(CommonHelper.TextBox31).ShouldBe("Group");
            GetTextbox(CommonHelper.TextBox36).ShouldBe("= Max(Fields.Demo7Total)");
            GetTextbox(CommonHelper.TextBox45).ShouldBe("= Max(Fields.colgrandTotalUniqueRespondents)");
            result.Items.Count.ShouldBe(3);
            result.StyleSheet.Count.ShouldBe(8);
            result.ReportParameters.Count.ShouldBe(10);
            result.Action.ShouldBeNull();
            result.Bindings.Count.ShouldBe(0);
            result.PageSettings.ShouldNotBeNull();
            result.Report.ShouldNotBeNull();
        }
    }
}
