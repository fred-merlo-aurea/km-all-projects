using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ecn.qatools.Infrastructure.ExtentionMethods;
using System.IO;

namespace ecn.qatools.Controllers
{
    public class HomeController : Controller
    {
        DefaultApiController api { get; set; }

        IEnumerable<string> getFileList()
        {
            return Directory.EnumerateFiles(Server.MapPath(@"~/saved"), "*.json", SearchOption.TopDirectoryOnly);
        }
        Models.IndexViewModel makeModel()
        {
            return new Models.IndexViewModel
            {
                Params = api.GetAll(),
                Saved = getFileList()
            };
        }

        public HomeController(Infrastructure.Abstract.IKeyValueDataStore dataStore)
        {
            api = new DefaultApiController(dataStore);
        }

        public ActionResult Index()
        {
            return View(makeModel());
        }

        public ActionResult Edit()
        {
            return View(makeModel());
        }

        public ActionResult Css()
        {
            return View((object)api.Get("CSS"));
        }

        public ActionResult Show(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return View("Save", new { });
            }

            string filename = getFileList().FirstOrDefault(x => x.Contains(id));
            if(String.IsNullOrWhiteSpace(filename))
            {
                //return Redirect( Url.Action("Index"));
                return View("Save", new { });
            }
                
            return View("Save", Infrastructure.JsonFileLoader.Load(filename));
        }

        [AcceptVerbs( "GET", "POST", "PUT")]
        public ActionResult Save()
        {
            IDictionary<string, object> values = new ExpandoObject();
            if (Request.QueryString.AllKeys.Any())
            {
                foreach(string k in Request.QueryString.Keys)
                {
                    values[k] = Request.QueryString[ k ];
                }
            }
            if(Request.Form.AllKeys.Any())
            {
                foreach (string k in Request.Form.Keys)
                {
                    values[k] = Request.Form[k];
                }
            }

            values.SaveAsJson(Server.MapPath(String.Format(@"~/saved/{0}.json", DateTime.Now.ToString("yyyyMMdd_mmhhss"))));

            return View( values as dynamic );
        }
    }
}