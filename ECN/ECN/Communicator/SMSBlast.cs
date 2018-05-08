using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Data;
using System.Web.Services;
using ECN.TextPowerAdvanced;
using ECN.TextPowerWebManagement;

namespace ECN.Communicator
{
    class SMSBlast
    {
        static string UID = "TeamKM";
        static string PWD = "teamkm2012";
        static string Campaign = "TeamKM";

        public static TextPowerAdvanced.MessageValidationInfo Adv_AuthenticationInfo(string Keyword)
        {
            TextPowerAdvanced.MessageValidationInfo adv_info = new TextPowerAdvanced.MessageValidationInfo();
            adv_info.UID = UID;
            adv_info.PWD = PWD;
            adv_info.Campaign = Campaign;
            adv_info.Keyword = Keyword;
            return adv_info;
        }

        public static TextPowerWebManagement.MessageValidationInfo WebMgmt_AuthenticationInfo(string Keyword)
        {
            TextPowerWebManagement.MessageValidationInfo webmgmt_info = new TextPowerWebManagement.MessageValidationInfo();
            webmgmt_info.UID = UID;
            webmgmt_info.PWD = PWD;
            webmgmt_info.Campaign = Campaign;
            webmgmt_info.Keyword = Keyword;
            return webmgmt_info;
        }

        private static void readXML(XmlNode node)
        {
            //    XmlNodeList list = node.ChildNodes;
            //    foreach (XmlNode xnode in list)
            //    {
            //        //Response.Write("<b>" + xnode.Name + "</b><br/>");
            //        //Response.Write("<u>Attributes</u> <br/>");
            //        //if (xnode.Attributes.Count > 0)
            //        //{
            //        //    foreach (XmlAttribute att in xnode.Attributes)
            //        //    {
            //        //        Response.Write(att.Name + ":");
            //        //        Response.Write(att.InnerXml + "<br/>");
            //        //    }
            //        //}

            //        //Response.Write("<u>Values</u> <br/>");
            //        //Response.Write(xnode.InnerXml + "<br/>");
            //    }
        }
      
        #region TEXTPOWER WEBMANAGEMENT API
        public static void WebMgmt_ManageOptIn(TextPowerWebManagement.MessageValidationInfo info, string action, string cellnumber, string carrier)
        {
            try
            {
                WebManagementServicesV2SoapClient webMgmt_client = new WebManagementServicesV2SoapClient();
                XmlNode ManageOptIn = webMgmt_client.ManageOptIn(info, action, cellnumber, "");
                readXML(ManageOptIn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WebMgmt_SendAutoWelcome(TextPowerWebManagement.MessageValidationInfo info, DataSet numberList)
        {
            try
            {
                WebManagementServicesV2SoapClient webMgmt_client = new WebManagementServicesV2SoapClient();
                for (int i = 0; i < numberList.Tables[0].Rows.Count; i++)
                {
                    XmlNode SendAutoWelcome = webMgmt_client.SendAutoWelcome(info, numberList.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void WebMgmt_SetWelcomeMsg(TextPowerWebManagement.MessageValidationInfo info, string message)
        {
            try
            {
                WebManagementServicesV2SoapClient webMgmt_client = new WebManagementServicesV2SoapClient();
                XmlNode SetAutoWelcome = webMgmt_client.SetAutoWelcome(info, message,0,false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region TEXTPOWER ADVANCED API
        public static void Adv_SendToSendList(TextPowerAdvanced.MessageValidationInfo info, string BlastID, string message)
        {
            try
            {
                AdvancedMessageServicesV2SoapClient adv_client = new AdvancedMessageServicesV2SoapClient();
                XmlNode SendToSendList = adv_client.SendToSendList(info, BlastID, message, false, DateTime.Now);
                readXML(SendToSendList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Adv_MergeSendList(TextPowerAdvanced.MessageValidationInfo info, string BlastID, DataSet numberList, bool newList)
        {
            try
            {
                AdvancedMessageServicesV2SoapClient adv_client = new AdvancedMessageServicesV2SoapClient();
                XmlNode MergeSendList = adv_client.MergeSendList(info, BlastID, numberList, newList);
                readXML(MergeSendList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet Adv_GetSendListMembers(TextPowerAdvanced.MessageValidationInfo info, string BlastID)
        {
            try
            {
                AdvancedMessageServicesV2SoapClient adv_client = new AdvancedMessageServicesV2SoapClient();
                DataSet SendListMembers = adv_client.GetSendListMembers(info, BlastID);
                return SendListMembers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
