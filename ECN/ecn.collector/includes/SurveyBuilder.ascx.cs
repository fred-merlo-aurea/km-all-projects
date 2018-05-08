using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DiagnosticsTrace = System.Diagnostics.Trace;

using KM.Common;
using Business = ECN_Framework_BusinessLayer.Collector;
using Entities = ECN_Framework_Entities.Collector;

namespace ecn.collector.includes
{
    public partial class SurveyBuilder : System.Web.UI.UserControl
    {
        private const string AttributeOnClick = "onclick";
        private const string AttributeOnKeyPress = "onkeypress";
        private const string AttributeOnBeforePaste = "onbeforepaste";
        private const string AttributeOnPaste = "onpaste";
        private const string AttributeClass = "class";

        private const string QuestionFormatCheckbox = "checkbox";
        private const string QuestionFormatRadio = "radio";
        private const string QuestionFormatTextbox = "textbox";
        private const string QuestionFormatGrid = "grid";
        private const string QuestionFormatDropDown = "dropdown";

        private const string FieldOptionValue = "OptionValue";

        private const string CssClassAnswer = "answer";
        private const string ItemNameSelect = "----- SELECT AN ITEM FROM LIST -----";
        private const string AttributeOnChange = "onChange";
        private const int QuestionMaxLength = 50;
        private const int TextControlWidth = 375;
        private const string AttributeMaxLength = "maxLength";
        private const int TextRowsCount = 5;
        private const int TextColumsCount = 50;
        private const string TextControlMaxLength = "499";
        private const int MaxLengthNone = -1;
        private const int GridCellPadding = 3;
        private const int GridCellSpacing = 0;

        private const int IdNone = -1;

        protected System.Web.UI.WebControls.Label lblPercentage;
        KMPlatform.Entity.User CurrentUser = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
        int _surveyID = -1;
        private bool _isViewOnly = true;
        
        #region Messages
        private string SURVEY_COMPLETED_MESSAGE = "<b><font face=verdana color=#A09D98 size=-1>Thank you. You have successfully completed the survey.</font></b>";
        private string SURVEY_HAS_COMPLETED_MESSAGE = "<b><font face=verdana color=#A09D98 size=-1>You have already completed the survey.</font></b>";
        private string SURVEY_EXPIRED_MESSAGE = "<b><font face=verdana color=#A09D98 size=-1>The survey you want to take is not active.</font></b>";
        private string SURVEY_INVALID_MESSAGE = "<b><font face=verdana color=#A09D98 size=-1>The survey you want to take not exists.</font></b>";
        private string INVALID_USER_MESSAGE = "<b><font face=verdana color=#A09D98 size=-1>Invalid user id. <a href=" + SURVEY_ADMIN_EMAIL + ">click here</a> to contact the survey administrator</font>";
        private static string SURVEY_ADMIN_EMAIL = "mailto:ashok@teckman.com?subject=ECNSurvey";
        #endregion

        #region getters & setters

        private string UserName
        {
            get
            {
                try { return Request["uid"].ToString(); }
                catch { return string.Empty; };
            }
        }

        private ECN_Framework_Entities.Collector.Survey CurrentSurvey
        {
            get
            {
                if (Session["CurrentSurvey" + _surveyID] == null)
                {

                    ECN_Framework_Entities.Collector.Survey _CurrentSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(_surveyID, CurrentUser);

                    Session["CurrentSurvey" + _surveyID] = _CurrentSurvey;
                    
                }
                return (ECN_Framework_Entities.Collector.Survey)Session["CurrentSurvey" + _surveyID];
            }
            set { Session["CurrentSurvey" + _surveyID] = value; }
        }

        private int BlastID
        {
            get
            {
                try { return Convert.ToInt32(Request["bid"].ToString()); }
                catch { return 0; };
            }
        }

        private int PageIndex
        {
            get
            {
                if (ViewState["PageIndex"] == null)
                    return 0;
                else
                    return (int)ViewState["PageIndex"];
            }

            set { ViewState["PageIndex"] = value; }
        }

        private bool ShowQuestionNo
        {
            get
            {
                if (ViewState["ShowQuestionNo"] == null)
                    return true;
                else
                    return (bool)ViewState["ShowQuestionNo"];
            }

            set { ViewState["ShowQuestionNo"] = value; }
        }

        private int ParticipantID
        {
            get
            {
                if (Session["ParticipantID"] == null)
                    return 0;
                else
                    return (int)Session["ParticipantID"];
            }

            set { Session["ParticipantID"] = value; }
        }

        public bool IsViewOnly
        {
            get { return _isViewOnly; }
            set { _isViewOnly = value; }
        }

