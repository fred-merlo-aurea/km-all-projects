using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using FrameworkUAD.Object;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.Web.Controllers.FileMapperWizard;
using UAS.Web.Models.FileMapperWizard;
using Code = FrameworkUAD_Lookup.Entity.Code;
using ShimCode = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode;
using Enums = FrameworkUAD_Lookup.Enums;

namespace UAS.Web.Tests.Controllers.FileMapperWizard
{
    /// <summary>
    ///     Unit tests for <see cref="FileMapperWizardController"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FileMapperWizardControllerTest
    {
        private const int ClientId = 25;
        private const int TransformationTypeId = 10;
        private const int TransformationId = 35;
        private const int UserId = 30;
        private const int PubId = 15;
        private const int SourceFileId = 20;
        private const int PubIdValue1 = 100;
        private const int PubIdValue2 = 200;
        private const int PubIdValue3 = 300;
        private const string AssignValue = "Assign Value";
        private const string DataMapping = "Data Mapping";
        private const string JoinColumns = "Join Columns";
        private const string SplitIntoRows = "Split Into Rows";

        private FileMapperWizardController _controller;
        private IDisposable _context;
        private Transformation _transformation;
        private List<Code> _transformationTypes;
        private List<TransformationPubMap> _transformationPubMaps;
        private List<TransformAssign> _assignTransforms;
        private List<TransformSplit> _splitTransforms;
        private List<TransformDataMap> _dataMapTransforms;
        private List<TransformSplitTrans> _splitTransTransforms;
        private List<TransformJoin> _joinTransTransforms;
        private Enums.CodeType _type;
        private List<Product> _products;
        private Client _client;
        private List<FieldMapping> _fieldMappings;
        private FileMapperWizardViewModel _model;

        private List<int> _newSavedPubCodes;
        private List<int> _oldSavedPubCodes;
        private FrameworkUAS.BusinessLogic.TransformAssign _transformAssignWorker;
        private FrameworkUAS.BusinessLogic.TransformDataMap _transformDataMapWorker;

        private int _fmlStandardId = 10;
        private int _fmlDemoId = 15;
        private int _fmlDemoOtherId = 20;
        private int _fmlDemoDateId = 25;
        private int _fmlIgnoredId = 30;
        private int _fmlKmTransformId = 35;
        private string _mappedColumnName;
        private FileMappingColumn _foundColumn;
        private List<Code> _fieldMappingTypes;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            _controller = new FileMapperWizardController();
            _transformation = new Transformation();
            _type = Enums.CodeType.Transformation;

            _transformationPubMaps = new List<TransformationPubMap>();
            _assignTransforms = new List<TransformAssign>();
            _splitTransforms = new List<TransformSplit>();
            _dataMapTransforms = new List<TransformDataMap>();
            _splitTransTransforms = new List<TransformSplitTrans>();
            _joinTransTransforms = new List<TransformJoin>();
            _products = new List<Product>();
            _fieldMappings = new List<FieldMapping>();
            _model = typeof(FileMapperWizardViewModel).CreateInstance();
            _model.setupViewModel = typeof(SetupViewModel).CreateInstance();
            _model.setupViewModel.SourceFileId = SourceFileId;
            _model.userCreatedTransformations = new UserCreatedTransformations();
            _model.userCreatedTransformations.transformationIds.Add(TransformationId);

            _client = typeof(Client).CreateInstance();
            _client.ClientID = ClientId;

            SetupControllerContext();

            _transformationTypes = new List<Code>
            {
                new Code
                {
                    CodeId = TransformationTypeId
                }
            };

            _fieldMappingTypes = new List<Code>
            {
                new Code {CodeId = _fmlIgnoredId, CodeName = Enums.FieldMappingTypes.Ignored.ToString()},
                new Code {CodeId = _fmlKmTransformId, CodeName = Enums.FieldMappingTypes.kmTransform.ToString()},
                new Code {CodeId = _fmlStandardId, CodeName = Enums.FieldMappingTypes.Standard.ToString()},
                new Code {CodeId = _fmlDemoId, CodeName = Enums.FieldMappingTypes.Demographic.ToString()},
                new Code
                {
                    CodeId = _fmlDemoOtherId,
                    CodeName = Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' ')
                },
                new Code
                {
                    CodeId = _fmlDemoDateId,
                    CodeName = Enums.FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' ')
                }
            };

            _oldSavedPubCodes = new List<int>
            {
                PubIdValue1, PubIdValue2, PubIdValue3
            };

            _newSavedPubCodes = new List<int>
            {
                PubIdValue2, PubIdValue3
            };

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SetupFakes()
        {
            ShimTransformation.AllInstances.SelectTransformationByIDInt32 = (transformation, id) =>
            {
                id.ShouldBe(TransformationId);
                return _transformation;
            };

            ShimCode.AllInstances.SelectEnumsCodeType = (type, codeType) =>
            {
                codeType.ShouldBe(_type);
                return _transformationTypes;
            };

            ShimTransformationPubMap.AllInstances.SelectInt32 = (map, id) =>
            {
                id.ShouldBe(TransformationId);

                return _transformationPubMaps;
            };

            ShimTransformAssign.AllInstances.SelectInt32 = (assign, id) =>
            {
                id.ShouldBe(TransformationId);

                return _assignTransforms;
            };

            ShimTransformSplit.AllInstances.SelectInt32 = (assign, id) =>
            {
                id.ShouldBe(TransformationId);

                return _splitTransforms;
            };

            ShimTransformDataMap.AllInstances.SelectInt32 = (assign, id) =>
            {
                id.ShouldBe(TransformationId);

                return _dataMapTransforms;
            };


            ShimTransformSplitTrans.AllInstances.SelectInt32 = (assign, id) =>
            {
                id.ShouldBe(TransformationId);

                return _splitTransTransforms;
            };

            ShimTransformJoin.AllInstances.SelectInt32 = (assign, id) =>
            {
                id.ShouldBe(TransformationId);

                return _joinTransTransforms;
            };

            ShimDateTime.NowGet = () => new DateTime(2018, 3, 2);
            ShimECNSession.CurrentSession = () =>
            {
                ECNSession session = typeof(ECNSession).CreateInstance();
                session.SetProperty(nameof(ECNSession.ClientID), value: ClientId);
                session.CurrentUser = new User
                {
                    UserID = UserId
                };

                return session;
            };

            ShimProduct.AllInstances.SelectClientConnectionsBoolean = (product, connections, includeCustomProperties) =>
            {
                connections.ShouldBe(_client.ClientConnections);
                return _products;
            };

            ShimFieldMapping.AllInstances.SelectInt32Boolean = (mapping, sourceFileId, includeCustomProperties) =>
            {
                sourceFileId.ShouldBe(SourceFileId);
                return _fieldMappings;
            };
        }

        private void SetupControllerContext()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(x => x.HttpMethod).Returns("GET");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Request).Returns(request.Object);

            var session = new Mock<HttpSessionStateBase>();
            session.Setup(x => x["BaseControlller_CurrentClient"]).Returns(_client);
            session.Setup(x => x["fmwModel"]).Returns(_model);
            mockHttpContext.Setup(x => x.Session).Returns(session.Object);

            _controller.ControllerContext = new ControllerContext(
                mockHttpContext.Object,
                new RouteData(),
                new Mock<ControllerBase>().Object);
        }

        private void SetupShimTransformationPub()
        {
            ShimTransformationPubMap.AllInstances.SelectInt32 = 
                (_, id) => new List<TransformationPubMap>
                {
                    new TransformationPubMap { PubID = PubIdValue1 },
                    new TransformationPubMap { PubID = PubIdValue2 },
                    new TransformationPubMap { PubID = PubIdValue3 },
                };
        }

        [Test]
        public void SelectFieldMappingTypeID_WhenFieldMappingTypesIsNull_ThrowsException()
        {
            // Arrange
            _fieldMappingTypes = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.SelectFieldMappingTypeID(_mappedColumnName, _foundColumn, _fieldMappingTypes);
            });
        }

        [Test]
        public void
            SelectFieldMappingTypeID_WhenFoundColumnIsNullAndMappedColumnsNameIsKmTransform_ReturnsFmlKmTransformId()
        {
            // Arrange
            _foundColumn = null;
            _mappedColumnName = "kmTransform";

            // Act
            var result = _controller.SelectFieldMappingTypeID(_mappedColumnName, _foundColumn, _fieldMappingTypes);

            // Assert
            result.ShouldBe(_fmlKmTransformId);
        }

        [Test]
        public void
            SelectFieldMappingTypeID_WhenFoundColumnIsNullAndMappedColumnsNameIsNotKmTransform_ReturnsFmlIgnoredId()
        {
            // Arrange
            _foundColumn = null;
            _mappedColumnName = "sample";

            // Act
            var result = _controller.SelectFieldMappingTypeID(_mappedColumnName, _foundColumn, _fieldMappingTypes);

            // Assert
            result.ShouldBe(_fmlIgnoredId);
        }

        [Test]
        public void
            SelectFieldMappingTypeID_WhenFoundColumnNameIsEmptyAndMappedColumnsNameIsKmTransform_ReturnsFmlKmTransformId()
        {
            // Arrange
            _foundColumn = new FileMappingColumn();
            _mappedColumnName = "kmTransform";

            // Act
            var result = _controller.SelectFieldMappingTypeID(_mappedColumnName, _foundColumn, _fieldMappingTypes);

            // Assert
            result.ShouldBe(_fmlKmTransformId);
        }

        [Test]
        public void
            SelectFieldMappingTypeID_WhenFoundColumnNameIsEmptyAndMappedColumnsNameIsNotKmTransform_ReturnsFmlIgnoredId()
        {
            // Arrange
            _foundColumn = new FileMappingColumn();
            _mappedColumnName = "sample";

            // Act
            var result = _controller.SelectFieldMappingTypeID(_mappedColumnName, _foundColumn, _fieldMappingTypes);

            // Assert
            result.ShouldBe(_fmlIgnoredId);
        }

        [Test]
        public void GetFieldMappingTypeId_WhenFoundColumnIsNull_ThrowsException()
        {
            // Arrange
            _foundColumn = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.GetFieldMappingTypeId(_fmlStandardId, _fmlDemoId, _fmlDemoOtherId,
                    _fmlDemoDateId, _foundColumn);
            });
        }

        [Test]
        public void GetFieldMappingTypeId_WhenFoundColumnIsDemographicDateTrue_ReturnsFmlDemoDateId()
        {
            // Arrange
            _foundColumn = new FileMappingColumn
            {
                IsDemographicDate = true
            };

            // Act
            var result = _controller.GetFieldMappingTypeId(_fmlStandardId, _fmlDemoId, _fmlDemoOtherId,
                _fmlDemoDateId, _foundColumn);

            // Assert
            result.ShouldBe(_fmlDemoDateId);
        }

        [Test]
        public void GetFieldMappingTypeId_WhenFoundColumnIsDemographicFalse_ReturnsFmlStandardId()
        {
            // Arrange
            _foundColumn = new FileMappingColumn
            {
                IsDemographicDate = false,
                IsDemographic = false
            };

            // Act
            var result = _controller.GetFieldMappingTypeId(_fmlStandardId, _fmlDemoId, _fmlDemoOtherId,
                _fmlDemoDateId, _foundColumn);

            // Assert
            result.ShouldBe(_fmlStandardId);
        }

        [Test]
        public void GetFieldMappingTypeId_WhenFoundColumnIsDemographicOtherFalse_ReturnsFmlDemoId()
        {
            // Arrange
            _foundColumn = new FileMappingColumn
            {
                IsDemographicDate = false,
                IsDemographic = true,
                IsDemographicOther = false
            };

            // Act
            var result = _controller.GetFieldMappingTypeId(_fmlStandardId, _fmlDemoId, _fmlDemoOtherId,
                _fmlDemoDateId, _foundColumn);

            // Assert
            result.ShouldBe(_fmlDemoId);
        }

        [Test]
        public void GetFieldMappingTypeId_WhenFoundColumnIsDemographicOtherTrue_ReturnsFmlDemoOtherId()
        {
            // Arrange
            _foundColumn = new FileMappingColumn
            {
                IsDemographicDate = false,
                IsDemographic = true,
                IsDemographicOther = true
            };

            // Act
            var result = _controller.GetFieldMappingTypeId(_fmlStandardId, _fmlDemoId, _fmlDemoOtherId,
                _fmlDemoDateId, _foundColumn);

            // Assert
            result.ShouldBe(_fmlDemoOtherId);
        }

        [Test]
        public void DeleteOldSavedPubs_WhenBothTransformAssignWorkerAndTransformDataMapWorkerIsNull_DeleteAllOldSavedPubCodes()
        {
            // Arrange
            SetupShimTransformationPub();

            ShimTransformationPubMap.AllInstances.DeleteInt32Int32 =
                (_, transformationId, pub) => 0;

            // Act
            var result = _controller.DeleteOldSavedPubs(
                TransformationId, 
                _transformAssignWorker, 
                _transformDataMapWorker);

            // Assert
            result.ShouldBe(_oldSavedPubCodes.Count);
        }

        [Test]
        public void DeleteOldSavedPubs_WhenOnlyTransformDataMapWorkerIsNull_ReturnCorrectValue()
        {
            // Arrange
            _transformAssignWorker = new FrameworkUAS.BusinessLogic.TransformAssign();
            _transformDataMapWorker = null;

            SetupShimTransformationPub();

            ShimTransformAssign.AllInstances.SelectInt32 =
                (_, id) => new List<TransformAssign>
                {
                    new TransformAssign { PubID = PubIdValue2 },
                    new TransformAssign { PubID = PubIdValue3 },
                };

            ShimTransformationPubMap.AllInstances.DeleteInt32Int32 =
                (_, transformationId, pub) => 0;

            // Act
            var result = _controller.DeleteOldSavedPubs(
                TransformationId, 
                _transformAssignWorker, 
                _transformDataMapWorker);

            // Assert
            result.ShouldBe(_oldSavedPubCodes.Except(_newSavedPubCodes).Count());
        }

        [Test]
        public void DeleteOldSavedPubs_WhenTransformDataMapWorkerReturnsNull_ThrowsException()
        {
            // Arrange
            _transformAssignWorker = new FrameworkUAS.BusinessLogic.TransformAssign();
            _transformDataMapWorker = null;

            SetupShimTransformationPub();

            ShimTransformAssign.AllInstances.SelectInt32 =
                (_, id) => null;

            ShimTransformationPubMap.AllInstances.DeleteInt32Int32 =
                (_, transformationId, pub) => 0;

            // Act, Assert
            Should.Throw<InvalidOperationException>(() => { 
                _controller.DeleteOldSavedPubs(
                    TransformationId, 
                    _transformAssignWorker, 
                    _transformDataMapWorker);
            });
        }

        [Test]
        public void DeleteOldSavedPubs_WhenOnlyTransformAssignWorkerIsNull_ReturnCorrectValue()
        {
            // Arrange
            _transformAssignWorker = null;
            _transformDataMapWorker = new FrameworkUAS.BusinessLogic.TransformDataMap();

            SetupShimTransformationPub();

            ShimTransformDataMap.AllInstances.SelectInt32 =
                (_, id) => new List<TransformDataMap>
                {
                    new TransformDataMap { PubID = PubIdValue2 },
                    new TransformDataMap { PubID = PubIdValue3 },
                };

            ShimTransformationPubMap.AllInstances.DeleteInt32Int32 =
                (_, transformationId, pub) => 0;

            // Act
            var result = _controller.DeleteOldSavedPubs(
                TransformationId, 
                _transformAssignWorker, 
                _transformDataMapWorker);

            // Assert
            result.ShouldBe(_oldSavedPubCodes.Except(_newSavedPubCodes).ToList().Count);
        }

        [Test]
        public void DeleteOldSavedPubs_WhenTransformAssignWorkerReturnsNull_ThrowsException()
        {
            // Arrange
            _transformAssignWorker = null;
            _transformDataMapWorker = new FrameworkUAS.BusinessLogic.TransformDataMap();

            SetupShimTransformationPub();
            ShimTransformDataMap.AllInstances.SelectInt32 =
                (_, id) => null;

            ShimTransformationPubMap.AllInstances.DeleteInt32Int32 =
                (_, transformationId, pub) => 0;

            // Act, Assert
            Should.Throw<InvalidOperationException>(() => { 
                _controller.DeleteOldSavedPubs(
                    TransformationId, 
                    _transformAssignWorker, 
                    _transformDataMapWorker);
            });
        }
    }
}