using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMEntities;

namespace KMModels
{
    public class ControlTypeModel : ModelBase
    {
        public int ControlType_Seq_ID { get; set; }
        public string Name { get; set; }
        public int MainType_ID { get; set; }

        public string KMPaidQueryString { get; set; }


        public override void FillData(object entity)
        {
            base.FillData(entity);
        }
    }

}
