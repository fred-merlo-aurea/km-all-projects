namespace ecn.accounts.includes
{
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
                try
                {
                    if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imagefile)))
                    {
                        sendFile();
                    }
                    else
                    {
                        sendError();
                    }
                }
                catch (Exception)
                {
                    sendError();
                }
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

        public void sendFile()
        {
            //create new image and bitmap objects. Load the image file and put into a resized bitmap.
            System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imagefile));
            ImageFormat thisformat = g.RawFormat;
            int its = Convert.ToInt32(thumbnailsize);
            Size thumbSize = newthumbSize(g.Width, g.Height, its);
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


        public void sendError()
        {
            //if no height, width, src then output "error"
            int its = Convert.ToInt32(thumbnailsize);
            Bitmap imgOutput = new Bitmap(its, its, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(imgOutput); //create a new graphic object from the above bmp
            g.Clear(Color.Yellow); //blank the image
            g.DrawString("ERROR!", new Font("verdana", 9, FontStyle.Bold), SystemBrushes.WindowText, new PointF(2, 2));
            //set the contenttype
            Response.ContentType = "image/gif";
            //send the resized image to the viewer
            imgOutput.Save(Response.OutputStream, ImageFormat.Gif); //output to the user
            //tidy up
            g.Dispose();
            imgOutput.Dispose();
        }
    }
}
