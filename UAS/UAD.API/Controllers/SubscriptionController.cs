using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KM.Common.Functions;
using FrameworkModel = FrameworkUAD.Object.SaveSubscriber;
using APIModel = UAD.API.Models.SaveSubscriber;

namespace UAD.API.Controllers
{
    /// <summary>
    /// API methods exposing subscription management
    /// </summary>
    [RoutePrefix("api/subscription")]
    public class SubscriptionController : AuthenticatedUserControllerBase
    {
        #region abstract member implementations
        /// <inheritdoc/>
        public override FrameworkUAD.Object.Enums.Entity FrameworkEntity
        {
            get { return FrameworkUAD.Object.Enums.Entity.Subscription; }
        }

        override public string ControllerName { get { return "Subscription"; } }

        #endregion abstract member implementations

        #region Methods
        #region Remove as its single and can be done in the multiple method
        ///// <summary>
        ///// Ability to add one subscription, demographics, product adhocs, and consensus adhocs.
        ///// </summary>
        ///// <param name="subscription">Subscription</param>
        ///// <returns>The subscription added for <code>Subscription</code></returns>
        /////
        ///// <example for="request"><![CDATA[
        /////  GET /api/subscription/methods/SaveSubscriber HTTP/1.1
        /////  Content-Type: application/json
        /////  Accept: application/json
        /////  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /////  X-Customer-ID: 99999
        /////  Host: api.kmpsgroup.com
        /////  Content-Length: 422   
        /////  
        /////     [
        /////         {  
        /////             "IGrp_No":"1627aea5-8e0a-4371-9022-9b504344e724",
        /////             "SequenceID":2147483647,
        /////             "AccountNumber":"String content",
        /////             "PubCode":"String content",
        /////             "QualificationDate":"2017-01-01",
        /////             "CategoryID":2147483647,
        /////             "TransactionID":2147483647,
        /////             "QSourceID":2147483647,
        /////             "Demo7":"String content", 
        /////             "Copies":2147483647,
        /////             "GraceIssues":2147483647,
        /////             "SubscriberSourceCode":"String content",
        /////             "Verify":"String content",
        /////             "Email":"String content",
        /////             "EmailStatus":"String content",
        /////             "FirstName":"String content",
        /////             "LastName":"String content",
        /////             "Company":"String content",
        /////             "Title":"String content",
        /////             "Occupation":"String content",
        /////             "Address1":"String content",
        /////             "Address2":"String content",
        /////             "Address3":"String content",
        /////             "City":"String content",
        /////             "State":"String content",
        /////             "ZipCode":"String content",
        /////             "Plus4":"String content",
        /////             "Country":"String content",
        /////             "County":"String content",
        /////             "Phone":"String content",
        /////             "PhoneExt":"String content",
        /////             "Fax":"String content",
        /////             "Website":"String content",
        /////             "Mobile":"String content",
        /////             "Gender":"String content",
        /////             "Age":2147483647,
        /////             "Birthdate":"2017-01-01",
        /////             "Income":"String content",
        /////             "EmailRenewPermission":true,
        /////             "FaxPermission":true,
        /////             "MailPermission":true,
        /////             "OtherProductsPermission":true,
        /////             "PhonePermission":true,
        /////             "TextPermission":true,
        /////             "ThirdPartyPermission":true,
        /////             "SubscriberProductDemographics":[
        /////                 { 
        /////                     "Name":"String ",
        /////                     "Value":"String "
        /////                 }
        /////             ],
        /////             "SubscriberProductCustomFields":[
        /////                 {
        /////                     "Name":"String ",
        /////                     "Value":"String "
        /////                 }
        /////             ],
        /////             "SubscriberConsensusCustomFields":[
        /////                 {
        /////                     "Name":"String ",
        /////                     "Value":"String "
        /////                 }
        /////             ]        
        /////         }
        /////     ]
        ///// 
        ///// ]]></example>
        ///// 
        ///// <example for="response"><![CDATA[
        ///// HTTP/1.1 200 OK
        ///// Cache-Control: no-cache
        ///// Pragma: no-cache
        ///// Content-Type: application/json; charset=utf-8
        ///// Expires: -1
        ///// Server: Microsoft-IIS/7.5
        ///// X-AspNet-Version: 4.0.30319
        ///// X-Powered-By: ASP.NET
        ///// Date: Fri, 10 Apr 2015 19:27:41 GMT
        ///// Content-Length: 259
        ///// 
        /////     [
        /////         {  
        /////             "IGrp_No":"1627aea5-8e0a-4371-9022-9b504344e724",
        /////             "SequenceID":2147483647,
        /////             "AccountNumber":"String content",
        /////             "PubCode":"String content",
        /////             "QualificationDate":"2017-01-01",
        /////             "CategoryID":2147483647,
        /////             "TransactionID":2147483647,
        /////             "QSourceID":2147483647,
        /////             "Demo7":"String content", 
        /////             "Copies":2147483647,
        /////             "GraceIssues":2147483647,
        /////             "SubscriberSourceCode":"String content",
        /////             "Verify":"String content",
        /////             "Email":"String content",
        /////             "EmailStatus":"String content",
        /////             "FirstName":"String content",
        /////             "LastName":"String content",
        /////             "Company":"String content",
        /////             "Title":"String content",
        /////             "Occupation":"String content",
        /////             "Address1":"String content",
        /////             "Address2":"String content",
        /////             "Address3":"String content",
        /////             "City":"String content",
        /////             "State":"String content",
        /////             "ZipCode":"String content",
        /////             "Plus4":"String content",
        /////             "Country":"String content",
        /////             "County":"String content",
        /////             "Phone":"String content",
        /////             "PhoneExt":"String content",
        /////             "Fax":"String content",
        /////             "Website":"String content",
        /////             "Mobile":"String content",
        /////             "Gender":"String content",
        /////             "Age":2147483647,
        /////             "Birthdate":"2017-01-01",
        /////             "Income":"String content",
        /////             "EmailRenewPermission":true,
        /////             "FaxPermission":true,
        /////             "MailPermission":true,
        /////             "OtherProductsPermission":true,
        /////             "PhonePermission":true,
        /////             "TextPermission":true,
        /////             "ThirdPartyPermission":true,
        /////             "SubscriberProductDemographics":[
        /////                 { 
        /////                     "Name":"String ",
        /////                     "Value":"String "
        /////                 }
        /////             ],
        /////             "SubscriberProductCustomFields":[
        /////                 {
        /////                     "Name":"String ",
        /////                     "Value":"String "
        /////                 }
        /////             ],
        /////             "SubscriberConsensusCustomFields":[
        /////                 {
        /////                     "Name":"String ",
        /////                     "Value":"String "
        /////                 }
        /////             ],
        /////             "SubscriptionID": 46,
        /////             "ProcessCode": "sample string 47",
        /////             "PubID": 48,
        /////             "IsPubCodeValid": true,
        /////             "IsCodeSheetValid": true,
        /////             "IsProductSubscriberCreated": true,
        /////             "LogMessage": "sample string 52",
        /////             "PubCodeMessage": "sample string 53",
        /////             "CodeSheetMessage": "sample string 54",
        /////             "SubscriberProductMessage": "sample string 55",
        /////             "SubscriberProductIdentifiers": {
        /////               "99b84733-0fc0-4d14-ade7-a209c5420e36": "sample string 2",
        /////               "ad9cd5f3-4864-4fbc-af81-a4a3806435c6": "sample string 4"
        /////             }
        /////         }
        /////     ]
        ///// ]]></example>            
        //[HttpPost]
        //[Route("methods/SaveSubscriber")]
        //public Models.SavedSubscriber SaveSubscriber([FromBody] Models.SaveSubscriber subscription)
        //{
        //    Models.SavedSubscriber retSavedSubscriber = new Models.SavedSubscriber();
        //    if (subscription == null)
        //    {
        //        RaiseInvalidMessageException("No model in request body");
        //    }
        //    else
        //    {
        //        //string test = RequestBody();
        //        //string test;
        //        try
        //        {
        //            //Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();                   
        //            //ADMS.Services.Validator.Validator val = new ADMS.Services.Validator.Validator();
        //            //FrameworkUAD.Object.SaveSubscriber subObject = Transform(subscription);
        //            UAD.API.Models.SaveSubscriber savedSubscriber = SaveSubscriberModel(APIClient, subscription);

        //                  if (savedSubscriber != null)
        //                  {
        //                      retSavedSubscriber = new Models.SavedSubscriber(savedSubscriber);
        //                      if (savedSubscriber.IsPubCodeValid == false || savedSubscriber.IsCodeSheetValid == false)
        //                      {
        //                          RaiseInvalidMessageException("PubCode or CodeSheet not valid - LogMessage: " + savedSubscriber.LogMessage + " - PubCodeMsg: " + savedSubscriber.PubCodeMessage + " - CodeSheetMsg: " + savedSubscriber.CodeSheetMessage + " - SubscriberProductMessage: " + savedSubscriber.SubscriberProductMessage);
        //                      }
        //                      else
        //                      {

        //                      }
        //                  }
        //                  else
        //                  {
        //                      RaiseInvalidMessageException("Error");                        
        //                  }                    
        //              }
        //              catch (Exception ex)
        //              {                    
        //                  //LogError(accessKey, ex, this.GetType().Name.ToString());
        //                  RaiseInternalServerError(Core_AMS.Utilities.StringFunctions.FriendlyServiceError());
        //              }
        //          }

        //          return retSavedSubscriber;
        //      }
        #endregion

