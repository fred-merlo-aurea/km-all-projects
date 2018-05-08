using System.Collections.Generic;
using FrameworkSubGen.BusinessLogic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using DF = FrameworkSubGen.DataAccess.Fakes;
using SubGenEntity = FrameworkSubGen.Entity;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestClass]
    public class ImportDimensionTest
    {
        private const int DummyImportDimensionId = 100;
        private bool _saveBulkXmlResult = false;

        [TestMethod]
        public void SaveBulkXml_ImportDimension_ReturnTrue()
        {
            //Arrange:
            var importDimension = new ImportDimension();
            var lstImportDimensions = new List<SubGenEntity.ImportDimension>();
            var importDimensionEntity = new SubGenEntity.ImportDimension()
            {
                ImportDimensionId = DummyImportDimensionId
            };
            lstImportDimensions.Add(importDimensionEntity);

            using (ShimsContext.Create())
            {
                DF.ShimImportDimension.SaveBulkXmlString = s =>
                {
                    _saveBulkXmlResult = true;
                    return true;
                };

                //Act:
                var result = importDimension.SaveBulkXml(lstImportDimensions);

                //Assert:
                result.ShouldBeTrue();
                _saveBulkXmlResult.ShouldBeTrue();
            }
        }

        [TestMethod]
        public void SaveBulkXml_ImportDimension_With300ImportEntity_ReturnTrue()
        {
            //Arrange:
            var importDimension = new ImportDimension();
            var lstImportDimensions = new List<SubGenEntity.ImportDimension>();
            for (var i = 0; i < 300; i++)
            {
                var importDimensionEntity = new SubGenEntity.ImportDimension()
                {
                    ImportDimensionId = i
                };
                lstImportDimensions.Add(importDimensionEntity);
            }

            using (ShimsContext.Create())
            {
                var saveBulkXmlInvokeCount = 0;
                DF.ShimImportDimension.SaveBulkXmlString = s =>
                {
                    saveBulkXmlInvokeCount++;
                    _saveBulkXmlResult = true;
                    return true;
                };

                //Act:
                var result = importDimension.SaveBulkXml(lstImportDimensions);

                //Assert:
                result.ShouldBeTrue();
                _saveBulkXmlResult.ShouldBeTrue();
                //saveBulkXmlInvokeCount should be 2 because list contains 300 entries 
                //and xml is saved in bulk of 250 so saveBulkXml will be invoked 2 times
                saveBulkXmlInvokeCount.ShouldBe(2);
            }
        }
    }
}
