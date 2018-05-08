using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using KM.Common;
using KM.Common.Utilities.Email;

namespace AMS_Geocoding
{
    class Program
    {
        #region variables
        readonly string engine = "Geocoding";
        readonly FrameworkUAS.BusinessLogic.EngineLog elWrk = new FrameworkUAS.BusinessLogic.EngineLog();
        readonly KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Geocoding;
        readonly KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //FrameworkUAS.Entity.EngineLog _engineLog;
        //FrameworkUAS.Entity.EngineLog engineLog
        //{
        //    get
        //    {
        //        if (_engineLog == null)
        //            _engineLog = elWrk.Select(client.ClientID, engine);
        //        return _engineLog;
        //    }
        //    set
        //    {
        //        _engineLog = value;
        //    }
        //}
        //KMPlatform.Entity.Client _client;
        //KMPlatform.Entity.Client client
        //{
        //    get
        //    {
        //        if (_client == null)
        //        {
        //            KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
        //            _client = cWrk.SelectFtpFolder(ConfigurationManager.AppSettings["EngineClient"].ToString(), true);
        //        }
        //        return _client;
        //    }
        //    set
        //    {
        //        _client = value;
        //    }
        //}
        #endregion
        static void Main(string[] args)
        {
            Program p = new Program();
            //string text = "!@#$%^&*()_+<,>./[]{}|:'\"?><=  abcdefghijklmnopqrstuvwxyz- ABCDEFGHIJKLMNOPQRSTUVWXYZ- 0123456789";
            //string clean = Core_AMS.Utilities.StringFunctions.Allow_Numbers_Letters_Spaces_Dashes(text);
            try
            {
                p.StartGeocoding();
            }
            catch(Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Geocoding;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, "Main", app, string.Empty);
            }
        }
        List<FrameworkUAD.Entity.Subscription> TruncateStrings(List<FrameworkUAD.Entity.Subscription> subscriptions)
        {
            foreach (FrameworkUAD.Entity.Subscription x in subscriptions)
            {
                #region truncate strings
                if (x.FName != null && x.FName.Length > 100)
                    x.FName = x.FName.Substring(0, 100);
                if (x.LName != null && x.LName.Length > 100)
                    x.LName = x.LName.Substring(0, 100);
                if (x.Title != null && x.Title.Length > 100)
                    x.Title = x.Title.Substring(0, 100);
                if (x.Company != null && x.Company.Length > 100)
                    x.Company = x.Company.Substring(0, 100);
                if (x.Address != null && x.Address.Length > 255)
                    x.Address = x.Address.Substring(0, 255);
                if (x.MailStop != null && x.MailStop.Length > 255)
                    x.MailStop = x.MailStop.Substring(0, 255);
                if (x.City != null && x.City.Length > 50)
                    x.City = x.City.Substring(0, 50);
                if (x.State != null && x.State.Length > 50)
                    x.State = x.State.Substring(0, 50);
                if (x.Zip != null && x.Zip.Length > 10)
                    x.Zip = x.Zip.Substring(0, 10);
                if (x.Plus4 != null && x.Plus4.Length > 4)
                    x.Plus4 = x.Plus4.Substring(0, 4);
                if (x.ForZip != null && x.ForZip.Length > 50)
                    x.ForZip = x.ForZip.Substring(0, 50);
                if (x.County != null && x.County.Length > 20)
                    x.County = x.County.Substring(0, 20);
                if (x.Country != null && x.Country.Length > 100)
                    x.Country = x.Country.Substring(0, 100);
                if (x.Email != null && x.Email.Length > 100)
                    x.Email = x.Email.Substring(0, 100);
                if (x.RegCode != null && x.RegCode.Length > 5)
                    x.RegCode = x.RegCode.Substring(0, 5);
                if (x.Verified != null && x.Verified.Length > 100)
                    x.Verified = x.Verified.Substring(0, 100);
                if (x.SubSrc != null && x.SubSrc.Length > 8)
                    x.SubSrc = x.SubSrc.Substring(0, 8);
                if (x.OrigsSrc != null && x.OrigsSrc.Length > 8)
                    x.OrigsSrc = x.OrigsSrc.Substring(0, 8);
                if (x.Par3C != null && x.Par3C.Length > 1)
                    x.Par3C = x.Par3C.Substring(0, 1);
                if (x.Source != null && x.Source.Length > 50)
                    x.Source = x.Source.Substring(0, 50);
                if (x.Priority != null && x.Priority.Length > 4)
                    x.Priority = x.Priority.Substring(0, 4);
                if (x.Sic != null && x.Sic.Length > 8)
                    x.Sic = x.Sic.Substring(0, 8);
                if (x.SicCode != null && x.SicCode.Length > 20)
                    x.SicCode = x.SicCode.Substring(0, 20);
                if (x.Gender != null && x.Gender.Length > 1024)
                    x.Gender = x.Gender.Substring(0, 1024);
                if (x.IGrp_Rank != null && x.IGrp_Rank.Length > 2)
                    x.IGrp_Rank = x.IGrp_Rank.Substring(0, 2);
                if (x.CGrp_Rank != null && x.CGrp_Rank.Length > 2)
                    x.CGrp_Rank = x.CGrp_Rank.Substring(0, 2);
                if (x.Address3 != null && x.Address3.Length > 255)
                    x.Address3 = x.Address3.Substring(0, 255);
                if (x.Home_Work_Address != null && x.Home_Work_Address.Length > 10)
                    x.Home_Work_Address = x.Home_Work_Address.Substring(0, 10);
                if (x.PubIDs != null && x.PubIDs.Length > 2000)
                    x.PubIDs = x.PubIDs.Substring(0, 2000);
                if (x.Demo7 != null && x.Demo7.Length > 1)
                    x.Demo7 = x.Demo7.Substring(0, 1);
                if (x.LatLonMsg != null && x.LatLonMsg.Length > 500)
                    x.LatLonMsg = x.LatLonMsg.Substring(0, 500);
                #endregion
            }
            return subscriptions;
        }
        void StartGeocoding()
        {
            //get list of clients from config file
            KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
            List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();
            List<string> engineClients = ConfigurationManager.AppSettings["EngineClients"].Split(',').ToList();
            foreach (string c in engineClients)
                clients.Add(cWorker.SelectFtpFolder(c, false));

            foreach (KMPlatform.Entity.Client c in clients)
            {
                StringBuilder clientErrors = new StringBuilder();
                try
                {
                     FrameworkUAS.Entity.EngineLog engineLog = elWrk.Select(c.ClientID, engine);

                    elWrk.UpdateIsRunning(engineLog.EngineLogId, true);
                    FrameworkUAD.BusinessLogic.Subscription subWorker = new FrameworkUAD.BusinessLogic.Subscription();
                    List<FrameworkUAD.Entity.Subscription> subscriptions = subWorker.SelectInValidAddresses(c.ClientConnections, c.DisplayName);
                    //subscriptions = TruncateStrings(subscriptions);

                    //batch this 10k per batch
                    int batchSize = 100;
                    //Set Config Batch Size
                    int.TryParse(ConfigurationManager.AppSettings["BatchSize"].ToString(), out batchSize);
                    if (!(batchSize > 0))
                        batchSize = 100;
                    int total = subscriptions.Count;
                    int counter = 0;
                    int processedCount = 0;

                    elWrk.UpdateIsRunning(engineLog.EngineLogId, true, "Subscription Count: " + total.ToString());

                    List<FrameworkUAD.Entity.Subscription> batchList = new List<FrameworkUAD.Entity.Subscription>();
                    bool logged = false;
                   int validCount = 0;
                    int invalidCount = 0;
                    foreach (FrameworkUAD.Entity.Subscription x in subscriptions)
                    {
                        counter++;
                        processedCount++;
                        batchList.Add(x);
                        if (processedCount == total || counter == batchSize)
                        {
                            string line = "Processing " + processedCount.ToString() + " of " + total.ToString();
                            elWrk.UpdateIsRunning(engineLog.EngineLogId, true, line);

                            StringBuilder sbXML = new StringBuilder();
                            try
                            {
                                batchList = ValidateAddressList(batchList);
                                batchList = TruncateStrings(batchList);
                                validCount += batchList.Count(y => y.IsLatLonValid == true);
                                invalidCount += batchList.Count(y => y.IsLatLonValid == false);
                                                                
                                sbXML.AppendLine("<XML>");
                                foreach (FrameworkUAD.Entity.Subscription s in batchList)
                                {
                                    sbXML.AppendLine("<Subscriber>");

                                    sbXML.AppendLine("<SubscriptionID>" + s.SubscriptionID.ToString() + "</SubscriptionID>");
                                    sbXML.AppendLine("<Address>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.Address.Trim()) + "</Address>");
                                    sbXML.AppendLine("<MailStop>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.MailStop.Trim()) + "</MailStop>");
                                    sbXML.AppendLine("<City>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.City.Trim()) + "</City>");
                                    sbXML.AppendLine("<State>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.State.Trim()) + "</State>");
                                    sbXML.AppendLine("<Zip>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.Zip.Trim()) + "</Zip>");
                                    sbXML.AppendLine("<Plus4>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.Plus4.Trim()) + "</Plus4>");
                                    sbXML.AppendLine("<Latitude>" + s.Latitude.ToString() + "</Latitude>");
                                    sbXML.AppendLine("<Longitude>" + s.Longitude.ToString() + "</Longitude>");
                                    sbXML.AppendLine("<IsLatLonValid>" + s.IsLatLonValid.ToString() + "</IsLatLonValid>");
                                    sbXML.AppendLine("<LatLongMsg>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.LatLonMsg.Trim()) + "</LatLongMsg>");
                                    sbXML.AppendLine("<Country>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.Country.Trim()) + "</Country>");
                                    sbXML.AppendLine("<County>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(s.County.Trim()) + "</County>");

                                    sbXML.AppendLine("</Subscriber>");
                                }
                                sbXML.AppendLine("</XML>");

                                //Core_AMS.ADMS.Logging log = new Core_AMS.ADMS.Logging();
                                //log.LogIssue(sbXML.ToString());

                                subWorker.AddressUpdate(sbXML.ToString(), c.ClientConnections);
                            }
                            catch (Exception ex)
                            {
                                if (logged == false)
                                {
                                    logged = true;
                                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Geocoding;
                                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".StartGeocoding", app, string.Empty);
                                }
                            }

                            counter = 0;
                            batchList = new List<FrameworkUAD.Entity.Subscription>();
                        }
                    }
                    string result = "Valid:" + validCount.ToString() + " - Bad:" + invalidCount.ToString();
                    elWrk.UpdateIsRunning(engineLog.EngineLogId, true, result);
                }
                catch (Exception ex)
                {
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Geocoding;
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".StartGeocoding", app, string.Empty);
                }

                if (clientErrors.Length > 0)
                {
                    var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                    emailService.SendEmail(clientErrors.ToString());
                }
            }
        }
        private List<FrameworkUAD.Entity.Subscription> ValidateAddressList(List<FrameworkUAD.Entity.Subscription> listDirty)
        {
            List<FrameworkUAD.Entity.Subscription> listSubs = new List<FrameworkUAD.Entity.Subscription>();
            Core_AMS.Utilities.AddressValidator addressValidator = new Core_AMS.Utilities.AddressValidator();
            List<Core_AMS.Utilities.AddressLocation> addressesToValidate = new List<Core_AMS.Utilities.AddressLocation>();
            string line = "";
            int rowCount = 0;
            var parser = new Core_AMS.Utilities.AddressParser();

            string addressLine;
            bool logged = false;
            #region Prep list
            foreach (var a in listDirty)
            {
                try
                {
                    Core_AMS.Utilities.AddressLocation addressLocation = new Core_AMS.Utilities.AddressLocation();
                    addressLocation.Street = a.Address != null ? addressValidator.CleanAddress(a.Address) : string.Empty;
                    addressLocation.MailStop = a.MailStop != null ? a.MailStop.Replace("'","") : string.Empty;
                    addressLocation.City = a.City != null ? a.City : string.Empty;
                    addressLocation.PostalCode = a.Zip != null ? a.Zip : string.Empty;
                    addressLocation.Region = a.State != null ? a.State : string.Empty;
                    addressLocation.Country = a.Country != null ? a.Country : string.Empty;

                    if (!String.IsNullOrWhiteSpace(addressLocation.Region) &&
                        !addressValidator.GetUsRegions().Contains(addressLocation.Region) &&
                        !addressValidator.GetCanadaRegions().Contains(addressLocation.Region))
                    {
                        addressLocation.Region = String.Empty;
                    }

                    if (a.Country != null)
                    {
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

                        if(a.Zip.Length == 9)
                            addressLocation.PostalCodePlusFour = a.Zip.Substring(5, 4);
                    }  
                    else if (addressLocation.PostalCode.Length == 5)
                    {
                        if(addressLocation.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) 
                            || addressLocation.Country.Equals("UNITED STATES OF AMERICA", StringComparison.CurrentCultureIgnoreCase)
                            || addressLocation.Country.Equals("USA", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (addressLocation.PostalCode.StartsWith("000"))
                                addressLocation.PostalCode = string.Empty;
                            else
                            {
                                string zipCheck = Core_AMS.Utilities.StringFunctions.GetNumbersOnly(addressLocation.PostalCode);
                                if (zipCheck.Length < 5)
                                    addressLocation.PostalCode = string.Empty;
                            }
                        }
                    }
                    addressLocation.RecordIdentifier = a.SubscriptionID;
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

                    if (addressLocation.Street.ToUpper().StartsWith("PO BOX") || addressLocation.Street.ToUpper().StartsWith("P O BOX") || addressLocation.Street.ToUpper().StartsWith("BOX ") || addressLocation.Street.ToUpper().StartsWith("PO ") || addressLocation.Street.ToUpper().Contains("&"))
                        addressLocation.Street = string.Empty;

                    if (!addressLocation.Street.Equals(null))
                    {
                        if (!string.IsNullOrEmpty(addressLocation.Street))
                            addressesToValidate.Add(addressLocation);
                        else if (!string.IsNullOrEmpty(addressLocation.PostalCode))
                            addressesToValidate.Add(addressLocation);
                    }
                    else if(!string.IsNullOrEmpty(addressLocation.PostalCode))
                        addressesToValidate.Add(addressLocation);

                    rowCount++;

                    line = string.Format("Records Validated: {0}", rowCount);
                    string backup = new string('\b', line.Length);
                    Console.Write(backup);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);
                }
                catch(Exception ex)
                {
                    //string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    if(logged == false)
                    {
                        logged = true;
                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Geocoding;
                        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ValidateAddressList", app, string.Empty);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Empty);
            #region validate
            Console.WriteLine("START: Address Validation - " + DateTime.Now.ToString());
            if (addressesToValidate.Count > 0)
            {
                try
                {
                    bool useGoogleBing = false;
                    bool.TryParse(ConfigurationManager.AppSettings["UseGoogleBing"].ToString(), out useGoogleBing);

                    addressesToValidate = addressValidator.ValidateAddresses(addressesToValidate, true, true, useGoogleBing).ToList();
                    Console.WriteLine(System.Environment.NewLine);
                    Console.WriteLine("Error count - " + addressesToValidate.Count(x => x.ErrorOccured == true).ToString());
                }
                catch { }
            }
            Console.WriteLine("DONE: Address Validation - " + DateTime.Now.ToString());
            #endregion
            #region prep SubTransformed list
            foreach (Core_AMS.Utilities.AddressLocation al in addressesToValidate.Where(x => x.ErrorOccured == false))
            {
                FrameworkUAD.Entity.Subscription oldData = listDirty.Single(x => x.SubscriptionID == al.RecordIdentifier);

                FrameworkUAD.Entity.Subscription sub = new FrameworkUAD.Entity.Subscription();
                sub.SubscriptionID = al.RecordIdentifier;

                if (!string.IsNullOrEmpty(al.Street))
                    sub.Address = al.Street;
                else
                    sub.Address = oldData.Address;

                if (!string.IsNullOrEmpty(al.MailStop))
                    sub.MailStop = al.MailStop;
                else
                    sub.MailStop = oldData.MailStop;

                if (!string.IsNullOrEmpty(al.City))
                    sub.City = al.City;
                else
                    sub.City = oldData.City;

                if (!string.IsNullOrEmpty(al.Region))
                    sub.State = al.Region;
                else
                    sub.State = oldData.State;

                if (!string.IsNullOrEmpty(al.PostalCode))
                    sub.Zip = al.PostalCode;
                else
                    sub.Zip = oldData.Zip;

                if (!string.IsNullOrEmpty(al.PostalCodePlusFour))
                    sub.Plus4 = al.PostalCodePlusFour;
                else
                    sub.Plus4 = oldData.Plus4;

                sub.Latitude = Convert.ToDecimal(al.Latitude);
                sub.Longitude = Convert.ToDecimal(al.Longitude);
                sub.IsLatLonValid = al.IsValid;
                sub.LatLonMsg = al.ValidationMessage;

                if (!string.IsNullOrEmpty(al.Country))
                    sub.Country = al.Country;
                else
                    sub.Country = oldData.Country;

                if (!string.IsNullOrEmpty(al.County))
                    sub.County = al.County;
                else
                    sub.County = oldData.County;

                listSubs.Add(sub);
            }
            #endregion
            #region update database
            foreach (Core_AMS.Utilities.AddressLocation al in addressesToValidate.Where(x => x.ErrorOccured == true))
            {
                listDirty.RemoveAll(x => x.SubscriptionID == al.RecordIdentifier);
            }
            var bad = (from d in listDirty
                       where !listSubs.Exists(x => x.SubscriptionID == d.SubscriptionID)
                       select d).ToList();
            bad.ForEach(x => x.IsLatLonValid = false);
            bad.ForEach(x => x.LatLonMsg = "Invalid Address unable to attempt geocoding - " + DateTime.Now.ToString());
            bad.ForEach(x => x.Longitude = 0);
            bad.ForEach(x => x.Latitude = 0);
            listSubs.AddRange(bad);

            foreach (var x in listSubs)
            {
                if (x.Address == null) x.Address = string.Empty;
                if (x.MailStop == null) x.MailStop = string.Empty;
                if (x.City == null) x.City = string.Empty;
                if (x.State == null) x.State = string.Empty;
                if (x.Zip == null) x.Zip = string.Empty;
                if (x.Plus4 == null) x.Plus4 = string.Empty;
                if (x.Country == null) x.Country = string.Empty;
                if (x.County == null) x.County = string.Empty;
                if (x.LatLonMsg == null) x.LatLonMsg = string.Empty;

                if(x.IsLatLonValid == false && string.IsNullOrEmpty(x.LatLonMsg))
                    x.LatLonMsg = "Invalid Address " + DateTime.Now.ToString();
                
            }
            #endregion

            return listSubs;
        }
    }
}
