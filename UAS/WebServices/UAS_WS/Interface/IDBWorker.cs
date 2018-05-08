using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IDBWorker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name="GetPubIDAndCodesByClientName")]
        Response<Dictionary<int, string>> GetPubIDAndCodesByClient(Guid accessKey, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<Dictionary<int, string>> GetPubIDAndCodesByClient(Guid accessKey, KMPlatform.Entity.Client client);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="transformationId"></param>
        /// <param name="transformationTypeId"></param>
        /// <param name="transformationName"></param>
        /// <param name="transformationDescription"></param>
        /// <param name="clientId"></param>
        /// <param name="userID"></param>
        /// <param name="isMapsPubCode"></param>
        /// <param name="isLastStep"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, int transformationId, int transformationTypeId, string transformationName, string transformationDescription, int clientId, int userID, bool isMapsPubCode = false, bool isLastStep = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name="GetClientSqlConnectionForClientName")]
        Response<SqlConnection> GetClientSqlConnection(Guid accessKey, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<SqlConnection> GetClientSqlConnection(Guid accessKey, KMPlatform.Entity.Client client);
    }
}
