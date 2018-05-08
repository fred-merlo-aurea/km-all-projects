using System;
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
    [TestFixture]
    public class AcsFileDetailTest
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

        private string GenerateRandomNumbers(int length)
        {
            var random = new Random();
            var output = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                output.Append(random.Next(0, 10));
            }
            return output.ToString();
        }

        [Test]
        public void Parse_OnEmptyString_ReturnEmptyObject()
        {
            // Arrange
            var detailRecord = string.Empty;
            var client = new ClientConnections();
            var acsFileDetail = new AcsFileDetail();
            var defaultId = 0;
            var defaultDate = DateTime.Now;
            var defaultTime = defaultDate.TimeOfDay;
            ShimDateTime.NowGet = () => defaultDate;

            // Act	
            var actualResult = acsFileDetail.Parse(detailRecord, client);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.RecordType.ShouldBeNullOrEmpty();
            actualResult.CaptureType.ShouldBeNullOrEmpty();
            actualResult.AcsActionId.ShouldBe(defaultId);
            actualResult.AcsFileDetailId.ShouldBe(defaultId);
            actualResult.CategoryCodeValue.ShouldBe(defaultId);
            actualResult.CreatedDate.ShouldBe(defaultDate);
            actualResult.CreatedTime.ShouldBe(defaultTime);
            actualResult.DeliverabilityCode.ShouldBeNullOrEmpty();
            actualResult.EndMarker.ShouldBeNullOrEmpty();
            actualResult.FeeNotification.ShouldBeNullOrEmpty();
            actualResult.FileVersion.ShouldBeNullOrEmpty();
            actualResult.Filler.ShouldBeNullOrEmpty();
            actualResult.FirstName.ShouldBeNullOrEmpty();
            actualResult.FulfillmentDate.ShouldBe(defaultDate);
            actualResult.HardcopyToElectronicFlag.ShouldBeNullOrEmpty();
            actualResult.Hyphen.ShouldBeNullOrEmpty();
            actualResult.IdTag.ShouldBeNullOrEmpty();
            actualResult.IntelligentMailBarcode.ShouldBeNullOrEmpty();
            actualResult.IntelligentMailPackageBarcode.ShouldBeNullOrEmpty();
            actualResult.IsIgnored.ShouldBeFalse();
            actualResult.KeylineSequenceSerialNumber.ShouldBeNullOrEmpty();
            actualResult.LastName.ShouldBeNullOrEmpty();
            actualResult.MadeAvailableDate.ShouldBe(defaultDate);
            actualResult.MailActionCode.ShouldBeNullOrEmpty();
            actualResult.MoveEffectiveDate.ShouldBe(defaultDate);
            actualResult.MoveType.ShouldBeNullOrEmpty();
            actualResult.NewAbbreviatedCityName.ShouldBeNullOrEmpty();
            actualResult.NewAddress1.ShouldBeNullOrEmpty();
            actualResult.NewAddress2.ShouldBeNullOrEmpty();
            actualResult.NewAddress3.ShouldBeNullOrEmpty();
            actualResult.NewAddressLabel.ShouldBeNullOrEmpty();
            actualResult.NewAddressType.ShouldBeNullOrEmpty();
            actualResult.NewCity.ShouldBeNullOrEmpty();
            actualResult.NewDeliveryPoint.ShouldBeNullOrEmpty();
            actualResult.NewPlus4Code.ShouldBeNullOrEmpty();
            actualResult.NewPmb.ShouldBeNullOrEmpty();
            actualResult.NewPostDirectional.ShouldBeNullOrEmpty();
            actualResult.NewPreDirectional.ShouldBeNullOrEmpty();
            actualResult.NewPrimaryNumber.ShouldBeNullOrEmpty();
            actualResult.NewSecondaryNumber.ShouldBeNullOrEmpty();
            actualResult.NewStateAbbreviation.ShouldBeNullOrEmpty();
            actualResult.NewStreetName.ShouldBeNullOrEmpty();
            actualResult.NewSuffix.ShouldBeNullOrEmpty();
            actualResult.NewUnitDesignator.ShouldBeNullOrEmpty();
            actualResult.NewUrb.ShouldBeNullOrEmpty();
            actualResult.NewZipCode.ShouldBeNullOrEmpty();
            actualResult.NixieFlag.ShouldBeNullOrEmpty();
            actualResult.NotificationType.ShouldBeNullOrEmpty();
            actualResult.OldAddress1.ShouldBeNullOrEmpty();
            actualResult.OldAddress2.ShouldBeNullOrEmpty();
            actualResult.OldAddress3.ShouldBeNullOrEmpty();
            actualResult.OldAddressType.ShouldBeNullOrEmpty();
            actualResult.OldCity.ShouldBeNullOrEmpty();
            actualResult.OldPostDirectional.ShouldBeNullOrEmpty();
            actualResult.OldPreDirectional.ShouldBeNullOrEmpty();
            actualResult.OldPrimaryNumber.ShouldBeNullOrEmpty();
            actualResult.OldSecondaryNumber.ShouldBeNullOrEmpty();
            actualResult.OldStateAbbreviation.ShouldBeNullOrEmpty();
            actualResult.OldStreetName.ShouldBeNullOrEmpty();
            actualResult.OldSuffix.ShouldBeNullOrEmpty();
            actualResult.OldUnitDesignator.ShouldBeNullOrEmpty();
            actualResult.OldUrb.ShouldBeNullOrEmpty();
            actualResult.OldZipCode.ShouldBeNullOrEmpty();
            actualResult.Prefix.ShouldBeNullOrEmpty();
            actualResult.ProcessCode.ShouldNotBeNull();
            actualResult.ProcessingType.ShouldBeNullOrEmpty();
            actualResult.ProductCode.ShouldBeNullOrEmpty();
            actualResult.ProductCode1.ShouldBe(defaultId);
            actualResult.ProductCode2.ShouldBe(defaultId);
            actualResult.ProductCode3.ShouldBe(defaultId);
            actualResult.ProductCode4.ShouldBe(defaultId);
            actualResult.ProductCode5.ShouldBe(defaultId);
            actualResult.ProductCode6.ShouldBe(defaultId);
            actualResult.ProductCodeFee1.ShouldBe(defaultId);
            actualResult.ProductCodeFee2.ShouldBe(defaultId);
            actualResult.ProductCodeFee3.ShouldBe(defaultId);
            actualResult.ProductCodeFee4.ShouldBe(defaultId);
            actualResult.ProductCodeFee5.ShouldBe(defaultId);
            actualResult.ProductCodeFee6.ShouldBe(defaultId);
            actualResult.SequenceID.ShouldBe(defaultId);
            actualResult.SequenceNumber.ShouldBe(defaultId);
            actualResult.ShapeOfMail.ShouldBeNullOrEmpty();
            actualResult.Suffix.ShouldBeNullOrEmpty();
            actualResult.TransactionCodeValue.ShouldBe(defaultId);
            actualResult.TypeOfAcs.ShouldBeNullOrEmpty();
            actualResult.UspsSiteID.ShouldBe(defaultId);
        }

        [Test]
        public void Parse_OnNonEmptyString_ReturnAcsFileDetailObject()
        {
            // Arrange
            var detailRecord = GenerateRandomNumbers(700);
            var client = new ClientConnections();
            var acsFileDetail = new AcsFileDetail();
            ShimDateTimeFunctions.ParseDateDateFormatString = (drmt, val) => new DateTime();

            // Act	
            var actualResult = acsFileDetail.Parse(detailRecord, client);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.RecordType.ShouldNotBeNull();
            actualResult.CaptureType.ShouldNotBeNull();
            actualResult.AcsActionId.ShouldNotBeNull();
            actualResult.AcsFileDetailId.ShouldNotBeNull();
            actualResult.CategoryCodeValue.ShouldNotBeNull();
            actualResult.CreatedDate.ShouldNotBeNull();
            actualResult.CreatedTime.ShouldNotBeNull();
            actualResult.DeliverabilityCode.ShouldNotBeNull();
            actualResult.EndMarker.ShouldNotBeNull();
            actualResult.FeeNotification.ShouldNotBeNull();
            actualResult.FileVersion.ShouldNotBeNull();
            actualResult.Filler.ShouldNotBeNull();
            actualResult.FirstName.ShouldNotBeNull();
            actualResult.FulfillmentDate.ShouldNotBeNull();
            actualResult.HardcopyToElectronicFlag.ShouldNotBeNull();
            actualResult.Hyphen.ShouldNotBeNull();
            actualResult.IdTag.ShouldNotBeNull();
            actualResult.IntelligentMailBarcode.ShouldNotBeNull();
            actualResult.IntelligentMailPackageBarcode.ShouldNotBeNull();
            actualResult.IsIgnored.ShouldNotBeNull();
            actualResult.KeylineSequenceSerialNumber.ShouldNotBeNull();
            actualResult.LastName.ShouldNotBeNull();
            actualResult.MadeAvailableDate.ShouldNotBeNull();
            actualResult.MailActionCode.ShouldNotBeNull();
            actualResult.MoveEffectiveDate.ShouldNotBeNull();
            actualResult.MoveType.ShouldNotBeNull();
            actualResult.NewAbbreviatedCityName.ShouldNotBeNull();
            actualResult.NewAddress1.ShouldNotBeNull();
            actualResult.NewAddress2.ShouldNotBeNull();
            actualResult.NewAddress3.ShouldNotBeNull();
            actualResult.NewAddressLabel.ShouldNotBeNull();
            actualResult.NewAddressType.ShouldNotBeNull();
            actualResult.NewCity.ShouldNotBeNull();
            actualResult.NewDeliveryPoint.ShouldNotBeNull();
            actualResult.NewPlus4Code.ShouldNotBeNull();
            actualResult.NewPmb.ShouldNotBeNull();
            actualResult.NewPostDirectional.ShouldNotBeNull();
            actualResult.NewPreDirectional.ShouldNotBeNull();
            actualResult.NewPrimaryNumber.ShouldNotBeNull();
            actualResult.NewSecondaryNumber.ShouldNotBeNull();
            actualResult.NewStateAbbreviation.ShouldNotBeNull();
            actualResult.NewStreetName.ShouldNotBeNull();
            actualResult.NewSuffix.ShouldNotBeNull();
            actualResult.NewUnitDesignator.ShouldNotBeNull();
            actualResult.NewUrb.ShouldNotBeNull();
            actualResult.NewZipCode.ShouldNotBeNull();
            actualResult.NixieFlag.ShouldNotBeNull();
            actualResult.NotificationType.ShouldNotBeNull();
            actualResult.OldAddress1.ShouldNotBeNull();
            actualResult.OldAddress2.ShouldNotBeNull();
            actualResult.OldAddress3.ShouldNotBeNull();
            actualResult.OldAddressType.ShouldNotBeNull();
            actualResult.OldCity.ShouldNotBeNull();
            actualResult.OldPostDirectional.ShouldNotBeNull();
            actualResult.OldPreDirectional.ShouldNotBeNull();
            actualResult.OldPrimaryNumber.ShouldNotBeNull();
            actualResult.OldSecondaryNumber.ShouldNotBeNull();
            actualResult.OldStateAbbreviation.ShouldNotBeNull();
            actualResult.OldStreetName.ShouldNotBeNull();
            actualResult.OldSuffix.ShouldNotBeNull();
            actualResult.OldUnitDesignator.ShouldNotBeNull();
            actualResult.OldUrb.ShouldNotBeNull();
            actualResult.OldZipCode.ShouldNotBeNull();
            actualResult.Prefix.ShouldNotBeNull();
            actualResult.ProcessCode.ShouldNotBeNull();
            actualResult.ProcessingType.ShouldNotBeNull();
            actualResult.ProductCode.ShouldNotBeNull();
            actualResult.ProductCode1.ShouldNotBeNull();
            actualResult.ProductCode2.ShouldNotBeNull();
            actualResult.ProductCode3.ShouldNotBeNull();
            actualResult.ProductCode4.ShouldNotBeNull();
            actualResult.ProductCode5.ShouldNotBeNull();
            actualResult.ProductCode6.ShouldNotBeNull();
            actualResult.ProductCodeFee1.ShouldNotBeNull();
            actualResult.ProductCodeFee2.ShouldNotBeNull();
            actualResult.ProductCodeFee3.ShouldNotBeNull();
            actualResult.ProductCodeFee4.ShouldNotBeNull();
            actualResult.ProductCodeFee5.ShouldNotBeNull();
            actualResult.ProductCodeFee6.ShouldNotBeNull();
            actualResult.SequenceID.ShouldNotBeNull();
            actualResult.SequenceNumber.ShouldNotBeNull();
            actualResult.ShapeOfMail.ShouldNotBeNull();
            actualResult.Suffix.ShouldNotBeNull();
            actualResult.TransactionCodeValue.ShouldNotBeNull();
            actualResult.TypeOfAcs.ShouldNotBeNull();
            actualResult.UspsSiteID.ShouldNotBeNull();
        }
    }
}
