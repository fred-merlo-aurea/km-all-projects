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
    public class PublicationSequence : ServiceBase, IPublicationSequence
    {
        /// <summary>
        /// Selects a list of PublicationSequence objects based on the publisher ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publisherID">the publisher ID</param>
        /// <returns>response.result will contain a list of PublicationSequence objects</returns>
        public Response<List<FrameworkUAS.Entity.PublicationSequence>> SelectForPublisher(Guid accessKey, int publisherID)
        {
            Response<List<FrameworkUAS.Entity.PublicationSequence>> response = new Response<List<FrameworkUAS.Entity.PublicationSequence>>();
            try
            {
                string param = "publisherID:" + publisherID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PublicationSequence", "SelectForPublisher");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.PublicationSequence worker = new FrameworkUAS.BusinessLogic.PublicationSequence();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectPublisher(publisherID);
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

        /// <summary>
        /// Selects a list of PublicationSequence objects based on the publication ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <returns>response.result will contain a list of PublicationSequence objects</returns>
        public Response<List<FrameworkUAS.Entity.PublicationSequence>> SelectForPublication(Guid accessKey, int publicationID)
        {
            Response<List<FrameworkUAS.Entity.PublicationSequence>> response = new Response<List<FrameworkUAS.Entity.PublicationSequence>>();
            try
            {
                string param = "publicationID:" + publicationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PublicationSequence", "SelectForPublication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.PublicationSequence worker = new FrameworkUAS.BusinessLogic.PublicationSequence();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectPublicationID(publicationID);
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

        /// <summary>
        /// Gets the next publication sequence ID from the publication ID and user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> GetNextSequenceID(Guid accessKey, int publicationID, int userID)
        {
            Response<int> response = new Response<int>();
            try
            {
                string param = "publicationID:" + publicationID.ToString() + " userID:" + userID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PublicationSequence", "GetNextSequenceID");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.PublicationSequence worker = new FrameworkUAS.BusinessLogic.PublicationSequence();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetNextSequenceID(publicationID, userID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
