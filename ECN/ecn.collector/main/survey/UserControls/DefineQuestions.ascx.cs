using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Collector;
using KM.Common;
using KM.Common.Functions;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using BusinessCollector = ECN_Framework_BusinessLayer.Collector;
using BusinessApplication = ECN_Framework_BusinessLayer.Application;
using StringFunctions = ECN_Framework_Common.Functions.StringFunctions;
using UIPage = System.Web.UI.Page;

namespace ecn.collector.main.survey.UserControls
{
    public partial class DefineQuestions : System.Web.UI.UserControl, IWizard
    {
        private const string ErrorParseIntTemplate = "Couldn't parse {0} to int.";
        private const string CommandEdit = "edit";
        private const string QuestionFormatTextBox = "textbox";
        private const string QuestionFormatGrid = "grid";
        private const int TextMaxChars = 499;
        private const string HtmlScriptTemplate = 
            "<script language='javascript'>onload=function(){showQ();};</script>";
        private const string ScriptKeyLoadPage = "loadPage";
        private const string CommandDelete = "delete";
        private const string CommandReorder = "reorder";
        private const string FieldHeader = "header";
        private const string FieldPageId = "PageID";
        private const string FieldDisplayText = "DisplayText";
        private const int QuestionText50 = 50;
        private const string FieldDisplayValue = "DisplayValue";
        private const string ItemTextSelectQuestion = "Select Question";
        private const string ItemValueNone = "0";

        #region Interface Methods & Properties
        int _surveyID = 0;
        public int SurveyID
        {
            set
            {
                _surveyID = value;
            }
            get
            {
                return _surveyID;
            }
        }
        
        string _errormessage = string.Empty;

        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }

        public string Responseoptions
        {
            set
            {
                Session["Responseoptions"] = value;
            }
            get
            {
                try
                {
                    return Session["Responseoptions"].ToString();
                }
                catch
                {
                    Session["Responseoptions"] = "";
                    return Session["Responseoptions"].ToString();
                }
            }
        }

