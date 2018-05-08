using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region PanelGroup

	/// <summary>
	/// Represents a group of <see cref="Panel"/> object.
	/// </summary>
	[
	ToolboxData("<{0}:PanelGroup runat=server></{0}:PanelGroup>"),
	Designer(typeof(PanelGroupDesigner)),
	ToolboxBitmap(typeof(Panel), "ToolBoxBitmap.PanelGroup.bmp"),
	ParseChildren(true,"Panels"),
	Serializable 
	]
	public class PanelGroup : CoreWebControl
    {
		#region Fields

		/// <summary>
		/// The collection of <see cref="Panel"/>.
		/// </summary>
		private PanelCollection _panels = null;

		#endregion

		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public PanelGroup()
		{
			_panels = new PanelCollection(this.Controls);
		}

		#endregion

		#region Properties

		#region Appearance
		
		/// <summary>
		/// Gets or sets the border color around the panel group.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(Color),"#B2B3B5"),
		NotifyParentProperty(true),
		Description("Border color around the panel group.")
		]
		public new Color BorderColor
		{
			get 
			{
				object borderColor = ViewState["BorderColor"];

				if (borderColor != null)
					return (Color)borderColor;

				return Color.FromArgb(0xB2,0xB3,0xB5);
			}

			set 
			{
				base.BorderColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width around the panel group.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(Unit),"1px"),
		NotifyParentProperty(true),
		Description("Border width around the panel group.")
		]
		public new Unit BorderWidth
		{
			get
			{
				object borderWidth = ViewState["BorderWidth"];

				if (borderWidth != null)
					return (Unit)borderWidth;

				return Unit.Parse("1px");
			}

			set
			{
				ViewState["BorderWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border style around the panel group.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(BorderStyle),"solid"),
		NotifyParentProperty(true),
		Description("Border style around the panel group.")
		]
		public new System.Web.UI.WebControls.BorderStyle BorderStyle
		{
			get
			{
				object borderStyle = ViewState["BorderStyle"];

				if (borderStyle != null)
					return (System.Web.UI.WebControls.BorderStyle)borderStyle;

				return System.Web.UI.WebControls.BorderStyle.Solid;
			}

			set
			{
				ViewState["BorderStyle"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell spacing of the panel group.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The cell spacing of the panel group."),
		DefaultValue("1")
		]
		public int CellSpacing
		{
			get
			{
				object cellSpacing  = ViewState["CellSpacingGroup"];

				if (cellSpacing != null)
					return (int)cellSpacing;

 				return 1;
			}
			set
			{
				ViewState["CellSpacingGroup"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell padding of the panel group.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The cell padding of the panel group."),
		DefaultValue("1")
		]
		public int CellPadding
		{
			get
			{
				object cellPadding = ViewState["CellPaddingGroup"];

				if (cellPadding != null)
					return (int)cellPadding;

				return 1;
			}
			set
			{
				ViewState["CellPaddingGroup"] = value;
			}
		}

		#endregion

		#region Behavior

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the control render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where the API javascript file is."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.SCRIPT_DIRECTORY)
#endif
		]
		public string ScriptDirectory
		{
			get
			{
				object scriptDirectory = ViewState["ScriptDirectory"];

				if (scriptDirectory != null)
					return (string)scriptDirectory;

#if (!FX1_1)
                return string.Empty;
#else
				return Define.SCRIPT_DIRECTORY;
#endif
			}
			set
			{
				ViewState["ScriptDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the external Html TextBox API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.IMAGES_DIRECTORY)
#endif
		]
		public string ImagesDirectory
		{
			get
			{
				object imagesDirectory = ViewState["ImagesDirectory"];

				if (imagesDirectory != null)
					return (string)imagesDirectory;
#if (!FX1_1)
                return string.Empty;
#else
				return Define.SCRIPT_DIRECTORY;
#endif
			}
			set
			{
				ViewState["ImagesDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the filename of the external script file.
		/// </summary>
		/// 
		[
		Bindable(false),
		Category("Behavior"),
		Description("The filename of the external script file."),
		DefaultValue("")
		]
		public string ExternalScript
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ExternalScript), string.Empty);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ExternalScript), value);
			}
		}

		#endregion

		#region Data

		/// <summary>
		/// Gets the Panels present in the group. 
		/// </summary>
		[
		Category("Data"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty)
		]
		public PanelCollection Panels
		{
			get { return _panels;}
		}

		#endregion
	
		#endregion

		#region Methods

		#region Render

		/// <summary>
		/// Notifies the <see cref="PanelGroup"/> control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
		}

		/// <summary> 
		/// Render the <see cref="Panel"/> to the output parameter specified.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the <see cref="Panel"/>.</param>
		protected override void Render(HtmlTextWriter output)
		{

			output.Write("<div>");

			if (this.BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet)
			{
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle,this.BorderStyle.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth,this.BorderWidth.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor,Utils.Color2Hex(this.BorderColor));
			}

			if (this.BackColor != Color.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor,Utils.Color2Hex(this.BackColor));

			if (Height.Value > 0)
				output.AddAttribute(HtmlTextWriterAttribute.Height,Height.ToString());

			if (Width.Value > 0)
				output.AddAttribute(HtmlTextWriterAttribute.Width, Width.ToString());

			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,CellSpacing.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,CellPadding.ToString());

			output.RenderBeginTag(HtmlTextWriterTag.Table); // Open TABLE 1

			foreach(Panel panel in _panels)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				panel.ScriptDirectory = ScriptDirectory;
				panel.ExternalScript = ExternalScript;
				panel.ImagesDirectory = ImagesDirectory;
				panel.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			output.RenderEndTag(); // Close TABLE 1

			output.Write("</div>");
		}

		#endregion

		#endregion
	}

	#endregion
}
