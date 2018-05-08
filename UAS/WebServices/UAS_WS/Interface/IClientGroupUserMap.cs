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
    public interface IClientGroupUserMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> SelectForClientGroup(Guid accessKey, int clientGroupID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> SelectForUser(Guid accessKey, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.ClientGroupUserMap x);
    }
}
