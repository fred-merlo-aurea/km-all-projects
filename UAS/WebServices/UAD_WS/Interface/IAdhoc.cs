using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IAdhoc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Adhoc>> SelectAll(Guid accessKey, KMPlatform.Object.ClientConnections client);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Adhoc>> SelectCategoryID(Guid accessKey, KMPlatform.Object.ClientConnections client, int categoryID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.Adhoc x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Delete(Guid accessKey, KMPlatform.Object.ClientConnections client, int categoryID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="adhocID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Delete_AdHoc(Guid accessKey, KMPlatform.Object.ClientConnections client, int adhocID);
    }
}
