using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolFontSize"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolFontSize : ToolDropDownList
	{
		private readonly string TOOLFONTSIZEKEY = "ToolFontSize";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFontSize"/> class.
		/// </summary>
		public ToolFontSize() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFontSize"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolFontSize(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolFontSize" + Editor.indexTools++;
			else
				this.ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Width = Unit.Parse("50px");
			this.Text = "Size";
			this.Height = Unit.Parse("22px");

#if (!FX1_1)
            this.DropDownImage = string.Empty;
#endif

            
			//this.ClientSideClick = "if (HTB_ie5) HTB_CommandBuilder('$EDITOR_ID$', 'fontsize', ATB_getSelectedValue('$CLIENT_ID$')); else HTB_Ns6FontSizeClicked('$EDITOR_ID$','$CLIENT_ID$');";
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
				this.ClientSideClick = string.Format("if (HTB_ie5) HTB_CommandBuilder('{0}', 'fontsize', ATB_getSelectedValue('{1}')); else HTB_Ns6FontSizeClicked('{0}','{1}');",editor.ClientID,this.ClientID);
			}
			//this.ClientSideClick = this.ClientSideClick.Replace("$CLIENT_ID$", this.ClientID);

			base.Items.Clear();
			string hiddenSize = string.Empty;

			if (Sizes.Count == 0)
			{
				this.Items.Add(new ToolItem("1","1"));
				this.Items.Add(new ToolItem("2","2"));
				this.Items.Add(new ToolItem("3","3"));
				this.Items.Add(new ToolItem("4","4"));
				this.Items.Add(new ToolItem("5","5"));
				this.Items.Add(new ToolItem("6","6"));
				this.Items.Add(new ToolItem("7","7"));
				
				if (isNs6)
					hiddenSize = "1;2;3;4;5;6;7";
			}
			else
			{
				foreach(object size in Sizes)
				{
					if (!(size is string))
						throw new InvalidCastException("Sizes must contains only string object.");

					this.Items.Add(new ToolItem((string)size,(string)size));

					if (isNs6)
						hiddenSize += size.ToString() + ";";
				}

				hiddenSize = hiddenSize.TrimEnd(';');
						
			}

			if (isNs6)
			{
                if (System.Web.HttpContext.Current != null)
                {
                    this.Overflow = "auto";
                }

				string scriptKey = this.ClientID + "_" + TOOLFONTSIZEKEY + "_Init";
				System.Text.StringBuilder initValues = new System.Text.StringBuilder();
				initValues.Append("<script language='javascript'>\n");
				initValues.Append("var ");
				initValues.Append(this.ClientID);
				initValues.Append("_Values = '");
				initValues.Append(hiddenSize); 
				initValues.Append("';\n");
				initValues.Append("</script>");
				Page.RegisterStartupScript(scriptKey, initValues.ToString());
			}
			
		}

		/// <summary>
		/// Gets or sets the ArrayList containing the font sizes..
		/// </summary>
		/// <value>The sizes.</value>
		[
			Bindable(false),
			Category("Data"),
			Description("Get or set the ArrayList containing the font sizes.")
		]
		public ArrayList Sizes
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(Sizes), new ArrayList());
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(Sizes), value);
			}
		}
	}
}
