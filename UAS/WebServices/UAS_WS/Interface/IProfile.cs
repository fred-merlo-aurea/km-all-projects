using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchFields"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Profile>> Search(Guid accessKey, string searchValue, string searchFields, string orderBy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="profileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.Profile> SelectForProfile(Guid accessKey, int profileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="publicationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Profile>> SelectForPublication(Guid accessKey, int publicationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="publisherID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Profile>> SelectForPublisher(Guid accessKey, int publisherID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="publicationID"></param>
        /// <param name="isSubscribed"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Profile>> SelectForPublicationSubscribed(Guid accessKey, int publicationID, bool isSubscribed);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="publicationID"></param>
        /// <param name="isProspect"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Profile>> SelectForPublicationProspect(Guid accessKey, int publicationID, bool isProspect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="publicationID"></param>
        /// <param name="isSubscribed"></param>
        /// <param name="isProspect"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Profile>> Select(Guid accessKey, int publicationID, bool isSubscribed, bool isProspect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.Profile> BindPublicationList(Guid accessKey, KMPlatform.Entity.Profile x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchList"></param>
        /// <returns></returns>
        [OperationContract(Name = "SearchList")]
        Response<List<KMPlatform.Entity.Profile>> Search(Guid accessKey, string searchValue, List<KMPlatform.Entity.Profile> searchList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.Profile x);
    }
}
