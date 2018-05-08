using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxToolkit = AjaxControlToolkit;
using DocumentFormat.OpenXml.Drawing.Charts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Controls;
using KMPS.MD.Controls.Fakes;
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
        private const string MethodBtnSelectDate_Click = "btnSelectDate_Click";
        private const string MethodIbChooseDate_Command = "ibChooseDate_Command";
        private const string LblID = "lblID";
        private const string RbToday = "rbToday";
        private const string RbTodayPlusMinus = "rbTodayPlusMinus";
        private const string RbOther = "rbOther";
        private const string DdlPlusMinus = "ddlPlusMinus";
        private const string TxtDays = "txtDays";
        private const string TxtDatePicker = "txtDatePicker";
        private const string DivTodayPlusMinus = "divTodayPlusMinus";
        private const string DivOther = "divOther";
        private const string MpeCalendar = "mpeCalendar";
        private const string OpenActivityFromDate = "OPENACTIVITYFROMDATE";
        private const string OpenActivityToDate = "OPENACTIVITYTODATE";
        private const string OpenEmailFromDate = "OPENEMAILFROMDATE";
        private const string OpenEmailToDate = "OPENEMAILTODATE";
        private const string ClickActivityFromDate = "CLICKACTIVITYFROMDATE";
        private const string ClickActivityToDate = "CLICKACTIVITYTODATE";
        private const string ClickEmailFromDate = "CLICKEMAILFROMDATE";
        private const string ClickEmailToDate = "CLICKEMAILTODATE";
        private const string VisitActivityFromDate = "VISITACTIVITYFROMDATE";
        private const string VisitActivityToDate = "VISITACTIVITYTODATE";
        private const string Plus = "Plus";
        private const string ExpToday = "EXP:Today";
        private const string ExpTodayPlus = "EXP:Today[+1]";
        private const string ExpTodayMinus = "EXP:Today[-1]";

        [Test]
        [TestCase(OpenActivityFromDate, true, false, false, "", TxtOpenActivityFrom)]
        [TestCase(OpenActivityFromDate, false, true, false, Plus, TxtOpenActivityFrom)]
        [TestCase(OpenActivityFromDate, false, true, false, "", TxtOpenActivityFrom)]
        [TestCase(OpenActivityFromDate, false, false, true, "", TxtOpenActivityFrom)]

        [TestCase(OpenActivityToDate, true, false, false, "", TxtOpenActivityTo)]
        [TestCase(OpenActivityToDate, false, true, false, Plus, TxtOpenActivityTo)]
        [TestCase(OpenActivityToDate, false, true, false, "", TxtOpenActivityTo)]
        [TestCase(OpenActivityToDate, false, false, true, "", TxtOpenActivityTo)]

        [TestCase(OpenEmailFromDate, true, false, false, "", TxtOpenEmailFromDate)]
        [TestCase(OpenEmailFromDate, false, true, false, Plus, TxtOpenEmailFromDate)]
        [TestCase(OpenEmailFromDate, false, true, false, "", TxtOpenEmailFromDate)]
        [TestCase(OpenEmailFromDate, false, false, true, "", TxtOpenEmailFromDate)]

        [TestCase(OpenEmailToDate, true, false, false, "", TxtOpenEmailToDate)]
        [TestCase(OpenEmailToDate, false, true, false, Plus, TxtOpenEmailToDate)]
        [TestCase(OpenEmailToDate, false, true, false, "", TxtOpenEmailToDate)]
        [TestCase(OpenEmailToDate, false, false, true, "", TxtOpenEmailToDate)]

        [TestCase(ClickActivityFromDate, true, false, false, "", TxtClickActivityFrom)]
        [TestCase(ClickActivityFromDate, false, true, false, Plus, TxtClickActivityFrom)]
        [TestCase(ClickActivityFromDate, false, true, false, "", TxtClickActivityFrom)]
        [TestCase(ClickActivityFromDate, false, false, true, "", TxtClickActivityFrom)]

        [TestCase(ClickActivityToDate, true, false, false, "", TxtClickActivityTo)]
        [TestCase(ClickActivityToDate, false, true, false, Plus, TxtClickActivityTo)]
        [TestCase(ClickActivityToDate, false, true, false, "", TxtClickActivityTo)]
        [TestCase(ClickActivityToDate, false, false, true, "", TxtClickActivityTo)]

        [TestCase(ClickEmailFromDate, true, false, false, "", TxtClickEmailFromDate)]
        [TestCase(ClickEmailFromDate, false, true, false, Plus, TxtClickEmailFromDate)]
        [TestCase(ClickEmailFromDate, false, true, false, "", TxtClickEmailFromDate)]
        [TestCase(ClickEmailFromDate, false, false, true, "", TxtClickEmailFromDate)]

        [TestCase(ClickEmailToDate, true, false, false, "", TxtClickEmailToDate)]
        [TestCase(ClickEmailToDate, false, true, false, Plus, TxtClickEmailToDate)]
        [TestCase(ClickEmailToDate, false, true, false, "", TxtClickEmailToDate)]
        [TestCase(ClickEmailToDate, false, false, true, "", TxtClickEmailToDate)]

        [TestCase(VisitActivityFromDate, true, false, false, "", TxtVisitActivityFrom)]
        [TestCase(VisitActivityFromDate, false, true, false, Plus, TxtVisitActivityFrom)]
        [TestCase(VisitActivityFromDate, false, true, false, "", TxtVisitActivityFrom)]
        [TestCase(VisitActivityFromDate, false, false, true, "", TxtVisitActivityFrom)]

        [TestCase(VisitActivityToDate, true, false, false, "", TxtVisitActivityTo)]
        [TestCase(VisitActivityToDate, false, true, false, Plus, TxtVisitActivityTo)]
        [TestCase(VisitActivityToDate, false, true, false, "", TxtVisitActivityTo)]
        [TestCase(VisitActivityToDate, false, false, true, "", TxtVisitActivityTo)]
        public void btnSelectDate_Click_MultipleCases_ReachEnd(string lblID, bool rbToday, bool rbTodayPlusMinus, bool rbOther, string ddlPlusMinus, string controlToAssert)
        {
            // Arrange
            InitializePageAndControls();
            SetupForBtnSelectDate_Click(lblID, rbToday, rbTodayPlusMinus, rbOther, ddlPlusMinus, "1");

            // Act	
            PrivateControl.Invoke(MethodBtnSelectDate_Click, new object[] { null, null });
            var textBoxField = PrivateControl.GetField(controlToAssert) as TextBox;

            // Assert
            textBoxField.ShouldSatisfyAllConditions(
                () => textBoxField.ShouldNotBeNull(),
                () => textBoxField.Text.ShouldNotBeNullOrWhiteSpace()
            );
        }

        [Test]
        [TestCase(OpenActivityFromDate, TxtOpenActivityFrom)]
        [TestCase(OpenActivityToDate, TxtOpenActivityTo)]
        [TestCase(OpenEmailFromDate, TxtOpenEmailFromDate)]
        [TestCase(OpenEmailToDate, TxtOpenEmailToDate)]
        [TestCase(ClickActivityFromDate, TxtClickActivityFrom)]
        [TestCase(ClickActivityToDate, TxtClickActivityTo)]
        [TestCase(ClickEmailFromDate, TxtClickEmailFromDate)]
        [TestCase(ClickEmailToDate, TxtClickEmailToDate)]
        [TestCase(VisitActivityFromDate, TxtVisitActivityFrom)]
        [TestCase(VisitActivityToDate, TxtVisitActivityTo)]
        public void btnSelectDate_Click_MultipleCasesOnException_ReachEnd(string lblID, string controlToAssert)
        {
            // Arrange
            InitializePageAndControls();
            SetupForBtnSelectDate_Click(lblID, false, true, false, string.Empty, DummyString);

            // Act	
            PrivateControl.Invoke(MethodBtnSelectDate_Click, new object[] { null, null });
            var textBoxField = PrivateControl.GetField(controlToAssert) as TextBox;

            // Assert
            textBoxField.ShouldSatisfyAllConditions(
                () => textBoxField.ShouldNotBeNull(),
                () => textBoxField.Text.ShouldBeNullOrWhiteSpace()
            );
        }

        [Test]
        [TestCase(OpenActivityFromDate, ExpToday, TxtOpenActivityFrom, RbToday)]
        [TestCase(OpenActivityFromDate, ExpTodayPlus, TxtOpenActivityFrom, RbTodayPlusMinus)]
        [TestCase(OpenActivityFromDate, ExpTodayMinus, TxtOpenActivityFrom, RbTodayPlusMinus)]
        [TestCase(OpenActivityFromDate, DummyString, TxtOpenActivityFrom, RbOther)]

        [TestCase(OpenActivityToDate, ExpToday, TxtOpenActivityTo, RbToday)]
        [TestCase(OpenActivityToDate, ExpTodayPlus, TxtOpenActivityTo, RbTodayPlusMinus)]
        [TestCase(OpenActivityToDate, ExpTodayMinus, TxtOpenActivityTo, RbTodayPlusMinus)]
        [TestCase(OpenActivityToDate, DummyString, TxtOpenActivityTo, RbOther)]

        [TestCase(OpenEmailFromDate, ExpToday, TxtOpenEmailFromDate, RbToday)]
        [TestCase(OpenEmailFromDate, ExpTodayMinus, TxtOpenEmailFromDate, RbTodayPlusMinus)]
        [TestCase(OpenEmailFromDate, ExpTodayPlus, TxtOpenEmailFromDate, RbTodayPlusMinus)]
        [TestCase(OpenEmailFromDate, DummyString, TxtOpenEmailFromDate, RbOther)]

        [TestCase(OpenEmailToDate, ExpToday, TxtOpenEmailToDate, RbToday)]
        [TestCase(OpenEmailToDate, ExpTodayPlus, TxtOpenEmailToDate, RbTodayPlusMinus)]
        [TestCase(OpenEmailToDate, ExpTodayMinus, TxtOpenEmailToDate, RbTodayPlusMinus)]
        [TestCase(OpenEmailToDate, DummyString, TxtOpenEmailToDate, RbOther)]

        [TestCase(ClickActivityFromDate, ExpToday, TxtClickActivityFrom, RbToday)]
        [TestCase(ClickActivityFromDate, ExpTodayPlus, TxtClickActivityFrom, RbTodayPlusMinus)]
        [TestCase(ClickActivityFromDate, ExpTodayMinus, TxtClickActivityFrom, RbTodayPlusMinus)]
        [TestCase(ClickActivityFromDate, DummyString, TxtClickActivityFrom, RbOther)]

        [TestCase(ClickActivityToDate, ExpToday, TxtClickActivityTo, RbToday)]
        [TestCase(ClickActivityToDate, ExpTodayPlus, TxtClickActivityTo, RbTodayPlusMinus)]
        [TestCase(ClickActivityToDate, ExpTodayMinus, TxtClickActivityTo, RbTodayPlusMinus)]
        [TestCase(ClickActivityToDate, DummyString, TxtClickActivityTo, RbOther)]

        [TestCase(ClickEmailFromDate, ExpToday, TxtClickEmailFromDate, RbToday)]
        [TestCase(ClickEmailFromDate, ExpTodayPlus, TxtClickEmailFromDate, RbTodayPlusMinus)]
        [TestCase(ClickEmailFromDate, ExpTodayMinus, TxtClickEmailFromDate, RbTodayPlusMinus)]
        [TestCase(ClickEmailFromDate, DummyString, TxtClickEmailFromDate, RbOther)]

        [TestCase(ClickEmailToDate, ExpToday, TxtClickEmailToDate, RbToday)]
        [TestCase(ClickEmailToDate, ExpTodayPlus, TxtClickEmailToDate, RbTodayPlusMinus)]
        [TestCase(ClickEmailToDate, ExpTodayMinus, TxtClickEmailToDate, RbTodayPlusMinus)]
        [TestCase(ClickEmailToDate, DummyString, TxtClickEmailToDate, RbOther)]

        [TestCase(VisitActivityFromDate, ExpToday, TxtVisitActivityFrom, RbToday)]
        [TestCase(VisitActivityFromDate, ExpTodayPlus, TxtVisitActivityFrom, RbTodayPlusMinus)]
        [TestCase(VisitActivityFromDate, ExpTodayMinus, TxtVisitActivityFrom, RbTodayPlusMinus)]
        [TestCase(VisitActivityFromDate, DummyString, TxtVisitActivityFrom, RbOther)]

        [TestCase(VisitActivityToDate, ExpToday, TxtVisitActivityTo, RbToday)]
        [TestCase(VisitActivityToDate, ExpTodayPlus, TxtVisitActivityTo, RbTodayPlusMinus)]
        [TestCase(VisitActivityToDate, ExpTodayMinus, TxtVisitActivityTo, RbTodayPlusMinus)]
        [TestCase(VisitActivityToDate, DummyString, TxtVisitActivityTo, RbOther)]
        public void ibChooseDate_Command_MultipleCases_ReachEnd(
            string command,
            string textValue,
            string controlThatHasValue,
            string controlToAssert)
        {
            // Arrange
            InitializePageAndControls();
            SetupForIbChooseDate_Command();
            var commandEventArgs = new CommandEventArgs(DummyString, command);
            PrivateControl.SetField(controlThatHasValue, GetTextBoxWithText(textValue));
            ShimActivity.AllInstances.LoadControls = (obj) => { };

            // Act	
            PrivateControl.Invoke(MethodIbChooseDate_Command, null, commandEventArgs);
            var lblID = PrivateControl.GetField(LblID) as Label;
            var rbField = PrivateControl.GetField(controlToAssert) as HtmlInputRadioButton;

            // Assert
            lblID.ShouldSatisfyAllConditions(
                () => lblID.ShouldNotBeNull(),
                () => lblID.Text.ShouldContain(command)
            );
            rbField.ShouldSatisfyAllConditions(
                () => rbField.ShouldNotBeNull(),
                () => rbField.Checked.ShouldBeTrue()
            );
        }

        private void SetupForIbChooseDate_Command()
        {
            PrivateControl.SetField(LblID, new Label());

            PrivateControl.SetField(RbToday, new HtmlInputRadioButton());
            PrivateControl.SetField(RbTodayPlusMinus, new HtmlInputRadioButton());
            PrivateControl.SetField(RbOther, new HtmlInputRadioButton());

            PrivateControl.SetField(DdlPlusMinus, new DropDownList());

            PrivateControl.SetField(TxtDays, new TextBox());
            PrivateControl.SetField(TxtDatePicker, new TextBox());

            PrivateControl.SetField(DivTodayPlusMinus, new HtmlGenericControl());
            PrivateControl.SetField(DivOther, new HtmlGenericControl());

            PrivateControl.SetField(MpeCalendar, new AjaxToolkit::ModalPopupExtender());
        }

        private void SetupForBtnSelectDate_Click(string lblID, bool rbToday, bool rbTodayPlusMinus, bool rbOther, string ddlPlusMinus, string txtDays)
        {
            PrivateControl.SetField(LblID, GetLabelWithText(lblID));
            PrivateControl.SetField(RbToday, GetInputRadioButtonWithValue(rbToday));
            PrivateControl.SetField(RbTodayPlusMinus, GetInputRadioButtonWithValue(rbTodayPlusMinus));
            PrivateControl.SetField(RbOther, GetInputRadioButtonWithValue(rbOther));
            PrivateControl.SetField(DdlPlusMinus, GetDropDownListWithItem(ddlPlusMinus));
            PrivateControl.SetField(TxtDays, GetTextBoxWithText(txtDays));
            PrivateControl.SetField(TxtDatePicker, GetTextBoxWithText(DummyString));
        }

        private HtmlInputRadioButton GetInputRadioButtonWithValue(bool radioButtonValue)
        {
            return new HtmlInputRadioButton
            {
                Checked = radioButtonValue
            };
        }

        private Label GetLabelWithText(string textValue)
        {
            return new Label
            {
                Text = textValue
            };
        }
    }
}
