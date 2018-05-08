using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using ActiveUp.WebControls;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolMultiCodeSnippets"/> object.
	/// </summary>
	[
		ToolboxItem(false),
		ParseChildren(true, "Items"),
		Serializable
	]
	public class ToolMultiCodeSnippets : ToolButton
	{
		#region Fields
        
		private ToolItemCollection _items = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolMultiCodeSnippets"/> class.
		/// </summary>
		public ToolMultiCodeSnippets() : base()
		{
			_Init(string.Empty);
		}
 
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolMultiCodeSnippets"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolMultiCodeSnippets(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
			{
				this.ID = "_toolMultiCodeSnippets" + Editor.indexTools++;
				this.PopupContents.ID = ID + "MultiCodeSnippetsPopup";
			}
			else
			{ 
				this.ID = id;
				this.PopupContents.ID = ID + "MultiCodeSnippetsPopup";
			}

#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ToolTip = "Multi code snippets";
			this.ImageURL = "codesnippets_off.gif";
#endif
			this.OverImageURL = "codesnippets_over.gif";
			this.UsePopupOnClick = true;

            
			this.PopupContents.TitleText = "Codes";
			/*this.PopupContents.Width = Unit.Parse("400px");
			this.PopupContents.Height = Unit.Parse("200px");*/

			this.PopupContents.AutoContent = true;

		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the number of columns.
		/// </summary>
		/// <value>The number of columns.</value>
		[
			DefaultValue(3)
		]
		public int NumberOfColumns
		{
			get
			{
				object numberOfColumns = ViewState["numberOfColumns"];
				if (numberOfColumns != null)
				{
					return (int)ViewState["numberOfColumns"];
				}
				else
					return 3;
			}
			set
			{
				ViewState["numberOfColumns"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether use button instead of link.
		/// </summary>
		/// <value><c>true</c> if use button instead of link; otherwise, <c>false</c>.</value>
		[
			DefaultValue(false)
		]
		public bool UseButton
		{
			get
			{
				object useButton = ViewState["useAsButton"];
				if (useButton != null)
				{
					return (bool)useButton;
				}
				else
					return false;
			}
			set
			{
				ViewState["useAsButton"] = value;
			}
		}

		/// <summary>
		/// Gets the code snippets.
		/// </summary>
		/// <value>The code snippets.</value>
		[
		DefaultValue(null),
		MergableProperty(false),
		PersistenceMode(PersistenceMode.InnerDefaultProperty),
		Description("Items of the contol.")
		]
		public ToolItemCollection CodeSnippets 
		{
			get 
			{
				if (_items == null) 
				{
					_items = new ToolItemCollection();
					if (IsTrackingViewState) 
					{
						((IStateManager)_items).TrackViewState();
					}
				}
				return _items;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.codesnippets_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.codesnippets_over.gif");
#endif

            this.RenderControl(output);
        }

		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
#if (!FX1_1)
            if (ImageURL == string.Empty)
                this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.codesnippets_off.gif");
            if (OverImageURL == string.Empty)
                this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.codesnippets_over.gif");
#endif

			base.OnPreRender(e);

			if (System.Web.HttpContext.Current != null)
			{

                string contents = string.Empty;
				Editor editor = (Editor)this.Parent.Parent.Parent;

				if (this.CodeSnippets.Count > 0)
				{
					contents += "<table>";
					contents += "<tr>";

					int i = 0;
					foreach(ToolItem item in CodeSnippets)
					{
						contents += "<td style='cursor:hand' nowrap>";
						contents += "<span>";

						string snippet = string.Format("HTB_SetSnippet('{0}', '{1}');",editor.ClientID,item.Value);

						if (this.UseButton)
						{
							contents += string.Format("<input type=button value={0} onclick=\\\"{1}\\\">",item.Text,snippet);
						}
	
						else
						{
							contents += string.Format("<a onclick=\\\"{1}\\\">{0}</a>",item.Text,snippet);
						}

						
						contents += "</span>";
						contents += "</td>"; 

						i++;

						if (i == NumberOfColumns)
						{
							i = 0;

							contents += "</tr>";
							contents += "<tr>";
						}
					}

					contents += "</tr>";
					contents += "</table>";
				}

				this.PopupContents.ContentText = contents;
				this.ClientSideClick = string.Format("HTB_SetPopupPosition('{0}','{1}');",editor.ClientID,this.PopupContents.ClientID);
			}
		}

		#endregion

		#region ViewState

		/// <summary>
		/// Loads the view state.
		/// </summary>
		/// <param name="savedState">The saved view state..</param>
		protected override void LoadViewState(object savedState) 
		{
			base.LoadViewState(savedState, (state) => ((IStateManager)CodeSnippets).LoadViewState(state));
		}

		/// <summary>
		/// Saves the view state.
		/// </summary>
		/// <returns></returns>
		protected override object SaveViewState() 
		{
			return base.SaveViewState(_items);
		}

		/// <summary>
		/// Tracks the view state.
		/// </summary>
		protected override void TrackViewState() 
		{
			base.TrackViewState(_items);
		}

		#endregion
	}
}