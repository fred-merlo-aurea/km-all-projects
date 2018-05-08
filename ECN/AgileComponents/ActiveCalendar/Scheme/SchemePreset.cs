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
using System.Drawing;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls.Scheme
{
	/// <summary>
	/// The base class for all the schemes.
	/// </summary>
	internal class SchemePreset
	{
		/// <summary>
		/// Next month image.
		/// </summary>
		protected string _nextMonthImage;

		/// <summary>
		/// Next year image.
		/// </summary>
		protected string _nextYearImage;

		/// <summary>
		/// Previous month image.
		/// </summary>
		protected string _prevMonthImage;

		/// <summary>
		/// Previous year image.
		/// </summary>
		protected string _prevYearImage;

		/// <summary>
		/// Border color.
		/// </summary>
		protected Color _borderColor;

		/// <summary>
		/// Blocked day style.
		/// </summary>
		protected CalendarDayStyleShort _blockedDayStyle;

		/// <summary>
		/// Header day style.
		/// </summary>
		protected CalendarDayStyleFull _dayHeaderStyle;

		/// <summary>
		/// Day style.
		/// </summary>
		protected CalendarDayStyleFull _dayStyle;

		/// <summary>
		/// Next and previous style.
		/// </summary>
		protected Style _nextPrevStyle;

		/// <summary>
		/// Other month day style.
		/// </summary>
		protected CalendarDayStyleFull _otherMonthDayStyle;

		/// <summary>
		/// Selected day style.
		/// </summary>
		protected CalendarDayStyleShort _selectedDayStyle;

		/// <summary>
		/// Selector style.
		/// </summary>
		protected Style _selectorStyle;

		/// <summary>
		/// Title style.
		/// </summary>
		protected CalendarDayStyleShort _titleStyle;

		/// <summary>
		/// Today day style.
		/// </summary>
		protected CalendarDayStyleShort _todayDayStyle;

		/// <summary>
		/// Today footer style.
		/// </summary>
		protected CalendarDayStyleFull _todayFooterStyle;

		/// <summary>
		/// Weekend style.
		/// </summary>
		protected CalendarDayStyleFull _weekendEndDayStyle;

		/// <summary>
		/// Week number style.
		/// </summary>
		protected CalendarDayStyleFull _weekNumberStyle;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public SchemePreset()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_nextMonthImage = "";
			_prevYearImage = "";
			_nextYearImage = "";
			_prevMonthImage = "";

			_borderColor = new Color();
			_blockedDayStyle = new CalendarDayStyleShort();
			_dayHeaderStyle = new CalendarDayStyleFull();
			_dayStyle = new CalendarDayStyleFull();
			_nextPrevStyle = new Style();
			_otherMonthDayStyle = new CalendarDayStyleFull();
			_selectedDayStyle = new CalendarDayStyleShort();
			_selectorStyle = new Style();
			_titleStyle = new CalendarDayStyleShort();
			_todayDayStyle = new CalendarDayStyleShort();
			_todayFooterStyle = new CalendarDayStyleFull();
			_weekendEndDayStyle = new CalendarDayStyleFull();
			_weekNumberStyle = new CalendarDayStyleFull();
		}
		
		/// <summary>
		/// Gets the next month image.
		/// </summary>
		public string NextMonthImage
		{
			get {return _nextMonthImage;}
		}

		/// <summary>
		/// Gets the next year image.
		/// </summary>
		public string NextYearImage
		{
			get {return _nextYearImage;}
		}

		/// <summary>
		/// Gets the previous image.
		/// </summary>
		public string PrevMonthImage
		{
			get {return _prevMonthImage;}
		}

		/// <summary>
		/// Gets the previous year image.
		/// </summary>
		public string PrevYearImage
		{
			get {return _prevYearImage;}
		}

		/// <summary>
		/// Gets the border color.
		/// </summary>
		public Color BorderColor
		{
			get {return _borderColor;}
		}

		/// <summary>
		/// Gets the blocked day style.
		/// </summary>
		public CalendarDayStyleShort BlockedDayStyle
		{
			get {return _blockedDayStyle;}
		}

		/// <summary>
		/// Gets the day header style.
		/// </summary>
		public CalendarDayStyleFull DayHeaderStyle
		{
			get {return _dayHeaderStyle;}
		}

		/// <summary>
		/// Gets the day style.
		/// </summary>
		public CalendarDayStyleFull DayStyle
		{
			get {return _dayStyle;}
		}

		/// <summary>
		/// Gets the next, previous style.
		/// </summary>
		public Style NextPrevStyle
		{
			get {return _nextPrevStyle;}
		}

		/// <summary>
		/// Gets the other month day style.
		/// </summary>
		public CalendarDayStyleFull OtherMonthDayStyle
		{
			get {return _otherMonthDayStyle;}
		}

		/// <summary>
		/// Gets the selected day style.
		/// </summary>
		public CalendarDayStyleShort SelectedDayStyle
		{
			get {return _selectedDayStyle;}
		}

		/// <summary>
		/// Gets the selector style.
		/// </summary>
		public Style SelectorStyle
		{
			get {return _selectorStyle;}
		}

		/// <summary>
		/// Gets the title style.
		/// </summary>
		public CalendarDayStyleShort TitleStyle
		{
			get {return _titleStyle;}
		}

		/// <summary>
		/// Gets the today style.
		/// </summary>
		public CalendarDayStyleShort TodayDayStyle
		{
			get {return _todayDayStyle;}
		}

		/// <summary>
		/// Gets the today footer style.
		/// </summary>
		public CalendarDayStyleFull TodayFooterStyle
		{
			get {return _todayFooterStyle;}
		}

		/// <summary>
		/// Gets the weekend day style.
		/// </summary>
		public CalendarDayStyleFull WeekendEndDayStyle
		{
			get {return _weekendEndDayStyle;}
		}

		/// <summary>
		/// Gets the week number style.
		/// </summary>
		public CalendarDayStyleFull WeekNumberStyle
		{
			get {return _weekNumberStyle;}
		}
	}
}
