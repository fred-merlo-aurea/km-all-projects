using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;

namespace ActiveUp.WebControls
{
	#region class ToolRadioButton

	/// <summary>
	/// Represents a <see cref="ToolRadioButton"/>.
	/// </summary>
	[
        ToolboxItem(false),
        Serializable
    ]
	public class ToolRadioButton : ToolCheckBox, IPostBackDataHandler
	{
		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public ToolRadioButton()
		{

		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the name of the group that the radio button belongs to.
		/// </summary>
		[
			Bindable(true), 
			DefaultValue(false),
			Description("The name of the group that the radio button belongs to."),
			NotifyParentProperty(true)		
		]
		public string GroupName
		{
			get
			{
				object groupName = ViewState["_groupName"];
				if (groupName != null)
					return (string)groupName;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_groupName"] = value;
			}
		}

		/// <summary>
		/// Gets an unique group name.
		/// </summary>
		private string UniqueGroupName
		{
			get
			{
				string groupName = GroupName;
				string uniqueID = base.UniqueID;
				int i = uniqueID.LastIndexOf(':');
				if (i >= 0)
				{
					groupName = String.Concat(uniqueID.Substring(0, i + 1), groupName);
				}
				return groupName;
			}
		}

		/// <summary>
		/// Gets the value attribute.
		/// </summary>
		private string ValueAttribute
		{
			get
			{
				string str = base.Attributes["value"];
				if (str == null)
				{
					if (base.ID != null)
					{
						str = base.ID;
					}
					else
					{
						str = base.UniqueID;
					}
				}
				return str;
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
			if (base.Page != null && !base.Checked && base.Enabled)
			{
				base.Page.RegisterRequiresPostBack(this);
			}
			if (GroupName.Length == 0)
			{
				GroupName = base.UniqueID;
			}
		}

		/// <summary>
		/// Write the input tag to be rendered on the client.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the control.</param>
		/// <param name="clientID">Id to identify the <see cref="ToolRadioButton"/>.</param>
		protected override void RenderInputTag(HtmlTextWriter output, string clientID)
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
			output.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueGroupName);
			output.AddAttribute(HtmlTextWriterAttribute.Value, ValueAttribute);
			if (base.Checked)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (base.AutoPostBack)
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

		#region Interface IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		/// </summary>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string uniqueGroupName = postCollection[UniqueGroupName];
			bool flag = false;
			if (uniqueGroupName != null && uniqueGroupName.Equals(ValueAttribute))
			{
				if (!base.Checked)
				{
					base.Checked = true;
					flag = true;
				}
			}
			else if (base.Checked)
			{
				base.Checked = false;
			}
			return flag;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			base.OnCheckedChanged(EventArgs.Empty);
		}

		#endregion
	}
	#endregion
}
