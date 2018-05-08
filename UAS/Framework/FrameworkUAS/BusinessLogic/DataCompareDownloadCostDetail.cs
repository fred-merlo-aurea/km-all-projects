using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownloadCostDetail
    {
        public List<Entity.DataCompareDownloadCostDetail> Select(int dcDownloadId)
        {
            return DataAccess.DataCompareDownloadCostDetail.SelectDataCompareDownloadId(dcDownloadId).ToList();
        }
        public List<Entity.DataCompareDownloadCostDetail> CreateCostDetail(int dcViewId, int dcTypeCodeID, string profileCount, string profileColumns, string demoColumns, int userId)
        {
            return DataAccess.DataCompareDownloadCostDetail.CreateCostDetail(dcViewId, dcTypeCodeID, profileCount, profileColumns, demoColumns, userId);
        }
    }
}
