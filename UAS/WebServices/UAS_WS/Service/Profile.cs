using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.Profile;
using EntityProfile = KMPlatform.Entity.Profile;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Profile : FrameworkServiceBase, IProfile
    {
        private const string EntityName = "Profile";
        private const string MethodSearch = "Search";
        private const string MethodSelectForProfile = "SelectForProfile";
        private const string MethodSelectForPublication = "SelectForPublication";
        private const string MethodSelectForPublisher = "SelectForPublisher";
        private const string MethodSelectForPublicationSubscribed = "SelectForPublicationSubscribed";
        private const string MethodSelectForPublicationProspect = "SelectForPublicationProspect";
        private const string MethodSelect = "Select";
        private const string MethodBindPublicationList = "BindPublicationList";
        private const string MethodSave = "Save";

        /// <summary>
        /// Creates a list of Profile objects dynamically based on the search value within the search fields and ordered by the 'orderBy' string
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="searchValue">the search value</param>
        /// <param name="searchFields">the search fields</param>
        /// <param name="orderBy">what the list is ordered by</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> Search(Guid accessKey, string searchValue, string searchFields, string orderBy)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SearchValue:{searchValue} SearchFields:{searchFields} OrderBy:{orderBy}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSearch,
                WorkerFunc = _ => new BusinessLogicWorker().Search(searchValue, searchFields, orderBy)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects an available Profile by the profile ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="profileID">the profile ID</param>
        /// <returns>response.result will contain a Profile object</returns>
        public Response<EntityProfile> SelectForProfile(Guid accessKey, int profileID)
        {
            var model = new ServiceRequestModel<EntityProfile>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"ProfileID: {profileID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForProfile,
                WorkerFunc = _ => new BusinessLogicWorker().Select(profileID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Profile objects based on the publication ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> SelectForPublication(Guid accessKey, int publicationID)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"PublicationID:{publicationID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForPublication,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublication(publicationID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Profile objects based on the publisher ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publisherID">the publisher ID</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> SelectForPublisher(Guid accessKey, int publisherID)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"PublisherID:{publisherID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForPublisher,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublisher(publisherID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Profile objects based on the publication ID and if the Profile object is subscribed or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="isSubscribed">boolean whether or not the Profile is subscribed or not</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> SelectForPublicationSubscribed(Guid accessKey, int publicationID, bool isSubscribed)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"PublicationID: {publicationID} IsSubscribed:{isSubscribed}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForPublicationSubscribed,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublicationSubscribed(publicationID, isSubscribed)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Profile objects based on the publication ID and if the Profile object is a prospect or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="isProspect">boolean if the Profile object is a prospect or not</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> SelectForPublicationProspect(Guid accessKey, int publicationID, bool isProspect)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"PublicationID: {publicationID} IsProspect:{isProspect}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForPublicationProspect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublicationProspect(publicationID, isProspect)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Profile objects based on the publication ID and if the Profile object is subscribed and if it is a prospect
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="isSubscribed">boolean if the Profile object is subscribed or not</param>
        /// <param name="isProspect">boolean if the Profile object is a prospect or not</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> Select(Guid accessKey, int publicationID, bool isSubscribed, bool isProspect)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"PublicationID: {publicationID} IsSubscribed:{isSubscribed} IsProspect:{isProspect}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublication(publicationID, isSubscribed, isProspect)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Binds a publication list to a Profile object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityProfile"/> object</param>
        /// <returns>response.result will contain a Profile object</returns>
        public Response<EntityProfile> BindPublicationList(Guid accessKey, EntityProfile entity)
        {
            var model = new ServiceRequestModel<EntityProfile>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodBindPublicationList,
                WorkerFunc = _ => new BusinessLogicWorker().BindPublicationList(entity)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Creates a list of Profile objects dynamically based on a search value within a given search list of other Profile objects 
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="searchValue">the search value</param>
        /// <param name="searchList">the search list to search through</param>
        /// <returns>response.result will contain a list of Profile objects</returns>
        public Response<List<EntityProfile>> Search(Guid accessKey, string searchValue, List<EntityProfile> searchList)
        {
            var model = new ServiceRequestModel<List<EntityProfile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SearchValue:{searchValue}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSearch,
                WorkerFunc = _ => new BusinessLogicWorker().Search(searchValue, searchList)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a Profile object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityProfile"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityProfile entity)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
