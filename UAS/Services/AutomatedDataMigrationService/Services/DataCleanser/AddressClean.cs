using Core.ADMS.Events;
using FrameworkUAS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADMS.Services.DataCleanser
{
    public class AddressClean : ServiceBase, IAddressClean
    {
        public event Action<FileProcessed> FileProcessed;
        public event Action<FileAddressGeocoded> FileAddressGeocoded;

        
        //Service Entry Point
        public void HandleFileValidated(FileValidated eventMessage)
        {
            try
            {
                if (eventMessage.SourceFile.IsDQMReady == true)
                {
                    if (eventMessage.IsValidFileType == true)
                    {
                        if (eventMessage.SourceFile.UseRealTimeGeocoding == true)
                        {
                            admsWrk.UpdateCurrentStep(eventMessage.AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Address_Clean, 1, "AddressClean", true, eventMessage.AdmsLog.SourceFileId);
                            try
                            {
                                // Validate address with existing in SubscriberFinal
                                ValidateAgainstExisting(eventMessage.Client, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileValidated",true,false);
                            }

                            try
                            {
                                // Address validation goes here
                                AddressStandardize(eventMessage.Client, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileValidated", true, false);
                            }
                        }
                        else
                            ConsoleMessage("Real time geocoding not enabled, skipping step.");
                    }
                }
                else
                    ConsoleMessage("Not ready for Address Validation....");
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileValidated", false, false);
            }
            finally
            {
                try
                {
                    FileAddressGeocoded geocoded = new FileAddressGeocoded(eventMessage.ImportFile, eventMessage.Client, eventMessage.IsKnownCustomerFileName, eventMessage.IsValidFileType,
                                                                            eventMessage.IsFileSchemaValid, eventMessage.SourceFile, eventMessage.AdmsLog, eventMessage.ValidationResult);

                    #region Call DQM in Single or Batch Mode
                    if (eventMessage.IsValidFileType == false)
                    {
                        FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
                        alWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                     FrameworkUAD_Lookup.Enums.FileStatusType.Completed,
                                     FrameworkUAD_Lookup.Enums.ADMS_StepType.Watching_for_File,
                                     FrameworkUAD_Lookup.Enums.ProcessingStatusType.Watching_for_File,
                                     FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_UAD_Import, 1, "Watching for File", true,
                                     eventMessage.AdmsLog.SourceFileId);

                        FileAddressGeocoded(geocoded);
                    }
                    else
                    {
                        ADMS.Services.Emailer.Emailer e = new Emailer.Emailer();
                        FileProcessed processed = new FileProcessed(eventMessage.Client, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog, eventMessage.ImportFile,
                                                                eventMessage.IsKnownCustomerFileName, eventMessage.IsValidFileType, eventMessage.IsFileSchemaValid, eventMessage.ValidationResult);
                        e.BackupReportBuilder(processed);

                        AddDQMReadyFile(geocoded);
                        //AddDQMReadyFile(geocoded);
                        //ADMSProcessingQue.RemoveClientFile(geocoded.Client, geocoded.ImportFile);
                    }
                    #endregion

                    

                    ThreadDictionary.Remove(eventMessage.ThreadId);
                }
                catch(Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileValidated_finally", true, false);
                }
            }
        }

        public static void AddDQMReadyFile(Core.ADMS.Events.FileAddressGeocoded eventMessage)
        {
            FrameworkUAS.Entity.AdmsLog admsLog = eventMessage.AdmsLog;
            string processCode = admsLog.ProcessCode;

            //CALL AND SAVE TO DB NOW
            bool isDemo = true;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

            FrameworkUAS.Entity.DQMQue q = new FrameworkUAS.Entity.DQMQue(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientID, isDemo, true, eventMessage.SourceFile.SourceFileID);
            FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
            dqmWorker.Save(q);
            eventMessage.AdmsLog.DQM = q;

            FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                         FrameworkUAD_Lookup.Enums.FileStatusType.Qued,
                         FrameworkUAD_Lookup.Enums.ADMS_StepType.Qued,
                         FrameworkUAD_Lookup.Enums.ProcessingStatusType.Qued,
                         FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DQM, 1, "Added file to DQM Que", true,
                         eventMessage.AdmsLog.SourceFileId);

        }

        public void ExecuteAddressCleanse(FrameworkUAS.Entity.AdmsLog admsLog, KMPlatform.Entity.Client client)
        {
            admsWrk.UpdateCurrentStep(admsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Address_Clean, 1, "AddressClean", true, admsLog.SourceFileId);
      
            FrameworkUAS.BusinessLogic.SourceFile sfWorker = new SourceFile();
            FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(admsLog.SourceFileId);

            if (sf.UseRealTimeGeocoding == true)
            {
                try
                {
                    // Standard Country and Region(State) cleanse
                    CountryRegionCleanse(sf.SourceFileID, admsLog.ProcessCode, client);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteAddressCleanse", false, false);
                }

                try
                {
                    // Validate address with existing in SubscriberFinal
                    ValidateAgainstExisting(client, sf.SourceFileID, admsLog.ProcessCode);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteAddressCleanse", false, false);
                }

                try
                {
                    // Address validation goes here
                    AddressStandardize(client, sf.SourceFileID, admsLog.ProcessCode);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteAddressCleanse", false, false);
                }
            }

        }

        #region Private Methods
        public void AddressStandardize(KMPlatform.Entity.Client client, int sourceFileId, string processCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType fsTypeName = FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_Cleansing)
        {
            BatchValidation(client, sourceFileId, fsTypeName, processCode);
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Address standardization is complete");
        }
        private void BatchValidation(KMPlatform.Entity.Client client, int sourceFileId, FrameworkUAD_Lookup.Enums.ProcessingStatusType fsTypeName, string processCode)
        {
            FrameworkUAD.BusinessLogic.SubscriberTransformed st = new FrameworkUAD.BusinessLogic.SubscriberTransformed();

            int batch = 25000;
            int totalCount = 0;
            int done = 0;
            int page = 1;

            if (fsTypeName == FrameworkUAD_Lookup.Enums.ProcessingStatusType.GeoCode)
                totalCount = st.CountForGeoCoding(client.ClientConnections, sourceFileId);
            else
            {
                totalCount = st.CountAddressValidation(client.ClientConnections, processCode, false);
            }

            int totalPages = 0;
            if (totalCount > 0 && totalCount < batch)
                totalPages = 1;
            else
            {
                totalPages = totalCount / batch;

                if (totalCount % batch > 0)
                    totalPages++;
            }
            if (totalCount > 0)
                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Total Addresses to Validate: " + totalCount.ToString());
            while (done < totalCount)
            {
                string line = "Page " + page.ToString() + " of " + totalPages.ToString();
                string backup = new string('\b', line.Length);
                Console.Write(backup);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(line);

                try
                {
                    List<FrameworkUAD.Entity.SubscriberTransformed> listDirty = new List<FrameworkUAD.Entity.SubscriberTransformed>();
                    if (fsTypeName != FrameworkUAD_Lookup.Enums.ProcessingStatusType.GeoCode)
                        listDirty = st.AddressValidation_Paging(page, batch, processCode, client.ClientConnections, false, sourceFileId).ToList();
                    else
                        listDirty = st.AddressValidation_Paging(page, batch, string.Empty, client.ClientConnections).ToList();

                    ValidateAddressList(listDirty, client, sourceFileId, processCode);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".BatchValidation", false, false);
                }
                done += batch;
                page++;
            }
        }
        private void ValidateAddressList(List<FrameworkUAD.Entity.SubscriberTransformed> listDirty, KMPlatform.Entity.Client client, int sourceFileId, string processCode)
        {
            List<FrameworkUAD.Entity.SubscriberTransformed> listSubscriberTransformed = new List<FrameworkUAD.Entity.SubscriberTransformed>();
            FrameworkUAD.BusinessLogic.SubscriberTransformed st = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
            Core_AMS.Utilities.AddressValidator addressValidator = new Core_AMS.Utilities.AddressValidator();
            List<Core_AMS.Utilities.AddressLocation> addressesToValidate = new List<Core_AMS.Utilities.AddressLocation>();
            string line = "";
            int rowCount = 0;
            var parser = new Core_AMS.Utilities.AddressParser();

            string addressLine;

            #region Prep list
            foreach (var a in listDirty)
            {
                try
                {
                    Core_AMS.Utilities.AddressLocation addressLocation = new Core_AMS.Utilities.AddressLocation();
                    addressLocation.Street = a.Address != null ? addressValidator.CleanAddress(a.Address) : string.Empty;
                    addressLocation.MailStop = a.MailStop != null ? a.MailStop.Replace("'", "") : string.Empty;
                    addressLocation.City = a.City != null ? a.City : string.Empty;
                    addressLocation.PostalCode = a.Zip != null ? a.Zip : string.Empty;
                    addressLocation.Region = a.State != null ? a.State : string.Empty;
                    if (addressLocation.PostalCode.Length == 4)
                        addressLocation.PostalCode = "0" + addressLocation.PostalCode;
                    addressLocation.SORecordIdentifier = a.SORecordIdentifier;// a.SORecordIdentifier != null ? a.SORecordIdentifier : Guid.NewGuid();
                    addressLocation.IsValid = false;

                    if (a.Country != null)
                    {
                        a.Country = a.Country.Trim();
                        if (a.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) && a.Plus4 != null)
                        {
                            if (a.Zip == null)
                                a.Zip = string.Empty;
                            if (a.Plus4 == null)
                                a.Plus4 = string.Empty;

                            if (a.Zip != null && a.Plus4 != null)
                            {
                                if (a.Zip.Length == 3 && a.Plus4.Length == 3)
                                    a.Zip = a.Zip + " " + a.Plus4;
                            }
                        }
                    }
                    else
                        a.Country = string.Empty;

                    if (addressLocation.PostalCode.Length == 4)
                        addressLocation.PostalCode = "0" + addressLocation.PostalCode;
                    else if (addressLocation.PostalCode.Length > 7)
                    {
                        a.Zip = a.Zip.Replace("-", "");
                        if (a.Zip.Length >= 5 && a.Zip.Length < 9)
                            addressLocation.PostalCode = a.Zip.Substring(0, 5);
                        else
                            addressLocation.PostalCode = a.Zip;

                        if (a.Zip.Length == 9)
                            addressLocation.PostalCodePlusFour = a.Zip.Substring(5, 4);
                    }
                    addressLocation.RecordIdentifier = a.SubscriberTransformedID;
                    addressLocation.IsValid = false;

                    // Parse address out and put mailstop data into mailstop column otherwise it will be lost when the address is
                    // sent to validation
                    if (string.IsNullOrEmpty(addressLocation.MailStop))
                    {
                        addressLine = addressLocation.Street + "," + addressLocation.City + " " + addressLocation.Region + " " + addressLocation.PostalCode.Replace(" ", "");
                        var result = parser.ParseAddress(addressLine);

                        if (result != null && !string.IsNullOrEmpty(result.SecondaryUnit) && !string.IsNullOrEmpty(result.SecondaryNumber))
                            addressLocation.MailStop = result.SecondaryUnit + " " + result.SecondaryNumber;
                    }

                    if (addressLocation.Street.ToUpper().Contains("PO BOX") || addressLocation.Street.ToUpper().Contains("P O BOX") || addressLocation.Street.ToUpper().Contains("BOX") || addressLocation.Street.ToUpper().Contains("PO") || addressLocation.Street.ToUpper().Contains("&"))
                        addressLocation.Street = string.Empty;

                    if (!addressLocation.Street.Equals(null))
                    {
                        if (!string.IsNullOrEmpty(addressLocation.Street))
                            addressesToValidate.Add(addressLocation);
                        else if (!string.IsNullOrEmpty(addressLocation.PostalCode))
                            addressesToValidate.Add(addressLocation);
                    }
                    else if (!string.IsNullOrEmpty(addressLocation.PostalCode))
                        addressesToValidate.Add(addressLocation);

                    rowCount++;

                    line = string.Format("Records Validated: {0}", rowCount);
                    string backup = new string('\b', line.Length);
                    Console.Write(backup);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ValidateAddressList");
                }
            }
            #endregion
            Console.WriteLine(string.Empty);
            #region validate
            Console.WriteLine("START: Address Validation - " + DateTime.Now.ToString());
            try
            {
                addressesToValidate = addressValidator.ValidateAddresses(addressesToValidate, true, true, false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ValidateAddressList");
            }
            Console.WriteLine("DONE: Address Validation - " + DateTime.Now.ToString());
            #endregion
            #region prep SubTransformed list
            foreach (Core_AMS.Utilities.AddressLocation al in addressesToValidate)
            {
                try
                {
                    listSubscriberTransformed.Add(new FrameworkUAD.Entity.SubscriberTransformed
                    {
                        Address = al.Street.ToString(),
                        MailStop = al.MailStop,
                        City = al.City,
                        State = al.Region,
                        Zip = al.PostalCode,
                        Plus4 = al.PostalCodePlusFour,
                        SORecordIdentifier = al.SORecordIdentifier,
                        Latitude = Convert.ToDecimal(al.Latitude),
                        Longitude = Convert.ToDecimal(al.Longitude),
                        IsLatLonValid = al.IsValid,
                        LatLonMsg = al.ValidationMessage,
                        Country = al.Country,
                        County = al.County
                    });
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ValidateAddressList");
                }
            }
            #endregion
            #region update database
            // Validate isMailable
            foreach (var x in listSubscriberTransformed)
            {
                //if (x.IsLatLonValid == true)
                //    x.IsMailable = true;
                //else
                //    x.IsMailable = false;

                if (x.Address == null) x.Address = string.Empty;
                if (x.MailStop == null) x.MailStop = string.Empty;
                if (x.City == null) x.City = string.Empty;
                if (x.State == null) x.State = string.Empty;
                if (x.Zip == null) x.Zip = string.Empty;
                if (x.Plus4 == null) x.Plus4 = string.Empty;
                if (x.Country == null) x.Country = string.Empty;
                if (x.County == null) x.County = string.Empty;
                if (x.LatLonMsg == null) x.LatLonMsg = string.Empty;
            }

            // Send list back to database for update
            ConsoleMessage("\n" + DateTime.Now.TimeOfDay.ToString() + " - Start Send data back to database for update");
            if (listSubscriberTransformed.Count > 0)
            {
                try
                {
                    st.AddressUpdateBulkSql(listSubscriberTransformed, client.ClientConnections);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ValidateAddressList");
                }
            }
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done Send data back to database for update");
            #endregion
        }
        public void CountryRegionCleanse(int sourceFileID, string processCode, KMPlatform.Entity.Client client)
        {
            FrameworkUAD_Lookup.BusinessLogic.Country cData = new FrameworkUAD_Lookup.BusinessLogic.Country();
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start country region cleanse");
            cData.CountryRegionCleanse(sourceFileID, processCode, client.ClientConnections);
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done country region cleanse");
        }
        private void ValidateAgainstExisting(KMPlatform.Entity.Client client, int sourceFileID, string processCode)
        {
            FrameworkUAD.BusinessLogic.SubscriberTransformed vae = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start validate against existing data");
            vae.AddressValidateExisting(client.ClientConnections, sourceFileID, processCode);
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done  validate against existing data");
        }
        private void ExecuteClientCustomProcsBeforeAddressValidation(List<FrameworkUAS.Entity.ClientCustomProcedure> list, int sourceFileID, string fileName, KMPlatform.Entity.Client client, string processCode)
        {
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start execute client custom procs before Address Validation");
            FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpData = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
            foreach (FrameworkUAS.Entity.ClientCustomProcedure ccp in list)
            {
                ccpData.ExecuteSproc(ccp.ProcedureName, sourceFileID, fileName, client, processCode);
            }
            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done execute client custom procs before Address Validation");
        }
        #endregion
    }
}
