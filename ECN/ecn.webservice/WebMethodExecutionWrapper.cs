using System;
using System.Configuration;
using ecn.webservice.classes;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Common.Objects;
using KM.Common.Managers;
using KMPlatform.BusinessLogic.Interfaces;
using KMPlatform.Entity;
using KMPlatform.Object;

namespace ecn.webservice
{
    public class WebMethodExecutionWrapper : IWebMethodExecutionWrapper
    {
        private const string ApplicationIdSetting = "KMCommon_Application";

        private ECN_Framework_Entities.Communicator.APILogging Log;
        private string _serviceMethodName;
        private string _methodName;
        private string _ecnAccessKey;
        private string _logInput;
        private int? LogID;

        private IResponseManager _responseManager;
        private IAPILoggingManager _apiLoggingManager;
        private ICustomerManager _customerManager;
        private IUser _userManager;
        private IApplicationLogManager _applicationLogManager;

        public IResponseManager ResponseManager
        {
            get
            {
                if (_responseManager == null)
                {
                    _responseManager = new ResponseManager();
                }
                return _responseManager;
            }
            set
            {
                _responseManager = value;
            }
        }

        public IAPILoggingManager ApiLoggingManager
        {
            get
            {
                if (_apiLoggingManager == null)
                {
                    _apiLoggingManager = new APILoggingManager();
                }
                return _apiLoggingManager;
            }
            set
            {
                _apiLoggingManager = value;
            }
        }

        public ICustomerManager CustomerManager
        {
            get
            {
                if (_customerManager == null)
                {
                    _customerManager = new CustomerManager();
                }
                return _customerManager;
            }
            set
            {
                _customerManager = value;
            }
        }

        public IUser UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = new KMPlatform.BusinessLogic.User();
                }
                return _userManager;
            }
            set
            {
                _userManager = value;
            }
        }

        public IApplicationLogManager ApplicationLogManager
        {
            get
            {
                if (_applicationLogManager == null)
                {
                    _applicationLogManager = new ApplicationLogManager();
                }
                return _applicationLogManager;
            }
            set
            {
                _applicationLogManager = value;
            }
        }
        
        public void Initialize(string serviceMethodName, string methodName, string ecnAccessKey, string logInput)
        {
            _serviceMethodName = serviceMethodName;
            _methodName = methodName;
            _ecnAccessKey = ecnAccessKey;
            _logInput = logInput;
        }

        public string Execute(Func<WebMethodExecutionContext, string> internalFunction)
        {
            Func<WebMethodExecutionContext, User, string> internalFunctionCall = (context, user) =>
            {
                var executionContext = CreateExecutionContext(user);
                return internalFunction(executionContext);
            };

            return ExecuteWtihExceptionHandling(internalFunctionCall);
        }

        public string Execute<TFunctionParams>(
            Func<WebMethodExecutionContext, TFunctionParams, string> internalFunction,
            TFunctionParams parameters)
        {
            Func<WebMethodExecutionContext, User, string> internalFunctionCall = (context, user) =>
            {
                var executionContext = CreateExecutionContext(user);
                return internalFunction(executionContext, parameters);
            };

            return ExecuteWtihExceptionHandling(internalFunctionCall);
        }

        private string ExecuteWtihExceptionHandling(
            Func<WebMethodExecutionContext, User, string> internalFunction)
        {
            try
            {
                Guid localGuid;
                if (!Guid.TryParse(_ecnAccessKey, out localGuid))
                {
                    return GetFailResponse(Consts.InvalidEcnAccessKeyResponseOutput);
                }

                Log = new ECN_Framework_Entities.Communicator.APILogging
                {
                    AccessKey = _ecnAccessKey,
                    APIMethod = _serviceMethodName,
                    Input = _logInput
                };
                Log.APILogID = ApiLoggingManager.Insert(Log);

                var user = UserManager.LogIn(localGuid);
                if (user != null)
                {
                    var customer = CustomerManager.GetByClientID(user.DefaultClientID, false);
                    if (customer != null)
                    {
                        user.CustomerID = customer.CustomerID;
                    }
                    else
                    {
                        throw new SecurityException("SECURITY VIOLATION!");
                    }

                    var executionContext = CreateExecutionContext(user);
                    return internalFunction(executionContext, user);
                }
                
                ApiLoggingManager.UpdateLog(Log.APILogID, null);
                return GetFailResponse(Consts.LoginFailedResponseOutput);
            }
            catch (ECNException ecnException)
            {
                ApiLoggingManager.UpdateLog(Log.APILogID, null);
                return GetFailResponse(ECNException.CreateErrorMessage(ecnException));
            }
            catch (SecurityException)
            {
                ApiLoggingManager.UpdateLog(Log.APILogID, null);
                return GetFailResponse(Consts.SecurityViolationResponseOutput);
            }
            catch (UserLoginException loginException)
            {
                ApiLoggingManager.UpdateLog(Log.APILogID, null);
                return GetFailResponse(loginException.UserStatus.ToString());
            }
            catch (Exception exception)
            {
                LogID = LogUnspecifiedException(exception, _serviceMethodName);
                ApiLoggingManager.UpdateLog(Log.APILogID, LogID);
                return GetFailResponse(exception.ToString());
            }
        }

        private string GetFailResponse(string output)
        {
            return ResponseManager.GetResponse(
                _methodName,
                SendResponse.ResponseCode.Fail,
                0,
                output);
        }

        private WebMethodExecutionContext CreateExecutionContext(User user)
        {
            return new WebMethodExecutionContext
            {
                User = user,
                ApiLoggingManager = ApiLoggingManager,
                ResponseManager = ResponseManager,
                ServiceMethodName = _serviceMethodName,
                MethodName = _methodName,
                ApiLogId = Log.APILogID
            };
        }

        private int LogUnspecifiedException(Exception exception, string sourceMethod)
        {
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdSetting]);

            return ApplicationLogManager.LogCriticalError(
                exception,
                sourceMethod,
                applicationId);
        }
    }
}