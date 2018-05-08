using System;
using System.Fakes;
using KM.Common.Functions;
using Microsoft.QualityTools.Testing.Fakes;

namespace FrameworkUAD.UnitTests.Entity
{
    public class EntityFakes
    {
        private IDisposable _shims;

        protected static readonly string ProcessCodeSample = "process code 111";
        protected static readonly int SourceFileIdSample = 1191;
        protected static readonly int RowNumberSample = 317;
        protected static readonly Guid RecordIdentifierSample = new Guid("{2CAA4AC3-196E-4CCC-8103-760E152F7390}");
        protected static readonly DateTime Now = new DateTime(2018, 01, 01);
        protected static readonly DateTime MinDate = DateTimeFunctions.GetMinDate();

        public void SetupFakes()
        {
            _shims = ShimsContext.Create();
            ShimDateTime.NowGet = () => Now;
        }

        public void ReleaseFakes()
        {
            _shims?.Dispose();
        }
    }
}
