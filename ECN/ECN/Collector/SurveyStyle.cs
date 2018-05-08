using System;
using System.Data;
using ecn.common.classes;

namespace ecn.collector.classes
{
	
	
	
	public class SurveyStyle
	{
		#region local variable

		//Page
		private int _surveyID;
		private string _pbgcolor =string.Empty;
		private bool _pborder =true;
		private string _pbordercolor =string.Empty;
		private string _pfontfamily =string.Empty;
		private string _palign =string.Empty;
		private string _pwidth =string.Empty;

		//header
		private string _hbgcolor =string.Empty;
		private string _hImage =string.Empty;
		private int _hleftmargin =5;
		private int _htopmargin =5;
		private int _hbottommargin =5;
		private int _hrightmargin =5;
		private string _halign =string.Empty;
		//footer
		private string _fbgcolor =string.Empty;
		private string _fImage =string.Empty;
		private int _fleftmargin =5;
		private int _ftopmargin =5;
		private int _fbottommargin =5;
		private int _frightmargin =5;
		private string _falign =string.Empty;
		//page header
		private string _phbgcolor =string.Empty;
		private string _phcolor =string.Empty;
		private string _phfontsize =string.Empty;
		private bool _phbold = true;
		//page description
		private string _pdbgcolor =string.Empty;
		private string _pdcolor =string.Empty;
		private string _pdfontsize =string.Empty;
		private bool _pdbold = true;
		//body
		private string _bbgcolor =string.Empty;
		//question
        private bool _showquestionno = true;
		private string _qcolor = string.Empty;
		private string _qfontsize = string.Empty;
		private bool _qbold = false;
		//answer
		private string _acolor = string.Empty;
		private string _afontsize = string.Empty;
		private bool _abold = false;

		#endregion

		#region public variables
		public int SurveyID
		{
			get {return (this._surveyID);}
			set {this._surveyID = value;}
		}
		public string Page_BGColor 
		{
			get {return (this._pbgcolor);}
			set {this._pbgcolor = value;}
		}
		public bool Page_Border
		{
			get {return (this._pborder);}
			set {this._pborder = value;}
		}
		public string Page_BorderColor
		{
			get {return (this._pbordercolor);}
			set {this._pbordercolor = value;}
		}
		public string Page_FontFamily
		{
			get {return (this._pfontfamily);}
			set {this._pfontfamily = value;}
		}
		public string Page_Align
		{
			get {return (this._palign);}
			set {this._palign = value;}
		}
		public string Page_Width
		{
			get {return (this._pwidth);}
			set {this._pwidth = value;}
		}
		public string Header_BGColor
		{
			get {return (this._hbgcolor);}
			set {this._hbgcolor = value;}
		}
		public string Header_Image
		{
			get {return (this._hImage);}
			set {this._hImage = value;}
		}

		public int Header_LeftMargin
		{
			get {return (this._hleftmargin);}
			set {this._hleftmargin = value;}
		}
		public int Header_TopMargin
		{
			get {return (this._htopmargin);}
			set {this._htopmargin = value;}
		}
		public int Header_BottomMargin
		{
			get {return (this._hbottommargin);}
			set {this._hbottommargin = value;}
		}
		public int  Header_RightMargin
		{
			get {return (this._hrightmargin);}
			set {this._hrightmargin = value;}
		}
		public string  Header_Align
		{
			get {return (this._halign);}
			set {this._halign = value;}
		}
		public string Footer_BGColor
		{
			get {return (this._fbgcolor);}
			set {this._fbgcolor = value;}
		}
		public string Footer_Image
		{
			get {return (this._fImage);}
			set {this._fImage = value;}
		}
		public int Footer_LeftMargin
		{
			get {return (this._fleftmargin);}
			set {this._fleftmargin = value;}
		}
		public int  Footer_TopMargin
		{
			get {return (this._ftopmargin);}
			set {this._ftopmargin = value;}
		}
		public int Footer_BottomMargin
		{
			get {return (this._fbottommargin);}
			set {this._fbottommargin = value;}
		}
		public int  Footer_RightMargin
		{
			get {return (this._frightmargin);}
			set {this._frightmargin = value;}
		}
		public string  Footer_Align
		{
			get {return (this._falign);}
			set {this._falign = value;}
		}
		public string PageHeader_BGColor
		{
			get {return (this._phbgcolor);}
			set {this._phbgcolor = value;}
		}
		public string  PageHeader_Color
		{
			get {return (this._phcolor);}
			set {this._phcolor = value;}
		}
		public string  PageHeader_FontSize
		{
			get {return (this._phfontsize);}
			set {this._phfontsize = value;}
		}
		public bool  PageHeader_Bold
		{
			get {return (this._phbold);}
			set {this._phbold = value;}
		}
		public string PageDesc_BGColor 
		{
			get {return (this._pdbgcolor);}
			set {this._pdbgcolor = value;}
		}
		public string  PageDesc_Color
		{
			get {return (this._pdcolor);}
			set {this._pdcolor = value;}
		}
		public string  PageDesc_FontSize
		{
			get {return (this._pdfontsize);}
			set {this._pdfontsize = value;}
		}
		public bool  PageDesc_Bold
		{
			get {return (this._pdbold);}
			set {this._pdbold = value;}
		}

