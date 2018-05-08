using System;
using System.Diagnostics.CodeAnalysis;
using ecn.webservice;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Webservice.Tests.Facades.ContentFacade
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ContentFacadeTest
    {
        private ecn.webservice.Facades.ContentFacade _facade;
        private WebMethodExecutionContext _context;
        private IDisposable _shims;
        private const string MethodName = "contentFacade";
        private const int Id = 10;

        [SetUp]
        public void Setup()
        {
            _facade = new ecn.webservice.Facades.ContentFacade();
            _context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = Id, UserID = Id },
                ApiLogId = Id,
                MethodName = MethodName,
                ApiLoggingManager = new APILoggingManager()
            };
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }
    }
}
