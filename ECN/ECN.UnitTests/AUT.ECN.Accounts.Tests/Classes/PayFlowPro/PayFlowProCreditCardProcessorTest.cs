using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.classes.PayFlowPro;
using ecn.common.classes.billing;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Classes.PayFlowPro
{
    [TestFixture]
    public class PayFlowProCreditCardProcessorTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (PayFlowProCreditCardProcessor) => Method (ProcessSalesTransaction) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_ProcessSalesTransaction_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            var amount = Fixture.Create<double>();
            var description = Fixture.Create<string>();
            Object[] parametersOutRanged = { creditCard, amount, description, null };
            Object[] parametersInDifferentNumber = { creditCard, amount };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var payFlowProCreditCardProcessor = CreateAnalyzer.CreateOrReturnStaticInstance<PayFlowProCreditCardProcessor>(Fixture, out exception);
            var methodName = "ProcessSalesTransaction";

            if (payFlowProCreditCardProcessor != null)
            {
                // Act
                var processSalesTransactionMethodInfo1 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var processSalesTransactionMethodInfo2 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var returnType1 = processSalesTransactionMethodInfo1.ReturnType;
                var returnType2 = processSalesTransactionMethodInfo2.ReturnType;
                var result1 = processSalesTransactionMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception1, parametersOutRanged);
                var result2 = processSalesTransactionMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception2, parametersOutRanged);
                var result3 = processSalesTransactionMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception3, parametersInDifferentNumber);
                var result4 = processSalesTransactionMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                payFlowProCreditCardProcessor.ShouldNotBeNull();
                processSalesTransactionMethodInfo1.ShouldNotBeNull();
                processSalesTransactionMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => processSalesTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => processSalesTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                payFlowProCreditCardProcessor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_VoidSaleTransaction_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            Object[] parametersOutRanged = { transactionId, null };
            Object[] parametersInDifferentNumber = { };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var payFlowProCreditCardProcessor = CreateAnalyzer.CreateOrReturnStaticInstance<PayFlowProCreditCardProcessor>(Fixture, out exception);
            var methodName = "VoidSaleTransaction";

            if (payFlowProCreditCardProcessor != null)
            {
                // Act
                var voidSaleTransactionMethodInfo1 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var voidSaleTransactionMethodInfo2 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var returnType1 = voidSaleTransactionMethodInfo1.ReturnType;
                var returnType2 = voidSaleTransactionMethodInfo2.ReturnType;
                var result1 = voidSaleTransactionMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception1, parametersOutRanged);
                var result2 = voidSaleTransactionMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception2, parametersOutRanged);
                var result3 = voidSaleTransactionMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception3, parametersInDifferentNumber);
                var result4 = voidSaleTransactionMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                payFlowProCreditCardProcessor.ShouldNotBeNull();
                voidSaleTransactionMethodInfo1.ShouldNotBeNull();
                voidSaleTransactionMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => voidSaleTransactionMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => voidSaleTransactionMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                payFlowProCreditCardProcessor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (PayFlowProCreditCardProcessor) => Method (AddProfile) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_AddProfile_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var startDate = Fixture.Create<DateTime>();
            Object[] parametersOutRanged = { profile, startDate, null };
            Object[] parametersInDifferentNumber = { profile };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var payFlowProCreditCardProcessor = CreateAnalyzer.CreateOrReturnStaticInstance<PayFlowProCreditCardProcessor>(Fixture, out exception);
            var methodName = "AddProfile";

            if (payFlowProCreditCardProcessor != null)
            {
                // Act
                var addProfileMethodInfo1 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var addProfileMethodInfo2 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var returnType1 = addProfileMethodInfo1.ReturnType;
                var returnType2 = addProfileMethodInfo2.ReturnType;
                var result1 = addProfileMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception1, parametersOutRanged);
                var result2 = addProfileMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception2, parametersOutRanged);
                var result3 = addProfileMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception3, parametersInDifferentNumber);
                var result4 = addProfileMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                payFlowProCreditCardProcessor.ShouldNotBeNull();
                addProfileMethodInfo1.ShouldNotBeNull();
                addProfileMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => addProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => addProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                payFlowProCreditCardProcessor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (PayFlowProCreditCardProcessor) => Method (CancelProfileByID) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_CancelProfileByID_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act, Assert
            Should.Throw<System.Exception>(() => payFlowProCreditCardProcessor.CancelProfileByID(profileId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_CancelProfileByID_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            Object[] parametersOutRanged = { profileId, null };
            Object[] parametersInDifferentNumber = { };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var payFlowProCreditCardProcessor = CreateAnalyzer.CreateOrReturnStaticInstance<PayFlowProCreditCardProcessor>(Fixture, out exception);
            var methodName = "CancelProfileByID";

            if (payFlowProCreditCardProcessor != null)
            {
                // Act
                var cancelProfileByIdMethodInfo1 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var cancelProfileByIdMethodInfo2 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var returnType1 = cancelProfileByIdMethodInfo1.ReturnType;
                var returnType2 = cancelProfileByIdMethodInfo2.ReturnType;
                var result1 = cancelProfileByIdMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception1, parametersOutRanged);
                var result2 = cancelProfileByIdMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception2, parametersOutRanged);
                var result3 = cancelProfileByIdMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception3, parametersInDifferentNumber);
                var result4 = cancelProfileByIdMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                payFlowProCreditCardProcessor.ShouldNotBeNull();
                cancelProfileByIdMethodInfo1.ShouldNotBeNull();
                cancelProfileByIdMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => cancelProfileByIdMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => cancelProfileByIdMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                payFlowProCreditCardProcessor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (PayFlowProCreditCardProcessor) => Method (ModifyProfile) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_ModifyProfile_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var startDate = Fixture.Create<DateTime>();
            Object[] parametersOutRanged = { profile, startDate, null };
            Object[] parametersInDifferentNumber = { profile };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var payFlowProCreditCardProcessor = CreateAnalyzer.CreateOrReturnStaticInstance<PayFlowProCreditCardProcessor>(Fixture, out exception);
            var methodName = "ModifyProfile";

            if (payFlowProCreditCardProcessor != null)
            {
                // Act
                var modifyProfileMethodInfo1 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var modifyProfileMethodInfo2 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var returnType1 = modifyProfileMethodInfo1.ReturnType;
                var returnType2 = modifyProfileMethodInfo2.ReturnType;
                var result1 = modifyProfileMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception1, parametersOutRanged);
                var result2 = modifyProfileMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception2, parametersOutRanged);
                var result3 = modifyProfileMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception3, parametersInDifferentNumber);
                var result4 = modifyProfileMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                payFlowProCreditCardProcessor.ShouldNotBeNull();
                modifyProfileMethodInfo1.ShouldNotBeNull();
                modifyProfileMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => modifyProfileMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => modifyProfileMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                payFlowProCreditCardProcessor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (PayFlowProCreditCardProcessor) => Method (QueryProfileHistory) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void PayFlowProCreditCardProcessor_QueryProfileHistory_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            Object[] parametersOutRanged = { profileId, null };
            Object[] parametersInDifferentNumber = { };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var payFlowProCreditCardProcessor = CreateAnalyzer.CreateOrReturnStaticInstance<PayFlowProCreditCardProcessor>(Fixture, out exception);
            var methodName = "QueryProfileHistory";

            if (payFlowProCreditCardProcessor != null)
            {
                // Act
                var queryProfileHistoryMethodInfo1 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var queryProfileHistoryMethodInfo2 = payFlowProCreditCardProcessor.GetType().GetMethod(methodName);
                var returnType1 = queryProfileHistoryMethodInfo1.ReturnType;
                var returnType2 = queryProfileHistoryMethodInfo2.ReturnType;
                var result1 = queryProfileHistoryMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception1, parametersOutRanged);
                var result2 = queryProfileHistoryMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception2, parametersOutRanged);
                var result3 = queryProfileHistoryMethodInfo1.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception3, parametersInDifferentNumber);
                var result4 = queryProfileHistoryMethodInfo2.GetResultMethodInfo<PayFlowProCreditCardProcessor, string>(payFlowProCreditCardProcessor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                payFlowProCreditCardProcessor.ShouldNotBeNull();
                queryProfileHistoryMethodInfo1.ShouldNotBeNull();
                queryProfileHistoryMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                    Should.NotThrow(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => queryProfileHistoryMethodInfo1.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => queryProfileHistoryMethodInfo2.Invoke(payFlowProCreditCardProcessor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                payFlowProCreditCardProcessor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #region Category : GetterSetter

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Host) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Host_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.Host.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Host) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_HostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHost = "HostNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameHost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Host_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHost = "Host";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameHost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Instance) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Instance_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameInstance = "Instance";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var randomString = Fixture.Create<string>();
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameInstance);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(payFlowProCreditCardProcessor, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Instance) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_InstanceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInstance = "InstanceNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameInstance));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Instance_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInstance = "Instance";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameInstance);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Partner) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Partner_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.Partner.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Partner) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_PartnerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePartner = "PartnerNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNamePartner));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Partner_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePartner = "Partner";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNamePartner);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Password) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Password_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.Password.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Password) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_PasswordNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePassword = "PasswordNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNamePassword));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Password_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePassword = "Password";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNamePassword);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Port) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Port_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var intType = payFlowProCreditCardProcessor.Port.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Port) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_PortNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePort = "PortNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNamePort));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Port_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePort = "Port";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNamePort);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.ProxyAddress.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_ProxyAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProxyAddress = "ProxyAddressNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProxyAddress = "ProxyAddress";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyLogon) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyLogon_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.ProxyLogon.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyLogon) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_ProxyLogonNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProxyLogon = "ProxyLogonNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyLogon));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyLogon_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProxyLogon = "ProxyLogon";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyLogon);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyPassword) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyPassword_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.ProxyPassword.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyPassword) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_ProxyPasswordNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProxyPassword = "ProxyPasswordNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyPassword));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyPassword_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProxyPassword = "ProxyPassword";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyPassword);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyPort) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyPort_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var intType = payFlowProCreditCardProcessor.ProxyPort.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (ProxyPort) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_ProxyPortNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProxyPort = "ProxyPortNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyPort));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_ProxyPort_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProxyPort = "ProxyPort";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameProxyPort);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (TestInstance) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_TestInstance_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTestInstance = "TestInstance";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var randomString = Fixture.Create<string>();
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameTestInstance);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(payFlowProCreditCardProcessor, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (TestInstance) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_TestInstanceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTestInstance = "TestInstanceNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameTestInstance));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_TestInstance_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTestInstance = "TestInstance";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameTestInstance);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (TimeOut) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_TimeOut_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var intType = payFlowProCreditCardProcessor.TimeOut.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (TimeOut) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_TimeOutNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTimeOut = "TimeOutNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameTimeOut));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_TimeOut_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTimeOut = "TimeOut";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameTimeOut);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (User) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_User_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.User.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (User) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_UserNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser = "UserNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameUser));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_User_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser = "User";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameUser);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Vendor) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Vendor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var stringType = payFlowProCreditCardProcessor.Vendor.GetType();

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

        #region General Getters/Setters : Class (PayFlowProCreditCardProcessor) => Property (Vendor) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Class_Invalid_Property_VendorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameVendor = "VendorNotPresent";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;

            // Act , Assert
            Should.NotThrow(() => payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameVendor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PayFlowProCreditCardProcessor_Vendor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameVendor = "Vendor";
            var payFlowProCreditCardProcessor = PayFlowProCreditCardProcessor.Instance;
            var propertyInfo = payFlowProCreditCardProcessor.GetType().GetProperty(propertyNameVendor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion
    }
}