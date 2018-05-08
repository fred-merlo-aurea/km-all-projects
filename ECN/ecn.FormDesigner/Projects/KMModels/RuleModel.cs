using System;
using System.Collections.Generic;
using KMEntities;
using KMEnums;
using System.ComponentModel.DataAnnotations;

namespace KMModels.PostModels
{
    public class RuleModel : ModelBase
    {
        private const string show = "show";

        [GetFromField("Rule_Seq_ID")]
        public int Id { get; set; }        
        public RuleTypes Type { get; set; }
        [GetFromField("Control_ID")]
        public int? ControlId { get; set; }
        public bool Show { get; set; }
        public string Action { get; set; }
        public string ActionJs { get; set; }
        public string UrlToRedirect { get; set; }
        public int Order { get; set; }
        public ConditionGroupModel MainConditionGroup { get; set; }
        public ResultType? ResultOnSubmit { get; set; }
        [GetFromField("UrlToRedirect")]
        public string UrlToRedirectKM { get; set; }
        [GetFromField("NonQualify")]
        public bool IsNonQualifyRule { get; set; }
        [GetFromField("SuspendpostDB")]
        public bool IsSuspendPostDB { get; set; }
        [GetFromField("Overwritedatapost")]
        public bool IsOverWriteDataPost { get; set; }

        public IEnumerable<OverwriteDataValueModel> OverwritePostValue { get; set; }

        public IEnumerable<ConditionGroupModel> ConditionGroup { get; set; }

        public IEnumerable<RequestQueryDataValueModel> RequestQueryValue { get; set; }


        public override void FillData(object entity)
        {
            base.FillData(entity);
            Rule rule = (Rule)entity;
            Show = rule.Action != null && rule.Action.ToLower() == show;
            MainConditionGroup = rule.ConditionGroup.ConvertToModel<ConditionGroupModel>();
            ConditionGroup = GetAllGroups();
            switch (Type)
            {
                case RuleTypes.Page:
                    if (!ControlId.HasValue)
                    {
                        ControlId = -1;
                    }
                    break;
                case RuleTypes.Form:
                    //ResultOnSubmit = UrlToRedirect == null ? ResultType.Message : ResultType.URL;
                    
                    ResultOnSubmit = (ResultType)rule.ResultType;
                    
                    IsNonQualifyRule =rule.NonQualify == 1 ? true : false;
                    IsSuspendPostDB = rule.SuspendpostDB == 1 ? true : false;
                    IsOverWriteDataPost = rule.Overwritedatapost == 1 ? true : false;
                   // OverwritePostValue = rule.OverwritedataPostValues.ConvertToModels<OverwriteDataValueModel>();
                   // RequestQueryValue = rule.RequestQueryValues.ConvertToModels<RequestQueryDataValueModel>();
                    if (rule.OverwritedataPostValues != null)
                    {
                        List<OverwritedataPostValue> Olist = new List<OverwritedataPostValue>();
                        foreach (var r in rule.OverwritedataPostValues)
                        {
                            //if (!r.IsDeleted)
                                Olist.Add(r);
                        }
                        OverwritePostValue = Olist.ConvertToModels<OverwriteDataValueModel>();
                    }
                    if (rule.RequestQueryValues != null)
                    {
                        List<RequestQueryValue> rlist = new List<RequestQueryValue>();
                        foreach (var r in rule.RequestQueryValues)
                        {
                            //if (!r.IsDeleted)
                                rlist.Add(r);
                        }
                        RequestQueryValue = rlist.ConvertToModels<RequestQueryDataValueModel>();
                    }
                    break;
            }
        }

        private IEnumerable<ConditionGroupModel> GetAllGroups()
        {
            List<ConditionGroupModel> res = new List<ConditionGroupModel>();
            Add(res, MainConditionGroup, true);

            return res;
        }

        private void Add(List<ConditionGroupModel> res, ConditionGroupModel gr, bool isMainGroup)
        {
            if (!isMainGroup)
            {
                res.Add(gr);
            }
            if (gr.ConditionGroups != null)
            {
                foreach (var inner in gr.ConditionGroups)
                {
                    Add(res, inner, false);
                }
            }
        }
    }
    public class OverwriteDataValueModel : ModelBase
    {
        protected const string Required = "Form Rules Fill in Value for Overwrite Data";
        private const int NameMaxLen = 100;
        private const string TooLong = "Value is too long";
        private const string RegexError = "Invalid Value for Overwrite Data Posting. Only allowed a-z, A-Z, 0-9 and _ characters.";


        [GetFromField("Control_ID")]
        public int FormField { get; set; }

        //[Required(ErrorMessage = Required)]
        //[MaxLength(NameMaxLen, ErrorMessage = TooLong)]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = RegexError)]
        public string Value { get; set; }

     
    }
    public class RequestQueryDataValueModel : ModelBase
    {
        protected const string Required = "Form Rules Fill in Name for Querystring Parameter";
        private const int NameMaxLen = 30;
        private const string TooLong = "Name is too long";
        private const string RegexError = "Invalid Name for Redirect to website. Only allowed a-z, A-Z, 0-9 and _ characters.";


        [GetFromField("Control_ID")]
        public int Value { get; set; }

        //[Required(ErrorMessage = Required)]
        //[MaxLength(NameMaxLen, ErrorMessage = TooLong)]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = RegexError)]
        public string Name { get; set; }


    }

}