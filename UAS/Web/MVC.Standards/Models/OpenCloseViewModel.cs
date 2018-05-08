using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Standards.Models
{
    public class OpenCloseViewModel
    {
        public int PubID { get; set; }
        public bool AllowDataEntry { get; set; }
        public bool AllowClientImport { get; set; }
        public bool AllowKMmport { get; set; }
        public bool AllowAddRemove { get; set; }
        public OpenCloseViewModel()
        {
            AllowDataEntry = false;
            AllowClientImport = false;
            AllowKMmport = false;
            AllowAddRemove = false;
        }
    }
}