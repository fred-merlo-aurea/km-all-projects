using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class TransactionCode : ServiceBase, ITransactionCode
    {
        /// <summary>
        /// Checks to see if the TransactionCode object exists by the transaction code type ID and the transaction code value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transactionCodeTypeID">the transaction code type ID</param>
        /// <param name="transactionCodeValue">the transaction code value</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Exists(Guid accessKey, int transactionCodeTypeID, int transactionCodeValue)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "transactionCodeTypeID:" + transactionCodeTypeID.ToString() + " transactionCodeValue:" + transactionCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCode worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(transactionCodeTypeID, transactionCodeValue);
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
        /// Selects a list of active TransactionCode objects based on if the transaction type is free or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="isFree">boolean to look for the free or not free transaction code types</param>
        /// <returns>response.result will contain a list of TransactionCode objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> SelectActiveIsFree(Guid accessKey, bool isFree)
        {
            Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> response = new Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
            try
            {
                string param = "isFree:" + isFree.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "SelectActiveIsFree");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCode worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectActiveIsFree(isFree);
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
        /// Selects a TransactionCode object based on the transaction code value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transactionCodeValue">the transaction code value</param>
        /// <returns>response.result will contain a TransactionCode object</returns>
        public Response<FrameworkUAD_Lookup.Entity.TransactionCode> SelectTransactionCodeValue(Guid accessKey, int transactionCodeValue)
        {
            Response<FrameworkUAD_Lookup.Entity.TransactionCode> response = new Response<FrameworkUAD_Lookup.Entity.TransactionCode>();
            try
            {
                string param = "transactionCodeValue:" + transactionCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "SelectTransactionCodeValue");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCode worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectTransactionCodeValue(transactionCodeValue);
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
        /// Selects a TransactionCode object based on the transaction code type ID and the transaction code value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transactionCodeTypeID">the transaction code type ID</param>
        /// <param name="transactionCodeValue">the transaction code value</param>
        /// <returns>response.result will contian a TransactionCode object</returns>
        public Response<FrameworkUAD_Lookup.Entity.TransactionCode> Select(Guid accessKey, int transactionCodeTypeID, int transactionCodeValue)
        {
            Response<FrameworkUAD_Lookup.Entity.TransactionCode> response = new Response<FrameworkUAD_Lookup.Entity.TransactionCode>();
            try
            {
                string param = "transactionCodeTypeID:" + transactionCodeTypeID.ToString() + " transactionCodeValue:" + transactionCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "SelectTransactionCodeValue");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCode worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transactionCodeTypeID, transactionCodeValue);
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
        /// Selects a list of TransactionCode objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>respons.result will contain a list of TransactionCode object</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> response = new Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCode worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
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
        /// Saves the given TransactionCode object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the TransactionCode object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.TransactionCode x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.TransactionCode>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransactionCode", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.TransactionCode worker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
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
