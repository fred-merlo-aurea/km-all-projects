using System;
using System.Configuration;
using ecn.webservice.classes;

namespace ecn.webservice.Facades
{
    public abstract class FacadeBase
    {
        private const string ApplicationIdSetting = "KMCommon_Application";

        protected string GetSuccessResponse(WebMethodExecutionContext context, string output, int id = 0)
        {
            return context.ResponseManager.GetResponse(
                context.MethodName,
                SendResponse.ResponseCode.Success,
                id,
                output);
        }

        protected string GetFailResponse(WebMethodExecutionContext context, string output, int id = 0)
        {
            return context.ResponseManager.GetResponse(
                context.MethodName,
                SendResponse.ResponseCode.Fail,
                id,
                output);
        }

        protected int LogUnspecifiedException(Exception exception, string sourceMethod, string note = "")
        {
            var appId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdSetting]);
            return KM.Common.Entity.ApplicationLog.LogCriticalError(
                exception, 
                sourceMethod,
                appId);
        }
    }
}