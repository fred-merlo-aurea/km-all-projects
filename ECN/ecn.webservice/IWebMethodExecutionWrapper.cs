using System;
using ecn.webservice.classes;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KM.Common.Managers;
using KMPlatform.BusinessLogic.Interfaces;

namespace ecn.webservice
{
    public interface IWebMethodExecutionWrapper
    {
        IResponseManager ResponseManager { get; set; }
        IAPILoggingManager ApiLoggingManager { get; set; }
        ICustomerManager CustomerManager { get; set; }
        IUser UserManager { get; set; }
        IApplicationLogManager ApplicationLogManager { get; set; }
        void Initialize(string serviceName, string methodName, string ecnAccessKey, string logInput);
        string Execute(Func<WebMethodExecutionContext, string> internalFunction);
        string Execute<TParamsType>(
            Func<WebMethodExecutionContext, TParamsType, string> internalFunction,
            TParamsType parameters);
    }
}