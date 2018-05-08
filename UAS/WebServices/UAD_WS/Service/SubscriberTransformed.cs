using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core_AMS.Utilities;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using KMPlatform.Object;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberTransformed;
using EntitySubscriberTransformed = FrameworkUAD.Entity.SubscriberTransformed;

namespace UAD_WS.Service
{
    public class SubscriberTransformed : FrameworkServiceBase, ISubscriberTransformed
    {
        private const string EntityName = "SubscriberTransformed";
        private const string MethodEnableIndexes = "EnableIndexes";
        private const string MethodAddressUpdateBulkSql = "AddressUpdateBulkSql";
        private const string MethodDisableIndexes = "DisableIndexes";
        private const string MethodSelect = "Select";
        private const string NullString = "NULL";
        private const string MethodSelectForFileAudit = "SelectForFileAudit";
        private const string MethodSelectByAddressValidation = "SelectByAddressValidation";
        private const string MethodSelectForGeoCoding = "SelectForGeoCoding";
        private const string MethodAddressValidationPaging = "AddressValidation_Paging";
        private const string MethodCountForGeoCoding = "CountForGeoCoding";
        private const string MethodSaveBulkInsert = "SaveBulkInsert";
        private const string MethodAddressValidateExisting = "AddressValidateExisting";
        private const string MethodDataMatching = "DataMatching";
        private const string MethodCountAddressValidation = "CountAddressValidation";
        private const string MethodSave = "Save";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodStandardRollUpToMaster = "StandardRollUpToMaster";

        #region selects
        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $" processCode:{processCode}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(processCode, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects the top number one SubscriberTransformed object in the SubscriberTransformed database table based on the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a SubscriberTransformed object</returns>
        public Response<EntitySubscriberTransformed> SelectTopOne(Guid accessKey, string processCode, ClientConnections client)
        {
            var param = $" processCode:{processCode}";
            var model = new ServiceRequestModel<EntitySubscriberTransformed>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectTopOne(processCode, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the client and the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> Select(Guid accessKey, ClientConnections client, int sourceFileID)
        {
            var param = $" sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client, sourceFileID)
            };

            return GetResponse(model);
        }
        
        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            var start = startDate?.ToString(CultureInfo.InvariantCulture) ?? NullString;
            var end = endDate?.ToString(CultureInfo.InvariantCulture) ?? NullString;
            var param = $" processCode:{processCode} sourceFileID:{sourceFileID} startDate:{start} endDate:{end}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForFileAudit,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client)
            };

            return GetResponse(model);
        }
        #endregion

