using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrameworkServices.Fakes;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ReportLibrary.Reports;
using Shouldly;
using UAS.UnitTests.ReportLibrary.Common;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using FrameworkUASService = FrameworkUAS.Service;

namespace UAS.UnitTests.ReportLibrary
{
    /// <summary>
    /// Unit test for <see cref="CategorySummary"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class CategorySummaryTest
    {
        private const string CatSummary = "CatSummary";
        private const string ReportTable = "table1";
        private const string FieldsCopies = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
        private const string FieldsCopiesSum = "= SUM(Fields.Copies)";
        private const string FieldsCategoryType = "= Fields.CategoryType";
        private CategorySummary _categorySummary;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;
        private bool _logCriticalError = false;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
        }

        [Test]
        public void InitializeComponent_ReportUtilitiesDebugIsTrue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = true;

            // Act
            CreateClassObject();

            // Assert
            var catSummary = Get<Telerik.Reporting.SqlDataSource>(CatSummary);
            var reportTable = Get<Telerik.Reporting.Table>(ReportTable);
            catSummary.ShouldNotBeNull();
            catSummary.ShouldSatisfyAllConditions(
                () => catSummary.CalculatedFields.Count.ShouldBe(0),
                () => catSummary.CommandTimeout.ShouldBe(30),
                () => catSummary.Container.ShouldBeNull(),
                () => catSummary.Parameters.Count.ShouldBe(3),
                () => catSummary.SelectCommand.ShouldBe("dbo.rpt_CategorySummary"),
                () => catSummary.Site.ShouldBeNull()
            );
            reportTable.ShouldNotBeNull();
            reportTable.ShouldSatisfyAllConditions(
                () => reportTable.Items.ShouldNotBeNull(),
                () => reportTable.Items.Count.ShouldBe(19),
                () => reportTable.KeepTogether.ShouldBeTrue(),
                () => reportTable.Container.ShouldBeNull(),
                () => reportTable.ColumnGroups.Count.ShouldBe(3),
                () => reportTable.Visible.ShouldBeTrue(),
                () => reportTable.Action.ShouldBeNull(),
                () => reportTable.RowGroups.Count.ShouldBe(2)
            );
            AssertMethodResult();
        }

        [Test]
        public void CategorySummary_LoadPageControlThrowException_LogCriticalErrorForApplication()
        {
            // Arrange
            CreateClassObject();
            var parameters = new object[] { this, EventArgs.Empty };
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();

            // Act
            _privateObject.Invoke(CommonHelper.GetParameters, parameters);

            // Assert
            _logCriticalError.ShouldBeTrue();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
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
                            ClientConnections = new KMPlatform.Object.ClientConnections()
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
                                    _logCriticalError = true;
                                    return new FrameworkUASService.Response<int>
                                        {
                                            Message = string.Empty,
                                            ProcessCode = string.Empty,
                                            Result = 1,
                                            Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error
                                        };
                                }
                        };
                    }
                };
            };
        }

        private object GetTextbox(string tbName)
        {
            var textBox = (Telerik.Reporting.TextBox)_privateObject.GetFieldOrProperty(tbName);
            var txt = textBox.Value;
            return txt;
        }

        private T Get<T>(string propName)
        {
            var val = (T)_privateObject.GetFieldOrProperty(propName);
            return val;
        }

        private void CreateClassObject()
        {
            _categorySummary = new CategorySummary();
            _privateObject = new PrivateObject(_categorySummary);
        }

        private void AssertMethodResult()
        {
            GetTextbox(CommonHelper.TextBox1).ToString().Trim().ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox2).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox3).ShouldBe("= Fields.Demo7");
            GetTextbox(CommonHelper.TextBox4).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox5).ShouldBe("Total Copies");
            GetTextbox(CommonHelper.TextBox6).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox7).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox8).ShouldBe("Grand Total");
            GetTextbox(CommonHelper.TextBox9).ShouldBe(FieldsCategoryType);
            GetTextbox(CommonHelper.TextBox10).ShouldBe("Category Type");
            GetTextbox(CommonHelper.TextBox11).ShouldBe("= Fields.Category");
            GetTextbox(CommonHelper.TextBox12).ShouldBe("Category");
            GetTextbox(CommonHelper.TextBox1).ToString().Trim().ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox14).ShouldBe(FieldsCopiesSum);
            GetTextbox(CommonHelper.TextBox15).ShouldBe(FieldsCopiesSum);
            GetTextbox(CommonHelper.TextBox16).ShouldBe("Total Records");
            GetTextbox(CommonHelper.TextBox17).ShouldBe("= IIf(Sum(Fields.Copies) > 0, Sum(Fields.RecordCount), 0)");
            GetTextbox(CommonHelper.TextBox18).ShouldBe("= IIf(Sum(Fields.Copies) > 0, Sum(Fields.RecordCount), 0)");
            GetTextbox(CommonHelper.TextBox19).ShouldBe("= Sum(Fields.RecordCount)");
            GetTextbox(CommonHelper.TextBox24).ShouldBe("=Now()");
            GetTextbox(CommonHelper.TextBox25).ShouldBe("As of Date: ");
            GetTextbox(CommonHelper.TextBox26).ShouldBe("=Parameters.IssueName");
            GetTextbox(CommonHelper.TextBox27).ShouldBe("Issue:");
            GetTextbox(CommonHelper.TextBox28).ShouldBe("= Parameters.ProductName.Value");
            GetTextbox(CommonHelper.TextBox29).ShouldBe("For Product: ");
        }
    }
}
