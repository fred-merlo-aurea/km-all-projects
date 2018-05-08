// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software 
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI; 
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Win32;
using System.Web;
using System.Globalization; 
using System.IO;
using System.Reflection;
using System.Data; 
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents the locale date format.
	/// </summary>
	public enum DateFormatLocale
	{
		/// <summary>
		/// German locale format.
		/// </summary>
		de = 0,
		/// <summary>
		/// English locale format.
		/// </summary>
		en, 
		/// <summary>
		/// English (Great Britain) locale format.
		/// </summary>
		en_GB,
		/// <summary>
		/// French locale format.
		/// </summary>
		fr,
		/// <summary>
		/// Italian locale format.
		/// </summary>
		it,
		/// <summary>
		/// Spanish locale format.
		/// </summary>
		es,
		/// <summary>
		/// Portuguese locale format.
		/// </summary>
		pt
	}

	/// <summary>
	/// Active Calendar is an ASP.NET server control that allows users to select a date and/or time quickly using a professional looking date picker with a client-side managed event system. No postback to the server required, no external script file required.
	/// </summary>
	[
	ToolboxData("<{0}:Calendar runat=server></{0}:Calendar>"),
	Designer(typeof(ActiveUp.WebControls.Design.CalendarControlDesigner)),
    Editor(typeof(ActiveUp.WebControls.Design.CalendarComponentEditor), typeof(ComponentEditor)),
    ToolboxBitmap(typeof(Calendar), "ToolBoxBitmap.Calendar.bmp"),
	ParseChildren(true)
	]
	public class Calendar : CoreWebControl, IPostBackEventHandler, IPostBackDataHandler
	{
		private static readonly object EventClick = new Object();
		private static readonly object EventChange = new Object();

		//private const string ActiveCalendarScriptKey = "ActiveCalendarIncludeScript";
		private string _nextMonthText, _nextMonthImage,  _prevMonthText, _prevMonthImage, _nextYearText, _nextYearImage, _prevYearText, _prevYearImage, _format, 
			_externalScript,_iconPath, _iconToolTip, _pickupDateValue, _scriptDirectory;
		private bool _autoAdjust, _monthNamesDisabled, _showDayHeader, _showNextPrevMonth,
			_showNextPrevYear, _useSelectors, _useTime, _showTodayFooter, _multiSelection;
		private int _cellPadding, _cellSpacing, _renderLevel = 2,_weekNumberFactor = 0;
		private DateTime _minDate, _maxDate;
		private System.Web.UI.WebControls.Style _nextPrevStyle, _selectorStyle;
		private CalendarDayStyleFull _dayHeaderStyle, _dayStyle, _weekEndDayStyle, _weekNumberStyle, _todayFooterStyle,_pickerTextBoxStyle,_otherMonthDayStyle,_toolTipStyle;
		private CalendarDayStyleShort _todayDayStyle, _selectedDayStyle, _blockedDayStyle, 
			_titleStyle, _timeStyle, _dayOverStyle;
		private string _prefixTodayFooter;
		private string[] _months, _days;
		private DateTime _oldDate;
		private System.Web.UI.WebControls.FirstDayOfWeek _firstDayOfWeek;
		private bool _navigationChangeDate = true, _showWeekNumber, _useDatePicker;
		private DayNameFormat _dayNameFormat = DayNameFormat.Short;
		private DateCollection _blockedDates = new DateCollection();
		private DateCollection _selectedDates = new DateCollection();
		private DateStyleCollection _styleDates = new DateStyleCollection();
		private DateToolTipCollection _toolTipCustomDates = new DateToolTipCollection();
		private DateFormatLocale _dateFormatLocale = DateFormatLocale.en;
		private bool _showDateOnStart = true;
		private string _weekNumberText = "Week";
		private string CLIENTSIDE_API = null; 
		private const string SCRIPTKEY = "ActiveCalendar";
		private bool _autoPostBack = false;
		private bool _useCustomDateFormat = false;
		private string _customDateFormat = "dd/mm/yyyy";
		private bool _showMonth = true, _showYear = true;
		private bool _showHeaderNavigation = true;
		private bool _use24HourFormat = true, _allowNull = true;
		private string _localizationFile = string.Empty;
		private LocalizationSettings _labels;
		private string _toolTipBlocked = string.Empty, _toolTipWeekend = string.Empty, _toolTipNormal = string.Empty, _toolTipToday = string.Empty, _toolTipSelected = string.Empty, _toolTipOther = string.Empty;

	    private const string PreviousMonthValue = "-1";
	    private const string PreviousYearValue = "-12";
	    private const string NextMonthValue = "1";
	    private const string NextYearValue = "12";
	    private const string Alignment_Right = "right";
	    private const string Alignment_Left = "left";
	    private const string BorderAttribute_0 = "0";
	    private const string HtmlSpace = "&nbsp;";

#if (LICENSE)
        //private string _license;

        /// <summary>
        /// Used for the license counter.
        /// </summary>
        private static int _useCounter;

#endif

		// custom javascript event
		private string _onDayClicked;
		private string _onBlurPicker;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public Calendar()
		{

			// Define default values
			_months = new string[] {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
			_days = new string[] {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
			_minDate = new DateTime(DateTime.Now.AddYears(-50).Year,1,1); 
			_maxDate = new DateTime(DateTime.Now.AddYears(3).Year,12,31);
			_format = "MONTH;/;DAY;/;YEAR;-;HOUR;:;MINUTE;";
			BorderColor = System.Drawing.Color.Black;
			BorderWidth = 1;
			_dayStyle = new CalendarDayStyleFull();
			_dayStyle.ForeColor = System.Drawing.Color.Black;
			_dayStyle.BackColor = System.Drawing.Color.White;
			_dayStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_nextPrevStyle = new System.Web.UI.WebControls.Style();
			_nextPrevStyle.ForeColor = System.Drawing.Color.DarkRed;
			_nextPrevStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_otherMonthDayStyle = new CalendarDayStyleFull();
			_otherMonthDayStyle.ForeColor = System.Drawing.Color.Silver;
			_otherMonthDayStyle.BackColor = System.Drawing.Color.White;
			_otherMonthDayStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_selectedDayStyle = new CalendarDayStyleShort();
			//_selectedDayStyle = new System.Web.UI.WebControls.Style();
			_selectedDayStyle.BackColor = System.Drawing.Color.Teal;
			_selectedDayStyle.ForeColor = System.Drawing.Color.White;
			_blockedDayStyle = new CalendarDayStyleShort();
			_blockedDayStyle.BackColor = System.Drawing.Color.White;
			_blockedDayStyle.ForeColor = System.Drawing.Color.Black;
			_selectorStyle = new System.Web.UI.WebControls.Style();
			_selectorStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_todayDayStyle = new CalendarDayStyleShort();
			_dayOverStyle = new CalendarDayStyleShort();
			_toolTipStyle = new CalendarDayStyleFull();
			_toolTipStyle.BackColor = Color.FromArgb(0xFF, 0xFF, 0xC0);
			_toolTipStyle.BorderColor = Color.FromArgb(0xE0, 0xE0, 0xE0);
			_toolTipStyle.BorderWidth = Unit.Parse("1px");
			_toolTipStyle.Font.Size = FontUnit.XSmall;
			_todayDayStyle.BackColor = System.Drawing.Color.DarkRed;
			_todayDayStyle.ForeColor = System.Drawing.Color.White;
			_weekEndDayStyle = new CalendarDayStyleFull();
			_weekEndDayStyle.BackColor = System.Drawing.Color.LightGray;
			_weekEndDayStyle.ForeColor = System.Drawing.Color.Black;
			_weekEndDayStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_weekNumberStyle = new CalendarDayStyleFull();
			_weekNumberStyle.ForeColor = System.Drawing.Color.Black;
			_weekNumberStyle.BackColor = System.Drawing.Color.White;
			_weekNumberStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_dayHeaderStyle = new CalendarDayStyleFull();
			_dayHeaderStyle.BackColor = System.Drawing.Color.Black;
			_dayHeaderStyle.ForeColor = System.Drawing.Color.White;
			_dayHeaderStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_pickerTextBoxStyle = new CalendarDayStyleFull();
			_firstDayOfWeek = System.Web.UI.WebControls.FirstDayOfWeek.Default;
			_cellPadding = 1;
			_cellSpacing = 0;
			_nextMonthText = "&gt;";
			_prevMonthText = "&lt;";
			_nextYearText = "&gt;&gt;";
			_prevYearText = "&lt;&lt;";
			_showNextPrevMonth = true;
			_showNextPrevYear = true;
			_showDayHeader = true;
			_showTodayFooter = false;
			_navigationChangeDate = true;
			_externalScript = string.Empty;
			_useDatePicker = false;
			_iconPath = string.Empty;
			_iconToolTip = string.Empty;
			_pickupDateValue = string.Empty;
			_titleStyle = new CalendarDayStyleShort();
			_timeStyle = new CalendarDayStyleShort();
			_todayFooterStyle = new CalendarDayStyleFull();
			_todayFooterStyle.ForeColor = System.Drawing.Color.Black;
			_todayFooterStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XSmall;
			_nextMonthImage = "";
			_prevMonthImage = "";
			_nextYearImage = "";
			_prevYearImage = "";
			_prefixTodayFooter = "";
			_multiSelection = false;
			//_license = "";
#if (!FX1_1)
            _scriptDirectory = string.Empty;
#else
			_scriptDirectory = Define.SCRIPT_DIRECTORY;
#endif

			/*#if (DEBUG)
			
			#else
						DateTime evalEnd = new DateTime(2003, 12, 30);
						if (DateTime.Now > evalEnd)
							throw new Exception("Trial period expired. Please register: http://www.activeup.com.");
		
			#endif*/
						
		}

		/// <summary>
		/// Register the client side script block in the ASP page.
		/// </summary>
		protected void RegisterScriptBlock() 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveCalendar.js"));
#else
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveCalendar.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterHiddenField("Containers","");

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
#endif					
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveCalendar.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
				
			}

			
			//Page.RegisterClientScriptBlock(ACTIVETOOLBARSCRIPTKEY, includeScript);	
			
			if (_renderLevel > 1)
			{
				// Create client script block.
				string startupString = "<script language='javascript'>\n";
				startupString += "// Variable declaration related to the control '" + ClientID + "'\n";
				startupString += ClientID + "_dayStyle='" + Utils.CreateStyleVariable(_dayStyle.BackColor,_dayStyle.BackgroundImage, _dayStyle.ForeColor,ImagesDirectory,_dayStyle.CssClass) + "';\n";	
				startupString += ClientID + "_otherMonthDayStyle='" + Utils.CreateStyleVariable(_otherMonthDayStyle.BackColor,_otherMonthDayStyle.BackgroundImage, _otherMonthDayStyle.ForeColor,ImagesDirectory) + "';\n";
				startupString += ClientID + "_selectedDayStyle='" + Utils.CreateStyleVariable(_selectedDayStyle.BackColor,_selectedDayStyle.BackgroundImage, _selectedDayStyle.ForeColor,ImagesDirectory) + "';\n";
				startupString += ClientID + "_blockedDayStyle='" + Utils.CreateStyleVariable(_blockedDayStyle.BackColor,_blockedDayStyle.BackgroundImage, _blockedDayStyle.ForeColor,ImagesDirectory) + "';\n";
				startupString += ClientID + "_weekEndDayStyle='" + Utils.CreateStyleVariable(_weekEndDayStyle.BackColor,_weekEndDayStyle.BackgroundImage, _weekEndDayStyle.ForeColor,ImagesDirectory,_weekEndDayStyle.CssClass) + "';\n";
				startupString += ClientID + "_todayDayStyle='" + Utils.CreateStyleVariable(_todayDayStyle.BackColor,_todayDayStyle.BackgroundImage, _todayDayStyle.ForeColor,ImagesDirectory) + "';\n";
				startupString += ClientID + "_dayOverStyle='" + Utils.CreateStyleVariable(_dayOverStyle.BackColor, _dayOverStyle.BackgroundImage, _dayOverStyle.ForeColor, ImagesDirectory) + "';\n";  
				startupString += ClientID + "_navigationChangeDate='" + _navigationChangeDate + "';\n";
				startupString += ClientID + "_showWeekNumber='" + _showWeekNumber + "';\n";
				startupString += ClientID + "_blockedDates='" + _blockedDates.GetItemListToString(",") + "';\n";
				startupString += ClientID + "_styleDates='" + _styleDates.GetItemListToString(",",ImagesDirectory) + "';\n";
				startupString += ClientID + "_useDatePicker='" + _useDatePicker + "';\n";	
				startupString += ClientID + "_useTime='" + _useTime + "';\n";
				startupString += ClientID + "_dateFormatLocale='" + _dateFormatLocale + "';\n";	
				startupString += ClientID + "_useCustomDateFormat='" + _useCustomDateFormat + "';\n";
				if (_useCustomDateFormat == true)
					startupString += ClientID + "_customDateFormat='" + _customDateFormat.ToLower() + "'\n";
				startupString += ClientID + "_multiSelection='" + _multiSelection + "';\n";	
				string firstDay = _firstDayOfWeek == System.Web.UI.WebControls.FirstDayOfWeek.Default ? "0" : ((int)_firstDayOfWeek).ToString();
				startupString += ClientID + "_firstDayOfWeek=" + firstDay + ";\n";
				//startupString += UniqueID + "_minDate='" + string.Format("{0}/{1}/{2}",_minDate.Year,_minDate.Month,_minDate.Day) + "';\n";
				//startupString += UniqueID + "_maxDate='" + string.Format("{0}/{1}/{2}",_maxDate.Year,_maxDate.Month,_maxDate.Day) + "';\n";
				startupString += ClientID + "_onClickClientSide=\"" + this.OnDayClicked + "\";\n";
				startupString += ClientID + "_onBlurPickerClientSide=\"" + this.OnBlurPicker + "\";\n";
				startupString += ClientID + "_weekNumberFactor=\"" + this.WeekNumberFactor + "\";\n";
				startupString += ClientID + "_showDayHeader='" + this.ShowDayHeader + "';\n";
				startupString += ClientID + "_showMonth='" + this.ShowMonth + "';\n";
				startupString += ClientID + "_showYear='" + this.ShowYear + "';\n";
				startupString += ClientID + "_toolTipTextBlocked='" + ToolTipBlocked + "';\n";
				startupString += ClientID + "_toolTipTextSelected='" + ToolTipSelected + "';\n";
				startupString += ClientID + "_toolTipTextToday='" + ToolTipToday + "';\n";
				startupString += ClientID + "_toolTipTextOther='" + ToolTipOther + "';\n";
				startupString += ClientID + "_toolTipTextWeekend='" + ToolTipWeekend + "';\n";
				startupString += ClientID + "_toolTipTextNormal='" + ToolTipNormal + "';\n";
				startupString += "try {\n";
				startupString += "ACL_RenderCalendar('" + ClientID + "');\n";
				startupString += "ACL_SetCalendarAbsolute('" + ClientID + "');\n"; 
								
				if (_useDatePicker) 
				{	
					startupString += "ACL_SetZIndex('" + ClientID + "',0);\n";
					startupString += "ACL_SetAsFirstPlan('" + ClientID + "',0);\n";
				}
				startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";

				startupString += "</script>\n";

				if (this.SelectedDate != DateTime.MinValue && _selectedDates.IsDatePresent(this.SelectedDate) == false)
					_selectedDates.Add(new DateCollectionItem(this.SelectedDate));

				// Render the startup script
				Page.RegisterStartupScript(ClientID + "_startup", startupString);

				Page.RegisterArrayDeclaration(ClientID + "_days",Utils.ConvertStringArrayToRegisterArray(_days));
				Page.RegisterArrayDeclaration(ClientID + "_months",Utils.ConvertStringArrayToRegisterArray(_months));

				// Register the array of blocked date
				/*string[] blockedDate = new string[_blockedDates.Count];

				for(int i = 0 ; i < _blockedDates.Count ; i++)
					blockedDate[i] = _blockedDates[i].Date.ToShortDateString();*/

				//Page.RegisterArrayDeclaration(this.UniqueID + "_blockedDate","'" + String.Join("','",blockedDate) + "'");

			}
		}                  

		/// <summary>
		/// Raises the PreRender event.
		/// </summary>
		/// <remarks>This method notifies the server control to perform any necessary prerendering steps prior to saving view state and rendering content.</remarks>
		/// <param name="e">An <see cref="EventArgs"/> object that contains the event data. </param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);
		
			// Determine if you can or want to use the client side validation.
			_renderLevel = DetermineRenderLevel();

			if (_renderLevel > 0) 
			{
				if (this.AutoDetectSsl && this.Page.Request.ServerVariables["HTTPS"].ToUpper() == "ON")
					this.EnableSsl = true;

				// Register the client side validation script.
				RegisterScriptBlock();
			}
		}

		/// <summary>
		/// Determine if we need to register the client side script and render the calendar, selectors with validation or selectors only.
		/// </summary>
		/// <returns>0 if scripting not allowed, 1 if not an uplevel browser but scripting allowed, 2 if all is OK.</returns>
		protected virtual int DetermineRenderLevel() 
		{
			// Must be on a page.
			// Check whether the client browser has turned off scripting and check
			// browser capabilities. Active Calendar needs the W3C DOM level 1 for
			// control manipulation, Internet Explorer 4+ and at least ECMAScript 1.2.
			Page page = Page;
			
			// If not in a Page, Client Script disabled or EcamScript version is not 1.2 or greater,
			// then no client script will be used.
			if (page == null || page.Request == null || !EnableClientScript ||
				!page.Request.Browser.JavaScript ||
				!(page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0)) 
				return 0;

			// Check the browser compatibility.
			System.Web.HttpBrowserCapabilities browser = page.Request.Browser; 

			// Check if the browser is compatible with dynamic InnerHTML writing.
            return 2;

            /*if (((browser.Browser.ToUpper().IndexOf("IE") > -1 && browser.MajorVersion >= 4)
                || (browser.Browser.ToUpper().IndexOf("FIREFOX") > -1)
				|| (browser.Browser.ToUpper().IndexOf("NETSCAPE") > -1 && browser.MajorVersion >= 5))
				&& !_useSelectors)
				return 2;
			else if (browser.Browser.ToUpper().IndexOf("OPERA") > -1 && browser.MajorVersion >= 3
				|| _useSelectors)
				return 1;
            */
          

			//return 0;
		}

		/// <summary>
		/// Returns the number of days in the specified month of the specified year (leap year check).
		/// </summary>
		/// <param name="year">The year</param>
		/// <param name="month">The month</param>
		/// <returns>The number of days</returns>
		public int GetDays(int year, int month)
		{
			if (month == 2)
				return (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) ? 29 : 28;
			else
				return (month == 4 || month == 6 || month == 9 || month == 11) ? 30 : 31;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e) 
		{
			base.OnInit(e);

			if (LocalizationFile != string.Empty)				
				LoadLocalizationSettings(LocalizationFile);
		}

		/// <summary>
		/// Updates the language.
		/// </summary>
		public void UpdateLanguage()
		{
			LoadLocalizationSettings(LocalizationFile);
		}

		/// <summary>
		/// Loads the localization settings.
		/// </summary>
		/// <param name="path">The path.</param>
		private void LoadLocalizationSettings(string path)
		{
			if (path != string.Empty)
			{
				if (!System.IO.Path.IsPathRooted(path))
					path = Page.Server.MapPath(path);

				this.Labels = LocalizationHelper.Load(path);

				if (this.Labels != null && this.Labels.Texts != null && this.Labels.Texts.Count > 0)
				{
					_months = new string[12];
					
					for (int i = 0 ; i < 12 ; i++)
					{
						if (Labels.Texts[i] != null)
							_months[i] = Labels.Texts[i].Value;
						else
							_months[i] = new string('_',3);
					}
					
					_days = new string[7];
					
					for (int i = 0, j = 12; i < 7 ; i++,j++)
					{
						if (Labels.Texts[i] != null)
							_days[i] = Labels.Texts[j].Value;
						else
							_days[i] = new string('_',3);
					}

				}

			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private LocalizationSettings Labels
		{
			get
			{
				if (_labels == null)
					_labels = new LocalizationSettings();
				return _labels;
			}
			set
			{
				_labels = value;
			}
		}

	    private CalendarNextPreviousValues GetNextPreviousValues(CalendarNextPreviousType monthYearType)
	    {
	        var nextPreviousValues = new CalendarNextPreviousValues();

            switch (monthYearType)
	        {
	            case CalendarNextPreviousType.PreviousMonth:
	                nextPreviousValues.ChangeMonthValue = PreviousMonthValue;
	                nextPreviousValues.ImagePath = _prevMonthImage;
	                nextPreviousValues.Alignment = Alignment_Left;
	                nextPreviousValues.CalendarText = _prevMonthText;
	                break;
	            case CalendarNextPreviousType.PreviousYear:
	                nextPreviousValues.ChangeMonthValue = PreviousYearValue;
	                nextPreviousValues.ImagePath = _prevYearImage;
	                nextPreviousValues.Alignment = Alignment_Left;
                    nextPreviousValues.CalendarText = _prevYearText;
	                break;
	            case CalendarNextPreviousType.NextYear:
	                nextPreviousValues.ChangeMonthValue = NextYearValue;
	                nextPreviousValues.ImagePath = _nextYearImage;
	                nextPreviousValues.Alignment = Alignment_Right;
                    nextPreviousValues.CalendarText = _nextYearText;
	                break;
	            case CalendarNextPreviousType.NextMonth:
	                nextPreviousValues.ChangeMonthValue = NextMonthValue;
	                nextPreviousValues.ImagePath = _nextMonthImage;
	                nextPreviousValues.Alignment = Alignment_Left;
                    nextPreviousValues.CalendarText = _nextMonthText;
	                break;
	        }

	        return nextPreviousValues;
	    }

        private void ShowNextPreviousMonthYear(CalendarNextPreviousType monthYearType, ref HtmlTextWriter output)
	    {
	        var nextPreviousValues = GetNextPreviousValues(monthYearType);

            _nextPrevStyle.AddAttributesToRender(output);
	        output.AddAttribute(HtmlTextWriterAttribute.Align, nextPreviousValues.Alignment);
	        output.RenderBeginTag(HtmlTextWriterTag.Td);
	        if (monthYearType == CalendarNextPreviousType.NextMonth)
	        {
	            output.Write(HtmlSpace);
	        }

	        _nextPrevStyle.AddAttributesToRender(output);
	        output.AddAttribute(HtmlTextWriterAttribute.Href,
	            "javascript:ACL_ChangeMonth('" + ClientID + "', " + nextPreviousValues.ChangeMonthValue + ");");
	        output.RenderBeginTag(HtmlTextWriterTag.A);

            if (string.IsNullOrEmpty(nextPreviousValues.ImagePath.Trim()))
	        {
	            output.Write(nextPreviousValues.CalendarText);
	        }
	        else
	        {
	            output.AddAttribute(HtmlTextWriterAttribute.Border, BorderAttribute_0);
	            output.AddAttribute(HtmlTextWriterAttribute.Src, 
	                Utils.ConvertToImageDir(ImagesDirectory, nextPreviousValues.ImagePath));
	            output.RenderBeginTag(HtmlTextWriterTag.Img);
	            output.RenderEndTag();
	        }

	        output.RenderEndTag();
	        if (monthYearType == CalendarNextPreviousType.PreviousMonth
                || monthYearType == CalendarNextPreviousType.PreviousYear)
	        {
	            output.Write(HtmlSpace);
	        }
	        output.RenderEndTag();
	    }

        private void RenderCalendar(HtmlTextWriter output/*, LicenseStatus licenseStatus*/)
		{
			/*output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Name,this.UniqueID + "_dayClicked");
				output.AddAttribute(HtmlTextWriterAttribute.Value,this.Page.GetPostBackEventReference(this, ""));
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();*/
			//base.Render(output);
			//Page page = Page;
			//System.Web.HttpBrowserCapabilities browser = page.Request.Browser;
			//output.Write(browser.W3CDomVersion.Major.ToString() + "-" + browser.EcmaScriptVersion.ToString() + "-" + browser.JavaScript.ToString());

			// Check if the control is registered
			//CheckRegistration();
			
			//if(_license.Product.ToUpper().StartsWith("ACL1"))
			//	output.Write("<!-- Registered to " + _license.Name + " -->");
			//else
			//	output.Write("<!-- Unregistered version -->");

			//output.Write(CheckLicense());

			// A workaround before finding a solution
			output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name,UniqueID);
            output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			output.AddAttribute(HtmlTextWriterAttribute.Value,SelectedDate.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			if(_renderLevel > 1 && _useSelectors == false)
			{
				int index, index2, day = 1, dayMax = GetDays(VisibleDate.Year, VisibleDate.Month),
					dayNext = 1, dayWeek = (int)(new DateTime(VisibleDate.Year, VisibleDate.Month, 1).DayOfWeek), showMonth = 1, showDay = 1, showYear = 1;
				string showColor = string.Empty;

				// The hidden field to store the day
				/*output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
					output.AddAttribute(HtmlTextWriterAttribute.Name,UniqueID + "_day");
					output.AddAttribute(HtmlTextWriterAttribute.Value,SelectedDate.Day.ToString());
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();*/
				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_day\" id=\"" + ClientID + "_day\" value=\"" + SelectedDate.Day.ToString() + "\">");

				// The hidden field to store the month
				/*output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
					output.AddAttribute(HtmlTextWriterAttribute.Name,UniqueID + "_month");
					output.AddAttribute(HtmlTextWriterAttribute.Value,SelectedDate.Month.ToString());
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();*/
				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_month\" id=\"" + ClientID + "_month\" value=\"" + SelectedDate.Month.ToString() + "\">");

				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_minDate\" id=\"" + ClientID + "_minDate\" value=\"" + string.Format("{0}/{1}/{2}",_minDate.Year,_minDate.Month,_minDate.Day) + "\">");

				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_maxDate\" id=\"" + ClientID + "_maxDate\" value=\"" + string.Format("{0}/{1}/{2}",_maxDate.Year,_maxDate.Month,_maxDate.Day) + "\">");

				// The hidden field to store the year
				/*output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
					output.AddAttribute(HtmlTextWriterAttribute.Name,UniqueID + "_year");
					output.AddAttribute(HtmlTextWriterAttribute.Value,SelectedDate.Year.ToString());
					output.RenderBeginTag(HtmlTextWriterTag.Input); 
					output.RenderEndTag();*/
				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_year\" id=\"" + ClientID + "_year\" value=\"" + SelectedDate.Year.ToString() + "\">");

				if (_multiSelection)
					output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_selectedDates\" id=\"" + ClientID + "_selectedDates\" value=\"" + SelectedDates.GetItemListToString(";") + "\">");

				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_blockedDates\" id=\"" + ClientID + "_blockedDates\" value=\"" + BlockedDates.GetItemListToString(";") + "\">");

				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_topZIndex\" id=\"" + ClientID + "_topZIndex\" value=\"\">");

				output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_autoPostBack\" id=\"" + ClientID + "_autoPostBack\" value=\"" + this.AutoPostBack.ToString() + "\">");

				// The hidden field to store if a post back have to be generated
				if (Events[EventClick] != null || _autoPostBack == true)
				{
					output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_doPostBackWhenClick\" id=\"" + ClientID + "_doPostBackWhenClick\" value=\"" + "True" + "\">");	
					output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
					output.AddAttribute(HtmlTextWriterAttribute.Id,this.UniqueID + "_dayClicked");
                    output.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + "_dayClicked");
					output.AddAttribute(HtmlTextWriterAttribute.Value,this.Page.GetPostBackEventReference(this,""));
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();
				}
				else
					output.Write("<input type=\"hidden\" name=\"" + UniqueID + "_doPostBackWhenClick\" id=\"" + ClientID + "_doPostBackWhenClick\" value=\"" + "False" + "\">");
				
				string left = string.Empty;
				string top = string.Empty;
				string position = string.Empty;
				IEnumerator enumerator = this.Style.Keys.GetEnumerator();
				while(enumerator.MoveNext())
				{
					if ((string)enumerator.Current.ToString().ToUpper() == "LEFT")
						left = this.Style[(string)enumerator.Current];
					else if ((string)enumerator.Current.ToString().ToUpper() == "TOP")
						top = this.Style[(string)enumerator.Current];
					else if ((string)enumerator.Current.ToString().ToUpper() == "POSITION")
						position = this.Style[(string)enumerator.Current];
				}
				//output.AddStyleAttribute("left",left);
				//output.AddStyleAttribute("top",top);
				string positionDiv = string.Empty;
				if (_useDatePicker)
					//output.AddStyleAttribute("position","relative");
					positionDiv = "position: relative;";
				else if (position != string.Empty)
					//output.AddStyleAttribute("position",position);
					positionDiv = "position: absolute;";
				//output.AddAttribute(HtmlTextWriterAttribute.Id,this.UniqueID + "_completeCalendar");
				//output.RenderBeginTag(HtmlTextWriterTag.Div);

				/*if (this.Page != null && this.Page.Request.Browser.Browser.ToUpper().IndexOf("NETSCAPE"))
					output.Write(string.Format("<div id=\"{0}\" onmousedown=\"return false;\" style=\"left: {1};top: {2};{3}\">", this.ClientID + "_completeCalendar", left, top, positionDiv));
				else*/
				output.Write(string.Format("<div id=\"{0}\" {4} style=\"left: {1};top: {2};{3}\">", this.ClientID + "_completeCalendar", left, top, positionDiv,Page.Request.Browser.Browser == "IE" ? "onmousedown=\"return false;\"" : string.Empty));

				if (_useDatePicker)
				{
                    //output.AddStyleAttribute("position", "absolute");
					// render pickup if necessary
					output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
					output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
					output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
					output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_pickupText");
					output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "_pickupText");
					if (ShowDateOnStart)
						if (SelectedDate != DateTime.MinValue)
						{
							if (_useCustomDateFormat)
								_pickupDateValue = DateOperation.FormatCustomDate(SelectedDate,_customDateFormat,_days,_months);
							else
								_pickupDateValue = DateOperation.FormatedDate(SelectedDate,_dateFormatLocale);

							if (_useTime)
								_pickupDateValue += string.Format(" {0:00}:{1:00}",SelectedDate.Hour,SelectedDate.Minute);
						}
					if (SelectedDate == DateTime.MinValue)
						_pickupDateValue = string.Empty;
					output.AddAttribute(HtmlTextWriterAttribute.Value, _pickupDateValue);
					output.AddAttribute("onblur",string.Format("return ACL_PickupTextValidation('{0}');",ClientID));
					output.AddAttribute("onkeypress",string.Format("return ACL_EnterValidation('{0}',event);",ClientID));
					_pickerTextBoxStyle.AddAttributesToRender(output,null,ImagesDirectory);
					if (!Enabled)
						output.AddAttribute("disabled",null);
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();
					output.RenderEndTag(); // TD

					output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
					output.AddAttribute(HtmlTextWriterAttribute.Align,"absmiddle");
					if (Enabled)
						output.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:ACL_ShowHideCalendar('" + ClientID + "',false);");
					output.RenderBeginTag(HtmlTextWriterTag.A); // open A
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(ImagesDirectory, _iconPath, "picker.gif", Page, this.GetType()));
					output.AddAttribute(HtmlTextWriterAttribute.Alt,_iconToolTip);
					output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
					output.RenderBeginTag(HtmlTextWriterTag.Img);	// open img
					output.RenderEndTag();	// close img
					output.RenderEndTag();	// close A
					output.RenderEndTag(); // TD
					output.RenderEndTag(); // TR
					output.RenderEndTag(); // TABLE
					//output.RenderEndTag();
					output.Write("</div>");
                    
					/*output.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_mask");
						output.AddAttribute("scrolling","no");
						output.AddStyleAttribute("position","absolute");
						output.AddStyleAttribute("display","none");
						output.AddStyleAttribute("top","0px");
						output.AddStyleAttribute("left","0px");
						output.RenderBeginTag(HtmlTextWriterTag.Iframe);
						output.RenderEndTag();*/

					string ssl = (EnableSsl) ? "src=\"blank.htm\" " : string.Empty;
                    output.Write(string.Format("<iframe {1}style=\"position: absolute; display:none; top : 0px; left : 0px; \" scrolling=\"no\" id=\"{0}\">Your browser does not support inline frames or is currently configured not to display inline frames.</iframe>", ClientID + "_mask", ssl));

					/*output.AddStyleAttribute("visibility", "visible");
						output.AddStyleAttribute("position", "relative");
						output.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "_div");
						output.RenderBeginTag(HtmlTextWriterTag.Div); // Open Div*/

					output.Write(string.Format("<div style=\"display:none; position: relative;\" id=\"{0}\">", this.ClientID + "_div"));
                    //output.Write("<iframe src=\"http://www.google.com\" style=\"position:absolute; visibility:hidden;\" frameborder=\"0\"></iframe>");
				}

				// Render the calendar
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, _cellPadding.ToString());
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, _cellSpacing.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(this.BorderColor));
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, this.BorderStyle.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, this.BorderWidth.ToString());
				output.RenderBeginTag(HtmlTextWriterTag.Table); 

				// Render the header
				if (!_showHeaderNavigation)
					output.AddStyleAttribute("visibility","hidden");
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Colspan, string.Format("{0}",_showWeekNumber ? 8 : 7));
				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				_titleStyle.AddAttributesToRender(output,ImagesDirectory);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				if (_showNextPrevMonth)
				{
				    ShowNextPreviousMonthYear(CalendarNextPreviousType.PreviousMonth, ref output);
				}
				if (_showNextPrevYear)
				{
				    ShowNextPreviousMonthYear(CalendarNextPreviousType.PreviousYear, ref output);
				}

				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				output.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				if (!_showMonth && !_showYear)
				{
					if (_useDatePicker)
					{
						WriteSelector(output, "_month_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", 1, 12, 2, VisibleDate.Month,false,true,false,2,1);
						WriteSelector(output, "_year_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", (SelectedDate.Year<_minDate.Year || VisibleDate.Year<_minDate.Year) && _autoAdjust ? (SelectedDate.Year<_minDate.Year ? SelectedDate.Year : VisibleDate.Year) : _minDate.Year, (SelectedDate.Year>_maxDate.Year || VisibleDate.Year>_maxDate.Year) && _autoAdjust ? (SelectedDate.Year>_maxDate.Year ? SelectedDate.Year : VisibleDate.Year) : _maxDate.Year, 4, VisibleDate.Year,false,true,false,2,1);
					}
					else
					{
						WriteSelector(output, "_month_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", 1, 12, 2, VisibleDate.Month,_showMonth,false,false,2,1);
						WriteSelector(output, "_year_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", (SelectedDate.Year<_minDate.Year || VisibleDate.Year<_minDate.Year) && _autoAdjust ? (SelectedDate.Year<_minDate.Year ? SelectedDate.Year : VisibleDate.Year) : _minDate.Year, (SelectedDate.Year>_maxDate.Year || VisibleDate.Year>_maxDate.Year) && _autoAdjust ? (SelectedDate.Year>_maxDate.Year ? SelectedDate.Year : VisibleDate.Year) : _maxDate.Year, 4, VisibleDate.Year,_showYear,true,false,2,1);
					}
				}
				else
				{
					if (_useDatePicker)
					{
						//WriteSelector(output, "_month_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", 1, 12, 2, VisibleDate.Month,false,true,false,2,1);
						WriteSelector(output, "_month_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", 1, 12, 2, VisibleDate.Month,_showMonth,true,false,2,1);
						WriteSelector(output, "_year_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", (SelectedDate.Year<_minDate.Year || VisibleDate.Year<_minDate.Year) && _autoAdjust ? (SelectedDate.Year<_minDate.Year ? SelectedDate.Year : VisibleDate.Year) : _minDate.Year, (SelectedDate.Year>_maxDate.Year || VisibleDate.Year>_maxDate.Year) && _autoAdjust ? (SelectedDate.Year>_maxDate.Year ? SelectedDate.Year : VisibleDate.Year) : _maxDate.Year, 4, VisibleDate.Year,_showYear,true,false,2,1);
					}
					else
					{
						//WriteSelector(output, "_month_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", 1, 12, 2, VisibleDate.Month,false,false,false,2,1);
						WriteSelector(output, "_month_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", 1, 12, 2, VisibleDate.Month,_showMonth,false,false,2,1);
						WriteSelector(output, "_year_selector", "javascript:ACL_RenderCalendar('" + ClientID + "');", (SelectedDate.Year<_minDate.Year || VisibleDate.Year<_minDate.Year) && _autoAdjust ? (SelectedDate.Year<_minDate.Year ? SelectedDate.Year : VisibleDate.Year) : _minDate.Year, (SelectedDate.Year>_maxDate.Year || VisibleDate.Year>_maxDate.Year) && _autoAdjust ? (SelectedDate.Year>_maxDate.Year ? SelectedDate.Year : VisibleDate.Year) : _maxDate.Year, 4, VisibleDate.Year,_showYear,false,false,2,1);
					}
				}
				output.RenderEndTag();
				if (_showNextPrevYear)
				{
				    ShowNextPreviousMonthYear(CalendarNextPreviousType.NextYear, ref output);
                }
				if (_showNextPrevMonth)  
				{
				    ShowNextPreviousMonthYear(CalendarNextPreviousType.NextMonth, ref output);
                }

				// tr
				output.RenderEndTag();
				// table
				output.RenderEndTag();
				// td
				output.RenderEndTag();
 
				// End of the header render
				output.RenderEndTag();

				// Render the days names
				if (_showDayHeader)
				{
					int firstDay = _firstDayOfWeek == System.Web.UI.WebControls.FirstDayOfWeek.Default ? 0 : (int)_firstDayOfWeek;
					_dayHeaderStyle.AddAttributesToRender(output,null,ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
	
					if (_showWeekNumber)
					{
						_dayHeaderStyle.AddAttributesToRender(output,null,ImagesDirectory);
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}%",100 /8));
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write(_weekNumberText);
						output.RenderEndTag();
					}

					for(index=0;index<7;index++)
					{
						_dayHeaderStyle.AddAttributesToRender(output,null,ImagesDirectory);
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}%",_showWeekNumber ? 100/8 : 100/7));
						output.RenderBeginTag(HtmlTextWriterTag.Td);

						string dayFormat = _days[index+firstDay < 7 ? index+firstDay : index+firstDay-7];

						switch (_dayNameFormat)
						{
							case System.Web.UI.WebControls.DayNameFormat.Full:
							{
							} break;

							case System.Web.UI.WebControls.DayNameFormat.FirstLetter:
							{
								dayFormat = dayFormat.Substring(0,1);
							} break;

							case System.Web.UI.WebControls.DayNameFormat.FirstTwoLetters:
							{
								dayFormat = dayFormat.Substring(0,2);
							} break;

							case System.Web.UI.WebControls.DayNameFormat.Short:
							{
								dayFormat = dayFormat.Substring(0,3);
							} break;

							default : break;
						}
						
						output.Write(dayFormat);

						output.RenderEndTag();
					}
					output.RenderEndTag();
				}

				// Render the days
				for(index=0;index<6;index++)
				{
					output.RenderBeginTag(HtmlTextWriterTag.Tr);

					if (_showWeekNumber)
					{
						int weekNumber = 1;
						int dayForWeekNumber = 1;
						int monthForWeekNumber = 1;
						int yearForWeekNumber = VisibleDate.Year;

						// The day is in the previous month
						if (index == 0)
						{
							dayForWeekNumber = 7 - dayWeek;
							monthForWeekNumber = VisibleDate.Month;
							/*yearForWeekNumber = monthForWeekNumber == 1 ? yearForWeekNumber - 1 : yearForWeekNumber;
							output.Write(dayForWeekNumber + "/" + monthForWeekNumber + "/" + yearForWeekNumber);*/
						}
						
						else if (showDay + 7 <= dayMax && VisibleDate.Month == showMonth)
						{
							dayForWeekNumber = showDay + 7;
							monthForWeekNumber = VisibleDate.Month;
							yearForWeekNumber = VisibleDate.Year;
						}

						else if (showDay + 7 > dayMax && VisibleDate.Month == showMonth)
						{
							dayForWeekNumber = (showDay + 7) - dayMax ;
							monthForWeekNumber = VisibleDate.Month == 12 ? 1 : (VisibleDate.Month+1);
							yearForWeekNumber  = VisibleDate.Year;
						}

						else
						{
							dayForWeekNumber = showDay + 7;
							monthForWeekNumber = VisibleDate.Month == 12 ? 1 : monthForWeekNumber + 1;
							yearForWeekNumber = VisibleDate.Month == 12 ? yearForWeekNumber + 1 : yearForWeekNumber;
						}

						weekNumber = DateOperation.GetWeekNumber(new DateTime(yearForWeekNumber,monthForWeekNumber,dayForWeekNumber));
			
						_weekNumberStyle.AddAttributesToRender(output,null,ImagesDirectory);
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_wn" + index.ToString());
						output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_wn" + index.ToString());
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write(string.Format("{0}",weekNumber.ToString()));
						output.RenderEndTag();
					}

					
					for(index2=0;index2<7;index2++)
					{
						// The day is in the Selected month
						if (day <= dayMax && !(index == 0 && index2 < dayWeek)) 
						{
							if(index2 == DateOperation.GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek.Saturday,_firstDayOfWeek) || index2 == DateOperation.GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek.Sunday,_firstDayOfWeek))
							{
								_weekEndDayStyle.AddAttributesToRender(output,null,ImagesDirectory);
								showColor = Utils.Color2Hex(_weekEndDayStyle.ForeColor);
							}
							else
								_dayStyle.AddAttributesToRender(output,null,ImagesDirectory);
							showColor = Utils.Color2Hex(_dayStyle.ForeColor);
							showDay = day;
							showMonth = VisibleDate.Month;
							showYear = VisibleDate.Year;
							day++;
						}
							// The day is in the previous month
						else if (index == 0)
						{
							//_dayStyle.AddAttributesToRender(output,null,ImagesDirectory);
							_otherMonthDayStyle.AddAttributesToRender(output,null,ImagesDirectory);
							//output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_otherMonthDayStyle.BackColor));
							showColor = Utils.Color2Hex(_otherMonthDayStyle.ForeColor);
							showDay = (GetDays(VisibleDate.Month == 1 ? VisibleDate.Year-1 : VisibleDate.Year, VisibleDate.Month == 1 ? 12 : VisibleDate.Month-1) - (dayWeek - index2 - 1));
							showMonth = VisibleDate.Month == 1 ? 12 : VisibleDate.Month-1;
							showYear = VisibleDate.Month == 1 ? VisibleDate.Year-1 : VisibleDate.Year;
						}
							// The day is in the next month
						else if (index != 0)
						{
							//_dayStyle.AddAttributesToRender(output,null,_ImagesDirectory);
							_otherMonthDayStyle.AddAttributesToRender(output,null,ImagesDirectory);
							//output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_otherMonthDayStyle.BackColor));
							showColor = Utils.Color2Hex(_otherMonthDayStyle.ForeColor);
							showDay = dayNext;
							showMonth = VisibleDate.Month == 12 ? 1 : (VisibleDate.Month-1+2);
							dayNext++;
							showYear = VisibleDate.Month == 12 ? VisibleDate.Year+1 : VisibleDate.Year;
						}
						
						// Check if this is a week end day
						/*if(index2 == GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek.Saturday) || index2 == GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek.Sunday))
						{
							_weekEndDayStyle.AddAttributesToRender(output,null,_ImagesDirectory);
							showColor = Utils.Color2Hex(_weekEndDayStyle.ForeColor);
						}*/

						DateTime dateToRender = new DateTime(showYear,showMonth,showDay);

						if (dateToRender >= MinDate && dateToRender <= MaxDate)
						{
							// check if this is a particular style
							bool dateIsParticularStyle = false;
							DateStyleCollectionItem dateStyle = null;
							foreach(DateStyleCollectionItem dateItem in _styleDates)
							{
								if (dateItem.Date == dateToRender)
								{
									dateIsParticularStyle = true;
									dateStyle = dateItem;
									break;
								}
							}

							if (dateIsParticularStyle == true)
							{
								output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(dateStyle.BackColor));
								//output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + dateStyle.BackgroundImage + ")");
								if (dateStyle.BackgroundImage != string.Empty)
									output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(ImagesDirectory,dateStyle.BackgroundImage) + ")");
								showColor = Utils.Color2Hex(dateStyle.ForeColor);
							}

							else
							{

								// check if this is a blocked date
						
								bool dateIsBlocked = false;
								foreach(DateCollectionItem dateItem in _blockedDates)
								{
									if (dateItem.Date == dateToRender)
									{
										dateIsBlocked = true;
										break;
									}
								}
								if (dateIsBlocked == true)
								{
									output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_blockedDayStyle.BackColor));
									showColor = Utils.Color2Hex(_blockedDayStyle.ForeColor);
								}

								else
								{
									bool dateIsSelected = false;
									if (_multiSelection == true)
									{
										foreach(DateCollectionItem dateItem in _selectedDates)
										{
											if (dateItem.Date == dateToRender)
											{
												dateIsSelected = true;
												output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_selectedDayStyle.BackColor));
												//output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + _selectedDayStyle.BackgroundImage + ")");
												if (_selectedDayStyle.BackgroundImage != string.Empty)
													output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(ImagesDirectory,_selectedDayStyle.BackgroundImage) + ")");
												showColor = Utils.Color2Hex(_selectedDayStyle.ForeColor);
												break;
											}
										}
									}

									if (dateIsSelected == false)
									{
										// Check if this is the selected day or not
										if (SelectedDate.Day == showDay && showMonth == SelectedDate.Month)
										{
											output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_selectedDayStyle.BackColor));
											//output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + _selectedDayStyle.BackgroundImage + ")");
											if (_selectedDayStyle.BackgroundImage != string.Empty)
												output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(ImagesDirectory,_selectedDayStyle.BackgroundImage) + ")");
											showColor = Utils.Color2Hex(_selectedDayStyle.ForeColor);
										}

											// check if this is the today date or not
										else if (showMonth == VisibleDate.Month && showDay == DateTime.Now.Day && VisibleDate.Month == DateTime.Now.Month && VisibleDate.Year == DateTime.Now.Year)
										{
											output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_todayDayStyle.BackColor));
											//output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + _todayDayStyle.BackgroundImage + ")");
											if (_todayDayStyle.BackgroundImage != string.Empty)
												output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(ImagesDirectory,_todayDayStyle.BackgroundImage) + ")");
											showColor = Utils.Color2Hex(_todayDayStyle.ForeColor);
										}
									}
								}

							}
						}
						else
						{
							output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_blockedDayStyle.BackColor));
							showColor = Utils.Color2Hex(_blockedDayStyle.ForeColor);

						}
						
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_d" + index.ToString() + index2.ToString());
						output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_d" + index.ToString() + index2.ToString());
						output.AddStyleAttribute("cursor","hand");
						/*if (_dayOverStyle.IsEmpty == false)
						{
							output.AddAttribute("onmouseover",string.Format("ACL_OverDay('{0}','{1}');",this.ClientID,ClientID + "_d" + index.ToString() + index2.ToString()));
							output.AddAttribute("onmouseout", string.Format("ACL_OutDay('{0}');",ClientID + "_d" + index.ToString() + index2.ToString()));
						}*/
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write("<font color='" + showColor + "'>" + showDay.ToString() + "</font>");
						output.RenderEndTag();
					}

					output.RenderEndTag();
				}

				if (_useTime)
				{ 
					_timeStyle.AddAttributesToRender(output,ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
					output.AddAttribute(HtmlTextWriterAttribute.Colspan, string.Format("{0}%",_showWeekNumber ? 100/8 : 100/7));
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					WriteSelector(output, "_hour", null, 0, 23, 2, SelectedDate.Hour,true);
					WriteSelector(output, "_minute", null, 0, 59, 2, SelectedDate.Minute,true);
					output.RenderEndTag();
					output.RenderEndTag();
				}
			
				if (_showTodayFooter)
				{
					output.AddStyleAttribute("cursor", "hand");
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
					output.AddAttribute(HtmlTextWriterAttribute.Colspan, string.Format("{0}%",_showWeekNumber ? 100/8 : 100/7));
					output.AddAttribute(HtmlTextWriterAttribute.Onclick,string.Format("ACL_ChangeToTodayDate('{0}')",this.ClientID));
					_todayFooterStyle.AddAttributesToRender(output,null,ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					string todayFooterText = "";
					if (_prefixTodayFooter.Trim() != string.Empty)
						todayFooterText += _prefixTodayFooter;
					if (_useCustomDateFormat == false)
						todayFooterText += DateOperation.FormatedDate(DateTime.Now,_dateFormatLocale);
					else
						todayFooterText += DateOperation.FormatCustomDate(DateTime.Now,_customDateFormat,_days,_months);
					output.Write(todayFooterText);
					output.RenderEndTag();
					output.RenderEndTag();
				}
					
				//output.Write(string.Format("<!--- Licensed to : {0} --->", licenseStatus.Info.Company));
					
				// End of the calendar render
				output.RenderEndTag();
				/*if (_useDatePicker)
					{
						output.RenderEndTag(); // close DIV
					}*/
				// Close Div
				output.Write("</div>");
				//output.RenderEndTag();

				_toolTipStyle.AddAttributesToRender(output,null,ImagesDirectory);
				output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_toolTipBox");
				output.AddStyleAttribute("position","absolute");
				output.AddStyleAttribute("visibility","hidden");

				output.RenderBeginTag(HtmlTextWriterTag.Div);
				output.RenderEndTag();
			}
			else
			{
				_useSelectors = true;

				/*// Check each element separated by semicolon char in the Format string.
				// If the element is the string representation of a specific date part,
				// the code render the selector. If not, the element is rendered as
				// text to the HtmlTextWriter.
				foreach(string element in Format.Split(';'))
				{
					switch (element.ToUpper())
					{
						case "DAY":
							WriteSelector(output, "_day", (_renderLevel > 0 ? "ACL_IsSelectorValid(" + ClientID + "_year," + ClientID + "_month," + ClientID + "_day)" : null), 1, 31, 2, SelectedDate.Day,true);
							break;
						case "MONTH":
							WriteSelector(output, "_month", (_renderLevel > 0 ? "ACL_IsSelectorValid(" + ClientID + "_year," + ClientID + "_month," + ClientID + "_day)" : null), 1, 12, 2, SelectedDate.Month,_showMonth);
							break;
						case "YEAR":
							WriteSelector(output, "_year", (_renderLevel > 0 ? "ACL_IsSelectorValid(" + ClientID + "_year," + ClientID + "_month," + ClientID + "_day)" : null), SelectedDate.Year<_minDate.Year && _autoAdjust ? SelectedDate.Year : _minDate.Year, SelectedDate.Year>_maxDate.Year && _autoAdjust ? SelectedDate.Year : _maxDate.Year, 4, SelectedDate.Year,_showYear);
							break;
						case "HOUR":
							WriteSelector(output, "_hour", null, 0, 23, 2, SelectedDate.Hour,true);
							break;
						case "MINUTE":
							WriteSelector(output, "_minute", null, 0, 59, 2, SelectedDate.Minute,true);
							break;
						case "SECOND":
							WriteSelector(output, "_second", null, 0, 59, 2, SelectedDate.Second,true);
							break;
						case "MILLISECOND":
							WriteSelector(output, "_millisecond", null, 0, 999, 3, SelectedDate.Millisecond,true);
							break;
						default:
							output.Write(element);
							break;
					}
				}*/

				// Check each element separated by semicolon char in the Format string.
				// If the element is the string representation of a specific date part,
				// the code render the selector. If not, the element is rendered as
				// text to the HtmlTextWriter.
				foreach(string preelement in Format.Split(';'))
				{
					//split elemnt string up to get increment
					String[] arrElement = preelement.Split('+');
					Int32 increment = 1;
					String element = preelement;
					if (arrElement.Length > 1)
					{
						element = arrElement[0].ToString();
						increment = ParseToNumber(arrElement[1].ToString());
					}

					switch (element.ToUpper())
					{
						case "DAY":
							WriteSelector(output, "_day", "ACL_IsSelectorValid(this.form." + UniqueID + "_year, this.form." + UniqueID + "_month, this.form." + UniqueID + "_day)", 1, 31, 2, SelectedDate.Day,true,true, AllowNull, 2, increment);
							break;
						case "MONTH":
							WriteSelector(output, "_month", "ACL_IsSelectorValid(this.form." + UniqueID + "_year, this.form." + UniqueID + "_month, this.form." + UniqueID + "_day)", 1, 12, 2, SelectedDate.Month, true,true,AllowNull, 2, increment);
							break;
						case "YEAR":
							WriteSelector(output, "_year", "ACL_IsSelectorValid(this.form." + UniqueID + "_year, this.form." + UniqueID + "_month, this.form." + UniqueID + "_day)", SelectedDate.Year<_minDate.Year && _autoAdjust ? SelectedDate.Year : _minDate.Year, SelectedDate.Year>_maxDate.Year && _autoAdjust ? SelectedDate.Year : _maxDate.Year, 4, SelectedDate.Year, true,true,AllowNull, 4, increment);
							break;
						case "HOUR":
							WriteSelector(output, "_hour", null, (Use24HourFormat ? 0 : 1), (Use24HourFormat ? 23 : 12), 2, (Use24HourFormat ? SelectedDate.Hour : GetMeridiemHour(SelectedDate.Hour)), true,true,AllowNull, 2, increment);
							break;
						case "MINUTE":
							WriteSelector(output, "_minute", null, 0, 59, 2, SelectedDate.Minute,true,true, AllowNull, 2, increment);
							break;
						case "SECOND":
							WriteSelector(output, "_second", null, 0, 59, 2, SelectedDate.Second, true,true,AllowNull, 2, increment);
							break;
						case "MILLISECOND":
							WriteSelector(output, "_millisecond", null, 0, 999, 3, SelectedDate.Millisecond, true,true,AllowNull, 3, increment);
							break;
						case "MERIDIEM":
							WriteSelector(output, "_meridiem", null, 0, 1, 2, (SelectedDate.Hour >= 12 ? 1 : 0), true,true,false, 2, increment);
							break;
						default:
							output.Write(element);
							break;
					}
				}
			}
		}

		/// <summary>
		///	Attempts to convert the inputted string to an Integer
		///	If it cant it returns 1
		///	Enlighten Designs
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private Int32 ParseToNumber(String input)
		{
			Double retval = 1;
			Double.TryParse(input, System.Globalization.NumberStyles.Integer, null, out retval);
			return Convert.ToInt32(retval);
		}

		/// <summary>
		/// Return the meridiem hour from the military hour.
		/// </summary>
		/// <param name="hour">The military hour.</param>
		/// <returns>The meridiem hour.</returns>
		public int GetMeridiemHour(int hour)
		{
			if (hour == 0)
				return 12;
			else if (hour <= 12)
				return hour;
			else
				return hour - 12;
		}
	
		/// <summary>
		/// Return the miliraty hour from the specified meridiem hour.
		/// </summary>
		/// <param name="hour">The meridiem hour.</param>
		/// <param name="meridiem">The meridiem.</param>
		/// <returns>The military hour.</returns>
		public int GetMiliratyHour(int hour, int meridiem)
		{
			if (hour == 12 && meridiem == 1)
				return 0;
			else if (meridiem == 1)
				return hour + 12;
			else
				return hour;
		}

		/// <summary>
		/// Sends server control content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.ACL, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else 
				RenderCalendar(output);
