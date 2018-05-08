using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

using ECN_Framework_Entities.Accounts;

using EmailMarketing.API.Attributes;

using System.Diagnostics;
using System.Text;

using AccessKeyType = EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType;
using System.Data;

using EmailMarketing.API.ExtentionMethods;
using ecn.common.classes;

namespace EmailMarketing.API.Controllers
{
    [AuthenticationRequired]
    [ExceptionsLogged]
    [Logged]
    [FriendlyExceptions(CatchUnfilteredExceptions = true)]
    [RaisesInvalidMessageOnModelError]
    public abstract class AuthenticatedUserControllerBase : ApiController
    {
        private const string LicenseTypeUnlimited = "UNLIMITED";
        private const string LicenseTypeNA = "N/A";
        private const string NoLicense = "NO LICENSE";
        private const string NoLicenseMessage = "NO LICENSES AVAILABLE";

        #region abstract properties

        /// <summary>
        /// When implemented in a derived class, returns the name of the executing controller
        /// </summary>
        abstract public string ControllerName { get; }

        /// <summary>
        /// When overridden in a derived class, get the framework entity for ECNError. <see cref="RaiseInvalidMessageException"/>
        /// </summary>
        abstract public ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity { get; }

        #endregion
        #region exception helpers

        /// <summary>
        /// throws a 400 including the given message
        /// </summary>
        /// <param name="message"></param>
        protected virtual void RaiseInvalidMessageException (string message)
        {
            List<ECN_Framework_Common.Objects.ECNError> error = new List<ECN_Framework_Common.Objects.ECNError>
                {
                    new ECN_Framework_Common.Objects.ECNError( FrameworkEntity, ECN_Framework_Common.Objects.Enums.Method.Save, message)
                };

            ECN_Framework_Common.Objects.ECNException exception =
                new ECN_Framework_Common.Objects.ECNException(error, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);

            throw exception;
        }

        /// <summary>
        /// throws a 400 error including a message composed by String.Format 
        /// replacing <code>args</code> in <code>format</code>
        /// </summary>
        /// <param name="format">message with <code>{n}</code> placeholders</param>
        /// <param name="args">objects to replace into format, calling ToString() on each if necessary</param>
        protected void RaiseInvalidMessageException(string format, params object[] args)
        {
            RaiseInvalidMessageException(String.Format(format, args));
        }

        /// <summary>
        /// Causes a general 500 error to be returned, given message is passed to the log as "logNote"
        /// </summary>
        /// <param name="logMessage"></param>
        protected virtual void RaiseInternalServerError(string logMessage)
        {
            HttpStatusCode failureCode = HttpStatusCode.InternalServerError;
            HttpResponseMessage failureMessage = new HttpResponseMessage(failureCode);
            LogError(1, new ApplicationException(logMessage), failureMessage.ReasonPhrase);
            throw new HttpResponseException(failureMessage);
        }

        /// <summary>
        /// Format arguments using String.Format to construct the logMessage
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        protected void RaiseInternalServerError(string format, params object[] args)
        {
            RaiseInternalServerError(String.Format(format, args));
        }

        /// <summary>
        /// Causes a standard <code>404 Not Found</code> error to be returned including the given resource type (e.g. "blast")
        /// and the ID of a resource of that type that does not exist.
        /// </summary>
        /// <param name="resourceID">ID of missing resource</param>
        /// <param name="resourceTypeName">defaults to the value returned by <see cref="ControllerName"/></param>
        protected virtual void RaiseNotFoundException(int resourceID, string resourceTypeName = null)
        {
            throw Exceptions.APIResourceNotFoundException.Factory(resourceTypeName??ControllerName, resourceID);
            /*
            throw new HttpResponseException(Request.CreateErrorResponse(
                   HttpStatusCode.NotFound,
                   Exceptions.APIResourceNotFoundException.GetAPIResourceNotFoundExceptionMessage(
                       Exceptions.APIResourceNotFoundException.Factory(resourceTypeName??ControllerName, resourceID))));
             * */
        }

