using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class BlastActivityOpensReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastActivityOpensReport> Get(List<ECN_Framework_Entities.Activity.BlastActivityOpens> openslist, List<ECN_Framework_Entities.Activity.EmailClients> eclist, List<ECN_Framework_Entities.Activity.Platforms> plist)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastActivityOpensReport> borpt = new List<ECN_Framework_Entities.Activity.Report.BlastActivityOpensReport>();
            ECN_Framework_Entities.Activity.Report.BlastActivityOpensReport bo_object;
            var querytotalcount = (from op in openslist
                                   where op.PlatformID != 5 && op.EmailClientID != 15
                                   select new { TotalCount = op.EmailID });

            var query = (from o in openslist
                         join ec in eclist on o.EmailClientID equals ec.EmailClientID
                         join pc in plist on o.PlatformID equals pc.PlatformID
                         where o.PlatformID != 5 && o.EmailClientID != 15
                         group o by new { ec.EmailClientName, pc.PlatformName } into gp
                         orderby gp.Count() descending
                         select new
                         {
                             EmailClientName = gp.Key.EmailClientName,
                             Opens = (gp.Count()),
                             Usage = (Math.Round(((float)gp.Count() * 100 / querytotalcount.Count()), 2)).ToString() + "%",
                             PlatformName = gp.Key.PlatformName
                         }).Distinct().ToList();

            foreach (var BlastOpens in query)
            {
                bo_object = new ECN_Framework_Entities.Activity.Report.BlastActivityOpensReport();
                bo_object.EmailClientName = BlastOpens.EmailClientName;
                bo_object.PlatformName = BlastOpens.PlatformName;
                bo_object.Usage = BlastOpens.Usage;
                bo_object.Opens = BlastOpens.Opens;
                borpt.Add(bo_object);
            }
            return borpt;
        }
    }
}
