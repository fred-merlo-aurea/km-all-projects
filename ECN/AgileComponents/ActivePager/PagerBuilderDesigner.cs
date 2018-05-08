// ActivePager
// Copyright (c) 2002 Active Up SPRL - http://www.activeup.com
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
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This class is used to render the pager at design time.
	/// </summary>
	/// <remarks>You should not use this class in your project.</remarks>
	internal class PagedControlDesigner : ControlDesigner
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public PagedControlDesigner()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Gets the design time HTML code.
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		public override string GetDesignTimeHtml()
		{
            PagedControl pagerBuilder = (PagedControl)base.Component;

			StringBuilder stringBuilder = new StringBuilder();

			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

			// Initialize the structure.
			System.Web.UI.WebControls.Table pagerPanel = new System.Web.UI.WebControls.Table();
			System.Web.UI.WebControls.TableRow pagerPanelRow = new System.Web.UI.WebControls.TableRow();
			System.Web.UI.WebControls.TableCell pagerPanelInfoCell = new System.Web.UI.WebControls.TableCell();
			System.Web.UI.WebControls.TableCell pagerPanelNavigationCell = new System.Web.UI.WebControls.TableCell();

			pagerPanelRow.Cells.Add(pagerPanelInfoCell);
			pagerPanelRow.Cells.Add(pagerPanelNavigationCell);

			pagerPanel.Rows.Add(pagerPanelRow);

			// Initialize structure appearance
			pagerPanel.ApplyStyle(pagerBuilder.PagerStyle);
			pagerPanel.Width = pagerBuilder.Width;
			pagerPanel.Height = pagerBuilder.Height;
			pagerPanel.HorizontalAlign = pagerBuilder.HorizontalAlign;

			pagerPanel.CellPadding = pagerBuilder.CellPadding;
			pagerPanel.CellSpacing = pagerBuilder.CellSpacing;

			pagerPanelRow.CssClass = pagerBuilder.PanelCssClass;

			//pagerBuilder.InfoPanelStyle.MergeWith(pagerBuilder.PagerStyle);
			//pagerPanelInfoCell.ApplyStyle(pagerBuilder.InfoPanelStyle);
			pagerPanelInfoCell.ApplyStyle(pagerBuilder.PagerStyle);
			pagerPanelInfoCell.HorizontalAlign = pagerBuilder.InfoPanelHorizontalAlign;
			pagerPanelInfoCell.VerticalAlign = pagerBuilder.InfoPanelVerticalAlign;
			pagerPanelInfoCell.Visible = !pagerBuilder.InfoPanelDisabled;

			//pagerBuilder.NavPanelStyle.MergeWith(pagerBuilder.PagerStyle);
			//pagerPanelNavigationCell.ApplyStyle(pagerBuilder.NavPanelStyle);
			pagerPanelNavigationCell.ApplyStyle(pagerBuilder.PagerStyle);
			pagerPanelNavigationCell.HorizontalAlign = pagerBuilder.NavPanelHorizontalAlign;
			pagerPanelNavigationCell.VerticalAlign = pagerBuilder.NavPanelVerticalAlign;

			// Initialize the info panel
			pagerBuilder.BuildInfo(pagerPanelInfoCell, true);

			// Initialize the navigation panel
			pagerBuilder.BuildNavigation(pagerPanelNavigationCell);

			// Add the whole structure to the control collection
			pagerPanel.RenderControl(writer);

			return stringWriter.ToString();
		}

		/// <summary>
		/// Gets the design time HTML code when empty (never used in ActivePager).
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		protected override string GetEmptyDesignTimeHtml() 
		{
			string text;
			text = "this should be never displayed.";
			return CreatePlaceHolderDesignTimeHtml(text);
		}

        protected override string GetErrorDesignTimeHtml(System.Exception e)
        {
            string text = string.Format("There was an error and the Pager control can't be displayed<br>Exception : {0}", e.Message);
            return this.CreatePlaceHolderDesignTimeHtml(text);
        }
	}
}
