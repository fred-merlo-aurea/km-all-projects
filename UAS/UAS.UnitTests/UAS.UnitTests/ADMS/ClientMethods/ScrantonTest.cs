using System;
using System.IO;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
using ADMS.ClientMethods;
using Core.ADMS.Events;
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
    public class ScrantonTest
    {
        private const string FieldDomainOnly = "Domain Only";
        private const string File1Txt = "1.txt";
        private IDisposable shims;

        [SetUp]
        public void Init()
        {
            shims = ShimsContext.Create();
            ShimAdHocDimension.AllInstances.DeleteInt32 = (dimension, i) => true;
            ShimFileMoved.AllInstances.SourceFileGet = moved => new SourceFile();
            var fileInfo = AdvanstarTest.CreateFileInfo(File1Txt);
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
        public void ScrantonSurveyDomainsImport_AgWorkerReturnsNullDataRowPresent_AgWorkerSavedAgDataInserted()
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
                dt.Columns.Add(FieldDomainOnly);
                var r = dt.NewRow();
                r[FieldDomainOnly] = FieldDomainOnly;
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

                if (list.Any(e => e.MatchValue == FieldDomainOnly))
                {
                    adHocDimensionSaveBulkSqlInsertTestRegionCalled = true;
                }
                return true;
            };

            var client = new KMPlatform.Entity.Client();
            var scranton = new Scranton();

            var fileInfo = new FileInfo(File1Txt);
            var fileMoved = new FileMoved { Client = client, ImportFile = fileInfo };

            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;

            // Act
            scranton.SurveyDomainsImport(fileMoved);

            // Assert
            Assert.AreEqual(2, adHocDimensionGroupSelectCnt);
            Assert.IsTrue(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertTestRegionCalled);
        }

        [Test]
        public void ScrantonSurveyDomainsImport_AgWorkerReturnsNotNullDataRowAbsent_AgWorkerNotSavedAgDataNot()
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
                dt.Columns.Add(FieldDomainOnly);
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
            scranton.SurveyDomainsImport(fileMoved);

            // Assert
            Assert.AreEqual(1, adHocDimensionGroupSelectCnt);
            Assert.IsFalse(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsFalse(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }

    }
}
