using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using KM.Common;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMModels;
using KMModels.PostModels;
using BusinessForm = ECN_Framework_BusinessLayer.FormDesigner;
using BusinessRule = ECN_Framework_BusinessLayer.FormDesigner.Rule;
using EntityForm = ECN_Framework_Entities.FormDesigner;
using EntityRule = ECN_Framework_Entities.FormDesigner.Rule;
using KMEntityUser = KMPlatform.Entity.User;

namespace KMManagers
{
    public class RuleManager : ManagerBase
    {
        public const string Show = "show";
        public const string Hide = "hide";
        private const int TransactionTimeout = 5;
        private const int OverrideDataPostOne = 1;
        private const int OverwriteDataValueSeqIdNone = -1;
        private const int ConditionGroup0 = 0;
        private const int ConditionGroup1 = 1;

        private RuleDbManager RM
        {
            get
            {
                return DB.RuleDbManager;
            }
        }
        private FormManager fm = new FormManager();
        private OverwriteDataPostManager ODPM = new OverwriteDataPostManager();
        private RequestQueryUrlManager RQM = new RequestQueryUrlManager();
        public IEnumerable<TModel> GetAllByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return GetAllByFormID(formId).ConvertToModels<TModel>();
        }

        internal IEnumerable<Rule> GetAllByFormID(int formId)
        {
            IEnumerable<int> controlIDs = DB.ControlDbManager.GetAllByFormID(formId).Select(x => x.Control_ID);
            return RM.GetAllByFormOrControls(formId, controlIDs);
        }

        //public IEnumerable<TModel> GetAllByFormIDAndType<TModel>(int formId, RuleTypes type) where TModel : ModelBase, new()
        //{
        //    IEnumerable<int> controlIDs = DB.ControlDbManager.GetAllByFormID(formId).Select(x => x.Control_ID);

        //    return RM.GetAllByFormOrControlsAndType(formId, controlIDs, type).ConvertToModels<TModel>();
        //}

        public void Save(KMEntityUser user, int channelId, FormRulesPostModel model)
        {
            Guard.NotNull(user, nameof(user));
            Guard.NotNull(model, nameof(model));

            var form = fm.GetByID(channelId, model.Id);
            form.UpdatedBy = user.UserName;
            form.LastUpdated = DateTime.Now;
            var list = new Dictionary<EntityRule, object[]>();
            if (model.Rules != null)
            {
                foreach (var ruleModel in model.Rules)
                {
                    InitializeRule(ruleModel, model.Id, list);
                }
            }

            var rules = BusinessRule.GetByFormID(model.Id, true);
            bool success;
            using (var transactionScope = new TransactionScope(
                                TransactionScopeOption.RequiresNew,
                                new TransactionOptions
                                {
                                    IsolationLevel = IsolationLevel.ReadUncommitted,
                                    Timeout = TimeSpan.FromMinutes(TransactionTimeout)
                                }))
            {
                var toRemove = rules.Where(x => !list.Keys.Select(y => y.Rule_Seq_ID).Contains(x.Rule_Seq_ID)).ToList();
                if (toRemove.Count > 0)
                {
                    SaveDeleted(toRemove);
                }

                var existing = rules.Where(x => list.Keys.Select(y => y.Rule_Seq_ID).Contains(x.Rule_Seq_ID)).ToList();
                if (existing.Count > 0)
                {
                    SaveExistings(existing, list);
                }

                if (list.Count > 0)
                {
                    SaveAdded(list);
                }
                success = true;
                transactionScope.Complete();
            }

            if (success)
            {
                fm.SaveChanges();
            }
        }

        private static void SaveDeleted(List<EntityRule> toRemove)
        {
            Guard.NotNull(toRemove, nameof(toRemove));

            foreach (var rule in toRemove)
            {
                var conditions = BusinessForm.Condition.GetByCondGroupID(rule.ConditionGroup_Seq_ID);
                foreach (var condition in conditions)
                {
                    BusinessForm.Condition.Delete(condition.Condition_Seq_ID);
                }

                BusinessForm.OverwriteDataPost.DeleteByRuleID(rule.Rule_Seq_ID);
                BusinessForm.RequestQueryValue.DeleteByRuleID(rule.Rule_Seq_ID);
                BusinessRule.Delete(rule.Rule_Seq_ID);
                BusinessForm.ConditionGroup.Delete(rule.ConditionGroup_Seq_ID);
            }
        }

