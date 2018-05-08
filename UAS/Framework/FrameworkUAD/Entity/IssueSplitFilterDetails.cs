using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Entity
{
    public class IssueSplitFilterDetails
    {
        public int FilterDetailID { get; set; }
        public int FilterID { get; set; }
        public string Name { get; set; }
        public string Values { get; set; }
        public string Text { get; set; }
        public string SearchCondition { get; set; }
        public string Group { get; set; }
    }
}
