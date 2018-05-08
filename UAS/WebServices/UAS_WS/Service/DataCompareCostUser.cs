using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class DataCompareCostUser : ServiceBase, IDataCompareCostUser
    {
        /// <summary>
        /// Selects a list of DataCompareCostToUser objects based on the user ID
        /// </summary>
        /// <param name="accessKey">teh access key</param>
        /// <param name="userId">the user ID</param>
        /// <returns>response.result will contain a list of DataCompareCostToUser objects</returns>
        public Response<List<FrameworkUAS.Entity.DataCompareCostUser>> Select(Guid accessKey, int userId)
        {
            Response<List<FrameworkUAS.Entity.DataCompareCostUser>> response = new Response<List<FrameworkUAS.Entity.DataCompareCostUser>>();
            try
            {
                string param = "UserID:" + userId.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, this.GetType().Name.ToString(), "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.DataCompareCostUser worker = new FrameworkUAS.BusinessLogic.DataCompareCostUser();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(userId);
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
