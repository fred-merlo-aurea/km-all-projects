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
    public interface ITransformationFieldMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.TransformationFieldMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="transformationID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForTransformation")]
        Response<List<FrameworkUAS.Entity.TransformationFieldMap>> Select(Guid accessKey, int transformationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformationFieldMap x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="TransformationName"></param>
        /// <param name="clientID"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Delete(Guid accessKey, string TransformationName, int clientID, string ColumnName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteSourceFile(Guid accessKey, int SourceFileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="FieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteFieldMapping(Guid accessKey, int FieldMappingID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="TransformationName"></param>
        /// <param name="clientID"></param>
        /// <param name="FieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteTransformationFieldMapping(Guid accessKey, string TransformationName, int clientID, int FieldMappingID);
    }
}
