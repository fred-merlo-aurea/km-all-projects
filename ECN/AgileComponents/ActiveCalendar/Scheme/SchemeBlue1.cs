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
	/// Scheme Blue1
	/// </summary>
	internal class SchemeBlue1 : SchemePreset
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SchemeBlue1() : base()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_prevMonthImage = "ArrowBlueL1.gif";
			_prevYearImage = "ArrowBlueL1.gif";
			_nextMonthImage = "ArrowBlueR1.gif";
			_nextYearImage = "ArrowBlueR1.gif";

			_borderColor = Color.FromName(KnownColor.LightSteelBlue.ToString());

			_selectedDayStyle.BackColor = Color.FromName(KnownColor.SteelBlue.ToString());
			_selectedDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_todayFooterStyle.Font.Size = FontUnit.XSmall;
			_todayFooterStyle.BackgroundImage = "blue1.gif";
			_todayFooterStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);
			_todayFooterStyle.ForeColor = Color.FromName(KnownColor.SteelBlue.ToString());

			_selectorStyle.Font.Size = FontUnit.XSmall;
			_selectorStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);
			_selectorStyle.ForeColor = Color.FromName(KnownColor.SteelBlue.ToString());

			_otherMonthDayStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);
			_otherMonthDayStyle.ForeColor = Color.FromName(KnownColor.Silver.ToString());

			_titleStyle.BackgroundImage = "blue1.gif";
			_titleStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);

			_weekNumberStyle.Font.Size = FontUnit.XSmall;
			_weekNumberStyle.BackgroundImage = "";
			_weekNumberStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_weekNumberStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());

			_nextPrevStyle.Font.Size = FontUnit.XSmall;
			_nextPrevStyle.ForeColor = Color.FromName(KnownColor.DarkRed.ToString());

			_dayHeaderStyle.Font.Size = FontUnit.XSmall;
			_dayHeaderStyle.BackgroundImage = "blue2.gif";
			_dayHeaderStyle.BackColor = Color.FromName(KnownColor.LightSteelBlue.ToString());
			_dayHeaderStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_blockedDayStyle.BackColor = Color.FromName(KnownColor.Black.ToString());
			_blockedDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_dayStyle.Font.Size = FontUnit.XSmall;
			_dayStyle.BackgroundImage = "";
			_dayStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);
			_dayStyle.ForeColor = Color.FromName(KnownColor.SteelBlue.ToString());

			_weekendEndDayStyle.BackgroundImage = "";
			_weekendEndDayStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);
			_weekendEndDayStyle.ForeColor = Color.FromName(KnownColor.SteelBlue.ToString());

			_weekNumberStyle.BackgroundImage = "";
			_weekNumberStyle.BackColor = Color.FromArgb(0xCC,0xDD,0xFF);
			_weekNumberStyle.ForeColor = Color.FromName(KnownColor.SteelBlue.ToString());
			_weekNumberStyle.Font.Italic = true;

			_todayDayStyle.BackColor = Color.FromName(KnownColor.LightSteelBlue.ToString());
			_todayDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());
			
		}
	}
}
