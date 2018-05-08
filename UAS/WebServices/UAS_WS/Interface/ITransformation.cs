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
    public interface ITransformation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.Transformation>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="transformationID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForTransformation")]
        Response<List<FrameworkUAS.Entity.Transformation>> Select(Guid accessKey, int transformationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClient")]
        Response<List<FrameworkUAS.Entity.Transformation>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientAndFile")]
        Response<List<FrameworkUAS.Entity.Transformation>> Select(Guid accessKey, int clientID, int sourceFileID, bool includeCustomProperties = false);


        //[OperationContract(Name = "SelectForClientName")]
        //Response<List<FrameworkUAS.Entity.Transformation>> Select(Guid accessKey, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectAssignedForFileName")]
        Response<List<FrameworkUAS.Entity.Transformation>> SelectAssigned(Guid accessKey, string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fieldMappingID"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectAssignedForMapping")]
        Response<List<FrameworkUAS.Entity.Transformation>> SelectAssigned(Guid accessKey, int fieldMappingID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="transformationName"></param>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientAndTransformation")]
        Response<List<FrameworkUAS.Entity.Transformation>> Select(Guid accessKey, string transformationName, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.Transformation x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="transformationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Delete(Guid accessKey, int transformationID);

        //void GetCustomProperties(FrameworkUAS.Entity.Transformation transformation);
    }
}
