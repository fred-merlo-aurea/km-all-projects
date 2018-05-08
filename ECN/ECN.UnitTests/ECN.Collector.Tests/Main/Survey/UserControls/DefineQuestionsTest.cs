using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.collector.main.survey.UserControls;
using ecn.collector.main.survey.UserControls.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CollectorEntities = ECN_Framework_Entities.Collector;
using DataLayerFakes = ECN_Framework_DataLayer.Collector.Fakes;

namespace ECN.Collector.Tests.Main.Survey.UserControls
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.collector.main.survey.UserControls.DefineQuestions"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class DefineQuestionsTest : PageHelper
    {
        private const string BtnQuestionSaveClickMethod = "btnQuestionSave_Click";
        private const string InitializeComponentMethod = "InitializeComponent";
        private const string GetResponseOptionXMLMethod = "getResponseOptionXML";
        private const string BtnPageSaveClickMethod = "btnPageSave_Click";
        private const string BtnReOrderSaveClickMethod = "btnReOrderSave_Click";
        private const string IdLblOption = "lblOption";
        private const string IdLblScore= "lblScore";
        private const string IdTxtOptionE= "txtOptionE";
        private const string IdTxtScoreE= "txtScoreE";
        private PrivateObject _defineQuestionsPrivateObject;
        private DefineQuestions _defineQuestionsInstance;
        private ShimDefineQuestions _shimDefineQuestions;
        private int _questionDeleteMethodCallCount;
        private bool _itemCommandRegisteredStartupScript;
        private string _responseOptionsPropertyWrapper;

        [TestCase("", true, "1", true, true, "grid")]
        [TestCase("10", false, "0", false, false, "textbox")]
        [TestCase("10", false, "0", false, false, "radio")]
        public void BtnQuestionSave_Click_NoErrorAndSurveryGridCalled(
            string lblquestionIDText,
            bool gridTypeSelected,
            string rbRequiredValue,
            bool positionSelected,
            bool questionSelected,
            string questionFormat)
        {
            // Arrange
            InitTestBtnQuestionSaveClick(
                lblquestionIDText: lblquestionIDText,
                positionSelected: positionSelected,
                questionSelected: questionSelected,
                gridTypeSelected: gridTypeSelected,
                questionFormat: questionFormat,
                rbRequiredValue: rbRequiredValue);
            var loadSurveyGridCalled = false;
            ShimDefineQuestions.AllInstances.LoadSurveyGridInt32 = (p, id) => { loadSurveyGridCalled = true; };

            // Act
            _defineQuestionsPrivateObject.Invoke(BtnQuestionSaveClickMethod, new object[] { null, new ImageClickEventArgs(0, 0) });

            // Assert
            loadSurveyGridCalled.ShouldSatisfyAllConditions(
                () => loadSurveyGridCalled.ShouldBeTrue(),
                () => _defineQuestionsInstance.ErrorMessage.ShouldBeNullOrEmpty());
        }

        [Test]
        public void BtnQuestionSave_Click_Exception_Error()
        {
            // Arrange
            InitTestBtnQuestionSaveClick(
                lblquestionIDText: "10",
                positionSelected: true,
                questionSelected: true,
                gridTypeSelected: true,
                questionFormat: "grid",
                rbRequiredValue: "1");
            var ecnErrors = new List<ECNError>() { new ECNError() { ErrorMessage = "error" } };
            var ecnException = new ECNException(ecnErrors);
            ShimQuestion.GetByQuestionIDInt32 = (id) => throw ecnException;

            // Act
            _defineQuestionsPrivateObject.Invoke(BtnQuestionSaveClickMethod, new object[] { null, new ImageClickEventArgs(0, 0) });

            // Assert
            _defineQuestionsInstance.ErrorMessage.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void OnInit_ControlsInitialized()
        {
            // Arrange
            var handlerMethodNames = new List<string> {
                "btnPageSave_Click" ,
                "btnPageCancel_Click" ,
                "btnQuestionSave_Click" ,
                "btnQuestionCancel_Click"};
            var subscripedHandlersNames = new List<string>();
            ShimImageButton.AllInstances.ClickAddImageClickEventHandler = (targetBtn, handler) =>
            {
                subscripedHandlersNames.Add(handler.Method.Name);
            };

            // Act
            _defineQuestionsPrivateObject.Invoke(InitializeComponentMethod, new object[] { });

            // Assert
            handlerMethodNames.RemoveAll(handlerName => subscripedHandlersNames.Contains(handlerName));
            handlerMethodNames.ShouldBeEmpty();
        }

        [Test]
        public void GetResponseOptionXML_ExpectedResult()
        {
            // Arrange
            var expectedResult = InitTestGetResponseOptionXML();
            
            // Act
            var result = _defineQuestionsPrivateObject.Invoke(GetResponseOptionXMLMethod, new object[] { });

            // Assert
            result.ShouldBe(expectedResult);
        }

        [TestCase("", "0", "0")]
        [TestCase("10", "1", "0")]
        [TestCase("1", "1", "1")]
        public void BtnPageSave_ClickMethod_PageSaved(string pageId, string drpPositionSelectedValue, string drpPageSelectedValue)
        {
            // Arrange
            InitTestBtnPageSaveClickMethod(pageId, drpPositionSelectedValue, drpPageSelectedValue);
            var loadSurveyGridMethodCalled = false;
            ShimDefineQuestions.AllInstances.LoadSurveyGridInt32 = (p, id) => { loadSurveyGridMethodCalled = true; };

            // Act
            var result = _defineQuestionsPrivateObject.Invoke(BtnPageSaveClickMethod, new object[] { null, new ImageClickEventArgs(0, 0) });

            // Assert
            loadSurveyGridMethodCalled.ShouldSatisfyAllConditions(
                () => loadSurveyGridMethodCalled.ShouldBeTrue(),
                () => _drpPosition.SelectedValue.ShouldBe("1"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BtnReOrderSave_Click_Reordered(bool plPReorderVisible)
        {
            // Arrange
            var loadSurveyGridMethodCalled = false;
            ShimDefineQuestions.AllInstances.LoadSurveyGridInt32 = (p, id) => { loadSurveyGridMethodCalled = true; };
            InitTestBtnReOrderSaveClick();

            // Act
            var result = _defineQuestionsPrivateObject.Invoke(BtnReOrderSaveClickMethod, new object[] { null, new ImageClickEventArgs(0, 0) });

            // Assert
            loadSurveyGridMethodCalled.ShouldBeTrue();
        }

        [TestCase(true, "pageHeader")]
        [TestCase(false, null)]
        public void RepPages_ItemCommand_AddCommand_ControlsInitialized(bool emptyQuestionCollection, string pageHeader)
        {
            // Arrange
            _drpQuestion.Items.Add(new ListItem("0", "0"));
            _drpQPosition.Items.Add(new ListItem("a", "a"));
            var arg = new CommandEventArgs("add", "1");
            DataLayerFakes.ShimPage.GetSqlCommand = (cmd) => new CollectorEntities.Page()
            {
                PageHeader = pageHeader
            };
            var questionsCollection = new List<CollectorEntities.Question>();
            if (!emptyQuestionCollection)
            {
                questionsCollection.Add(new CollectorEntities.Question());
            }
            DataLayerFakes.ShimQuestion.GetListSqlCommand = (cmd) => questionsCollection;

            // Act
            _defineQuestionsInstance.repPages_ItemCommand(null, new RepeaterCommandEventArgs(null, null, arg));

            // Assert
            _plQuestionno.ShouldSatisfyAllConditions(
                () => _plQuestionno.Visible.ShouldBeFalse(),
                () => _lblpageID.Text.ShouldBe("1"),
                () => _drpQPosition.SelectedValue.ShouldBe("a"),
                () => _plQposition.Visible.ShouldBe(!emptyQuestionCollection));
        }

        [TestCase(true, "radio")]
        [TestCase(false, "dropdown")]
        public void RepPages_ItemCommand_BranchCommand_ControlsInitialized(bool emptyBranchingQuestionList, string format)
        {
            // Arrange
            var arg = new CommandEventArgs("branch", "1");
            _plbranch.Visible = false;
            InitTestRepPagesItemCommandBranchCommand(emptyBranchingQuestionList, format);

            // Act
            _defineQuestionsInstance.repPages_ItemCommand(null, new RepeaterCommandEventArgs(null, null, arg));

            // Assert
            _plQuestionno.ShouldSatisfyAllConditions(
                () => _lblpageID.Text.ShouldBe("1"),
                () => _drpBQuestion.SelectedValue.ShouldBe(emptyBranchingQuestionList
                        ? "0"
                        : "1"),
                () => _plbranch.Visible.ShouldBe(!emptyBranchingQuestionList));
        }

        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _responseOptionsPropertyWrapper = string.Empty;
            _questionDeleteMethodCallCount = 0;
            _itemCommandRegisteredStartupScript = false;
            _defineQuestionsInstance = new DefineQuestions();
            _shimDefineQuestions = new ShimDefineQuestions(_defineQuestionsInstance);
            _defineQuestionsPrivateObject = new PrivateObject(_defineQuestionsInstance);
            InitializeAllControls(_defineQuestionsInstance);
            _shimDefineQuestions.ResponseoptionsGet = () => _responseOptionsPropertyWrapper;
            _shimDefineQuestions.ResponseoptionsSetString = (responseOptions) => _responseOptionsPropertyWrapper = responseOptions;
            ShimQuestion.DeleteInt32User = (i, u) => _questionDeleteMethodCallCount++;
            SetPageControls();
            var shimEcnSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimEcnSession;
            ECNSession.CurrentSession().CurrentUser = new KMPlatform.Entity.User() { UserID = 10 };
        }

        private void InitTestRepPagesItemCommandBranchCommand(bool emptyBranchingQuestionList, string format)
        {
            DataLayerFakes.ShimPage.GetSqlCommand = (cmd) => new CollectorEntities.Page();
            var questionsCollection = new List<CollectorEntities.Question>();
            questionsCollection.Add(new CollectorEntities.Question()
            {
                Format = format,
                Number = 1,
                QuestionText = "questionText",
                QuestionID = 1
            });
            var branchQuestionsCollection = new List<CollectorEntities.Question>();
            if (!emptyBranchingQuestionList)
            {
                branchQuestionsCollection.Add(new CollectorEntities.Question()
                {
                    QuestionID = 1
                });
            }
            DataLayerFakes.ShimQuestion.GetListSqlCommand = (cmd) =>
            {
                if (cmd.CommandText.Contains("SurveyBranching"))
                {
                    return branchQuestionsCollection;
                }
                return questionsCollection;
            };
            DataLayerFakes.ShimResponseOptions.GetListSqlCommand = (cmd) => new List<CollectorEntities.ResponseOptions>();
        }

        private void InitTestBtnReOrderSaveClick()
        {
            _lblpageID.Text = "1";
            _lblquestionID.Text = "20";
            _drpRPPosition.Items.Add(new ListItem("1", "1") { Selected = true });
            _drpRToPage.Items.Add(new ListItem("1", "1") { Selected = true });
            _drpRToQuestion.Items.Add(new ListItem("1", "1") { Selected = true });
            _drpRQPosition.Items.Add(new ListItem("1", "1") { Selected = true });
            _drpQPage.SelectedValue = "1";
            ShimDefineQuestions.AllInstances.Loaddropdowns = (p) => { };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) => 1;
            ShimDataFunctions.ExecuteSqlCommandString = (cmd, conn) => 1;
        }

        private void InitTestBtnPageSaveClickMethod(string pageId, string drpPositionSelectedValue, string drpPageSelectedValue)
        {
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) => 1;
            _lblpageID.Text = pageId;
            ShimDefineQuestions.AllInstances.Loaddropdowns = (p) => { };
            _drpPosition.Items.Clear();
            _drpPosition.Items.Add(new ListItem("0", "0") { Selected = drpPositionSelectedValue == "0" });
            _drpPosition.Items.Add(new ListItem("1", "1") { Selected = drpPositionSelectedValue == "1" });
            _drpPages.Items.Clear();
            _drpPages.Items.Add(new ListItem("0", "0") { Selected = drpPageSelectedValue == "0" });
            _drpPages.Items.Add(new ListItem("1", "1") { Selected = drpPageSelectedValue == "1" });
        }

        private string InitTestGetResponseOptionXML()
        {
            var counter = 0;
            _gvResponseOption.EditIndex = 0;
            ShimControl.AllInstances.FindControlString = (targetControl, id) =>
            {
                if (id == IdLblOption)
                {
                    return new Label() { Text = $"op{counter++}" }; ;
                }
                else if (id == IdLblScore)
                {
                    return new Label() { Text = $"Score{counter++}" };
                }
                else if (id == IdTxtOptionE)
                {
                    return new TextBox() { Text = $"OptionE{counter++}" };
                }
                else if (id == IdTxtScoreE)
                {
                    return new TextBox() { Text = $"ScoreE{counter++}" };
                }
                return null;
            };
            ShimGridView.AllInstances.RowsGet = (grid) =>
            {
                return new GridViewRowCollection(new ArrayList()
                {
                    new GridViewRow(0,0, DataControlRowType.DataRow, DataControlRowState.Normal),
                    new GridViewRow(1,0, DataControlRowType.DataRow, DataControlRowState.Normal)
                });
            };
            return "<options><option score='ScoreE1'><![CDATA[OptionE0]]></option><option score='Score3'><![CDATA[op2]]></option></options>";
        }

        private void InitTestBtnQuestionSaveClick(string lblquestionIDText, bool gridTypeSelected, string rbRequiredValue, bool positionSelected, bool questionSelected, string questionFormat)
        {
            _lblquestionID.Text = lblquestionIDText;
            _lblpageID.Text = "10";
            _txtMaxChars.Text = "600";
            _rbRequired.SelectedValue = rbRequiredValue;
            _rbQuestionFormat.SelectedValue = questionFormat;
            _txtOptions.Text = "options";
            _txtGridRow.Text = "gridRow";
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                if (cmd.CommandText == "sp_SaveQuestion")
                {
                    return 10;
                }
                return 0;
            };
            DataLayerFakes.ShimQuestion.GetSqlCommand = (cmd) => new CollectorEntities.Question() { SurveyID = 10 };
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => new CollectorEntities.Survey();
            ShimDefineQuestions.AllInstances.getResponseOptionXML = (p) => "options";
            if (gridTypeSelected)
            {
                _rbGridType.SelectedValue = "";
            }
            if (questionSelected)
            {
                _drpQuestion.SelectedValue = "1";
            }
            else
            {
                _drpQuestion.Items.Clear();
            }
            if (positionSelected)
            {
                _drpQPosition.SelectedValue = "1";
            }
            else
            {
                _drpQPosition.Items.Clear();
            }
            DataLayerFakes.ShimQuestion.GetSqlCommand = (command) => new CollectorEntities.Question();
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimGroupDataFields.SaveGroupDataFieldsUser = (grp, user) => 1;
            ECN_Framework_DataLayer.Communicator.Fakes.ShimGroupDataFields.GetSqlCommand = (cmd) => null;
            ShimDefineQuestions.AllInstances.Loaddropdowns = (p) => { };
        }
    }
}
