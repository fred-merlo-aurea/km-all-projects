//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KMEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Control
    {
        public Control()
        {
            this.Conditions = new HashSet<Condition>();
            this.FormControlProperties = new HashSet<FormControlProperty>();
            this.FormControlPropertyGrids = new HashSet<FormControlPropertyGrid>();
            this.Rules = new HashSet<Rule>();
            this.ThirdPartyQueryValues = new HashSet<ThirdPartyQueryValue>();
            this.NewsletterGroups = new HashSet<NewsletterGroup>();
            this.ControlCategories = new HashSet<ControlCategory>();
            this.OverwritedataPostValues = new HashSet<OverwritedataPostValue>();
            this.RequestQueryValues = new HashSet<RequestQueryValue>();
        }
    
        public int Control_ID { get; set; }
        public int Form_Seq_ID { get; set; }
        public int Order { get; set; }
        public int Type_Seq_ID { get; set; }
        public string FieldLabel { get; set; }
        public Nullable<int> FieldID { get; set; }
        public System.Guid HTMLID { get; set; }
        public string FieldLabelHTML { get; set; }
    
        public virtual ICollection<Condition> Conditions { get; set; }
        public virtual ICollection<FormControlProperty> FormControlProperties { get; set; }
        public virtual ICollection<FormControlPropertyGrid> FormControlPropertyGrids { get; set; }
        public virtual ICollection<Rule> Rules { get; set; }
        public virtual ICollection<ThirdPartyQueryValue> ThirdPartyQueryValues { get; set; }
        public virtual ControlType ControlType { get; set; }
        public virtual ICollection<NewsletterGroup> NewsletterGroups { get; set; }
        public virtual ICollection<ControlCategory> ControlCategories { get; set; }
        public virtual ICollection<OverwritedataPostValue> OverwritedataPostValues { get; set; }
        public virtual ICollection<RequestQueryValue> RequestQueryValues { get; set; }
        public virtual Form Form { get; set; }
    }
}