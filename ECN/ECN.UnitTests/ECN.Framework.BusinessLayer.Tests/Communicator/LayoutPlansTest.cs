
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_BusinessLayer.Communicator;
using KM.Common.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using LayoutPlansEntity = ECN_Framework_Entities.Communicator.LayoutPlans;
using ShimLayoutPlans = ECN_Framework_DataLayer.Communicator.Fakes.ShimLayoutPlans;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class LayoutPlansTest
    {
        private const int SmartFormId = 10;
        private const int CustomerId = 20;
        private const string CacheTypeSf = "SF_";
        private const string CacheTypeForm = "Form_";
        private const string LayoutPlanGroupCacheName = "CACHE_LAYOUTPLAN_";

        private Guid? _token;
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [Test]
        public void CreateLayoutPlansList_WhenIsCacheEnabledIsFalseAndTokenIsNull_ReturnsLayoutPlansEntityList()
        {
            // Arrange
            var resultSmartFormdId = 0;
            var resultCustomerId = 0;
            _token = null;

            ShimCacheUtil.IsCacheEnabled = () => false;
            ShimLayoutPlans.GetBySmartFormIDInt32Int32 = (smartFormId, customerId) =>
            {
                resultSmartFormdId = smartFormId;
                resultCustomerId = customerId;
                return new List<LayoutPlansEntity>();
            };

            // Act
            var result = LayoutPlans.CreateLayoutPlansList(SmartFormId, CustomerId, CacheTypeSf, _token);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<LayoutPlansEntity>>(),
                () => resultCustomerId.ShouldBe(CustomerId),
                () => resultSmartFormdId.ShouldBe(SmartFormId));
        }

        [Test]
        public void CreateLayoutPlansList_WhenIsCacheEnabledIsFalseAndTokenIsNotNull_ReturnsLayoutPlansEntityList()
        {
            // Arrange
            var resultToken = string.Empty;
            _token = new Guid();

            ShimCacheUtil.IsCacheEnabled = () => false;
            ShimLayoutPlans.GetByFormTokenUIDGuid = guid => 
            {
                resultToken = guid.ToString();
                return new List<LayoutPlansEntity>();
            };

            // Act
            var result = LayoutPlans.CreateLayoutPlansList(SmartFormId, CustomerId, CacheTypeSf, _token);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<LayoutPlansEntity>>(),
                () => resultToken.ShouldBe(_token.ToString()));
        }

        [Test]
        public void CreateLayoutPlansList_WhenIsCacheEnabledIsTrueAndTokenIsNull_ReturnsLayoutPlansEntityList()
        {
            // Arrange
            var resultSmartFormdId = 0;
            var resultCustomerId = 0;
            var cacheKeyResult = string.Empty;
            _token = null;

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, layoutName, check) =>
            {
                cacheKeyResult = cacheKey;
                return null;
            };

            ShimLayoutPlans.GetBySmartFormIDInt32Int32 = (smartFormId, customerId) =>
            {
                resultSmartFormdId = smartFormId;
                resultCustomerId = customerId;
                return new List<LayoutPlansEntity>();
            };

            ShimCacheUtil.AddToCacheStringObjectStringBoolean = (s, o, arg3, arg4) => { };

            // Act
            var result = LayoutPlans.CreateLayoutPlansList(SmartFormId, CustomerId, CacheTypeSf, _token);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<LayoutPlansEntity>>(),
                () => resultCustomerId.ShouldBe(CustomerId),
                () => resultSmartFormdId.ShouldBe(SmartFormId),
                () => cacheKeyResult.ShouldBe($"{LayoutPlanGroupCacheName}{CacheTypeSf}{SmartFormId}"));
        }

        [Test]
        public void CreateLayoutPlansList_WhenIsCacheEnabledIsTrueAndTokenIsNotNull_ReturnsLayoutPlansEntityList()
        {
            // Arrange
            var resultToken = string.Empty;
            var cacheKeyResult = string.Empty;
            _token = new Guid();

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, layoutName, check) =>
            {
                cacheKeyResult = cacheKey;
                return null;
            };

            ShimLayoutPlans.GetByFormTokenUIDGuid = guid =>
            {
                resultToken = guid.ToString();
                return new List<LayoutPlansEntity>();
            };

            ShimCacheUtil.AddToCacheStringObjectStringBoolean = (s, o, arg3, arg4) => { };

            // Act
            var result = LayoutPlans.CreateLayoutPlansList(SmartFormId, CustomerId, CacheTypeForm, _token);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<LayoutPlansEntity>>(),
                () => resultToken.ShouldBe(_token.ToString()),
                () => cacheKeyResult.ShouldBe($"{LayoutPlanGroupCacheName}{CacheTypeForm}{_token}"));
        }

        [Test]
        public void CreateLayoutPlansList_WhenIsCacheEnabledIsTrueAndGetFromCacheIsNotNull_ReturnsLayoutPlansEntityList()
        {
            // Arrange
            var cacheKeyResult = string.Empty;
            _token = new Guid();

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, layoutName, check) =>
            {
                cacheKeyResult = cacheKey;
                return new List<LayoutPlansEntity>();
            };

            // Act
            var result = LayoutPlans.CreateLayoutPlansList(SmartFormId, CustomerId, CacheTypeForm, _token);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<LayoutPlansEntity>>(),
                () => cacheKeyResult.ShouldBe($"{LayoutPlanGroupCacheName}{CacheTypeForm}{_token}"));
        }
    }
}


