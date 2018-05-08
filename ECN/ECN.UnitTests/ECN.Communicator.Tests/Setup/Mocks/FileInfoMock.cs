using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Shim = System.IO.Fakes.ShimFileInfo;
using ShimFileSystemInfo = System.IO.Fakes.ShimFileSystemInfo;

namespace ECN.Communicator.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class FileInfoMock
    {
        public FileInfoMock()
        {
            FilePaths = new Dictionary<FileSystemInfo, string>();
            FileLengths = new Dictionary<string, long>();
            LastWriteTimes = new Dictionary<string, DateTime>();
            SetupShims();
        }

        public IDictionary<FileSystemInfo, string> FilePaths { get; }

        public IDictionary<string, long> FileLengths { get; }

        public IDictionary<string, DateTime> LastWriteTimes { get; }

        private void SetupShims()
        {
            Shim.ConstructorString = Constructor;
            Shim.AllInstances.NameGet = NameGet;
            Shim.AllInstances.LengthGet = LengthGet;
            ShimFileSystemInfo.AllInstances.LastWriteTimeGet = LastWriteTime;
        }

        private DateTime LastWriteTime(FileSystemInfo instance)
        {
            var result = default(DateTime);
            string path = null;
            if (FilePaths.TryGetValue(instance, out path))
            {
                LastWriteTimes.TryGetValue(path, out result);
            }
            return result;
        }

        private long LengthGet(FileInfo instance)
        {
            var result = default(long);
            string path = null;
            if (FilePaths.TryGetValue(instance, out path))
            {
                FileLengths.TryGetValue(path, out result);
            }
            return result;
        }

        private string NameGet(FileInfo instance)
        {
            string result = null;
            string path = null;
            if (FilePaths.TryGetValue(instance, out path))
            {
                result = Path.GetFileName(path);
            }
            return result;
        }

        private void Constructor(FileInfo instance, string path)
        {
            FilePaths[instance] = path;
        }
    }
}
