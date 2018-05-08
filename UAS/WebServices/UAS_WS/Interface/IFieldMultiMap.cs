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
    public interface IFieldMultiMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.FieldMultiMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.FieldMultiMap>> SelectFieldMappingID(Guid accessKey, int fieldMappingID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fieldMultiMapID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Entity.FieldMultiMap> SelectFieldMultiMapID(Guid accessKey, int fieldMultiMapID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.FieldMultiMap x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteBySourceFileID(Guid accessKey, int SourceFileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="FieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteByFieldMappingID(Guid accessKey, int FieldMappingID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="FieldMultiMapID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteByFieldMultiMapID(Guid accessKey, int FieldMultiMapID);
    }
}
