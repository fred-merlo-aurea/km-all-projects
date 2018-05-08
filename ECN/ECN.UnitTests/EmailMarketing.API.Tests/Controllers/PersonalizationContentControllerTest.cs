using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace EmailMarketing.API.Tests.Controllers
{
    /// <summary>
    ///     Unit tests for <see cref="EmailMarketing.API.Controllers.PersonalizationContentController"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class PersonalizationContentControllerTest
    {
        private IDisposable _shimObject;

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
    }
}