		public string Body_BGColor
		{
			get {return (this._bbgcolor);}
			set {this._bbgcolor = value;}
		}

        public bool  ShowQuestionNo
		{
            get { return (this._showquestionno); }
            set { this._showquestionno = value; }
		}
        
		public string  Question_Color
		{
			get {return (this._qcolor);}
			set {this._qcolor = value;}
		}
		public string  Question_FontSize
		{
			get {return (this._qfontsize);}
			set {this._qfontsize = value;}
		}
		public bool Question_Bold
		{
			get {return (this._qbold);}
			set {this._qbold = value;}
		}

		public string  Answer_Color
		{
			get {return (this._acolor);}
			set {this._acolor = value;}
		}
		public string  Answer_FontSize
		{
			get {return (this._afontsize);}
			set {this._afontsize = value;}
		}
		public bool Answer_Bold 
		{
			get {return (this._abold);}
			set {this._abold = value;}
		}
		#endregion 

		public SurveyStyle()
		{
		}

		public SurveyStyle(string mode, int ID)
		{
			DataTable dt;
			
			if (mode.ToLower()=="template")
				dt = DataFunctions.GetDataTable("select * from templates where templateID = " + ID);
			else
			{
				dt = DataFunctions.GetDataTable("select * from SurveyStyles where SurveyID = " + ID);
				_surveyID = ID;
			}
			if (dt.Rows.Count > 0)
			{
				//Page
				this._palign=dt.Rows[0]["pAlign"].ToString();
				this._pbgcolor=dt.Rows[0]["pbgcolor"].ToString();
				this._pborder= dt.Rows[0].IsNull("pBorder")?false:Convert.ToBoolean(dt.Rows[0]["pBorder"].ToString());
				this._pbordercolor=dt.Rows[0]["pBordercolor"].ToString();
				this._pfontfamily=dt.Rows[0]["pfontfamily"].ToString();
				this._pwidth = dt.Rows[0]["pWidth"].ToString();
				//Header
				this._halign=dt.Rows[0]["hAlign"].ToString();
				this._hbgcolor=dt.Rows[0]["hbgcolor"].ToString();
				this._hImage=dt.Rows[0]["hImage"].ToString();

				char[] delimiter = {' '};
				string hMargin = dt.Rows[0]["hMargin"].ToString().Replace("px","");
				string[] ahMargin = hMargin.Split(delimiter);

				this._hleftmargin=Convert.ToInt32(ahMargin[0]);
				this._htopmargin=Convert.ToInt32(ahMargin[1]);
				this._hbottommargin=Convert.ToInt32(ahMargin[2]);
				this._hrightmargin=Convert.ToInt32(ahMargin[3]);

				//Page Header
				this._phbgcolor = dt.Rows[0]["phbgcolor"].ToString();
				this._phbold = dt.Rows[0].IsNull("phBold")?false:Convert.ToBoolean(dt.Rows[0]["phBold"].ToString());
				this._phcolor = dt.Rows[0]["phcolor"].ToString();
				this._phfontsize = dt.Rows[0]["phfontsize"].ToString();
				//Page Description
				this._pdbgcolor = dt.Rows[0]["pdbgcolor"].ToString();
				this._pdbold = dt.Rows[0].IsNull("pdBold")?false:Convert.ToBoolean(dt.Rows[0]["pdBold"].ToString());
				this._pdcolor = dt.Rows[0]["pdcolor"].ToString();
				this._pdfontsize = dt.Rows[0]["pdfontsize"].ToString();
				//body
				this._bbgcolor = dt.Rows[0]["bbgcolor"].ToString();
				//question
                this._showquestionno = dt.Rows[0].IsNull("ShowQuestionNo") ? true : Convert.ToBoolean(dt.Rows[0]["ShowQuestionNo"].ToString());
				this._qbold = dt.Rows[0].IsNull("qBold")?false:Convert.ToBoolean(dt.Rows[0]["qBold"].ToString());
				this._qcolor = dt.Rows[0]["qcolor"].ToString();
				this._qfontsize = dt.Rows[0]["qfontsize"].ToString();
				//answer
				this._abold = dt.Rows[0].IsNull("aBold")?false:Convert.ToBoolean(dt.Rows[0]["aBold"].ToString());
				this._acolor = dt.Rows[0]["acolor"].ToString();
				this._afontsize = dt.Rows[0]["afontsize"].ToString();
				//footer
				this._falign = dt.Rows[0]["fAlign"].ToString();
				this._fbgcolor = dt.Rows[0]["fbgcolor"].ToString();
				this._fImage = dt.Rows[0]["fImage"].ToString();
				
				string fMargin = dt.Rows[0]["fMargin"].ToString().Replace("px","");
				string[] afMargin = fMargin.Split(delimiter);

				this._fleftmargin=Convert.ToInt32(afMargin[0]);
				this._ftopmargin=Convert.ToInt32(afMargin[1]);
				this._fbottommargin=Convert.ToInt32(afMargin[2]);
				this._frightmargin=Convert.ToInt32(afMargin[3]);
			
			}
		}
		

