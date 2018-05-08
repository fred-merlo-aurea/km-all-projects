using System;
using System.Linq;
using Core.ADMS.Events;

namespace ADMS.Services.UAD
{
    public interface IUADProcessor
    {
        void HandleFileCleansed(FileCleansed eventMessage);
        event Action<FileProcessed> FileProcessed;
    }
}
