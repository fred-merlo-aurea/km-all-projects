using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace UAS.UnitTests.ControlCenter.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        private IDisposable _context;

        public void SetupFakes()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context?.Dispose();
        }

    }
}
