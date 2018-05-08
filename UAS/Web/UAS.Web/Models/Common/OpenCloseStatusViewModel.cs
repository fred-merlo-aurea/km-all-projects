using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Common
{
    public class OpenCloseStatusViewModel
    {
        public int PubID { get; set; }
        public string PubName { get; set; }
        public bool AllowDataEntry { get; set; }
        public bool AllowClientImport { get; set; }
        public bool AllowKMmport { get; set; }
        public bool AllowAddRemove { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasFullAccess { get; set; }
        public List<MenuActions> MenuList { get; set; }
        public OpenCloseStatusViewModel()
        {
            AllowDataEntry = false; 
            AllowClientImport = false;
            AllowKMmport = false;
            AllowAddRemove = false;
            PubName = "";
            PubID = 0;
            ErrorMessage = "";
            MenuList = new List<MenuActions>();
            HasFullAccess = false;
        }
    }

    public class MenuActions
    {
        public int  ActionId { get; set; }
        public string ActionName { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
    }
}