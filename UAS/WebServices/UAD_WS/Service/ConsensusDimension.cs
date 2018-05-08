using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class ConsensusDimension : ServiceBase, IConsensusDimension
    {
        /// <summary>
        /// Saves a list of ConsensusDimension objects from the XML based on the master group ID and the client 
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of ConsensusDimension objects</param>
        /// <param name="masterGroupID">the master group ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveXML(Guid accessKey, List<FrameworkUAD.Object.ConsensusDimension> list, int masterGroupID, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = " masterGroupID:" + masterGroupID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ConsensusDimension", "SaveXML");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ConsensusDimension worker = new FrameworkUAD.BusinessLogic.ConsensusDimension();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveXML(list, masterGroupID, client);
                    if (response.Result == true || response.Result == false)
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
