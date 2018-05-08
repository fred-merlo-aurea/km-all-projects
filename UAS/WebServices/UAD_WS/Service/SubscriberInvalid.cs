using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriberInvalid : ServiceBase, ISubscriberInvalid
    {
        /// <summary>
        /// Select a list of SubscriberInvalid objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberInvalid objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberInvalid>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberInvalid>> response = new Response<List<FrameworkUAD.Entity.SubscriberInvalid>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SubscriberInvalid", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberInvalid worker = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client).ToList();
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
        /// Selects a list of SubscriberInvalid objects based ont the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberInvalid objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberInvalid>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriberInvalid>> response = new Response<List<FrameworkUAD.Entity.SubscriberInvalid>>();
            try
            {
                string param = " processCode:" + processCode;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberInvalid", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberInvalid worker = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(processCode, client).ToList();
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
        /// Selects a list of SubscriberInvalid objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberInvalid objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberInvalid>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
	        Response<List<FrameworkUAD.Entity.SubscriberInvalid>> response = new Response<List<FrameworkUAD.Entity.SubscriberInvalid>>();
	        try
	        {
                string sd = "NULL";
                string ed = "NULL";
                if (startDate != null && startDate.Value != null)
                    sd = startDate.Value.ToString();

                if (endDate != null && endDate.Value != null)
                    ed = endDate.Value.ToString();

                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString() + " startDate:" + sd + " endDate:" + ed;		        
		        FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberInvalid", "SelectForFileAudit");
		        response.Status = auth.Status;

		        if (auth.IsAuthenticated == true)
		        {
			        FrameworkUAD.BusinessLogic.SubscriberInvalid worker = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
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
        /// Saves the list of SubscriberInvalid objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberInvalid list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberInvalid> list, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SubscriberInvalid", "SaveBulkSqlInsert");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberInvalid worker = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkSqlInsert(list, client);
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
