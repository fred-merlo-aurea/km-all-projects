using System.Configuration;

namespace AMS_Operations
{
    // KMAMS-2016=>KMAMS-2048: AMS_Operations hardcoded values
    class OperationsSettings
    {
        private const string FTPPath = "ftp.knowledgemarketing.com";

        // Properities' Default Values
        private static readonly string ClientArchivePath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\ADMS\Client Archive";
        private static readonly string SourceMediaPrinterFileExportPath_Default = @"C:\ADMS\Logs\ADMS\bbcExport.csv";
        private static readonly string SAETransferSourcePath_Default = @"\\ftp.knowledgemarketing.com\c$\Users\Saetb\Documents\FTP\";
        private static readonly string SAETransferTargetPath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\SAETB\ADMS";
        private static readonly string HWPublishingTransferSourcePath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\HWPublishing";
        private static readonly string HWPublishingTransferTargetPath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\HWPublishing\ADMS";
        private static readonly string MoveFileSourcePath_Default = @"C:\ADMS\Client Archive";
        private static readonly string MoveFileTargetPath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\ADMS\Client Archive";
        private static readonly string ValidationRootPath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\";
        private static readonly string ClearTempPath_Default = @"C:\Users\Administrator\AppData\Local\Temp";
        private static readonly string StagnitoFTPPath_Default = "ftp://38.113.83.248/";
        private static readonly string StagnitoFTPUser_Default = "Stasha";
        private static readonly string StagnitoFTPPassword_Default = "Stasha";
        private static readonly string StagnitoDownloadTargetPath_Default = @"\\ftp.knowledgemarketing.com\LocalUser\Stagnito\ADMS\";

        public static string ClientArchivePath
        {
            get
            {
                return GetValueFromConfigOrDefault("ClientArchivePath", ClientArchivePath_Default);
            }
        }

        public static string SourceMediaPrinterFileExportPath
        {
            get
            {
                return GetValueFromConfigOrDefault("SourceMediaPrinterFileExportPath", SourceMediaPrinterFileExportPath_Default);
            }
        }

        public static string SAETransferSourcePath
        {
            get
            {
                return GetValueFromConfigOrDefault("SAETransferSourcePath", SAETransferSourcePath_Default);
            }
        }

        public static string SAETransferTargetPath
        {
            get
            {
                return GetValueFromConfigOrDefault("SAETransferTargetPath", SAETransferTargetPath_Default);
            }
        }

        public static string HWPublishingTransferSourcePath
        {
            get
            {
                return GetValueFromConfigOrDefault("HWPublishingTransferSourcePath", HWPublishingTransferSourcePath_Default);
            }
        }

        public static string HWPublishingTransferTargetPath
        {
            get
            {
                return GetValueFromConfigOrDefault("HWPublishingTransferTargetPath", HWPublishingTransferTargetPath_Default);
            }
        }

        public static string MoveFileSourcePath
        {
            get
            {
                return GetValueFromConfigOrDefault("MoveFileSourcePath", MoveFileSourcePath_Default);
            }
        }

        public static string MoveFileTargetPath
        {
            get
            {
                return GetValueFromConfigOrDefault("MoveFileTargetPath", MoveFileTargetPath_Default);
            }
        }

        public static string ValidationRootPath
        {
            get
            {
                return GetValueFromConfigOrDefault("ValidationRootPath", ValidationRootPath_Default);
            }
        }

        public static string ClearTempPath
        {
            get
            {
                return GetValueFromConfigOrDefault("ClearTempPath", ClearTempPath_Default);
            }
        }

        public static string StagnitoFTPPath
        {
            get
            {
                return GetValueFromConfigOrDefault("StagnitoFTPPath", StagnitoFTPPath_Default);
            }
        }

        public static string StagnitoFTPUser
        {
            get
            {
                return GetValueFromConfigOrDefault("StagnitoFTPUser", StagnitoFTPUser_Default);
            }
        }

        public static string StagnitoFTPPassword
        {
            get
            {
                return GetValueFromConfigOrDefault("StagnitoFTPPassword", StagnitoFTPPassword_Default);
            }
        }

        public static string StagnitoDownloadTargetPath
        {
            get
            {
                return GetValueFromConfigOrDefault("StagnitoDownloadTargetPath", StagnitoDownloadTargetPath_Default);
            }
        }

        // Utility Methods
        private static string GetValueFromConfigOrDefault(string key, string defaultValue)
        {
            var configValue = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(configValue))
            {
                configValue = defaultValue;
            }
            return configValue;
        }
    }
}
