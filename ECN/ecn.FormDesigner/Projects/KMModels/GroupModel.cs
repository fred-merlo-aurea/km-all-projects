using KMModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels
{
    public class GroupModel
    {
        public GroupModel()
        {
            Category = new ControlCategory();
            UDFs = new List<Udf>();
        }

        public int CustomerID { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public ControlCategory Category { get; set; }

        public int Order { get; set; }

        public bool Default { get; set; }

        public string LabelHTML { get; set; }

        public List<Udf> UDFs { get; set; }

        internal void Initialize(string groupId, string customerId)
        {
            GroupID = int.Parse(groupId);
            CustomerID = int.Parse(customerId);
        }
    }
}
