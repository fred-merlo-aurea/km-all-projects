using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Communicator.MVC.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.mvc.Controllers.SubscriberController" />
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberControllerTest
    {
        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private IDisposable _shimObject;
    }
}