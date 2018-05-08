using System;
using System.Linq;
using Core.ADMS.Events;

namespace ADMS.Services.FileWatcher
{
    public interface IFileWatcher
    {
        System.IO.FileSystemWatcher CreateFileSystemWatcher(KMPlatform.Entity.Client client, string directory);
        event Action<FileDetected> FileDetected;
        event Action<FileProcessed> FileProcessed;
        void Activate(bool isActivated = false);
        void CheckForFiles();
    }
}
