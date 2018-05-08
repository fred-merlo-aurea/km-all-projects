using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;

namespace ecn.collector.main.survey.UserControls
{
	public partial class DefineTemplate : System.Web.UI.UserControl, IWizard
	{

		int _surveyID = 0;
		protected System.Web.UI.WebControls.Button btnSelect;
		protected System.Web.UI.WebControls.Button btnCustomize;
		protected System.Web.UI.WebControls.ImageButton btnSelectTemplate;
        
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

		public void Initialize() 
		{
			if (SurveyID > 0)
			{
				try
				{
					lblTemplateErrorMessage.Text = "";
					lblTemplateErrorMessage.Visible = false;
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);


                    if (objSurvey.ResponseCount>0)
					{
						plToobar.Visible = false;
						plTemplates.Visible = false;
					}
					else
					{
						hImage.Attributes.Add("readonly","true");
						fImage.Attributes.Add("readonly","true");

						btnTemplateSave.Attributes.Add("onclick","javascript:return tValidate();");

						hleftmargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						hrightmargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						htopmargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						hbottommargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");

						fleftmargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						frightmargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						ftopmargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						fbottommargin.Attributes.Add("onkeypress","return checkKeyPressForInteger(this, event)");
						LoadTemplates();
					}
					LoadStylesFromSurvey(SurveyID);
				}
				catch (Exception ex)
				{
					ErrorMessage = ex.Message;
				}
			}
		}