#else
			RenderCalendar(output);
#endif
			
		}

		private void WriteSelector(HtmlTextWriter output, string suffix, string onchange, int min, int max, int padding, int selectedValue, bool show)
		{
			WriteSelector(output,suffix,onchange,min, max,padding,selectedValue,show,false,true,2,1);
		}

		/// <summary>
		/// This method create a selector based on the parameters.
		/// </summary>
		/// <param name="output">The HtmlTextWriter to write.</param>
		/// <param name="suffix">The suffix to use to identify the selector with the LoadPostData method.</param>
		/// <param name="onchange">The value of the OnChange attribute of the selector to use with the client side validator.</param>
		/// <param name="min">The minimum value of the selector.</param>
		/// <param name="max">The maximum value of the selector.</param>
		/// <param name="padding">The number of chars to use with padding.</param>
		/// <param name="selectedValue">The selected value.</param>
		/// <param name="show">if set to <c>true</c> shows the selector.</param>
		/// <param name="relative">if set to <c>true</c> use relative position.</param>
		/// <param name="allowNull">if set to <c>true</c> allow null value.</param>
		/// <param name="nullChars">The null chars.</param>
		/// <param name="increment">The increment.</param>
		private void WriteSelector(HtmlTextWriter output, string suffix, string onchange, int min, int max, int padding, int selectedValue,bool show,bool relative,bool allowNull, int nullChars, int increment)
		{
			// Some variable we will use
			int index;

			// Check if the actual year value can be displayed in the selector
			//			if (selectedValue < min || selectedValue > max)
			//				throw new Exception("The year value (" + SelectedDate.Year.ToString() + ") of the Date property is greater than the maximum (" + max.ToString() + ") or less than the minimum (" + min.ToString() + "). Please adjust values or set AutoAdjust property to true.");

			// Write the selector
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + suffix);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + suffix);
						
			// Render the validation action code only if needed
			if(_renderLevel > 0 && onchange != null)
				output.AddAttribute(HtmlTextWriterAttribute.Onchange, onchange);

			// Apply the style
			_selectorStyle.AddAttributesToRender(output);
			
			/*if (show) 
			{
				output.AddStyleAttribute("position","relative");
				output.AddStyleAttribute("visibility","visible");
			}
			else 
			{
				if (relative)
					output.AddStyleAttribute("position","relative");
				else
					output.AddStyleAttribute("position","absolute");

				output.AddStyleAttribute("visibility","hidden");
			}*/
			if (!show) 
			{
				if (relative)
					output.AddStyleAttribute("position","relative");
				else
					output.AddStyleAttribute("position","absolute");

				output.AddStyleAttribute("visibility","hidden");
			}

			output.RenderBeginTag(HtmlTextWriterTag.Select);

			if (allowNull)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Option);
				for(index=0;index<nullChars;index++)
					output.InnerWriter.Write("-");
				output.RenderEndTag();
			}
			
			// Write the option tags
			for(index=min;index<=max;index+=increment)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Value, index.ToString());

				// Set the selected value
				if (SelectedDate != DateTime.MinValue && index == selectedValue)
					output.AddAttribute(HtmlTextWriterAttribute.Selected, null);
				
				output.RenderBeginTag(HtmlTextWriterTag.Option);
				if ((suffix == "_month" || suffix == "_month_selector") && !_monthNamesDisabled)
					output.InnerWriter.Write(_months[index-1]);
				else if (suffix == "_meridiem")
					output.InnerWriter.Write((index == 0 ? "AM" : "PM"));
				else
					output.InnerWriter.Write(index.ToString().PadLeft(padding, '0'));
				output.RenderEndTag();
			}

			// Write the selector end tag
			output.RenderEndTag();
		}

		/// <summary>
		/// Processes post back data for an the server control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>true if the server control's state changes as a result of the post back; otherwise false.</returns>
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			int day, month, year, hour, minute, second, millisecond;

			_oldDate = SelectedDate;
			if (_useSelectors == false)
			{
				_minDate = DateTime.Parse(postCollection[UniqueID + "_minDate"]);
				_maxDate = DateTime.Parse(postCollection[UniqueID + "_maxDate"]);
			}

			day = Convert.ToInt16(postCollection[UniqueID + "_day"]);
			month = Convert.ToInt16(postCollection[UniqueID + "_month"]);
			year = Convert.ToInt16(postCollection[UniqueID + "_year"]);

			hour = Convert.ToInt16(postCollection[UniqueID + "_hour"]);
			minute = Convert.ToInt16(postCollection[UniqueID + "_minute"]);
			second = Convert.ToInt16(postCollection[UniqueID + "_second"]);
			millisecond = Convert.ToInt16(postCollection[UniqueID + "_millisecond"]);

			if (_useSelectors == false)
			{

				int visibleYear = Int32.Parse(postCollection[UniqueID + "_year_selector"]);
				int visibleMonth = Int32.Parse(postCollection[UniqueID + "_month_selector"]);
				int visibleDay = VisibleDate.Day;
				int maxDay = DateTime.DaysInMonth(visibleYear,visibleMonth);
				if (visibleDay > maxDay)
					visibleDay = maxDay;
				this.VisibleDate = new DateTime(visibleYear,visibleMonth,visibleDay);

                try
				{
					string delimString = ";";
					char [] delimiter = delimString.ToCharArray();
					string [] split = null;

					_selectedDates.Clear();
					if (_multiSelection)
					{
						string selDateFromPage = postCollection[UniqueID + "_selectedDates"];
					
						split = selDateFromPage.Split(delimiter);

						for(int i = 0 ; i < split.Length - 1; i++)
						{
							if (split[i].Trim() != "")
								_selectedDates.Add(new DateCollectionItem(DateTime.Parse(split[i])));
						}

						if (_selectedDates.Count > 0)
							SelectedDate = _selectedDates[0].Date;
						else if (_selectedDates.Count == 0)
							SelectedDate = DateTime.MinValue;

					}
					else
					{
						SelectedDate = new DateTime(year == 0 ? DateTime.Now.Year : year, month == 0 ? DateTime.Now.Month : month, day == 0 ? DateTime.Now.Day : day, hour, minute, second, millisecond);
						_selectedDates.Clear();
						_selectedDates.Add(new DateCollectionItem(SelectedDate));
					}

					_blockedDates.Clear();
					string blockedDateFromPage = postCollection[UniqueID + "_blockedDates"];

					split = blockedDateFromPage.Split(delimiter);

					for (int i = 0 ; i < split.Length - 1 ; i++)
					{
						_blockedDates.Add(new DateCollectionItem(DateTime.Parse(split[i])));
					}
                

					if (_useDatePicker)
					{
						_pickupDateValue = DateOperation.FormatedDate(SelectedDate,_dateFormatLocale);
						if (_useTime)
							_pickupDateValue += string.Format(" {0:00}:{1:00}",SelectedDate.Hour,SelectedDate.Minute);
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
			else
			{
				try
				{
					SelectedDate = new DateTime(year == 0 ? DateTime.Now.Year : year, month == 0 ? DateTime.Now.Month : month, day == 0 ? DateTime.Now.Day : day, hour, minute, second, millisecond);				
					SelectedDates.Clear();
				}

				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
			
			if (!SelectedDate.Equals(_oldDate))
				return true;

			return false;
		}

		/// <summary>
		/// Signals the server control object to notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			/*OnDayClickedServer(new DayClickedEventArgs(SelectedDate));

			if (_oldDate != SelectedDate)
				OnDateChanged(new DateChangedEventArgs(_oldDate, SelectedDate));*/
			
		}

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the
		/// control has changed.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			OnDayClickedServer(new DayClickedEventArgs(SelectedDate));

			if (_oldDate != SelectedDate)
				OnDateChanged(new DateChangedEventArgs(_oldDate, SelectedDate));
		}

		/// <summary>
		/// Occurs when the Date property value changes.
		/// </summary>
		public event DateChangedEventHandler DateChanged
		{
			add
			{
				Events.AddHandler(EventChange,value);
			}

			remove
			{
				Events.RemoveHandler(EventChange,value);
			}
		}

		/// <summary>
		/// Occurs when a day is clicked.
		/// </summary>
		public event DayClickedEventHandler DayClicked
		{
			add
			{
				Events.AddHandler(EventClick,value);
			}

			remove
			{
				Events.RemoveHandler(EventClick,value);
			}
		}

		/// <summary>
		/// The DateChanged event handler.
		/// </summary>
		public delegate void DateChangedEventHandler(object sender, DateChangedEventArgs e);

		/// <summary>
		/// The DayClicked event handler.
		/// </summary>
		public delegate void DayClickedEventHandler(object sender, DayClickedEventArgs e);

		/// <summary>
		/// Raises the DateChanged event.
		/// </summary>
		/// <param name="e"> <see cref="DateChangedEventArgs"/> that contains the event data.</param>
		public virtual void OnDateChanged(DateChangedEventArgs e) 
		{
			// Check if someone use our event.
			/*if (DateChanged != null)
				DateChanged(this,e);*/

			DateChangedEventHandler changeHandler = (DateChangedEventHandler)Events[EventChange];
			if (changeHandler != null)
				changeHandler(this,e);
		}

		/// <summary>
		/// Raise the DayClickedServer.
		/// </summary>
		/// <param name="e"><see cref="DayClickedEventArgs"/> that contains the event data.</param>
		public virtual void OnDayClickedServer(DayClickedEventArgs e)
		{
			/*if (DayClicked != null)
				DayClicked(this,e);*/

			DayClickedEventHandler clickHandler = (DayClickedEventHandler)Events[EventClick];
			if (clickHandler != null)
				clickHandler(this,e);
		}

		/*private void CheckRegistration()
		{
			// Checks the registry.
			string registryRegCode = null;
			
			Microsoft.Win32.RegistryKey hKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Active Up\ActiveCalendar");
			
			if (hKey != null && hKey.ValueCount > 0)
				registryRegCode = (string)hKey.GetValue(@"CODE");
				
			// Initialize info about the registration;
			string key = "FLATCODE";

			if (RegistrationCode != null)
				_license = new License(RegistrationCode, key);
			else if (registryRegCode != null)
				_license = new License(registryRegCode, key);
			else
				_license = new License("815D20DBE917CC499DD837E7D81BEBE9F22FB3F126FB38A7", key);
			
		}*/

		#region Properties

		#region Appearance

		/// <summary>
        /// Gets or sets the background color of the <see cref="Toolbar"/>.
        /// </summary>
        [
            Bindable(true),
            Category("Appearance"),
            DefaultValue(typeof(Color), "#000000"),
            NotifyParentProperty(true),
            Description("Background color of the calendar.")
        ]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

		/// <summary>
		/// Gets or sets the border width of the calendar.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ArgumentException">The specified border width is a negative value.</exception>
        [
            Bindable(true),
            Category("Appearance"),
            DefaultValue(typeof(Unit), "1px"),
            NotifyParentProperty(true),
            Description("Border width of the calendar.")
        ]
        public override Unit BorderWidth
        {
            get { return base.BorderWidth; }
            set { base.BorderWidth = value; }
        }

		/// <summary>
		/// Gets or sets the displayed format for the days header.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(DayNameFormat.Short),
		Description("Display formats for the days of the week ")
		]
		public DayNameFormat DayNameFormat
		{
			get
			{
				return _dayNameFormat;
			}

			set
			{
				_dayNameFormat = value;
			}

		}

		/// <summary>
		/// Gets or sets the text of the week number.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue("Week"),
		Description("Display the text of the week number.")
		]
		public string WeekNumberText
		{
			get {return _weekNumberText;}
			set {_weekNumberText = value;}
		}

		/// <summary>
		/// Gets or sets the format to use to render the selectors.
		/// </summary>
		/// <remarks>You can specify the display layout using the <c>format specifiers</c> like <c>"hour"</c> or <c>"month"</c>.<br></br>
		/// Fields must be separated by ; (semicolon) char. Non reconized fields are send to the HtmlTextWriter as literal text.<br></br>
		/// Here are some examples (<c>[</c> and <c>]</c> represent a selector:<br></br><br></br>
		///	<c>"month;/;day;/;year"</c> will display<br></br><c>[MONTH]/[DAY]/[YEAR]</c>.<br></br><br></br>
		///	<c>"hour;:;minute"</c> will display<br></br><c>[HOUR]:[MINUTE]</c>.<br></br><br></br>
		///	<c>"Date : ;day;/;month;/;year; Time : ;hour;:;minute;:;second"</c> will display<br></br><c>Date : [DAY]/[MONTH]/[YEAR] Time : [HOUR]:[MINUTE]:[SECOND]</c>.<br></br><br></br>
		///	<table><tr><td bgcolor="#F0F0F0">Format Specifier</td><td bgcolor="#F0F0F0">Name</td></tr>
		///	<tr><td><b>day</b></td><td>The day part (1 to 31).</td></tr>
		///	<tr><td><b>month</b></td><td>The month part (1 to 12).</td></tr>
		///	<tr><td><b>year</b></td><td>The year part (variable).</td></tr>
		///	<tr><td><b>hour</b></td><td>The hour part (0 to 23).</td></tr>
		///	<tr><td><b>minute</b></td><td>The minute part (0 to 59).</td></tr>
		///	<tr><td><b>second</b></td><td>The second part (0 to 59).</td></tr>
		///	<tr><td><b>millisecond</b></td><td>The millisecond part (0 to 999).</td></tr>
		///	<tr><td><i>other literal</i></td><td>Rendered as literal text.</td></tr>
		///	</table>
		/// </remarks>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Format to use to render the selectors."),
		DefaultValue("MONTH;/;DAY;/;YEAR;-;HOUR;:;MINUTE;")
		]
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}

		/// <summary>
		/// Gets or sets the first day of the week.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("First day of the week."),
		DefaultValue(FirstDayOfWeek.Default)
		]
		public System.Web.UI.WebControls.FirstDayOfWeek FirstDayOfWeek
		{
			get
			{
				return _firstDayOfWeek;
			}
			set
			{
				_firstDayOfWeek = value;
			}
		}

		/// <summary>
		/// Specify if you want to display month names or month numbers. False by default.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Specify if you want to display month names or month numbers. False by default."),
		DefaultValue(false)
		]
		public bool MonthNamesDisabled
		{
			get
			{
				return _monthNamesDisabled;
			}
			set
			{
				_monthNamesDisabled = value;
			}
		}

		/// <summary>
		/// Specify if you want to display day header. True by default.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Specify if you want to display day header. False by default."),
		DefaultValue(true)
		]
		public bool ShowDayHeader
		{
			get
			{
				return _showDayHeader;
			}
			set
			{
				_showDayHeader = value;
			}
		}

		/// <summary>
		/// Specify the prefix of the today date located in the footer.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Specify the prefix of the today date located in the footer."),
		DefaultValue("")
		]
		public string PrefixTodayFooter
		{
			get
			{
				return _prefixTodayFooter;
			}

			set
			{
				_prefixTodayFooter = value;
			}
		}

		/// <summary>
		/// Specify if you want to display today footer. False by default.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Specify if you want to display today footer. False by default."),
		DefaultValue(false)
		]
		public bool ShowTodayFooter
		{
			get
			{
				return _showTodayFooter;
			}
			set
			{
				_showTodayFooter = value;
			}
		}


		/// <summary>
		/// Gets or sets the text displayed for next month the navigation.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Text displayed for next month the navigation."),
		DefaultValue("&gt;")
		]
		public string NextMonthText
		{
			get
			{
				return _nextMonthText;
			}
			set
			{
				_nextMonthText = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for next month the navigation.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Image displayed for next month the navigation."),
		DefaultValue("")
		]
		public string NextMonthImage
		{
			get
			{
				return _nextMonthImage;
			}

			set
			{
				_nextMonthImage = value;
			}
		}

		/// <summary>
		/// Gets or sets the text displayed for previous month the navigation.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Text displayed for previous month the navigation."),
		DefaultValue("&lt;")
		]
		public string PrevMonthText
		{
			get
			{
				return _prevMonthText;
			}
			set
			{
				_prevMonthText = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for previous month the navigation.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Image displayed for previous month the navigation."),
		DefaultValue("")
		]
		public string PrevMonthImage
		{
			get
			{
				return _prevMonthImage;
			}

			set
			{
				_prevMonthImage = value;
			}

		}

		/// <summary>
		/// Gets or sets the text displayed for next year the navigation.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Text displayed for next year the navigation."),
		DefaultValue("&gt;&gt;")
		]
		public string NextYearText
		{
			get
			{
				return _nextYearText;
			}
			set
			{
				_nextYearText = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for next year the navigation.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Image displayed for next year the navigation.")]
		public string NextYearImage
		{
			get
			{
				return _nextYearImage;
			}

			set
			{
				_nextYearImage = value;
			}
		}

		/// <summary>
		/// Gets or sets the text displayed for previous year the navigation.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Text displayed for previous year the navigation."),
		DefaultValue("&lt;&lt;")
		]
		public string PrevYearText
		{
			get
			{
				return _prevYearText;
			}
			set
			{
				_prevYearText = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for previous year the navigation.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Image displayed for previous year the navigation.")]
		public string PrevYearImage
		{
			get
			{
				return _prevYearImage;
			}

			set
			{
				_prevYearImage = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether client-side validation is enabled.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating whether client-side validation is enabled."),
		DefaultValue(true)
		]
		public bool EnableClientScript 
		{
			get 
			{
				object o = ViewState["EnableClientScript"];
				return((o == null) ? true : (bool)o);
			}
			set 
			{
				ViewState["EnableClientScript"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the minimum date to display selectable.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Minimum date to display selectable.")
		]
		public DateTime MinDate
		{
			get
			{
				return _minDate;
			}

			set
			{
				if (value > _maxDate)
					throw new Exception(string.Format("The minimum date ({0}) is greater than the maximum ({1}). Please adjust values or set AutoAdjust property to true.",((DateTime)value).ToShortDateString(), MaxDate.ToShortDateString()));

				_minDate = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum date to display selectable.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Maximum date to display selectable.")
		]
		public DateTime MaxDate
		{
			get
			{
				return _maxDate;
			}

			set
			{
				if (value < _minDate)
					throw new Exception(string.Format("The maximum date ({0}) is less than the minimum ({1}). Please adjust values or set AutoAdjust property to true.",((DateTime)value).ToShortDateString(), MinDate.ToShortDateString()));
				_maxDate = value;
			}
		}

		/// <summary>
		/// Gets or sets the amount of space between cells.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Amount of space between cells."),
		DefaultValue(1)

		]
		public int CellPadding
		{
			get
			{
				return _cellPadding;
			}
			set
			{
				_cellPadding = value;
			}
		}

		/// <summary>
		/// Gets or sets the amount of space between the content of cells and the cell's border.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Amount of space between the content of cells and the cell's border."),
		DefaultValue(0)
		]
		public int CellSpacing
		{
			get
			{
				return _cellSpacing;
			}
			set
			{
				_cellSpacing = value;
			}
		}

		/// <summary>
		/// Set to true is we want to auto adjust the maximum and/or minimum year with the data.
		/// </summary>
		/// <remarks>Setting a high value to <see cref="MaxDate"/> or setting a low value in <see cref="MinDate"/> can produce performance problem.
		/// For each year, more than 20 bytes are added to the browser HTML output.
		/// By setting this property to true, you can prevent from exception throws without having to set very high maximum year or very low minimum year.
		/// If the maximum year value is less than the actual date year value, the maximum year will be adjusted to the actual date year. Same for the minimum year.</remarks>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Set to true is we want to auto adjust the maximum and/or minimum year with the data."),
		DefaultValue(false)
		]
		public bool AutoAdjust
		{
			get
			{
				return _autoAdjust;
			}
			set
			{
				_autoAdjust = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating wheter Active Calendar displays the next and previous month navigation elements.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating wheter Active Calendar displays the next and previous month navigation elements."),
		DefaultValue(true)
		]
		public bool ShowNextPrevMonth
		{
			get
			{
				return _showNextPrevMonth;
			}
			set
			{
				_showNextPrevMonth = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating wheter Active Calendar displays the next and previous year navigation elements.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating wheter Active Calendar displays the next and previous year navigation elements."),
		DefaultValue(true)
		]
		public bool ShowNextPrevYear
		{
			get
			{
				return _showNextPrevYear;
			}
			set
			{
				_showNextPrevYear = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating wheter Active Calendar displays the number of week.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating wheter Active Calendar displays the number of week."),
		DefaultValue(false)
		]	
		public bool ShowWeekNumber
		{
			get
			{
				return _showWeekNumber;
			}

			set
			{
				_showWeekNumber = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating wheter Active Calendar displays the months selector.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating wheter Active Calendar displays the months selector."),
		DefaultValue(true)
		]	
		public bool ShowMonth
		{
			get
			{
				return _showMonth;
			}

			set
			{
				_showMonth = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating wheter Active Calendar displays the header navigation.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating wheter Active Calendar displays the header navigation."),
		DefaultValue(true)
		]	
		public bool ShowHeaderNavigation
		{
			get
			{
				return _showHeaderNavigation;
			}

			set
			{
				_showHeaderNavigation = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the value indicating wheter Active Calendar displays the years selector.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating wheter Active Calendar displays the years selector."),
		DefaultValue(true)
		]	
		public bool ShowYear
		{
			get
			{
				return _showYear;
			}

			set
			{
				_showYear = value;
			}
		}

		/// <summary>
		/// Gets or sets the factor used to increase or decrease the week number.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Factor used to increase or decrease the week number."),
			DefaultValue(0)
		]
		public int WeekNumberFactor
		{
			get
			{
				return _weekNumberFactor;
			}

			set
			{
				_weekNumberFactor = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if you want to use selectors to select all the date elements.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Value indicating if you want to use selectors to select all the date elements."),
		DefaultValue(false)
		]
		public bool UseSelectors
		{
			get
			{
				return _useSelectors;
			}
			set
			{
				_useSelectors = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the control render.</remarks>
		[Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where the API javascript file is."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.SCRIPT_DIRECTORY)
#endif
		]
		public string ScriptDirectory
		{
			get
			{
                if (_scriptDirectory == null)
#if (!FX1_1)
                    return "";
#else
                    return Define.SCRIPT_DIRECTORY;
#endif
                else
                    return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if you want to allow time selecting in the calendar.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Indicates if you want to allow time selecting in the calendar."),
		//DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		NotifyParentPropertyAttribute(true),
		DefaultValue(false)
		]
		public bool UseTime
		{
			get
			{
				return _useTime;
			}
			set
			{
				_useTime = value;
			}
		}

		/// <summary>
		/// Gets or sets the month names.
		/// </summary>
		/// <remarks>
		/// <code>
		/// // This line will replace the january month name by 'Janvier'
		/// MyDate.Months[0] = "Janvier";
		/// 
		/// // This line will replace the december month name by 'Décembre (noel)'
		/// MyDate.Months[11] = "Décembre (noel)";
		/// </code>
		/// </remarks>
		[
		Bindable(true),
		Category("Appearance"),
		Description("The month names."),
		//PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)
		TypeConverter(typeof(StringArrayConverter))
		]
		public string[] Months
		{
			get
			{
				return _months;
			}
			set
			{
				_months = value;
			}
		}

		/// <summary>
		/// Sets the month names.
		/// </summary>
		/// <remarks>
		/// This array of string contains the month names. You can set your own month names to match with your culture. If your website is in french, you will prefer use Février in replacment of February.
		/// By default, the months are in english. If the <see cref="MonthNamesDisabled"/> property is set to true, numbers will replace month names.
		/// <code>
		/// // Please verify that MonthNamesDisabled property is not set to true
		/// &lt;AU:ActiveCalendar runat="server" id="MyCalendar" SetMonthNames="Janvier,F&amp;eacute;vrier,Mars,Avril,Mai,Juin,Juillet,Ao&amp;ucirc;t,Septembre,Octobre,Novembre,D&amp;eacute;cembre"&gt;&lt;/AU:ActiveDateTime&gt;
		/// </code>
		/// </remarks>
		public string SetMonthNames
		{
			set
			{
				string[] months = value.Split(',');
				int index;

				for(index=0;index<months.Length;index++)
				{
					_months[index] = months[index];
				}
			}
		}

		/// <summary>
		/// Gets or sets the day names.
		/// </summary>
		/// <remarks>
		/// <code>
		/// // This line will replace the sunday day name by 'DIM'
		/// MyDate.Days[0] = "DIM";
		/// 
		/// // This line will replace the saturday day name by 'SAM'
		/// MyDate.Days[6] = "SAM";
		/// </code>
		/// </remarks>
		[
		Bindable(true),
		Browsable(true),
		Category("Appearance"),
		Description("The day names."),
		PersistenceMode(PersistenceMode.Attribute),
		TypeConverter(typeof(StringArrayConverter)),
		]
		public string[] Days
		{
			get
			{
				return _days;
			}
			set
			{
				_days = value;
			}
		}

		/// <summary>
		/// Sets the day names.
		/// </summary>
		/// <remarks>
		/// This array of string contains the day names. You can set your own day names to match with your culture. If your website is in french, you will prefer use LUN (Lundi) in replacment of MON (Monday).
		/// By default, the days are in english. If the <see cref="ShowDayHeader"/> property is set to true, the day names will not be displayed.
		/// <code>
		/// // Please verify that DayNamesDisabled property is not set to true
		/// &lt;AU:ActiveCalendar runat="server" id="MyCalendar" SetDayNames="DIM,LUN,MAR,MER,JEU,VEN,SAM"&gt;&lt;/AU:ActiveDateTime&gt;
		/// </code>
		/// </remarks>
		public string SetDayNames
		{
			set
			{
				string[] days = value.Split(',');
				int index;

				for(index=0;index<days.Length;index++)
				{
					_days[index] = days[index];
				}
			}
		}

		
		/// <summary>
		/// Gets or sets the file with the localized labels.
		/// </summary>
		[Bindable(false),
		Category("Appearance"),
		Description("Gets or sets the file with the localized labels.")	]
		public string LocalizationFile
		{
			get
			{
				return _localizationFile;
			}
			set
			{
				_localizationFile = value;
			}
		}

		#endregion

		#region DateOperation

		/// <summary>
		/// Allow to select several dates.
		/// </summary>
		[
		Bindable(true),	
		Category("Date operation"),
		Description("Allow to select several dates"),
		DefaultValue(false)
		]
		public bool MultiSelection
		{
			get
			{
				return _multiSelection;
			}

			set
			{
				if (_useDatePicker == true && value == true)
					throw new ArgumentException("The MultiSelection property cannot be enabled if you use the ActiveCalendar as DatePicker. You have to set the property UseDatePicker to false.");	
				
				_multiSelection = value;
			}
		}
			
		/// <summary>
		/// Gets or sets the selected date.
		/// </summary>
		[Bindable(true),
		Category("Date operation"),
		Description("The selected date.")]
		public DateTime SelectedDate
		{
			get
			{
				if (ViewState["_selectedDate"] == null)
					ViewState["_selectedDate"] = DateTime.Parse(DateTime.Now.ToShortDateString());
				return (DateTime)ViewState["_selectedDate"];
			}
			set
			{
				ViewState["_selectedDate"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the visible date.
		/// </summary>
		[Bindable(true),
		Category("Date operation"),
		Description("The visible date.")]
		public DateTime VisibleDate
		{
			get
			{
				if (ViewState["_visibleDate"] == null)
					ViewState["_visibleDate"] = DateTime.Parse(DateTime.Now.ToShortDateString());
				return (DateTime)ViewState["_visibleDate"];
			}
			set
			{
				ViewState["_visibleDate"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the collection containing the blocked dates.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("Date operation"),
		Description("Blocked dates. Dates that are not selectionable."),
		DefaultValue(null),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public DateCollection BlockedDates
		{
			get
			{
				return _blockedDates;
			}
		}

		/// <summary>
		/// Gets or sets the collection containing the selected dates.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("Date operation"),
		Description("The selected dates."),
		DefaultValue(null),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public DateCollection SelectedDates
		{
			get
			{
				return _selectedDates;
			}
		}

		#endregion

		#region DatePicker

		/// <summary>
		/// Gets or sets the tooltip text of the icon.
		/// </summary>
		[
		Bindable(true),
		Category("Date picker"),
		Description("Tooltip text for icon")
		]
		public string IconToolTip
		{
			get
			{
				return _iconToolTip;
			}

			set
			{
				_iconToolTip = value;
			}
		}		

		/// <summary>
		/// Gets or sets the relative path of the icon.
		/// </summary>
		[
		Bindable(true),
		Category("Date picker"),
		Description("Relative path of the icon."),
		]
		public string IconPath
		{
			get
			{
				return _iconPath;
			}

			set
			{
				_iconPath = value;
			}
		}

		/// <summary>
		/// Indicates if the calendar is used as picker calendar.
		/// </summary>
		[
		Bindable(true),
		Category("Date picker"),
		Description("Indicates if the ActiveCalendar is used as picker calendar."),
		DefaultValue(false)
		]
		public bool UseDatePicker
		{
			get
			{
				return _useDatePicker;
			}

			set
			{
				if (_multiSelection == true && value == true)
					throw new ArgumentException("The UseDatePicker property cannot be enabled if you use the multi selection. You have to set the property MultiSelection to false.");	
					
				_useDatePicker = value;
			}
		}

		/// <summary>
		/// Gets or sets the values indicates if the selected date must be displayed in the textbox when the application start.
		/// </summary>
		[
		Bindable(true),
		Category("Date picker"),
		Description("Indicates if the selected date must be displayed in the textbox when the application start."),
		DefaultValue(true)
		]
		public bool ShowDateOnStart
		{
			get {return _showDateOnStart;}
			set {_showDateOnStart = value;}
		}

		#endregion

		#region JScript

		/// <summary>
		/// Gets or sets the relative or absolute path to the external ActiveCalendar API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the ActiveCalendar render.</remarks>
		[Bindable(false),
		Category("JScript"),
		Description("Relative or absolute path to the external ActiveCalendar API javascript file.")	]
		public string ExternalScript
		{
			get
			{
				return _externalScript;
			}
			set
			{
				_externalScript = value;
			}
		}

		/// <summary>
		/// Gets or sets the additional JScript client-side functions to call when a click on a day occurs.
		/// </summary>
		[
		Bindable(true),
		Category("JScript"),
		Description("Additional JScript client-side functions to call when a click on a day occurs.")
		]
		public string OnDayClicked
		{
			get
			{
				return _onDayClicked;
			}

			set
			{
				_onDayClicked = value;
			}
		}

		/// <summary>
		/// Gets or sets the additional JScript client-side functions to call when a blur event on the picker textbox occurs.
		/// </summary>
		[
		Bindable(true),
		Category("JScript"),
		Description("Additional JScript client-side functions to call when a blur event on the picker textbox occurs.")
		]
		public string OnBlurPicker
		{
			get
			{
				return _onBlurPicker;
			}

			set
			{
				_onBlurPicker = value;
			}
		}

		#endregion

		#region Data

		/*
		/// <summary>
		/// Gets or sets the registration code.
		/// </summary>
		/// <remarks>The registration code is not required to run the control, but a signature will be displayed at the bottom. If you have a registration code, you can enter it with this property. Note that you can register the control globally using the <c>register.exe</c> file provided with the package. This will let you use the control on multiple websites without having to specify each time the registration code in the <c>RegistrationCode</c> property.</remarks>
		[
			Bindable(false),
			Category("Data"),
			Description("Registration code.")
		]
		public string RegistrationCode
		{
			get
			{
				return _registrationCode;
			}
			set
			{
				_registrationCode = value;
			}
		}*/


/*
#if (LICENSE)

		/// <summary>
		/// Gets or sets the license key.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("Lets you specify the license key.")
		]
		public string License
		{
			get
			{
				return _license;
			}
		
			set
			{
				_license = value;
			}
		}

#endif
*/
		#endregion

		#region Styles
		
		/// <summary>
		/// Gets or sets the Cascading Style Sheet (CSS) class rendered by the calendar on the client.
		/// </summary>
		/// <value></value>
		[
			Browsable(false),
            DesignOnly(true),
			Description("N/A")
		]
		public new string CssClass
		{
			get
			{
				return this.CssClass;
			}

			set
			{
				this.CssClass = value;
			}

		}

		/// <summary>
		/// Gets or sets the style properties of the day over.
		/// </summary>
		[
		Bindable(false),
		Category("Style"),
		Description("Style properties of the day over."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleShort DayOverStyle
		{
			get
			{
				return _dayOverStyle;
			}
			set
			{
				_dayOverStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the day names header.
		/// </summary>
		[Bindable(true),
		Category("Style"),
		Description("Style properties of the day names header.")]
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[DefaultValueAttribute(null)]
		[NotifyParentPropertyAttribute(true)]
		public CalendarDayStyleFull DayHeaderStyle
		{
			get
			{
				return _dayHeaderStyle;
			}
			set
			{
				_dayHeaderStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the day.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the day."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull DayStyle
		{
			get
			{
				return _dayStyle;
			}
			set
			{
				_dayStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the weekend days.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the weekend days."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull WeekEndDayStyle
		{
			get
			{
				return _weekEndDayStyle;
			}
			set
			{
				_weekEndDayStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the weekend number.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the weekend number."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull WeekNumberStyle
		{
			get
			{
				return _weekNumberStyle;
			}
			set
			{
				_weekNumberStyle = value;
			}
		}	

		/// <summary>
		/// Gets or sets the style properties of the next and previous navigation elements.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the next and previous navigation elements."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public System.Web.UI.WebControls.Style NextPrevStyle
		{
			get
			{
				return _nextPrevStyle;
			}
			set
			{
				_nextPrevStyle = value;
			}
		}


		/// <summary>
		/// Gets or sets the style properties of the selected day.
		/// </summary>
		[
		Bindable(false),
		Category("Style"),
		Description("Style properties of the selected day."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleShort SelectedDayStyle
		{
			get
			{
				return _selectedDayStyle;
			}
			set
			{
				_selectedDayStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the title.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of title bar."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleShort TitleStyle
		{
			get
			{
				return _titleStyle;
			}

			set
			{
				_titleStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the today footer.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of today footer style."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull TodayFooterStyle
		{
			get
			{
				return _todayFooterStyle;
			}

			set
			{
				_todayFooterStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the properties style of the time.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the time."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleShort TimeStyle
		{
			get
			{
				return _timeStyle;
			}

			set
			{
				_timeStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the blocked day.
		/// </summary>
		[
		Bindable(false),
		Category("Style"),
		Description("Style properties of the blocked day."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleShort BlockedDayStyle
		{
			get
			{
				return _blockedDayStyle;
			}
			set
			{
				_blockedDayStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the day that is not in the selected month.
		/// </summary>
		[
		Bindable(false),
		Category("Style"),
		Description("Style properties of the day that is not in the selected month."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull OtherMonthDayStyle
		{
			get
			{
				return _otherMonthDayStyle;
			}
			set
			{
				_otherMonthDayStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the today day.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the today day."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleShort TodayDayStyle
		{
			get
			{
				return _todayDayStyle;
			}
			set
			{
				_todayDayStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the selectors.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the selectors."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public System.Web.UI.WebControls.Style SelectorStyle
		{
			get
			{
				return _selectorStyle;
			}
			set
			{
				_selectorStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the collection containing the style for a particular date.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("Style"),
		Description("The syle of a particular date."),
		DefaultValue(null),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public DateStyleCollection StyleDates
		{
			get
			{
				return _styleDates;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the textbox in case in picker mode.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the textbox in picker mode."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull PickerTextBoxStyle
		{
			get
			{
				return _pickerTextBoxStyle;
			}
			set
			{
				_pickerTextBoxStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the style properties of the tooltip.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		Description("Style properties of the tooltip."),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
		DefaultValueAttribute(null),
		NotifyParentPropertyAttribute(true)
		]
		public CalendarDayStyleFull ToolTipStyle
		{
			get
			{
				return _toolTipStyle;
			}
			set
			{
				_toolTipStyle = value;
			}
		}

		#endregion

		#region Behavior

		
		/// <summary>
		/// Indicates if you want to generate a postback each times a date is selected.
		/// </summary>
		[
		Bindable(true),	
		Category("Behavior"),
		Description("Indicates if you want to generate a postback each times a date is selected."),
		DefaultValue(false)
		]
		public bool AutoPostBack 
		{
			get {return _autoPostBack;}
			set {_autoPostBack = value;}
		}

		/// <summary>
		/// Gets or sets if the date must be change when the navigation change.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Indicates if the date must be change when the navigation change."),
		DefaultValue(true)
		]
		public bool NavigationChangeDate
		{
			get
			{
				return _navigationChangeDate;
			}

			set
			{
				_navigationChangeDate = value;
			}
		}

		/// <summary>
		/// Specify whether you want the editor to auto detect the SSL environment.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Specify whether you want the editor to auto detect the SSL environment."),
		DefaultValue(true)
		]
		public bool AutoDetectSsl { get; set; } = true;

		/// <summary>
		/// Set to true if you need to use the control in a secure web page.
		/// </summary>
		[Bindable(false),
		Category("Behavior"),
		DefaultValue(false),
		Description("Set it to true if you need to use the control in a secure web page.")	]
		public new bool EnableSsl { get; set; }

		#endregion

		#region Format

		/// <summary>
		/// Gets or sets the local format used.
		/// </summary>
		[
		Bindable(true),
		Category("Format"),
		Description("Locale format used."),
		DefaultValue(DateFormatLocale.en)
		]
		public DateFormatLocale DateFormatLocale
		{
			get
			{
				return _dateFormatLocale;
			}

			set
			{
				_dateFormatLocale = value;
			}
		}

		/// <summary>
		/// Gets or sets if you want to use the custom date format.
		/// </summary>
		[
		Bindable(true),
		Category("Format"),
		Description("If the Custom Date Format must be used."),
		DefaultValue(false)
		]
		public bool UseCustomDateFormat
		{
			get
			{
				return _useCustomDateFormat;
			}

			set
			{
				_useCustomDateFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the custom date format.
		/// </summary>
		[
		Bindable(true),
		Category("Format"),
		Description("The custom date format to apply"),
		DefaultValue("dd/mm/yyyy")
		]
		public string CustomDateFormat
		{
			get
			{
				return _customDateFormat;
			}

			set
			{
				_customDateFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets value indicating if you want to use the 24h (military) format.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets or sets value indicating if you want to use the 24h (military) format."),
		DefaultValue(true)
		]
		public bool Use24HourFormat
		{
			get
			{
				return _use24HourFormat;
			}
			set
			{
				_use24HourFormat = value;
			}
		}

		
		/// <summary>
		/// Gets or sets then value indicating if the control can accept null value (DateTime.MinValue).
		/// </summary>
		[Bindable(true),
		Category("Behavior"),
		Description("Gets or sets then value indicating if the control can accept null value (DateTime.MinValue).")]
		public bool AllowNull
		{
			get
			{
				return _allowNull;
			}
			set
			{
				_allowNull = value;
			}
		}

		#endregion

		#region ToolTip

		/// <summary>
		/// Gets or sets the tooltip text for a particular date.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("ToolTip"),
		Description("The tooltip of a particular date."),
		DefaultValue(null),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public DateToolTipCollection ToolTipCustomDates
		{
			get
			{
				return _toolTipCustomDates;
			}
		}

		/// <summary>
		/// Tooltip text of the blocked dates.
		/// </summary>
		[
		Bindable(true),	
		Category("ToolTip"),
		Description("Tooltip text of the blocked dates."),
		DefaultValue(false)
		]
		public string ToolTipBlocked
		{
			get {return _toolTipBlocked;}
			set {_toolTipBlocked = value;}
		}

		/// <summary>
		/// Tooltip text of the weekend dates.
		/// </summary>
		[
		Bindable(true),	
		Category("ToolTip"),
		Description("Tooltip text of the blocked dates."),
		DefaultValue(false)
		]
		public string ToolTipWeekend
		{
			get {return _toolTipWeekend;}
			set {_toolTipWeekend = value;}
		}

		/// <summary>
		/// Tooltip text of the normal dates.
		/// </summary>
		[
		Bindable(true),	
		Category("ToolTip"),
		Description("Tooltip text of the normal dates."),
		DefaultValue(false)
		]
		public string ToolTipNormal
		{
			get {return _toolTipNormal;}
			set {_toolTipNormal = value;}
		}

		/// <summary>
		/// Tooltip text of the other dates.
		/// </summary>
		[
		Bindable(true),	
		Category("ToolTip"),
		Description("Tooltip text of the other dates."),
		DefaultValue(false)
		]
		public string ToolTipOther
		{
			get {return _toolTipOther;}
			set {_toolTipOther = value;}
		}

		/// <summary>
		/// Tooltip text of the selected date.
		/// </summary>
		[
		Bindable(true),	
		Category("ToolTip"),
		Description("Tooltip text of the selected date."),
		DefaultValue(false)
		]
		public string ToolTipSelected
		{
			get {return _toolTipSelected;}
			set {_toolTipSelected = value;}
		}

		/// <summary>
		/// Tooltip text of the today date.
		/// </summary>
		[
		Bindable(true),	
		Category("ToolTip"),
		Description("Tooltip text of the today date."),
		DefaultValue(false)
		]
		public string ToolTipToday
		{
			get {return _toolTipToday;}
			set {_toolTipToday = value;}
		}

		#endregion

		#endregion
	}
}
