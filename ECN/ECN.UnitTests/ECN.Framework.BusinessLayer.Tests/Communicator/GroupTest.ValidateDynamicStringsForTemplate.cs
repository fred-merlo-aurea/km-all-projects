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
        private const string TestedMethod_ValidateDynamicStringsForTemplate = "ValidateDynamicStringsForTemplate";
        private const int GroupID = 1;
        private const string ContentValue = "ContentValue";

        [Test]
        public void ValidateDynamicStringsForTemplate_DoesNotThrowException()
        {
            //Arrange                
            _listOfContent.Add(ContentValue);

            var methodArguments = new object[] 
            {
                _listOfContent,
                GroupID,
                _user
            };

            //Act //Assert
            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicStringsForTemplate, methodArguments);
            });
        }

        [Test]
        public void ValidateDynamicStringsForTemplate_MatchingBadSnippetRegexForOddOcurrences_ThrowsException()
        {
            //Arrange            
            _listOfContent.Add(DataTableContentRegexMathcingValue);

            var methodArguments = new object[]
           {
                _listOfContent,
                GroupID,
                _user
           };

            //Act and Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
                            {
                                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicStringsForTemplate, methodArguments);
                            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }

        [Test]
        public void ValidateDynamicStringsForTemplate_MatchingBadSnippetRegexForEvenOcurrences_ThrowsException()
        {
            //Arrange            
            _listOfContent.Add(DataTableContentRegexMathcingValueMoreThanOnce);

            var methodArguments = new object[]
           {
                _listOfContent,
                GroupID,
                _user
           };

            //Act and Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicStringsForTemplate, methodArguments);
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }

        [Test]
        public void ValidateDynamicStringsForTemplate_MatchingBadSnippetRegexForTwoOcurrences_ThrowsException()
        {
            //Arrange            
            _listOfContent.Add(DataTableContentRegexMathcingValueTwice);

            var methodArguments = new object[]
           {
                _listOfContent,
                GroupID,
                _user
           };

            //Act and Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateDynamicStringsForTemplate, methodArguments);
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }
    }
}
