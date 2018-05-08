using System;
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
    public class AtcomTest
    {
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
        public void AtcomCompanyAdHocImport_WorkerSelectNull_AdHocDimensionGroupReselected()
        {
            // Arrange
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new System.Data.DataTable();
                dt.Columns.Add("MATCHFIELD");
                dt.Columns.Add("TYPE");
                dt.Columns.Add("REGION");
                dt.Columns.Add("COUNTRY");
                var r = dt.NewRow();
                r["MATCHFIELD"] = "1,2";
                r["REGION"] = "TestRegion";
                dt.Rows.Add(r);
                return dt;
            };

            var adHocDimensionGroupRegionSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
                {
                    if ((adHocDimensionGroupRegionSelectCnt++ % 2) == 0)
                    {
                        return null;
                    }

                    return new AdHocDimensionGroup();
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

                if (list.Any(e => e.DimensionValue == "TestRegion"))
                {
                    adHocDimensionSaveBulkSqlInsertTestRegionCalled = true;
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
            var atcom = new Atcom();
            atcom.AtcomCompanyAdHocImport(new FileMoved { Client = client });

            // Assert
            Assert.AreEqual(6, adHocDimensionGroupRegionSelectCnt);
            Assert.IsTrue(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertTestRegionCalled);
        }

        [Test]
        public void AtcomCompanyAdHocImport_WorkerSelectInstance_AdHocDimensionGroupReselected()
        {
            // Arrange
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new System.Data.DataTable();
                dt.Columns.Add("MATCHFIELD");
                dt.Columns.Add("TYPE");
                dt.Columns.Add("REGION");
                dt.Columns.Add("COUNTRY");
                return dt;
            };

            var adHocDimensionGroupRegionSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
                {
                    adHocDimensionGroupRegionSelectCnt++;
                return new AdHocDimensionGroup();
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

                if (list.Any(e => e.DimensionValue == "TestRegion"))
                {
                    adHocDimensionSaveBulkSqlInsertTestRegionCalled = true;
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
            var atcom = new Atcom();
            atcom.AtcomCompanyAdHocImport(new FileMoved { Client = client });

            // Assert
            Assert.AreEqual(3, adHocDimensionGroupRegionSelectCnt);
            Assert.IsFalse(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsFalse(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }
    }
}