		public string RenderStyle()
		{
			return	"<style>.surveybody {height:100%;background-color:" + this._pbgcolor + "; FONT-FAMILY:" + this._pfontfamily + ";text-align:" + this._palign + "} " +
				" .outertable {background-color:" + this._bbgcolor + ";" + (this._pborder?";BORDER:" + this._pbordercolor + " 1px solid;":"") +" MARGIN:0px " + (this._palign.ToLower()=="left"?"":" auto")+ "; OVERFLOW:hidden; WIDTH:" + this._pwidth + "; text-align:left;}" +
				" .divHeader { background-color:" + this._hbgcolor + "; TEXT-ALIGN:" + this._halign + ";}" +
				" .divHeaderIMG { MARGIN-left:" + this._hleftmargin + "px;MARGIN-top:" + this._htopmargin + "px;MARGIN-bottom:" + this._hbottommargin + "px;MARGIN-right:" + this._hrightmargin + "px; }" +
				" .divpageHeader { PADDING-left:15px;PADDING-right:5px;PADDING-top:8px;PADDING-bottom:8px; FONT-WEIGHT: " + (this._qbold?"bold":"normal") + "; FONT-SIZE: " + this._phfontsize + "; background-color: " + this._phbgcolor + "; COLOR: " + this._phcolor+ ";}" +
				" .divpageDesc { PADDING-LEFT:20px;PADDING-right:20px;PADDING-top:6px;PADDING-bottom:6px; FONT-WEIGHT: " + (this._pdbold?"bold":"normal") + "; FONT-SIZE: " + this._pdfontsize + "; background-color: " + this._pdbgcolor + "; COLOR: " + this._pdcolor + ";}" +
				" .surveytable { PADDING:20px 20px 20px 20px;}" +
                " .questionno {visibility:"  + (this._showquestionno?"visible":"hidden")+ "}"+
				" .question { PADDING:10px 10px 5px 20px;FONT-WEIGHT:" + (this._qbold?"bold":"normal") + "; FONT-SIZE:" + this._qfontsize + "; COLOR:" + this._qcolor + "}" +
				" .vstyle { PADDING:10px 10px 5px 20px;FONT-WEIGHT:" + (this._qbold?"bold":"normal") + "; FONT-SIZE:" + this._qfontsize + "; COLOR:red; background-color:#ffffff;}" +
				" .answer { PADDING:5px 5px 5px 40px; FONT-WEIGHT:" + (this._abold?"bold":"normal") + "; FONT-SIZE:" + this._afontsize + "; COLOR:" + this._acolor + "}" +
				" .answer select {FONT-WEIGHT:" + (this._abold?"bold":"normal") + "; FONT-SIZE:" + this._afontsize + "; COLOR:" + this._acolor + "}" +
				" .gridColumn { text-align:center; FONT-WEIGHT:" + (this._abold?"bold":"normal") + "; FONT-SIZE:" + this._afontsize + "; COLOR:" + this._acolor + "}" +
				" .gridRow { text-align:left; FONT-WEIGHT:" + (this._abold?"bold":"normal") + "; FONT-SIZE:" + this._afontsize + "; COLOR:" + this._acolor + "}" +
				" .divFooter { background-color:" + this._fbgcolor + ";text-align:" + this._falign+ ";}" +
				" .tblSurveyGrid {background-color:white;} " + 
				" .tblSurveyGrid td {border:1px #cccccc solid } " + 
				" .progresslabel {font-size:10px;	font-family:Arial, Helvetica, sans-serif;	color:#000000;	font-weight:normal;} " + 
				" .divFooterIMG { MARGIN-left:" + this._fleftmargin + "px;MARGIN-top:" + this._ftopmargin + "px;MARGIN-bottom:" + this._fbottommargin + "px;MARGIN-right:" + this._frightmargin + "px;}</style>";
		}


