using System;
using System.Collections.Generic;
using System.Data;
using ecn.blastengine;
using ecn.common.classes.Fakes;
using ecn.communicator.classes;
using ECN.TestHelpers;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.blastengine.Fakes;
using CommonObjects = ECN_Framework_Common.Objects;
using CommonFakes = KM.Common.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework.Common.Fakes;
using ecn.communicator.classes.Fakes;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ECN.BlastEngine.Tests
{
    public partial class BlastEngineTest
    {
        private const string METHOD_HandleSingleBlast = "HandleSingleBlast";

        [Test]
        public void HandleSingleBlast_OnException_LogError()
        {
            // Arrange
            var emailFn = new EmailFunctions();
            var blastEngine = new ECNBlastEngine();
            var exceptionLogged = false;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new Exception("Exception from SqlCommand");
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            
            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_HandleSingleBlast,
                new object[] { emailFn }, blastEngine);

            // Assert
            Assert.IsTrue(exceptionLogged);
        }

        [Test]
        public void HandleSingleBlast_OnECNException_LogError()
        {
            // Arrange
            var emailFn = new EmailFunctions();
            var blastEngine = new ECNBlastEngine();
            var exceptionLogged = false;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                    throw new CommonObjects::ECNException(new List<CommonObjects::ECNError>()
                {
                   new CommonObjects::ECNError
                   {
                       ErrorMessage = "HandleSingleBlast",
                   }
                });
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_HandleSingleBlast,
                new object[] { emailFn }, blastEngine);

            // Assert
            Assert.IsTrue(exceptionLogged);
        }

        [Test]
        public void HandleSingleBlast_RefBlastIsNull_LogError()
        {
            // Arrange
            var emailFn = new EmailFunctions();
            var blastEngine = new ECNBlastEngine();
            var blastUpdated = false;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) => 
                {
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("EmailID", typeof(int));
                table.Columns.Add("BlastID", typeof(int));
                table.Columns.Add("LayoutPlanID", typeof(int));
                table.Columns.Add("BlastSingleID", typeof(int));
                table.Columns.Add("GroupID", typeof(int));
                table.Columns.Add("refBlastID", typeof(int));
                var row = table.NewRow();
                row[0] = 1;
                row[1] = 2;
                row[2] = 3;
                row[3] = 4;
                row[4] = 5;
                row[5] = DBNull.Value;
                table.Rows.Add(row);
                return table;
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimDataFunctions.ExecuteString = (str) => 
            {
                blastUpdated = true;
                return 1;
            };
            ShimDataFunctions.ExecuteScalarString = (str) =>
            {
                return "2";
            };

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_HandleSingleBlast,
                new object[] { emailFn }, blastEngine);

            // Assert
            Assert.IsTrue(blastUpdated);
        }

        [Test]
        public void HandleSingleBlast_RefBlastNotNull_ReachEnd()
        {
            // Arrange
            var emailFn = new EmailFunctions();
            var blastEngine = new ECNBlastEngine();
            var exceptionLogged = false;
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            var blast = new BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            var reachedEnd = false;

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    return 1;
                };
            CommonFakes::ShimFileFunctions.LogActivityBooleanStringString =
                (output, activity, suffix) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("EmailID", typeof(int));
                table.Columns.Add("BlastID", typeof(int));
                table.Columns.Add("LayoutPlanID", typeof(int));
                table.Columns.Add("BlastSingleID", typeof(int));
                table.Columns.Add("GroupID", typeof(int));
                table.Columns.Add("refBlastID", typeof(int));
                var row = table.NewRow();
                row[0] = 1;
                row[1] = 2;
                row[2] = 3;
                row[3] = 4;
                row[4] = 5;
                row[5] = 6;
                table.Rows.Add(row);
                return table;
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimDataFunctions.ExecuteString = (str) =>
            {
                return 1;
            };
            ShimDataFunctions.ExecuteScalarString = (str) =>
            {
                if (str.Contains("DELETE FROM BlastSingles WHERE BlastSingleID"))
                {
                    reachedEnd = true;
                }
                return "1";
            };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            ShimChannelCheck.ConstructorInt32 = (chn, custId) => { };
            ShimBlast.GetMasterRefBlastInt32Int32UserBoolean = (id, emailId, user, child) =>
            {
                return blast;
            };
            
            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_HandleSingleBlast,
                new object[] { emailFn }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }

        [Test]
        public void HandleSingleBlast_RefTriggerIDIsZero_ReachEnd()
        {
            // Arrange
            var emailFn = new EmailFunctions();
            var blastEngine = new ECNBlastEngine();
            var exceptionLogged = false;
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            appSettings.Add("Communicator_VirtualPath", "TestString");
            var blast = new BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            var reachedEnd = false;

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    return 1;
                };
            CommonFakes::ShimFileFunctions.LogActivityBooleanStringString =
                (output, activity, suffix) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("EmailID", typeof(int));
                table.Columns.Add("BlastID", typeof(int));
                table.Columns.Add("LayoutPlanID", typeof(int));
                table.Columns.Add("BlastSingleID", typeof(int));
                table.Columns.Add("GroupID", typeof(int));
                table.Columns.Add("refBlastID", typeof(int));
                var row = table.NewRow();
                row[0] = 1;
                row[1] = 2;
                row[2] = 3;
                row[3] = 4;
                row[4] = 5;
                row[5] = 6;
                table.Rows.Add(row);
                return table;
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimDataFunctions.ExecuteString = (str) =>
            {
                if (str.Contains("update [blast] set attempttotal="))
                {
                    reachedEnd = true;
                }
                return 1;
            };
            ShimDataFunctions.ExecuteScalarString = (str) =>
            {
                if (str.Contains("SELECT RefTriggerID FROM TriggerPlans WHERE TriggerPlanID = "))
                {
                    return "0";
                }
                return "1";
            };
            ShimDataFunctions.ExecuteScalarStringString = (str, cmd) =>
            {
                return "1";
            };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            ShimChannelCheck.AllInstances.getHostName = (ch) => "TestHostName";
            ShimChannelCheck.AllInstances.getBounceDomain = (ch) => "TestDomain";
            ShimChannelCheck.ConstructorInt32 = (chn, custId) => { };
            ShimBlast.GetMasterRefBlastInt32Int32UserBoolean = (id, emailId, user, child) =>
            {
                return blast;
            };
            ShimEmailFunctions.AllInstances.SendBlastSingleBlastInt32Int32StringStringString =
                (fn, obj, email, group, setting, hostname, domain) => { };

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_HandleSingleBlast,
                new object[] { emailFn }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }

        [Test]
        public void HandleSingleBlast_RefBlastNotNullOpensCountIsZero_ReachEnd()
        {
            // Arrange
            var emailFn = new EmailFunctions();
            var blastEngine = new ECNBlastEngine();
            var exceptionLogged = false;
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            appSettings.Add("Communicator_VirtualPath", "TestString");
            var blast = new BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            var reachedEnd = false;

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    return 1;
                };
            CommonFakes::ShimFileFunctions.LogActivityBooleanStringString =
                (output, activity, suffix) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("EmailID", typeof(int));
                table.Columns.Add("BlastID", typeof(int));
                table.Columns.Add("LayoutPlanID", typeof(int));
                table.Columns.Add("BlastSingleID", typeof(int));
                table.Columns.Add("GroupID", typeof(int));
                table.Columns.Add("refBlastID", typeof(int));
                var row = table.NewRow();
                row[0] = 1;
                row[1] = 2;
                row[2] = 3;
                row[3] = 4;
                row[4] = 5;
                row[5] = 6;
                table.Rows.Add(row);
                return table;
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimDataFunctions.ExecuteString = (str) =>
            {
                return 1;
            };
            ShimDataFunctions.ExecuteScalarString = (str) =>
            {
                if (str.Contains("SELECT COUNT(EmailID) AS 'OpensCount'"))
                {
                    if (str.Contains("AND ActionTypeCode = 'open'"))
                    {
                        reachedEnd = true;
                    }
                    return "0";
                }
                return "1";
            };
            ShimDataFunctions.ExecuteScalarStringString = (str, cmd) =>
            {
                return "1";
            };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            ShimChannelCheck.AllInstances.getHostName = (ch) => "TestHostName";
            ShimChannelCheck.AllInstances.getBounceDomain = (ch) => "TestDomain";
            ShimChannelCheck.ConstructorInt32 = (chn, custId) => { };
            ShimBlast.GetMasterRefBlastInt32Int32UserBoolean = (id, emailId, user, child) =>
            {
                return blast;
            };
            ShimEmailFunctions.AllInstances.SendBlastSingleBlastInt32Int32StringStringString =
                (fn, obj, email, group, setting, hostname, domain) => { };

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_HandleSingleBlast,
                new object[] { emailFn }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }
    }
}
