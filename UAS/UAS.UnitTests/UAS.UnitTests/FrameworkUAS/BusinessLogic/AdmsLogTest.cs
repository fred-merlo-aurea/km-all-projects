using System;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.BusinessLogic;
using FrameworkUAS.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using EntityFileLog = FrameworkUAS.Entity.FileLog;
using Enums = FrameworkUAD_Lookup.Enums;
using ShimFileLog = FrameworkUAS.BusinessLogic.Fakes.ShimFileLog;

namespace UAS.UnitTests.FrameworkUAS.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdmsLogTest
    {
        private const string ProcessCode = "MyProcessCode";
        private const int FinalRecordCount = 3;
        private const int FinalProfileCount = 7;
        private const int FinalDemoCount = 15;
        private const int IgnoredRecordCount = 31;
        private const int IgnoredProfileCount = 63;
        private const int IgnoredDemoCount = 127;
        private const int MatchedRecordCount = 255;
        private const int UadConsensusCount = 511;
        private const int SourceFileId = 1023;
        private const int UpdatedByUserId = 1;
        private const int UserId = 32;
        private const int SuccessResult = 42;
        private const string CurrentStatus = "MyCurrentStatus";

        private IDisposable _shimContext;

        [SetUp]
        public void SetUp()
        {
            _shimContext = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext?.Dispose();
        }

        [Test]
        public void UpdateTransformedCounts_ValidParameter_DataAccessCalled()
        {
            // Arrange
            var admsLog = new AdmsLog();
            var actualProcessCode = string.Empty;
            var actualFinalRecordCount = 0;
            var actualFinalProfileCount = 0;
            var actualFinalDemoCount = 0;
            var actualUpdatedByUserId = 0;
            var actualCreateLog = true;
            var actualSourceFileId = 0;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) => throw new InvalidOperationException();

            ShimAdmsLog.UpdateTransformedCountsStringInt32Int32Int32Int32BooleanInt32 = (
                processCode,
                finalRecordCount,
                finalProfileCount,
                finalDemoCount,
                updatedByUserId,
                createLog,
                sourceFileId) =>
                {
                    actualProcessCode = processCode;
                    actualFinalRecordCount = finalRecordCount;
                    actualFinalProfileCount = finalProfileCount;
                    actualFinalDemoCount = finalDemoCount;
                    actualUpdatedByUserId = updatedByUserId;
                    actualCreateLog = createLog;
                    actualSourceFileId = sourceFileId;
                    return true;
                };

            // Act
            var success = admsLog.UpdateTransformedCounts(
                ProcessCode,
                FinalRecordCount,
                FinalProfileCount,
                FinalDemoCount);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualProcessCode.ShouldBe(ProcessCode),
                () => actualFinalRecordCount.ShouldBe(FinalRecordCount),
                () => actualFinalProfileCount.ShouldBe(FinalProfileCount),
                () => actualFinalDemoCount.ShouldBe(FinalDemoCount),
                () => actualUpdatedByUserId.ShouldBe(1),
                () => actualCreateLog.ShouldBe(false),
                () => actualSourceFileId.ShouldBe(0));
        }
        
        [Test]
        public void UpdateTransformedCounts_ValidParameter_LogWritten()
        {
            // Arrange
            var admsLog = new AdmsLog();
            EntityFileLog actualFileLog = null;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                actualFileLog = fileLog;
                return true;
            };

            ShimAdmsLog.UpdateTransformedCountsStringInt32Int32Int32Int32BooleanInt32 = (
                processCode,
                finalRecordCount,
                finalProfileCount,
                finalDemoCount,
                updatedByUserId,
                createLog,
                sourceFileId) => true;

            // Act
            var success = admsLog.UpdateTransformedCounts(
                ProcessCode,
                FinalRecordCount,
                FinalProfileCount,
                FinalDemoCount,
                UpdatedByUserId,
                true,
                SourceFileId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualFileLog.ProcessCode.ShouldBe(ProcessCode),
                () => actualFileLog.SourceFileID.ShouldBe(SourceFileId),
                () => actualFileLog.Message.ShouldBe(
                    $"Update AdmsLog Transformed Counts - Records:{FinalRecordCount}  Profiles:{FinalProfileCount}  Demos:{FinalDemoCount}"));
        } 
        
        [Test]
        public void UpdateFinalCounts_ValidParameter_DataAccessCalled()
        {
            // Arrange
            var admsLog = new AdmsLog();
            var actualProcessCode = string.Empty;
            var actualFinalRecordCount = 0;
            var actualFinalProfileCount = 0;
            var actualFinalDemoCount = 0;
            var actualMatchedRecordCount = 0;
            var actualUadConsensusCount = 0;
            var actualUpdatedByUserId = 0;
            var actualCreateLog = true;
            var actualSourceFileId = 0;
            var actualArchiveCount = 0;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) => throw new InvalidOperationException();

            ShimAdmsLog.UpdateFinalCountsStringInt32Int32Int32Int32Int32Int32BooleanInt32Int32 = (
                processCode,
                finalRecordCount,
                finalProfileCount,
                finalDemoCount,
                matchedRecordCount,
                uadConsensusCount,
                updatedByUserId,
                createLog,
                sourceFileId,
                archiveCount) =>
                {
                    actualProcessCode = processCode;
                    actualFinalRecordCount = finalRecordCount;
                    actualFinalProfileCount = finalProfileCount;
                    actualFinalDemoCount = finalDemoCount;
                    actualMatchedRecordCount = matchedRecordCount;
                    actualUadConsensusCount = uadConsensusCount;
                    actualUpdatedByUserId = updatedByUserId;
                    actualCreateLog = createLog;
                    actualSourceFileId = sourceFileId;
                    actualArchiveCount = archiveCount;
                    return true;
                };

            // Act
            var success = admsLog.UpdateFinalCounts(
                ProcessCode,
                FinalRecordCount,
                FinalProfileCount,
                FinalDemoCount,
                MatchedRecordCount,
                UadConsensusCount);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualProcessCode.ShouldBe(ProcessCode),
                () => actualFinalRecordCount.ShouldBe(FinalRecordCount),
                () => actualFinalProfileCount.ShouldBe(FinalProfileCount),
                () => actualFinalDemoCount.ShouldBe(FinalDemoCount),
                () => actualMatchedRecordCount.ShouldBe(MatchedRecordCount),
                () => actualUadConsensusCount.ShouldBe(UadConsensusCount),
                () => actualUpdatedByUserId.ShouldBe(1),
                () => actualCreateLog.ShouldBe(false),
                () => actualSourceFileId.ShouldBe(0),
                () => actualArchiveCount.ShouldBe(0));
        }
        
        [Test]
        public void UpdateFinalCounts_ValidParameter_LogWritten()
        {
            // Arrange
            var admsLog = new AdmsLog();
            EntityFileLog actualFileLog = null;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
                {
                    actualFileLog = fileLog;
                    return true;
                };

            ShimAdmsLog.UpdateFinalCountsStringInt32Int32Int32Int32Int32Int32BooleanInt32Int32 = (
                processCode,
                finalRecordCount,
                finalProfileCount,
                finalDemoCount,
                matchedRecordCount,
                uadConsensusCount,
                updatedByUserId,
                createLog,
                sourceFileId,
                archiveCount) => true;

            // Act
            var success = admsLog.UpdateFinalCounts(
                ProcessCode,
                FinalRecordCount,
                FinalProfileCount,
                FinalDemoCount,
                MatchedRecordCount,
                UadConsensusCount,
                UpdatedByUserId,
                true,
                SourceFileId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualFileLog.ProcessCode.ShouldBe(ProcessCode),
                () => actualFileLog.SourceFileID.ShouldBe(SourceFileId),
                () => actualFileLog.Message.ShouldBe(
                    $"Update AdmsLog Final Counts - Records:{FinalRecordCount}  Profiles:{FinalProfileCount}  Demos:{FinalDemoCount}"));
        }

        [Test]
        public void UpdateFinalCountsAfterProcessToLive_ValidParameters_DataAccessCalled()
        {
            // Arrange
            var admsLog = new AdmsLog();
            var actualProcessCode = string.Empty;
            var actualFinalRecordCount = 0;
            var actualFinalProfileCount = 0;
            var actualFinalDemoCount = 0;
            var actualIgnoredRecordCount = 0;
            var actualIgnoredProfileCount = 0;
            var actualIgnoredDemoCount = 0;
            var actualMatchedRecordCount = 0;
            var actualUadConsensusCount = 0;
            var actualUpdatedByUserId = 0;
            var actualCreateLog = true;
            var actualSourceFileId = 0;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) => throw new InvalidOperationException();

            ShimAdmsLog.UpdateFinalCountsAfterProcessToLiveStringInt32Int32Int32Int32Int32Int32Int32Int32Int32BooleanInt32 = (
                processCode,
                finalRecordCount,
                finalProfileCount,
                finalDemoCount,
                ignoredRecordCount,
                ingnoredProfileCount,
                ignoredDemoCount,
                matchedRecordCount,
                uadConsensusCount,
                updatedByUserId,
                createLog,
                sourceFileId) =>
                {
                    actualProcessCode = processCode;
                    actualFinalRecordCount = finalRecordCount;
                    actualFinalProfileCount = finalProfileCount;
                    actualFinalDemoCount = finalDemoCount;
                    actualIgnoredRecordCount = ignoredRecordCount;
                    actualIgnoredProfileCount = ingnoredProfileCount;
                    actualIgnoredDemoCount = ignoredDemoCount;
                    actualMatchedRecordCount = matchedRecordCount;
                    actualUadConsensusCount = uadConsensusCount;
                    actualUpdatedByUserId = updatedByUserId;
                    actualCreateLog = createLog;
                    actualSourceFileId = sourceFileId;
                    return true;
                };

            // Act
            var success = admsLog.UpdateFinalCountsAfterProcessToLive(
                ProcessCode,
                FinalRecordCount,
                FinalProfileCount,
                FinalDemoCount,
                IgnoredRecordCount,
                IgnoredProfileCount,
                IgnoredDemoCount,
                MatchedRecordCount,
                UadConsensusCount);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualProcessCode.ShouldBe(ProcessCode),
                () => actualFinalRecordCount.ShouldBe(FinalRecordCount),
                () => actualFinalProfileCount.ShouldBe(FinalProfileCount),
                () => actualFinalDemoCount.ShouldBe(FinalDemoCount),
                () => actualIgnoredRecordCount.ShouldBe(IgnoredRecordCount),
                () => actualIgnoredProfileCount.ShouldBe(IgnoredProfileCount),
                () => actualIgnoredDemoCount.ShouldBe(IgnoredDemoCount),
                () => actualMatchedRecordCount.ShouldBe(MatchedRecordCount),
                () => actualUadConsensusCount.ShouldBe(UadConsensusCount),
                () => actualUpdatedByUserId.ShouldBe(1),
                () => actualCreateLog.ShouldBe(false),
                () => actualSourceFileId.ShouldBe(0));
        }
        
        [Test]
        public void UpdateFinalCountsAfterProcessToLive_ValidParameters_LogWritten()
        {
            // Arrange
            var admsLog = new AdmsLog();
            EntityFileLog actualFileLog = null;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                actualFileLog = fileLog;
                return true;
            };

            ShimAdmsLog.UpdateFinalCountsAfterProcessToLiveStringInt32Int32Int32Int32Int32Int32Int32Int32Int32BooleanInt32 = (
                processCode,
                finalRecordCount,
                finalProfileCount,
                finalDemoCount,
                ignoredRecordCount,
                ingnoredProfileCount,
                ignoredDemoCount,
                matchedRecordCount,
                uadConsensusCount,
                _,
                __,
                ___) => true;

            // Act
            var success = admsLog.UpdateFinalCountsAfterProcessToLive(
                ProcessCode,
                FinalRecordCount,
                FinalProfileCount,
                FinalDemoCount,
                IgnoredRecordCount,
                IgnoredProfileCount,
                IgnoredDemoCount,
                MatchedRecordCount,
                UadConsensusCount,
                UpdatedByUserId,
                true,
                SourceFileId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualFileLog.ProcessCode.ShouldBe(ProcessCode),
                () => actualFileLog.SourceFileID.ShouldBe(SourceFileId),
                () => actualFileLog.Message.ShouldBe(
                    $"Update AdmsLog Finished Counts - Records:{FinalRecordCount}  Profiles:{FinalProfileCount}  Demos:{FinalDemoCount}"));
        } 
        
        [Test]
        public void UpdateExecutionPoint_ValidParameters_DataAccessCalled()
        {
            // Arrange
            var admsLog = new AdmsLog();
            var actualProcessCode = string.Empty;
            var actualExecutionPointType = string.Empty;
            var actualUserId = 0;
            var actulStatusMessage = string.Empty;
           
            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) => throw new InvalidOperationException();

            ShimAdmsLog.UpdateExecutionPointStringStringInt32String = (
                processCode,
                executionPointType,
                userId,
                statusMessage) =>
                {
                    actualProcessCode = processCode;
                    actualExecutionPointType = executionPointType;
                    actualUserId = userId;
                    actulStatusMessage = statusMessage;
                    return SuccessResult;
                };

            // Act
            var result = admsLog.UpdateExecutionPoint(ProcessCode, Enums.ExecutionPointType.ADMS_ClientMethods, UserId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(SuccessResult),
                () => actualProcessCode.ShouldBe(ProcessCode),
                () => actualExecutionPointType.ShouldBe("ADMS ClientMethods"),
                () => actualUserId.ShouldBe(UserId),
                () => actulStatusMessage.ShouldBe(string.Empty));
        }
        
        [Test]
        public void UpdateExecutionPoint_ValidParameters_LogWritten()
        {
            // Arrange
            var admsLog = new AdmsLog();
            EntityFileLog actualFileLog = null;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                actualFileLog = fileLog;
                return true;
            };

            ShimAdmsLog.UpdateExecutionPointStringStringInt32String = (_, __, ___, ____) => SuccessResult;

            // Act
            var result = admsLog.UpdateExecutionPoint(
                ProcessCode,
                Enums.ExecutionPointType.ADMS_ClientMethods,
                UserId,
                CurrentStatus,
                true,
                SourceFileId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(SuccessResult),
                () => actualFileLog.ProcessCode.ShouldBe(ProcessCode),
                () => actualFileLog.SourceFileID.ShouldBe(SourceFileId),
                () => actualFileLog.Message.ShouldBe(
                    "Update AdmsLog - set ExecutionPointId:ADMS_ClientMethods  Current Status: MyCurrentStatus"));
        }
        
        [Test]
        public void UpdateProcessingStatus_ValidParameters_DataAccessCalled()
        {
            // Arrange
            var admsLog = new AdmsLog();
            var actualProcessCode = string.Empty;
            var actualProcessingStatus = string.Empty;
            var actualUserId = 0;
            var actulStatusMessage = string.Empty;
           
            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) => throw new InvalidOperationException();

            ShimAdmsLog.UpdateProcessingStatusStringStringInt32String = (
                processCode,
                processingStatus,
                userId,
                statusMessage) =>
                {
                    actualProcessCode = processCode;
                    actualProcessingStatus = processingStatus;
                    actualUserId = userId;
                    actulStatusMessage = statusMessage;
                    return SuccessResult;
                };

            // Act
            var result = admsLog.UpdateProcessingStatus(ProcessCode, Enums.ProcessingStatusType.In_Matching, UserId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(SuccessResult),
                () => actualProcessCode.ShouldBe(ProcessCode),
                () => actualProcessingStatus.ShouldBe("In Matching"),
                () => actualUserId.ShouldBe(UserId),
                () => actulStatusMessage.ShouldBe(string.Empty));
        }
        
        [Test]
        public void UpdateProcessingStatus_ValidParameters_LogWritten()
        {
            // Arrange
            var admsLog = new AdmsLog();
            EntityFileLog actualFileLog = null;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                actualFileLog = fileLog;
                return true;
            };

            ShimAdmsLog.UpdateProcessingStatusStringStringInt32String = (_, __, ___, ____) => SuccessResult;

            // Act
            var result = admsLog.UpdateProcessingStatus(
                ProcessCode,
                Enums.ProcessingStatusType.In_Matching,
                UserId,
                CurrentStatus,
                true,
                SourceFileId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(SuccessResult),
                () => actualFileLog.ProcessCode.ShouldBe(ProcessCode),
                () => actualFileLog.SourceFileID.ShouldBe(SourceFileId),
                () => actualFileLog.Message.ShouldBe(
                    "Update AdmsLog - set ProcessingStatusId:In_Matching  Current Status: MyCurrentStatus"));
        }
    }
}
