using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Accounts;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    public partial class BaseChannelUpdateEmailTest
    {
        private const string PhErrorControl = "phError";
        private const string LblErrorMessageControl = "lblErrorMessage";
        private const string BtnSaveClickMethodName = "btnSave_Click";
        private const string TxtPageHeader = "txtPageHeader";
        private const string TxtPageFooter = "txtPageFooter";
        private const string TxtPageText = "txtPageText";
        private const string TxtConfirmationPageText = "txtConfirmationPageText";
        private const string TxtFinalConfirmationText = "txtFinalConfirmationText";
        private const string TxtEmailFooter = "txtEmailFooter";
        private const string TxtEmailHeader = "txtEmailHeader";
        private const string TxtEmailBody = "txtEmailBody";
        private const string ChkOverrideDefault = "chkOverrideDefault";
        private const string TxtOldEmailLabel = "txtOldEmailLabel";
        private const string TxtNewEmailLabel = "txtNewEmailLabel";
        private const string TxtButtonLabel = "txtButtonLabel";
        private const string TxtReEnterEmailLabel = "txtReEnterEmailLabel";
        private const string TxtFromEmail = "txtFromEmail";
        private const string TxtEmailSubject = "txtEmailSubject";

        private LandingPageAssign _savedLPA;
        private List<LandingPageAssignContent> _savedListLPAContents;
        private bool _isLandingPageAssignContentDeleted;

        private string[] TextBoxControlNames => new string[]
        {
            TxtFromEmail,
            TxtEmailSubject,
            TxtPageText,
            TxtConfirmationPageText,
            TxtFinalConfirmationText,
            TxtEmailFooter,
            TxtEmailHeader,
            TxtEmailBody,
            TxtOldEmailLabel,
            TxtNewEmailLabel,
            TxtButtonLabel,
            TxtReEnterEmailLabel
        };

        private static Tuple<string, string>[] ControlNameAndErrorMessage => new Tuple<string, string>[]
        {
            Tuple.Create(TxtPageHeader, " in Page Header"),
            Tuple.Create(TxtPageFooter, " in Page Footer"),
            Tuple.Create(TxtPageText, " in Page Text"),
            Tuple.Create(TxtConfirmationPageText, " in Confirmation Page Text"),
            Tuple.Create(TxtFinalConfirmationText, " in Final Confirmation Text"),
            Tuple.Create(TxtEmailFooter, " in Email Footer"),
            Tuple.Create(TxtEmailHeader, " in Email Header"),
            Tuple.Create(TxtEmailBody, " in Email Body"),
        };

        [Test, TestCaseSource(nameof(ControlNameAndErrorMessage))]
        public void btnSave_Click_WhenControlsAreInValid_SetsPageErrorLable(Tuple<string, string> controlNameAndErrorMessage)
        {
            // Arrange
            Get<TextBox>(_privateTestObject, controlNameAndErrorMessage.Item1).Text = InvalidCodeSnippet;

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.
                ShouldContain($"LandingPage: There is a badly formed codesnippet{controlNameAndErrorMessage.Item2}");
        }

        [Test]
        public void btnSave_Click_WhenValid_SavesAllLPAs()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            SetPageControls();

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            _isLandingPageAssignContentDeleted.ShouldBeTrue();
            _savedLPA.ShouldNotBeNull();
            _savedLPA.ShouldSatisfyAllConditions(
                () => _savedLPA.LPID.ShouldBe(5),
                () => _savedLPA.Header.ShouldBe(TxtPageHeader),
                () => _savedLPA.Footer.ShouldBe(TxtPageFooter),
                () => _savedLPA.BaseChannelDoesOverride.Value.ShouldBeTrue());

            var displayTexts = new List<string>(TextBoxControlNames);
            displayTexts.Add(false.ToString());

            _savedListLPAContents.ShouldSatisfyAllConditions(
                () => _savedListLPAContents.ShouldNotBeEmpty(),
                () => _savedListLPAContents.Count.ShouldBe(13),
                () => _savedListLPAContents.Select(x => x.Display).ShouldBe(displayTexts, true),
                () => _savedListLPAContents.Select(x => x.LPOID.Value).ShouldBe(
                    Enumerable.Range(18, _savedListLPAContents.Count)));
        }

        private void SetFakesForBtnSaveClickMethod()
        {
            _savedLPA = new LandingPageAssign();
            _savedListLPAContents = new List<LandingPageAssignContent>();
            _isLandingPageAssignContentDeleted = false;
            ShimLandingPageAssign.SaveLandingPageAssignUser = (lpa, user) => { _savedLPA = lpa; };

            ShimLandingPageAssignContent.DeleteInt32User = (lpid, user) => { _isLandingPageAssignContentDeleted = true; };
            ShimLandingPageAssignContent.SaveLandingPageAssignContentUser = (lpaContent, user) => { _savedListLPAContents.Add(lpaContent); };
        }

        private void SetPageControls()
        {
            foreach (var controlName in TextBoxControlNames)
            {
                Get<TextBox>(_privateTestObject, controlName).Text = controlName;
            }

            Get<TextBox>(_privateTestObject, TxtPageHeader).Text = TxtPageHeader;
            Get<TextBox>(_privateTestObject, TxtPageFooter).Text = TxtPageFooter;
            Get<CheckBox>(_privateTestObject, ChkOverrideDefault).Checked = true;
        }
    }
}
