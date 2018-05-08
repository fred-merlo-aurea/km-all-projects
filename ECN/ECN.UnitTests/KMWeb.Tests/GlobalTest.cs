using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace KMWeb.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="KMWeb.MvcApplication"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class MvcApplicationTest
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
            _errorMessage = null;
            _applicationState.Clear();
            _applicationID = default(int);
            _shimObject.Dispose();
        }
    }
}
