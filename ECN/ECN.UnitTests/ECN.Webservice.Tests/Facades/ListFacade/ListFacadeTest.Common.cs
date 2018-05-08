using System;
using ecn.webservice;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Webservice.Tests.Facades.ListFacade
{
    [TestFixture]
    public partial class ListFacadeTest
    {
        private ecn.webservice.Facades.ListFacade _facade;
        private WebMethodExecutionContext _context;
        private IDisposable _shims;
        private const string Name = "listFacade";
        private const int Id = 10;

        [SetUp]
        public void Setup()
        {
            _facade = new ecn.webservice.Facades.ListFacade();
            _context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = Id, UserID = Id },
                ApiLogId = Id,
                MethodName = Name,
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
