using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class History
    {
        public History()
        {
        }
        public UAS.Web.Models.Circulations.BatchHistoryWithName ResultsOpen { get; set; }
        public UAS.Web.Models.Circulations.BatchHistoryWithName ResultsFinalized { get; set; }
        //public List<string> Clients { get; set; }
        //public List<string> Usernames { get; set; }
        //public List<string> Products { get; set; }
        //public string Product { get; set; }
        //public string Username { get; set; }
        //public string Client { get; set; }
        public Dictionary<int, string> Clients { get; set; }
        public Dictionary<int, string> Users { get; set; }
        public Dictionary<int, string> Products { get; set; }
        public KeyValuePair<int, string> Product { get; set; }
        public KeyValuePair<int, string> User { get; set; }
        public KeyValuePair<int, string> Client { get; set; }

        //properties used for searches via UI
        public DateTime StartDate { get; set; } //  $('#StartDate').val(),
        public DateTime EndDate { get; set; } // $('#EndDate').val(),
        public string BatchNumber { get; set; } //$('#BatchNumber').val()
        public int UserId { get; set; } // $('#ddlUserName').val(),
        public string ProductCode { get; set; }
    }
}