using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AMSServicesDocumentation.Models;

namespace AMSServicesDocumentation.Controllers
{
    /// <summary>
    /// API methods exposing the UAD Services Content object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>
    /// <remarks>For more information on REST, try this 
    /// <a href="http://en.wikipedia.org/wiki/Representational_state_transfer">Wikipedia article</a>.</remarks>
    //[ConditionalDataBehavior]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UADService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UADService.svc or UADService.svc.cs at the Solution Explorer and start debugging.
    public class UADServiceController : ApiController
    {
        #region REST

        #region GET
        #region GetSubscriberExamples
        /// <summary>
        /// This method return all consensus level data for an email address. This includes the associated products, profile, and demographic data.
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <returns>Response object containing a list of ClientSubscription objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         String email = "email@emailaddress.com";
        ///         Response response = client.GetSubscriber( guid, email);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim email as String = "email@emailaddress.com"
        ///         Dim response As Response = client.GetSubscriber( guid, email)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
        /// 
        /// <example for="request"><![CDATA[
        /// GET https://uadservices.kmpsgroup.com/UAD/UADService.svc/GetSubscriber/?accessKey=00000000-0000-0000-0000-000000000000&email=email@email.com HTTP/1.1
        /// Accept: application/json
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 422
        /// Host: uadservices.kmpsgroup.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 (OK)
        /// Content-Length: 11105
        /// Cache-Control: private
        /// Content-Type: application/json; charset=utf-8
        /// Date: Tue, 26 Jul 2016 14:00:44 GMT
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// 
        /// {
        /// 	"Message": "Success",
        /// 	"ProcessCode": "b75a46lhYwsq_07262016_09:00:43",
        /// 	"Result": [
        /// 		{
        /// 			"AccountNumber": null,
        /// 			"Address": "123 5th street",
        /// 			"Address2": "",
        /// 			"Address3": "",
        /// 			"Age": 0,
        /// 			"Birthdate": null,
        /// 			"City": "MyCity",
        /// 			"Company": "Myco",
        /// 			"Country": "UNITED STATES",
        /// 			"County": "",
        /// 			"Demo7": "A",
        /// 			"Email": "myemail@myemail.com",
        /// 			"EmailID": 0,
        /// 			"EmailRenewPermission": null,
        /// 			"ExternalKeyID": -1,
        /// 			"ExternalKeyId": 0,
        /// 			"Fax": "0000000000",
        /// 			"FaxPermission": null,
        /// 			"FirstName": "First",
        /// 			"ForZip": "",
        /// 			"FullAddress": null,
        /// 			"Gender": null,
        /// 			"IGrp_No": "00000000-0000-0000-0000-000000000000",
        /// 			"Income": null,
        /// 			"IsMailable": false,
        /// 			"LastName": "Last",
        /// 			"Latitude": 50.000000000000000,
        /// 			"Longitude": -50.000000000000000,
        /// 			"MailPermission": null,
        /// 			"Mobile": "0000000000",
        /// 			"Occupation": null,
        /// 			"OtherProductsPermission": null,
        /// 			"Phone": "0000000000",
        /// 			"PhoneExt": null,
        /// 			"PhonePermission": null,
        /// 			"Plus4": "",
        /// 			"ProductList": [
        /// 				{
        /// 					"AccountNumber": "00000000",
        /// 					"Address1": "123 Fake Ave.",
        /// 					"Address2": "",
        /// 					"Address3": "",
        /// 					"Age": 0,
        /// 					"Birthdate": "\/Date(-2208967200000-0600)\/",
        /// 					"CarrierRoute": "R085",
        /// 					"CategoryID": 101,
        /// 					"City": "Minneapolis",
        /// 					"ClientName": "KMPS SourceMedia Subscribers ( UAD \/ CIRC )",
        /// 					"Company": "KM",
        /// 					"Copies": 1,
        /// 					"Country": "UNITED STATES",
        /// 					"County": "Carver",
        /// 					"DateCreated": "\/Date(1454081938387-0600)\/",
        /// 					"DateUpdated": null,
        /// 					"Demo7": "A",
        /// 					"Email": "email@email.com",
        /// 					"EmailID": 000000000,
        /// 					"EmailRenewPermission": null,
        /// 					"EmailStatus": "Active",
        /// 					"ExternalKeyID": 0,
        /// 					"Fax": "0000000000",
        /// 					"FaxPermission": null,
        /// 					"FirstName": "TIMOTHY",
        /// 					"FullAddress": "123 Fake Ave, , Minneapolis, MN, 55312-4100, UNITED STATES",
        /// 					"FullName": "FIRST LAST",
        /// 					"Gender": "M",
        /// 					"GraceIssues": 0,
        /// 					"IGrp_No": "00000000-0000-0000-0000-000000000000",
        /// 					"Income": "",
        /// 					"LastName": "LAST",
        /// 					"Latitude": 0,
        /// 					"Longitude": 0,
        /// 					"MailPermission": null,
        /// 					"Mobile": "",
        /// 					"Name": "KM - Knowledge Marketing",
        /// 					"Occupation": "",
        /// 					"OnBehalfOf": "",
        /// 					"OrigsSrc": "AAA000",
        /// 					"OtherProductsPermission": null,
        /// 					"Phone": "0000000000",
        /// 					"PhoneExt": "",
        /// 					"PhonePermission": null,
        /// 					"Plus4": "",
        /// 					"PubCode": "KM",
        /// 					"PubID": 1,
        /// 					"PubSubscriptionID": 000000,
        /// 					"PubType": "PUBLICATIONS",
        /// 					"QDate": "\/Date(1354341600000-0600)\/",
        /// 					"QSourceID": 0000,
        /// 					"ReqFlag": 0000,
        /// 					"SequenceID": 000000,
        /// 					"State": "CA",
        /// 					"StatusUpdatedDate": "\/Date(1469219023657-0500)\/",
        /// 					"StatusUpdatedReason": "update",
        /// 					"SubscriberProductDemographics": [
        /// 						{
        /// 							"Name": "AIRMAIL",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "BUSINESS",
        /// 							"Value": "NBK"
        /// 						},
        /// 						{
        /// 							"Name": "DEMO1",
        /// 							"Value": "Y"
        /// 						},
        /// 						{
        /// 							"Name": "DEMO38",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "DEMO40",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "DEMO41",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "DEMO9",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "FUNCTION",
        /// 							"Value": "ZZZ"
        /// 						},
        /// 						{
        /// 							"Name": "MEDIADEF",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "OWSDAILYENEWS",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "PUBCODE",
        /// 							"Value": "OWS"
        /// 						},
        /// 						{
        /// 							"Name": "SALES",
        /// 							"Value": "10M"
        /// 						},
        /// 						{
        /// 							"Name": "SALESREP",
        /// 							"Value": ""
        /// 						},
        /// 						{
        /// 							"Name": "SUBSCRIPTION",
        /// 							"Value": ""
        /// 						}
        /// 					],
        /// 					"SubscriberSourceCode": "Y212REQ",
        /// 					"SubscriptionStatusID": 5,
        /// 					"TextPermission": null,
        /// 					"ThirdPartyPermission": null,
        /// 					"Title": "PERSONAL BANKER",
        /// 					"TransactionID": 100,
        /// 					"Verify": "",
        /// 					"Website": "",
        /// 					"ZipCode": "90058"
        /// 				},
        /// 				{
        /// 					"AccountNumber": "",
        /// 					"Address1": "",
        /// 					"Address2": "",
        /// 					"Address3": "",
        /// 					"Age": 0,
        /// 					"Birthdate": "\/Date(-2208967200000-0600)\/",
        /// 					"CarrierRoute": "",
        /// 					"CategoryID": 101,
        /// 					"City": "",
        /// 					"ClientName": "KMPS SourceMedia Subscribers ( UAD \/ CIRC )",
        /// 					"Company": "",
        /// 					"Copies": 1,
        /// 					"Country": "",
        /// 					"County": "",
        /// 					"DateCreated": "\/Date(1461000962127-0500)\/",
        /// 					"DateUpdated": null,
        /// 					"Demo7": "A",
        /// 					"Email": "email@email.com",
        /// 					"EmailID": 0,
        /// 					"EmailRenewPermission": null,
        /// 					"EmailStatus": "Active",
        /// 					"ExternalKeyID": 0,
        /// 					"Fax": "",
        /// 					"FaxPermission": null,
        /// 					"FirstName": "",
        /// 					"FullAddress": ", , , , , ",
        /// 					"FullName": " ",
        /// 					"Gender": "",
        /// 					"GraceIssues": 0,
        /// 					"IGrp_No": "738e61cd-94f8-438e-b045-544c6999ea21",
        /// 					"Income": "",
        /// 					"LastName": "",
        /// 					"Latitude": -1.000000000000000,
        /// 					"Longitude": -1.000000000000000,
        /// 					"MailPermission": null,
        /// 					"Mobile": "",
        /// 					"Name": "KM - Knowledge Marketing",
        /// 					"Occupation": "",
        /// 					"OnBehalfOf": "",
        /// 					"OrigsSrc": "",
        /// 					"OtherProductsPermission": null,
        /// 					"Phone": "",
        /// 					"PhoneExt": "",
        /// 					"PhonePermission": null,
        /// 					"Plus4": "",
        /// 					"PubCode": "SMGOO",
        /// 					"PubID": 226,
        /// 					"PubSubscriptionID": 25995933,
        /// 					"PubType": "ENEWSLETTERS",
        /// 					"QDate": "\/Date(1460955600000-0500)\/",
        /// 					"QSourceID": -1,
        /// 					"ReqFlag": 0,
        /// 					"SequenceID": 1187786,
        /// 					"State": "",
        /// 					"StatusUpdatedDate": "\/Date(1469541644383-0500)\/",
        /// 					"StatusUpdatedReason": "Subscribed",
        /// 					"SubscriberProductDemographics": [
        /// 						{
        /// 							"Name": "PUBCODE",
        /// 							"Value": "SMGOO"
        /// 						}
        /// 					],
        /// 					"SubscriberSourceCode": "",
        /// 					"SubscriptionStatusID": 3,
        /// 					"TextPermission": null,
        /// 					"ThirdPartyPermission": null,
        /// 					"Title": "",
        /// 					"TransactionID": 101,
        /// 					"Verify": "",
        /// 					"Website": "",
        /// 					"ZipCode": ""
        /// 				},
        /// 				{
        /// 					"AccountNumber": "",
        /// 					"Address1": "",
        /// 					"Address2": "",
        /// 					"Address3": "",
        /// 					"Age": 0,
        /// 					"Birthdate": "\/Date(-2208967200000-0600)\/",
        /// 					"CarrierRoute": "",
        /// 					"CategoryID": 101,
        /// 					"City": "",
        /// 					"ClientName": "KMPS SourceMedia Subscribers ( UAD \/ CIRC )",
        /// 					"Company": "",
        /// 					"Copies": 1,
        /// 					"Country": "",
        /// 					"County": "",
        /// 					"DateCreated": "\/Date(1468597365777-0500)\/",
        /// 					"DateUpdated": null,
        /// 					"Demo7": "A",
        /// 					"Email": "email@email.com",
        /// 					"EmailID": 0,
        /// 					"EmailRenewPermission": null,
        /// 					"EmailStatus": "Active",
        /// 					"ExternalKeyID": 0,
        /// 					"Fax": "",
        /// 					"FaxPermission": null,
        /// 					"FirstName": "",
        /// 					"FullAddress": ", , , , , ",
        /// 					"FullName": " ",
        /// 					"Gender": "",
        /// 					"GraceIssues": 0,
        /// 					"IGrp_No": "738e61cd-94f8-438e-b045-544c6999ea21",
        /// 					"Income": "",
        /// 					"LastName": "",
        /// 					"Latitude": -1.000000000000000,
        /// 					"Longitude": -1.000000000000000,
        /// 					"MailPermission": null,
        /// 					"Mobile": "",
        /// 					"Name": "IMTWP15 - IMT White Paper 2015",
        /// 					"Occupation": "",
        /// 					"OnBehalfOf": "",
        /// 					"OrigsSrc": "",
        /// 					"OtherProductsPermission": null,
        /// 					"Phone": "",
        /// 					"PhoneExt": "",
        /// 					"PhonePermission": null,
        /// 					"Plus4": "",
        /// 					"PubCode": "IMTWP15",
        /// 					"PubID": 557,
        /// 					"PubSubscriptionID": 28479477,
        /// 					"PubType": "WHITE PAPERS",
        /// 					"QDate": "\/Date(1442984400000-0500)\/",
        /// 					"QSourceID": -1,
        /// 					"ReqFlag": 0,
        /// 					"SequenceID": 3963,
        /// 					"State": "",
        /// 					"StatusUpdatedDate": "\/Date(1469219023657-0500)\/",
        /// 					"StatusUpdatedReason": "update",
        /// 					"SubscriberProductDemographics": [
        /// 						{
        /// 							"Name": "EMPLOYEESLEADREPORT",
        /// 							"Value": "D"
        /// 						},
        /// 						{
        /// 							"Name": "PUBCODE",
        /// 							"Value": "IMTWP15"
        /// 						},
        /// 						{
        /// 							"Name": "REVENUELEADREPORT",
        /// 							"Value": "F"
        /// 						}
        /// 					],
        /// 					"SubscriberSourceCode": "",
        /// 					"SubscriptionStatusID": 3,
        /// 					"TextPermission": null,
        /// 					"ThirdPartyPermission": null,
        /// 					"Title": "",
        /// 					"TransactionID": 101,
        /// 					"Verify": "",
        /// 					"Website": "",
        /// 					"ZipCode": ""
        /// 				}
        /// 				
        /// 	],
        /// 	"Status": 0
        /// }
        /// ]]></example>
        #endregion GetSubscriberExamples
        [Route("~/GetSubscriber")]
        [HttpGet]
        public Response<List<ClientSubscription>> GetSubscriber( Guid accessKey, String emailAddress)
        {
            Response<List<ClientSubscription>> response = new Response<List<ClientSubscription>>();
            return response;
        }
        #region GetDemographicsExamples
        /// <summary>
        /// This method allows you to get specific consensus level demographic data for a record in the database.  
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="dimensions">List of SubscriberConsensusDemographics.</param>
        /// <returns>Response object containing a list of SubscriberConsensus objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         List<SubscriberConsensusDemographic> demoList = new List<SubscriberConsensusDemographic>();
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         String email = "email@emailaddress.com";
        ///         Response response = client.GetDemographics( guid, email, demoList);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim demoList = New List(Of SubscriberConsensusDemographic)
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim email as String = "email@emailaddress.com"
        ///         Dim response As Response = client.GetDemographics( guid, email, demoList)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
        
