using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using Core_AMS.Utilities;
using System.IO;
using KM.Common.Import;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IImportVessel
    {
        [OperationContract]
        Response<string> GetCustomerErrorMessage(Guid accessKey, FrameworkUAD.Object.ImportVessel iv);

        [OperationContract]
        Response<string> GetBadData(Guid accessKey, FrameworkUAD.Object.ImportVessel iv);

        [OperationContract]
        Response<string> GetCleanOriginalData(Guid accessKey, FrameworkUAD.Object.ImportVessel iv);

        [OperationContract]
        Response<string> GetTransformedData(Guid accessKey, FrameworkUAD.Object.ImportVessel iv);

        [OperationContract]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVessel(Guid accessKey, FileInfo fileInfo, FileConfiguration fileConfig = null);

        [OperationContract(Name = "GetImportVesselBatch")]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVessel(Guid accessKey, FileInfo fileInfo, int startRow, int takeRowCount, FileConfiguration fileConfig = null);

        [OperationContract]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVesselExcel(Guid accessKey, FileInfo fileInfo);

        [OperationContract]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVesselDbf(Guid accessKey, FileInfo fileInfo);

        [OperationContract(Name = "GetImportVesselDbfBatch")]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVesselDbf(Guid accessKey, FileInfo fileInfo, int startRow, int takeRowCount);

        [OperationContract]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVesselText(Guid accessKey, FileInfo fileInfo, FileConfiguration fileConfig);

        [OperationContract(Name = "GetImportVesselTextBatch")]
        Response<FrameworkUAD.Object.ImportVessel> GetImportVesselText(Guid accessKey, FileInfo fileInfo, int startRow, int takeRowCount, FileConfiguration fileConfig);

        [OperationContract]
        Response<FrameworkUAD.Object.ImportVessel> LoadFileImportVessel(Guid accessKey, FileInfo file, FileConfiguration fileConfig);

        [OperationContract(Name = "LoadFileImportVesselBatch")]
        Response<FrameworkUAD.Object.ImportVessel> LoadFileImportVessel(Guid accessKey, FileInfo file, int startRow, int takeRowCount, FileConfiguration fileConfig);
    }
}
