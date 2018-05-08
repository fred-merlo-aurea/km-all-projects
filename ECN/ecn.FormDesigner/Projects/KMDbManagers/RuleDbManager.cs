using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using KMEntities;
using KMEnums;

namespace KMDbManagers
{
    public class RuleDbManager : DbManagerBase
    {
        public Rule GetByID(int id)
        {
            return KM.Rules.SingleOrDefault(x => x.Rule_Seq_ID == id);
        }

        public void AddRule(Rule r, int? formId, int? controlId, int groupId, Dictionary<int,int> Controls = null)
        {
            Rule newR = new Rule();
            newR.Form_Seq_ID = formId;
            newR.Control_ID = controlId;
            newR.ConditionGroup_Seq_ID = groupId;
            newR.Type = r.Type;
            newR.Action = r.Action;
            newR.ActionJs = r.ActionJs;
            newR.UrlToRedirect = r.UrlToRedirect;
            newR.Order = r.Order;
            newR.SuspendpostDB = r.SuspendpostDB;
            newR.NonQualify = r.NonQualify;
            newR.Overwritedatapost =r.Overwritedatapost;
            
            newR.ResultType = r.ResultType;
            if (r.Type == 3 && Controls != null)
            {
                newR.OverwritedataPostValues = AddOverWriteDataPostValues(r.OverwritedataPostValues, Controls);
                newR.RequestQueryValues = AddRequestQueryValues(r.RequestQueryValues, Controls);// AddRequestQueryValues(r.RequestQueryValues);
            }
            Add(newR);
            SaveChanges();
        }

        public void Add(Rule newR)
        {
            KM.Rules.Add(newR);
          
        }

        private ICollection<OverwritedataPostValue> AddOverWriteDataPostValues(ICollection<OverwritedataPostValue> odpv, Dictionary<int, int> Controls)
        {
            List<OverwritedataPostValue> newColl = new List<OverwritedataPostValue>();
            

            foreach(OverwritedataPostValue oCurrent in odpv)
            {
                OverwritedataPostValue oNew = new OverwritedataPostValue();
                oNew.Control_ID = Controls[oCurrent.Control_ID];
                oNew.Value = oCurrent.Value;
                newColl.Add(oNew);
            }


            return newColl;
        }

        private ICollection<RequestQueryValue> AddRequestQueryValues(ICollection<RequestQueryValue> odpv, Dictionary<int, int> Controls)
        {
            List<RequestQueryValue> newColl = new List<RequestQueryValue>();


            foreach (RequestQueryValue rCurrent in odpv)
            {
                RequestQueryValue oNew = new RequestQueryValue();
                oNew.Control_ID = Controls[rCurrent.Control_ID];
                oNew.Name = rCurrent.Name;
                
                newColl.Add(oNew);
            }


            return newColl;
        }

        public void Remove(Rule r)
        {
            KM.Rules.Remove(KM.Rules.Single(x => x.Rule_Seq_ID == r.Rule_Seq_ID));
        }

        public void Remove(IEnumerable<Rule> rules)
        {
            foreach (var r in rules)
            {
                KM.Rules.Remove(KM.Rules.Single(x => x.Rule_Seq_ID == r.Rule_Seq_ID));
            }
            
        }
       
        public IEnumerable<Rule> GetAllByFormOrControls(int formId, IEnumerable<int> controlIDs)
        {
            return KM.Rules.Where(x => (x.Form_Seq_ID.HasValue && x.Form_Seq_ID == formId) 
                                    || (x.Control_ID.HasValue && controlIDs.Contains(x.Control_ID.Value))).OrderBy(x => x.Order).ToList();
        }

        public IEnumerable<Rule> GetAllByFormOrControlsAndType(int formId, IEnumerable<int> controlIDs, RuleTypes type)
        {
            return KM.Rules.Where(x => (x.Form_Seq_ID.HasValue && x.Form_Seq_ID == formId)
                                    || (x.Control_ID.HasValue && controlIDs.Contains(x.Control_ID.Value))).Where(x => x.Type == (int)type).ToList();
        }

        public IEnumerable<Rule> GetByCondGroupIDs(IEnumerable<int> IDs)
        {
            return KM.Rules.Where(x => IDs.Contains(x.ConditionGroup_Seq_ID));
        }
    }
}