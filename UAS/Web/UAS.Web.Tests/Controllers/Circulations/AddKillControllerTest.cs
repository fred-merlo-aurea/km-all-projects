using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using FrameworkUAD_Lookup.Entity;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.Web.Controllers.Circulations;
using ShimTransactionCode = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimTransactionCode;

namespace UAS.Web.Tests.Controllers.Circulations
{
    [TestFixture]
    public partial class AddKillControllerTest : ControllerTestBase
    {
        private const int TestValue1 = 1;
        private const int TestValue2 = 2;
        private const int TestValue3 = 3;
        private const int TestNumber100 = 100;
        private const int TestNumber200 = 200;
        private const int TestNumber300 = 300;
        private const int TestNumberOutOfRange = 1000;
        private const string SampleText1 = "Sample 1";
        private const string SampleText2 = "Sample 2";
        private const string SampleText3 = "Sample 3";
        private const bool IsSelected = true;

        private AddKillController _controller;
        private TransactionCodeType _xCodeTypeActiveFree;
        private IDisposable _context;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _context = ShimsContext.Create();
            _controller = new AddKillController();
            Initialize(_controller);
            MockControllerContext();
            ShimTransactionCode.AllInstances.Select =
                _ => new List<TransactionCode>
                {
                    new TransactionCode
                    {
                        TransactionCodeID = TestValue1,
                        TransactionCodeTypeID = TestNumber100, 
                        TransactionCodeName = SampleText1,
                        TransactionCodeValue = TestValue1
                    },
                    new TransactionCode
                    {
                        TransactionCodeID = TestValue2,
                        TransactionCodeTypeID = TestNumber200, 
                        TransactionCodeName = SampleText2,
                        TransactionCodeValue = TestValue2
                    },
                    new TransactionCode
                    {
                        TransactionCodeID = TestValue3,
                        TransactionCodeTypeID = TestNumber300, 
                        TransactionCodeName = SampleText3,
                        TransactionCodeValue = TestValue3
                    }
                };
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Dispose();
        }

        [Test]
        public void GenerateXCodeList_WhenXCodeTypeActiveFreeIsNull_ThrowsException()
        {
            // Arrange
            _xCodeTypeActiveFree = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.GenerateXCodeList(_xCodeTypeActiveFree);
            });
        }

        [Test]
        public void GenerateXCodeList_WhenXCodeListIsNull_ReturnsEmptyList()
        {
            // Arrange
            _xCodeTypeActiveFree = new TransactionCodeType()
            {
                TransactionCodeTypeID = TestNumberOutOfRange
            };

            // Act
            var result = _controller.GenerateXCodeList(_xCodeTypeActiveFree);

            // Arrange
            result.ShouldNotBeNull();
            result.Count.ShouldBe(0);
        }

        [Test]
        [TestCase(TestNumber100, TestValue1, SampleText1)]
        [TestCase(TestNumber200, TestValue2, SampleText2)]
        [TestCase(TestNumber300, TestValue3, SampleText3)]
        public void GenerateXCodeList_MustAddOneElementInXCodeList_ReturnsCorrectValues(
            int transactionCodeTypeId, 
            int testValue, 
            string sampleText)
        {
            // Arrange
            _xCodeTypeActiveFree = new TransactionCodeType()
            {
                TransactionCodeTypeID = transactionCodeTypeId
            };
            
            // Act
            var result = _controller.GenerateXCodeList(_xCodeTypeActiveFree);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result[0].Text.ShouldBe(string.Format("{0}. {1}", testValue, sampleText.ToUpper())),
                () => result[0].Value.ShouldBe(testValue.ToString()),
                () => result[0].Selected.ShouldBe(IsSelected)
            );
        }
    }
}
