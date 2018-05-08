using System;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.Encryption;
using EntityEncryption = FrameworkUAS.Object.Encryption;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Encryption : FrameworkServiceBase, IEncryption
    {
        private const string EntityName = "Encryption";
        private const string MethodEncrypt = "Encrypt";
        private const string MethodDecrypt = "Decrypt";

        /// <summary>
        /// Encrypts the Encryption object (passwords)
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityEncryption"/> object</param>
        /// <returns>response.result will contain an Encryption object</returns>
        public Response<EntityEncryption> Encrypt(Guid accessKey, EntityEncryption entity)
        {
            var model = new ServiceRequestModel<EntityEncryption>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodEncrypt,
                WorkerFunc = _ => new BusinessLogicWorker().Encrypt(entity)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Decrypts the Encryption object (passwords)
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityEncryption"/> object</param>
        /// <returns>response.result will contain an Encryption object</returns>
        public Response<EntityEncryption> Decrypt(Guid accessKey, EntityEncryption entity)
        {
            var model = new ServiceRequestModel<EntityEncryption>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDecrypt,
                WorkerFunc = _ => new BusinessLogicWorker().Decrypt(entity)
            };

            return GetResponse(model);
        }
    }
}
