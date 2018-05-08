using System;

using Microsoft.QualityTools.Testing.Fakes;

namespace ECN.Framework.Common.Tests.Functions.FileImporter_cs
{
    public class Fakes
    {
        private IDisposable _shims;
        public void SetupFakes()
        {
            _shims = ShimsContext.Create();
        }

        public void DisposeFakes()
        {
            _shims?.Dispose();
        }
    }
}
