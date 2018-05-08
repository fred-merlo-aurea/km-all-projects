using System;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerVariable : ServiceBase, IServerVariable
    {
        /// <summary>
        /// Gets the Server variables
        /// </summary>
        /// <returns>response.result will contain a ServerVariable object</returns>
        public Response<KMPlatform.Object.ServerVariable> GetServerVariables()
        {
            Response<KMPlatform.Object.ServerVariable> response = new Response<KMPlatform.Object.ServerVariable>();
            try
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.No_Access_Key_Required;

                KMPlatform.BusinessLogic.ServerVariable worker = new KMPlatform.BusinessLogic.ServerVariable();
                response.Message = "No AccessKey Required";
                response.Result = worker.GetServerVariables();
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
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                Guid accessKey = Guid.NewGuid();
                LogError(accessKey, ex, "ServerVariable", "GetServerVariables");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
