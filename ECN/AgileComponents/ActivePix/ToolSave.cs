using ActiveUp.WebControls;
using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.IO;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolSave"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolSave : ToolButton,INamingContainer,IPostBackEventHandler,IPostBackDataHandler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSave"/> class.
		/// </summary>
		public ToolSave() : base()
		{
			_Init(string.Empty);			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSave"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolSave(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
			{
				this.ID = "_toolSave" + (ImageEditor.indexTools).ToString();
			}
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
			
			this.Click += new EventHandler(this.SaveClicked);
		}

		/// <summary>
		/// Gets the file name.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <returns></returns>
		public string GetFileName(string fileName)
		{
			int ndx = fileName.LastIndexOf("/");
			if (ndx > 0)
				return fileName.Substring(ndx + 1);

			ndx = fileName.LastIndexOf(@"\");
			if (ndx > 0)
				return fileName.Substring(ndx + 1);

			return fileName;
		}

		private void SaveClicked(object obj, EventArgs e)
		{
			// Get Parent Editor
			ImageEditor editor = (ImageEditor)this.Parent.Parent;
			string tempFile = editor.GetTempFileName();
			/*string tempFile = string.Empty;
			// Create the new name
			if (editor.SaveURL != string.Empty)
			{
				tempFile = GetFileName(editor.SaveURL);
				if (tempFile == string.Empty)
					tempFile = editor.GetTempFileName();
			}
			else
			{
				tempFile = editor.GetTempFileName();
			}*/

			if (!editor.DirectWrite)
			{
				ActiveUp.WebControls.ImageJob job;
				job = new ActiveUp.WebControls.ImageJob(editor.WorkFile);
				//job.License = editor.License;

				ImageSettings blankSettings = new ImageSettings();
				ImageSettings saveSettings = editor.SaveSettings;
				if (saveSettings != null && !blankSettings.Equals(saveSettings))
				{
					if (saveSettings.MaxHeight != 0)
						job.ResizeImage(saveSettings.MaxWidth, saveSettings.MaxWidth,
							saveSettings.ConstrainProportions, saveSettings.ResizeSmaller);
					job.Save(Page.Server.MapPath(editor.TempDirectory) + tempFile, saveSettings.Compression,
						saveSettings.Quality, saveSettings.Format);
				}
				else
					job.Save(Page.Server.MapPath(editor.TempDirectory) + tempFile, saveSettings.Compression,
						saveSettings.Quality, saveSettings.Format);
				
				editor.OnPostProcessing(EventArgs.Empty);

				//job.Save(Page.Server.MapPath(editor.TempDirectory) + tempFile, editor.SaveSettings.Compression, editor.SaveSettings.Quality, editor.SaveSettings.Format);

				job.Dispose();

				// Delete old temp file
				System.IO.File.Delete(Page.Server.MapPath(editor.TempURL));

				editor.TempURL = editor.TempDirectory + tempFile;
				//editor.ImageURL = editor.TempURL;
				if (editor.SaveURL != string.Empty)
					editor.ImageURL = editor.SaveURL;

				System.IO.File.Copy(Page.Server.MapPath(editor.TempURL), Page.Server.MapPath(editor.ImageURL), true);
			}

			editor.EditorMode = ImageEditorMode.View;

			editor.OnSave(this,Page.Server.MapPath(editor.ImageURL));
		}

		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
#if (!FX1_1)
            	if (ImageURL == string.Empty)
                	ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.save_off.gif");
                if (OverImageURL == string.Empty)
                	OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.save_over.gif");
#endif
            base.OnPreRender(e);

        }
	}
}
