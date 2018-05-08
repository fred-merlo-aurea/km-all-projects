using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.menu.Models
{
    public class PostMenu
    {
        

        public string Application { get; set; }

        public string AccountChange { get; set; }

        public bool DataEntryLock { get; set; }

        public List<LockMenu> Locks { get; set; }

        public MenuModel Menu { get; set; }

        public ClientDropDown cdd { get; set; }

        
    }

    public class LockMenu
    {
        public string Name { get; set; }

        public string RedirectURL { get; set; }

        public bool Locked { get; set; }
    }
}