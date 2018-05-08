using Circulation_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace Circulation_WS.Service
{
    public class TransactionCode : ServiceBase, ITransactionCode
    {
        public Response<bool> Exists(Guid accessKey, int transactionCodeTypeID, int transactionCodeValue)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "transactionCodeTypeID:" + transactionCodeTypeID.ToString() + " transactionCodeValue:" + transactionCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCode worker = new FrameworkCirculation.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(transactionCodeTypeID, transactionCodeValue);
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
        public Response<List<FrameworkCirculation.Entity.TransactionCode>> SelectActiveIsFree(Guid accessKey, bool isFree)
        {
            Response<List<FrameworkCirculation.Entity.TransactionCode>> response = new Response<List<FrameworkCirculation.Entity.TransactionCode>>();
            try
            {
                string param = "isFree:" + isFree.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "SelectActiveIsFree");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCode worker = new FrameworkCirculation.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectActiveIsFree(isFree);
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
        public Response<FrameworkCirculation.Entity.TransactionCode> SelectTransactionCodeValue(Guid accessKey, int transactionCodeValue)
        {
            Response<FrameworkCirculation.Entity.TransactionCode> response = new Response<FrameworkCirculation.Entity.TransactionCode>();
            try
            {
                string param = "transactionCodeValue:" + transactionCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "SelectTransactionCodeValue");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCode worker = new FrameworkCirculation.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectTransactionCodeValue(transactionCodeValue);
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

        public Response<FrameworkCirculation.Entity.TransactionCode> Select(Guid accessKey, int transactionCodeTypeID, int transactionCodeValue)
        {
            Response<FrameworkCirculation.Entity.TransactionCode> response = new Response<FrameworkCirculation.Entity.TransactionCode>();
            try
            {
                string param = "transactionCodeTypeID:" + transactionCodeTypeID.ToString() + " transactionCodeValue:" + transactionCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "SelectTransactionCodeValue");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCode worker = new FrameworkCirculation.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transactionCodeTypeID, transactionCodeValue);
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
        public Response<List<FrameworkCirculation.Entity.TransactionCode>> Select(Guid accessKey)
        {
            Response<List<FrameworkCirculation.Entity.TransactionCode>> response = new Response<List<FrameworkCirculation.Entity.TransactionCode>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCode worker = new FrameworkCirculation.BusinessLogic.TransactionCode();
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
        public Response<int> Save(Guid accessKey, FrameworkCirculation.Entity.TransactionCode x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkCirculation.Entity.TransactionCode>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.TransactionCode worker = new FrameworkCirculation.BusinessLogic.TransactionCode();
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
