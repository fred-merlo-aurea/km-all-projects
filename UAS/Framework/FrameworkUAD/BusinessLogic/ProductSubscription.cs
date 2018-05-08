using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Text;
using FrameworkUAD.BusinessLogic.Helpers;
using KM.Common;
using KM.Common.Functions;
using KMPlatform.Entity;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductSubscription
    {
        private const string NameProductSubscriptionDetail = "ProductSubscriptionDetail";

        public List<Entity.ProductSubscription> Select(int subscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            List<Entity.ProductSubscription> retList = null;
            retList = DataAccess.ProductSubscription.Select(subscriptionID, client, clientDisplayName);
            if (includeCustomProperties == true)
            {
                foreach (var x in retList)
                {
                    x.SubscriberProductDemographics = GetCustomProperties(subscriptionID, x.PubCode, client).ToList();
                }
            }
            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        public Entity.ProductSubscription Select(Guid sfRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            Entity.ProductSubscription retItem = null;
            retItem = DataAccess.ProductSubscription.Select(sfRecordIdentifier, client);
            if (retItem != null)
                return FormatZipCode(retItem);
            else
                return retItem;
        }
        public  Entity.ProductSubscription SelectSequenceIDPubID(int seqnum, int pubid, KMPlatform.Object.ClientConnections client)
        {
            Entity.ProductSubscription retItem = null;
            retItem = DataAccess.ProductSubscription.SelectSequenceIDPubID(seqnum, pubid, client);
            return retItem;
        }
        public Entity.ProductSubscription SelectProductSubscription(int pubSubscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            Entity.ProductSubscription retItem = null;
            retItem = DataAccess.ProductSubscription.SelectProductSubscription(pubSubscriptionID, client, clientDisplayName);

            //string xml = ServiceStack.Text.XmlSerializer.SerializeToString<Entity.ProductSubscription>(retList);
            //string json = ServiceStack.Text.JsonSerializer.SerializeToString<Entity.ProductSubscription>(retList);
            if (retItem != null)
                return FormatZipCode(retItem);
            else
                return retItem;
        }

        private List<Object.SubscriberProductDemographic> GetCustomProperties(int subscriptionID, string pubCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.SubscriberProductDemographic> pubDetails = new List<Object.SubscriberProductDemographic>();
            SubscriberProductDemographic spdData = new SubscriberProductDemographic();
            pubDetails = spdData.Select(subscriptionID, pubCode, client).ToList();
            return pubDetails;
        }
        public DataTable Select_For_Export(int page, int pageSize, string columns, int productID, KMPlatform.Object.ClientConnections client)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ProductSubscription.Select_For_Export(page, pageSize, columns, productID, client);
            try
            {
                return FormatZipCode(dtCMS);
            }
            catch
            {
                return dtCMS;
            }
        }
        public DataTable Select_For_Export_Static(int productID, string cols, List<int> subs, KMPlatform.Object.ClientConnections client)
        {
            DataTable dtCMS = new DataTable();
            string s = "<XML>";
            subs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            dtCMS = DataAccess.ProductSubscription.Select_For_Export_Static(productID, cols, s, client);
            try
            {
                return FormatZipCode(dtCMS);
            }
            catch
            {
                return dtCMS;
            }
        }
        public DataTable Select_For_Export_Static(int productID, int issueid, string cols, List<int> subs, KMPlatform.Object.ClientConnections client)
        {
            DataTable dtCMS = new DataTable();
            string s = "<XML>";
            subs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            dtCMS = DataAccess.ProductSubscription.Select_For_Export_Static(productID, issueid, cols, s, client);
            try
            {
                return FormatZipCode(dtCMS);
            }
            catch
            {
                return dtCMS;
            }
        }
       

        public bool Update_Requester_Flags(int productID, int issueID, KMPlatform.Object.ClientConnections client)
        {
            bool success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                success = DataAccess.ProductSubscription.Update_Requester_Flags(client, productID, issueID);
                scope.Complete();
            }
            return success;
        }
        public List<FrameworkUAD.Object.PubSubscriptionAdHoc> Get_AdHocs(int pubID, int pubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            DataTable result;
            List<FrameworkUAD.Object.PubSubscriptionAdHoc> lst = new List<Object.PubSubscriptionAdHoc>();
            result = DataAccess.PubSubscriptionsExtension.GetAdHoc(client, pubSubscriptionID, pubID);
            Dictionary<string, string> adhocs = new Dictionary<string, string>();
            foreach (DataRow dr in result.Rows)
            {
                foreach (DataColumn dc in result.Columns)
                {
                    lst.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc(dc.ColumnName, dr[dc].ToString()));
                }
            }
            return lst;
        }
        public List<string> Get_AdHocs(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<string> lst = new List<string>();
            lst = DataAccess.PubSubscriptionsExtension.GetAdHocs(client, pubID);
            return lst;
        }

        public Entity.ProductSubscription FormatCanadianZip(Entity.ProductSubscription ps)
        {
            if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
            {
                if (ps.ZipCode.Length == 6)
                    ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);
            }
            return ps;
        }
        public Entity.ProductSubscription FormatZipCode(Entity.ProductSubscription ps)
        {
            if (ps != null)
            {
                try
                {
                    #region Canada
                    if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
                    {
                        if (ps.ZipCode.Length == 6)
                        {
                            ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);
                            ps.Plus4 = string.Empty;
                        }
                        else if (ps.ZipCode.Length == 7 && ps.ZipCode.Contains(" "))
                        {
                            ps.Plus4 = string.Empty;
                        }
                        else if (ps.ZipCode.Length == 3 && ps.Plus4.Length == 3)
                        {
                            ps.ZipCode = ps.ZipCode + " " + ps.Plus4;
                            ps.Plus4 = string.Empty;
                        }
                        else if (ps.ZipCode.Length > 7)
                        {
                            if (ps.ZipCode.Contains(" "))
                                ps.ZipCode = ps.ZipCode.Substring(0, 7);
                            else
                                ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);

                            ps.Plus4 = string.Empty;
                        }
                    }
                    #endregion
                    #region USA
                    else if (ps.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 1)
                    {
                        var zipCodeArgs = ZipCodeMethodsHelper.ExecuteUsaFormatting(ps.ZipCode, ps.Plus4);
                        ps.ZipCode = zipCodeArgs.ZipCode;
                        ps.Plus4 = zipCodeArgs.Plus4Code;
                    }
                    #endregion
                    #region Mexico or Foreign
                    else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                    {
                        //do nothing with ZipCode - just keep whatever is there
                        ps.Plus4 = string.Empty;
                    }
                    #endregion
                }
                catch { }//suppress any null errors
            }
            return ps;
        }
        public List<Entity.ProductSubscription> FormatZipCode(List<Entity.ProductSubscription> list)
        {
            if (list != null)
            {
                foreach (Entity.ProductSubscription ps in list)
                {
                    try
                    {
                        #region Canada
                        if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
                        {
                            if (ps.ZipCode.Length == 6)
                            {
                                ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);
                                ps.Plus4 = string.Empty;
                            }
                            else if (ps.ZipCode.Length == 7 && ps.ZipCode.Contains(" "))
                            {
                                ps.Plus4 = string.Empty;
                            }
                            else if (ps.ZipCode.Length == 3 && ps.Plus4.Length == 3)
                            {
                                ps.ZipCode = ps.ZipCode + " " + ps.Plus4;
                                ps.Plus4 = string.Empty;
                            }
                            else if (ps.ZipCode.Length > 7)
                            {
                                if (ps.ZipCode.Contains(" "))
                                    ps.ZipCode = ps.ZipCode.Substring(0, 7);
                                else
                                    ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);

                                ps.Plus4 = string.Empty;
                            }
                        }
                        #endregion
                        #region USA
                        else if (ps.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 1)
                        {
                            var zipCodeArgs = ZipCodeMethodsHelper.ExecuteUsaFormatting(ps.ZipCode, ps.Plus4);
                            ps.ZipCode = zipCodeArgs.ZipCode;
                            ps.Plus4 = zipCodeArgs.Plus4Code;
                        }
                        #endregion
                        #region Mexico or Foreign
                        else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                        {
                            //do nothing with ZipCode - just keep whatever is there
                            ps.Plus4 = string.Empty;
                        }
                        #endregion
                    }
                    catch { }//suppress any null errors
                }
            }
            return list;
        }
        public DataTable FormatZipCode(DataTable dt)
        {
            bool addedPlus4 = false;
            if (dt != null)
            {
                if (!dt.Columns.Contains("Plus4"))
                {
                    dt.Columns.Add("Plus4");
                    addedPlus4 = true;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        if (dt.Columns.Contains("Country") && dt.Columns.Contains("ZipCode"))
                        {
                            #region Canada
                            if (dr["Country"].ToString().Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || (dt.Columns.Contains("CountryID") && dr["CountryID"].ToString() == "2"))
                            {
                                if (dr["ZipCode"].ToString().Length == 6)
                                {
                                    dr["ZipCode"] = dr["ZipCode"].ToString().Substring(0, 3) + " " + dr["ZipCode"].ToString().Substring(3, 3);
                                    if (dt.Columns.Contains("Plus4"))
                                        dr["Plus4"] = string.Empty;
                                }
                                else if (dr["ZipCode"].ToString().Length == 7 && dr["ZipCode"].ToString().Contains(" "))
                                {
                                    if (dt.Columns.Contains("Plus4"))
                                        dr["Plus4"] = string.Empty;
                                }
                                else if (dr["ZipCode"].ToString().Length == 3 && dr["Plus4"].ToString().Length == 3)
                                {
                                    dr["ZipCode"] = dr["ZipCode"].ToString() + " " + dr["Plus4"].ToString();
                                    dr["Plus4"] = string.Empty;
                                }
                                else if (dr["ZipCode"].ToString().Length > 7)
                                {
                                    string cleansedZip = dr["ZipCode"].ToString().Replace(" ","");
                                    if (cleansedZip.Length >= 6)
                                        dr["ZipCode"] = cleansedZip.Substring(0, 3) + " " + cleansedZip.Substring(3, 3);

                                    dr["Plus4"] = string.Empty;
                                }
                            }
                            #endregion
                            #region USA
                            else if (dr["Country"].ToString().Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) || (dt.Columns.Contains("CountryID") && dr["CountryID"].ToString() == "1"))
                            {
                                var zipCodeArgs = ZipCodeMethodsHelper.ExecuteUsaFormatting(dr["ZipCode"].ToString(), dr["Plus4"].ToString());
                                dr["ZipCode"] = zipCodeArgs.ZipCode;
                                dr["Plus4"] = zipCodeArgs.Plus4Code;
                            }
                            #endregion
                            #region Mexico or Foreign
                            else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                            {
                                //do nothing with ZipCode - just keep whatever is there
                                dr["Plus4"] = string.Empty;
                            }
                            #endregion
                        }

                    }
                    catch { }//suppress any null errors
                }
                if (addedPlus4 == true)
                    dt.Columns.Remove("Plus4");
                dt.AcceptChanges();
            }
            return dt;
        }
        #region old format Canadian Zips
        //public List<Entity.ProductSubscription> FormatCanadianZip(List<Entity.ProductSubscription> list)
        //{
        //    foreach (var ps in list)
        //    {
        //        if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
        //        {
        //            if (ps.ZipCode.Length == 6)
        //                ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);
        //        }
        //    }
        //    return list;
        //}
        //public DataTable FormatCanadianZip(DataTable dt)
        //{
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (dt.Columns.Contains("Country") && dt.Columns.Contains("ZipCode"))
        //            //.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
        //        {
        //            if (dr["Country"].ToString().Equals("Canada", StringComparison.CurrentCultureIgnoreCase) && dr["ZipCode"].ToString().Length == 6)
        //                dr["ZipCode"] = dr["ZipCode"].ToString().Substring(0, 3) + " " + dr["ZipCode"].ToString().Substring(3, 3);
        //        }
        //    }
        //    return dt;
        //}
        #endregion

        #region Save
        public int ProfileSave(Entity.ProductSubscription curr, Entity.ProductSubscription orig, bool saveWaveMailing, int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes ult,
                            int userLogTypeID, FrameworkUAS.Object.Batch batch, KMPlatform.Object.ClientConnections client, Entity.ProductSubscription waveMail = null,
                            Entity.WaveMailingDetail waveMailDetail = null)
        {
            FrameworkUAD.BusinessLogic.History historyW = new History();
            FrameworkUAD.BusinessLogic.HistorySubscription historySubW = new HistorySubscription();
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            KMPlatform.BusinessLogic.UserLog ulW = new KMPlatform.BusinessLogic.UserLog();
            FrameworkUAD.BusinessLogic.WaveMailingDetail waveMailDetailW = new WaveMailingDetail();

            int pubSubscriptionID = 0;
            try
            {
                int userID = 0;
                if (curr.IsNewSubscription)
                    userID = curr.CreatedByUserID;
                else
                    userID = (curr.UpdatedByUserID ?? -1);

                curr.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(curr.Phone);
                curr.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(curr.Mobile);
                curr.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(curr.Fax);

                using (TransactionScope scope = new TransactionScope())
                {
                    pubSubscriptionID = DataAccess.ProductSubscription.Save(curr, client);
                    scope.Complete();
                }
                curr = SelectProductSubscription(pubSubscriptionID, client, "");

                #region Write to Logs

                int userLogID = 0;

                #region User Log

                if (orig.IsInActiveWaveMailing == true && saveWaveMailing == true)
                {
                    userLogID = ulW.CreateLog(applicationID, ult, userID, "ProductSubscription",
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(orig),
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(waveMail), userLogTypeID).UserLogID;

                    waveMailDetailW.Save(waveMailDetail, client);
                }
                else
                {
                    userLogID = ulW.CreateLog(applicationID, ult, userID, "ProductSubscription",
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(orig),
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(curr), userLogTypeID).UserLogID;
                }

                #endregion
                #region History - Subscription, Batch, User Log
                int historySubscriptionID = 0;
                historySubscriptionID = historySubW.Save(curr, userID, client);

                FrameworkUAD.Entity.Batch rtnBatch = SaveBatch(curr.PubID, batch, userID, client);
                int historyID = historyW.AddHistoryEntry(client, rtnBatch.BatchID, rtnBatch.BatchCount, curr.PubID, curr.PubSubscriptionID, curr.SubscriptionID, historySubscriptionID, 0, userID, 0).HistoryID;
                //UserLog HistoryID - HistoryToUserLog
                if (userLogID > 0)
                    historyW.Insert_History_To_UserLog(historyID, userLogID, client);
                #endregion

                #endregion
            }
            catch(Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog log = new KMPlatform.BusinessLogic.ApplicationLog();
                log.LogCriticalError(msg, "ProductSubscription.ProfileSave", KMPlatform.BusinessLogic.Enums.Applications.Circulation);
            }
            return pubSubscriptionID;
        }

        public int FullSave(Entity.ProductSubscription curr, Entity.ProductSubscription orig, bool saveWaveMailing, int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes ult,
                    int userLogTypeID, FrameworkUAS.Object.Batch batch, int clientID, bool madeResponseChange, bool madePaidChange, bool madeBillToChange,
                    List<Entity.ProductSubscriptionDetail> answers, Entity.ProductSubscription waveMail = null, Entity.WaveMailingDetail waveMailDetail = null,
                    Entity.SubscriptionPaid subPaid = null, Entity.PaidBillTo billTo = null, List<Entity.ProductSubscriptionDetail> subscriberAnswers = null)
        {
            KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            int pubSubscriptionID = 0;
            int userID = 0;
            if (curr.IsNewSubscription)
                userID = curr.CreatedByUserID;
            else
                userID = (curr.UpdatedByUserID ?? -1);

            curr.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(curr.Phone);
            curr.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(curr.Mobile);
            curr.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(curr.Fax);


            KMPlatform.BusinessLogic.Client clientW = new KMPlatform.BusinessLogic.Client();

            client = clientW.Select(clientID, false);
            using (TransactionScope scope = new TransactionScope())
            {
                pubSubscriptionID = DataAccess.ProductSubscription.Save(curr, client.ClientConnections);
                scope.Complete();
            }
            #region AdHoc Fields
            List<int> adHocUserLogIDs = new List<int>();
            if (curr.AdHocFields != null && curr.AdHocFields.Count > 0)
            {
                KMPlatform.BusinessLogic.UserLog userLogW = new KMPlatform.BusinessLogic.UserLog();
                List<KMPlatform.Entity.UserLog> ulList = new List<KMPlatform.Entity.UserLog>();
                List<FrameworkUAD.Object.PubSubscriptionAdHoc> origAdHocs = new List<Object.PubSubscriptionAdHoc>();
                List<string> adhocs = new List<string>();
                adhocs = Get_AdHocs(curr.PubID, client.ClientConnections);
                origAdHocs = Get_AdHocs(curr.PubID, pubSubscriptionID, client.ClientConnections);
                DataAccess.PubSubscriptionsExtension.Save(client.ClientConnections, pubSubscriptionID, curr.PubID, curr.AdHocFields);
                if (origAdHocs != null)
                {
                    List<string> missing = adhocs.Except(origAdHocs.Select(x => x.AdHocField)).ToList();
                    missing.ForEach(x => origAdHocs.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc(x, "")));
                    foreach (FrameworkUAD.Object.PubSubscriptionAdHoc ahc in curr.AdHocFields)
                    {
                        FrameworkUAD.Object.PubSubscriptionAdHoc a = origAdHocs.Where(x => x.AdHocField == ahc.AdHocField).FirstOrDefault();

                        if (a != null)
                        {
                            if (a.Value != ahc.Value)
                            {
                                ulList.Add(new KMPlatform.Entity.UserLog()
                                {
                                    ApplicationID = applicationID,
                                    UserLogTypeID = userLogTypeID,
                                    UserID = userID,
                                    Object = "PubSubscriptionAdHoc",
                                    FromObjectValues = jf.ToJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(a),
                                    ToObjectValues = jf.ToJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(ahc),
                                    DateCreated = DateTime.Now
                                });
                            }
                        }
                    }
                    if (ulList.Count > 0)
                    {
                        using (TransactionScope innerScope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            List<KMPlatform.Entity.UserLog> userLogList = userLogW.SaveBulkInsert(ulList, client);
                            if (userLogList != null)
                                adHocUserLogIDs = userLogList.Select(x => x.UserLogID).ToList();
                            innerScope.Complete();
                        }
                    }
                }

                origAdHocs = Get_AdHocs(curr.PubID, pubSubscriptionID, client.ClientConnections);
                DataAccess.PubSubscriptionsExtension.Save(client.ClientConnections, pubSubscriptionID, curr.PubID, curr.AdHocFields);
                if (origAdHocs != null)
                {
                    foreach (FrameworkUAD.Object.PubSubscriptionAdHoc ahc in curr.AdHocFields)
                    {
                        FrameworkUAD.Object.PubSubscriptionAdHoc a = origAdHocs.Where(x => x.AdHocField == ahc.AdHocField).FirstOrDefault();
                        if (a != null)
                        {
                            if (a.Value != ahc.Value)
                            {
                                ulList.Add(new KMPlatform.Entity.UserLog()
                                {
                                    ApplicationID = applicationID,
                                    UserLogTypeID = userLogTypeID,
                                    UserID = userID,
                                    Object = "PubSubscriptionAdHoc",
                                    FromObjectValues = jf.ToJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(a),
                                    ToObjectValues = jf.ToJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(ahc),
                                    DateCreated = DateTime.Now
                                });
                            }
                        }
                    }
                    if (ulList.Count > 0)
                    {
                        using (TransactionScope innerScope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            List<KMPlatform.Entity.UserLog> userLogList = userLogW.SaveBulkInsert(ulList, client);
                            if (userLogList != null)
                                adHocUserLogIDs = userLogList.Select(x => x.UserLogID).ToList();
                            innerScope.Complete();
                        }
                    }
                }
            }

            #endregion

            curr = SelectProductSubscription(pubSubscriptionID, client.ClientConnections, "");

            #region Write to Logs

            KMPlatform.BusinessLogic.UserLog ulW = new KMPlatform.BusinessLogic.UserLog();
            FrameworkUAD.BusinessLogic.WaveMailingDetail waveMailDetailW = new WaveMailingDetail();
            int userLogID = 0;

            using (TransactionScope innerScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (orig.IsInActiveWaveMailing == true && saveWaveMailing == true)
                {
                    userLogID = ulW.CreateLog(applicationID, ult, userID, "ProductSubscription",
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(orig),
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(waveMail), userLogTypeID).UserLogID;

                    waveMailDetailW.Save(waveMailDetail, client.ClientConnections);
                }
                else
                {
                    userLogID = ulW.CreateLog(applicationID, ult, userID, "ProductSubscription",
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(orig),
                                  jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(curr), userLogTypeID).UserLogID;
                }
                innerScope.Complete();
            }

            int userLogIDPaid = 0;
            int userLogIDPaidBillTo = 0;
            int historyPaidBillToID = 0;
            int historyPaidID = 0;
            #region Save Paid Fields
            if (curr.IsPaid == true)
            {
                userLogIDPaid = SavePaid(pubSubscriptionID, subPaid, userID, madePaidChange, applicationID, client.ClientConnections);
                if (subPaid != null && subPaid.SubscriptionPaidID > 0)
                    billTo.SubscriptionPaidID = subPaid.SubscriptionPaidID;

                userLogIDPaidBillTo = SavePaidBillTo(pubSubscriptionID, billTo, subPaid, userID, madeBillToChange, applicationID, client.ClientConnections);
            }
            #endregion

            // Only create history and increment batch if something actually saved
            if ((userLogID > 0 || userLogIDPaid > 0 || userLogIDPaidBillTo > 0))
            {
                FrameworkUAD.Entity.Batch rtnBatch = SaveBatch(curr.PubID, batch, userID, client.ClientConnections);

                int historySubscriptionID = 0;
                FrameworkUAD.BusinessLogic.HistorySubscription historySubW = new HistorySubscription();
                FrameworkUAD.BusinessLogic.HistoryPaid historyPaidW = new HistoryPaid();
                FrameworkUAD.BusinessLogic.HistoryPaidBillTo historyPaidBillToW = new HistoryPaidBillTo();
                historySubscriptionID = historySubW.Save(curr, userID, client.ClientConnections);

                if (subPaid != null && subPaid.SubscriptionPaidID > 0)
                    historyPaidID = historyPaidW.Save(subPaid, userID, client.ClientConnections);
                if (billTo != null && billTo.SubscriptionPaidID > 0)
                    historyPaidBillToID = historyPaidBillToW.Save(billTo, userID, client.ClientConnections);

                //History

                FrameworkUAD.BusinessLogic.History historyW = new History();
                int historyID = historyW.AddHistoryEntry(client.ClientConnections, rtnBatch.BatchID, rtnBatch.BatchCount, curr.PubID, curr.PubSubscriptionID, curr.SubscriptionID, historySubscriptionID,
                                                       historyPaidID, userID, historyPaidBillToID).HistoryID;

                //HistoryMarketing
                //List<int> marketingUserLogIDs = new List<int>();
                //if (madeResponseChange)
                //    marketingUserLogIDs = SaveMarketingMap(pubSubscriptionID, applicationID, userID, ult, permissions, client, historyID);

                //HistoryResponse
                List<int> responseUserLogIDs = new List<int>();

                if (madeResponseChange)
                    responseUserLogIDs = SaveResponses(answers, applicationID, userID, curr.SubscriptionID, pubSubscriptionID, curr.PubID, client, historySubscriptionID);

                //UserLog HistoryID - HistoryToUserLog
                if (userLogID > 0)
                    historyW.Insert_History_To_UserLog(historyID, userLogID, client.ClientConnections);
                if (historySubscriptionID > 0)
                    historyW.Insert_History_To_UserLog(historyID, historySubscriptionID, client.ClientConnections);
                if (userLogIDPaid > 0)
                    historyW.Insert_History_To_UserLog(historyID, userLogIDPaid, client.ClientConnections);
                if (userLogIDPaidBillTo > 0)
                    historyW.Insert_History_To_UserLog(historyID, userLogIDPaidBillTo, client.ClientConnections);
                //foreach (int i in marketingUserLogIDs)
                //    historyW.Insert_History_To_UserLog(historyID, i, client.ClientConnections);
                foreach (int i in responseUserLogIDs)
                    historyW.Insert_History_To_UserLog(historyID, i, client.ClientConnections);
                foreach (int i in adHocUserLogIDs)
                    historyW.Insert_History_To_UserLog(historyID, i, client.ClientConnections);
            }

            #endregion


            return pubSubscriptionID;
        }

        private int SavePaid(int pubSubscriptionID, Entity.SubscriptionPaid subPaid, int userID, bool madePaidChange, int applicationID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.BusinessLogic.SubscriptionPaid subPaidW = new SubscriptionPaid();
            KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            FrameworkUAD.Entity.SubscriptionPaid spOriginal = subPaidW.Select(pubSubscriptionID, client);
            KMPlatform.BusinessLogic.UserLog userLogW = new KMPlatform.BusinessLogic.UserLog();
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();

            int userLogID = -1;
            subPaid.PubSubscriptionID = pubSubscriptionID;
            if (subPaid.SubscriptionPaidID > 0)
            {
                subPaid.DateUpdated = DateTime.Now;
                subPaid.UpdatedByUserID = userID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            }
            else
            {
                subPaid.DateCreated = DateTime.Now;
                subPaid.CreatedByUserID = userID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            }
            if (madePaidChange)
            {
                int userLogTypeID = 0;
                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();

                userLogTypeID = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, ult.ToString()).CodeId;

                int spIntID = subPaidW.Save(subPaid, client);

                subPaid.SubscriptionPaidID = spIntID;

                using (TransactionScope innerScope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    userLogID = userLogW.CreateLog(applicationID, ult, userID, "SubscriptionPaid", jf.ToJson<FrameworkUAD.Entity.SubscriptionPaid>(spOriginal),
                        jf.ToJson<FrameworkUAD.Entity.SubscriptionPaid>(subPaid), userLogTypeID).UserLogID;

                    innerScope.Complete();
                }
            }
            return userLogID;
        }

        private int SavePaidBillTo(int pubsubscriptionID, Entity.PaidBillTo billTo, Entity.SubscriptionPaid subPaid, int userID, bool madeBillToChange,
            int applicationID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.BusinessLogic.PaidBillTo paidBillToW = new PaidBillTo();
            FrameworkUAD.Entity.PaidBillTo spOriginal = paidBillToW.SelectSubscription(pubsubscriptionID, client);
            KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            KMPlatform.BusinessLogic.UserLog userLogW = new KMPlatform.BusinessLogic.UserLog();
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();

            billTo.PubSubscriptionID = pubsubscriptionID;
            if (billTo.PaidBillToID > 0)
            {
                billTo.DateUpdated = DateTime.Now;
                billTo.UpdatedByUserID = userID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            }
            else
            {
                billTo.DateCreated = DateTime.Now;
                billTo.CreatedByUserID = userID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            }
            if (subPaid != null)
                billTo.SubscriptionPaidID = subPaid.SubscriptionPaidID;

            int userLogID = -1;

            if (madeBillToChange)
            {
                int pbtIDResponse = paidBillToW.Save(billTo, client);
                billTo.PaidBillToID = pbtIDResponse;

                int userLogTypeID = 0;
                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();

                userLogTypeID = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, ult.ToString()).CodeId;

                jf = new Core_AMS.Utilities.JsonFunctions();

                using (TransactionScope innerScope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    userLogID = userLogW.CreateLog(applicationID, ult, userID, "PaidBillTo", jf.ToJson<FrameworkUAD.Entity.PaidBillTo>(spOriginal),
                                                    jf.ToJson<FrameworkUAD.Entity.PaidBillTo>(billTo), userLogTypeID).UserLogID;

                    innerScope.Complete();
                }
            }

            return userLogID;
        }

        private List<int> SaveResponses(
            IEnumerable<Entity.ProductSubscriptionDetail> answers,
            int applicationId,
            int userId,
            int subscriptionId,
            int pubSubscriptionId,
            int pubId,
            Client client,
            int historyId)
        {
            Guard.NotNull(answers, nameof(answers));
            Guard.NotNull(client, nameof(client));
            var args = new SaveResponseArgs
            {
                ApplicationId = applicationId,
                UserId = userId,
                SubscriptionId = subscriptionId,
                PubSubscriptionId = pubSubscriptionId,
                PubId = pubId,
                HistoryId = historyId,
                Client = client
            };
            args.CurrentAnswers.AddRange(answers);

            PrepareSaveResponseArgs(args);
            AddRemovedResponses(args);
            AddNewResponses(args);
            AddUpdatedResponses(args);

            var userLogIds = SaveLogs(args);

            return userLogIds.ToList();
        }

        private void PrepareSaveResponseArgs(SaveResponseArgs args)
        {
            Guard.NotNull(args, nameof(args));

            var currentIds = args.CurrentAnswers.Select(x => x.CodeSheetID)
                .ToList();
            var originalList = args.PubSubDetailWorker.Select(args.PubSubscriptionId, args.Client.ClientConnections)
                .ToList();
            var origIds = originalList.Select(x => x.CodeSheetID)
                .ToList();

            args.CurrentAnswers.ForEach(x =>
            {
                x.PubSubscriptionID = args.PubSubscriptionId;
                x.SubscriptionID = args.SubscriptionId;
            });

            var removedList = originalList.Where(x => !currentIds.Contains(x.CodeSheetID));
            args.RemovedList.AddRange(removedList);
            var addedList = args.CurrentAnswers.Where(x => !origIds.Contains(x.CodeSheetID));
            args.AddedList.AddRange(addedList);

            var codeSheetWorker = new CodeSheet();
            var responseGroupWorker = new ResponseGroup();
            var codeSheetList = codeSheetWorker.Select(args.PubId, args.Client.ClientConnections);
            var responseGroupList = responseGroupWorker.Select(args.PubId, args.Client.ClientConnections);

            foreach (var subDetail in args.CurrentAnswers)
            {
                var groupId = codeSheetList.Where(x => x.CodeSheetID == subDetail.CodeSheetID)
                    .OrderByDescending(x => x.DateCreated)
                    .Select(x => x.ResponseGroupID)
                    .SingleOrDefault();
                if (groupId > 0)
                {
                    var codes = codeSheetList.Where(x => x.ResponseGroupID == groupId)
                        .ToList();
                    var responseGroup = responseGroupList.FirstOrDefault(x => x.ResponseGroupID == groupId);
                    var matchId = originalList.Select(a => a.CodeSheetID)
                        .Intersect(codes.Select(b => b.CodeSheetID))
                        .FirstOrDefault();
                    var origin = originalList.FirstOrDefault(x => x.CodeSheetID == matchId);

                    if (origin != null
                        && responseGroup?.IsMultipleValue == false
                        && (origin.CodeSheetID != subDetail.CodeSheetID || origin.ResponseOther != subDetail.ResponseOther))
                    {
                        args.AddedList.Remove(subDetail);
                        args.RemovedList.Remove(origin);
                        args.PubSubDetailChanges.Add(new ProductSubscriptionDetailChange(origin, subDetail));
                    }

                    args.ResponseMaps.Add(new Entity.HistoryResponseMap
                    {
                        PubSubscriptionDetailID = subDetail.PubSubscriptionDetailID,
                        PubSubscriptionID = args.PubSubscriptionId,
                        SubscriptionID = args.SubscriptionId,
                        CodeSheetID = subDetail.CodeSheetID,
                        DateCreated = DateTime.Now,
                        CreatedByUserID = args.UserId,
                        ResponseOther = subDetail.ResponseOther
                    });
                }
            }
        }

        private void AddRemovedResponses(SaveResponseArgs args)
        {
            Guard.NotNull(args, nameof(args));

            int userLogTypeId;

            using (var innerScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                userLogTypeId = args.CodeWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, FrameworkUAD_Lookup.Enums.UserLogTypes.Delete.ToString())
                    .CodeId;
                innerScope.Complete();
            }

            foreach (var item in args.RemovedList)
            {
                args.UserLogs.Add(new UserLog
                {
                    ApplicationID = args.ApplicationId,
                    UserLogTypeID = userLogTypeId,
                    UserID = args.UserId,
                    Object = NameProductSubscriptionDetail,
                    FromObjectValues = args.JsonFunctions.ToJson(item),
                    ToObjectValues = string.Empty,
                    DateCreated = DateTime.Now
                });

                args.ResponseMaps.Add(new Entity.HistoryResponseMap
                {
                    PubSubscriptionDetailID = item.PubSubscriptionDetailID,
                    PubSubscriptionID = args.PubSubscriptionId,
                    SubscriptionID = args.SubscriptionId,
                    CodeSheetID = item.CodeSheetID,
                    DateCreated = DateTime.Now,
                    CreatedByUserID = args.UserId,
                    ResponseOther = item.ResponseOther
                });
            }
        }

        private void AddNewResponses(SaveResponseArgs args)
        {
            Guard.NotNull(args, nameof(args));

            int userLogTypeId;

            using (var innerScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                userLogTypeId = args.CodeWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, FrameworkUAD_Lookup.Enums.UserLogTypes.Add.ToString())
                    .CodeId;
                innerScope.Complete();
            }

            foreach (var item in args.AddedList)
            {
                args.UserLogs.Add(new UserLog()
                {
                    ApplicationID = args.ApplicationId,
                    UserLogTypeID = userLogTypeId,
                    UserID = args.UserId,
                    Object = NameProductSubscriptionDetail,
                    FromObjectValues = string.Empty,
                    ToObjectValues = args.JsonFunctions.ToJson(item),
                    DateCreated = DateTime.Now
                });
            }
        }

        private void AddUpdatedResponses(SaveResponseArgs args)
        {
            Guard.NotNull(args, nameof(args));

            int userLogTypeId;
            using (var innerScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                userLogTypeId = args.CodeWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, FrameworkUAD_Lookup.Enums.UserLogTypes.Edit.ToString())
                    .CodeId;
                innerScope.Complete();
            }

            foreach (var item in args.PubSubDetailChanges)
            {
                args.UserLogs.Add(new UserLog()
                {
                    ApplicationID = args.ApplicationId,
                    UserLogTypeID = userLogTypeId,
                    UserID = args.UserId,
                    Object = NameProductSubscriptionDetail,
                    FromObjectValues = args.JsonFunctions.ToJson(item.OriginalValues),
                    ToObjectValues = args.JsonFunctions.ToJson(item.NewValues),
                    DateCreated = DateTime.Now
                });
            }
        }

        private IEnumerable<int> SaveLogs(SaveResponseArgs args)
        {
            Guard.NotNull(args, nameof(args));

            var results = args.PubSubDetailWorker.ProductSubscriptionDetailUpdateBulkSql(args.Client.ClientConnections, args.CurrentAnswers);

            foreach (var item in args.ResponseMaps)
            {
                var subDetail = results.FirstOrDefault(x => x.CodeSheetID == item.CodeSheetID);
                if (subDetail != null)
                {
                    item.PubSubscriptionDetailID = subDetail.PubSubscriptionDetailID;
                }
                item.HistorySubscriptionID = args.HistoryId;
                item.CreatedByUserID = args.UserId;
            }

            List<UserLog> userLogList;
            using (var innerScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var userLog = new KMPlatform.BusinessLogic.UserLog();
                userLogList = userLog.SaveBulkInsert(args.UserLogs, args.Client);
                innerScope.Complete();
            }

            var userLogIds = userLogList.Select(u => u.UserLogID);
            new HistoryResponseMap().SaveBulkUpdate(args.ResponseMaps, args.Client.ClientConnections);

            return userLogIds;
        }

        private List<int> SaveMarketingMap(int pubSubscriptionID, int applicationID, int userID, KMPlatform.BusinessLogic.Enums.UserLogTypes ult, List<Entity.MarketingMap> permissions,
            KMPlatform.Entity.Client c, int historyID)
        {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            FrameworkUAD.BusinessLogic.MarketingMap mmW = new MarketingMap();
            KMPlatform.BusinessLogic.UserLog userLogW = new KMPlatform.BusinessLogic.UserLog();
            FrameworkUAD.BusinessLogic.HistoryMarketingMap historyMMW = new HistoryMarketingMap();
            FrameworkUAD.BusinessLogic.History historyW = new History();
            List<int> userLogIDs = new List<int>();
            List<Entity.MarketingMap> origPermissions = new List<Entity.MarketingMap>();

            origPermissions = mmW.SelectSubscriber(pubSubscriptionID, c.ClientConnections);


            if (origPermissions.Count == 0)
            {
                //origPermissions = permissions.Clone();
                origPermissions = permissions.Select(x => Core_AMS.Utilities.ObjectFunctions.DeepCopy(x)).ToList();
                origPermissions.ForEach(x => x.IsActive = false);
            }

            List<KMPlatform.Entity.UserLog> userLogList = new List<KMPlatform.Entity.UserLog>();
            List<FrameworkUAD.Entity.HistoryMarketingMap> hmmList = new List<FrameworkUAD.Entity.HistoryMarketingMap>();
            List<KMPlatform.Entity.UserLog> ulList = new List<KMPlatform.Entity.UserLog>();

            int userLogTypeID = 0;
            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();

            userLogTypeID = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, ult.ToString()).CodeId;

            permissions.ForEach(x =>
            {
                x.PubSubscriptionID = pubSubscriptionID;
            });

            var addedList = from b in permissions
                            join a in origPermissions on b.MarketingID equals a.MarketingID
                            where b.IsActive != a.IsActive && b.IsActive == true
                            select b;
            var removedList = from b in permissions
                              join a in origPermissions on b.MarketingID equals a.MarketingID
                              where b.IsActive != a.IsActive && b.IsActive == false
                              select b;

            #region Removed Responses
            foreach (FrameworkUAD.Entity.MarketingMap mm in removedList)
            {
                ulList.Add(new KMPlatform.Entity.UserLog()
                {
                    ApplicationID = applicationID,
                    UserLogTypeID = userLogTypeID,
                    UserID = userID,
                    Object = "MarketingMap",
                    FromObjectValues = jf.ToJson<FrameworkUAD.Entity.MarketingMap>(mm),
                    ToObjectValues = "",
                    DateCreated = DateTime.Now
                });

                hmmList.Add(new FrameworkUAD.Entity.HistoryMarketingMap()
                {
                    MarketingID = mm.MarketingID,
                    PubSubscriptionID = pubSubscriptionID,
                    PublicationID = mm.PublicationID,
                    IsActive = mm.IsActive,
                    DateCreated = DateTime.Now,
                    CreatedByUserID = userID
                });
            }
            #endregion
            #region New Responses
            foreach (FrameworkUAD.Entity.MarketingMap mm in addedList)
            {
                ulList.Add(new KMPlatform.Entity.UserLog()
                {
                    ApplicationID = applicationID,
                    UserLogTypeID = userLogTypeID,
                    UserID = userID,
                    Object = "MarketingMap",
                    FromObjectValues = "",
                    ToObjectValues = jf.ToJson<FrameworkUAD.Entity.MarketingMap>(mm),
                    DateCreated = DateTime.Now
                });

                hmmList.Add(new FrameworkUAD.Entity.HistoryMarketingMap()
                {
                    MarketingID = mm.MarketingID,
                    PubSubscriptionID = pubSubscriptionID,
                    PublicationID = mm.PublicationID,
                    IsActive = mm.IsActive,
                    DateCreated = DateTime.Now,
                    CreatedByUserID = userID
                });
            }
            #endregion

            mmW.SaveBulkUpdate(permissions, c.ClientConnections);

            foreach (var x in ulList)
            {
                if (x.FromObjectValues == null)
                    x.FromObjectValues = "";
                if (x.ToObjectValues == null)
                    x.ToObjectValues = "";
            }

            userLogList = userLogW.SaveBulkInsert(ulList, c);

            foreach (var x in userLogList)
            {
                userLogIDs.Add(x.UserLogID);
            }

            List<FrameworkUAD.Entity.HistoryMarketingMap> hmmID = new List<FrameworkUAD.Entity.HistoryMarketingMap>();
            hmmID = historyMMW.SaveBulkUpdate(hmmList, c.ClientConnections);

            historyW.Insert_History_To_HistoryMarketingMap_List(historyID, hmmID, c.ClientConnections);

            return userLogIDs;
        }

        private FrameworkUAD.Entity.Batch SaveBatch(int productID, FrameworkUAS.Object.Batch b, int userID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.BusinessLogic.Batch batchW = new Batch();

            if (b != null && b.PublicationID == productID && b.IsActive == true && b.BatchCount < 100)
            {
                FrameworkUAD.Entity.Batch uadBatch = new FrameworkUAD.Entity.Batch();
                b.BatchCount = ++b.BatchCount;
                uadBatch.BatchCount = b.BatchCount;
                uadBatch.BatchID = b.BatchID;
                uadBatch.BatchNumber = b.BatchNumber;
                uadBatch.DateCreated = b.DateCreated;
                uadBatch.DateFinalized = b.DateFinalized;
                uadBatch.IsActive = b.IsActive;
                uadBatch.PublicationID = b.PublicationID;
                uadBatch.UserID = b.UserID;

                batchW.Save(uadBatch, client);
                return uadBatch;
            }
            else
            {
                if (productID > 0)
                {
                    FrameworkUAS.Service.Response<FrameworkUAD.Entity.Batch> newBatchResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Batch>();
                    FrameworkUAD.Entity.Batch newBatch = new FrameworkUAD.Entity.Batch();
                    FrameworkUAS.Object.Batch uasBatch = new FrameworkUAS.Object.Batch();

                    if (b != null && b.BatchID > 0 && b.BatchCount >= 100)
                    {
                        newBatch.BatchCount = b.BatchCount;
                        newBatch.BatchID = b.BatchID;
                        newBatch.BatchNumber = b.BatchNumber;
                        newBatch.DateCreated = b.DateCreated;
                        newBatch.DateFinalized = DateTime.Now;
                        newBatch.IsActive = false;
                        newBatch.PublicationID = b.PublicationID;
                        newBatch.UserID = b.UserID;

                        batchW.Save(newBatch, client);
                    }
                    newBatch = batchW.StartNewBatch(userID, productID, client);
                    return newBatch;
                }
                else
                    return null;
            }
        }
        #endregion

        #region Circ Merge

        public List<Entity.ProductSubscription> Search(KMPlatform.Object.ClientConnections client, string clientDisplayName, string fName = "", string lName = "", string company = "", string title = "", string add1 = "", string city = "", string regionCode = "", string zip = "", string country = "", string email = "", string phone = "", int sequenceID = 0, string account = "", int publisherId = 0, int publicationId = 0, int subscriptionID = 0)
        {
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.Search(client, clientDisplayName, fName, lName, company, title, add1, city, regionCode, zip, country, email, phone, sequenceID, account, publisherId, publicationId,subscriptionID).ToList();
            foreach (Entity.ProductSubscription s in x)
            {
                //BindCustomProperties(s);
                s.IsNewSubscription = false;
                FormatPublicationToolTip(s);
            }
            return FormatZipCode(x);
        }
        public List<Entity.ProductSubscription> SearchSuggestMatch(KMPlatform.Object.ClientConnections client, int publisherId, int publicationId, string firstName = "", string lastName = "", string email = "")
        {
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.SearchSuggestMatch(client, publisherId, publicationId, firstName, lastName, email).ToList();
            foreach (Entity.ProductSubscription s in x)
            {
                //BindCustomProperties(s);
                s.IsNewSubscription = false;
                FormatPublicationToolTip(s);
            }
            return FormatZipCode(x);
        }
        public Entity.ProductSubscription FormatPublicationToolTip(Entity.ProductSubscription x)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Publications");
            sb.AppendLine(string.Empty);

            if (x.PublicationToolTip != null)
            {
                string[] pubs = x.PublicationToolTip.Split(',');
                foreach (string s in pubs)
                    sb.AppendLine(s);

                x.PublicationToolTip = sb.ToString();
            }
            else
                x.PublicationToolTip = string.Empty;
            return FormatZipCode(x);
        }
        public Entity.ProductSubscription BindPublicationList(Entity.ProductSubscription x)
        {
            //FrameworkUAD.Entity.Product pubWorker = new FrameworkUAD.Entity.Product();
            //x.PublicationList = pubWorker.SelectSubscription(x.SubscriptionID).ToList();
            return FormatZipCode(x);
        }
        public Entity.ProductSubscription BindCustomProperties(Entity.ProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            MarketingMap mmWorker = new MarketingMap();
            Prospect pWorker = new Prospect();
            //SubscriptionSearchResult ssrWorker = new SubscriptionSearchResult();
            //Subscription sWorker = new Subscription();
            //SubscriptionResponseMap srmWorker = new SubscriptionResponseMap();

            ProductSubscriptionDetail pubSubDetailWorker = new ProductSubscriptionDetail();
            //Product pubWorker = new Product();

            x.MarketingMapList = mmWorker.SelectSubscriber(x.SubscriptionID, client).ToList();//SubscriptionMarketingMap.Select(x.SubscriptionID).ToList();
            x.ProspectList = pWorker.Select(x.SubscriptionID, client).ToList();
            //x.SubscriptionSearchResults = ssrWorker.Select(x.SubscriptionID, client).ToList();
            //x.SubscriptionList = sWorker.SelectSubscriber(x.SubscriptionID).ToList();
            //x.ResponseMapList = srmWorker.SelectSubscription(x.SubscriptionID, client).ToList();
            x.ProductMapList = pubSubDetailWorker.Select(x.PubSubscriptionID, client).ToList();

            //x.PublicationList = pubWorker.SelectSubscription(x.SubscriptionID).ToList();

            return FormatZipCode(x);
        }
        public List<Entity.ProductSubscription> SelectPublication(int productID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.SelectPublication(productID, client, clientDisplayName).ToList();
            foreach (Entity.ProductSubscription s in x)
            {
                s.IsNewSubscription = false;
            }
            return FormatZipCode(x);
        }
        public List<Entity.ProductSubscription> SelectSequence(int sequenceID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.SelectSequence(sequenceID, client);
            foreach (Entity.ProductSubscription s in x)
            {
                s.IsNewSubscription = false;
            }
            return FormatZipCode(x);
        }
        public List<Entity.ProductSubscription> SelectSequence(List<string> sequenceIDs, KMPlatform.Object.ClientConnections client)
        {
            StringBuilder sb = new StringBuilder();
            //join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
            foreach (string afd in sequenceIDs)
            {
                sb.Append(afd + ",");
            }
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.SelectSequence(sb.ToString().TrimEnd(','), client);
            foreach (Entity.ProductSubscription s in x)
            {
                s.IsNewSubscription = false;
            }
            return FormatZipCode(x);
        }
        public List<Entity.ProductSubscription> SelectPaging(int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.SelectPaging(page, pageSize, productID, client, clientDisplayName);
            return FormatZipCode(x);
        }
        public List<Entity.ProductSubscription> SearchAddressZip(string address1, string zipCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscription> x = null;
            x = DataAccess.ProductSubscription.SearchAddressZip(address1, zipCode, client).ToList();
            foreach (Entity.ProductSubscription s in x)
            {
                //BindCustomProperties(s);
                // s.IsNewSubscriber = false;
                FormatPublicationToolTip(s);
            }
            return FormatZipCode(x);
        }


        public int UpdateLock(int SubscriptionID, bool IsLocked, int UserID, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SubscriptionID = DataAccess.ProductSubscription.UpdateSubscription(SubscriptionID, IsLocked, UserID, client);
                scope.Complete();
            }

            return SubscriptionID;
        }
        public int UpdateQDate(int SubscriptionID, DateTime? QSourceDate, int UpdatedByUserID, KMPlatform.Object.ClientConnections client)
        {
            int ID = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                ID = DataAccess.ProductSubscription.UpdateQDate(SubscriptionID, QSourceDate, UpdatedByUserID, client);
                scope.Complete();
            }

            return ID;
        }
        public bool ClearWaveMailingInfo(int waveMailingDetail, KMPlatform.Object.ClientConnections client)
        {
            bool success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                success = DataAccess.ProductSubscription.ClearWaveMailingInfo(waveMailingDetail, client);
                scope.Complete();
            }
            return success;
        }
        public bool ClearIMBSeq(int productID, KMPlatform.Object.ClientConnections client)
        {
            bool success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                success = DataAccess.ProductSubscription.ClearIMBSeq(productID, client);
                scope.Complete();
            }
            return success;
        }
        public bool SaveBulkWaveMailing(string xml, int waveMailingID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.ProductSubscription.SaveBulkWaveMailing(xml, waveMailingID, client);
                scope.Complete();
            }

            return complete;
        }
        public int SelectCount(int productID, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.ProductSubscription.SelectCount(productID, client);
        }
        public bool SaveBulkActionIDUpdate(string xml, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.ProductSubscription.SaveBulkActionIDUpdate(xml, client);
                scope.Complete();
            }

            return complete;
        }
        public int Save(Entity.ProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
            x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
            x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);

            //DO NOT DELETE.
            //if (x.IsInActiveWaveMailing == false && x.Country.ToLower() == FrameworkUAD_Lookup.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ").ToLower())
            //{
            //    FrameworkUAD_Lookup.BusinessLogic.ZipCode zipW = new FrameworkUAD_Lookup.BusinessLogic.ZipCode();
            //    FrameworkUAD_Lookup.BusinessLogic.Region regionW = new FrameworkUAD_Lookup.BusinessLogic.Region();
            //    List<FrameworkUAD_Lookup.Entity.ZipCode> zips = zipW.Select();

            //    FrameworkUAD_Lookup.Entity.ZipCode zip = new FrameworkUAD_Lookup.Entity.ZipCode();
            //    if(!string.IsNullOrEmpty(x.ZipCode))
            //    {
            //        zip = zips.Where(y => y.Zip == x.ZipCode && y.CountryId == x.CountryID).FirstOrDefault();
            //    }
            //    else if(!string.IsNullOrEmpty(x.City) && x.RegionID > 0)
            //        zip = zips.Where(y => y.PrimaryCity == x.City && y.CountryId == x.CountryID && x.RegionID == y.RegionId).FirstOrDefault();
            //    if (zip != null && zip.ZipCodeId > 0)
            //    {
            //        if (zip.PrimaryCity != x.City)
            //            x.City = zip.PrimaryCity;
            //        if (zip.RegionId != x.RegionID)
            //        {
            //            FrameworkUAD_Lookup.Entity.Region r = regionW.Select().Where(y => y.RegionID == zip.RegionId).FirstOrDefault();
            //            x.RegionID = (zip.RegionId ?? 0);
            //            x.RegionCode = r.RegionCode;
            //        }
            //        if (zip.County != x.County)
            //            x.County = zip.County;
            //    }
            //}

            using (TransactionScope scope = new TransactionScope())
            {
                x.SubscriptionID = DataAccess.ProductSubscription.Save(x, client);
                scope.Complete();
            }

            return x.SubscriptionID;
        }

        /// <summary>
        /// Goes through DQM Process and will return SubscriptionID and PubSubscriptionID
        /// </summary>
        /// <param name="PubID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Dictionary<int, int> SaveDQM(Entity.ProductSubscription x, string processCode, string clientName, string clientDisplayName, int clientID, KMPlatform.Object.ClientConnections client, int fileRecurrenceTypeId, int databaseFileTypeId)
        {
            x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
            x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
            x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);

            //insert to SO and ST then DQMQue
            //string processCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();

            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.Entity.SourceFile sourceFile = sfData.Select(clientID, "UAD_WS_AddSubscriber");
            if (sourceFile == null)
            {


                KMPlatform.BusinessLogic.Service sworker = new KMPlatform.BusinessLogic.Service();
                KMPlatform.Entity.Service s = sworker.Select(KMPlatform.Enums.Services.UADFILEMAPPER, true);

                //create the UAD_WS_AddSubscriber file for the client
                sourceFile = new FrameworkUAS.Entity.SourceFile();
                sourceFile.FileRecurrenceTypeId = fileRecurrenceTypeId;// codes.Single(y => y.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.Recurring.ToString())).CodeId;
                sourceFile.DatabaseFileTypeId = databaseFileTypeId;// dbFiletypes.Single(y => y.CodeName.Equals(FrameworkUAD_Lookup.Enums.GetDatabaseFileType(FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString()))).CodeId;
                sourceFile.FileName = "UAD_WS_AddSubscriber";
                sourceFile.ClientID = clientID;
                sourceFile.IsDQMReady = true;
                sourceFile.ServiceID = s.ServiceID;
                sourceFile.ServiceFeatureID = s.ServiceFeatures.Single(y => y.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.UAD_Api.ToString().Replace("_", " "))).ServiceFeatureID;
                sourceFile.CreatedByUserID = 1;
                sourceFile.QDateFormat = "MMDDYYYY";
                sourceFile.BatchSize = 2500;
                sourceFile.SourceFileID = sfData.Save(sourceFile);
            }

            SaveSubscriberTransformed(x, client, processCode, sourceFile.SourceFileID);
            //now call DQM sprocs pass ProcessCode

            DataAccess.ProductSubscription.CountryRegionCleanse(sourceFile.SourceFileID, processCode, client);

            FrameworkUAD.BusinessLogic.SubscriberFinal sf = new FrameworkUAD.BusinessLogic.SubscriberFinal();
            FrameworkUAD_Lookup.BusinessLogic.Code lookupWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> dbFiletypes = lookupWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);
            string fileType = string.Empty;
            if (dbFiletypes.Exists(y => y.CodeId == databaseFileTypeId))
                fileType = dbFiletypes.Single(y => y.CodeId == databaseFileTypeId).CodeName;

            sf.SaveDQMClean(client, processCode, fileType);

            FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
            stWorker.DataMatching(client, sourceFile.SourceFileID, processCode);
            stWorker.StandardRollUpToMaster(client, sourceFile.SourceFileID, processCode);

            DataAccess.ProductSubscription.UpdateMasterDB(client, processCode);
            DataAccess.ProductSubscription.DedupeMasterDB(client, processCode);

            //once done call SubscriberFinal by ProcessCode then can call PubSubscriptions by SFRecordIdentifier
            //will then have PubSubscriptionID and SubscriptionID
            FrameworkUAD.Entity.ProductSubscription ps = new Entity.ProductSubscription();
            ps = DataAccess.ProductSubscription.SelectProcessCode(processCode, client, clientDisplayName);

            Dictionary<int, int> retItem = new Dictionary<int, int>();
            if (ps != null)
                retItem.Add(ps.SubscriptionID, ps.PubSubscriptionID);
            return retItem;
        }
        private void SaveSubscriberTransformed(Entity.ProductSubscription x, KMPlatform.Object.ClientConnections client, string processCode, int sourceFileID)
        {
            try
            {
                #region x Null Value Checks
                if (x.Address1 == null)
                    x.Address1 = string.Empty;
                if (x.Address2 == null)
                    x.Address2 = string.Empty;
                if (x.Address3 == null)
                    x.Address3 = string.Empty;

                if (x.City == null)
                    x.City = string.Empty;
                if (x.Company == null)
                    x.Company = string.Empty;
                if (x.Country == null)
                    x.Country = string.Empty;

                if (x.County == null)
                    x.County = string.Empty;

                if (x.Fax == null)
                    x.Fax = string.Empty;

                if (x.FirstName == null)
                    x.FirstName = string.Empty;
                if (x.Gender == null)
                    x.Gender = string.Empty;
                if (x.LastName == null)
                    x.LastName = string.Empty;
                if (x.Mobile == null)
                    x.Mobile = string.Empty;

                if (x.Phone == null)
                    x.Phone = string.Empty;

                if (x.Plus4 == null)
                    x.Plus4 = string.Empty;

                if (x.RegionCode == null)
                    x.RegionCode = string.Empty;

                if (x.Title == null)
                    x.Title = string.Empty;

                if (x.ZipCode == null)
                    x.ZipCode = string.Empty;

                #endregion
                #region ProductSubscription Null Value Checks
                if (x.Demo7 == null)
                    x.Demo7 = string.Empty;
                if (x.Email == null)
                    x.Email = string.Empty;
                if (x.EmailStatusID == 0)
                    x.EmailStatusID = 1;
                if (x.PubCode == null)
                    x.PubCode = string.Empty;
                if (x.PubName == null)
                    x.PubName = string.Empty;
                if (x.PubTypeDisplayName == null)
                    x.PubTypeDisplayName = string.Empty;
                if (x.QualificationDate == null)
                    x.QualificationDate = DateTime.Now;
                if (x.Status == null)
                    x.Status = FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString();
                if (x.StatusUpdatedDate == null)
                    x.StatusUpdatedDate = DateTime.Now;
                if (x.StatusUpdatedReason == null)
                    x.StatusUpdatedReason = "Subscribed";
                if (x.QualificationDate == null)
                    x.QualificationDate = DateTimeFunctions.GetMinDate();
                #endregion
                List<FrameworkUAD.Entity.SubscriberOriginal> addSO = new List<FrameworkUAD.Entity.SubscriberOriginal>();
                List<FrameworkUAD.Entity.SubscriberTransformed> addST = new List<FrameworkUAD.Entity.SubscriberTransformed>();

                FrameworkUAD.Entity.SubscriberOriginal newSO = new FrameworkUAD.Entity.SubscriberOriginal();

                #region SO
                newSO.ProcessCode = processCode;
                newSO.Address = x.Address1;
                newSO.Address3 = x.Address3;
                newSO.CategoryID = x.PubCategoryID > 0 ? x.PubCategoryID : 0;///////////////////////////////////
                newSO.City = x.City;
                newSO.Company = x.Company;
                newSO.Country = x.Country;
                newSO.County = x.County;
                newSO.CreatedByUserID = 1;
                newSO.DateCreated = DateTime.Now;
                newSO.Demo7 = x.Demo7.Length > 0 ? x.Demo7 : string.Empty;///////////////////////////////////
                newSO.Email = x.Email.Length > 0 ? x.Email : string.Empty;///////////////////////////////////////////
                //newSO.EmailExists = newSO.Email.Length > 0 ? true : false;
                newSO.EmailStatusID = x.EmailStatusID;
                newSO.Fax = x.Fax;
                newSO.FName = x.FirstName;
                newSO.Gender = x.Gender;
                //newSO.IsDQMProcessFinished = false;
                //newSO.IsMailable = !string.IsNullOrEmpty(x.Address1) ? true : false;
                //newSO.IsUpdatedInLive = false;
                newSO.LName = x.LastName;
                newSO.Mobile = x.Mobile;
                newSO.Phone = x.Phone;
                newSO.Plus4 = x.Plus4;
                newSO.PubCode = x.PubCode;//////////////////////////////
                //newSO.PubIDs = x.PubID.ToString();/////////////////////////////
                newSO.ProcessCode = processCode;
                newSO.QDate = x.QualificationDate.ToString().Length > 0 ? x.QualificationDate : DateTime.Now;/////////////////////////////
                newSO.QSourceID = x.PubQSourceID > 0 ? x.PubQSourceID : 0;//////////////////////////////////////////////
                newSO.Sequence = x.SequenceID;
                newSO.SourceFileID = sourceFileID;
                newSO.State = x.RegionCode;
                //newSO.StatusUpdatedDate = x.StatusUpdatedDate.ToString().Length > 0 ? x.StatusUpdatedDate : DateTime.Now;
                //newSO.StatusUpdatedReason = x.StatusUpdatedReason;
                newSO.Title = x.Title;
                newSO.TransactionID = x.PubTransactionID > 0 ? x.PubTransactionID : 0;///////////////////////////////
                newSO.Zip = x.ZipCode;
                newSO.IsActive = x.IsActive;
                newSO.Par3C = x.Par3CID.ToString();
                newSO.Verified = x.Verify;
                newSO.SubSrc = x.SubSrcID.ToString();
                #endregion
                addSO.Add(newSO);
                FrameworkUAD.Entity.SubscriberTransformed newST = new FrameworkUAD.Entity.SubscriberTransformed();
                #region ST
                newST.ProcessCode = processCode;
                newST.Address = x.Address1;
                newST.Address3 = x.Address3;
                newST.CategoryID = x.PubCategoryID > 0 ? x.PubCategoryID : 0;///////////////////////////////////
                newST.City = x.City;
                newST.Company = x.Company;
                newST.Country = x.Country;
                newST.County = x.County;
                newST.CreatedByUserID = 1;
                newST.DateCreated = DateTime.Now;
                newST.Demo7 = x.Demo7.Length > 0 ? x.Demo7 : string.Empty;///////////////////////////////////
                newST.Email = x.Email.Length > 0 ? x.Email : string.Empty;///////////////////////////////////////////
                //newST.EmailExists = newST.Email.Length > 0 ? true : false;
                newST.EmailStatusID = x.EmailStatusID;
                newST.Fax = x.Fax;
                newST.FName = x.FirstName;
                newST.Gender = x.Gender;
                //newST.IsDQMProcessFinished = false;
                //newST.IsMailable = false;
                //newST.IsMailable = !string.IsNullOrEmpty(x.Address1) ? true : false;
                //newST.IsUpdatedInLive = false;
                newST.LName = x.LastName;
                newST.Mobile = x.Mobile;
                newST.Phone = x.Phone;
                newST.Plus4 = x.Plus4;
                newST.PubCode = x.PubCode;//////////////////////////////
                //newST.PubIDs = x.PubID.ToString();/////////////////////////////
                newST.QDate = x.QualificationDate.ToString().Length > 0 ? x.QualificationDate : DateTime.Now;/////////////////////////////
                newST.QSourceID = x.PubQSourceID > 0 ? x.PubQSourceID : 0;//////////////////////////////////////////////
                newST.Sequence = x.SequenceID;
                newST.SORecordIdentifier = newSO.SORecordIdentifier;
                newST.SourceFileID = sourceFileID;
                newST.State = x.RegionCode;
                //newST.StatusUpdatedDate = x.StatusUpdatedDate.ToString().Length > 0 ? x.StatusUpdatedDate : DateTime.Now;
                //newST.StatusUpdatedReason = x.StatusUpdatedReason;
                newST.Title = x.Title;
                newST.TransactionID = x.PubTransactionID > 0 ? x.PubTransactionID : 0;///////////////////////////////
                newST.Zip = x.ZipCode;
                newST.IsActive = x.IsActive;
                newST.Par3C = x.Par3CID.ToString();
                newST.Verified = x.Verify;
                newST.SubSrc = x.SubSrcID.ToString();
                newST.SubGenBillingAddressId = x.SubGenBillingAddressId;
                newST.SubGenMailingAddressId = x.SubGenMailingAddressId;
                newST.SubGenPublicationID = x.SubGenPublicationID;
                newST.SubGenSubscriberID = x.SubGenSubscriberID;
                newST.SubGenSubscriptionID = x.SubGenSubscriptionID;
                newST.IssuesLeft = x.IssuesLeft;
                newST.UnearnedReveue = x.UnearnedReveue;
                #endregion
                addST.Add(newST);

                //newST.DemographicTransformedList
                FrameworkUAD.BusinessLogic.SubscriberOriginal soData = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
                FrameworkUAD.BusinessLogic.SubscriberTransformed stData = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                soData.SaveBulkSqlInsert(addSO, client);
                stData.SaveBulkSqlInsert(addST, client, false);
            }
            catch (Exception ex)
            {
                #region Log Error
                string errorMsg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-25, -1, errorMsg + " ThreadID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), processCode));
                #endregion
            }
        }

        #endregion

        #region Simple ProductSubscriptions
        public List<Entity.ActionProductSubscription> SelectProductID(int PubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ActionProductSubscription> retList = null;
            retList = DataAccess.ProductSubscription.SelectProductID(PubID, client);

            return retList;
        }
        public List<Entity.ActionProductSubscription> SelectProductID(int PubID, int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ActionProductSubscription> retList = null;
            retList = DataAccess.ProductSubscription.SelectProductID(PubID, issueID, client);

            return retList;
        }
        public List<Entity.CopiesProductSubscription> SelectAllActiveIDs(int productID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.CopiesProductSubscription> retList = new List<Entity.CopiesProductSubscription>();
            retList = DataAccess.ProductSubscription.SelectAllActiveIDs(productID, client);
            return retList;
        }
        #endregion

        public List<Entity.ProductSubscription> SelectForUpdate(int productID, int issueid, List<int> pubsubs, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscription> retItem = null;
            string s = "<XML>";
            pubsubs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            retItem = DataAccess.ProductSubscription.SelectForUpdate(productID, issueid, s, client);

            foreach (Entity.ProductSubscription ps in retItem)
            {
                ps.ProductMapList = DataAccess.ProductSubscriptionDetail.Select(ps.PubSubscriptionID, client).ToList();

                DataTable result = new DataTable();
                List<FrameworkUAD.Object.PubSubscriptionAdHoc> lst = new List<Object.PubSubscriptionAdHoc>();
                result = DataAccess.PubSubscriptionsExtension.GetAdHoc(client, ps.PubSubscriptionID, productID);
                Dictionary<string, string> adhocs = new Dictionary<string, string>();
                foreach (DataRow dr in result.Rows)
                {
                    foreach (DataColumn dc in result.Columns)
                    {
                        lst.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc(dc.ColumnName, dr[dc].ToString()));
                    }
                }
                ps.AdHocFields = lst;
            }

            return retItem;
        }

        public bool RecordUpdate(HashSet<int> pubSubscriptionIds, string changes, int issueid, int productID, int userid, KMPlatform.Object.ClientConnections client)
        { 
            bool success = false;

            string s = "<XML>";
            pubSubscriptionIds.ToList().ForEach(x => s += "<ID>" + x.ToString() + "</ID>");
            s += "</XML>";

            success = DataAccess.ProductSubscription.RecordUpdate(s, changes, issueid, productID, userid, client);
            
            return success;
        }

        internal class ProductSubscriptionDetailChange
        {
            public FrameworkUAD.Entity.ProductSubscriptionDetail OriginalValues { get; set; }
            public FrameworkUAD.Entity.ProductSubscriptionDetail NewValues { get; set; }

            public ProductSubscriptionDetailChange(FrameworkUAD.Entity.ProductSubscriptionDetail origValues, FrameworkUAD.Entity.ProductSubscriptionDetail newValues)
            {
                this.OriginalValues = origValues;
                this.NewValues = newValues;
            }
        }
    }
}
