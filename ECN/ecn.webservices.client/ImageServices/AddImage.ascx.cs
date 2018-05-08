using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace ecn.webservices.client.ImageServices
{
    public partial class AddImage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //for testing
                FileInfo fInfo = new FileInfo(@"c:\temp\34293_1413154542070_1628736174_926716_6566178_n.jpg");
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(@"c:\temp\34293_1413154542070_1628736174_926716_6566178_n.jpg", FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                byte[] encodedImageTest = br.ReadBytes((int)numBytes);                
                br.Close();
                fStream.Close();
                fStream.Dispose();
                //end testing


                txtReturn.Text = "";
                //ImageManager_localhost.ImageManager ws = new ecn.webservices.client.ImageManager_localhost.ImageManager();
                ImageManager_PROD.ImageManager ws = new ecn.webservices.client.ImageManager_PROD.ImageManager();
                //ImageManager_Test.ImageManager ws = new ecn.webservices.client.ImageManager_Test.ImageManager();
                if (txtFolder.Text.Trim().Length > 0)
                {
                    txtReturn.Text = ws.AddImage(txtAccessKey.Text.Trim(), encodedImageTest, txtImageName.Text.Trim(), txtFolder.Text.Trim());
                }
                else
                {
                    txtReturn.Text = ws.AddImage(txtAccessKey.Text.Trim(), encodedImageTest, txtImageName.Text.Trim());
                }
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}