        //<example for="request"><![CDATA[ ]]></example>

        //<example for="response"><![CDATA[ ]]></example>
        #endregion GetDemographicsExamples
        [Route("~/GetDemographics")]
        [HttpPost]
        public Response<List<SubscriberConsensus>> GetDemographics( Guid accessKey, string emailAddress,[FromBody] List<SubscriberConsensusDemographic> dimensions = null)
        {
            Response<List<SubscriberConsensus>> response = new Response<List<SubscriberConsensus>>();
            return response;
        }
        #region GetProductDemographicsExamples
        /// <summary>
        /// This method allows you to get specific product level demographic data for a record in the database.  
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="productCode">Product code.</param>
        /// <param name="dimensions">List of SubscriberProductDemographics.</param>
        /// <returns>Response object containing a list of SubscriberProduct objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         List<SubscriberProductDemographic> demoList = new List<SubscriberProductDemographic>();
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         String email = "email@emailaddress.com";
        ///         String productCode = "PRODUCT";
        ///         Response response = client.GetProductDemographics( guid, email, productCode, demoList);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim demoList = New List(Of SubscriberProductDemographic)
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim email as String = "email@emailaddress.com"
        ///         Dim productCode as String = "PRODUCT"
        ///         Dim response As Response = client.GetProductDemographics( guid, email, productCode, demoList)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
         
