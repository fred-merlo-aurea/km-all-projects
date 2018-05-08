using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolFontFace"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolFontFace : ToolDropDownList
	{
		private readonly string TOOLFONTFACEKEY = "ToolFontFace";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFontFace"/> class.
		/// </summary>
		public ToolFontFace() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFontFace"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolFontFace(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolFontFace" + Editor.indexTools++;
			else
				this.ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Width = Unit.Parse("150px");
			this.Height = Unit.Parse("22px");
			this.Text = "Font Face";

			//this.ClientSideClick = "if (HTB_ie5) HTB_CommandBuilder('$EDITOR_ID$', 'fontname', ATB_getSelectedValue('$CLIENT_ID$')); else HTB_Ns6FontFaceClicked('$EDITOR_ID$','$CLIENT_ID$');";
		}
 
		/// <summary>
		/// Do some work before rendering the control.
		/// </summary> 
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);

			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("IE") == -1)
				isNs6 = true;

			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
			{
				//this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
				this.ClientSideClick = string.Format("if (HTB_ie5) HTB_CommandBuilder('{0}', 'fontname', ATB_getSelectedValue('{1}')); else HTB_Ns6FontFaceClicked('{0}','{1}');",editor.ClientID,this.ClientID);
			}
			//this.ClientSideClick = this.ClientSideClick.Replace("$CLIENT_ID$", this.ClientID);

			base.Items.Clear();
			string hiddenFace = string.Empty;

			if (Fonts.Count == 0)
			{
				this.Items.Add(new ToolItem("<font face='Arial' size='2'>Arial</font>","Arial"));
				this.Items.Add(new ToolItem("<font face='Arial Black' size='2'>Arial Black</font>","Arial Black"));
				this.Items.Add(new ToolItem("<font face='Arial Narrow' size='2'>Arial Narrow</font>","Arial Narrow"));
				this.Items.Add(new ToolItem("<font face='Comic Sans MS' size='2'>Comic Sans MS</font>","Comic Sans MS"));
				this.Items.Add(new ToolItem("<font face='Courier New' size='2'>Courier New</font>","Courier New"));
				this.Items.Add(new ToolItem("<font face='System' size='2'>System</font>","System"));
				this.Items.Add(new ToolItem("<font face='Times New Roman' size='2'>Times New Roman</font>","Times New Roman"));
				this.Items.Add(new ToolItem("<font face='Verdana' size='2'>Verdana</font>","Verdana"));
				this.Items.Add(new ToolItem("<font face='Wingdings' size='2'>Wingdings</font>","Wingdings"));
                if (isNs6)
                {
                    hiddenFace = "Arial;Arial Black;Arial Narrow;Comic Sans MS;Courier New;System;Times New Roman;Verdana;Wingdings";
                    Overflow = "auto";
                }
			}
			else
			{
				foreach(object font in Fonts)
				{
					if (!(font is string))
						throw new InvalidCastException("Fonts must contains only string object.");

					this.Items.Add(new ToolItem(string.Format("<font face='{0}' size='2'>{0}</font>",font),(string)font));
					if (isNs6)
						hiddenFace += (string)font + ";";
				}

				hiddenFace = hiddenFace.TrimEnd(';');
			}

			if (isNs6)
			{
				string scriptKey = this.ClientID + "_" + TOOLFONTFACEKEY + "_Init";
				System.Text.StringBuilder initValues = new System.Text.StringBuilder();
				initValues.Append("<script language='javascript'>\n");
				initValues.Append("var ");
				initValues.Append(this.ClientID);
				initValues.Append("_Values = '");
				initValues.Append(hiddenFace); 
				initValues.Append("';\n");
				initValues.Append("</script>");
				Page.RegisterStartupScript(scriptKey, initValues.ToString());
			}

		}

		/// <summary>
		/// Gets or sets the ArrayList containing the font names.
		/// </summary>
		/// <value>The fonts.</value>
		[
			Bindable(false),
			Category("Data"),
			Description("Get or set the ArrayList containing the font names.")
		]
		public ArrayList Fonts
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(Fonts), new ArrayList());
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(Fonts), value);
			}
		}
	}
}
