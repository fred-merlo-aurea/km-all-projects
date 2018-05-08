using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriberDemographicArchive : ServiceBase, ISubscriberDemographicArchive
    {
        /// <summary>
        /// Selects a list of SubscriberDemographicArchive objects based on the pub ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubID">the pub ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicArchive objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> SelectPublication(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>>();
            try
            {
                string param = " pubID:" + pubID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicArchive", "SelectPublication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicArchive worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicArchive();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectPublication(pubID, client).ToList();
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
        /// Selects a list of SubscriberDemographicArchive objects based on the SA record Identifier
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SARecordIdentifier">the SA record Identifier</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicArchive objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> SelectSubscriberOriginal(Guid accessKey, Guid SARecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>>();
            try
            {
                string param = " SARecordIdentifier:" + SARecordIdentifier.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicArchive", "SelectSubscriberOriginal");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicArchive worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicArchive();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectSubscriberOriginal(SARecordIdentifier, client).ToList();
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
        /// Selects a list of SubscriberDemographicArchive objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicArchive objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
	        Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>>();
	        try
	        {
                string sd = "NULL";
                string ed = "NULL";
                if (startDate != null && startDate.Value != null)
                    sd = startDate.Value.ToString();

                if (endDate != null && endDate.Value != null)
                    ed = endDate.Value.ToString();

                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString() + " startDate:" + sd + " endDate:" + ed;		        
		        FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicArchive", "SelectForFileAudit");
		        response.Status = auth.Status;

		        if (auth.IsAuthenticated == true)
		        {
			        FrameworkUAD.BusinessLogic.SubscriberDemographicArchive worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicArchive();
			        response.Message = "AccessKey Validated";
			        response.Result = worker.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client).ToList();
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
        /// Saves the SubscriberDemographicArchive object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the SubscriberDemographicArchive object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberDemographicArchive x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.SubscriberDemographicArchive>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicArchive", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicArchive worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicArchive();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
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
