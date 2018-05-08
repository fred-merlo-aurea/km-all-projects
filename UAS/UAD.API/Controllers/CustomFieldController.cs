using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.Description;

using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using FrameworkModel = FrameworkUAD.Object.SaveSubscriber;
using APIModel = UAD.API.Models.SaveSubscriber;

namespace UAD.API.Controllers
{
    /// <summary>
    /// API methods exposing custom fields for subscription management
    /// </summary>
    [RoutePrefix("api/customfield")]
    public class CustomFieldController : AuthenticatedUserControllerBase
    {
        #region abstract member implementations
        
        /// <inheritdoc/>
        public override FrameworkUAD.Object.Enums.Entity FrameworkEntity
        {
            get { return FrameworkUAD.Object.Enums.Entity.Subscription; }
        }

        override public string ControllerName { get { return "CustomField"; } }

        #endregion abstract member implementations

        #region Methods
        /// <summary>
        /// Retrieves the product demographics and product adhocs identified by <pre>productCode</pre> or all demographics and product adhocs if product wasn't provided.
        /// </summary>
        /// <param name="productCode">productCode</param>
        /// <returns>A list of product demographics and product adhocs for <pre>productCode</pre> or a list of demographics and product level adhocs if product wasn't provided.</returns>
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/customfield/methods/GetCustomFields HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422   
        ///  
        ///  "product"
        ///  
        ///  OR pass no parameters to get all
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
        ///             "ProductId": 0,
        ///             "ProductCode": "PubCode",
        ///             "Name": "Test",
        ///             "DisplayName": "Test",
        ///             "DisplayOrder": 0,
        ///             "IsMultipleValue": true,
        ///             "IsRequired": false,
        ///             "IsActive": true,
        ///             "IsAdHoc": false
        ///         },
        ///         {
        ///             "ProductId": 0,
        ///             "ProductCode": "PubCode",
        ///             "Name": "Test",
        ///             "DisplayName": "Test",
        ///             "DisplayOrder": 0,
        ///             "IsMultipleValue": true,
        ///             "IsRequired": false,
        ///             "IsActive": true,
        ///             "IsAdHoc": false
        ///         }
        ///     ]
        /// 
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetCustomFields")]
        public List<Models.CustomField> GetCustomFields([FromBody] string productCode)
        {
            List<Models.CustomField> customFields = new List<Models.CustomField>();

            List<FrameworkUAD.Object.CustomField> response = new List<FrameworkUAD.Object.CustomField>();
            FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
            response = worker.GetCustomFields(APIClient.ClientConnections, productCode).ToList();

            if (response != null && response.Count > 0)
            {
                foreach (FrameworkUAD.Object.CustomField cf in response)
                {
                    Models.CustomField mcf = new Models.CustomField(cf);
                    customFields.Add(mcf);
                }
            }
            else
            {
                RaiseNotFoundException(-1, String.Format(@"ProductCode ""{0}""", productCode));
            }

            return customFields;
        }

        /// <summary>
        /// Retrieves the Consensus adhocs and master groups
        /// </summary>
        /// <returns>A list of Consensus adhocs and master groups</returns>
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/customfield/methods/GetConsensusCustomFields HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422                
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
        ///             "ProductId": 0,
        ///             "ProductCode": "PubCode",
        ///             "Name": "Test",
        ///             "DisplayName": "Test",
        ///             "DisplayOrder": 1,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "IsActive": true,
        ///             "IsAdHoc": false
        ///         },
        ///         {
        ///             "ProductId": 0,
        ///             "ProductCode": "PubCode",
        ///             "Name": "Test",
        ///             "DisplayName": "Test",
        ///             "DisplayOrder": 2,
        ///             "IsMultipleValue": false,
        ///             "IsRequired": false,
        ///             "IsActive": true,
        ///             "IsAdHoc": false
        ///         }
        ///     ]
        /// 
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetConsensusCustomFields")]
        public List<Models.CustomField> GetConsensusCustomFields()
        {
            List<Models.CustomField> customFields = new List<Models.CustomField>();

            List<FrameworkUAD.Object.CustomField> response = new List<FrameworkUAD.Object.CustomField>();
            FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
            response = worker.GetConsensusCustomFields(APIClient.ClientConnections).ToList();

            if (response != null && response.Count > 0)
            {
                foreach (FrameworkUAD.Object.CustomField cf in response)
                {
                    Models.CustomField mcf = new Models.CustomField(cf);
                    customFields.Add(mcf);
                }
            }
            else
            {
                RaiseNotFoundException(-1, null);
            }

            return customFields;
        }

