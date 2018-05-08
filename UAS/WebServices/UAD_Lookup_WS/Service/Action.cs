using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAD_Lookup_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD_Lookup.BusinessLogic.Action;
using EntityAction = FrameworkUAD_Lookup.Entity.Action;

namespace UAD_Lookup_WS.Service
{
    public class Action : FrameworkServiceBase, IAction
    {
        private const string EntityName = "Action";
        private const string MethodSave = "Save";
        private const string MethodValidate = "Validate";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Checks to see if an action exists based on the action type ID, category code ID, and the transaction code ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="actionTypeID">the action type ID</param>
        /// <param name="categoryCodeID">the category code ID</param>
        /// <param name="transactionCodeID">the transaction code ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Exists(Guid accessKey, int actionTypeID, int categoryCodeID, int transactionCodeID)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "ActionTypeID:" + actionTypeID.ToString() + " CategoryCodeID:" + categoryCodeID.ToString() + " TransactionCodeID:" + transactionCodeID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Action", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Action worker = new FrameworkUAD_Lookup.BusinessLogic.Action();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(actionTypeID, categoryCodeID, transactionCodeID);
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
        /// Gets an action based on the action ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="actionID">the action ID</param>
        /// <param name="getChildren">boolean to get the child actions</param>
        /// <returns>response.result will containa a list of Action objects</returns>
        public Response<FrameworkUAD_Lookup.Entity.Action> GetByActionID(Guid accessKey, int actionID, bool getChildren = false)
        {
            Response<FrameworkUAD_Lookup.Entity.Action> response = new Response<FrameworkUAD_Lookup.Entity.Action>();
            try
            {
                string param = "ActionID:" + actionID.ToString() + " GetChildren:" + getChildren.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Action", "GetByActionID");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Action worker = new FrameworkUAD_Lookup.BusinessLogic.Action();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetByActionID(actionID, getChildren);
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
        /// Selects an Action object based on the category code ID and transaction code ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeID">the category code ID</param>
        /// <param name="transactionCodeID">the transaction code ID</param>
        /// <returns>response.result will contain an Action object</returns>
        public Response<FrameworkUAD_Lookup.Entity.Action> Select(Guid accessKey, int categoryCodeID, int transactionCodeID)
        {
            Response<FrameworkUAD_Lookup.Entity.Action> response = new Response<FrameworkUAD_Lookup.Entity.Action>();
            try
            {
                string param = "CategoryCodeID:" + categoryCodeID.ToString() + " TransactionCodeID:" + transactionCodeID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Action", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Action worker = new FrameworkUAD_Lookup.BusinessLogic.Action();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(categoryCodeID, transactionCodeID);
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
        /// Selects an Action object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of Action objects</returns>
        public Response<List<EntityAction>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityAction>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Validates the given Action object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="action">the Action object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Validate(Guid accessKey, EntityAction action)
        {
            var param = $"ActionID:{action.ActionID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodValidate,
                WorkerFunc = _ => new BusinessLogicWorker().Validate(action)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the given Action object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="action">the Action object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.Action action)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(action),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(action);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
