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
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// ASP.NET control that provides extended features to the standard button including multi-styles, post in any form on the page, disable on submit, confirm on submit and more. 
	/// </summary>
	[
        Serializable,
        ToolboxBitmap(typeof(ExtendedButton), "ToolBoxBitmap.Button.bmp")
    ]
	public class ExtendedButton : ExtendedWebControl, IPostBackEventHandler, IPostBackDataHandler
	{
		private const string ExternalScriptFx1 = "ActiveInput.js";
		private int x = -1;
		private int y = -1;

		#region properties


		/// <summary>
		/// Gets the resource name containing the javascript logic.
		/// </summary>
		protected override string ResourceName
		{
			get
			{
				return "ActiveUp.WebControls._resources.ActiveInput.js";
			}
		}

		/// <summary>
		/// Gets the script key used to register the javascript logic.
		/// </summary>
		protected override string ScriptKey
		{
			get
			{
				return "ACTIVEINPUT";
			}
		}

		protected override string ExternalScriptDefault
		{
			get
			{
				return Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ExternalScriptFx1);
			}
		}

		/// <summary>
		/// Gets or sets the type of button to render.
		/// </summary>
		/// <remarks>You can render the same button as a link, a button or an image. Please see the <see cref="ButtonType"/> enumeration for details.</remarks>
		[Bindable(false), Category("Behavior"), DefaultValue(true), Description("the type of button to render")]
		public ExtendedButtonType ButtonType
		{
			get
			{
				object obj1 = this.ViewState["ButtonType"];
				if (obj1 != null)
				{
					return (ExtendedButtonType) obj1;
				}
				return ExtendedButtonType.Button;
			}
			set
			{
				this.ViewState["ButtonType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the button cause the page validation.
		/// </summary>
		[Bindable(false), Category("Behavior"), DefaultValue(true), Description("the value indicating if the button cause the page validation.")]
		public bool CausesValidation
		{
			get
			{
				object obj1 = this.ViewState["CausesValidation"];
				if (obj1 != null)
				{
					return (bool) obj1;
				}
				return true;
			}
			set
			{
				this.ViewState["CausesValidation"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the button should disable itself when clicked (submit).
		/// </summary>
		[Bindable(false), Category("Behavior"), DefaultValue(true), Description("the value indicating if the button should disable itself when clicked (submit).")]
		public bool DisableOnClick
		{
			get
			{
				object obj1 = this.ViewState["DisableOnClick"];
				if (obj1 != null)
				{
					return (bool) obj1;
				}
				return false;
			}
			set
			{
				this.ViewState["DisableOnClick"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to use for the confirmation box.
		/// </summary>
		/// <remarks>If you leave this property empty, no confirmation is asked.</remarks>
		[DefaultValue(""), Description("The value indicating if the button should call the confirmation box before applying its onclick events."), Bindable(true), Category("Behavior")]
		public string ConfirmationMessage
		{
			get
			{
				string text1 = (string) this.ViewState["ConfirmationMessage"];
				if (text1 != null)
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ConfirmationMessage"] = value;
			}
		}
 
		/// <summary>
		/// Gets or sets the text to use in the button.
		/// </summary>
		/// <remarks>This property is used in the Link and Button rendering modes.</remarks>
		[DefaultValue(""), Description("The text to use in the button."), Bindable(true), Category("Appearance")]
		public string Text
		{
			get
			{
				string text1 = (string) this.ViewState["Text"];
				if (text1 != null)
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the URL to use for the button.
		/// </summary>
		/// <remarks>If this property is left empty, the standard behavior of the button is to do a postback. However if you specify a valid link, any server-side events will be ignored and the specified link (page) will be accessed just like a standard HTML hyperlink.</remarks>
		[DefaultValue(""), Description("The URL to use for the button."), Bindable(true), Category("Appearance")]
		public string NavigateUrl
		{
			get
			{
				string obj = (string) this.ViewState["NavigateUrl"];
				if (obj != null)
				{
					return obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["NavigateUrl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the target Form object to post to.
		/// </summary>
		[DefaultValue(""), Description("Button_Text"), Bindable(true), Category("Appearance")]
		public string TargetForm
		{
			get
			{
				string text1 = (string) this.ViewState["TargetForm"];
				if (text1 != null)
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["TargetForm"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the image URL to use for the Image rendering mode.
		/// </summary>
		public virtual string ImageUrl
		{
			get
			{
				string text1 = (string) this.ViewState["ImageUrl"];
				if (text1 != null)
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ImageUrl"] = value;
			}
		}
  		#endregion

		#region dotnet
		private string GetPostBackEventReference(Control control, string argument)
		{
            if (System.Web.HttpContext.Current != null)
            {
                if (this.NavigateUrl != string.Empty)
                {
                    return string.Format("self.location='{0}';", this.NavigateUrl);
                }
                else if (this.TargetForm != string.Empty || this.DisableOnClick == true)
                {
                    this.Page.RegisterRequiresPostBack(this);

                    string clientScript = @"
<script language=""javascript"" type=""text/javascript"">
<!--
	function __doPostBackExtended(eventTarget, eventArgument, targetForm, disableOnSubmit, control) {
		var theform;
		if (window.navigator.appName.toLowerCase().indexOf(""microsoft"") > -1) {
			theform = document.getElementById(targetForm);
		}
		else {
			theform = document.forms[targetForm];
		}
		//theform.__EVENTTARGET.value = eventTarget.split(""$"").join("":"");
		//theform.__EVENTARGUMENT.value = eventArgument;
		if (disableOnSubmit)
			control.disabled=true;
		theform.submit();
	}
// -->
</script>
";
                    //control.Page.RegisterClientScriptBlock(ScriptKey + "_PostBack", clientScript);
                    control.Page.RegisterStartupScript(ScriptKey + "_PostBack", clientScript);

                    if (this.TargetForm == string.Empty)
                    {
                        Control form = FindParentFormControl(this);
                        if (form != null)
                            this.TargetForm = form.ClientID;
                    }

                    return string.Format("__doPostBackExtended('{0}','{1}', '{2}', {3}, this)", control.UniqueID.Replace(":", "$"), argument, this.TargetForm, this.DisableOnClick.ToString().ToLower());
                }
                else
                {
                    return this.Page.GetPostBackEventReference(control, argument);
                }

            }

            return string.Empty;

		}

		private Control FindParentFormControl(Control control)
		{
			if (control == null)
				return null;

			if (control.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlForm")
				return control;
			else
				return FindParentFormControl(control.Parent);
		}

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered
		/// to the specified <see cref="T:System.Web.UI.HtmlTextWriter" qualify="true"/>. This method is used primarily by control
		/// developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" qualify="true"/> that represents the output stream to render HTML content on the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}

			string validationString = "if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate();";
			//string validationString = "alert(Page_ClientValidate());";

			// Common
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			
			// Specific
			switch (ButtonType)
			{
				case ExtendedButtonType.Button:

					writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);
					if (this.NavigateUrl == string.Empty)
						writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
					else
						writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
					break;

				case ExtendedButtonType.Image:

					writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImageUrl);
					writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
					writer.AddAttribute(HtmlTextWriterAttribute.Alt, this.Text);
					//writer.AddAttribute(HtmlTextWriterAttribute.Alt, this.Text);
					break;

				case ExtendedButtonType.Link:
					writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
					validationString = string.Format("{{if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate()) {0}}}", this.Page.GetPostBackEventReference(this, "")) ;
					break;
			}

			string onclick = base.Attributes["onclick"];
			if (onclick == null)
				onclick = string.Empty;

			//onclick = "this.disabled=true;";

			// The click event
			if (this.Page != null)
			{
				// Use validators
				if (this.CausesValidation && this.Page.Validators.Count > 0)
				{
					if (this.ConfirmationMessage == string.Empty)
					{
						onclick = validationString + onclick;
					}
					else
					{
						onclick = string.Format("{{if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate()) if (confirm('{0}')) {{ {1} }} else return false;}}", this.ConfirmationMessage, this.GetPostBackEventReference(this, "")) + onclick;
					}
				}
				else if (this.ConfirmationMessage != string.Empty)
				{
					onclick = string.Format("{{if (confirm('{0}')) {{ {1}{2} }} else return false;}}", this.ConfirmationMessage, onclick, this.GetPostBackEventReference(this, string.Empty));
				}
				else
					onclick = this.GetPostBackEventReference(this, string.Empty);
			}

			/*// Confirmation event
			if (this.ConfirmationMessage != string.Empty)
			{
				if (onclick != string.Empty)
				{
					onclick = "{if (confirm('" + this.ConfirmationMessage + "')) " + onclick + " }";
				}
				else
					onclick = "return confirm('" + this.ConfirmationMessage + "');";
			}*/
			
			if (onclick != string.Empty)
			{
				base.Attributes.Remove("onclick");
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onclick);
				writer.AddAttribute("language", "javascript");
			}

			base.AddAttributesToRender(writer);
		}

		/// <summary>
		/// Notifies the server control that an element, either XML
		/// or HTML, was parsed, and adds the element to the server control's <see cref="T:System.Web.UI.ControlCollection"/>
		/// object.
		/// </summary>
		/// <param name="obj">An <see cref="T:System.Object"/> that represents the parsed element.</param>
		protected override void AddParsedSubObject(object obj)
		{
			if (!(obj is LiteralControl))
			{
				throw new Exception(string.Format("ExtendedButton can't have children controls of type {0}.", obj.GetType().Name.ToString()));
			}
			this.Text = ((LiteralControl) obj).Text;
		}

		/// <summary>
		/// Renders the specified writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			
#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AIP, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				writer.Write(StaticContainer.TrialMessage);
			}
			else
			{
				RenderExtendedButton(writer);
			}

#else
			RenderExtendedButton(writer);
#endif
		}

		/// <summary>
		/// Renders the extended button.
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void RenderExtendedButton(HtmlTextWriter writer) 
		{
			AddAttributesToRender(writer);

			switch (ButtonType)
			{
				case ExtendedButtonType.Button:
				case ExtendedButtonType.Image:

					writer.RenderBeginTag(HtmlTextWriterTag.Input);
					writer.RenderEndTag();
					break;
				case ExtendedButtonType.Link:
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(this.Text);
					writer.RenderEndTag();
					break;
			}
		}

		#endregion

		#region events
		/*protected virtual void OnClick(ExtendedButtonClickEventArgs e)
		{
			EventHandler handler1 = (EventHandler) base.Events[ExtendedButton.EventClick];
			if (handler1 != null)
			{
				handler1(this, e);
			}
		}*/

		/*protected virtual void OnCommand(CommandEventArgs e)
		{
			//CommandEventHandler handler1 = (CommandEventHandler) base.Events[ExtendedButton.EventCommand];
			EventHandler handler1 = (EventHandler) base.Events[ExtendedButton.EventCommand];
			if (handler1 != null)
			{
				handler1(this, e);
			}
			//base.RaiseBubbleEvent(this, e);
		}*/

		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			if (this.CausesValidation)
			{
				this.Page.Validate();
			}
			this.OnClick(new ExtendedButtonClickEventArgs(this.ButtonType, this.x, this.y));
			this.OnCommand(new CommandEventArgs(this.CommandName, this.CommandArgument));
		}
 
		/// <summary>
		/// Gets or sets the command argument.
		/// </summary>
		/// <value>The command argument.</value>
		[DefaultValue(""), Description("Button_CommandArgument"), Category("Behavior"), Bindable(true)]
		public string CommandArgument
		{
			get
			{
				string text1 = (string) this.ViewState["CommandArgument"];
				if (text1 != null)
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CommandArgument"] = value;
			}
		}
 

		/// <summary>
		/// Gets or sets the command name.
		/// </summary>
		/// <value>The command name.</value>
		[DefaultValue(""), Description("Button_Command"), Category("Behavior")]
		public string CommandName
		{
			get
			{
				string text1 = (string) this.ViewState["CommandName"];
				if (text1 != null)
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CommandName"] = value;
			}
		}
 

		/// <summary>
		/// Click server event.
		/// </summary>
		public event EventHandler Click;
     
		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="e">The <see cref="ActiveUp.WebControls.ExtendedButtonClickEventArgs"/> instance containing the event data.</param>
		protected virtual void OnClick(ExtendedButtonClickEventArgs e)
		{
			if (Click != null)
				Click(this,e);
		}

		/// <summary>
		/// Command server event.
		/// </summary>
		public event CommandEventHandler Command;

		/// <summary>
		/// Raises the command event.
		/// </summary>
		/// <param name="e">The <see cref="System.Web.UI.WebControls.CommandEventArgs"/> instance containing the event data.</param>
		protected virtual void OnCommand(CommandEventArgs e)
		{
			if (Command != null)
				Command(this,e);
		}

		/*public event CommandEventHandler Command;

		protected virtual void OnCommand(CommandEventArgs e)
		{
			if (Command != null)
				Command(this,e);
		}*/

		/*[Category("Action")]
		public event EventHandler Click
		{
			add
			{
				Events.AddHandler(EventClick, value);
			}
			remove
			{
				Events.RemoveHandler(EventClick, value);
			}
		}
 
		[Category("Action")]
		public event CommandEventHandler Command
		{
			add
			{
				Events.AddHandler(EventCommand, value);
			}
			remove
			{
				Events.RemoveHandler(EventCommand, value);
			}
		}*/

		/*private static readonly object EventClick = null;
 		private static readonly object EventCommand = null;*/
		#endregion

		#region postdata
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string clientID = this.UniqueID;
			string xPos = postCollection[clientID + ".x"];
			string yPos = postCollection[clientID + ".y"];
			if (((xPos != null) && (yPos != null)) && ((xPos.Length > 0) && (yPos.Length > 0)))
			{
				this.x = int.Parse(xPos);
				this.y = int.Parse(yPos);
				this.Page.RegisterRequiresRaiseEvent(this);
			}
			return false;
		}
 
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
		}
		#endregion

	}
}
