using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolParagraph"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolParagraph : ToolDropDownList
	{
		private readonly string TOOLPARAGRAPHKEY = "ToolParagraph";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolParagraph"/> class.
		/// </summary>
		public ToolParagraph() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolParagraph"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolParagraph(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolParagraph" + Editor.indexTools++;
			else
				this.ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Width = Unit.Parse("100px");
			this.Height = Unit.Parse("22px");
			this.Text = "Paragraph";
			this.Cellpadding = 2;
			this.ItemBackColor = Color.Empty;
			this.ItemBackColorRollOver = Color.Transparent;
			this.ItemBorderColor = Color.Transparent;
			this.ItemBorderColorRollOver = Color.FromArgb(0x31,0x6A,0xC5);
			this.ItemBorderStyle = BorderStyle.Solid;
			this.ItemBorderWidth = Unit.Parse("1px");

			//this.ClientSideClick = "if (HTB_ie5) HTB_CommandBuilder('$EDITOR_ID$', 'formatblock', ATB_getSelectedValue('$CLIENT_ID$')); else HTB_Ns6ParagraphClicked('$EDITOR_ID$','$CLIENT_ID$');";
		}
        
		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{

			base.OnPreRender(e);

			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
			{
				//this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
				this.ClientSideClick = string.Format("if (HTB_ie5) HTB_CommandBuilder('{0}', 'formatblock', ATB_getSelectedValue('{1}')); else HTB_Ns6ParagraphClicked('{0}','{1}');",editor.ClientID,this.ClientID);
			}
			//this.ClientSideClick = this.ClientSideClick.Replace("$CLIENT_ID$", this.ClientID);

			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("IE") == -1)
				isNs6 = true;

			base.Items.Clear();
			string hiddenParagraph = string.Empty;

			if (ParagraphStyles.Count == 0)
			{
				this.ParagraphStyles.Add(new ToolItem("<p align='center'>Normal</p>","Normal"));
				this.ParagraphStyles.Add(new ToolItem("<h1 align='center'>Heading 1</h1>","<h1>"));
				this.ParagraphStyles.Add(new ToolItem("<h2 align='center'>Heading 2</h2>","<h2>"));
				this.ParagraphStyles.Add(new ToolItem("<h3 align='center'>Heading 3</h3>","<h3>"));
				this.ParagraphStyles.Add(new ToolItem("<h4 align='center'>Heading 4</h4>","<h4>"));
				this.ParagraphStyles.Add(new ToolItem("<h5 align='center'>Heading 5</h5>","<h5>"));
				this.ParagraphStyles.Add(new ToolItem("<h6 align='center'>Heading 6</h6>","<h6>"));
				this.ParagraphStyles.Add(new ToolItem("<address align='center'>Address</address>","<address>"));
				this.ParagraphStyles.Add(new ToolItem("<pre align='center'>Formatted</pre>","<pre>"));

                if (isNs6)
                {
                    hiddenParagraph = "normal;<h1>;<h2>;<h3>;<h4>;<h5>;<h6>;<address>;<pre>";
                    Overflow = "auto";
                }
			}
			else
			{
				foreach(ToolItem paragraph in ParagraphStyles)
				{
					if (isNs6)
						hiddenParagraph += paragraph.Value + ";";
				}

				hiddenParagraph = hiddenParagraph.TrimEnd(';');
			}

			if (isNs6)
			{
				string scriptKey = this.ClientID + "_" + TOOLPARAGRAPHKEY + "_Init";
				System.Text.StringBuilder initValues = new System.Text.StringBuilder();
				initValues.Append("<script language='javascript'>\n");
				initValues.Append("var ");
				initValues.Append(this.ClientID);
				initValues.Append("_Values = '");
				initValues.Append(hiddenParagraph); 
				initValues.Append("';\n");
				initValues.Append("</script>");
				Page.RegisterStartupScript(scriptKey, initValues.ToString());
			}
		}

		/// <summary>
		/// Gets the paragraph styles.
		/// </summary>
		/// <value>The paragraph styles.</value>
		public ToolItemCollection ParagraphStyles
		{
			get
			{
				return base.Items;
			}
		}

	}
}
