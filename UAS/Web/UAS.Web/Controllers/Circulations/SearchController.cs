using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Circulations;

namespace UAS.Web.Controllers.Circulations
{
    public class SearchController : BaseController
    {

        #region Entity Declarations
        private SubscriptionManager subMgr;
        private EntityLists entlst;
        private bool saveWaveMailing = false;
        private int applicationID = 0;
        #endregion

        

        public SearchController()
        {


        }
        
        

        #region Bind Search Panel
        [HttpGet]
        public ActionResult Index()
        {
           

            if (CurrentClientID > 0)
            {
                //ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.PostMenu Menu = new ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.PostMenu("/uas.web/search", "AMSCircMVC", Request.Url.PathAndQuery.Substring(0, Request.Url.PathAndQuery.IndexOf("?") > 0 ? Request.Url.PathAndQuery.IndexOf("?") : Request.Url.PathAndQuery.Length));
             
                    if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
                    {
                        Search search = new Search();
                        return View(search);
                    }
                    else
                    {
                        return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
                    }
               

            }
            else
            {
                return Redirect("/ecn.accounts/main/");
            }

        }
        #endregion

        #region Get Search Results
        [HttpPost]
        public ActionResult Index(Search model)
        {
             if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
                {
                    bool validModel = false;
                    bool selectother = false;
                    if (string.IsNullOrEmpty(model.AccountNumber) && (string.IsNullOrEmpty(model.Product) || model.Product == "Product") && string.IsNullOrEmpty(model.LastName) && string.IsNullOrEmpty(model.FirstName) && string.IsNullOrEmpty(model.PhoneNumber)
                       && string.IsNullOrEmpty(model.Address) && string.IsNullOrEmpty(model.City) && string.IsNullOrEmpty(model.State) && string.IsNullOrEmpty(model.Country) && string.IsNullOrEmpty(model.Company)
                       && string.IsNullOrEmpty(model.Email) && (model.SequenceNumber == null || model.SequenceNumber == 0) && (model.SubscriptionID == null || model.SubscriptionID == 0))
                    {
                        validModel = false;
                        //If only title or zipcode is provided ask user to provide at least one more search criteria.
                        if (!string.IsNullOrEmpty(model.UserTitle) || !string.IsNullOrEmpty(model.ZipCode))
                        {
                            selectother = true;
                            validModel = false;
                        }
                    }
                    else
                    {
                        validModel = true;
                    }
                    if (validModel)
                    {
                        if (string.IsNullOrEmpty(model.AccountNumber))
                            model.AccountNumber = "";
                        if (!string.IsNullOrEmpty(model.PhoneNumber))
                            model.PhoneNumber = model.PhoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace(".", "").Replace("-", "").Replace("+", "");

                        if (model.Product != "Product")
                        {
                            model.PublicationID = Convert.ToInt16(model.Product);
                        }
                        else
                        {
                            model.PublicationID = 0;
                        }
                        #region Search - If Model is get the list of Subscriptions
                        if (ModelState.IsValid)
                        {
                            List<FrameworkUAD.Entity.ProductSubscription> bwResp = new List<FrameworkUAD.Entity.ProductSubscription>();
                            bwResp = new FrameworkUAD.BusinessLogic.ProductSubscription().Search(
                                CurrentUser.CurrentClient.ClientConnections,
                                CurrentUser.CurrentClient.DisplayName,
                                model.FirstName,
                                model.LastName,
                                model.Company,
                                model.UserTitle,
                                model.Address,
                                model.City,
                                model.State,
                                model.ZipCode,
                                model.Country,
                                model.Email,
                                model.PhoneNumber,
                                model.SequenceNumber ?? 0,
                                model.AccountNumber,
                                CurrentClientID,
                                model.PublicationID ?? 0,
                                model.SubscriptionID ?? 0);
                            return PartialView("Partials/Index/_SearchResults", bwResp);
                        }
                        #endregion
                    }
                    else
                    {
                        if (selectother)
                            ModelState.AddModelError("SelectOtherToo", "Please use at least one other search criteria.");
                        else
                            ModelState.AddModelError("NoDataSelected", "Please select atleast one selection criteria.");
                        return PartialView("Partials/Index/_SearchErrors", ModelState);

                    }
                    return PartialView("Partials/Index/_SearchResults", new List<FrameworkUAD.Entity.ProductSubscription>());
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
                }
           
            
        }
        #endregion

       

