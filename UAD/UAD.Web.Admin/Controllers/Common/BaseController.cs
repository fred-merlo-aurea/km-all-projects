using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FrameworkUAD.Object;
using KM.Common;

namespace UAD.Web.Admin.Controllers.Common
{
    [CustomAuthorizeAttribute]
    public class BaseController : Controller
    {
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

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        protected KMPlatform.Object.ClientConnections ClientConnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    _clientconnections = new KMPlatform.Object.ClientConnections(CurrentUser.CurrentClient);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
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

                return (List<KMPlatform.Entity.Client>)Session["BaseControlller_ClientList"];
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
                KMPlatform.Entity.Client client = (KMPlatform.Entity.Client)Session["BaseControlller_CurrentClient"];
                if (Session["BaseControlller_CurrentClient"] == null || client.ClientID != ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID)
                {
                    KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
                    Session["BaseControlller_CurrentClient"] = cWrk.Select(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID);
                }

                return (KMPlatform.Entity.Client)Session["BaseControlller_CurrentClient"];
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

                return (List<KMPlatform.Entity.ClientGroup>)Session["BaseControlller_ClientGroupList"];
            }
            set
            {
                Session["BaseControlller_ClientGroupList"] = value;
            }
        }

        protected JsonResult DeleteInternal(string itemName, Action deleteItemAction)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            try
            {
                deleteItemAction();
                messageToView.Text = $"{itemName} has been deleted successfully.";
                messageToView.Success = true;
                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (UADException ex)
            {
                if (ex.ErrorList.Any())
                {
                    messageToView.Text = ex.ErrorList[0].ErrorMessage;
                }
                else
                {
                    messageToView.Text = "Unexpected error occurred";
                }

                messageToView.Success = false;
                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        protected JsonResult GetErrorJsonResult(UADException exception)
        {
            Guard.NotNull(exception, nameof(exception));

            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            var errorStringBuilder = new StringBuilder();
            foreach (var er in exception.ErrorList)
            {
                errorStringBuilder.AppendLine(er.ErrorMessage + "<br/>");
            }

            messageToView.Text = errorStringBuilder.ToString();
            messageToView.Success = false;
            var json = serializer.Serialize(messageToView);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}