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
	/// Represents a <see cref="ToolCodeSnippets"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolCodeSnippets : ToolDropDownList
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCodeSnippets"/> class.
		/// </summary>
		public ToolCodeSnippets() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCodeSnippets"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolCodeSnippets(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolCodeSnippets" + Editor.indexTools++;
			else
				this.ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Text = "Code Snippets";
			this.Height = Unit.Parse("22px");
			//this.ClientSideClick = "HTB_SetSnippet(\'$EDITOR_ID$\', ATB_getSelectedValue('$CLIENT_ID$'));";
		}

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			
			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
			{
				//this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
				this.ClientSideClick = string.Format("HTB_SetSnippet(\'{0}\', ATB_getSelectedValue('{1}'));",editor.ClientID,this.ClientID);
			}
			//this.ClientSideClick = this.ClientSideClick.Replace("$CLIENT_ID$", this.ClientID);
			
			if (CustomTitle != string.Empty)
				this.Text = CustomTitle;

			if (Snippets.Count == 0)
			{
				this.Items.Add(new ToolItem("<b>No snippet defined</b>",string.Empty));
			}

            base.OnPreRender(e);
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			base.Render(output);
		}

		/// <summary>
		/// Gets the snippets.
		/// </summary>
		/// <value>The snippets.</value>
		public ToolItemCollection Snippets
		{
			get
			{
				return base.Items;
			}
		}

		/// <summary>
		/// Gets or sets the custom title.
		/// </summary>
		/// <value>The custom title.</value>
		public string CustomTitle 
		{
			set
			{
				ViewState["CustomTitle"] = value;
			}

			get
			{
				object customTitle = ViewState["CustomTitle"];
				if (customTitle == null)
					return string.Empty;
				return (string)customTitle;
			}
		}

		

	}
}
