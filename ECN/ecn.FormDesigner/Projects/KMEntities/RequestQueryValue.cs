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
    
    public partial class RequestQueryValue
    {
        public int RequestQueryValue_Seq_ID { get; set; }
        public int Control_ID { get; set; }
        public int Rule_Seq_ID { get; set; }
        public string Name { get; set; }
    
        public virtual Control Control { get; set; }
        public virtual Rule Rule { get; set; }
    }
}
