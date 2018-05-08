using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Facebook;

namespace ecn.communicator.main.dripmarketing
{
    public partial class editDripCampaign : ECN_Framework.WebPageHelper
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    CheckAuthorization();
        //}

        //private void CheckAuthorization()
        //{
        //    string app_id = "630229463672996";
        //    string app_secret = "741859ce9250e3be648ea13534368be1";
        //    string scope = "publish_stream,manage_pages";
        //    string access_token = "";

        //    //Rohit-Profile Access Token for MyApp1
        //    //string access_token = "CAAI9MLtPKKQBAD25EK2uA5SlkIRxoC9ip1Nao1ddvcq8HEPx3u86RbMwc9ustqdm2IlZAjWSQqCTt6zDFTtg2dvqLavpNgC201SSkn7CZCdEdxS1tuW0yqr19rzgW0NZAnSKck3HpJbdbCG3ZCSuAPOMT6VZAo7UZD&expires=5182392";

        //    if (Request["code"] == null)
        //    {
        //        Response.Redirect(string.Format(
        //            "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}",
        //            app_id, Request.Url.AbsoluteUri, scope));
        //    }
        //    else
        //    {
        //        Dictionary<string, string> tokens = new Dictionary<string, string>();

        //        string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}",
        //            app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);

        //        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

        //        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        //        {
        //            StreamReader reader = new StreamReader(response.GetResponseStream());

        //            string vals = reader.ReadToEnd();

        //            foreach (string token in vals.Split('&'))
        //            {
        //                tokens.Add(token.Substring(0, token.IndexOf("=")),
        //                    token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
        //            }
        //        }

        //        access_token = tokens["access_token"];
            
        //        //string access_token = "CAAI9MLtPKKQBAD25EK2uA5SlkIRxoC9ip1Nao1ddvcq8HEPx3u86RbMwc9ustqdm2IlZAjWSQqCTt6zDFTtg2dvqLavpNgC201SSkn7CZCdEdxS1tuW0yqr19rzgW0NZAnSKck3HpJbdbCG3ZCSuAPOMT6VZAo7UZD&expires=5182392";


        //        FacebookClient client = new FacebookClient(access_token);

        //        var postID= client.Post("/me/feed", new { message = "test post from access token" });
        //        Response.Write(postID.ToString());
        //        var comments = client.Get("/585789142_10151482699514143/comments");
        //        var likes = client.Get("/585789142_10151482699514143/likes");
        //        Response.Write(likes.ToString());
        //    }
        //}


    }
}