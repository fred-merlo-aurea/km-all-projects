using Circulation_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace Circulation_WS.Service
{
    public class SubscriptionStatus : ServiceBase, ISubscriptionStatus
    {
        public Response<List<FrameworkCirculation.Entity.SubscriptionStatus>> Select(Guid accessKey)
        {
            Response<List<FrameworkCirculation.Entity.SubscriptionStatus>> response = new Response<List<FrameworkCirculation.Entity.SubscriptionStatus>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatus worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatus();
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
        public Response<FrameworkCirculation.Entity.SubscriptionStatus> Select(Guid accessKey, int categoryCodeID, int transactionCodeID)
        {
            Response<FrameworkCirculation.Entity.SubscriptionStatus> response = new Response<FrameworkCirculation.Entity.SubscriptionStatus>();
            try
            {
                string param = "categoryCodeID:" + categoryCodeID.ToString() + " transactionCodeID:" + transactionCodeID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatus worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatus();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(categoryCodeID, transactionCodeID);
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
        public Response<FrameworkCirculation.Entity.SubscriptionStatus> Select(Guid accessKey, int subscriptionStatusID)
        {
            Response<FrameworkCirculation.Entity.SubscriptionStatus> response = new Response<FrameworkCirculation.Entity.SubscriptionStatus>();
            try
            {
                string param = "subscriptionStatusID:" + subscriptionStatusID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatus worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatus();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionStatusID);
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
        public Response<int> Save(Guid accessKey, FrameworkCirculation.Entity.SubscriptionStatus x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkCirculation.Entity.SubscriptionStatus>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatus worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatus();
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
