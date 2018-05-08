using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

using KMEnums;
using KMEntities;

namespace KMModels.PostModels
{
    public class FormRulesPostModel : PostModelBase
    {
        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        public IEnumerable<RuleModel> Rules { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);
        }
    }

}
