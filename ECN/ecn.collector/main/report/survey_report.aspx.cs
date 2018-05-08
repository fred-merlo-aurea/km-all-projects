using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Linq;
using CrystalDecisions.Shared; 
using CrystalDecisions.CrystalReports.Engine;
using ECN_Framework_Common.Objects;
using System.Collections.Generic;
using System.Text;

namespace ecn.collector.main.report {

    public partial class SurveyReport : Page
	{
        private Hashtable htFilters = new Hashtable();

		private int SurveyID 
		{
			get 
			{
				try{return Convert.ToInt32(Request.QueryString["surveyID"]);}
				catch{return 0;}
			}
		}

		private string Filters
		{
			get 
			{
				try{return ViewState["Filters"].ToString();}
				catch{return string.Empty;}
			}
			set
			{
				ViewState["Filters"] = value;
			}
		}


		protected void Page_Load(object sender, System.EventArgs e) 
		{
			lblErrorMessage.Visible = false;
			if (!IsPostBack)
			{
                Master.CurrentMenuCode = ECN_Framework_Common.Objects.Collector.Enums.MenuCode.SURVEY; 
				ViewState["SortField"] = "CompletedDate";
				ViewState["SortDirection"] = "DESC";
                ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);
                lblSurveyTitle.Text = s.SurveyTitle;
                lblTotalCompleted.Text = ECN_Framework_BusinessLayer.Collector.Survey.GetCompletedResponseCount(SurveyID, string.Empty).ToString();
				lblTotalAbondoned.Text = ECN_Framework_BusinessLayer.Collector.Survey.GetIncompleteResponseCount(SurveyID).ToString();

				lblTotalRespondents.Text = Convert.ToString(Convert.ToInt32(lblTotalCompleted.Text) + Convert.ToInt32(lblTotalAbondoned.Text));

				phResults.Visible = true;
				phRespondent.Visible = false;
			}
			LoadQuestionGrid();
		}

