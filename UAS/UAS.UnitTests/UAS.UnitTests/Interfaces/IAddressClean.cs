using System;
using Core.ADMS.Events;
using FrameworkUAD_Lookup;
using FrameworkUAS.Entity;
using KMPlatform.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface IAddressClean
    {
        event Action<FileAddressGeocoded> FileAddressGeocoded;
        event Action<FileProcessed> FileProcessed;

        void AddressStandardize(Client client, int sourceFileId, string processCode, Enums.ProcessingStatusType fsTypeName = Enums.ProcessingStatusType.In_Cleansing);
        void CountryRegionCleanse(int sourceFileID, string processCode, Client client);
        void ExecuteAddressCleanse(AdmsLog admsLog, Client client);
        void HandleFileValidated(FileValidated eventMessage);
    }
}