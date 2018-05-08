using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ActiveUp.WebControls;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ImageHandler"/> object.
	/// </summary>
	public class ImageHandler : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			//Set the content type
			Response.ContentType="image/jpeg";
			ImageJob job;

			try
			{
				string file = Request.QueryString["file"];

				job = new ImageJob(Server.MapPath(file));

				// Process the image
				foreach(string command in Request.QueryString.Keys)
				{
					// Tolerance is a vertue.
					string commandUC = command.ToUpper();

					// Parameters container.
					string values = string.Empty;

					// Add text
					if (commandUC.StartsWith("TEXT"))
					{
						values = Request.QueryString[command];
						string[] prms = values.Split(';');

						// We need some explicit conversion here.
						FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), prms[3], true);
						StringAlignment alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), prms[8], true);;

						job.AddText(prms[0], prms[1], Convert.ToInt32(prms[2]), fontStyle,
							Color.FromName(prms[4]), Convert.ToBoolean(prms[5]),
							Convert.ToInt32(prms[6]), Convert.ToInt32(prms[7]), alignment);
					}
					else if (commandUC.StartsWith("ROTATE"))
					{
						values = Request.QueryString[command];
						if (values.ToUpper() == "LEFT")
							job.RotateLeft();
						else if (values.ToUpper() == "RIGHT")
							job.RotateRight();
						else
							job.Rotate(float.Parse(values));
					}
					else if (commandUC.StartsWith("FLIP"))
					{
						values = Request.QueryString[command];
						if (values.ToUpper().StartsWith("HORI"))
							job.Flip(FlipType.Horizontal);
						else if (values.ToUpper().StartsWith("VERT"))
							job.Flip(FlipType.Vertical);
						else if (values.ToUpper().StartsWith("BOTH"))
							job.Flip(FlipType.Both);
					}
					else if (commandUC.StartsWith("CROP"))
					{
						values = Request.QueryString[command];
						Selection selection = new Selection(values.Replace(";",","));
						job.Crop(selection);
					}
					else if (commandUC.StartsWith("RESIZE"))
					{
						values = Request.QueryString[command];
						string[] prms = values.Split(';');
						job.ResizeImage(Convert.ToInt32(prms[0]), Convert.ToInt32(prms[1]),
							Convert.ToBoolean(prms[2]), Convert.ToBoolean(prms[3]));
					}
					else if (commandUC.StartsWith("CANVAS"))
					{
						values = HttpUtility.UrlDecode(Request.QueryString[command]);
						string[] prms = values.Split(';');
						
						// We need some explicit conversion here.
						AnchorType anchor = (AnchorType)Enum.Parse(typeof(AnchorType), prms[2], true);

						job.ResizeCanvas(Convert.ToInt32(prms[0]), Convert.ToInt32(prms[1]),
							anchor, System.Drawing.Color.FromName(prms[3]));
					}
					else if (commandUC.StartsWith("ZOOM"))
					{
						values = Request.QueryString[command];
						string[] prms = values.Split(';');

						job.Zoom(Convert.ToInt32(prms[0]), Convert.ToInt32(prms[1]),
							float.Parse(prms[2]));
					}
				}

				// Send the data to the browser
				job.Save(Response.OutputStream, FileCompression.CCITT3, 70, FileFormat.Jpeg);
			}
			catch
			{
				Bitmap bitMapImage = new System.Drawing.Bitmap(300, 100, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				Graphics graphicImage = Graphics.FromImage( bitMapImage );

				graphicImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

				SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Red);
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;

				graphicImage.DrawString("Invalid parameters!", new Font("Arial", 23,FontStyle.Bold ), drawBrush, new Point( 150, 30 ), stringFormat );

				bitMapImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

				graphicImage.Dispose();
				bitMapImage.Dispose();
			}
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