        #region Address validation and geocoding selects
        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the the client, the source file ID and whether the Address is a valid latitude/longitude
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectByAddressValidation(Guid accessKey, ClientConnections client, int sourceFileID, bool isLatLonValid)
        {
            var param = $" sourceFileID:{sourceFileID} isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAddressValidation(client, sourceFileID, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the the client, the source file ID, the process code and whether the Address is a valid latitude/longitude
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectByAddressValidation(
            Guid accessKey,
            ClientConnections client,
            string processCode,
            int sourceFileID,
            bool isLatLonValid)
        {
            var param = $" processCode:{processCode} sourceFileID:{sourceFileID} isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAddressValidation(client, processCode, sourceFileID, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the the client, the process code and whether the Address is a valid latitude/longitude
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="processCode">the process code</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid)
        {
            var param = $" processCode:{processCode} isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAddressValidation(client, processCode, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects based on the the client and whether the Address is a valid latitude/longitude
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            var param = $" isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAddressValidation(client, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects where the latitude/longitude is invalid based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForGeoCoding,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForGeoCoding(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberTransformed objects where the latitude/longitude is invalid based on the client and the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> SelectForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            var param = $" sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForGeoCoding,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForGeoCoding(client, sourceFileID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects the given amount of SubscriberTransformed objects from the current page and page size based on the process code, the client, the source file ID
        /// and whether the latitude/longitude is valid
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="currentPage">the current page</param>
        /// <param name="pageSize">the page size</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a list of SubscriberTransformed objects</returns>
        public Response<List<EntitySubscriberTransformed>> AddressValidation_Paging(Guid accessKey, int currentPage, int pageSize, string processCode, KMPlatform.Object.ClientConnections client, bool isLatLonValid = false, int sourceFileID = 0)
        {
            var param = $" currentPage:{currentPage} pageSize:{pageSize} processCode:{processCode} isLatLonValid:{isLatLonValid} sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntitySubscriberTransformed>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodAddressValidationPaging,
                WorkerFunc = _ => new BusinessLogicWorker().AddressValidation_Paging(currentPage, pageSize, processCode, client, isLatLonValid, sourceFileID)
            };

            return GetResponse(model);
        }
        #endregion

        #region Geocode counts
        /// <summary>
        /// Gets the count of SubscriberTransformed objects in the SubscriberTransformed database table based on the client, the sourcefile ID and whether the address latitude/longitude is valid
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="isLatLonValid">boolean whether the address latitude/longitude is valid</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> CountAddressValidation(Guid accessKey, ClientConnections client, int sourceFileID, bool isLatLonValid)
        {
            var param = $" sourceFileID:{sourceFileID} isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCountAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().CountAddressValidation(client, sourceFileID, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets a count of the SubscriberTransformed objects in the SubscriberTransformed databse table based on the client, the process code and whether the address latitude/longitude is valid
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="processCode">the process code</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> CountAddressValidation(Guid accessKey, ClientConnections client, string processCode, bool isLatLonValid)
        {
            var param = $" processCode:{processCode} isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCountAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().CountAddressValidation(client, processCode, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets a count of the SubscriberTransformed objects in the SubscriberTransformed database table based on the client and whether the address latitude/longitude is valid
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="isLatLonValid">boolean if the address latitude/longitude is valid</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> CountAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            var param = $" isLatLonValid:{isLatLonValid}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCountAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().CountAddressValidation(client, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets a count of the SubscriberTransformed objects in the SubscriberTransformed database table based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> CountForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCountForGeoCoding,
                WorkerFunc = _ => new BusinessLogicWorker().CountForGeoCoding(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets a count of the SubscriberTransformed objects in the SubscriberTransformed databse table based on the client and the sourcefile ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> CountForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            var param = $" sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCountForGeoCoding,
                WorkerFunc = _ => new BusinessLogicWorker().CountForGeoCoding(client, sourceFileID)
            };

            return GetResponse(model);
        }
        #endregion

        #region Save  / Update methods
        /// <summary>
        /// Saves the SubscriberTransformed object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntitySubscriberTransformed"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySubscriberTransformed entity, KMPlatform.Object.ClientConnections client)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves and Inserts the list of 250 SubscriberTransformed objects at a time to the SubscriberTransformed database table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberTransformed list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkInsert(list, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves and Insert the list of up to 2500 SubscriberTransformed objects at a time by hardcoding it into the databse table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberTransformed list to be saved</param>
        /// <param name="client">the client object</param>
        /// <param name="isDataCompare"></param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client, bool isDataCompare)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list, client, isDataCompare)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Hardcodes the address information from each SubscriberTransformed object into the database and then saves
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of SubscriberTransformed objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> AddressUpdateBulkSql(Guid accessKey, List<FrameworkUAD.Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodAddressUpdateBulkSql,
                WorkerFunc = _ => new BusinessLogicWorker().AddressUpdateBulkSql(list, client)
            };

            return GetResponse(model);
        }

        #endregion

        #region Jobs / Operations
        /// <summary>
        /// Updates the SubscriberFinal database for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> StandardRollUpToMaster(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodStandardRollUpToMaster,
                WorkerFunc = _ => new BusinessLogicWorker().StandardRollUpToMaster(client, sourceFileID, processCode)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Validates for existing addresses between SubscriberTransformed and SubscriberFinal
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> AddressValidateExisting(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodAddressValidateExisting,
                WorkerFunc = _ => new BusinessLogicWorker().AddressValidateExisting(client, sourceFileID, processCode)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        public Response<bool> DataMatching(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDataMatching,
                WorkerFunc = _ => new BusinessLogicWorker().DataMatching(client, sourceFileID, processCode)
            };

            return GetResponse(model);
        }

        public Response<bool> DisableIndexes(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDisableIndexes,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().DisableIndexes(client) */
            };

            return GetResponse(model);
        }

        public Response<bool> EnableIndexes(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodEnableIndexes,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().EnableIndexes(client) */
            };

            return GetResponse(model);
        }
        #endregion
    }
}
