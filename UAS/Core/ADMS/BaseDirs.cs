using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Core.ADMS
{
    public class BaseDirs
    {
        public static string getBaseDir()
        {
            return ADMS.Settings.BaseDir;
        }
        public static string getAppsDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseAppsDir);
        }
        public static string getApplicationReportingDir()
        {
            return createDirectory(getAppsDir(), ADMS.Settings.ApplicationReportingDir);
        }
        public static string getFtpDir()
        {
            bool useLocalFtpDirectory = false;
            bool.TryParse(ConfigurationManager.AppSettings["UseLocalFtpDirectory"].ToString(), out useLocalFtpDirectory);
            if (useLocalFtpDirectory == true)
                return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseFTPDir);
            else
                return createDirectory(string.Empty, ADMS.Settings.BaseFTPDir);
        }
        public static string getConfigDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseConfigurationDir);
        }
        public static string getLogDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseLogDir);
        }
        public static string getClientRepoDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseClientRepoDir);
        }
        public static string getClientStagingDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseClientStagingDir);
        }
        public static string getClientArchiveDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseClientArchiveDir);
        }
        public static string getClientFileResultEmail()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseClientFileResultEmailDir);
        }
        public static string getSuppressionDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseClientSuppressionDir);
        }
        public static string getClientExpectedSchemasDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseClientExpectedSchemasDir);
        }
        public static string getExportDataDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseExportDataDir);
        }
        public static string getExportDataUADDir()
        {
            return createDirectory(createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseExportDataDir), ADMS.Settings.BaseExportDataUADDir);
        }
        public static string getExportDataUASDir()
        {
            return createDirectory(createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseExportDataDir), ADMS.Settings.BaseExportDataUASDir);
        }
        public static string getExportDataCircDir()
        {
            return createDirectory(createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseExportDataDir), ADMS.Settings.BaseExportDataCircDir);
        }
        #region Fulfillment Directories
        public static string GetFulfillmentDir()
        {
            return createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseFulfillmentDir);
        }
        public static string GetFulfillmentZipDir()
        {
            return createDirectory(createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseFulfillmentDir), ADMS.Settings.BaseFulfillmentZipDir);
        }
        public static string GetFulfillmentZipAcsDir()
        {
            return createDirectory(createDirectory(createDirectory(ADMS.Settings.BaseDir, ADMS.Settings.BaseFulfillmentDir), ADMS.Settings.BaseFulfillmentZipDir), ADMS.Settings.BaseFulfillmentZipAcsDir);
        }
        #endregion
        public static string createDirectory(string dir1, string dir2)
        {
            if (!dir1.EndsWith("\\") && !dir2.StartsWith("\\"))
            {
                return dir1 + "\\" + dir2;
            }
            else if (dir1.EndsWith("\\") && dir2.StartsWith("\\"))
            {
                return dir1 + dir2.TrimStart('\\');
            }

            return dir1 + dir2;
        }
        public static Dictionary<string, string> GetBaseDirectories()
        {
            Dictionary<String, String> dirs = new Dictionary<String, String>();
            dirs.Add("Ftp", getFtpDir());
            dirs.Add("Log", getLogDir());
            dirs.Add("FileConfig", getClientExpectedSchemasDir());
            dirs.Add("Repo", getClientRepoDir());
            dirs.Add("Staging", getClientStagingDir());
            dirs.Add("Archive", getClientArchiveDir());
            dirs.Add("Applications", getAppsDir());
            dirs.Add("Reporting", getApplicationReportingDir());
            dirs.Add("Configuration", getConfigDir());
            dirs.Add("Suppression", getSuppressionDir());
            dirs.Add("ExportData", getExportDataDir());
            dirs.Add("ExportDataUAD", getExportDataUADDir());
            dirs.Add("ExportDataUAS", getExportDataUASDir());
            dirs.Add("ExportDataCirc", getExportDataCircDir());
            dirs.Add("FileResultEmail", getClientFileResultEmail());
            dirs.Add("Fulfillment",GetFulfillmentDir());
            dirs.Add("FulfillmentZip",GetFulfillmentZipDir());
            dirs.Add("ACS", GetFulfillmentZipAcsDir());
            return dirs;
        }

        public enum BaseDirectory
        {
            Applications,
            Archive,
            Ftp,
            Log,
            FileConfig,
            Repo,
            Staging,
            Configuration,
            ExportData,
            ExportDataUAD,
            ExportDataUAS,
            ExportDataCirc,
            FileResultEmail,
            Fulfillment,
            FulfillmentZip,
            ACS
        }
    }
}
