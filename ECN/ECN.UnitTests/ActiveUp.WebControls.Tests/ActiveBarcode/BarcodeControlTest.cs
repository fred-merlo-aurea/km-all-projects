using System;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveBarcode
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BarcodeControlTest
    {
        private Barcode _barcode;
        private PrivateObject _privateObject;
        private IDisposable _shims;
        private const string Lic = "lic";
        private const string Pcn = "pcn";
        private const string Date = "date";
        private const string DateGet = "03/24/2018";
        private const string Quantity = "quan";
        private const string LicPcn = "BACD";
        private const string HibcPcn = "hibcPcn";
        private const string Hibc = "hibc";
        private const string LicGet = "BCCC";
        private const string MethodName = "GenerateBarcodeHIBC";
        private const string NullValue = "null";
        private const string Serial = "serial";
        private const string Number = "2";

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void GenerateBarcodeHIBC_ForAllValues_ReturnBitMap()
        {
            // Arrange
            using (_barcode = new Barcode())
            {
                _privateObject = new PrivateObject(_barcode);
                SetUp(NullValue);

                // Act
                var bitMap = _privateObject.Invoke(MethodName) as Bitmap;

                // Assert
                bitMap.ShouldNotBeNull();
                bitMap.ShouldSatisfyAllConditions(
                    () => bitMap.Height.ShouldBe(3),
                    () => bitMap.Width.ShouldBe(176),
                    () => bitMap.VerticalResolution.ShouldBe(96));
            }
        }

        [Test]
        [TestCase(Date)]
        [TestCase(Quantity)]
        [TestCase(LicGet)]
        [TestCase(Hibc)]
        [TestCase(HibcPcn)]
        public void GenerateBarcodeHIBC_ForInvalidData_ThrowException(string param)
        {
            // Arrange
            using (_barcode = new Barcode())
            {
                _privateObject = new PrivateObject(_barcode);
                SetUp(param);
                // Act
                try
                {
                    _privateObject.Invoke(MethodName);
                }
                catch (Exception ex)
                {
                    NUnit.Framework.Assert.IsTrue(ex is Exception);
                }
            }
        }

        protected void SetUp(string param)
        {
            _shims = ShimsContext.Create();
            if (param.Equals(Date))
            {
                ShimSecondaryData.AllInstances.DateGet = (x) => Date;
            }
            else 
            {
                ShimSecondaryData.AllInstances.DateGet = (x) => DateGet;
            }
            if (param.Equals(Quantity))
            {
                ShimSecondaryData.AllInstances.QuantityGet = (x) => Quantity;
            }
            if (param.Equals(LicGet))
            {
                ShimBarcode.AllInstances.LicGs1Get = (x) => Lic;
            }
            else
            {
                ShimBarcode.AllInstances.LicGs1Get = (x) => LicPcn;
            }
            if (param.Equals(Hibc))
            {
                ShimBarcode.AllInstances.HIBCTypeGet = (x) => HIBCType.GS1;
                ShimBarcode.AllInstances.LicGs1Get = (x) => Lic;
            }
            if (param.Equals(HibcPcn))
            {
                ShimBarcode.AllInstances.HIBCTypeGet = (x) => HIBCType.GS1;
                ShimBarcode.AllInstances.PcnGet = (x) => Pcn;
            }
            ShimBarcode.AllInstances.PcnGet = (x) => LicPcn;
            ShimBarcode.AllInstances.UnitOfMeasureGet = (x) => 5;
            ShimBarcode.AllInstances.PrimaryEncodingGet = (x) => PrimaryEncodingMode.Code128;
            ShimSecondaryData.AllInstances.SecondaryEncodingGet = (x) => SecondaryEncodingMode.Code128;
            ShimSecondaryData.AllInstances.SerialGet = (x) => Serial;
            ShimBarcode.AllInstances.DpiGet = (x) => new Unit(Number);
        }
    }
}