        private static void SaveExistings(IEnumerable<EntityRule> existing, Dictionary<EntityRule, object[]> list)
        {
            Guard.NotNull(existing, nameof(existing));

            foreach (var rule in existing)
            {
                var edited = list.Keys.Single(x => x.Rule_Seq_ID == rule.Rule_Seq_ID);
                var conditionGroup = rule.ConditionGroup;
                conditionGroup.LogicGroup = (bool) list[edited][ConditionGroup0];
                var condGroupId = BusinessForm.ConditionGroup.Save(conditionGroup);

                var forDelete = BusinessForm.ConditionGroup.GetByMainGroupID(condGroupId);
                foreach (var cgDelete in forDelete)
                {
                    BusinessForm.ConditionGroup.Delete(cgDelete.ConditionGroup_Seq_ID);
                }

                var conditions = BusinessForm.Condition.GetByCondGroupID(condGroupId);
                foreach (var condition in conditions)
                {
                    BusinessForm.Condition.Delete(condition.Condition_Seq_ID);
                }

                foreach (var cGroup in (IEnumerable<EntityForm.ConditionGroup>) list[edited][ConditionGroup1])
                {
                    if (cGroup.ConditionGroup_Seq_ID != condGroupId)
                    {
                        var conditionGroupNew = new EntityForm.ConditionGroup
                        {
                            MainGroup_ID = condGroupId,
                            LogicGroup = cGroup.LogicGroup
                        };
                        var newGroupId = BusinessForm.ConditionGroup.Save(conditionGroupNew);

                        foreach (var condition in cGroup.Conditions)
                        {
                            var cNew = new EntityForm.Condition
                            {
                                ConditionGroup_Seq_ID = newGroupId,
                                Control_ID = condition.Control_ID,
                                Operation_ID = condition.Operation_ID,
                                Value = condition.Value
                            };

                            BusinessForm.Condition.Save(cNew);
                        }
                    }
                }

                if (edited.Type == (int)RuleTypes.Form)
                {
                    SaveExistingForm(edited);
                }

                BusinessRule.Save(edited);
                list.Remove(edited);
            }
        }

        private static void SaveExistingForm(EntityRule edited)
        {
            Guard.NotNull(edited, nameof(edited));

            if (edited.Overwritedatapost == OverrideDataPostOne &&
                edited.OverwritePostValue != null &&
                edited.OverwritePostValue.Any())
            {
                BusinessForm.OverwriteDataPost.DeleteByRuleID(edited.Rule_Seq_ID);

                foreach (var overwriteDataPost in edited.OverwritePostValue)
                {
                    overwriteDataPost.OverwritedataValue_Seq_ID = -1;
                    BusinessForm.OverwriteDataPost.Save(overwriteDataPost);
                }
            }
            else
            {
                BusinessForm.OverwriteDataPost.DeleteByRuleID(edited.Rule_Seq_ID);
            }

            if (edited.ResultType == (int) ResultType.URL ||
                edited.ResultType == (int) ResultType.KMPaidPage &&
                edited.RequestQueryValue != null &&
                edited.RequestQueryValue.Any())
            {
                BusinessForm.RequestQueryValue.DeleteByRuleID(edited.Rule_Seq_ID);

                foreach (var requestQueryValue in edited.RequestQueryValue)
                {
                    requestQueryValue.RequestQueryValue_Seq_ID = -1;
                    BusinessForm.RequestQueryValue.Save(requestQueryValue);
                }
            }
            else
            {
                BusinessForm.RequestQueryValue.DeleteByRuleID(edited.Rule_Seq_ID);
            }
        }