         //<example for="request"><![CDATA[ ]]></example>

        //<example for="response"><![CDATA[ ]]></example>
        #endregion GetProductDemographicsExamples
        [Route("~/GetProductDemographics")]
        [HttpPost]
        public Response<List<SubscriberProduct>> GetProductDemographics(Guid accessKey, string emailAddress, string productCode, [FromBody] List<SubscriberProductDemographic> dimensions = null)
        {
            Response<List<SubscriberProduct>> response = new Response<List<SubscriberProduct>>();
            return response;
        }
        #region GetCustomFieldsExamples
        /// <summary>
        /// This method returns a list of all of the dimensions you have configured for a single product.
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="productCode">Product code.</param>
        /// <returns>Response object containing a list of CustomField objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         String productCode = "PRODUCT";
        ///         Response response = client.GetCustomFields( guid, productCode);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim productCode as String = "PRODUCT"
        ///         Dim response As Response = client.GetCustomFields( guid, productCode)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
        /// 
        /// <example for="request"><![CDATA[
        /// GET https://uadservices.kmpsgroup.com/UAD/UADService.svc/GetCustomFields?accessKey=00000000-0000-0000-0000-000000000000&productCode=Product HTTP/1.1
        /// Accept: application/json
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 422
        /// Host: uadservices.kmpsgroup.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 (OK)
        /// Content-Length: 2841
        /// Cache-Control: private
        /// Content-Type: application/json; charset=utf-8
        /// Date: Tue, 26 Jul 2016 15:50:14 GMT
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// 
        /// {
        ///     "Message": "Success",
        ///     "ProcessCode": "E5O9C8X1v5bf_07262016_10:50:14",
        ///     "Result": [
        ///         {
        ///             "DisplayName": "PUBCODE",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "PUBCODE",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 1
        ///         },
        ///         {
        ///             "DisplayName": "SALES REP",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "SALESREP",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 1
        ///         },
        ///         {
        ///             "DisplayName": "ADVICE",
        ///             "DisplayOrder": 1,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": true,
        ///             "Name": "DEMO1",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 1
        ///         },
        ///         {
        ///             "DisplayName": "BUSINESS",
        ///             "DisplayOrder": 2,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": true,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 1
        ///         },
        ///         {
        ///             "DisplayName": "SALES",
        ///             "DisplayOrder": 5,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "SALES",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 1
        ///         },
        ///         {
        ///             "DisplayName": "MOBILE",
        ///             "DisplayOrder": 7,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "DEMO40",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 1
        ///         }
        ///     ],
        ///     "Status": 0
        /// }
        /// ]]></example>
        #endregion GetCustomFieldsExamples
        [Route("~/GetCustomFields")]
        [HttpGet]
        public Response<List<CustomField>> GetCustomFields( Guid accessKey, string productCode = "")
        {
            Response<List<CustomField>> response = new Response<List<CustomField>>();
            return response;
        }
        #region GetCustomFieldsExamples
        /// <summary>
        /// This method returns a list of all of the consensus level dimensions in your UAD.
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <returns>Response object containing a list of CustomField objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         Response response = client.GetConsensusCustomFields( guid);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim response As Response = client.GetConsensusCustomFields( guid)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
        /// 
        /// <example for="request"><![CDATA[
        /// GET https://uadservices.kmpsgroup.com/UAD/UADService.svc/GetConsensusCustomFields?accessKey=00000000-0000-0000-0000-000000000000 HTTP/1.1
        /// Accept: application/json
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 422
        /// Host: uadservices.kmpsgroup.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 (OK)
        /// Content-Length: 7259
        /// Cache-Control: private
        /// Content-Type: application/json; charset=utf-8
        /// Date: Tue, 26 Jul 2016 15:58:14 GMT
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// 
        /// {
        ///     "Message": "Success",
        ///     "ProcessCode": "6D95ElMP5uGO_07262016_10:58:14",
        ///     "Result": [
        ///         {
        ///             "DisplayName": "Job Function",
        ///             "DisplayOrder": 6,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Job Function",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Number of Employees",
        ///             "DisplayOrder": 8,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Number of Employees",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Organization Type of Business",
        ///             "DisplayOrder": 11,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Organization Type of Business",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Primary Job Title",
        ///             "DisplayOrder": 12,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Primary Job Title",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "PUBCODE",
        ///             "DisplayOrder": 13,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "PUBCODE",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Event Class",
        ///             "DisplayOrder": 16,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Event Class",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Department",
        ///             "DisplayOrder": 17,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Department",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Conference Status",
        ///             "DisplayOrder": 18,
        ///             "IsActive": true,
        ///             "IsAdHoc": false,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Conference Status",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Account Rep Name",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": true,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Account Rep Name",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Changed By Name",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": true,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Changed By Name",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Changed On",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": true,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Changed On",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Event End Date",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": true,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Event End Date",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Event Order #",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": true,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Event Order #",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         },
        ///         {
        ///             "DisplayName": "Event Order Total",
        ///             "DisplayOrder": 0,
        ///             "IsActive": true,
        ///             "IsAdHoc": true,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "Name": "Event Order Total",
        ///             "ProductCode": "Consensus",
        ///             "ProductId": 0
        ///         }
        ///     ],
        ///     "Status": 0
        /// }
        /// ]]></example>
        #endregion GetCustomFieldsExamples
        [Route("~/GetConsensusCustomFields")]
        [HttpGet]
        public Response<List<CustomField>> GetConsensusCustomFields( Guid accessKey)
        {
            Response<List<CustomField>> response = new Response<List<CustomField>>();
            return response;
        }
        #region GetCustomFieldValuesExamples
        /// <summary>
        /// This method returns a list of all of the descriptions and codes that are setup for a product level dimension.
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="productCode">Product code.</param>
        /// <param name="customFieldName">Custom field name your searching for.</param>
        /// <returns>Response object containing a list of CustomFieldValue objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         String productCode = "PRODUCT";
        ///         String customFieldName = "Business";
        ///         Response response = client.GetCustomFieldValues( guid, productCode, customFieldName);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim productCode as String = "PRODUCT"
        ///         Dim customFieldName as String = "Business"
        ///         Dim response As Response = client.GetCustomFieldValues( guid, productCode, customFieldName)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
        /// 
        /// <example for="request"><![CDATA[
        /// GET https://uadservices.kmpsgroup.com/UAD/UADService.svc/GetCustomFieldValues?accessKey=00000000-0000-0000-0000-000000000000&productCode=PRODUCT&customFieldName=BUSINESS HTTP/1.1
        /// Accept: application/json
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 422
        /// Host: uadservices.kmpsgroup.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 (OK)
        /// Content-Length: 4160
        /// Cache-Control: private
        /// Content-Type: application/json; charset=utf-8
        /// Date: Tue, 26 Jul 2016 16:05:45 GMT
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// 
        /// {
        ///     "Message": "Success",
        ///     "ProcessCode": "F71tE6BPEoXe_07262016_11:05:43",
        ///     "Result": [
        ///         {
        ///             "Description": "Accounting",
        ///             "DisplayOrder": 4,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "ACT"
        ///         },
        ///         {
        ///             "Description": "Commerical Bank",
        ///             "DisplayOrder": 6,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "CBK"
        ///         },
        ///         {
        ///             "Description": "Community Bank",
        ///             "DisplayOrder": 7,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "CUK"
        ///         },
        ///         {
        ///             "Description": "Consulting Firm",
        ///             "DisplayOrder": 8,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "CON"
        ///         },
        ///         {
        ///             "Description": "Education",
        ///             "DisplayOrder": 10,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "EDU"
        ///         },
        ///         {
        ///             "Description": "Family Office",
        ///             "DisplayOrder": 11,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "FAM"
        ///         },
        ///         {
        ///             "Description": "Government",
        ///             "DisplayOrder": 12,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "GOV"
        ///         },
        ///         {
        ///             "Description": "Insurance",
        ///             "DisplayOrder": 14,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "INS"
        ///         },
        ///         {
        ///             "Description": "Investment Bank",
        ///             "DisplayOrder": 15,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "INV"
        ///         },
        ///         {
        ///             "Description": "Legal",
        ///             "DisplayOrder": 16,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "LEG"
        ///         },
        ///         {
        ///             "Description": "Private or Foreign Bank",
        ///             "DisplayOrder": 17,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "PRB"
        ///         },
        ///         {
        ///             "Description": "Technology Vendor",
        ///             "DisplayOrder": 20,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "VEN"
        ///         },
        ///         {
        ///             "Description": "Trust Company",
        ///             "DisplayOrder": 22,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "TCO"
        ///         },
        ///         {
        ///             "Description": "Other",
        ///             "DisplayOrder": 23,
        ///             "IsConsensus": false,
        ///             "IsOther": true,
        ///             "Name": "BUSINESS",
        ///             "ProductCode": "PRODUCT",
        ///             "ProductId": 9,
        ///             "Value": "ZZZ"
        ///         }
        ///     ],
        ///     "Status": 0
        /// }
        /// ]]></example>
        #endregion GetCustomFieldValuesExamples
        [Route("~/GetCustomFieldValues")]
        [HttpGet]
        public Response<List<CustomFieldValue>> GetCustomFieldValues(Guid accessKey, string productCode = "", string customFieldName = "")
        {
            Response<List<CustomFieldValue>> response = new Response<List<CustomFieldValue>>();
            return response;
        }
        #region GetConsensusCustomFieldValuesExamples
        /// <summary>
        /// This method returns a list of all of the descriptions and codes that are setup for a consensus level dimension.
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="customFieldName">Custom field name your searching for.</param>
        /// <returns>Response object containing a list of CustomFieldValue objects.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         String customFieldName = "Business";
        ///         Response response = client.GetConsensusCustomFieldValues( guid, customFieldName);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim customFieldName as String = "Business"
        ///         Dim response As Response = client.GetConsensusCustomFieldValues( guid, customFieldName)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>
        /// 
        /// <example for="request"><![CDATA[
        /// GET Service at https://uadservices.kmpsgroup.com/UAD/UADService.svc/GetConsensusCustomFieldValues?accessKey=00000000-0000-0000-0000-000000000000&customFieldName=Department HTTP/1.1
        /// Accept: application/json
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 422
        /// Host: uadservices.kmpsgroup.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 (OK)
        /// Content-Length: 662
        /// Cache-Control: private
        /// Content-Type: application/json; charset=utf-8
        /// Date: Tue, 26 Jul 2016 16:16:26 GMT
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// 
        /// {
        ///     "Message": "Success",
        ///     "ProcessCode": "6lbzreJzk47a_07262016_11:16:26",
        ///     "Result": [
        ///         {
        ///             "Description": "EMAIL",
        ///             "DisplayOrder": 0,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "Department",
        ///             "ProductCode": "",
        ///             "ProductId": 0,
        ///             "Value": "EMAIL"
        ///         },
        ///         {
        ///             "Description": "ONSITE",
        ///             "DisplayOrder": 0,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "Department",
        ///             "ProductCode": "",
        ///             "ProductId": 0,
        ///             "Value": "ONSITE"
        ///         },
        ///         {
        ///             "Description": "TELE",
        ///             "DisplayOrder": 0,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "Department",
        ///             "ProductCode": "",
        ///             "ProductId": 0,
        ///             "Value": "TELE"
        ///         },
        ///         {
        ///             "Description": "WEB",
        ///             "DisplayOrder": 0,
        ///             "IsConsensus": false,
        ///             "IsOther": false,
        ///             "Name": "Department",
        ///             "ProductCode": "",
        ///             "ProductId": 0,
        ///             "Value": "WEB"
        ///         }
        ///     ],
        ///     "Status": 0
        /// }
        /// ]]></example>
        #endregion GetConsensusCustomFieldValuesExamples
        [Route("~/GetConsensusCustomFieldValues")]
        [HttpGet]
        public Response<List<CustomFieldValue>> GetConsensusCustomFieldValues( Guid accessKey, string customFieldName = "")
        {
            Response<List<CustomFieldValue>> response = new Response<List<CustomFieldValue>>();
            return response;
        }
        
