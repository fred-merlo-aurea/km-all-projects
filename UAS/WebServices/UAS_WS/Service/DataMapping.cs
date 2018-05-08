using System;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class DataMapping : ServiceBase, IDataMapping
    {
        /// <summary>
        /// Saves a DataMapping object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the DataMapping object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.DataMapping x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.DataMapping>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DataMapping", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.DataMapping worker = new FrameworkUAS.BusinessLogic.DataMapping();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result >= 0)
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
