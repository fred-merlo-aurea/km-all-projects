using System;
using System.Web.Fakes;
using System.Collections.Specialized;
using Shouldly;
using NUnit.Framework;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class ATHB_SO_subscribeTest 
    {
        private const string TestedMethodName_CreateNote = "CreateNote";

        [Test]
        public void ATHB_SO_subscribe_CreateNote_WithSuccess()
        {
            //Arrange
            var result = String.Empty;

            //Act
            result = (string)_privateTestedObject.Invoke(TestedMethodName_CreateNote);

            //Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void ATHB_SO_subscribe_CreateNote_WithExceptionCaught()
        {
            //Arrange
            var result = String.Empty;

            ShimHttpRequest.StaticConstructor = () =>
            {
                var shimHttpRequest = new ShimHttpRequest()
                {
                    RawUrlGet = () => null
                };
            };

            //Act
            Should.NotThrow(() =>
            {
                result = (string)_privateTestedObject.Invoke(TestedMethodName_CreateNote);
            });

            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void ATHB_SO_subscribe_CreateNote_WithNullPropertiesValues_WithSucces()
        {
            //Arrange
            var result = String.Empty;
            QueryString = new NameValueCollection();

            //Act
            Should.NotThrow(() =>
            {
                result = (string)_privateTestedObject.Invoke(TestedMethodName_CreateNote);
            });
            result.ShouldNotBeNullOrEmpty();
        }
    }
}
