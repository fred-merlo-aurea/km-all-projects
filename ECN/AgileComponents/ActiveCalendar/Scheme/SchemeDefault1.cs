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
	/// Default scheme.
	/// </summary>
	internal class SchemeDefault1 : SchemePreset
	{
		/// <summary>
		/// The default constrcutor.
		/// </summary>
		public SchemeDefault1() : base()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_borderColor = Color.FromName(KnownColor.Black.ToString());

			_selectedDayStyle.BackColor = Color.FromName(KnownColor.Teal.ToString());
			_selectedDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());
			
			_todayFooterStyle.Font.Size = FontUnit.XSmall;
			_todayFooterStyle.BackgroundImage = "";
			_todayFooterStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_todayFooterStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());

			_selectorStyle.Font.Size = FontUnit.XSmall;

			_otherMonthDayStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_otherMonthDayStyle.ForeColor = Color.FromName(KnownColor.Silver.ToString());
			
			_weekNumberStyle.Font.Size = FontUnit.XSmall;
			_weekNumberStyle.BackgroundImage = "";
			_weekNumberStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_weekNumberStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());

			_nextPrevStyle.Font.Size = FontUnit.XSmall;
			_nextPrevStyle.ForeColor = Color.FromName(KnownColor.DarkRed.ToString());

			_dayHeaderStyle.Font.Size = FontUnit.XSmall;
			_dayHeaderStyle.BackgroundImage = "";
			_dayHeaderStyle.BackColor = Color.FromName(KnownColor.Black.ToString());
			_dayHeaderStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

			_blockedDayStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_blockedDayStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());
			
			_dayStyle.Font.Size = FontUnit.XSmall; 
			_dayStyle.BackgroundImage = "";
			_dayStyle.BackColor = Color.FromName(KnownColor.White.ToString());
			_dayStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());
			
			_weekendEndDayStyle.BackgroundImage = "";
			_weekendEndDayStyle.BackColor = Color.FromName(KnownColor.LightGray.ToString());
			_weekendEndDayStyle.ForeColor = Color.FromName(KnownColor.Black.ToString());

			_todayDayStyle.BackColor = Color.FromName(KnownColor.DarkRed.ToString());
			_todayDayStyle.ForeColor = Color.FromName(KnownColor.White.ToString());

		}
	}
}
