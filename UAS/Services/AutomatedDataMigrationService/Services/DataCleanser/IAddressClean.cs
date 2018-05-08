using System;
using System.Linq;
using Core.ADMS.Events;

namespace ADMS.Services.DataCleanser
{
    public interface IAddressClean
    {
        void HandleFileValidated(FileValidated eventMessage);
        event Action<FileProcessed> FileProcessed;
        event Action<FileAddressGeocoded> FileAddressGeocoded;
    }
}
