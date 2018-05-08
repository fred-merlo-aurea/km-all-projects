using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.Web.Controllers.FileMapperWizard;
using UAS.Web.Models.FileMapperWizard;

namespace UAS.Web.Tests.Controllers.FileMapperWizard
{
    /// <summary>
    ///     Unit tests for <see cref="FileMapperWizardController.LoadTransformationDetail"/>
    /// </summary>
    public partial class FileMapperWizardControllerTest
    {
        private static string[] _delimeters = {":", ",", ";", "\t", "~", "|"};

        [Test]
        public void LoadTransformationDetail_NullAssignTransformWithIDGreaterThanZero_EmptyAssignTransformMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = AssignValue;
            var expectedView = "Partials/Transformation/_transformationAssignValue";
            _assignTransforms = null;

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            _products = new List<Product>
            {
                product
            };

            var expectedModel = new TransformationAssignModel(new List<TransformAssignModel>(), _products);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyTransformationAssignModel(result.Model as TransformationAssignModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullAssignTransformWithIDLessThanEqualToZero_EmptyAssignTransformMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = AssignValue;
            var expectedView = "Partials/Transformation/_transformationAssignValue";
            _assignTransforms = null;

            Product product = typeof(Product).CreateInstance();
            product.IsActive = false;
            _products = new List<Product>
            {
                product
            };

            _products = _products.Where(x => x.IsActive).ToList();

            var expectedModel = new TransformationAssignModel(new List<TransformAssignModel>(), _products, true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, 0);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyTransformationAssignModel(result.Model as TransformationAssignModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_AssignTransform_VerifyAssignTransformMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = AssignValue;
            var expectedView = "Partials/Transformation/_transformationAssignValue";
            _assignTransforms = new List<TransformAssign>
            {
                new TransformAssign
                {
                    TransformationID = TransformationId,
                    Value = "0",
                    PubID = 0,
                    TransformAssignID = 1
                },
                new TransformAssign
                {
                    TransformationID = TransformationId,
                    Value = "1",
                    PubID = 0,
                    TransformAssignID = 2
                },
                new TransformAssign
                {
                    TransformationID = TransformationId,
                    Value = "1",
                    PubID = PubId,
                    TransformAssignID = 3
                }
            };

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            product.PubID = PubId;
            product.PubCode = "pub-code";

            _products = new List<Product>
            {
                product
            };

            var assignMaps = new List<TransformAssignModel>
            {
                new TransformAssignModel(0, "1", 0, new List<int> {0}, "ALL PRODUCTS", "0", _products),
                new TransformAssignModel(0, "2,3", 1, new List<int> {0, PubId}, "pub-code", "1", _products)
            };

            var expectedModel = new TransformationAssignModel(assignMaps, _products, true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyTransformationAssignModel(result.Model as TransformationAssignModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullDataMapTransformWithIDGreaterThanZero_EmptyDataTransformsMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = DataMapping;
            var expectedView = "Partials/Transformation/_transformationChangeValue";
            _dataMapTransforms = null;

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            _products = new List<Product>
            {
                product
            };

            var expectedModel = new TransformationChangeValueModel(new List<TransformDataMapModel>(), _products);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyChangeValueModel(result.Model as TransformationChangeValueModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullAssignTransformWithIDLessThanEqualToZero_EmptyDataTransformsMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = DataMapping;
            var expectedView = "Partials/Transformation/_transformationChangeValue";
            _dataMapTransforms = null;

            Product product = typeof(Product).CreateInstance();
            product.IsActive = false;
            _products = new List<Product>
            {
                product
            };

            _products = _products.Where(x => x.IsActive).ToList();

            var expectedModel = new TransformationChangeValueModel(new List<TransformDataMapModel>(), _products, true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, 0);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyChangeValueModel(result.Model as TransformationChangeValueModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_DataTransform_VerifyDataTransformMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = DataMapping;
            var expectedView = "Partials/Transformation/_transformationChangeValue";
            _dataMapTransforms = new List<TransformDataMap>
            {
                new TransformDataMap
                {
                    TransformationID = TransformationId,
                    MatchType = "0",
                    SourceData = "source-data",
                    DesiredData = "desired-data",
                    PubID = 0,
                    TransformDataMapID = 1
                },
                new TransformDataMap
                {
                    TransformationID = TransformationId,
                    MatchType = "1",
                    SourceData = "source-data",
                    DesiredData = "desired-data",
                    PubID = 0,
                    TransformDataMapID = 2
                },
                new TransformDataMap
                {
                    TransformationID = TransformationId,
                    MatchType = "1",
                    SourceData = "source-data",
                    DesiredData = "desired-data",
                    PubID = PubId,
                    TransformDataMapID = 3
                }
            };

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            product.PubID = PubId;
            product.PubCode = "pub-code";

            _products = new List<Product>
            {
                product
            };

            var maps = new List<TransformDataMapModel>
            {
                new TransformDataMapModel(0, "1", 0, new List<int> {0}, "ALL PRODUCTS", "0", "source-data", "desired-data", _products),
                new TransformDataMapModel(0, "2,3", 1, new List<int> {0, PubId}, "pub-code", "1", "source-data", "desired-data", _products)
            };

            var expectedModel = new TransformationChangeValueModel(maps, _products, true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyChangeValueModel(result.Model as TransformationChangeValueModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullJoinTransformWithIDLessThanZero_EmptyJoinTransformsMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = JoinColumns;
            var expectedView = "Partials/Transformation/_transformationJoinColumns";
            _dataMapTransforms = null;

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            _products = new List<Product>
            {
                product
            };

            _fieldMappings = new List<FieldMapping>
            {
                typeof(FieldMapping).CreateInstance()
            };

            var expectedModel = new TransformationJoinColumnModel(
                _fieldMappings
                    .Select(x => x.IncomingField).ToList(),
                _products,
                true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, 0);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyJoinColumnModel(result.Model as TransformationJoinColumnModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullJoinTransformWithIDGreaterThanZero_EmptyJoinTransformsMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = JoinColumns;
            var expectedView = "Partials/Transformation/_transformationJoinColumns";
            _dataMapTransforms = null;

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            _products = new List<Product>
            {
                product
            };

            _fieldMappings = new List<FieldMapping>
            {
                typeof(FieldMapping).CreateInstance()
            };

            var expectedModel =
                new TransformationJoinColumnModel(_fieldMappings.Select(x => x.IncomingField).ToList(), _products);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyJoinColumnModel(result.Model as TransformationJoinColumnModel, expectedModel);
        }

        [Test]
        [TestCaseSource(nameof(_delimeters))]
        public void LoadTransformationDetail_JoinTransform_VerifyJoinMaps(string delimiter)
        {
            // Arrange
            _transformationTypes.First().CodeName = JoinColumns;
            var expectedView = "Partials/Transformation/_transformationJoinColumns";

            TransformJoin transformJoin = typeof(TransformJoin).CreateInstance();
            transformJoin.Delimiter = delimiter;
            _joinTransTransforms = new List<TransformJoin>
            {
                transformJoin
            };

            _transformationPubMaps = new List<TransformationPubMap>
            {
                typeof(TransformationPubMap).CreateInstance()
            };

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            product.PubID = PubId;
            product.PubCode = "pub-code";

            _products = new List<Product>
            {
                product
            };

            _fieldMappings = new List<FieldMapping>
            {
                typeof(FieldMapping).CreateInstance()
            };

            var expectedModel = new TransformationJoinColumnModel(
                transformJoin.TransformJoinID,
                transformJoin.ColumnsToJoin,
                transformJoin.Delimiter,
                _fieldMappings
                    .Select(x => x.IncomingField).ToList(),
                _products,
                _transformationPubMaps
                    .Select(x => x.PubID).Distinct().ToList(),
                true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifyJoinColumnModel(result.Model as TransformationJoinColumnModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullSplitTransformWithIDLessThanZero_EmptySplitTransformsMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = SplitIntoRows;
            var expectedView = "Partials/Transformation/_transformationSplitIntoRows";
            _splitTransforms = new List<TransformSplit>();

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            _products = new List<Product>
            {
                product
            };

            var expectedModel = new TransformationSplitIntoRowModel(_products, true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, 0);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifySplitColumnModel(result.Model as TransformationSplitIntoRowModel, expectedModel);
        }

        [Test]
        public void LoadTransformationDetail_NullSplitTransformWithIDGreaterThanZero_EmptySplitTransformsMaps()
        {
            // Arrange
            _transformationTypes.First().CodeName = SplitIntoRows;
            var expectedView = "Partials/Transformation/_transformationSplitIntoRows";
            _splitTransforms = new List<TransformSplit>();

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            _products = new List<Product>
            {
                product
            };

            var expectedModel = new TransformationSplitIntoRowModel(_products);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifySplitColumnModel(result.Model as TransformationSplitIntoRowModel, expectedModel);
        }

        [Test]
        [TestCaseSource(nameof(_delimeters))]
        public void LoadTransformationDetail_SplitTransform_VerifySplitMaps(string delimiter)
        {
            // Arrange
            _transformationTypes.First().CodeName = SplitIntoRows;
            var expectedView = "Partials/Transformation/_transformationSplitIntoRows";

            TransformSplit transformSplit = typeof(TransformSplit).CreateInstance();
            transformSplit.Delimiter = delimiter;
            _splitTransforms = new List<TransformSplit>
            {
                transformSplit
            };

            _transformationPubMaps = new List<TransformationPubMap>
            {
                typeof(TransformationPubMap).CreateInstance()
            };

            Product product = typeof(Product).CreateInstance();
            product.IsActive = true;
            product.PubID = PubId;
            product.PubCode = "pub-code";

            _products = new List<Product>
            {
                product
            };

            var expectedModel = new TransformationSplitIntoRowModel(
                transformSplit.TransformSplitID,
                transformSplit.Delimiter,
                _products,
                _transformationPubMaps
                    .Select(x => x.PubID).Distinct().ToList(),
                true);

            // Act
            var result =
                (PartialViewResult) _controller.LoadTransformationDetail(TransformationTypeId, TransformationId);

            // Assert
            result.ViewName.ShouldBe(expectedView);
            VerifySplitColumnModel(result.Model as TransformationSplitIntoRowModel, expectedModel);
        }

        private static void VerifyTransformationAssignModel(
            TransformationAssignModel actual,
            TransformationAssignModel expected)
        {
            actual.ShouldSatisfyAllConditions(
                () => actual.ShouldNotBeNull(),
                () => actual.isEnabled.ShouldBe(expected.isEnabled),
                () => actual.products.IsListContentMatched(expected.products).ShouldBeTrue(),
                () => actual.selectedProducts
                    .IsContentMatched(expected.selectedProducts)
                    .ShouldBeTrue(),
                () => actual.transformAssignMaps
                    .IsListContentMatched(expected.transformAssignMaps, nameof(TransformAssignModel.selectedPubID))
                    .ShouldBeTrue()
            );
        }

        private static void VerifyChangeValueModel(
            TransformationChangeValueModel actual,
            TransformationChangeValueModel expected)
        {
            actual.ShouldSatisfyAllConditions(
                () => actual.isEnabled.ShouldBe(expected.isEnabled),
                () => actual.products.IsListContentMatched(expected.products).ShouldBeTrue(),
                () => actual.selectedProducts
                    .IsContentMatched(expected.selectedProducts)
                    .ShouldBeTrue(),
                () => actual.transformDataMaps
                    .IsListContentMatched(
                        expected.transformDataMaps,
                        nameof(TransformDataMapModel.selectedMatchType),
                        nameof(TransformDataMapModel.selectedPubID))
                    .ShouldBeTrue());
        }

        private static void VerifyJoinColumnModel(
            TransformationJoinColumnModel actual,
            TransformationJoinColumnModel expected)
        {
            actual.ShouldSatisfyAllConditions(
                () => actual.isEnabled.ShouldBe(expected.isEnabled),
                () => actual.products.IsListContentMatched(expected.products).ShouldBeTrue(),
                () => actual.selectedProducts.IsContentMatched(expected.selectedProducts).ShouldBeTrue(),
                () => actual.availableColumns.IsListContentMatched(expected.availableColumns),
                () => actual.TransformJoinId.ShouldBe(expected.TransformJoinId),
                () => actual.ColumnsToJoin.ShouldBe(expected.ColumnsToJoin),
                () => actual.Delimiter.ShouldBe(expected.Delimiter));
        }

        private void VerifySplitColumnModel(
            TransformationSplitIntoRowModel actual,
            TransformationSplitIntoRowModel expected)
        {
            actual.ShouldSatisfyAllConditions(
                () => actual.isEnabled.ShouldBe(expected.isEnabled),
                () => actual.Delimiter.ShouldBe(expected.Delimiter),
                () => actual.TransformSplitId.ShouldBe(expected.TransformSplitId),
                () => actual.products.IsListContentMatched(expected.products).ShouldBeTrue());
        }
    }
}