using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS.MD.Main.Widgets
{
    public partial class ScreenSize : System.Web.UI.UserControl
    {
        private bool _isFirstTime = false;

        public bool _IsFirstTime
        {
            set { this._isFirstTime = value; }
            get { return this._isFirstTime; }
        }

        override protected void OnInit(EventArgs e)
        {
            if (Request.Cookies["screenInfo"] == null)
            {
                if (string.IsNullOrEmpty(Request.QueryString["_height"]) || string.IsNullOrEmpty(Request.QueryString["_width"]))
                {
                    this._IsFirstTime = true;
                }
                else
                {
                    HttpCookie screensize = new HttpCookie("screenInfo");
                    screensize.Values["ScreenHeight"] = Request.QueryString["_height"];
                    screensize.Values["ScreenWidth"] = Request.QueryString["_width"];
                    screensize.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(screensize);
                }
            }
            this.DataBind();
        }

        public string get_width()
        {
            if (Request.Cookies["screenInfo"] != null)
                return Request.Cookies["screenInfo"]["ScreenWidth"];
            else
                return "";
        }

        public string get_height()
        {
            if (Request.Cookies["screenInfo"] != null)
                return Request.Cookies["screenInfo"]["ScreenHeight"];
            else
                return "";
        }
 
    }
}