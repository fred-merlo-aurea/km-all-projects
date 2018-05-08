using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;
using CollectorEntities = ECN_Framework_Entities.Collector;

namespace ECN.Collector.Tests.includes
{
    public partial class SurveyBuilderTest
    {
        private const string SampleOption1 = "SampleOption1";
        private const string SampleOption2 = "SampleOption2";
        private const string SampleStatement1 = "SampleStatement1";
        private const string SampleStatement2 = "SampleStatement2";
        private const string CreateOptionControlMethodName = "CreateOptionControl";

        [Test]
        public void CreateOptionControl_WhenQuestionFormatCheckbox_RetrunsCheckBoxControl()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("CheckBox");

            // Act
            var control = _privateObject.Invoke(CreateOptionControlMethodName, question) as Control;

            // Assert
            control.ShouldNotBeNull();
            control.ShouldBeOfType(typeof(CheckBoxList));
            var checkBoxList = (CheckBoxList)control;
            checkBoxList.ShouldSatisfyAllConditions(
                () => checkBoxList.CssClass.ShouldBe("answer"),
                () => checkBoxList.DataTextField.ShouldBe(nameof(CollectorEntities.ResponseOptions.OptionValue)),
                () => checkBoxList.DataValueField.ShouldBe(nameof(CollectorEntities.ResponseOptions.OptionValue)),
                () => checkBoxList.ID.ShouldBe("question_1"),
                () => checkBoxList.Attributes.Keys.OfType<string>().ShouldContain("onclick"),
                () => checkBoxList.Attributes["onclick"].ShouldContain("javascript:EnableTextControl('c'"),
                () => checkBoxList.Items.Count.ShouldBe(2),
                () => checkBoxList.Items.FindByValue(question.ResponseList[0].Value).Selected.ShouldBeTrue());
        }

        [Test]
        public void CreateOptionControl_WhenQuestionFormatDropDown_RetrunsDropDownControl()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("DropDown");

            // Act
            var control = _privateObject.Invoke(CreateOptionControlMethodName, question) as Control;

