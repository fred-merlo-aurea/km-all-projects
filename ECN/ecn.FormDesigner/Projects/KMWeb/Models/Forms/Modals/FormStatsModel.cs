using KMEnums;
using KMManagers;
using KMManagers.APITypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.Models.Forms.Modals
{
    public class FormStatsModel 
    {
        public string FormName;
        public int Total;
        public int UnknownVisitors;
        public int KnownVisitors;
    }
}
