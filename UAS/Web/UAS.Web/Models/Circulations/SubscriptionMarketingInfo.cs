using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class SubscriptionMarketingInfo
    {
        #region Private Properties
        private string _accountNumber;
        private string _memberGroup;
        private string _origSubSrc;
        private string _subSrc;
        private int _subSrcID;
        private int _deliverability;
        private string _verify;
        private bool _triggerQualDate;
        private bool _enabled;
        #endregion

        #region Public Properties
        public string AccountNumber
        {
            get { return _accountNumber; }
            set
            {
                _accountNumber = value;
               
            }
        }
        public string MemberGroup
        {
            get { return _memberGroup; }
            set
            {
                _memberGroup = value;
               
            }
        }
        public string OriginalSubscriberSourceCode
        {
            get { return _origSubSrc; }
            set
            {
                _origSubSrc = value;
                
            }
        }
        public string SubscriberSourceCode
        {
            get { return _subSrc; }
            set
            {
                _subSrc = value;
               
            }
        }
        public int? SubSrcID
        {
            get { return _subSrcID; }
            set
            {
                _subSrcID = (value ?? 0);
               
            }
        }
        public int? Deliverability
        {
            get { return _deliverability; }
            set
            {
                if (_deliverability != (value ?? 0))
                    _deliverability = (value ?? 0);
               
            }
        }
        public string Verify
        {
            get { return _verify; }
            set
            {
                _verify = value;
                
            }
        }
        public bool TriggerQualDate
        {
            get { return _triggerQualDate; }
            set
            {
                _triggerQualDate = value;
               
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
               
            }
        }

        public DateTime CreatedDate { get; set;}
        public DateTime? UpdatedDate { get; set; }

        public List<SelectListItem> DeliveryMethods
        {
            get;
            set;
        }

        public List<SelectListItem> SubSourceList { get; set; }
        #endregion

        public SubscriptionMarketingInfo()
        {

        }
        public SubscriptionMarketingInfo(FrameworkUAD.Entity.ProductSubscription productSubscription, EntityLists entlst)
        {
            #region Deliverability & SubSource

            int deliverCodeTypeId = entlst.codeTypeList.SingleOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString())).CodeTypeId;

            DeliveryMethods = entlst.MediaTypesSelectList;

            if (productSubscription.IsNewSubscription == false && !string.IsNullOrEmpty(productSubscription.Demo7))
            {
                int demo7 = entlst.codeList.SingleOrDefault(x => x.CodeTypeId == deliverCodeTypeId && x.CodeValue.Equals(productSubscription.Demo7)).CodeId;
                Deliverability = demo7;
            }

            int subSourceID = entlst.codeTypeList.SingleOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Subscriber_Source.ToString().Replace("_", " "))).CodeTypeId;

            SubSourceList = entlst.SubsrcTypesSelectList;

            #endregion

            if (productSubscription != null)
            {
                if (!string.IsNullOrEmpty(productSubscription.OrigsSrc))
                {
                    OriginalSubscriberSourceCode = productSubscription.OrigsSrc;
                }
                else if (string.IsNullOrEmpty(productSubscription.OrigsSrc) && !string.IsNullOrEmpty(productSubscription.SubscriberSourceCode))
                {
                    OriginalSubscriberSourceCode = productSubscription.SubscriberSourceCode;
                }

                SubscriberSourceCode = productSubscription.SubscriberSourceCode;
                SubSrcID = productSubscription.SubSrcID;
                AccountNumber = productSubscription.AccountNumber;
                MemberGroup = productSubscription.MemberGroup;
                Verify = productSubscription.Verify;
                
                #region DatePickers
                if (productSubscription.DateCreated != null && !productSubscription.IsNewSubscription)
                    CreatedDate = productSubscription.DateCreated;


                if (productSubscription.DateUpdated != null)
                    UpdatedDate = productSubscription.DateUpdated;
               
                #endregion
            }
        }

    }
}