using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KMEntities;
using KMManagers.Tests.Helpers;
using KMModels;
using KMModels.PostModels;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;

namespace KMManagers.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class RuleManagerTest
    {
        private IDisposable _shimObject;
        private RuleManager _ruleManager;
        private Form _form;
        private KMPlatform.Entity.User _user;
        private FormRulesPostModel _model;
        private List<ECN_Framework_Entities.FormDesigner.Rule> _rules;
        private ECN_Framework_Entities.FormDesigner.Rule _defaultRule;
        private List<ECN_Framework_Entities.FormDesigner.RequestQueryValue> _requestQueryValues;
        private List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost> _overwriteDataPost;
        private const int ChannelID = 5;
        private const int ControlID = 1;
        private const int FormModelId = 10;
        private const int RuleModelId = 12;
        private const int RuleSeqId = 100;
        private const int RuleSeqIdToBeRemoved = 102;
        private const int RuleSeqIdToBeAdded = 103;        
        private const int ConditionGroupSeqID = 70;
        private const int ConditionSeqID = 60;
        private const int DummyInt = 2;
        private const bool TrueBoolean = true;
        private const string Username = "dummy_username";
        private const string DummyString = "DummyStringTextValue";

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _form = KMManagerTestsHelper.GetForm();
            InitializeMethodParameters();

            KMManagerTestsHelper.CreateAppSettingsShim();

            KMManagerTestsHelper.CreateWebAppSettingsShim();

            KMManagerTestsHelper.CreateEmailGroupShim();

            KMManagerTestsHelper.CreateMXValidateShim();

            KMManagerTestsHelper.CreateFormManagerShim(new List<ECN_Framework_Entities.Communicator.GroupDataFields>(), _form);

            KMManagerTestsHelper.CreateDbResolverShim();

            _ruleManager = new RuleManager();

            CreateConditionShims();
            CreateConditionGroupShims();
            CreateOverwriteDataPostShims();
            CreateRequestQueryValue();

            ECN_Framework_DataLayer.FormDesigner.Fakes.ShimRule.GetByFormIDInt32 = (formID) => _rules;
            ECN_Framework_DataLayer.FormDesigner.Fakes.ShimRule.GetByRuleIDInt32 = (ruleID) => _defaultRule;
            ECN_Framework_DataLayer.FormDesigner.Fakes.ShimRule.SaveRule = (rule) => DummyInt;
            ECN_Framework_DataLayer.FormDesigner.Fakes.ShimRule.DeleteInt32 = (ruleID) => { };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
        
        private void InitializeMethodParameters()
        {
            _user = new KMPlatform.Entity.User()
            {
                UserName = Username
            };

            var mainConditionGroup = new ConditionGroupModel()
            {                
                LogicGroup = KMEnums.ConditionType.And,
                Conditions = new List<ConditionModel>()
                {
                    new ConditionModel()
                    {
                        ControlId = ControlID,
                        ComparisonType = KMEnums.ComparisonType.After,
                        IsSelectableItem = TrueBoolean,
                        Value = DummyString
                    }
                }
            };

            var requestQueryValue = new RequestQueryDataValueModel()
            {
                Name = DummyString,
                Value = DummyInt
            };

            var overwritePostValue = new OverwriteDataValueModel()
            {
                FormField = ControlID,
                Value = DummyString
            };

            _model = new FormRulesPostModel()
            {
                Id = FormModelId,
                Rules = new List<RuleModel>()
                {
                    new RuleModel()
                    {
                        Id = RuleModelId,
                        Type = KMEnums.RuleTypes.Form,
                        MainConditionGroup = mainConditionGroup,
                        ControlId = ControlID,
                        Order = DummyInt,
                        Show = TrueBoolean,
                        ConditionGroup = new List<ConditionGroupModel>(){mainConditionGroup},
                        RequestQueryValue = new List<RequestQueryDataValueModel>(){requestQueryValue},
                        OverwritePostValue = new List<OverwriteDataValueModel>(){ overwritePostValue },
                        ResultOnSubmit = KMEnums.ResultType.KMPaidPage,
                        Action = DummyString,
                        ActionJs = DummyString,
                        IsOverWriteDataPost = true,
                        UrlToRedirectKM = DummyString
                    }
                }
            };

            _defaultRule = CreateRule(RuleSeqId);
            _rules = new List<ECN_Framework_Entities.FormDesigner.Rule>();

            _requestQueryValues = new List<ECN_Framework_Entities.FormDesigner.RequestQueryValue>()
            {
                new ECN_Framework_Entities.FormDesigner.RequestQueryValue()
                {
                    Name = DummyString,
                    Rule_Seq_ID = RuleSeqId
                }
            };

            _overwriteDataPost = new List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost>()
            {
                new ECN_Framework_Entities.FormDesigner.OverwriteDataPost()
                {
                    Control_ID = ControlID,
                    Rule_Seq_ID = RuleSeqId,
                    Value = DummyString
                }
            };
        }

        private ECN_Framework_Entities.FormDesigner.Rule CreateRule(int ruleSeqId)
        {
            return new ECN_Framework_Entities.FormDesigner.Rule()
            {
                Rule_Seq_ID = ruleSeqId,
                ConditionGroup_Seq_ID = ConditionGroupSeqID,
                Type = (int)KMEnums.RuleTypes.Form
            };
        }

        private static void CreateConditionGroupShims()
        {
            var condigionGroup = new ECN_Framework_Entities.FormDesigner.ConditionGroup()
            {
                ConditionGroup_Seq_ID = ConditionGroupSeqID,
                LogicGroup = true,
                Conditions = new List<ECN_Framework_Entities.FormDesigner.Condition>()
                {
                    new ECN_Framework_Entities.FormDesigner.Condition()
                    {
                        Control_ID = ControlID,
                        Operation_ID= (int)KMEnums.ComparisonType.After,                        
                        Value = DummyString
                    }
                }
            };
            var conditionGroups = new List<ECN_Framework_Entities.FormDesigner.ConditionGroup>() { condigionGroup };

            ShimConditionGroup.GetByCondGroupIDInt32Boolean = (seqId, getChildren) => condigionGroup;
            ShimConditionGroup.SaveConditionGroup = (conditionGroup) => DummyInt;
            ShimConditionGroup.GetByMainGroupIDInt32Boolean = (cgID, fillChildren) => conditionGroups;
            ShimConditionGroup.DeleteInt32 = (condGroupId) => { };
            ShimConditionGroup.SaveConditionGroup = (conditionGroup) => DummyInt;
        }

        private static void CreateConditionShims()
        {
            ShimCondition.GetByCondGroupIDInt32 =
                (seqId) =>
                {
                    var condition = new ECN_Framework_Entities.FormDesigner.Condition()
                    {
                        Condition_Seq_ID = ConditionSeqID
                    };
                    return new List<ECN_Framework_Entities.FormDesigner.Condition>() { condition };
                };
            ShimCondition.DeleteInt32 = (condId) => { };
            ShimCondition.SaveCondition = (condition) => DummyInt;
        }

        private void CreateRequestQueryValue()
        {
            ShimRequestQueryValue.GetByRuleIDInt32 = (seqId) => _requestQueryValues;
            ShimRequestQueryValue.DeleteByRuleIDInt32 = (seqID) => { };
            ShimRequestQueryValue.SaveRequestQueryValue = (rqv) => DummyInt;
        }

        private void CreateOverwriteDataPostShims()
        {
            ShimOverwriteDataPost.GetByRuleIDInt32 = (seqId) => _overwriteDataPost;
            ShimOverwriteDataPost.DeleteByRuleIDInt32 = (seqID) => { };
            ShimOverwriteDataPost.SaveOverwriteDataPost = (odp) => DummyInt;
        }
    }
}
