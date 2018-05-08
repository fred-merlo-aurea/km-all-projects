using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.IO;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolCustomTags"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolCustomTags : ToolDropDownList
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCustomTags"/> class.
		/// </summary>
		public ToolCustomTags() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCustomTags"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolCustomTags(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolCustomTags" + Editor.indexTools++;
			else
				this.ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			//this.Width = Unit.Parse("150px");
			this.Height = Unit.Parse("22px");
			//this.Text = TitleText;
			this.ClientSideClick = "HTB_ExecuteCustomTags('$EDITOR_ID$', ATB_getSelectedValue('$CLIENT_ID$'));";

			this.Cellpadding = 0;
			this.ItemBackColor = Color.Empty;
			this.ItemBackColorRollOver = Color.Transparent;
			this.ItemBorderColor = Color.Transparent;
			this.ItemBorderColorRollOver = Color.FromArgb(0x31,0x6A,0xC5);
			this.ItemBorderStyle = BorderStyle.Solid;
			this.ItemBorderWidth = Unit.Parse("1px");
			this.ItemAlign = Align.Center;
		}

		/// <summary>
		/// Parse the specified CssFile and include the class names in the TagCollection.
		/// </summary>
		/// <param name="filename">The full path to the file.</param>
		/// <param name="tagName">The tag name to use.</param>
		/// <param name="attributeName">The attribute name to use.</param>
		/// <returns>The number of classes added.</returns>
		public int ParseCssFile(string filename, string tagName, string attributeName)
		{
			return this.ParseCssFile(filename, tagName, attributeName, string.Empty, string.Empty);
		}

		/// <summary>
		/// Parse the specified CssFile and include the class names in the TagCollection using exclude and include filters.
		/// </summary>
		/// <param name="filename">The full path to the file.</param>
		/// <param name="tagName">The tag name to use.</param>
		/// <param name="attributeName">The attribute name to use.</param>
		/// <param name="includeFilter">The string to find to include.</param>
		/// <param name="excludeFilter">The string to find to exclude.</param>
		/// <returns>The number of classes added.</returns>
		public int ParseCssFile(string filename, string tagName, string attributeName, string includeFilter, string excludeFilter)
		{
			int addedCount = 0; 
			string line, className;
			bool inDeclaration = false;

			FileStream cssFile = new FileStream(filename, FileMode.Open, FileAccess.Read);

			StreamReader cssRead = new StreamReader(cssFile);
			cssRead.BaseStream.Seek(0, SeekOrigin.Begin);

			ArrayList emails = new ArrayList();

			while (cssRead.Peek() > -1) 
			{
				line = cssRead.ReadLine();

				if (line.IndexOf("{") > -1)
					inDeclaration = true;

				if (line.IndexOf("}") > -1)
					inDeclaration = false;

				// Check if we are in a class declaration line.
				if (!inDeclaration || (inDeclaration && line.IndexOf("{") > 1))
				{
					if (inDeclaration)
						className = line.Substring(0, line.IndexOf("{")).Trim().Trim('}').Trim('.');
					else
						className = line.Trim().Trim('}').Trim('.');
					
					if (className.Length > 0 && ValidateClassName(className, includeFilter, excludeFilter))
					{
						this.Tags.Add(className, tagName, attributeName, className);
						addedCount++;
					}
					
				}
			}

			cssRead.Close();
			cssFile.Close();

			return addedCount;
		}

		/// <summary>
		/// Validate the specified class.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="includeFilter">The include filter.</param>
		/// <param name="excludeFilter">The exclude filter.</param>
		/// <returns></returns>
		public bool ValidateClassName(string className, string includeFilter, string excludeFilter)
		{
			bool validated = true;

			if (includeFilter != null && includeFilter != string.Empty && className.IndexOf(includeFilter) == -1)
				validated = false;
			
			if (excludeFilter != null && excludeFilter != string.Empty && className.IndexOf(excludeFilter) > -1)
				validated = false;

			return validated;
		}

		/// <summary>
		/// Gets or sets the custom tags.
		/// </summary>
		[Bindable(false),
		Category("Data"),
		//DefaultValue(false),
		Description("Get or set the custom collection containing the custom tags.")	]
		public TagCollection Tags
		{
			get
			{
				if (ViewState["_customTags"] == null)
					ViewState["_customTags"] = new TagCollection();
				return (TagCollection)ViewState["_customTags"];
			}
			set
			{
				ViewState["_customTags"] = value;
			}
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary> 
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);

			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
				this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
			this.ClientSideClick = this.ClientSideClick.Replace("$CLIENT_ID$", this.ClientID);

			this.Text = TitleText;

			base.Items.Clear();

			for(int i = 0 ; i < Tags.Labels.Length ; i++)
			{
				this.Items.Add(new ToolItem(string.Format("<{0} {1}={2} align=center>{3}</{0}>",Tags.TagNames[i],Tags.AttributeNames[i],Tags.AttributeValues[i],Tags.Labels[i]),string.Format("HTB_SetCustomTag('{0}','{1}','{2}','{3}');",editor.ClientID,Tags.TagNames[i],Tags.AttributeNames[i],Tags.AttributeValues[i])));
			}

		}


		/// <summary>
		/// Gets or sets the title text.
		/// </summary>
		/// <value>The title text.</value>
		public string TitleText
		{
			get
			{
				object titleText = ViewState["TitleText"];

				if (titleText == null)
				{
					return "Custom Tags";
				}

				return (string)titleText;
			}

			set
			{
				ViewState["TitleText"] = value;
			}
		}

	}
}
