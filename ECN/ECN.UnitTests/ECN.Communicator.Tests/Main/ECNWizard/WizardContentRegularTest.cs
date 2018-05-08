using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Controls;
using ECN.Tests.Helpers;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestFixture]
    public class WizardContentRegularTest
    {
        private const string DisplayNone = "display:none";
        private const string DisplayInline = "display:inline";
        private const string StyleAttribute = "style";

        private string[] TextBoxesNames = new string[]
        {
                "txtEmailFrom",
                "txtReplyTo",
                "txtEmailFromName"
        };

        private string[] DropDownNames = new string[]
        {
                "drpEmailFrom",
                "drpReplyTo",
                "drpEmailFromName"
        };

        private string[] RequiredFieldValidatorNames = new string[]
        {
            "val_txtEmailFrom",
            "val_txtReplyTo",
            "val_txtEmailFromName"
        };

        [Test]
        public void BtnChangeEnvelopeOnclick_VisibleEmailFromDropDownList_ShowDropDownListsAndHideTextBoxes()
        {
            // Arrange
            HttpContext.Current = MockHelpers.FakeHttpContext();
            var wizardContent = new WizardContent();
            wizardContent.Page = new Page();
            ReflectionHelper.SetValue(wizardContent, "drpEmailFrom", new DropDownList { Visible = true });
            SetVisibleProperty(wizardContent, DropDownNames, true);
            SetStyleAttribute(wizardContent, TextBoxesNames, DisplayNone);
            SetControlToValidate(wizardContent, RequiredFieldValidatorNames, new string[] { string.Empty, string.Empty, string.Empty });

            // Act
            ReflectionHelper.ExecuteMethod(wizardContent, "btnChangeEnvelope_onclick", new object[] { null, null });

            // Assert
            AssertVisibleProperties(wizardContent, DropDownNames, false);
            AssertStyleAttributes(wizardContent, TextBoxesNames, DisplayInline);
            AssertControlToValidate(wizardContent, RequiredFieldValidatorNames, TextBoxesNames);
        }

        [Test]
        public void BtnChangeEnvelopeOnclick_InVisibleEmailFromDropDownList_ShowDropDownListsAndHideTextBoxes()
        {
            // Arrange
            HttpContext.Current = MockHelpers.FakeHttpContext();
            var wizardContent = new WizardContent();
            wizardContent.Page = new Page();
            ReflectionHelper.SetValue(wizardContent, "drpEmailFrom", new DropDownList { Visible = false });
            SetVisibleProperty(wizardContent, DropDownNames, false);
            SetStyleAttribute(wizardContent, TextBoxesNames, DisplayInline);
            SetControlToValidate(wizardContent, RequiredFieldValidatorNames, new string[] { string.Empty, string.Empty, string.Empty });

            // Act
            ReflectionHelper.ExecuteMethod(wizardContent, "btnChangeEnvelope_onclick", new object[] { null, null });

            // Assert
            AssertVisibleProperties(wizardContent, DropDownNames, true);
            AssertStyleAttributes(wizardContent, TextBoxesNames, DisplayNone);
            AssertControlToValidate(wizardContent, RequiredFieldValidatorNames, DropDownNames);
        }

        private void SetStyleAttribute(WizardContent wizardContent, string[] textBoxesNames, string displayAttribute)
        {
            var textBox = new TextBox();
            textBox.Attributes.Add(StyleAttribute, displayAttribute);
            foreach (var controlName in TextBoxesNames)
            {
                ReflectionHelper.SetValue(wizardContent, controlName, textBox);
            }
        }

        private void SetVisibleProperty(WizardContent wizardContent, string[] dropDownNames, bool isVisible)
        {
            var dropDownList = new DropDownList() { Visible = isVisible };
            foreach (var controlName in dropDownNames)
            {
                ReflectionHelper.SetValue(wizardContent, controlName, dropDownList);
            }
        }
        private void SetControlToValidate(WizardContent wizardContent, string[] requiredFieldValidatorNames, string[] controlNames)
        {
            for (var i = 0; i < requiredFieldValidatorNames.Length; i++)
            {
                var requiredFieldValidator = new RequiredFieldValidator { ControlToValidate = controlNames[i] };
                ReflectionHelper.SetValue(wizardContent, requiredFieldValidatorNames[i], requiredFieldValidator);
            }
        }

        private void AssertControlToValidate(WizardContent wizardContent, string[] requiredFieldValidatorNames, string[] controlNames)
        {
            for (var i = 0; i < requiredFieldValidatorNames.Length; i++)
            {
                var control = ReflectionHelper.GetFieldInfoFromInstanceByName(wizardContent, requiredFieldValidatorNames[i]).GetValue(wizardContent) as RequiredFieldValidator;
                Assert.That(control, Is.Not.Null);
                Assert.That(control.ControlToValidate, Is.EqualTo(controlNames[i]));
            }
        }

        private void AssertVisibleProperties(WizardContent wizardContent, string[] dropDownNames, bool isVisible)
        {
            foreach (var controlName in dropDownNames)
            {
                var control = ReflectionHelper.GetFieldInfoFromInstanceByName(wizardContent, controlName).GetValue(wizardContent) as DropDownList;
                Assert.That(control, Is.Not.Null);
                Assert.That(control.Visible, Is.EqualTo(isVisible));
            }
        }

        private void AssertStyleAttributes(WizardContent wizardContent, string[] textBoxesNames, string displayMode)
        {
            foreach (var controlName in textBoxesNames)
            {
                var control = ReflectionHelper.GetFieldInfoFromInstanceByName(wizardContent, controlName).GetValue(wizardContent) as TextBox;
                Assert.That(control, Is.Not.Null);
                Assert.That(control.Attributes, Is.Not.Null);
                Assert.That(control.Attributes.Count, Is.EqualTo(1));
                Assert.That(control.Attributes[StyleAttribute], Is.EqualTo(displayMode));
            }
        }
    }
}