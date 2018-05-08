using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmailMarketing.Service.Models;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using System.Text;
//using ECN_Framework_Common;
using ECN_Framework_Entities;
using ECN_Framework_BusinessLayer;
using ecn.webservice.classes;

namespace EmailMarketing.Service.Controllers
{
    public class MessageController : ApiController
    {
        private ECN_Framework_Entities.Communicator.APILogging Log = null;
        private static string ServiceName = "ecn.webservice.ContentManager.";
        private static string MethodName = string.Empty;
        private static int? LogID = null;

        // Retrieving Defined XML Services from the database   LayoutIDs: 281570, 282144
        private static string messageReturned = GetXMLMessage("9B3A9216-911E-4512-847C-CD0739B6BF31", 281570);

        // Retrieving Message Data
        public Message[] message = new Message[]
        {
            new Message {
                            MessageResponse = messageReturned
                        }
        };

        public IEnumerable<Message> GetMessage()
        {
            return message;
        }

        #region Get Message - GetMessage()
        public static string GetXMLMessage(string ecnAccessKey, int layoutID)
        {
            ECN_Framework_Entities.Communicator.APILogging Log = new ECN_Framework_Entities.Communicator.APILogging();

            try
            {
                MethodName = "GetMessage";
                Log = new ECN_Framework_Entities.Communicator.APILogging();
                Log.AccessKey = ecnAccessKey;
                Log.APIMethod = ServiceName + MethodName;
                Log.Input = "<ROOT><LayoutID>" + layoutID.ToString() + "</LayoutID></ROOT>";
                Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                ECN_Framework_Entities.Accounts.User user = ECN_Framework_BusinessLayer.Accounts.User.GetByAccessKey(ecnAccessKey, true);
                if (user != null)
                {
                    List<ECN_Framework_Entities.Communicator.Layout> layoutList = new List<ECN_Framework_Entities.Communicator.Layout>();
                    layoutList.Add(ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutID, user, false));
                    if (layoutList.Count > 0 && layoutList[0] != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Success, 0, BuildLayoutReturnXML(layoutList));
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LAYOUT DOESN'T EXIST");
                    }
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }
        #endregion

        private static int LogUnspecifiedException(Exception ex, string sourceMethod)
        {
            return KM.Common.Entity.ApplicationLog.LogCriticalError(ex, sourceMethod, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
        }

        private static string BuildLayoutReturnXML(List<ECN_Framework_Entities.Communicator.Layout> layoutList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<DocumentElement xmlns=\"\">");
            foreach (ECN_Framework_Entities.Communicator.Layout layout in layoutList)
            {
                sb.Append("<Layout><LayoutID>");
                sb.Append(layout.LayoutID);
                sb.Append("</LayoutID><FolderID>");
                if (layout.FolderID != null)
                    sb.Append(layout.FolderID.Value);
                sb.Append("</FolderID><TemplateID>");
                if (layout.TemplateID != null)
                    sb.Append(layout.TemplateID.Value);
                sb.Append("</TemplateID><LayoutName>");
                sb.Append(layout.LayoutName);
                sb.Append("</LayoutName><ModifyDate>");
                if (layout.UpdatedDate != null)
                    sb.Append(layout.UpdatedDate.Value);
                else if (layout.CreatedDate != null)
                    sb.Append(layout.CreatedDate.Value);
                sb.Append("</ModifyDate><ContentSlot1>");
                if (layout.ContentSlot1 != null)
                    sb.Append(layout.ContentSlot1.Value);
                sb.Append("</ContentSlot1><ContentSlot2>");
                if (layout.ContentSlot2 != null)
                    sb.Append(layout.ContentSlot2.Value);
                sb.Append("</ContentSlot2><ContentSlot3>");
                if (layout.ContentSlot3 != null)
                    sb.Append(layout.ContentSlot3.Value);
                sb.Append("</ContentSlot3><ContentSlot4>");
                if (layout.ContentSlot4 != null)
                    sb.Append(layout.ContentSlot4.Value);
                sb.Append("</ContentSlot4><ContentSlot5>");
                if (layout.ContentSlot5 != null)
                    sb.Append(layout.ContentSlot5.Value);
                sb.Append("</ContentSlot5><ContentSlot6>");
                if (layout.ContentSlot6 != null)
                    sb.Append(layout.ContentSlot6.Value);
                sb.Append("</ContentSlot6><ContentSlot7>");
                if (layout.ContentSlot7 != null)
                    sb.Append(layout.ContentSlot7.Value);
                sb.Append("</ContentSlot7><ContentSlot8>");
                if (layout.ContentSlot8 != null)
                    sb.Append(layout.ContentSlot8.Value);
                sb.Append("</ContentSlot8><ContentSlot9>");
                if (layout.ContentSlot9 != null)
                    sb.Append(layout.ContentSlot9.Value);
                sb.Append("</ContentSlot9></Layout>");
            }
            sb.Append("</DocumentElement>");
            return sb.ToString();
        }


    }
}
