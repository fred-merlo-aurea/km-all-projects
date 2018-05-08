// ActiveInput
// Copyright (c) 2005 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Masked textbox control to allows strictly formatted data input with real time previewing.
	/// </summary>
    [ToolboxBitmap(typeof(MaskedTextBox), "ToolBoxBitmap.Masked.bmp")]
	public class MaskedTextBox : ExtendedTextBox
	{
		private const string DelimiterSymbol = ",";
		private const string PercentageSymbol = "%";
		private const string DollarSymbol = "$";

		/// <summary>
		/// Adds attributes to the control tag.
		/// </summary>
		/// <param name="writer">The current HtmlTextWriter object.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.TabIndex != 0)
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString(NumberFormatInfo.InvariantInfo));
			writer.AddAttribute("onFocus", "return AIP_controlFocus(this);");
			writer.AddAttribute("onKeyDown", "return AIP_pressing(event,this);");

			switch (Mode)
			{
				case InputMode.NotSet:
					if (this.Mask != string.Empty)
						writer.AddAttribute("mask", this.Mask);
					writer.AddAttribute("modeInput", "none");
					break;
				case InputMode.Mask:
					writer.AddAttribute("customFormat", this.Mask);
					writer.AddAttribute("modeInput", "mask");
					break;
				case InputMode.Alpha:
					if (this.Mask != string.Empty)
						writer.AddAttribute("mask", this.Mask);
					writer.AddAttribute("modeInput", "character");
					break;
				case InputMode.Currency:
					writer.AddAttribute("modeInput", "currency");
					writer.AddAttribute("presision", this.Precision.ToString());
					writer.AddAttribute("delimiter", this.Delimiter);
					writer.AddAttribute("specialSymbol", this.Symbol.ToString());
					break;
				case InputMode.Date:
					writer.AddAttribute("modeInput", "datetime");
					writer.AddAttribute("format", this.DateFormat);
					break;
				case InputMode.Numeric:
					writer.AddAttribute("modeInput", "numeric");
					if (this.Mask != string.Empty)
						writer.AddAttribute("mask", this.Mask);
					else
					{
						writer.AddAttribute("presision", this.Precision.ToString());
						writer.AddAttribute("delimiter", this.Delimiter);
					}
					break;
				case InputMode.Percent:
					writer.AddAttribute("modeInput", "percent");
					writer.AddAttribute("presision", this.Precision.ToString());
					writer.AddAttribute("delimiter", this.Delimiter);
					writer.AddAttribute("specialSymbol", this.Symbol);
					break;
			}
		
		}

		/// <summary>
		/// Gets or sets the input mode of the textbox.
		/// </summary>
		/// <remarks>Please refer to <see cref="InputMode"/> enumeration for details.</remarks>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("the input mode of the textbox.")]
		public InputMode Mode
		{
			get
			{
				object obj1 = this.ViewState["Mode"];
				if (obj1 != null)
				{
					return (InputMode)obj1;
				}
				return InputMode.NotSet;
			}
			set
			{
				this.ViewState["Mode"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the precision of the decimals.
		/// </summary>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The precision of the decimals.")]
		public int Precision
		{
			get
			{
				object obj1 = this.ViewState["Precision"];
				if (obj1 != null)
				{
					return (int)obj1;
				}
				return 0;
			}
			set
			{
				this.ViewState["Precision"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the decimal delimiter.
		/// </summary>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The decimal delimited.")]
		public string Delimiter
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(Delimiter), DelimiterSymbol);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(Delimiter), value);
			}
		}

		/// <summary>
		/// Gets or sets the symbol to use for the percent and currency input modes.
		/// </summary>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The symbol to use for the percent and currency input modes.")]
		public string Symbol
		{
			get
			{
				var defaultValue = (this.Mode == InputMode.Percent) ? 
					PercentageSymbol : 
					DollarSymbol;
				return ViewStateHelper.GetFromViewState(ViewState, nameof(Symbol), defaultValue);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(Symbol), value);
			}
		}

		/// <summary>
		/// Gets or sets the mask to apply to the input.
		/// </summary>
		[Category("Behavior"), Bindable(true), DefaultValue(false)]
		public string Mask
		{
			get
			{
				object obj1 = this.ViewState["Mask"];
				if (obj1 != null)
				{
					return (string)obj1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Mask"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the date format to use in Date mode.
		/// </summary>
		/// <remarks>Use the following strings to specify the date parts : DD for day, MM for month and YYYY for year.</remarks>
		[Category("Behavior"), Bindable(true), DefaultValue(false)]
		public string DateFormat
		{
			get
			{
				object obj1 = this.ViewState["DateFormat"];
				if (obj1 != null)
				{
					return (string)obj1;
				}
				return "MM/DD/YYYY";
			}
			set
			{
				this.ViewState["DateFormat"] = value;
			}
		}
	}
}
