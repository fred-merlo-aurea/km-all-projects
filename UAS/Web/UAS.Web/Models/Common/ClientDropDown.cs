using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Common
{
    public class ClientDropDown
    {
        public ClientDropDown()
        {
            Clients = new List<KMPlatform.Entity.Client>();
            ClientGroups = new List<KMPlatform.Entity.ClientGroup>();
            Products = new List<KMPlatform.Object.Product>();
            SelectedClientGroupID = -1;
            SelectedClientID = -1;
            SelectedProductID = -1;
            CurrentClientGroupID = -1;
            CurrentClientID = -1;
            CurrentProductID = -1;
            ClientGroupItems = null;
            ClientItems = null;
            ProductItems = null;
        }
        public List<KMPlatform.Entity.Client> Clients { get; set; }

        public List<KMPlatform.Entity.ClientGroup> ClientGroups { get; set; }

        public List<KMPlatform.Object.Product> Products { get; set; }

        public int ClientGroupID { get; set; }

        public int ClientID { get; set; }

        public int ProductID { get; set; }

        public int SelectedClientID { get; set; }

        public int SelectedClientGroupID { get; set; }

        public int SelectedProductID { get; set; }

        public int CurrentProductID { get; set; }

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

        public SelectList ProductItems
        {
            get;
            set;

        }

    }
}
