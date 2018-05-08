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
	/// ASP.NET control to input strictly formatted numerics.
	/// </summary>
    [ToolboxBitmap(typeof(NumericTextBox), "ToolBoxBitmap.Numeric.bmp")]
	public class NumericTextBox : ExtendedTextBox
	{
		private const string DelimiterSymbol = ",";

		/// <summary>
		/// OnPreRender Call
		/// </summary>
		/// <param name="e">EventArgs</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (this.NumericType != NumericValues.NotSet)
			{
				switch (this.NumericType)
				{
					case NumericValues.Byte:
						this.MinValue = "0";
						this.MaxValue = "255";
						this.Precision = 0;
						break;

					case NumericValues.Int16:
						this.MinValue = "-32768";
						this.MaxValue = "32767";
						this.Precision = 0;
						break;

					case NumericValues.Int32:
						this.MinValue = "-2147483648";
						this.MaxValue = "2147483647";
						this.Precision = 0;
						break;

					case NumericValues.Int64:
						this.MinValue = "-9223372036854775808";
						this.MaxValue = "9223372036854775807";
						this.Precision = 0;
						break;

				}
			}
		}

		/// <summary>
		/// Add attributes to the tag.
		/// </summary>
		/// <param name="writer">The current HtmlTextWriter object.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.TabIndex != 0)
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString(NumberFormatInfo.InvariantInfo));
			if (this.ReadOnly == false) 
			{
				writer.AddAttribute("onFocus", string.Format("return AIP_numeralsBefore(this, '{0}');", this.CurrencySymbol));
				writer.AddAttribute("onBlur", string.Format("return AIP_numeralsAfter(this,'{4}',{0},{1},{2},'{3}');", this.MinValue.ToString(), this.MaxValue.ToString(), this.Precision.ToString(), this.Delimiter, this.CurrencySymbol));
				writer.AddAttribute("onKeyPress", string.Format("return AIP_numeralsOnly(event,this,{0},{1},{2},'{3}');", this.MinValue.ToString(), this.MaxValue.ToString(), this.Precision.ToString(), this.Delimiter));
			}
		}

		/// <summary>
		/// Gets or sets the minimum value allowed.
		/// </summary>
		/// <remarks>Because the control allows the developer to specify multiple types of numerics, you need to specify the value as it's string representation.</remarks>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The minimum value allowed.")]
		public string MinValue
		{
			get
			{
				object obj1 = this.ViewState["MinValue"];
				if (obj1 != null)
				{
					return (string)obj1;
				}
				return "-2147483648";
			}
			set
			{
				this.ViewState["MinValue"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value allowed.
		/// </summary>
		/// <remarks>Because the control allows the developer to specify multiple types of numerics, you need to specify the value as it's string representation.</remarks>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The maximum value allowed.")]
		public string MaxValue
		{
			get
			{
				object obj1 = this.ViewState["MaxValue"];
				if (obj1 != null)
				{
					return (string)obj1;
				}
				return "2147483647";
			}
			set
			{
				this.ViewState["MaxValue"] = value;
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
					return (int) obj1;
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
		/// Gets or sets the currency symbol or code.
		/// </summary>
		/// <remarks>If you want to enable the currency auto format, just specify a symbol like $ or a code like USD in this property.</remarks>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The currency symbol or code.")]
		public string CurrencySymbol
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(CurrencySymbol), string.Empty);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(CurrencySymbol), value);
			}
		}
		
		/// <summary>
		/// Gets or sets the numeric CLR type.
		/// </summary>
		/// <remarks>This property can be used to set automatically the maximum and minimum values depending of the type. For exemple if you need to have an Int32, you will have to specify NumericType.Int32 and the control will automatically set -2147483648 as the minimum value and 2147483647 as the maximum value. Please note that this features is only available for byte and int data types. For Double and Decimal, you have to enter the maximum and minimum value manually using the string representation of the data.</remarks>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("The numeric CLR type.")]
		public NumericValues NumericType
		{
			get
			{
				object obj1 = this.ViewState["NumericType"];
				if (obj1 != null)
				{
					return (NumericValues) obj1;
				}
				return NumericValues.NotSet;
			}
			set
			{
				this.ViewState["NumericType"] = value;
			}
		}
	}
}
