using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed;
using EntitySubscriberDemographicTransformed = FrameworkUAD.Entity.SubscriberDemographicTransformed;

namespace UAD_WS.Service
{
    public class SubscriberDemographicTransformed : FrameworkServiceBase, ISubscriberDemographicTransformed
    {
        private const string EntityName = "SubscriberDemographicTransformed";
        private const string MethodSelectPublication = "SelectPublication";
        private const string MethodSelectSubscriberTransformed = "SelectSubscriberTransformed";

        /// <summary>
        /// Selects a list of SubscriberDemographicTransformed objects based on the pub ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubId">the pub ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicTransformed objects</returns>
        public Response<List<EntitySubscriberDemographicTransformed>> SelectPublication(Guid accessKey, int pubId, ClientConnections client)
        {
            var param = $" pubID:{pubId}";
            var model = new ServiceRequestModel<List<EntitySubscriberDemographicTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectPublication,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublication(pubId, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberDemographicTransformed objects based on the SO recodr Identifier and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SORecordIdentifier">the SO record Identifier</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicTransformed objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>> SelectSubscriberOriginal(Guid accessKey, Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>>();
            try
            {
                string param = " SORecordIdentifier:" + SORecordIdentifier.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicTransformed", "SelectSubscriberOriginal");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectSubscriberOriginal(SORecordIdentifier, client).ToList();
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
        /// Selects a list of SubscriberDemographicTransformed objects based on the SO recodr Identifier and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="stRecordIdentifier">the ST record Identifier</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicTransformed objects</returns>
        public Response<List<EntitySubscriberDemographicTransformed>> SelectSubscriberTransformed(Guid accessKey, Guid stRecordIdentifier, ClientConnections client)
        {
            var param = $" STRecordIdentifier:{stRecordIdentifier}";
            var model = new ServiceRequestModel<List<EntitySubscriberDemographicTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSubscriberTransformed,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSubscriberTransformed(stRecordIdentifier, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberDemographicTransformed objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicTransformed objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
	        Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>>();
	        try
	        {
                string sd = "NULL";
                string ed = "NULL";
                if (startDate != null && startDate.Value != null)
                    sd = startDate.Value.ToString();

                if (endDate != null && endDate.Value != null)
                    ed = endDate.Value.ToString();

                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString() + " startDate:" + sd + " endDate:" + ed;		        
		        FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicTransformed", "SelectForFileAudit");
		        response.Status = auth.Status;

		        if (auth.IsAuthenticated == true)
		        {
			        FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed();
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
        /// Saves the SubscriberDemographicTransformed object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the SubscriberDemographicTransformed object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberDemographicTransformed x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.SubscriberDemographicTransformed>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicTransformed", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed();
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