		public bool Save() 
		{
			if (Page.IsValid)
			{
				try
				{
                    ECN_Framework_Entities.Collector.SurveyStyles sStyle = new ECN_Framework_Entities.Collector.SurveyStyles();
					sStyle.SurveyID = SurveyID;
					//Page
					sStyle.pbgcolor = pbgcolor.Text ;
					sStyle.pBorder = pborder.SelectedItem.Value=="1"?true:false;
					sStyle.pBordercolor=pBordercolor.Text;
					sStyle.pfontfamily = pfont.SelectedItem.Value;

					sStyle.pWidth =pWidth.SelectedItem.Value;
					sStyle.pAlign= pAlign.SelectedItem.Value;
					sStyle.bbgcolor = bbgcolor.Text;
                    
					//Header
					sStyle.hbgcolor= hbgcolor.Text;
					sStyle.hAlign = halign.SelectedItem.Value;
                    StringBuilder hMargin = new StringBuilder();
                    hMargin.Append(hleftmargin.Text == string.Empty ? "0" : hleftmargin.Text + " px");
                    hMargin.Append(htopmargin.Text == string.Empty ? "0" : htopmargin.Text + " px");
                    hMargin.Append(hbottommargin.Text == string.Empty ? "0" : hbottommargin.Text + " px");
                    hMargin.Append(hrightmargin.Text == string.Empty ? "0" : hrightmargin.Text + " px");
                    sStyle.hMargin = hMargin.ToString();
					sStyle.hImage = hImage.Text;
				
					//footer
					sStyle.fbgcolor=fbgcolor.Text;
					sStyle.fAlign = falign.SelectedItem.Value;
                    StringBuilder fMargin = new StringBuilder();
                    fMargin.Append(fleftmargin.Text == string.Empty ? "0" : fleftmargin.Text + " px");
                    fMargin.Append(ftopmargin.Text == string.Empty ? "0" : ftopmargin.Text + " px");
                    fMargin.Append(fbottommargin.Text == string.Empty ? "0" : fbottommargin.Text + " px");
                    fMargin.Append(frightmargin.Text == string.Empty ? "0" : frightmargin.Text + " px");
                    sStyle.fMargin = fMargin.ToString();
                    sStyle.fImage = fImage.Text;
					//Page Header
					sStyle.phbgcolor= phbgcolor.Text;
					sStyle.phcolor=phcolor.Text;
					sStyle.phfontsize=phfontsize.SelectedItem.Value;
					sStyle.phBold = phbold.SelectedItem.Value=="1"?true:false;
				
					//Page Description
					sStyle.pdbgcolor = pdbgcolor.Text;
					sStyle.pdcolor = pdcolor.Text;
					sStyle.pdfontsize = pdfontsize.SelectedItem.Value;
					sStyle.pdbold = pdbold.SelectedItem.Value=="1"?true:false;

					//question
                    sStyle.ShowQuestionNo = drpShowQuestionNO.SelectedItem.Value == "1" ? true : false;
					sStyle.qcolor=qcolor.Text;
					sStyle.qfontsize = qfontsize.SelectedItem.Value;
					sStyle.qbold = qbold.SelectedItem.Value=="1"?true:false;

					//answer
					sStyle.acolor = acolor.Text;
					sStyle.afontsize = afontsize.SelectedItem.Value;
					sStyle.abold = abold.SelectedItem.Value=="1"?true:false;

                    ECN_Framework_BusinessLayer.Collector.SurveyStyles.Save(sStyle, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    objSurvey.CompletedStep = 3;
                    ECN_Framework_BusinessLayer.Collector.Survey.Save(objSurvey, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
				}
				catch (Exception ex)
				{
					ErrorMessage = ex.Message;
				}
			}
			return true;
		}

        private void LoadStylesFromSurvey(int SurveyID)
		{
			if (SurveyID > 0)
			{
                ECN_Framework_Entities.Collector.SurveyStyles sStyle = ECN_Framework_BusinessLayer.Collector.SurveyStyles.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
					
                //Page
                pbgcolor.Text = sStyle.pbgcolor;
                pborder.ClearSelection();
                pborder.Items.FindByValue(sStyle.pBorder ? "1" : "0").Selected = true;
                pBordercolor.Text = sStyle.pBordercolor;
                pfont.ClearSelection();
                pfont.Items.FindByValue(sStyle.pfontfamily).Selected = true;
                pWidth.ClearSelection();
                pWidth.Items.FindByValue(sStyle.pWidth).Selected = true;
                pAlign.ClearSelection();
                pAlign.Items.FindByValue(sStyle.pAlign).Selected = true;
                bbgcolor.Text = sStyle.bbgcolor;

                char[] delimiter = { ' ' };
                string hMargin = sStyle.hMargin.Replace("px", "");
                string[] ahMargin = hMargin.Split(delimiter);

                string hleftmarginValue = ahMargin[0];
                string htopmarginValue = ahMargin[1];
                string hbottommarginValue = ahMargin[2];
                string hrightmarginValue = ahMargin[3];

                string fMargin = sStyle.fMargin.Replace("px", "");
                string[] afMargin = fMargin.Split(delimiter);

                string fleftmarginValue = afMargin[0];
                string ftopmarginValue = afMargin[1];
                string fbottommarginValue = afMargin[2];
                string frightmarginValue = afMargin[3];

                //Header
                hbgcolor.Text = sStyle.hbgcolor;
                halign.ClearSelection();
                halign.Items.FindByValue(sStyle.hAlign).Selected = true;
                hleftmargin.Text = hleftmarginValue.ToString();
                hrightmargin.Text = hrightmarginValue.ToString();
                htopmargin.Text = htopmarginValue.ToString();
                hbottommargin.Text = hbottommarginValue.ToString();
                hImage.Text = sStyle.hImage;
                imgHeader.ImageUrl = sStyle.hImage;
                //footer
                fbgcolor.Text = sStyle.fbgcolor;
                falign.ClearSelection();
                falign.Items.FindByValue(sStyle.fAlign).Selected = true;
                fleftmargin.Text = fleftmarginValue.ToString();
                frightmargin.Text = frightmarginValue.ToString();
                ftopmargin.Text = ftopmarginValue.ToString();
                fbottommargin.Text = fbottommarginValue.ToString();
                fImage.Text = sStyle.fImage;
                imgFooter.ImageUrl = sStyle.fImage;
                //Page Header
                phbgcolor.Text = sStyle.phbgcolor;
                phcolor.Text = sStyle.phcolor;
                phfontsize.ClearSelection();
                phfontsize.Items.FindByValue(sStyle.phfontsize).Selected = true;
                phbold.ClearSelection();
                phbold.Items.FindByValue(sStyle.phBold ? "1" : "0").Selected = true;

                //Page Description
                pdbgcolor.Text = sStyle.pdbgcolor;
                pdcolor.Text = sStyle.pdcolor;
                pdfontsize.ClearSelection();
                pdfontsize.Items.FindByValue(sStyle.pdfontsize).Selected = true;
                pdbold.ClearSelection();
                pdbold.Items.FindByValue(sStyle.pdbold ? "1" : "0").Selected = true;

                //question
                drpShowQuestionNO.ClearSelection();

                if (sStyle.ShowQuestionNo)
                    drpShowQuestionNO.Items.FindByValue("1").Selected = true;
                else
                    drpShowQuestionNO.Items.FindByValue("0").Selected = true;

                qcolor.Text = sStyle.qcolor;
                qfontsize.ClearSelection();
                qfontsize.Items.FindByValue(sStyle.qfontsize).Selected = true;
                qbold.ClearSelection();
                qbold.Items.FindByValue(sStyle.qbold ? "1" : "0").Selected = true;

                //answer
                acolor.Text = sStyle.acolor;
                afontsize.ClearSelection();
                afontsize.Items.FindByValue(sStyle.afontsize).Selected = true;
                abold.ClearSelection();
                abold.Items.FindByValue(sStyle.abold ? "1" : "0").Selected = true;

                //header Images.

                //footer Images

                plstyles.Text = ECN_Framework_BusinessLayer.Collector.SurveyStyles.RenderStyle(sStyle);

                if (plToobar.Visible)
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loadfunctions", "<script language='javascript'>onload=function(){loadfunctions();};</script>");

			}
		}

        private void LoadStylesFromTemplate(int TemplateID)
        {
            if (SurveyID > 0)
            {
                ECN_Framework_Entities.Collector.Templates sStyle = ECN_Framework_BusinessLayer.Collector.Templates.GetByTemplateID(TemplateID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
			

                //Page
                pbgcolor.Text = sStyle.pbgcolor;
                pborder.ClearSelection();
                pborder.Items.FindByValue(sStyle.pBorder ? "1" : "0").Selected = true;
                pBordercolor.Text = sStyle.pBordercolor;
                pfont.ClearSelection();
                pfont.Items.FindByValue(sStyle.pfontfamily).Selected = true;
                pWidth.ClearSelection();
                pWidth.Items.FindByValue(sStyle.pWidth).Selected = true;
                pAlign.ClearSelection();
                pAlign.Items.FindByValue(sStyle.pAlign).Selected = true;
                bbgcolor.Text = sStyle.bbgcolor;

                //Header
                hbgcolor.Text = sStyle.hbgcolor;
                halign.ClearSelection();
                halign.Items.FindByValue(sStyle.hAlign).Selected = true;
                char[] delimiter = { ' ' };
                string hMargin = sStyle.hMargin.Replace("px", "");
                string[] ahMargin = hMargin.Split(delimiter);

                string hleftmarginValue = ahMargin[0];
                string htopmarginValue = ahMargin[1];
                string hbottommarginValue = ahMargin[2];
                string hrightmarginValue = ahMargin[3];

                string fMargin = sStyle.fMargin.Replace("px", "");
                string[] afMargin = fMargin.Split(delimiter);

                string fleftmarginValue = afMargin[0];
                string ftopmarginValue = afMargin[1];
                string fbottommarginValue = afMargin[2];
                string frightmarginValue = afMargin[3];

                hleftmargin.Text = hleftmarginValue.ToString();
                hrightmargin.Text = hrightmarginValue.ToString();
                htopmargin.Text = htopmarginValue.ToString();
                hbottommargin.Text = hbottommarginValue.ToString();
                hImage.Text = sStyle.hImage;
                imgHeader.ImageUrl = sStyle.hImage;
                //footer
                fbgcolor.Text = sStyle.fbgcolor;
                falign.ClearSelection();
                falign.Items.FindByValue(sStyle.fAlign).Selected = true;
                fleftmargin.Text = fleftmarginValue.ToString();
                frightmargin.Text = frightmarginValue.ToString();
                ftopmargin.Text = ftopmarginValue.ToString();
                fbottommargin.Text = fbottommarginValue.ToString();
                fImage.Text = sStyle.fImage;
                imgFooter.ImageUrl = sStyle.fImage;
                //Page Header
                phbgcolor.Text = sStyle.phbgcolor;
                phcolor.Text = sStyle.phcolor;
                phfontsize.ClearSelection();
                phfontsize.Items.FindByValue(sStyle.phfontsize).Selected = true;
                phbold.ClearSelection();
                phbold.Items.FindByValue(sStyle.phBold ? "1" : "0").Selected = true;

                //Page Description
                pdbgcolor.Text = sStyle.pdbgcolor;
                pdcolor.Text = sStyle.pdcolor;
                pdfontsize.ClearSelection();
                pdfontsize.Items.FindByValue(sStyle.pdfontsize).Selected = true;
                pdbold.ClearSelection();
                pdbold.Items.FindByValue(sStyle.pdbold ? "1" : "0").Selected = true;

                //question
                drpShowQuestionNO.ClearSelection();

                if (sStyle.ShowQuestionNo)
                    drpShowQuestionNO.Items.FindByValue("1").Selected = true;
                else
                    drpShowQuestionNO.Items.FindByValue("0").Selected = true;

                qcolor.Text = sStyle.qcolor;
                qfontsize.ClearSelection();
                qfontsize.Items.FindByValue(sStyle.qfontsize).Selected = true;
                qbold.ClearSelection();
                qbold.Items.FindByValue(sStyle.qbold ? "1" : "0").Selected = true;

                //answer
                acolor.Text = sStyle.acolor;
                afontsize.ClearSelection();
                afontsize.Items.FindByValue(sStyle.afontsize).Selected = true;
                abold.ClearSelection();
                abold.Items.FindByValue(sStyle.abold ? "1" : "0").Selected = true;

                //header Images.

                //footer Images

                plstyles.Text = ECN_Framework_BusinessLayer.Collector.Templates.RenderStyle(sStyle);

                if (plToobar.Visible)
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loadfunctions", "<script language='javascript'>onload=function(){loadfunctions();};</script>");

            }
        }

		private void LoadTemplates() 
		{
            List<ECN_Framework_Entities.Collector.Templates> templateList = ECN_Framework_BusinessLayer.Collector.Templates.GetbyCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            dlTemplates.DataSource = templateList;
			dlTemplates.DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{

			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.btnTemplateSave.Click += new System.Web.UI.ImageClickEventHandler(this.btnTemplateSave_Click);

		}
		#endregion

		
		public void dlTemplates_itemcommand(object sender, DataListCommandEventArgs e)
		{
			string sqlquery = string.Empty;
			if (e.CommandName.ToLower() == "select")
                LoadStylesFromTemplate(Convert.ToInt32(e.CommandArgument.ToString()));

		}

		private void btnTemplateSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int TemplateID = 0;
            bool checkDups = ECN_Framework_BusinessLayer.Collector.Templates.Exists(txtTemplateName.Text, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            if (checkDups)
			{
				lblTemplateErrorMessage.Text="<u>ERROR:</u>Template name '<font color=#000000>" + txtTemplateName.Text + "'</font> already exists. Please enter a different name";
				lblTemplateErrorMessage.Visible=true;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "Templatepopup", "<script language='javascript'>onload=function(){loadfunctions();ShowSaveTemplate();};</script>");
				return;
			}

            ECN_Framework_Entities.Collector.Templates sStyle = new ECN_Framework_Entities.Collector.Templates();
            
			//Page
            sStyle.pbgcolor = pbgcolor.Text;
            sStyle.pBorder = pborder.SelectedItem.Value == "1" ? true : false;
            sStyle.pBordercolor = pBordercolor.Text;
            sStyle.pfontfamily = pfont.SelectedItem.Value;

            sStyle.pWidth = pWidth.SelectedItem.Value;
            sStyle.pAlign = pAlign.SelectedItem.Value;
            sStyle.bbgcolor = bbgcolor.Text;

			//Header
            sStyle.hbgcolor = hbgcolor.Text;
            sStyle.hAlign = halign.SelectedItem.Value;

            StringBuilder hMargin = new StringBuilder();
            hMargin.Append(hleftmargin.Text == string.Empty ? "0" : hleftmargin.Text + " px");
            hMargin.Append(htopmargin.Text == string.Empty ? "0" : htopmargin.Text + " px");
            hMargin.Append(hbottommargin.Text == string.Empty ? "0" : hbottommargin.Text + " px");
            hMargin.Append(hrightmargin.Text == string.Empty ? "0" : hrightmargin.Text + " px");
            sStyle.hMargin = hMargin.ToString();
            
            sStyle.hImage = hImage.Text;	
			//footer
            sStyle.fbgcolor = fbgcolor.Text;
            sStyle.fAlign = falign.SelectedItem.Value;


            StringBuilder fMargin = new StringBuilder();
            fMargin.Append(fleftmargin.Text == string.Empty ? "0" : fleftmargin.Text + " px");
            fMargin.Append(ftopmargin.Text == string.Empty ? "0" : ftopmargin.Text + " px");
            fMargin.Append(fbottommargin.Text == string.Empty ? "0" : fbottommargin.Text + " px");
            fMargin.Append(frightmargin.Text == string.Empty ? "0" : frightmargin.Text + " px");

            sStyle.fMargin = fMargin.ToString();
            sStyle.fImage = fImage.Text;
			//Page Header
            sStyle.phbgcolor = phbgcolor.Text;
            sStyle.phcolor = phcolor.Text;
            sStyle.phfontsize = phfontsize.SelectedItem.Value;
            sStyle.phBold = phbold.SelectedItem.Value == "1" ? true : false;
				
			//Page Description
            sStyle.pdbgcolor = pdbgcolor.Text;
            sStyle.pdcolor = pdcolor.Text;
            sStyle.pdfontsize = pdfontsize.SelectedItem.Value;
            sStyle.pdbold = pdbold.SelectedItem.Value == "1" ? true : false;

			//question
            sStyle.ShowQuestionNo = drpShowQuestionNO.SelectedItem.Value == "1" ? true : false;
            sStyle.qcolor = qcolor.Text;
            sStyle.qfontsize = qfontsize.SelectedItem.Value;
            sStyle.qbold = qbold.SelectedItem.Value == "1" ? true : false;

			//answer
            sStyle.acolor = acolor.Text;
            sStyle.afontsize = afontsize.SelectedItem.Value;
            sStyle.abold = abold.SelectedItem.Value == "1" ? true : false;
            sStyle.TemplateName = txtTemplateName.Text;
            sStyle.IsDefault = chkTDefault.Checked;
            sStyle.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            sStyle.TemplateImage = "/ecn.images/images//SurveyDefTemplate.jpg";
            ECN_Framework_BusinessLayer.Collector.Templates.Save(sStyle, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            TemplateID = sStyle.TemplateID;
			txtTemplateName.Text = "";
			lblTemplateErrorMessage.Text="";
			LoadTemplates();
			if (TemplateID > 0)
				LoadStylesFromTemplate(TemplateID);
		}

		protected void btnReset_Click(object sender, System.EventArgs e)
		{
			LoadStylesFromSurvey(SurveyID);
		}

	}
}
