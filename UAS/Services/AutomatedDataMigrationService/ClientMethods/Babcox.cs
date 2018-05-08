using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Entity;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using System.Data;
using System.Text;
using KM.Common.Import;
using Enums = KM.Common.Enums;

namespace ADMS.ClientMethods
{
    public class Babcox : ClientSpecialCommon
    {
        List<string> columnHeaders;

        public void ProcessMultiResponseData(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            //take existing file - do split into rows - create new file - go back into processing
            //
            #region parse file to DataTable
            FileConfiguration fc = new FileConfiguration();
            fc.FileExtension = ".csv";
            fc.FileColumnDelimiter = Enums.ColumnDelimiter.tab.ToString();
            fc.IsQuoteEncapsulated = false;

            DataTable dt = FileImporter.LoadFile(eventMessage.ImportFile, fc);
            Console.WriteLine("Table rows:" + dt.Rows.Count.ToString());
            #endregion

            Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
            StringBuilder sb = new StringBuilder();

            #region create the headers and NEW file
            columnHeaders = new List<string>();

            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append("\"" + dc.ColumnName + "\"" + ",");
                columnHeaders.Add(dc.ColumnName);
            }
            Console.WriteLine("Column Headers:" + columnHeaders.Count.ToString());
            StringBuilder final = new StringBuilder();
            final.Append(sb.ToString().TrimEnd(','));
            final.Append(System.Environment.NewLine);
            ff.CreateAppendFile(eventMessage.ImportFile.DirectoryName + "\\" + eventMessage.ImportFile.Name.Replace(eventMessage.ImportFile.Extension, "").ToString() + "_NEW" + eventMessage.ImportFile.Extension, final.ToString());
            #endregion