        /// <summary>
        /// Ability to add one or multiple subscription, demographics, product adhocs, and consensus adhocs.
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <returns>A list of subscriptions added for <code>Subscription</code></returns>
        ///
        /// <example for="request"><![CDATA[
        ///  POST http://api.kmpsgroup.com/api/subscription/methods/SaveSubscriber HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 4000   
        ///  
        ///     [
        ///         {
        ///             "IGrp_No": "00000000-0000-0000-0000-000000000000",
        ///             "SequenceID": 0,
        ///             "AccountNumber": "0",
        ///             "PubCode": "PUBCODE",
        ///             "QualificationDate": "2017-10-25",
        ///             "CategoryID": 10,
        ///             "TransactionID": 10,
        ///             "QSourceID": 1874,
        ///             "Demo7": "A",
        ///             "Copies": 1,
        ///             "GraceIssues": 1,
        ///             "SubscriberSourceCode": "SS20",
        ///             "Verify": "Yes",
        ///             "Email": "test@test.com",
        ///             "EmailStatus": "Active",
        ///             "FirstName": "Test",
        ///             "LastName": "Test",
        ///             "Company": "Test Inc.",
        ///             "Title": "Owner",
        ///             "Occupation": "Owner",
        ///             "Address1": "123 Main Street",
        ///             "Address2": "Apt 101",
        ///             "Address3": "",
        ///             "City": "Test",
        ///             "State": "Test",
        ///             "ZipCode": "55555",
        ///             "Plus4": "5555",
        ///             "Country": "United States",
        ///             "County": "New Test",
        ///             "Phone": "1234567890",
        ///             "PhoneExt": "12",
        ///             "Fax": "2345678901",
        ///             "Website": "www.test.com",
        ///             "Mobile": "3456789012",
        ///             "Gender": "male",
        ///             "Age": 44,
        ///             "Birthdate": "1980-10-25",
        ///             "Income": "90000",
        ///             "EmailRenewPermission": true,
        ///             "FaxPermission": false,
        ///             "MailPermission": true,
        ///             "OtherProductsPermission": true,
        ///             "PhonePermission": true,
        ///             "TextPermission": false,
        ///             "ThirdPartyPermission": true,
        ///             "SubscriberProductDemographics": [
        ///                 {
        ///                     "Name": "SPD1",
        ///                     "Value": "16"
        ///                 },
        ///                 {
        ///                     "Name": "SPD2",
        ///                     "Value": "06"
        ///                 }
        ///             ],
        ///             "SubscriberProductCustomFields": [
        ///                 {
        ///                     "Name": "SPCF1",
        ///                     "Value": "X45ED"
        ///                 },
        ///                 {
        ///                     "Name": "SPCF2",
        ///                     "Value": "Active"
        ///                 }
        ///             ],
        ///             "SubscriberConsensusCustomFields": [
        ///                 {
        ///                     "Name": "SCCF1",
        ///                     "Value": "Commercial"
        ///                 },
        ///                 {
        ///                     "Name": "SCCF2",
        ///                     "Value": "ENG101"
        ///                 }
        ///             ]
        ///         },
        ///         {
        ///             "IGrp_No": "00000000-0000-0000-0000-000000000000",
        ///             "SequenceID": 0,
        ///             "AccountNumber": "0",
        ///             "PubCode": "PUBCODE",
        ///             "QualificationDate": "2017-10-25",
        ///             "CategoryID": 10,
        ///             "TransactionID": 10,
        ///             "QSourceID": 1874,
        ///             "Demo7": "A",
        ///             "Copies": 1,
        ///             "GraceIssues": 1,
        ///             "SubscriberSourceCode": "SS20",
        ///             "Verify": "Yes",
        ///             "Email": "test@test.com",
        ///             "EmailStatus": "Active",
        ///             "FirstName": "Test",
        ///             "LastName": "Test",
        ///             "Company": "Test Inc.",
        ///             "Title": "Owner",
        ///             "Occupation": "Owner",
        ///             "Address1": "123 Main Street",
        ///             "Address2": "Apt 101",
        ///             "Address3": "",
        ///             "City": "Test",
        ///             "State": "Test",
        ///             "ZipCode": "55555",
        ///             "Plus4": "5555",
        ///             "Country": "United States",
        ///             "County": "New Test",
        ///             "Phone": "1234567890",
        ///             "PhoneExt": "12",
        ///             "Fax": "2345678901",
        ///             "Website": "www.test.com",
        ///             "Mobile": "3456789012",
        ///             "Gender": "male",
        ///             "Age": 44,
        ///             "Birthdate": "1980-10-25",
        ///             "Income": "90000",
        ///             "EmailRenewPermission": true,
        ///             "FaxPermission": false,
        ///             "MailPermission": true,
        ///             "OtherProductsPermission": true,
        ///             "PhonePermission": true,
        ///             "TextPermission": false,
        ///             "ThirdPartyPermission": true,
        ///             "SubscriberProductDemographics": [
        ///                 {
        ///                     "Name": "SPD1",
        ///                     "Value": "16"
        ///                 },
        ///                 {
        ///                     "Name": "SPD2",
        ///                     "Value": "06"
        ///                 }
        ///             ],
        ///             "SubscriberProductCustomFields": [
        ///                 {
        ///                     "Name": "SPCF1",
        ///                     "Value": "X45ED"
        ///                 },
        ///                 {
        ///                     "Name": "SPCF2",
        ///                     "Value": "Active"
        ///                 }
        ///             ],
        ///             "SubscriberConsensusCustomFields": [
        ///                 {
        ///                     "Name": "SCCF1",
        ///                     "Value": "Commercial"
        ///                 },
        ///                 {
        ///                     "Name": "SCCF2",
        ///                     "Value": "ENG101"
        ///                 }
        ///             ]
        ///         }
        ///     ]
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 CREATED
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 10 Apr 2015 19:27:41 GMT
        /// Content-Length: 450
        /// 
        ///     [
        ///         {
        ///             "ProcessCode": "10yO8Y9Aca19_11102017_10:25:08",
        ///             "PubID": 0,
        ///             "StatusCode": "201",
        ///             "StatusMessage": "Saved Subscriber: 00000000-0000-0000-0000-000000000000 for Product: PUBCODE All PubCodes are valid. All Response Groups and Values are valid."
        ///         },
        ///         {
        ///             "ProcessCode": "9IX3ub2G33o7_11102017_10:25:12",
        ///             "PubID": 0,
        ///             "StatusCode": "201",
        ///             "StatusMessage": "Saved Subscriber: 00000000-0000-0000-0000-000000000000 for Product: PUBCODE All PubCodes are valid. All Response Groups and Values are valid."
        ///         }
        ///     ]
        /// ]]></example>  
        [HttpPost]
        [Route("methods/SaveSubscriber")]        
        public IHttpActionResult SaveSubscriber([FromBody] List<Models.SaveSubscriber> subscriptions)
        {
            List<Models.SavedSubscriber> retSavedSubscibers = new List<Models.SavedSubscriber>();
            if (subscriptions == null)
            {
                RaiseInvalidMessageException("No model in request body");
            }
            else
            {
                bool hasError = false;
                try
                {
                    foreach (Models.SaveSubscriber subscription in subscriptions)
                    {
                        UAD.API.Models.SaveSubscriber savedSubscriber = SaveSubscriberModel(APIClient, subscription);
                        Models.SavedSubscriber convertedSaveSubscriber = new Models.SavedSubscriber(savedSubscriber);
                        retSavedSubscibers.Add(convertedSaveSubscriber);
                    }
                }
                catch (Exception ex)
                {
                    hasError = true;
                    RaiseInternalServerError(Core_AMS.Utilities.StringFunctions.FriendlyServiceError());
                }

                if (!hasError)
                {
                    if (subscriptions != null)
                    {
                        int subscriptionCount = subscriptions.Count();
                        int recordFailedCount = subscriptions.Count(x => x.IsPubCodeValid == false);
                        int codesheetFailedCount = subscriptions.Count(x => x.IsCodeSheetValid == false);

                        if (subscriptionCount == recordFailedCount)
                        {
                            return new System.Web.Http.Results.ResponseMessageResult(Request.CreateResponse((HttpStatusCode) 400, retSavedSubscibers));
                        }
                        else
                        {
                            if (recordFailedCount > 0 || codesheetFailedCount > 0)
                            {
                                return new System.Web.Http.Results.ResponseMessageResult(Request.CreateResponse((HttpStatusCode) 207, retSavedSubscibers));
                            }
                        }
                    }
                    else
                    {
                        RaiseInvalidMessageException("Error");
                    }
                }
            }

            return new System.Web.Http.Results.ResponseMessageResult(Request.CreateResponse((HttpStatusCode) 201, retSavedSubscibers));
        }

        /// <summary>
        /// Retrieves the Subscription resource identified by <pre>emailAddress</pre>
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <returns>A list of subscriptions for <code>emailAddress</code></returns>
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/subscription/methods/GetSubscriber HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422   
        ///  
        ///  "someone@somewhere.com"
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 10 Apr 2015 19:27:41 GMT
        /// Content-Length: 259
        /// 
        ///     [
        ///         {
        ///             "ExternalKeyID": -1,
        ///             "FirstName": "Test",
        ///             "LastName": "Test",
        ///             "Title": "Test",
        ///             "Company": "",
        ///             "Address": "",
        ///             "Address2": "",
        ///             "City": "",
        ///             "State": "Test",
        ///             "Zip": "",
        ///             "Plus4": "",
        ///             "ForZip": "",
        ///             "County": "",
        ///             "Country": "Test",
        ///             "Phone": "",
        ///             "Fax": "",
        ///             "Income": null,
        ///             "Gender": "",
        ///             "Address3": "",
        ///             "Mobile": "",
        ///             "Score": 1,
        ///             "Latitude": -1,
        ///             "Longitude": -1,
        ///             "Demo7": "A",
        ///             "IGrp_No": "00000000-0000-0000-0000-000000000000",
        ///             "TransactionDate": "2016-05-10 07:02:00",
        ///             "QDate": "2016-05-09 00:00:00",
        ///             "Email": "test@test.com",
        ///             "SubscriptionID": 0,
        ///             "IsMailable": false,
        ///             "Age": 0,
        ///             "Birthdate": null,
        ///             "Occupation": null,
        ///             "PhoneExt": null,
        ///             "Website": null,
        ///             "FullAddress": null,
        ///             "AccountNumber": null,
        ///             "ExternalKeyId": 0,
        ///             "EmailID": 0,
        ///             "MailPermission": null,
        ///             "FaxPermission": null,
        ///             "PhonePermission": null,
        ///             "OtherProductsPermission": null,
        ///             "ThirdPartyPermission": null,
        ///             "EmailRenewPermission": null,
        ///             "TextPermission": null,
        ///             "demo": [
        ///                 {
        ///                     "Name": "demo1",
        ///                     "DisplayName": "",
        ///                     "Value": ""
        ///                 },
        ///                 {
        ///                     "Name": "demo2",
        ///                     "DisplayName": "",
        ///                     "Value": ""
        ///                 }
        ///             ],
        ///             "ProductList": [
        ///                 {
        ///                     "PubID": 0,
        ///                     "PubSubscriptionID": 0,
        ///                     "SubscriptionStatusID": 3,
        ///                     "Demo7": "A",
        ///                     "QDate": "2016-05-09 00:00:00",
        ///                     "QSourceID": -1,
        ///                     "CategoryID": 101,
        ///                     "TransactionID": 101,
        ///                     "Email": "test@test.com",
        ///                     "StatusUpdatedDate": "2017-11-10 10:47:01.5848079-06:00",
        ///                     "StatusUpdatedReason": "Subscribed",
        ///                     "DateCreated": "2016-05-06 15:25:31.223",
        ///                     "DateUpdated": "2016-05-09 11:46:03.167",
        ///                     "EmailStatus": "Active",
        ///                     "Copies": 1,
        ///                     "GraceIssues": 0,
        ///                     "OnBehalfOf": "",
        ///                     "SubscriberSourceCode": "Test",
        ///                     "OrigsSrc": "Test",
        ///                     "SequenceID": 0,
        ///                     "AccountNumber": "",
        ///                     "Verify": "",
        ///                     "ExternalKeyID": 0,
        ///                     "FirstName": "Test",
        ///                     "LastName": "Test",
        ///                     "Company": "",
        ///                     "Title": "Test",
        ///                     "Occupation": "Test",
        ///                     "Address1": "",
        ///                     "Address2": "",
        ///                     "Address3": "",
        ///                     "City": "",
        ///                     "State": "Test",
        ///                     "ZipCode": "",
        ///                     "Plus4": "",
        ///                     "CarrierRoute": "",
        ///                     "County": "",
        ///                     "Country": "Test",
        ///                     "Latitude": -1,
        ///                     "Longitude": -1,
        ///                     "Phone": "",
        ///                     "Fax": "",
        ///                     "Mobile": "",
        ///                     "Website": "",
        ///                     "Birthdate": "1900-01-01 00:00:00",
        ///                     "Age": 0,
        ///                     "Income": "",
        ///                     "Gender": "",
        ///                     "PhoneExt": "",
        ///                     "IGrp_No": "00000000-0000-0000-0000-000000000000",
        ///                     "ReqFlag": 0,
        ///                     "PubCode": "PUBCODE",
        ///                     "Name": "PUBCODE - PUBNAME",
        ///                     "PubType": "Test",
        ///                     "ClientName": "Test",
        ///                     "FullName": "Test Test",
        ///                     "FullAddress": ", , , Test, , Test",
        ///                     "EmailID": 0,
        ///                     "MailPermission": null,
        ///                     "FaxPermission": null,
        ///                     "PhonePermission": null,
        ///                     "OtherProductsPermission": null,
        ///                     "ThirdPartyPermission": null,
        ///                     "EmailRenewPermission": null,
        ///                     "TextPermission": null,
        ///                     "SubscriberProductDemographics": [
        ///                         {
        ///                             "Name": "SPD1",
        ///                             "Value": ""
        ///                         },
        ///                         {
        ///                             "Name": "SPD2",
        ///                             "Value": ""
        ///                         }
        ///                     ]
        ///                 }
        ///             ]
        ///         }
        ///     ]
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetSubscriber")]
        public List<Models.Subscription> GetSubscriber([FromBody] string emailAddress)
        {
            if (emailAddress == null || string.IsNullOrEmpty(emailAddress))
            {
                RaiseInvalidMessageException("emailAddress is required");
            }

            List<Models.Subscription> subscriptionList = new List<Models.Subscription>();
            
            FrameworkUAD.BusinessLogic.ClientSubscription worker = new FrameworkUAD.BusinessLogic.ClientSubscription();
            List<FrameworkUAD.Object.Subscription> objSubsciption = worker.Select(emailAddress, APIClient.ClientConnections, APIClient.DisplayName, true).ToList();

            if (objSubsciption != null && objSubsciption.Count > 0)
            {
                foreach (FrameworkUAD.Object.Subscription s in objSubsciption)
                {
                    Models.Subscription subscription = new Models.Subscription(s);
                    subscriptionList.Add(subscription);
                }
            }
            else
            {
                RaiseNotFoundException(-1, String.Format(@"Email ""{0}""", emailAddress));
            }

            return subscriptionList;
        }

