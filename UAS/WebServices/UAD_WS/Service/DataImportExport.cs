using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAD_WS.Interface;

namespace UAD_WS.Service
{
    public class DataImportExport : ServiceBase, IDataImportExport
    {
        /// <summary>
        /// Selects a list of DataImportExport objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of DataImportExport objects</returns>
        public Response<List<FrameworkUAD.Entity.DataImportExport>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.DataImportExport>> response = new Response<List<FrameworkUAD.Entity.DataImportExport>>();
            try
            {
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "DataImportExport", "Select");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.DataImportExport worker = new FrameworkUAD.BusinessLogic.DataImportExport();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
