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
    [ServiceKnownType(typeof(DateTime?))]
    public interface ISourceFile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.SourceFile>> Select(Guid accessKey, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectByDeleted")]
        Response<List<FrameworkUAS.Entity.SourceFile>> Select(Guid accessKey, bool includeCustomProperties = false, bool isDeleted = false);


        //[OperationContract(Name = "SelectForClientName")]
        //Response<List<FrameworkUAS.Entity.SourceFile>> Select(Guid accessKey, string clientName, bool includeCustomProperties = false, bool isDeleted = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientByDeleted")]
        Response<List<FrameworkUAS.Entity.SourceFile>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false, bool isDeleted = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientID")]
        Response<List<FrameworkUAS.Entity.SourceFile>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientName"></param>
        /// <param name="fileName"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClientAndFile")]
        Response<FrameworkUAS.Entity.SourceFile> Select(Guid accessKey, string clientName, string fileName, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Entity.SourceFile> SelectForSourceFile(Guid accessKey, int sourceFileID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.SourceFile>> SelectSpecialFiles(Guid accessKey, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectSpecialFilesForClient")]
        Response<List<FrameworkUAS.Entity.SourceFile>> SelectSpecialFiles(Guid accessKey, int clientID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="defaultRules">will default Rules be applied to SourceFile</param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.SourceFile x, bool defaultRules = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="SourceFileID"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Delete(Guid accessKey, int SourceFileID, int ClientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> IsFileNameUnique(Guid accessKey, int clientId, string fileName);

        //void GetCustomProperties(FrameworkUAS.Entity.SourceFile sourceFile);
    }
}
