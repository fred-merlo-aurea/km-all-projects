using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Drawing.Charts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Controls;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using TestCommonHelpers;

namespace KMPS.MD.Tests.Controls
{
    public partial class ActivityTests
    {
        private const string OpenCriteria = "Open Criteria";
        private const string ClickCriteria = "Click Criteria";
        private const string VisitCriteria = "Visit Criteria";

        [Test]
        public void GetActivityFilters_OnEmptyFields_ReturnEmptyList()
        {
            // Arrange
            InitializePageAndControls();
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(string.Empty));

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldBeEmpty();
        }

        [Test]
        public void GetActivityFilters_OpenActivity_ReturnList()
        {
            // Arrange OpenCriteria
            InitializePageAndControls();
            PrivateControl.SetField(TxtOpenBlastID, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenEmailSubject, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(DummyString));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(string.Empty));

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () => actualResult[0].ShouldNotBeNull(),
                () => actualResult[0].Name.ShouldContain(OpenCriteria),
                () => actualResult[1].ShouldNotBeNull(),
                () => actualResult[1].Text.ShouldContain(DummyString),
                () => actualResult[2].ShouldNotBeNull(),
                () => actualResult[2].Text.ShouldContain(DummyString)
            );
        }

        [Test]
        [TestCase("DATERANGE", "")]
        [TestCase("XDAYS","Custom")]
        [TestCase("XDAYS", DummyString)]
        [TestCase("MONTH", "")]
        [TestCase("YEAR", "")]
        public void GetActivityFilters_OpenActivityAndDATERANGE_ReturnList(string dataRange, string customValue)
        {
            // Arrange OpenCriteria
            InitializePageAndControls();
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(DummyString));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(TxtOpenBlastID, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(RadCBOpenCampaigns, GetRadComboBoxWithText(DummyString));
            SetOpenActivityFields(dataRange, customValue);
            SetOpenEmailFields(dataRange, customValue);

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () => actualResult[0].ShouldNotBeNull(),
                () => actualResult[0].Name.ShouldContain(OpenCriteria),
                () => actualResult[1].ShouldNotBeNull(),
                () => actualResult[1].Text.ShouldContain(DummyString),
                () => actualResult[2].ShouldNotBeNull(),
                () => actualResult[2].Text.ShouldContain(DummyString),
                () => actualResult[3].ShouldNotBeNull(),
                () => actualResult[3].Text.ShouldContain(DummyString)
            );
        }

        [Test]
        public void GetActivityFilters_ClickActivity_ReturnList()
        {
            // Arrange OpenCriteria
            InitializePageAndControls();
            PrivateControl.SetField(TxtLink, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickBlastID, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickEmailSubject, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(DummyString));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(string.Empty));

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () => actualResult[0].ShouldNotBeNull(),
                () => actualResult[0].Name.ShouldContain(ClickCriteria),
                () => actualResult[1].ShouldNotBeNull(),
                () => actualResult[1].Text.ShouldContain(DummyString),
                () => actualResult[2].ShouldNotBeNull(),
                () => actualResult[2].Text.ShouldContain(DummyString)
            );
        }

        [Test]
        [TestCase("DATERANGE", "")]
        [TestCase("XDAYS", "Custom")]
        [TestCase("XDAYS", DummyString)]
        [TestCase("MONTH", "")]
        [TestCase("YEAR", "")]
        public void GetActivityFilters_ClickActivityAndDATERANGE_ReturnList(string dataRange, string customValue)
        {
            // Arrange OpenCriteria
            InitializePageAndControls();
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(DummyString));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(TxtLink, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickBlastID, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickEmailSubject, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(RadCBClickCampaigns, GetRadComboBoxWithText(DummyString));
            SetClickActivityFields(dataRange, customValue);
            SetClickEmailFields(dataRange, customValue);

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () => actualResult[0].ShouldNotBeNull(),
                () => actualResult[0].Name.ShouldContain(ClickCriteria),
                () => actualResult[1].ShouldNotBeNull(),
                () => actualResult[1].Text.ShouldContain(DummyString),
                () => actualResult[2].ShouldNotBeNull(),
                () => actualResult[2].Text.ShouldContain(DummyString),
                () => actualResult[3].ShouldNotBeNull(),
                () => actualResult[3].Text.ShouldContain(DummyString)
            );
        }

        [Test]
        public void GetActivityFilters_VisitActivity_ReturnList()
        {
            // Arrange OpenCriteria
            InitializePageAndControls();
            PrivateControl.SetField(TxtURL, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpDomain, GetDropDownListWithItem(DummyString));
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(DummyString));

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () => actualResult[0].ShouldNotBeNull(),
                () => actualResult[0].Name.ShouldContain(VisitCriteria),
                () => actualResult[1].ShouldNotBeNull(),
                () => actualResult[1].Text.ShouldContain(DummyString),
                () => actualResult[2].ShouldNotBeNull(),
                () => actualResult[2].Text.ShouldContain(DummyString)
            );
        }

        [Test]
        [TestCase("DATERANGE", "")]
        [TestCase("XDAYS", "Custom")]
        [TestCase("XDAYS", DummyString)]
        [TestCase("MONTH", "")]
        [TestCase("YEAR", "")]
        public void GetActivityFilters_VisitActivityAndDATERANGE_ReturnList(string dataRange, string customValue)
        {
            // Arrange OpenCriteria
            InitializePageAndControls();
            PrivateControl.SetField(TxtURL, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpDomain, GetDropDownListWithItem(DummyString));
            PrivateControl.SetField(DrpOpenActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpClickActivity, GetDropDownListWithItem(string.Empty));
            PrivateControl.SetField(DrpVisitActivity, GetDropDownListWithItem(DummyString));
            SetVisitActivityFields(dataRange, customValue);

            // Act	
            var actualResult = _testEntity.GetActivityFilters();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () => actualResult[0].ShouldNotBeNull(),
                () => actualResult[0].Name.ShouldContain(VisitCriteria),
                () => actualResult[1].ShouldNotBeNull(),
                () => actualResult[1].Text.ShouldContain(DummyString),
                () => actualResult[2].ShouldNotBeNull(),
                () => actualResult[2].Text.ShouldContain(DummyString),
                () => actualResult[3].ShouldNotBeNull(),
                () => actualResult[3].Text.ShouldContain(DummyString)
            );
        }

        private void SetOpenActivityFields(string dataRange, string customValue)
        {
            PrivateControl.SetField(DrpOpenActivityDateRange, GetDropDownListWithItem(dataRange));
            PrivateControl.SetField(DrpOpenActivityDays, GetDropDownListWithItem(customValue));   
            PrivateControl.SetField(TxtOpenActivityFrom, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenActivityTo, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenActivityFromMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenActivityToMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenActivityFromYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenActivityToYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtCustomOpenActivityDays, GetTextBoxWithText(DummyString));
        }

        private void SetOpenEmailFields(string dataRange, string value)
        {
            PrivateControl.SetField(TxtOpenEmailSubject, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpOpenEmailDateRange, GetDropDownListWithItem(dataRange));
            PrivateControl.SetField(TxtOpenEmailFromDate, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenEmailToDate, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtCustomOpenEmailDays, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpOpenEmailDays, GetDropDownListWithItem(value));
            PrivateControl.SetField(TxtOpenEmailFromMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenEmailToMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenEmailFromYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtOpenEmailToYear, GetTextBoxWithText(DummyString));
        }

        private void SetClickActivityFields(string dataRange, string customValue)
        {
            PrivateControl.SetField(DrpClickActivityDateRange, GetDropDownListWithItem(dataRange));
            PrivateControl.SetField(DrpClickActivityDays, GetDropDownListWithItem(customValue));
            PrivateControl.SetField(TxtClickActivityFrom, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickActivityTo, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickActivityFromMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickActivityToMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickActivityFromYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickActivityToYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtCustomClickActivityDays, GetTextBoxWithText(DummyString));
        }

        private void SetClickEmailFields(string dataRange, string value)
        {
            PrivateControl.SetField(TxtClickEmailSubject, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpClickEmailDateRange, GetDropDownListWithItem(dataRange));
            PrivateControl.SetField(TxtClickEmailFromDate, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickEmailToDate, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtCustomClickEmailDays, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(DrpClickEmailDays, GetDropDownListWithItem(value));
            PrivateControl.SetField(TxtClickEmailFromMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickEmailToMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickEmailFromYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtClickEmailToYear, GetTextBoxWithText(DummyString));
        }

        private void SetVisitActivityFields(string dataRange, string customValue)
        {
            PrivateControl.SetField(DrpVisitActivityDateRange, GetDropDownListWithItem(dataRange));
            PrivateControl.SetField(DrpVisitActivityDays, GetDropDownListWithItem(customValue));
            PrivateControl.SetField(TxtVisitActivityFrom, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtVisitActivityTo, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtVisitActivityFromMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtVisitActivityToMonth, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtVisitActivityFromYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtVisitActivityToYear, GetTextBoxWithText(DummyString));
            PrivateControl.SetField(TxtCustomVisitActivityDays, GetTextBoxWithText(DummyString));
        }

        private DropDownList GetDropDownListWithItem(string value)
        {
            return new DropDownList
            {
                Items = { new ListItem(value, value) },
                SelectedIndex = 0
            };
        }

        private TextBox GetTextBoxWithText(string value)
        {
            return new TextBox
            {
                Text = value
            };
        }

        private RadComboBox GetRadComboBoxWithText(string value)
        {
            return new RadComboBox
            {
                Items =
                {
                    new RadComboBoxItem
                    {
                        Text = value,
                        Value = value,
                        Checked = true
                    },
                    new RadComboBoxItem
                    {
                        Text = value,
                        Value = value,
                        Checked = true
                    }
                },
                CheckBoxes = true
            };
        }
    }
}
