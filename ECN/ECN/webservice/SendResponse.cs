using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Services.Protocols;

namespace ecn.webservice.classes
{
    public class SendResponse {
        public enum FaultCode {
            client = 0,
            server = 1
          }

        public enum ResponseCode
        {
            Success = 0,
            Fail = 1
        }

        public static string response(string webMethod, ResponseCode status, int id, string output)
        {
            return response(webMethod, ((int)status).ToString(), id, output);
        }

        public static string response(string webMethod, string status, int id, string output)
        {
            string wsNamespace = "http://www.ecn5.com";
            //Identify the location of the FaultCode
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode responseNode = xmlDoc.CreateNode(XmlNodeType.Element, "Response", wsNamespace);

            //Create and set the value for the ID node - associated to List / message / anything.. 
            XmlNode webMethodNode = xmlDoc.CreateNode(XmlNodeType.Element, "WebMethod", wsNamespace);
            webMethodNode.InnerText = webMethod;

            //Create and set the value for the STATUS node to say 0 = ERROR, 1 = OK
            XmlNode statusNode = xmlDoc.CreateNode(XmlNodeType.Element, "ErrorCode", wsNamespace);
            statusNode.InnerText = status;

            //Create and set the ID of the entity
            XmlNode idNode = xmlDoc.CreateNode(XmlNodeType.Element, "ID", wsNamespace);
            idNode.InnerText = id.ToString();

            XmlNode outputNode = xmlDoc.CreateNode(XmlNodeType.Element, "Outputxml", wsNamespace);

            XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
            xfrag.InnerXml = output;          
            //XmlNode respOutputNode = xmlDoc.CreateNode(XmlNodeType.CDATA, "", wsNamespace);
            //respOutputNode.InnerText = output;
            outputNode.AppendChild(xfrag);

            //Append the Error child element nodes to the root detail node.
            responseNode.AppendChild(webMethodNode);
            responseNode.AppendChild(statusNode);
            responseNode.AppendChild(idNode);
            responseNode.AppendChild(outputNode);

            xmlDoc.AppendChild(responseNode);

            return xmlDoc.InnerXml.ToString();
        }

    }
}
