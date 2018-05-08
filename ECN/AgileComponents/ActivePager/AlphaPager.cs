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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using Microsoft.Win32;
using System.Text;
using System.Drawing;
//using ActiveUp.WebControls.Common;
using System.Text.RegularExpressions;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="AlphaPager"/> object.
	/// </summary>
	[DefaultProperty("Text"), 
	ToolboxData("<{0}:AlphaPager runat=server></{0}:AlphaPager>")]
	[ToolboxBitmap(typeof(AlphaPager), "ToolBoxBitmap.Pager.bmp")]
	//[Designer(typeof(PagerBuilderDesigner))]
	[ParseChildren(true)]
	[PersistChildren(false)]
	public class AlphaPager : PagedControl, INamingContainer
	{
		private PagerElementCollection _pagerElements;

		/// <summary>
		/// Initializes a new instance of the <see cref="AlphaPager"/> class.
		/// </summary>
		public AlphaPager()
		{
			CurrentPage = 0;
			
			PagerElements.Add("", "*");

			int index = 65;
			for(index=65;index<91;index++)
				PagerElements.Add(Convert.ToString((char)index));
		}

		#region Properties
		/// <summary>
		/// Gets or sets the pager elements.
		/// </summary>
		/// <value>The pager elements.</value>
		public PagerElementCollection PagerElements
		{
			get
			{
				if (_pagerElements == null)
					_pagerElements = new PagerElementCollection();
				return _pagerElements;
			}
			set
			{
				_pagerElements = value;
			}
		}

		/// <summary>
		/// Gets or sets the pager filters.
		/// </summary>
		/// <value>The pager filters.</value>
		public PagerElementCollection PagerFilters
		{
			get
			{
				if (ViewState["_pagerFilters"] == null)
					ViewState["_pagerFilters"] = new PagerElementCollection();
				return (PagerElementCollection)ViewState["_pagerFilters"];
			}
			set
			{
				ViewState["_pagerFilters"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the current index.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the current page.")] 
		public int CurrentPage
		{
			get
			{
				EnsureChildControls();
				return (int)ViewState["_currentPage"];
			}

			set
			{
				ViewState["_currentPage"] = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the current filter.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the current filter.")] 
		public PagerElement CurrentFilter
		{
			get
			{
				EnsureChildControls();
				return (PagerElement)ViewState["_currentFilter"];
			}

			set
			{
				ViewState["_currentFilter"] = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the current index.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the current page.")] 
		public string CurrentKey
		{
			get
			{
				EnsureChildControls();
				return this.PagerElements[this.CurrentPage].Key;
			}
		}

		/// <summary>
		/// Gets or sets the current index.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the current page.")] 
		public string CurrentLabel
		{
			get
			{
				EnsureChildControls();
				return this.PagerElements[this.CurrentPage].Label;
			}
		}

		/// <summary>
		/// Executed when the previous page link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnPreviousPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnPreviousPage_Click");

			CurrentPage -= 1;
			if (CurrentPage < 0)
				CurrentPage = 0;

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when the next page link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnNextPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnNext_Click");

			if (CurrentPage + 1 < this.PagerElements.Count)
				CurrentPage = CurrentPage + 1;

			this.OnIndexChanged(new EventArgs());
            
		}

		/// <summary>
		/// Executed when a specific page is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnGotoPage_Click(object sender, System.EventArgs e)
		{
			string requestedPage = "";
				
			if (sender.GetType().FullName == "System.Web.UI.WebControls.LinkButton")
				requestedPage = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;
			else if (sender.GetType().FullName == "System.Web.UI.WebControls.DropDownList")
				requestedPage = ((System.Web.UI.WebControls.DropDownList)sender).SelectedItem.Value;
			
			CurrentPage = this.PagerElements.GetIndexFromKey(requestedPage);
			
			this.OnIndexChanged(new EventArgs());
		}

		private void OnFilter_Select(object sender, System.EventArgs e)
		{
			string requestedKey = "";
				
			if (sender.GetType().FullName == "System.Web.UI.WebControls.DropDownList")
				requestedKey = ((System.Web.UI.WebControls.DropDownList)sender).SelectedItem.Value;
			
			if (requestedKey != string.Empty)
				CurrentFilter = this.PagerFilters[requestedKey];

			this.OnFilterChanged(new EventArgs());
		}
		#endregion

		// All the private methods.
		#region Private Methods
		/// <summary>
		/// Builds the information panel content.
		/// </summary>
		/// <param name="cell">The table cell.</param>
		/// <param name="designTime">Specify if we need the design time string.</param>
		internal override void BuildInfo(TableCell cell, bool designTime)
		{
			/*string info = InfoTemplate;

			System.Web.UI.WebControls.Label infoText = new System.Web.UI.WebControls.Label();
			infoText.Text = info;*/

			if (this.PagerFilters.Count > 0)
			{
				DropDownList _dropDownList = new DropDownList();
				_dropDownList.SelectedIndexChanged += new EventHandler(OnFilter_Select);
				_dropDownList.DataSource = this.PagerFilters;
				_dropDownList.DataTextField = "Label";
				_dropDownList.DataValueField = "Key";
				_dropDownList.DataBind();
				_dropDownList.ID = "_pageFilters";
				_dropDownList.AutoPostBack = true;
				_dropDownList.ApplyStyle(this.SelectorStyle);

				if (this.CurrentFilter != null)
					_dropDownList.Items.FindByValue(this.CurrentFilter.Key).Selected = true;

				cell.Controls.Add(_dropDownList);
			}
		}

		internal override void BuildNavigation(TableCell cell)
		{
			int index=0;

			// Previous
			System.Web.UI.WebControls.LinkButton previous = new System.Web.UI.WebControls.LinkButton();
			if (this.CurrentPage == 0)
				previous.Enabled = false;
			previous.CausesValidation = this.CausesValidation;
			previous.Click += new EventHandler(OnPreviousPage_Click);
			previous.Text = PreviousText;
			cell.Controls.Add(previous);

			cell.Controls.Add(new LiteralControl(NavSeparator));
			
			if (this.PageSelectorEnabled)
			{
				System.Web.UI.WebControls.DropDownList pageSelector = new System.Web.UI.WebControls.DropDownList();

				pageSelector.ApplyStyle(this.SelectorStyle);

				for(index=0;index<this.PagerElements.Count;index++)
				{
					ListItem item = new ListItem(this.PagerElements[index].Label, this.PagerElements[index].Key);
					if (index==this.CurrentPage)
						item.Selected = true;

					pageSelector.Items.Add(item);
				}

				pageSelector.AutoPostBack = true;
				pageSelector.SelectedIndexChanged += new EventHandler(OnGotoPage_Click);
				cell.Controls.Add(pageSelector);

				cell.Controls.Add(new LiteralControl(NavSeparator));
			}

			// Group
			if (this.PagerElements.Count != 0)
				cell.Controls.Add(new LiteralControl(PageGroupLeftSeparator));

			for(index=0;index<this.PagerElements.Count;index++)
			{

				if (index != CurrentPage)
				{
					System.Web.UI.WebControls.LinkButton gotoPage = new System.Web.UI.WebControls.LinkButton();
					gotoPage.CausesValidation = CausesValidation;
					gotoPage.Click += new EventHandler(OnGotoPage_Click);
					gotoPage.Text = this.PagerElements[index].Label;
					gotoPage.CommandArgument = this.PagerElements[index].Key;
					cell.Controls.Add(gotoPage);
				}
				else
					cell.Controls.Add(new LiteralControl("<b>" + this.PagerElements[index].Label + "</b>"));

				if (index != this.PagerElements.Count - 1)
					cell.Controls.Add(new LiteralControl("&nbsp;"));
			}


			if (this.PagerElements.Count != 0)
				cell.Controls.Add(new LiteralControl(PageGroupRightSeparator));

			cell.Controls.Add(new LiteralControl(NavSeparator));

			// Next
			System.Web.UI.WebControls.LinkButton next = new System.Web.UI.WebControls.LinkButton();
			if (CurrentPage >= this.PagerElements.Count-1)
				next.Enabled = false;
			next.CausesValidation = CausesValidation;
			next.Click += new EventHandler(OnNextPage_Click);
			next.Text = NextText;
			cell.Controls.Add(next);
		}
		#endregion

		// All the .NET API related methods.
		#region DOTNET API
		#endregion

		// All the delegates.
		#region Delegates
		/// <summary>
		/// Occurs when the index is changed.
		/// </summary>
		public event EventHandler FilterChanged;

		/// <summary>
		/// Call all IndexChanged event handlers if any.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected virtual void OnFilterChanged(EventArgs e)
		{
			if (FilterChanged != null)
				FilterChanged(this,e);
		}
		#endregion

        protected override void Render(HtmlTextWriter output)
        {            
            StringBuilder stringBuilder = new StringBuilder();
            HtmlTextWriter writer = new HtmlTextWriter(new System.IO.StringWriter(stringBuilder));
                       
            base.Render(writer);
                        
            string newOutput = string.Empty;
            string previousOutput = stringBuilder.ToString();

            newOutput = previousOutput.Replace("span", "div");
            newOutput = Regex.Replace(newOutput, "<table.*?>", "");
            newOutput = Regex.Replace(newOutput, "<tr.*?>", "");
            newOutput = Regex.Replace(newOutput, "<td.*?>", "");
            newOutput = Regex.Replace(newOutput, "</table>", "");
            newOutput = Regex.Replace(newOutput, "</tr>", "");
            newOutput = Regex.Replace(newOutput, "</td>", "");

            Match disabled = Regex.Match(newOutput, "disabled=");

            if (disabled != null)
            {
                Match match = Regex.Match(newOutput, "<a disabled=.*?/a>");
                string replacement = match.ToString().Replace("<a", "<span");
                replacement = replacement.Replace("/a>", "/span>");
                replacement = replacement.Replace("disabled=", "class=");
                newOutput = Regex.Replace(newOutput, "<a disabled=.*?/a>", replacement);               
            }
          
            Match m = Regex.Match(newOutput, "style=\".*?\"");
            string style = m.Groups[0].ToString();

            if(style.Equals("style=\"display:inline-block;border-width:0px;\""))
                style = string.Empty;
            else
                style = style.Replace("display:inline-block;border-width:0px;", string.Empty);

            newOutput = Regex.Replace(newOutput, "style=\".*?\"", style);

            output.Write(newOutput);          
            
        }        
	}
}
