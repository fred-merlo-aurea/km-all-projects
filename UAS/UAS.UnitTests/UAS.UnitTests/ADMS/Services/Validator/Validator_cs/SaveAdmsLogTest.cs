using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using ClientEntity = KMPlatform.Entity.Client;
using LookupEnums = FrameworkUAD_Lookup.Enums;
using ServiceSavedSubscriber = FrameworkUAD.ServiceResponse.SavedSubscriber;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SaveAdmsLogTest
    {
        private const int AdmsLogId = 1;
        private const string ApiKey = "API";
        private const string UadWsAddSubscriberKey = "UAD_WS_AddSubscriber";

        private PrivateObject _validatorPrivateObject;
        private ClientEntity _clientEntity;
        private ServiceSavedSubscriber _savedSubscriber;
        private SourceFile _sourceFile;

        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();

            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));
            _clientEntity = new ClientEntity();
            _savedSubscriber = new ServiceSavedSubscriber();
            _sourceFile = new SourceFile();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [Test]
        public void SaveAdmsLog_WhenClientEntityIsNull_ThrowsException()
        {
            // Arrange
            _clientEntity = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SaveAdmsLogMethod, _clientEntity, _savedSubscriber, _sourceFile));
        }

        [Test]
        public void SaveAdmsLog_WhenServiceSavedSubscriberIsNull_ThrowsException()
        {
            // Arrange
            _savedSubscriber = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SaveAdmsLogMethod, _clientEntity, _savedSubscriber, _sourceFile));
        }

        [Test]
        public void SaveAdmsLog_WhenSourceFileIsNull_ThrowsException()
        {
            // Arrange
            _sourceFile = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SaveAdmsLogMethod, _clientEntity, _savedSubscriber, _sourceFile));
        }

        [Test]
        public void SaveAdmsLog_ShouldCreateAdmsLog_ReturnsAdmsLog()
        {
            // Arrange
            _clientEntity.ClientID = 10;
            _savedSubscriber.ProcessCode = "Sample Process Code";
            _sourceFile.SourceFileID = 20;

            AdmsLog admsResult;
            ShimAdmsLog.AllInstances.SaveAdmsLog = (log, admsLog) =>
            {
                admsResult = admsLog;
                return AdmsLogId;
            };

            // Act
            var result = _validatorPrivateObject.Invoke(SaveAdmsLogMethod, _clientEntity, _savedSubscriber, _sourceFile) as AdmsLog;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ClientId.ShouldBe(_clientEntity.ClientID),
                () => result.StatusMessage.ShouldBe(LookupEnums.FileStatusType.Detected.ToString()),
                () => result.AdmsStepId.ShouldBe(0),
                () => result.DateCreated.ShouldNotBeNull(),
                () => result.FileLogDetails.ShouldBeOfType<List<FileLog>>(),
                () => result.FileNameExact.ShouldBe(UadWsAddSubscriberKey),
                () => result.FileStart.ShouldNotBeNull(),
                () => result.FileStatusId.ShouldBe(0),
                () => result.ImportFile.ShouldBeNull(),
                () => result.ProcessCode.ShouldBe(_savedSubscriber.ProcessCode),
                () => result.ProcessingStatusId.ShouldBe(0),
                () => result.ExecutionPointId.ShouldBe(0),
                () => result.RecordSource.ShouldBe(ApiKey),
                () => result.SourceFileId.ShouldBe(_sourceFile.SourceFileID),
                () => result.ThreadId.ShouldBe(Thread.CurrentThread.ManagedThreadId),
                () => result.AdmsLogId.ShouldBe(AdmsLogId),
                () => result.ShouldBeOfType<AdmsLog>());
        }
    }
}
