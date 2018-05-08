using System;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace ActiveUp.WebControls
{
	internal class IconEditor : UITypeEditor
	{

		internal FileDialog fileDialog;

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (fileDialog == null)
			{
				fileDialog = new OpenFileDialog();
				fileDialog.Filter = "Bitmap files|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.ico";
			}
	
			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				FileStream fileStream = new FileStream(fileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				
				ActiveUp.WebControls.Icon icon = new ActiveUp.WebControls.Icon(fileDialog.FileName);
				value = icon;
			}
					
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		protected virtual Image LoadFromStream(Stream stream)
		{
			byte[] bs = new byte[(uint)stream.Length];
			stream.Read(bs, 0, (int)stream.Length);
			return Image.FromStream(new MemoryStream(bs));
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is ActiveUp.WebControls.Icon)
			{
						
				if (((ActiveUp.WebControls.Icon)e.Value).FileName != null)
				{
					Stream stream = null;

					if (((ActiveUp.WebControls.Icon)e.Value).FileName.IndexOf("ActiveTreeView.") >= 0)
					{
						Assembly asm = Assembly.GetExecutingAssembly();
						string[] s = asm.GetManifestResourceNames();
						stream = asm.GetManifestResourceStream(((ActiveUp.WebControls.Icon)e.Value).FileName);
					}

					else
					{
						FileStream fileStream = new FileStream(((ActiveUp.WebControls.Icon)e.Value).FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
						stream = fileStream;
					}

					if (stream != null)
					{
						byte[] bs = new byte[(uint)stream.Length];
						stream.Read(bs, 0, (int)stream.Length);

						Image image = Image.FromStream(new MemoryStream(bs));
						Rectangle rectangle = e.Bounds;
						rectangle.Width = rectangle.Width - 1;
						rectangle.Height = rectangle.Height - 1;
						e.Graphics.DrawRectangle(SystemPens.WindowFrame, rectangle);
						e.Graphics.DrawImage(image, e.Bounds);
					}
				}

				else
				{
					Rectangle rectangle = e.Bounds;
					rectangle.Width = rectangle.Width - 1;
					rectangle.Height = rectangle.Height - 1;
					e.Graphics.DrawRectangle(SystemPens.WindowFrame, rectangle);
				}
			}

		}

	}
}
