using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISubscriberTransformed
    {
        #region Selects
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<FrameworkUAD.Entity.SubscriberTransformed> SelectTopOne(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForSourceFile")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID);
        
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);
        #endregion

        #region GeoCoding Selects and Address Validation
        [OperationContract(Name = "SelectAddressValidationForSourceFile")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, bool isLatLonValid);

        [OperationContract(Name = "SelectAddressValidationForProcessCodeAndSourceFile")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, string processCode, int sourceFileID, bool isLatLonValid);

        [OperationContract(Name = "SelectAddressValidationForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid);

        [OperationContract(Name = "SelectAddressValidation")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, bool isLatLonValid);

        [OperationContract(Name = "SelectGeoCoding")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectGeoCodingForSourceFile")]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> SelectForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberTransformed>> AddressValidation_Paging(Guid accessKey, int currentPage, int pageSize, string processCode, KMPlatform.Object.ClientConnections client, bool isLatLonValid = false, int sourceFileID = 0);
        #endregion

        #region GeoCode Counts
        [OperationContract(Name = "CountAddressValidationForSourceFile")]
        Response<int> CountAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, bool isLatLonValid);

        [OperationContract(Name = "CountAddressValidationForProcessCode")]
        Response<int> CountAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid);

        [OperationContract]
        Response<int> CountAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, bool isLatLonValid);

        [OperationContract(Name = "CountGeoCoding")]
        Response<int> CountForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "CountGeoCodingForSourceFile")]
        Response<int> CountForGeoCoding(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID);
        #endregion

        #region Saving / Updating
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberTransformed x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client, bool isDataCompare);

        [OperationContract]
        Response<bool> AddressUpdateBulkSql(Guid accessKey, List<FrameworkUAD.Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client);
        #endregion

        #region Jobs / Operations
        [OperationContract]
        Response<bool> StandardRollUpToMaster(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode);

        [OperationContract]
        Response<bool> AddressValidateExisting(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode);

        [OperationContract]
        Response<bool> DataMatching(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode);

        [OperationContract]
        Response<bool> DisableIndexes(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> EnableIndexes(Guid accessKey, KMPlatform.Object.ClientConnections client);
        #endregion
    }
}