        /// <summary>
        /// Retrieves the valid demographic values identified by a combination of <pre>productCode</pre> and/or <pre>customFieldName</pre>. If no values passed it will return all valid demographic values.
        /// </summary>
        /// <param name="productCode">emailAddress</param>
        /// <param name="customFieldName">productCode</param>        
        /// <returns>A list of valid demographic values identified by a combination of <pre>productCode</pre> and/or <pre>customFieldName</pre></returns>. If no parameters are passed it will return all demographic values.
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/customfield/methods/GetCustomFieldValues HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422   
        ///  
        ///  {
        ///     "productCode": "product",
        ///     "customFieldName": "field"
        ///  }
        ///  
        ///  OR
        ///  
        ///  {
        ///     "productCode": "product"
        ///  }
        ///  
        ///  OR
        /// 
        ///  {
        ///     "customFieldName": "field"
        ///  }
        ///  
        ///  OR blank to retrieve all
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
        ///             "ProductId": 0,
        ///             "ProductCode": "Test",
        ///             "Name": "Test",
        ///             "Value": "Test",
        ///             "Description": "Test",
        ///             "DisplayOrder": 0,
        ///             "IsOther": false,
        ///             "IsConsensus": false
        ///         },
        ///         {
        ///             "ProductId": 0,
        ///             "ProductCode": "Test",
        ///             "Name": "Test",
        ///             "Value": "Test",
        ///             "Description": "Test",
        ///             "DisplayOrder": 0,
        ///             "IsOther": false,
        ///             "IsConsensus": false
        ///         }
        ///     ]       
        /// 
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetCustomFieldValues")]
        public List<Models.CustomFieldValue> GetCustomFieldValues([FromBody] Models.ApiParameterProductCustField parameters)
        {
            string productCode = null;
            if (parameters != null && !string.IsNullOrEmpty(parameters.productCode))
                productCode = parameters.productCode;

            string customFieldName = null;
            if (parameters != null && !string.IsNullOrEmpty(parameters.customFieldName))
                customFieldName = parameters.customFieldName;

            List<Models.CustomFieldValue> customFieldValues = new List<Models.CustomFieldValue>();

            List<FrameworkUAD.Object.CustomFieldValue> response = new List<FrameworkUAD.Object.CustomFieldValue>();
            FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
            response = worker.GetCustomFieldValues(APIClient.ClientConnections, productCode, customFieldName).ToList();

            if (response != null && response.Count > 0)
            {
                foreach (FrameworkUAD.Object.CustomFieldValue cfv in response)
                {
                    Models.CustomFieldValue mcfv = new Models.CustomFieldValue(cfv);
                    customFieldValues.Add(mcfv);
                }
            }
            else
            {
                RaiseNotFoundException(-1, String.Format(@"ProductCode and CustomFieldName ""{0}""", productCode + " and " + customFieldName));
            }

            return customFieldValues;
        }

        /// <summary>
        /// Retrieves the master code sheet values identified by <pre>customFieldName</pre>. If <pre>customFieldName</pre> is not passed then it returns all master code sheet values.
        /// </summary>        
        /// <param name="customFieldName">productCode</param>        
        /// <returns>A list of master code sheet values identified by <pre>customFieldName</pre>. If <pre>customFieldName</pre></returns> is not passed then it returns all master code sheet values.
        ///
        /// <example for="request"><![CDATA[
        ///  GET http://api.kmpsgroup.com/api/customfield/methods/GetConsensusCustomFieldValues HTTP/1.1
        ///  Content-Type: application/json
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.kmpsgroup.com
        ///  Content-Length: 422   
        ///         
        ///  "customFieldName"
        ///  
        ///  OR blank to retrieve all
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
        ///             "ProductId": 0,
        ///             "ProductCode": "",
        ///             "Name": "Test",
        ///             "Value": "Test",
        ///             "Description": "Yes",
        ///             "DisplayOrder": 0,
        ///             "IsOther": false,
        ///             "IsConsensus": false
        ///         },
        ///         {
        ///             "ProductId": 0,
        ///             "ProductCode": "",
        ///             "Name": "Test",
        ///             "Value": "Test",
        ///             "Description": "No",
        ///             "DisplayOrder": 0,
        ///             "IsOther": false,
        ///             "IsConsensus": false
        ///         }
        ///     ]       
        /// 
        /// ]]></example>    
        [HttpGet]
        [Route("methods/GetConsensusCustomFieldValues")]
        public List<Models.CustomFieldValue> GetConsensusCustomFieldValues([FromBody] string customFieldName)
        {
            List<Models.CustomFieldValue> customFieldValues = new List<Models.CustomFieldValue>();

            List<FrameworkUAD.Object.CustomFieldValue> response = new List<FrameworkUAD.Object.CustomFieldValue>();
            FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
            response = worker.GetConsensusCustomFieldValues(APIClient.ClientConnections, customFieldName).ToList();

            if (response != null && response.Count > 0)
            {
                foreach (FrameworkUAD.Object.CustomFieldValue cfv in response)
                {
                    Models.CustomFieldValue mcfv = new Models.CustomFieldValue(cfv);
                    customFieldValues.Add(mcfv);
                }
            }
            else
            {
                RaiseNotFoundException(-1, String.Format(@"CustomFieldName ""{0}""", customFieldName));
            }

            return customFieldValues;
        }
        #endregion
    }
}