using System.Collections.Generic;
using Shouldly;
using NUnit.Framework;
using KMManagers.Tests.Helpers;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;

namespace KMManagers.Tests
{
    [TestFixture]
    public partial class RuleManagerTest
    {
        [Test]
        public void RuleManagerSave_EditRule_WithoutRuleCollectionValues_WithSucces()
        {
            //Arrange            
            _rules.Add(_defaultRule);
            var isSaved = false;
            ShimRequestQueryValue.GetByRuleIDInt32 = (seqId) => new List<ECN_Framework_Entities.FormDesigner.RequestQueryValue>();
            ShimOverwriteDataPost.GetByRuleIDInt32 = (seqId) => new List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost>();

            //Act
            Should.NotThrow(() => _ruleManager.Save(_user, ChannelID, _model));
            isSaved = KMManagerTestsHelper.FormManagerChangesAreSaved;

            //Assert
            isSaved.ShouldBeTrue();
        }

        [Test]
        public void RuleManagerSave_EditRule_RuleTypeAsForm_WithRuleCollectionsValues_WithSucces()
        {
            //Arrange                        
            _rules.Add(_defaultRule);
            var isSaved = false;

            //Act
            Should.NotThrow(() => _ruleManager.Save(_user, ChannelID, _model));
            isSaved = KMManagerTestsHelper.FormManagerChangesAreSaved;

            //Assert
            isSaved.ShouldBeTrue();
        }

        [Test]
        public void RuleManagerSave_DeleteRule_WithSucces()
        {
            //Arrange            
            var ruleToBeRemoved = CreateRule(RuleSeqIdToBeRemoved);
            _rules.Add(ruleToBeRemoved);
            var isSaved = false;

            //Act
            Should.NotThrow(() => _ruleManager.Save(_user, ChannelID, _model));
            isSaved = KMManagerTestsHelper.FormManagerChangesAreSaved;

            //Assert
            isSaved.ShouldBeTrue();
        }

        [Test]
        public void RuleManagerSave_AddRule_WithCollectionValues_WithSucces()
        {
            //Arrange            
            var ruleToBeAdded = CreateRule(RuleSeqIdToBeAdded);
            ruleToBeAdded.UrlToRedirect = DummyString;
            ruleToBeAdded.OverwritePostValue = _overwriteDataPost;
            ruleToBeAdded.RequestQueryValue = _requestQueryValues;

            _rules.Add(ruleToBeAdded);

            var isSaved = false;

            //Act
            Should.NotThrow(() => _ruleManager.Save(_user, ChannelID, _model));
            isSaved = KMManagerTestsHelper.FormManagerChangesAreSaved;

            //Assert
            isSaved.ShouldBeTrue();
        }
    }
}
