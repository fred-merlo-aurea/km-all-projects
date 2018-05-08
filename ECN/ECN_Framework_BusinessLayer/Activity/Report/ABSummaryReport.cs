using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class ABSummaryReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.ABSummaryReport> Get(int customerID, DateTime startdate, DateTime enddate)
        {
            List<ECN_Framework_Entities.Activity.Report.ABSummaryReport> reportList = ECN_Framework_DataLayer.Activity.Report.ABSummaryReport.Get(customerID, startdate, enddate);
            List<int> sampleids = new List<int>();

            // Get all sampleids for batched processing
            foreach (ECN_Framework_Entities.Activity.Report.ABSummaryReport report in reportList)
            {
                if (!sampleids.Contains(report.SampleID))
                    sampleids.Add(report.SampleID);
            }
            // Determine single winner in each sampleid batch
            foreach (int id in sampleids)
            {
                List<ECN_Framework_Entities.Activity.Report.ABSummaryReport> AB = new List<ECN_Framework_Entities.Activity.Report.ABSummaryReport>();
                AB = (from rpt in reportList 
                      where rpt.SampleID == id
                      where rpt.CampaignItemType.ToLower() == "ab"
                      select rpt).ToList();
                ECN_Framework_Entities.Activity.Report.ABSummaryReport champion = (from rpt in reportList
                                                                                   where rpt.SampleID == id
                                                                                   where rpt.CampaignItemType.ToLower() == "champion"
                                                                                   select rpt).FirstOrDefault();
                if (champion != null) // If champion already selected, 
                    (from rpt in reportList
                     where rpt.SampleID == champion.SampleID
                     where rpt.EmailSubject == champion.EmailSubject
                     where rpt.LayoutName == champion.LayoutName
                     select rpt).First().Winner = true; // set winner to the blast that generated the champion
                else if(AB != null && AB.Count() > 0)
                    DetermineWinner(AB).Winner = true;
            }

            foreach(ECN_Framework_Entities.Activity.Report.ABSummaryReport ab in reportList)
            {
                ab.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(ab.EmailSubject);
            }
            return reportList;
        }

        private static ECN_Framework_Entities.Activity.Report.ABSummaryReport DetermineWinner(List<ECN_Framework_Entities.Activity.Report.ABSummaryReport> AB)
        {
            // assert AB.Count() >= 1
            // assert all WinTypes are the same
            //assert win type is clicks or opens
            ECN_Framework_Entities.Activity.Report.ABSummaryReport defaultWinner = AB[0]; //Blast A
            foreach (ECN_Framework_Entities.Activity.Report.ABSummaryReport rpt in AB) // Blast A is the first one scheduled, so its id is the lowest
                if (rpt.BlastID < defaultWinner.BlastID)
                    defaultWinner = rpt;

            string winType = AB[0].ABWinnerType; // Current Win Criterion
            string oWinType = winType; // Original Win Criterion

            List<double> percents = new List<double>(); // List of scores 
            while(winType != "tie")
            {
                foreach(ECN_Framework_Entities.Activity.Report.ABSummaryReport rpt in AB)
                    percents.Add(CalculateCriterion(winType, rpt));
                double max = percents.Max();
                // Never should be zero
                IEnumerable<double> maxes = (from num in percents where num == max select num);
                if (maxes.Count() == 1) // If no tie
                    return (from rpt in AB where CalculateCriterion(winType, rpt) == max select rpt).First();
                else if(maxes.Count() > 1) // If tie
                {
                    // Remove reports that weren't tied for first
                    for(int i = AB.Count()-1; i >= 0; i--)
                    {
                        if(CalculateCriterion(winType, AB[i]) != max)
                            AB.RemoveAt(i);
                    }
                    // Use next decision criterion
                    if (winType == "clicks" && oWinType == "clicks")
                        winType = "opens";
                    else if (winType == "clicks" && oWinType == "opens")
                        winType = "topens";
                    else if (winType == "opens" && oWinType == "opens")
                        winType = "clicks";
                    else if (winType == "opens" && oWinType == "clicks")
                        winType = "tclicks";
                    else if (winType == "tclicks" && oWinType == "clicks")
                        winType = "topens";
                    else if (winType == "tclicks" && oWinType == "opens")
                        winType = "tie";
                    else if (winType == "topens" && oWinType == "opens")
                        winType = "tclicks";
                    else if (winType == "topens" && oWinType == "clicks")
                        winType = "tie";
                }
                percents.Clear();
            }
            return defaultWinner;
        }
        private static double CalculateCriterion(string winType, ECN_Framework_Entities.Activity.Report.ABSummaryReport rpt)
        {
            switch (winType.ToLower())
            {
                case "clicks":
                    return rpt.Delivered == 0 ? 0 : ((double)rpt.Clicks / (double)rpt.Delivered);
                case "opens":
                    return rpt.Delivered == 0 ? 0 : ((double)rpt.Opens / (double)rpt.Delivered);
                case "tclicks":
                    return (double)rpt.Clicks;
                case "topens":
                    return (double)rpt.Opens;
                default:
                    return 0;
            }
        }
    }
}