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
	/// ASP.NET control that is used as a base class for others controls.
	/// </summary>
	public class ExtendedTextBox : ExtendedWebControl, IPostBackDataHandler
	{
		private const string ExternalScriptFx1 = "ActiveInput.js";

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
		/// Renders the control.
		/// </summary>
		/// <param name="writer">The HtmlTextWriter.</param>
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
				RenderExtendedTextBox(writer);
			}

#else
			RenderExtendedTextBox(writer);
#endif
		}

		/// <summary>
		/// Renders the extended text box.
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void RenderExtendedTextBox(HtmlTextWriter writer) 
		{
			this.AddAttributesToRender(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
			if (this.ReadOnly)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
			}
			if (!this.Enabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			this.ControlStyle.AddAttributesToRender(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, System.Web.HttpUtility.HtmlEncode(this.Text));
			if (TabIndex > 0)
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex,TabIndex.ToString());
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
		}

		/// <summary>
		/// Overridden. 
		/// </summary>
		/// <param name="obj">Object</param>
		protected override void AddParsedSubObject(object obj)
		{
			if (!(obj is LiteralControl))
			{
				throw new Exception(string.Format("ExtendedTextBox can't have children controls of type {0}.", obj.GetType().Name.ToString()));
			}
			this.Text = ((LiteralControl) obj).Text;
		}
 
		/// <summary>
		/// Overridden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			
			/*if (((this.Page != null) && this.AutoPostBack) && this.Enabled)
			{
				Page.RegisterPostBackScript();
			}*/
		}
 
		/// <summary>
		/// Load the posted data.
		/// </summary>
		/// <param name="postDataKey">The data key.</param>
		/// <param name="postCollection">The data collection.</param>
		/// <returns>Value indicating if the data changed.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string text1 = this.Text;
			string text2 = postCollection[postDataKey];
			if (!text1.Equals(text2))
			{
				this.Text = text2;
				return true;
			}
			return false;
		}
 
		/// <summary>
		/// Called server-side when the text changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.OnTextChanged(EventArgs.Empty);
		}
 
		/// <summary>
		/// Gets or sets a value indicating whether an automatic postback to the server will occur whenever the user modifies the text in the TextBox control and then tabs out of the control.
		/// </summary>
		[Category("Behavior"), DefaultValue(false), Description("Value indicating whether an automatic postback to the server will occur whenever the user modifies the text in the TextBox control and then tabs out of the control.")]
		public virtual bool AutoPostBack
		{
			get
			{
				object obj1 = this.ViewState["AutoPostBack"];
				if (obj1 != null)
				{
					return (bool) obj1;
				}
				return false;
			}
			set
			{
				this.ViewState["AutoPostBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the contents of the TextBox control can be changed.
		/// </summary>
		[Category("Behavior"), Bindable(true), DefaultValue(false), Description("value indicating whether the contents of the TextBox control can be changed.")]
		public virtual bool ReadOnly
		{
			get
			{
				object obj1 = this.ViewState["ReadOnly"];
				if (obj1 != null)
				{
					return (bool) obj1;
				}
				return false;
			}
			set
			{
				this.ViewState["ReadOnly"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text content of the TextBox control.
		/// </summary>
		[Bindable(true), Category("Appearance"), DefaultValue("text content of the TextBox control."), PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
		public virtual string Text
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
		/// Gets or sets a value indicating whether the text content wraps within a multiline text box.
		/// </summary>
		[Category("Layout"), DefaultValue(true), Description("value indicating whether the text content wraps within a multiline text box.")]
		public virtual bool Wrap
		{
			get
			{
				object obj1 = this.ViewState["Wrap"];
				if (obj1 != null)
				{
					return (bool) obj1;
				}
				return true;
			}
			set
			{
				this.ViewState["Wrap"] = value;
			}
		}

		/// <summary>
		/// Occurs when the content of the text box changes between posts to the server.
		/// </summary>
		[Category("Action")]
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(ExtendedTextBox.EventTextChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ExtendedTextBox.EventTextChanged, value);
			}
		}

		/// <summary>
		/// The EventTextChanged event.
		/// </summary>
		private static readonly object EventTextChanged = null;

		/// <summary>
		/// Raises the TextChanged event. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnTextChanged(EventArgs e)
		{
			EventHandler handler1 = (EventHandler) base.Events[ExtendedTextBox.EventTextChanged];
			if (handler1 != null)
			{
				handler1(this, e);
			}
		}
 


	}
}
