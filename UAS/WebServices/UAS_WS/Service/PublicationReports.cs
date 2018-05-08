using UAS_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicationReports : ServiceBase, IPublicationReports
    {
        /// <summary>
        /// Selects a list of PublicationReports objects based on the publication ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <returns>response.result will contain a list of PublicationReports objects</returns>
        public Response<List<FrameworkUAS.Entity.PublicationReports>> SelectPublication(Guid accessKey, int publicationID)
        {
            Response<List<FrameworkUAS.Entity.PublicationReports>> response = new Response<List<FrameworkUAS.Entity.PublicationReports>>();
            try
            {
                string param = "publicationID:" + publicationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Publication", "SelectPublication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.PublicationReports worker = new FrameworkUAS.BusinessLogic.PublicationReports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectPublication(publicationID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
