using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Transactions.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class GroupTest
    {
        private IDisposable _shimObject;
        private PrivateType _testedClass;
        private DataTable _dataTableContent;
        private List<string> _listOfContent;
        private KMPlatform.Entity.User _user;
        private const string TestedClassName = "ECN_Framework_BusinessLayer.Communicator.Group";
        private const string TestedClassAssemblyName= "ECN_Framework_BusinessLayer";
        private const string DataTableContentColumnName = "columnName";
        private const string DataTableContentRegexMathcingValue = "%%";
        private const string DataTableContentRegexMathcingValueTwice = "%%Content%%";
        private const string DataTableContentRegexMathcingValueMoreThanOnce = "%%Content%%Wrong%%%%";
        private const string GroupDataFieldShortName = "groupDataFieldShortName";
        private const string ErrorFieldName = "ErrorField";
        private const int ValidLayoutId = 1;
        private const int ErrorLayoutId = 2;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testedClass = new PrivateType(TestedClassAssemblyName, TestedClassName);
            _listOfContent = new List<string>();
            _user = new KMPlatform.Entity.User();

            _dataTableContent = new DataTable();
            _dataTableContent.Columns.Add(DataTableContentColumnName);
            _dataTableContent.Rows.Add(DataTableContentColumnName);

            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 =
                (groupId) =>
                {
                    var groupDataList = new List<GroupDataFields>();
                    groupDataList.Add(new GroupDataFields()
                    {
                        ShortName = GroupDataFieldShortName
                    });
                    return groupDataList;
                };

            ShimGroupDataFields.GetByGroupID_NoAccessCheck_UseAmbientTransactionInt32 =
                (groupId) =>
                {
                    var groupDataList = new List<GroupDataFields>();
                    groupDataList.Add(new GroupDataFields()
                    {
                        ShortName = GroupDataFieldShortName
                    });
                    return groupDataList;
                };

            ShimEmail.GetColumnNames =
                () =>
                { 
                    return _dataTableContent;
                };

            ShimEmail.GetColumnNames_UseAmbientTransaction =
                () =>
                {
                    return _dataTableContent;
                };

            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean =
                (layoutId, getChildren) =>
                {
                    if (layoutId == ValidLayoutId)
                    {
                        return GetLayout(GroupDataFieldShortName);
                    }

                    if (layoutId == ErrorLayoutId)
                    {
                        return GetLayout(ErrorFieldName);
                    }

                    return null;
                };

            ShimLayout.GetByLayoutID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (layoutId, getChildren) =>
                {
                    if (layoutId == 1)
                    {
                        return GetLayout(GroupDataFieldShortName);
                    }

                    if (layoutId == 2)
                    {
                        return GetLayout(ErrorFieldName);
                    }

                    return null;
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private Layout GetLayout(string fieldName)
        {
            Layout layout;
            DynamicTag dynamicTag;
            DynamicTagRule dynamicTagRule;
            ECN_Framework_Entities.Communicator.Rule rule;
            RuleCondition ruleCondition;

            layout = new Layout
            {
                Slot1 = new Content()
            };

            dynamicTag = new DynamicTag();
            dynamicTagRule = new DynamicTagRule();

            ruleCondition = new RuleCondition
            {
                Field = fieldName
            };
            rule = new ECN_Framework_Entities.Communicator.Rule
            {
                RuleConditionsList = new List<RuleCondition>()
            };
            rule.RuleConditionsList.Add(ruleCondition);

            dynamicTagRule.Rule = rule;

            dynamicTag.DynamicTagRulesList = new List<DynamicTagRule>
            {
                dynamicTagRule
            };

            layout.Slot1.DynamicTagList = new List<DynamicTag>
            {
                dynamicTag
            };

            return layout;
        }
    }
}
