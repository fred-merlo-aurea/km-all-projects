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
	/// Scheme Orange1
	/// </summary>
	internal class SchemeOrange1 : SchemePreset
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SchemeOrange1() : base()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_prevMonthImage = "ArrowBrownL1.gif";
			_prevYearImage = "ArrowBrownL1.gif";
			_nextMonthImage = "ArrowBrownR1.gif";
			_nextYearImage = "ArrowBrownR1.gif";

			_borderColor = Color.FromArgb(0xFF,0xC0,0x80);

			_selectedDayStyle.BackColor = Color.FromName(KnownColor.SandyBrown.ToString());
			_selectedDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_todayFooterStyle.Font.Size = FontUnit.XSmall;
			_todayFooterStyle.BackgroundImage = "orange1.gif";
			_todayFooterStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());
			_todayFooterStyle.ForeColor = Color.FromName(KnownColor.SandyBrown.ToString());

			_selectorStyle.Font.Size = FontUnit.XSmall;
			_selectorStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());
			_selectorStyle.ForeColor = Color.FromName(KnownColor.SandyBrown.ToString());

			_otherMonthDayStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());
			_otherMonthDayStyle.ForeColor = Color.FromName(KnownColor.Silver.ToString());

			_titleStyle.BackgroundImage = "orange1.gif";
			_titleStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());

			_weekNumberStyle.Font.Size = FontUnit.XSmall;
			_weekNumberStyle.BackgroundImage = "";
			_weekNumberStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_weekNumberStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());

			_nextPrevStyle.Font.Size = FontUnit.XSmall;
			_nextPrevStyle.ForeColor = Color.FromName(KnownColor.DarkRed.ToString());

			_dayHeaderStyle.Font.Size = FontUnit.XSmall;
			_dayHeaderStyle.BackgroundImage = "orange2.gif";
			_dayHeaderStyle.BackColor = Color.FromArgb(0xFF,0xC0,0x80);
			_dayHeaderStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_blockedDayStyle.BackColor = Color.FromName(KnownColor.Black.ToString());
			_blockedDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_dayStyle.Font.Size = FontUnit.XSmall;
			_dayStyle.BackgroundImage = "";
			_dayStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());
			_dayStyle.ForeColor = Color.FromName(KnownColor.SandyBrown.ToString());

			_weekendEndDayStyle.BackgroundImage = "";
			_weekendEndDayStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());
			_weekendEndDayStyle.ForeColor = Color.FromName(KnownColor.SandyBrown.ToString());

			_weekNumberStyle.BackgroundImage = "";
			_weekNumberStyle.BackColor = Color.FromName(KnownColor.PapayaWhip.ToString());
			_weekNumberStyle.ForeColor = Color.FromName(KnownColor.SandyBrown.ToString());
			_weekNumberStyle.Font.Italic = true;

			_todayDayStyle.BackColor = Color.FromName(KnownColor.Wheat.ToString());
			_todayDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());
		}
	}
}
