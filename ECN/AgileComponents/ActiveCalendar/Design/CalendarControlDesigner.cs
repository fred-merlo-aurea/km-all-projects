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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.IO;

namespace ActiveUp.WebControls.Design
{
	#region class CalendarControlDesigner

	internal class CalendarControlDesigner : ControlDesigner
	{
		private DesignerVerbCollection designerVerbs;

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				if (designerVerbs == null) 
				{
					designerVerbs = new DesignerVerbCollection();
					designerVerbs.Add(new DesignerVerb("Property Builder...", new EventHandler(this.OnPropertyBuilder)));
				}

				return designerVerbs;
			}
		}

		private void OnPropertyBuilder(object sender, EventArgs e) 
		{
			CalendarComponentEditor compEditor = new CalendarComponentEditor();
			compEditor.EditComponent(Component);
		}

		public override void Initialize(IComponent component) 
		{
			if (!(component is ActiveUp.WebControls.Calendar)) 
			{
				throw new ArgumentException("Component must be a ActiveUp.WebControls.ActiveCalendar control.", "component");
			}
			base.Initialize(component);
		}

		/// <summary>
		/// Gets the design time HTML code.
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		public override string GetDesignTimeHtml()
		{
			Calendar calendar = (Calendar)base.Component;

			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter output = new HtmlTextWriter(stringWriter);

            if(calendar.UseSelectors == false)
			{
				int index, index2, day = 1, dayMax = calendar.GetDays(calendar.VisibleDate.Year, calendar.VisibleDate.Month),
					dayNext = 1, dayWeek = (int)(new DateTime(calendar.VisibleDate.Year, calendar.VisibleDate.Month, 1).DayOfWeek), showMonth = 1, showDay = 1, showYear = 1;
				string showColor = string.Empty;
				string pickupDateValue = string.Empty;

				string left = string.Empty;
				string top = string.Empty;
				string position = string.Empty;
				IEnumerator enumerator = calendar.Style.Keys.GetEnumerator();
				while(enumerator.MoveNext())
				{
					if ((string)enumerator.Current.ToString().ToUpper() == "LEFT")
						left = calendar.Style[(string)enumerator.Current];
					else if ((string)enumerator.Current.ToString().ToUpper() == "TOP")
						top = calendar.Style[(string)enumerator.Current];
					else if ((string)enumerator.Current.ToString().ToUpper() == "POSITION")
						position = calendar.Style[(string)enumerator.Current];
				}
				string positionDiv = string.Empty;
				if (calendar.UseDatePicker)
					positionDiv = "position: relative;";
				else if (position != string.Empty)
					positionDiv = "position: absolute;";

				output.Write(string.Format("<div style=\"left: {0};top: {1};{2}\">", left, top, positionDiv));

				if (calendar.UseDatePicker)
				{
					// render pickup if necessary
                    output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
					output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
					output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
					output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
					if (calendar.ShowDateOnStart)
						if (calendar.SelectedDate != DateTime.MinValue)
						{
							if (calendar.UseCustomDateFormat)
								pickupDateValue = DateOperation.FormatCustomDate(calendar.SelectedDate,calendar.CustomDateFormat,calendar.Days,calendar.Months);
							else
								pickupDateValue = DateOperation.FormatedDate(calendar.SelectedDate,calendar.DateFormatLocale);
						}
					output.AddAttribute(HtmlTextWriterAttribute.Value,  pickupDateValue);
					calendar.PickerTextBoxStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();
					output.RenderEndTag(); // TD

					output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
					output.AddAttribute(HtmlTextWriterAttribute.Align,"absmiddle");
					output.AddAttribute(HtmlTextWriterAttribute.Src,Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.IconPath,"picker.gif",calendar.Page,this.GetType()));
					output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
					output.RenderBeginTag(HtmlTextWriterTag.Img);	// open img
					output.RenderEndTag();	// close img
					output.RenderEndTag(); // TD
					output.RenderEndTag(); // TR
					output.RenderEndTag(); // TABLE
					output.Write("</div>");
                    
					output.Write("<div style=\"visibility: visible; position: relative;\" id=\"{0}\">");

				}

				// Render the calendar
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, calendar.CellPadding.ToString());
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, calendar.CellSpacing.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(calendar.BorderColor));
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, calendar.BorderStyle.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, calendar.BorderWidth.ToString());
				output.RenderBeginTag(HtmlTextWriterTag.Table); 

				// Render the header
				if (!calendar.ShowHeaderNavigation)
					output.AddStyleAttribute("visibility","hidden");
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Colspan, string.Format("{0}",calendar.ShowWeekNumber ? 8 : 7));
				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				calendar.TitleStyle.AddAttributesToRender(output,calendar.ImagesDirectory);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				if (calendar.ShowNextPrevMonth)
				{
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.RenderBeginTag(HtmlTextWriterTag.A);
					
					if (calendar.PrevMonthImage.Trim() == string.Empty)
						output.Write(calendar.PrevMonthText);
					else
					{
						output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
						output.AddAttribute(HtmlTextWriterAttribute.Src,Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.PrevMonthImage));
						output.RenderBeginTag(HtmlTextWriterTag.Img);
						output.RenderEndTag();
					}

					output.RenderEndTag();
					output.Write("&nbsp;");
					output.RenderEndTag();
				}
				if (calendar.ShowNextPrevYear)
				{
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.RenderBeginTag(HtmlTextWriterTag.A);

					if (calendar.PrevYearImage.Trim() == string.Empty)
						output.Write(calendar.PrevYearText);
					else
					{
						output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
						output.AddAttribute(HtmlTextWriterAttribute.Src,Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.PrevYearImage));
						output.RenderBeginTag(HtmlTextWriterTag.Img);
						output.RenderEndTag();
					}

					output.RenderEndTag();
					output.Write("&nbsp;");
					output.RenderEndTag();
				}
				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				output.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				if (!calendar.ShowMonth && !calendar.ShowYear)
				{
					if (calendar.UseDatePicker)
					{
						WriteSelector(output, "_month_selector", 1, 12, 2, calendar.VisibleDate.Month,calendar.SelectorStyle,calendar.Months,calendar.ShowMonth,true);
						WriteSelector(output, "_year_selector", (calendar.SelectedDate.Year<calendar.MinDate.Year || calendar.VisibleDate.Year<calendar.MinDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year<calendar.MinDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MinDate.Year, (calendar.SelectedDate.Year>calendar.MaxDate.Year || calendar.VisibleDate.Year>calendar.MaxDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year>calendar.MaxDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MaxDate.Year, 4, calendar.VisibleDate.Year,calendar.SelectorStyle,calendar.Months,calendar.ShowYear,true);
					}

					else
					{
						WriteSelector(output, "_month_selector", 1, 12, 2, calendar.VisibleDate.Month,calendar.SelectorStyle,calendar.Months,calendar.ShowMonth);
						WriteSelector(output, "_year_selector", (calendar.SelectedDate.Year<calendar.MinDate.Year || calendar.VisibleDate.Year<calendar.MinDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year<calendar.MinDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MinDate.Year, (calendar.SelectedDate.Year>calendar.MaxDate.Year || calendar.VisibleDate.Year>calendar.MaxDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year>calendar.MaxDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MaxDate.Year, 4, calendar.VisibleDate.Year,calendar.SelectorStyle,calendar.Months,calendar.ShowYear,true);
					}
				}
				else
				{
					if (calendar.UseDatePicker)
					{
						WriteSelector(output, "_month_selector", 1, 12, 2, calendar.VisibleDate.Month,calendar.SelectorStyle,calendar.Months,calendar.ShowMonth,true);
						WriteSelector(output, "_year_selector", (calendar.SelectedDate.Year<calendar.MinDate.Year || calendar.VisibleDate.Year<calendar.MinDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year<calendar.MinDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MinDate.Year, (calendar.SelectedDate.Year>calendar.MaxDate.Year || calendar.VisibleDate.Year>calendar.MaxDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year>calendar.MaxDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MaxDate.Year, 4, calendar.VisibleDate.Year,calendar.SelectorStyle,calendar.Months,calendar.ShowYear,true);
					}

					else
					{
						WriteSelector(output, "_month_selector", 1, 12, 2, calendar.VisibleDate.Month,calendar.SelectorStyle,calendar.Months,calendar.ShowMonth,false);
						WriteSelector(output, "_year_selector", (calendar.SelectedDate.Year<calendar.MinDate.Year || calendar.VisibleDate.Year<calendar.MinDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year<calendar.MinDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MinDate.Year, (calendar.SelectedDate.Year>calendar.MaxDate.Year || calendar.VisibleDate.Year>calendar.MaxDate.Year) && calendar.AutoAdjust ? (calendar.SelectedDate.Year>calendar.MaxDate.Year ? calendar.SelectedDate.Year : calendar.VisibleDate.Year) : calendar.MaxDate.Year, 4, calendar.VisibleDate.Year,calendar.SelectorStyle,calendar.Months,calendar.ShowYear,false);
					}
				}
				
				output.RenderEndTag();
				if (calendar.ShowNextPrevYear)
				{
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					output.Write("&nbsp;");
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.RenderBeginTag(HtmlTextWriterTag.A);
					
					if (calendar.NextYearImage.Trim() == string.Empty)
						output.Write(calendar.NextYearText);
					else
					{
						output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
						output.AddAttribute(HtmlTextWriterAttribute.Src,Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.NextYearImage));
						output.RenderBeginTag(HtmlTextWriterTag.Img);
						output.RenderEndTag();
					}

					output.RenderEndTag();
					output.RenderEndTag();
				}
				if (calendar.ShowNextPrevMonth)
				{
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					output.Write("&nbsp;");
					calendar.NextPrevStyle.AddAttributesToRender(output);
					output.RenderBeginTag(HtmlTextWriterTag.A);
					if (calendar.NextMonthImage.Trim() == string.Empty)
						output.Write(calendar.NextMonthText);
					else
					{
						output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
						output.AddAttribute(HtmlTextWriterAttribute.Src,Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.NextMonthImage));
						output.RenderBeginTag(HtmlTextWriterTag.Img);
						output.RenderEndTag();

					}
					output.RenderEndTag();
					output.RenderEndTag();
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
				if (calendar.ShowDayHeader)
				{
					int firstDay = calendar.FirstDayOfWeek == System.Web.UI.WebControls.FirstDayOfWeek.Default ? 0 : (int)calendar.FirstDayOfWeek;
					calendar.DayHeaderStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
	
					if (calendar.ShowWeekNumber)
					{
						calendar.DayHeaderStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}%",100 /8));
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write(calendar.WeekNumberText);
						output.RenderEndTag();
					}

					for(index=0;index<7;index++)
					{
						calendar.DayHeaderStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}%",calendar.ShowWeekNumber ? 100/8 : 100/7));
						output.RenderBeginTag(HtmlTextWriterTag.Td);

						string dayFormat = calendar.Days[index+firstDay < 7 ? index+firstDay : index+firstDay-7];

						switch (calendar.DayNameFormat)
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

					if (calendar.ShowWeekNumber)
					{
						int weekNumber = 1;
						int dayForWeekNumber = 1;
						int monthForWeekNumber = 1;
						int yearForWeekNumber = calendar.VisibleDate.Year;

						// The day is in the previous month
						if (index == 0)
						{
							dayForWeekNumber = 7 - dayWeek;
							monthForWeekNumber = calendar.VisibleDate.Month;
						}
						
						else if (showDay + 7 <= dayMax && calendar.VisibleDate.Month == showMonth)
						{
							dayForWeekNumber = showDay + 7;
							monthForWeekNumber = calendar.VisibleDate.Month;
							yearForWeekNumber = calendar.VisibleDate.Year;
						}

						else if (showDay + 7 > dayMax && calendar.VisibleDate.Month == showMonth)
						{
							dayForWeekNumber = (showDay + 7) - dayMax ;
							monthForWeekNumber = calendar.VisibleDate.Month == 12 ? 1 : (calendar.VisibleDate.Month+1);
							yearForWeekNumber  = calendar.VisibleDate.Year;
						}

						else
						{
							dayForWeekNumber = showDay + 7;
							monthForWeekNumber = calendar.VisibleDate.Month == 12 ? 1 : monthForWeekNumber + 1;
							yearForWeekNumber = calendar.VisibleDate.Month == 12 ? yearForWeekNumber + 1 : yearForWeekNumber;
						}

						weekNumber = DateOperation.GetWeekNumber(new DateTime(yearForWeekNumber,monthForWeekNumber,dayForWeekNumber));
			
						calendar.WeekNumberStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write(string.Format("{0}",weekNumber.ToString()));
						output.RenderEndTag();
					}

					
					for(index2=0;index2<7;index2++)
					{
						// The day is in the Selected month
						if (day <= dayMax && !(index == 0 && index2 < dayWeek)) 
						{
							if(index2 == DateOperation.GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek.Saturday,calendar.FirstDayOfWeek) || index2 == DateOperation.GetDayPos(System.Web.UI.WebControls.FirstDayOfWeek.Sunday,calendar.FirstDayOfWeek))
							{
								calendar.WeekEndDayStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
								showColor = Utils.Color2Hex(calendar.WeekEndDayStyle.ForeColor);
							}
							else
								calendar.DayStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
							showColor = Utils.Color2Hex(calendar.DayStyle.ForeColor);
							showDay = day;
							showMonth = calendar.VisibleDate.Month;
							showYear = calendar.VisibleDate.Year;
							day++;
						}
							// The day is in the previous month
						else if (index == 0)
						{
							calendar.OtherMonthDayStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
							showColor = Utils.Color2Hex(calendar.OtherMonthDayStyle.ForeColor);
							showDay = (calendar.GetDays(calendar.VisibleDate.Month == 1 ? calendar.VisibleDate.Year-1 : calendar.VisibleDate.Year, calendar.VisibleDate.Month == 1 ? 12 : calendar.VisibleDate.Month-1) - (dayWeek - index2 - 1));
							showMonth = calendar.VisibleDate.Month == 1 ? 12 : calendar.VisibleDate.Month-1;
							showYear = calendar.VisibleDate.Month == 1 ? calendar.VisibleDate.Year-1 : calendar.VisibleDate.Year;
						}
							// The day is in the next month
						else if (index != 0)
						{
							calendar.OtherMonthDayStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
							showColor = Utils.Color2Hex(calendar.OtherMonthDayStyle.ForeColor);
							showDay = dayNext;
							showMonth = calendar.VisibleDate.Month == 12 ? 1 : (calendar.VisibleDate.Month-1+2);
							dayNext++;
							showYear = calendar.VisibleDate.Month == 12 ? calendar.VisibleDate.Year+1 : calendar.VisibleDate.Year;
						}
						
						DateTime dateToRender = new DateTime(showYear,showMonth,showDay);

						if (dateToRender >= calendar.MinDate && dateToRender <= calendar.MaxDate)
						{
							// check if this is a particular style
							bool dateIsParticularStyle = false;
							DateStyleCollectionItem dateStyle = null;
							foreach(DateStyleCollectionItem dateItem in calendar.StyleDates)
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
								if (dateStyle.BackgroundImage != string.Empty)
									output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(calendar.ImagesDirectory,dateStyle.BackgroundImage) + ")");
								showColor = Utils.Color2Hex(dateStyle.ForeColor);
							}

							else
							{

								// check if this is a blocked date
						
								bool dateIsBlocked = false;
								foreach(DateCollectionItem dateItem in calendar.BlockedDates)
								{
									if (dateItem.Date == dateToRender)
									{
										dateIsBlocked = true;
										break;
									}
								}
								if (dateIsBlocked == true)
								{
									output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(calendar.BlockedDayStyle.BackColor));
									showColor = Utils.Color2Hex(calendar.BlockedDayStyle.ForeColor);
								}

								else
								{
									bool dateIsSelected = false;
									if (calendar.MultiSelection == true)
									{
										foreach(DateCollectionItem dateItem in calendar.SelectedDates)
										{
											if (dateItem.Date == dateToRender)
											{
												dateIsSelected = true;
												output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(calendar.SelectedDayStyle.BackColor));
												if (calendar.SelectedDayStyle.BackgroundImage != string.Empty)
													output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.SelectedDayStyle.BackgroundImage) + ")");
												showColor = Utils.Color2Hex(calendar.SelectedDayStyle.ForeColor);
												break;
											}
										}
									}

									if (dateIsSelected == false)
									{
										// Check if this is the selected day or not
										if (calendar.SelectedDate.Day == showDay && showMonth == calendar.SelectedDate.Month)
										{
											output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(calendar.SelectedDayStyle.BackColor));
											if (calendar.SelectedDayStyle.BackgroundImage != string.Empty)
												output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.SelectedDayStyle.BackgroundImage) + ")");
											showColor = Utils.Color2Hex(calendar.SelectedDayStyle.ForeColor);
										}

											// check if this is the today date or not
										else if (showMonth == calendar.VisibleDate.Month && showDay == DateTime.Now.Day && calendar.VisibleDate.Month == DateTime.Now.Month && calendar.VisibleDate.Year == DateTime.Now.Year)
										{
											output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(calendar.TodayDayStyle.BackColor));
											if (calendar.TodayDayStyle.BackgroundImage != string.Empty)
												output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,"url(" + Utils.ConvertToImageDir(calendar.ImagesDirectory,calendar.TodayDayStyle.BackgroundImage) + ")");
											showColor = Utils.Color2Hex(calendar.TodayDayStyle.ForeColor);
										}
									}
								}

							}
						}
						else
						{
							output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(calendar.BlockedDayStyle.BackColor));
							showColor = Utils.Color2Hex(calendar.BlockedDayStyle.ForeColor);

						}
						
						output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write("<font color='" + showColor + "'>" + showDay.ToString() + "</font>");
						output.RenderEndTag();
					}

					output.RenderEndTag();
				}

				if (calendar.UseTime)
				{ 
					calendar.TimeStyle.AddAttributesToRender(output,calendar.ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
					output.AddAttribute(HtmlTextWriterAttribute.Colspan, string.Format("{0}%",calendar.ShowWeekNumber ? 100/8 : 100/7));
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					WriteSelector(output, "_hour", 0, 23, 2, calendar.SelectedDate.Hour,calendar.SelectorStyle,calendar.Months,true);
					WriteSelector(output, "_minute", 0, 59, 2, calendar.SelectedDate.Minute,calendar.SelectorStyle,calendar.Months,true);
					output.RenderEndTag();
					output.RenderEndTag();
				}
			
				if (calendar.ShowTodayFooter)
				{
					output.AddStyleAttribute("cursor", "hand");
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
					output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
					output.AddAttribute(HtmlTextWriterAttribute.Colspan, string.Format("{0}%",calendar.ShowWeekNumber ? 100/8 : 100/7));
					calendar.TodayFooterStyle.AddAttributesToRender(output,null,calendar.ImagesDirectory);
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					string todayFooterText = "";
					if (calendar.PrefixTodayFooter.Trim() != string.Empty)
						todayFooterText += calendar.PrefixTodayFooter;
					if (calendar.UseCustomDateFormat == false)
						todayFooterText += DateOperation.FormatedDate(DateTime.Now,calendar.DateFormatLocale);
					else
						todayFooterText += DateOperation.FormatCustomDate(DateTime.Now,calendar.CustomDateFormat,calendar.Days,calendar.Months);
					output.Write(todayFooterText);
					output.RenderEndTag();
					output.RenderEndTag();
				}
					
				// End of the calendar render
				output.RenderEndTag();
				output.Write("</div>");

			}
			else
			{
				// Check each element separated by semicolon char in the Format string.
				// If the element is the string representation of a specific date part,
				// the code render the selector. If not, the element is rendered as
				// text to the HtmlTextWriter.
				foreach(string element in calendar.Format.Split(';'))
				{
					switch (element.ToUpper())
					{
						case "DAY":
							WriteSelector(output, "_day", 1, 31, 2, calendar.SelectedDate.Day,calendar.SelectorStyle,calendar.Months,true);
							break;
						case "MONTH":
							WriteSelector(output, "_month", 1, 12, 2, calendar.SelectedDate.Month,calendar.SelectorStyle,calendar.Months,calendar.ShowMonth);
							break;
						case "YEAR":
							WriteSelector(output, "_year", calendar.SelectedDate.Year < calendar.MinDate.Year && calendar.AutoAdjust ? calendar.SelectedDate.Year : calendar.MinDate.Year, calendar.SelectedDate.Year> calendar.MaxDate.Year && calendar.AutoAdjust ? calendar.SelectedDate.Year : calendar.MaxDate.Year, 4, calendar.SelectedDate.Year,calendar.SelectorStyle,calendar.Months,calendar.ShowYear);
							break;
						case "HOUR":
							WriteSelector(output, "_hour", 0, 23, 2, calendar.SelectedDate.Hour,calendar.SelectorStyle, calendar.Months,true);
							break;
						case "MINUTE":
							WriteSelector(output, "_minute", 0, 59, 2, calendar.SelectedDate.Minute,calendar.SelectorStyle, calendar.Months,true);
							break;
						case "SECOND":
							WriteSelector(output, "_second", 0, 59, 2, calendar.SelectedDate.Second,calendar.SelectorStyle, calendar.Months,true);
							break;
						case "MILLISECOND":
							WriteSelector(output, "_millisecond", 0, 999, 3, calendar.SelectedDate.Millisecond,calendar.SelectorStyle, calendar.Months,true);
							break;
						case "MERIDIEM":
							WriteSelector(output, "_meridiem", 0, 1, 2, (calendar.SelectedDate.Hour >= 12 ? 1 : 0),calendar.SelectorStyle,calendar.Months, true);
							break;

						default:
							output.Write(element);
							break;
					}
				}
			}

			//calendar.RenderControl(output);

    		return stringWriter.ToString();
		}

		private void WriteSelector(HtmlTextWriter output, string suffix, int min, int max, int padding, int selectedValue, System.Web.UI.WebControls.Style selectorStyle, string[] months, bool show)
		{
			WriteSelector(output, suffix, min, max, padding, selectedValue, selectorStyle, months, show,true);
		}

		private void WriteSelector(HtmlTextWriter output, string suffix, int min, int max, int padding, int selectedValue, System.Web.UI.WebControls.Style selectorStyle, string[] months, bool show, bool relative)
		{
            int index;
			selectorStyle.AddAttributesToRender(output);

            if (!show)
            {
                if (relative)
                    output.AddStyleAttribute("position", "relative");
                else
                    output.AddStyleAttribute("position", "absolute");

                output.AddStyleAttribute("visibility", "hidden");
            }
			output.RenderBeginTag(HtmlTextWriterTag.Select);
			
			for(index=min;index<=max;index++)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Value, index.ToString());

				if (index == selectedValue)
					output.AddAttribute(HtmlTextWriterAttribute.Selected, null);

				output.RenderBeginTag(HtmlTextWriterTag.Option);
				if (suffix.StartsWith("_month"))
					output.InnerWriter.Write(months[index-1]);
				else if (suffix == "_meridiem")
					output.InnerWriter.Write((index == 0 ? "AM" : "PM"));
				else
					output.InnerWriter.Write(index.ToString().PadLeft(padding, '0'));
				output.RenderEndTag();
			}

			output.RenderEndTag();
		}

	}

	#endregion
}
