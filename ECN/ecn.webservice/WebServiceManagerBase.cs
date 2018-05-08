using System;
using System.Configuration;
using System.Web.Services;

namespace ecn.webservice
{
    public abstract class WebServiceManagerBase : WebService
    {
        protected IWebMethodExecutionWrapper _executionWrapper;

        protected WebServiceManagerBase()
        {
        }

        protected WebServiceManagerBase(IWebMethodExecutionWrapper executionWrapper)
        {
            _executionWrapper = executionWrapper;
        }

        protected int LogUnspecifiedException(Exception ex, string sourceMethod)
        {
            return KM.Common.Entity.ApplicationLog.LogCriticalError(ex, sourceMethod, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
        }

        protected IWebMethodExecutionWrapper InitializeExecutionWrapper(
            string serviceName,
            string methodName,
            string ecnAccessKey,
            string logInput)
        {
            var wrapper = GetExecutionWrapper();

            wrapper.Initialize(serviceName,
                methodName,
                ecnAccessKey,
                logInput);

            return wrapper;
        }

        private IWebMethodExecutionWrapper GetExecutionWrapper()
        {
            if (_executionWrapper != null)
            {
                return _executionWrapper;
            }

            return new WebMethodExecutionWrapper();
        }
    }
}