        public void Initialize()
        {

            if (SurveyID != 0)
            {
                try
                {
                    

                    chkShowAllPages.Checked = true;
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);					
					
                    LoadSurveyGrid(dlPages.SelectedIndex > -1 ? Convert.ToInt32(dlPages.DataKeys[dlPages.SelectedIndex]) : 0);
                    Loaddropdowns();

                    lbpAdd.Attributes.Add("onclick", "javascript:return showP();");

                    if (objSurvey.ResponseCount>0)
                        lbpAdd.Visible = false;

                    btnQuestionSave.Attributes.Add("onclick", "javascript:return qValidate();");
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
        }
        
        public bool Save()
        {
            try
            {
                List<ECN_Framework_Entities.Collector.Question> qList = ECN_Framework_BusinessLayer.Collector.Question.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                int qCount = qList.Count;

                if (qCount > 0)
                {
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);									
                    objSurvey.CompletedStep = 2;
                    ECN_Framework_BusinessLayer.Collector.Survey.Save(objSurvey, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                    string shortName = objSurvey.SurveyID.ToString() + "_completionDt";
                    ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(shortName, objSurvey.GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (gdf == null)
                    {
                        gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                        gdf.GroupID = objSurvey.GroupID;
                        gdf.ShortName = shortName;
                        gdf.LongName = shortName;
                        gdf.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        gdf.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                        gdf.IsPublic = "N";
                        gdf.SurveyID = SurveyID;
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    }

                    string shortName_BlastID = objSurvey.SurveyID.ToString() + "_blastID";
                    ECN_Framework_Entities.Communicator.GroupDataFields gdf_BlastID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(shortName_BlastID, objSurvey.GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (gdf_BlastID == null)
                    {
                        gdf_BlastID = new ECN_Framework_Entities.Communicator.GroupDataFields();
                        gdf_BlastID.GroupID = objSurvey.GroupID;
                        gdf_BlastID.ShortName = shortName_BlastID;
                        gdf_BlastID.LongName = shortName_BlastID;
                        gdf_BlastID.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        gdf_BlastID.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                        gdf_BlastID.IsPublic = "N";
                        gdf_BlastID.SurveyID = SurveyID;
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf_BlastID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    }


                    return true;
                }
                else
                {
                    ErrorMessage = "ERROR: Survey should have at least one question.";
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }
        #endregion

        #region Data Load
        private void Loaddropdowns()
        {
            if (SurveyID > 0)
            {
                List<ECN_Framework_Entities.Collector.Page> pageList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                var result = (from src in pageList
                              orderby src.Number
                              select new
                              {
                                  PageID = src.PageID,
                                  header = "P. " + src.Number + " - " + ((src.PageHeader.Length > 75) ? src.PageHeader.Substring(0, 74) : src.PageHeader),
                                  PageDesc = src.PageDesc,
                                  number = src.Number
                              }).ToList();
                drpPages.DataSource = result;
                drpPages.DataTextField = "header";
                drpPages.DataValueField = "number";
                drpPages.DataBind();

                drpPages.Items.Insert(0, new ListItem("Last Page", "0"));
            }
        }
        
        private void LoadSurveyGrid(int PageID)
        {
            int pgCount = 0;
            string bgcolor = string.Empty;
            if (SurveyID > 0)
            {
                List<ECN_Framework_Entities.Collector.Page> pList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                Dictionary<int, bool> BranchingExists = new Dictionary<int, bool>();
                foreach (ECN_Framework_Entities.Collector.Page p in pList)
                {
                    BranchingExists.Add(p.PageID, ECN_Framework_BusinessLayer.Collector.Page.BranchFromExists(p.PageID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser));
                    pgCount = p.Number;
                }

                var pagesOrdered = (from src in pList
                               orderby src.Number descending 
                               select src).ToList();

                ECN_Framework_Entities.Collector.Page  lastPage = new   ECN_Framework_Entities.Collector.Page();
                try
                {
                    lastPage = pagesOrdered[0];
                }
                catch { }

                if (chkShowAllPages.Checked || PageID == 0)
                {
                    var result1 = (from src in pList
                                  orderby src.Number
                                  select new
                                  {
                                      PageID = src.PageID,
                                      PageHeader = src.PageHeader,
                                      PageDesc = src.PageDesc,
                                      number = src.Number,
                                      branchingExists = BranchingExists[src.PageID],
                                      IsLastPage = src.PageID == lastPage.PageID ? true : false,
                                      PageCount = pgCount
                                  }).ToList();
                    repPages.DataSource = result1;
                    bgcolor = "#ffffff";
                }
                else
                {
                    var result2 = (from src in pList
                                  orderby src.Number
                                  where src.PageID == PageID
                                  select new
                                  {
                                      PageID = src.PageID,
                                      PageHeader = src.PageHeader,
                                      PageDesc = src.PageDesc,
                                      number = src.Number,
                                      branchingExists = BranchingExists[src.PageID],
                                      IsLastPage = src.PageID == lastPage.PageID ? true : false,
                                      PageCount = pgCount
                                  }).ToList();
                    bgcolor = "#fdefd5";
                    repPages.DataSource = result2;
                }
                               
                repPages.DataBind();                
                var result = (from src in pList
                              orderby src.Number
                              select new
                              {
                                  PageID= src.PageID,
                                  PageDesc= src.PageDesc,
                                  PageHeader= src.PageHeader,
                                  number= src.Number,
                                  bgcolor = src.PageID == PageID ? bgcolor : "#ffffff"
                              }).ToList();
                dlPages.DataSource = result;
                dlPages.DataBind();
            }
        }


        public DataTable LoadQuestionGrid(int PageID)
        {
            return ECN_Framework_BusinessLayer.Collector.Question.GetQuestionsGridByPageID(PageID);
        }

        #endregion

        #region Page datalist events
        public void dlPages_itemcommand(object sender, DataListCommandEventArgs e)
        {
            ECN_Framework_Entities.Collector.Page page = ECN_Framework_BusinessLayer.Collector.Page.GetByPageID(Convert.ToInt32(e.CommandArgument.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            if (e.CommandName.ToLower() == "edit")
            {                
                if (page!=null)
                {
                    lblpageID.Text = e.CommandArgument.ToString();
                    txtPageHeader.Text = page.PageHeader;
                    txtPageDesc.Text = page.PageDesc;
                    plposition.Visible = false;
                    Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadPage", "<script language='javascript'>onload=function(){showP();};</script>");
                }
            }
            else if (e.CommandName.ToLower() == "delete")
            {
                ECN_Framework_BusinessLayer.Collector.Page.Delete(Convert.ToInt32(e.CommandArgument.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                dlPages.SelectedIndex = -1;
                LoadSurveyGrid(0);
                Loaddropdowns();
            }
            else if (e.CommandName.ToLower() == "reorder")
            {
                lblpageID.Text = e.CommandArgument.ToString();
                lblReorderTitle.Text = "Re-order Page";
                List<ECN_Framework_Entities.Collector.Page> pageList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                var result = (from src in pageList
                              where src.PageID!= Convert.ToInt32(e.CommandArgument.ToString())
                              orderby src.Number
                              select new
                              {
                                  PageID = src.PageID,
                                  header = "P. " + src.Number.ToString() + " - " + ((src.PageHeader.Length > 75) ? src.PageHeader.Substring(0, 74) : src.PageHeader),
                                  PageDesc = src.PageDesc,
                                  number = src.Number
                              }).ToList();
                drpRToPage.DataSource = result;
                drpRToPage.DataTextField = "header";
                drpRToPage.DataValueField = "PageID";
                drpRToPage.DataBind();

                drpRToPage.ClearSelection();
                drpRToPage.Items.Insert(0, new ListItem("Select Page", "0"));
                lblRPNo.Text = "<strong>" + page.Number.ToString() + "</strong> <br> " + page.PageHeader;
                plPReorder.Visible = true;
                plQReorder.Visible = false;
                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadPage", "<script language='javascript'>onload=function(){showR();};</script>");
            }
            else if (e.CommandName.ToLower() == "select")
            {
                chkShowAllPages.Checked = false;
                dlPages.SelectedIndex = e.Item.ItemIndex;
                LoadSurveyGrid(Convert.ToInt32(e.CommandArgument.ToString()));
            }
        }

        public void dlPages_itemdatabound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                LinkButton lbDelete = (LinkButton)e.Item.FindControl("lbpDelete");
                LinkButton lbpReorder = (LinkButton)e.Item.FindControl("lbpReorder");

                if (ECN_Framework_BusinessLayer.Collector.Page.BranchFromExists(Convert.ToInt32(dlPages.DataKeys[e.Item.ItemIndex].ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    lbpReorder.Attributes.Add("onclick", "javascript:alert('Branching Exists! Cannot Reorder this page');return false;");
                }

                if (ECN_Framework_BusinessLayer.Collector.Page.BranchToExists(Convert.ToInt32(dlPages.DataKeys[e.Item.ItemIndex].ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    lbpReorder.Attributes.Add("onclick", "javascript:alert('Branching Exists! Cannot Reorder this page');return false;");
                    lbDelete.Attributes.Add("onclick", "javascript:alert('Branching Exists! Cannot Delete this page');return false;");
                }
                else
                {
                    lbDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete this page?');");
                }

                if ( ECN_Framework_BusinessLayer.Collector.Survey.HasResponses(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    LinkButton lbpedit = (LinkButton)e.Item.FindControl("lbpedit");

                    lbpedit.Visible = false;
                    lbpReorder.Visible = false;
                    lbDelete.Visible = false;
                }
            }
        }
        #endregion

        #region Page Repeater events
        public void repPages_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            ECN_Framework_Entities.Collector.Page page = ECN_Framework_BusinessLayer.Collector.Page.GetByPageID(Convert.ToInt32(e.CommandArgument.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            List<ECN_Framework_Entities.Collector.Question> qList = ECN_Framework_BusinessLayer.Collector.Question.GetByPageID(Convert.ToInt32(e.CommandArgument), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);           
          
            if (e.CommandName.ToLower() == "add")
            {
                
                QReset();
                Responseoptions = "";               
                loadgrid();
                var result = (from src in qList
                              orderby src.Number
                              select new
                              {
                                  DisplayValue = src.QuestionID,
                                  DisplayText = string.IsNullOrEmpty(RemoveHTML(src.QuestionText)) ? "Q. " + src.Number.ToString() : ((RemoveHTML(src.QuestionText).Length > 50) ? RemoveHTML(src.QuestionText).Substring(0, 49) : RemoveHTML(src.QuestionText))
                              }).ToList();
                if (result.Count > 0)
                {
                    plQposition.Visible = true;                   
                    drpQuestion.DataSource = result;
                    drpQuestion.DataTextField = "DisplayText";
                    drpQuestion.DataValueField = FieldDisplayValue;
                    drpQuestion.DataBind();
                    drpQuestion.Items[result.Count - 1].Selected = true;
                }
                else
                {
                    plQposition.Visible = false;
                    drpQuestion.ClearSelection();
                    drpQuestion.Items.Insert(0, new ListItem("New Question", "0"));
                    drpQuestion.Items.FindByValue("0").Selected = true;
                }

                drpQPosition.ClearSelection();
                drpQPosition.Items.FindByValue("a").Selected = true;
                lblQPageno.Text = "<strong>" + page.Number + "</strong><br>" + page.PageHeader == null ? "" : page.PageHeader;
                lblpageID.Text = e.CommandArgument.ToString();
                plQuestionno.Visible = false;
                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadPage", "<script language='javascript'>onload=function(){showQ();};</script>");
            }
            else if (e.CommandName == "branch")
            {
                plbranch.Visible = false;
                lblpageID.Text = e.CommandArgument.ToString();
                drpBQuestion.ClearSelection();
                var result = (from src in qList
                              where src.Format == "radio" || src.Format == "dropdown"
                              orderby src.Number
                              select new
                              {
                                  DisplayValue = src.QuestionID,
                                  DisplayText = "Q. " + src.Number.ToString() + " - " + src.QuestionText == null ? "" : ((RemoveHTML(src.QuestionText).Length > 50) ? RemoveHTML(src.QuestionText).Substring(0, 49) : RemoveHTML(src.QuestionText))
                              }).ToList();
                drpBQuestion.DataSource = result;
                drpBQuestion.DataTextField = "DisplayText";
                drpBQuestion.DataValueField = FieldDisplayValue;
                drpBQuestion.DataBind();

                drpBQuestion.Items.Insert(0, new ListItem("Select Question", "0"));
                List<ECN_Framework_Entities.Collector.Question> branchingQuestionList = ECN_Framework_BusinessLayer.Collector.Question.GetBranchingQuestionsByPageID(Convert.ToInt32(e.CommandArgument.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                if (branchingQuestionList.Count > 0)
                {
                    plbranch.Visible = true;
                    drpBQuestion.ClearSelection();
                    drpBQuestion.Items.FindByValue(branchingQuestionList[0].QuestionID.ToString()).Selected = true;
                    List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(Convert.ToInt32(branchingQuestionList[0].QuestionID.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    dgBranch.DataSource = roList;
                    dgBranch.DataBind();
                }
                else
                {
                    dgBranch.DataBind();
                }

                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadPage", "<script language='javascript'>onload=function(){showB();};</script>");
            }

        }

        private string RemoveHTML(string dirty)
        {
            string final = "";

            final = HtmlFunctions.StripTextFromHtml(dirty);

            return final;
        }

        public void repPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                if (ECN_Framework_BusinessLayer.Collector.Survey.HasResponses(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    //ECN_Framework_Entities.Collector.Question currentQ = (ECN_Framework_Entities.Collector.Question)e.Item.DataItem;

                    LinkButton lbqAdd = (LinkButton)e.Item.FindControl("lbqAdd");
                    lbqAdd.Visible = false;

                    
                    LinkButton lbqBranch = (LinkButton)e.Item.FindControl("lbqBranch");
                    lbqBranch.Visible = false;
                }
                else
                {
                    LinkButton lbqBranch = (LinkButton)e.Item.FindControl("lbqBranch");
                    if (lbqBranch.Text.ToLower() == "hide")
                        lbqBranch.Visible = false;
                }
            }
        }

        #endregion

        #region question Repeater events
        public void rpQuestionsGrid_itemcommand(object sender, RepeaterCommandEventArgs eventArgse)
        {
            var commandArgument = ParseInt32WithThrow(eventArgse.CommandArgument?.ToString());
            var question = BusinessCollector.Question.GetByQuestionID(commandArgument);
            if (string.Equals(eventArgse.CommandName, CommandEdit, StringComparison.OrdinalIgnoreCase))
            {
                var questionId = ParseInt32WithThrow(eventArgse.CommandArgument?.ToString());
                EditQuestions(questionId, question);
            }
            else if (string.Equals(eventArgse.CommandName, CommandDelete, StringComparison.OrdinalIgnoreCase))
            {
                BusinessCollector.Question.Delete(
                    ParseInt32WithThrow(eventArgse.CommandArgument?.ToString()), 
                    BusinessApplication.ECNSession.CurrentSession().CurrentUser);
                LoadSurveyGrid(dlPages.SelectedIndex > -1 ?
                    ParseInt32WithThrow(dlPages.DataKeys[dlPages.SelectedIndex].ToString()) : 
                    0);
                Loaddropdowns();
            }
            else if (string.Equals(eventArgse.CommandName, CommandReorder, StringComparison.OrdinalIgnoreCase))
            {
                var questionId = ParseInt32WithThrow(eventArgse.CommandArgument?.ToString());
                ReoderQuestions(questionId, question);
            }
        }

        private void EditQuestions(int questionId, Question question)
        {
            if (question != null)
            {
                lblquestionID.Text = questionId.ToString();
                var page = BusinessCollector.Page.GetByPageID(
                    question.PageID,
                    BusinessApplication.ECNSession.CurrentSession().CurrentUser);
                rbQuestionFormat.ClearSelection();
                rbQuestionFormat.Items.FindByValue(question.Format.ToLower()).Selected = true;
                txtQuestion.Text = question.QuestionText;
                rbRequired.ClearSelection();
                rbRequired.Items.FindByValue(Convert.ToInt32(question.Required).ToString()).Selected = true;
                rbAddTextbox.ClearSelection();
                rbAddTextbox.Items.FindByValue(Convert.ToInt32(question.ShowTextControl).ToString()).Selected = true;

                rbGridRequired.ClearSelection();
                rbGridRequired.Items.FindByValue(question.GridValidation.ToString()).Selected = true;

                lblpageID.Text = question.PageID.ToString();
                lblQPageno.Text = $"Pg.{page.Number} - {page.PageHeader}";

                plQposition.Visible = false;
                plQuestionno.Visible = true;

                lblQno.Text = question.Number.ToString();
                var roList = FillResponseOptionses(question);

                if (string.Equals(question.Format, QuestionFormatGrid, StringComparison.OrdinalIgnoreCase))
                {
                    rbGridType.ClearSelection();
                    rbGridType.Items.FindByValue(question.Grid_control_Type.ToLower()).Selected = true;
                    PopulateResponseOptions(roList);
                    PopulateGridRow(BusinessCollector.GridStatements.GetByQuestionID(
                        question.QuestionID,
                        BusinessApplication.ECNSession.CurrentSession().CurrentUser));
                }

                if (BusinessCollector.SurveyBranching.Exists(
                    question.QuestionID,
                    BusinessApplication.ECNSession.CurrentSession().CurrentUser))
                {
                    rbQuestionFormat.Enabled = false;
                    txtOptions.Enabled = false;
                }
                else
                {
                    rbQuestionFormat.Enabled = true;
                    txtOptions.Enabled = true;
                }

                loadgrid();
                Page.ClientScript.RegisterStartupScript(
                    typeof(UIPage),
                    ScriptKeyLoadPage,
                    HtmlScriptTemplate);
            }
        }

        private List<ResponseOptions> FillResponseOptionses(Question question)
        {
            Guard.NotNull(question, nameof(question));

            var roList = BusinessCollector.ResponseOptions.GetByQuestionID(
                question.QuestionID,
                BusinessApplication.ECNSession.CurrentSession().CurrentUser);
            if (!string.Equals(question.Format, QuestionFormatTextBox, StringComparison.OrdinalIgnoreCase))
            {
                var builder = new StringBuilder();
                foreach (var responseOptions in roList)
                {
                    var responseScore = responseOptions.Score == 0 ? string.Empty : responseOptions.Score.ToString();
                    builder.AppendFormat(
                        $"<option optionID=\"{Guid.NewGuid()}\" " +
                        $"text=\"{CleanXMLString(responseOptions.OptionValue)}\" " +
                        $"score=\"{responseScore}\"></option>");
                }

                Responseoptions = builder.ToString();

                txtMaxChars.Text = TextMaxChars.ToString();
                PopulateResponseOptions(roList);
            }
            else
            {
                txtMaxChars.Text = question.MaxLength.ToString();
            }

            return roList;
        }

        private void ReoderQuestions(int questionId, Question question)
        {
            Guard.NotNull(question, nameof(question));

            var pageId = question.PageID;
            lblReorderTitle.Text = "Re-order Question";
            lblquestionID.Text = questionId.ToString();
            var pageList = BusinessCollector.Page.GetBySurveyID(
                SurveyID,
                BusinessApplication.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in pageList
                orderby src.Number
                select new
                {
                    PageID = src.PageID,
                    header = $"P. {src.Number} - {src.PageHeader}",
                    PageDesc = src.PageDesc,
                    number = src.Number
                }).ToList();
            drpQPage.DataSource = result;
            drpQPage.DataTextField = FieldHeader;
            drpQPage.DataValueField = FieldPageId;
            drpQPage.DataBind();

            drpQPage.ClearSelection();
            drpQPage.Items.FindByValue(pageId.ToString()).Selected = true;

            //Check if this question has branching - if Yes - dont allow reorder to other pages.
            drpQPage.Enabled = !BusinessCollector.SurveyBranching.Exists(
                questionId,
                BusinessApplication.ECNSession.CurrentSession().CurrentUser);

            FillDrpToQuestions(questionId);

            var questionTextTrimmed = ((question.QuestionText.Length > QuestionText50)
                ? question.QuestionText.Substring(0, QuestionText50 - 1)
                : question.QuestionText);
            lblRQNo.Text = string.Format("{0}. {1}", question.Number, question.QuestionText) == null
                ? string.Empty
                : questionTextTrimmed;

            plPReorder.Visible = false;
            plQReorder.Visible = true;
            Page.ClientScript.RegisterStartupScript(
                typeof(System.Web.UI.Page),
                ScriptKeyLoadPage,
                HtmlScriptTemplate);
        }

        private void FillDrpToQuestions(int questionId)
        {
            var qList = BusinessCollector.Question.GetByPageID(
                ParseInt32WithThrow(drpQPage.SelectedItem.Value),
                BusinessApplication.ECNSession.CurrentSession().CurrentUser);
            var questions = (from src in qList
                where src.QuestionID != questionId
                orderby src.Number
                select new
                {
                    DisplayValue = src.QuestionID,
                    DisplayText = string.Format(
                        "Q. {0} - {1}",
                        src.Number,
                        (string.IsNullOrWhiteSpace(src.QuestionText)
                            ? string.Empty
                            : ((src.QuestionText.Length > QuestionText50)
                                ? src.QuestionText.Substring(0, QuestionText50 - 1)
                                : src.QuestionText)))
                }).ToList();
            drpRToQuestion.DataSource = questions;
            drpRToQuestion.DataTextField = FieldDisplayText;
            drpRToQuestion.DataValueField = FieldDisplayValue;
            drpRToQuestion.DataBind();

            drpRToQuestion.ClearSelection();
            drpRToQuestion.Items.Insert(0, new ListItem(ItemTextSelectQuestion, ItemValueNone));
        }

        public void rpQuestionsGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbqDelete = (LinkButton)e.Item.FindControl("lbqDelete");
                lbqDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete this question?');");
                if ( ECN_Framework_BusinessLayer.Collector.Survey.HasResponses(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    LinkButton lbqEdit = (LinkButton)e.Item.FindControl("lbqEdit");
                    LinkButton lbqReOrder = (LinkButton)e.Item.FindControl("lbqReOrder");

                    lbqEdit.Visible = false;
                    lbqReOrder.Visible = false;
                    lbqDelete.Visible = false;
                }
            }
        }

        #endregion

        #region Page - Save/Cancel events
        private void btnPageSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ECN_Framework_Entities.Collector.Page page = new ECN_Framework_Entities.Collector.Page();
            page.PageID = lblpageID.Text == string.Empty ? -1: Convert.ToInt32(lblpageID.Text);
            page.SurveyID = SurveyID;
            page.PageHeader = txtPageHeader.Text;
            page.PageDesc = txtPageDesc.Text;
            int PageNumber = 0;
            try
            {
                if (drpPages.SelectedIndex > -1)
                {
                    if (Convert.ToInt32(drpPosition.SelectedItem.Value) == 0)
                        PageNumber = Convert.ToInt32(drpPages.SelectedItem.Value);
                    else
                        PageNumber = Convert.ToInt32(drpPages.SelectedItem.Value) == 0 ? 0 : Convert.ToInt32(drpPages.SelectedItem.Value) + 1;
                }
            }
            catch
            {
                PageNumber = 0;
            }
            page.Number = PageNumber;
            ECN_Framework_BusinessLayer.Collector.Page.Save(page, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            txtPageHeader.Text = "";
            txtPageDesc.Text = "";
            lblpageID.Text = "";
            drpPosition.ClearSelection();
            drpPosition.Items.FindByValue("1").Selected = true;
            plposition.Visible = true;
            Loaddropdowns();
            LoadSurveyGrid(dlPages.SelectedIndex > -1 ? Convert.ToInt32(dlPages.DataKeys[dlPages.SelectedIndex]) : 0);
        }

        private void btnPageCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lblpageID.Text = "";
            txtPageHeader.Text = "";
            txtPageDesc.Text = "";
            plposition.Visible = true;
        }

        #endregion

        #region Question - Save/Cancel events
        private void btnQuestionSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {

                ECN_Framework_Entities.Collector.Question q;
                if (lblquestionID.Text == "")
                    q = new ECN_Framework_Entities.Collector.Question();
                else
                    q = ECN_Framework_BusinessLayer.Collector.Question.GetByQuestionID(Convert.ToInt32(lblquestionID.Text));


                q.SurveyID = SurveyID;
                q.Format = rbQuestionFormat.SelectedItem.Value;
                q.PageID = Convert.ToInt32(lblpageID.Text);
                q.Grid_control_Type = rbGridType.SelectedIndex > -1 ? rbGridType.SelectedItem.Value : "";
                q.QuestionText = txtQuestion.Text;
                q.MaxLength = Convert.ToInt32(txtMaxChars.Text);

                if (q.MaxLength > 499)
                {
                    q.MaxLength = 499;
                }
                    

                try
                {
                    q.Required = rbRequired.SelectedValue == "1" ? true : false;
                }
                catch (Exception)
                {
                    q.Required = false;
                }
                q.GridValidation = (rbQuestionFormat.SelectedItem.Value.ToLower() == "grid" ? (rbGridRequired.SelectedIndex > -1 ? Convert.ToInt32(rbGridRequired.SelectedItem.Value) : 0) : 0);
                try
                {
                    q.ShowTextControl = (rbQuestionFormat.SelectedItem.Value.ToLower() == "grid" || rbQuestionFormat.SelectedItem.Value.ToLower() == "textbox" ? false : (rbAddTextbox.SelectedValue == "1" ? true : false));
                }
                catch (Exception)
                {
                    q.ShowTextControl = false;
                }

                string position = drpQPosition.SelectedIndex > -1 ? drpQPosition.SelectedItem.Value : "a";
                int targetID = drpQuestion.SelectedIndex > -1 ? Convert.ToInt32(drpQuestion.SelectedItem.Value) : 0;
                string options = string.Empty;

                if (rbQuestionFormat.SelectedItem.Value.ToLower() == "grid")
                    options = getValues(txtOptions.Text).Replace("''", "'");
                else if (rbQuestionFormat.SelectedItem.Value.ToLower() == "textbox")
                    options = "<options></options>";
                else
                    options = getResponseOptionXML();

                string gridrow = getValues(txtGridRow.Text).Replace("''", "'");


                ECN_Framework_BusinessLayer.Collector.Question.Save(q, position, targetID, options, gridrow, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                QReset();
                LoadSurveyGrid(dlPages.SelectedIndex > -1 ? Convert.ToInt32(dlPages.DataKeys[dlPages.SelectedIndex]) : 0);
                Loaddropdowns();
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                ErrorMessage = "";
                foreach (ECN_Framework_Common.Objects.ECNError error in ex.ErrorList)
                {
                    ErrorMessage = ErrorMessage + error.ErrorMessage + "<br/>";
                }
            }
        }

        private void btnQuestionCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            QReset();
        }

        #endregion

        #region Branch - Save/Cancel events

        private void btnbranchSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (drpBQuestion.SelectedIndex > -1)
            {
                if (Convert.ToInt32(drpBQuestion.SelectedItem.Value) > 0)
                {
                    ECN_Framework_BusinessLayer.Collector.SurveyBranching.DeletebyPageID(Convert.ToInt32(lblpageID.Text), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    for (int i = 0; i < dgBranch.Items.Count; i++)
                    {
                        DropDownList drpBPage = (DropDownList)dgBranch.Items[i].FindControl("drpBPage");
                        if (drpBPage.SelectedIndex > -1 && drpBPage.SelectedValue != "")
                        {
                            ECN_Framework_Entities.Collector.SurveyBranching sb = new ECN_Framework_Entities.Collector.SurveyBranching();
                            sb.SurveyID = SurveyID;
                            sb.QuestionID = Convert.ToInt32(drpBQuestion.SelectedItem.Value);
                            sb.OptionID = Convert.ToInt32(dgBranch.DataKeys[i].ToString());
                            sb.PageID = (Convert.ToInt32(drpBPage.SelectedValue) == 0 ? -1 : Convert.ToInt32(drpBPage.SelectedValue));
                            sb.EndSurvey = (Convert.ToInt32(drpBPage.SelectedValue)) == 0 ? true : false;
                            ECN_Framework_BusinessLayer.Collector.SurveyBranching.Save(sb, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        }
                    }
                }
            }
            lblpageID.Text = "";
            LoadSurveyGrid(dlPages.SelectedIndex > -1 ? Convert.ToInt32(dlPages.DataKeys[dlPages.SelectedIndex]) : 0);
        }

        private void btnbranchCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lblpageID.Text = "";
        }

        protected void drpBQuestion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            plbranch.Visible = true;
            List<ECN_Framework_Entities.Collector.ResponseOptions> retList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(Convert.ToInt32(drpBQuestion.SelectedItem.Value), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            dgBranch.DataSource = retList;
            dgBranch.DataBind();
            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadPage", "<script language='javascript'>onload=function(){showB();};</script>");
        }

        public void dgBranch_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList drpBPage = (DropDownList)e.Item.FindControl("drpBPage");
                drpBPage.ClearSelection();
                List<ECN_Framework_Entities.Collector.Page> pList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                ECN_Framework_Entities.Collector.Question q = ECN_Framework_BusinessLayer.Collector.Question.GetByQuestionID(Convert.ToInt32(drpBQuestion.SelectedItem.Value));
                ECN_Framework_Entities.Collector.Page p = ECN_Framework_BusinessLayer.Collector.Page.GetByPageID(q.PageID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                var result= (from src in pList
                             where src.Number > p.Number
                             orderby src.Number
                            select new 
                            {
                                PageID= src.PageID,
                                header= "P. "+ src.Number.ToString() + " - " + src.PageHeader
                            }).ToList();
                drpBPage.DataSource = result;
                drpBPage.DataTextField = "header";
                drpBPage.DataValueField = "PageID";
                drpBPage.DataBind();
                drpBPage.Items.Add(new ListItem("End Survey", "0"));
                drpBPage.Items.Insert(0, new ListItem("---Select Page---", ""));

                try
                {
                    ECN_Framework_Entities.Collector.SurveyBranching sb = ECN_Framework_BusinessLayer.Collector.SurveyBranching.GetByOptionID(Convert.ToInt32(dgBranch.DataKeys[e.Item.ItemIndex].ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    string PageID = string.Empty;
                    if (sb.EndSurvey == false)
                        PageID = sb.PageID.ToString();
                    else
                        PageID = "0";
                    drpBPage.Items.FindByValue(PageID).Selected = true;
                }
                catch
                {
                }
            }
        }

        #endregion

        #region ReOrder Popup - Save/Cancel events

        private void btnReOrderSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (plPReorder.Visible)
            {
                ECN_Framework_BusinessLayer.Collector.Page.Reorder(Convert.ToInt32(lblpageID.Text), drpRPPosition.SelectedItem.Value, Convert.ToInt32(drpRToPage.SelectedItem.Value), 
                                                                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            else if (plQReorder.Visible)
            {
                if (Convert.ToInt32(drpQPage.SelectedItem.Value) > 0)
                {
                    if (lblquestionID.Text != drpRToQuestion.SelectedItem.Value)
                    {
                        ECN_Framework_BusinessLayer.Collector.Question.Reorder(Convert.ToInt32(drpQPage.SelectedItem.Value), drpRQPosition.SelectedItem.Value, Convert.ToInt32(lblquestionID.Text), Convert.ToInt32(drpRToQuestion.SelectedItem.Value),
                                                                                ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                 
                    }
                }
            }
            Loaddropdowns();
            LoadSurveyGrid(0);
            lblpageID.Text = "";
            lblquestionID.Text = "";
        }

        private void btnReOrderCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lblpageID.Text = "";
            lblquestionID.Text = "";
        }

        protected void drpQPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            List<ECN_Framework_Entities.Collector.Question> qList = ECN_Framework_BusinessLayer.Collector.Question.GetByPageID(Convert.ToInt32(drpQPage.SelectedItem.Value), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in qList
                          select new
                          {
                              DisplayValue= src.QuestionID,
                              DisplayText = "Q. " + src.Number.ToString() + " - " + ((src.QuestionText.Length > 50) ? src.QuestionText.Substring(0, 49) : src.QuestionText)
                          }).ToList();
            drpRToQuestion.DataSource = result;
            drpRToQuestion.DataTextField = "DisplayText";
            drpRToQuestion.DataValueField = FieldDisplayValue;
            drpRToQuestion.DataBind();
            drpRToQuestion.ClearSelection();
            drpRToQuestion.Items.Insert(0, new ListItem("Select Question", "0"));

            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadPage", "<script language='javascript'>onload=function(){showR();};</script>");
        }

        #endregion

        private void QReset()
        {
            lblpageID.Text = "";
            lblquestionID.Text = "";
            rbQuestionFormat.ClearSelection();
            txtQuestion.Text = "";
            lblQPageno.Text = "";
            drpQuestion.ClearSelection();
            plQposition.Visible = true;
            rbRequired.ClearSelection();
            rbGridRequired.ClearSelection();
            txtOptions.Text = "";
            txtGridRow.Text = "";
            rbGridType.ClearSelection();
            rbAddTextbox.ClearSelection();
            txtMaxChars.Text = "499";
        }

       

        private void PopulateResponseOptions(List<ECN_Framework_Entities.Collector.ResponseOptions> Options)
        {
            char lf = (char)10;        // line feed character
            string optionsString = ""; // string to hold response options once they've been pulled from the DataTable

            foreach (ECN_Framework_Entities.Collector.ResponseOptions o in Options)
            {
                optionsString += o.OptionValue + lf.ToString(); // add the response option and a line feed character to the string
            }
            txtOptions.Text = optionsString;     // output the string containing the options to an editable field
        }

        private void PopulateGridRow(List<ECN_Framework_Entities.Collector.GridStatements> GridStatements)
        {
            char lf = (char)10;           // line feed character
            string statementsString = ""; // string to hold response options once they've been pulled from the DataTable
            foreach (ECN_Framework_Entities.Collector.GridStatements g in GridStatements)
            {
                statementsString += g.GridStatement + lf.ToString();
            }
            txtGridRow.Text = statementsString;   // output the string containing the statements to an editable field
        }

        private string getResponseOptionXML()
        {
            string XML = string.Empty;
            string Option = string.Empty;
            string score = string.Empty;
            char tab = (char)9;
            foreach (GridViewRow gvr in gvResponseOption.Rows)
            {
                Option = string.Empty;
                score = string.Empty;

                if (gvr.RowIndex != gvResponseOption.EditIndex)
                {
                    Label lblOption = (Label)gvr.FindControl("lblOption");
                    Label lblScore = (Label)gvr.FindControl("lblScore");

                    Option = StringFunctions.Remove(lblOption.Text, tab.ToString());
                    score = lblScore.Text.Trim();
                }
                else
                {
                    TextBox txtOptionE = (TextBox)gvr.FindControl("txtOptionE");
                    TextBox txtScoreE = (TextBox)gvr.FindControl("txtScoreE");

                    Option = StringFunctions.Remove(txtOptionE.Text,tab.ToString());
                    score = txtScoreE.Text.Trim();
                }

                XML += "<option score='" + score + "'><![CDATA[" + Option + "]]></option>";

            }

            return "<options>" + XML + "</options>";
        }

        private string getValues(string values)
        {
            string txtboxvalues = string.Empty;

            StringTokenizer st = new StringTokenizer(values, '\n'); // get the list of grid statements from the text box and parse it
            char lf = (char)10; // line feed character
            char cr = (char)13; // carriage return character
            char tab = (char)9;
            while (st.HasMoreTokens())
            { // loop through the list of response options
                string statement = StringFunctions.CleanString(st.NextToken()); // strip problematic characters
                if (statement != "")
                {
                    statement = StringFunctions.Remove(statement, lf.ToString()); // strip line feed characters
                    statement = StringFunctions.Remove(statement, cr.ToString()); // strip carriage return characters
                    statement = StringFunctions.Remove(statement, tab.ToString());
                    if (statement != string.Empty)
                    {
                        if (txtboxvalues == string.Empty)
                            txtboxvalues = "<option><![CDATA[" + statement + "]]></option>";
                        else
                            txtboxvalues += "<option><![CDATA[" + statement + "]]></option>";
                    }
                }
            }
            return "<options>" + txtboxvalues + "</options>"; ;
        }

        public void chkShowAllPages_oncheckchanged(object sender, EventArgs e)
        {
            if (chkShowAllPages.Checked)
                dlPages.SelectedIndex = -1;

            LoadSurveyGrid(dlPages.SelectedIndex > -1 ? Convert.ToInt32(dlPages.DataKeys[dlPages.SelectedIndex]) : 0);
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.btnPageSave.Click += new System.Web.UI.ImageClickEventHandler(this.btnPageSave_Click);
            this.btnPageCancel.Click += new System.Web.UI.ImageClickEventHandler(this.btnPageCancel_Click);
            this.btnQuestionSave.Click += new System.Web.UI.ImageClickEventHandler(this.btnQuestionSave_Click);
            this.btnQuestionCancel.Click += new System.Web.UI.ImageClickEventHandler(this.btnQuestionCancel_Click);
            this.btnbranchSave.Click += new System.Web.UI.ImageClickEventHandler(this.btnbranchSave_Click);
            this.btnbranchCancel.Click += new System.Web.UI.ImageClickEventHandler(this.btnbranchCancel_Click);
            this.dgBranch.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgBranch_ItemDataBound);
            this.btnReOrderSave.Click += new System.Web.UI.ImageClickEventHandler(this.btnReOrderSave_Click);
            this.btnReOrderCancel.Click += new System.Web.UI.ImageClickEventHandler(this.btnReOrderCancel_Click);

        }
        #endregion

        #region gridview events
        private void loadgrid()
        {
            if (Responseoptions != "")
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader("<xml>" + Responseoptions + "</xml>"));

                gvResponseOption.DataSource = ds;
                gvResponseOption.DataBind();
                gvResponseOption.Visible = true;
            }
            else
            {
                gvResponseOption.Visible = false;
            }

        }
        
        protected void AddOptions(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Responseoptions = Responseoptions + "<option optionID=\"" + System.Guid.NewGuid().ToString() + "\" text=\"" + CleanXMLString(txtOption.Text) + "\" score=\"" + txtScore.Text + "\"></option>";
            txtOption.Text = "";
            txtScore.Text = "";
            loadgrid();
        }

        protected void gvResponseOption_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvResponseOption.EditIndex = e.NewEditIndex;
            loadgrid();
        }

        protected void gvResponseOption_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string ID = gvResponseOption.DataKeys[gvResponseOption.EditIndex].Value.ToString();

            GridViewRow row = gvResponseOption.Rows[gvResponseOption.EditIndex];

            TextBox txtOptionE = (TextBox)row.FindControl("txtOptionE");
            TextBox txtScoreE = (TextBox)row.FindControl("txtScoreE");

            UpdateXMLData(ID, txtOptionE.Text, txtScoreE.Text);

            gvResponseOption.EditIndex = -1;
            loadgrid();
        }

        protected void gvResponseOption_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvResponseOption.EditIndex = -1;
            loadgrid();
        }

        protected void gvResponseOption_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string ID = gvResponseOption.DataKeys[e.RowIndex].Value.ToString();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<xml>" + Responseoptions + "</xml>");

            XmlNode node = doc.SelectSingleNode("//option[@optionID='" + ID + "']");

            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
                Responseoptions = doc.DocumentElement.InnerXml;
            }

            loadgrid();
        }

        private void UpdateXMLData(string ID, string option, string score)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<xml>" + Responseoptions + "</xml>");

            XmlNode node = doc.SelectSingleNode("//option[@optionID='" + ID + "']");

            if (node != null)
            {
                node.Attributes["text"].Value = CleanXMLString(option);
                node.Attributes["score"].Value = score;
            }

            Responseoptions = doc.DocumentElement.InnerXml;
        }
        #endregion

        private string CleanXMLString(string s)
        {
            s = s.Replace("\"", "&quot;");
            s = s.Replace("&", "&amp;");
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            return s;
        }

        private static int ParseInt32WithThrow(string strInt)
        {
            int resut;
            if (!int.TryParse(strInt, out resut))
            {
                throw new InvalidOperationException(string.Format(ErrorParseIntTemplate, strInt));
            }
            return resut;
        }
    }
}
