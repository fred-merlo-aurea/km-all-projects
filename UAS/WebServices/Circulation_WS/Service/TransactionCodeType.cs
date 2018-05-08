using Circulation_WS.Interface;
using FrameworkCirculation.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace Circulation_WS.Service
{
    public class TransactionCodeType : ServiceBase, ITransactionCodeType
    {
        public Response<bool> Exists(Guid accessKey, string transactionCodeTypeName)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "transactionCodeTypeName:" + transactionCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCodeType worker = new FrameworkCirculation.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(transactionCodeTypeName);
                    if (response.Result == true || response.Result == false)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<FrameworkCirculation.Entity.TransactionCodeType> Select(Guid accessKey, Enums.TransactionCodeType transactionCodeTypeName)
        {
            Response<FrameworkCirculation.Entity.TransactionCodeType> response = new Response<FrameworkCirculation.Entity.TransactionCodeType>();
            try
            {
                string param = "transactionCodeTypeName:" + transactionCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCodeType worker = new FrameworkCirculation.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transactionCodeTypeName);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<FrameworkCirculation.Entity.TransactionCodeType> Select(Guid accessKey, string transactionCodeTypeName)
        {
            Response<FrameworkCirculation.Entity.TransactionCodeType> response = new Response<FrameworkCirculation.Entity.TransactionCodeType>();
            try
            {
                string param = "transactionCodeTypeName:" + transactionCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCodeType worker = new FrameworkCirculation.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transactionCodeTypeName);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<List<FrameworkCirculation.Entity.TransactionCodeType>> Select(Guid accessKey)
        {
            Response<List<FrameworkCirculation.Entity.TransactionCodeType>> response = new Response<List<FrameworkCirculation.Entity.TransactionCodeType>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCodeType worker = new FrameworkCirculation.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<List<FrameworkCirculation.Entity.TransactionCodeType>> Select(Guid accessKey, bool isFree)
        {
            Response<List<FrameworkCirculation.Entity.TransactionCodeType>> response = new Response<List<FrameworkCirculation.Entity.TransactionCodeType>>();
            try
            {
                string param = "isFree:" + isFree.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCodeType worker = new FrameworkCirculation.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(isFree);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<int> Save(Guid accessKey, FrameworkCirculation.Entity.TransactionCodeType x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkCirculation.Entity.TransactionCodeType>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCodeType worker = new FrameworkCirculation.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
