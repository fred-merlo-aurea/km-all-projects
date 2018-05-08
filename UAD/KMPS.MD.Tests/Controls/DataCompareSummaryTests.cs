using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using DataCompareSummary = KMPS.MD.Controls.DataCompareSummary;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DataCompareSummaryTests : BaseControlTests
    {
        private DataCompareSummary _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new DataCompareSummary();
            InitializeUserControl(_testEntity);
        }

        [Test]
        [TestCase("PubID", 1)]
        [TestCase("PubID", 2)]
        [TestCase("BrandID", 1)]
        [TestCase("BrandID", 2)]
        [TestCase("UserID", 1)]
        [TestCase("UserID", 2)]
        [TestCase("ViewType", Enums.ViewType.ProductView)]
        [TestCase("ViewType", Enums.ViewType.ConsensusView)]
        public void Property_Set_GetSameValue(string propertyName, object value)
        {
            // Arrange, Act
            PrivateControl.SetProperty(propertyName, value);
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(value);
        }

        [Test]
        [TestCase("PubID", 0)]
        [TestCase("UserID", 0)]
        [TestCase("BrandID", 0)]
        [TestCase("ViewType", Enums.ViewType.None)]
        public void Property_Get_DefaultValue(string propertyName, object defaultValue)
        {
            // Arrange, Act
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(defaultValue);
        }
    }
}
