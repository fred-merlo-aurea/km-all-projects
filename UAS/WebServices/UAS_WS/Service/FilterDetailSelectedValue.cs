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
    public class FilterDetailSelectedValue : ServiceBase, IFilterDetailSelectedValue
    {
        /// <summary>
        /// Selects a list of FilterDetailSelectedValue objects based on the filter detail ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="filterDetailID">the filter detail ID</param>
        /// <returns>response.result will contain a list of FilterDetailSelectedValue objects</returns>
        public Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>> Select(Guid accessKey, int filterDetailID)
        {
            Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>> response = new Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "FileStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FilterDetailSelectedValue worker = new FrameworkUAS.BusinessLogic.FilterDetailSelectedValue();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(filterDetailID);
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
        /// Save the FilterDetailSelectedValue object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the FilterDetailSelectedValue</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.FilterDetailSelectedValue x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.FilterDetailSelectedValue>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FilterDetailSelectedValue", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FilterDetailSelectedValue worker = new FrameworkUAS.BusinessLogic.FilterDetailSelectedValue();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
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
