using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UAS.Web.Models.Common
{
    public class ClientInfoDataSource
    {
        private static string filePath = HttpContext.Current.Server.MapPath("~/App_Data/ClientInfos.xml");

        private static XDocument clientInfosXDoc;

        public ClientInfoDataSource()
        {
            clientInfosXDoc = XDocument.Load(filePath);
        }

        /// <summary>
        /// Get ClientInfo by ClientId
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public ClientInfo GetClientInfoByClientId(string clientID)
        {
            var query = from clientInfoXml in clientInfosXDoc.Descendants("ClientID")
                        where clientInfoXml.Value == clientID
                        select clientInfoXml.Parent;

            return convertToClientInfo(query.FirstOrDefault());
        }



        /// <summary>
        /// Insert ClientInfo message to XML file
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        public void InsertClientInfo(ClientInfo clientInfo)
        {
            clientInfosXDoc.Root.Add(convertToClientInfoXElement(clientInfo));
        }

        /// <summary>
        /// Update ActiveTime and RefreshTime
        /// </summary>
        /// <param name="clientInfo"></param>
        public void UpdateClientInfo(ClientInfo clientInfo)
        {
            var query = from x in clientInfosXDoc.Root.Elements()
                        where x.Element("ClientID").Value == clientInfo.ClientID
                        select x;
            if (query.Count() > 0)
            {
                query.FirstOrDefault().Element("ActiveTime").Value = clientInfo.ActiveTime.ToString("MM/dd/yyyy HH:mm:ss");
                query.FirstOrDefault().Element("RefreshTime").Value = clientInfo.RefreshTime.ToString("MM/dd/yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Save data source changes
        /// </summary>
        public void Save()
        {
            clientInfosXDoc.Save(filePath);
        }

        /// <summary>
        /// Convert XML message to Class
        /// </summary>
        /// <param name="clientInfoXml"></param>
        /// <returns></returns>
        private ClientInfo convertToClientInfo(XElement clientInfoXml)
        {
            if (clientInfoXml != null)
            {
                ClientInfo clientInfo = new ClientInfo();
                clientInfo.ClientID = clientInfoXml.Element("ClientID").Value;
                clientInfo.ActiveTime = DateTime.Parse(clientInfoXml.Element("ActiveTime").Value);
                clientInfo.RefreshTime = DateTime.Parse(clientInfoXml.Element("RefreshTime").Value);
                return clientInfo;
            }
            return null;
        }
        /// <summary>
        /// Convert Class to XML message
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        private XElement convertToClientInfoXElement(ClientInfo clientInfo)
        {
            if (clientInfo != null)
            {
                XElement xDoc = new XElement("ClientInfo",
                    new XElement("ClientID", clientInfo.ClientID),
                    new XElement("ActiveTime", clientInfo.ActiveTime.ToString("MM/dd/yyyy HH:mm:ss")),
                    new XElement("RefreshTime", clientInfo.RefreshTime.ToString("MM/dd/yyyy HH:mm:ss")));
                return xDoc;
            }
            return null;
        }
    }
}