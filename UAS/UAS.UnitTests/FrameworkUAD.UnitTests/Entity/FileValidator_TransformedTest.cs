using System;
using Core_AMS.Utilities;
using FrameworkUAD.Entity;
using KM.Common.Functions;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class FileValidator_TransformedTest
    {
        private const string DummyStringValue = "DummyString";
        private const string DummyPhoneNumber = "9898989898";
        private const int DummyIntValue = 10;
        private const string StatusUpdatedReasonDefaultValue = "Subscribed";

        [Test]
        public void FileValidatorTransformed_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var fileValidatorTransformed = new FileValidator_Transformed();

            // Assert
            fileValidatorTransformed.ShouldSatisfyAllConditions(
                () => fileValidatorTransformed.SourceFileID.ShouldBe(-1),
                () => fileValidatorTransformed.ProcessCode.ShouldBe(string.Empty));

            CommonPropertyDefaultValueAssert(fileValidatorTransformed);
        }

        [Test]
        public void FileValidatorTransformed_ParameterCtorSetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var fileValidatorTransformed = new FileValidator_Transformed(DummyIntValue, DummyStringValue);

            // Assert
            fileValidatorTransformed.ShouldSatisfyAllConditions(
                () => fileValidatorTransformed.SourceFileID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.ProcessCode.ShouldBe(DummyStringValue));

            CommonPropertyDefaultValueAssert(fileValidatorTransformed);
        }

        [Test]
        public void FileValidatorTransformed_SetAndGetValue_ReturnsSetValue()
        {
            // Arrange
            var dummyDate = DateTime.Now.Date;
            var dummyGuidValue = Guid.NewGuid();

            // Act
            var fileValidatorTransformed = new FileValidator_Transformed
            {
                SourceFileID = DummyIntValue,
                PubCode = DummyStringValue,
                Sequence = DummyIntValue,
                FName = DummyStringValue,
                LName = DummyStringValue,
                Company = DummyStringValue,
                Address = DummyStringValue,
                MailStop = DummyStringValue,
                City = DummyStringValue,
                State = DummyStringValue,
                Zip = DummyStringValue,
                Plus4 = DummyStringValue,
                ForZip = DummyStringValue,
                County = DummyStringValue,
                Country = DummyStringValue,
                CountryID = DummyIntValue,
                Phone = DummyPhoneNumber,
                PhoneExists = true,
                Fax = DummyPhoneNumber,
                FaxExists = true,
                Email = DummyStringValue,
                EmailExists = true,
                CategoryID = DummyIntValue,
                TransactionID = DummyIntValue,
                TransactionDate = dummyDate,
                QDate = dummyDate,
                QSourceID = DummyIntValue,
                RegCode = DummyStringValue,
                Verified = DummyStringValue,
                SubSrc = DummyStringValue,
                OrigsSrc = DummyStringValue,
                Par3C = DummyStringValue,
                Source = DummyStringValue,
                Priority = DummyStringValue,
                IGrp_No = dummyGuidValue,
                IGrp_Cnt = DummyIntValue,
                CGrp_No = dummyGuidValue,
                CGrp_Cnt = DummyIntValue,
                StatList = true,
                Sic = DummyStringValue,
                SicCode = DummyStringValue,
                Gender = DummyStringValue,
                IGrp_Rank = DummyStringValue,
                CGrp_Rank = DummyStringValue,
                Address3 = DummyStringValue,
                Home_Work_Address = DummyStringValue,
                Demo7 = DummyStringValue,
                Mobile = DummyPhoneNumber,
                Latitude = DummyIntValue,
                Longitude = DummyIntValue,
                IsLatLonValid = true,
                LatLonMsg = DummyStringValue,
                DateCreated = dummyDate,
                DateUpdated = dummyDate,
                CreatedByUserID = DummyIntValue,
                UpdatedByUserID = DummyIntValue,
                ProcessCode = DummyStringValue
            };

            // Assert
            var expectedPhoneNumber = StringFunctions.FormatPhoneNumbersOnly(DummyPhoneNumber);

            fileValidatorTransformed.ShouldSatisfyAllConditions(
                () => fileValidatorTransformed.SourceFileID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.PubCode.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Sequence.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.FName.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.LName.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Company.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Address.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.MailStop.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.City.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.State.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Zip.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Plus4.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.ForZip.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.County.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Country.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.CountryID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.Phone.ShouldBe(expectedPhoneNumber),
                () => fileValidatorTransformed.PhoneExists.ShouldBeTrue(),
                () => fileValidatorTransformed.Fax.ShouldBe(expectedPhoneNumber),
                () => fileValidatorTransformed.FaxExists.ShouldBeTrue(),
                () => fileValidatorTransformed.Email.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.EmailExists.ShouldBeTrue(),
                () => fileValidatorTransformed.CategoryID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.TransactionID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.TransactionDate?.Date.ShouldBe(dummyDate),
                () => fileValidatorTransformed.QDate?.Date.ShouldBe(dummyDate),
                () => fileValidatorTransformed.QSourceID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.RegCode.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Verified.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.SubSrc.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.OrigsSrc.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Par3C.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Source.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Priority.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.IGrp_No.ShouldBe(dummyGuidValue),
                () => fileValidatorTransformed.IGrp_Cnt.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.CGrp_No.ShouldBe(dummyGuidValue),
                () => fileValidatorTransformed.CGrp_Cnt.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.StatList.ShouldBeTrue(),
                () => fileValidatorTransformed.Sic.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.SicCode.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Gender.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.IGrp_Rank.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.CGrp_Rank.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Address3.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Home_Work_Address.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Demo7.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.Mobile.ShouldBe(expectedPhoneNumber),
                () => fileValidatorTransformed.Latitude.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.Longitude.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.IsLatLonValid.ShouldBeTrue(),
                () => fileValidatorTransformed.LatLonMsg.ShouldBe(DummyStringValue),
                () => fileValidatorTransformed.DateCreated.Date.ShouldBe(dummyDate),
                () => fileValidatorTransformed.DateUpdated?.ShouldBe(dummyDate),
                () => fileValidatorTransformed.CreatedByUserID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.UpdatedByUserID.ShouldBe(DummyIntValue),
                () => fileValidatorTransformed.ProcessCode.ShouldBe(DummyStringValue));
        }

        public void CommonPropertyDefaultValueAssert(FileValidator_Transformed fileValidatorTransformed)
        {
            fileValidatorTransformed.ShouldSatisfyAllConditions(
                () => fileValidatorTransformed.PubCode.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Sequence.ShouldBe(-1),
                () => fileValidatorTransformed.FName.ShouldBe(string.Empty),
                () => fileValidatorTransformed.LName.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Company.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Address.ShouldBe(string.Empty),
                () => fileValidatorTransformed.MailStop.ShouldBe(string.Empty),
                () => fileValidatorTransformed.City.ShouldBe(string.Empty),
                () => fileValidatorTransformed.State.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Zip.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Plus4.ShouldBe(string.Empty),
                () => fileValidatorTransformed.ForZip.ShouldBe(string.Empty),
                () => fileValidatorTransformed.County.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Country.ShouldBe(string.Empty),
                () => fileValidatorTransformed.CountryID.ShouldBe(-1),
                () => fileValidatorTransformed.Phone.ShouldBe(string.Empty),
                () => fileValidatorTransformed.PhoneExists.ShouldBeFalse(),
                () => fileValidatorTransformed.Fax.ShouldBe(string.Empty),
                () => fileValidatorTransformed.FaxExists.ShouldBeFalse(),
                () => fileValidatorTransformed.Email.ShouldBe(string.Empty),
                () => fileValidatorTransformed.EmailExists.ShouldBeFalse(),
                () => fileValidatorTransformed.CategoryID.ShouldBe(-1),
                () => fileValidatorTransformed.TransactionID.ShouldBe(-1),
                () => fileValidatorTransformed.TransactionDate?.Date.ShouldBe(DateTimeFunctions.GetMinDate()),
                () => fileValidatorTransformed.QDate?.Date.ShouldBe(DateTime.Now.Date),
                () => fileValidatorTransformed.QSourceID.ShouldBe(-1),
                () => fileValidatorTransformed.RegCode.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Verified.ShouldBe(string.Empty),
                () => fileValidatorTransformed.SubSrc.ShouldBe(string.Empty),
                () => fileValidatorTransformed.OrigsSrc.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Par3C.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Source.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Priority.ShouldBe(string.Empty),
                () => fileValidatorTransformed.IGrp_No.ShouldBe(Guid.Empty),
                () => fileValidatorTransformed.IGrp_Cnt.ShouldBe(-1),
                () => fileValidatorTransformed.CGrp_Cnt.ShouldBe(-1),
                () => fileValidatorTransformed.StatList.ShouldBeFalse(),
                () => fileValidatorTransformed.Sic.ShouldBe(string.Empty),
                () => fileValidatorTransformed.SicCode.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Gender.ShouldBe(string.Empty),
                () => fileValidatorTransformed.IGrp_Rank.ShouldBe(string.Empty),
                () => fileValidatorTransformed.CGrp_Rank.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Address3.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Home_Work_Address.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Demo7.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Mobile.ShouldBe(string.Empty),
                () => fileValidatorTransformed.Latitude.ShouldBe(-1),
                () => fileValidatorTransformed.Longitude.ShouldBe(-1),
                () => fileValidatorTransformed.IsLatLonValid.ShouldBeFalse(),
                () => fileValidatorTransformed.LatLonMsg.ShouldBe(string.Empty),
                () => fileValidatorTransformed.DateCreated.Date.ShouldBe(DateTime.Now.Date),
                () => fileValidatorTransformed.DateUpdated?.ShouldBe(DateTimeFunctions.GetMinDate()),
                () => fileValidatorTransformed.CreatedByUserID.ShouldBe(-1),
                () => fileValidatorTransformed.UpdatedByUserID.ShouldBe(-1),
                () => fileValidatorTransformed.FV_TransformedID.ShouldBe(-1),
                () => fileValidatorTransformed.Title.ShouldBe(string.Empty),
                () => fileValidatorTransformed.PubIDs.ShouldBe(string.Empty),
                () => fileValidatorTransformed.IsExcluded.ShouldBeFalse(),
                () => fileValidatorTransformed.Score.ShouldBe(-1),
                () => fileValidatorTransformed.Ignore.ShouldBeFalse(),
                () => fileValidatorTransformed.IsDQMProcessFinished.ShouldBeFalse(),
                () => fileValidatorTransformed.IsUpdatedInLive.ShouldBeFalse(),
                () => fileValidatorTransformed.ImportRowNumber.ShouldBe(-1),
                () => fileValidatorTransformed.SuppressedDate.ShouldBe(DateTimeFunctions.GetMinDate()),
                () => fileValidatorTransformed.StatusUpdatedDate.ShouldBe(DateTimeFunctions.GetMinDate()),
                () => fileValidatorTransformed.DQMProcessDate.ShouldBe(DateTimeFunctions.GetMinDate()),
                () => fileValidatorTransformed.UpdateInLiveDate.ShouldBe(DateTimeFunctions.GetMinDate()),
                () => fileValidatorTransformed.FV_DemographicTransformedList.ShouldNotBeNull(),
                () => fileValidatorTransformed.Demo31.ShouldBeTrue(),
                () => fileValidatorTransformed.Demo32.ShouldBeTrue(),
                () => fileValidatorTransformed.Demo33.ShouldBeTrue(),
                () => fileValidatorTransformed.Demo34.ShouldBeTrue(),
                () => fileValidatorTransformed.Demo35.ShouldBeTrue(),
                () => fileValidatorTransformed.Demo36.ShouldBeTrue(),
                () => fileValidatorTransformed.EmailStatusID.ShouldBe(1),
                () => fileValidatorTransformed.StatusUpdatedReason.ShouldBe(StatusUpdatedReasonDefaultValue),
                () => fileValidatorTransformed.IsMailable.ShouldBeTrue());
        }
    }
}