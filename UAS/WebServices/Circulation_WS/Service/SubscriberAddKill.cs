using Circulation_WS.Interface;
using FrameworkUAS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circulation_WS.Service
{
    public class SubscriberAddKill : ServiceBase, ISubscriberAddKill
    {
        public Response<List<FrameworkCirculation.Entity.SubscriberAddKill>> Select(Guid accessKey)
        {
            Response<List<FrameworkCirculation.Entity.SubscriberAddKill>> response = new Response<List<FrameworkCirculation.Entity.SubscriberAddKill>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SubscriberAddKill", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriberAddKill worker = new FrameworkCirculation.BusinessLogic.SubscriberAddKill();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<int> Save(Guid accessKey, FrameworkCirculation.Entity.SubscriberAddKill subAddKill)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkCirculation.Entity.SubscriberAddKill>(subAddKill);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberAddKill", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriberAddKill worker = new FrameworkCirculation.BusinessLogic.SubscriberAddKill();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(subAddKill);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<int> UpdateSubscription(Guid accessKey, int addKillID, int productID, string subscriptionIDs, bool deleteAddRemoveID)
        {
            Response<int> response = new Response<int>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, String.Empty, "SubscriberAddKill", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.SubscriberAddKill worker = new FrameworkCirculation.BusinessLogic.SubscriberAddKill();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.UpdateSubscription(addKillID, productID, subscriptionIDs, deleteAddRemoveID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
