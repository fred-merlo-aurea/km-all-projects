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
    
    public partial class FormStatisticLog
    {
        public long FormStatisticLog_Seq_ID { get; set; }
        public Nullable<long> FormStatistic_Seq_ID { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public Nullable<System.DateTime> StartPage { get; set; }
        public Nullable<System.DateTime> FinishPage { get; set; }
    
        public virtual FormStatistic FormStatistic { get; set; }
    }
}
