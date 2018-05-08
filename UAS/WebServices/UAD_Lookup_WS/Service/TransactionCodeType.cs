using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class TransactionCodeType : ServiceBase, ITransactionCodeType
    {
        /// <summary>
        /// Checks to see if the TransactionCodeType object exists by its name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transactionCodeTypeName">the transaction code type name</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Exists(Guid accessKey, string transactionCodeTypeName)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "transactionCodeTypeName:" + transactionCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(transactionCodeTypeName);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a TransactionCodeType object based on the transaction code type enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transactionCodeTypeName">the transaction code type name enum (Free_Active, Free_Inactive, Paid_Active, Paid_Inactive)</param>
        /// <returns>response.result will contain a TransactionCodeType object</returns>
        public Response<FrameworkUAD_Lookup.Entity.TransactionCodeType> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.TransactionCodeType transactionCodeTypeName)
        {
            Response<FrameworkUAD_Lookup.Entity.TransactionCodeType> response = new Response<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
            try
            {
                string param = "transactionCodeTypeName:" + transactionCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transactionCodeTypeName);
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
        /// Selects a TransactionCodeType object based on the transaction code type name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transactionCodeTypeName">the transaction code type name</param>
        /// <returns>response.result will contain a TransactionCodeType object</returns>
        public Response<FrameworkUAD_Lookup.Entity.TransactionCodeType> Select(Guid accessKey, string transactionCodeTypeName)
        {
            Response<FrameworkUAD_Lookup.Entity.TransactionCodeType> response = new Response<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
            try
            {
                string param = "transactionCodeTypeName:" + transactionCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transactionCodeTypeName);
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
        /// Selects a list of TransactionCodeType objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransactionCodeType objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> response = new Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
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
        /// Selects a list of TransactionCodeType objects based on if it is free or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="isFree">boolean to look for the free or not free code types</param>
        /// <returns>response.result will contain a list of TransactionCodeType objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> Select(Guid accessKey, bool isFree)
        {
            Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> response = new Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
            try
            {
                string param = "isFree:" + isFree.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(isFree);
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
        /// Saves the given TransactionCodeType object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the TransactionCodeType object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.TransactionCodeType x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.TransactionCodeType>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCodeType", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
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
