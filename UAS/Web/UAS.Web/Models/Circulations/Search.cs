using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class Search
    {   
        
        public string Product { get; set; }
        public int? SequenceNumber { get; set; }
        public string AccountNumber { get; set; }
        public string OldAccountNumber { get; set; }
        public int? PublicationID { get; set; }
        public int? SubscriptionID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserTitle { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NewSubscription { get; set; }
        public Search()
        {
            
        }


    }
}