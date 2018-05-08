using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class PubCodes : ServiceBase, IPubCodes
    {
        /// <summary>
        /// Selects a list of PubCodes based on the database name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="dbName">the database name</param>
        /// <returns>response.result will contain a list of PubCodes</returns>
        public Response<List<FrameworkUAD.Object.PubCode>> Select(Guid accessKey, string dbName)
        {
            Response<List<FrameworkUAD.Object.PubCode>> response = new Response<List<FrameworkUAD.Object.PubCode>>();
            try
            {
                string param = "dbName:" + dbName;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PubCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.PubCode worker = new FrameworkUAD.BusinessLogic.PubCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(dbName).ToList();
                    if (response.Result != null)
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

        /// <summary>
        /// Selects all of the PubCodes based on the client and database name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="dbName">the database name</param>
        /// <returns>response.result will contain a list of PubCodes</returns>
        public Response<List<FrameworkUAD.Object.PubCode>> SelectAllPubs(Guid accessKey, KMPlatform.Object.ClientConnections client, string dbName)
        {
            Response<List<FrameworkUAD.Object.PubCode>> response = new Response<List<FrameworkUAD.Object.PubCode>>();
            try
            {
                string param = "dbName:" + dbName;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PubCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.PubCode worker = new FrameworkUAD.BusinessLogic.PubCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client).ToList();
                    if (response.Result != null)
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