		public void Save()
		{
			if (_surveyID > 0)
			{
				String sqlquery = "update SurveyStyles set " + 
					" pAlign = '" + this._palign + "'," +
					" pbgcolor = '" + this._pbgcolor + "'," +
					" pBorder = '" + (this._pborder?"1":"0") + "'," +
					" pBordercolor = '" + this._pbordercolor + "'," +
					" pfontfamily = '" + this._pfontfamily + "'," +
					" pWidth = '" + this._pwidth + "'," +
					" hImage = '" + this._hImage + "'," +
					" hAlign = '" + this._halign + "'," +
					" hbgcolor = '" + this._hbgcolor + "'," +
					" hMargin = '" + _hleftmargin + " px" +  _htopmargin  + " px" + _hbottommargin + " px" + _hrightmargin + " px'," +
					" phbgcolor = '" + this._phbgcolor + "'," +
					" phBold = '" + (this._phbold?"1":"0") + "'," +
					" phcolor = '" + this._phcolor + "'," +
					" phfontsize = '" + this._phfontsize + "'," +
					" pdbgcolor = '" + this._pdbgcolor + "'," +
					" pdBold = '" + (this._pdbold?"1":"0") + "'," +
					" pdcolor = '" + this._pdcolor + "'," +
					" pdfontsize = '" + this._pdfontsize + "'," +
					" bbgcolor = '" + this._bbgcolor + "'," +
                    " ShowQuestionNo = '" + (this._showquestionno ? "1" : "0") + "'," +
					" qBold = '" + (this._qbold?"1":"0") + "'," +
					" qcolor = '" + this._qcolor + "'," +
					" qfontsize = '" + this._qfontsize + "'," +
					" aBold = '" + (this._abold?"1":"0") + "'," +
					" acolor = '" + this._acolor + "'," +
					" afontsize = '" + this._afontsize + "'," +
					" fImage = '" + this._fImage + "'," +
					" fAlign = '" + this._falign + "'," +
					" fbgcolor = '" + this._fbgcolor + "'," +
					" fMargin = '" + _fleftmargin + " px" +  _ftopmargin  + " px" + _fbottommargin + " px" + _frightmargin + " px'" +
					" where SurveyID = " + SurveyID;

				DataFunctions.Execute(sqlquery);
			}
		}

		public int AddToTemplate(int CustomerID, string TemplateName, bool SetToDefault)
		{
			int TemplateID = 0;
			String sqlquery = string.Empty;

			if (SetToDefault)
				sqlquery = "update templates set IsDefault=0 where customerID = " + CustomerID + ";";

			sqlquery += "Insert into Templates (CustomerID,TemplateName,TemplateImage,IsDefault," +
				"pbgcolor,pAlign,pBorder,pBordercolor,pfontfamily,pWidth," +
				"hImage,hAlign,hMargin,hbgcolor," +
				"phbgcolor,phfontsize,phcolor,phBold," +
				"pdbgcolor,pdfontsize,pdcolor,pdbold," +
				"bbgcolor," +
				"qcolor,qfontsize,qbold," +
				"acolor,afontsize,abold," +
				"fImage,fAlign,fMargin,fbgcolor," +
                "IsActive, ShowQuestionNo) values (" + CustomerID + ",'" + TemplateName.Replace("'", "''") + "', '/ecn.images/images//SurveyDefTemplate.jpg'," + (SetToDefault ? 1 : 0) + "," + 
				"'" + this._pbgcolor + "','" + this._palign + "','" + (this._pborder?"1":"0") + "','" + this._pbordercolor + "','" + this._pfontfamily + "','" + this._pwidth + "'," + 
				"'" + this._hImage + "','" + this._halign + "','" + _hleftmargin + " px" +  _htopmargin  + " px" + _hbottommargin + " px" + _hrightmargin + " px','" + this._hbgcolor + "'," +
				"'" + this._phbgcolor + "','" + this._phfontsize + "','" + this._phcolor + "','" + (this._phbold?"1":"0") +  "'," + 
				"'" + this._pdbgcolor + "','" + this._pdfontsize + "','" + this._pdcolor + "','" + (this._pdbold?"1":"0") +  "'," + 
				"'" + this._bbgcolor + "'," + 
				"'" + this._qcolor + "','" + this._qfontsize + "','" + (this._qbold?"1":"0") +  "'," + 
				"'" + this._acolor + "','" + this._afontsize + "','" + (this._abold?"1":"0") +  "'," + 
				"'" + this._fImage + "','" + this._falign + "','" + + _fleftmargin + " px" +  _ftopmargin  + " px" + _fbottommargin + " px" + _frightmargin + " px','" + this._fbgcolor +  "'," + 
				"1,  " + (this.ShowQuestionNo ? "1" : "0") + ");select @@IDENTITY"; 

			TemplateID = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));
			return TemplateID;
		}
	}
}
