using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolUpdate"/> object.
	/// </summary>
	[ToolboxItem(false)]
	public class ToolUpdate : ToolButton,IPostBackEventHandler, IPostBackDataHandler
	{
		private const string ImageUrlResourceName = "ActiveUp.WebControls._resources.Images.save_off.gif";
		private const string OverImageUrlResourceName = "ActiveUp.WebControls._resources.Images.save_over.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolUpdate"/> class.
		/// </summary>
		public ToolUpdate() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolUpdate"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolUpdate(string id) : base(id)
		{
			_Init(id);
		}
		 
		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolUpdate" + Editor.indexTools++;
			else
				this.ID = id;
#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "save_off.gif";
			this.OverImageURL = "save_over.gif";
#endif
			this.ToolTip = "Save";
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
		public override void RenderDesign(HtmlTextWriter output)
		{
			SetImageUrl(ImageUrlResourceName, OverImageUrlResourceName);
			this.RenderControl(output);
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary> 
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			var editor = this.Parent.Parent.Parent as Editor;
			ToolOnPreRender(ImageUrlResourceName, OverImageUrlResourceName, editor?.ClientID);
			base.OnPreRender(e);
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			if (SaveClicked != null)
				this.Click += SaveClicked;
			base.Render(output);
		}

		#region Events
		
		/// <summary>
		/// The SaveClicked event handler. Fire when you click.
		/// </summary>
		[Category("Event")]
		public event EventHandler SaveClicked;

		/// <summary>
		/// A OnSaveClicked event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSaveClicked(EventArgs e) 
		{
			if (SaveClicked != null)
				SaveClicked(this,e);
		}

		#endregion

		#region Interface IPostBack

		/// <summary>
		/// A RaisePostBackEvent.
		/// </summary>
		/// <param name="eventArgument">eventArgument</param>
		public new void RaisePostBackEvent(String eventArgument)
		{
			Page.Trace.Write(ID,"RaisePostBackEvent");
			OnSaveClicked(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Write(ID,"LoadPostData");
			return false;
		}

		#endregion
	}
}