        #endregion GET
        #region POST
        #region SaveSubscriberExamples
        /// <summary>
        /// This method allows you to insert or update one or more records in the UAD for one or multiple products
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="newSubscriber">The SaveSubscriber object your saving.</param>
        /// <returns>Response object containing a SavedSubscriber object.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         SaveSubscriber saveSub = new SaveSubscriber();
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         Response response = client.SaveSubscriber( guid, saveSub);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim saveSub as SaveSubscriber = new SaveSubscriber()
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim response As Response = client.SaveSubscriber( guid, saveSub)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>

        //<example for="request"><![CDATA[ ]]></example>

        //<example for="response"><![CDATA[ ]]></example>
        #endregion SaveSubscriberExamples
        [Route("~/SaveSubscriber")]
        [HttpPost]
        public AMSServicesDocumentation.Models.Response<SavedSubscriber> SaveSubscriber(Guid accessKey, [FromBody] SaveSubscriber newSubscriber)
        {
            Response<SavedSubscriber> response = new Response<SavedSubscriber>();
            return response;
        }
        #region SavePaidSubscriberExamples
        /// <summary>
        /// This method allows you to insert or update a paid subscriber record.
        /// </summary>
        /// <param name="accessKey">Client access key.</param>
        /// <param name="newPaidSubscription">The PaidSubscription object your saving.</param>
        /// <returns>Response object containing an int.</returns>
        /// <example for="c"><![CDATA[
        /// class Test
        /// {
        ///     static void Main()
        ///     {
        ///         UADServiceClient client = new UADServiceClient();
        ///
        ///         // Use the 'client' variable to call operations on the service.
        ///         PaidSubscription paidSub = new PaidSubscription();
        ///         Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
        ///         Response response = client.SavePaidSubscriber( guid, paidSub);
        /// 
        ///         // Always close the client.
        ///         client.Close();
        ///     }
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="vb"><![CDATA[
        /// Class Test
        ///     Shared Sub Main()
        ///         Dim client As UADServiceClient = New UADServiceClient()
        ///         ' Use the 'client' variable to call operations on the service.
        ///         Dim paidSub as PaidSubscription = new PaidSubscription()
        ///         Dim guid as Guid = new Guid("00000000-0000-0000-0000-000000000000")
        ///         Dim response As Response = client.SavePaidSubscriber( guid, paidSub)
        /// 
        ///         ' Always close the client.
        ///         client.Close()
        ///     End Sub
        /// End Class
        /// ]]></example>

        //<example for="request"><![CDATA[ ]]></example>

        //<example for="response"><![CDATA[ ]]></example>
        #endregion SavePaidSubscriberExamples
        [Route("~/SavePaidSubscriber")]
        [HttpPost]
        public Response<int> SavePaidSubscriber(Guid accessKey, [FromBody] PaidSubscription newPaidSubscription)
        {
            Response<int> response = new Response<int>();
            return response;
        }
        #endregion POST
        #region PUT

        #endregion PUT
        #region DELETE

        #endregion DELETE
        #endregion REST
    }
}