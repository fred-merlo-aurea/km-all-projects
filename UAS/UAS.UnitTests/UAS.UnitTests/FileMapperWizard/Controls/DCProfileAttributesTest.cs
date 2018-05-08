using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;


namespace UAS.UnitTests.FileMapperWizard.Controls
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class DcProfileAttributesTest
    {
        private IDisposable _context;
        
        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            CheckBoxCheckInit();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}