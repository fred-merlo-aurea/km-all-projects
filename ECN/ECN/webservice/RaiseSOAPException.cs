using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Services.Protocols;

namespace ecn.webservice.classes
{
    public class RaiseSOAPException {
        public enum FaultCode {
            client = 0,
            server = 1
          }

        public RaiseSOAPException() { }

        public SoapException throwException(string uri, string wsNamespace, string status, string id, string message, FaultCode faultCde) {
            XmlQualifiedName faultCodeLocation = null;
            //Identify the location of the FaultCode
            switch (faultCde) {
            case FaultCode.client:
              faultCodeLocation = SoapException.ClientFaultCode;
              break;
            case FaultCode.server:
              faultCodeLocation = SoapException.ServerFaultCode;
              break;
            }


            XmlDocument xmlDoc = new XmlDocument();
            //Create the Detail node
            XmlNode rootNode = xmlDoc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            //Build specific details for the SoapException 
            //Add first child of detail XML element.
            XmlNode responseNode = xmlDoc.CreateNode(XmlNodeType.Element, "Response", wsNamespace);

            //Create and set the value for the ID node - associated to List / message / anything.. 
            XmlNode idNode = xmlDoc.CreateNode(XmlNodeType.Element, "ID", wsNamespace);
            idNode.InnerText = id;

            //Create and set the value for the STATUS node to say 0 = ERROR, 1 = OK
            XmlNode statusNode = xmlDoc.CreateNode(XmlNodeType.Element, "Status", wsNamespace);
            statusNode.InnerText = status;

            //Create and set the value for the Message node - error message
            XmlNode messageNode = xmlDoc.CreateNode(XmlNodeType.Element, "Message", wsNamespace);
            messageNode.InnerText = message;

            //Append the Error child element nodes to the root detail node.
            responseNode.AppendChild(idNode);
            responseNode.AppendChild(statusNode);
            responseNode.AppendChild(messageNode);

            //Append the Detail node to the root node
            rootNode.AppendChild(responseNode);

            //Construct the exception
            SoapException soapEx = new SoapException(message, faultCodeLocation, uri, rootNode);
            //Raise the exception  back to the caller
            return soapEx;
        }

    }
}
