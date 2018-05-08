namespace ecn.creator.includes {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	
	///		Summary description for thumbnail.

    public partial class thumbnail : System.Web.UI.Page
    {
        public string imagefile = "";
        public string thumbnailsize = "50";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            getRequestVars();
            //make sure nothing has gone to the client
            Response.Clear();
            if (imagefile == "")
            {
                sendError();
            }
            else
            {
                if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imagefile)))
                    sendFile();
                else
                    sendError();
            }
            Response.End();
        }

        private void getRequestVars()
        {
            try
            {
                imagefile = Request.QueryString["image"].ToString();
                thumbnailsize = Request.QueryString["size"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
        }

        public Size newthumbSize(int currentwidth, int currentheight, int newsize)
        {
            // Calculate the Size of the new image
            int thewidth = 0;
            int theheight = 0;

            if (currentheight <= 100 && currentwidth <= 175)
            {
                Size newSize = new Size(currentwidth, currentheight);
                return newSize;
            }
            else
            {

                if (currentheight > currentwidth)
                {
                    //portrait
                    theheight = newsize;
                    thewidth = Convert.ToInt32(((double)newsize / (double)currentheight) * (double)currentwidth);
                }
                else
                {
                    //landscape
                    theheight = Convert.ToInt32(((double)newsize / (double)currentwidth) * (double)currentheight);
                    thewidth = newsize;
                }
                if (thewidth > 0 && theheight > 0)
                {
                    Size newSize = new Size(thewidth, theheight);
                    return newSize;
                }
                else
                {
                    Size defaultSize = new Size(1, 1);
                    return defaultSize;
                }
            }
        }

        public void sendFile()
        {
            //create new image and bitmap objects. Load the image file and put into a resized bitmap.
            try
            {
                // Response.Write(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imagefile));

                System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imagefile));
                ImageFormat thisformat = g.RawFormat;
                int its = Convert.ToInt32(thumbnailsize);
                Size thumbSize = newthumbSize(g.Width, g.Height, its);

                if (g.Width == thumbSize.Width || g.Height == thumbSize.Height)
                {
                    g.Save(Response.OutputStream, thisformat);
                    g.Dispose();
                }
                else
                {
                    //Response.Write(thumbSize.Width.ToString());
                    Bitmap imgOutput = new Bitmap(g, thumbSize.Width, thumbSize.Height);
                    //set the contenttype
                    if (thisformat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                    {
                        Response.ContentType = "image/gif";
                    }
                    else
                    {
                        Response.ContentType = "image/jpeg";
                    }
                    //send the resized image to the viewer
                    imgOutput.Save(Response.OutputStream, thisformat); //output to the user
                    //tidy up
                    g.Dispose();
                    imgOutput.Dispose();
                }
            }
            catch
            {
                sendError();
            }
        }


        public void sendError()
        {
            int its = Convert.ToInt32(thumbnailsize);
            Bitmap imgOutput = new Bitmap(150, 150, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(imgOutput); //create a new graphic object from the above bmp
            g.Clear(Color.Yellow); //blank the image
            g.DrawString("NOT VALID IMAGE", new Font("arial", 9, FontStyle.Bold), SystemBrushes.WindowText, new PointF(2, 2));
            Response.ContentType = "image/gif";
            imgOutput.Save(Response.OutputStream, ImageFormat.Gif); //output to the user
            g.Dispose();
            imgOutput.Dispose();
        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }
}
