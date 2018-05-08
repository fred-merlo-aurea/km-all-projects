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
    public interface IAdHocDimensionGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.AdHocDimensionGroup>> Select(Guid accessKey, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClient")]
        Response<List<FrameworkUAS.Entity.AdHocDimensionGroup>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="adHocDimensionGroupName"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientAdHocDimesionGroupName")]
        Response<List<FrameworkUAS.Entity.AdHocDimensionGroup>> Select(Guid accessKey, int clientID, string adHocDimensionGroupName, bool includeCustomProperties = false);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="adHocDimensionGroupName"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientSourceFileAdHocDimensionGroupName")]
        Response<FrameworkUAS.Entity.AdHocDimensionGroup> Select(Guid accessKey, int clientID, int sourceFileID, string adHocDimensionGroupName, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientSourceFile")]
        Response<List<FrameworkUAS.Entity.AdHocDimensionGroup>> Select(Guid accessKey, int clientID, int sourceFileID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="adHocDimensionGroupId"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Entity.AdHocDimensionGroup> SelectByAdHocDimensionGroupId(Guid accessKey, int adHocDimensionGroupId, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.AdHocDimensionGroup x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAS.Entity.AdHocDimensionGroup> list);
    }
}
