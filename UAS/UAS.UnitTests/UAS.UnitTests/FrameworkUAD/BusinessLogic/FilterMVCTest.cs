using System;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Text;
using FrameworkUAD.BusinessLogic;
using KM.Common.Functions.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    /// <summary>
    ///     Unit tests for <see cref="FilterMVC"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FilterMVCTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
