using System;
using System.Diagnostics.CodeAnalysis;
using KMPS.MD.Helpers;
using KMPS.MD.Objects;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace KMPS.MD.Tests.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class AdhocHelperTests
    {
        private const string SampleFieldValue = "FieldValueSample";

        private AdhocDataListItemControlSet _controlSet;
        private Field _field;
        private bool _drpAdhocSearchSelectedSetCalled;
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            _field = new Field();
            _controlSet = AdhocDataListItemControlSetTests.CreateNonEmptyControlSet();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }
    }
}