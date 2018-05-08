using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls
{
    public class ControlCategory : ModelBase
    {
        public ControlCategory() { }

        public int? CategoryID { get; set; }

        public string CategoryName { get; set; }
    }
}