        /// <summary>
        /// Retrieves the Subscription(s) by <pre>emailAddress</pre> and associated consensus adhocs and master groups specified by name in <pre>dimensions</pre>. If dimensions is not passed then it returns all associated consensus adhocs and master groups.
        /// </summary>
        /// <param name="emailAddress">emailAddress</param>
        /// <param name="dimensions">dimensions</param>
        /// <returns>A list of subscriptions for <code>emailAddress</code> with associated consensus adhocs and master groups data specified by name in <code>dimensions</code>. If dimensions is not passed then it returns all associated consensus adhocs and master groups.</returns>
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/subscription/methods/GetDemographics HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422   
        ///  
        ///  {
        ///     "emailAddress": "someone@somewhere.com",
        ///     "dimensions": [
        ///        {
        ///            "Name": "Name 1",
        ///            "DisplayName": "DisplayName 1",
        ///            "Value": "Value 1"
        ///        },
        ///        {
        ///            "Name": "Name 2",
        ///            "DisplayName": "DisplayName 2",
        ///            "Value": "Value 2"
        ///        }
        ///     ]
        ///  }
        ///  
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 10 Apr 2015 19:27:41 GMT
        /// Content-Length: 259
        /// 
        ///     [
        ///         {
        ///             "ExternalKeyID": -1,
        ///             "FirstName": "Test",
        ///             "LastName": "Test",
        ///             "Title": "",
        ///             "Company": "Test",
        ///             "Address": "",
        ///             "Address2": "",
        ///             "City": "",
        ///             "State": "Test",
        ///             "Zip": "",
        ///             "Plus4": "",
        ///             "ForZip": "",
        ///             "County": "",
        ///             "Country": "Test",
        ///             "Phone": "1234567890",
        ///             "Fax": "",
        ///             "MailPermission": null,
        ///             "FaxPermission": null,
        ///             "PhonePermission": null,
        ///             "OtherProductsPermission": null,
        ///             "ThirdPartyPermission": null,
        ///             "EmailRenewPermission": null,
        ///             "Gender": "",
        ///             "Address3": "",
        ///             "Home_Work_Address": "",
        ///             "Mobile": "",
        ///             "Score": 0,
        ///             "Latitude": -1,
        ///             "Longitude": -1,
        ///             "Demo7": "A",
        ///             "IGrp_No": "00000000-0000-0000-0000-000000000000",
        ///             "Par3C": "-1",
        ///             "TransactionDate": "1900-01-01 00:00:00",
        ///             "QDate": "2017-07-06 00:00:00",
        ///             "Email": "test@test.com",
        ///             "SubscriptionID": 0,
        ///             "IsActive": true,
        ///             "Demographics": [
        ///                 {
        ///                     "Name": "Test",
        ///                     "DisplayName": "Test",
        ///                     "Value": ""
        ///                 }
        ///             ]
        ///         }
        ///     ]
        /// 
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetDemographics")]        
        public List<Models.SubscriberConsensus> GetDemographics([FromBody] Models.ApiParameterDemo parameters)
        {
            string emailAddress = parameters.emailAddress;
            List<Models.SubscriberConsensusDemographic> dimensions = parameters.dimensions;

            if (emailAddress == null || string.IsNullOrEmpty(emailAddress))
            {
                RaiseInvalidMessageException("emailAddress is required");
            }

            List<Models.SubscriberConsensus> subscriberConsensus = new List<Models.SubscriberConsensus>();
            List<FrameworkUAD.Object.SubscriberConsensus> objectSubscriberConsensus = new List<FrameworkUAD.Object.SubscriberConsensus>();

            List<FrameworkUAD.Object.SubscriberConsensusDemographic> modelSubscriberConsensusDemographic = null;
            if (dimensions != null && dimensions.Count() > 0)
                modelSubscriberConsensusDemographic = SubscriberConsensusDemographicTransform(dimensions);

            FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
            objectSubscriberConsensus = worker.GetDemographics(APIClient.ClientConnections, emailAddress, modelSubscriberConsensusDemographic).ToList();
                
            if (objectSubscriberConsensus != null && objectSubscriberConsensus.Count > 0)
            { 
                foreach (FrameworkUAD.Object.SubscriberConsensus sc in objectSubscriberConsensus)
                {
                    Models.SubscriberConsensus msc = new Models.SubscriberConsensus(sc);
                    subscriberConsensus.Add(msc);
                }
            }
            else
            {
                RaiseNotFoundException(-1, String.Format(@"Email ""{0}""", emailAddress));
            }

            return subscriberConsensus;
        }

        /// <summary>
        /// Retrieves the Subscription(s) by <pre>emailAddress</pre> and <pre>productCode</pre> and associated product demographics specified by name in <pre>dimensions</pre>. If dimensions is not passed then it returns all product demographics.
        /// </summary>
        /// <param name="emailAddress">emailAddress</param>
        /// <param name="productCode">productCode</param>
        /// <param name="dimensions">dimensions</param>
        /// <returns>A list of subscriptions for <code>emailAddress</code> and <code>productCode</code> with associated product demographic specified by name in <code>dimensions</code>. If dimensions is not passed then it returns all product demographics.</returns>
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/subscription/methods/GetProductDemographics HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422   
        ///  
        ///  {
        ///     "emailAddress": "someone@somewhere.com",
        ///     "productCode": "product",
        ///     "dimensions": [
        ///        {
        ///            "Name": "Name 1",        
        ///            "Value": "Value 1"
        ///        },
        ///        {
        ///            "Name": "Name 2",        
        ///            "Value": "Value 2"
        ///        }
        ///     ]
        ///  }
        ///  
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 10 Apr 2015 19:27:41 GMT
        /// Content-Length: 259
        /// 
        ///     [
        ///         {
        ///             "ExternalKeyID": 0,
        ///             "FirstName": "",
        ///             "LastName": "",
        ///             "Title": "",
        ///             "Company": "",
        ///             "Address": "",
        ///             "Address2": "",
        ///             "City": "",
        ///             "State": "Test",
        ///             "Zip": "",
        ///             "Plus4": "",
        ///             "ForZip": "",
        ///             "County": "",
        ///             "Country": "Test",
        ///             "Phone": "",
        ///             "Fax": "",
        ///             "MailPermission": false,
        ///             "FaxPermission": false,
        ///             "PhonePermission": false,
        ///             "OtherProductsPermission": false,
        ///             "ThirdPartyPermission": false,
        ///             "EmailRenewPermission": false,
        ///             "Gender": "",
        ///             "Address3": "",
        ///             "Home_Work_Address": "",
        ///             "Mobile": "",
        ///             "Score": 0,
        ///             "Latitude": -1,
        ///             "Longitude": -1,
        ///             "Demo7": "A",
        ///             "IGrp_No": "00000000-0000-0000-0000-000000000000",
        ///             "Par3C": "",
        ///             "TransactionDate": "1900-01-01 00:00:00",
        ///             "QDate": "2016-04-13 00:00:00",
        ///             "Email": "test@test.com",
        ///             "SubscriptionID": 0,
        ///             "IsActive": true,
        ///             "Demographics": [
        ///                 {
        ///                     "Name": "BUSINESS",
        ///                     "Value": ""
        ///                 }
        ///             ]
        ///         }
        ///     ]        
        /// 
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetProductDemographics")]
        public List<Models.SubscriberProduct> GetProductDemographics([FromBody] Models.ApiParameterProductDemo parameters)
        {
            string emailAddress = parameters.emailAddress;
            string productCode = parameters.productCode;
            List<Models.SubscriberProductDemographic> dimensions = parameters.dimensions;

            if (emailAddress == null || string.IsNullOrEmpty(emailAddress))
            {
                RaiseInvalidMessageException("emailAddress is required");
            }
            if (productCode == null || string.IsNullOrEmpty(productCode))
            {
                RaiseInvalidMessageException("productCode is required");
            }

            List<Models.SubscriberProduct> subscriberProduct = new List<Models.SubscriberProduct>();
            List<FrameworkUAD.Object.SubscriberProductDemographic> objectDimensions = null;
            if (dimensions != null && dimensions.Count() > 0)
            {
                objectDimensions = new List<FrameworkUAD.Object.SubscriberProductDemographic>();
                foreach (Models.SubscriberProductDemographic spd in dimensions)
                {
                    FrameworkUAD.Object.SubscriberProductDemographic prodDemo = new FrameworkUAD.Object.SubscriberProductDemographic();
                    prodDemo.Name = spd.Name;
                    prodDemo.Value = spd.Value;
                    prodDemo.DemoUpdateAction = spd.DemoUpdateAction;
                    objectDimensions.Add(prodDemo);
                }
            }

            List<FrameworkUAD.Object.SubscriberProduct> response = new List<FrameworkUAD.Object.SubscriberProduct>();
            FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
            response = worker.GetProductDemographics(APIClient.ClientConnections, emailAddress, productCode, objectDimensions).ToList();

            if (response != null && response.Count > 0)
            {
                foreach (FrameworkUAD.Object.SubscriberProduct sp in response)
                {
                    Models.SubscriberProduct msp = new Models.SubscriberProduct(sp);
                    subscriberProduct.Add(msp);
                }
            }
            else
            {
                RaiseNotFoundException(-1, String.Format(@"Email and ProductCode ""{0}""", emailAddress + " and " + productCode));
            }

            return subscriberProduct;
        }
        #endregion

        #region Helpers
        public List<FrameworkUAD.Object.SubscriberConsensusDemographic> SubscriberConsensusDemographicTransform(List<Models.SubscriberConsensusDemographic> model)
        {
            List<FrameworkUAD.Object.SubscriberConsensusDemographic> sc = new List<FrameworkUAD.Object.SubscriberConsensusDemographic>();
                                
            foreach (Models.SubscriberConsensusDemographic scd in model)
            {
                FrameworkUAD.Object.SubscriberConsensusDemographic demo = new FrameworkUAD.Object.SubscriberConsensusDemographic();
                demo.Name = scd.Name;
                demo.DisplayName = scd.DisplayName;
                demo.Value = scd.Value;
                sc.Add(demo);
            }

            return sc;
        }
        #endregion

