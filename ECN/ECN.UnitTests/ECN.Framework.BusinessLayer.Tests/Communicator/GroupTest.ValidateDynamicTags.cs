using System;
using System.Collections.Generic;
using System.Reflection;
using ECN_Framework_Common.Objects;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public partial class GroupTest
    {
        private const string TestedMethod_ValidateDynamicTags = "ValidateDynamicTags";
        private const string TestedMethod_ValidateDynamicTagsUseAmbientTransaction = "ValidateDynamicTags_UseAmbientTransaction";

        [Test]
        public void ValidateDynamicTags_DoesNotThrowException()
        {
            var methodArguments = new object[]
            {
                GroupID,
                ValidLayoutId,
                _user
            };

            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicTags, methodArguments);
            });
        }

        [Test]
        public void ValidateDynamicTags_ThrowException()
        {
            var methodArguments = new object[]
            {
                GroupID,
                ErrorLayoutId,
                _user
            };

            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicTags, methodArguments);
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }

        [Test]
        public void ValidateDynamicTags_UseAmbientTransaction_DoesNotThrowException()
        {
            var methodArguments = new object[]
            {
                GroupID,
                ValidLayoutId,
                _user
            };

            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicTagsUseAmbientTransaction, methodArguments);
            });
        }

        [Test]
        public void ValidateDynamicTags_UseAmbientTransaction_ThrowException()
        {
            var methodArguments = new object[]
            {
                GroupID,
                ErrorLayoutId,
                _user
            };

            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicTagsUseAmbientTransaction, methodArguments);
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }
    }
}
