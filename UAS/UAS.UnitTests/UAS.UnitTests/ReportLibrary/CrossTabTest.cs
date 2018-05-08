using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrameworkUAD.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ReportLibrary.Reports;
using ReportLibrary.Reports.Fakes;
using Shouldly;
using Telerik.Reporting;
using UAS.UnitTests.ReportLibrary.Common;

namespace UAS.UnitTests.ReportLibrary
{
    /// <summary>
    /// Unit test for <see cref="CrossTab"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class CrossTabTest
    {
        private const string Main = "Main";
        private const string Fields = "Fields";
        private const string IssueDates = "IssueDates";
        private const string Crosstab1 = "crosstab1";
        private const string GeoTable = "GeoTable";
        private const string Total = "Total";
        private const string FieldsCopies = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
        private const string BusinessXFunction = "BusinessXFunction";
        private const string SubReport = "SubReport";
        private const string CrossSubReport = "CrossSubReport";
        private const string TotalUniqueRespondents = "= Max(Fields.colTotalUniqueRespondents)";
        private const int ExpectedCommandTimeout = 180;
        private const int SubReportCommandTimeout = 30;
        private const int ItemsCount = 28;
        private CrossTab _crossTab;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
            ShimReportUtilities.GetResponsesInt32 = (x) =>
            {
                return new List<ResponseGroup>
                  {
                    new ResponseGroup()
                  };
            };
        }

        [Test]
        public void InitializeComponent_CrossTabReportUtilitiesDebugIsTrue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = true;

            // Act
            CreateClassObject();

            // Assert
            var businessXFunction = Get<SqlDataSource>(BusinessXFunction);
            var subReport = Get<SqlDataSource>(SubReport);
            var fields = Get<SqlDataSource>(Fields);
            var crosstab1 = Get<Crosstab>(Crosstab1);
            var crossSubReport = Get<Crosstab>(CrossSubReport);

            businessXFunction.ShouldSatisfyAllConditions(
                () => businessXFunction.ShouldNotBeNull(),
                () => businessXFunction.CalculatedFields.Count.ShouldBe(0),
                () => businessXFunction.CommandTimeout.ShouldBe(ExpectedCommandTimeout),
                () => businessXFunction.ConnectionString.ShouldNotBeNullOrEmpty(),
                () => businessXFunction.Container.ShouldBeNull(),
                () => businessXFunction.Name.ShouldBe("BusinessXFunction"),
                () => businessXFunction.Parameters.Count.ShouldBe(8),
                () => businessXFunction.ProviderName.ShouldBe("System.Data.SqlClient"),
                () => businessXFunction.Site.ShouldBeNull()
            );

            subReport.ShouldSatisfyAllConditions(
                () => subReport.CalculatedFields.Count.ShouldBe(0),
                () => subReport.CommandTimeout.ShouldBe(ExpectedCommandTimeout),
                () => subReport.ConnectionString.ShouldNotBeNullOrEmpty(),
                () => subReport.Container.ShouldBeNull(),
                () => subReport.Name.ShouldBe("SubReport"),
                () => subReport.Parameters.Count.ShouldBe(6),
                () => subReport.ProviderName.ShouldBe("System.Data.SqlClient"),
                () => subReport.Site.ShouldBeNull()
            );

            fields.ShouldSatisfyAllConditions(
                () => fields.CalculatedFields.Count.ShouldBe(0),
                () => fields.CommandTimeout.ShouldBe(SubReportCommandTimeout),
                () => fields.ConnectionString.ShouldNotBeNullOrEmpty(),
                () => fields.Container.ShouldBeNull(),
                () => fields.Parameters.Count.ShouldBe(1),
                () => fields.ProviderName.ShouldBe(string.Empty),
                () => fields.SelectCommand.ShouldBe("dbo.o_GetDemosAndProfileFields"),
                () => fields.Site.ShouldBeNull(),
                () => fields.Container.ShouldBeNull()
            );

            crosstab1.ShouldSatisfyAllConditions(
                () => crosstab1.Action.ShouldBeNull(),
                () => crosstab1.ColumnGroups.Count.ShouldBe(2),
                () => crosstab1.Items.Count.ShouldBe(ItemsCount),
                () => crosstab1.RowGroups.Count.ShouldBe(3),
                () => crosstab1.DesignMode.ShouldBeFalse()
            );

