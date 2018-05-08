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
    public interface IAdHocDimension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="adHocDimensionGroupId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.AdHocDimension>> Select(Guid accessKey, int adHocDimensionGroupId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Delete(Guid accessKey, int SourceFileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAS.Entity.AdHocDimension> list);
    }
}
