using System;
using System.Drawing;
using System.Collections.Specialized;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
	#region class ToolTextBox

	/// <summary>
	/// Represents a <see cref="ToolTextBox"/> object.
	/// </summary>
	[
		DefaultEventAttribute("TextChanged"),
		ControlBuilderAttribute(typeof(System.Web.UI.WebControls.TextBoxControlBuilder)),
		DefaultPropertyAttribute("Text"),
		ValidationPropertyAttribute("Text"),
		ParseChildrenAttribute(false),
		Serializable,
        ToolboxItem(false)
	]
	public class ToolTextBox : ToolBase, IPostBackDataHandler
	{
		#region Variables

		/// <summary>
		/// Event occurs when the text value change.
		/// </summary>
		private readonly static object EventTextChanged = new Object();

		#endregion

		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public ToolTextBox()
		{
			BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
			BorderWidth = 1;
			BorderColor = Color.FromArgb(0xB4,0xB1,0xA3);
			BorderColorRollOver = Color.FromArgb(0x31,0x6A,0xC5);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value that indicates whether or not the control posts back to the server each time a user interacts with the control. 
		/// </summary>
		[
			Bindable(true), 
			Category("Behavior"), 
			DefaultValue(false),
			Description("N/A")
		] 
		public bool AutoPostBack
		{
			get
			{
				object autoPostBack = ViewState["_autoPostBack"];

				if (autoPostBack != null)
					return (bool)autoPostBack;
				else
					return false;
			}

			set
			{
				ViewState["_autoPostBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		NotifyParentProperty(true),
		Description("Background color of the ToolTextBox.")
		]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(BorderStyle),"Solid"),
		NotifyParentProperty(true),
		Description("Border style of the ToolTextBox.")
		]
		public override BorderStyle BorderStyle
		{
			get { return base.BorderStyle;}
			set { base.BorderStyle = value;}
		}

		/// <summary>
		/// Gets or sets the display width of the <see cref="ToolTextBox"/> in characters.
		/// </summary>
		[
			Bindable(true), 
			Category("Appearance"), 
			DefaultValue(0),
			Description("The display width of the ToolTextBox in characters.")
		]
		public int Columns
		{
			get 
			{
				object columns = ViewState["_columns"];
				
				if (columns != null)
					return (int)columns;
				else
					return 0;
			}

			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");
				
				ViewState["_columns"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum number of characters allowed in the <see cref="ToolTextBox"/>.
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(0),
			Description("The maximum number of characters allowed in the ToolTextBox.")
		]
		public int MaxLength
		{
			get
			{
				object maxLength = ViewState["_maxLength"];

				if (maxLength != null)
					return (int)maxLength;
				else
					return 0;
			}

			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");

				ViewState["_maxlength"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the behavior mode (single-line, multiline, or password) of the <see cref="ToolTextBox"/>.
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(System.Web.UI.WebControls.TextBoxMode.SingleLine),
			Description("The behavior mode (single-line, multiline, or password) of the ToolTextBox.")
		]
		public System.Web.UI.WebControls.TextBoxMode TextMode
		{
			get
			{
				object textMode = ViewState["_textMode"];

				if (textMode != null)
                    return (System.Web.UI.WebControls.TextBoxMode)textMode;
				else
					return System.Web.UI.WebControls.TextBoxMode.SingleLine;
			}

			set
			{
				if (value < System.Web.UI.WebControls.TextBoxMode.SingleLine || value > System.Web.UI.WebControls.TextBoxMode.Password)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ViewState["_textMode"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the contents of the <see cref="ToolTextBox"/> control can be changed.
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(true),
			Description("A value indicating whether the contents of the ToolTextBox control can be changed.")
		]
		public bool ReadOnly
		{
			get
			{
				object readOnly = ViewState["_readOnly"];

				if (readOnly != null)
					return (bool)readOnly;
				else
					return false;
			}

			set
			{
				ViewState["_readOnly"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of rows displayed in a multiline text box.
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(0),
			Description("The number of rows displayed in a multiline text box.")
		]
		public int Rows
		{
			get
			{
				object rows = ViewState["_rows"];

				if (rows != null)
					return (int)rows;
				else
					return 0;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				ViewState["_rows"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the text content wraps within a multiline text box.
		/// </summary>
		[
			Bindable(true),
			Category("Layhout"),
			DefaultValue(true),
			Description("A value indicating whether the text content wraps within a multiline text box.")
		]
		public bool Wrap
		{
			get
			{
				object wrap = ViewState["_wrap"];

				if (wrap != null)
					return (bool)wrap;
				else
					return false;
			}

			set
			{
				ViewState["_wrap"] = value;
			}
		}

		/// <summary>
		/// Gets or the the border color.
		/// </summary>
		[
			Bindable(true),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValue(typeof(System.Drawing.Color),"#316AC5"),
			Category("Appearance"),
			Description("The botder color rollover.")
		]
		public Color BorderColorRollOver
		{
			get
			{
				object borderColorRollOver;
				borderColorRollOver = ViewState["_borderColorRollOver"];
				if (borderColorRollOver != null)
					return (Color)borderColorRollOver;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_borderColorRollOver"] = value;
			}
		}

		/// <summary>
		/// Use the TagKey property to determine the System.Web.UI.HtmlTextWriterTag value that is associated with this <see cref="ToolTextBox"/>.
		/// </summary>
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				if (TextMode == System.Web.UI.WebControls.TextBoxMode.MultiLine)
				{
					return HtmlTextWriterTag.Textarea;
				}
				else
				{
					return HtmlTextWriterTag.Input;
				}
			}
		}

		/// <summary>
		/// Indicates if the Text property have to be saved in the ViewState.
		/// </summary>
		private bool SaveTextViewState
		{
			get
			{
				if (Events[EventTextChanged] != null || !Enabled || !Visible || base.GetType() != typeof(ToolTextBox))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!SaveTextViewState)
			{
				base.ViewState.SetItemDirty("Text", false);
			}
			if (base.Page != null && AutoPostBack && base.Enabled)
			{		
				base.Page.RegisterRequiresPostBack(this);
			}
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{

			base.RenderBeginTag(output);
			if (TextMode == System.Web.UI.WebControls.TextBoxMode.MultiLine)
			{
				System.Web.HttpUtility.HtmlEncode(Text, output);
			}
			base.RenderEndTag(output);
		}

		/// <summary>
		/// Notifies the <see cref="ToolTextBox"/> that an element, either XML or HTML, was parsed, and adds the element to the server control's ControlCollection object.
		/// </summary>
		/// <param name="obj">An Object that represents the parsed element.</param>
		protected override void AddParsedSubObject(object obj)
		{
			if (!(obj is LiteralControl))
			{
				throw new System.Web.HttpException("ToolTextBox cannot have children");
			}
			Text = ((LiteralControl)obj).Text;
		}

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered to the specified System.Web.UI.HtmlTextWriter.
		/// </summary>
		/// <param name="output">A System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter output)
		{
			if (base.Page != null)
			{
				base.Page.VerifyRenderingInServerForm(this);
			}

			output.AddAttribute(HtmlTextWriterAttribute.Name, base.UniqueID);
			System.Web.UI.WebControls.TextBoxMode textBoxMode = TextMode;
			if (textBoxMode == System.Web.UI.WebControls.TextBoxMode.MultiLine)
			{
				int i = Rows;
				if (i > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Rows, i.ToString(NumberFormatInfo.InvariantInfo));
				}
				i = Columns;
				if (i > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Cols, i.ToString(NumberFormatInfo.InvariantInfo));
				}
				if (!Wrap)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Wrap, "off");
				}
			}
			else
			{
				if (textBoxMode == System.Web.UI.WebControls.TextBoxMode.Password)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Type, "password");
				}
				else
				{
					output.AddAttribute(HtmlTextWriterAttribute.Type, "text");
					string str = Text;
					if (str.Length > 0)
					{
						output.AddAttribute(HtmlTextWriterAttribute.Value, str);
					}
				}
				
				int i = MaxLength;
				if (i > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Maxlength, i.ToString(NumberFormatInfo.InvariantInfo));
				}
				i = Columns;
				if (i > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Size, i.ToString(NumberFormatInfo.InvariantInfo));
				}
			}
			if (ReadOnly)
			{
				output.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
			}
			if (AllowRollOver)
				output.AddAttribute(string.Format("onMouseover=\"this.style.borderColor='{0}'\"",Utils.Color2Hex(this.BorderColorRollOver)),null);
			output.AddAttribute(string.Format("onMouseout=\"this.style.borderColor='{0}'\"",Utils.Color2Hex(this.BorderColor)),null);

			base.AddAttributesToRender(output);
			if (AutoPostBack && base.Page != null)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Onchange, base.Page.GetPostBackClientEvent(this, ""));
				output.AddAttribute("language", "javascript");
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the content of the text box changes between posts to the server.
		/// </summary>
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(EventTextChanged, value);
			}

			remove
			{
				base.Events.RemoveHandler(EventTextChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="TextChanged"/> event. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">A System.EventArgs that contains event information.</param>
		protected virtual void OnTextChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[EventTextChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		#endregion

		#region Interface IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		/// </summary>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string text = Text;
			string postedData = postCollection[postDataKey];
			if (text.Equals(postedData))
			{
				return false;
			}

			Text = postedData;
			return true;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			OnTextChanged(EventArgs.Empty);
		}

		#endregion

	}

	#endregion
}
