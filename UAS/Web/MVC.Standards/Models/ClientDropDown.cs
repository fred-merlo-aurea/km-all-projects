using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Standards.Models
{
    public class ClientDropDown
    {
        public ClientDropDown()
        {
            Clients = new List<KMPlatform.Entity.Client>();
            ClientGroups = new List<KMPlatform.Entity.ClientGroup>();
            SelectedClientGroupID = -1;
            SelectedClientID = -1;
            CurrentClientGroupID = -1;
            CurrentClientID = -1;
            ClientGroupItems = null;
            ClientItems = null;
        }
        public List<KMPlatform.Entity.Client> Clients { get; set; }

        public List<KMPlatform.Entity.ClientGroup> ClientGroups { get; set; }

        public int ClientGroupID { get; set; }

        public int ClientID { get; set; }

        public int SelectedClientID { get; set; }

        public int SelectedClientGroupID { get; set; }

        public int CurrentClientID { get; set; }

        public int CurrentClientGroupID { get; set; }

        public SelectList ClientGroupItems
        {
            get;
            set;
        }

        public SelectList ClientItems
        {
            get;
            set;

        }

    }
}
