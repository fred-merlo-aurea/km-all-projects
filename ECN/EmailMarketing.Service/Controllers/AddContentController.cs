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
    public class AddContentController : ApiController
    {
        private ECN_Framework_Entities.Communicator.APILogging Log = null;
        private static string ServiceName = "ecn.webservice.ContentManager.";
        private static string MethodName = string.Empty;
        private static int? LogID = null;

        // Retrieving XML Content from the database  ContentIDs: 247491, 293196
        private static string contentResponseReturned = AddContentMain("9B3A9216-911E-4512-847C-CD0739B6BF31", "Test Title", "Content", "Content Text", 0);

        public Content[] addContentMessage = new Content[] 
        { 
            new Content {   
                            Response = contentResponseReturned  // "<DocumentElement xmlns=''><Content><ContentID>293198</ContentID><FolderID>0</FolderID><ContentTitle>MASTER6/24/2014 12:22:16 PM - Blast Content</ContentTitle><ModifyDate>6/24/2014 12:22:13 PM</ModifyDate><ContentSource><![CDATA[<br /> <table align='left' cellpadding='0' cellspacing='0' style='width: 604px; height: 289px;'> <tr> <td> <table cellpadding='0' cellspacing='0' style='width: 100%; height: 100%;'> <tr> <td style='height: 25px;'><img height='25' src='http://images.ecn5.com/images/surv_env_top.jpg' width='604' /></td> </tr> <tr> <td> <table cellpadding='0' cellspacing='0' style='width: 100%; height: 233px;'> <tr> <td style='width: 28px;'><img height='233' src='http://images.ecn5.com/images/surv_env_left.jpg' width='28' /></td> <td align='left' style='width: 280px; font-family: Arial, Helvetica, sans-serif; font-size: x-small; background-color: rgb(255, 255, 255);' valign='top'>Dear %%FirstName%%,<br /><br /><a href='http://test.ecn5.com/ecn.collector/front/default.aspx?sid=2305&amp;bid=%%blastid%%&amp;uid=%%EmailAddress%%>http://test.ecn5.com/ecn.collector/front/default.aspx?sid=2305&amp;bid=%%blastid%%&amp;uid=%%EmailAddress%%</a></td> <td style='width: 47px;'><img height='233' src='http://images.ecn5.com/images/surv_env_center.jpg' width='47' /></td> <td><a href='http://www.ecn5.com/ecn.collector/front/default.aspx?sid=2305&amp;bid=%%blastid%%&amp;uid=%%EmailAddress%%'><img border='0' height='233' src='http://images.ecn5.com/images/surv_env_button.jpg' width='209' /></a></td> <td style='width: 40px;'><img height='233' src='http://images.ecn5.com/images/surv_env_right.jpg' width='40' /></td> </tr> </table> </td> </tr> <tr> <td style='height: 32px;'><img border='0' height='32' src='http://images.ecn5.com/images/surv_env_bottom.jpg' width='604' /></td> </tr> </table> </td> </tr> </table> ]]></ContentSource><ContentText><![CDATA[MASTER6/24/2014 12:22:16 PM, http://www.ecn5.com/ecn.collector/front/default.aspx?sid=2305&bid=%%blastid%%&uid=%%EmailAddress%%6/24/2014 12:22:16 PM]]></ContentText></Content></DocumentElement>"
                        }
        };

        public IEnumerable<Content> GetAddContentMessage()
        {
            return addContentMessage;
        }

        private static string AddContentMain(string ecnAccessKey, string Title, string ContentHTML, string ContentText, int? FolderID)
        {
            ECN_Framework_Entities.Communicator.APILogging Log = new ECN_Framework_Entities.Communicator.APILogging();

            try
            {
                MethodName = "AddContentMain";
                Log = new ECN_Framework_Entities.Communicator.APILogging();
                Log.AccessKey = ecnAccessKey;
                Log.APIMethod = ServiceName + MethodName;
                Log.Input = "<ROOT><Title>" + Title + "</Title><ContentHTML><![CDATA[" + ContentHTML + "]]></ContentHTML><ContentText><![CDATA[" + ContentText + "]]></ContentText><FolderID>" + (FolderID.HasValue ? FolderID.Value.ToString() : string.Empty) + "</FolderID></ROOT>";
                Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                ECN_Framework_Entities.Accounts.User user = ECN_Framework_BusinessLayer.Accounts.User.GetByAccessKey(ecnAccessKey, true);
                if (user != null)
                {
                    if (FolderID == null || FolderID.Value == 0 || ECN_Framework_BusinessLayer.Communicator.Folder.Exists(FolderID.Value, user.CustomerID.Value))
                    {
                        if (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(Title, FolderID == null ? 0 : FolderID.Value, user.CustomerID.Value))
                        {
                            if (Title.Trim().ToString().Length < 1)
                                Title = "CONTENT_" + DateTime.Now.ToString("yyyyMMdd-HH:mm:ss");
                            ECN_Framework_Entities.Communicator.Content content = new ECN_Framework_Entities.Communicator.Content();
                            content.CreatedUserID = user.UserID;
                            content.LockedFlag = "Y";
                            content.ContentSource = ContentHTML;
                            content.ContentMobile = ContentHTML;
                            content.ContentText = ContentText;
                            content.FolderID = FolderID;
                            content.ContentTypeCode = ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML.ToString();
                            content.ContentTitle = Title;
                            content.CustomerID = user.CustomerID.Value;
                            content.Sharing = "N";

                            ECN_Framework_BusinessLayer.Communicator.Content.ReadyContent(content, false);
                            ECN_Framework_BusinessLayer.Communicator.Content.Save(content, user);
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Success, content.ContentID, "CONTENT CREATED");
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, Title + " ALREADY EXISTS FOR CUSTOMER");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "FOLDER DOES NOT EXIST FOR CUSTOMER");
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

        private static int LogUnspecifiedException(Exception ex, string sourceMethod)
        {
            return KM.Common.Entity.ApplicationLog.LogCriticalError(ex, sourceMethod, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
        }


    }
}
