using System;
using System.Collections.Generic;
using KMEntities;
using KMEnums;

namespace KMModels.PostModels
{
    public class ConditionGroupModel : ModelBase
    {
        [GetFromField("ConditionGroup_Seq_ID")]
        public int Id { get; set; }
        public ConditionType LogicGroup { get; set; }
        public IEnumerable<ConditionGroupModel> ConditionGroups { get; set; }
        public IEnumerable<ConditionModel> Conditions { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);
            ConditionGroup gr = (ConditionGroup)entity;            
        
            ConditionGroups = gr.ConditionGroup1.ConvertToModels<ConditionGroupModel>();
            Conditions = gr.Conditions.ConvertToModels<ConditionModel>();
            foreach (ConditionModel c in Conditions) 
            {                
                c.Type = LogicGroup;
            }
            
        }
    }
}