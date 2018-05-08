using System;

namespace KM.Common.Import
{
    [Serializable]
    public class FileConfiguration
    {
        public FileConfiguration() { }

        public string FileFolder { get; set; }

        public string FileExtension { get; set; }

        public bool IsQuoteEncapsulated { get; set; }

        public string FileColumnDelimiter { get; set; }

        public int ColumnCount { get; set; }

        public string ColumnHeaders { get; set; }
    }
}