        private void CreateSurveyPages()
        {
            CurrentSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(_surveyID, CurrentUser);
            List<ECN_Framework_Entities.Collector.Page> itemList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(GetSurveyID(), CurrentUser);
            ECN_Framework_Entities.Collector.Page p = new ECN_Framework_Entities.Collector.Page();
            p.IsIntroPage = true;
            itemList.Insert(0, p);
            p = new ECN_Framework_Entities.Collector.Page();
            p.IsThankYouPage = true;
            itemList.Add(p);
            Session["SurveySession" + _surveyID] = itemList;
            LoadAnswers();
        }

        private List<ECN_Framework_Entities.Collector.Page> SurveySession
        {
            get
            {
                if (Session["SurveySession" + _surveyID] == null)
                {

                    CurrentSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(_surveyID, CurrentUser);
                    List<ECN_Framework_Entities.Collector.Page> itemList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(GetSurveyID(), CurrentUser);
                    ECN_Framework_Entities.Collector.Page p = new ECN_Framework_Entities.Collector.Page();
                    if (CurrentSurvey.IntroHTML.ToString() != string.Empty)
                    {
                        p.IsIntroPage = true;
                        itemList.Insert(0, p);
                    }


                    p = new ECN_Framework_Entities.Collector.Page();
                    p.IsThankYouPage = true;
                    itemList.Add(p);

                    Session["SurveySession" + _surveyID] = itemList;
                    LoadAnswers();
                }
                return (List<ECN_Framework_Entities.Collector.Page>)Session["SurveySession" + _surveyID];
            }
            set { Session["SurveySession" + _surveyID] = value; }
        }

        private ECN_Framework_Entities.Collector.Page GetPage(int pageIndex)
        {
            List<ECN_Framework_Entities.Collector.Page> ss = SurveySession;
            return ss[pageIndex];
        }

        private int GetSurveyID()
        {
            string[] surveyIDKeys = new string[] { "surveyID", "sid" };
            int surveyID = -1;
            foreach (string key in surveyIDKeys)
            {
                if (Request[key] == null)
                {
                    continue;
                }
                else
                {
                    int.TryParse(Request[key].ToString(), out surveyID);
                }
            }
            return surveyID;
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            _surveyID = GetSurveyID();
            
            lblErrorMessage.Text = "";
            phError.Visible = false;

            btnNext.Attributes.Add("onclick", "javascript:return validate();");
            
            if (!IsPostBack)
            {
                Session.Clear();
                
                if (CurrentSurvey == null)
                {
                    imgHeader.Visible = false;
                    imgFooter.Visible = false;
                    phError.Visible = true;
                    btnNext.Visible = false;
                    btnPrevious.Visible = false;
                    plPageHeader.Visible = false;
                    plPageDesc.Visible = false;
                    lblErrorMessage.Text = SURVEY_INVALID_MESSAGE;
                    return;
                }

                ECN_Framework_Entities.Collector.SurveyStyles sStyle = ECN_Framework_BusinessLayer.Collector.SurveyStyles.GetBySurveyID(_surveyID, CurrentUser);

                if (sStyle.hImage != string.Empty)
                    imgHeader.ImageUrl = sStyle.hImage;
                else
                    imgHeader.Visible = false;

                if (sStyle.fImage != string.Empty)
                    imgFooter.ImageUrl = sStyle.fImage;
                else
                    imgFooter.Visible = false;

                plstyletag.Text = ECN_Framework_BusinessLayer.Collector.SurveyStyles.RenderStyle(sStyle);
                ShowQuestionNo = sStyle.ShowQuestionNo;
                DateTime expDate = CurrentSurvey.DisableDate == null ? DateTime.Now.AddDays(1) : CurrentSurvey.DisableDate.Value;

                if (expDate < DateTime.Now || (!CurrentSurvey.IsActive && !IsViewOnly))
                {
                    phError.Visible = false;
                    btnNext.Visible = false;
                    btnPrevious.Visible = false;
                    plPageHeader.Visible = false;
                    plPageDesc.Visible = false;
                    plInactiveSurvey.Visible = true;
                    return;
                }

                if (!IsViewOnly && CurrentSurvey != null)
                {
                    // --sunil - 08/07/2007 - get the ParticipantID from DB for initial load.

                    //commented by sunil - get ParticipantID from Session - if not exists get from DB
                    //if (ParticipantID == 0)
                    
                    if (UserName != string.Empty)
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(UserName))
                        {
                            ParticipantID = ECN_Framework_BusinessLayer.Collector.Participant.GetByUserName(UserName, CurrentSurvey.CustomerID);
                            //check if the participant has previously completed teh survey
                            if (ECN_Framework_BusinessLayer.Collector.Participant.CompletedSurvey(_surveyID, ParticipantID, CurrentUser))
                            {
                                phError.Visible = true;
                                btnNext.Visible = false;
                                btnPrevious.Visible = false;
                                plPageHeader.Visible = false;
                                plPageDesc.Visible = false;
                                lblErrorMessage.Text = SURVEY_HAS_COMPLETED_MESSAGE;
                                return;
                            }
                        }
                        else
                        {
                            phError.Visible = true;
                            btnNext.Visible = false;
                            btnPrevious.Visible = false;
                            plPageHeader.Visible = false;
                            plPageDesc.Visible = false;
                            lblErrorMessage.Text = INVALID_USER_MESSAGE;
                            return;
                        }
                    }

                }

