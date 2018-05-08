using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ProductSubscription;
using EntityActionProductSubscription = FrameworkUAD.Entity.ActionProductSubscription;
using EntityCopiesProductSubscription = FrameworkUAD.Entity.CopiesProductSubscription;
using EntityProductSubscription = FrameworkUAD.Entity.ProductSubscription;
using EntityPubSubscriptionAdHoc = FrameworkUAD.Object.PubSubscriptionAdHoc;

namespace UAD_WS.Service
{
    public class ProductSubscription : FrameworkServiceBase, IProductSubscription
    {
        private const string EntityName = "ProductSubscription";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";
        private const string MethodSaveBulkActionIdUpdate = "SaveBulkActionIDUpdate";
        private const string MethodUpdateRequesterFlag = "UpdateRequesterFlag";
        private const string MethodSelectCount = "SelectCount";
        private const string MethodSelectPaging = "SelectPaging";
        private const string MethodSelectPublication = "SelectPublication";
        private const string MethodSearch = "Search";
        private const string MethodSearchSuggestMatch = "SearchSuggestMatch";
        private const string MethodUpdateLock = "UpdateLock";
        private const string MethodSelectSequence = "SelectSequence";
        private const string MethodUpdateQDate = "UpdateQDate";
        private const string MethodSearchAddressZip = "SearchAddressZip";
        private const string MethodSaveBulkWaveMailing = "SaveBulkWaveMailing";
        private const string MethodClearWaveMailingInfo = "ClearWaveMailingInfo";
        private const string MethodSelectForExport = "SelectForExport";
        private const string MethodSelectForExportStatic = "SelectForExportStatic";
        private const string MethodSelectAllActiveIDs = "SelectAllActiveIDs";
        private const string MethodSaveDqm = "SaveDQM";
        private const string MethodSelectForUpdate = "SelectForUpdate";
        private const string MethodRecordUpdate = "RecordUpdate";

