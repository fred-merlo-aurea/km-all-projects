using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC.Standards.Common
{
    public class BaseController : Controller
    {
        protected List<FrameworkUAD_Lookup.Entity.Code> CodeList
        {
            get
            {
                if (Session["BaseControlller_CodeList"] == null)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    Session["BaseControlller_CodeList"] = codeWorker.Select();
                }

                return (List<FrameworkUAD_Lookup.Entity.Code>) Session["BaseControlller_CodeList"];
            }
            set
            {
                Session["BaseControlller_CodeList"] = value;
            }
        }
        protected KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }
        protected int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }
        protected int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }
        protected List<KMPlatform.Entity.Client> AdminClientList
        {
            get
            {
                if (Session["BaseControlller_ClientList"] == null)
                {
                    KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
                    Session["BaseControlller_ClientList"] = cWrk.SelectActiveForClientGroupLite(CurrentClientGroupID).OrderBy(x => x.ClientName).ToList();//cWrk.Select(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID);
                }

                return (List<KMPlatform.Entity.Client>) Session["BaseControlller_ClientList"];
            }
            set
            {
                Session["BaseControlller_ClientList"] = value;
            }
        }
        protected KMPlatform.Entity.Client CurrentClient
        {
            get
            {
                if (Session["BaseControlller_CurrentClient"] == null)
                {
                    KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
                    Session["BaseControlller_CurrentClient"] = cWrk.Select(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID);
                }

                return (KMPlatform.Entity.Client) Session["BaseControlller_CurrentClient"];
            }
            set
            {
                Session["BaseControlller_CurrentClient"] = value;
            }
        }
        protected List<KMPlatform.Entity.ClientGroup> ClientGroupList
        {
            get
            {
                if (Session["BaseControlller_ClientGroupList"] == null)
                {
                    KMPlatform.BusinessLogic.ClientGroup cgWorker = new KMPlatform.BusinessLogic.ClientGroup();
                    Session["BaseControlller_ClientGroupList"] = cgWorker.SelectForAMSWithClientList(false);
                }

                return (List<KMPlatform.Entity.ClientGroup>) Session["BaseControlller_ClientGroupList"];
            }
            set
            {
                Session["BaseControlller_ClientGroupList"] = value;
            }
        }
        protected int CurrentProductID
        {
            get
            {
                if (Session["BaseController_UASWebProductID"] == null)
                {
                    Session["BaseController_UASWebProductID"] = 0;
                }

                return (int) Session["BaseController_UASWebProductID"];
            }
            set
            {
                Session["BaseController_UASWebProductID"] = value;
            }
        }
        protected List<ECN_Framework_Entities.Accounts.BaseChannel> BaseChannelList
        {
            get { return ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll(); }
        }
        protected string ApiKey
        {
            get
            {
                return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.AccessKey.ToString();// Ticket.UserData;
            }
        }
        protected int UserID
        {
            get
            {
                return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().UserID;// int.Parse(Ticket.Name);
            }
        }
        private FormsAuthenticationTicket Ticket
        {
            get
            {
                return ((FormsIdentity) System.Web.HttpContext.Current.User.Identity).Ticket;
            }
        }

        protected JavaScriptRedirectResult JavaScriptRedirectToAction(string actionName)
        {
            return new JavaScriptRedirectResult(actionName);
        }
        protected JavaScriptRedirectResult JavaScriptRedirectToAction(string actionName, object routeValues)
        {
            return new JavaScriptRedirectResult(actionName, routeValues);
        }
        protected JavaScriptRedirectResult JavaScriptRedirectToAction(string actionName, string controllerName)
        {
            return new JavaScriptRedirectResult(actionName, controllerName);
        }
        protected JavaScriptRedirectResult JavaScriptRedirectToAction(string actionName, string controllerName, object routeValues)
        {
            return new JavaScriptRedirectResult(actionName, controllerName, routeValues);
        }
    }
}
