using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ReportLibrary.Reports;
using Shouldly;
using Telerik.Reporting;
using UAS.UnitTests.ReportLibrary.Common;

namespace UAS.UnitTests.ReportLibrary
{
    /// <summary>
    /// Unit test for <see cref="FullSubscriberDetails"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class FullSubscriberDetailsTest
    {
        private const string PubSubData = "PubSubData";
        private const string Table = "table1";
        private const string ConnectionString = "Data Source=216.17.41.191;Initial Catalog=MTGMasterDB_TEst;User ID=webuser;Password=webuser#23#";
        private const int ExpectedCommandTimeout = 30;
        private FullSubscriberDetails _fullSubscriberDetails;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void InitializeComponent_FullSubscriberDetailsReportUtilitiesDebugIsTrue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = true;

            // Act
            CreateClassObject();

            // Assert
            var pubSubData = Get<SqlDataSource>(PubSubData);
            var table = Get<Table>(Table);

            pubSubData.ShouldSatisfyAllConditions(
                () => pubSubData.ShouldNotBeNull(),
                () => pubSubData.CalculatedFields.Count.ShouldBe(0),
                () => pubSubData.CommandTimeout.ShouldBe(ExpectedCommandTimeout),
                () => pubSubData.ConnectionString.ShouldNotBeNullOrEmpty(),
                () => pubSubData.ConnectionString.ShouldBe(ConnectionString),
                () => pubSubData.Container.ShouldBeNull(),
                () => pubSubData.Name.ShouldBe("PubSubData"),
                () => pubSubData.Parameters.Count.ShouldBe(3),
                () => pubSubData.ProviderName.ShouldBe("System.Data.SqlClient"),
                () => pubSubData.Site.ShouldBeNull()
            );
            table.ShouldSatisfyAllConditions(
                () => table.ShouldNotBeNull(),
                () => table.ColumnGroups.Count.ShouldBe(25),
                () => table.Items.Count.ShouldBe(54),
                () => table.RowGroups.Count.ShouldBe(1),
                () => table.Body.ShouldNotBeNull(),
                () => table.Body.Columns.Count.ShouldBe(25),
                () => table.Body.Rows.Count.ShouldBe(2),
                () => table.Report.ShouldNotBeNull(),
                () => table.Report.ReportParameters.Count.ShouldBe(6),
                () => table.Report.StyleSheet.Count.ShouldBe(5),
                () => table.DataSource.ShouldNotBeNull(),
                () => table.DesignMode.ShouldBeFalse()
            );
            AssertMethodResult();
        }

        [Test]
        public void InitializeComponent_FullSubscriberDetailsReportUtilitiesDebugIsFalse_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = false;

            // Act
            CreateClassObject();

