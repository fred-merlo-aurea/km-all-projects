using System;
using System.IO;

namespace ECN_Framework_Common.Functions
{
    public class DataStorageCalculator
    {
        private double _sizeInBytes;

        public double GetSizeInBytes(string folderPath)
        {
            GetRootDirSize(folderPath);
            return _sizeInBytes;
        }

        public decimal GetSizeInKiloBytes(string folderPath)
        {
            GetRootDirSize(folderPath);
            Decimal KB = 0.0M;
            KB = Math.Round(Convert.ToDecimal((_sizeInBytes / 1024)), 2);
            return KB;
        }

        public decimal GetSizeInMegaBytes(string folderPath)
        {
            GetRootDirSize(folderPath);
            Decimal KB = 0.0M, MB = 0.0M;
            KB = Math.Round(Convert.ToDecimal((_sizeInBytes / 1024)), 2);
            MB = Math.Round(Convert.ToDecimal(KB / 1024), 2);
            return MB;
        }

        private void GetRootDirSize(string folderPath)
        {
            Size(folderPath, true);
        }

        private double Size(string directory, bool deep)
        {
            var dir = new DirectoryInfo(directory);
            foreach (FileInfo fileInfo in dir.GetFiles())
            {
                _sizeInBytes += fileInfo.Length;
            }
            if (deep)
            {
                foreach (var directoryInfo in dir.GetDirectories())
                {
                    Size(directoryInfo.FullName, deep);
                }
            }
            return _sizeInBytes;
        }
    }
}
