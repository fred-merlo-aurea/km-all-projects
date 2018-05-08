using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.QualityTools.Testing.Fakes;

using ADMS.ClientMethods;
using Core.ADMS.Events;
using Core.ADMS.Fakes;
using Core.ADMS.Events.Fakes;
using Core_AMS.Utilities.Fakes;

using FrameworkUAS.Entity;
using FrameworkUAS.Entity.Fakes;

using NUnit.Framework;

using ShimAdHocDimension = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimension;
using ShimAdHocDimensionGroup = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimensionGroup;
using ShimSourceFileEntity = FrameworkUAS.Entity.Fakes.ShimSourceFile;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    public class SpecialtyFoodsTest
    {
        private const string File1Txt = "1.txt";

        private const string FieldCompany = "COMPANY";
        private const string Company1 = "Company1";
        private IDisposable shims;

        [SetUp]
        public void Init()
        {
            shims = ShimsContext.Create();
            ShimAdHocDimension.AllInstances.DeleteInt32 = (dimension, i) => true;
            ShimFileMoved.AllInstances.SourceFileGet = moved => new SourceFile();
            var fileInfo = AdvanstarTest.CreateFileInfo("1.txt");
            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;
            ShimSourceFileBase.AllInstances.SourceFileIDGet = file => 1;
        }

        [TearDown]
        public void Cleanup()
        {
            shims?.Dispose();
            shims = null;
        }

        [Test]
        public void SpecialtyFoodsBRNFLAGAdHocImport_EmptyTableSourceFileSelectNull_AdHocDimensionGroupReselected()
        {
            // Arrange
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
                {
                    var dt = new System.Data.DataTable();
                    return dt;
                };

            var adHocDimensionGroupSelectCalledCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
                {
                    adHocDimensionGroupSelectCalledCnt++;
                    var r = new AdHocDimensionGroup();
                    return r;
                };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
                {
                    adHocDimensionSaveBulkSqlInsertCalled = true;
                    if (list.Any())
                    {
                        adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                    }
                    return true;
                };

            var client = new KMPlatform.Entity.Client();

            // Act
            var food = new SpecialtyFoods();
            food.BRNFLAGAdHocImport(client, new FileMoved());

            // Assert
            Assert.AreEqual(1, adHocDimensionGroupSelectCalledCnt);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsFalse(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }

        [Test]
        public void SpecialtyFoodsBRNFLAGAdHocImport_EmptyTableSourceFileSelectInstance_AdHocDimensionGroupReselected()
        {
            // Arrange
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
                {
                    var dt = new System.Data.DataTable();
                    return dt;
                };

            var adHocDimensionGroupSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
                {
                    if ((adHocDimensionGroupSelectCnt++ % 2) == 0)
                    {
                        return null;
                    }
                    else
                    {
                        var r = new AdHocDimensionGroup();
                        return r;
                    }
                };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
                {
                    adHocDimensionSaveBulkSqlInsertCalled = true;
                    if (list.Any())
                    {
                        adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                    }
                    return true;
                };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
                {
                    adHocDimensionGroupSaved = true;
                    return true;
                };

            var client = new KMPlatform.Entity.Client();

            // Act
            var food = new SpecialtyFoods();
            food.BRNFLAGAdHocImport(client, new FileMoved());

            // Assert
            Assert.AreEqual(2, adHocDimensionGroupSelectCnt);
            Assert.IsTrue(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsFalse(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }

        [Test]
        public void SpecialtyFoodsBRNFLAGAdHocImport_NonEmptyTableSourceFileSelectInstance_AdHocDimensionGroupReselected()
        {
            // Arrange
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new System.Data.DataTable();
                dt.Columns.Add(FieldCompany);
                var r = dt.NewRow();
                r["COMPANY"] = 1;
                dt.Rows.Add(r);
                return dt;
            };

            var adHocDimensionGroupSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                if ((adHocDimensionGroupSelectCnt++ % 2) == 0)
                {
                    return null;
                }
                else
                {
                    var r = new AdHocDimensionGroup();
                    return r;
                }
            };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
            {
                adHocDimensionSaveBulkSqlInsertCalled = true;
                if (list.Any())
                {
                    adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                }
                return true;
            };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
            {
                adHocDimensionGroupSaved = true;
                return true;
            };

            var client = new KMPlatform.Entity.Client();

            // Act
            var food = new SpecialtyFoods();
            food.BRNFLAGAdHocImport(client, new FileMoved());

            // Assert
            Assert.AreEqual(2, adHocDimensionGroupSelectCnt);
            Assert.IsTrue(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }

        [Test]
        public void SpecialtyFoodsKBFLAGAdHocImport_AgWorkerReturnsNullDataRowPresent_AgWorkerSavedAgDataInserted()
        {
            // Arrange
            var adHocDimensionGroupSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                if ((adHocDimensionGroupSelectCnt++ % 2) == 0)
                {
                    return null;
                }

                var r = new AdHocDimensionGroup();
                return r;
            };

            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new System.Data.DataTable();
                dt.Columns.Add(FieldCompany);
                var r = dt.NewRow();
                r[FieldCompany] = Company1;
                dt.Rows.Add(r);
                return dt;
            };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
            {
                adHocDimensionGroupSaved = true;
                return true;
            };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            var adHocDimensionSaveBulkSqlInsertTestRegionCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
            {
                adHocDimensionSaveBulkSqlInsertCalled = true;
                if (list.Any())
                {
                    adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                }

                if (list.Any(e => e.MatchValue == Company1))
                {
                    adHocDimensionSaveBulkSqlInsertTestRegionCalled = true;
                }
                return true;
            };

            var client = new KMPlatform.Entity.Client();

            var fileInfo = new FileInfo(File1Txt);
            var fileMoved = new FileMoved { Client = client, ImportFile = fileInfo };

            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;

            // Act
            var food = new SpecialtyFoods();
            food.KBFLAGAdHocImport(client, new FileMoved());

            // Assert
            Assert.AreEqual(2, adHocDimensionGroupSelectCnt);
            Assert.IsTrue(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertTestRegionCalled);
        }

        [Test]
        public void SpecialtyFoodsKBFLAGAdHocImport_AgWorkerReturnsNotNullDataRowAbsent_AgWorkerNotSavedAgDataNot()
        {
            // Arrange
            var adHocDimensionGroupSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                adHocDimensionGroupSelectCnt++;

                var r = new AdHocDimensionGroup();
                return r;
            };

            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new System.Data.DataTable();
                dt.Columns.Add(FieldCompany);
                return dt;
            };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
            {
                adHocDimensionGroupSaved = true;
                return true;
            };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
            {
                adHocDimensionSaveBulkSqlInsertCalled = true;
                if (list.Any())
                {
                    adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                }

                return true;
            };

            var client = new KMPlatform.Entity.Client();
            var scranton = new Scranton();

            var fileInfo = new FileInfo(File1Txt);
            var fileMoved = new FileMoved { Client = client, ImportFile = fileInfo };

            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;

            // Act
            var food = new SpecialtyFoods();
            food.KBFLAGAdHocImport(client, new FileMoved());

            // Assert
            Assert.AreEqual(1, adHocDimensionGroupSelectCnt);
            Assert.IsFalse(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsFalse(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }
    }
}
