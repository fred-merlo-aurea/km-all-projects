using System;
using System.Configuration;
using System.Linq;

namespace Core.ADMS
{
    public static class Settings
    {
        private static object _ftpNetworkPathSingletonLock = new object();

        static string  _EmailFrom = "KMFTP@TeamKM.com";
        public static string EmailFrom
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailFrom"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailFrom"].ToString()))
                    return ConfigurationManager.AppSettings["EmailFrom"].ToString();
                else
                    return _EmailFrom;
            }
            set
            {
                _EmailFrom = value;
            }
        }

        public static string AdminEmail
        {
            get
            {
                string adminEmail = "platform-services@TeamKM.com";
                if (ConfigurationManager.AppSettings["AdminEmail"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["AdminEmail"].ToString()))
                    adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
                return adminEmail;
            }
        }

        public static string WebFormEmail
        {
            get
            {
                string adminEmail = "platform-services@TeamKM.com";
                if (ConfigurationManager.AppSettings["WebFormEmail"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebFormEmail"].ToString()))
                    adminEmail = ConfigurationManager.AppSettings["WebFormEmail"];
                return adminEmail;
            }
        }

        static string _smtp = "108.160.208.101";
        public static string SMTP
        {
            get
            {
                if (ConfigurationManager.AppSettings["MailServer"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServer"].ToString()))
                    return ConfigurationManager.AppSettings["MailServer"].ToString();
                else
                    return _smtp;
            }
            set
            {
                _smtp = value;
            }
        }

        public static string WatcherConfig
        {
            get
            {
                return "WatcherConfig.xml";
            }
        }

        static string _baseDir = "C:\\ADMS";
        public static string BaseDir
        {
            get
            {
                return _baseDir;
            }
            set
            {
                _baseDir = value;
            }
        }

        public static string BaseFulfillmentDir
        {
            get
            {
                return "Fulfillment";
            }
        }
        public static string BaseFulfillmentZipDir
        {
            get
            {
                return "Zip Files";
            }
        }
        public static string BaseFulfillmentZipAcsDir
        {
            get
            {
                return "ACS";
            }
        }

        public static string BaseAppsDir
        {
            get
            {
                return "Applications";
            }
        }

        private static string _FTPNetworkPath;
        public static string FTPNetworkPath
        {
            get
            {
                //double-check lock singleton initialization, not the best one
                if (string.IsNullOrWhiteSpace(_FTPNetworkPath))
                {
                    lock(_ftpNetworkPathSingletonLock)
                    {
                        if(string.IsNullOrWhiteSpace(_FTPNetworkPath))
                        {
                            var ftpNetworkPath = ConfigurationManager.AppSettings["FTPNetworkPath"];
                            if (string.IsNullOrEmpty(ftpNetworkPath))
                            {
                                ftpNetworkPath = "10.181.1.146";
                            }
                            if (!ftpNetworkPath.EndsWith("\\"))
                            {
                                ftpNetworkPath += "\\";
                            }
                            ftpNetworkPath = @"\\" + ftpNetworkPath;
                            //Change the filed once, it is singleton, concurrent processes may corrupt data if doing operations other than field initialization
                            _FTPNetworkPath = ftpNetworkPath;
                        }
                    }
                }
                return _FTPNetworkPath;
            }
        }

        public static string BaseFTPDir
        {
            get
            {
                bool useLocalFTP = false;
                bool.TryParse(ConfigurationManager.AppSettings["UseLocalFtpDirectory"].ToString(), out useLocalFTP);
                if (useLocalFTP == true)
                    return "ftp_repository";
                else
                {
                    bool isNetworkDeployed = false;
                    bool.TryParse(ConfigurationManager.AppSettings["IsNetworkDeployed"].ToString(), out isNetworkDeployed);
                    if (!isNetworkDeployed)
                    {
                        return "ftp_repository";
                    }
                    return FTPNetworkPath + @"LocalUser\";
                }
                    
            }
        }

        public static string BaseLogDir
        {
            get
            {
                return "Logs";
            }
        }

        public static string BaseClientExpectedSchemasDir
        {
            get
            {
                return "Expected Client Schemas";
            }
        }
        public static string BaseClientRepoDir
        {
            get
            {
                return "Client Repositories";
            }
        }

        public static string BaseClientStagingDir
        {
            get
            {
                return "Client Staging";
            }
        }

        public static string BaseClientArchiveDir
        {
            get
            {
                return "Client Archive";
            }
        }
        public static string BaseClientFileResultEmailDir
        {
            get
            {
                return "Client File Result Emails";
            }
        }
        public static string BaseClientSuppressionDir
        {
            get
            {
                return "Client Suppression";
            }
        }
        public static string BaseArchiveProcessedDir
        {
            get
            {
                return "Processed";
            }
        }

        public static string BaseArchiveInvalidDir
        {
            get
            {
                return "Invalid";
            }
        }

        public static string MessageFilePath
        {
            get
            {
                return "C:\\ADMS\\Configuration";
            }
        }

        public static string FileSchemaAssociation
        {
            get
            {
                return "C:\\ADMS\\Configuration\\FileSchemaAssociation.xml";
            }
        }

        public static string BaseConfigurationDir
        {
            get
            {
                return "Configuration";
            }
        }

        public static string MoverConfigPath
        {
            get
            {
                return @"C:\ADMS\Configuration\FileMover";
            }
        }
        public static string MoverConfigFile
        {
            get
            {
                return "MoverConfig.xml";
            }
        }

        public static string BaseExportDataDir
        {
            get
            {
                return "Export Data";
            }
        }
        public static string BaseExportDataUADDir
        {
            get
            {
                return "UAD";
            }
        }
        public static string BaseExportDataUASDir
        {
            get
            {
                return "UAS";
            }
        }
        public static string BaseExportDataCircDir
        {
            get
            {
                return "Circ";
            }
        }
        public static string ApplicationReportingDir
        {
            get
            {
                return "Reporting";
            }
        }
    }
}
