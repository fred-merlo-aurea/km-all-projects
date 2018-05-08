using System;
using System.Diagnostics.CodeAnalysis;
using ecn.webservice.CustomAPI;
using ecn.webservice.CustomAPI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Webservice.Tests.CustomAPI
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.webservice.CustomAPI.SaversAPI"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class SaversAPITest
    {
        private IDisposable _shimContext;
        private PrivateObject _saversAPIPrivateObject;
        private SaversAPI _saversAPIInstance;
        private ShimSaversAPI _shimSaversAPI;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _saversAPIInstance = new SaversAPI();
            _shimSaversAPI = new ShimSaversAPI(_saversAPIInstance);
            _saversAPIPrivateObject = new PrivateObject(_saversAPIInstance);
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }
    }
}
