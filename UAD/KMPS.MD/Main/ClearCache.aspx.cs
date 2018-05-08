using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Web; 
using System.Web.UI; 
using System.Web.UI.WebControls; 
using System.Collections;
using KMPS.MD.Objects;

namespace KMPS.MD.Main
{
    public partial class ClearCache : KMPS.MD.Main.WebPageHelper 
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
            } 
        } 
 
        protected void btnClearCache_Click(object sender, EventArgs e) 
        { 
            foreach (DictionaryEntry CachedItem in Cache) 
            { 
                string CacheKey = CachedItem.Key.ToString(); 
                Cache.Remove(CacheKey);  
            }

            List<Pubs> lp = Pubs.GetAll(Master.clientconnections);

            foreach (Pubs p in lp)
            {
                List<ResponseGroup> lrg = KMPS.MD.Objects.ResponseGroup.GetByPubID(Master.clientconnections, p.PubID);

                foreach (ResponseGroup rg in lrg)
                {
                    CodeSheet.DeleteCache(Master.clientconnections, rg.ResponseGroupID);
                }

                KMPS.MD.Objects.ResponseGroup.DeleteCache(Master.clientconnections, p.PubID);
            }

            MasterGroup.DeleteCache(Master.clientconnections);
            MasterCodeSheet.DeleteCache(Master.clientconnections);
            Pubs.DeleteCache(Master.clientconnections);
 
            Response.Write("All items removed from Cache"); 
        } 
    } 
}  