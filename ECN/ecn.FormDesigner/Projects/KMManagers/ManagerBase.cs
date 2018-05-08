using System;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMModels;

namespace KMManagers
{
    public abstract class ManagerBase : APIRunnerBase
    {
        protected DbResolver DB = new DbResolver();

        protected void FillCondition(Condition c, ConditionModel cm)
        {
            c.Control_ID = cm.ControlId;
            c.Operation_ID = (int)cm.ComparisonType;
            if (cm.IsSelectableItem && c.Operation_ID != (int)SelectionComparisonType.Is)
            {
                c.Operation_ID = (int)SelectionComparisonType.IsNot;
            }
            c.Value = cm.Value ?? string.Empty;
        }

        protected void FillCondition_ECN(ECN_Framework_Entities.FormDesigner.Condition c, ConditionModel cm)
        {
            c.Control_ID = cm.ControlId;
            c.Operation_ID = (int)cm.ComparisonType;
            if (cm.IsSelectableItem && c.Operation_ID != (int)SelectionComparisonType.Is)
            {
                c.Operation_ID = (int)SelectionComparisonType.IsNot;
            }
            c.Value = cm.Value ?? string.Empty;
        }
    }
}