        private static void SaveAdded(IReadOnlyDictionary<EntityRule, object[]> list)
        {
            Guard.NotNull(list, nameof(list));

            foreach (var item in list)
            {
                var conditionGroupNew = new EntityForm.ConditionGroup {LogicGroup = (bool)list[item.Key][ConditionGroup0] };
                var groupId = BusinessForm.ConditionGroup.Save(conditionGroupNew);

                item.Key.ConditionGroup_Seq_ID = groupId;
                var newRuleId = BusinessRule.Save(item.Key);

                foreach (var conditionGroup in (IEnumerable<EntityForm.ConditionGroup>)list[item.Key][ConditionGroup1])
                {
                    if (conditionGroup.ConditionGroup_Seq_ID != groupId)
                    {
                        conditionGroupNew.MainGroup_ID = groupId;
                        conditionGroupNew.LogicGroup = conditionGroup.LogicGroup;
                        var newGroupId = BusinessForm.ConditionGroup.Save(conditionGroupNew);

                        foreach (var condition in conditionGroup.Conditions)
                        {
                            var conditionNew = new EntityForm.Condition
                            {
                                ConditionGroup_Seq_ID = newGroupId,
                                Control_ID = condition.Control_ID,
                                Operation_ID = condition.Operation_ID,
                                Value = condition.Value
                            };

                            BusinessForm.Condition.Save(conditionNew);
                        }
                    }
                }

                if (item.Key.Type == (int)RuleTypes.Form)
                {
                    if ((item.Key.Overwritedatapost == OverrideDataPostOne) && item.Key.OverwritePostValue != null)
                    {
                        foreach (var overwriteDataPost in item.Key.OverwritePostValue)
                        {
                            overwriteDataPost.OverwritedataValue_Seq_ID = OverwriteDataValueSeqIdNone;
                            overwriteDataPost.Rule_Seq_ID = newRuleId;
                            BusinessForm.OverwriteDataPost.Save(overwriteDataPost);
                        }
                    }

                    if (item.Key.UrlToRedirect != null && (item.Key.RequestQueryValue != null))
                    {
                        foreach (var requestQueryValue in item.Key.RequestQueryValue)
                        {
                            requestQueryValue.RequestQueryValue_Seq_ID = -1;
                            requestQueryValue.Rule_Seq_ID = newRuleId;
                            BusinessForm.RequestQueryValue.Save(requestQueryValue);
                        }
                    }
                }
            }
        }


        private void InitializeRule(RuleModel model, int formId, Dictionary<ECN_Framework_Entities.FormDesigner.Rule, object[]> list)
        {
            ECN_Framework_Entities.FormDesigner.Rule r = null;
            if (model.Id == 0)
            {
                r = new ECN_Framework_Entities.FormDesigner.Rule();
                r.Type = (int)model.Type;

            }
            else
            {
                r = ECN_Framework_BusinessLayer.FormDesigner.Rule.GetByRuleID(model.Id, true);
            }
            FillRule(r, model, formId);
            list.Add(r, new object[] { model.MainConditionGroup.LogicGroup == ConditionType.And, GetDbGroups_ECN(model) });

        }

        private IEnumerable<ConditionGroup> GetDbGroups(RuleModel model)
        {
            List<ConditionGroup> res = new List<ConditionGroup>();
            foreach (var grm in model.ConditionGroup)
            {
                res.Add(GetDbGroup(grm));
            }

            return res;
        }

        private IEnumerable<ECN_Framework_Entities.FormDesigner.ConditionGroup> GetDbGroups_ECN(RuleModel model)
        {
            List<ECN_Framework_Entities.FormDesigner.ConditionGroup> res = new List<ECN_Framework_Entities.FormDesigner.ConditionGroup>();
            foreach (var grm in model.ConditionGroup)
            {
                res.Add(GetDbGroup_ECN(grm));
            }

            return res;
        }

        private ConditionGroup GetDbGroup(ConditionGroupModel model)
        {
            ConditionGroup gr = new ConditionGroup();
            gr.LogicGroup = model.LogicGroup == ConditionType.And;
            gr.ConditionGroup1 = new List<ConditionGroup>();
            if (model.ConditionGroups != null)
            {
                foreach (var grm in model.ConditionGroups)
                {
                    gr.ConditionGroup1.Add(GetDbGroup(grm));
                }
            }
            if (model.Conditions != null)
            {
                foreach (var cm in model.Conditions)
                {
                    Condition c = new Condition();
                    FillCondition(c, cm);
                    gr.Conditions.Add(c);
                }
            }
            return gr;
        }

        private ECN_Framework_Entities.FormDesigner.ConditionGroup GetDbGroup_ECN(ConditionGroupModel model)
        {
            ECN_Framework_Entities.FormDesigner.ConditionGroup gr = new ECN_Framework_Entities.FormDesigner.ConditionGroup();
            gr.LogicGroup = model.LogicGroup == ConditionType.And;
            gr.ConditionGroup1 = new List<ECN_Framework_Entities.FormDesigner.ConditionGroup>();
            if (model.ConditionGroups != null)
            {
                foreach (var grm in model.ConditionGroups)
                {
                    gr.ConditionGroup1.Add(GetDbGroup_ECN(grm));
                }
            }
            if (model.Conditions != null)
            {
                foreach (var cm in model.Conditions)
                {
                    ECN_Framework_Entities.FormDesigner.Condition c = new ECN_Framework_Entities.FormDesigner.Condition();
                    FillCondition_ECN(c, cm);
                    gr.Conditions.Add(c);
                }
            }
            return gr;
        }

