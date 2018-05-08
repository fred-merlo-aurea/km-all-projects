using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UAD.DataCompare.Web.Controllers.Common
{
    [Authorize]
    public class BaseController : Controller
    {
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
                return ((FormsIdentity)System.Web.HttpContext.Current.User.Identity).Ticket;
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