            // Assert
            var table = Get<Table>(Table);
            table.ShouldSatisfyAllConditions(
                () => table.ShouldNotBeNull(),
                () => table.ColumnGroups.Count.ShouldBe(25),
                () => table.Items.Count.ShouldBe(54),
                () => table.RowGroups.Count.ShouldBe(1),
                () => table.Body.ShouldNotBeNull(),
                () => table.Body.Columns.Count.ShouldBe(25),
                () => table.Body.Rows.Count.ShouldBe(2),
                () => table.Report.ShouldNotBeNull(),
                () => table.Report.ReportParameters.Count.ShouldBe(6),
                () => table.Report.StyleSheet.Count.ShouldBe(5),
                () => table.DataSource.ShouldBeNull(),
                () => table.DesignMode.ShouldBeFalse()
            );
            AssertMethodResult();
        }

        private object GetTextbox(string tbName)
        {
            var textBox = _privateObject.GetFieldOrProperty(tbName) as TextBox;
            return textBox.Value;
        }
        private T Get<T>(string propName) where T : class, new()
        {
            return _privateObject.GetFieldOrProperty(propName) as T;
        }

        private void CreateClassObject()
        {
            _fullSubscriberDetails = new FullSubscriberDetails();
            _privateObject = new PrivateObject(_fullSubscriberDetails);
        }

        private void AssertMethodResult()
        {
            GetTextbox(CommonHelper.TextBox28).ShouldBe("= Fields.Copies");
            GetTextbox(CommonHelper.TextBox35).ShouldBe("= Fields.demo7");
            GetTextbox(CommonHelper.TextBox36).ShouldBe("= Fields.CategoryCode");
            GetTextbox(CommonHelper.TextBox37).ShouldBe("= Fields.TransactionCode");
            GetTextbox(CommonHelper.TextBox38).ShouldBe("= Fields.QSource");
            GetTextbox(CommonHelper.TextBox39).ShouldBe("= Fields.Qualificationdate");
            GetTextbox(CommonHelper.TextBox40).ShouldBe("= Fields.SubscriberSourceCode");
            GetTextbox(CommonHelper.TextBox41).ShouldBe("= Fields.OrigsSrc");
            GetTextbox(CommonHelper.TextBox42).ShouldBe("= Fields.Company");
            GetTextbox(CommonHelper.TextBox43).ShouldBe("= Fields.Title");
            GetTextbox(CommonHelper.TextBox44).ShouldBe("= Fields.FirstName");
            GetTextbox(CommonHelper.TextBox45).ShouldBe("= Fields.LastName");
            GetTextbox(CommonHelper.TextBox46).ShouldBe("= Fields.Address1");
            GetTextbox(CommonHelper.TextBox47).ShouldBe("= Fields.Address2");
            GetTextbox(CommonHelper.TextBox48).ShouldBe("= Fields.Address3");
            GetTextbox(CommonHelper.TextBox49).ShouldBe("= Fields.RegionCode");
            GetTextbox(CommonHelper.TextBox50).ShouldBe("= Fields.City");
            GetTextbox(CommonHelper.TextBox51).ShouldBe("= Fields.ZipCode");
            GetTextbox(CommonHelper.TextBox52).ShouldBe("= Fields.Plus4");
            GetTextbox(CommonHelper.TextBox53).ShouldBe("= Fields.Country");
            GetTextbox(CommonHelper.TextBox54).ShouldBe("= Fields.Phone");
            GetTextbox(CommonHelper.TextBox55).ShouldBe("= Fields.Mobile");
            GetTextbox(CommonHelper.TextBox56).ShouldBe("= Fields.Fax");
            GetTextbox(CommonHelper.TextBox57).ShouldBe("= Fields.Email");
            GetTextbox(CommonHelper.TextBox58).ShouldBe("= Fields.Website");
            GetTextbox(CommonHelper.TextBox2).ShouldBe("Copies");
            GetTextbox(CommonHelper.TextBox3).ShouldBe("demo7");
            GetTextbox(CommonHelper.TextBox4).ShouldBe("Category Code");
            GetTextbox(CommonHelper.TextBox5).ShouldBe("Transaction Code");
            GetTextbox(CommonHelper.TextBox6).ShouldBe("QSource");
            GetTextbox(CommonHelper.TextBox7).ShouldBe("Qualificationdate");
            GetTextbox(CommonHelper.TextBox8).ShouldBe("Subscriber Source Code");
            GetTextbox(CommonHelper.TextBox9).ShouldBe("Origs Src");
            GetTextbox(CommonHelper.TextBox10).ShouldBe("Company");
            GetTextbox(CommonHelper.TextBox11).ShouldBe("Title");
            GetTextbox(CommonHelper.TextBox12).ShouldBe("First Name");
            GetTextbox(CommonHelper.TextBox13).ShouldBe("Last Name");
            GetTextbox(CommonHelper.TextBox14).ShouldBe("Address1");
            GetTextbox(CommonHelper.TextBox15).ShouldBe("Address2");
            GetTextbox(CommonHelper.TextBox16).ShouldBe("Address3");
            GetTextbox(CommonHelper.TextBox17).ShouldBe("Region Code");
            GetTextbox(CommonHelper.TextBox18).ShouldBe("City");
            GetTextbox(CommonHelper.TextBox19).ShouldBe("Zip Code");
            GetTextbox(CommonHelper.TextBox20).ShouldBe("Plus4");
            GetTextbox(CommonHelper.TextBox21).ShouldBe("Country");
            GetTextbox(CommonHelper.TextBox22).ShouldBe("Phone");
            GetTextbox(CommonHelper.TextBox23).ShouldBe("Mobile");
            GetTextbox(CommonHelper.TextBox24).ShouldBe("Fax");
            GetTextbox(CommonHelper.TextBox25).ShouldBe("Email");
            GetTextbox(CommonHelper.TextBox26).ShouldBe("Website");
            GetTextbox(CommonHelper.TextBox60).ShouldBe("SequenceID");
            GetTextbox(CommonHelper.TextBox59).ShouldBe("= Fields.SequenceID");
            GetTextbox(CommonHelper.TextBox29).ShouldBe("For Product: ");
            GetTextbox(CommonHelper.TextBox30).ShouldBe("= Parameters.ProductName.Value");
            GetTextbox(CommonHelper.TextBox31).ShouldBe("Issue:");
            GetTextbox(CommonHelper.TextBox32).ShouldBe("=Parameters.IssueName");
            GetTextbox(CommonHelper.TextBox33).ShouldBe("As of Date: ");
            GetTextbox(CommonHelper.TextBox34).ShouldBe("=Now()");
        }
    }
}
