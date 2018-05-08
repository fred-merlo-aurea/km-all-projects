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
    public interface IFieldMapping
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.FieldMapping>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForSourceFile")]
        Response<List<FrameworkUAS.Entity.FieldMapping>> Select(Guid accessKey, int sourceFileID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Entity.FieldMapping> SelectForFieldMapping(Guid accessKey, int fieldMappingID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClient")]
        Response<List<FrameworkUAS.Entity.FieldMapping>> Select(Guid accessKey, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClientAndFile")]
        Response<List<FrameworkUAS.Entity.FieldMapping>> Select(Guid accessKey, int clientID, string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> ColumnReorder(Guid accessKey, int SourceFileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.FieldMapping x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Delete(Guid accessKey, int SourceFileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="FieldMappingID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> DeleteMapping(Guid accessKey, int FieldMappingID);
    }
}
