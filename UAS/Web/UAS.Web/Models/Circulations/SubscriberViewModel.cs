using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class SubscriberViewModel
    {
       
        public string ClientName { get; set; }
       
        //Common
        public bool AllowDataEntry { get; set; }
        public bool Enabled { get; set; }
        public bool Saved { get; set; }
        //Status Tab Transition Fields 
        public int CategoryCodeTypeID { get; set; }
        public string TransactionName { get; set; }
        public bool SubStatusEnabled { get; set; }
        public bool TriggerQualDate { get; set; }
        public bool IsCountryEnabled { get; set; }
        public bool btnPOKillChecked { get; set; }
        public bool btnPersonKillChecked { get; set; }
        public bool btnOnBehalfKillChecked { get; set; }
        public bool CategoryFreePaidEnabled { get; set; }
        public bool CategoryCodeEnabled { get; set; }
        public bool ReactivateButtonEnabled { get; set; }
        //End
        public bool MadeAdHocChange { get; set; }
        public bool MadeResponseChange { get; set; }
        public bool MadePaidChange { get; set; }
        public bool MadePaidBillToChange { get; set; }
        public bool IsCopiesEnabled { get; set; }
        public bool AreQuestionsRequired { get; set; }
        public FrameworkUAD.Entity.ProductSubscription PubSubscription { get; set; }
        public FrameworkUAD.Entity.ProductSubscription OriginalPubSubscription { get; set; }
        public FrameworkUAD.Entity.Product Product { get; set; }
        public FrameworkUAD.Entity.PaidBillTo MyPaidBillTo { get; set; }
        public FrameworkUAD.Entity.SubscriptionPaid MySubscriptionPaid { get; set; }
        public List<HistoryContainer> BatchHistoryList { get; set; }
        public List<Question> QuestionList { get; set; }
        public List<FrameworkUAD.Entity.Product> ProductList { get; set; }
        public EntityLists entlst { get; set; }
        public Dictionary<string, string> ErrorList { get; set; }

        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> ProductResponseList { get; set; }
        public string CreatedByUser { get; internal set; }
        public string LastModifiedByUser { get; internal set; }

        public SubscriberViewModel(FrameworkUAD.Entity.Product product, FrameworkUAD.Entity.ProductSubscription prodSubscription)
        {
            entlst = new EntityLists();
            Product = product;
            PubSubscription = prodSubscription;
            MyPaidBillTo = new FrameworkUAD.Entity.PaidBillTo();
            MySubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
            ErrorList = new Dictionary<string, string>();
            ProductList = new List<FrameworkUAD.Entity.Product>();
           
        }

        public SubscriberViewModel()
        {
            Product = new FrameworkUAD.Entity.Product();
            entlst = new EntityLists();
            MyPaidBillTo = new FrameworkUAD.Entity.PaidBillTo();
            MySubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
            ErrorList = new Dictionary<string, string>();
            ProductList = new List<FrameworkUAD.Entity.Product>();
            QuestionList = new List<Question>();
            OriginalPubSubscription = new FrameworkUAD.Entity.ProductSubscription();
        }

    }
    
    public class Question
    {

        public string DisplayName { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Answer> SelectedAnswers { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultiple { get; set; }
        public int GroupID { get; set; }
        public bool OtherChanged { get; set; }
        public string OtherValue { get; set; }
        public bool ShowOther { get; set; }
        public int ResponseCounter { get; set; }
        public int ResponseDisplay { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool ShowMore { get; set; }
        public bool IsSelected { get; set; }
        public Question(string name, int id, List<Answer> answers,   bool required, bool multiple, string other)
        {
            this.DisplayName = name;
            this.GroupID = id;
            this.Answers = answers.OrderBy(x => x.DisplayOrder).ToList();
            this.IsRequired = required;
            this.IsMultiple = multiple;
            this.ShowMore = false;
            this.ResponseDisplay = 50;
            this.OtherValue = other;
            this.DateCreated = null;
            this.IsSelected = false;
            if (this.Answers.Where(x => x.IsOther == true && x.IsSelected == true).Count() > 0)
                this.ShowOther = true;
        }

        public Question(string name, int id, List<Answer> answers, List<Answer> selectedAnswers, bool required, bool multiple, string other)
        {
            this.ResponseDisplay = answers.Count() > 50 ? 50 : answers.Count();
            this.ShowMore = answers.Count() > 50 ? true : false;
            this.ResponseCounter = answers.Count();
            this.DisplayName = name;
            this.GroupID = id;
            this.Answers = answers.OrderBy(x => x.DisplayOrder).ToList();
            this.IsRequired = required;
            this.IsMultiple = multiple;
            this.OtherValue = other;
            this.SelectedAnswers = selectedAnswers;
            this.DateCreated = selectedAnswers!=null && selectedAnswers.Count>0
                    ? selectedAnswers.OrderByDescending(t => t.DateCreated).Select(x => x.DateCreated).FirstOrDefault()
                    :null;
            this.IsSelected = selectedAnswers != null && selectedAnswers.Count > 0 ? true : false;
            if (this.Answers.Where(x => x.IsOther == true && x.IsSelected == true).Count() > 0)
                this.ShowOther = true;
        }


    }

    public class Answer
    {
        public int CodeSheetID { get; set; }
        public int PubID { get; set; }
        public string ResponseDesc { get; set; }
        public int ResponseGroupID { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsOther { get; set; }
        public bool IsSelected { get; set; }
        public DateTime? DateCreated { get; set; }
        public Answer(int id, int pubID, string description, string value, int respGrpID, int? order, bool? active, bool? other, bool selected,DateTime? dateCreated)
        {
            this.CodeSheetID = id;
            this.PubID = pubID;
            this.ResponseDesc = value + ". " + description;
            this.ResponseGroupID = respGrpID;
            this.DisplayOrder = order;
            this.IsActive = active;
            this.IsOther = other;
            this.IsSelected = selected;
            this.DateCreated = dateCreated;
        }


    }
}