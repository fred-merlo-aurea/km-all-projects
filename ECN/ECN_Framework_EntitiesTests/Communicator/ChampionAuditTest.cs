using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class ChampionAuditTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ChampionAudit) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var championAuditId = Fixture.Create<int>();
            var auditTime = Fixture.Create<DateTime?>();
            var sampleId = Fixture.Create<int?>();
            var blastIDA = Fixture.Create<int?>();
            var blastIDB = Fixture.Create<int?>();
            var blastIDChampion = Fixture.Create<int?>();
            var clicksA = Fixture.Create<int?>();
            var clicksB = Fixture.Create<int?>();
            var opensA = Fixture.Create<int?>();
            var opensB = Fixture.Create<int?>();
            var bouncesA = Fixture.Create<int?>();
            var bouncesB = Fixture.Create<int?>();
            var blastIDWinning = Fixture.Create<int?>();
            var reason = Fixture.Create<string>();

            // Act
            championAudit.ChampionAuditID = championAuditId;
            championAudit.AuditTime = auditTime;
            championAudit.SampleID = sampleId;
            championAudit.BlastIDA = blastIDA;
            championAudit.BlastIDB = blastIDB;
            championAudit.BlastIDChampion = blastIDChampion;
            championAudit.ClicksA = clicksA;
            championAudit.ClicksB = clicksB;
            championAudit.OpensA = opensA;
            championAudit.OpensB = opensB;
            championAudit.BouncesA = bouncesA;
            championAudit.BouncesB = bouncesB;
            championAudit.BlastIDWinning = blastIDWinning;
            championAudit.Reason = reason;

            // Assert
            championAudit.ChampionAuditID.ShouldBe(championAuditId);
            championAudit.AuditTime.ShouldBe(auditTime);
            championAudit.SampleID.ShouldBe(sampleId);
            championAudit.BlastIDA.ShouldBe(blastIDA);
            championAudit.BlastIDB.ShouldBe(blastIDB);
            championAudit.BlastIDChampion.ShouldBe(blastIDChampion);
            championAudit.ClicksA.ShouldBe(clicksA);
            championAudit.ClicksB.ShouldBe(clicksB);
            championAudit.OpensA.ShouldBe(opensA);
            championAudit.OpensB.ShouldBe(opensB);
            championAudit.BouncesA.ShouldBe(bouncesA);
            championAudit.BouncesB.ShouldBe(bouncesB);
            championAudit.BlastIDWinning.ShouldBe(blastIDWinning);
            championAudit.SendToNonWinner.ShouldBe(true);
            championAudit.Reason.ShouldBe(reason);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (AuditTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_AuditTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameAuditTime = "AuditTime";
            var championAudit = Fixture.Create<ChampionAudit>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameAuditTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(championAudit, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (AuditTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_AuditTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAuditTime = "AuditTimeNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameAuditTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_AuditTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAuditTime = "AuditTime";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameAuditTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDA) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDA_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.BlastIDA = random;

            // Assert
            championAudit.BlastIDA.ShouldBe(random);
            championAudit.BlastIDA.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDA_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.BlastIDA = null;

            // Assert
            championAudit.BlastIDA.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDA_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastIDA = "BlastIDA";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameBlastIDA);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.BlastIDA.ShouldBeNull();
            championAudit.BlastIDA.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDA) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_BlastIDANotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastIDA = "BlastIDANotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameBlastIDA));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDA_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastIDA = "BlastIDA";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameBlastIDA);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDB) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDB_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.BlastIDB = random;

            // Assert
            championAudit.BlastIDB.ShouldBe(random);
            championAudit.BlastIDB.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDB_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.BlastIDB = null;

            // Assert
            championAudit.BlastIDB.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDB_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastIDB = "BlastIDB";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameBlastIDB);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.BlastIDB.ShouldBeNull();
            championAudit.BlastIDB.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDB) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_BlastIDBNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastIDB = "BlastIDBNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameBlastIDB));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDB_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastIDB = "BlastIDB";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameBlastIDB);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDChampion) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDChampion_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.BlastIDChampion = random;

            // Assert
            championAudit.BlastIDChampion.ShouldBe(random);
            championAudit.BlastIDChampion.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDChampion_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.BlastIDChampion = null;

            // Assert
            championAudit.BlastIDChampion.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDChampion_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastIDChampion = "BlastIDChampion";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameBlastIDChampion);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.BlastIDChampion.ShouldBeNull();
            championAudit.BlastIDChampion.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDChampion) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_BlastIDChampionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastIDChampion = "BlastIDChampionNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameBlastIDChampion));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDChampion_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastIDChampion = "BlastIDChampion";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameBlastIDChampion);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDWinning) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDWinning_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.BlastIDWinning = random;

            // Assert
            championAudit.BlastIDWinning.ShouldBe(random);
            championAudit.BlastIDWinning.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDWinning_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.BlastIDWinning = null;

            // Assert
            championAudit.BlastIDWinning.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDWinning_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastIDWinning = "BlastIDWinning";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameBlastIDWinning);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.BlastIDWinning.ShouldBeNull();
            championAudit.BlastIDWinning.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BlastIDWinning) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_BlastIDWinningNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastIDWinning = "BlastIDWinningNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameBlastIDWinning));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BlastIDWinning_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastIDWinning = "BlastIDWinning";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameBlastIDWinning);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BouncesA) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesA_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.BouncesA = random;

            // Assert
            championAudit.BouncesA.ShouldBe(random);
            championAudit.BouncesA.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesA_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.BouncesA = null;

            // Assert
            championAudit.BouncesA.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesA_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBouncesA = "BouncesA";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameBouncesA);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.BouncesA.ShouldBeNull();
            championAudit.BouncesA.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BouncesA) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_BouncesANotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBouncesA = "BouncesANotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameBouncesA));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesA_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBouncesA = "BouncesA";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameBouncesA);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BouncesB) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesB_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.BouncesB = random;

            // Assert
            championAudit.BouncesB.ShouldBe(random);
            championAudit.BouncesB.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesB_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.BouncesB = null;

            // Assert
            championAudit.BouncesB.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesB_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBouncesB = "BouncesB";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameBouncesB);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.BouncesB.ShouldBeNull();
            championAudit.BouncesB.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (BouncesB) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_BouncesBNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBouncesB = "BouncesBNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameBouncesB));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_BouncesB_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBouncesB = "BouncesB";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameBouncesB);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (ChampionAuditID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ChampionAuditID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            championAudit.ChampionAuditID = Fixture.Create<int>();
            var intType = championAudit.ChampionAuditID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (ChampionAuditID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_ChampionAuditIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChampionAuditID = "ChampionAuditIDNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameChampionAuditID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ChampionAuditID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChampionAuditID = "ChampionAuditID";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameChampionAuditID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (ClicksA) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksA_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.ClicksA = random;

            // Assert
            championAudit.ClicksA.ShouldBe(random);
            championAudit.ClicksA.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksA_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.ClicksA = null;

            // Assert
            championAudit.ClicksA.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksA_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameClicksA = "ClicksA";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameClicksA);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.ClicksA.ShouldBeNull();
            championAudit.ClicksA.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (ClicksA) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_ClicksANotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicksA = "ClicksANotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameClicksA));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksA_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicksA = "ClicksA";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameClicksA);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (ClicksB) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksB_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.ClicksB = random;

            // Assert
            championAudit.ClicksB.ShouldBe(random);
            championAudit.ClicksB.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksB_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.ClicksB = null;

            // Assert
            championAudit.ClicksB.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksB_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameClicksB = "ClicksB";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameClicksB);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.ClicksB.ShouldBeNull();
            championAudit.ClicksB.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (ClicksB) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_ClicksBNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicksB = "ClicksBNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameClicksB));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_ClicksB_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicksB = "ClicksB";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameClicksB);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (OpensA) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensA_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.OpensA = random;

            // Assert
            championAudit.OpensA.ShouldBe(random);
            championAudit.OpensA.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensA_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.OpensA = null;

            // Assert
            championAudit.OpensA.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensA_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameOpensA = "OpensA";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameOpensA);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.OpensA.ShouldBeNull();
            championAudit.OpensA.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (OpensA) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_OpensANotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpensA = "OpensANotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameOpensA));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensA_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpensA = "OpensA";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameOpensA);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (OpensB) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensB_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.OpensB = random;

            // Assert
            championAudit.OpensB.ShouldBe(random);
            championAudit.OpensB.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensB_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.OpensB = null;

            // Assert
            championAudit.OpensB.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensB_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameOpensB = "OpensB";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameOpensB);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.OpensB.ShouldBeNull();
            championAudit.OpensB.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (OpensB) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_OpensBNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpensB = "OpensBNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameOpensB));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_OpensB_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpensB = "OpensB";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameOpensB);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (Reason) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Reason_Property_String_Type_Verify_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            championAudit.Reason = Fixture.Create<string>();
            var stringType = championAudit.Reason.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (Reason) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_ReasonNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReason = "ReasonNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameReason));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Reason_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReason = "Reason";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameReason);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (SampleID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_SampleID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();
            var random = Fixture.Create<int>();

            // Act , Set
            championAudit.SampleID = random;

            // Assert
            championAudit.SampleID.ShouldBe(random);
            championAudit.SampleID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_SampleID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var championAudit = Fixture.Create<ChampionAudit>();    

            // Act , Set
            championAudit.SampleID = null;

            // Assert
            championAudit.SampleID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_SampleID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSampleID = "SampleID";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameSampleID);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.SampleID.ShouldBeNull();
            championAudit.SampleID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (SampleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_SampleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleIDNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameSampleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_SampleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleID";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameSampleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (SendToNonWinner) Nullable Property Test
        
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_SendToNonWinner_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSendToNonWinner = "SendToNonWinner";
            var championAudit = Fixture.Create<ChampionAudit>();
            var propertyInfo = championAudit.GetType().GetProperty(propertyNameSendToNonWinner);

            // Act , Set
            propertyInfo.SetValue(championAudit, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            championAudit.SendToNonWinner.ShouldBeNull();
            championAudit.SendToNonWinner.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAudit) => Property (SendToNonWinner) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_Class_Invalid_Property_SendToNonWinnerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendToNonWinner = "SendToNonWinnerNotPresent";
            var championAudit  = Fixture.Create<ChampionAudit>();

            // Act , Assert
            Should.NotThrow(() => championAudit.GetType().GetProperty(propertyNameSendToNonWinner));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAudit_SendToNonWinner_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendToNonWinner = "SendToNonWinner";
            var championAudit  = Fixture.Create<ChampionAudit>();
            var propertyInfo  = championAudit.GetType().GetProperty(propertyNameSendToNonWinner);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ChampionAudit) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAudit_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ChampionAudit());
        }

        #endregion

        #region General Constructor : Class (ChampionAudit) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAudit_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfChampionAudit = Fixture.CreateMany<ChampionAudit>(2).ToList();
            var firstChampionAudit = instancesOfChampionAudit.FirstOrDefault();
            var lastChampionAudit = instancesOfChampionAudit.Last();

            // Act, Assert
            firstChampionAudit.ShouldNotBeNull();
            lastChampionAudit.ShouldNotBeNull();
            firstChampionAudit.ShouldNotBeSameAs(lastChampionAudit);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAudit_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstChampionAudit = new ChampionAudit();
            var secondChampionAudit = new ChampionAudit();
            var thirdChampionAudit = new ChampionAudit();
            var fourthChampionAudit = new ChampionAudit();
            var fifthChampionAudit = new ChampionAudit();
            var sixthChampionAudit = new ChampionAudit();

            // Act, Assert
            firstChampionAudit.ShouldNotBeNull();
            secondChampionAudit.ShouldNotBeNull();
            thirdChampionAudit.ShouldNotBeNull();
            fourthChampionAudit.ShouldNotBeNull();
            fifthChampionAudit.ShouldNotBeNull();
            sixthChampionAudit.ShouldNotBeNull();
            firstChampionAudit.ShouldNotBeSameAs(secondChampionAudit);
            thirdChampionAudit.ShouldNotBeSameAs(firstChampionAudit);
            fourthChampionAudit.ShouldNotBeSameAs(firstChampionAudit);
            fifthChampionAudit.ShouldNotBeSameAs(firstChampionAudit);
            sixthChampionAudit.ShouldNotBeSameAs(firstChampionAudit);
            sixthChampionAudit.ShouldNotBeSameAs(fourthChampionAudit);
        }

        #endregion

        #region General Constructor : Class (ChampionAudit) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAudit_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var championAuditId = -1;
            var reason = "clicks";

            // Act
            var championAudit = new ChampionAudit();

            // Assert
            championAudit.ChampionAuditID.ShouldBe(championAuditId);
            championAudit.AuditTime.ShouldBeNull();
            championAudit.SampleID.ShouldBeNull();
            championAudit.BlastIDA.ShouldBeNull();
            championAudit.BlastIDB.ShouldBeNull();
            championAudit.BlastIDChampion.ShouldBeNull();
            championAudit.ClicksA.ShouldBeNull();
            championAudit.ClicksB.ShouldBeNull();
            championAudit.OpensA.ShouldBeNull();
            championAudit.OpensB.ShouldBeNull();
            championAudit.BouncesA.ShouldBeNull();
            championAudit.BouncesB.ShouldBeNull();
            championAudit.BlastIDWinning.ShouldBeNull();
            championAudit.SendToNonWinner.ShouldBe(true);
            championAudit.Reason.ShouldBe(reason);
        }

        #endregion

        #endregion

        #endregion
    }
}