        ReportDocument report = new ReportDocument();
        protected void Page_Unload(object sender, System.EventArgs e)
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }

		private void LoadFilterGrid() 
		{
			if (Filters != string.Empty)
			{
				htFilters.Clear();

				string[] strFilters = Filters.ToString().Split(',');
					
				for (int i=0; i<strFilters.Length;i++)
				{
					string[] IDs = strFilters[i].ToString().Split('|');

					if (!htFilters.ContainsKey(Convert.ToInt32(IDs[0])))
					{
						if (IDs.Length != 3)
							htFilters.Add(Convert.ToInt32(IDs[0]), IDs[1]);
						else
							htFilters.Add(Convert.ToInt32(IDs[0]), IDs[1] + "|" + IDs[2]);
					}
				}
			}

            dgFilter.DataSource = ECN_Framework_BusinessLayer.Collector.Question.GetFilterValues(Filters);
			dgFilter.DataBind();
		}

		protected void dgFilter_ItemCommand(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string strFilter = string.Empty;

			if (e.CommandName.ToLower() == "remove")
			{
				if (Filters != string.Empty)
				{
					string[] strFilters = Filters.ToString().Split(',');
					
					for (int i=0; i<strFilters.Length;i++)
					{
						string[] IDs = strFilters[i].ToString().Split('|');

						if (IDs[0].ToString() != e.CommandArgument.ToString())
						{
							if (IDs.Length != 3)
								strFilter += (strFilter==string.Empty?IDs[0].ToString() +"|" + IDs[1].ToString():"," +IDs[0].ToString() +"|" + IDs[1].ToString());
							else
								strFilter += (strFilter==string.Empty?IDs[0].ToString() +"|" + IDs[1].ToString() +"|" + IDs[2].ToString():"," +IDs[0].ToString() +"|" + IDs[1].ToString()+"|" + IDs[2].ToString());
						}
					}
				}
			}
			Filters = strFilter;
			htFilters.Clear();
			LoadQuestionGrid();
            LoadRespondentGrid();
		}

		private void LoadQuestionGrid() 
		{
			LoadFilterGrid();
            lblFilterCount.Text = ECN_Framework_BusinessLayer.Collector.Survey.GetCompletedResponseCount(SurveyID, Filters).ToString();
            DataTable dtQuestion = ECN_Framework_BusinessLayer.Collector.Question.GetQuestionsWithFilterCount(SurveyID, Filters);
			repQuestions.DataSource = dtQuestion;
			repQuestions.DataBind();
			dgQuestions.DataSource = dtQuestion;
			dgQuestions.DataBind();		
		}

		protected void repQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Separator)
			{
				rowcount = 1;
			}
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				PlaceHolder plotherformat = (PlaceHolder)e.Item.FindControl("plotherformat");
				PlaceHolder plgridformat = (PlaceHolder)e.Item.FindControl("plgridformat");
				
				Label lblQuestiontype = (Label)e.Item.FindControl("lblQuestiontype");
				Label lblQuestionID = (Label)e.Item.FindControl("lblQuestionID");
				Label lblTotalRespondentsCount = (Label)e.Item.FindControl("lblTotalRespondentsCount");

                if (rbpercentusing.SelectedItem.Value == "1")
                    lblTotalRespondentsCount.Text = lblFilterCount.Text;
                else
                    lblTotalRespondentsCount.Text = getrespondentsforQuestion(Convert.ToInt32(lblQuestionID.Text));

				if (lblQuestiontype.Text.ToLower() == "radio" ||lblQuestiontype.Text.ToLower() == "checkbox" ||lblQuestiontype.Text.ToLower() == "dropdown" || lblQuestiontype.Text.ToLower() == "textbox")
				{
					Repeater repAnswers = (Repeater) e.Item.FindControl("repAnswers");
                    repAnswers.DataSource = LoadAnswersGrid(Convert.ToInt32(lblQuestionID.Text), Convert.ToInt32(lblTotalRespondentsCount.Text));
					repAnswers.DataBind();

					plotherformat.Visible = true;
					plgridformat.Visible=false;

				}
				else if (lblQuestiontype.Text.ToLower() == "grid")
				{
					DataGrid dgGridResponse = (DataGrid) e.Item.FindControl("dgGridResponse");

					dgGridResponse.DataSource = LoadAnswersGrid(Convert.ToInt32(lblQuestionID.Text), 0);
					dgGridResponse.DataBind();
					
					plgridformat.Visible=true;
					plotherformat.Visible = false;
				}
			}
		}

        private string getrespondentsforQuestion(int questionID)
        {
            string totalrespondentsforquestion = "0";

            try
            {
                foreach (DataGridItem dItem in dgQuestions.Items)
                {
                    if (dItem.ItemType == ListItemType.Item || dItem.ItemType == ListItemType.AlternatingItem)
                    {
                        if (Convert.ToInt32(dgQuestions.DataKeys[dItem.ItemIndex]) == questionID)
                        {
                            Label lblQuestionTotal = (Label) dItem.FindControl("lblQuestionTotal");
                            totalrespondentsforquestion = lblQuestionTotal.Text; // dItem.Cells[2].Text;
                            break;
                        }
                    }
                }
            }
            catch
            { }

            return totalrespondentsforquestion;
        }

		private DataTable LoadAnswersGrid(int QuestionID, int QuestionCount)
		{
            return ECN_Framework_BusinessLayer.Collector.Participant.GetQuestionResponse(0, QuestionID, Filters, QuestionCount);
		}

		int rowcount = 1;
		public void repAnswers_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				string classname = string.Empty;

				PlaceHolder plbar = (PlaceHolder) e.Item.FindControl("plbar");
				Label lblQID = (Label) e.Item.FindControl("lblQID");
				Label lblOID = (Label) e.Item.FindControl("lblOID");
                bool bHasOtherResponse = ((Label) e.Item.FindControl("lblHasOtherResponse")).Text=="1"?true:false;

                Label lbloptionvalue = (Label)e.Item.FindControl("lbloptionvalue");
				if (Convert.ToInt32(lblOID.Text) == 0)
				{
                    lbloptionvalue.Text = "<a href=\"javascript:void(0)\" style=\"color:blue;font-weight:normal;text-decoration:underline\" onclick=\"window.open('ViewTextResponse.aspx?qid=" + lblQID.Text + "&filterstr=" + Filters + "','','width=700,height=600,resizable=yes,scrollbars=yes');\">Answered</a>";
				}
                else if (bHasOtherResponse)
                    lbloptionvalue.Text = "<a href=\"javascript:void(0)\" style=\"color:blue;font-weight:normal;text-decoration:underline\" onclick=\"window.open('ViewTextResponse.aspx?qid=" + lblQID.Text + "&Other=1&filterstr=" + Filters + "','','width=700,height=600,resizable=yes,scrollbars=yes');\">" + lbloptionvalue.Text + "</a>";
                    

				PlaceHolder plCheckbox = (PlaceHolder) e.Item.FindControl("plCheckbox");
				plCheckbox.Controls.Add(CreateCheckboxControl(Convert.ToInt32(lblQID.Text), 0, Convert.ToInt32(lblOID.Text)));

				Label lblRatio = (Label) e.Item.FindControl("lblRatio");
				switch (rowcount) 
				{
					case 1:
						rowcount++;
						classname = "bargreen";
						break;
					case 2:
						rowcount++;
						classname = "barred";
						break;
					case 3:
						rowcount++;
						classname = "barteal";
						break;
					case 4:
						rowcount++;
						classname = "barblue";
						break;
					case 5:
						classname = "baryellow";
						rowcount =1;
						break;
				}

				if (lblRatio.Text != "0")
					plbar.Controls.Add(new LiteralControl("<Div class='" + classname + "' style='width:100%;height:17px;vertical-align:middle;'></div>"));//" + lblRatio.Text+ "% 

			}
		}

		private CheckBox CreateCheckboxControl(int QuestionID, int Statement_ID, int OptionID) 
		{			
			CheckBox chk = new CheckBox();
			chk.Checked = false;
			chk.Attributes.Add("onclick","resetcheckbox(this, " + QuestionID + ")");

			if (Statement_ID == 0)
				chk.ID = string.Format("checkbox_{0}_{1}", QuestionID, OptionID);
			else
				chk.ID = string.Format("checkbox_{0}_{1}_{2}", QuestionID, Statement_ID, OptionID);

			if (htFilters.ContainsKey(QuestionID))
			{
				string Statement_OptionID = htFilters[QuestionID].ToString();

				if (Statement_OptionID.IndexOf("|") > -1)
				{
					if (Convert.ToInt32(Statement_OptionID.Substring(0,Statement_OptionID.IndexOf("|")))== Statement_ID && Convert.ToInt32(Statement_OptionID.Substring(Statement_OptionID.IndexOf("|")+1)) == OptionID)
						chk.Checked = true;
				}
				else
				{
					if (Convert.ToInt32(htFilters[QuestionID]) == OptionID)
						chk.Checked = true;
				}
			}

			return chk;
		}

		protected void lnkSurveyResults_Click(object sender, System.EventArgs e)
		{
			phRespondent.Visible = false;
			phResults.Visible = true;
			LoadQuestionGrid();
		}

		protected void lnkToRespondents_Click(object sender, System.EventArgs e)
		{
			phRespondent.Visible = true;
			phResults.Visible = false;
			dgRespondent.CurrentPageIndex = 0;
			RespondentPager.CurrentIndex =0;
			LoadRespondentGrid();
			btnSaveToGroup.Visible = false;
			rbExistingGroup.Checked = false;
			rbNewGroup.Checked = false;
			plExistingGroup.Visible=false;
			plNewGroup.Visible = false;
		}

		private void FilterResults()
		{
			string strFilter = string.Empty;

			foreach (RepeaterItem qitem in repQuestions.Items)
			{
				Repeater repAnswers = (Repeater) qitem.FindControl("repAnswers");
				Label lblQuestiontype = (Label) qitem.FindControl("lblQuestiontype");
				Label lblQuestionID = (Label) qitem.FindControl("lblQuestionID");

				if (lblQuestiontype.Text.ToLower() == "textbox" || lblQuestiontype.Text.ToLower() == "radio" ||lblQuestiontype.Text.ToLower() == "checkbox" ||lblQuestiontype.Text.ToLower() == "dropdown")
				{
					foreach (RepeaterItem aitem in repAnswers.Items)
					{
						Label lblQID = (Label) aitem.FindControl("lblQID");
						Label lblOID = (Label) aitem.FindControl("lblOID");

						PlaceHolder plCheckbox = (PlaceHolder) aitem.FindControl("plCheckbox");

						CheckBox chk = (CheckBox) plCheckbox.FindControl(string.Format("checkbox_{0}_{1}", lblQID.Text, lblOID.Text));

						if (chk != null)
						{
							if (chk.Checked)
								strFilter += (strFilter==string.Empty?lblQID.Text +"|" + lblOID.Text:"," +lblQID.Text.ToString() +"|" + lblOID.Text);

						}
					}
				}
				else if (lblQuestiontype.Text.ToLower() == "grid")
				{
					DataGrid dgGridResponse = (DataGrid) qitem.FindControl("dgGridResponse");
					foreach (DataGridItem dgitem in dgGridResponse.Items)
					{
						for (int i=3;i<dgitem.Cells.Count;i++)
						{
							if (i % 2 == 0)
							{
								try
								{
									CheckBox chk = (CheckBox) dgitem.Cells[i].FindControl(string.Format("checkbox_{0}_{1}_{2}", dgitem.Cells[0].Text, dgitem.Cells[1].Text, dgitem.Cells[i-1].Text));

									if (chk.Checked)
									{
										strFilter += (strFilter==string.Empty?dgitem.Cells[0].Text +"|" + dgitem.Cells[1].Text +"|" + dgitem.Cells[i-1].Text:"," +dgitem.Cells[0].Text +"|"  + dgitem.Cells[1].Text +"|" + dgitem.Cells[i-1].Text);
										//Response.Write( dgitem.Cells[0].Text + ", " + dgitem.Cells[1].Text + "," + dgitem.Cells[i-1].Text  +"<BR>");
									}
								}
								catch
								{}

							}
						}
					}
				}

			}
			Filters = strFilter;
			LoadQuestionGrid();
            LoadRespondentGrid();
		}

		public void dgGridResponse_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				
				e.Item.Cells[0].Visible = false;
				e.Item.Cells[1].Visible = false;
				e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Left;
				e.Item.Cells[2].Width= Unit.Percentage(30);

				for (int i=3;i<e.Item.Cells.Count;i++)
				{
					if (i % 2 == 0)
					{
						if (e.Item.Cells[i].Text == "0" )
						{
							//e.Item.Cells[i].Controls.Add(CreateCheckboxControl(Convert.ToInt32(e.Item.Cells[0].Text), Convert.ToInt32(e.Item.Cells[1].Text), Convert.ToInt32(e.Item.Cells[i-1].Text)));
							e.Item.Cells[i].Controls.Add(new LiteralControl("&nbsp;"));
						}
						else
						{
							//e.Item.Cells[i].Text = "<input type='checkbox'>&nbsp;" + e.Item.Cells[i].Text;

							e.Item.Cells[i].Controls.Add(CreateCheckboxControl(Convert.ToInt32(e.Item.Cells[0].Text), Convert.ToInt32(e.Item.Cells[1].Text), Convert.ToInt32(e.Item.Cells[i-1].Text)));
							e.Item.Cells[i].Controls.Add(new LiteralControl("&nbsp;" + e.Item.Cells[i].Text));
						}
					}
					else
						e.Item.Cells[i].Visible = false;
	
				}
			}
			else if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
			{
				e.Item.Cells[0].Visible = false;
				e.Item.Cells[1].Visible = false;

				e.Item.Cells[2].Text="";

				for (int i=3;i<e.Item.Cells.Count;i++)
				{
					if (i % 2 != 0)
						e.Item.Cells[i].Visible = false;
				}
			}
		}

		#region Respondent view
		private void LoadRespondentGrid()
		{
            List<ECN_Framework_Entities.Communicator.Email> eList = ECN_Framework_BusinessLayer.Collector.Participant.GetParticipants(SurveyID, Filters);
            var result = (from src in eList
                          select new
                          {
                                EmailID= src.EmailID,
                                EmailAddress = src.EmailAddress.Contains("@survey_" + SurveyID.ToString()) ? "Anonymous": src.EmailAddress,
                                SurveyID = SurveyID.ToString()
                          }).ToList();

            if(ViewState["SortDirection"].ToString()=="DESC")
            {
                 result = (from src in result
                           orderby src.EmailAddress descending
                            select src).ToList();
            }
            else
            {
                 result = (from src in result
                           orderby src.EmailAddress 
                            select src).ToList();
            }
			dgRespondent.DataSource = result;
			dgRespondent.DataBind();
            RespondentPager.RecordCount = result.Count;
		}		

		protected void RespondentPager_IndexChanged(object sender, System.EventArgs e)
		{
			LoadRespondentGrid();
		}

		protected void dgRespondent_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
			if (e.SortExpression.ToLower() == ViewState["SortField"].ToString().ToLower())
			{
				switch (ViewState["SortDirection"].ToString())
				{
					case "ASC":
						ViewState["SortDirection"] = "DESC";
						break;
					case "DESC":
						ViewState["SortDirection"] = "ASC";
						break;
				}
			}
			else
			{
				ViewState["SortField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}

			LoadRespondentGrid();
		}

		protected void rbExistingGroup_CheckedChanged(object sender, System.EventArgs e)
		{
			plExistingGroup.Visible=true;
			plNewGroup.Visible = false;
			LoadRespondentGrid();
			LoadGroupsDR();
			btnSaveToGroup.Visible = true;

		}

		protected void rbNewGroup_CheckedChanged(object sender, System.EventArgs e)
		{
			plExistingGroup.Visible=false;
			plNewGroup.Visible = true;
			LoadRespondentGrid();
			btnSaveToGroup.Visible = true;

		}

		private void LoadGroupsDR()
		{
            List<ECN_Framework_Entities.Communicator.Group> gList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            var result = (from src in gList
                          orderby src.GroupName
                          select src).ToList();
            drpGroup.DataSource = result;
			drpGroup.DataTextField = "GroupName";
			drpGroup.DataValueField = "GroupID";
			drpGroup.DataBind();

			drpGroup.Items.Insert(0, new ListItem("----- Select List -----","0"));
		}

		private void showDivAddToGroup()
		{
			ClientScript.RegisterStartupScript(typeof(Page), "loadPage","<script language='javascript'>onload=function(){showDivPopup('divAddToGroup');};</script>");
		}

		protected void btnSaveToGroup_Click(object sender, EventArgs e)
		{
			string gname = string.Empty;
			int GroupID = 0;
 
			if (Page.IsValid)
			{
				if (rbNewGroup.Checked || rbExistingGroup.Checked)
				{
					if (rbNewGroup.Checked)
					{
                        try
                        {
                            gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(txtGroupName.Text);
                            ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                            group.GroupName = gname;
                            group.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                            group.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                            group.FolderID = 0;
                            group.PublicFolder = 0;
                            group.IsSeedList = false;
                            group.AllowUDFHistory = "N";
                            group.OwnerTypeCode = "customer";
                            ECN_Framework_BusinessLayer.Communicator.Group.Save(group, Master.UserSession.CurrentUser);
                            if (ECN_Framework_BusinessLayer.Communicator.UserGroup.Exists(Master.UserSession.CurrentUser.UserID))
                            {
                                ECN_Framework_Entities.Communicator.UserGroup ug = new ECN_Framework_Entities.Communicator.UserGroup();
                                ug.UserID = Master.UserSession.CurrentUser.UserID;
                                ug.GroupID = group.GroupID;
                                ug.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                                ug.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                                ECN_Framework_BusinessLayer.Communicator.UserGroup.Save(ug, Master.UserSession.CurrentUser);
                            }
                            GroupID = group.GroupID;
						} 
						catch(ECNException ex) 
						{
							lblErrorMessage.Text = "<font color='#000000'>\""+gname+"\"</font> already exists. Please enter a different name.";
							lblErrorMessage.Visible = true;
                            return;
						}
					}
					else
					{
						GroupID = Convert.ToInt32(drpGroup.SelectedItem.Value);
					}

					if (GroupID > 0)
					{
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, Master.UserSession.CurrentUser);
                        List<ECN_Framework_Entities.Communicator.Email> eList = ECN_Framework_BusinessLayer.Collector.Participant.GetParticipants(SurveyID, Filters);
                        var result = (from src in eList
                                      where src.EmailAddress.Contains("@survey_" + SurveyID.ToString()) == false
                                      select src).ToList();

                        StringBuilder xmlUDF = new StringBuilder("");
                        xmlUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>");
                        StringBuilder xmlProfile = new StringBuilder("");
                        xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                        foreach(ECN_Framework_Entities.Communicator.Email email in result)
                        {
                            xmlProfile.Append("<Emails>");
                            xmlProfile.Append("<emailaddress>");
                            xmlProfile.Append(email.EmailAddress);
                            xmlProfile.Append("</emailaddress>");
                            xmlProfile.Append("</Emails>");
                        }
                        xmlProfile.Append("</XML>");
                        ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(Master.UserSession.CurrentUser, Master.UserSession.CurrentUser.CustomerID, GroupID, xmlProfile.ToString(), xmlUDF.ToString(), "HTML", "S", false,"","Ecn.collector.main.report.survey_report.btnSaveToGroup_Click");
                        int TotalInserts = result.Count;
                        if (TotalInserts > 1)
                        {
                            lblErrorMessage.Text = TotalInserts + " emails have been added to the " + group.GroupName + " group.";
                        }
                        else
                        {
                            lblErrorMessage.Text = TotalInserts + " email has been added to the " + group.GroupName + " group.";
                        }
                        lblErrorMessage.Visible = true;
                        txtGroupName.Text = "";
					}

				}
				else
				{
                    lblErrorMessage.Text = "Select New group or Existing group ";
					lblErrorMessage.Visible = true;
                    return;
				}
			}
			LoadRespondentGrid();
            
		}

		#endregion

		protected void lnktoExl_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			GenerateReport(CRExportEnum.XLS);
		}
        protected void lnkToPDF_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			GenerateReport(CRExportEnum.PDF);
		}

		private void GenerateReport(CRExportEnum ExportFormat)
		{

			Hashtable cParams = new Hashtable();
			cParams.Add("@SurveyID", SurveyID.ToString());
			cParams.Add("@filterstr",Filters.ToString());
			cParams.Add("@EmailID","0");
			cParams.Add("@title",lblSurveyTitle.Text.ToString());
            cParams.Add("@totalRespondent", lblFilterCount.Text.ToString());
            if (phResults.Visible)
                cParams.Add("@percentusing", rbpercentusing.SelectedItem.Value);
            cParams.Add("@ExportFormat", ExportFormat);

			string reportname = string.Empty;

			report = new ReportDocument();

			report = CRReport.GetReport(Server.MapPath((phResults.Visible? "rpt_SurveyReport.rpt":"rpt_SurveyRespondents.rpt")), cParams);

            
			crv.ReportSource = report;
			crv.Visible = true;
            

			CRReport.Export(report, ExportFormat, (phResults.Visible? "SurveyReport_":"SurveyRespondent_")+ SurveyID.ToString() + "." + ExportFormat.ToString());
		}

        protected void repQuestions_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			if (e.CommandName.ToLower() == "filter")
			{
				FilterResults();
			}
		}

        protected void rbpercentusing_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (phResults.Visible)
                lnkSurveyResults_Click(sender, e);
            else
                lnkToRespondents_Click(sender, e);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);
            string groupID = s.GroupID.ToString();
            string jsScript = " window.open('download.aspx?chID="+ Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString() + "&custID=" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "&grpID=" + groupID + "&fileType=.xls&subType=*&srchType=like&srchEm=', '', 'width=500,height=200status=yes,toolbar=no,scrollbar=yes');";        
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), jsScript, true);

            LoadRespondentGrid();
        }

        protected void btnExportToGroup_Click(object sender, EventArgs e)
        {
            LoadRespondentGrid();
            mpeExportToGroup.Show();
        }

        protected void imgbtnCloseExport_Click(object sender, EventArgs e)
        {
            mpeExportToGroup.Hide();
        }
	}
}
