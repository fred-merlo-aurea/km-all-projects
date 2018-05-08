using System;
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
    public class ParaListGeneratorTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (ParaListGenerator) => Method (GetAuthString) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetAuthString_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var processor = Fixture.Create<PayFlowProCreditCardProcessor>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act, Assert
            Should.NotThrow(() => paraListGenerator.GetAuthString(processor));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetAuthString_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var processor = Fixture.Create<PayFlowProCreditCardProcessor>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act
            Func<string> getAuthString1 = () => paraListGenerator.GetAuthString(processor);
            Func<string> getAuthString2 = () => paraListGenerator.GetAuthString(processor);
            var result1 = getAuthString1();
            var result2 = getAuthString2();
            var target1 = getAuthString1.Target;
            var target2 = getAuthString2.Target;

            // Assert
            getAuthString1.ShouldNotBeNull();
            getAuthString2.ShouldNotBeNull();
            getAuthString1.ShouldNotBe(getAuthString2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getAuthString1.Invoke());
            Should.NotThrow(() => getAuthString2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetAuthString_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var processor = Fixture.Create<PayFlowProCreditCardProcessor>();
            Object[] parametersOfGetAuthString = {processor};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetAuthString";

            // Act
            var getAuthStringMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getAuthStringMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getAuthStringMethodInfo1.ReturnType;
            var returnType2 = getAuthStringMethodInfo2.ReturnType;

            // Assert
            parametersOfGetAuthString.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getAuthStringMethodInfo1.ShouldNotBeNull();
            getAuthStringMethodInfo2.ShouldNotBeNull();
            getAuthStringMethodInfo1.ShouldBe(getAuthStringMethodInfo2);
            if(getAuthStringMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetAuthString))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOfGetAuthString), exceptionType: invokeException.GetType());
                Should.Throw(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOfGetAuthString), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOfGetAuthString));
                Should.NotThrow(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOfGetAuthString));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetAuthString_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var processor = Fixture.Create<PayFlowProCreditCardProcessor>();
            Object[] parametersOutRanged = {processor, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetAuthString";

            if(paraListGenerator != null)
            {
                // Act
                var getAuthStringMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getAuthStringMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getAuthStringMethodInfo1.ReturnType;
                var returnType2 = getAuthStringMethodInfo2.ReturnType;
                var result1 = getAuthStringMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getAuthStringMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getAuthStringMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getAuthStringMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getAuthStringMethodInfo1.ShouldNotBeNull();
                getAuthStringMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getAuthStringMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getAuthStringMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ParaListGenerator) => Method (GetSalesTransactionParas) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetSalesTransactionParas_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            var amount = Fixture.Create<double>();
            var description = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act, Assert
            Should.NotThrow(() => paraListGenerator.GetSalesTransactionParas(creditCard, amount, description));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetSalesTransactionParas_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            var amount = Fixture.Create<double>();
            var description = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act
            Func<string> getSalesTransactionParas1 = () => paraListGenerator.GetSalesTransactionParas(creditCard, amount, description);
            Func<string> getSalesTransactionParas2 = () => paraListGenerator.GetSalesTransactionParas(creditCard, amount, description);
            var result1 = getSalesTransactionParas1();
            var result2 = getSalesTransactionParas2();
            var target1 = getSalesTransactionParas1.Target;
            var target2 = getSalesTransactionParas2.Target;

            // Assert
            getSalesTransactionParas1.ShouldNotBeNull();
            getSalesTransactionParas2.ShouldNotBeNull();
            getSalesTransactionParas1.ShouldNotBe(getSalesTransactionParas2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getSalesTransactionParas1.Invoke());
            Should.NotThrow(() => getSalesTransactionParas2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetSalesTransactionParas_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            var amount = Fixture.Create<double>();
            var description = Fixture.Create<string>();
            Object[] parametersOfGetSalesTransactionParas = {creditCard, amount, description};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetSalesTransactionParas";

            // Act
            var getSalesTransactionParasMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getSalesTransactionParasMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getSalesTransactionParasMethodInfo1.ReturnType;
            var returnType2 = getSalesTransactionParasMethodInfo2.ReturnType;

            // Assert
            parametersOfGetSalesTransactionParas.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getSalesTransactionParasMethodInfo1.ShouldNotBeNull();
            getSalesTransactionParasMethodInfo2.ShouldNotBeNull();
            getSalesTransactionParasMethodInfo1.ShouldBe(getSalesTransactionParasMethodInfo2);
            if(getSalesTransactionParasMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetSalesTransactionParas))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOfGetSalesTransactionParas), exceptionType: invokeException.GetType());
                Should.Throw(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOfGetSalesTransactionParas), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOfGetSalesTransactionParas));
                Should.NotThrow(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOfGetSalesTransactionParas));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetSalesTransactionParas_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            var amount = Fixture.Create<double>();
            var description = Fixture.Create<string>();
            Object[] parametersOutRanged = {creditCard, amount, description, null};
            Object[] parametersInDifferentNumber = {creditCard, amount};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetSalesTransactionParas";

            if(paraListGenerator != null)
            {
                // Act
                var getSalesTransactionParasMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getSalesTransactionParasMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getSalesTransactionParasMethodInfo1.ReturnType;
                var returnType2 = getSalesTransactionParasMethodInfo2.ReturnType;
                var result1 = getSalesTransactionParasMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getSalesTransactionParasMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getSalesTransactionParasMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getSalesTransactionParasMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getSalesTransactionParasMethodInfo1.ShouldNotBeNull();
                getSalesTransactionParasMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ParaListGenerator) => Method (GetVoidSalesTransactionParas) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetVoidSalesTransactionParas_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act, Assert
            Should.NotThrow(() => paraListGenerator.GetVoidSalesTransactionParas(transactionId));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetVoidSalesTransactionParas_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act
            Func<string> getVoidSalesTransactionParas1 = () => paraListGenerator.GetVoidSalesTransactionParas(transactionId);
            Func<string> getVoidSalesTransactionParas2 = () => paraListGenerator.GetVoidSalesTransactionParas(transactionId);
            var result1 = getVoidSalesTransactionParas1();
            var result2 = getVoidSalesTransactionParas2();
            var target1 = getVoidSalesTransactionParas1.Target;
            var target2 = getVoidSalesTransactionParas2.Target;

            // Assert
            getVoidSalesTransactionParas1.ShouldNotBeNull();
            getVoidSalesTransactionParas2.ShouldNotBeNull();
            getVoidSalesTransactionParas1.ShouldNotBe(getVoidSalesTransactionParas2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getVoidSalesTransactionParas1.Invoke());
            Should.NotThrow(() => getVoidSalesTransactionParas2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetVoidSalesTransactionParas_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            Object[] parametersOfGetVoidSalesTransactionParas = {transactionId};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetVoidSalesTransactionParas";

            // Act
            var getVoidSalesTransactionParasMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getVoidSalesTransactionParasMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getVoidSalesTransactionParasMethodInfo1.ReturnType;
            var returnType2 = getVoidSalesTransactionParasMethodInfo2.ReturnType;

            // Assert
            parametersOfGetVoidSalesTransactionParas.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getVoidSalesTransactionParasMethodInfo1.ShouldNotBeNull();
            getVoidSalesTransactionParasMethodInfo2.ShouldNotBeNull();
            getVoidSalesTransactionParasMethodInfo1.ShouldBe(getVoidSalesTransactionParasMethodInfo2);
            if(getVoidSalesTransactionParasMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetVoidSalesTransactionParas))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOfGetVoidSalesTransactionParas), exceptionType: invokeException.GetType());
                Should.Throw(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOfGetVoidSalesTransactionParas), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOfGetVoidSalesTransactionParas));
                Should.NotThrow(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOfGetVoidSalesTransactionParas));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetVoidSalesTransactionParas_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var transactionId = Fixture.Create<string>();
            Object[] parametersOutRanged = {transactionId, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetVoidSalesTransactionParas";

            if(paraListGenerator != null)
            {
                // Act
                var getVoidSalesTransactionParasMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getVoidSalesTransactionParasMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getVoidSalesTransactionParasMethodInfo1.ReturnType;
                var returnType2 = getVoidSalesTransactionParasMethodInfo2.ReturnType;
                var result1 = getVoidSalesTransactionParasMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getVoidSalesTransactionParasMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getVoidSalesTransactionParasMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getVoidSalesTransactionParasMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getVoidSalesTransactionParasMethodInfo1.ShouldNotBeNull();
                getVoidSalesTransactionParasMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getVoidSalesTransactionParasMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getVoidSalesTransactionParasMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ParaListGenerator) => Method (GetParasToAddProfile) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToAddProfile_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var start = Fixture.Create<DateTime>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act, Assert
            Should.NotThrow(() => paraListGenerator.GetParasToAddProfile(profile, start));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToAddProfile_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var start = Fixture.Create<DateTime>();
            Object[] parametersOfGetParasToAddProfile = {profile, start};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToAddProfile";

            // Act
            var getParasToAddProfileMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getParasToAddProfileMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getParasToAddProfileMethodInfo1.ReturnType;
            var returnType2 = getParasToAddProfileMethodInfo2.ReturnType;

            // Assert
            parametersOfGetParasToAddProfile.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getParasToAddProfileMethodInfo1.ShouldNotBeNull();
            getParasToAddProfileMethodInfo2.ShouldNotBeNull();
            getParasToAddProfileMethodInfo1.ShouldBe(getParasToAddProfileMethodInfo2);
            if(getParasToAddProfileMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetParasToAddProfile))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToAddProfile), exceptionType: invokeException.GetType());
                Should.Throw(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToAddProfile), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToAddProfile));
                Should.NotThrow(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToAddProfile));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToAddProfile_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var start = Fixture.Create<DateTime>();
            Object[] parametersOutRanged = {profile, start, null};
            Object[] parametersInDifferentNumber = {profile};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToAddProfile";

            if(paraListGenerator != null)
            {
                // Act
                var getParasToAddProfileMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getParasToAddProfileMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getParasToAddProfileMethodInfo1.ReturnType;
                var returnType2 = getParasToAddProfileMethodInfo2.ReturnType;
                var result1 = getParasToAddProfileMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getParasToAddProfileMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getParasToAddProfileMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getParasToAddProfileMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getParasToAddProfileMethodInfo1.ShouldNotBeNull();
                getParasToAddProfileMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToAddProfileMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToAddProfileMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ParaListGenerator) => Method (GetParasToCancelProfile) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToCancelProfile_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act, Assert
            Should.NotThrow(() => paraListGenerator.GetParasToCancelProfile(profileId));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToCancelProfile_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act
            Func<string> getParasToCancelProfile1 = () => paraListGenerator.GetParasToCancelProfile(profileId);
            Func<string> getParasToCancelProfile2 = () => paraListGenerator.GetParasToCancelProfile(profileId);
            var result1 = getParasToCancelProfile1();
            var result2 = getParasToCancelProfile2();
            var target1 = getParasToCancelProfile1.Target;
            var target2 = getParasToCancelProfile2.Target;

            // Assert
            getParasToCancelProfile1.ShouldNotBeNull();
            getParasToCancelProfile2.ShouldNotBeNull();
            getParasToCancelProfile1.ShouldNotBe(getParasToCancelProfile2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getParasToCancelProfile1.Invoke());
            Should.NotThrow(() => getParasToCancelProfile2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToCancelProfile_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            Object[] parametersOfGetParasToCancelProfile = {profileId};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToCancelProfile";

            // Act
            var getParasToCancelProfileMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getParasToCancelProfileMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getParasToCancelProfileMethodInfo1.ReturnType;
            var returnType2 = getParasToCancelProfileMethodInfo2.ReturnType;

            // Assert
            parametersOfGetParasToCancelProfile.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getParasToCancelProfileMethodInfo1.ShouldNotBeNull();
            getParasToCancelProfileMethodInfo2.ShouldNotBeNull();
            getParasToCancelProfileMethodInfo1.ShouldBe(getParasToCancelProfileMethodInfo2);
            if(getParasToCancelProfileMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetParasToCancelProfile))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToCancelProfile), exceptionType: invokeException.GetType());
                Should.Throw(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToCancelProfile), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToCancelProfile));
                Should.NotThrow(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToCancelProfile));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToCancelProfile_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            Object[] parametersOutRanged = {profileId, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToCancelProfile";

            if(paraListGenerator != null)
            {
                // Act
                var getParasToCancelProfileMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getParasToCancelProfileMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getParasToCancelProfileMethodInfo1.ReturnType;
                var returnType2 = getParasToCancelProfileMethodInfo2.ReturnType;
                var result1 = getParasToCancelProfileMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getParasToCancelProfileMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getParasToCancelProfileMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getParasToCancelProfileMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getParasToCancelProfileMethodInfo1.ShouldNotBeNull();
                getParasToCancelProfileMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToCancelProfileMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToCancelProfileMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ParaListGenerator) => Method (GetParasToModifyProfile) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToModifyProfile_Method_With_2_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var start = Fixture.Create<DateTime>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act
            Func<string> getParasToModifyProfile1 = () => paraListGenerator.GetParasToModifyProfile(profile, start);
            Func<string> getParasToModifyProfile2 = () => paraListGenerator.GetParasToModifyProfile(profile, start);
            var result1 = getParasToModifyProfile1();
            var result2 = getParasToModifyProfile2();
            var target1 = getParasToModifyProfile1.Target;
            var target2 = getParasToModifyProfile2.Target;

            // Assert
            getParasToModifyProfile1.ShouldNotBeNull();
            getParasToModifyProfile2.ShouldNotBeNull();
            getParasToModifyProfile1.ShouldNotBe(getParasToModifyProfile2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getParasToModifyProfile1.Invoke());
            Should.NotThrow(() => getParasToModifyProfile2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToModifyProfile_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var start = Fixture.Create<DateTime>();
            Object[] parametersOfGetParasToModifyProfile = {profile, start};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToModifyProfile";

            // Act
            var getParasToModifyProfileMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getParasToModifyProfileMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getParasToModifyProfileMethodInfo1.ReturnType;
            var returnType2 = getParasToModifyProfileMethodInfo2.ReturnType;

            // Assert
            parametersOfGetParasToModifyProfile.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getParasToModifyProfileMethodInfo1.ShouldNotBeNull();
            getParasToModifyProfileMethodInfo2.ShouldNotBeNull();
            getParasToModifyProfileMethodInfo1.ShouldBe(getParasToModifyProfileMethodInfo2);
            if(getParasToModifyProfileMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetParasToModifyProfile))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToModifyProfile), exceptionType: invokeException.GetType());
                Should.Throw(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToModifyProfile), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToModifyProfile));
                Should.NotThrow(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToModifyProfile));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToModifyProfile_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var start = Fixture.Create<DateTime>();
            Object[] parametersOutRanged = {profile, start, null};
            Object[] parametersInDifferentNumber = {profile};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToModifyProfile";

            if(paraListGenerator != null)
            {
                // Act
                var getParasToModifyProfileMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getParasToModifyProfileMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getParasToModifyProfileMethodInfo1.ReturnType;
                var returnType2 = getParasToModifyProfileMethodInfo2.ReturnType;
                var result1 = getParasToModifyProfileMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getParasToModifyProfileMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getParasToModifyProfileMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getParasToModifyProfileMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getParasToModifyProfileMethodInfo1.ShouldNotBeNull();
                getParasToModifyProfileMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToModifyProfileMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToModifyProfileMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ParaListGenerator) => Method (GetParasToQueryProfileHistory) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToQueryProfileHistory_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act, Assert
            Should.NotThrow(() => paraListGenerator.GetParasToQueryProfileHistory(profileId));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToQueryProfileHistory_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            var paraListGenerator  = Fixture.Create<ParaListGenerator>();

            // Act
            Func<string> getParasToQueryProfileHistory1 = () => paraListGenerator.GetParasToQueryProfileHistory(profileId);
            Func<string> getParasToQueryProfileHistory2 = () => paraListGenerator.GetParasToQueryProfileHistory(profileId);
            var result1 = getParasToQueryProfileHistory1();
            var result2 = getParasToQueryProfileHistory2();
            var target1 = getParasToQueryProfileHistory1.Target;
            var target2 = getParasToQueryProfileHistory2.Target;

            // Assert
            getParasToQueryProfileHistory1.ShouldNotBeNull();
            getParasToQueryProfileHistory2.ShouldNotBeNull();
            getParasToQueryProfileHistory1.ShouldNotBe(getParasToQueryProfileHistory2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getParasToQueryProfileHistory1.Invoke());
            Should.NotThrow(() => getParasToQueryProfileHistory2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToQueryProfileHistory_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            Object[] parametersOfGetParasToQueryProfileHistory = {profileId};
            System.Exception exception, invokeException;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToQueryProfileHistory";

            // Act
            var getParasToQueryProfileHistoryMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
            var getParasToQueryProfileHistoryMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
            var returnType1 = getParasToQueryProfileHistoryMethodInfo1.ReturnType;
            var returnType2 = getParasToQueryProfileHistoryMethodInfo2.ReturnType;

            // Assert
            parametersOfGetParasToQueryProfileHistory.ShouldNotBeNull();
            paraListGenerator.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getParasToQueryProfileHistoryMethodInfo1.ShouldNotBeNull();
            getParasToQueryProfileHistoryMethodInfo2.ShouldNotBeNull();
            getParasToQueryProfileHistoryMethodInfo1.ShouldBe(getParasToQueryProfileHistoryMethodInfo2);
            if(getParasToQueryProfileHistoryMethodInfo1.DoesInvokeThrow(paraListGenerator, out invokeException, parametersOfGetParasToQueryProfileHistory))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToQueryProfileHistory), exceptionType: invokeException.GetType());
                Should.Throw(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToQueryProfileHistory), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOfGetParasToQueryProfileHistory));
                Should.NotThrow(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOfGetParasToQueryProfileHistory));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ParaListGenerator_GetParasToQueryProfileHistory_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var profileId = Fixture.Create<string>();
            Object[] parametersOutRanged = {profileId, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var paraListGenerator  = CreateAnalyzer.CreateOrReturnStaticInstance<ParaListGenerator>(Fixture, out exception);
            var methodName = "GetParasToQueryProfileHistory";

            if(paraListGenerator != null)
            {
                // Act
                var getParasToQueryProfileHistoryMethodInfo1 = paraListGenerator.GetType().GetMethod(methodName);
                var getParasToQueryProfileHistoryMethodInfo2 = paraListGenerator.GetType().GetMethod(methodName);
                var returnType1 = getParasToQueryProfileHistoryMethodInfo1.ReturnType;
                var returnType2 = getParasToQueryProfileHistoryMethodInfo2.ReturnType;
                var result1 = getParasToQueryProfileHistoryMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception1, parametersOutRanged);
                var result2 = getParasToQueryProfileHistoryMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception2, parametersOutRanged);
                var result3 = getParasToQueryProfileHistoryMethodInfo1.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception3, parametersInDifferentNumber);
                var result4 = getParasToQueryProfileHistoryMethodInfo2.GetResultMethodInfo<ParaListGenerator, string>(paraListGenerator, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                paraListGenerator.ShouldNotBeNull();
                getParasToQueryProfileHistoryMethodInfo1.ShouldNotBeNull();
                getParasToQueryProfileHistoryMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                    Should.NotThrow(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getParasToQueryProfileHistoryMethodInfo1.Invoke(paraListGenerator, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getParasToQueryProfileHistoryMethodInfo2.Invoke(paraListGenerator, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                paraListGenerator.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}