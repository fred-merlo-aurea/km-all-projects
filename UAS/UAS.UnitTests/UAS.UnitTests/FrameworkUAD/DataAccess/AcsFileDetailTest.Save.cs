using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="AcsFileDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsFileDetailsTest
    {
        private const string CommandText = "e_AcsFileDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.AcsFileDetail _detail;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _detail = typeof(Entity.AcsFileDetail).CreateInstance();

            // Act
            AcsFileDetail.Save(_detail, new ClientConnections());

            // Assert
            _detail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _detail = typeof(Entity.AcsFileDetail).CreateInstance(true);

            // Act
            AcsFileDetail.Save(_detail, new ClientConnections());

            // Assert
            _detail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
               () => _saveCommand.Parameters["@AcsFileDetailId"].Value.ShouldBe(_detail.AcsFileDetailId),
               () => _saveCommand.Parameters["@RecordType"].Value.ShouldBe(_detail.RecordType),
               () => _saveCommand.Parameters["@FileVersion"].Value.ShouldBe(_detail.FileVersion),
               () => _saveCommand.Parameters["@SequenceNumber"].Value.ShouldBe(_detail.SequenceNumber),
               () => _saveCommand.Parameters["@AcsMailerId"].Value.ShouldBe(_detail.AcsMailerId),
               () => _saveCommand.Parameters["@KeylineSequenceSerialNumber"].Value.ShouldBe(_detail.KeylineSequenceSerialNumber),
               () => _saveCommand.Parameters["@MoveEffectiveDate"].Value.ShouldBe(_detail.MoveEffectiveDate),
               () => _saveCommand.Parameters["@MoveType"].Value.ShouldBe(_detail.MoveType),
               () => _saveCommand.Parameters["@DeliverabilityCode"].Value.ShouldBe(_detail.DeliverabilityCode),
               () => _saveCommand.Parameters["@UspsSiteID"].Value.ShouldBe(_detail.UspsSiteID),
               () => _saveCommand.Parameters["@LastName"].Value.ShouldBe(_detail.LastName),
               () => _saveCommand.Parameters["@FirstName"].Value.ShouldBe(_detail.FirstName),
               () => _saveCommand.Parameters["@Prefix"].Value.ShouldBe(_detail.Prefix),
               () => _saveCommand.Parameters["@Suffix"].Value.ShouldBe(_detail.Suffix),
               () => _saveCommand.Parameters["@OldAddressType"].Value.ShouldBe(_detail.OldAddressType),
               () => _saveCommand.Parameters["@OldUrb"].Value.ShouldBe(_detail.OldUrb),
               () => _saveCommand.Parameters["@OldPrimaryNumber"].Value.ShouldBe(_detail.OldPrimaryNumber),
               () => _saveCommand.Parameters["@OldPreDirectional"].Value.ShouldBe(_detail.OldPreDirectional),
               () => _saveCommand.Parameters["@OldStreetName"].Value.ShouldBe(_detail.OldStreetName),
               () => _saveCommand.Parameters["@OldSuffix"].Value.ShouldBe(_detail.OldSuffix),
               () => _saveCommand.Parameters["@OldPostDirectional"].Value.ShouldBe(_detail.OldPostDirectional),
               () => _saveCommand.Parameters["@OldUnitDesignator"].Value.ShouldBe(_detail.OldUnitDesignator),
               () => _saveCommand.Parameters["@OldSecondaryNumber"].Value.ShouldBe(_detail.OldSecondaryNumber),
               () => _saveCommand.Parameters["@OldCity"].Value.ShouldBe(_detail.OldCity),
               () => _saveCommand.Parameters["@OldStateAbbreviation"].Value.ShouldBe(_detail.OldStateAbbreviation),
               () => _saveCommand.Parameters["@OldZipCode"].Value.ShouldBe(_detail.OldZipCode),
               () => _saveCommand.Parameters["@NewAddressType"].Value.ShouldBe(_detail.NewAddressType),
               () => _saveCommand.Parameters["@NewPmb"].Value.ShouldBe(_detail.NewPmb),
               () => _saveCommand.Parameters["@NewUrb"].Value.ShouldBe(_detail.NewUrb),
               () => _saveCommand.Parameters["@NewPrimaryNumber"].Value.ShouldBe(_detail.NewPrimaryNumber),
               () => _saveCommand.Parameters["@NewPreDirectional"].Value.ShouldBe(_detail.NewPreDirectional),
               () => _saveCommand.Parameters["@NewStreetName"].Value.ShouldBe(_detail.NewStreetName),
               () => _saveCommand.Parameters["@NewSuffix"].Value.ShouldBe(_detail.NewSuffix),
               () => _saveCommand.Parameters["@NewPostDirectional"].Value.ShouldBe(_detail.NewPostDirectional),
               () => _saveCommand.Parameters["@NewUnitDesignator"].Value.ShouldBe(_detail.NewUnitDesignator),
               () => _saveCommand.Parameters["@NewSecondaryNumber"].Value.ShouldBe(_detail.NewSecondaryNumber),
               () => _saveCommand.Parameters["@NewCity"].Value.ShouldBe(_detail.NewCity),
               () => _saveCommand.Parameters["@NewStateAbbreviation"].Value.ShouldBe(_detail.NewStateAbbreviation),
               () => _saveCommand.Parameters["@NewZipCode"].Value.ShouldBe(_detail.NewZipCode),
               () => _saveCommand.Parameters["@Hyphen"].Value.ShouldBe(_detail.Hyphen),
               () => _saveCommand.Parameters["@NewPlus4Code"].Value.ShouldBe(_detail.NewPlus4Code),
               () => _saveCommand.Parameters["@NewDeliveryPoint"].Value.ShouldBe(_detail.NewDeliveryPoint),
               () => _saveCommand.Parameters["@NewAbbreviatedCityName"].Value.ShouldBe(_detail.NewAbbreviatedCityName),
               () => _saveCommand.Parameters["@NewAddressLabel"].Value.ShouldBe(_detail.NewAddressLabel),
               () => _saveCommand.Parameters["@FeeNotification"].Value.ShouldBe(_detail.FeeNotification),
               () => _saveCommand.Parameters["@NotificationType"].Value.ShouldBe(_detail.NotificationType),
               () => _saveCommand.Parameters["@IntelligentMailBarcode"].Value.ShouldBe(_detail.IntelligentMailBarcode),
               () => _saveCommand.Parameters["@IntelligentMailPackageBarcode"].Value.ShouldBe(_detail.IntelligentMailPackageBarcode),
               () => _saveCommand.Parameters["@IdTag"].Value.ShouldBe(_detail.IdTag),
               () => _saveCommand.Parameters["@HardcopyToElectronicFlag"].Value.ShouldBe(_detail.HardcopyToElectronicFlag),
               () => _saveCommand.Parameters["@TypeOfAcs"].Value.ShouldBe(_detail.TypeOfAcs),
               () => _saveCommand.Parameters["@FulfillmentDate"].Value.ShouldBe(_detail.FulfillmentDate),
               () => _saveCommand.Parameters["@ProcessingType"].Value.ShouldBe(_detail.ProcessingType),
               () => _saveCommand.Parameters["@CaptureType"].Value.ShouldBe(_detail.CaptureType),
               () => _saveCommand.Parameters["@MadeAvailableDate"].Value.ShouldBe(_detail.MadeAvailableDate),
               () => _saveCommand.Parameters["@ShapeOfMail"].Value.ShouldBe(_detail.ShapeOfMail),
               () => _saveCommand.Parameters["@MailActionCode"].Value.ShouldBe(_detail.MailActionCode),
               () => _saveCommand.Parameters["@NixieFlag"].Value.ShouldBe(_detail.NixieFlag),
               () => _saveCommand.Parameters["@ProductCode1"].Value.ShouldBe(_detail.ProductCode1),
               () => _saveCommand.Parameters["@ProductCodeFee1"].Value.ShouldBe(_detail.ProductCodeFee1),
               () => _saveCommand.Parameters["@ProductCode2"].Value.ShouldBe(_detail.ProductCode2),
               () => _saveCommand.Parameters["@ProductCodeFee2"].Value.ShouldBe(_detail.ProductCodeFee2),
               () => _saveCommand.Parameters["@ProductCode3"].Value.ShouldBe(_detail.ProductCode3),
               () => _saveCommand.Parameters["@ProductCodeFee3"].Value.ShouldBe(_detail.ProductCodeFee3),
               () => _saveCommand.Parameters["@ProductCode4"].Value.ShouldBe(_detail.ProductCode4),
               () => _saveCommand.Parameters["@ProductCodeFee4"].Value.ShouldBe(_detail.ProductCodeFee4),
               () => _saveCommand.Parameters["@ProductCode5"].Value.ShouldBe(_detail.ProductCode5),
               () => _saveCommand.Parameters["@ProductCodeFee5"].Value.ShouldBe(_detail.ProductCodeFee5),
               () => _saveCommand.Parameters["@ProductCode6"].Value.ShouldBe(_detail.ProductCode6),
               () => _saveCommand.Parameters["@ProductCodeFee6"].Value.ShouldBe(_detail.ProductCodeFee6),
               () => _saveCommand.Parameters["@Filler"].Value.ShouldBe(_detail.Filler),
               () => _saveCommand.Parameters["@EndMarker"].Value.ShouldBe(_detail.EndMarker),
               () => _saveCommand.Parameters["@ProductCode"].Value.ShouldBe(_detail.ProductCode),
               () => _saveCommand.Parameters["@OldAddress1"].Value.ShouldBe(_detail.OldAddress1),
               () => _saveCommand.Parameters["@OldAddress2"].Value.ShouldBe(_detail.OldAddress2),
               () => _saveCommand.Parameters["@OldAddress3"].Value.ShouldBe(_detail.OldAddress3),
               () => _saveCommand.Parameters["@NewAddress1"].Value.ShouldBe(_detail.NewAddress1),
               () => _saveCommand.Parameters["@NewAddress2"].Value.ShouldBe(_detail.NewAddress2),
               () => _saveCommand.Parameters["@NewAddress3"].Value.ShouldBe(_detail.NewAddress3),
               () => _saveCommand.Parameters["@SequenceID"].Value.ShouldBe(_detail.SequenceID),
               () => _saveCommand.Parameters["@TransactionCodeValue"].Value.ShouldBe(_detail.TransactionCodeValue),
               () => _saveCommand.Parameters["@CategoryCodeValue"].Value.ShouldBe(_detail.CategoryCodeValue),
               () => _saveCommand.Parameters["@IsIgnored"].Value.ShouldBe(_detail.IsIgnored),
               () => _saveCommand.Parameters["@AcsActionId"].Value.ShouldBe(_detail.AcsActionId),
               () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe(_detail.ProcessCode));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}