            int totalRows = dt.Rows.Count;
            int counter = 1;
            //bool sentError = false;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    Console.WriteLine(counter.ToString() + " of " + totalRows.ToString());
                    sb = new StringBuilder();
                    if (eventMessage.ImportFile.Name.Contains("CM KM DATE FILE"))
                        sb = CmKmDateFile(dr);
                    else if (eventMessage.ImportFile.Name.Contains("TG KM DATE FILE"))
                        sb = TgKmDateFile(dr);
                    else if (eventMessage.ImportFile.Name.Contains("MPN KM DATE FILE"))
                        sb = MpnKmDateFile(dr);
                    ff.CreateAppendFile(eventMessage.ImportFile.DirectoryName + "\\" + eventMessage.ImportFile.Name.Replace(eventMessage.ImportFile.Extension, "").ToString() + "_NEW" + eventMessage.ImportFile.Extension, sb.ToString());
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessMultiResponseData");
                }
                counter++;
            }

            eventMessage.SourceFile.Delimiter = "comma";
            eventMessage.SourceFile.IsTextQualifier = true;

            //delete original file
            System.IO.File.Delete(eventMessage.ImportFile.FullName);
            //rename new file to original
            System.IO.File.Copy(eventMessage.ImportFile.DirectoryName + "\\" + eventMessage.ImportFile.Name.Replace(eventMessage.ImportFile.Extension, "").ToString() + "_NEW" + eventMessage.ImportFile.Extension, eventMessage.ImportFile.FullName);
            //delete the temp NEW file
            System.IO.File.Delete(eventMessage.ImportFile.DirectoryName + "\\" + eventMessage.ImportFile.Name.Replace(eventMessage.ImportFile.Extension, "").ToString() + "_NEW" + eventMessage.ImportFile.Extension);
            //continue ADMS process
            ADMS.Services.Validator.Validator val = new Services.Validator.Validator();
            KMPlatform.BusinessLogic.ServiceFeature sfData = new KMPlatform.BusinessLogic.ServiceFeature();
            KMPlatform.Entity.ServiceFeature serviceFeature = sfData.SelectServiceFeature(eventMessage.SourceFile.ServiceFeatureID);
            val.ProcessFileAsObject(eventMessage, false, serviceFeature);
        }
        private StringBuilder CmKmDateFile(DataRow dr)
        {
            //CM KM DATE FILE.csv -PROGRAMGROUP
            string values = dr["PROGRAMGROUP"].ToString().Trim();
            StringBuilder sbReturn = new StringBuilder();
            if (values.Length > 0)
            {
                foreach (char v in values)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ch in columnHeaders)
                    {
                        if (!ch.Equals("PROGRAMGROUP", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                        else
                            sb.Append("\"" + v + "\"" + ",");
                    }
                    sbReturn.AppendLine(sb.ToString().TrimEnd(','));
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string ch in columnHeaders)
                {
                    sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                }
                sbReturn.AppendLine(sb.ToString().TrimEnd(','));
            }

            return sbReturn;
        }
        private StringBuilder TgKmDateFile(DataRow dr)
        {
            //TG KM DATE FILE.csv - ICBRANDSSPECIALIZED and ICBRANDSREPAIRED
            string specialValues = dr["ICBRANDSSPECIALIZED"].ToString().Trim();
            string repairValues = dr["ICBRANDSREPAIRED"].ToString().Trim();


            StringBuilder sbReturn = new StringBuilder();
            if (specialValues.Length == 0 && repairValues.Length == 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string ch in columnHeaders)
                {
                    sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                }
                sbReturn.AppendLine(sb.ToString().TrimEnd(','));
            }
            else if (specialValues.Length > 0 && repairValues.Length == 0)
            {
                foreach (char v in specialValues)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ch in columnHeaders)
                    {
                        if (!ch.Equals("ICBRANDSSPECIALIZED", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                        else
                            sb.Append("\"" + v + "\"" + ",");
                    }
                    sbReturn.AppendLine(sb.ToString().TrimEnd(','));
                }
            }
            else if (specialValues.Length == 0 && repairValues.Length > 0)
            {
                foreach (char v in repairValues)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ch in columnHeaders)
                    {
                        if (!ch.Equals("ICBRANDSREPAIRED", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                        else
                            sb.Append("\"" + v + "\"" + ",");
                    }
                    sbReturn.AppendLine(sb.ToString().TrimEnd(','));
                }
            }
            else
            {
                foreach (char v in specialValues)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ch in columnHeaders)
                    {
                        if (!ch.Equals("ICBRANDSSPECIALIZED", StringComparison.CurrentCultureIgnoreCase) && !ch.Equals("ICBRANDSREPAIRED", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                        else if (ch.Equals("ICBRANDSREPAIRED", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + "\"" + ",");//just a blank
                        else
                            sb.Append("\"" + v + "\"" + ",");//specialValue
                    }
                    sbReturn.AppendLine(sb.ToString().TrimEnd(','));
                }

                foreach (char v in repairValues)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ch in columnHeaders)
                    {
                        if (!ch.Equals("ICBRANDSSPECIALIZED", StringComparison.CurrentCultureIgnoreCase) && !ch.Equals("ICBRANDSREPAIRED", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                        else if (ch.Equals("ICBRANDSSPECIALIZED", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + "\"" + ",");//just a blank
                        else
                            sb.Append("\"" + v + "\"" + ",");//repairValue
                    }
                    sbReturn.AppendLine(sb.ToString().TrimEnd(','));
                }
            }

            return sbReturn;
        }
        private StringBuilder MpnKmDateFile(DataRow dr)
        {
            //MPN KM DATE FILE.csv - FRANCHISE
            string values = dr["FRANCHISES"].ToString().Trim();
            StringBuilder sbReturn = new StringBuilder();
            if (values.Length > 0)
            {
                foreach (char v in values)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ch in columnHeaders)
                    {
                        if (!ch.Equals("FRANCHISES", StringComparison.CurrentCultureIgnoreCase))
                            sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                        else
                            sb.Append("\"" + v + "\"" + ",");
                    }
                    sbReturn.AppendLine(sb.ToString().TrimEnd(','));
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string ch in columnHeaders)
                {
                    sb.Append("\"" + dr[ch].ToString() + "\"" + ",");
                }
                sbReturn.AppendLine(sb.ToString().TrimEnd(','));
            }

            return sbReturn;
        }
    }
}
