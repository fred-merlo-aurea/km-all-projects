using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.MD.Objects
{
    public class FilterVenn
    {
        #region Properties
        public string SelectedFilterNo { get; set; }
        public string SuppressedFilterNo { get; set; }
        public string SelectedFilterOperation { get; set; }
        public string SuppressedFilterOperation { get; set; }
        public string FilterDescription { get; set; }
        public int Count { get; set; }
        #endregion

        public FilterVenn()
        {
            SelectedFilterNo = string.Empty;
            SuppressedFilterNo = string.Empty;
            SelectedFilterOperation = string.Empty;
            SuppressedFilterOperation = string.Empty;
            FilterDescription = string.Empty;
            Count = 0;
        }
    }
}
