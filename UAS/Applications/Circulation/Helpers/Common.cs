using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Circulation.Modules;
using Circulation.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Reflection;

namespace Circulation.Helpers
{
    public class Common
    {
        public class HistoryData
        {
            public int SortIndex { get; set; }
            public string DisplayText { get; set; }
            public string PropertyName { get; set; }

            public HistoryData(int index, string text, string name)
            {
                this.SortIndex = index;
                this.DisplayText = text;
                this.PropertyName = name;
            }
        }
        public static bool HistoryOpened { get; set; }
        public static void OpenCircPopoutWindow(UserControl uc)
        {
            //FrameworkUAS.Object.AppData.myAppData = appData;
            bool openWindow = true;
            HistoryOpened = false;

            foreach (Window w in Application.Current.Windows)
            {
                if (w.Title.Contains("Batch History"))
                    HistoryOpened = true;
            }
            //Run checks before opening Popout window for Circ.
            if (uc.GetType() == typeof(SubscriptionContainer))
            {
                SubscriptionContainer sc = (SubscriptionContainer)uc;
                if (sc.IsNewSubscription)
                {
                    if (FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened) 
                    {
                        Core_AMS.Utilities.WPF.Message("A new subscription windows is already open. Please close before opening a new one.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Open Window Exception");
                        openWindow = false;
                    }
                    else
                        FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened = true;
                } 
                else
                {
                    if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount >= 10)
                    {
                        Core_AMS.Utilities.WPF.Message("Maximum number of subscription edit windows are already open. Please close one and retry.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Open Window Exception");
                        openWindow = false;
                    }
                    else
                        FrameworkUAS.Object.AppData.myAppData.OpenWindowCount++;
                }
            }
            else if(uc.GetType() == typeof(History))
            {
                if (HistoryOpened == true)
                {
                    Core_AMS.Utilities.WPF.Message("Please close other History window before opening a new one.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Open Window Exception");
                    openWindow = false;
                }
                else
                    FrameworkUAS.Object.AppData.myAppData.OpenWindowCount++;
            }

            if (openWindow)
            {
                Popout popout = new Popout(uc);
                //popout.Owner = Core_AMS.Utilities.WPF.GetMainWindow();
                popout.Show();
            }
            else
                return;
        }

