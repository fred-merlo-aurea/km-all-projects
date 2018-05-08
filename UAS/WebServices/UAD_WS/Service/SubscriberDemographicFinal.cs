using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriberDemographicFinal : ServiceBase, ISubscriberDemographicFinal
    {
        /// <summary>
        /// Selects a list of SubscriberDemographicFinal objects based on the pub ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubID">the pub ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicFinal objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> SelectPublication(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>>();
            try
            {
                string param = " pubID:" + pubID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicFinal", "SelectPublication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicFinal worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicFinal();
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
        /// Selects a list of SubscriberDemographicFinal objects based on the SF record Identifier and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SFRecordIdentifier">the SF record Identifier</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicFinal objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> SelectSubscriberOriginal(Guid accessKey, Guid SFRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>>();
            try
            {
                string param = " SFRecordIdentifier:" + SFRecordIdentifier.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicFinal", "SelectSubscriberOriginal");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicFinal worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicFinal();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectSubscriberOriginal(SFRecordIdentifier, client).ToList();
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
        /// Selects a list of SubscriberDemographicFinal objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberDemographicFinal objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
	        Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> response = new Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>>();
	        try
	        {
                string sd = "NULL";
                string ed = "NULL";
                if (startDate != null && startDate.Value != null)
                    sd = startDate.Value.ToString();

                if (endDate != null && endDate.Value != null)
                    ed = endDate.Value.ToString();

                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString() + " startDate:" + sd + " endDate:" + ed;		        
		        FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicFinal", "SelectForFileAudit");
		        response.Status = auth.Status;

		        if (auth.IsAuthenticated == true)
		        {
			        FrameworkUAD.BusinessLogic.SubscriberDemographicFinal worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicFinal();
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
        /// Saves the SubscriberDemographicFinal object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the SubscriberDemographicFinal object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberDemographicFinal x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.SubscriberDemographicFinal>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberDemographicFinal", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberDemographicFinal worker = new FrameworkUAD.BusinessLogic.SubscriberDemographicFinal();
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
