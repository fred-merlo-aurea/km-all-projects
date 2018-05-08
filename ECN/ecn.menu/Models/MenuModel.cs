using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;

using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;

namespace ecn.menu.Models
{
    [Serializable]
    
    public partial class MenuModel
    {
        public MenuModel()
        {
            MenuID = -1;
            MenuName = "";
            URL = "";
            ParentMenuID = -1;
            Children = new List<MenuModel>();
            Customers = new List<ECN_Framework_Entities.Accounts.Customer>();
            IsSelected = false;
            
        }

        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public int ParentMenuID { get; set; }
        public List<MenuModel> Children { get; set; }
        public List<ECN_Framework_Entities.Accounts.Customer> Customers { get; set; }

        public bool IsSelected { get; set; }

        
    }
}