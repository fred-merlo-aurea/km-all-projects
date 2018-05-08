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
using Microsoft.Win32;
using System.Text;
using ActiveUp.WebControls.Common;
//using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="PagedControl"/> object.
	/// </summary>
	[DefaultProperty("Text"), 
	ToolboxData("<{0}:PagedControl runat=server></{0}:PagedControl>")]
	[Designer(typeof(PagedControlDesigner))]
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class PagedControl : System.Web.UI.WebControls.WebControl, INamingContainer
	{
        private string _previousText, _nextText, _infoTemplate, _infoTemplateNoRecords, _cssClass,
			_panelCssClass, _navSeparator, _pageGroupLeftSeparator, _pageGroupRightSeparator, _noRecordsInfoText,
            _navigationTemplate, _navigationTemplateNoRecords;
		private int _cellPadding, _cellSpacing;
		private System.Web.UI.WebControls.Style _style, _selectorStyle;//, _infoPanelStyle, _navPanelStyle;
		private System.Web.UI.WebControls.HorizontalAlign _infoPanelHorizontalAlign, _navPanelHorizontalAlign;
		private System.Web.UI.WebControls.VerticalAlign _infoPanelVerticalAlign, _navPanelVerticalAlign;
        bool _debug, _infoPanelDisabled, _causeValidation, _pageSelectorEnabled, _navPanelDisabled;// _useQuery
#if (LICENSE)
		/// <summary>
		/// Used for the license counter.
		/// </summary>
		internal static int _useCounter;

		//private string _license = string.Empty;
#endif
	

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedControl"/> class.
		/// </summary>
		public PagedControl()
		{
			_debug = true;

			//_useQuery = false;

			_pageGroupLeftSeparator = "[";
			_pageGroupRightSeparator = "]";

			_infoPanelDisabled = false;
            _navPanelDisabled = false;

			_pageSelectorEnabled = false;

			_causeValidation = false;
			_cellPadding = 2;
			_cellSpacing = 1;
			BorderWidth = 0;

			_cssClass = string.Empty;
			_panelCssClass = string.Empty;

			_navPanelHorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;

			_previousText = "<b>Prev.</b>";
			_nextText = "<b>Next</b>";
			_navSeparator = "&nbsp;&nbsp;&nbsp;";
		}

		// Properties of the control.
		#region Properties

		/// <summary>
		/// Gets or sets the Cascading Style Sheet name of the panel where the status is displayed.
		/// </summary>
		/// <remarks>This property will affect the table line (TR) of the control that display both the info panel and navigation panel. To apply borders or other styles, use the <see cref="PagerStyle"/> property.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the Cascading Style Sheet name of the panel where the status is displayed.")]
		public string PanelCssClass
		{
			get
			{
				return _panelCssClass;
			}

			set
			{
				_panelCssClass = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the cell padding value of the table that contain the pager panels.
		/// </summary>
		[Bindable(true),
		Category("Appearance"), 
		Description("Gets or sets the cell padding value of the table that contain the pager panels.")]
		public int CellPadding
		{
			get
			{
				return _cellPadding;
			}

			set
			{
				_cellPadding = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the cell spacing value of the table that contain the page panels.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the cell spacing value of the table that contain the page panels.")] 
		public int CellSpacing
		{
			get
			{
				return _cellSpacing;
			}

			set
			{
				_cellSpacing = value;
				ChildControlsCreated = false;
			}
		}

        /// <summary>
        /// Gets or sets the navigation template of the pager.
        /// </summary>
        /// <remarks>The default value of this property is <c>Total Records: &lt;b&gt;$RECORDCOUNT$&lt;/b&gt; - Page: &lt;b&gt;$CURRENTPAGE$&lt;/b&gt; of &lt;b&gt;$PAGECOUNT$&lt;/b&gt;</c>.</remarks>
        /// <example>
        /// This property  allows your to specify exactly what you want to display in the information panel.<br></br>
        /// <br></br>
        /// The value of this property consist of a string containing both literal and tag content. To display the record count, you must include <c>$RECORDCOUNT$</c>.<br></br>
        /// <br></br>
        /// The available tags are :<br></br>
        /// <br></br>
        /// <c>$RECORDCOUNT$</c>  : display the total number of records contained in the list.<br></br>
        /// <c>$CURRENTPAGE$</c>  : display the current selected page in the list.<br></br>
        /// <c>$PAGECOUNT$</c>    : display the total number of pages contained in the list.<br></br>
        /// <c>$CURRENTINDEX$</c> : display the current index.<br></br>
        /// <br></br>
        /// <b>Example</b><br></br>
        /// <br></br>
        /// &quot;Page &lt;b&gt;$CURRENTPAGE$&lt;/b&gt;/$PAGECOUNT$. &lt;b&gt;$RECORDCOUNT$&lt;/b&gt; records found.&quot;
        /// <br></br>
        /// <i>... will display something like ...</i><br></br>
        /// <br></br>
        /// Page <b>2</b>/23. <b>237</b> records found.<br></br>
        /// </example>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the information template of the pager.")] 
		public string InfoTemplate
		{
			get
			{
				return _infoTemplate;
			}

			set
			{
				_infoTemplate = value;
				ChildControlsCreated = false;
			}
		}

        /// <summary>
        /// Gets or sets the info template used no records is available. If left empty InfoTemplate property will be used instead.
        /// </summary>
        /// <remarks>You can use the same tags as <seealso cref="InfoTemplate"/>.</remarks>
        /// <value>The info template.</value>
        [Bindable(true),
        Category("Appearance"),
        Description("Gets or sets the info template used no records is available.")]
        public string InfoTemplateNoRecords
        {
            get
            {
                return _infoTemplateNoRecords;
            }

            set
            {
                _infoTemplateNoRecords = value;
                ChildControlsCreated = false;
            }
        }

        /// <summary>
        /// Gets or sets the information template of the pager.
        /// </summary>
        /// <remarks>The default value of this property is <c>"$PREVIOUSPAGE$" + NavSeparator + "$FIRSTPAGE$ $PAGEGROUP$ $LASTPAGE$" + NavSeparator + "$NEXTPAGE$"</c>. The NavSeparator is obsolete but still used in the contructor to assure backward compatibility.</remarks>
        /// <example>
        /// This property allows your to specify exactly what you want to display in the navigation panel.<br></br>
        /// <br></br>
        /// The value of this property consist of a string containing both literal and tag content. To display the record count, you must include <c>$PREVIOUSPAGE$</c>.<br></br>
        /// <br></br>
        /// The available tags are :<br></br>
        /// <br></br>
        /// <c>$PAGESIZES$</c>  : display the page sizes selector.<br></br>
        /// <c>$PREVIOUSPAGE$</c>  : display the previous page link.<br></br>
        /// <c>$NEXTPAGE$</c>    : display the next page link.<br></br>
        /// <c>$FIRSTPAGE$</c> : display the first page link.<br></br>
        /// <c>$LASTPAGE$</c> : display the last page link.<br></br>
        /// <c>$PAGEGROUP$</c> : display the page group.<br></br>
        /// <c>$PAGESELECTOR$</c> : display the page selector (goto).<br></br>
        /// <br></br>
        /// <b>Example</b><br></br>
        /// <br></br>
        /// $PREVIOUSPAGE$ - $PAGEGROUG$ - $NEXTPAGE$
        /// <br></br>
        /// <i>... will display something like ...</i><br></br>
        /// <br></br>
        /// Prev. [1 2 3 4 5] Next<br></br>
        /// </example>
        [Bindable(true),
        Category("Appearance"),
        Description("Gets or sets the navigation template of the pager.")]
        public string NavigationTemplate
        {
            get
            {
                return _navigationTemplate;
            }

            set
            {
                _navigationTemplate = value;
                ChildControlsCreated = false;
            }
        }

        /// <summary>
        /// Gets or sets the navigation template used no records is available. If left empty NavigationTemplate property will be used instead.
        /// </summary>
        /// <remarks>You can use the same tags as <seealso cref="NavigationTemplate"/>.</remarks>
        /// <value>The navigation template.</value>
        [Bindable(true),
        Category("Appearance"),
        Description("Gets or sets the navigation template used no records is available.")]
        public string NavigationTemplateNoRecords
        {
            get
            {
                return _navigationTemplateNoRecords;
            }

            set
            {
                _navigationTemplateNoRecords = value;
                ChildControlsCreated = false;
            }
        }

		/// <summary>
		/// Gets or sets the vertical alignment of the navigation panel content.
		/// </summary>
		/// <remarks>The pager is divided in two zone. The information zone that contain the information about the pager status and the navigation zone that contain all the navigation logic.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the vertical alignment of the navigation panel content.")] 
		public System.Web.UI.WebControls.VerticalAlign NavPanelVerticalAlign
		{
			get
			{
				return _navPanelVerticalAlign;
			}

			set
			{
				_navPanelVerticalAlign = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the horizontal aligmnent of the navigation panel content.
		/// </summary>
		/// <remarks>The pager is divided in two zone. The information zone that contain the information about the pager status and the navigation zone that contain all the navigation logic.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the horizontal aligmnent of the navigation panel content.")] 
		public System.Web.UI.WebControls.HorizontalAlign NavPanelHorizontalAlign
		{
			get
			{
				return _navPanelHorizontalAlign;
			}

			set
			{
				_navPanelHorizontalAlign = value;
				ChildControlsCreated = false;
			}
		}


        /// <summary>
        /// Gets or sets a value indicating whether [nav panel disabled].
        /// </summary>
        /// <value><c>true</c> if [nav panel disabled]; otherwise, <c>false</c>.</value>
        [Bindable(true),
        Category("Appearance"),
        Description("Specify whether the navigation panel is displayed or not."),
        DefaultValue(false)]
        public bool NavPanelDisabled
        {
            get
            {
                return _navPanelDisabled;
            }
            set
            {
                _navPanelDisabled = value;
            }
        }

		/// <summary>
		/// Gets or sets the vertical aligmnent of the information panel content.
		/// </summary>
		/// <remarks>The pager is divided in two zone. The information zone that contain the information about the pager status and the navigation zone that contain all the navigation logic.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the vertical aligmnent of the information panel content.")] 
		public System.Web.UI.WebControls.VerticalAlign InfoPanelVerticalAlign
		{
			get
			{
				return _infoPanelVerticalAlign;
			}

			set
			{
				_infoPanelVerticalAlign = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the horizontal aligmnent of the information panel content.
		/// </summary>
		/// <remarks>The pager is divided in two zone. The information zone that contain the information about the pager status and the navigation zone that contain all the navigation logic.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the horizontal aligmnent of the information panel content.")] 
		public System.Web.UI.WebControls.HorizontalAlign InfoPanelHorizontalAlign
		{
			get
			{
				return _infoPanelHorizontalAlign;
			}

			set
			{
				_infoPanelHorizontalAlign = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the horizontal aligmnent of the pager.
		/// </summary>
		/// <remarks>This property work exactly like the HorizontalAlign of a Table control.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the horizontal aligmnent of the pager.")] 
		public System.Web.UI.WebControls.HorizontalAlign HorizontalAlign
		{
			get
			{
				if (ViewState["_horizontalAlign"] == null)
					ViewState["_horizontalAlign"] = System.Web.UI.WebControls.HorizontalAlign.NotSet;
				return (System.Web.UI.WebControls.HorizontalAlign)ViewState["_horizontalAlign"];
			}

			set
			{
				ViewState["_horizontalAlign"] = value;
				ChildControlsCreated = false;
			}
		}

		/*/// <summary>
		/// Gets or sets the width of the pager.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the width of the pager.")] 
		public System.Web.UI.WebControls.Unit Width
		{
			get
			{
				return _width;
			}

			set
			{
				_width = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the height of the pager.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the height of the pager.")] 
		public System.Web.UI.WebControls.Unit Height
		{
			get
			{
				return _height;
			}

			set
			{
				_height = value;
				ChildControlsCreated = false;
			}
		}*/

		/// <summary>
		/// Gets or sets the style of the pager.
		/// </summary>
		/// <remarks>If you want to apply a style for both information panel an navigation panel, using this property is the best thing to do.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the style of the pager.")] 
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[DefaultValueAttribute(null)]
		[NotifyParentPropertyAttribute(true)]
		public System.Web.UI.WebControls.Style PagerStyle
		{
			get
			{
				//if (ViewState["_style"] == null)
				//	ViewState["_style"] = new System.Web.UI.WebControls.Style();
			    if (_style == null)
					_style = new System.Web.UI.WebControls.Style();
				//return (System.Web.UI.WebControls.Style)ViewState["_style"];
				return _style;
			}

			set
			{
				//ViewState["_style"] = value;
				_style = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the style of the page selector.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the style of the page selector.")] 
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[DefaultValueAttribute(null)]
		[NotifyParentPropertyAttribute(true)]
		public System.Web.UI.WebControls.Style SelectorStyle
		{
			get
			{
				//if (ViewState["_style"] == null)
				//	ViewState["_style"] = new System.Web.UI.WebControls.Style();
				if (_selectorStyle == null)
					_selectorStyle = new System.Web.UI.WebControls.Style();
				//return (System.Web.UI.WebControls.Style)ViewState["_style"];
				return _selectorStyle;
			}

			set
			{
				//ViewState["_style"] = value;
				_selectorStyle = value;
				ChildControlsCreated = false;
			}
		}

		/*/// <summary>
		/// Gets or sets the style of the information panel.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the style of the information panel.")] 
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[DefaultValueAttribute(null)]
		[NotifyParentPropertyAttribute(true)]
		public System.Web.UI.WebControls.Style InfoPanelStyle
		{
			get
			{
				if (this._infoPanelStyle == null)
					_infoPanelStyle = new System.Web.UI.WebControls.Style();
				return _infoPanelStyle;
			}

			set
			{
				_infoPanelStyle = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the style of the information panel.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the style of the navigation panel.")] 
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[DefaultValueAttribute(null)]
		[NotifyParentPropertyAttribute(true)]
		public System.Web.UI.WebControls.Style NavPanelStyle
		{
			get
			{
				if (this._navPanelStyle == null)
					_navPanelStyle = new System.Web.UI.WebControls.Style();
				return _navPanelStyle;
			}

			set
			{
				_navPanelStyle = value;
				ChildControlsCreated = false;
			}
		}*/

		/// <summary>
		/// Specify whether the information panel is displayed or not.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Specify whether the information panel is displayed or not."),
		DefaultValue(false)] 
		public bool InfoPanelDisabled
		{
			get
			{
				return _infoPanelDisabled;
			}
			set
			{
				_infoPanelDisabled = value;
			}
		}

		/// <summary>
		/// Specify whether the page selector navigation is displayed or not.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Specify whether the page selector navigation is displayed or not."),
		DefaultValue(false)] 
		public bool PageSelectorEnabled
		{
			get
			{
				return _pageSelectorEnabled;
			}
			set
			{
				_pageSelectorEnabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to display for the previous page link.
		/// </summary>
		/// <remarks>The default value of this property is <c>&quot;&lt;b&gt;Prev.&lt;/b&gt;&quot;</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the text to display for the previous page link.")] 
		public string PreviousText
		{
			get
			{
				return _previousText;
			}

			set
			{
				_previousText = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the text to display for the next page link.
		/// </summary>
		/// <remarks>The default value of this property is <c>&quot;&lt;b&gt;Next&lt;/b&gt;&quot;</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the text to display for the next page link.")] 
		public string NextText
		{
			get
			{
				return _nextText;
			}

			set
			{
				_nextText = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the text to use as a separator between navigation elements.
		/// </summary>
		/// <remarks>The default value of this property is <c>&quot;&amp;nbsp;&amp;nbsp;&amp;nbsp;&quot;</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the text to use as a separator between navigation elements.")]
        [Obsolete("This property is obsolete. Please use the NavigationTemplate property instead.")]
		public string NavSeparator
		{
			get
			{
				return _navSeparator;
			}

			set
			{
				_navSeparator = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the text to use as a separator at the left of the page group.
		/// </summary>
		/// <remarks>The default value of this property is <c>[</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the text to use as a separator at the left of the page group.")] 
		public string PageGroupLeftSeparator
		{
			get
			{
				return _pageGroupLeftSeparator;
			}

			set
			{
				_pageGroupLeftSeparator = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the text to use as a separator at the right of the page group.
		/// </summary>
		/// <remarks>The default value of this property is <c>]</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the text to use as a separator at the right of the page group.")] 
		public string PageGroupRightSeparator
		{
			get
			{
				return _pageGroupRightSeparator;
			}

			set
			{
				_pageGroupRightSeparator = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Specify whether the link buttons cause the validation process to start when clicked.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Specify whether the link buttons cause the validation process to start when clicked.")] 
		public bool CausesValidation
		{
			get
			{
				return _causeValidation;
			}
			set
			{
				_causeValidation = value;
			}
		}
/*
#if (LICENSE)
		/// <summary>
		/// Gets or sets the license key.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("Lets you specify the license key.")
		]
		public string License
		{
			get
			{
				return _license;
			}
		
			set
			{
				_license = value;
			}
		}
#endif
*/
		#endregion

		// All the private methods.
		#region Private Methods
		internal abstract void BuildInfo(TableCell cell, bool designTime);

		internal abstract void BuildNavigation(TableCell cell);

		#endregion

		// All the .NET API related methods.
		#region DOTNET API
		/// <summary>
		/// Create the child controls.
		/// </summary>
		protected override void CreateChildControls()
		{
			//PrintStatus();

			// Initialize the structure.
			System.Web.UI.WebControls.Table pagerPanel = new System.Web.UI.WebControls.Table();
			System.Web.UI.WebControls.TableRow pagerPanelRow = new System.Web.UI.WebControls.TableRow();
			System.Web.UI.WebControls.TableCell pagerPanelInfoCell = new System.Web.UI.WebControls.TableCell();
			System.Web.UI.WebControls.TableCell pagerPanelNavigationCell = new System.Web.UI.WebControls.TableCell();

			pagerPanelRow.Cells.Add(pagerPanelInfoCell);
			pagerPanelRow.Cells.Add(pagerPanelNavigationCell);

			pagerPanel.Rows.Add(pagerPanelRow);

			// Initialize structure appearance
			pagerPanel.ApplyStyle(this.PagerStyle);
			pagerPanel.Width = this.Width;
			pagerPanel.Height = this.Height;
			pagerPanel.HorizontalAlign = this.HorizontalAlign;

			pagerPanel.CellPadding = CellPadding;
			pagerPanel.CellSpacing = CellSpacing;
			pagerPanel.BorderWidth = BorderWidth;
			pagerPanel.CssClass = CssClass;

			pagerPanelRow.CssClass = PanelCssClass;

			//this.InfoPanelStyle.MergeWith(this.PagerStyle);
			//pagerPanelInfoCell.ApplyStyle(this.InfoPanelStyle);
			pagerPanelInfoCell.ApplyStyle(this.PagerStyle);
			pagerPanelInfoCell.HorizontalAlign = InfoPanelHorizontalAlign;
			pagerPanelInfoCell.VerticalAlign = InfoPanelVerticalAlign;
			pagerPanelInfoCell.Visible = !this.InfoPanelDisabled;

			//this.NavPanelStyle.MergeWith(this.PagerStyle);
			//pagerPanelNavigationCell.ApplyStyle(this.NavPanelStyle);
			pagerPanelNavigationCell.ApplyStyle(this.PagerStyle);
			pagerPanelNavigationCell.HorizontalAlign = NavPanelHorizontalAlign;
			pagerPanelNavigationCell.VerticalAlign = NavPanelVerticalAlign;
            pagerPanelNavigationCell.Visible = !this.NavPanelDisabled;

			// Initialize the info panel
			BuildInfo(pagerPanelInfoCell, false);

			// Initialize the navigation panel
			BuildNavigation(pagerPanelNavigationCell);

			// Add the whole structure to the control collection
			this.Controls.Add(pagerPanel);
			
			//PrintStatus();
		}

		
		/// <summary>
		/// Shows debug trace messages.
		/// </summary>
		/// <param name="line">The text to add in the trace.</param>
		internal void DebugTrace(string line)
		{
			if (_debug)
				Page.Trace.Write("ActivePager", line);
		}

		/// <summary> 
		/// Render the control to the specified output.
		/// </summary>
		/// <param name="output">The HTML writer to write out to.</param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.APG, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

            //_useCounter++;

            //if (!(this.Parent is ActiveUp.WebControls.ToolbarsContainer) && /*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
            //{
            //    _useCounter = 0;
            //    output.Write(StaticContainer.TrialMessage);
            //}
            //else
            //{
				RenderPagedControl(output);
            //}

#else
			RenderPagedControl(output);
#endif
		}

		/// <summary>
		/// Renders the paged control.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderPagedControl(HtmlTextWriter output) 
		{
			EnsureChildControls();

			base.Render(output);
		}

		#endregion

		// All the delegates.
		#region Delegates
		/// <summary>
		/// Occurs when the index is changed.
		/// </summary>
		public event EventHandler IndexChanged;

		//public delegate void IndexChangedEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Call all IndexChanged event handlers if any.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected virtual void OnIndexChanged(EventArgs e)
		{
			if (IndexChanged != null)
				IndexChanged(this,e);
		}

        /// <summary>
        /// Occurs when page size is changed.
        /// </summary>
        public event EventHandler PageSizeChanged;

        /// <summary>
        /// Raises the <see cref="E:PageSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPageSizeChanged(EventArgs e)
        {
            if (PageSizeChanged != null)
                PageSizeChanged(this, e);
        }
		#endregion

	}

}
