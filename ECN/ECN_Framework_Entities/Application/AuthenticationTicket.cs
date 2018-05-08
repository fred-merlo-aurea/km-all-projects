using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Application
{
    public class AuthenticationTicket
    {
        //public int MasterUserID{ get; set; }
        public int UserID{ get; set; }
        public int CustomerID { get; set; }
        public int BaseChannelID  { get; set; }
        public int ClientGroupID { get; set; }
        public int ClientID { get; set; }
        public int ProductID { get; set; }
        public Guid AccessKey { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