            crosstab1.ShouldSatisfyAllConditions(
               () => crosstab1.Action.ShouldBeNull(),
               () => crosstab1.ColumnGroups.Count.ShouldBe(2),
               () => crosstab1.Items.Count.ShouldBe(ItemsCount),
               () => crosstab1.RowGroups.Count.ShouldBe(3),
               () => crosstab1.DesignMode.ShouldBeFalse()
           );

            crossSubReport.ShouldSatisfyAllConditions(
                () => crossSubReport.Action.ShouldBeNull(),
                () => crossSubReport.ColumnGroups.Count.ShouldBe(4),
                () => crossSubReport.Items.Count.ShouldBe(15),
                () => crossSubReport.RowGroups.Count.ShouldBe(2),
                () => crossSubReport.DesignMode.ShouldBeFalse(),
                () => crossSubReport.Report.ShouldNotBeNull(),
                () => crossSubReport.Report.ReportParameters.Count.ShouldBe(11)
            );
            AssertMethodResult();
        }

        [Test]
        public void InitializeComponent_CrossTabReportUtilitiesDebugIsFalse_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = false;

            // Act
            CreateClassObject();

            // Assert
            var crosstab1 = Get<Crosstab>(Crosstab1);
            var crossSubReport = Get<Crosstab>(CrossSubReport);
            AssertMethodResult();
            crosstab1.DataSource.ShouldBeNull();
            crossSubReport.DataSource.ShouldBeNull();
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
            _crossTab = new CrossTab();
            _privateObject = new PrivateObject(_crossTab);
        }

        private void AssertMethodResult()
        {
            GetTextbox(CommonHelper.TextBox1).ShouldBe("= Fields.Column");
            GetTextbox(CommonHelper.TextBox2).ShouldBe("Total Responses");
            GetTextbox(CommonHelper.TextBox43).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox47).ShouldBe("= Fields.ColGroupDisplay");
            GetTextbox(CommonHelper.TextBox52).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox3).ShouldBe("= Fields.Row");
            GetTextbox(CommonHelper.TextBox39).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox35).ShouldBe("= Fields.GroupDisplay");
            GetTextbox(CommonHelper.TextBox5).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox36).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox4).ShouldBe("Total Responses");
            GetTextbox(CommonHelper.TextBox37).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox13).ShouldBe("= Fields.CategoryType + \" Total\"");
            GetTextbox(CommonHelper.TextBox28).ShouldBe("= Fields.CategoryType + \" Percent\"");
            GetTextbox(CommonHelper.TextBox31).ShouldBe("= Fields.CodeName");
            GetTextbox(CommonHelper.TextBox14).ShouldBe("Total");
            GetTextbox(CommonHelper.TextBox16).ShouldBe("= Fields.Row");
            GetTextbox(CommonHelper.TextBox20).ShouldBe("Total");
            GetTextbox(CommonHelper.TextBox6).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox7).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox8).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox9).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox21).ShouldBe(TotalUniqueRespondents);
            GetTextbox(CommonHelper.TextBox40).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox41).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox42).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox44).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox49).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox24).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox25).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox26).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox27).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox30).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox32).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox33).ShouldBe(FieldsCopies);
            GetTextbox(CommonHelper.TextBox45).ShouldBe("= Max(Fields.colgrandTotalUniqueRespondents)");
            GetTextbox(CommonHelper.TextBox50).ShouldBe(TotalUniqueRespondents);
            GetTextbox(CommonHelper.TextBox38).ShouldBe("Group");
            GetTextbox(CommonHelper.TextBox53).ShouldBe(string.Empty);
            GetTextbox(CommonHelper.TextBox55).ShouldBe("Group");
            GetTextbox(CommonHelper.TextBox29).ShouldBe("= CDbl( CDbl(IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0))/Exec(\"CrossSubRe" +
    "port\",CDbl(Sum(Fields.Copies))))");
            GetTextbox(CommonHelper.TextBox23).ShouldBe("= Parameters.Row.Value");
            GetTextbox(CommonHelper.TextBox34).ShouldBe("= Parameters.Row.Label + \" Totals\"");
            GetTextbox(CommonHelper.TextBox10).ShouldBe("For Product: ");
            GetTextbox(CommonHelper.TextBox11).ShouldBe("= Parameters.ProductName.Value");
            GetTextbox(CommonHelper.TextBox12).ShouldBe("Issue:");
            GetTextbox(CommonHelper.TextBox15).ShouldBe("=Parameters.IssueName");
            GetTextbox(CommonHelper.TextBox17).ShouldBe("As of Date: ");
            GetTextbox(CommonHelper.TextBox18).ShouldBe("=Now()");
        }
    }
}