                //CreateSurveyPages();
                RenderPage(true);
            }
            else
                RenderPage(false);


        }

        private void RenderPage(bool bAddValidation)
        {
            ECN_Framework_Entities.Collector.Page p = GetPage(PageIndex);
            //ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(GetSurveyID(), CurrentUser);
            //ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(GetSurveyID(), CurrentUser);
            plSurveyContent.Controls.Clear();

            if (p.IsIntroPage)
            {
                //plPageHeader.Visible=false;
                //plPageDesc.Visible =false;
                lblPageHeader.Text = "&nbsp;";
                lblPageDesc.Text = "&nbsp;";
                btnPrevious.Visible = false;
                btnNext.Text = "Next";
                plSurveyContent.Controls.Add(new LiteralControl(CurrentSurvey.IntroHTML.ToString()));
                ShowProgress(false);
            }
            else if (p.IsThankYouPage)
            {
                //plPageHeader.Visible=false;
                //plPageDesc.Visible =false;
                lblPageHeader.Text = "&nbsp;";
                lblPageDesc.Text = "&nbsp;";
                btnPrevious.Visible = false;
                btnNext.Visible = false;


                string ThankYouMessage = CurrentSurvey.ThankYouHTML.ToString();

                ThankYouMessage = ThankYouMessage.Replace("%%NEsurveypopup%%", "<a id='ahsPopup' href='http://www.ecn5.com/neebo/nesubscribe.aspx?emailID=%%emailID%%&surveyID=%%surveyID%%' class='highslide'\" onclick=\"return hs.htmlExpand(this, {objectType: 'iframe', height:425, width:550, objectHeight:425, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false} );\">Click here to Subscribe</a><script>function init() {return hs.htmlExpand(document.getElementById('ahsPopup'), {objectType: 'iframe', height:425, width:550, objectHeight:425, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false} );}window.onload =init;</Script>");
                ThankYouMessage = ThankYouMessage.Replace("%%surveyID%%", _surveyID.ToString());
                ThankYouMessage = ThankYouMessage.Replace("%%emailID%%", ParticipantID.ToString());

                if (ThankYouMessage == string.Empty)
                    ThankYouMessage = SURVEY_COMPLETED_MESSAGE;

                plSurveyContent.Controls.Add(new LiteralControl(ThankYouMessage));
                ShowProgress(false);
            }
            else
            {
                if (PageIndex == 0)
                    btnPrevious.Visible = false;
                else
                    btnPrevious.Visible = true;

                if (PageIndex == SurveySession.Count - 2)
                    btnNext.Text = "Finish";
                else
                    btnNext.Text = "Next";

                if (p.PageHeader == string.Empty)
                {
                    //plPageHeader.Visible=false;
                    lblPageHeader.Text = "&nbsp;";
                }
                else
                {
                    //plPageHeader.Visible=true;
                    lblPageHeader.Text = p.PageHeader;
                }

                if (p.PageDesc == string.Empty)
                {
                    //plPageDesc.Visible=false;
                    lblPageDesc.Text = "&nbsp;";
                }
                else
                {
                    //plPageDesc.Visible=true;
                    lblPageDesc.Text = p.PageDesc;
                }

                RenderQuestions(bAddValidation, p.QuestionList);
                ShowProgress(true);
            }

        }

        private void RenderQuestions(bool bAddValidation, List<ECN_Framework_Entities.Collector.Question> questions)
        {
            string strQuestions = string.Empty;

            // start appending validations 
            string surveyValidation = "<script type='text/javascript'>";
            int arrayCount = 0;


            foreach (ECN_Framework_Entities.Collector.Question q in questions)
            {
                List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(q.QuestionID, CurrentUser);

                if ((q.Required || q.GridValidation > 0) && bAddValidation)
                {
                    if (q.Format.ToLower() == "checkbox" || q.Format.ToLower() == "radio")
                        surveyValidation += "fv[" + arrayCount + "] = \"" + GetIDForControl(q) + "," + roList.Count + "\";";
                    else if (q.Format.ToLower() == "dropdown" || q.Format.ToLower() == "textbox")
                        surveyValidation += "fv[" + arrayCount + "] = \"" + GetIDForControl(q) + "\";";
                    else if (q.Format.ToLower() == "grid")
                    {
                        string gridstatementIDs = string.Empty;
                        foreach (ECN_Framework_Entities.Collector.GridStatements grid in ECN_Framework_BusinessLayer.Collector.GridStatements.GetByQuestionID(q.QuestionID, CurrentUser))
                        {
                            gridstatementIDs += gridstatementIDs == string.Empty ? grid.GridStatementID.ToString() : "|" + grid.GridStatementID.ToString();
                        }
                        surveyValidation += "fv[" + arrayCount + "] = \"" + GetIDForControl(q) + "," + q.GridValidation + "," + gridstatementIDs + "\";";
                    }

                    arrayCount++;
                }

                string strValidationMessage = string.Empty;

                if (q.GridValidation > 0)
                {
                    strValidationMessage = string.Empty;

                    if (q.GridValidation == 1)
                        strValidationMessage = "<BR>(One response is required)";
                    else if (q.GridValidation == 2)
                        strValidationMessage = "<BR>(At least One response is required)";
                    else if (q.GridValidation == 3)
                        strValidationMessage = "<BR>(At least One response per line is required)";

                    //plSurveyContent.Controls.Add(new LiteralControl("&nbsp;<DIV style='display:none;background-color:white;font-size:12px;color:red;' id='divg_" +  string.Format("question_{0}", q.QuestionID) + "'>"+ strValidationMessage +"</DIV>")); 
                }

                if (ShowQuestionNo)
                    plSurveyContent.Controls.Add(new LiteralControl("<DIV class='question' id='divq_" + GetIDForControl(q) + "'>" + q.Number + ". " + q.QuestionText + " " + strValidationMessage + "</DIV>"));
                else
                    plSurveyContent.Controls.Add(new LiteralControl("<DIV class='question' id='divq_" + GetIDForControl(q) + "'>" + q.QuestionText + " " + strValidationMessage + "</DIV>"));

                plSurveyContent.Controls.Add(new LiteralControl("<DIV class='answer'>"));
                plSurveyContent.Controls.Add(CreateOptionControl(q));
                plSurveyContent.Controls.Add(new LiteralControl("</DIV>"));

                if (q.ShowTextControl)
                {
                    plSurveyContent.Controls.Add(new LiteralControl("<DIV class='answer'>"));
                    plSurveyContent.Controls.Add(CreateOptionTextControl(q));
                    plSurveyContent.Controls.Add(new LiteralControl("</DIV>"));
                }
                plSurveyContent.Controls.Add(new LiteralControl("<BR><DIV><HR></DIV>"));
            }
            surveyValidation += "</script>";

            if (bAddValidation)
                Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "val", surveyValidation);

        }

        private Control CreateOptionTextControl(ECN_Framework_Entities.Collector.Question question)
        {
            TextBox txtOptions = new TextBox();
            txtOptions.Width = 300;
            txtOptions.Attributes.Add("maxLength", "100");
            txtOptions.Enabled = false;
            txtOptions.ID = string.Format("question_{0}_TEXT", question.QuestionID.ToString());

            foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
            {
                if (answer.ID == -2)
                {
                    txtOptions.Enabled = true;
                    txtOptions.Text = answer.Value;
                }
            }

            return txtOptions;
        }

        private Control CreateOptionControl(Entities.Question question)
        {
            Guard.NotNull(question, nameof(question));

            var roList = Business.ResponseOptions.GetByQuestionID(question.QuestionID, CurrentUser);

            switch (question.Format.ToLower().Trim())
            {
                case QuestionFormatCheckbox:
                    var chkOption = CreateCheckboxControl(question, roList);
                    return chkOption;
                case QuestionFormatDropDown:
                    var ddlOption = CreateDropDownControl(question, roList);
                    return ddlOption;
                case QuestionFormatRadio:
                    var rdoOptions = CreateRadioControl(question, roList);
                    return rdoOptions;
                case QuestionFormatTextbox:
                    var txtOptions = CreateTextboxControl(question);
                    return txtOptions;
                case QuestionFormatGrid:
                    var tblGrid = CreateGridControl(question);
                    return tblGrid;
                default:
                    throw new InvalidOperationException($"Unknown type of control -- {question.Format}");
            }
        }

        private HtmlTable CreateGridControl(Entities.Question question)
        {
            Guard.NotNull(question, nameof(question));

            var tblGrid = new HtmlTable();
            tblGrid.CellPadding = GridCellPadding;
            tblGrid.CellSpacing = GridCellSpacing;
            tblGrid.Attributes.Add(AttributeClass, "tblSurveyGrid");
            var isFirst = true;
            var gridStatementses = Business.GridStatements.GetByQuestionID(question.QuestionID, CurrentUser);
            foreach (var grid in gridStatementses)
            {
                if (isFirst)
                {
                    isFirst = false;
                    tblGrid.Rows.Add(CreateTableRowForGridHeader(question));
                }

                tblGrid.Rows.Add(CreateTableRowForGrid(question, grid));
            }

            return tblGrid;
        }

        private TextBox CreateTextboxControl(Entities.Question question)
        {
            Guard.NotNull(question, nameof(question));

            var txtOptions = new TextBox();
            if (question.MaxLength > 0 && question.MaxLength <= QuestionMaxLength)
            {
                txtOptions.Width = TextControlWidth;
                txtOptions.Attributes.Add(AttributeMaxLength, question.MaxLength.ToString());
            }
            else
            {
                txtOptions.TextMode = TextBoxMode.MultiLine;
                txtOptions.Rows = TextRowsCount;
                txtOptions.Columns = TextColumsCount;
                txtOptions.Attributes.Add(
                    AttributeMaxLength,
                    question.MaxLength == 0 ? 
                        TextControlMaxLength :
                        question.MaxLength == MaxLengthNone ? 
                            TextControlMaxLength :
                            question.MaxLength.ToString());
                txtOptions.Attributes.Add(AttributeOnKeyPress, "doKeypress(this, event);");
                txtOptions.Attributes.Add(AttributeOnBeforePaste, "doBeforePaste(this, event);");
                txtOptions.Attributes.Add(AttributeOnPaste, "doPaste(this, event);");
            }

            txtOptions.ID = GetIDForControl(question);

            if (question.ResponseList.Count > 0)
            {
                foreach (var answer in question.ResponseList)
                {
                    txtOptions.Text = answer.Value;
                }
            }

            return txtOptions;
        }

        private RadioButtonList CreateRadioControl(
            Entities.Question question, 
            IReadOnlyCollection<Entities.ResponseOptions> responseOptions)
        {
            Guard.NotNull(question, nameof(question));

            var rdoOptions = new RadioButtonList
            {
                CssClass = CssClassAnswer,
                DataSource = responseOptions,
                DataTextField = FieldOptionValue,
                DataValueField = FieldOptionValue,
                ID = GetIDForControl(question)
            };

            if (question.ShowTextControl)
            {
                rdoOptions.Attributes.Add(
                    AttributeOnClick,
                    $"javascript:EnableTextControl(\'r\', \'{GetIDForControl(question)}\',{responseOptions.Count});");
            }

            rdoOptions.DataBind();

            if (question.ResponseList.Count > 0)
            {
                rdoOptions.ClearSelection();
                foreach (var answer in question.ResponseList)
                {
                    try
                    {
                        if (answer.ID == IdNone)
                        {
                            rdoOptions.Items.FindByValue(answer.Value).Selected = true;
                        }
                    }
                    catch (Exception exception)
                    {
                        DiagnosticsTrace.TraceError(exception.ToString());
                    }
                }
            }

            return rdoOptions;
        }

        private DropDownList CreateDropDownControl(
            Entities.Question question, 
            IReadOnlyCollection<Entities.ResponseOptions> responseOptionses)
        {
            Guard.NotNull(question, nameof(question));

            var ddlOption = new DropDownList
            {
                DataSource = responseOptionses,
                DataTextField = FieldOptionValue,
                DataValueField = FieldOptionValue,
                ID = GetIDForControl(question)
            };

            if (question.ShowTextControl)
            {
                ddlOption.Attributes.Add(
                    AttributeOnChange,
                    $"javascript:EnableTextControl(\'d\', \'{GetIDForControl(question)}\',{(responseOptionses.Count + 1)});");
            }

            ddlOption.DataBind();
            ddlOption.Items.Insert(0, new ListItem(ItemNameSelect, string.Empty));

            ddlOption.ClearSelection();
            foreach (var answer in question.ResponseList)
            {
                try
                {
                    if (answer.ID == IdNone)
                    {
                        ddlOption.Items.FindByValue(answer.Value).Selected = true;
                    }
                }
                catch (Exception exception)
                {
                    DiagnosticsTrace.TraceError(exception.ToString());
                }
            }

            return ddlOption;
        }

        private CheckBoxList CreateCheckboxControl(
            Entities.Question question, 
            IReadOnlyCollection<Entities.ResponseOptions> responseOptionses)
        {
            Guard.NotNull(question, nameof(question));

            var chkOption = new CheckBoxList
            {
                CssClass = CssClassAnswer,
                DataSource = responseOptionses,
                DataTextField = FieldOptionValue,
                DataValueField = FieldOptionValue,
                ID = GetIDForControl(question)
            };

            if (question.ShowTextControl)
            {
                chkOption.Attributes.Add(
                    AttributeOnClick,
                    $"javascript:EnableTextControl(\'c\',\'{GetIDForControl(question)}\',{responseOptionses.Count});");
            }

            chkOption.DataBind();

            chkOption.ClearSelection();
            foreach (var answer in question.ResponseList)
            {
                try
                {
                    if (answer.ID == IdNone)
                    {
                        chkOption.Items.FindByValue(answer.Value).Selected = true;
                    }
                }
                catch (Exception exception)
                {
                    DiagnosticsTrace.TraceError(exception.ToString());
                }
            }

            return chkOption;
        }

        public HtmlTableRow CreateTableRowForGridHeader(ECN_Framework_Entities.Collector.Question question)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell statementCell = new HtmlTableCell();
            HtmlTableCell optionCell;

            statementCell.Controls.Add(new LiteralControl("&nbsp;"));
            row.Cells.Add(statementCell);
            List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(question.QuestionID, CurrentUser);

            foreach (ECN_Framework_Entities.Collector.ResponseOptions option in roList)
            {
                optionCell = new HtmlTableCell();
                optionCell.VAlign = "middle";
                optionCell.Align = "center";
                optionCell.Attributes.Add("class", "gridColumn");
                optionCell.InnerText = option.OptionValue;
                row.Cells.Add(optionCell);
            }

            return row;
        }

        public HtmlTableRow CreateTableRowForGrid(ECN_Framework_Entities.Collector.Question question, ECN_Framework_Entities.Collector.GridStatements grid)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell statementCell = new HtmlTableCell();
            HtmlTableCell optionCell;
            Label lbl = new Label();
            List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(question.QuestionID, CurrentUser);


            lbl.CssClass = "gridRow";
            string lblText = "";
            if (grid.GridStatement.ToLower().IndexOf("http://") > -1)
            {
                char[] delimiter = { ' ' };
                string[] lblarray = grid.GridStatement.Split(delimiter);

                for (int j = 0; j < lblarray.Length; j++)
                {
                    if (lblarray[j].ToLower().IndexOf("http://") > -1)
                    {
                        lblText = lblText + "<a href='" + lblarray[j] + "' target='_blank'>" + lblarray[j] + "</a>";
                    }
                    else
                        lblText = lblText + lblarray[j] + " ";
                }
            }
            else
            {
                lblText = grid.GridStatement;
            }
            lbl.Text = lblText;
            statementCell.Controls.Add(lbl);
            row.Cells.Add(statementCell);

            foreach (ECN_Framework_Entities.Collector.ResponseOptions option in roList)
            {
                optionCell = new HtmlTableCell();
                optionCell.VAlign = "middle";
                optionCell.Align = "center";
                if (question.Grid_control_Type == "r")
                {
                    RadioButton options = new RadioButton();
                    options.ID = GetIDForGridControl(question, grid, option);
                    options.GroupName = "radiogroup_" + question.QuestionID + "_" + grid.GridStatementID;

                    if (question.GridValidation == 1)
                        options.Attributes.Add("onclick", "resetcontrol(this, " + question.QuestionID + ")");

                    if (question.ResponseList.Count > 0)
                    {
                        foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
                        {
                            if (answer.ID == grid.GridStatementID && answer.Value == option.OptionValue)
                                options.Checked = true;
                        }
                    }

                    optionCell.Controls.Add(options);
                }
                else if (question.Grid_control_Type == "c")
                {
                    CheckBox options = new CheckBox();
                    options.ID = GetIDForGridControl(question, grid, option);

                    if (question.GridValidation == 1)
                        options.Attributes.Add("onclick", "resetcontrol(this, " + question.QuestionID + ")");

                    if (question.ResponseList.Count > 0)
                    {
                        foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
                        {
                            if (answer.ID == grid.GridStatementID && answer.Value == option.OptionValue)
                                options.Checked = true;
                        }
                    }
                    optionCell.Controls.Add(options);
                }
                row.Cells.Add(optionCell);
            }
            return row;
        }

        private int GetAnswersFromControl(List<ECN_Framework_Entities.Collector.Question> questions)
        {
            //index incrementer for Branching
            int PageIndexIncrementer = 0;

            foreach (ECN_Framework_Entities.Collector.Question question in questions)
            {
                List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(question.QuestionID, CurrentUser);
                ECN_Framework_BusinessLayer.Collector.Question.ResetAnswers(question);
                switch (question.Format)
                {
                    case "textbox":
                        TextBox txtControl = (TextBox)plSurveyContent.FindControl(GetIDForControl(question));
                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, txtControl.Text);
                        break;
                    case "dropdown":
                        DropDownList ddlControl = (DropDownList)plSurveyContent.FindControl(GetIDForControl(question));

                        string ddlvalue = ddlControl.SelectedValue;
                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, ddlControl.SelectedValue);

                        if (ECN_Framework_BusinessLayer.Collector.SurveyBranching.Exists(question.QuestionID, CurrentUser))
                            PageIndexIncrementer = GetBranchPageIndex(question, ddlvalue);

                        //ADD option TEXT value with ID = -2
                        if (question.ShowTextControl)
                        {
                            TextBox txtoptControl = (TextBox)plSurveyContent.FindControl(string.Format("question_{0}_TEXT", question.QuestionID.ToString()));
                            if ((roList[roList.Count - 1]).OptionValue == ddlvalue)
                                ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, -2, txtoptControl.Text);
                        }

                        break;
                    case "radio":
                        RadioButtonList rdoControl = (RadioButtonList)plSurveyContent.FindControl(GetIDForControl(question));
                        string rdoControlvalue = rdoControl.SelectedValue;

                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, rdoControlvalue);
                        if (ECN_Framework_BusinessLayer.Collector.SurveyBranching.Exists(question.QuestionID, CurrentUser))
                            PageIndexIncrementer = GetBranchPageIndex(question, rdoControlvalue);

                        //ADD option TEXT value with ID = -2
                        if (question.ShowTextControl)
                        {
                            TextBox txtoptControl = (TextBox)plSurveyContent.FindControl(string.Format("question_{0}_TEXT", question.QuestionID.ToString()));
                            if ((roList[roList.Count - 1]).OptionValue == rdoControlvalue)
                                ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, -2, txtoptControl.Text);
                        }

                        break;
                    case "checkbox":
                        string chkvalue = string.Empty;
                        CheckBoxList chkControl = (CheckBoxList)plSurveyContent.FindControl(GetIDForControl(question));
                        foreach (ListItem item in chkControl.Items)
                        {
                            if (item.Selected)
                            {
                                ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, item.Value);
                                chkvalue = item.Value;
                            }
                        }

                        //ADD option TEXT value with ID = -2
                        if (question.ShowTextControl)
                        {
                            TextBox txtoptControl = (TextBox)plSurveyContent.FindControl(string.Format("question_{0}_TEXT", question.QuestionID.ToString()));
                            if ((roList[roList.Count - 1]).OptionValue == chkvalue)
                                ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, -2, txtoptControl.Text);
                        }


                        break;
                    case "grid":
                        List<ECN_Framework_Entities.Collector.GridStatements> gsList = ECN_Framework_BusinessLayer.Collector.GridStatements.GetByQuestionID(question.QuestionID, CurrentUser);
                        foreach (ECN_Framework_Entities.Collector.GridStatements grid in gsList)
                        {

                            foreach (ECN_Framework_Entities.Collector.ResponseOptions option in roList)
                            {
                                if (question.Grid_control_Type == "r")
                                {
                                    RadioButton rdoGrid = (RadioButton)plSurveyContent.FindControl(GetIDForGridControl(question, grid, option));
                                    if (rdoGrid.Checked)
                                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, grid.GridStatementID, option.OptionValue);
                                }

                                else if (question.Grid_control_Type == "c")
                                {
                                    CheckBox chkBox = (CheckBox)plSurveyContent.FindControl(GetIDForGridControl(question, grid, option));

                                    if (chkBox.Checked)
                                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, grid.GridStatementID, option.OptionValue);
                                }

                            }
                        }
                        break;
                }
            }
            return PageIndexIncrementer;
        }

        private int GetBranchPageIndex(ECN_Framework_Entities.Collector.Question question, string optionvalue)
        {
            int newPageIndex = 0;
            List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(question.QuestionID, CurrentUser);
            foreach (ECN_Framework_Entities.Collector.ResponseOptions o in roList)
            {
                ECN_Framework_Entities.Collector.SurveyBranching sb = ECN_Framework_BusinessLayer.Collector.SurveyBranching.GetByOptionID(o.OptionID, CurrentUser);
                if (sb != null && o.OptionValue == optionvalue)
                {
                    if (sb.PageID == -1)
                    {
                        newPageIndex = SurveySession.Count - 1;
                        break;
                    }
                    else
                    {
                        for (int i = PageIndex + 1; i < SurveySession.Count; i++)
                        {
                            ECN_Framework_Entities.Collector.Page ss = GetPage(i);

                            if (ss.PageID == sb.PageID)
                            {
                                newPageIndex = i;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return newPageIndex;
        }
        private string GetIDForControl(ECN_Framework_Entities.Collector.Question question)
        {
            return string.Format("question_{0}", question.QuestionID.ToString());
        }

        private string GetIDForGridControl(ECN_Framework_Entities.Collector.Question question, ECN_Framework_Entities.Collector.GridStatements grid, ECN_Framework_Entities.Collector.ResponseOptions option)
        {
            return string.Format("question_{0}_{1}_{2}", question.QuestionID, grid.GridStatementID, option.OptionID);
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion

        protected void btnPrevious_Click(object sender, System.EventArgs e)
        {
            PageIndex--;

            for (int i = PageIndex; i >= 0; i--)
            {
                ECN_Framework_Entities.Collector.Page ss1 = GetPage(i);
                if (ss1.IsIntroPage || ss1.IsThankYouPage)
                {
                    break;
                }

                if (ss1.IsSkipped)
                {
                    PageIndex--;
                }
            }
            RenderPage(true);

        }

        protected void btnNext_Click(object sender, System.EventArgs e)
        {
            ECN_Framework_Entities.Collector.Page ss = GetPage(PageIndex); ;

            if (!(ss.IsIntroPage || ss.IsThankYouPage))
            {
                int PageIndexIncrementer = SaveAnswers(ss.QuestionList);

                if (PageIndexIncrementer == 0)
                {
                    PageIndex++;
                    ((ECN_Framework_Entities.Collector.Page)GetPage(PageIndex)).IsSkipped = false;
                }
                else
                {
                    for (int i = PageIndex + 1; i < SurveySession.Count; i++)
                    {
                        if (i == PageIndexIncrementer)
                            break;
                        else
                        {
                            ECN_Framework_Entities.Collector.Page ss1 = GetPage(i);
                            ss1.IsSkipped = true;
                            if (!IsViewOnly)
                            {
                                foreach (ECN_Framework_Entities.Collector.Question q in ss1.QuestionList)
                                {
                                    ECN_Framework_BusinessLayer.Collector.Response.Delete(GetSurveyID(), ParticipantID, q.Number, CurrentUser);
                                }
                            }
                        }
                    }
                    PageIndex = PageIndexIncrementer;
                }
            }
            else
            {
                PageIndex++;
            }

            if (!IsViewOnly && PageIndex == SurveySession.Count - 1)
            {
                ECN_Framework_BusinessLayer.Collector.Participant.InsertCompletionDt(ParticipantID, _surveyID, CurrentUser);
                if (BlastID > 0)
                {
                    ECN_Framework_BusinessLayer.Collector.Participant.InsertBlastID(ParticipantID, _surveyID, BlastID, CurrentUser);
                }
            }
            RenderPage(true);
        }

        private int SaveAnswers(List<ECN_Framework_Entities.Collector.Question> questions)
        {
            int PageIndexIncrementer = GetAnswersFromControl(questions);

            if (!IsViewOnly)
            {
                string userName = UserName;
                //if particpant ID = 0, create the user and add to the survey group
                if (ParticipantID == 0)
                {
                    if (userName == string.Empty)
                        userName = String.Format("{0}-{1}@survey_{2}.com", System.Guid.NewGuid().ToString().Substring(1, 5), System.DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss"), _surveyID);
                    ECN_Framework_BusinessLayer.Collector.Survey.AddUserToSurveyGroup(_surveyID, userName, Request.UserHostAddress, CurrentUser);
                }
                if(CurrentSurvey == null)
                    CurrentSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(_surveyID, CurrentUser);
                if (ParticipantID == 0)
                {
                    ParticipantID = (ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(userName, CurrentSurvey.CustomerID)).EmailID;

                }
                foreach (ECN_Framework_Entities.Collector.Question q in questions)
                {
                    ECN_Framework_BusinessLayer.Collector.Response.Save_NoAccessCheck(q, ParticipantID, GetSurveyID(), CurrentUser);
                }
            }
            return PageIndexIncrementer;
        }

        private void LoadAnswers()
        {
            if (!IsViewOnly)
            {
                for (int i = 0; i < SurveySession.Count; i++)
                {
                    ECN_Framework_Entities.Collector.Page ss = GetPage(i);
                    if (!(ss.IsThankYouPage || ss.IsIntroPage))
                    {
                        foreach (ECN_Framework_Entities.Collector.Question question in ss.QuestionList)
                        {
                            List<ECN_Framework_Entities.Collector.Response> responseList = ECN_Framework_BusinessLayer.Collector.Response.GetByQuestion(_surveyID, question.Number, ParticipantID, CurrentUser);
                            ECN_Framework_BusinessLayer.Collector.Question.LoadResponses(question, responseList);
                            if (question.ShowTextControl)
                            {
                                try
                                {
                                    string txtvalue = ECN_Framework_BusinessLayer.Collector.Response.GetTEXTResponses(_surveyID, question.Number, ParticipantID, CurrentUser);
                                    if (txtvalue != string.Empty)
                                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, -2, txtvalue);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }

        private void ShowProgress(bool isProgressBarVisible)
        {
            if (isProgressBarVisible)
            {
                int totalpages = 0;
                int percentage = 0;
                double PercentforPage = 0;

                if (((ECN_Framework_Entities.Collector.Page)GetPage(0)).IsIntroPage)
                {
                    totalpages = SurveySession.Count - 2;
                    PercentforPage = 100.00 / totalpages;

                    if (PageIndex > 1)
                        percentage = Convert.ToInt32((PageIndex - 1) * PercentforPage);
                }
                else
                {
                    totalpages = SurveySession.Count - 1;
                    PercentforPage = 100.00 / totalpages;

                    if (PageIndex > 0)
                        percentage = Convert.ToInt32(PageIndex * PercentforPage);

                }

                plbarHTML.Controls.Clear();
                plbarHTML.Controls.Add(new LiteralControl("<table cellpadding='0' cellspacing='0' width='250px' style='padding-top:5px;' ><tr><td width='5%' class='progresslabel'>" + percentage + "%&nbsp;</td><td><table  style='border:1px solid #cccccc;' cellpadding='0' cellspacing='0' width='100%'><tr><td width='" + percentage + "%' style='background:url(/ecn.images/images/green_02.jpg)'></td><td width='95%'>&nbsp;</td></tr></table></td></tr></table>"));
                plbarHTML.Visible = true;
                plProgressBar.Visible = true;
            }
            else
            {
                plProgressBar.Visible = false;
                plbarHTML.Controls.Clear();
                plbarHTML.Visible = false;
            }
        }
    }
}
