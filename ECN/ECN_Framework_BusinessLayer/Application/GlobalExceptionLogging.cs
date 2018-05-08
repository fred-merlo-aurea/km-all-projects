using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using ECN_Framework_Common.Objects;
using KM.Common.Entity;

namespace ECN_Framework_BusinessLayer.Application
{
    /// <summary>
    ///  Helper methods for Global exception handling
    /// </summary>
    public class GlobalExceptionLogging
    {
        private const string DefaultGlobalSourceMethodName = "Global.asax";
        private const string HodtBase = "KMWeb";
        private const string PostMethod = "post";
        private const string HttpPostServerVariableKey = "HTTP_HOST";
        readonly string _sourceMethod;
        readonly int _applicationId;

        public GlobalExceptionLogging( string sourceMethod, int applicationId)
        {
            _sourceMethod = sourceMethod;
            _applicationId = applicationId;
        }

        public void LogNonCriticalError(HttpRequest request, Exception exception)
        {
            var userInfo = FormatUserInfoSwollowingErrors(request, exception);
            ApplicationLog.LogNonCriticalError(exception,
                _sourceMethod,
                _applicationId,
                userInfo);
        }

        /// <summary>
        /// This method duplicates functionality of <see cref="LogNonCriticalError(HttpRequest, Exception)" />
        /// keeping ECN session info in case when attempt to get Page Url fails with exception
        /// </summary>
        /// <param name="request">Currente request</param>
        /// <param name="exception">Exception to log</param>
        public void LogNonCriticalErrorWithExtendedHandling(HttpRequest request, Exception exception)
        {
            var logMessage = new StringBuilder();
            try
            {
                AppendLogInfo(request, exception, logMessage);
            }
            catch (Exception ex) // Let's swollow it and log whatever was added
            {
                Trace.TraceError($"Exception formatting log message. Exception: {ex.ToString()}");
            }

            LogNonCriticalErrorWithLogMessage(exception, logMessage.ToString());
        }

        public void LogCriticalErrorWithCustomInfo(Exception exception, string customInfo)
        {
            ApplicationLog.LogCriticalError(exception, _sourceMethod, _applicationId, customInfo);
        }

        public void LogNonCriticalErrorWithLogMessage(Exception exception, string customInfo)
        {
            ApplicationLog.LogNonCriticalError(exception, _sourceMethod, _applicationId, customInfo);
        }

        public void LogCryticalError(Exception exception)
        {
            ApplicationLog.LogCriticalError(exception, DefaultGlobalSourceMethodName, _applicationId);
        }

        public void LogCriticalErrorWithAdminEmailVariables(HttpRequest request, HttpException httpError)
        {
            var adminEmailVariables = new StringBuilder();
            try
            {
                adminEmailVariables.AppendLine($"<BR>Page URL: {HttpContext.Current.Request.ServerVariables[HttpPostServerVariableKey]}{request.RawUrl}");
                adminEmailVariables.AppendLine($"<BR>SPY Info:&nbsp;[{request.UserHostAddress}] / [{request.UserAgent}]");
                if (request.UrlReferrer != null)
                {
                    adminEmailVariables.AppendLine($"<BR>Referring URL: { request.UrlReferrer.ToString() }");
                }
                adminEmailVariables.AppendLine("<BR>HEADERS");
                var headers = new StringBuilder();
                foreach (var key in request.Headers.AllKeys)
                {
                    headers.Append($"<BR>{key}:{request.Headers[key]}");
                }
                adminEmailVariables.AppendLine(headers.ToString());
            }
            catch (Exception ex) // to stay consistent with legacy behavior
            {
                Trace.TraceError($"Exception while formatting log message. Exception: {ex.ToString()}");
            }

            LogCriticalErrorWithCustomInfo(httpError, adminEmailVariables.ToString());
        }

