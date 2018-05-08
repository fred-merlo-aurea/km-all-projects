using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KMPS.MD.Objects;

namespace KMPS.MD.Main.Widgets
{
    /// <summary>
    /// Summary description for SearchProduct
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SearchProduct : System.Web.Services.WebService
    {
        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }

        public SearchProduct()
        {
        }
        
        [WebMethod(EnableSession = true)]
        public string[] GetProductList(string prefixText, int count)
        {
            List<Pubs> pubList = Pubs.GetSearchEnabled(clientconnections);
                        
            List<string> items = new List<string>(count);

            foreach (Pubs p in pubList)
            {
                if (p.PubName.ToLower().Contains(prefixText.ToLower()))
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(p.PubName, p.PubID.ToString());
                    items.Add(item);
                }
            }

            return items.ToArray();
        }
    }
}