        /// <summary>
        /// Causes a standard <code>404 Not Found</code> error to be returned including the given resource type (e.g. "blast")
        /// and the ID of a resource of that type that does not exist.  NOTE: this version should be used only
        /// if RaseNotFoundException() results in a 500 (rather than 404 expected.)
        /// </summary>
        /// <param name="resourceID">ID of missing resource</param>
        /// <param name="resourceTypeName">defaults to the value returned by <see cref="ControllerName"/></param>
        protected virtual void RaiseNotFoundExceptionViaDirectThrow(int resourceID, string resourceTypeName = null)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(
                   HttpStatusCode.NotFound,
                   Exceptions.APIResourceNotFoundException.GetAPIResourceNotFoundExceptionMessage(
                       Exceptions.APIResourceNotFoundException.Factory(resourceTypeName??ControllerName, resourceID))));
        }

        #endregion exception helper
        #region model validation helper

        protected void EnsureModelIsValid(object model)
        {
            if (model == null || false == ModelState.IsValid)
            {
                RaiseInvalidMessageException(String.Format("invalid model: {0}",
                    String.Join(",", from x in ModelState.Values
                                     select String.Join(",",
                                          from y in x.Errors
                                          select y.ErrorMessage))));
            }
        }

        #endregion model validation helper
        #region properties

        /// <summary>
        /// Get the APIAccessKey extracted from HTTP Headers
        /// </summary>
        public string APIAccessKey
        {
            get
            {
                return Request.Properties.ContainsKey(Strings.Headers.APIAccessKeyHeader) ? Request.Properties[Strings.Headers.APIAccessKeyHeader].ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Get the ID of the customer extracted from HTTP Headers
        /// </summary>
        public int CustomerID
        {
            get
            {
                return (int)Request.Properties[Strings.Headers.CustomerIdHeader];
            }
        }

        /// <summary>
        /// Get the APIAccessKey extracted from HTTP Headers
        /// </summary>
        public ECN_Framework_Entities.Accounts.BaseChannel APIBaseChannel
        {
            get
            {
                if (Request.Properties.ContainsKey(Strings.Properties.APIBaseChannelStashKey))
                {
                    return (ECN_Framework_Entities.Accounts.BaseChannel)Request.Properties[Strings.Properties.APIBaseChannelStashKey];
                }
                else if (Request.Properties.ContainsKey(Strings.Properties.APICustomerStashKey))
                {
                    return ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(APICustomer.BaseChannelID.Value);
                }
                else
                    return null;
                
            }
        }

        /// <summary>
        /// Get the current Framework user object inferred from HTTP Headers
        /// </summary>
        public KMPlatform.Entity.User APIUser
        {
            get
            {
                return Request.Properties.ContainsKey(Strings.Headers.APIAccessKeyHeader)
                     ? (KMPlatform.Entity.User)Request.Properties[Strings.Properties.APIUserStashKey]
                     : null;
            }
        }

        /// <summary>
        /// Get the current Framework customer object inferred from HTTP headers
        /// </summary>
        public ECN_Framework_Entities.Accounts.Customer APICustomer
        {
            get
            {
                return (ECN_Framework_Entities.Accounts.Customer)Request.Properties[Strings.Properties.APICustomerStashKey];
            }
        }

        /// <summary>
        /// Get/Set the ErrorLogID written during this request, if any.
        /// </summary>
        public int? CommonErrorLogID { get; set; }

        /// <summary>
        /// The name of the invoked method
        /// </summary>
        public string MethodName
        {
            //get; internal set; 
            get
            {
                return GetRouteDataStringValueOrEmpty(Strings.Routing.RouteMethodKey);
            }
        }

        /// <summary>
        /// Returns the name of the controller responding to the current request from routing data.
        /// </summary>
        public virtual string RouteDataControllerName
        {
            //get; internal set; 
            get
            {
                return GetRouteDataStringValueOrEmpty(Strings.Routing.RouteControllerKey);
            }
        }

        /// <summary>
        /// Returns the ID value for the subject of the current request from routing data
        /// </summary>
        public string SubjectID
        {
            //get; internal set; 
            get
            {
                return GetRouteDataStringValueOrEmpty(Strings.Routing.RouteSubjectIDKey);
            }
        }

        /// <summary>
        /// Retrieve the value of a particular key from the route data of the current request
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetRouteDataStringValueOrEmpty(string key)
        {
            var routeData = Request.GetRouteData().Values;
            return routeData.ContainsKey(key) ? routeData[key].ToString() : String.Empty;
        }

        /// <summary>
        /// Exposes the logging instance for this controller
        /// </summary>
        public ECN_Framework_Entities.Communicator.APILogging Logger;

        /// <summary>
        /// (TODO)  extracts interesting datapoints from the request/environment and returns an XML string for logging
        /// </summary>
        /// <returns>XML as string</returns>
        public string RequestSignatureXML
        {
            get
            {
               // return @"<root>\r\n          <request>\r\n            <uri>" + Request.RequestUri.ToString() + @"</uri>\r\n          </request>\r\n        </root>";
                return HttpContext.Current.Request.ToRaw();
            }
        }

        string _requestSignatureString = null;
        /// <summary>
        /// Translates selected request properties into an API "method signature" for logging
        /// </summary>
        public string RequestSignatureString
        {
            get
            {
                if (_requestSignatureString == null)
                {
                    //return ControllerName + "." + MethodName + "(" + SubjectID + ")";
                    string[] parts = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                    if (parts.Any(x => x.Equals("search", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        parts = parts.SkipWhile(x => false == x.Equals("search", StringComparison.InvariantCultureIgnoreCase)).ToArray();
                    }
                    else if (parts.Any(x => x.Equals("api", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        parts = parts.SkipWhile(x => false == x.Equals("api", StringComparison.InvariantCultureIgnoreCase)).Skip(1).ToArray();
                    }
                    _requestSignatureString = String.Join(".", parts);
                }
                return _requestSignatureString;
            }

        }

        /// <summary>
        /// Verifies the license and raise invalid exception message of no license available
        /// </summary>
        public void CheckLicense()
        {
            var licenseCheck = new LicenseCheck();
            var licensed = licenseCheck.Current(APICustomer.CustomerID.ToString());
            var licenseAvailable = licenseCheck.Available(APICustomer.CustomerID.ToString());

            if (licensed.Equals(LicenseTypeUnlimited))
            {
                licenseAvailable = LicenseTypeNA;
            }

            if (licenseAvailable == NoLicense)
            {
                RaiseInvalidMessageException(NoLicenseMessage);
            }
        }

        internal void LogRequest()
        {
            Logger = new ECN_Framework_Entities.Communicator.APILogging();
            Logger.Input = RequestSignatureXML ;
            Logger.AccessKey = APIAccessKey;
            Logger.APIMethod = RequestSignatureString;
            Logger.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Logger);
        }


        /// <summary>
        /// Emits a log message out to the database and DOES update the master log record for this request.
        /// </summary>
        /// <param name="severity">error is deemed critical if 1 (one) is passed</param>
        /// <param name="Ex">An exception object</param>
        /// <param name="httpErrorReason">The "ReasonPhrase" associated with the HTTP status code returned for this request.</param>
        internal void LogError(int severity, Exception Ex, string httpErrorReason)
        {
            try
            {
                int applicationID = Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]);
                string note = String.Format("{0}\n{1}", httpErrorReason, HttpContext.Current.Request.ToRaw());
            CommonErrorLogID = severity == 1
                    ? KM.Common.Entity.ApplicationLog.LogCriticalError(Ex, RequestSignatureString, applicationID, note: note)
                    : KM.Common.Entity.ApplicationLog.LogNonCriticalError(Ex.ToString(), RequestSignatureString, applicationID, note: note);
                UpdateLog();
            }
            catch { }
        }

        /// <summary>
        /// Updates the application log entry, appending <see cref="CommonErrorLogID"/> if
        /// an error was recorded in the common ErrorLog during this request.
        /// </summary>
        internal void UpdateLog()
        {
            UpdateLog(CommonErrorLogID);
        }

        /// <summary>
        /// Used to update the application log entry for the current request.
        /// </summary>
        /// <param name="ErrorLogID">Log ID for the error record associated with the current request, or NULL if no error occured.</param>
        internal void UpdateLog(int? ErrorLogID)
        {
            if (null != APIAccessKey && null != Logger)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Logger.APILogID, ErrorLogID);
            }
        }

        /// <summary>
        /// Convenience method, creates an HttpResponseMessage from a
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="model"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        internal HttpResponseMessage CreateResponseWithLocation<T>(HttpStatusCode statusCode, T model, int id, string routeName = Strings.Routing.DefaultApiRouteName)
        {
            var response = Request.CreateResponse(statusCode, model);
            response.Headers.Location = new Uri(Url.Link(routeName, new
            {
                controller = ControllerName,
                id = id
            }));
            return response;
        }

        #endregion public properties and methods
    }
}