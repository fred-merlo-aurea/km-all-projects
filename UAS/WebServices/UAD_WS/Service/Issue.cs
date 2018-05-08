using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Issue;
using EntityIssue = FrameworkUAD.Entity.Issue;

namespace UAD_WS.Service
{
    public class Issue : FrameworkServiceBase, IIssue
    {
        private const string EntityName = "Issue";
        private const string MethodArchiveAll = "ArchiveAll";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";
        private const string MethodSelectForPublisher = "SelectForPublisher";
        private const string MethodSelectForPublication = "SelectForPublication";

        /// <summary>
        /// Selects a list of Issue objects based on the publication ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Issue objects</returns>
        public Response<List<EntityIssue>> SelectForPublication(Guid accessKey, int publicationID, ClientConnections client)
        {
            var param = $"publicationID:{publicationID}";
            var model = new ServiceRequestModel<List<EntityIssue>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForPublication,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublication(publicationID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Issue objects based on the publisher ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publisherID">the publisher ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Issue objects</returns>
        public Response<List<EntityIssue>> SelectForPublisher(Guid accessKey, int publisherID, ClientConnections client)
        {
            var param = $"publicationID:{publisherID}";
            var model = new ServiceRequestModel<List<EntityIssue>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForPublisher,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPublisher(publisherID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Issue objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Issue objects</returns>
        public Response<List<EntityIssue>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityIssue>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a Issue object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityIssue"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityIssue entity, ClientConnections client)
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

        public Response<bool> ArchiveAll(Guid accessKey, int productID, int issueID, Dictionary<int, string> imb, Dictionary<int, string> compImb, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodArchiveAll,
                WorkerFunc = _ => new BusinessLogicWorker().ArchiveAll(productID, issueID, imb, compImb, client)
            };

            return GetResponse(model);
        }

        public Response<bool> BulkInsertSubGenIDs(Guid accessKey, List<FrameworkUAD.Entity.IssueCloseSubGenMap> ids, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodArchiveAll,
                WorkerFunc = _ => new BusinessLogicWorker().BulkInsertSubGenIDs(ids, client)
            };

            return GetResponse(model);
        }

        public Response<bool> ValidateArchive(Guid accessKey, int pubId, int issueId, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodArchiveAll,
                WorkerFunc = _ => new BusinessLogicWorker().ValidateArchive(pubId, issueId, client)
            };

            return GetResponse(model);
        }

        public Response<bool> RollBackIssue(Guid accessKey, int pubId, int issueId, int origIMB, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodArchiveAll,
                WorkerFunc = _ => new BusinessLogicWorker().RollBackIssue(pubId, issueId, origIMB, client)
            };

            return GetResponse(model);
        }
    }
}
