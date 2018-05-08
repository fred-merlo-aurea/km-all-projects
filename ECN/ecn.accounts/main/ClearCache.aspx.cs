using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace ecn.accounts.main
{
    public partial class ClearCache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCacheGrid();

        }

        private void LoadCacheGrid()
        {
            List<string> keys = new List<string>();
            foreach (DictionaryEntry CachedItem in Cache)
            {
                keys.Add(CachedItem.Key.ToString());
            }


            gvCache.DataSource = keys;
            gvCache.DataBind();

            //for (int i = 0; i < keys.Count; i++)
            //    Cache.Remove(keys[i]);
        }

        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry CachedItem in Cache)
            {
                string CacheKey = CachedItem.Key.ToString();
                Cache.Remove(CacheKey);
            }

            Response.Write("All items removed from Cache");
        }
    }
}