using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using KM.Common;

namespace KMPS_JF.Forms
{
    public partial class ClearCache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                foreach (DictionaryEntry CachedItem in Cache)
                {
                    string CacheKey = CachedItem.Key.ToString();
                    Response.Write(CacheKey + "<BR>");
                }

                CacheUtil.FlushRegion("JOINTFORMS");
            }
        }

        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry CachedItem in Cache)
            {
                string CacheKey = CachedItem.Key.ToString();
                Cache.Remove(CacheKey); 
            }

            CacheUtil.FlushRegion("JOINTFORMS");

            Response.Write("All items removed from Cache");
        }
    }
}
