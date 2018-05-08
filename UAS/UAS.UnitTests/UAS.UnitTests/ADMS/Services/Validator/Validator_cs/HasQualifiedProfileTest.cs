using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using UASObject = FrameworkUAS.Object;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HasQualifiedProfileTest : Fakes
    {
        private const string SampleAccountNumber = "Account12345";
        private const string SampleFName = "SampleFName";
        private const string SampleLName = "SampleLName";
        private const string SampleAddress = "SampleAddress";
        private const string SampleZip = "SampleZip";
        private const string SampleCity = "SampleCity";
        private const string SampleRegCode = "SampleRegCode";
        private const string SampleCountry = "SampleCountry";
        private const string SamplePhone = "1234567890";
        private const string SampleFax = "1234567890";
        private const string SampleMobile = "1234567890";
        private const string SampleCompany = "SampleCompany";
        private const string SampleTitle = "SampleTitle";
        private const string QualifiedPhoneKey = "QualifiedPhone";
        private const string QualifiedTitleKey = "QualifiedTitle";
        private const string QualifiedCompanyKey = "QualifiedCompany";
        private const string QualifiedAddressKey = "QualifiedAddress";
        private const string QualifiedNameKey = "QualifiedName";
        private const string SampleTestFile = "test.txt";
        private const string HasQualifiedProfileMethodName = "HasQualifiedProfile";
        private static readonly Dictionary<string, string[]> TestRulesDictionary = new Dictionary<string, string[]>
        {
            { QualifiedPhoneKey, new string [] { "Phone_Mobile_Fax", "Phone", "Mobile", "Fax" }  },
            { QualifiedTitleKey, new string [] { "Title" } },
            { QualifiedCompanyKey, new string [] { "Company" }  },
            { QualifiedAddressKey, new string [] { "Address", "Zip_City", "State_Country"}  },
            { QualifiedNameKey, new string [] { "FName_LName", "FName", "LName" }  }
        };
        private TestEntity testEntity;
        private FrameworkUAD_Lookup.Enums.FileTypes dft;
        private ImportFile dataIV;
        private AdmsLog admsLog;

        [SetUp]
        public void SetUp()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            dft = FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms;
            dataIV = new ImportFile(new FileInfo(SampleTestFile));
            admsLog = new AdmsLog();
        }

        [Test]
        public void HasQualifiedProfile_WithEmptyCheckList_ReturnsEmptyQualifiedProfileSet()
        {
            // Arrange
            var checkList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed { AccountNumber = SampleAccountNumber }
            };
            var sourceFile = new SourceFile { RuleSets = GetRuleSets() };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            dataIV.ShouldNotBeNull();
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("First name and last name are missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Address is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Zip code and City are missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Country and state are missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Phone, fax or mobile is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Company is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Title is missing."));
        }

        [Test]
        public void HasQualifiedProfile_WithQualifiedCheckList_RetrunsQualifiedProfileSetWithNoErrors()
        {
            // Arrange
            var subscriberTransformed = GetSampleSubscriberTransformed();
            var checkList = new HashSet<SubscriberTransformed> { subscriberTransformed };
            var sourceFile = new SourceFile { RuleSets = GetRuleSets() };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result.ShouldContain(checkList.First());
            dataIV.ImportErrorCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldBeEmpty();
        }

        [Test]
        public void HasQualifiedProfile_WithNonDateRangeRulesAndUnQualifiedCheckList_ReturnsImportErrors()
        {
            // Arrange
            var checkList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed { AccountNumber = SampleAccountNumber }
            };
            var ruleSets = GetRuleSets();
            ruleSets.First().StartDay = DateTime.Now.AddDays(2).Day; // Have StartDay more than Date.Now.Day to get NonDateRange
            var sourceFile = new SourceFile { RuleSets = ruleSets };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            dataIV.ShouldNotBeNull();
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("First name and last name are missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Address is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Zipcode or city is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("State or country is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Phone, mobile, or fax is missing."));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("Title is missing."));
        }

        [Test]
        public void HasQualifiedProfile_WithNonDateRangeRulesAndQualifiedProfile_ReturnsQualifiedProfileSet()
        {
            // Arrange
            var subscriberTransformed = GetSampleSubscriberTransformed();
            var checkList = new HashSet<SubscriberTransformed> { subscriberTransformed };
            var ruleSets = GetRuleSets();
            ruleSets.First().StartDay = DateTime.Now.AddDays(2).Day; // Have StartDay more than Date.Now.Day to get NonDateRange
            var sourceFile = new SourceFile { RuleSets = ruleSets };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result.ShouldContain(checkList.First());
            dataIV.ImportErrorCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldBeEmpty();
        }

        [Test]
        public void HasQualifiedProfile_WithNonDateRangeRulesAndEmptyRulesAndBreakAlwaysCode_ReturnsImportErrorList()
        {
            // Arrange
            var subscriberTransformed = GetSampleSubscriberTransformed();
            var checkList = new HashSet<SubscriberTransformed> { subscriberTransformed };
            var sourceFile = new SourceFile { RuleSets = GetBreakRules() };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;
            
            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.HasError.ShouldBeTrue();
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().Phone));
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().FName));
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().LName));
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().Address));
        }

        [Test]
        public void HasQualifiedProfile_WithNonDateRangeRulesAndEmptyRulesAndBreakFalseCode_ReturnsImportErrorList()
        {
            // Arrange
            var subscriberTransformed = GetSampleSubscriberTransformed();
            var checkList = new HashSet<SubscriberTransformed> { subscriberTransformed };
            var ruleSets = GetBreakRules();
            ruleSets.First().Rules.RemoveWhere(x => x.RuleChainId > 0);
            // Add BreakFalse RuleCode
            ruleSets.First().Rules.Add(new UASObject.Rule
            {
                ConditionChainId = (int)RuleCodeName.BreakFalse,
                RuleGroupChainId = (int)RuleCodeName.BreakFalse,
                RuleChainId = (int)RuleCodeName.BreakFalse
            });
            var sourceFile = new SourceFile { RuleSets = ruleSets };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed> ;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.HasError.ShouldBeTrue();
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().Phone));
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().FName));
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().LName));
            dataIV.ImportErrors.ShouldContain(x => x.BadDataRow.Contains(checkList.First().Address));
        }

        [Test]
        public void HasQualifiedProfile_WithNonDateRangeRulesAndQualifiedProfileAndBreakTrue_WithQualifiedProfile()
        {
            // Arrange
            var subscriberTransformed = GetSampleSubscriberTransformed();
            var checkList = new HashSet<SubscriberTransformed> { subscriberTransformed };
            var ruleSets = GetBreakRules();
            ruleSets.First().Rules.RemoveWhere(x => x.RuleChainId > 0);
            // Add BreakTrue RuleCode
            ruleSets.First().Rules.Add(new UASObject.Rule
            {
                ConditionChainId = (int)RuleCodeName.BreakTrue,
                RuleGroupChainId = (int)RuleCodeName.BreakTrue,
                RuleChainId = (int)RuleCodeName.BreakTrue,
                RuleName = QualifiedTitleKey,
                RuleValues = new HashSet<RuleValue>
                {
                    new RuleValue
                    {
                        Value = 
                        TestRulesDictionary[QualifiedTitleKey][0]
                    }
                }
            });
            var sourceFile = new SourceFile { RuleSets = ruleSets };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result.ShouldContain(checkList.First());
            dataIV.ImportErrorCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldBeEmpty();
        }

        [Test]
        public void HasQualifiedProfile_WithDateRangeRulesAndBreakAlwaysTrue_ReturnsEmptyProfileSet()
        {
            // Arrange
            var checkList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };
            var ruleSets = GetRuleSets();
            ruleSets.First().Rules.Add(new UASObject.Rule { RuleChainId = (int)RuleCodeName.BreakTrue });
            var sourceFile = new SourceFile { RuleSets = ruleSets };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;
            
            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result.ShouldContain(checkList.First());
            dataIV.ImportErrorCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldBeEmpty();
        }

        [Test]
        public void HasQualifiedProfile_WithDateRangeRulesAndBreakAlways_ReturnsQualifiedProfile()
        {
            // Arrange
            var checkList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };
            var ruleSets = GetRuleSets();
            ruleSets.First().Rules.RemoveWhere(x => x.RuleChainId > 0);
            ruleSets.First().Rules.Add(new UASObject.Rule { RuleChainId = (int)RuleCodeName.BreakAlways });
            var sourceFile = new SourceFile { RuleSets = ruleSets };
            var parameters = new object[]
            {
                checkList,
                dataIV,
                dft,
                admsLog,
                sourceFile
            };

            // Act
            var result = ReflectionHelper.CallOverloadedMethod(
                testEntity.Validator.GetType(),
                GetMethodParametertype(),
                HasQualifiedProfileMethodName,
                parameters,
                testEntity.Validator) as HashSet<SubscriberTransformed>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result.ShouldContain(checkList.First());
            dataIV.ImportErrorCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldBeEmpty();
        }

        private Type[] GetMethodParametertype()
        {
            return new Type[]
            {
                typeof(HashSet<SubscriberTransformed>),
                typeof(ImportFile),
                typeof(FrameworkUAD_Lookup.Enums.FileTypes),
                typeof(AdmsLog),
                typeof(SourceFile),
            };
        }
        
        private HashSet<UASObject.RuleSet> GetRuleSets()
        {
            var qualifiedProfileEnum = FrameworkUAD_Lookup.Enums.ExecutionPointType.QualifiedProfile;
            var ruleSets = new HashSet<UASObject.RuleSet>
                {
                    new UASObject.RuleSet
                    {
                        ExecutionOrder = 1,
                        _executionPoint = qualifiedProfileEnum.ToString(),
                        StartMonth = DateTime.Now.AddMonths(-1).Month,
                        EndMonth = DateTime.Now.AddMonths(5).Month,
                        StartDay = DateTime.Now.AddDays(-5).Day,
                        EndDay = DateTime.Now.AddDays(5).Day,
                        StartYear = 0,
                        Rules = new HashSet<UASObject.Rule>
                        {
                            new UASObject.Rule
                            {
                                ConditionGroup = 1,
                                ConditionChainId= (int)RuleCodeName.Or,
                                RuleGroup = 1,
                                RuleGroupChainId = (int)RuleCodeName.Or,
                                RuleId = 1,
                                RuleChainId = (int)RuleCodeName.Or,
                                RuleOrder = 1,
                                RuleName = QualifiedPhoneKey,
                                RuleValues = new HashSet<RuleValue>
                                {
                                    new RuleValue { Value = TestRulesDictionary[QualifiedPhoneKey][0] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedPhoneKey][1] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedPhoneKey][2] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedPhoneKey][3] }
                                }
                            },
                            new UASObject.Rule
                            {
                                ConditionGroup = 1,
                                ConditionChainId= (int)RuleCodeName.Or,
                                RuleGroup = 1,
                                RuleGroupChainId = (int)RuleCodeName.Or,
                                RuleId = 2,
                                RuleChainId = (int)RuleCodeName.Or,
                                RuleOrder = 2,
                                RuleName = QualifiedCompanyKey,
                                RuleValues = new HashSet<RuleValue>
                                {
                                    new RuleValue { Value = TestRulesDictionary[QualifiedCompanyKey][0] },
                                }
                            },
                            new UASObject.Rule
                            {
                                ConditionGroup = 1,
                                ConditionChainId= (int)RuleCodeName.And,
                                RuleGroup = 3,
                                RuleGroupChainId = (int)RuleCodeName.And,
                                RuleId = 3,
                                RuleChainId = (int)RuleCodeName.And,
                                RuleOrder = 3,
                                RuleName = QualifiedAddressKey,
                                RuleValues = new HashSet<RuleValue>
                                {
                                    new RuleValue { Value = TestRulesDictionary[QualifiedAddressKey][0] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedAddressKey][1] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedAddressKey][2] }
                                }
                            },
                            new UASObject.Rule
                            {
                                ConditionGroup = 2,
                                ConditionChainId= (int)RuleCodeName.And,
                                RuleGroup = 1,
                                RuleGroupChainId = (int)RuleCodeName.And,
                                RuleId= 4,
                                RuleChainId = (int)RuleCodeName.And,
                                RuleOrder = 4,
                                RuleName = QualifiedNameKey,
                                RuleValues = new HashSet<RuleValue>
                                {
                                    new RuleValue { Value = TestRulesDictionary[QualifiedNameKey][0] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedNameKey][1] },
                                    new RuleValue { Value = TestRulesDictionary[QualifiedNameKey][2] }
                                }
                            },
                            new UASObject.Rule
                            {
                                ConditionGroup = 2,
                                ConditionChainId = (int)RuleCodeName.Or,
                                RuleGroup = 1,
                                RuleGroupChainId = (int)RuleCodeName.Or,
                                RuleId = 5,
                                RuleChainId = (int)RuleCodeName.Or,
                                RuleOrder = 5,
                                RuleName = QualifiedTitleKey,
                                RuleValues = new HashSet<RuleValue>
                                {
                                    new RuleValue { Value = TestRulesDictionary[QualifiedTitleKey][0] }
                                }
                            },
                        }
                    },
                };
            return ruleSets;
        }

        private HashSet<UASObject.RuleSet> GetBreakRules()
        {
            var qualifiedProfileEnum = FrameworkUAD_Lookup.Enums.ExecutionPointType.QualifiedProfile;
            var ruleSets = new HashSet<UASObject.RuleSet>
            {
                new UASObject.RuleSet
                {
                   ExecutionOrder = 1,
                   _executionPoint = qualifiedProfileEnum.ToString(),
                   Rules = new HashSet<UASObject.Rule>
                   {
                       new UASObject.Rule
                       {
                           ConditionChainId = (int)RuleCodeName.BreakAlways,
                           RuleChainId = (int)RuleCodeName.BreakAlways,
                           RuleGroupChainId = (int)RuleCodeName.BreakAlways,
                       }
                   }

                }
            };
            return ruleSets;
        }

        private SubscriberTransformed GetSampleSubscriberTransformed()
        {
            return new SubscriberTransformed
            {
                AccountNumber = SampleAccountNumber,
                FName = SampleFName,
                LName = SampleLName,
                Address = SampleAddress,
                Zip = SampleZip,
                City = SampleCity,
                RegCode = SampleRegCode,
                Country = SampleCountry,
                Phone = SamplePhone,
                Fax = SampleFax,
                Mobile = SampleMobile,
                Company = SampleCompany,
                Title = SampleTitle
            };
        }
    }
}
