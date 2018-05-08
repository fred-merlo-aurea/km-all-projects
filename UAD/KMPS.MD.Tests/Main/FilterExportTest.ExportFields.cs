using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Main
{
    public partial class FilterExportTest
    {
        private const string ProperCase = "PROPERCASE";
        private const string LowerCase = "LOWERCASE";
        private const string UpperCase = "UPPERCASE";
        private const string CamelCase = "CAMELCASE";
        private const string AvailableProfileFieldsListBox = "lstAvailableProfileFields";
        private const string AvailableDemoFieldsListBox = "lstAvailableDemoFields";
        private const string AvailableAdhocFieldsListBox = "lstAvailableAdhocFields";
        private const string SelectedFieldsListBox = "lstSelectedFields";
        private const string ExportFieldsMethodName = "ExportFields";

        [Test]
        [TestCase(ProperCase)]
        [TestCase(LowerCase)]
        [TestCase(UpperCase)]
        [TestCase(CamelCase)]
        public void ExportFields_WhenFilterScheduleIDHasValue_PopulatesFields(string fieldCaseType)
        {
            // Arrange
            CreatePageShimObject(true);
            var lstSelectedDataSource = CreateSelectedFieldsDataSource();
            BindSelectedFieldsDropDown(lstSelectedDataSource);
            CommonPageFakeObject(lstSelectedDataSource, fieldCase: fieldCaseType);
            BindxportDropDown(1);
            BindPageInputControl("1", true);
            BindDownloadTemplateDropDown(0);

            // Act
            _privateObject.Invoke(ExportFieldsMethodName);

            // Assert
            AssertAllFieldsListBox(fieldCaseType, lstSelectedDataSource);
        }

        [Test]
        [TestCase(ProperCase)]
        [TestCase(LowerCase)]
        [TestCase(UpperCase)]
        [TestCase(CamelCase)]
        public void ExportFields_WhenFilterScheduleIDIsEmpty_PopulatesFields(string fieldCaseType)
        {
            // Arrange
            CreatePageShimObject(true);
            var lstSelectedDataSource = CreateSelectedFieldsDataSource();
            BindSelectedFieldsDropDown(lstSelectedDataSource);
            CommonPageFakeObject(lstSelectedDataSource, fieldCase: fieldCaseType);
            BindxportDropDown(1);
            BindPageInputControl("1", true);
            BindDownloadTemplateDropDown(0);
            ShimHttpRequest.AllInstances.QueryStringGet = (x) => { return CreateAppSettingsKey(filterScheduleId: "0"); };

            // Act
            _privateObject.Invoke(ExportFieldsMethodName);

            // Assert
            AssertAllFieldsListBox(fieldCaseType, lstSelectedDataSource);
        }

        [Test]
        [TestCase(ProperCase)]
        [TestCase(LowerCase)]
        [TestCase(UpperCase)]
        [TestCase(CamelCase)]
        public void ExportFields_WhenDropDownSelectedValueIsZero_PopulatesFields(string fieldCaseType)
        {
            // Arrange
            CreatePageShimObject(true);
            var lstSelectedDataSource = CreateSelectedFieldsDataSource();
            BindSelectedFieldsDropDown(lstSelectedDataSource);
            CommonPageFakeObject(lstSelectedDataSource, fieldCase: fieldCaseType);
            BindxportDropDown(1);
            BindPageInputControl("1", true);
            BindDownloadTemplateDropDown(-1);
            var drpDownloadTemplate = GetField<DropDownList>(DropDownListDownloadTemplate);
            drpDownloadTemplate.Items.RemoveAt(0);
            drpDownloadTemplate.Items.Add(new ListItem { Value = "0" });

            // Act
            _privateObject.Invoke(ExportFieldsMethodName);

            // Assert
            AssertAllFieldsListBox(fieldCaseType, lstSelectedDataSource);
        }

        private void AssertAllFieldsListBox(string fieldCaseType, Dictionary<string, string> lstSelectedDataSource)
        {
            var profileListBox = GetField<ListBox>(AvailableProfileFieldsListBox);
            var demoListBox = GetField<ListBox>(AvailableDemoFieldsListBox);
            var adhocListBox = GetField<ListBox>(AvailableAdhocFieldsListBox);
            var selectedListBox = GetField<ListBox>(SelectedFieldsListBox);

            // Assert
            profileListBox.ShouldSatisfyAllConditions(
                () => profileListBox.Items.Count.ShouldBe(1),
                () => profileListBox.Items.FindByText("6").ShouldNotBeNull(),
                () => demoListBox.Items.Count.ShouldBe(2),
                () => demoListBox.Items.FindByText("4").ShouldNotBeNull(),
                () => demoListBox.Items.FindByText("5").ShouldNotBeNull(),
                () => adhocListBox.Items.Count.ShouldBe(2),
                () => adhocListBox.Items.FindByText("13").ShouldNotBeNull(),
                () => adhocListBox.Items.FindByText("14").ShouldNotBeNull(),
                () => selectedListBox.Items.Count.ShouldBe(15),
                () => selectedListBox.Items.FindByText("7").ShouldNotBeNull(),
                () => selectedListBox.Items.FindByValue($@"{lstSelectedDataSource.Where(x => x.Value == "7").
                                                            Select(x => x.Key).First()}|{fieldCaseType.ToUpper()}").ShouldNotBeNull());
        }
    }
}
