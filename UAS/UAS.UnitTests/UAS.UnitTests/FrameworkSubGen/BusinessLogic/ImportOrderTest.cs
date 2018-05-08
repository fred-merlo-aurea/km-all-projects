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
    public class ImportOrderTest
    {
        private const int DummyOrderId = 10;
        private bool _saveBulkXmlResult = false;

        [TestMethod]
        public void SaveBulkXml_ImportOrder_ReturnTrue()
        {
            //Arrange:
            var importOrder = new ImportOrder();
            var lstImportOrders = new List<SubGenEntity.ImportOrder>();
            var importOrderEntity = new SubGenEntity.ImportOrder()
            {
                OrderID = DummyOrderId
            };
            lstImportOrders.Add(importOrderEntity);

            using (ShimsContext.Create())
            {
                DF.ShimImportOrder.SaveBulkXmlString = s =>
                {
                    _saveBulkXmlResult = true;
                    return true;
                };

                //Act:
                var result = importOrder.SaveBulkXml(lstImportOrders);

                //Assert:
                result.ShouldBeTrue();
                _saveBulkXmlResult.ShouldBeTrue();
            }
        }

        [TestMethod]
        public void SaveBulkXml_ImportOrder_With300ImportEntity_ReturnTrue()
        {
            //Arrange:
            var importOrder = new ImportOrder();
            var lstImportOrders = new List<SubGenEntity.ImportOrder>();
            for (var i = 0; i < 300; i++)
            {
                var importOrderEntity = new SubGenEntity.ImportOrder()
                {
                    OrderID = i
                };
                lstImportOrders.Add(importOrderEntity);
            }

            using (ShimsContext.Create())
            {
                var saveBulkXmlInvokeCount = 0;
                DF.ShimImportOrder.SaveBulkXmlString = s =>
                {
                    saveBulkXmlInvokeCount++;
                    _saveBulkXmlResult = true;
                    return true;
                };

                //Act:
                var result = importOrder.SaveBulkXml(lstImportOrders);

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
