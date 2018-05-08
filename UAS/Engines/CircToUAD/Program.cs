using ADMS.Services.Emailer;
using Core.ADMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CircToUAD
{
    class Program
    {

        #region Workers
        //private static KMPlatform.BusinessLogic.Client clientWorker;
        ////private static KMPlatform.BusinessLogic.Client publisherWorker;
        ////private static FrameworkUAD.BusinessLogic.Product publicationWorker;
        //private static FrameworkCirculation.BusinessLogic.HistorySubscription hsWorker;
        //private static FrameworkUAD.BusinessLogic.Product productWorker;
        //private static FrameworkUAD.BusinessLogic.ProductTypes prodTypeWorker;
        //private static FrameworkCirculation.BusinessLogic.Action actionWorker;
        //private static FrameworkCirculation.BusinessLogic.Deliverability deliverWorker;
        //private static FrameworkUAD.BusinessLogic.SubscriptionResponseMap srmWorker;
        ////private static FrameworkUAD.BusinessLogic.Response responseWorker;
        //private static FrameworkUAD.BusinessLogic.CodeSheet codeSheetWorker;
        //private static FrameworkCirculation.BusinessLogic.Marketing marketingWorker;
        //private static FrameworkCirculation.BusinessLogic.MarketingMap marketingMapWorker;
        //private static FrameworkCirculation.BusinessLogic.CategoryCode categoryCodeWorker;
        //private static FrameworkCirculation.BusinessLogic.TransactionCode transactionCodeWorker;
        #endregion
        #region Entity/List
        //private static List<KMPlatform.Entity.Client> clientList;
        ////private static List<FrameworkCirculation.Entity.Publisher> publisherList;
        ////private static List<FrameworkCirculation.Entity.Publication> publicationList;
        //private static List<FrameworkUAD.Object.SaveSubscriber> subscriberList;
        //private static List<FrameworkUAD.Entity.Product> productList;
        //private static List<FrameworkCirculation.Entity.HistorySubscription> histSubscriptionList;
        //private static List<FrameworkUAD.Entity.ProductTypes> prodTypeList;
        //private static List<FrameworkCirculation.Entity.Action> actionList;
        //private static List<FrameworkCirculation.Entity.Deliverability> deliverList;
        //private static List<FrameworkUAD.Entity.Response> responseList;
        //private static List<FrameworkUAD.Entity.CodeSheet> codeSheetList;
        //private static List<FrameworkCirculation.Entity.Marketing> marketingList;
        //private static List<FrameworkCirculation.Entity.CategoryCode> categoryCodeList;
        //private static List<FrameworkCirculation.Entity.TransactionCode> transactionCodeList;
        //#endregion
        //#region Others
        ////private static int count = 0;
        //private static bool isRunning = false;
        //private static System.Timers.Timer myTimer;
        //private static ADMS.Services.Validator.Validator val;
        #endregion

        public static void Main(string[] args)
        {
        //    ConsoleMessage(DateTime.Now + " Circulation to UAD Engine");
        //    int min = 0;
        //    int.TryParse(ConfigurationManager.AppSettings["ExecuteCircToUADMinutes"].ToString(), out min);
        //    min = min * 60000;

        //    myTimer = new Timer(min);
        //    myTimer.Start();
        //    myTimer.Elapsed += new ElapsedEventHandler(SendCirculationData);

        //    #region Load List that are least likely to change
        //    clientWorker = new KMPlatform.BusinessLogic.Client();
        //    clientList = clientWorker.Select();

        //    //publisherWorker = new FrameworkCirculation.BusinessLogic.Publisher();
        //    //publisherList = publisherWorker.Select();

        //    actionWorker = new FrameworkCirculation.BusinessLogic.Action();
        //    actionList = actionWorker.Select();

        //    deliverWorker = new FrameworkCirculation.BusinessLogic.Deliverability();
        //    deliverList = deliverWorker.Select();

        //    marketingWorker = new FrameworkCirculation.BusinessLogic.Marketing();
        //    marketingList = marketingWorker.Select();

        //    categoryCodeWorker = new FrameworkCirculation.BusinessLogic.CategoryCode();
        //    categoryCodeList = categoryCodeWorker.Select();

        //    transactionCodeWorker = new FrameworkCirculation.BusinessLogic.TransactionCode();
        //    transactionCodeList = transactionCodeWorker.Select();

        //    #endregion

        //    Console.ReadKey();
        //}
        //private static void SendCirculationData(object source, ElapsedEventArgs e)
        //{
        //    myTimer.Stop();

        //    try
        //    {          
        //        if (isRunning == false)
        //        {
        //            ConsoleMessage("\n" + DateTime.Now + " Start Circulation to UAD Subscriber List Transfer");

        //            isRunning = true;

        //            val = new ADMS.Services.Validator.Validator();
        //            hsWorker = new FrameworkCirculation.BusinessLogic.HistorySubscription();

        //            productWorker = new FrameworkUAD.BusinessLogic.Product();
        //            productList = new List<FrameworkUAD.Entity.Product>();
        //            histSubscriptionList = new List<FrameworkCirculation.Entity.HistorySubscription>();
        //            prodTypeWorker = new FrameworkUAD.BusinessLogic.ProductTypes();
        //            prodTypeList = new List<FrameworkUAD.Entity.ProductTypes>();
        //            srmWorker = new FrameworkUAD.BusinessLogic.SubscriptionResponseMap();
        //            codeSheetWorker = new FrameworkUAD.BusinessLogic.CodeSheet();
        //            responseList = new List<FrameworkUAD.Entity.Response>();
        //            marketingMapWorker = new FrameworkCirculation.BusinessLogic.MarketingMap();

        //            // Get data that has not been uploaded to UAD
        //            ConsoleMessage(DateTime.Now + " Start Get Circulation Subscriber list for transfer");
        //            histSubscriptionList = hsWorker.Select(false);
        //            ConsoleMessage(DateTime.Now + " Done Get Circulation Subscriber list for transfer");

        //            ConsoleMessage(DateTime.Now + " Total Records for transfer: " + histSubscriptionList.Count());

        //            if (histSubscriptionList.Count() > 0)
        //            {
        //                var clientIds = histSubscriptionList.Select(x => x.PublisherID).Distinct();

        //                ConsoleMessage(DateTime.Now + " Distinct Client in Subscriber list: " + clientIds.Count());

        //                // Process in bulk by clientid/publisherId
        //                foreach (int cId in clientIds)
        //                {
        //                    KMPlatform.Entity.Client client = clientList.SingleOrDefault(c => c.ClientID == cId);
        //                    ConsoleMessage(DateTime.Now + " Begin Subscriber List Transform for Client: " + client.ClientName);

        //                    // Get UAD pubs list and Circulation Publication list by client
        //                    productList.AddRange(productWorker.Select(client));

        //                    // Get UAD productTypes list by client
        //                    prodTypeList = prodTypeWorker.Select(client);

        //                    // Get response 
        //                    codeSheetList = codeSheetWorker.Select(client);

        //                    subscriberList = new List<FrameworkUAD.Object.SaveSubscriber>();

        //                    foreach (FrameworkCirculation.Entity.HistorySubscription h in histSubscriptionList)
        //                    {
        //                        if (cId == h.PublisherID)
        //                        {
        //                            FrameworkUAD.Entity.Product p = productList.SingleOrDefault(pr => pr.PubID == h.PublicationID);

        //                            if (p != null)
        //                            {
        //                                // Get Demographics for each subscription  
        //                                List<FrameworkUAD.Object.SubscriberProductDemographic> spdList = new List<FrameworkUAD.Object.SubscriberProductDemographic>();
        //                                List<FrameworkUAD.Entity.SubscriptionResponseMap> srmList = srmWorker.SelectSubscription(client, h.SubscriptionID);

        //                                foreach (var xy in srmList)
        //                                {
        //                                    FrameworkUAD.Entity.CodeSheet rp = codeSheetList.SingleOrDefault(r => r.CodeSheetID == xy.ResponseID);
        //                                    spdList.Add(new FrameworkUAD.Object.SubscriberProductDemographic
        //                                    {
        //                                        Name = rp.ResponseGroup,
        //                                        Value = rp.ResponseValue
        //                                    });
        //                                }

        //                                // Get DEMO31 through DEMO36 which is marketing in Circ
        //                                List<FrameworkCirculation.Entity.MarketingMap> marketingMapList = marketingMapWorker.SelectSubscriber(h.SubscriberID);
        //                                bool Demo31 = marketingMapList.Exists(mm => mm.MarketingID == marketingList.SingleOrDefault(m => m.MarketingName.Equals(FrameworkCirculation.BusinessLogic.Enums.Marketing.Mail.ToString())).MarketingID && mm.SubscriberID == h.SubscriberID);
        //                                bool Demo32 = marketingMapList.Exists(mm => mm.MarketingID == marketingList.SingleOrDefault(m => m.MarketingName.Equals(FrameworkCirculation.BusinessLogic.Enums.Marketing.Fax.ToString())).MarketingID && mm.SubscriberID == h.SubscriberID);
        //                                bool Demo33 = marketingMapList.Exists(mm => mm.MarketingID == marketingList.SingleOrDefault(m => m.MarketingName.Equals(FrameworkCirculation.BusinessLogic.Enums.Marketing.Phone.ToString())).MarketingID && mm.SubscriberID == h.SubscriberID);
        //                                bool Demo34 = marketingMapList.Exists(mm => mm.MarketingID == marketingList.SingleOrDefault(m => m.MarketingName.Equals(FrameworkCirculation.BusinessLogic.Enums.Marketing.Trade_Shows.ToString().Replace("_", " "))).MarketingID && mm.SubscriberID == h.SubscriberID);
        //                                bool Demo35 = marketingMapList.Exists(mm => mm.MarketingID == marketingList.SingleOrDefault(m => m.MarketingName.Equals(FrameworkCirculation.BusinessLogic.Enums.Marketing.Advertising.ToString())).MarketingID && mm.SubscriberID == h.SubscriberID);
        //                                bool Demo36 = marketingMapList.Exists(mm => mm.MarketingID == marketingList.SingleOrDefault(m => m.MarketingName.Equals(FrameworkCirculation.BusinessLogic.Enums.Marketing.Email.ToString())).MarketingID && mm.SubscriberID == h.SubscriberID);

        //                                // Get Values for transactionCode and CategoryCode
        //                                FrameworkCirculation.Entity.Action soloAction = actionList.SingleOrDefault(a => a.ActionID == h.ActionID_Current);
        //                                int catCodeValue = categoryCodeList.SingleOrDefault(v => v.CategoryCodeID == soloAction.CategoryCodeID).CategoryCodeValue;
        //                                int transCodeValue = transactionCodeList.SingleOrDefault(t => t.TransactionCodeID == soloAction.TransactionCodeID).TransactionCodeValue;

        //                                FrameworkUAD.Object.SaveSubscriber s = new FrameworkUAD.Object.SaveSubscriber();

        //                                s.Address = h.Address1;
        //                                s.Address3 = h.Address3;
        //                                s.City = h.City;
        //                                s.Company = h.Company;
        //                                s.Country = h.Country;
        //                                s.County = h.County;
        //                                s.Fax = h.Fax;
        //                                s.FName = h.FirstName;
        //                                s.Gender = h.Gender;
        //                                s.LName = h.LastName;
        //                                s.MailStop = h.Address2;
        //                                s.Mobile = h.Mobile;
        //                                s.Phone = h.Phone;
        //                                s.Plus4 = h.Plus4;
        //                                s.Sequence = h.SequenceID;
        //                                s.State = h.RegionCode;
        //                                s.SubscriptionID = s.SubscriptionID;
        //                                s.Title = h.Title;
        //                                s.Zip = h.ZipCode;
        //                                s.Demo31 = Demo31;
        //                                s.Demo32 = Demo32;
        //                                s.Demo33 = Demo33;
        //                                s.Demo34 = Demo34;
        //                                s.Demo35 = Demo35;
        //                                s.Demo36 = Demo36;
        //                                s.IsActive = h.IsActive;

        //                                string deliverCode = string.Empty;
        //                                if (h.DeliverabilityID > 0)
        //                                    deliverCode = deliverList.SingleOrDefault(d => d.DeliverabilityID == h.DeliverabilityID).DeliverabilityCode;

        //                                s.Products.Add(new FrameworkUAD.Entity.ProductSubscription
        //                                {
        //                                    Demo7 = deliverCode,
        //                                    CreatedByUserID = h.SubscriberCreatedByUserID,
        //                                    DateCreated = DateTime.Now,
        //                                    Email = h.Email,
        //                                    PubCode = p.PubCode,
        //                                    PubID = p.PubID,
        //                                    PubName = p.PubName,
        //                                    PubQSourceID = h.QSourceID,
        //                                    PubSubscriptionID = h.SubscriptionID,// leave blank?
        //                                    PubTransactionID = transCodeValue,
        //                                    PubCategoryID = catCodeValue,
        //                                    PubTypeDisplayName = prodTypeList.SingleOrDefault(f => f.PubTypeID == p.PubTypeID).PubTypeDisplayName,
        //                                    QualificationDate = h.QSourceDate == null ? h.QSourceDate.Value : DateTime.Now,
        //                                    SubscriberProductDemographics = spdList
        //                                });

        //                                subscriberList.Add(s);

        //                                string line = "Subscriber Transformed: " + subscriberList.Count();
        //                                string backup = new string('\b', line.Length);
        //                                Console.Write(backup);
        //                                Console.ForegroundColor = ConsoleColor.Green;
        //                                Console.Write(line);
        //                            }
        //                            else
        //                            {
        //                                FrameworkUAD.Entity.Product prod = productList.SingleOrDefault(pu => pu.PubID == h.PublicationID);
        //                                ConsoleMessage("\n" + DateTime.Now + " Product does not exist: " + prod.PubCode);
        //                            }
        //                        }
        //                    }

        //                    ConsoleMessage("\n" + DateTime.Now + " Send Subscriber List to UAD, Total: " + subscriberList.Count + " for Client: " + client.ClientName);
        //                    // Send subscribers in list per client
        //                    val.SaveSubscribers(client, subscriberList,true);
        //                }

        //                List<int> hsIDs = new List<int>();

        //                hsIDs.AddRange(histSubscriptionList.Select(x => x.HistorySubscriptionID));

        //                ConsoleMessage("\n" + DateTime.Now + " Set IsUadUpdated to true ");
        //                hsWorker.UpdateIsUadUpdated(hsIDs);

        //                ConsoleMessage("\n" + DateTime.Now + " Done Circulation to UAD Subscriber List Transfer ");
        //            }
        //            else
        //                ConsoleMessage(DateTime.Now + " No Records detected for transfer to UAD ");

        //            isRunning = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = Core.Utilities.StringFunctions.FormatException(ex);
        //        ConsoleMessage(message);
        //        Logging logger = new Logging();
        //        logger.LogIssue(ex);

        //        StringBuilder sbDetail = new StringBuilder();
        //        sbDetail.AppendLine("CircToUAD.Program.SendCirculationData - Unhandled Exception");
        //        sbDetail.AppendLine(System.Environment.NewLine);
        //        Emailer emWorker = new Emailer();
        //        emWorker.EmailException(ex, sbDetail.ToString());
        //    }

        //    myTimer.Start();
        //}

        //private static void ConsoleMessage(string message)
        //{
        //    System.Console.ForegroundColor = ConsoleColor.White;
        //    System.Console.WriteLine(message);
        //    System.Console.ResetColor();
        }
    }
}