        public void LogCriticalErrorWithRequestBody(HttpRequest request, Exception exception)
        {
            var userInfo = new StringBuilder();
            try
            {
                userInfo.Append(FormatLogMessage(request, exception));

                if (!request.Url.Host.Contains(HodtBase) && request.HttpMethod.Equals(PostMethod, StringComparison.OrdinalIgnoreCase))
                {
                    var requestInput = string.Empty;
                    using (var reader = new StreamReader(request.InputStream))
                    {
                        requestInput = reader.ReadToEnd();
                    }
                    request.InputStream.Seek(0, 0);
                    userInfo.AppendLine($"<BR>Request Body: {requestInput}");
                }
            }
            catch (Exception ex)  // to stay consistent with legacy behavior
            {
                Trace.TraceError($"Exception while formatting log message. Exception: {ex.ToString()}");
            }

            LogCriticalErrorWithCustomInfo(exception, userInfo.ToString());
        }

        public Enums.ErrorMessage ClassifyAndLogHttpException(HttpException httpError, HttpRequest request)
        {
            var error = Enums.ErrorMessage.HardError;
            if (httpError.GetHttpCode() == 404)
            {
                error = Enums.ErrorMessage.PageNotFound;
            }
            else if (httpError.GetHttpCode() == 400
                || httpError.GetBaseException().GetType() == typeof(HttpRequestValidationException))
            {
                error = Enums.ErrorMessage.InvalidLink;
            }
            else if (httpError.GetBaseException().GetType() == typeof(System.Web.UI.ViewStateException))
            {
                error = Enums.ErrorMessage.Timeout;
                this.LogCryticalError(httpError);
            }
            else if (httpError.GetBaseException().GetType() == typeof(ArgumentException))
            {
                error = Enums.ErrorMessage.InvalidLink;
                this.LogCryticalError(httpError);
            }
            else if (httpError.GetBaseException().GetType() == typeof(HttpException)
                || httpError.GetBaseException().GetType() == typeof(HttpRequestValidationException))
            {
                error = Enums.ErrorMessage.InvalidLink;
            }
            else
            {
                this.LogCriticalErrorWithAdminEmailVariables(request, httpError);
            }

            return error;
        }        

        /// <summary>
        ///  Creates log message that includes in Context information
        /// </summary>
        /// <param name="request">Current <see cref="HttpRequest"/></param>
        /// <param name="exception">Exception chatched by global exception handler</param>
        /// <returns></returns>
        public static string FormatLogMessage(HttpRequest request, Exception exception)
        {
            var logMessage = new StringBuilder();

            AppendLogInfo(request, exception, logMessage);

            return logMessage.ToString();
        }

        private static void AppendLogInfo(HttpRequest request, Exception exception, StringBuilder logMessage)
        {
            logMessage.Append(FormatEcnUserInfo());

            logMessage.AppendLine($"<BR>Page URL: {HttpContext.Current.Request.ServerVariables[HttpPostServerVariableKey] + request.RawUrl}");
            logMessage.AppendLine($"<BR>User Agent: {request.UserAgent}");

            var excException = exception as ECNException;
            if (excException != null)
            {
                foreach (var ecnError in excException.ErrorList)
                {
                    logMessage.AppendLine($"<BR>Entity: {ecnError.Entity}");
                    logMessage.AppendLine($"<BR>Method: {ecnError.Method}");
                    logMessage.AppendLine($"<BR>Message: {ecnError.ErrorMessage}");
                }
            }
        }

        private static string FormatEcnUserInfo()
        {
            var ecnSession = ECNSession.CurrentSession();
            var userInfo = new StringBuilder();

            if (ecnSession != null && ecnSession.CurrentUser != null)
            {
                userInfo.AppendLine($"<BR><BR>CustomerID: {ecnSession.CurrentUser.CustomerID}");
                userInfo.AppendLine($"<BR>UserName: {ecnSession.CurrentUser.UserName}");
            }
            return userInfo.ToString();
        }

        private static string FormatUserInfoSwollowingErrors(HttpRequest request, Exception exception)
        {
            try
            {
                return FormatLogMessage(request, exception);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
