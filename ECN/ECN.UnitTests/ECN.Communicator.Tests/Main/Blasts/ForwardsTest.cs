using System.Diagnostics.CodeAnalysis;
using Shouldly;
using NUnit.Framework;
using ecn.communicator.blastsmanager;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="forwards"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ForwardsTest : BaseBlastsTest<forwards>
    {
        [Test]
        public void GetUdfName_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFName, UDFNameValue);

            // Act, Assert
            testObject.getUDFName().ShouldBe(UDFNameValue);
        }

        [Test]
        public void GetUdfName_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFName().ShouldBeEmpty();
        }

        [Test]
        public void GetUdfData_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFdata, UDFdataValue);

            // Act, Assert
            testObject.getUDFData().ShouldBe(UDFdataValue);
        }

        [Test]
        public void GetUdfData_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFData().ShouldBeEmpty();
        }
    }
}
