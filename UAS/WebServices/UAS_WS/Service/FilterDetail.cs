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
    public class FilterDetail :ServiceBase, IFilterDetail
    {
        /// <summary>
        /// Selects a list of FilterDetail objects based on the filter ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="filterID">the filter ID</param>
        /// <returns>response.result will contain a list of FilterDetail objects</returns>
        public Response<List<FrameworkUAS.Entity.FilterDetail>> Select(Guid accessKey, int filterID)
        {
            Response<List<FrameworkUAS.Entity.FilterDetail>> response = new Response<List<FrameworkUAS.Entity.FilterDetail>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "FileStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FilterDetail worker = new FrameworkUAS.BusinessLogic.FilterDetail();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(filterID);
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
        /// Saves a FilterDetail object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the FilterDetail object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.FilterDetail x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.FilterDetail>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FilterDetail", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FilterDetail worker = new FrameworkUAS.BusinessLogic.FilterDetail();
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
