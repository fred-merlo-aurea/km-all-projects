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
    public interface IAdHocDimensionGroupPubcodeMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="adHocDimensionGroupId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap>> Select(Guid accessKey, int adHocDimensionGroupId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap> list);
    }
}
