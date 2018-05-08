using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Models
{
    public class User
    {
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public string UserName { get; set; }
        public Guid AccessKey { get; set; }
    }
}