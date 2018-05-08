using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.ClientCustomProcedure;
using EntityClientCustomProcedure = FrameworkUAS.Entity.ClientCustomProcedure;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCustomProcedure : FrameworkServiceBase, IClientCustomProcedure
    {
        private const string EntityName = "ClientCustomProcedure";
        private const string MethodSaveReturnId = "SaveReturnID";

        /// <summary>
        /// Selects a list of ClientCustomProcedure objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ClientCustomProcedure objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> response = new Response<List<FrameworkUAS.Entity.ClientCustomProcedure>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ClientCustomProcedure", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientCustomProcedure worker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select().ToList();
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
        /// Selects a list of ClientCustomProcedure objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will contain a list of ClientCustomProcedure objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> Select(Guid accessKey, int clientID)
        {
            Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> response = new Response<List<FrameworkUAS.Entity.ClientCustomProcedure>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "ClientCustomProcedure", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientCustomProcedure worker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectClient(clientID);
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
        /// Saves a ClientCustomProcedure object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientCustomProcedure object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.ClientCustomProcedure x)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.ClientCustomProcedure>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientCustomProcedure", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientCustomProcedure worker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
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

        /// <summary>
        /// Saves the ClientCustomProcedure object's return ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityClientCustomProcedure"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> SaveReturnID(Guid accessKey, EntityClientCustomProcedure entity)
        {
            var param = new UtilityJsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveReturnId,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().SaveReturnID(entity);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Executes Clients custom procedures
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sproc">the store procedure</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will containa a boolean</returns>
        public Response<bool> ExecuteSproc(Guid accessKey, string sproc, int sourceFileID, KMPlatform.Entity.Client client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "Sproc:" + sproc + " SourceFileID:" + sourceFileID.ToString() + " ClientID:" + client.ClientID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientCustomProcedure", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientCustomProcedure worker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                    response.Message = "AccessKey Validated";
                    FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                    FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(sourceFileID);
                    response.Result = worker.ExecuteSproc(sproc, sourceFileID, sf.FileName, client);
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
