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
    public interface ITransformationFieldMultiMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.TransformationFieldMultiMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="transformationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.TransformationFieldMultiMap>> SelectTransformationID(Guid accessKey, int transformationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformationFieldMultiMap x);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="FieldMultiMapID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteByFieldMultiMapID(Guid accessKey, int FieldMultiMapID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteBySourceFileID(Guid accessKey, int SourceFileID);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <param name="FieldMultiMapID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteBySourceFileIDAndFieldMultiMapID(Guid accessKey, int SourceFileID, int FieldMultiMapID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="FieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteByFieldMappingID(Guid accessKey, int FieldMappingID);
    }
}
