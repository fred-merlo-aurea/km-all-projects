using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using CKEditor.NET;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_Entities.Collector;
using NUnit.Framework;
using Shouldly;

namespace ECN.Collector.Tests.Main.Survey.UserControls
{
    public partial class DefineQuestionsTest
    {
        private const int ScoreToTest = 500500;
        private Label _lblquestionID;
        private Label _lblQno;
        private Label _lblRQNo;
        private Label _lblpageID;
        private Label _lblQPageno;
        private Label _lblReorderTitle;
        private TextBox _txtMaxChars;
        private TextBox _txtOptions;
        private RadioButtonList _rbQuestionFormat;
        private RadioButtonList _rbRequired;
        private RadioButtonList _rbAddTextbox;
        private RadioButtonList _rbGridRequired;
        private RadioButtonList _rbGridType;
        private PlaceHolder _plQposition;
        private PlaceHolder _plQuestionno;
        private PlaceHolder _plPReorder;
        private PlaceHolder _plQReorder;
        private DropDownList _drpQPage;
        private DropDownList _drpRToQuestion;
        private CKEditorControl _txtQuestion;
        private GridView _gvResponseOption;
        private TextBox _txtGridRow;
        private DropDownList _drpPages;
        private DropDownList _drpPosition;
        private Repeater _repPages;
        private DropDownList _drpQPosition;
        private DropDownList _drpQuestion;
        private DropDownList _drpRPPosition;
        private DropDownList _drpRToPage;
        private DropDownList _drpRQPosition;
        private PlaceHolder _plbranch;
        private DropDownList _drpBQuestion;

        [TestCase(true, null)]
        [TestCase(false, "pageHeader")]
        public void RpQuestionsGrid_itemcommand_EditTextbox_NoErrors(bool surveyExist, string pageHeader)
        {
            // Arrange
            InitTest_RpQuestionsGrid_Itemcommand(question: out Question question, page: out Page page, pageHeader: pageHeader, pageNumber: 1, questionFormat: "textbox", commandArg: "10", commandName: "edit", repeaterArg: out RepeaterCommandEventArgs e, surveyBranchingExit: surveyExist);

            // Act
            _defineQuestionsInstance.rpQuestionsGrid_itemcommand(null, e);

            // Assert
            _txtMaxChars.ShouldSatisfyAllConditions(
                 () => _rbQuestionFormat.Enabled.ShouldBe(!surveyExist),
                 () => _txtOptions.Enabled.ShouldBe(!surveyExist),
                 () => _lblquestionID.Text.ShouldBe(e.CommandArgument.ToString()),
                 () => _txtQuestion.Text.ShouldBe(question.QuestionText),
                 () => _lblpageID.Text.ShouldBe(question.PageID.ToString()),
                 () => _plQposition.Visible.ShouldBeFalse(),
                 () => _plQuestionno.Visible.ShouldBeTrue(),
                 () => _lblQno.Text.ShouldBe(question.Number.ToString()),
                 () => _txtMaxChars.Text.ShouldBe(question.MaxLength.ToString()),
                 () => _itemCommandRegisteredStartupScript.ShouldBeTrue());
        }

