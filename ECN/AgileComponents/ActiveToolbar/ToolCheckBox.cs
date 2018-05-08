// ActiveToolbar 1.x
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;

namespace ActiveUp.WebControls
{
	#region class ToolCheckBox 

	/// <summary>
	/// Represents a checkbox.
	/// </summary>
	[
        ToolboxItem(false),
        Serializable
    ]
	public class ToolCheckBox : ToolBase, IPostBackDataHandler
	{
		#region Variables

		/// <summary>
		/// Event occurs when the checked value change.
		/// </summary>
        private readonly static object EventCheckedChanged = new Object();

		#endregion

		#region Constructor

		/// <summary>
		/// Default constrcutor.
		/// </summary>
		public ToolCheckBox()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="ToolCheckBox"/> control is checked.
		/// </summary>
		[
			Bindable(true), 
			DefaultValue(false),
			Description("A value indicating whether the ToolCheckBox control is checked."),
			Category("Data")
		] 
		public bool Checked
		{
			get
			{
				object isChecked = ViewState["_checked"];
				if (isChecked != null)
				{
					return (bool)isChecked;
				}
				else
					return false;
			}

			set
			{
				ViewState["_checked"] = value;
			}
		}


		/// <summary>
		/// Gets or sets a value that indicates whether or not the control posts back to the server each time a user interacts with the control. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("Indicates whether or not the control posts back to the server each time a user interacts with the control.")
		] 
		public bool AutoPostBack
		{
			get
			{
				object autoPostBack = ViewState["_autoPostBack"];
				if (autoPostBack != null)
				{
					return (bool)autoPostBack;
				}
				else
				{
					return false;
				}
			}

			set
			{
				base.ViewState["_autoPostBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border color when the mouse is over the <see cref="ToolCheckBox"/>.
		/// </summary>
		[
		Bindable(true),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValue(typeof(System.Drawing.Color),""),
		Category("Appearance")
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
		/// Gets or sets text alignment options for the <see cref="ToolCheckBox"/>.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			DefaultValue(System.Web.UI.WebControls.TextAlign.Right),
			Description("text alignment options for the ToolCheckBox.")
		]
		public System.Web.UI.WebControls.TextAlign TextAlign
		{
			get
			{
				object textAlign = ViewState["_textAlign"];
				if (textAlign != null)
				{
					return (System.Web.UI.WebControls.TextAlign)textAlign;
				}
				else
					return System.Web.UI.WebControls.TextAlign.Right;
			}
			
			set
			{
				if (value < System.Web.UI.WebControls.TextAlign.Left || value > System.Web.UI.WebControls.TextAlign.Right)
					throw new ArgumentOutOfRangeException("value");

				ViewState["_textAlign"] = value;
			}
		}

		/// <summary>
		/// Save the checked value in the viewstate.
		/// </summary>
		private bool SaveCheckedViewState
		{
			get
			{
				if (Events[EventCheckedChanged] != null || !Enabled)
				{
					return true;
				}
				Type type = base.GetType();
				if (type == typeof(ToolCheckBox))
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Notifies the ToolCheckBox control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			if (Page != null && Enabled)
			{
				base.Page.RegisterRequiresPostBack(this);
			}
			if (!SaveCheckedViewState)
			{
				ViewState.SetItemDirty("Checked", false);
			}
		}

		
		/// <summary>
		/// Sends the ToolCheckBox content to a provided HtmlTextoutput object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextoutput object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			bool useSpan = false;
			
			if (!Enabled)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
				useSpan = true;
			}

			if (BorderColorRollOver != Color.Empty && AllowRollOver)
			{
				output.AddAttribute(string.Format("onMouseover=\"this.style.borderColor='{0}'\"",Utils.Color2Hex(this.BorderColorRollOver)),null);
				output.AddAttribute(string.Format("onMouseout=\"this.style.borderColor='{0}'\"",Utils.Color2Hex(this.BorderColor)),null);
			}

			string toolTipText = ToolTip;
			if (toolTipText.Length > 0)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Title, toolTipText);
				useSpan = true;
			}

			if (ControlStyleCreated)
			{
				System.Web.UI.WebControls.Style style = base.ControlStyle;
				style.AddAttributesToRender(output, this);
				useSpan = true;
			}

			if (Attributes.Count != 0)
			{
				System.Web.UI.AttributeCollection attributeCollection = Attributes;
				string attributeValue = attributeCollection["value"];
				if (attributeValue != null)
				{
					attributeCollection.Remove("value");
				}
				if (attributeCollection.Count != 0)
				{
					attributeCollection.AddAttributes(output);
					useSpan = true;
				}
				if (attributeValue != null)
				{
					attributeCollection["value"] = attributeValue;
				}
			}
			if (useSpan)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Span);
			}

			if(this.ClientSideClick != string.Empty)
				output.AddAttribute(HtmlTextWriterAttribute.Onclick, this.ClientSideClick);

			/*if (this.ClientSideOver != string.Empty)
				output.AddAttribute("onmouseover", this.ClientSideOver);*/
				
			if (Text.Length == 0)
			{
				RenderInputTag(output, ClientID);
			}
			else if (TextAlign == System.Web.UI.WebControls.TextAlign.Left)
			{
				RenderLabel(output, Text, ClientID);
				RenderInputTag(output, ClientID);
			}
			else
			{
				RenderInputTag(output, ClientID);
				RenderLabel(output, Text, ClientID);
			}

			if (useSpan)
			{
				output.RenderEndTag();
			}

		}

		/// <summary>
		/// Write the label to be rendered on the client.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the control.</param>
		/// <param name="text">Text to render.</param>
		/// <param name="clientID">Id to identify the <see cref="ToolCheckBox"/>.</param>
		private void RenderLabel(HtmlTextWriter output, string text, string clientID)
		{
			output.AddAttribute(HtmlTextWriterAttribute.For, clientID);
			output.RenderBeginTag(HtmlTextWriterTag.Label);
			output.Write(text);
			output.RenderEndTag();
		}

		/// <summary>
		/// Write the input tag to be rendered on the client.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the control.</param>
		/// <param name="clientID">Id to identify the <see cref="ToolCheckBox"/>.</param>
		protected virtual void RenderInputTag(HtmlTextWriter output, string clientID)
		{
			string backImage = string.Empty;
			if (BackImage != string.Empty)
			{
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
					backImage = "url(" + Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,BackImage) + ")";
				else
					backImage = "url(" + BackImage + ")";
			}
			if (backImage != string.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,backImage);

			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			output.AddAttribute(HtmlTextWriterAttribute.Id, clientID);
			output.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
			output.AddAttribute(HtmlTextWriterAttribute.Name, ClientID);
			if (Checked)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (AutoPostBack)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Onclick, base.Page.GetPostBackClientEvent(this, ""));
				output.AddAttribute("language", "javascript");
			}
			string str = base.AccessKey;
			if (str.Length > 0)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Accesskey, str);
			}
			int i = base.TabIndex;
			if (i != 0)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Tabindex, i.ToString(NumberFormatInfo.InvariantInfo));
			}
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
		}

		
		#endregion

		#region Events

		/// <summary>
		/// Occurs when the value of the <see cref="Checked"/> property changes between posts to the server.
		/// </summary>
		public event EventHandler CheckedChanged
		{
			add
			{
				Events.AddHandler(EventCheckedChanged, value);
			}

			remove
			{
				Events.RemoveHandler(EventCheckedChanged, value);
			}
		}		

		/// <summary>
		/// Raises the <see cref="CheckedChanged"/> event of the <see cref="ToolCheckBox"/> control. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)Events[EventCheckedChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		#endregion

		#region Interface IPostBackDataHandler

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			OnCheckedChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Processes post-back data from the control.
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		/// </summary>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string str = postCollection[postDataKey];
			bool flag1 = (str != null) ? (str.Length > 0) : false;
			bool flag2 = flag1 == Checked == false;
			Checked = flag1;
			return flag2;
		}

		#endregion
	}

	#endregion
} 

