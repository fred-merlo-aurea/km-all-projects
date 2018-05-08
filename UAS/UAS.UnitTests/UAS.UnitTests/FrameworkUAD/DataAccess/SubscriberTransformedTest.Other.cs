using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Core_AMS.Utilities;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class SubscriberTransformedTest
    {
        [Test]
        public void AddressValidation_Paging_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.AddressValidation_Paging(CurrentPage, PageSize, ProcessCode, Client, true, SourceFileID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamCurrentPage].Value.ShouldBe(CurrentPage),
                () => _sqlCommand.Parameters[ParamPageSize].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcAddressValidationPaging),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void CountAddressValidation_WhenCalledWithSourceFileId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.CountAddressValidation(Client, SourceFileID, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcCountAddressValidationSourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void CountAddressValidation_WhenCalledWithProcessCode_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.CountAddressValidation(Client, ProcessCode, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcCountAddressValidationProcessCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void CountAddressValidation_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.CountAddressValidation(Client, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcCountAddressValidation),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void CountForGeoCoding_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.CountForGeoCoding(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcCountForGeoCoding),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void CountForGeoCoding_WhenCalledWithSourceFileId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.CountForGeoCoding(Client, SourceFileID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcCountForGeoCodingSourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetDistinctPubCodes_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<string> { null, null };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return new List<string> { "1", "2" }.GetSqlDataReader();
            };

            // Act
            var result = SubscriberTransformed.GetDistinctPubCodes(Client, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetDistinctPubCodes),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => result.ShouldNotBeNull(),
                () => result.ShouldBe(expectedList));
        }

        [Test]
        public void AddressUpdateBulkSql_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<Entity.SubscriberTransformed>();
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (Entity.SubscriberTransformed x in expectedList)
            {
                xml.AppendLine("<Subscriber>");

                xml.AppendLine($"<SORecordIdentifier>{x.SORecordIdentifier}</SORecordIdentifier>");
                xml.AppendLine($"<Address>{StringFunctions.CleanXMLString(x.Address.Trim())}</Address>");
                xml.AppendLine($"<MailStop>{StringFunctions.CleanXMLString(x.MailStop.Trim())}</MailStop>");
                xml.AppendLine($"<City>{StringFunctions.CleanXMLString(x.City.Trim())}</City>");
                xml.AppendLine($"<State>{StringFunctions.CleanXMLString(x.State.Trim())}</State>");
                xml.AppendLine($"<Zip>{StringFunctions.CleanXMLString(x.Zip.Trim())}</Zip>");
                xml.AppendLine($"<Plus4>{StringFunctions.CleanXMLString(x.Plus4.Trim())}</Plus4>");
                xml.AppendLine($"<Latitude>{x.Latitude}</Latitude>");
                xml.AppendLine($"<Longitude>{x.Longitude}</Longitude>");
                xml.AppendLine($"<IsLatLonValid>{x.IsLatLonValid}</IsLatLonValid>");
                xml.AppendLine($"<LatLongMsg>{StringFunctions.CleanXMLString(x.LatLonMsg.Trim())}</LatLongMsg>");
                xml.AppendLine($"<Country>{StringFunctions.CleanXMLString(x.Country.Trim())}</Country>");
                xml.AppendLine($"<County>{StringFunctions.CleanXMLString(x.County.Trim())}</County>");

                xml.AppendLine("</Subscriber>");
            }
            xml.AppendLine("</XML>");

            // Act
            var result = SubscriberTransformed.AddressUpdateBulkSql(expectedList, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamXml].Value.ShouldBe(xml.ToString()),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcAddressUpdateBulkSql),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void StandardRollUpToMaster_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.StandardRollUpToMaster(
                Client,
                SourceFileID,
                ProcessCode,
                true,
                true,
                true,
                true,
                true,
                true,
                true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamMailPermissionOverRide].Value.ShouldBe(MailPermissionOverRide),
                () => _sqlCommand.Parameters[ParamFaxPermissionOverRide].Value.ShouldBe(FaxPermissionOverRide),
                () => _sqlCommand.Parameters[ParamPhonePermissionOverRide].Value.ShouldBe(PhonePermissionOverRide),
                () => _sqlCommand.Parameters[ParamOtherProductsPermissionOverRide].Value.ShouldBe(OtherProductsPermissionOverRide),
                () => _sqlCommand.Parameters[ParamThirdPartyPermissionOverRide].Value.ShouldBe(ThirdPartyPermissionOverRide),
                () => _sqlCommand.Parameters[ParamEmailRenewPermissionOverRide].Value.ShouldBe(EmailRenewPermissionOverRide),
                () => _sqlCommand.Parameters[ParamTextPermissionOverRide].Value.ShouldBe(TextPermissionOverRide),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcStandardRollUpToMaster),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void StandardRollUpToMaster_WhenCalledWithTextPermission_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.StandardRollUpToMaster(
                Client,
                SourceFileID,
                ProcessCode,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                false,
                false,
                false,
                false);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamMailPermissionOverRide].Value.ShouldBe(MailPermissionOverRide),
                () => _sqlCommand.Parameters[ParamFaxPermissionOverRide].Value.ShouldBe(FaxPermissionOverRide),
                () => _sqlCommand.Parameters[ParamPhonePermissionOverRide].Value.ShouldBe(PhonePermissionOverRide),
                () => _sqlCommand.Parameters[ParamOtherProductsPermissionOverRide].Value.ShouldBe(OtherProductsPermissionOverRide),
                () => _sqlCommand.Parameters[ParamThirdPartyPermissionOverRide].Value.ShouldBe(ThirdPartyPermissionOverRide),
                () => _sqlCommand.Parameters[ParamEmailRenewPermissionOverRide].Value.ShouldBe(EmailRenewPermissionOverRide),
                () => _sqlCommand.Parameters[ParamTextPermissionOverRide].Value.ShouldBe(TextPermissionOverRide),
                () => _sqlCommand.Parameters[ParamUpdateEmail].Value.ShouldBe(UpdateEmail),
                () => _sqlCommand.Parameters[ParamUpdatePhone].Value.ShouldBe(UpdatePhone),
                () => _sqlCommand.Parameters[ParamUpdateFax].Value.ShouldBe(UpdateFax),
                () => _sqlCommand.Parameters[ParamUpdateMobile].Value.ShouldBe(UpdateMobile),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcStandardRollUpToMaster),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DataMatching_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.DataMatching(Client, SourceFileID, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDataMatching),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SequenceDataMatching_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SequenceDataMatching(Client, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSequenceDataMatching),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DataMatching_multiple_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.DataMatching_multiple(Client, SourceFileId, ProcessCode, MatchFields);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamMatchFields].Value.ShouldBe(MatchFields),
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDataMatchingMultiple),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DataMatching_single_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.DataMatching_single(Client, ProcessCode, MatchField);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamMatchField].Value.ShouldBe(MatchField),
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDataMatchingSingle),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void AddressValidExisting_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.AddressValidExisting(Client, SourceFileID, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcAddressValidExisting),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void RevertXmlFormattingAfterBulkInsert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.RevertXmlFormattingAfterBulkInsert(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcRevertXmlFormattingAfterBulkInsert),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}