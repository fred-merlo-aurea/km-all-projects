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
    
    public partial class Notification
    {
        public int Notification_Seq_ID { get; set; }
        public Nullable<int> ConditionGroup_Seq_ID { get; set; }
        public bool IsConfirmation { get; set; }
        public bool IsInternalUser { get; set; }
        public bool IsDoubleOptIn { get; set; }
        public string FromName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string LandingPage { get; set; }
        public int Form_Seq_ID { get; set; }
    
        public virtual ConditionGroup ConditionGroup { get; set; }
        public virtual Form Form { get; set; }
    }
}
