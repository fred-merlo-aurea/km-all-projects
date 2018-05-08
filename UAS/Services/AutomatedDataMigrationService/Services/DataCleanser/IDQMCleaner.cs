using System;
using System.Linq;
using Core.ADMS.Events;

namespace ADMS.Services.DataCleanser
{
    
    public interface IDQMCleaner
    {
        void HandleFileAddressGeocoded(FileAddressGeocoded eventMessage);
        event Action<FileCleansed> FileCleansed;
        event Action<FileProcessed> FileProcessed;
    }
}
