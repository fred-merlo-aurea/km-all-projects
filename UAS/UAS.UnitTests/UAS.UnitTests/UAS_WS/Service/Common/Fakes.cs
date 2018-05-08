using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;
using UAS_WS.Fakes;
using WebServiceFramework.Fakes;
using ShimApiLog = KMPlatform.BusinessLogic.Fakes.ShimApiLog;

namespace UAS.UnitTests.UAS_WS.Service.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes : WebServiceTestBase
    {
        private IDisposable _context;
 
        public void SetupFakes()
        {
            _context = ShimsContext.Create();
            ShimApiLog.AllInstances.SaveApiLog = (_, __) => 0;
            ShimServiceBase.GetCallingIp = () => string.Empty;
            ShimFrameworkServiceBase.GetCallingIp = () => string.Empty;
        }
 
        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }
    }
}