﻿using Circulation_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace Circulation_WS.Service
{
    public class SubscriptionStatusMatrix : ServiceBase, ISubscriptionStatusMatrix
    {
        public Response<List<FrameworkCirculation.Entity.SubscriptionStatusMatrix>> Select(Guid accessKey)
        {
            Response<List<FrameworkCirculation.Entity.SubscriptionStatusMatrix>> response = new Response<List<FrameworkCirculation.Entity.SubscriptionStatusMatrix>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatusMatrix", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatusMatrix worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatusMatrix();
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
        public Response<FrameworkCirculation.Entity.SubscriptionStatusMatrix> Select(Guid accessKey, int subscriptionStatusID, int categoryCodeID, int transactionCodeID)
        {
            Response<FrameworkCirculation.Entity.SubscriptionStatusMatrix> response = new Response<FrameworkCirculation.Entity.SubscriptionStatusMatrix>();
            try
            {
                string param = "subscriptionStatusID:" + subscriptionStatusID.ToString() + " categoryCodeID:" + categoryCodeID.ToString() + " transactionCodeID:" + transactionCodeID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatusMatrix", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatusMatrix worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatusMatrix();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionStatusID, categoryCodeID, transactionCodeID);
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
        public Response<int> Save(Guid accessKey, FrameworkCirculation.Entity.SubscriptionStatusMatrix x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkCirculation.Entity.SubscriptionStatusMatrix>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatusMatrix", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriptionStatusMatrix worker = new FrameworkCirculation.BusinessLogic.SubscriptionStatusMatrix();
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