            // Assert
            control.ShouldNotBeNull();
            control.ShouldBeOfType(typeof(DropDownList));
            var dropDownList = (DropDownList)control;
            dropDownList.ShouldSatisfyAllConditions(
                () => dropDownList.DataTextField.ShouldBe(nameof(CollectorEntities.ResponseOptions.OptionValue)),
                () => dropDownList.DataValueField.ShouldBe(nameof(CollectorEntities.ResponseOptions.OptionValue)),
                () => dropDownList.ID.ShouldBe("question_1"),
                () => dropDownList.Attributes.Keys.OfType<string>().ShouldContain("onChange"),
                () => dropDownList.Attributes["onChange"].ShouldContain("javascript:EnableTextControl('d'"),
                () => dropDownList.Items.Count.ShouldBe(3),
                () => dropDownList.Items.FindByValue(question.ResponseList[0].Value).Selected.ShouldBeTrue());
        }

        [Test]
        public void CreateOptionControl_WhenQuestionFormatRadioButtonList_RetrunsRadioButtonListControl()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("Radio");

            // Act
            var control = _privateObject.Invoke(CreateOptionControlMethodName, question) as Control;

            // Assert
            control.ShouldNotBeNull();
            control.ShouldBeOfType(typeof(RadioButtonList));
            var radioButtonList = (RadioButtonList)control;
            radioButtonList.ShouldSatisfyAllConditions(
                () => radioButtonList.CssClass.ShouldBe("answer"),
                () => radioButtonList.DataTextField.ShouldBe(nameof(CollectorEntities.ResponseOptions.OptionValue)),
                () => radioButtonList.DataValueField.ShouldBe(nameof(CollectorEntities.ResponseOptions.OptionValue)),
                () => radioButtonList.ID.ShouldBe("question_1"),
                () => radioButtonList.Attributes.Keys.OfType<string>().ShouldContain("onclick"),
                () => radioButtonList.Attributes["onclick"].ShouldContain("javascript:EnableTextControl('r'"),
                () => radioButtonList.Items.Count.ShouldBe(2),
                () => radioButtonList.Items.FindByValue(question.ResponseList[0].Value).Selected.ShouldBeTrue());
        }

        [Test]
        public void CreateOptionControl_WhenQuestionFormatTextBox_RetrunsTextBoxControl()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("TextBox");
            question.MaxLength = 30;

            // Act
            var control = _privateObject.Invoke(CreateOptionControlMethodName, question) as Control;

            // Assert
            control.ShouldNotBeNull();
            control.ShouldBeOfType(typeof(TextBox));
            var textBox = (TextBox)control;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ID.ShouldBe("question_1"),
                () => textBox.Attributes.Keys.OfType<string>().ShouldContain("maxLength"),
                () => textBox.Attributes["maxLength"].ShouldContain(question.MaxLength.ToString()),
                () => textBox.Text.ShouldContain(question.ResponseList[1].Value));
        }

        [Test]
        public void CreateOptionControl_WhenQuestionFormatTextBoxMultiline_RetrunsTextBoxControl()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("TextBox");
            question.MaxLength = 100;

            // Act
            var control = _privateObject.Invoke(CreateOptionControlMethodName, question) as Control;

            // Assert
            control.ShouldNotBeNull();
            control.ShouldBeOfType(typeof(TextBox));
            var textBox = (TextBox)control;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ID.ShouldBe("question_1"),
                () => textBox.TextMode.ShouldBe(TextBoxMode.MultiLine),
                () => textBox.Rows.ShouldBe(5),
                () => textBox.Columns.ShouldBe(50),
                () => textBox.Attributes.Keys.Count.ShouldBe(4),
                () => textBox.Attributes.Keys.OfType<string>().ShouldContain("maxLength"),
                () => textBox.Attributes.Keys.OfType<string>().ShouldContain("onkeypress"),
                () => textBox.Attributes.Keys.OfType<string>().ShouldContain("onbeforepaste"),
                () => textBox.Attributes.Keys.OfType<string>().ShouldContain("onpaste"),
                () => textBox.Attributes["maxLength"].ShouldContain(question.MaxLength.ToString()),
                () => textBox.Attributes["onkeypress"].ShouldContain("doKeypress(this, event);"),
                () => textBox.Attributes["onbeforepaste"].ShouldContain("doBeforePaste(this, event);"),
                () => textBox.Attributes["onpaste"].ShouldContain("doPaste(this, event);"),
                () => textBox.Text.ShouldContain(question.ResponseList[1].Value));
        }

        [Test]
        public void CreateOptionControl_WhenQuestionFormatGrid_RetrunsHtmlTableControl()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("Grid");
            
            // Act
            var control = _privateObject.Invoke(CreateOptionControlMethodName, question) as Control;

            // Assert
            control.ShouldNotBeNull();
            control.ShouldBeOfType(typeof(HtmlTable));
            var grid = (HtmlTable)control;
            grid.ShouldSatisfyAllConditions(
                () => grid.CellPadding.ShouldBe(3),
                () => grid.CellSpacing.ShouldBe(0),
                () => grid.Attributes.Keys.Count.ShouldBe(3),
                () => grid.Attributes.Keys.OfType<string>().ShouldContain("class"),
                () => grid.Attributes["class"].ShouldContain("tblSurveyGrid"),
                () => grid.Rows.Count.ShouldBe(3),
                () => grid.Rows[0].Cells[1].InnerText.ShouldBe(question.ResponseList[0].Value),
                () => grid.Rows[0].Cells[2].InnerText.ShouldBe(question.ResponseList[1].Value),
                () => grid.Rows[1].Cells[0].Controls[0].ShouldNotBeNull(),
                () => grid.Rows[1].Cells[0].Controls[0].ShouldBeOfType<Label>(),
                () => ((Label)grid.Rows[1].Cells[0].Controls[0]).Text.ShouldBe(SampleStatement1),
                () => grid.Rows[2].Cells[0].Controls[0].ShouldNotBeNull(),
                () => grid.Rows[2].Cells[0].Controls[0].ShouldBeOfType<Label>(),
                () => ((Label)grid.Rows[2].Cells[0].Controls[0]).Text.ShouldBe(SampleStatement2)
                );
        }

        [Test]
        public void CreateOptionControl_WhenQuestionFormatUnknown_ThrowsException()
        {
            // Arrange
            SetFakesForCreateOptionControlMethod();
            var question = GetQuestion("ComboBox");

            // Act
            var innerExp = Should.Throw<TargetInvocationException>(() => 
                _privateObject.Invoke(CreateOptionControlMethodName, question)).InnerException;

            // Assert
            innerExp.ShouldSatisfyAllConditions(
                () => innerExp.ShouldNotBeNull(),
                () => innerExp.ShouldBeOfType<InvalidOperationException>(),
                () => innerExp.Message.ShouldContain($"Unknown type of control -- {question.Format}"));
        }

        private void SetFakesForCreateOptionControlMethod()
        {
            ShimResponseOptions.GetByQuestionIDInt32User = (qid, user) => new List<CollectorEntities.ResponseOptions>
            {
                new CollectorEntities.ResponseOptions { OptionID = 1 , QuestionID = 1, OptionValue = SampleOption1, Score = 50 },
                new CollectorEntities.ResponseOptions { OptionID = 2 , QuestionID = 1, OptionValue = SampleOption2, Score = 50 },
            };
            ShimGridStatements.GetByQuestionIDInt32User = (qid, user) => new List<CollectorEntities.GridStatements>
            {
                new CollectorEntities.GridStatements { GridStatementID = 1, QuestionID = 1, GridStatement = SampleStatement1 },
                new CollectorEntities.GridStatements { GridStatementID = 2, QuestionID = 1, GridStatement = SampleStatement2 },
            };
            ShimUser.GetByAccessKeyStringBoolean = (accesskey, b) => new KMPlatform.Entity.User { UserID = 1, UserName = TestUser };
        }

        private CollectorEntities.Question GetQuestion(string format)
        {
            return new CollectorEntities.Question
            {
                QuestionID = 1,
                Format = format,
                ShowTextControl = true,
                ResponseList = new List<CollectorEntities.Response>
                {
                    new CollectorEntities.Response { ID = -1, Value = SampleOption1 },
                    new CollectorEntities.Response { ID = 1, Value = SampleOption2 }
                }
            };
        }
    }
}