        public static void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        public static void PhoneNumberValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        public static void AlphaNumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9a-zA-Z ]").IsMatch(e.Text);
        }

        public static void PhoneAutoFormat(TextBox tb, int characterChange = 0)
        {
            string p = tb.Text;
            Regex rgx = new Regex("[^0-9]");
            int caretLocation = tb.SelectionStart;

            p = rgx.Replace(p, "");
            #region Add dashes
            //Adds dashes
            if (p.Length >= 3 && p.Length <= 11)
            {
                p = p.Substring(0, 3) + '-' + p.Substring(3, p.Length - 3);
                if ((caretLocation == 4 && characterChange > 0))
                    caretLocation++;
            }
            if (p.Length >= 7 && p.Length <= 11)
            {
                p = p.Substring(0, 7) + '-' + p.Substring(7, p.Length - 7);
                if ((caretLocation == 8 && characterChange > 0))
                    caretLocation++;
            }
            #endregion
            #region Extra number handler
            //Removes dashes if 11 or more numbers are entered
            if (p.Length > 11)
            {

                if (caretLocation > 3 && caretLocation < 8)
                {
                    caretLocation = caretLocation - 1;
                    p = rgx.Replace(p, "");
                }
                else if (caretLocation == 12)
                {
                    if (p.Contains("-"))
                    {
                        caretLocation = caretLocation - 2;
                    }
                    p = rgx.Replace(p, "");
                }
                else
                {
                    p = rgx.Replace(p, "");
                }

            }

            //Handles the caret location if dashes are readded
            else if (p.Length == 11 && characterChange < 0 && p.Contains('-'))
            {
                if (caretLocation > 2 && caretLocation < 7)
                {
                    caretLocation = caretLocation + 1;
                }
                else if (caretLocation >= 7 && !(p.Substring(caretLocation, 1).Equals("-")))
                {
                    //if(!(p.Substring(caretLocation, 1).Equals("-")))
                    //{
                    caretLocation = caretLocation + 2;
                    //}
                }

            }
            #endregion
            #region Delete handler
            //Handles the deletes around a dash
            if (characterChange <= 0)
            {
                if (caretLocation == 8)
                {
                    caretLocation = caretLocation - 1;
                }
                else if (caretLocation == 4)
                {
                    caretLocation = caretLocation - 1;
                }

            }
            #endregion
            tb.Text = p;
            tb.SelectionStart = caretLocation;
        }

        public static string zipPlus4TextInput(int countryId, string postal)
        {
            string fullZip = postal;
           
                if (countryId == 1 || countryId == null || countryId == 0)
                {
                    if (fullZip.Length == 5)
                    {
                        fullZip = fullZip + "-";
                    }
                }
                else if (countryId == 2)
                {
                    if (fullZip.Length == 3)
                    {
                        fullZip = fullZip + " ";
                    }
                }
          
            return fullZip;
        }

        public static bool ZipValidation(int countryId, string zip)
        {
            Regex usZipRegEx = new Regex("(^[0-9]{5}$)|(^[0-9]{5}-[0-9]{4}$)");
            Regex caZipRegEx = new Regex("(^[A-Z]{1}[0-9]{1}[A-Z]{1}\\s[0-9]{1}[A-Z]{1}[0-9]$)");

            string fullZip = zip;

            if (countryId != null && (countryId == 1 || countryId == 2) && !string.IsNullOrEmpty(fullZip))
            {
                bool validZipCode;
                if ((usZipRegEx.IsMatch(fullZip)) || (caZipRegEx.IsMatch(fullZip)))
                    validZipCode = true;
                else
                    validZipCode = false;

                if (validZipCode == false)
                {
                    Core_AMS.Utilities.WPF.Message("Postal Code is invalid.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Postal Code");
                    return false;
                }
            }
            return true;
        }
        public static string ZipFormat(string currZip, string newText, int countryID, int index)
        {
            if (newText.All(char.IsLetterOrDigit))
            {
                if (countryID == 1) // USA
                {
                    if (currZip.Length < 10)
                    {
                        if (currZip.Length < 5)
                            currZip = currZip.Insert(index, newText);
                        else if ((currZip.Length + newText.Length) > 5 && !currZip.Contains("-"))
                        {
                            if (currZip.Length == 9)
                                currZip = currZip.Insert(5, "-");
                            else
                            {
                                currZip = currZip.Insert(index, newText);
                                currZip = currZip.Insert(5, "-");
                            }
                        }
                        else
                        {
                            string[] s = currZip.Split('-');
                            int zip = s[0].Length;
                            if(zip == 5)
                                index = currZip.Length;
                            currZip = currZip.Insert(index, newText);
                        }
                    }
                }
                else if (countryID == 2) // CANADA
                {
                    if (currZip.Length < 7)
                    {
                        if (currZip.Length == 3)
                            currZip += " " + newText;
                        else if (currZip.Length >= 5 && !currZip.Contains(" "))
                        {
                            if (currZip.Length == 6)
                                currZip = currZip.Insert(3, " ");
                            else
                            {
                                currZip = currZip.Insert(index, newText);
                                currZip = currZip.Insert(3, " ");
                            }
                        }
                        else
                        {
                            string[] s = currZip.Split(' ');
                            int zip = s[0].Length;
                            if (zip == 3)
                                index = currZip.Length;
                            currZip = currZip.Insert(index, newText);
                        }
                    }
                }
                else if (countryID > 2) // OTHER
                    currZip += newText;
            }
            return currZip;
        }

        public static bool CheckResponse<T>(List<T> response, FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes status, bool showServiceError = true)
        {
            if (response != null && status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                return true;
            else
            {
                if(showServiceError == true)
                    Core_AMS.Utilities.WPF.MessageServiceError();
                return false;
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(depObj);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public static IEnumerable<DependencyObject> GetChildObjects(DependencyObject parent)
        {
            if (parent == null)
                yield break;

            if (parent is ContentElement || parent is FrameworkElement)
            {
                //use the logical tree for content / framework elements 
                foreach (object obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null)
                        yield return (DependencyObject)obj;

                }
            }
            else
            {
                //use the visual tree per default 
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is DependencyObject)
                    {
                        yield return (DependencyObject)child;
                    }

                    foreach (DependencyObject childOfChild in FindVisualChildren<DependencyObject>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static FrameworkUAD.Entity.Batch CurrentBatch(int productID)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> bWorker = FrameworkServices.ServiceClient.UAD_BatchClient();

            if (FrameworkUAS.Object.AppData.myAppData.BatchList != null && FrameworkUAS.Object.AppData.myAppData.BatchList.Exists(x => x.PublicationID == productID && x.IsActive == true && x.BatchCount < 100))
            {
                FrameworkUAD.Entity.Batch uadBatch = new FrameworkUAD.Entity.Batch();
                FrameworkUAS.Object.Batch uasBatch = FrameworkUAS.Object.AppData.myAppData.BatchList.Single(x => x.PublicationID == productID && x.IsActive == true && x.BatchCount < 100);

                uasBatch.BatchCount = ++uasBatch.BatchCount;

                uadBatch.BatchCount = uasBatch.BatchCount;

                //uadBatch.BatchCount = 100;
                //uasBatch.BatchCount = 100;

                uadBatch.BatchID = uasBatch.BatchID;
                uadBatch.BatchNumber = uasBatch.BatchNumber;
                uadBatch.DateCreated = uasBatch.DateCreated;
                uadBatch.DateFinalized = uasBatch.DateFinalized;
                uadBatch.IsActive = uasBatch.IsActive;
                uadBatch.PublicationID = uasBatch.PublicationID;
                uadBatch.UserID = uasBatch.UserID;

                bWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, uadBatch, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

                return uadBatch;
            }
            else
            {
                if (productID > 0)
                {
                    FrameworkUAS.Service.Response<FrameworkUAD.Entity.Batch> newBatchResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Batch>();
                    FrameworkUAD.Entity.Batch newBatch = new FrameworkUAD.Entity.Batch();
                    FrameworkUAS.Object.Batch uasBatch = new FrameworkUAS.Object.Batch();

                    FrameworkUAS.Object.Batch batch = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.PublicationID == productID && x.IsActive == true).FirstOrDefault();

                    if (batch != null && batch.BatchID > 0 && batch.BatchCount >= 100)
                    {
                        newBatch.BatchCount = batch.BatchCount;
                        newBatch.BatchID = batch.BatchID;
                        newBatch.BatchNumber = batch.BatchNumber;
                        newBatch.DateCreated = batch.DateCreated;
                        newBatch.DateFinalized = DateTime.Now;
                        newBatch.IsActive = false;
                        newBatch.PublicationID = batch.PublicationID;
                        newBatch.UserID = batch.UserID;

                        int index = FrameworkUAS.Object.AppData.myAppData.BatchList.IndexOf(batch);
                        FrameworkUAS.Object.AppData.myAppData.BatchList.RemoveAt(index);
                        bWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newBatch, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        Core_AMS.Utilities.WPF.Message("Batch #" + batch.BatchNumber + " has reached 100 Transactions. The next batch will open automatically.");
                    }
                    newBatchResponse = bWorker.Proxy.StartNewBatch(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, productID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (newBatchResponse.Result != null && newBatchResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        newBatch = newBatchResponse.Result;

                    if (FrameworkUAS.Object.AppData.myAppData.BatchList == null)
                        FrameworkUAS.Object.AppData.myAppData.BatchList = new List<FrameworkUAS.Object.Batch>();

                    uasBatch.BatchCount = newBatch.BatchCount;
                    uasBatch.BatchID = newBatch.BatchID;
                    uasBatch.BatchNumber = newBatch.BatchNumber;
                    uasBatch.DateCreated = newBatch.DateCreated;
                    uasBatch.DateFinalized = newBatch.DateFinalized;
                    uasBatch.IsActive = newBatch.IsActive;
                    uasBatch.PublicationID = newBatch.PublicationID;
                    uasBatch.UserID = newBatch.UserID;

                    FrameworkUAS.Object.AppData.myAppData.BatchList.Add(uasBatch);
                    return newBatch;
                }
                else
                    return null;
            }
        }

        public static void RefreshBatches()
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> bClient = FrameworkServices.ServiceClient.UAD_BatchClient();
            List<FrameworkUAD.Entity.Batch> existing = null;

            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User != null && 
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections != null)
                existing = bClient.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, 
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (existing != null)
            {
                if (existing.Count > 0)
                {
                    List<FrameworkUAS.Object.Batch> b = new List<FrameworkUAS.Object.Batch>();

                    foreach (var x in existing)
                    {
                        b.Add(new FrameworkUAS.Object.Batch
                        {
                            BatchCount = x.BatchCount,
                            BatchID = x.BatchID,
                            BatchNumber = x.BatchNumber,
                            DateCreated = x.DateCreated,
                            DateFinalized = x.DateFinalized,
                            IsActive = x.IsActive,
                            PublicationID = x.PublicationID,
                            UserID = x.UserID
                        });
                    }

                    FrameworkUAS.Object.AppData.myAppData.BatchList = b;
                }
            }
        }

        #region JSON Functions
        public static List<HistoryData> JsonComparer(string type, string fromJSON, string toJSON,
               FrameworkUAD_Lookup.Entity.CodeType deliverCodeTypeID,
               List<FrameworkUAD_Lookup.Entity.Action> actionList,
               List<FrameworkUAD_Lookup.Entity.CategoryCode> cat,
               List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catType,
               List<FrameworkUAD_Lookup.Entity.TransactionCode> trans,
               List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sStatusList,
               List<FrameworkUAD_Lookup.Entity.Code> qSourceList,
               List<FrameworkUAD_Lookup.Entity.Code> parList,
               List<FrameworkUAD_Lookup.Entity.Code> codeList,
               List<FrameworkUAD_Lookup.Entity.Code> marketingList,
               List<FrameworkUAD_Lookup.Entity.Country> countryList,
               List<FrameworkUAD_Lookup.Entity.Region> regionList,
               List<FrameworkUAD.Entity.ResponseGroup> questions,
               List<FrameworkUAD.Entity.CodeSheet> answers)
        {
            List<HistoryData> history = new List<HistoryData>();

            bool isWaveMailChange = false;
            string waveMail = "";
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            object jsonOne = null;
            object jsonTwo = null;

            if (type == "ProductSubscription")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.ProductSubscription>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.ProductSubscription>(toJSON);

            }
            else if (type == "Subscription")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.Subscription>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.Subscription>(toJSON);

            }
            else if (type == "SubscriptionPaid")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.SubscriptionPaid>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.SubscriptionPaid>(toJSON);

            }
            else if (type == "PaidBillTo")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.PaidBillTo>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.PaidBillTo>(toJSON);

                if(jsonOne == null)
                    jsonOne = new FrameworkUAD.Entity.PaidBillTo();
                if(jsonTwo == null)
                    jsonTwo = new FrameworkUAD.Entity.PaidBillTo();
            }
            else if (type == "MarketingMap")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.MarketingMap>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.MarketingMap>(toJSON);

            }
            else if (type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail") //SubscriptionResponseMap to support old code and logs.
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.ProductSubscriptionDetail>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.ProductSubscriptionDetail>(toJSON);
            }
            else if (type == "PubSubscriptionAdHoc")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(toJSON);
            }

            List<string> filter = IncludeColumns();
            #region FROM Obj - TO Obj
            if(jsonOne != null || jsonTwo != null)
            {
                string originalValue = "";
                string newValue = "";
                PropertyInfo[] propertyInfos = new PropertyInfo[120];
                PropertyInfo piWaveMail;
                if(jsonOne != null)
                {
                    propertyInfos = jsonOne.GetType().GetProperties();
                    piWaveMail = propertyInfos.Where(x => x.Name.Equals("IsInActiveWaveMailing")).FirstOrDefault();
                    if(piWaveMail != null)
                        isWaveMailChange = (bool)piWaveMail.GetValue(jsonOne);
                }
                else if(jsonTwo != null)
                {
                    propertyInfos = jsonTwo.GetType().GetProperties();
                    piWaveMail = propertyInfos.Where(x => x.Name.Equals("IsInActiveWaveMailing")).FirstOrDefault();
                    if(piWaveMail != null)
                        isWaveMailChange = (bool)piWaveMail.GetValue(jsonTwo);
                }

                if (type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail")
                {
                    #region Responses
                    FrameworkUAD.Entity.CodeSheet responseNameFrom = new FrameworkUAD.Entity.CodeSheet();
                    FrameworkUAD.Entity.CodeSheet responseNameTo = new FrameworkUAD.Entity.CodeSheet();
                    FrameworkUAD.Entity.ResponseGroup rgFrom = new FrameworkUAD.Entity.ResponseGroup();
                    FrameworkUAD.Entity.ResponseGroup rgTo = new FrameworkUAD.Entity.ResponseGroup();
                    string responseOtherFrom = "";
                    string responseOtherTo = "";
                    string responseFrom = "";
                    string responseTo = "";
                    PropertyInfo response;

                    if (jsonOne != null)
                    {
                        int responseFromID = Convert.ToInt32(jsonOne.GetType().GetProperty("CodeSheetID").GetValue(jsonOne));
                        propertyInfos = jsonOne.GetType().GetProperties();
                        response = propertyInfos.Where(x => x.Name.Equals("ResponseOther")).FirstOrDefault();
                        if (response != null)
                            responseOtherFrom = (string)response.GetValue(jsonOne);

                        responseNameFrom = (from a in answers
                                            join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                            where a.CodeSheetID == responseFromID
                                            select a).SingleOrDefault();

                        rgFrom = (from a in answers
                                  join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                  where a.CodeSheetID == responseFromID && a.IsActive == true && q.IsActive == true
                                  select q).SingleOrDefault();

                        responseFrom = responseNameFrom.ResponseDesc;
                        if (responseOtherFrom.Length > 0)
                            responseFrom += " - " + responseOtherFrom;
                    }
                    if (jsonTwo != null)
                    {
                        int responseToID = Convert.ToInt32(jsonTwo.GetType().GetProperty("CodeSheetID").GetValue(jsonTwo));
                        propertyInfos = jsonTwo.GetType().GetProperties();
                        response = propertyInfos.Where(x => x.Name.Equals("ResponseOther")).FirstOrDefault();
                        if (response != null)
                            responseOtherTo = (string)response.GetValue(jsonTwo);

                        responseNameTo = (from a in answers
                                          join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                          where a.CodeSheetID == responseToID
                                          select a).SingleOrDefault();

                        rgTo = (from a in answers
                                join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                where a.CodeSheetID == responseToID && a.IsActive == true && q.IsActive == true
                                select q).SingleOrDefault();

                        responseTo = responseNameTo.ResponseDesc;
                        if(responseOtherTo.Length > 0)
                            responseTo += " - " + responseOtherTo;
                    }

                    if (responseFrom != null || responseTo != null)
                    {
                        string rgName = "";
                        if(rgFrom.DisplayName == "")
                            rgName = rgTo.DisplayName;
                        else
                            rgName = rgFrom.DisplayName;
                        history.Add(new HistoryData(0, responseFrom + " TO " + responseTo, rgName));
                    }
                    #endregion
                }
                else if(type == "SubscriptionPaid")
                {
                    string[] exclude = new string[] { "DateCreated", "DateUpdated", "PubSubscriptionID", "SubscriptionPaidID", "UpdatedByUserID", "CreatedByUserID" };
                    foreach(PropertyInfo p in propertyInfos.Where(x=> !exclude.Contains(x.Name)))
                    {
                        if (jsonOne != null)
                            originalValue = p.GetValue(jsonOne).ToString().Trim();
                        if (jsonTwo != null)
                            newValue = p.GetValue(jsonTwo).ToString().Trim();

                        if(!originalValue.Equals(newValue))
                            history.Add(new HistoryData(0, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, p.Name));
                    }
                }
                else
                {
                    foreach (PropertyInfo p in propertyInfos)
                    {
                        int index = 0;
                        bool include = false;
                        string adHoc = "";
                        if (p.Name.Equals("IsActive") && type == "Subscriber")
                            include = false;
                        else
                        {
                            include = filter.Contains(p.Name, StringComparer.CurrentCultureIgnoreCase);
                            index = filter.IndexOf(p.Name);
                        }
                        if (include == true || type == "PubSubscriptionAdHoc")
                        {
                            if (jsonOne != null && jsonTwo != null)
                            {
                                originalValue = (p.GetValue(jsonOne) ?? "").ToString();
                                newValue = (jsonTwo.GetType().GetProperty(p.Name).GetValue(jsonTwo) ?? "").ToString().Trim();
                            }
                            else if (jsonOne != null)
                                originalValue = (p.GetValue(jsonOne) ?? "").ToString();
                            else if (jsonTwo != null)
                                newValue = (p.GetValue(jsonTwo) ?? "").ToString();

                            if (isWaveMailChange)
                                waveMail = " (Wave Mail Change)";

                            if (type == "PubSubscriptionAdHoc")
                                adHoc = " (AdHoc Change)";

                            #region Name specific
                            if (!originalValue.Trim().Equals(newValue))
                            {
                                #region Country
                                if (p.Name.Equals("CountryID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pcountryListName = countryList.Where(s => s.CountryID == Convert.ToInt32(originalValue)).Select(x => x.ShortName).SingleOrDefault();
                                    string ncountryListName = countryList.Where(s => s.CountryID == Convert.ToInt32(newValue)).Select(x => x.ShortName).SingleOrDefault();

                                    if (type.Equals("Subscriber") || type.Equals("ProductSubscription"))
                                    {
                                        history.Add(new HistoryData(index, pcountryListName + " TO " + ncountryListName + waveMail, "Country"));
                                    }
                                    else if (type.Equals("PaidBillTo"))
                                    {
                                        history.Add(new HistoryData(index, pcountryListName + " TO " + ncountryListName + waveMail, "Bill To - Country"));
                                    }
                                }
                                #endregion
                                #region State
                                else if (p.Name.Equals("RegionID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pRegionName = regionList.Where(s => s.RegionID == Convert.ToInt32(originalValue)).Select(x => x.RegionName).SingleOrDefault();
                                    string nRegionName = regionList.Where(s => s.RegionID == Convert.ToInt32(newValue)).Select(x => x.RegionName).SingleOrDefault();

                                    if (type.Equals("Subscriber") || type.Equals("ProductSubscription"))
                                    {
                                        history.Add(new HistoryData(index, pRegionName + " TO " + nRegionName + waveMail, "State"));
                                    }
                                    else if (type.Equals("PaidBillTo"))
                                    {
                                        history.Add(new HistoryData(index, pRegionName + " TO " + nRegionName + waveMail, "Bill To - State"));
                                    }
                                }
                                #endregion
                                #region SubscriptionStatusID
                                else if (p.Name.Equals("SubscriptionStatusID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pStatusName = sStatusList.Where(s => s.SubscriptionStatusID == Convert.ToInt32(originalValue)).Select(x => x.StatusName).SingleOrDefault();
                                    string nStatusName = sStatusList.Where(s => s.SubscriptionStatusID == Convert.ToInt32(newValue)).Select(x => x.StatusName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pStatusName + " TO " + nStatusName + waveMail, "Subscription Status"));
                                }
                                #endregion
                                #region QSource
                                else if (p.Name.Equals("PubQSourceID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pDisplayName = qSourceList.Where(q => q.CodeId == Convert.ToInt32(originalValue)).Select(x => x.DisplayName).SingleOrDefault();
                                    string nDisplayName = qSourceList.Where(q => q.CodeId == Convert.ToInt32(newValue)).Select(x => x.DisplayName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pDisplayName + " TO " + nDisplayName + waveMail, "Qualification Source"));
                                }
                                #endregion
                                #region QDate
                                else if (p.Name.Equals("QSourceDate"))
                                {
                                    string origDate = string.Empty;
                                    string newDate = string.Empty;
                                    if (!string.IsNullOrEmpty(originalValue))
                                    {
                                        DateTime d = Convert.ToDateTime(originalValue);
                                        origDate = d.Date.ToShortDateString();
                                    }
                                    if (!string.IsNullOrEmpty(newValue))
                                    {
                                        DateTime n = Convert.ToDateTime(newValue);
                                        newDate = n.Date.ToShortDateString();
                                    }
                                    history.Add(new HistoryData(index, origDate + " TO " + newDate + waveMail, "Qualification Date"));
                                }
                                #endregion
                                #region Demo7
                                else if (p.Name.Equals("DeliverabilityID") || p.Name.Equals("DEMO7", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pDeName = "";
                                    string nDeName = "";
                                    if (p.Name.Equals("DeliverabilityID"))
                                    {
                                        pDeName = codeList.Where(d => d.CodeId == Convert.ToInt32(originalValue)).Select(x => x.DisplayName).SingleOrDefault();
                                        nDeName = codeList.Where(d => d.CodeId == Convert.ToInt32(newValue)).Select(x => x.DisplayName).SingleOrDefault();
                                    }
                                    else if (deliverCodeTypeID.CodeTypeId > 0)
                                    {
                                        pDeName = codeList.Where(d => d.CodeValue == originalValue && d.CodeTypeId == deliverCodeTypeID.CodeTypeId).Select(x => x.DisplayName).SingleOrDefault();
                                        nDeName = codeList.Where(d => d.CodeValue == newValue && d.CodeTypeId == deliverCodeTypeID.CodeTypeId).Select(x => x.DisplayName).SingleOrDefault();
                                    }

                                    history.Add(new HistoryData(index, pDeName + " TO " + nDeName + waveMail, "Media Type"));
                                }
                                #endregion
                                #region Par3C
                                else if (p.Name.Equals("Par3CID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pParName = parList.Where(a => a.CodeId == Convert.ToInt32(originalValue)).Select(x => x.DisplayName).SingleOrDefault();
                                    string nParName = parList.Where(a => a.CodeId == Convert.ToInt32(newValue)).Select(x => x.DisplayName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pParName + " TO " + nParName + waveMail, "Par3C"));
                                }
                                #endregion
                                #region SubSrc
                                else if (p.Name.Equals("SubSrcID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pssTypeName = codeList.Where(s => s.CodeId == Convert.ToInt32(originalValue)).Select(x => x.CodeName).SingleOrDefault();
                                    string nssTypeName = codeList.Where(s => s.CodeId == Convert.ToInt32(newValue)).Select(x => x.CodeName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pssTypeName + " TO " + nssTypeName + waveMail, "Subscriber Type"));
                                }
                                else if (p.Name.Equals("SubscriberSourceCode"))
                                {
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Subscriber Source Code"));
                                }
                                else if (p.Name.Equals("OrigsSrc"))
                                {
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Original Subscriber Source Code"));
                                }
                                #endregion
                                #region Address Type
                                else if (p.Name.Equals("AddressTypeID") || p.Name.Equals("AddressTypeCodeId", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pssTypeName = string.Empty;
                                    string nssTypeName = string.Empty;
                                    if (Convert.ToInt32(originalValue) > 0)
                                        pssTypeName = codeList.Where(s => s.CodeId == Convert.ToInt32(originalValue)).Select(x => x.CodeName).SingleOrDefault();

                                    if (Convert.ToInt32(newValue) > 0)
                                        nssTypeName = codeList.Where(s => s.CodeId == Convert.ToInt32(newValue)).Select(x => x.CodeName).SingleOrDefault();

                                    if (!string.IsNullOrEmpty(pssTypeName) || !string.IsNullOrEmpty(nssTypeName))
                                        history.Add(new HistoryData(index, pssTypeName + " TO " + nssTypeName + waveMail, "Address Type"));
                                }
                                #endregion
                                #region Various Profile Fields
                                else if (p.Name == "FirstName")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "First Name"));
                                else if (p.Name == "LastName")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Last Name"));
                                else if (p.Name == "Address1")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Address 1"));
                                else if (p.Name == "Address2")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Address 2"));
                                else if (p.Name == "Address3")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Address 3"));
                                else if (p.Name == "PhoneExt")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Phone Ext"));
                                else if (p.Name == "ZipCode")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Zip"));
                                #endregion
                                #region Contact Method
                                else if (type == "MarketingMap")
                                {
                                    string marketingName;
                                    int marketingID = -1;
                                    bool marketFromIsActive = false;
                                    bool marketToIsActive = false;

                                    if (jsonOne != null)
                                    {
                                        //marketFromIsActive = Convert.ToBoolean(jsonOne.GetType().GetProperty("IsActive").GetValue(jsonOne));
                                        marketFromIsActive = true;
                                        marketingID = Convert.ToInt32(jsonOne.GetType().GetProperty("MarketingID").GetValue(jsonOne));
                                    }
                                    if (jsonTwo != null)
                                    {
                                        marketToIsActive = Convert.ToBoolean(jsonTwo.GetType().GetProperty("IsActive").GetValue(jsonTwo));
                                        marketingID = Convert.ToInt32(jsonTwo.GetType().GetProperty("MarketingID").GetValue(jsonTwo));
                                    }

                                    marketingName = marketingList.Where(m => m.CodeId == marketingID).Select(x => x.CodeName).SingleOrDefault();

                                    string key = string.Empty;

                                    if (!string.IsNullOrEmpty(marketingName))
                                        key = "Contact Method - " + marketingName.Trim();

                                    string value = marketFromIsActive.ToString() + " TO " + marketToIsActive.ToString();

                                    if (history.Select(x => x.PropertyName == key).Count() == 0 && history.Select(x => x.DisplayText == value).Count() == 0)
                                        history.Add(new HistoryData(index, value, key));
                                }
                                #endregion
                                #region SequenceID
                                else if (p.Name == "SequenceID")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Sequence #"));
                                #endregion
                                #region MemberGroup
                                else if (p.Name == "MemberGroup")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Member Group"));
                                #endregion
                                #region Plus4
                                else if (p.Name == "Plus4")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Zip Plus 4"));
                                #endregion
                                #region Category & Transaction
                                else if (p.Name == "PubCategoryID")
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string cat1 = "";
                                    string cat2 = "";
                                    int catTypeID1 = 0;
                                    int catTypeID2 = 0;
                                    if (Convert.ToInt32(originalValue) > 0)
                                    {
                                        cat1 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(originalValue)).Select(x => x.CategoryCodeName).FirstOrDefault();
                                        catTypeID1 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(originalValue)).Select(x => x.CategoryCodeTypeID).FirstOrDefault();
                                    }
                                    if (Convert.ToInt32(newValue) > 0)
                                    {
                                        cat2 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(newValue)).Select(x => x.CategoryCodeName).FirstOrDefault();
                                        catTypeID2 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(newValue)).Select(x => x.CategoryCodeTypeID).FirstOrDefault();
                                    }

                                    if (catTypeID1 != catTypeID2)
                                    {
                                        string catType1 = "";
                                        string catType2 = "";

                                        catType1 = catType.Where(x => x.CategoryCodeTypeID == catTypeID1).Select(x => x.CategoryCodeTypeName).FirstOrDefault();
                                        catType2 = catType.Where(x => x.CategoryCodeTypeID == catTypeID2).Select(x => x.CategoryCodeTypeName).FirstOrDefault();

                                        if (catType1 != "" && catType2 != "")
                                            history.Add(new HistoryData(index, catType1 + " TO " + catType2 + waveMail, "Free/Paid"));
                                    }

                                    if (cat1 != "" || cat2 != "")
                                        history.Add(new HistoryData(index, cat1 + " TO " + cat2 + waveMail, "Category Type"));

                                }
                                else if (p.Name == "PubTransactionID")
                                {
                                    string tran1 = "";
                                    string tran2 = "";
                                    if (Convert.ToInt32(originalValue) > 0)
                                        tran1 = trans.Where(x => x.TransactionCodeID == Convert.ToInt32(originalValue)).Select(x => x.TransactionCodeName).FirstOrDefault();
                                    if (Convert.ToInt32(newValue) > 0)
                                        tran2 = trans.Where(x => x.TransactionCodeID == Convert.ToInt32(newValue)).Select(x => x.TransactionCodeName).FirstOrDefault();

                                    if (tran1 != "" || tran2 != null)
                                        history.Add(new HistoryData(index, tran1 + " TO " + tran2 + waveMail, "Transaction Type"));
                                }
                                #endregion
                                #region Responses - No Other
                                else if ((type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail") && !p.Name.Equals("ResponseOther"))
                                {
                                    FrameworkUAD.Entity.CodeSheet responseNameFrom = new FrameworkUAD.Entity.CodeSheet();
                                    FrameworkUAD.Entity.CodeSheet responseNameTo = new FrameworkUAD.Entity.CodeSheet();
                                    int responseFromID = Convert.ToInt32(jsonOne.GetType().GetProperty("CodeSheetID").GetValue(jsonOne));
                                    int responseToID = Convert.ToInt32(jsonTwo.GetType().GetProperty("CodeSheetID").GetValue(jsonTwo));

                                    responseNameFrom = (from a in answers
                                                        join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                        where a.CodeSheetID == responseFromID
                                                        select a).SingleOrDefault();
                                    responseNameTo = (from a in answers
                                                      join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                      where a.CodeSheetID == responseToID
                                                      select a).SingleOrDefault();

                                    FrameworkUAD.Entity.ResponseGroup rtType = new FrameworkUAD.Entity.ResponseGroup();
                                    rtType = (from a in answers
                                              join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                              where a.CodeSheetID == responseToID && a.IsActive == true && q.IsActive == true
                                              select q).SingleOrDefault();


                                    string key = rtType.DisplayName;

                                    if (rtType != null && responseNameTo.IsOther == false && history.Select(x => x.PropertyName == key).Count() == 0)
                                        history.Add(new HistoryData(index, responseNameFrom.ResponseDesc.ToString() + " TO " + responseNameTo.ResponseDesc.ToString() + waveMail, key));
                                }
                                #endregion
                                #region Responses - Other
                                else if ((type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail") && p.Name.Equals("ResponseOther"))
                                {
                                    if (!string.IsNullOrEmpty(newValue))
                                    {
                                        FrameworkUAD.Entity.CodeSheet responseNameFrom = new FrameworkUAD.Entity.CodeSheet();
                                        FrameworkUAD.Entity.CodeSheet responseNameTo = new FrameworkUAD.Entity.CodeSheet();
                                        int responseFromID = Convert.ToInt32(jsonOne.GetType().GetProperty("CodeSheetID").GetValue(jsonOne));
                                        int responseToID = Convert.ToInt32(jsonTwo.GetType().GetProperty("CodeSheetID").GetValue(jsonTwo));
                                        string responseFromOther = Convert.ToString(jsonOne.GetType().GetProperty("ResponseOther").GetValue(jsonOne));
                                        string responseToOther = Convert.ToString(jsonTwo.GetType().GetProperty("ResponseOther").GetValue(jsonTwo));

                                        responseNameFrom = (from a in answers
                                                            join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                            where a.CodeSheetID == responseFromID
                                                            select a).SingleOrDefault();
                                        responseNameTo = (from a in answers
                                                          join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                          where a.CodeSheetID == responseToID
                                                          select a).SingleOrDefault();

                                        FrameworkUAD.Entity.ResponseGroup rtType = new FrameworkUAD.Entity.ResponseGroup();
                                        rtType = (from a in answers
                                                  join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                  where a.CodeSheetID == responseToID && a.IsActive == true && q.IsActive == true
                                                  select q).SingleOrDefault();


                                        string key = rtType.DisplayName;
                                        if (rtType != null && history.Select(x => x.PropertyName == key).Count() == 0)
                                        {
                                            string toValues = " TO ";
                                            string fromValues = "";
                                            if (responseNameTo.IsOther == false)
                                            {
                                                toValues += responseNameTo.ResponseDesc;
                                            }
                                            else
                                                toValues += responseToOther;
                                            if (responseNameFrom.IsOther == false)
                                                fromValues += responseNameFrom.ResponseDesc;
                                            else
                                                fromValues += responseFromOther;

                                            history.Add(new HistoryData(index, fromValues + toValues, key));
                                        }
                                    }
                                }
                                #endregion
                                else
                                {
                                    if (type == "PubSubscriptionAdHoc")
                                    {
                                        FrameworkUAD.Object.PubSubscriptionAdHoc adhoc = jsonOne as FrameworkUAD.Object.PubSubscriptionAdHoc;
                                        if (adhoc != null)
                                            history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail + adHoc, adhoc.AdHocField));
                                    }
                                    else
                                        history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail + adHoc, p.Name));
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion

            return history;
        }

        private static List<string> IncludeColumns()
        {
            List<string> filter = new List<string>();
            filter.Add("SequenceID");
            filter.Add("FirstName");
            filter.Add("LastName");
            filter.Add("Title");
            filter.Add("Company");
            filter.Add("AddressTypeCodeId");
            filter.Add("Address1");
            filter.Add("Address2");
            filter.Add("Address3");
            filter.Add("City");
            filter.Add("County");
            filter.Add("CountryID");
            filter.Add("RegionID");
            filter.Add("ZipCode");
            filter.Add("Plus4");
            filter.Add("Phone");
            filter.Add("PhoneExt");
            filter.Add("Mobile");
            filter.Add("Fax");
            filter.Add("Email");
            filter.Add("Website");
            filter.Add("PubCategoryID");
            filter.Add("PubTransactionID");
            filter.Add("MemberGroup");
            filter.Add("SubscriberSourceCode");
            filter.Add("OrigsSrc");
            filter.Add("SubSrcID");
            filter.Add("Demo7");
            filter.Add("Verify");
            filter.Add("PubQSourceID");
            filter.Add("Par3CID");
            filter.Add("Qualificationdate");
            filter.Add("Copies");
            filter.Add("MarketingID");
            filter.Add("MailPermission");
            filter.Add("FaxPermission");
            filter.Add("PhonePermission");
            filter.Add("OtherProductsPermission");
            filter.Add("EmailRenewPermission");
            filter.Add("ThirdPartyPermission");
            filter.Add("TextPermission");
            return filter;
        }
        #endregion
    }
}
