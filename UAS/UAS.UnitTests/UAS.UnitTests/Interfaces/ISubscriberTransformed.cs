using System;
using System.Collections.Generic;
using FrameworkUAD.Object;
using KMPlatform.Object;
using UADFramework = FrameworkUAD.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface ISubscriberTransformed
    {
        bool AddressUpdateBulkSql(List<UADFramework.SubscriberTransformed> list, ClientConnections client);
        bool AddressValidateExisting(ClientConnections client, int sourceFileID, string processCode);
        List<UADFramework.SubscriberTransformed> AddressValidation_Paging(int currentPage, int pageSize, string processCode, ClientConnections client, bool isLatLonValid = false, int sourceFileID = 0);
        int CountAddressValidation(ClientConnections client, bool isLatLonValid);
        int CountAddressValidation(ClientConnections client, string processCode, bool isLatLonValid);
        int CountAddressValidation(ClientConnections client, int sourceFileID, bool isLatLonValid);
        int CountForGeoCoding(ClientConnections client);
        int CountForGeoCoding(ClientConnections client, int sourceFileID);
        bool DataMatching(ClientConnections client, int sourceFileID, string processCode);
        bool DataMatching_multiple(ClientConnections client, int sourceFileId, string processCode, string matchFields);
        bool DataMatching_single(ClientConnections client, string processCode, string matchField);
        void FormatData(UADFramework.SubscriberTransformed x);
        List<string> GetDistinctPubCodes(ClientConnections client, string processCode);
        bool RevertXmlFormattingAfterBulkInsert(string processCode, ClientConnections client);
        int Save(UADFramework.SubscriberTransformed x, ClientConnections client);
        bool SaveBulkInsert(List<UADFramework.SubscriberTransformed> list, ClientConnections client);
        bool SaveBulkSqlInsert(List<UADFramework.SubscriberTransformed> list, ClientConnections client, bool isDataCompare);
        List<UADFramework.SubscriberTransformed> Select(ClientConnections client);
        List<UADFramework.SubscriberTransformed> Select(string processCode, ClientConnections client);
        List<UADFramework.SubscriberTransformed> Select(ClientConnections client, int sourceFileID);
        List<UADFramework.SubscriberTransformed> SelectByAddressValidation(ClientConnections client, bool isLatLonValid);
        List<UADFramework.SubscriberTransformed> SelectByAddressValidation(ClientConnections client, string processCode, bool isLatLonValid);
        List<UADFramework.SubscriberTransformed> SelectByAddressValidation(ClientConnections client, int sourceFileID, bool isLatLonValid);
        List<UADFramework.SubscriberTransformed> SelectByAddressValidation(ClientConnections client, string processCode, int sourceFileID, bool isLatLonValid);
        DimensionErrorCount SelectDimensionCount(string processCode, ClientConnections client);
        List<UADFramework.SubscriberTransformed> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, ClientConnections client);
        List<UADFramework.SubscriberTransformed> SelectForGeoCoding(ClientConnections client);
        List<UADFramework.SubscriberTransformed> SelectForGeoCoding(ClientConnections client, int sourceFileID);
        List<ImportRowNumber> SelectImportRowNumbers(ClientConnections client, string ProcessCode);
        UADFramework.SubscriberTransformed SelectTopOne(string processCode, ClientConnections client);
        bool SequenceDataMatching(ClientConnections client, string processCode);
        bool StandardRollUpToMaster(ClientConnections client, int sourceFileID, string processCode);
        bool StandardRollUpToMaster(ClientConnections client, int sourceFileID, string processCode, bool mailPermissionOverRide = false, bool faxPermissionOverRide = false, bool phonePermissionOverRide = false, bool otherProductsPermissionOverRide = false, bool thirdPartyPermissionOverRide = false, bool emailRenewPermissionOverRide = false, bool textPermissionOverRide = false, bool updateEmail = true, bool updatePhone = true, bool updateFax = true, bool updateMobile = true);
    }
}