        /// <summary>
        /// Selects a list of ProductSubscription objects based on the subscription ID, the client  and whether to include the available custom properties or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties</param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> Select(Guid accessKey, int subscriptionID, ClientConnections client, bool includeCustomProperties = false)
        {
            var auth = Authenticate(accessKey, false, string.Empty, EntityName, MethodSelect);
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(subscriptionID, client, auth.Client.DisplayName, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a ProductSubscription object based on the subscription ID, product ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubSubscriptionID">the pub subscription ID</param>
        /// <param name="client">the client object</param>
        /// <param name="clientDisplayName"></param>
        /// <returns>response.result will contain a ProductSubscription object</returns>
        public Response<EntityProductSubscription> SelectProductSubscription(Guid accessKey, int pubSubscriptionID, ClientConnections client, string clientDisplayName)
        {
            var model = new ServiceRequestModel<EntityProductSubscription>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectProductSubscription(pubSubscriptionID, client, clientDisplayName)
            };

            return GetResponse(model);
        }
        
        /// <summary>
        /// Updates the PubSubscriptionDetail data table with the information on the xml document for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="xml">the name of the xml document</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkActionIDUpdate(Guid accessKey, string xml, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkActionIdUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkActionIDUpdate(xml, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Updates the RequesterFlags in PubSubscriptions and IssueCompDetails
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productID">product id</param>
        /// <param name="issueID">issue id</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> UpdateRequesterFlags(Guid accessKey, int productID, int issueID, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateRequesterFlag,
                WorkerFunc = _ => new BusinessLogicWorker().Update_Requester_Flags(productID, issueID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the count of how many products with the given product ID are in the PubSubscriptions data table
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productID">the product ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> SelectCount(Guid accessKey, int productID, ClientConnections client)
        {
            var param = $"productID:{productID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCount,
                WorkerFunc = _ => new BusinessLogicWorker().SelectCount(productID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a certain amount of ProductSubscription objects that are in the PubSubscription data table
        /// based on the current page you want, the page size, the product ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="page">the current page</param>
        /// <param name="pageSize">the page size</param>
        /// <param name="productID">the product ID</param>
        /// <param name="client">the client ID</param>
        /// <param name="clientDisplayName"></param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> SelectPaging(
            Guid accessKey,
            int page,
            int pageSize,
            int productID,
            ClientConnections client,
            string clientDisplayName)
        {
            var param = $"page:{page} pageSize:{pageSize} productID:{productID}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectPaging,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPaging(page, pageSize, productID, client, clientDisplayName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of ProductSubscription objects based on the product Id and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productID">the product ID</param>
        /// <param name="client">the client object</param>
        /// <param name="clientDisplayName"></param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> SelectPublication(Guid accessKey, int productID, ClientConnections client, string clientDisplayName)
        {
            var param = $"publicationID:{productID}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectPublication,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublication(productID, client, clientDisplayName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches for ProductSubscription objects by its possible properties that can be given as a parameter
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="fName">the first name</param>
        /// <param name="lName">the last name</param>
        /// <param name="company">the company</param>
        /// <param name="title">the title of the subscription</param>
        /// <param name="add1">address 1</param>
        /// <param name="city">the city</param>
        /// <param name="regionCode">the region code</param>
        /// <param name="zip">the zip code</param>
        /// <param name="country">the counrty</param>
        /// <param name="email">the email address</param>
        /// <param name="phone">the phone number</param>
        /// <param name="sequenceID">the sequence ID</param>
        /// <param name="account">the account</param>
        /// <param name="publisherId">the publisher ID</param>
        /// <param name="publicationId">the publication ID</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="clientDisplayName"></param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> Search(
            Guid accessKey,
            ClientConnections client,
            string clientDisplayName,
            string fName = "",
            string lName = "",
            string company = "",
            string title = "",
            string add1 = "",
            string city = "",
            string regionCode = "",
            string zip = "",
            string country = "",
            string email = "",
            string phone = "",
            int sequenceID = 0,
            string account = "",
            int publisherId = 0,
            int publicationId = 0,
            int subscriptionID = 0)
        {
            var param = $"clientDisplayName: {clientDisplayName} firstName: {fName}lastName: {lName} company:{company} title:{title} add1:{add1} city:{city} regionCode:{regionCode} zip:{zip} country:{country} email:{email} phone:{phone} sequenceID:{sequenceID} account:{account} publisherId:{publisherId} publicationId:{publicationId}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSearch,
                WorkerFunc = _ => new BusinessLogicWorker().Search(
                    client,
                    clientDisplayName,
                    fName,
                    lName,
                    company,
                    title,
                    add1,
                    city,
                    regionCode,
                    zip,
                    country,
                    email,
                    phone,
                    sequenceID,
                    account,
                    publisherId,
                    publicationId,
                    subscriptionID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches for ProductSubscription objects based on the publisher ID, publication ID, first name, last name, email address and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client</param>
        /// <param name="publisherId">the publisher ID</param>
        /// <param name="publicationId">the publication ID</param>
        /// <param name="firstName">the first name</param>
        /// <param name="lastName">the last name</param>
        /// <param name="email">the email address</param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> SearchSuggestMatch(
            Guid accessKey,
            ClientConnections client,
            int publisherId,
            int publicationId,
            string firstName = "",
            string lastName = "",
            string email = "")
        {
            var param = $"publisherId: {publisherId} publicationId:{publicationId} firstName:{firstName} lastName:{lastName} email:{email}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSearchSuggestMatch,
                WorkerFunc = _ => new BusinessLogicWorker().SearchSuggestMatch(
                    client,
                    publisherId,
                    publicationId,
                    firstName,
                    lastName,
                    email)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Updates the lock status of the ProductSubscriptions and sets that it was updated by the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="isLocked">boolean if to lock or unlock the productSubscriptions</param>
        /// <param name="UserID">the user ID</param>
        /// <param name="client">the client</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> UpdateLock(Guid accessKey, int subscriptionID, bool isLocked, int UserID, ClientConnections client)
        {
            var param = $"subscriberID: {subscriptionID} updatedByUserID:{UserID} isLocked:{isLocked}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateLock,
                WorkerFunc = _ => new BusinessLogicWorker().UpdateLock(subscriptionID, isLocked, UserID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of ProductSubscription objects based on the sequence ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sequenceID">the sequence ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> SelectSequence(Guid accessKey, int sequenceID, ClientConnections client)
        {
            var param = $"sequenceID:{sequenceID}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSequence,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSequence(sequenceID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Updates when the productSubscription was queued and sets that it was updated by the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="qSourceDate">the date when the productSubscription was queued</param>
        /// <param name="updatedByUserID">the ID of the user that did the update</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> UpdateQDate(Guid accessKey, int subscriptionID, DateTime? qSourceDate, int updatedByUserID, ClientConnections client)
        {
            var param = qSourceDate.HasValue
                ? $"subscriptionID:{subscriptionID} qSourceDate:{qSourceDate.Value.ToShortDateString()} updatedByUserID:{updatedByUserID}"
                : $"subscriptionID:{subscriptionID} updatedByUserID:{updatedByUserID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateQDate,
                WorkerFunc = _ => new BusinessLogicWorker().UpdateQDate(subscriptionID, qSourceDate, updatedByUserID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches ProductSubscription objects by the address and the zip code
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="address1">the address</param>
        /// <param name="zipCode">the zip code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ProductSubscription objects</returns>
        public Response<List<EntityProductSubscription>> SearchAddressZip(Guid accessKey, string address1, string zipCode, ClientConnections client)
        {
            var param = $"address1: {address1} zipCode:{zipCode}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSearchAddressZip,
                WorkerFunc = _ => new BusinessLogicWorker().SearchAddressZip(address1, zipCode, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves Batches of mailing IDs from an xml document for the ProductSubscription objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="xml">the name of the xml document</param>
        /// <param name="waveMailingID">the mailing ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkWaveMailing(Guid accessKey, string xml, int waveMailingID, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkWaveMailing,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkWaveMailing(xml, waveMailingID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Clears the ProductSubscription objects mailing IDs for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="waveMailingID">the mailing ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> ClearWaveMailingInfo(Guid accessKey, int waveMailingID, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodClearWaveMailingInfo,
                WorkerFunc = _ => new BusinessLogicWorker().ClearWaveMailingInfo(waveMailingID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a ProductSubscription object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityProductSubscription"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityProductSubscription entity, ClientConnections client)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }

        public Response<int> ProfileSave(Guid accessKey, FrameworkUAD.Entity.ProductSubscription curr, FrameworkUAD.Entity.ProductSubscription orig, bool saveWaveMailing, int applicationID, 
            KMPlatform.BusinessLogic.Enums.UserLogTypes ult, FrameworkUAS.Object.Batch batch, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.ProductSubscription waveMail = null,
            FrameworkUAD.Entity.WaveMailingDetail waveMailDetail = null)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(curr);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProductSubscription", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscription worker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    int userLogTypeID = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, ult.ToString()).CodeId;
                    response.Message = "AccessKey Validated";
                    response.Result = worker.ProfileSave(curr, orig, saveWaveMailing, applicationID, ult, userLogTypeID, batch, client, waveMail, waveMailDetail);
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
        public Response<int> FullSave(Guid accessKey, FrameworkUAD.Entity.ProductSubscription curr, FrameworkUAD.Entity.ProductSubscription orig, bool saveWaveMailing, int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes ult,
                    FrameworkUAS.Object.Batch batch, int clientID, bool madeResponseChange, bool madePaidChange, bool madeBillToChange,
                    List<FrameworkUAD.Entity.ProductSubscriptionDetail> answers,   
                    FrameworkUAD.Entity.ProductSubscription waveMail = null, FrameworkUAD.Entity.WaveMailingDetail waveMailDetail = null, FrameworkUAD.Entity.SubscriptionPaid subPaid = null, FrameworkUAD.Entity.PaidBillTo billTo = null,
                    List<FrameworkUAD.Entity.ProductSubscriptionDetail> subscriberAnswers = null)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(curr);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProductSubscription", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscription worker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    int userLogTypeID = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, ult.ToString()).CodeId;
                    response.Message = "AccessKey Validated";
                    response.Result = worker.FullSave(curr, orig, saveWaveMailing, applicationID, ult, userLogTypeID, batch, clientID, madeResponseChange, madePaidChange, madeBillToChange, 
                        answers, waveMail, waveMailDetail, subPaid, billTo, subscriberAnswers);
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

        public Response<List<EntityActionProductSubscription>> SelectActionSubscription(Guid accessKey, int productId, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityActionProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectProductID(productId, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityActionProductSubscription>> SelectArchiveActionSubscription(Guid accessKey, int productId, int issueID, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityActionProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectProductID(productId, issueID, client)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectForExport(Guid accessKey, int page, int pageSize, string columns, int productID, ClientConnections client)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForExport,
                WorkerFunc = _ => new BusinessLogicWorker().Select_For_Export(page, pageSize, columns, productID, client)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectForExportStatic(Guid accessKey, int productID, string cols, List<int> subs, ClientConnections client)
        {
            var param = $"ClientConnString:{client.ClientLiveDBConnectionString} ProductId:{productID} Columns:{cols}";
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForExportStatic,
                WorkerFunc = _ => new BusinessLogicWorker().Select_For_Export_Static(productID, cols, subs, client)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectForExportStatic(Guid accessKey, int productID,int issueId, string cols, List<int> subs, ClientConnections client)
        {
            var param = $"ClientConnString:{client.ClientLiveDBConnectionString} ProductId:{productID} Columns:{cols}";
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForExportStatic,
                WorkerFunc = _ => new BusinessLogicWorker().Select_For_Export_Static(productID, issueId, cols, subs, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityCopiesProductSubscription>> SelectAllActiveIDs(Guid accessKey, int productID, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityCopiesProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectAllActiveIDs,
                WorkerFunc = _ => new BusinessLogicWorker().SelectAllActiveIDs(productID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a ProductSubscription object and goes through the DataMatching DQM Algorithms
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="clientName"></param>
        /// <param name="clientDisplayName"></param>
        /// <param name="clientID"></param>
        /// <param name="client"></param>
        /// <returns>response.result will contain a dictionsry of the SubscriptionID and PubSubscriptionID</returns>
        public Response<Dictionary<int, int>> SaveDQM(Guid accessKey, FrameworkUAD.Entity.ProductSubscription x, string clientName, string clientDisplayName, int clientID, KMPlatform.Object.ClientConnections client)
        {
            Response<Dictionary<int, int>> response = new Response<Dictionary<int, int>>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProductSubscription", "SaveDQM");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscription worker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                    response.Message = "AccessKey Validated";
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    int fileRecurrenceTypeId = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.File_Recurrence, FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.One_Time.ToString()).CodeId;
                    int databaseFileTypeId = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Database_File, FrameworkUAD_Lookup.Enums.DatabaseDestinationTypes.UAD.ToString()).CodeId;

                    response.Result = worker.SaveDQM(x, response.ProcessCode, clientName, clientDisplayName, clientID, client, fileRecurrenceTypeId, databaseFileTypeId);
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

        public Response<List<EntityPubSubscriptionAdHoc>> Get_AdHocs_PubSubscription(Guid accessKey, int pubID, int pubSubscriptionID, ClientConnections client)
        {
            var param = $"PubSubscriptionID: {pubSubscriptionID}, PubID: {pubID}";
            var model = new ServiceRequestModel<List<EntityPubSubscriptionAdHoc>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveDqm,
                WorkerFunc = _ => new BusinessLogicWorker().Get_AdHocs(pubID, pubSubscriptionID, client)
            };

            return GetResponse(model);
        }

        public Response<List<string>> Get_AdHocs(Guid accessKey, int pubID, ClientConnections client)
        {
            var param = $"PubID: {pubID}";
            var model = new ServiceRequestModel<List<string>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveDqm,
                WorkerFunc = _ => new BusinessLogicWorker().Get_AdHocs(pubID, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityProductSubscription>> SelectForUpdate(Guid accessKey, int productID, int issueId, List<int> subs, ClientConnections client)
        {
            var param = $"productID: {productID} issueId:{issueId}";
            var model = new ServiceRequestModel<List<EntityProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForUpdate(productID, issueId, subs, client)
            };

            return GetResponse(model);
        }

        public Response<bool> RecordUpdate(
            Guid accessKey,
            HashSet<int> pubSubscriptionIds,
            string changes,
            int issueId,
            int productID,
            int userId,
            ClientConnections client)
        {
            var param = $"productID: {productID} issueId:{issueId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodRecordUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().RecordUpdate(pubSubscriptionIds, changes, issueId, productID, userId, client)
            };

            return GetResponse(model);
        }
    }
}