        private void FillRule(ECN_Framework_Entities.FormDesigner.Rule r, RuleModel model, int formId)
        {
            ///
            r.Control_ID = null;
            r.Form_Seq_ID = null;
            if (model.ControlId.HasValue && model.ControlId > -1)
            {
                r.Control_ID = model.ControlId;
            }
            if (r.Control_ID == null)
            {
                r.Form_Seq_ID = formId;
            }
            r.Action = null;
            r.ActionJs = null;
            r.UrlToRedirect = null;
            r.Order = model.Order;
            r.NonQualify = 0;
            r.Overwritedatapost = 0;
            r.SuspendpostDB = 0;
            r.ResultType = 0;
            switch (model.Type)
            {
                case RuleTypes.Field:
                    r.Action = model.Show ? Show : Hide;
                    break;
                case RuleTypes.Form:
                    r.NonQualify = model.IsNonQualifyRule ? 1 : 0;
                    r.Overwritedatapost = model.IsOverWriteDataPost ? 1 : 0;
                    r.SuspendpostDB = model.IsSuspendPostDB ? 1 : 0;
                    r.ResultType = (int)model.ResultOnSubmit;
                    if ((model.IsOverWriteDataPost) && (model.OverwritePostValue != null))
                    {
                        r.OverwritePostValue = new List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost>();
                        foreach (OverwriteDataValueModel owr in model.OverwritePostValue)
                        {

                            bool bExist = false;
                            if (r.OverwritePostValue != null && r.OverwritePostValue.Exists(p => p.Control_ID == owr.FormField && p.Rule_Seq_ID == model.Id))
                                bExist = true;
                            if (bExist == false)
                                r.OverwritePostValue.Add(new ECN_Framework_Entities.FormDesigner.OverwriteDataPost() { Rule_Seq_ID = model.Id > 0 ? model.Id : 0, Value = owr.Value, Control_ID = owr.FormField });
                            else
                            {
                                ECN_Framework_Entities.FormDesigner.OverwriteDataPost odpv = r.OverwritePostValue.FirstOrDefault(p => p.Control_ID == owr.FormField && p.Rule_Seq_ID == model.Id);
                                if (odpv != null)
                                {
                                    r.OverwritePostValue.Remove(odpv);
                                    odpv.Value = owr.Value;
                                    r.OverwritePostValue.Add(odpv);
                                }
                            }
                        }
                    }
                    else
                    {
                        r.OverwritePostValue = new List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost>();
                    }
                    if (model.ResultOnSubmit.HasValue && (model.ResultOnSubmit == ResultType.URL) || (model.ResultOnSubmit == ResultType.KMPaidPage))
                    {
                        if (model.ResultOnSubmit == ResultType.URL)
                            r.UrlToRedirect = model.UrlToRedirect;
                        else if (model.ResultOnSubmit == ResultType.KMPaidPage)
                            r.UrlToRedirect = model.UrlToRedirectKM;
                        if (model.RequestQueryValue != null)
                        {
                            r.RequestQueryValue = new List<ECN_Framework_Entities.FormDesigner.RequestQueryValue>();
                            foreach (RequestQueryDataValueModel rq in model.RequestQueryValue)
                            {
                                bool bExist = false;
                                if ( r.RequestQueryValue != null && r.RequestQueryValue.Exists(p => p.Name == rq.Name && p.Rule_Seq_ID == model.Id))
                                    bExist = true;
                                 
                                if (bExist == false)
                                   r.RequestQueryValue.Add(new ECN_Framework_Entities.FormDesigner.RequestQueryValue() { Rule_Seq_ID = model.Id > 0 ? model.Id : 0, Name = rq.Name, Control_ID = rq.Value });
                                else
                                {
                                    ECN_Framework_Entities.FormDesigner.RequestQueryValue rqv = r.RequestQueryValue.FirstOrDefault(p => p.Control_ID == rq.Value && p.Rule_Seq_ID == model.Id);
                                    if (rqv != null)
                                    {
                                        r.RequestQueryValue.Remove(rqv);
                                        rqv.Name = rq.Name;
                                        r.RequestQueryValue.Add(rqv);
                                    }
                                }
                            }
                        }
                        else
                        {
                            r.RequestQueryValue = new List<ECN_Framework_Entities.FormDesigner.RequestQueryValue>();
                        }

                    }
                    else
                    {
                        r.Action = model.Action;
                        r.ActionJs = model.ActionJs;
                    }
                    break;
            }



   
    ///
}
    }
}