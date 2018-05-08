using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Object
{
    public class FilterDetails
    {
        #region Properties
        public int FilterDetailsID { get; set; }
        public int? FilterID { get; set; }
        public string Name { get; set; }
        public string Values { get; set; }
        public string Text { get; set; }
        public string SearchCondition { get; set; }
        public FrameworkUAD.BusinessLogic.Enums.FiltersType FilterType { get; set; }
        public string Group { get; set; }
        public int FilterGroupID { get; set; }
        #endregion
        public FilterDetails()
        {
            Name = string.Empty;
            Values = string.Empty;
            Text = string.Empty;
            SearchCondition = string.Empty;
            FilterType = FrameworkUAD.BusinessLogic.Enums.FiltersType.None;
            Group = string.Empty;
            FilterGroupID = 0;

        }

        public FilterDetails(string name, string values, string text, string searchcondition, FrameworkUAD.BusinessLogic.Enums.FiltersType filtertype, string group)
        {
            Name = name;
            Values = values;
            Text = text;
            SearchCondition = searchcondition;
            FilterType = filtertype;
            Group = group;
        }
    }
}
