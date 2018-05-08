using System;
using System.Data;
using System.Linq;

namespace ecn.communicator.admin.deliverability
{
    public class DeliverabilityQueryResults
    {
        public double TotalSent { get; private set; }
        public double SoftBounces { get; private set; }
        public double SBPerc { get; private set; }
        public double HardBounces { get; private set; }
        public double HBPerc { get; private set; }
        public double MailBlock { get; private set; }
        public double MBPerc { get; private set; }
        public double Complaint { get; private set; }
        public double ComPerc { get; private set; }
        public double OptOut { get; private set; }
        public double OptOPerc { get; private set; }
        public double MasterSupp { get; private set; }
        public double MSPerc { get; private set; }

        public void CalculateDeliverability(IGrouping<object, DataRow> queryGroup)
        {
            TotalSent = CalculateSum(queryGroup, "TotalSent");
            SoftBounces = CalculateSum(queryGroup, "SoftBounces");
            SBPerc = CalculatePrecentage(queryGroup, "SoftBounces");
            HardBounces = CalculateSum(queryGroup, "HardBounces");
            HBPerc = CalculatePrecentage(queryGroup, "HardBounces");
            MailBlock = CalculateSum(queryGroup, "MailBlock");
            MBPerc = CalculatePrecentage(queryGroup, "MailBlock");
            Complaint = CalculateSum(queryGroup, "Complaint");
            ComPerc = CalculatePrecentage(queryGroup, "Complaint");
            OptOut = CalculateSum(queryGroup, "OptOut");
            OptOPerc = CalculatePrecentage(queryGroup, "OptOut");
            MasterSupp = CalculateSum(queryGroup, "MasterSupp");
            MSPerc = CalculatePrecentage(queryGroup, "MasterSupp");
        }

        private int CalculateSum(IGrouping<object, DataRow> queryGroup, string rowName)
        {
            if (queryGroup == null)
            {
                throw new ArgumentNullException(nameof(queryGroup));
            }

            return queryGroup.Sum(x => x.Field<int>(rowName));
        }

        private double CalculatePrecentage(IGrouping<object, DataRow> group, string rowName)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return Math.Round(
                (group.Sum(x => Convert.ToDouble(x.Field<int>(rowName))) * 100) /
                    group.Sum(x => Convert.ToDouble(x.Field<int>("TotalSent"))),
                2);
        }
    }
}
