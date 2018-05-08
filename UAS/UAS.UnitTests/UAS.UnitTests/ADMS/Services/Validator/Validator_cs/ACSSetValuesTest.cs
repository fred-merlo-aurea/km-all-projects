using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Data;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Core.ADMS.Events;
using System.IO;
using System.Linq;
using FrameworkUADDataAccess = FrameworkUAD.DataAccess;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    /// Unit Tests for <see cref="Validator.Validator.ACS_SetValues"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ACSSetValuesTest : Fakes
    {
        private const string SampleProcessCode = "PETI";
        private const string SampleAcsCode = "SampleAcsCode";
        private const string SampleTestFile = "test.txt";
        private const string SampleKeylineSequenceNumber = "12";
        private const string DeliverabilityCodestringP = "P";
        private const string DeliverabilityCodestringQ = "Q";
        private const string IMAGProductCode = "IMAG";
        private const int AcsMailerId1 = 1;
        private const int AcsMailerId2 = 2;
        private const int SampleSequenceId = 1;
        private const string AcsFileUpdateCommand = "e_AcsFileDetail_Update_Xml";
        private const string ACS_SetValuesMethodName = "ACS_SetValues";
        private TestEntity testEntity;

        /// <summary>
        /// Test SetUp
        /// </summary>
        [SetUp]
        public void Setup()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdUsedFromPublication_UpdatesAcsList()
        {
            // Arrange
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId1,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes();
            SetAcsMailerInfoFakes();
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleAcsCode, result[0].AcsMailerId);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WIthAcsMailerIdAsBXNDYYQ_UpdatesAcsList()
        {
            // Arrange
            var eventMessage = GetEventMessage();
            var acsMailerId = AcsMailerId.BXNDYYQ.ToString();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId, deliverabilityCodestring: DeliverabilityCodestringP);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNDYYQ);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(DeliverabilityCodestringP, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code32, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNDYXS_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNDYXS.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId, deliverabilityCodestring: DeliverabilityCodestringQ);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNDYXS);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(DeliverabilityCodestringQ, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code31, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNDYWV_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNDYWV.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNDYWV);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNQVJK_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNQVJK.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNQVJK);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNPHZV_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNPHZV.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNPHZV);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNPHYX_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNPHYX.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNPHYX);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNPHXZ_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNPHXZ.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNPHXZ);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNRRWP_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNRRWP.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNRRWP);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsBXNQVLF_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = AcsMailerId.BXNQVLF.ToString();
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes(acsCode: acsMailerId, mailerId: (int)AcsMailerId.BXNQVLF);
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(SampleProcessCode, result[0].ProductCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAs999944316_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = "999944316";
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes();
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(IMAGProductCode, result[0].ProductCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        /// <summary>
        /// <see cref="Validator.Validator.ACS_SetValues"/>
        /// </summary>
        [Test]
        public void ACS_SetValues_WithAcsMailerIdAsSomeOtherId_UpdatesAcsList()
        {
            // Arrange
            var acsMailerId = "999999999";
            var eventMessage = GetEventMessage();
            var publicationList = new List<Product>
            {
                new Product
                {
                    IsActive = true,
                    AcsMailerInfoId = AcsMailerId2,
                    PubCode = SampleProcessCode
                }
            };
            SetAcsFileDetailsFakes(acsMailerId: acsMailerId);
            SetAcsMailerInfoFakes();
            var parameters = new object[]
            {
                eventMessage,
                publicationList
            };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ACS_SetValuesMethodName,
                parameters,
                testEntity.Validator) as List<AcsFileDetail>;

            // Assert
            Assert.AreEqual(AcsFileUpdateCommand, sqlCommandText);
            Assert.IsTrue(isSqlCommandExecuted);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(string.Empty, result[0].ProductCode);
            Assert.AreEqual(acsMailerId, result[0].AcsMailerId);
            Assert.AreEqual(SampleProcessCode, result[0].ProcessCode);
            Assert.AreEqual(SampleKeylineSequenceNumber, result[0].KeylineSequenceSerialNumber);
            Assert.AreEqual(SampleSequenceId, result[0].SequenceID);
            Assert.AreEqual(string.Empty, result[0].DeliverabilityCode);
            Assert.AreEqual((int)TransactionCodes.Code21, result[0].TransactionCodeValue);
        }

        private FileMoved GetEventMessage()
        {
            var fileInfo = new FileInfo(SampleTestFile);
            var client = new KMPlatform.Entity.Client();
            var sourceFile = new SourceFile();
            var admsLog = new AdmsLog();
            var isKnownFile = false;
            var threadId = 1;
            return new FileMoved(fileInfo, client, sourceFile, admsLog, isKnownFile, threadId);
        }

        private void SetAcsFileDetailsFakes(string deliverabilityCodestring = null, string acsMailerId = SampleAcsCode)
        {
            FrameworkUADDataAccess.Fakes.ShimAcsFileDetail.GetListSqlCommand = (cmd) =>
            {
                return new List<AcsFileDetail>
                {
                    new AcsFileDetail
                    {
                        AcsMailerId = acsMailerId,
                        ProcessCode = SampleProcessCode,
                        KeylineSequenceSerialNumber = SampleKeylineSequenceNumber,
                        DeliverabilityCode = deliverabilityCodestring ?? string.Empty
                    }
                };
            };
        }

        private void SetAcsMailerInfoFakes(string acsCode = SampleAcsCode,int mailerId = (int)AcsMailerId.BXNDYYQ)
        {
            FrameworkUADDataAccess.Fakes.ShimAcsMailerInfo.GetListSqlCommand = (cmd) =>
            {
                return new List<AcsMailerInfo>
                {
                    new AcsMailerInfo
                    {
                        AcsMailerInfoId = AcsMailerId1,
                        AcsCode = acsCode
                    },
                    new AcsMailerInfo
                    {
                        MailerID = mailerId,
                        AcsMailerInfoId = AcsMailerId2,        
                    },
                };
            };
        }
    }
}
