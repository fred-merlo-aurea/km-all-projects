using System;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class DBWorker : ServiceBase, IDBWorker
    {
        /// <summary>
        /// Gets the pub ID and pub code from the client's name and stores it in a dictionary
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientName">the client name</param>
        /// <returns>response.result will contain a Dictionary</returns>
        public Response<Dictionary<int, string>> GetPubIDAndCodesByClient(Guid accessKey, string clientName)
        {
            Response<Dictionary<int, string>> response = new Response<Dictionary<int, string>>();
            try
            {
                string param = "clientName:" + clientName;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DBWorker", "GetPubIDAndCodesByClient");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(clientName);
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
        /// Gets the pub ID and pub code from the Client object and stores it in a dictionary
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a Dictionary</returns>
        public Response<Dictionary<int, string>> GetPubIDAndCodesByClient(Guid accessKey, KMPlatform.Entity.Client client) 
        {
            Response<Dictionary<int, string>> response = new Response<Dictionary<int, string>>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Client>(client);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DBWorker", "GetPubIDAndCodesByClient");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);
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
        /// Saves Transformation object properties
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationId">the transformation ID</param>
        /// <param name="transformationTypeId">the transformation type ID</param>
        /// <param name="transformationName">the transformation name</param>
        /// <param name="transformationDescription">the transformation description</param>
        /// <param name="clientId">the client ID</param>
        /// <param name="userID">the user ID</param>
        /// <param name="isMapsPubCode">boolean to do a data map for the assigned pub code</param>
        /// <param name="isLastStep">boolean is last step the data map</param>
        /// <returns>response.result will containa an integer</returns>
        public Response<int> Save(Guid accessKey, int transformationId, int transformationTypeId, string transformationName, string transformationDescription, int clientId, int userID, bool isMapsPubCode = false, bool isLastStep = false)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = "transformationId:" + transformationId.ToString() + " transformationTypeId:" + transformationTypeId.ToString() + " transformationName:" + transformationName + " transformationDescription:" + transformationDescription + " clientId:" + clientId.ToString() + " userID:" + userID.ToString() + " isMapsPubCode:" + isMapsPubCode.ToString() + " isLastStep:" + isLastStep.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DBWorker", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAS.Object.DBWorker.Save(transformationId, transformationTypeId, transformationName, transformationDescription, clientId, userID, isMapsPubCode, isLastStep);
                    if (response.Result >= 0)
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
        /// Gets the Sql connection of the specified client name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientName">the client name</param>
        /// <returns>response.result will contain a SqlConnection</returns>
        public Response<SqlConnection> GetClientSqlConnection(Guid accessKey, string clientName)
        {
            Response<SqlConnection> response = new Response<SqlConnection>();
            try
            {
                string param = "clientName:" + clientName;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DBWorker", "GetClientSqlConnection");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAS.Object.DBWorker.GetClientSqlConnection(clientName);
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
        /// Gets the Sql connection of the specified Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a SqlConnection</returns>
        public Response<SqlConnection> GetClientSqlConnection(Guid accessKey, KMPlatform.Entity.Client client)
        {
            Response<SqlConnection> response = new Response<SqlConnection>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Client>(client);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DBWorker", "GetClientSqlConnection");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAS.Object.DBWorker.GetClientSqlConnection(client);
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
