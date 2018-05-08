using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ecn.activityengines
{
    public partial class EncryptionTestSubmitter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string templink = "http://localhost/ecn.activityengines/Click.aspx?" + Encrypt_UrlEncode_QueryString("b=524451&e=173427304&lid=1484182");
            //Response.Redirect(templink,true);  
            string templink = "http://localhost/ecn.activityengines/SClick.aspx?" + Encrypt_UrlEncode_QueryString("b=1568203&e=154605961&media=LinkedIn");
            Response.Redirect(templink, true);
            //string templink = "http://localhost/ecn.activityengines/SPreview.aspx?" + Encrypt_UrlEncode_QueryString("b=1568203&e=154605961&media=Twitter");
            //Response.Redirect(templink, true); 
            //string templink = "http://localhost/ecn.activityengines/SPreview.aspx?jfGhkPGguUkFLEDhgZLVvB3vh62cEWKdT9aVwTmz6UVkQswBcmO7WXCTmIwj1E1m&fb_source=message";
            //Response.Redirect(templink, true);
            //string templink = "localhost/ecn.activityengines/SPreview.aspx?jfGhkPGguUkFLEDhgZLVvB3vh62cEWKdT9aVwTmz6UVkQswBcmO7WXCTmIwj1E1m&fb_source=message";
            //Response.Redirect(templink, true);
        }

        private string Encrypt_UrlEncode_QueryString(string qstring)
        {
            KM.Common.Entity.Encryption ec = new KM.Common.Entity.Encryption();
            return System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(qstring, ec));
            //wgh - commented for testing
            //KM.Common.Encryption ec = new KM.Common.Encryption();// KM.Common.Encryption.Get();
            //ec.PassPhrase = "p$yaQat3?U@r5truX6Vepra++8?&68t8-uB9CuW?UtHaZapUJ-2e8&!3-du2AMA*";
            //ec.SaltValue = "7emAha2hEdrUCephekas3uzuje6uGasab5Axu5t64u8a*HEyUtr9pr+bra4uJeXE";
            //ec.HashAlgorithm = "SHA1";
            //ec.PasswordIterations = 2;
            //ec.InitVector = "d3EdrEp=ucR-cAwr";
            //ec.KeySize = 256;
            //return System.Web.HttpContext.Current.Server.UrlEncode(ec.Encrypt(qstring, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize));
            return "";
        }
    }
}