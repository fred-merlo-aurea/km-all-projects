using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class ReportGroups : ServiceBase, IReportGroups
    {
        /// <summary>
        /// Selects a list of ReportGroups based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ReportGroups objects</returns>
        public Response<List<FrameworkUAD.Entity.ReportGroups>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.ReportGroups>> response = new Response<List<FrameworkUAD.Entity.ReportGroups>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ReportGroups", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ReportGroups worker = new FrameworkUAD.BusinessLogic.ReportGroups();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the ReportGroup object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="x">the ReportGroup object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.ReportGroups x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.ReportGroups>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ReportGroups", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ReportGroups worker = new FrameworkUAD.BusinessLogic.ReportGroups();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(client, x);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
