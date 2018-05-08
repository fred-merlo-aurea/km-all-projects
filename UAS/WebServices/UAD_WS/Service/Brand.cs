using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Brand;
using EntityBrand = FrameworkUAD.Entity.Brand;

namespace UAD_WS.Service
{
    public class Brand : FrameworkServiceBase, IBrand
    {
        private const string EntityName = "Brand";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of Brand objects based on the given client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Brand objects</returns>
        public Response<List<EntityBrand>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityBrand>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }
        
        /// <summary>
        /// Saves the Brand object based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="x">the Brand object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.Brand x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.Brand>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Brand", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Brand worker = new FrameworkUAD.BusinessLogic.Brand();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
                    if (response.Result > 0)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
