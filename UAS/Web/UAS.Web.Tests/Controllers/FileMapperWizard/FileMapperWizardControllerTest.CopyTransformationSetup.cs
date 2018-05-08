using System;
using ECN_Framework_BusinessLayer.Application;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.Web.Controllers.FileMapperWizard;

namespace UAS.Web.Tests.Controllers.FileMapperWizard
{
    /// <summary>
    ///     Unit tests for <see cref="FileMapperWizardController.CopyTransformationSetup"/>
    /// </summary>
    public partial class FileMapperWizardControllerTest
    {
        [Test]
        public void CopyTransformationSetup_WhenCalled_SaveTransformation()
        {
            // Arrange
            SetTransformation();

            Transformation actual = null;
            Transformation expected = typeof(Transformation).CreateInstance();
            expected.TransformationID = 0;
            expected.TransformationTypeID = TransformationTypeId;
            expected.TransformationName = _transformation.TransformationName + "_Copy";
            expected.IsActive = true;
            expected.DateCreated = DateTime.Now;
            expected.CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            expected.IsTemplate = false;

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) =>
            {
                actual = trans;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        [Test]
        public void CopyTransformationSetup_WhenCalledWithPubMap_SavePubMapTransformation()
        {
            // Arrange
            SetTransformation();

            TransformationPubMap actual = null;
            _transformationPubMaps.Add(new TransformationPubMap());
            TransformationPubMap expected = new TransformationPubMap
            {
                TransformationPubMapID = 0,
                TransformationID = TransformationId,
                DateCreated = DateTime.Now,
                CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID
            };

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) => TransformationId;
            ShimTransformationPubMap.AllInstances.SaveTransformationPubMap = (map, pubMap) =>
            {
                actual = pubMap;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        [Test]
        public void CopyTransformationSetup_WhenCalledWithAssignValues_SaveAssignValuesTransformation()
        {
            // Arrange
            SetTransformation();

            _transformationTypes.ForEach(x => x.CodeName = "Assign Value");

            TransformAssign actual = null;
            _assignTransforms.Add(typeof(TransformAssign).CreateInstance());
            TransformAssign expected = typeof(TransformAssign).CreateInstance();
            expected.TransformAssignID = 0;
            expected.TransformationID = TransformationId;
            expected.DateCreated = DateTime.Now;
            expected.CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID;

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) => TransformationId;
            ShimTransformAssign.AllInstances.SaveTransformAssign = (map, assign) =>
            {
                actual = assign;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        [Test]
        public void CopyTransformationSetup_WhenCalledWithDataMapValues_SaveDataMapValuesTransformation()
        {
            // Arrange
            SetTransformation();

            _transformationTypes.ForEach(x => x.CodeName = "Data Mapping");

            TransformDataMap actual = null;
            _dataMapTransforms.Add(typeof(TransformDataMap).CreateInstance());
            TransformDataMap expected = typeof(TransformDataMap).CreateInstance();
            expected.TransformDataMapID = 0;
            expected.TransformationID = TransformationId;
            expected.DateCreated = DateTime.Now;
            expected.CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID;

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) => TransformationId;
            ShimTransformDataMap.AllInstances.SaveTransformDataMap = (map, dataMap) =>
            {
                actual = dataMap;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        [Test]
        public void CopyTransformationSetup_WhenCalledWithJoinValues_SaveJoinValuesTransformation()
        {
            // Arrange
            SetTransformation();

            _transformationTypes.ForEach(x => x.CodeName = "Join Columns");

            TransformJoin actual = null;
            _joinTransTransforms.Add(typeof(TransformJoin).CreateInstance());
            TransformJoin expected = typeof(TransformJoin).CreateInstance();
            expected.TransformJoinID = 0;
            expected.TransformationID = TransformationId;
            expected.DateCreated = DateTime.Now;
            expected.CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID;

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) => TransformationId;
            ShimTransformJoin.AllInstances.SaveTransformJoin = (map, join) =>
            {
                actual = join;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        [Test]
        public void CopyTransformationSetup_WhenCalledWithSplitValues_SaveSplitValuesTransformation()
        {
            // Arrange
            SetTransformation();

            _transformationTypes.ForEach(x => x.CodeName = "Split Into Rows");

            TransformSplit actual = null;
            _splitTransforms.Add(typeof(TransformSplit).CreateInstance());
            TransformSplit expected = typeof(TransformSplit).CreateInstance();
            expected.TransformSplitID = 0;
            expected.TransformationID = TransformationId;
            expected.DateCreated = DateTime.Now;
            expected.CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID;

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) => TransformationId;
            ShimTransformSplit.AllInstances.SaveTransformSplit = (map, split) =>
            {
                actual = split;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        [Test]
        public void CopyTransformationSetup_WhenCalledWithSplitTransValues_SaveSplitTransValuesTransformation()
        {
            // Arrange
            SetTransformation();

            _transformationTypes.ForEach(x => x.CodeName = "Split Transform");

            TransformSplitTrans actual = null;
            _splitTransTransforms.Add(typeof(TransformSplitTrans).CreateInstance());
            TransformSplitTrans expected = typeof(TransformSplitTrans).CreateInstance();
            expected.SplitTransformID = 0;
            expected.TransformationID = TransformationId;
            expected.DateCreated = DateTime.Now;
            expected.CreatedByUserID = ECNSession.CurrentSession().CurrentUser.UserID;

            ShimTransformation.AllInstances.SaveTransformation = (transformation, trans) => TransformationId;
            ShimTransformSplitTrans.AllInstances.SaveTransformSplitTrans = (map, split) =>
            {
                actual = split;
                return 1;
            };

            // Act
            _controller.CopyTransformationSetup(TransformationId, null);

            // Assert
            actual.IsContentMatched(expected).ShouldBeTrue();
        }

        private void SetTransformation()
        {
            _transformation = typeof(Transformation).CreateInstance();
            _transformation.TransformationID = TransformationId;
            _transformation.TransformationTypeID = TransformationTypeId;
        }
    }
}