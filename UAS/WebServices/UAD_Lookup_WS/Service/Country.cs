using System;
using System.Collections.Generic;
using System.Linq;
using UAD_Lookup_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class Country : ServiceBase, ICountry
    {
        /// <summary>
        /// Selects a list of Country objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of Country objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.Country>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.Country>> response = new Response<List<FrameworkUAD_Lookup.Entity.Country>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Country", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Country worker = new FrameworkUAD_Lookup.BusinessLogic.Country();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
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
        /// 
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CountryRegionCleanse(Guid accessKey, int sourceFileID, string processCode)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, " SourceFileID:" + sourceFileID.ToString() + " ProcessCode:" + processCode, "Country", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                    FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(sourceFileID);
                    KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
                    KMPlatform.Entity.Client c = cWorker.Select(sf.ClientID);

                    FrameworkUAD_Lookup.BusinessLogic.Country worker = new FrameworkUAD_Lookup.BusinessLogic.Country();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.CountryRegionCleanse(sourceFileID, processCode, c.ClientConnections);
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
