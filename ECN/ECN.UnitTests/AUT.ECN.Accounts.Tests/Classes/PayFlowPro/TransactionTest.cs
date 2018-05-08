using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.classes.PayFlowPro;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Classes.PayFlowPro
{
    [TestFixture]
    public class TransactionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (Transaction) => Method (FindTransactionByProcessedDate) (Return Type :  Transaction) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Transaction_FindTransactionByProcessedDate_Static_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var transactions = Fixture.Create<ArrayList>();
            var expectedProcessDate = Fixture.Create<DateTime>();

            // Act, Assert
            Should.NotThrow(() => Transaction.FindTransactionByProcessedDate(transactions, expectedProcessDate));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Transaction_FindTransactionByProcessedDate_Static_Method_With_2_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var transactions = Fixture.Create<ArrayList>();
            var expectedProcessDate = Fixture.Create<DateTime>();

            // Act
            Func<Transaction> findTransactionByProcessedDate1 = () => Transaction.FindTransactionByProcessedDate(transactions, expectedProcessDate);
            Func<Transaction> findTransactionByProcessedDate2 = () => Transaction.FindTransactionByProcessedDate(transactions, expectedProcessDate);
            var result1 = findTransactionByProcessedDate1();
            var result2 = findTransactionByProcessedDate2();
            var target1 = findTransactionByProcessedDate1.Target;
            var target2 = findTransactionByProcessedDate2.Target;

            // Assert
            findTransactionByProcessedDate1.ShouldNotBeNull();
            findTransactionByProcessedDate2.ShouldNotBeNull();
            findTransactionByProcessedDate1.ShouldNotBe(findTransactionByProcessedDate2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => findTransactionByProcessedDate1.Invoke());
            Should.NotThrow(() => findTransactionByProcessedDate2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Transaction_FindTransactionByProcessedDate_Static_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var transactions = Fixture.Create<ArrayList>();
            var expectedProcessDate = Fixture.Create<DateTime>();
            Object[] parametersOfFindTransactionByProcessedDate = {transactions, expectedProcessDate};
            System.Exception exception, invokeException;
            var transaction  = CreateAnalyzer.CreateOrReturnStaticInstance<Transaction>(Fixture, out exception);
            var methodName = "FindTransactionByProcessedDate";

            // Act
            var findTransactionByProcessedDateMethodInfo1 = transaction.GetType().GetMethod(methodName);
            var findTransactionByProcessedDateMethodInfo2 = transaction.GetType().GetMethod(methodName);
            var returnType1 = findTransactionByProcessedDateMethodInfo1.ReturnType;
            var returnType2 = findTransactionByProcessedDateMethodInfo2.ReturnType;

            // Assert
            parametersOfFindTransactionByProcessedDate.ShouldNotBeNull();
            transaction.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            findTransactionByProcessedDateMethodInfo1.ShouldNotBeNull();
            findTransactionByProcessedDateMethodInfo2.ShouldNotBeNull();
            findTransactionByProcessedDateMethodInfo1.ShouldBe(findTransactionByProcessedDateMethodInfo2);
            if(findTransactionByProcessedDateMethodInfo1.DoesInvokeThrow(transaction, out invokeException, parametersOfFindTransactionByProcessedDate))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOfFindTransactionByProcessedDate), exceptionType: invokeException.GetType());
                Should.Throw(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOfFindTransactionByProcessedDate), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOfFindTransactionByProcessedDate));
                Should.NotThrow(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOfFindTransactionByProcessedDate));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Transaction_FindTransactionByProcessedDate_Static_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var transactions = Fixture.Create<ArrayList>();
            var expectedProcessDate = Fixture.Create<DateTime>();
            Object[] parametersOutRanged = {transactions, expectedProcessDate, null};
            Object[] parametersInDifferentNumber = {transactions};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var transaction  = CreateAnalyzer.CreateOrReturnStaticInstance<Transaction>(Fixture, out exception);
            var methodName = "FindTransactionByProcessedDate";

            if(transaction != null)
            {
                // Act
                var findTransactionByProcessedDateMethodInfo1 = transaction.GetType().GetMethod(methodName);
                var findTransactionByProcessedDateMethodInfo2 = transaction.GetType().GetMethod(methodName);
                var returnType1 = findTransactionByProcessedDateMethodInfo1.ReturnType;
                var returnType2 = findTransactionByProcessedDateMethodInfo2.ReturnType;
                var result1 = findTransactionByProcessedDateMethodInfo1.GetResultMethodInfo<Transaction, Transaction>(transaction, out exception1, parametersOutRanged);
                var result2 = findTransactionByProcessedDateMethodInfo2.GetResultMethodInfo<Transaction, Transaction>(transaction, out exception2, parametersOutRanged);
                var result3 = findTransactionByProcessedDateMethodInfo1.GetResultMethodInfo<Transaction, Transaction>(transaction, out exception3, parametersInDifferentNumber);
                var result4 = findTransactionByProcessedDateMethodInfo2.GetResultMethodInfo<Transaction, Transaction>(transaction, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                transaction.ShouldNotBeNull();
                findTransactionByProcessedDateMethodInfo1.ShouldNotBeNull();
                findTransactionByProcessedDateMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOutRanged));
                    Should.NotThrow(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOutRanged));
                    Should.NotThrow(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => findTransactionByProcessedDateMethodInfo1.Invoke(transaction, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => findTransactionByProcessedDateMethodInfo2.Invoke(transaction, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                transaction.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Transaction) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var transaction = Fixture.Create<Transaction>();
            var id = Fixture.Create<string>();
            var dateTime = Fixture.Create<DateTime>();
            var resultCode = Fixture.Create<int>();
            var statusCode = Fixture.Create<int>();
            var amount = Fixture.Create<double>();

            // Act
            transaction.ID = id;
            transaction.DateTime = dateTime;
            transaction.ResultCode = resultCode;
            transaction.StatusCode = statusCode;
            transaction.Amount = amount;

            // Assert
            transaction.ID.ShouldBe(id);
            transaction.DateTime.ShouldBe(dateTime);
            transaction.ResultCode.ShouldBe(resultCode);
            transaction.StatusCode.ShouldBe(statusCode);
            transaction.Amount.ShouldBe(amount);
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (Amount) (Type : double) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Amount_Property_Double_Type_Verify_Test()
        {
            // Arrange
            var transaction = Fixture.Create<Transaction>();
            transaction.Amount = Fixture.Create<double>();
            var doubleType = transaction.Amount.GetType();

            // Act
            var isTypeDouble = typeof(double) == (doubleType);
            var isTypeNullableDouble = typeof(double?) == (doubleType);
            var isTypeString = typeof(string) == (doubleType);
            var isTypeInt = typeof(int) == (doubleType);
            var isTypeDecimal = typeof(decimal) == (doubleType);
            var isTypeLong = typeof(long) == (doubleType);
            var isTypeBool = typeof(bool) == (doubleType);
            var isTypeFloat = typeof(float) == (doubleType);
            var isTypeIntNullable = typeof(int?) == (doubleType);
            var isTypeDecimalNullable = typeof(decimal?) == (doubleType);
            var isTypeLongNullable = typeof(long?) == (doubleType);
            var isTypeBoolNullable = typeof(bool?) == (doubleType);
            var isTypeFloatNullable = typeof(float?) == (doubleType);

            // Assert
            isTypeDouble.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDouble.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (Amount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Class_Invalid_Property_AmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAmount = "AmountNotPresent";
            var transaction  = Fixture.Create<Transaction>();

            // Act , Assert
            Should.NotThrow(() => transaction.GetType().GetProperty(propertyNameAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Amount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAmount = "Amount";
            var transaction  = Fixture.Create<Transaction>();
            var propertyInfo  = transaction.GetType().GetProperty(propertyNameAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (DateTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_DateTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateTime = "DateTime";
            var transaction = Fixture.Create<Transaction>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = transaction.GetType().GetProperty(propertyNameDateTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(transaction, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (DateTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Class_Invalid_Property_DateTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateTime = "DateTimeNotPresent";
            var transaction  = Fixture.Create<Transaction>();

            // Act , Assert
            Should.NotThrow(() => transaction.GetType().GetProperty(propertyNameDateTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_DateTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateTime = "DateTime";
            var transaction  = Fixture.Create<Transaction>();
            var propertyInfo  = transaction.GetType().GetProperty(propertyNameDateTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (ID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_ID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var transaction = Fixture.Create<Transaction>();
            transaction.ID = Fixture.Create<string>();
            var stringType = transaction.ID.GetType();

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

        #region General Getters/Setters : Class (Transaction) => Property (ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Class_Invalid_Property_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameId = "IDNotPresent";
            var transaction  = Fixture.Create<Transaction>();

            // Act , Assert
            Should.NotThrow(() => transaction.GetType().GetProperty(propertyNameId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameId = "ID";
            var transaction  = Fixture.Create<Transaction>();
            var propertyInfo  = transaction.GetType().GetProperty(propertyNameId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (ResultCode) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_ResultCode_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var transaction = Fixture.Create<Transaction>();
            transaction.ResultCode = Fixture.Create<int>();
            var intType = transaction.ResultCode.GetType();

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

        #region General Getters/Setters : Class (Transaction) => Property (ResultCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Class_Invalid_Property_ResultCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResultCode = "ResultCodeNotPresent";
            var transaction  = Fixture.Create<Transaction>();

            // Act , Assert
            Should.NotThrow(() => transaction.GetType().GetProperty(propertyNameResultCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_ResultCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResultCode = "ResultCode";
            var transaction  = Fixture.Create<Transaction>();
            var propertyInfo  = transaction.GetType().GetProperty(propertyNameResultCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Transaction) => Property (StatusCode) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_StatusCode_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var transaction = Fixture.Create<Transaction>();
            transaction.StatusCode = Fixture.Create<int>();
            var intType = transaction.StatusCode.GetType();

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

        #region General Getters/Setters : Class (Transaction) => Property (StatusCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_Class_Invalid_Property_StatusCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatusCode = "StatusCodeNotPresent";
            var transaction  = Fixture.Create<Transaction>();

            // Act , Assert
            Should.NotThrow(() => transaction.GetType().GetProperty(propertyNameStatusCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Transaction_StatusCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatusCode = "StatusCode";
            var transaction  = Fixture.Create<Transaction>();
            var propertyInfo  = transaction.GetType().GetProperty(propertyNameStatusCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Transaction) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Transaction_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            var processedDateTime = Fixture.Create<DateTime>();
            var resultCode = Fixture.Create<int>();
            var statusCode = Fixture.Create<int>();
            var amount = Fixture.Create<double>();

            // Act, Assert
            Should.NotThrow(() => new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount));
        }

        #endregion

        #region General Constructor : Class (Transaction) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Transaction_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfTransaction = Fixture.CreateMany<Transaction>(2).ToList();
            var firstTransaction = instancesOfTransaction.FirstOrDefault();
            var lastTransaction = instancesOfTransaction.Last();

            // Act, Assert
            firstTransaction.ShouldNotBeNull();
            lastTransaction.ShouldNotBeNull();
            firstTransaction.ShouldNotBeSameAs(lastTransaction);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Transaction_5_Objects_Creation_5_Paramters_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            var processedDateTime = Fixture.Create<DateTime>();
            var resultCode = Fixture.Create<int>();
            var statusCode = Fixture.Create<int>();
            var amount = Fixture.Create<double>();
            var firstTransaction = new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount);
            var secondTransaction = new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount);
            var thirdTransaction = new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount);
            var fourthTransaction = new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount);
            var fifthTransaction = new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount);
            var sixthTransaction = new Transaction(transactionId, processedDateTime, resultCode, statusCode, amount);

            // Act, Assert
            firstTransaction.ShouldNotBeNull();
            secondTransaction.ShouldNotBeNull();
            thirdTransaction.ShouldNotBeNull();
            fourthTransaction.ShouldNotBeNull();
            fifthTransaction.ShouldNotBeNull();
            sixthTransaction.ShouldNotBeNull();
            firstTransaction.ShouldNotBeSameAs(secondTransaction);
            thirdTransaction.ShouldNotBeSameAs(firstTransaction);
            fourthTransaction.ShouldNotBeSameAs(firstTransaction);
            fifthTransaction.ShouldNotBeSameAs(firstTransaction);
            sixthTransaction.ShouldNotBeSameAs(firstTransaction);
            sixthTransaction.ShouldNotBeSameAs(fourthTransaction);
        }

        #endregion

        #endregion

        #endregion
    }
}