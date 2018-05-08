using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class Action
    {
        public bool Exists(int actionTypeID, int categoryCodeID, int transactionCodeID)
        {
            List<Entity.Action> all = Select().ToList();
            if (all.Exists(x => x.ActionTypeID == actionTypeID && x.CategoryCodeID == categoryCodeID && x.TransactionCodeID == transactionCodeID))
                return true;
            else
                return false;
            //return DataAccess.Action.Exists(actionTypeID,categoryCodeID,transactionCodeID);
        }

        public Entity.Action GetByActionID(int actionID, bool getChildren = false)
        {
            Entity.Action action = null;
            List<Entity.Action> all = Select().ToList();
            if(all.Exists(x => x.ActionID == actionID))
                action = all.SingleOrDefault(x => x.ActionID == actionID);
            if (action != null && getChildren)
            {
                //campaign.CampaignItems = CampaignItem.GetByCampaignID(campaignID, customerID, getChildren);
            }
            return action;
        }
        public Entity.Action Select(int categoryCodeID, int transactionCodeID)
        {
            Entity.Action item = null;
            item = DataAccess.Action.Select(categoryCodeID, transactionCodeID);
            return item;
        }

        public Entity.Action Select(int actionID)
        {
            Entity.Action item = null;
            item = DataAccess.Action.Select(actionID);
            return item;
        }
        public List<Entity.Action> Select()
        {
            List<Entity.Action> retList = null;
            retList = DataAccess.Action.Select();
            return retList;
        }

        public bool Validate(Entity.Action action)
        {
            List<string> errorList = new List<string>();
            if (action.ActionTypeID <= 0)
                errorList.Add("ActionTypeID is invalid");

            if (action.CategoryCodeID <= 0)
                errorList.Add("CategoryCodeID is invalid");

            if (action.TransactionCodeID <= 0)
                errorList.Add("TransactionCodeID is invalid");

            if (errorList.Count > 0)
            {
                return false;
            }
            return true;
        }
        public int Save(Entity.Action action)
        {
            Validate(action);
            using (TransactionScope scope = new TransactionScope())
            {
                action.ActionID = DataAccess.Action.Save(action);
                scope.Complete();
            }
            return action.ActionID;
        }
    }
}
