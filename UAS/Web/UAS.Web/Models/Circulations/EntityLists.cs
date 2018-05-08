using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class EntityLists
    {
       
        private FrameworkUAD_Lookup.BusinessLogic.Country _countryBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.Region _regionBO { get; set; }
        private FrameworkUAD.BusinessLogic.Product _productBO { get; set; }
        private FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.CodeType _codeTypeBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.Code _codeBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType _transactionTypeCodeBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.TransactionCode _transactionCodeBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.CategoryCode _categoryCodeBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType _categoryCodeTypeBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.Action _actionBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus _subscriptionStatusBO { get; set; }
        private FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix _subscriptionStatusMatrixBO { get; set; }

        public List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList{ get; set; }
        public List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList{ get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> qSourceList{ get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> parList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> marketingList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> mediaTypeList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> subscrtypelst { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> codeList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> addressTypeList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Country> countryList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Region> regions { get; set; }
        public List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.CategoryCode> categoryCodeList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Action> actionList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sstList { get; set; }
        public List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> ssmList { get; set; }
        public List<FrameworkUAD.Entity.CodeSheet> codeSheet { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> paymentTypes { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> creditCardTypes { get; set; }
      

        public List<SelectListItem> PayMentTypeSelectList { get; set; }
        public List<SelectListItem> CreditCardTypeSelectList { get; set; }
        public List<SelectListItem> creditcardMonthSelectList { get; set; }
        public List<SelectListItem> creditcardYearSelectList { get; set; }

        public EntityLists()
        {
            _codeTypeBO = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
            codeTypeList = _codeTypeBO.Select();

            _codeBO = new FrameworkUAD_Lookup.BusinessLogic.Code();
            codeList = _codeBO.Select();

            _countryBO = new FrameworkUAD_Lookup.BusinessLogic.Country();
            countryList = _countryBO.Select().OrderBy(x =>x.SortOrder).ToList();

            countryList = countryList.OrderByDescending(o => o.CountryID == 1)
                .ThenByDescending(o => o.CountryID == 2)
                .ThenByDescending(o => o.CountryID == 429)
                .ThenBy(x => x.ShortName).ToList();

            _regionBO = new FrameworkUAD_Lookup.BusinessLogic.Region();
            regions = _regionBO.Select().OrderBy(x=>x.RegionName).ToList();
           

            _categoryCodeTypeBO = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
            catTypeList = _categoryCodeTypeBO.Select();
            

            _categoryCodeBO = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
            categoryCodeList = _categoryCodeBO.Select();
            

            int addrType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Address.ToString())).CodeTypeId;
            addressTypeList = codeList.Where(x => x.CodeTypeId == addrType).OrderBy(x => x.DisplayOrder).ToList();

            if (addressTypeList.Where(x => x.CodeId == 0).Count() == 0)
                addressTypeList.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, DisplayName = "", IsActive = true });
           
            int qSourceType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).CodeTypeId;
            qSourceList = codeList.Where(x => x.CodeTypeId == qSourceType).OrderBy(x=>x.DisplayOrder).ToList();
            

            int par3cType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Par3c.ToString())).CodeTypeId;
            parList = codeList.Where(x => x.CodeTypeId == par3cType).OrderBy(x => x.DisplayOrder).ToList();
            
            int mediaType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString())).CodeTypeId;
            mediaTypeList = codeList.Where(x => x.CodeTypeId == mediaType).OrderBy(x => x.DisplayOrder).ToList();
            
            int subscrtype = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Subscriber_Source.ToString().Replace("_", " "))).CodeTypeId;
            subscrtypelst = codeList.Where(x => x.CodeTypeId == subscrtype).OrderBy(x => x.DisplayOrder).ToList();
            

            //Payment Type List
            int paymentTypeId = codeTypeList.SingleOrDefault(s => s.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Payment.ToString()).CodeTypeId;
            paymentTypes = codeList.Where(x => x.CodeTypeId == paymentTypeId).ToList();
            if (paymentTypes.Where(x => x.CodeId == 0).Count() == 0)
                paymentTypes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, CodeTypeId = paymentTypeId, IsActive = true, CodeValue = "", CodeName = "", DisplayName = "" });

            PayMentTypeSelectList = new List<SelectListItem>();
            foreach (var cc in paymentTypes)
            {
                PayMentTypeSelectList.Add(new SelectListItem { Text = cc.DisplayName, Value = cc.CodeId.ToString() });
            }

            //Credit Card Type List
            int ccTypeId = codeTypeList.SingleOrDefault(s => s.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Credit_Card.ToString().Replace("_", " ")).CodeTypeId;
            creditCardTypes = codeList.Where(x => x.CodeTypeId == ccTypeId).ToList();
            if (creditCardTypes.Where(x => x.CodeId == 0).Count() == 0)
                creditCardTypes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, CodeTypeId = ccTypeId, IsActive = true, CodeValue = "", CodeName = "", DisplayName = "" });
            CreditCardTypeSelectList = new List<SelectListItem>();
            foreach (var cc in creditCardTypes)
            {
                CreditCardTypeSelectList.Add(new SelectListItem { Text = cc.DisplayName, Value = cc.CodeId.ToString() });
            }


            //Credit Card  Month Selector List
            this.creditcardMonthSelectList = new List<SelectListItem>();
          
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "1",Text = "1, Janurary" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "2",Text="2, February" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "3",Text="3, March" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "4",Text="4, April" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "5",Text="5, May" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "6",Text="6, June" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "7",Text="7, July" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "8",Text="8, August" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "9",Text="9, September" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "10",Text = "10, October" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "11",Text = "11, November" });
            this.creditcardMonthSelectList.Add(new SelectListItem { Value = "12",Text = "12, December" });
            //Credit Card  Year Selector List
            this.creditcardYearSelectList = new List<SelectListItem>();
           
            // Build years
            for (int i = 15; i >= 0; i--)
            {
                int year = DateTime.Now.Year - i;
                this.creditcardYearSelectList.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            }
            int yearCounter = 0;
            for (int i = 15; i <= 30; i++)
            {
                yearCounter += 1;
                int year = DateTime.Now.Year + yearCounter;
                this.creditcardYearSelectList.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            }

            int marketingType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Marketing.ToString())).CodeTypeId;
            marketingList = codeList.Where(x => x.CodeTypeId == marketingType).ToList();

            _transactionCodeBO = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
            transCodeList = _transactionCodeBO.Select();

            _transactionTypeCodeBO = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType();
            transCodeTypeList = _transactionTypeCodeBO.Select();

            _actionBO = new FrameworkUAD_Lookup.BusinessLogic.Action();
            actionList = _actionBO.Select();

           

            _subscriptionStatusBO = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus();
            sstList = _subscriptionStatusBO.Select();

            _subscriptionStatusMatrixBO = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix();
            ssmList = _subscriptionStatusMatrixBO.Select();

            
        }

    }
}