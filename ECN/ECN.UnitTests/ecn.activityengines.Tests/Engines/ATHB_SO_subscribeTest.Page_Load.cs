using System;
using Shouldly;
using NUnit.Framework;
using ecn.activityengines.Fakes;
using ecn.communicator.classes.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class ATHB_SO_subscribeTest
    {
        private const string TestedMethodName_PageLoad = "Page_Load";

        [Test]
        public void ATHB_SO_subscribeTest_Page_Load_WtihCacheValue_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs};
            _shimCache.ItemGetString = (value) => _user;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));
        }

        [Test]
        public void ATHB_SO_subscribeTest_Page_Load_NewSubscriber_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };

            ShimGroups.AllInstances.WhatEmailString = (groups, email) => null;

            ShimATHB_SO_subscribe.AllInstances.SubscribeToGroup = (subscribeObj) => _emails;

            var row =_smartFormsHistoryTable.Rows[0];
            row[ColumnResponseUserScreen] = UrlValue + SnippetUnsubscribe;

            QueryString[CustomerIdKey] = CustomerIdValueNewSubscriber;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));
        }

        [Test]
        public void ATHB_SO_subscribeTest_Page_Load_AlternativeFlow_ExceptionCaught()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            ShimATHB_SO_subscribe.AllInstances.SubscribeToGroup = (subscribeObj) => throw new Exception();

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));
        }
    }
}
