using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.QualityTools.Testing.Fakes;

using NUnit.Framework;

namespace UAS.UnitTests.FramewrokUAD.BusinessLogic.Common
{
    [ExcludeFromCodeCoverage]
    public class Fake
    {
        private IDisposable _context;

        public void SetupFakes()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }
    }
}
