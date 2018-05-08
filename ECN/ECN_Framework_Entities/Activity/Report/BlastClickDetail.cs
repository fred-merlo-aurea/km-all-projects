using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class BlastClickDetail
    {
        public BlastClickDetail() { }

        [ExportAttribute(Format= 0)]
        public string Campaign { get; set; }
        [ExportAttribute(Format = 0)]
        public string Link { get; set; }
        [ExportAttribute(Format = 0)]
        public string FirstName { get; set; }
        [ExportAttribute(Format = 0)]
        public string LastName { get; set; }
        [ExportAttribute(Format = 0)]
        public string Title { get; set; }
        [ExportAttribute(Format = 0)]
        public string Company { get; set; }
        [ExportAttribute(Format = 0)]
        public string Address { get; set; }
        [ExportAttribute(Format = 0)]
        public string City { get; set; }
        [ExportAttribute(Format = 0)]
        public string State { get; set; }
        [ExportAttribute(Format = 0)]
        public string PostalCode { get; set; }
        [ExportAttribute(Format = 0)]
        public string Country { get; set; }
        [ExportAttribute(Format = 0)]
        public string Phone { get; set; }
        [ExportAttribute(Format = 0)]
        public string Email { get; set; }
        [ExportAttribute(Format = 0)]
        public int ID { get; set; }
        
    }
}
