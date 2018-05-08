using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkUAD.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="Product"/>
    /// </summary>
    [TestFixture]
    public partial class ProductTest
    {
        private const int Rows = 5;
        private const int PubTypeId = 0;
        private const int PubId = 2;
        private const string PubCode = "pub-code";
        private const int BrandId = 6;
        private const int FromId = 9;
        private const int ToId = 10;
        private const int UserId = 11;
        private const string ProcExistsByPubTypeId = "e_Product_Exists_ByPubTypeID";
        private const string ProcSelect = "e_Product_Select_PubID";
        private const string ProcSelectPubCode = "e_Product_Select_PubCode";
        private const string ProcCopy = "e_Product_Copy";
        private const string ProcUpdateLock = "e_Publication_UpdateLock";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.Product> _list;
        private Entity.Product _objWithRandomValues;
        private Entity.Product _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void ExistsByPubTypeID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Product.ExistsByPubTypeID(PubTypeId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubTypeID"].Value.ShouldBe(PubTypeId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByPubTypeId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Product.Select(Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void SelectPubId_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Product.Select(PubId, Client, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPubCode_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Product.Select(PubCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubCode"].Value.ShouldBe(PubCode),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPubCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBrandID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Product.SelectBrandID(Client, BrandId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Product.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void Copy_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Product.Copy(Client, FromId, ToId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FromPubID"].Value.ShouldBe(FromId),
                () => _sqlCommand.Parameters["@ToPubID"].Value.ShouldBe(ToId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcCopy),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateLock_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Product.UpdateLock(Client, UserId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateLock),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}