using System.Collections.Generic;
using System.Linq;
using FrameworkUAD_Lookup;

namespace UAS.Web.Controllers.Dashboard
{
    public class ProcessingLimits
    {
        public readonly int Limit015K;
        public readonly int Limit1550K;
        public readonly int Limit50100K;
        public readonly int Limit100Max;

        public ProcessingLimits(IReadOnlyCollection<FrameworkUAD_Lookup.Entity.Code> codes)
        {
            Limit015K = GetLimit(codes, Enums.RecordProcessTimeType.RecordProcessTime_0_15K);
            Limit1550K = GetLimit(codes, Enums.RecordProcessTimeType.RecordProcessTime_15_50K);
            Limit50100K = GetLimit(codes, Enums.RecordProcessTimeType.RecordProcessTime_50_100K);
            Limit100Max = GetLimit(codes, Enums.RecordProcessTimeType.RecordProcessTime_100_Max);
        }

        private static int GetLimit(IReadOnlyCollection<FrameworkUAD_Lookup.Entity.Code> codes, Enums.RecordProcessTimeType timeType)
        {
            var time = timeType.ToString().Replace(DashboardController.DelimiterUnderscore, DashboardController.DelimiterSpace);
            var codeValue = codes.FirstOrDefault(x => x.CodeName.Equals(time))?.CodeValue;

            int result;
            int.TryParse(codeValue, out result);
            return result;
        }
    }
}