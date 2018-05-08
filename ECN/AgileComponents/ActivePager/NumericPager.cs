using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Win32;
using System.Text;
using System.Drawing;
//using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="NumericPager"/> object.
	/// </summary>
	[
	DefaultProperty("Text"), 
	ToolboxData("<{0}:NumericPager runat=server></{0}:NumericPager>"),
	ToolboxBitmap(typeof(NumericPager), "ToolBoxBitmap.Pager.bmp")
	]
	public class NumericPager : PagerBuilder
	{
		
	}
}
