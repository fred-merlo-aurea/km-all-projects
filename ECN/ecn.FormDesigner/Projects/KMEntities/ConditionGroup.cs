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
    
    public partial class ConditionGroup
    {
        public ConditionGroup()
        {
            this.Conditions = new HashSet<Condition>();
            this.ConditionGroup1 = new HashSet<ConditionGroup>();
            this.Notifications = new HashSet<Notification>();
            this.Rules = new HashSet<Rule>();
        }
    
        public int ConditionGroup_Seq_ID { get; set; }
        public Nullable<int> MainGroup_ID { get; set; }
        public bool LogicGroup { get; set; }
    
        public virtual ICollection<Condition> Conditions { get; set; }
        public virtual ICollection<ConditionGroup> ConditionGroup1 { get; set; }
        public virtual ConditionGroup ConditionGroup2 { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Rule> Rules { get; set; }
    }
}