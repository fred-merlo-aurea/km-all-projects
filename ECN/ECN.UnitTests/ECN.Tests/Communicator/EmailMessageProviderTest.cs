using System;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using ecn.communicator.classes;
using Shouldly;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EmailMessageProviderTest
    {
        private EmailMessageProvider _emailMessageProvider;

        [SetUp]
        public void SetUp()
        {
            _emailMessageProvider = EmailMessageProvider.CreateInstance("format", "user@km.com", "from", "from@km.com", "user", "123.65.45");
        }

        [TestCase("unmatched")]
        public void GetSubjectUTF_ForMatchesNull_ReturnSameString(String dynamicSubject)
        {
            // Act
            var result = _emailMessageProvider.GetSubjectUTF(dynamicSubject);

            // Assert
            result.ShouldBe(" unmatched");
        }

        [TestCase("\\u02e6bB7bB7bB")]
        public void GetSubjectUTF_ForNonSMatches_ShouldReturnEncString(String dynamicSubject)
        {
            // Act
            var result = _emailMessageProvider.GetSubjectUTF(dynamicSubject);

            // Assert
            result.ShouldBe(" =?utf-8?Q?=CB=A6=00bB7bB7bB?=");
        }

        [TestCase("\\ud01B\\u0111")]
        public void GetSubjectUTF_ForMatches_ShouldReturnEncString(String dynamicSubject)
        {
            // Act
            var result = _emailMessageProvider.GetSubjectUTF(dynamicSubject);

            // Assert
            result.ShouldBe(" =?utf-8?Q?=ED=80=9B=C4=91?=");
        }
    }
}
