﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34209
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EmailMarketing.API.Specs.EmailGroup
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [TechTalk.SpecRun.FeatureAttribute("ManageSubscriberWithProfile", Description="In order manage Email Group member profiles\r\nAs an API user\r\nI want methods to co" +
        "ntrol Email Group membership and update profile and user defined fields", SourceFile="EmailGroup\\ManageSubscriberWithProfile.feature", SourceLine=0)]
    public partial class ManageSubscriberWithProfileFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ManageSubscriberWithProfile.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ManageSubscriberWithProfile", "In order manage Email Group member profiles\r\nAs an API user\r\nI want methods to co" +
                    "ntrol Email Group membership and update profile and user defined fields", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [TechTalk.SpecRun.FeatureCleanup()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        [TechTalk.SpecRun.ScenarioCleanup()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Add Subscribers", new string[] {
                "EmailGroup_API",
                "AddSubscribersWithProfile_API"}, SourceLine=8)]
        public virtual void AddSubscribers()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add Subscribers", new string[] {
                        "EmailGroup_API",
                        "AddSubscribersWithProfile_API"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 12
   testRunner.Given("I initialize a Subscriber Profile Test-Data Provider", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
     testRunner.And("I have a valid API Access Key of \"8CAB09B9-BEC9-453F-A689-E85D5C9E4898\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("I have a Customer ID of 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("I set ControllerName to \"EmailGroup\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("I set ActionName to \"methods/ManageSubscriberWithProfile\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And("I set Test Data Provider GroupID to 234388", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Email Address",
                        "Group ID",
                        "Format",
                        "Subscribe Type",
                        "UDF 1 Name",
                        "UDF 1 Value",
                        "UDF 2 Name",
                        "UDF 2 Value",
                        "TF 1.1 N",
                        "TF 1.1 V",
                        "TF 1.2 N",
                        "TF 1.2 V",
                        "TF 2.1 N",
                        "TF 2.1 V",
                        "TF 2.2 N",
                        "TF 2.2 V"});
            table1.AddRow(new string[] {
                        "#GlobalMasterSupressionList#",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#ChannelMasterSupressionList#",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#MasterSupressionGroup#",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@format.com",
                        "-1",
                        "Unknown",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@subscribe-type.com",
                        "-1",
                        "HTML",
                        "MasterSuppress",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@for-unsubscribe.com",
                        "-1",
                        "HTML",
                        "Unsubscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid_email",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@udfs.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "BadUDF",
                        "BadUDF_Val",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid2@udfs.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "BadUDF",
                        "BadUDF_Val",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@udfsandtype.com",
                        "-1",
                        "HTML",
                        "MasterSuppress",
                        "BadUDF",
                        "BadUDF_Val",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@udfsandformat.com",
                        "-1",
                        "Unknown",
                        "Subscribe",
                        "BadUDF",
                        "BadUDF_Val",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@missingtfpkey.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "Property1Name",
                        "Prop1Value",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid2@missingtfpkey.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "BadTF",
                        "BadTF",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@tf.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "BadTF",
                        "BadTFVal",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@tfandtype.com",
                        "-1",
                        "HTML",
                        "MasterSuppress",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "BadTF",
                        "BadTFVal",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "invalid@tfandformat.com",
                        "-1",
                        "Unknown",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "BadTF",
                        "BadTFVal",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#NotInTestGroup#",
                        "-1",
                        "HTML",
                        "Unsubscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#NotInTestGroup# 2",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#SubscribedInTestGroup#",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#SubscribedInTestGroup# 2",
                        "-1",
                        "HTML",
                        "Unsubscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#UnsubscribedInTestGroup#",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#UnsubscribedInTestGroup# 2",
                        "-1",
                        "HTML",
                        "Unsubscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new01@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new02@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "EMPTY LIST",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new03@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "EMPTY LIST",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new04@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "EMPTY SET",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new05@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "EMPTY SET",
                        "",
                        "EMPTY SET",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new06@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new07@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "UDF2",
                        "UDF2_Val",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new08@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new09@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "",
                        "",
                        "RECORDKEY",
                        "98765",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new10@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "Property1Name",
                        "Prop1Val",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new11@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "",
                        "",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "Property1Name",
                        "Prop1Val1",
                        "RECORDKEY",
                        "98765",
                        "Property1Name",
                        "Prop1Val2"});
            table1.AddRow(new string[] {
                        "#rand#-new12@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new13@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "",
                        "",
                        "RECORDKEY",
                        "98765",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new14@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "Property1Name",
                        "Prop1Val",
                        "",
                        "",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "#rand#-new15@valid.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "UDF1_Val",
                        "",
                        "",
                        "RECORDKEY",
                        "54321",
                        "Property1Name",
                        "Prop1Val1",
                        "RECORDKEY",
                        "98765",
                        "Property1Name",
                        "Prop1Val2"});
            table1.AddRow(new string[] {
                        "foo@bar.com",
                        "-1",
                        "HTML",
                        "Subscribe",
                        "UDF1",
                        "#RAND#",
                        "UDF2",
                        "#RAND#",
                        "RECORDKEY",
                        "pkey-value1",
                        "Property1Name",
                        "t1-#RAND#",
                        "RECORDKEY",
                        "pkey-#RAND#",
                        "Property1Name",
                        "t2-#RAND#"});
#line 19
  testRunner.And("I have an Email Profile List", ((string)(null)), table1, "And ");
#line 61
 testRunner.When("I invoke POST", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
 testRunner.Then("I should receive an HTTP Response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 63
  testRunner.And("status should be \"200 OK\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
  testRunner.And("HttpRequestContent should contain an object", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
  testRunner.And("the object should be an Enumeration of SubscriptionResult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Email Address",
                        "Group ID",
                        "Status",
                        "Result"});
            table2.AddRow(new string[] {
                        "#GlobalMasterSupressionList#",
                        "-1",
                        "M",
                        "Skipped, MasterSuppressed"});
            table2.AddRow(new string[] {
                        "#ChannelMasterSupressionList#",
                        "-1",
                        "M",
                        "Skipped, MasterSuppressed"});
            table2.AddRow(new string[] {
                        "#MasterSupressionGroup#",
                        "-1",
                        "M",
                        "Skipped, MasterSuppressed"});
            table2.AddRow(new string[] {
                        "invalid@format.com",
                        "-1",
                        "None",
                        "Skipped, InvalidFormatTypeCode"});
            table2.AddRow(new string[] {
                        "invalid@subscribe-type.com",
                        "-1",
                        "None",
                        "Skipped, InvalidSubscribeTypeCode"});
            table2.AddRow(new string[] {
                        "invalid@for-unsubscribe.com",
                        "-1",
                        "None",
                        "Skipped, UnknownSubscriber"});
            table2.AddRow(new string[] {
                        "invalid_email",
                        "-1",
                        "None",
                        "Skipped, InvalidEmailAddress"});
            table2.AddRow(new string[] {
                        "invalid@udfs.com",
                        "-1",
                        "None",
                        "Skipped, UnknownCustomField"});
            table2.AddRow(new string[] {
                        "invalid2@udfs.com",
                        "-1",
                        "None",
                        "Skipped, UnknownCustomField"});
            table2.AddRow(new string[] {
                        "invalid@udfsandtype.com",
                        "-1",
                        "None",
                        "Skipped, InvalidSubscribeTypeCode, UnknownCustomField"});
            table2.AddRow(new string[] {
                        "invalid@udfsandformat.com",
                        "-1",
                        "None",
                        "Skipped, InvalidFormatTypeCode, UnknownCustomField"});
            table2.AddRow(new string[] {
                        "invalid@missingtfpkey.com",
                        "-1",
                        "None",
                        "Skipped, MissingTransactionalPrimaryKeyField"});
            table2.AddRow(new string[] {
                        "invalid2@missingtfpkey.com",
                        "-1",
                        "None",
                        "Skipped, UnknownTransactionalField, MissingTransactionalPrimaryKeyField"});
            table2.AddRow(new string[] {
                        "invalid@tf.com",
                        "-1",
                        "None",
                        "Skipped, UnknownTransactionalField"});
            table2.AddRow(new string[] {
                        "invalid@tfandtype.com",
                        "-1",
                        "None",
                        "Skipped, InvalidSubscribeTypeCode, UnknownTransactionalField"});
            table2.AddRow(new string[] {
                        "invalid@tfandformat.com",
                        "-1",
                        "None",
                        "Skipped, InvalidFormatTypeCode, UnknownTransactionalField"});
            table2.AddRow(new string[] {
                        "#NotInTestGroup#",
                        "-1",
                        "None",
                        "Skipped, UnknownSubscriber"});
            table2.AddRow(new string[] {
                        "#NotInTestGroup# 2",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#SubscribedInTestGroup#",
                        "-1",
                        "S",
                        "Updated, Subscribed"});
            table2.AddRow(new string[] {
                        "#SubscribedInTestGroup# 2",
                        "-1",
                        "U",
                        "Updated, Unsubscribed"});
            table2.AddRow(new string[] {
                        "#UnsubscribedInTestGroup#",
                        "-1",
                        "S",
                        "Updated, Subscribed"});
            table2.AddRow(new string[] {
                        "#UnsubscribedInTestGroup# 2",
                        "-1",
                        "U",
                        "Updated, Unsubscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new01@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new02@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new03@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new04@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new05@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new06@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new07@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new08@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new09@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new10@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new11@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new12@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new13@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new14@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "#rand#-new15@valid.com",
                        "-1",
                        "S",
                        "New, Subscribed"});
            table2.AddRow(new string[] {
                        "foo@bar.com",
                        "-1",
                        "S",
                        "Updated, Subscribed"});
#line 66
  testRunner.And("the Enumeration of SubscriptionResult should validate as", ((string)(null)), table2, "And ");
#line 107
  testRunner.And("I can cleanup EmailGroup Test Records from the DataBase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.TestRunCleanup()]
        public virtual void TestRunCleanup()
        {
TechTalk.SpecFlow.TestRunnerManager.GetTestRunner().OnTestRunEnd();
        }
    }
}
#pragma warning restore
#endregion