using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//Custom KM Namespaces
using UAS.Web.Models.Circulations;


namespace UAS.Web.Models.Dashboard
{
    public class DashboardModel
    {
        public bool isCirc { get; set; }
        public bool isUAD { get; set; }
        public List<FrameworkUAS.Entity.EngineLog> EngineLogs { get; set; }
        public List<Models.Circulations.FileStatus> CircList { get; set; }
        public List<Models.Circulations.FileStatus> UadList { get; set; }
        public List<Models.Circulations.FileStatus> ApiList { get; set; }
        public List<KMPlatform.Object.Product> Products { get; set; }
    }
}