        #region SaveSubscriber Methods
        public Models.SaveSubscriber SaveSubscriberModel(KMPlatform.Entity.Client client, UAD.API.Models.SaveSubscriber savedSubscriber)
        {
            savedSubscriber.ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();

            #region Setup for client
            FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();

            FrameworkUAD.BusinessLogic.Product prodData = new FrameworkUAD.BusinessLogic.Product();
            List<FrameworkUAD.Entity.Product> products = prodData.Select(client.ClientConnections);

            List<FrameworkUAD.Entity.EmailStatus> statuses = new List<FrameworkUAD.Entity.EmailStatus>();
            FrameworkUAD.BusinessLogic.EmailStatus workerES = new FrameworkUAD.BusinessLogic.EmailStatus();
            statuses = workerES.Select(client.ClientConnections);
            #endregion

            int sfID = 0;
            try
            {
                bool isDemo = true;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

                //null checks
                if (savedSubscriber.SubscriberProductDemographics == null)                
                    savedSubscriber.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();
                
                if (savedSubscriber.SubscriberProductCustomFields == null)                
                    savedSubscriber.SubscriberProductCustomFields = new List<Models.SubscriberConsensusDemographic>();
                
                if (savedSubscriber.SubscriberConsensusCustomFields == null)                
                    savedSubscriber.SubscriberConsensusCustomFields = new List<Models.SubscriberConsensusDemographic>();
                
                //make sure client has Custom Props
                KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
                client = cData.Select(client.ClientID, true);

                #region SourceFileID from SourceFile table by clientID where FileName = UAD_WS_AddSubscriber
                FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                FrameworkUAS.Entity.SourceFile sourceFile = sfData.Select(client.ClientID, "UAD_WS_AddSubscriber");
                if (sourceFile == null)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Code cworker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    List<FrameworkUAD_Lookup.Entity.Code> codes = cworker.Select(FrameworkUAD_Lookup.Enums.CodeType.File_Recurrence);
                    List<FrameworkUAD_Lookup.Entity.Code> dbFiletypes = cworker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);

                    KMPlatform.BusinessLogic.Service sworker = new KMPlatform.BusinessLogic.Service();
                    KMPlatform.Entity.Service s = sworker.Select(KMPlatform.Enums.Services.UADFILEMAPPER, true);

                    //create the UAD_WS_AddSubscriber file for the client
                    sourceFile = new FrameworkUAS.Entity.SourceFile();
                    sourceFile.FileRecurrenceTypeId = 0;
                    if (codes.Exists(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.Recurring.ToString())))
                        sourceFile.FileRecurrenceTypeId = codes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.Recurring.ToString())).CodeId;
                    sourceFile.DatabaseFileTypeId = dbFiletypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.GetDatabaseFileType(FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString()))).CodeId;
                    sourceFile.FileName = "UAD_WS_AddSubscriber";
                    sourceFile.ClientID = client.ClientID;
                    sourceFile.IsDQMReady = true;
                    sourceFile.ServiceID = s.ServiceID;
                    sourceFile.ServiceFeatureID = 0;
                    if (s.ServiceFeatures.Exists(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.UAD_Api.ToString().Replace("_", " "))))
                        sourceFile.ServiceFeatureID = s.ServiceFeatures.Single(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.UAD_Api.ToString().Replace("_", " "))).ServiceFeatureID;
                    sourceFile.CreatedByUserID = 1;
                    sourceFile.QDateFormat = "MMDDYYYY";
                    sourceFile.BatchSize = 2500;
                    sourceFile.SourceFileID = sfData.Save(sourceFile);
                }
                sfID = sourceFile.SourceFileID;
                #endregion

                #region initial creation of the AdmsLog object
                FrameworkUAS.Entity.AdmsLog admsLog = new FrameworkUAS.Entity.AdmsLog();
                admsLog.ClientId = client.ClientID;
                admsLog.StatusMessage = FrameworkUAD_Lookup.Enums.FileStatusType.Detected.ToString();
                admsLog.AdmsStepId = 0;
                admsLog.DateCreated = DateTime.Now;
                admsLog.FileLogDetails = new System.Collections.Generic.List<FrameworkUAS.Entity.FileLog>();
                admsLog.FileNameExact = "UAD_WS_AddSubscriber";
                admsLog.FileStart = DateTime.Now;
                admsLog.FileStatusId = 0;
                admsLog.ImportFile = null;
                admsLog.ProcessCode = savedSubscriber.ProcessCode;
                admsLog.ProcessingStatusId = 0;
                admsLog.ExecutionPointId = 0;
                admsLog.RecordSource = "API";
                admsLog.SourceFileId = sourceFile.SourceFileID;
                admsLog.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

                FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
                admsLog.AdmsLogId = alWrk.Save(admsLog);
                #endregion

                //set file status object
                admsWrk.Update(admsLog.ProcessCode,
                                         FrameworkUAD_Lookup.Enums.FileStatusType.ApiProcessing,
                                         FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Api_Validation,
                                         FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_ApiValidation,
                                         FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_ApiValidation, 1,
                                         "Start: api data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                         admsLog.SourceFileId);


                //subscription object coming in will have the PubCode set but will not have the PubID set - lets set that
                savedSubscriber = SetProductID(savedSubscriber, ref savedSubscriber, products);
                //now our PubID is set and if Pubcode does not exist IsPubCodeValid will be false
                if (savedSubscriber.IsPubCodeValid == true)
                {
                    //validate subscriber - ProductDetail to codesheet
                    //here we will match product value to codesheet values if not match will remove record
                    savedSubscriber = ValidateProductSubscription(savedSubscriber, ref savedSubscriber, client, statuses);

                    //validate that any incoming SubscriberConsensusDemographic items exist - remove those that do not exist
                    ValidateConsensusDemographics(ref savedSubscriber, client, savedSubscriber.ProcessCode);
                    ValidateProductConsensusDemographics(ref savedSubscriber, client, savedSubscriber.ProcessCode);

                    //insert to SubscriberOriginal & Demo --> SubscriberTransformed and STDemographics
                    savedSubscriber = SaveSubscriberTransformed(client, savedSubscriber, sourceFile, savedSubscriber.ProcessCode);
                    //result.AppendLine("Save complete at " + DateTime.Now.ToString());
                    if (savedSubscriber.IsProductSubscriberCreated == true)
                    {
                        //call DQMCleanse which then runs DQM and calls UAD to move new subscriber to UAD 
                        admsWrk.Update(admsLog.ProcessCode,
                                         FrameworkUAD_Lookup.Enums.FileStatusType.ApiProcessing,
                                         FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Api_End,
                                         FrameworkUAD_Lookup.Enums.ProcessingStatusType.ApiValidated,
                                         FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_ApiSaveSubscriber, 1,
                                         "End: api data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                         admsLog.SourceFileId);

                        //call Address Validation first
                        ADMS.Services.DataCleanser.AddressClean ac = new ADMS.Services.DataCleanser.AddressClean();
                        ac.ExecuteAddressCleanse(admsLog, client);

                        //Add to DQMQue 
                        FrameworkUAS.Entity.DQMQue q = new FrameworkUAS.Entity.DQMQue(savedSubscriber.ProcessCode, client.ClientID, isDemo, false, sfID);
                        FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
                        dqmWorker.Save(q);
                    }
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".SaveSubscriber");
            }

            return savedSubscriber;
        }        

        public Models.SaveSubscriber SetProductID(Models.SaveSubscriber response, ref Models.SaveSubscriber subscription, List<FrameworkUAD.Entity.Product> products)
        {
            try
            {
                var notExistPubCode = new List<Models.SaveSubscriber>();
                var badPubCode = new List<string>();

                #region Product Check   
                if (!string.IsNullOrEmpty(subscription.PubCode))
                {
                    string pubcode = subscription.PubCode;
                    if (products.Exists(x => x.PubCode.Equals(pubcode, StringComparison.CurrentCultureIgnoreCase)))
                        subscription.PubID = products.Single(x => x.PubCode.Equals(pubcode, StringComparison.CurrentCultureIgnoreCase)).PubID;
                    else
                    {
                        notExistPubCode.Add(subscription);
                        if (!badPubCode.Contains(subscription.PubCode))
                            badPubCode.Add(subscription.PubCode);
                    }
                }
                else
                {
                    notExistPubCode.Add(subscription);
                    if (!badPubCode.Contains("xx--BLANK--xx"))
                        badPubCode.Add("xx--BLANK--xx");
                }
                #endregion

                if (badPubCode.Count > 0)
                {
                    response.IsPubCodeValid = false;
                    if (badPubCode.Count == 1)
                    {
                        response.PubCodeMessage += "An invalid PubCode was detected." + System.Environment.NewLine;
                        response.PubCodeMessage += "Invalid PubCode: " + badPubCode.First().ToString() + System.Environment.NewLine;
                    }
                    else
                    {
                        response.PubCodeMessage += "Invalid PubCodes were detected as follows." + System.Environment.NewLine;
                        foreach (string pc in badPubCode)
                        {
                            response.PubCodeMessage += "PubCode: " + pc + System.Environment.NewLine;
                        }
                    }

                    //foreach (FrameworkUAD.Object.ProductSubscription ps in notExistPubCode)
                    //    subscription.Products.Remove(ps);
                }
                else
                {
                    response.IsPubCodeValid = true;
                    response.PubCodeMessage += "All PubCodes are valid." + System.Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".SetProductID");
            }

            return response;
        }        
        public Models.SaveSubscriber ValidateProductSubscription(Models.SaveSubscriber response, ref Models.SaveSubscriber subscription, KMPlatform.Entity.Client client, List<FrameworkUAD.Entity.EmailStatus> statuses)
        {
            try
            {
                //validate subscriber - ProductDetail to codesheet
                //get distinct ps.PubID from subscription.Products
                //get CodeSheet from UAD for each PubID
                List<int> distinctPubIDs = new List<int>();
                if (subscription.PubID != null)
                {
                    //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                    //{
                        //if (ps.Status != null)// && !string.IsNullOrEmpty(ps.Status))
                        //{
                        //    if (statuses.Exists(x => x.Status.Equals(ps.Status.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                        //        ps.EmailStatusID = statuses.SingleOrDefault(x => x.Status.Equals(ps.Status.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID;
                        //}
                        //else
                        //{
                        //    if (statuses.Exists(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                        //        ps.EmailStatusID = statuses.SingleOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID;
                        //}
                        //if (ps != null)
                        //{
                            if (!distinctPubIDs.Contains(subscription.PubID))
                                distinctPubIDs.Add(subscription.PubID);
                        //}
                    //}
                }

                List<FrameworkUAD.Entity.CodeSheet> allCS = new List<FrameworkUAD.Entity.CodeSheet>();
                FrameworkUAD.BusinessLogic.CodeSheet csData = new FrameworkUAD.BusinessLogic.CodeSheet();
                foreach (int pubID in distinctPubIDs)
                {
                    allCS.AddRange(csData.Select(pubID, client.ClientConnections));
                }

                FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                List<Models.SubscriberProductDemographic> badDemos = new List<Models.SubscriberProductDemographic>();
                if (subscription.PubID != null)
                {
                    //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                    //{
                    if (subscription.SubscriberProductDemographics != null)
                    {
                        int pubID = subscription.PubID;
                        foreach (Models.SubscriberProductDemographic d in subscription.SubscriberProductDemographics)
                        {
                            //need to handle leading 0 on single digit numbers
                            if (!string.IsNullOrEmpty(d.Name) && !string.IsNullOrEmpty(d.Value)
                                && !psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                if (d.Value == "0"
                                    || d.Value == "1"
                                    || d.Value == "2"
                                    || d.Value == "3"
                                    || d.Value == "4"
                                    || d.Value == "5"
                                    || d.Value == "6"
                                    || d.Value == "7"
                                    || d.Value == "8"
                                    || d.Value == "9")
                                {
                                    if ((!allCS.Exists(x => x.PubID == pubID &&
                                                        x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                        x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))
                                        &&
                                        (!allCS.Exists(x => x.PubID == pubID &&
                                                        x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                        x.ResponseValue.Equals("0" + d.Value, StringComparison.CurrentCultureIgnoreCase)))

                                        )
                                    {
                                        //bad
                                        badDemos.Add(d);
                                    }
                                }
                                else if (d.Value == "00"
                                    || d.Value == "01"
                                    || d.Value == "02"
                                    || d.Value == "03"
                                    || d.Value == "04"
                                    || d.Value == "05"
                                    || d.Value == "06"
                                    || d.Value == "07"
                                    || d.Value == "08"
                                    || d.Value == "09")
                                {
                                    if ((!allCS.Exists(x => x.PubID == pubID &&
                                                        x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                        x.ResponseValue.Equals(d.Value.Replace("0", "").ToString(), StringComparison.CurrentCultureIgnoreCase)))
                                        &&
                                        (!allCS.Exists(x => x.PubID == pubID &&
                                                        x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                        x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))

                                        )
                                    {
                                        //bad
                                        badDemos.Add(d);
                                    }
                                }
                                else if (!allCS.Exists(x => x.PubID == pubID &&
                                                        x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                        x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    //bad
                                    badDemos.Add(d);
                                }
                            }
                        }
                    }
                    //}
                }

                if (badDemos.Count > 0)
                {
                    response.IsCodeSheetValid = false;
                    if (badDemos.Count == 1)
                    {
                        response.CodeSheetMessage += "An invalid Response Group or Value was detected, this will be removed and not inserted to your UAD." + System.Environment.NewLine;
                        response.CodeSheetMessage += "Invalid Response Group:Value - " + badDemos.First().Name.ToString() + ":" + badDemos.First().Value.ToString() + System.Environment.NewLine;
                    }
                    else
                    {
                        response.CodeSheetMessage += "Invalid Response Groups or Values were detected as follows, these will be removed and not inserted to your UAD." + System.Environment.NewLine;
                        foreach (Models.SubscriberProductDemographic d in badDemos)
                        {
                            string badName = d.Name != null ? d.Name.ToString() : string.Empty;
                            string badValue = d.Value != null ? d.Value.ToString() : string.Empty;
                            response.CodeSheetMessage += "Response Group:Value - " + badName + ":" + badValue + System.Environment.NewLine;
                        }
                    }
                    if (subscription.SubscriberProductDemographics != null)
                    {
                        //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                        //{
                        foreach (Models.SubscriberProductDemographic d in badDemos)
                        {
                            if (subscription.SubscriberProductDemographics.Contains(d))
                                subscription.SubscriberProductDemographics.Remove(d);
                        }
                        //}
                    }
                }
                else
                {
                    response.IsCodeSheetValid = true;
                    response.CodeSheetMessage += "All Response Groups and Values are valid." + System.Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".ValidateProductSubscription");
            }

            return response;
        }        
        public void ValidateConsensusDemographics(ref Models.SaveSubscriber subscription, KMPlatform.Entity.Client client, string processCode)
        {
            //make sure that each SubscriberConsensusDemographic name exists in SubscriptionsExtensionMapper
            //select CustomField from SubscriptionsExtensionMapper where Active = 1

            try
            {
                FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper semWorker = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList = semWorker.SelectAll(client.ClientConnections);

                List<Models.SubscriberConsensusDemographic> badConDemos = new List<Models.SubscriberConsensusDemographic>();
                if (subscription != null && subscription.SubscriberConsensusCustomFields != null && semList != null)
                {
                    foreach (var check in subscription.SubscriberConsensusCustomFields)
                    {
                        if (semList.Exists(x => x.CustomField.Equals(check.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                        {
                            if (!badConDemos.Contains(check))
                                badConDemos.Add(check);
                        }
                    }
                }
                foreach (var rem in badConDemos)
                    subscription.SubscriberConsensusCustomFields.Remove(rem);
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".ValidateConsensusDemographics");
            }
        }        
        public void ValidateProductConsensusDemographics(ref Models.SaveSubscriber subscription, KMPlatform.Entity.Client client, string processCode)
        {
            //make sure that each SubscriberConsensusDemographic name exists in SubscriptionsExtensionMapper
            //select CustomField from SubscriptionsExtensionMapper where Active = 1

            try
            {
                int pubID = subscription.PubID;
                FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections).Where(x => x.PubID == pubID).ToList();

                List<Models.SubscriberConsensusDemographic> badConDemos = new List<Models.SubscriberConsensusDemographic>();
                if (subscription != null && subscription.SubscriberProductCustomFields != null && psemList != null)
                {
                    foreach (var check in subscription.SubscriberProductCustomFields)
                    {
                        if (psemList.Exists(x => x.CustomField.Equals(check.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                        {
                            if (!badConDemos.Contains(check))
                                badConDemos.Add(check);
                        }
                    }
                }
                foreach (var rem in badConDemos)
                    subscription.SubscriberProductCustomFields.Remove(rem);
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".ValidateConsensusDemographics");
            }
        }        
        public Models.SaveSubscriber SaveSubscriberTransformed(KMPlatform.Entity.Client client, Models.SaveSubscriber subscription, FrameworkUAS.Entity.SourceFile sf, string processCode)
        {
            try
            {
                int demoAppendId = 0;
                int demoReplaceId = 0;
                int demoOverwriteId = 0;
                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> demoUpdateCodes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Append")).CodeId.ToString(), out demoAppendId);
                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Replace")).CodeId.ToString(), out demoReplaceId);
                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Overwrite")).CodeId.ToString(), out demoOverwriteId);

                FrameworkUAD_Lookup.BusinessLogic.TransactionCode tWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                FrameworkUAD_Lookup.BusinessLogic.CategoryCode catWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                FrameworkUAD.BusinessLogic.EmailStatus emailWorker = new FrameworkUAD.BusinessLogic.EmailStatus();
                List<FrameworkUAD_Lookup.Entity.TransactionCode> tranCodes = tWorker.SelectActiveIsFree(true).ToList();
                List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = catWorker.SelectActiveIsFree(true).ToList();
                List<FrameworkUAD.Entity.EmailStatus> emailStatusCodes = emailWorker.Select(client.ClientConnections);

                #region Subscription Null Value Checks
                if (subscription.Address1 == null)
                    subscription.Address1 = string.Empty;

                if (subscription.SequenceID == null)
                    subscription.SequenceID = 0;
                if (subscription.AccountNumber == null)
                    subscription.AccountNumber = string.Empty;
                if (subscription.PubCode == null)
                    subscription.PubCode = string.Empty;
                if (subscription.QualificationDate == null)
                    subscription.QualificationDate = DateTime.Now;
                if (subscription.QualificationDate == null)
                    subscription.QualificationDate = DateTimeFunctions.GetMinDate();

                if (subscription.CategoryID == null)
                    subscription.CategoryID = 0;
                if (subscription.TransactionID == null)
                    subscription.TransactionID = 0;
                if (subscription.QSourceID == null)
                    subscription.QSourceID = 0;
                if (subscription.Demo7 == null)
                    subscription.Demo7 = string.Empty;
                if (subscription.Copies == null)
                    subscription.Copies = 0;
                if (subscription.GraceIssues == null)
                    subscription.GraceIssues = 0;
                if (subscription.SubscriberSourceCode == null)
                    subscription.SubscriberSourceCode = string.Empty;
                if (subscription.Verify == null)
                    subscription.Verify = string.Empty;
                if (subscription.Email == null)
                    subscription.Email = string.Empty;
                if (subscription.EmailStatus == null)
                    subscription.EmailStatus = FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString();
                if (subscription.FirstName == null)
                    subscription.FirstName = string.Empty;
                if (subscription.LastName == null)
                    subscription.LastName = string.Empty;
                if (subscription.Company == null)
                    subscription.Company = string.Empty;
                if (subscription.Title == null)
                    subscription.Title = string.Empty;
                if (subscription.Occupation == null)
                    subscription.Occupation = string.Empty;
                if (subscription.Address1 == null)
                    subscription.Address1 = string.Empty;
                if (subscription.Address2 == null)
                    subscription.Address2 = string.Empty;
                if (subscription.Address3 == null)
                    subscription.Address3 = string.Empty;
                if (subscription.City == null)
                    subscription.City = string.Empty;
                if (subscription.State == null)
                    subscription.State = string.Empty;
                if (subscription.ZipCode == null)
                    subscription.ZipCode = string.Empty;
                if (subscription.Plus4 == null)
                    subscription.Plus4 = string.Empty;
                if (subscription.Country == null)
                    subscription.Country = string.Empty;
                if (subscription.County == null)
                    subscription.County = string.Empty;
                if (subscription.Phone == null)
                    subscription.Phone = string.Empty;
                if (subscription.PhoneExt == null)
                    subscription.PhoneExt = string.Empty;
                if (subscription.Fax == null)
                    subscription.Fax = string.Empty;
                if (subscription.Website == null)
                    subscription.Website = string.Empty;
                if (subscription.Mobile == null)
                    subscription.Mobile = string.Empty;
                if (subscription.Gender == null)
                    subscription.Gender = string.Empty;
                if (subscription.Age == null)
                    subscription.Age = 0;
                if (subscription.Birthdate == null)
                    subscription.Birthdate = DateTime.Now;
                if (subscription.Income == null)
                    subscription.Income = string.Empty;

                if (!(subscription.EmailRenewPermission == true || subscription.EmailRenewPermission == false))
                    subscription.EmailRenewPermission = false;
                if (!(subscription.FaxPermission == true || subscription.FaxPermission == false))
                    subscription.FaxPermission = false;
                if (!(subscription.MailPermission == true || subscription.MailPermission == false))
                    subscription.MailPermission = false;
                if (!(subscription.OtherProductsPermission == true || subscription.OtherProductsPermission == false))
                    subscription.OtherProductsPermission = false;
                if (!(subscription.PhonePermission == true || subscription.PhonePermission == false))
                    subscription.PhonePermission = false;
                if (!(subscription.TextPermission == true || subscription.TextPermission == false))
                    subscription.TextPermission = false;
                if (!(subscription.ThirdPartyPermission == true || subscription.ThirdPartyPermission == false))
                    subscription.ThirdPartyPermission = false;

                #endregion
                //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                //{
                if (subscription.SubscriberProductDemographics == null)
                    subscription.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();

                List<FrameworkUAD.Entity.SubscriberOriginal> addSO = new List<FrameworkUAD.Entity.SubscriberOriginal>();
                List<FrameworkUAD.Entity.SubscriberTransformed> addST = new List<FrameworkUAD.Entity.SubscriberTransformed>();
                FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                FrameworkUAD.Entity.SubscriberOriginal newSO = new FrameworkUAD.Entity.SubscriberOriginal();

                #region SO
                newSO.ProcessCode = processCode;
                newSO.Address = subscription.Address1;
                newSO.Address3 = subscription.Address3;
                FrameworkUAD_Lookup.Entity.CategoryCode cc = null;
                if (catCodes.Exists(x => x.CategoryCodeValue == subscription.CategoryID))
                    cc = catCodes.SingleOrDefault(x => x.CategoryCodeValue == subscription.CategoryID);
                newSO.CategoryID = cc != null ? cc.CategoryCodeID : 0;
                newSO.City = subscription.City;
                newSO.Company = subscription.Company;
                newSO.Country = subscription.Country;
                newSO.County = subscription.County;
                newSO.CreatedByUserID = 1;
                newSO.DateCreated = DateTime.Now;
                newSO.MailPermission = subscription.MailPermission;
                newSO.FaxPermission = subscription.FaxPermission;
                newSO.PhonePermission = subscription.PhonePermission;
                newSO.OtherProductsPermission = subscription.OtherProductsPermission;
                newSO.ThirdPartyPermission = subscription.ThirdPartyPermission;
                newSO.EmailRenewPermission = subscription.EmailRenewPermission;
                newSO.TextPermission = subscription.TextPermission;

                newSO.Demo7 = subscription.Demo7;
                newSO.Email = subscription.Email;

                newSO.Fax = subscription.Fax;
                newSO.FName = subscription.FirstName;
                newSO.Gender = subscription.Gender;

                newSO.LName = subscription.LastName;
                newSO.MailStop = subscription.Address2;
                newSO.Mobile =subscription.Mobile;
                newSO.Phone = subscription.Phone;
                newSO.Plus4 = subscription.Plus4;
                newSO.PubCode = subscription.PubCode;

                newSO.ProcessCode = processCode;
                newSO.QDate = subscription.QualificationDate.ToString().Length > 0 ? subscription.QualificationDate : DateTime.Now;
                newSO.QSourceID = subscription.QSourceID > 0 ? subscription.QSourceID : 0;
                newSO.Sequence = subscription.SequenceID;
                newSO.SourceFileID = sf.SourceFileID;
                newSO.State = subscription.State;

                newSO.Title = subscription.Title;
                FrameworkUAD_Lookup.Entity.TransactionCode tc = null;
                if (tranCodes.Exists(x => x.TransactionCodeValue == subscription.TransactionID))
                    tc = tranCodes.SingleOrDefault(x => x.TransactionCodeValue == subscription.TransactionID);
                newSO.TransactionID = tc != null ? tc.TransactionCodeID : 0;
                newSO.Zip = subscription.ZipCode;
                newSO.IsActive = true;
                newSO.AccountNumber = subscription.AccountNumber;
                newSO.Copies = subscription.Copies;
                newSO.GraceIssues = subscription.GraceIssues;
                newSO.SubSrc = subscription.SubscriberSourceCode;
                newSO.Verified = subscription.Verify;

                #region EmailStatus
                string emailStatusValue = subscription.EmailStatus;
                int emailStatusID = -1;
                int emailStatusAsInt = -1;
                int.TryParse(subscription.EmailStatus, out emailStatusAsInt);

                if (emailStatusCodes.FirstOrDefault(x => x.EmailStatusID == emailStatusAsInt) != null)
                    emailStatusID = emailStatusAsInt;
                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.S.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.P.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Unverified.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.U.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.UnSubscribe.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.M.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.MasterSuppressed.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                else if (emailStatusCodes.FirstOrDefault(x => x.Status.Contains(emailStatusValue)) != null)
                    int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Contains(emailStatusValue)).EmailStatusID.ToString(), out emailStatusID);
                else
                {
                    if (!string.IsNullOrEmpty(subscription.Email))
                        int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                    else
                        emailStatusID = -1;
                }
                #endregion

                newSO.EmailStatusID = emailStatusID;
                newSO.Occupation = subscription.Occupation;
                newSO.Website = subscription.Website;

                #endregion
                #region SODemographic
                #region SubscriberProductDemographics
                if (subscription.SubscriberProductDemographics != null)
                {
                    foreach (Models.SubscriberProductDemographic d in subscription.SubscriberProductDemographics)
                    {
                        //d.Name = MAFField
                        //d.Value = Value
                        if (!string.IsNullOrEmpty(d.Value))
                        {
                            FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                            newSDO.CreatedByUserID = 1;
                            newSDO.DateCreated = DateTime.Now;
                            newSDO.MAFField = d.Name;
                            newSDO.NotExists = false;
                            newSDO.PubID = subscription.PubID;
                            newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                            newSDO.Value = d.Value;
                            if (psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                newSDO.IsAdhoc = true;
                            else
                                newSDO.IsAdhoc = false;
                            if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Append)
                                newSDO.DemographicUpdateCodeId = demoAppendId;
                            else if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite)
                                newSDO.DemographicUpdateCodeId = demoOverwriteId;
                            else
                                newSDO.DemographicUpdateCodeId = demoReplaceId;

                            newSO.DemographicOriginalList.Add(newSDO);
                        }
                    }
                }
                else
                    subscription.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();
                #endregion
                #region SubscriberProductCustomFields
                if (subscription.SubscriberProductCustomFields != null)
                {
                    foreach (var cd in subscription.SubscriberProductCustomFields)
                    {
                        if (!string.IsNullOrEmpty(cd.Value))
                        {
                            FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                            newSDO.CreatedByUserID = 1;
                            newSDO.DateCreated = DateTime.Now;
                            newSDO.MAFField = cd.Name;
                            newSDO.NotExists = false;
                            newSDO.PubID = subscription.PubID;
                            newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                            newSDO.Value = cd.Value;
                            newSDO.DemographicUpdateCodeId = demoAppendId;
                            newSDO.IsAdhoc = true;

                            //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                            //    newSDO.IsResponseOther = true;

                            newSO.DemographicOriginalList.Add(newSDO);
                        }
                    }
                }
                #endregion
                #region SubscriberConsensusCustomFields
                if (subscription.SubscriberConsensusCustomFields != null)
                {
                    foreach (var cd in subscription.SubscriberConsensusCustomFields)
                    {
                        if (!string.IsNullOrEmpty(cd.Value))
                        {
                            FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                            newSDO.CreatedByUserID = 1;
                            newSDO.DateCreated = DateTime.Now;
                            newSDO.MAFField = cd.Name;
                            newSDO.NotExists = false;
                            newSDO.PubID = subscription.PubID;
                            newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                            newSDO.Value = cd.Value;
                            newSDO.DemographicUpdateCodeId = demoAppendId;
                            newSDO.IsAdhoc = true;

                            newSO.DemographicOriginalList.Add(newSDO);
                        }
                    }
                }
                #endregion
                #endregion
                addSO.Add(newSO);
                FrameworkUAD.Entity.SubscriberTransformed newST = new FrameworkUAD.Entity.SubscriberTransformed();
                #region ST
                newST.ProcessCode = processCode;
                newST.Address = subscription.Address1;
                newST.Address3 = subscription.Address3;
                newST.CategoryID = newSO.CategoryID;
                newST.City = subscription.City;
                newST.Company = subscription.Company;
                newST.Country = subscription.Country;
                newST.County = subscription.County;
                newST.CreatedByUserID = 1;
                newST.DateCreated = DateTime.Now;
                newST.MailPermission = subscription.MailPermission;
                newST.FaxPermission = subscription.FaxPermission;
                newST.PhonePermission = subscription.PhonePermission;
                newST.OtherProductsPermission = subscription.OtherProductsPermission;
                newST.ThirdPartyPermission = subscription.ThirdPartyPermission;
                newST.EmailRenewPermission = subscription.EmailRenewPermission;
                newST.TextPermission = subscription.TextPermission;
                
                newST.Demo7 = subscription.Demo7.Length > 0 ? subscription.Demo7 : string.Empty;
                newST.Email = subscription.Email.Length > 0 ? subscription.Email : string.Empty;

                newST.Fax = subscription.Fax;
                newST.FName = subscription.FirstName;
                newST.Gender = subscription.Gender;

                newST.LName = subscription.LastName;
                newST.MailStop = subscription.Address2;
                newST.Mobile = subscription.Mobile;
                newST.Phone = subscription.Phone;
                newST.Plus4 = subscription.Plus4;
                newST.PubCode = subscription.PubCode;

                newST.QDate = subscription.QualificationDate.ToString().Length > 0 ? subscription.QualificationDate : DateTime.Now;
                newST.QSourceID = subscription.QSourceID > 0 ? subscription.QSourceID : 0;
                newST.Sequence = subscription.SequenceID;
                newST.SORecordIdentifier = newSO.SORecordIdentifier;
                newST.SourceFileID = sf.SourceFileID;
                newST.State = subscription.State;

                newST.Title = subscription.Title;
                newST.TransactionID = newSO.TransactionID;
                newST.Zip = subscription.ZipCode;
                newST.IsActive = true;// ps.IsActive != null ? ps.IsActive : subscription.IsActive;


                newST.AccountNumber = subscription.AccountNumber;
                newST.Copies = subscription.Copies;
                newST.GraceIssues = subscription.GraceIssues;
                newST.SubSrc = subscription.SubscriberSourceCode;
                newST.Verified = subscription.Verify;
                newST.EmailStatusID = newSO.EmailStatusID;
                newST.Occupation = subscription.Occupation;
                newST.Website = subscription.Website;
                #endregion
                #region STDemographic
                #region SubscriberProductDemographics
                if (subscription.SubscriberProductDemographics != null)
                {
                    foreach (Models.SubscriberProductDemographic d in subscription.SubscriberProductDemographics)
                    {
                        if (!string.IsNullOrEmpty(d.Value))
                        {
                            FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                            newSDT.CreatedByUserID = 1;
                            newSDT.DateCreated = DateTime.Now;
                            newSDT.MAFField = d.Name;
                            newSDT.NotExists = false;
                            newSDT.PubID = subscription.PubID;
                            newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                            newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                            newSDT.Value = d.Value;
                            if (psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                newSDT.IsAdhoc = true;
                            else
                                newSDT.IsAdhoc = false;
                            if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Append)
                                newSDT.DemographicUpdateCodeId = demoAppendId;
                            else if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite)
                                newSDT.DemographicUpdateCodeId = demoOverwriteId;
                            else
                                newSDT.DemographicUpdateCodeId = demoReplaceId;

                            newST.DemographicTransformedList.Add(newSDT);
                        }
                    }
                }
                else
                    subscription.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();
                #endregion
                #region SubscriberProductCustomFields
                if (subscription.SubscriberProductCustomFields != null)
                {
                    foreach (var cd in subscription.SubscriberProductCustomFields)
                    {
                        if (!string.IsNullOrEmpty(cd.Value))
                        {
                            FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                            newSDT.CreatedByUserID = 1;
                            newSDT.DateCreated = DateTime.Now;
                            newSDT.MAFField = cd.Name;
                            newSDT.NotExists = false;
                            newSDT.PubID = subscription.PubID;
                            newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                            newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                            newSDT.Value = cd.Value;
                            newSDT.DemographicUpdateCodeId = demoAppendId;
                            newSDT.IsAdhoc = true;

                            //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                            //    newSDO.IsResponseOther = true;

                            newST.DemographicTransformedList.Add(newSDT);
                        }
                    }
                }
                #endregion
                #region SubscriberConsensusCustomFields
                if (subscription.SubscriberConsensusCustomFields != null)
                {
                    foreach (var cd in subscription.SubscriberConsensusCustomFields)
                    {
                        if (!string.IsNullOrEmpty(cd.Value))
                        {
                            FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                            newSDT.CreatedByUserID = 1;
                            newSDT.DateCreated = DateTime.Now;
                            newSDT.MAFField = cd.Name;
                            newSDT.NotExists = false;
                            newSDT.PubID = subscription.PubID;
                            newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                            newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                            newSDT.Value = cd.Value;
                            newSDT.DemographicUpdateCodeId = demoAppendId;
                            newSDT.IsAdhoc = true;

                            newST.DemographicTransformedList.Add(newSDT);
                        }
                    }
                }
                #endregion
                #endregion
                addST.Add(newST);

                #region do inserts
                FrameworkUAD.BusinessLogic.SubscriberOriginal soData = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
                FrameworkUAD.BusinessLogic.SubscriberTransformed stData = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                if (soData.SaveBulkSqlInsert(addSO, client.ClientConnections) == true && stData.SaveBulkSqlInsert(addST, client.ClientConnections, false) == true)
                {
                    subscription.SubscriberProductMessage += "Saved Subscriber: " + newSO.SORecordIdentifier.ToString() + " for Product: " + newSO.PubCode + System.Environment.NewLine;
                    subscription.IsProductSubscriberCreated = true;
                    //subscription.SubscriberProductIdentifiers.Add(newSO.SORecordIdentifier, newSO.PubCode);
                }
                else
                {
                    subscription.IsProductSubscriberCreated = false;
                    subscription.SubscriberProductMessage += "FAILED to create a Subscriber for Product: " + subscription.PubCode + System.Environment.NewLine;
                }
                #endregion
                //}

            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".SaveSubscriberTransformed");
            }

            return subscription;            
        }
        #endregion

        #region SaveSubscriber List Methods
        public List<Models.SaveSubscriber> SetProductIDList(List<Models.SaveSubscriber> subscriptions, List<FrameworkUAD.Entity.Product> products)
        {
            try
            {
                foreach (Models.SaveSubscriber subscription in subscriptions)
                {
                    var notExistPubCode = new List<Models.SaveSubscriber>();
                    var badPubCode = new List<string>();

                    #region Product Check   
                    if (!string.IsNullOrEmpty(subscription.PubCode))
                    {
                        string pubcode = subscription.PubCode;
                        if (products.Exists(x => x.PubCode.Equals(pubcode, StringComparison.CurrentCultureIgnoreCase)))
                            subscription.PubID = products.Single(x => x.PubCode.Equals(pubcode, StringComparison.CurrentCultureIgnoreCase)).PubID;
                        else
                        {
                            notExistPubCode.Add(subscription);
                            if (!badPubCode.Contains(subscription.PubCode))
                                badPubCode.Add(subscription.PubCode);
                        }
                    }
                    else
                    {
                        notExistPubCode.Add(subscription);
                        if (!badPubCode.Contains("xx--BLANK--xx"))
                            badPubCode.Add("xx--BLANK--xx");
                    }
                    #endregion

                    if (badPubCode.Count > 0)
                    {
                        subscription.IsPubCodeValid = false;
                        if (badPubCode.Count == 1)
                        {
                            subscription.PubCodeMessage += "An invalid PubCode was detected." + System.Environment.NewLine;
                            subscription.PubCodeMessage += "Invalid PubCode: " + badPubCode.First().ToString() + System.Environment.NewLine;
                        }
                        else
                        {
                            subscription.PubCodeMessage += "Invalid PubCodes were detected as follows." + System.Environment.NewLine;
                            foreach (string pc in badPubCode)
                            {
                                subscription.PubCodeMessage += "PubCode: " + pc + System.Environment.NewLine;
                            }
                        }

                        //foreach (FrameworkUAD.Object.ProductSubscription ps in notExistPubCode)
                        //    subscription.Products.Remove(ps);
                    }
                    else
                    {
                        subscription.IsPubCodeValid = true;
                        subscription.PubCodeMessage += "All PubCodes are valid." + System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".SetProductID");
            }

            return subscriptions;
        }
        public List<Models.SaveSubscriber> ValidateProductSubscriptionList(List<Models.SaveSubscriber> subscriptions, KMPlatform.Entity.Client client, List<FrameworkUAD.Entity.EmailStatus> statuses)
        {
            try
            {
                //validate subscriber - ProductDetail to codesheet
                //get distinct ps.PubID from subscription.Products
                //get CodeSheet from UAD for each PubID
                List<int> distinctPubIDs = new List<int>();
                foreach (Models.SaveSubscriber subscription in subscriptions)
                {
                    if (subscription.PubID != null)
                    {
                        if (!distinctPubIDs.Contains(subscription.PubID))
                            distinctPubIDs.Add(subscription.PubID);

                    }
                }

                List<FrameworkUAD.Entity.CodeSheet> allCS = new List<FrameworkUAD.Entity.CodeSheet>();
                FrameworkUAD.BusinessLogic.CodeSheet csData = new FrameworkUAD.BusinessLogic.CodeSheet();
                foreach (int pubID in distinctPubIDs)
                {
                    allCS.AddRange(csData.Select(pubID, client.ClientConnections));
                }

                FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                foreach (Models.SaveSubscriber subscription in subscriptions)
                {
                    List<Models.SubscriberProductDemographic> badDemos = new List<Models.SubscriberProductDemographic>();
                    if (subscription.PubID != null)
                    {
                        //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                        //{
                        if (subscription.SubscriberProductDemographics != null)
                        {
                            int pubID = subscription.PubID;
                            foreach (Models.SubscriberProductDemographic d in subscription.SubscriberProductDemographics)
                            {
                                //need to handle leading 0 on single digit numbers
                                if (!string.IsNullOrEmpty(d.Name) && !string.IsNullOrEmpty(d.Value)
                                    && !psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    if (d.Value == "0"
                                        || d.Value == "1"
                                        || d.Value == "2"
                                        || d.Value == "3"
                                        || d.Value == "4"
                                        || d.Value == "5"
                                        || d.Value == "6"
                                        || d.Value == "7"
                                        || d.Value == "8"
                                        || d.Value == "9")
                                    {
                                        if ((!allCS.Exists(x => x.PubID == pubID &&
                                                            x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                            x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))
                                            &&
                                            (!allCS.Exists(x => x.PubID == pubID &&
                                                            x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                            x.ResponseValue.Equals("0" + d.Value, StringComparison.CurrentCultureIgnoreCase)))

                                            )
                                        {
                                            //bad
                                            badDemos.Add(d);
                                        }
                                    }
                                    else if (d.Value == "00"
                                        || d.Value == "01"
                                        || d.Value == "02"
                                        || d.Value == "03"
                                        || d.Value == "04"
                                        || d.Value == "05"
                                        || d.Value == "06"
                                        || d.Value == "07"
                                        || d.Value == "08"
                                        || d.Value == "09")
                                    {
                                        if ((!allCS.Exists(x => x.PubID == pubID &&
                                                            x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                            x.ResponseValue.Equals(d.Value.Replace("0", "").ToString(), StringComparison.CurrentCultureIgnoreCase)))
                                            &&
                                            (!allCS.Exists(x => x.PubID == pubID &&
                                                            x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                            x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))

                                            )
                                        {
                                            //bad
                                            badDemos.Add(d);
                                        }
                                    }
                                    else if (!allCS.Exists(x => x.PubID == pubID &&
                                                            x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                            x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))
                                    {
                                        //bad
                                        badDemos.Add(d);
                                    }
                                }
                            }
                        }
                        //}
                    }

                    if (badDemos.Count > 0)
                    {
                        subscription.IsCodeSheetValid = false;
                        if (badDemos.Count == 1)
                        {
                            subscription.CodeSheetMessage += "An invalid Response Group or Value was detected, this will be removed and not inserted to your UAD." + System.Environment.NewLine;
                            subscription.CodeSheetMessage += "Invalid Response Group:Value - " + badDemos.First().Name.ToString() + ":" + badDemos.First().Value.ToString() + System.Environment.NewLine;
                        }
                        else
                        {
                            subscription.CodeSheetMessage += "Invalid Response Groups or Values were detected as follows, these will be removed and not inserted to your UAD." + System.Environment.NewLine;
                            foreach (Models.SubscriberProductDemographic d in badDemos)
                            {
                                string badName = d.Name != null ? d.Name.ToString() : string.Empty;
                                string badValue = d.Value != null ? d.Value.ToString() : string.Empty;
                                subscription.CodeSheetMessage += "Response Group:Value - " + badName + ":" + badValue + System.Environment.NewLine;
                            }
                        }
                        if (subscription.SubscriberProductDemographics != null)
                        {
                            //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                            //{
                            foreach (Models.SubscriberProductDemographic d in badDemos)
                            {
                                if (subscription.SubscriberProductDemographics.Contains(d))
                                    subscription.SubscriberProductDemographics.Remove(d);
                            }
                            //}
                        }
                    }
                    else
                    {
                        subscription.IsCodeSheetValid = true;
                        subscription.CodeSheetMessage += "All Response Groups and Values are valid." + System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".ValidateProductSubscription");
            }

            return subscriptions;
        }
        public void ValidateConsensusDemographicsList(ref List<Models.SaveSubscriber> subscriptions, KMPlatform.Entity.Client client, string processCode)
        {
            //make sure that each SubscriberConsensusDemographic name exists in SubscriptionsExtensionMapper
            //select CustomField from SubscriptionsExtensionMapper where Active = 1

            try
            {
                FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper semWorker = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList = semWorker.SelectAll(client.ClientConnections);

                foreach (Models.SaveSubscriber subscription in subscriptions)
                {
                    List<Models.SubscriberConsensusDemographic> badConDemos = new List<Models.SubscriberConsensusDemographic>();

                    if (subscription != null && subscription.SubscriberConsensusCustomFields != null && semList != null)
                    {
                        foreach (var check in subscription.SubscriberConsensusCustomFields)
                        {
                            if (semList.Exists(x => x.CustomField.Equals(check.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                            {
                                if (!badConDemos.Contains(check))
                                    badConDemos.Add(check);
                            }
                        }
                    }
                    foreach (var rem in badConDemos)
                        subscription.SubscriberConsensusCustomFields.Remove(rem);
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".ValidateConsensusDemographics");
            }
        }
        public void ValidateProductConsensusDemographicsList(ref List<Models.SaveSubscriber> subscriptions, KMPlatform.Entity.Client client, string processCode)
        {
            //make sure that each SubscriberConsensusDemographic name exists in SubscriptionsExtensionMapper
            //select CustomField from SubscriptionsExtensionMapper where Active = 1

            try
            {
                FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                foreach (Models.SaveSubscriber subscription in subscriptions)
                {
                    int pubID = subscription.PubID;
                    List<Models.SubscriberConsensusDemographic> badConDemos = new List<Models.SubscriberConsensusDemographic>();
                    if (subscription != null && subscription.SubscriberProductCustomFields != null && psemList != null)
                    {
                        foreach (var check in subscription.SubscriberProductCustomFields)
                        {
                            if (psemList.Where(x => x.PubID == pubID).ToList().Exists(x => x.CustomField.Equals(check.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                            {
                                if (!badConDemos.Contains(check))
                                    badConDemos.Add(check);
                            }
                        }
                    }
                    foreach (var rem in badConDemos)
                        subscription.SubscriberProductCustomFields.Remove(rem);
                }
            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".ValidateConsensusDemographics");
            }
        }
        public List<Models.SaveSubscriber> SaveSubscriberTransformedList(KMPlatform.Entity.Client client, List<Models.SaveSubscriber> subscriptions, FrameworkUAS.Entity.SourceFile sf, string processCode)
        {
            try
            {
                int demoAppendId = 0;
                int demoReplaceId = 0;
                int demoOverwriteId = 0;
                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> demoUpdateCodes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Append")).CodeId.ToString(), out demoAppendId);
                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Replace")).CodeId.ToString(), out demoReplaceId);
                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Overwrite")).CodeId.ToString(), out demoOverwriteId);

                FrameworkUAD_Lookup.BusinessLogic.TransactionCode tWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                FrameworkUAD_Lookup.BusinessLogic.CategoryCode catWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                FrameworkUAD.BusinessLogic.EmailStatus emailWorker = new FrameworkUAD.BusinessLogic.EmailStatus();
                List<FrameworkUAD_Lookup.Entity.TransactionCode> tranCodes = tWorker.SelectActiveIsFree(true).ToList();
                List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = catWorker.SelectActiveIsFree(true).ToList();
                List<FrameworkUAD.Entity.EmailStatus> emailStatusCodes = emailWorker.Select(client.ClientConnections);

                foreach (Models.SaveSubscriber subscription in subscriptions)
                {
                    #region Subscription Null Value Checks
                    if (subscription.Address1 == null)
                        subscription.Address1 = string.Empty;

                    if (subscription.SequenceID == null)
                        subscription.SequenceID = 0;
                    if (subscription.AccountNumber == null)
                        subscription.AccountNumber = string.Empty;
                    if (subscription.PubCode == null)
                        subscription.PubCode = string.Empty;
                    if (subscription.QualificationDate == null)
                        subscription.QualificationDate = DateTime.Now;
                    if (subscription.QualificationDate == null)
                        subscription.QualificationDate = DateTimeFunctions.GetMinDate();

                    if (subscription.CategoryID == null)
                        subscription.CategoryID = 0;
                    if (subscription.TransactionID == null)
                        subscription.TransactionID = 0;
                    if (subscription.QSourceID == null)
                        subscription.QSourceID = 0;
                    if (subscription.Demo7 == null)
                        subscription.Demo7 = string.Empty;
                    if (subscription.Copies == null)
                        subscription.Copies = 0;
                    if (subscription.GraceIssues == null)
                        subscription.GraceIssues = 0;
                    if (subscription.SubscriberSourceCode == null)
                        subscription.SubscriberSourceCode = string.Empty;
                    if (subscription.Verify == null)
                        subscription.Verify = string.Empty;
                    if (subscription.Email == null)
                        subscription.Email = string.Empty;
                    if (subscription.EmailStatus == null)
                        subscription.EmailStatus = FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString();
                    if (subscription.FirstName == null)
                        subscription.FirstName = string.Empty;
                    if (subscription.LastName == null)
                        subscription.LastName = string.Empty;
                    if (subscription.Company == null)
                        subscription.Company = string.Empty;
                    if (subscription.Title == null)
                        subscription.Title = string.Empty;
                    if (subscription.Occupation == null)
                        subscription.Occupation = string.Empty;
                    if (subscription.Address1 == null)
                        subscription.Address1 = string.Empty;
                    if (subscription.Address2 == null)
                        subscription.Address2 = string.Empty;
                    if (subscription.Address3 == null)
                        subscription.Address3 = string.Empty;
                    if (subscription.City == null)
                        subscription.City = string.Empty;
                    if (subscription.State == null)
                        subscription.State = string.Empty;
                    if (subscription.ZipCode == null)
                        subscription.ZipCode = string.Empty;
                    if (subscription.Plus4 == null)
                        subscription.Plus4 = string.Empty;
                    if (subscription.Country == null)
                        subscription.Country = string.Empty;
                    if (subscription.County == null)
                        subscription.County = string.Empty;
                    if (subscription.Phone == null)
                        subscription.Phone = string.Empty;
                    if (subscription.PhoneExt == null)
                        subscription.PhoneExt = string.Empty;
                    if (subscription.Fax == null)
                        subscription.Fax = string.Empty;
                    if (subscription.Website == null)
                        subscription.Website = string.Empty;
                    if (subscription.Mobile == null)
                        subscription.Mobile = string.Empty;
                    if (subscription.Gender == null)
                        subscription.Gender = string.Empty;
                    if (subscription.Age == null)
                        subscription.Age = 0;
                    if (subscription.Birthdate == null)
                        subscription.Birthdate = DateTime.Now;
                    if (subscription.Income == null)
                        subscription.Income = string.Empty;

                    if (!(subscription.EmailRenewPermission == true || subscription.EmailRenewPermission == false))
                        subscription.EmailRenewPermission = false;
                    if (!(subscription.FaxPermission == true || subscription.FaxPermission == false))
                        subscription.FaxPermission = false;
                    if (!(subscription.MailPermission == true || subscription.MailPermission == false))
                        subscription.MailPermission = false;
                    if (!(subscription.OtherProductsPermission == true || subscription.OtherProductsPermission == false))
                        subscription.OtherProductsPermission = false;
                    if (!(subscription.PhonePermission == true || subscription.PhonePermission == false))
                        subscription.PhonePermission = false;
                    if (!(subscription.TextPermission == true || subscription.TextPermission == false))
                        subscription.TextPermission = false;
                    if (!(subscription.ThirdPartyPermission == true || subscription.ThirdPartyPermission == false))
                        subscription.ThirdPartyPermission = false;

                    #endregion
                    //foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                    //{
                    if (subscription.SubscriberProductDemographics == null)
                        subscription.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();

                    List<FrameworkUAD.Entity.SubscriberOriginal> addSO = new List<FrameworkUAD.Entity.SubscriberOriginal>();
                    List<FrameworkUAD.Entity.SubscriberTransformed> addST = new List<FrameworkUAD.Entity.SubscriberTransformed>();
                    FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                    List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                    FrameworkUAD.Entity.SubscriberOriginal newSO = new FrameworkUAD.Entity.SubscriberOriginal();

                    #region SO
                    newSO.ProcessCode = processCode;
                    newSO.Address = subscription.Address1;
                    newSO.Address3 = subscription.Address3;
                    FrameworkUAD_Lookup.Entity.CategoryCode cc = null;
                    if (catCodes.Exists(x => x.CategoryCodeValue == subscription.CategoryID))
                        cc = catCodes.SingleOrDefault(x => x.CategoryCodeValue == subscription.CategoryID);
                    newSO.CategoryID = cc != null ? cc.CategoryCodeID : 0;
                    newSO.City = subscription.City;
                    newSO.Company = subscription.Company;
                    newSO.Country = subscription.Country;
                    newSO.County = subscription.County;
                    newSO.CreatedByUserID = 1;
                    newSO.DateCreated = DateTime.Now;
                    newSO.MailPermission = subscription.MailPermission;
                    newSO.FaxPermission = subscription.FaxPermission;
                    newSO.PhonePermission = subscription.PhonePermission;
                    newSO.OtherProductsPermission = subscription.OtherProductsPermission;
                    newSO.ThirdPartyPermission = subscription.ThirdPartyPermission;
                    newSO.EmailRenewPermission = subscription.EmailRenewPermission;
                    newSO.TextPermission = subscription.TextPermission;

                    newSO.Demo7 = subscription.Demo7;
                    newSO.Email = subscription.Email;

                    newSO.Fax = subscription.Fax;
                    newSO.FName = subscription.FirstName;
                    newSO.Gender = subscription.Gender;

                    newSO.LName = subscription.LastName;
                    newSO.MailStop = subscription.Address2;
                    newSO.Mobile = subscription.Mobile;
                    newSO.Phone = subscription.Phone;
                    newSO.Plus4 = subscription.Plus4;
                    newSO.PubCode = subscription.PubCode;

                    newSO.ProcessCode = processCode;
                    newSO.QDate = subscription.QualificationDate.ToString().Length > 0 ? subscription.QualificationDate : DateTime.Now;
                    newSO.QSourceID = subscription.QSourceID > 0 ? subscription.QSourceID : 0;
                    newSO.Sequence = subscription.SequenceID;
                    newSO.SourceFileID = sf.SourceFileID;
                    newSO.State = subscription.State;

                    newSO.Title = subscription.Title;
                    FrameworkUAD_Lookup.Entity.TransactionCode tc = null;
                    if (tranCodes.Exists(x => x.TransactionCodeValue == subscription.TransactionID))
                        tc = tranCodes.SingleOrDefault(x => x.TransactionCodeValue == subscription.TransactionID);
                    newSO.TransactionID = tc != null ? tc.TransactionCodeID : 0;
                    newSO.Zip = subscription.ZipCode;
                    newSO.IsActive = true;
                    newSO.AccountNumber = subscription.AccountNumber;
                    newSO.Copies = subscription.Copies;
                    newSO.GraceIssues = subscription.GraceIssues;
                    newSO.SubSrc = subscription.SubscriberSourceCode;
                    newSO.Verified = subscription.Verify;

                    #region EmailStatus
                    string emailStatusValue = subscription.EmailStatus;
                    int emailStatusID = -1;
                    int emailStatusAsInt = -1;
                    int.TryParse(subscription.EmailStatus, out emailStatusAsInt);

                    if (emailStatusCodes.FirstOrDefault(x => x.EmailStatusID == emailStatusAsInt) != null)
                        emailStatusID = emailStatusAsInt;
                    else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.S.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                    else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.P.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Unverified.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                    else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.U.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.UnSubscribe.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                    else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.M.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.MasterSuppressed.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                    else if (emailStatusCodes.FirstOrDefault(x => x.Status.Contains(emailStatusValue)) != null)
                        int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Contains(emailStatusValue)).EmailStatusID.ToString(), out emailStatusID);
                    else
                    {
                        if (!string.IsNullOrEmpty(subscription.Email))
                            int.TryParse(emailStatusCodes.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                        else
                            emailStatusID = -1;
                    }
                    #endregion

                    newSO.EmailStatusID = emailStatusID;
                    newSO.Occupation = subscription.Occupation;
                    newSO.Website = subscription.Website;

                    #endregion
                    #region SODemographic
                    #region SubscriberProductDemographics
                    if (subscription.SubscriberProductDemographics != null)
                    {
                        foreach (Models.SubscriberProductDemographic d in subscription.SubscriberProductDemographics)
                        {
                            //d.Name = MAFField
                            //d.Value = Value
                            if (!string.IsNullOrEmpty(d.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                                newSDO.CreatedByUserID = 1;
                                newSDO.DateCreated = DateTime.Now;
                                newSDO.MAFField = d.Name;
                                newSDO.NotExists = false;
                                newSDO.PubID = subscription.PubID;
                                newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDO.Value = d.Value;
                                if (psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                    newSDO.IsAdhoc = true;
                                else
                                    newSDO.IsAdhoc = false;
                                if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Append)
                                    newSDO.DemographicUpdateCodeId = demoAppendId;
                                else if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite)
                                    newSDO.DemographicUpdateCodeId = demoOverwriteId;
                                else
                                    newSDO.DemographicUpdateCodeId = demoReplaceId;

                                newSO.DemographicOriginalList.Add(newSDO);
                            }
                        }
                    }
                    else
                        subscription.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();
                    #endregion
                    #region SubscriberProductCustomFields
                    if (subscription.SubscriberProductCustomFields != null)
                    {
                        foreach (var cd in subscription.SubscriberProductCustomFields)
                        {
                            if (!string.IsNullOrEmpty(cd.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                                newSDO.CreatedByUserID = 1;
                                newSDO.DateCreated = DateTime.Now;
                                newSDO.MAFField = cd.Name;
                                newSDO.NotExists = false;
                                newSDO.PubID = subscription.PubID;
                                newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDO.Value = cd.Value;
                                newSDO.DemographicUpdateCodeId = demoAppendId;
                                newSDO.IsAdhoc = true;

                                //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                                //    newSDO.IsResponseOther = true;

                                newSO.DemographicOriginalList.Add(newSDO);
                            }
                        }
                    }
                    #endregion
                    #region SubscriberConsensusCustomFields
                    if (subscription.SubscriberConsensusCustomFields != null)
                    {
                        foreach (var cd in subscription.SubscriberConsensusCustomFields)
                        {
                            if (!string.IsNullOrEmpty(cd.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                                newSDO.CreatedByUserID = 1;
                                newSDO.DateCreated = DateTime.Now;
                                newSDO.MAFField = cd.Name;
                                newSDO.NotExists = false;
                                newSDO.PubID = subscription.PubID;
                                newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDO.Value = cd.Value;
                                newSDO.DemographicUpdateCodeId = demoAppendId;
                                newSDO.IsAdhoc = true;

                                newSO.DemographicOriginalList.Add(newSDO);
                            }
                        }
                    }
                    #endregion
                    #endregion
                    addSO.Add(newSO);
                    FrameworkUAD.Entity.SubscriberTransformed newST = new FrameworkUAD.Entity.SubscriberTransformed();
                    #region ST
                    newST.ProcessCode = processCode;
                    newST.Address = subscription.Address1;
                    newST.Address3 = subscription.Address3;
                    newST.CategoryID = newSO.CategoryID;
                    newST.City = subscription.City;
                    newST.Company = subscription.Company;
                    newST.Country = subscription.Country;
                    newST.County = subscription.County;
                    newST.CreatedByUserID = 1;
                    newST.DateCreated = DateTime.Now;
                    newST.MailPermission = subscription.MailPermission;
                    newST.FaxPermission = subscription.FaxPermission;
                    newST.PhonePermission = subscription.PhonePermission;
                    newST.OtherProductsPermission = subscription.OtherProductsPermission;
                    newST.ThirdPartyPermission = subscription.ThirdPartyPermission;
                    newST.EmailRenewPermission = subscription.EmailRenewPermission;
                    newST.TextPermission = subscription.TextPermission;

                    newST.Demo7 = subscription.Demo7.Length > 0 ? subscription.Demo7 : string.Empty;
                    newST.Email = subscription.Email.Length > 0 ? subscription.Email : string.Empty;

                    newST.Fax = subscription.Fax;
                    newST.FName = subscription.FirstName;
                    newST.Gender = subscription.Gender;

                    newST.LName = subscription.LastName;
                    newST.MailStop = subscription.Address2;
                    newST.Mobile = subscription.Mobile;
                    newST.Phone = subscription.Phone;
                    newST.Plus4 = subscription.Plus4;
                    newST.PubCode = subscription.PubCode;

                    newST.QDate = subscription.QualificationDate.ToString().Length > 0 ? subscription.QualificationDate : DateTime.Now;
                    newST.QSourceID = subscription.QSourceID > 0 ? subscription.QSourceID : 0;
                    newST.Sequence = subscription.SequenceID;
                    newST.SORecordIdentifier = newSO.SORecordIdentifier;
                    newST.SourceFileID = sf.SourceFileID;
                    newST.State = subscription.State;

                    newST.Title = subscription.Title;
                    newST.TransactionID = newSO.TransactionID;
                    newST.Zip = subscription.ZipCode;
                    newST.IsActive = true;// ps.IsActive != null ? ps.IsActive : subscription.IsActive;


                    newST.AccountNumber = subscription.AccountNumber;
                    newST.Copies = subscription.Copies;
                    newST.GraceIssues = subscription.GraceIssues;
                    newST.SubSrc = subscription.SubscriberSourceCode;
                    newST.Verified = subscription.Verify;
                    newST.EmailStatusID = newSO.EmailStatusID;
                    newST.Occupation = subscription.Occupation;
                    newST.Website = subscription.Website;
                    #endregion
                    #region STDemographic
                    #region SubscriberProductDemographics
                    if (subscription.SubscriberProductDemographics != null)
                    {
                        foreach (Models.SubscriberProductDemographic d in subscription.SubscriberProductDemographics)
                        {
                            if (!string.IsNullOrEmpty(d.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                newSDT.CreatedByUserID = 1;
                                newSDT.DateCreated = DateTime.Now;
                                newSDT.MAFField = d.Name;
                                newSDT.NotExists = false;
                                newSDT.PubID = subscription.PubID;
                                newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                                newSDT.Value = d.Value;
                                if (psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                    newSDT.IsAdhoc = true;
                                else
                                    newSDT.IsAdhoc = false;
                                if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Append)
                                    newSDT.DemographicUpdateCodeId = demoAppendId;
                                else if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite)
                                    newSDT.DemographicUpdateCodeId = demoOverwriteId;
                                else
                                    newSDT.DemographicUpdateCodeId = demoReplaceId;

                                newST.DemographicTransformedList.Add(newSDT);
                            }
                        }
                    }
                    else
                        subscription.SubscriberProductDemographics = new List<Models.SubscriberProductDemographic>();
                    #endregion
                    #region SubscriberProductCustomFields
                    if (subscription.SubscriberProductCustomFields != null)
                    {
                        foreach (var cd in subscription.SubscriberProductCustomFields)
                        {
                            if (!string.IsNullOrEmpty(cd.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                newSDT.CreatedByUserID = 1;
                                newSDT.DateCreated = DateTime.Now;
                                newSDT.MAFField = cd.Name;
                                newSDT.NotExists = false;
                                newSDT.PubID = subscription.PubID;
                                newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                                newSDT.Value = cd.Value;
                                newSDT.DemographicUpdateCodeId = demoAppendId;
                                newSDT.IsAdhoc = true;

                                //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                                //    newSDO.IsResponseOther = true;

                                newST.DemographicTransformedList.Add(newSDT);
                            }
                        }
                    }
                    #endregion
                    #region SubscriberConsensusCustomFields
                    if (subscription.SubscriberConsensusCustomFields != null)
                    {
                        foreach (var cd in subscription.SubscriberConsensusCustomFields)
                        {
                            if (!string.IsNullOrEmpty(cd.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                newSDT.CreatedByUserID = 1;
                                newSDT.DateCreated = DateTime.Now;
                                newSDT.MAFField = cd.Name;
                                newSDT.NotExists = false;
                                newSDT.PubID = subscription.PubID;
                                newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                                newSDT.Value = cd.Value;
                                newSDT.DemographicUpdateCodeId = demoAppendId;
                                newSDT.IsAdhoc = true;

                                newST.DemographicTransformedList.Add(newSDT);
                            }
                        }
                    }
                    #endregion
                    #endregion
                    addST.Add(newST);

                    #region do inserts
                    FrameworkUAD.BusinessLogic.SubscriberOriginal soData = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
                    FrameworkUAD.BusinessLogic.SubscriberTransformed stData = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                    if (soData.SaveBulkSqlInsert(addSO, client.ClientConnections) == true && stData.SaveBulkSqlInsert(addST, client.ClientConnections, false) == true)
                    {
                        subscription.SubscriberProductMessage += "Saved Subscriber: " + newSO.SORecordIdentifier.ToString() + " for Product: " + newSO.PubCode + System.Environment.NewLine;
                        subscription.IsProductSubscriberCreated = true;
                        //subscription.SubscriberProductIdentifiers.Add(newSO.SORecordIdentifier, newSO.PubCode);
                    }
                    else
                    {
                        subscription.IsProductSubscriberCreated = false;
                        subscription.SubscriberProductMessage += "FAILED to create a Subscriber for Product: " + subscription.PubCode + System.Environment.NewLine;
                    }
                    #endregion
                }
                //}

            }
            catch (Exception ex)
            {
                //LogError(ex, client, this.GetType().Name.ToString() + ".SaveSubscriberTransformed");
            }

            return subscriptions;
        }
        #endregion
    }
}