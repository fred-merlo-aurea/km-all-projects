using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.blastsmanager;
using ECN.Tests.Helpers;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestFixture]
    public class RegBlastTest
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

        [Test]
        public void BtnChangeEnvelopeOnclick_VisibleEmailFromDropDownList_ShowDropDownListsAndHideTextBoxes()
        {
            // Arrange
            HttpContext.Current = MockHelpers.FakeHttpContext();
            var regBlast = new regBlast();
            ReflectionHelper.SetValue(regBlast, "drpEmailFrom", new DropDownList { Visible = true });
            SetVisibleProperty(regBlast, DropDownNames, true);
            SetStyleAttribute(regBlast, TextBoxesNames, DisplayNone);

            // Act
            ReflectionHelper.ExecuteMethod(regBlast, "btnChangeEnvelope_onclick", new object[] { null, null });

            // Assert
            AssertVisibleProperties(regBlast, DropDownNames, false);
            AssertStyleAttributes(regBlast, TextBoxesNames, DisplayInline);
        }

        [Test]
        public void BtnChangeEnvelopeOnclick_InVisibleEmailFromDropDownList_ShowDropDownListsAndHideTextBoxes()
        {
            // Arrange
            HttpContext.Current = MockHelpers.FakeHttpContext();
            var regBlast = new regBlast();
            ReflectionHelper.SetValue(regBlast, "drpEmailFrom", new DropDownList { Visible = false });
            SetVisibleProperty(regBlast, DropDownNames, false);
            SetStyleAttribute(regBlast, TextBoxesNames, DisplayInline);

            // Act
            ReflectionHelper.ExecuteMethod(regBlast, "btnChangeEnvelope_onclick", new object[] { null, null });

            // Assert
            AssertVisibleProperties(regBlast, DropDownNames, true);
            AssertStyleAttributes(regBlast, TextBoxesNames, DisplayNone);
        }

        private void SetStyleAttribute(regBlast regBlast, string[] textBoxesNames, string displayAttribute)
        {
            var textBox = new TextBox();
            textBox.Attributes.Add(StyleAttribute, displayAttribute);
            foreach (var controlName in TextBoxesNames)
            {
                ReflectionHelper.SetValue(regBlast, controlName, textBox);
            }
        }

        private void SetVisibleProperty(regBlast regBlast, string[] dropDownNames, bool isVisible)
        {
            var dropDownList = new DropDownList() { Visible = isVisible };
            foreach (var controlName in dropDownNames)
            {
                ReflectionHelper.SetValue(regBlast, controlName, dropDownList);
            }
        }

        private void AssertVisibleProperties(regBlast regBlast, string[] dropDownNames, bool isVisible)
        {
            foreach (var controlName in dropDownNames)
            {
                var control = ReflectionHelper.GetFieldInfoFromInstanceByName(regBlast, controlName).GetValue(regBlast) as DropDownList;
                Assert.That(control, Is.Not.Null);
                Assert.That(control.Visible, Is.EqualTo(isVisible));
            }
        }

        private void AssertStyleAttributes(regBlast regBlast, string[] textBoxesNames, string displayMode)
        {
            foreach (var controlName in textBoxesNames)
            {
                var control = ReflectionHelper.GetFieldInfoFromInstanceByName(regBlast, controlName).GetValue(regBlast) as TextBox;
                Assert.That(control, Is.Not.Null);
                Assert.That(control.Attributes, Is.Not.Null);
                Assert.That(control.Attributes.Count, Is.EqualTo(1));
                Assert.That(control.Attributes[StyleAttribute], Is.EqualTo(displayMode));
            }
        }
    }
}