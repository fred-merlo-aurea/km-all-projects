using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberInvalid
    {
        public List<Entity.SubscriberInvalid> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberInvalid> x = null;
            x = DataAccess.SubscriberInvalid.Select(client).ToList();
            return x;
        }
        public List<Entity.SubscriberInvalid> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberInvalid> x = null;
            x = DataAccess.SubscriberInvalid.Select(processCode, client).ToList();
            return x;
        }
        public List<Entity.SubscriberInvalid> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberInvalid> x = null;
            x = DataAccess.SubscriberInvalid.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }

        public bool SaveBulkSqlInsert(List<Entity.SubscriberInvalid> list, KMPlatform.Object.ClientConnections client)
        {
            foreach (Entity.SubscriberInvalid x in list)
                FormatData(x);
            int BatchSize = 250000;
            bool done = true;
            if (list != null && list.Count > 0)
            {
                int sourceFileID = list.FirstOrDefault().SourceFileID;
                string processCode = list.FirstOrDefault().ProcessCode;

                if (list.Count >= BatchSize)
                {
                    #region batching for excel files when list is > 2500 records

                    int total = list.Count;
                    int counter = 0;
                    int processedCount = 0;

                    List<Entity.SubscriberInvalid> bulkProcessList = new List<Entity.SubscriberInvalid>();
                    foreach (Entity.SubscriberInvalid x in list)
                    {
                        counter++;
                        processedCount++;
                        done = false;
                        bulkProcessList.Add(x);
                        if (processedCount == total || counter == BatchSize)
                        {
                            try
                            {
                                //List<Entity.SubscriberTransformed> bulkDistinctProcessList = new List<Entity.SubscriberTransformed>();
                                //bulkDistinctProcessList = bulkProcessList.GroupBy(item => new { item.SORecordIdentifier, item.PubCode, item.OriginalImportRow }).Select(group => group.First()).ToList();

                                done = DataAccess.SubscriberInvalid.SaveBulkSqlInsert(bulkProcessList, client);
                                if (done == true)
                                {
                                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                                    fl.Save(new FrameworkUAS.Entity.FileLog(x.SourceFileID, -99, "Start Bulk Insert  SubscriberDemographicInvalid : processed count = " + processedCount.ToString(), x.ProcessCode));

                                    List<Entity.SubscriberDemographicInvalid> sendDemos = new List<Entity.SubscriberDemographicInvalid>();
                                    foreach (Entity.SubscriberInvalid st in bulkProcessList)
                                    {
                                        sendDemos.AddRange(st.DemographicInvalidList);
                                        if (sendDemos.Count >= BatchSize)//send 10k at a time
                                        {
                                            done = DataAccess.SubscriberDemographicInvalid.SaveBulkSqlInsert(sendDemos, client);
                                            sendDemos = new List<Entity.SubscriberDemographicInvalid>();
                                        }
                                    }
                                    //this will get the remaining items inserted
                                    if (sendDemos.Count > 0)
                                    {
                                        done = DataAccess.SubscriberDemographicInvalid.SaveBulkSqlInsert(sendDemos, client);
                                        sendDemos = new List<Entity.SubscriberDemographicInvalid>();
                                    }

                                    fl.Save(new FrameworkUAS.Entity.FileLog(x.SourceFileID, -99, "End Bulk Insert SubscriberDemographicInvalid : processed count = " + processedCount.ToString(), x.ProcessCode));
                                }
                            }
                            catch (Exception ex)
                            {
                                done = false;
                                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                                fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, message, processCode));
                            }
                            counter = 0;
                            bulkProcessList = new List<Entity.SubscriberInvalid>();
                        }
                    }
                    #endregion
                }
                else
                {
                    #region standard process for when list is <= 2500 records - mainly non excel files
                    try
                    {
                        //List<Entity.SubscriberTransformed> bulkDistinctProcessList = new List<Entity.SubscriberTransformed>();
                        //bulkDistinctProcessList = list.GroupBy(item => new { item.SORecordIdentifier, item.PubCode, item.OriginalImportRow }).Select(group => group.First()).ToList();

                        done = DataAccess.SubscriberInvalid.SaveBulkSqlInsert(list, client);
                        if (done == true)
                        {
                            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, "Start Bulk Insert  SubscriberDemographicInvalid", processCode));
                            List<Entity.SubscriberDemographicInvalid> sendDemos = new List<Entity.SubscriberDemographicInvalid>();
                            foreach (Entity.SubscriberInvalid st in list)
                            {
                                sendDemos.AddRange(st.DemographicInvalidList);
                                if (sendDemos.Count >= BatchSize)//send 10k at a time
                                {
                                    done = DataAccess.SubscriberDemographicInvalid.SaveBulkSqlInsert(sendDemos, client);
                                    sendDemos = new List<Entity.SubscriberDemographicInvalid>();
                                }
                            }
                            //this will get the remaining items inserted
                            if (sendDemos.Count > 0)
                            {
                                done = DataAccess.SubscriberDemographicInvalid.SaveBulkSqlInsert(sendDemos, client);
                                sendDemos = new List<Entity.SubscriberDemographicInvalid>();
                            }
                            fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, "End Bulk Insert SubscriberDemographicInvalid", processCode));
                        }
                    }
                    catch (Exception ex)
                    {
                        done = false;
                        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                        fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, processCode));
                    }
                    #endregion
                }
            }

            //foreach (Entity.SubscriberInvalid x in list)
            //    FormatData(x);

            //bool done = true;
            //if (list != null && list.Count > 0)
            //{
            //    int sourceFileID = list.FirstOrDefault().SourceFileID;
            //    string processCode = list.FirstOrDefault().ProcessCode;
            //    using (TransactionScope scope = new TransactionScope())
            //    {
            //        try
            //        {
            //            done = DataAccess.SubscriberInvalid.SaveBulkSqlInsert(list, client);
            //            if (done == true)
            //            {
            //                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
            //                fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, "Start Bulk Insert  SubscriberDemographicInvalid", processCode));
            //                done = DataAccess.SubscriberDemographicInvalid.SaveBulkSqlInsert(list, client);
            //                fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, "End Bulk Insert SubscriberDemographicInvalid", processCode));
            //            }
            //            scope.Complete();
            //        }
            //        catch (Exception ex)
            //        {
            //            scope.Dispose();
            //            done = false;
            //            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
            //            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, processCode));
            //        }
            //    }
            //}
            return done;
        }

        public void FormatData(Entity.SubscriberInvalid x)
        {
            try
            {
                //x = PopulateNull(x);

                //if (x.SIRecordIdentifier == Guid.Empty)
                //    x.SIRecordIdentifier = Guid.NewGuid();
                if (x.Phone != null)
                    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
                if (x.Mobile != null)
                    x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
                if (x.Fax != null)
                    x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);
                //if (x.Email != null && x.Email.Length <= 4)
                //    x.Email = string.Empty;

                #region truncate strings
                if (x.PubCode != null && x.PubCode.Length > 100)
                    x.PubCode = x.PubCode.Substring(0, 100);
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
                if (x.Zip != null && x.Zip.Length > 50)
                    x.Zip = x.Zip.Substring(0, 50);
                if (x.Plus4 != null && x.Plus4.Length > 50)
                    x.Plus4 = x.Plus4.Substring(0, 50);
                if (x.ForZip != null && x.ForZip.Length > 50)
                    x.ForZip = x.ForZip.Substring(0, 50);
                if (x.County != null && x.County.Length > 100)
                    x.County = x.County.Substring(0, 100);
                if (x.Country != null && x.Country.Length > 100)
                    x.Country = x.Country.Substring(0, 100);
                if (x.Email != null && x.Email.Length > 100)
                    x.Email = x.Email.Substring(0, 100);
                if (x.RegCode != null && x.RegCode.Length > 5)
                    x.RegCode = x.RegCode.Substring(0, 5);
                if (x.Verified != null && x.Verified.Length > 100)
                    x.Verified = x.Verified.Substring(0, 100);
                if (x.SubSrc != null && x.SubSrc.Length > 25)
                    x.SubSrc = x.SubSrc.Substring(0, 25);
                if (x.OrigsSrc != null && x.OrigsSrc.Length > 25)
                    x.OrigsSrc = x.OrigsSrc.Substring(0, 25);
                if (x.Par3C != null && x.Par3C.Length > 10)
                    x.Par3C = x.Par3C.Substring(0, 10);
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

                if (x.Address3 != null && x.Address3.Length > 255)
                    x.Address3 = x.Address3.Substring(0, 255);
                if (x.Home_Work_Address != null && x.Home_Work_Address.Length > 10)
                    x.Home_Work_Address = x.Home_Work_Address.Substring(0, 10);

                if (x.Demo7 != null && x.Demo7.Length > 1)
                    x.Demo7 = x.Demo7.Substring(0, 1);
  
                if (x.AccountNumber != null && x.AccountNumber.Length > 50)
                    x.AccountNumber = x.AccountNumber.Substring(0, 50);
                if (x.Occupation != null && x.Occupation.Length > 50)
                    x.Occupation = x.Occupation.Substring(0, 50);
                if (x.Website != null && x.Website.Length > 255)
                    x.Website = x.Website.Substring(0, 255);
                #endregion

                if (x.QDate == DateTime.Parse("0001-01-01T00:00:00") || x.QDate == DateTime.MinValue || x.QDate <= DateTime.Parse("1/1/1900"))
                    x.QDate = DateTime.Now;

            }
            catch (Exception ex)
            {
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "FormatData"));
            }
        }
    }
}