        #region Get Subscriber Profile
        [HttpGet]
        public ActionResult GetProfile(int id = 0)
        {
           

            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess) )
            {

                SubscriberProfile subscriberProfile = new SubscriberProfile();
                ViewBag.AllowdataEntry = false;
                ViewBag.InActiveOrUnsubscribed = false;
                try
                {
                    KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
                    if (CurrentClient != null)
                        _client = CurrentClient;
                    else
                        _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);

                    FrameworkUAD.Entity.ProductSubscription originalSubscription = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectProductSubscription(id, _client.ClientConnections, _client.DisplayName);
                    FrameworkUAD.Entity.Product _myProduct = new FrameworkUAD.BusinessLogic.Product().Select(originalSubscription.PubID, _client.ClientConnections, false);
                    List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
                    List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select();
                    FrameworkUAD_Lookup.Entity.TransactionCodeType freeType = transCodeTypeList.Where(x => x.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Active.ToString().Replace("_", " "))).FirstOrDefault();
                    FrameworkUAD_Lookup.Entity.TransactionCodeType paidType = transCodeTypeList.Where(x => x.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Active.ToString().Replace("_", " "))).FirstOrDefault();

                    if (freeType != null && paidType != null)
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode paid = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == paidType.TransactionCodeTypeID).FirstOrDefault();
                        FrameworkUAD_Lookup.Entity.TransactionCode free = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == freeType.TransactionCodeTypeID).FirstOrDefault();
                        if (paid != null)
                            subscriberProfile.PaidFreeTransactionCode = paid.TransactionCodeID;
                        if (free != null)
                            subscriberProfile.PaidFreeTransactionCode = free.TransactionCodeID;
                    }

                    if (originalSubscription != null)
                    {
                        if (originalSubscription.CountryID == 0)
                            subscriberProfile.CountryID = 1;
                        else
                            subscriberProfile.CountryID = originalSubscription.CountryID;

                        if (originalSubscription.CountryID != 0 && (originalSubscription.CountryID == 1|| originalSubscription.CountryID == 1|| originalSubscription.CountryID == 429))
                        {
                            if (!string.IsNullOrEmpty(originalSubscription.ZipCode) && !string.IsNullOrEmpty(originalSubscription.Plus4)
                                && originalSubscription.CountryID == 1)
                            {
                                if (originalSubscription.ZipCode.Length > 5)
                                    originalSubscription.FullZip = originalSubscription.ZipCode.Replace("-", "");
                                else
                                    originalSubscription.FullZip = originalSubscription.ZipCode + originalSubscription.Plus4;
                            }
                            else if (!string.IsNullOrEmpty(originalSubscription.ZipCode) && originalSubscription.CountryID == 2)
                            {
                                if (originalSubscription.ZipCode.Length > 3)
                                    originalSubscription.FullZip = originalSubscription.ZipCode.Replace(" ", "");
                                else
                                    originalSubscription.FullZip = originalSubscription.ZipCode + originalSubscription.Plus4;
                            }
                            else
                                originalSubscription.FullZip = originalSubscription.ZipCode + originalSubscription.Plus4;


                        }
                        else
                        {
                            originalSubscription.FullZip = originalSubscription.ZipCode + originalSubscription.Plus4;
                        }

                        if (!string.IsNullOrEmpty(originalSubscription.Phone) && originalSubscription.Phone.Trim().Length == 10)
                            originalSubscription.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(originalSubscription.Phone.Trim());
                        if (!string.IsNullOrEmpty(originalSubscription.Mobile) && originalSubscription.Mobile.Trim().Length == 10)
                            originalSubscription.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(originalSubscription.Mobile.Trim());
                        if (!string.IsNullOrEmpty(originalSubscription.Fax) && originalSubscription.Fax.Trim().Length == 10)
                            originalSubscription.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(originalSubscription.Fax.Trim());

                        subscriberProfile.Address1 = originalSubscription.Address1;
                        subscriberProfile.Address2 = originalSubscription.Address2;
                        subscriberProfile.Address3 = originalSubscription.Address3;
                        subscriberProfile.AddressTypeID = originalSubscription.AddressTypeCodeId;
                        subscriberProfile.City = originalSubscription.City;
                        subscriberProfile.FirstName = originalSubscription.FirstName;
                        subscriberProfile.LastName = originalSubscription.LastName;
                        subscriberProfile.Title = originalSubscription.Title;
                        subscriberProfile.Company = originalSubscription.Company;
                        subscriberProfile.City = originalSubscription.City;
                        subscriberProfile.Country = originalSubscription.Country;
                        subscriberProfile.RegionID = originalSubscription.RegionID;
                        subscriberProfile.RegionCode = originalSubscription.RegionCode;
                        if (originalSubscription.CountryID == 1 && originalSubscription.FullZip.Length.Equals(9))
                        {
                            subscriberProfile.FullZip = originalSubscription.FullZip.Substring(0, 5) + "-" + originalSubscription.FullZip.Substring(5, 4);
                        }
                        else
                        {
                            subscriberProfile.FullZip = originalSubscription.FullZip;
                        }
                        subscriberProfile.County = originalSubscription.County;
                        subscriberProfile.Phone = originalSubscription.Phone;
                        subscriberProfile.PhoneExt = originalSubscription.PhoneExt;
                        subscriberProfile.PhoneCode = originalSubscription.PhoneCode;
                        subscriberProfile.Mobile = originalSubscription.Mobile;
                        subscriberProfile.Fax = originalSubscription.Fax;
                        subscriberProfile.Email = originalSubscription.Email;
                        subscriberProfile.Website = originalSubscription.Website;
                        subscriberProfile.SequenceID = originalSubscription.SequenceID;
                        subscriberProfile.SubscriptionID = originalSubscription.SubscriptionID;
                        subscriberProfile.PubSubscriptionID = originalSubscription.PubSubscriptionID;
                        subscriberProfile.PubID = originalSubscription.PubID;
                        subscriberProfile.IsInActiveWaiveMailing = originalSubscription.IsInActiveWaveMailing;
                        if (_myProduct.AllowDataEntry)
                        {
                            ViewBag.AllowdataEntry = true;
                        }
                        if (!originalSubscription.IsSubscribed || !originalSubscription.IsActive)
                        {
                            ViewBag.InActiveOrUnsubscribed = true;
                        }
                        

                    }
                }
                catch
                {
                    return Content("Error occured while retriving data. Please try again.");
                }

                return PartialView("Partials/Index/_SubscriberProfile", subscriberProfile);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region Save Subscriber Profile
        [HttpPost]
        public JsonResult SaveProfile(SubscriberProfile model)
        {
            
            #region Expando Object Serilizer
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
            #endregion

            #region User does not have access to This feature.
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess) )
            {
                string appName = "CIRC";
                int pubId = model.PubID;
                int pubsubid = model.PubSubscriptionID;
                #region Entity Declaration
                

                FrameworkUAD.Entity.WaveMailingDetail myWMDetail;
                FrameworkUAD.Entity.ProductSubscription waveMailSubscriber;

                KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
                if (CurrentClient != null)
                    _client = CurrentClient;
                else
                    _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);

                FrameworkUAD.Entity.Product _myProduct = new FrameworkUAD.BusinessLogic.Product().Select(pubId, _client.ClientConnections);
                #endregion

                #region Data Entry is allowed - Save changes
                if (_myProduct.AllowDataEntry == true)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();
                    FrameworkUAD.Entity.ProductSubscription orgSubscriber = _productSubBO.SelectProductSubscription(pubsubid, _client.ClientConnections, _client.DisplayName);
                    FrameworkUAD.Entity.ProductSubscription currentSubscriber = new FrameworkUAD.Entity.ProductSubscription(orgSubscriber);

                    currentSubscriber.FirstName = model.FirstName;
                    currentSubscriber.LastName = model.LastName;
                    currentSubscriber.FullName = model.FirstName + " " + model.LastName;

                    currentSubscriber.Title = model.Title;
                    currentSubscriber.Company = model.Company;

                    currentSubscriber.Address1 = model.Address1;
                    currentSubscriber.Address2 = model.Address2;
                    currentSubscriber.Address3 = model.Address3;
                    currentSubscriber.City = model.City;

                    currentSubscriber.PhoneExt = model.PhoneExt;
                    currentSubscriber.County = model.County;
                    currentSubscriber.Email = model.Email;
                    currentSubscriber.Website = model.Website;

                    if (model.AddressTypeID == null || model.AddressTypeID == 0)
                    {
                        currentSubscriber.AddressTypeCodeId = 0;
                    }
                    else
                    {
                        currentSubscriber.AddressTypeCodeId = (int)model.AddressTypeID;
                    }
                    
                    if (model.CountryID == 0 | model.CountryID == null)
                    {
                        currentSubscriber.CountryID = 1;
                        currentSubscriber.Country = "UNITED STATES";
                    }
                    else
                    {
                        currentSubscriber.CountryID = (int)model.CountryID;
                        currentSubscriber.Country = model.Country;
                        currentSubscriber.PhoneCode = model.PhoneCode;
                    }
                    List<FrameworkUAD_Lookup.Entity.Region> regionsList = new FrameworkUAD_Lookup.BusinessLogic.Region().Select();
                     if (!string.IsNullOrEmpty(model.RegionCode))
                    {
                        currentSubscriber.RegionID = regionsList.Where(y => y.RegionCode == model.RegionCode).Select(x => x.RegionID).FirstOrDefault();
                        currentSubscriber.RegionCode = regionsList.Where(y => y.RegionCode == model.RegionCode).Select(x => x.RegionCode).FirstOrDefault();

                    }
                    else
                    {
                        currentSubscriber.RegionID = orgSubscriber.RegionID;
                        currentSubscriber.RegionCode = orgSubscriber.RegionCode;
                    }
                    
                    if (string.IsNullOrEmpty(model.FullZip))
                    {
                        currentSubscriber.ZipCode = "";
                        currentSubscriber.Plus4 = "";
                    }
                    else if (currentSubscriber.CountryID == 1)
                    {
                        string zip = "";
                        string plus4 = "";
                        string fulZip = model.FullZip.Replace("_", "").Replace("-", "").Replace(" ", "");
                        for (int i = 0; i < fulZip.Length; i++)
                        {
                            if (i < 5)
                                zip += fulZip[i].ToString();
                            else
                                plus4 += fulZip[i].ToString();
                        }
                        currentSubscriber.ZipCode = zip;
                        currentSubscriber.Plus4 = plus4;
                    }
                    else
                    {
                        currentSubscriber.ZipCode = model.FullZip.Replace("_", "").Replace("-", "").Replace(" ", "");
                        currentSubscriber.Plus4 = "";
                    }

                    currentSubscriber.FullAddress = model.Address1 + ", " + model.Address2 + ", " + model.Address3 + ", " + model.City + ", " + model.RegionCode + " " + model.FullZip + ", " + model.Country;

                    if (!Comparision.AreEqual(currentSubscriber.Address1, orgSubscriber.Address1) || !Comparision.AreEqual(currentSubscriber.Address2, orgSubscriber.Address2)
                        || !Comparision.AreEqual(currentSubscriber.Address3, orgSubscriber.Address3) || !Comparision.AreEqual(currentSubscriber.City, orgSubscriber.City)
                        || !Comparision.AreEqual(currentSubscriber.Country, orgSubscriber.Country) || !Comparision.AreEqual(currentSubscriber.County, orgSubscriber.County)
                        || !Comparision.AreEqual(currentSubscriber.RegionCode, orgSubscriber.RegionCode)|| !Comparision.AreEqual(currentSubscriber.FullZip, orgSubscriber.FullZip))
                    {
                      
                        List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
                        List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select();

                        FrameworkUAD_Lookup.Entity.TransactionCodeType freeType = transCodeTypeList.Where(x => x.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Active.ToString().Replace("_", " "))).FirstOrDefault();
                        FrameworkUAD_Lookup.Entity.TransactionCodeType paidType = transCodeTypeList.Where(x => x.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Active.ToString().Replace("_", " "))).FirstOrDefault();

                        if (freeType != null && paidType != null)
                        {
                            int CategorytypeId = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select().Where(x => x.CategoryCodeID == currentSubscriber.PubCategoryID).Select(x => x.CategoryCodeTypeID).First();
                            var CaregoryType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select().Where(x => x.CategoryCodeTypeID == CategorytypeId).First();
                            FrameworkUAD_Lookup.Entity.TransactionCode paid = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == paidType.TransactionCodeTypeID).FirstOrDefault();
                            FrameworkUAD_Lookup.Entity.TransactionCode free = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == freeType.TransactionCodeTypeID).FirstOrDefault();
                            if (free != null && CaregoryType.IsFree)
                                currentSubscriber.PubTransactionID = free.TransactionCodeID;
                            else
                                currentSubscriber.PubTransactionID = paid.TransactionCodeID;

                        }
                    }
                    if (!string.IsNullOrEmpty(model.Phone))
                    {
                        model.Phone = model.Phone.Replace("_", "").Replace("-", "").Replace(" ", "");
                        if (model.Phone.Length == 10)
                        {
                            currentSubscriber.Phone = model.Phone;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.Mobile))
                    {
                        model.Mobile = model.Mobile.Replace("_", "").Replace("-", "").Replace(" ", "");
                        if (model.Mobile.Length == 10)
                        {
                            currentSubscriber.Mobile = model.Mobile;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.Fax))
                    {
                        model.Fax = model.Fax.Replace("_", "").Replace("-", "").Replace(" ", "");
                        if (model.Fax.Length == 10)
                        {
                            currentSubscriber.Fax = model.Fax;
                        }
                    }

                    #region Information Changed then Save Data
                    if (CheckChanges(orgSubscriber, currentSubscriber))
                    {
                        foreach (var cp in CurrentUser.CurrentClientGroup.SecurityGroups)
                        {
                            foreach (var s in cp.Services)
                            {
                                foreach (var a in s.Applications)
                                {
                                    if (a.ApplicationName.Contains(FrameworkUAS.BusinessLogic.Enums.Applications.Circulation.ToString()))
                                    {
                                        applicationID = a.ApplicationID;
                                        appName = a.ApplicationName;
                                        break;
                                    }
                                }
                            }
                        }

                        //Change values from Model
                        saveWaveMailing = false;

                        KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
                        currentSubscriber.DateUpdated = DateTime.Now;
                        currentSubscriber.UpdatedByUserID = CurrentUser.UserID;

                        List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
                        if (CodeList != null)
                            codeList = CodeList;
                        else
                            codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();


                        int userLogID = codeList.Where(y => y.CodeName == KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit.ToString()).Select(x => x.CodeId).FirstOrDefault();

                        //Compare Original and Current Subscriptions
                        #region Compare Subscriptions
                        myWMDetail = new FrameworkUAD.Entity.WaveMailingDetail();
                        //currentSubscriber.WaveMailingID = orgSubscriber.WaveMailingID;
                        myWMDetail.PubSubscriptionID = orgSubscriber.PubSubscriptionID;
                        myWMDetail.SubscriptionID = orgSubscriber.SubscriptionID;
                        myWMDetail.WaveMailingID = orgSubscriber.WaveMailingID;
                        waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(currentSubscriber);
                        if (orgSubscriber.IsInActiveWaveMailing == true)
                        {
                            currentSubscriber.IsInActiveWaveMailing = true;
                            if (orgSubscriber.FirstName != currentSubscriber.FirstName)
                            {
                                myWMDetail.FirstName = currentSubscriber.FirstName;
                                currentSubscriber.FirstName = orgSubscriber.FirstName;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.LastName != currentSubscriber.LastName)
                            {
                                myWMDetail.LastName = currentSubscriber.LastName;
                                currentSubscriber.LastName = orgSubscriber.LastName;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Title != currentSubscriber.Title)
                            {
                                myWMDetail.Title = currentSubscriber.Title;
                                currentSubscriber.Title = orgSubscriber.Title;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Company != currentSubscriber.Company)
                            {
                                myWMDetail.Company = currentSubscriber.Company;
                                currentSubscriber.Company = orgSubscriber.Company;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Address1 != currentSubscriber.Address1)
                            {
                                myWMDetail.Address1 = currentSubscriber.Address1;
                                currentSubscriber.Address1 = orgSubscriber.Address1;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Address2 != currentSubscriber.Address2)
                            {
                                myWMDetail.Address2 = currentSubscriber.Address2;
                                currentSubscriber.Address2 = orgSubscriber.Address2;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Address3 != currentSubscriber.Address3)
                            {
                                myWMDetail.Address3 = currentSubscriber.Address3;
                                waveMailSubscriber.Address3 = currentSubscriber.Address3;
                                currentSubscriber.Address3 = orgSubscriber.Address3;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.AddressTypeCodeId != currentSubscriber.AddressTypeCodeId)
                            {
                                myWMDetail.AddressTypeID = currentSubscriber.AddressTypeCodeId;
                                currentSubscriber.AddressTypeCodeId = orgSubscriber.AddressTypeCodeId;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.City != currentSubscriber.City)
                            {
                                myWMDetail.City = currentSubscriber.City;
                                currentSubscriber.City = orgSubscriber.City;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.RegionCode != currentSubscriber.RegionCode)
                            {
                                myWMDetail.RegionCode = currentSubscriber.RegionCode;
                                currentSubscriber.RegionCode = orgSubscriber.RegionCode;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.RegionID != currentSubscriber.RegionID)
                            {
                                myWMDetail.RegionID = currentSubscriber.RegionID;
                                currentSubscriber.RegionID = orgSubscriber.RegionID;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.ZipCode != currentSubscriber.ZipCode)
                            {
                                myWMDetail.ZipCode = currentSubscriber.ZipCode;
                                currentSubscriber.ZipCode = orgSubscriber.ZipCode;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Plus4 != currentSubscriber.Plus4)
                            {
                                myWMDetail.Plus4 = currentSubscriber.Plus4;
                                currentSubscriber.Plus4 = orgSubscriber.Plus4;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.County != currentSubscriber.County)
                            {
                                myWMDetail.County = currentSubscriber.County;
                                currentSubscriber.County = orgSubscriber.County;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Country != currentSubscriber.Country)
                            {
                                myWMDetail.Country = currentSubscriber.Country;
                                currentSubscriber.Country = orgSubscriber.Country;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.CountryID != currentSubscriber.CountryID)
                            {
                                myWMDetail.CountryID = currentSubscriber.CountryID;
                                currentSubscriber.CountryID = orgSubscriber.CountryID;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Email != currentSubscriber.Email)
                            {
                                myWMDetail.Email = currentSubscriber.Email;
                                currentSubscriber.Email = orgSubscriber.Email;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Phone != currentSubscriber.Phone)
                            {
                                myWMDetail.Phone = currentSubscriber.Phone;
                                currentSubscriber.Phone = orgSubscriber.Phone;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Fax != currentSubscriber.Fax)
                            {
                                myWMDetail.Fax = currentSubscriber.Fax;
                                currentSubscriber.Fax = orgSubscriber.Fax;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Mobile != currentSubscriber.Mobile)
                            {
                                myWMDetail.Mobile = currentSubscriber.Mobile;
                                currentSubscriber.Mobile = orgSubscriber.Mobile;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Demo7 != currentSubscriber.Demo7)
                            {
                                myWMDetail.Demo7 = currentSubscriber.Demo7;
                                currentSubscriber.Demo7 = orgSubscriber.Demo7;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.PubCategoryID != currentSubscriber.PubCategoryID)
                            {
                                myWMDetail.PubCategoryID = currentSubscriber.PubCategoryID;
                                currentSubscriber.PubCategoryID = orgSubscriber.PubCategoryID;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.PubTransactionID != currentSubscriber.PubTransactionID)
                            {
                                myWMDetail.PubTransactionID = currentSubscriber.PubTransactionID;
                                currentSubscriber.PubTransactionID = orgSubscriber.PubTransactionID;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.IsSubscribed != currentSubscriber.IsSubscribed)
                            {
                                myWMDetail.IsSubscribed = currentSubscriber.IsSubscribed;
                                currentSubscriber.IsSubscribed = orgSubscriber.IsSubscribed;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.SubscriptionStatusID != currentSubscriber.SubscriptionStatusID)
                            {
                                myWMDetail.SubscriptionStatusID = currentSubscriber.SubscriptionStatusID;
                                currentSubscriber.SubscriptionStatusID = orgSubscriber.SubscriptionStatusID;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.Copies != currentSubscriber.Copies)
                            {
                                myWMDetail.Copies = currentSubscriber.Copies;
                                currentSubscriber.Copies = orgSubscriber.Copies;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.PhoneExt != currentSubscriber.PhoneExt)
                            {
                                myWMDetail.PhoneExt = currentSubscriber.PhoneExt;
                                currentSubscriber.PhoneExt = orgSubscriber.PhoneExt;
                                saveWaveMailing = true;
                            }
                            if (orgSubscriber.IsPaid != currentSubscriber.IsPaid)
                            {
                                myWMDetail.IsPaid = currentSubscriber.IsPaid;
                                currentSubscriber.IsPaid = orgSubscriber.IsPaid;
                                saveWaveMailing = true;
                            }

                        }
                        #endregion

                        FrameworkUAD.BusinessLogic.Batch _batchBO = new FrameworkUAD.BusinessLogic.Batch();

                        KMPlatform.Object.ClientConnections kmclientobj = new KMPlatform.Object.ClientConnections(_client);

                        FrameworkUAD.Entity.Batch batch = _batchBO.Select(kmclientobj).Where(x => x.PublicationID == pubId && x.UserID == CurrentUser.UserID && x.IsActive == true).FirstOrDefault();

                        FrameworkUAS.Object.Batch b = new FrameworkUAS.Object.Batch();
                        if (batch != null)
                        {
                            b.BatchCount = batch.BatchCount;
                            b.BatchID = batch.BatchID;
                            b.BatchNumber = batch.BatchNumber;
                            b.DateCreated = batch.DateCreated;
                            b.DateFinalized = batch.DateFinalized;
                            b.IsActive = batch.IsActive;
                            b.PublicationID = batch.PublicationID;
                            b.UserID = batch.UserID;
                        }
                        #region Saving Data
                        try
                        {
                            int id = _productSubBO.ProfileSave(currentSubscriber, orgSubscriber, saveWaveMailing, applicationID, ult, userLogID, b, _client.ClientConnections, waveMailSubscriber, myWMDetail);

                            string notify = "";
                            if (b != null && b.BatchCount >= 100)
                            {
                                notify = "Batch #" + b.BatchNumber + " has reached 100 Transactions. The next batch will open automatically.";
                            }

                            if (id > 0)
                            {
                                messageToView.Text = "Record Saved. " + notify;
                                messageToView.Success = true;
                                
                            }
                            else
                            {
                                messageToView.Success = false;
                                messageToView.Text = "Something went wrong. Please try later.";
                            }
                            var json = serializer.Serialize(messageToView);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            KMPlatform.BusinessLogic.ApplicationLog worker = new KMPlatform.BusinessLogic.ApplicationLog();
                            Guid accessKey = CurrentUser.AccessKey;
                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(appName);
                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            worker.LogCriticalError(accessKey.ToString(), formatException, app, this.GetType().Name.ToString() + ".SaveNew", CurrentClientID);
                            messageToView.Text = "Something went wrong. Please try later.";
                            messageToView.Success = false;
                            var json = serializer.Serialize(messageToView);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }

                        #endregion
                    }
                    #endregion

                    #region Do not save data if info not changed
                    else
                    {
                        messageToView.Text = "Subscriber change is required to save.";
                        messageToView.Success = false;
                        var json = serializer.Serialize(messageToView);
                        return Json(json, JsonRequestBehavior.AllowGet);

                    }
                    #endregion
                }
                #endregion

                else
                {
                    messageToView.Text = "This publication is currently locked to process lists. Data can not be saved.";
                    messageToView.Success = false;
                    var json = serializer.Serialize(messageToView);
                    return Json(json, JsonRequestBehavior.AllowGet);

                }
            }
            #endregion

            else
            {


                messageToView.Text = "User does not have access to this feature.";
                messageToView.Success = false;
                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);

            }

        }
        #endregion

        #region Get Full Subscriber Record view and Json Result
        [HttpGet]
        public ActionResult Subscribe(int PubSubscriptionID = 0, int PubID = 0)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                SubscriberViewModel subscriber = new SubscriberViewModel();
                ViewBag.PubSubscriptionID = PubSubscriptionID;
                ViewBag.PubID = PubID;
                CurrentProductID = PubID;
                return View(subscriber);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult GetSubscriber(int psid = 0, int pid = 0, string type = "")
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {

                SubscriberViewModel subscriber = new SubscriberViewModel();
                KMPlatform.BusinessLogic.Client _clientBO = new KMPlatform.BusinessLogic.Client();
                FrameworkUAD.BusinessLogic.Product _productBO = new FrameworkUAD.BusinessLogic.Product();
                FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();
                FrameworkUAD.Entity.ProductSubscription originalSubscription = new FrameworkUAD.Entity.ProductSubscription();
                FrameworkUAD.BusinessLogic.ProductSubscriptionDetail _pubSubDetailBO = new FrameworkUAD.BusinessLogic.ProductSubscriptionDetail();
                List<FrameworkUAD.Entity.ProductSubscriptionDetail> pubSubDetailList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                FrameworkUAD.BusinessLogic.Subscription _subBO = new FrameworkUAD.BusinessLogic.Subscription();
                FrameworkUAD.Entity.Product _myProduct;

                if (CurrentClientID > 0)
                {
                    KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
                    if (CurrentClient != null)
                        _client = CurrentClient;
                    else
                        _client = _clientBO.Select(CurrentClientID);

                    if (pid == 0)
                        pid = CurrentProductID;
                    
                    _myProduct = _productBO.Select(pid, _client.ClientConnections, false);
                    entlst = new EntityLists();
                    //if (KM.Common.CacheHelper.GetCurrentCache("Circ_AllList") == null)
                    //{
                        
                    //    KM.Common.CacheHelper.AddToCache("Circ_AllList", entlst);
                    //}
                    //else
                    //{
                    //    entlst = (EntityLists) KM.Common.CacheHelper.GetCurrentCache("Circ_AllList");
                    //}
                    //Edit subscription
                    if (psid > 0 && _myProduct != null && string.IsNullOrEmpty(type))
                    {
                        originalSubscription = _productSubBO.SelectProductSubscription(psid, _client.ClientConnections, _client.DisplayName);
                        if (originalSubscription == null)
                        {
                            originalSubscription = new FrameworkUAD.Entity.ProductSubscription();

                        }
                        pubSubDetailList = _pubSubDetailBO.Select(originalSubscription.PubSubscriptionID, CurrentClient.ClientConnections);
                        if (pubSubDetailList == null)
                        {
                            pubSubDetailList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                        }
                        originalSubscription.ProductMapList = pubSubDetailList;

                        subscriber = new SubscriberViewModel(_myProduct, originalSubscription);
                        subMgr = new SubscriptionManager(_client, CurrentUser, _myProduct, originalSubscription, entlst, originalSubscription);
                        subscriber = subMgr.GetViewModel();

                    }
                    //New Subscription without any sequence # search
                    else if (psid == 0 && _myProduct != null && string.IsNullOrEmpty(type))
                    {
                        originalSubscription = new FrameworkUAD.Entity.ProductSubscription();
                        if (originalSubscription == null)
                        {
                            originalSubscription = new FrameworkUAD.Entity.ProductSubscription();

                        }
                        pubSubDetailList = _pubSubDetailBO.Select(originalSubscription.PubSubscriptionID, CurrentClient.ClientConnections);
                        if (pubSubDetailList == null)
                        {
                            pubSubDetailList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                        }
                        originalSubscription.ProductMapList = pubSubDetailList;

                        originalSubscription.PubID = _myProduct.PubID;
                        subscriber = new SubscriberViewModel(_myProduct, originalSubscription);
                        subMgr = new SubscriptionManager(_client, CurrentUser, _myProduct, originalSubscription, entlst, originalSubscription);
                        subscriber = subMgr.GetViewModel();

                    }
                    //SearchNew Subscription with any sequence # search
                    else if (psid == 0 && _myProduct != null && !string.IsNullOrEmpty(type))
                    {
                        originalSubscription = SearchBySequence(pid, type);
                        if (originalSubscription == null)
                        {
                            originalSubscription = new FrameworkUAD.Entity.ProductSubscription();

                        }
                        pubSubDetailList = _pubSubDetailBO.Select(originalSubscription.PubSubscriptionID, CurrentClient.ClientConnections);
                        if (pubSubDetailList == null)
                        {
                            pubSubDetailList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                        }
                        originalSubscription.ProductMapList = pubSubDetailList;

                        subscriber = new SubscriberViewModel(_myProduct, originalSubscription);
                        subMgr = new SubscriptionManager(_client, CurrentUser, _myProduct, originalSubscription, entlst, originalSubscription);
                        subscriber = subMgr.GetViewModel();

                        if (subscriber.PubSubscription.PubSubscriptionID == 0)
                        {
                            subscriber.ErrorList.Add("No_Results_Found", "No search results found.");
                        }

                    }

                    //If Subscriber selected from potential match results based on first name  or lastname or email
                    else if (psid > 0 && _myProduct != null && !string.IsNullOrEmpty(type))
                    {
                        if (type == "PRODUCT")
                        {
                            originalSubscription = _productSubBO.SelectProductSubscription(psid, _client.ClientConnections, _client.DisplayName);

                        }
                        else
                        {
                            FrameworkUAD.Entity.Subscription sub = _subBO.Select(psid, CurrentUser.CurrentClient.ClientConnections, CurrentClient.DisplayName, false);
                            originalSubscription = new FrameworkUAD.Entity.ProductSubscription(sub);
                            // Task-47706: AMS Web - Adding a new subscriber in AMS Entry and subscriber has a possible name match existing in a UAD product, the State value isn’t pre-populating on entry screen.
                            if (!string.IsNullOrEmpty(originalSubscription.RegionCode))
                            {
                                originalSubscription.RegionID = new FrameworkUAD_Lookup.BusinessLogic.Region().Select().First(r => r.RegionCode == originalSubscription.RegionCode).RegionID;
                            }
                        }
                        if (originalSubscription == null)
                        {
                            originalSubscription = new FrameworkUAD.Entity.ProductSubscription();

                        }
                        pubSubDetailList = _pubSubDetailBO.Select(originalSubscription.PubSubscriptionID, CurrentClient.ClientConnections);
                        if (pubSubDetailList == null)
                        {
                            pubSubDetailList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                        }
                        originalSubscription.ProductMapList = pubSubDetailList;

                        subscriber = new SubscriberViewModel(_myProduct, originalSubscription);
                        subMgr = new SubscriptionManager(_client, CurrentUser, _myProduct, originalSubscription, entlst, originalSubscription);
                        subscriber = subMgr.GetViewModel();



                    }
                    //Else return error message
                    else
                    {
                        subscriber.ErrorList.Add("NoPublisherSelected", "Please re-select Publisher.");
                    }

                    ViewBag.PubSubscriptionID = originalSubscription.PubSubscriptionID;
                    ViewBag.PubID = pid;
                    var prodlist = _productBO.Select(new KMPlatform.Object.ClientConnections(CurrentClient));
                    subscriber.ProductList = prodlist.Where(x => x.IsCirc == true && x.IsActive == true).ToList();
                }
                else
                {
                    subscriber.ErrorList.Add("NoPublisherSelected", "Please Select Publisher.");
                }

                var jsonResult = Json(subscriber, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
                
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region Save Full Subscriber Record
        [HttpPost]
        public ActionResult SaveSubscriberNew(SubscriberViewModel data)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess) )
            {
                Search searchmodel = new Search();
                SubscriberViewModel savedmodel = new SubscriberViewModel();
                KMPlatform.BusinessLogic.Client _clientBO = new KMPlatform.BusinessLogic.Client();
                FrameworkUAD.BusinessLogic.Product _productBO = new FrameworkUAD.BusinessLogic.Product();
                FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();

                //If current client is not null select current client to avoid roundtrip to DB
                KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
                if (CurrentClient != null)
                {
                    _client = CurrentClient;
                }
                else
                {
                    _client = _clientBO.Select(CurrentClientID);
                }
                //Check If current product and selected product is different
                data.Product = _productBO.Select(data.PubSubscription.PubID, _client.ClientConnections);
                data.PubSubscription.PubCode = data.Product.PubCode;
                data.PubSubscription.PubName = data.Product.PubName;

                int notSearched = 0;
                if (Session["Search"] != null)
                    searchmodel = (Search)Session["Search"];
                else
                    notSearched = 1;

                string requiredFields = RequiredFields(data);

                if (string.IsNullOrEmpty(requiredFields))
                {

                    if (data.PubSubscription.PubSubscriptionID > 0)
                    {
                        data.OriginalPubSubscription = _productSubBO.SelectProductSubscription(data.PubSubscription.PubSubscriptionID, _client.ClientConnections, CurrentClient.DisplayName);
                    }
                    subMgr = new SubscriptionManager(_client, CurrentUser, data.Product, data.PubSubscription, data.entlst, data.OriginalPubSubscription);

                    savedmodel = subMgr.SaveSubscription(data);

                    if (savedmodel.ErrorList.ContainsKey("Success_Complete"))
                    {
                        ViewBag.PubSubscriptionID = savedmodel.PubSubscription.PubSubscriptionID;
                        ViewBag.PubID = savedmodel.PubSubscription.PubID;
                        if (notSearched != 1)
                        {
                            //try
                            //{
                            //    FrameworkUAD.Entity.ProductSubscription prev = searchmodel.SearchResults.Select(x => x).Where(y => y.PubSubscriptionID == savedmodel.PubSubscription.PubSubscriptionID).FirstOrDefault();
                            //    searchmodel.SearchResults.Remove(prev);
                            //    searchmodel.SearchResults.Add(savedmodel.PubSubscription);
                            //    searchmodel.SearchResults = searchmodel.SearchResults.OrderBy(x => x.FullName).ToList();
                            //    Session["Search"] = searchmodel;
                            //}
                            //catch (Exception)
                            //{
                            //    Session["Search"] = null;
                            //}
                        }
                        return Json(savedmodel, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        return Json(savedmodel, JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {
                    data.ErrorList.Add("ValidationErrors", "Please update or provide answers/selections for the following fields: " + requiredFields);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region Private Helper Methods
        #region Search Subscriber 

        [HttpPost]
        public JsonResult SearchMatch(FrameworkUAD.Entity.ProductSubscription matchsub)
        {

            List<FrameworkUAD.Entity.ProductSubscription> subs = new List<FrameworkUAD.Entity.ProductSubscription>();
            FrameworkUAD.Entity.ProductSubscription soloSubscription = new FrameworkUAD.Entity.ProductSubscription();


            if (!string.IsNullOrEmpty(matchsub.SequenceID.ToString()) && matchsub.SequenceID != 0)
            {
                soloSubscription = SearchBySequence(matchsub.PubID, matchsub.SequenceID.ToString());
                subs.Add(soloSubscription);
                var jsonResult = Json(subs, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            else if (!string.IsNullOrEmpty(matchsub.FirstName) || !string.IsNullOrEmpty(matchsub.LastName) || !string.IsNullOrEmpty(matchsub.Email))
            {
                DataTable result = DoSuggestMatch(matchsub);

                List<MatchResult> resultList = Comparision.ConvertDataTable<MatchResult>(result);
                var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }

            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }


        }

        private FrameworkUAD.Entity.ProductSubscription SearchBySequence(int PubID, string seqSearch = "0")
        {
            // This method is for suggest match and search
            List<FrameworkUAD.Entity.ProductSubscription> subs = new List<FrameworkUAD.Entity.ProductSubscription>();
            List<FrameworkUAD.Entity.ProductSubscription> checkSubs = new List<FrameworkUAD.Entity.ProductSubscription>();
            FrameworkUAD.Entity.ProductSubscription soloSubscription = new FrameworkUAD.Entity.ProductSubscription();

            KMPlatform.Entity.Client soloPublisher = new KMPlatform.Entity.Client();
            KMPlatform.Object.Product soloPublication = new KMPlatform.Object.Product();

            FrameworkUAD.BusinessLogic.ProductSubscription prodSubWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();

            int seq = 0;
            int.TryParse(seqSearch, out seq);

            if (seq > 0)
            {
                #region Search on Sequence
                subs = prodSubWorker.Search(CurrentUser.CurrentClient.ClientConnections, CurrentUser.CurrentClient.DisplayName, "", "", "", "", "", "", "", "", "", "", "", seq);
                // Need to do a bulk select here for subs by SubscriptionID

                checkSubs = subs.Where(x => x.PubID == PubID).ToList();
                if (checkSubs.Count > 0)
                {
                    if (checkSubs.Count > 0)
                        soloSubscription = subs.OrderByDescending(x => x.DateUpdated).FirstOrDefault(x => x.PubID == PubID);
                    else
                        soloSubscription = subs.FirstOrDefault(x => x.PubID == PubID);
                }
                else
                    soloSubscription = subs.SingleOrDefault(x => x.PubID == PubID);
                #endregion


            }

            //else if (!string.IsNullOrEmpty(tbFname.Text) || !string.IsNullOrEmpty(tbLname.Text) || !string.IsNullOrEmpty(tbEmail.Text))
            //{
            //    fnameSuggestMatch = string.Empty;
            //    lnameSuggestMatch = string.Empty;
            //    emailSuggestMatch = string.Empty;

            //    DoSuggestMatch();
            //}
            //else
            //{
            //    Core_AMS.Utilities.WPF.Message("At least one search condition must exist to perform a search.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Search");
            //    busy.IsBusy = false;
            //}
            return soloSubscription;

        }

        private DataTable DoSuggestMatch(FrameworkUAD.Entity.ProductSubscription matchsub)
        {
            DataTable result = new DataTable();

            FrameworkUAD.BusinessLogic.Subscription subWorker = new FrameworkUAD.BusinessLogic.Subscription();

            result = subWorker.FindMatches(matchsub.PubID, matchsub.FirstName, matchsub.LastName, matchsub.Company, matchsub.Address1, matchsub.RegionCode, matchsub.ZipCode, matchsub.Phone, matchsub.Email, matchsub.Title, CurrentUser.CurrentClient.ClientConnections);
            result.Rows.Cast<DataRow>().Where(r => (int)r["SubscriptionID"] == matchsub.SubscriptionID).ToList().ForEach(r => r.Delete());
            result.AcceptChanges();

            return result;

        }

        #endregion
        #region Check if changes made to Subscriber Profile
        private bool CheckChanges(FrameworkUAD.Entity.ProductSubscription orgSubscriber, FrameworkUAD.Entity.ProductSubscription currentSubscriber)
        {
            bool infochanged = false;

            if ((!Comparision.AreEqual(orgSubscriber.FirstName, currentSubscriber.FirstName)) || (!Comparision.AreEqual(orgSubscriber.LastName, currentSubscriber.LastName)) || (!Comparision.AreEqual(orgSubscriber.Title, currentSubscriber.Title))
            || (!Comparision.AreEqual(orgSubscriber.Company, currentSubscriber.Company)) || (!Comparision.AreEqual(orgSubscriber.Address1, currentSubscriber.Address1)) || (!Comparision.AreEqual(orgSubscriber.Address2, currentSubscriber.Address2))
            || (!Comparision.AreEqual(orgSubscriber.Address3, currentSubscriber.Address3)) || (orgSubscriber.AddressTypeCodeId != currentSubscriber.AddressTypeCodeId) || (!Comparision.AreEqual(orgSubscriber.City, currentSubscriber.City))
            || (!Comparision.AreEqual(orgSubscriber.RegionCode, currentSubscriber.RegionCode)) || (orgSubscriber.RegionID != currentSubscriber.RegionID) || (!Comparision.AreEqual(orgSubscriber.ZipCode, currentSubscriber.ZipCode))
            || (!Comparision.AreEqual(orgSubscriber.Plus4, currentSubscriber.Plus4)) || (!Comparision.AreEqual(orgSubscriber.County, currentSubscriber.County)) || (!Comparision.AreEqual(orgSubscriber.Country, currentSubscriber.Country))
            || (orgSubscriber.CountryID != currentSubscriber.CountryID) || (!Comparision.AreEqual(orgSubscriber.Email, currentSubscriber.Email)) || (!Comparision.AreEqual(orgSubscriber.Phone, currentSubscriber.Phone))
            || (!Comparision.AreEqual(orgSubscriber.Fax, currentSubscriber.Fax)) || (!Comparision.AreEqual(orgSubscriber.Mobile, currentSubscriber.Mobile)) || (!Comparision.AreEqual(orgSubscriber.PhoneExt, currentSubscriber.PhoneExt))
            || (!Comparision.AreEqual(orgSubscriber.Website, currentSubscriber.Website)))
            {
                infochanged = true;
            }

            return infochanged;
        }
        #endregion
        #region Check if any required field is missing
        private string RequiredFields(SubscriberViewModel data)
        {
            string required = "";
            if (data.PubSubscription.IsNewSubscription == false)
            {
                if (data.TriggerQualDate == true)
                {
                    required += "Qualification Date, ";
                }
                if (data.PubSubscription.IsSubscribed == true && (data.PubSubscription.PubCategoryID == 0))
                {
                    required += "Category Type, ";
                }
                if (string.IsNullOrEmpty(data.PubSubscription.OnBehalfOf) && data.btnOnBehalfKillChecked == true)
                {
                    required += "On Behalf Request, ";
                }
            }
            else if (data.PubSubscription.IsNewSubscription == true)
            {
                if (data.TriggerQualDate == true)
                {
                    required += "Qualification Date, ";
                }
                if (data.PubSubscription.IsSubscribed == true && data.PubSubscription.PubCategoryID == 0)
                {
                    required += "Category Type, ";
                }
            }
            if (string.IsNullOrEmpty(data.PubSubscription.Demo7))
            {
                required += "Media Type, ";
            }

            if (data.AreQuestionsRequired)
            {

                if (data.ProductResponseList != null)
                {
                    string unAnswered = RequiredAnswered(data);

                    if (!string.IsNullOrEmpty(unAnswered) && data.PubSubscription.IsSubscribed == true)
                    {
                        required += unAnswered + ", ";
                    }
                }

            }

            return required;
        }
        #endregion
        #region Check if all required responses are provided
        private string RequiredAnswered(SubscriberViewModel data)
        {
            StringBuilder sb = new StringBuilder();
            List<string> questionNotAnswered = new List<string>();
            KMPlatform.BusinessLogic.Client _clientBO = new KMPlatform.BusinessLogic.Client();
            FrameworkUAD.BusinessLogic.Product _productBO = new FrameworkUAD.BusinessLogic.Product();
            FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();
            KMPlatform.Entity.Client _client = _clientBO.Select(CurrentClientID);


            FrameworkUAD.BusinessLogic.ResponseGroup _responseGroupBO = new FrameworkUAD.BusinessLogic.ResponseGroup();
            FrameworkUAD.BusinessLogic.CodeSheet _codeSheetBO = new FrameworkUAD.BusinessLogic.CodeSheet();

            List<FrameworkUAD.Entity.ResponseGroup> questions = _responseGroupBO.Select(data.Product.PubID, _client.ClientConnections);
            FrameworkUAD_Lookup.BusinessLogic.Code codeBO = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = codeBO.Select();
            if (questions != null)
            {
                questions = questions.Where(x => x.IsActive == true && (!x.ResponseGroupName.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase) || !x.DisplayName.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase))
                   && (x.ResponseGroupTypeId == codeList.SingleOrDefault(r => r.CodeName.Equals(FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " "))).CodeId ||
                    x.ResponseGroupTypeId == codeList.SingleOrDefault(r => r.CodeName.Equals(FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_Only.ToString().Replace("_", " "))).CodeId))
                   .OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();
            }

            List<FrameworkUAD.Entity.CodeSheet> answers = _codeSheetBO.Select(data.Product.PubID, _client.ClientConnections);
            if (answers == null || answers.Count == 0)
            {
                answers = answers.Where(x => x.IsActive == true).ToList();
            }
            foreach (var q in questions)
            {
                if (q.IsRequired == true)
                {
                    var x = (from rml in data.ProductResponseList
                             join r in answers on rml.CodeSheetID equals r.CodeSheetID
                             join rt in questions on r.ResponseGroupID equals rt.ResponseGroupID
                             where rt.IsRequired == true && rt.ResponseGroupID == q.ResponseGroupID
                             select rml.SubscriptionID).ToList();
                    if (x.Count == 0)
                    {
                        if (!questionNotAnswered.Contains(q.DisplayName))
                        {
                            questionNotAnswered.Add(q.DisplayName);
                        }
                    }
                }
            }

            if (questionNotAnswered.Count != 0)
            {
                foreach (string s in questionNotAnswered)
                {
                    if (sb.Length == 0)
                        sb.Append(s);
                    else
                        sb.Append("," + s);
                }
            }
            string ua = sb.ToString();
            return ua;
        }
        #endregion
        #endregion

       
        
        public ActionResult SubscriberLastDateUpdated(int psid)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                KMPlatform.Entity.Client _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);
                if (psid > 0)
                {
                    var pubsub = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectProductSubscription(psid, _client.ClientConnections, _client.DisplayName);
                    return Json(pubsub.DateUpdated, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }
        #region Unload and unlock Subcriber
        private void Subscriber_Unloaded(int psid)
        {
            KMPlatform.BusinessLogic.Client _clientBO = new KMPlatform.BusinessLogic.Client();
            FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();
            KMPlatform.Entity.Client _client = _clientBO.Select(CurrentClientID);
            _productSubBO.UpdateLock(psid, false, CurrentUser.UserID, _client.ClientConnections);

        }

        // Head: Default/RecordCloseTime
        [HttpHead]
        public void RecordCloseTime(int psid)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                Subscriber_Unloaded(psid);
            }
           

        }
        #endregion
        
    }
}