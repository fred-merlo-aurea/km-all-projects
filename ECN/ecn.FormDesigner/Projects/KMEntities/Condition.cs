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
    
    public partial class Condition
    {
        public int Condition_Seq_ID { get; set; }
        public int Control_ID { get; set; }
        public int ConditionGroup_Seq_ID { get; set; }
        public int Operation_ID { get; set; }
        public string Value { get; set; }
    
        public virtual ConditionGroup ConditionGroup { get; set; }
        public virtual Control Control { get; set; }
    }
}
