using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ecn.common.classes;
using ecn.common.classes.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Common
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BaseChannelTest
    {
        private const string BaseChannelValue = "BaseChannelValue";
        private readonly string[] QueryParameters = new string[]
        {
              "@name,",
              "@channelPartnerType,",
              "@ratesXML,",
              "@salutation,",
              "@contactName,",
              "@contactTitle,",
              "@phone,",
              "@fax,",
              "@email,",
              "@streetAddress,",
              "@city,",
              "@state,",
              "@country,",
              "@zip,",
              "@bounceDomain,",
              "@channelURL,",
              "@webAddress,",
              "@AccessCommunicator,",
              "@AccessCreator,",
              "@AccessCollector,",
              "@AccessCharity,",
              "@AccessPublisher,",
              "@channelType"
        };

        private SqlCommand _sqlCommand;
        private IDisposable _context;
        private PrivateObject _baseChannelPrivateObject;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp() => _context.Dispose();
    }
}
