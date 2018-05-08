using System.Collections.Generic;
using ECN_Framework_Entities.Activity.Report;
using ecn.communicator.main.blasts.Interfaces;

namespace ecn.communicator.main.blasts.Helpers
{
    public class BlastReportAdapter : IBlastReport
    {
        public IList<BlastReport> Get(int blastID)
        {
            return ECN_Framework_BusinessLayer.Activity.Report.BlastReport.Get(blastID);
        }
    }
}