        [Test]
        public void RpQuestionsGrid_itemcommand_EditGrid_NoErrors()
        {
            // Arrange
            InitTest_RpQuestionsGrid_Itemcommand(question: out Question question, page: out Page page, pageHeader: "pageHeader", pageNumber: 1, questionFormat: "grid", commandArg: "10", commandName: "edit", repeaterArg: out RepeaterCommandEventArgs e);

            var roList = new List<ResponseOptions>();
            roList.Add(new ResponseOptions()
            {
                OptionID = 1,
                OptionValue = "ov1",
                QuestionID = 1,
                Score = 0
            });
            roList.Add(new ResponseOptions()
            {
                OptionID = 2,
                OptionValue = "ov2",
                QuestionID = 2,
                Score = ScoreToTest
            });
            ShimResponseOptions.GetByQuestionIDInt32User = (i, u) => roList;
            var gridStatement = new List<GridStatements>();
            gridStatement.Add(new GridStatements() { GridStatement = "gridStatement" });
            ShimGridStatements.GetByQuestionIDInt32User = (i, u) => gridStatement;

            // Act
            _defineQuestionsInstance.rpQuestionsGrid_itemcommand(null, e);

            // Assert
            _defineQuestionsInstance.ShouldSatisfyAllConditions(
                () => _defineQuestionsInstance.Responseoptions.ShouldContain("ov1"),
                () => _defineQuestionsInstance.Responseoptions.ShouldContain("ov2"),
                () => _defineQuestionsInstance.Responseoptions.ShouldContain(ScoreToTest.ToString()),
                () => _txtMaxChars.Text.ShouldBe("499"),
                () => _txtGridRow.Text.ShouldBe("gridStatement" + (char)10),
                () => _itemCommandRegisteredStartupScript.ShouldBeTrue());
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void RpQuestionsGrid_itemcommand_DeleteCommand_DeleteCalled(int selectedPageIndex)
        {
            // Arrange
            var dataKeyCol = new DataKeyCollection(new ArrayList() { 1 });
            ShimBaseDataList.AllInstances.DataKeysGet = (d) => dataKeyCol;
            var dlPagedList = new DataList();
            dlPagedList.SelectedIndex = selectedPageIndex;
            _defineQuestionsPrivateObject.SetField("dlPages", BindingFlags.Instance | BindingFlags.NonPublic, dlPagedList);
            var pagesList = new List<Page>();
            pagesList.Add(new Page() { PageID = 500 });
            ECN_Framework_BusinessLayer.Collector.Fakes.ShimPage.GetBySurveyIDInt32User = (i, u) => pagesList;
            ECN_Framework_BusinessLayer.Collector.Fakes.ShimPage.BranchFromExistsInt32User = (i, u) => true;
            _defineQuestionsPrivateObject.SetField("chkShowAllPages", BindingFlags.Instance | BindingFlags.NonPublic, new CheckBox() { Checked = false });
            _defineQuestionsInstance.SurveyID = 1;
            InitTest_RpQuestionsGrid_Itemcommand(question: out Question question, page: out Page page, pageHeader: "pageHeader", pageNumber: 1, questionFormat: string.Empty, commandArg: "10", commandName: "delete", repeaterArg: out RepeaterCommandEventArgs e);

            // Act
            _defineQuestionsInstance.rpQuestionsGrid_itemcommand(null, e);

            // Assert
            var dropQDataSource = dlPagedList.DataSource as IList;
            var pageResult = dropQDataSource[0];

            dropQDataSource.ShouldSatisfyAllConditions(
                () => dropQDataSource.ShouldNotBeNull(),
                () => dropQDataSource.Count.ShouldBeGreaterThan(0),
                () => pageResult.GetType().GetProperty("PageID").GetValue(pageResult).ToString().ShouldBe("500"),
                () => _questionDeleteMethodCallCount.ShouldBe(1));
        }

        [TestCase(true, "longQuestionText")]
        [TestCase(false, "shortQuestionText")]
        public void RpQuestionsGrid_itemcommand_ReorderCommand_NoErrors(bool surveyExist, string questionText)
        {
            // Arrange
            if (questionText == "longQuestionText")
            {
                questionText = "this is very long question text test.......................";
            }
            InitTest_RpQuestionsGrid_Itemcommand(question: out Question question, page: out Page page, pageHeader: string.Empty, pageNumber: 1, questionFormat: string.Empty, commandArg: "10", commandName: "reorder", repeaterArg: out RepeaterCommandEventArgs e, surveyBranchingExit: surveyExist);
            var questions = new List<Question>();
            questions.Add(new Question() { QuestionID = 950 });
            ShimQuestion.GetByPageIDInt32User = (i, u) => questions;
            var pagesList = new List<Page>();
            pagesList.Add(new Page() { PageID = 1 });
            ECN_Framework_BusinessLayer.Collector.Fakes.ShimPage.GetBySurveyIDInt32User = (i, u) => pagesList;

            // Act
            _defineQuestionsInstance.rpQuestionsGrid_itemcommand(null, e);

            // Assert
            var dropQDataSource = _drpQPage.DataSource as IList;
            var dropQDataSourcePage = dropQDataSource[0];
            var dropRDataSource = _drpRToQuestion.DataSource as IList;
            var dropRDataSourcePage = dropRDataSource[0];
            dropQDataSource.ShouldSatisfyAllConditions(
                    () => dropQDataSource.ShouldNotBeNull(),
                    () => dropQDataSource.Count.ShouldBeGreaterThan(0),
                    () => dropQDataSourcePage.GetType().GetProperty("PageID").GetValue(dropQDataSourcePage).ToString().ShouldBe("1"),
                    () => dropRDataSource.ShouldNotBeNull(),
                    () => dropRDataSource.Count.ShouldBeGreaterThan(0),
                    () => dropRDataSourcePage.GetType().GetProperty("DisplayValue").GetValue(dropRDataSourcePage).ToString().ShouldBe("950"),
                    () => _lblReorderTitle.Text.ShouldBe("Re-order Question"),
                    () => _lblquestionID.Text.ShouldBe(e.CommandArgument.ToString()),
                    () => _drpQPage.Enabled.ShouldBe(!surveyExist),
                    () => _plQReorder.Visible.ShouldBeTrue(),
                    () => _plPReorder.Visible.ShouldBeFalse(),
                    () => _lblRQNo.Text.ShouldBe(question.Number.ToString() + ". " + question.QuestionText == null ? "" : ((question.QuestionText.Length > 50) ? question.QuestionText.Substring(0, 49) : question.QuestionText)),
                    () => _itemCommandRegisteredStartupScript.ShouldBeTrue());
        }

        private void InitTest_RpQuestionsGrid_Itemcommand(out Question question, string questionFormat, out Page page, string pageHeader, int pageNumber, out RepeaterCommandEventArgs repeaterArg, string commandName, object commandArg, bool surveyBranchingExit = false)
        {
            var roList = new List<ResponseOptions>();
            question = new Question();
            question.Format = questionFormat;
            question.PageID = 1;
            question.Number = 1;
            question.QuestionText = "questionText";
            question.GridValidation = 1;
            question.MaxLength = 100;
            var getByQuestionResult = question;
            page = new Page();
            page.PageHeader = pageHeader;
            page.Number = pageNumber;
            var getByPageIDResult = page;
            var shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new KMPlatform.Entity.User() { UserID = 0 };
            ShimECNSession.CurrentSession = () => shimECNSession;
            ECN_Framework_BusinessLayer.Collector.Fakes.ShimPage.GetByPageIDInt32User = (id, u) => getByPageIDResult;
            repeaterArg = new RepeaterCommandEventArgs(null, null, new CommandEventArgs(commandName, commandArg));
            ShimQuestion.GetByQuestionIDInt32 = (id) => getByQuestionResult;
            ShimResponseOptions.GetByQuestionIDInt32User = (i, u) => roList;
            ShimGridStatements.GetByQuestionIDInt32User = (i, u) => null;
            ShimSurveyBranching.ExistsInt32User = (i, u) => surveyBranchingExit;
            SetPageControls();
        }
        private void SetPageControls()
        {
            _lblquestionID = new Label();
            _defineQuestionsPrivateObject.SetField("lblquestionID", BindingFlags.Instance | BindingFlags.NonPublic, _lblquestionID);
            _lblQno = new Label();
            _defineQuestionsPrivateObject.SetField("lblQno", BindingFlags.Instance | BindingFlags.NonPublic, _lblQno);
            _lblRQNo = new Label();
            _defineQuestionsPrivateObject.SetField("lblRQNo", BindingFlags.Instance | BindingFlags.NonPublic, _lblRQNo);
            _lblpageID = new Label();
            _defineQuestionsPrivateObject.SetField("lblpageID", BindingFlags.Instance | BindingFlags.NonPublic, _lblpageID);
            _lblQPageno = new Label();
            _defineQuestionsPrivateObject.SetField("lblQPageno", BindingFlags.Instance | BindingFlags.NonPublic, _lblQPageno);
            _lblReorderTitle = new Label();
            _defineQuestionsPrivateObject.SetField("lblReorderTitle", BindingFlags.Instance | BindingFlags.NonPublic, _lblReorderTitle);
            _txtMaxChars = new TextBox();
            _defineQuestionsPrivateObject.SetField("txtMaxChars", BindingFlags.Instance | BindingFlags.NonPublic, _txtMaxChars);
            _txtOptions = new TextBox();
            _defineQuestionsPrivateObject.SetField("txtOptions", BindingFlags.Instance | BindingFlags.NonPublic, _txtOptions);
            _rbQuestionFormat = new RadioButtonList();
            _rbQuestionFormat.Items.Add(new ListItem("grid", "grid"));
            _rbQuestionFormat.Items.Add(new ListItem("textbox", "textbox"));
            _rbQuestionFormat.Items.Add(new ListItem("radio", "radio"));
            _defineQuestionsPrivateObject.SetField("rbQuestionFormat", BindingFlags.Instance | BindingFlags.NonPublic, _rbQuestionFormat);
            _rbRequired = new RadioButtonList();
            _rbRequired.Items.Add(new ListItem("0", "0"));
            _rbRequired.Items.Add(new ListItem("1", "1"));
            _defineQuestionsPrivateObject.SetField("rbRequired", BindingFlags.Instance | BindingFlags.NonPublic, _rbRequired);
            _rbAddTextbox = new RadioButtonList();
            _rbAddTextbox.Items.Add(new ListItem("0", "0"));
            _rbAddTextbox.Items.Add(new ListItem("1", "1"));
            _defineQuestionsPrivateObject.SetField("rbAddTextbox", BindingFlags.Instance | BindingFlags.NonPublic, _rbAddTextbox);
            _rbGridRequired = new RadioButtonList();
            _rbGridRequired.Items.Add(new ListItem("0", "0"));
            _rbGridRequired.Items.Add(new ListItem("1", "1"));
            _defineQuestionsPrivateObject.SetField("rbGridRequired", BindingFlags.Instance | BindingFlags.NonPublic, _rbGridRequired);
            _rbGridType = new RadioButtonList();
            _rbGridType.Items.Add(new ListItem("", ""));
            _defineQuestionsPrivateObject.SetField("rbGridType", BindingFlags.Instance | BindingFlags.NonPublic, _rbGridType);
            _plQposition = new PlaceHolder();
            _defineQuestionsPrivateObject.SetField("plQposition", BindingFlags.Instance | BindingFlags.NonPublic, _plQposition);
            _plQuestionno = new PlaceHolder();
            _defineQuestionsPrivateObject.SetField("plQuestionno", BindingFlags.Instance | BindingFlags.NonPublic, _plQuestionno);
            _plPReorder = new PlaceHolder();
            _defineQuestionsPrivateObject.SetField("plPReorder", BindingFlags.Instance | BindingFlags.NonPublic, _plPReorder);
            _plQReorder = new PlaceHolder();
            _defineQuestionsPrivateObject.SetField("plQReorder", BindingFlags.Instance | BindingFlags.NonPublic, _plQReorder);
            _drpQPage = new DropDownList();
            _drpQPage.Items.Add(new ListItem("0", "0"));
            _drpQPage.Items.Add(new ListItem("1", "1"));
            _defineQuestionsPrivateObject.SetField("drpQPage", BindingFlags.Instance | BindingFlags.NonPublic, _drpQPage);
            _drpRToQuestion = new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpRToQuestion", BindingFlags.Instance | BindingFlags.NonPublic, _drpRToQuestion);
            _txtQuestion = new CKEditorControl();
            _defineQuestionsPrivateObject.SetField("txtQuestion", BindingFlags.Instance | BindingFlags.NonPublic, _txtQuestion);
            _gvResponseOption = new GridView();
            _defineQuestionsPrivateObject.SetField("gvResponseOption", BindingFlags.Instance | BindingFlags.NonPublic, _gvResponseOption);
            _txtGridRow = new TextBox();
            _defineQuestionsPrivateObject.SetField("txtGridRow", BindingFlags.Instance | BindingFlags.NonPublic, _txtGridRow);
            _drpPages = new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpPages", BindingFlags.Instance | BindingFlags.NonPublic, _drpPages);
            _repPages = new Repeater();
            _defineQuestionsPrivateObject.SetField("repPages", BindingFlags.Instance | BindingFlags.NonPublic, _repPages);
            ShimClientScriptManager.AllInstances.RegisterStartupScriptTypeStringString = (csm, t, s1, s2) => _itemCommandRegisteredStartupScript = true;
            var uiPage = new System.Web.UI.Page();
            _defineQuestionsInstance.Page = uiPage;
            _drpQPosition = new DropDownList();
            _drpQPosition.Items.Add(new ListItem("1", "1"));
            _defineQuestionsPrivateObject.SetField("drpQPosition", BindingFlags.Instance | BindingFlags.NonPublic, _drpQPosition);
            _drpQuestion = new DropDownList();
            _drpQuestion.Items.Add(new ListItem("1", "1"));
            _defineQuestionsPrivateObject.SetField("drpQuestion", BindingFlags.Instance | BindingFlags.NonPublic, _drpQuestion);
            _drpPosition = new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpPosition", BindingFlags.Instance | BindingFlags.NonPublic, _drpPosition);
            _drpRPPosition = new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpRPPosition", BindingFlags.Instance | BindingFlags.NonPublic, _drpRPPosition);
            _drpRToPage = new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpRToPage", BindingFlags.Instance | BindingFlags.NonPublic, _drpRToPage);
            _drpRQPosition= new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpRQPosition", BindingFlags.Instance | BindingFlags.NonPublic, _drpRQPosition);
            _plbranch = new PlaceHolder();
            _defineQuestionsPrivateObject.SetField("plbranch", BindingFlags.Instance | BindingFlags.NonPublic, _plbranch);
            _drpBQuestion = new DropDownList();
            _defineQuestionsPrivateObject.SetField("drpBQuestion", BindingFlags.Instance | BindingFlags.NonPublic, _drpBQuestion);
        }
    }
}
