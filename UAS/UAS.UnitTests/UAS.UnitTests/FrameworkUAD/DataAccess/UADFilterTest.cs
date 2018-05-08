using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;
using Enums = FrameworkUAD.BusinessLogic.Enums;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="UADFilter"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UadFilterTest
    {
        private const int Rows = 5;
        private const int UserId = 1;
        private const int PubId = 3;
        private const int BrandId = 4;
        private const int FilterId = 7;
        private const string FilterName = "filter-name";
        private const int FilterCategoryId = 9;
        private const int QuestionCategoryId = 10;
        private const string QuestionName = "question-name";
        private const string FilterQuery = "select f.*, b.BrandID, BrandName from Filters f left outer join brand b on f.brandID = b.BrandID left outer join filtersegmentation fs on f.filterID = fs.FilterID ";
        private const string FilterEndQuery = " and f.IsDeleted=0 and isnull(b.IsDeleted,0) = 0 and fs.filtersegmentationID is null and f.FilterID in (select FilterID from FilterGroup with (nolock) Group by FilterID having COUNT(FilterID) = 1) order by Name ASC";
        private const string FilterIdQuery = "Select * from Filters where FilterID = @FilterID";
        private const string ProcSelectAll = "e_Filters_Select_All";
        private const string ProcGetByUserId = "e_Filters_Select_UserID";
        private const string GetByIdQuery = "select * from Filters where FilterID = @FilterID";
        private const string ProcExistsByFilterName = "e_Filters_Exists_ByFilterName";
        private const string ProcExistsByFilterCategoryId = "e_Filters_Exists_ByFilterCategoryID";
        private const string ProcExistsByQuestionCategoryId = "e_Filters_Exists_ByQuestionCategoryID";
        private const string ProcExistsQuestionName = "e_Filters_Exists_QuestionName";
        private const string ProcSave = "e_Filters_Save";
        private const string ProcDelete = "e_Filters_Delete";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.UADFilter> _list;
        private Entity.UADFilter _objWithRandomValues;
        private Entity.UADFilter _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.UADFilter)
                .CreateInstance();
            _objWithRandomValues.FilterType = Enums.ViewType.CrossProductView.ToString();
            _objWithRandomValues.ViewType = Enums.ViewType.CrossProductView;
            _objWithRandomValues.BrandID = BrandId;
            _objWithRandomValues.PubID = PubId;
            _objWithRandomValues.FilterId = FilterId;
            _objWithRandomValues.FilterCategoryID = FilterCategoryId;
            _objWithRandomValues.QuestionCategoryID = QuestionCategoryId;
            _objWithRandomValues.FilterId = FilterId;
            _objWithRandomValues.CreatedUserID = UserId;

            _objWithDefaultValues = typeof(Entity.UADFilter)
                .CreateInstance(true);

            _list = new List<Entity.UADFilter>
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
        [TestCase(true, true, BrandId, Enums.ViewType.ProductView, " where filterType = 'ProductView' and pubID = @PubID ")]
        [TestCase(true, true, 0, Enums.ViewType.CrossProductView, " where filterType = 'CrossProductView' ")]
        [TestCase(true, true, BrandId, Enums.ViewType.None, " where (Isnull(filterType, '') = 'ConsensusView' or filterType = 'RecencyView') ")]
        [TestCase(true, false, 0, Enums.ViewType.ProductView, " where filterType = 'ProductView' and pubID = @PubID ")]
        [TestCase(true, false, BrandId, Enums.ViewType.RecencyView, " where filterType = 'RecencyView' ")]
        [TestCase(true, false, 0, Enums.ViewType.CrossProductView, " where filterType = 'CrossProductView' ")]
        [TestCase(true, false, BrandId, Enums.ViewType.ConsensusView, " where Isnull(filterType, '') = 'ConsensusView' and Isnull(pubID,0)=0 ")]
        [TestCase(true, false, 0, Enums.ViewType.None, "")]
        [TestCase(false, false, BrandId, Enums.ViewType.None, "")]
        public void GetFilterByUserIDType_WhenCalled_VerifySqlParameters(bool isAdmin, bool isViewMode, int brandId, Enums.ViewType viewType, string script)
        {
            // Arrange, Act
            var result = UADFilter.GetFilterByUserIDType(Client, UserId, viewType, PubId, brandId, isAdmin, isViewMode);
            script = FilterQuery + script;
            if (isViewMode)
            {
                if (brandId > 0)
                {
                    script += " and b.brandID = @BrandID ";
                }
                script += " and f.CreatedUserID = @UserID ";
            }
            else
            {
                if (!isAdmin)
                {
                    script += " and islocked = 0 or (islocked = 1 and f.CreatedUserID = @UserID) ";
                }

                script += brandId > 0 ? " and b.brandID = @BrandID " : " and (b.brandID = 0 or b.brandID is null) ";
            }

            script += FilterEndQuery;

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _sqlCommand.Parameters["@BrandID"].Value.ShouldBe(brandId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(script),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void GetByFilterID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.GetByFilterID(Client, FilterId);
            _objWithDefaultValues.ViewType = Enums.ViewType.ConsensusView;
            _objWithDefaultValues.FilterType = Enums.ViewType.ConsensusView.ToString();

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(FilterIdQuery),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void GetAll_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetAll(Client);
            _list.Last().FilterType = Enums.ViewType.ConsensusView.ToString();
            _list.Last().ViewType = Enums.ViewType.ConsensusView;

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectAll),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetNotInBrand_TypeAddedinName_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            AddProductViews();
            _list.ToList().ForEach(x => x.BrandID = 0);
            _list.Last().BrandID = null;

            // Act
            var result = UADFilter.GetNotInBrand_TypeAddedinName(Client);
            _list = _list.Where(x => x.BrandID == 0 || x.BrandID == null).ToList();
            SetProductViews();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetByBrandID_TypeAddedinName_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            AddProductViews();
            _list.ToList().ForEach(x => x.BrandID = BrandId);
            _list.First().BrandID = 0;

            // Act
            var result = UADFilter.GetByBrandID_TypeAddedinName(Client, BrandId);
            _list = _list.Where(x => x.BrandID == BrandId).ToList();
            SetProductViews();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetByBrandID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetByBrandID(Client, BrandId);
            _list = _list.Where(x => x.BrandID == BrandId).ToList();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetInBrand_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetInBrand(Client);
            _list = _list.Where(x => x.BrandID > 0).ToList();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetNotInBrand_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetNotInBrand(Client);
            _list = _list.Where(x => x.BrandID == 0 || x.BrandID == null).ToList();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetByUserID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.GetByUserID(Client, UserId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetByUserId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetInBrandByUserID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetInBrandByUserID(Client, UserId);
            _list = _list.Where(x => x.BrandID > 0).ToList();

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetByUserId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetByBrandIDUserID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetByBrandIDUserID(Client, BrandId, UserId);
            _list = _list.Where(x => x.BrandID == BrandId).ToList();

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetByUserId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetNotInBrandByUserID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = UADFilter.GetNotInBrandByUserID(Client, UserId);
            _list = _list.Where(x => x.BrandID == 0 || x.BrandID == null).ToList();

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetByUserId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetByID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.GetByID(Client, FilterId);
            _objWithDefaultValues.ViewType = Enums.ViewType.ConsensusView;
            _objWithDefaultValues.FilterType = Enums.ViewType.ConsensusView.ToString();

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(GetByIdQuery),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void ExistsByFilterName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.ExistsByFilterName(Client, FilterId, FilterName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterName"].Value.ShouldBe(FilterName),
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByFilterName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ExistsByFilterCategoryID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.ExistsByFilterCategoryID(Client, FilterCategoryId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@filterCategoryID"].Value.ShouldBe(FilterCategoryId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByFilterCategoryId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ExistsByQuestionCategoryID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.ExistsByQuestionCategoryID(Client, QuestionCategoryId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@questionCategoryID"].Value.ShouldBe(QuestionCategoryId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByQuestionCategoryId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ExistsQuestionName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.ExistsQuestionName(Client, FilterId, QuestionName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@QuestionName"].Value.ShouldBe(QuestionName),
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsQuestionName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Insert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = UADFilter.insert(Client, _objWithRandomValues);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(_objWithRandomValues.FilterId),
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(_objWithRandomValues.Name),
                () => _sqlCommand.Parameters["@Notes"].Value.ShouldBe(_objWithRandomValues.Notes),
                () => _sqlCommand.Parameters["@FilterXML"].Value.ShouldBe(_objWithRandomValues.FilterXML),
                () => _sqlCommand.Parameters["@FilterType"].Value.ShouldBe(_objWithRandomValues.FilterType),
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(_objWithRandomValues.PubID),
                () => _sqlCommand.Parameters["@BrandID"].Value.ShouldBe(_objWithRandomValues.BrandID),
                () => _sqlCommand.Parameters["@FilterCategoryID"].Value.ShouldBe(_objWithRandomValues.FilterCategoryID),
                () => _sqlCommand.Parameters["@AddtoSalesView"].Value.ShouldBe(_objWithRandomValues.AddtoSalesView),
                () => _sqlCommand.Parameters["@QuestionName"].Value.ShouldBe(_objWithRandomValues.QuestionName),
                () => _sqlCommand.Parameters["@QuestionCategoryID"].Value.ShouldBe(_objWithRandomValues.QuestionCategoryID),
                () => _sqlCommand.Parameters["@IsDeleted"].Value.ShouldBe(_objWithRandomValues.IsDeleted),
                () => _sqlCommand.Parameters["@UpdatedUserID"].Value.ShouldBe(_objWithRandomValues.UpdatedUserID),
                () => _sqlCommand.Parameters["@UpdatedDate"].Value.ShouldBe(_objWithRandomValues.UpdatedDate),
                () => _sqlCommand.Parameters["@CreatedUserID"].Value.ShouldBe(_objWithRandomValues.CreatedUserID),
                () => _sqlCommand.Parameters["@CreatedDate"].Value.ShouldBe(_objWithRandomValues.CreatedDate),
                () => _sqlCommand.Parameters["@IsLocked"].Value.ShouldBe(_objWithRandomValues.IsLocked),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcSave),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Delete_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            UADFilter.Delete(Client, FilterId, UserId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.CommandText.ShouldBe(ProcDelete),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetProductViews()
        {
            _list = _list.Select(filter => new Entity.UADFilter
            {
                FilterId = filter.FilterId,
                Name =
                    filter.FilterType == "ProductView"
                        ? "[Product] " + filter.Name
                        : filter.FilterType == "RecencyView"
                            ? "[Recency] " + filter.Name
                            : filter.FilterType == "CrossProductView"
                                ? "[CrossProduct] " + filter.Name
                                : "[Consensus] " + filter.Name
            }).ToList();
        }

        private void AddProductViews()
        {
            _list.Add(new Entity.UADFilter
            {
                ViewType = Enums.ViewType.ProductView,
                FilterType = Enums.ViewType.ProductView.ToString()
            });

            _list.Add(new Entity.UADFilter
            {
                ViewType = Enums.ViewType.RecencyView,
                FilterType = Enums.ViewType.RecencyView.ToString()
            });

            _list.Add(new Entity.UADFilter
            {
                ViewType = Enums.ViewType.CrossProductView,
                FilterType = Enums.ViewType.CrossProductView.ToString()
            });
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
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

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimSqlCommand.AllInstances.ExecuteReader = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}