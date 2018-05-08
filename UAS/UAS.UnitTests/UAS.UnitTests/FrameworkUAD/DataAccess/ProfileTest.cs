using System;
using System.Collections.Generic;
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
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="Profile"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ProfileTest
    {
        private const string CommandText = "e_Profile_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Profile _profile;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _dataTable = new DataTable();
            _objWithRandomValues = typeof(Entity.Profile).CreateInstance();
            _objWithDefaultValues = typeof(Entity.Profile).CreateInstance(true);

            _list = new List<Entity.Profile>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };

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
            _profile = typeof(Entity.Profile).CreateInstance();

            // Act
            Profile.Save(_profile, new ClientConnections());

            // Assert
            _profile.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _profile = typeof(Entity.Profile).CreateInstance(true);

            // Act
            Profile.Save(_profile, new ClientConnections());

            // Assert
            _profile.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@ProfileID"].Value.ShouldBe(_profile.ProfileID),
                () => _saveCommand.Parameters["@FirstName"].Value.ShouldBe(_profile.FirstName),
                () => _saveCommand.Parameters["@LastName"].Value.ShouldBe(_profile.LastName),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe(_profile.Company),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe(_profile.Title),
                () => _saveCommand.Parameters["@Occupation"].Value.ShouldBe(_profile.Occupation),
                () => _saveCommand.Parameters["@AddressTypeID"].Value.ShouldBe(_profile.AddressTypeID),
                () => _saveCommand.Parameters["@Address1"].Value.ShouldBe(_profile.Address1),
                () => _saveCommand.Parameters["@Address2"].Value.ShouldBe(_profile.Address2),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe(_profile.City),
                () => _saveCommand.Parameters["@RegionCode"].Value.ShouldBe(_profile.RegionCode),
                () => _saveCommand.Parameters["@RegionID"].Value.ShouldBe(_profile.RegionID),
                () => _saveCommand.Parameters["@ZipCode"].Value.ShouldBe(_profile.ZipCode),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe(_profile.Plus4),
                () => _saveCommand.Parameters["@CarrierRoute"].Value.ShouldBe(_profile.CarrierRoute),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe(_profile.County),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe(_profile.Country),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_profile.CountryID),
                () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_profile.Latitude),
                () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_profile.Longitude),
                () => _saveCommand.Parameters["@IsAddressValidated"].Value.ShouldBe(_profile.IsAddressValidated),
                () => _saveCommand.Parameters["@AddressValidationDate"].Value.ShouldBe((object)_profile.AddressValidationDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressValidationSource"].Value.ShouldBe(_profile.AddressValidationSource),
                () => _saveCommand.Parameters["@AddressValidationMessage"].Value.ShouldBe(_profile.AddressValidationMessage),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe(_profile.Email),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe(_profile.Phone),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe(_profile.Fax),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe(_profile.Mobile),
                () => _saveCommand.Parameters["@Website"].Value.ShouldBe(_profile.Website),
                () => _saveCommand.Parameters["@Age"].Value.ShouldBe(_profile.Age),
                () => _saveCommand.Parameters["@BirthDate"].Value.ShouldBe(_profile.BirthDate),
                () => _saveCommand.Parameters["@Income"].Value.ShouldBe(_profile.Income),
                () => _saveCommand.Parameters["@Gender"].Value.ShouldBe(_profile.Gender),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_profile.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_profile.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_profile.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_profile.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DatabaseSource"].Value.ShouldBe(_profile.DatabaseSource),
                () => _saveCommand.Parameters["@DatabaseTable"].Value.ShouldBe(_profile.DatabaseTable),
                () => _saveCommand.Parameters["@TableID"].Value.ShouldBe(_profile.TableID));
        }

        private void SetupFakes()
        {
            var connection = new ShimSqlConnection().Instance;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}
