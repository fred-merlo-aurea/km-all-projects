using System;
using System.Collections;
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
    public class ResponseParserTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (ResponseParser) => Method (IsTransactionSuccessful) (Return Type :  bool) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_IsTransactionSuccessful_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => ResponseParser.IsTransactionSuccessful(response));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_IsTransactionSuccessful_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act
            Func<bool> isTransactionSuccessful1 = () => ResponseParser.IsTransactionSuccessful(response);
            Func<bool> isTransactionSuccessful2 = () => ResponseParser.IsTransactionSuccessful(response);
            var result1 = isTransactionSuccessful1();
            var result2 = isTransactionSuccessful2();
            var target1 = isTransactionSuccessful1.Target;
            var target2 = isTransactionSuccessful2.Target;

            // Assert
            isTransactionSuccessful1.ShouldNotBeNull();
            isTransactionSuccessful2.ShouldNotBeNull();
            isTransactionSuccessful1.ShouldNotBe(isTransactionSuccessful2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => isTransactionSuccessful1.Invoke());
            Should.NotThrow(() => isTransactionSuccessful2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_IsTransactionSuccessful_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOfIsTransactionSuccessful = {response};
            System.Exception exception, invokeException;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "IsTransactionSuccessful";

            // Act
            var isTransactionSuccessfulMethodInfo1 = responseParser.GetType().GetMethod(methodName);
            var isTransactionSuccessfulMethodInfo2 = responseParser.GetType().GetMethod(methodName);
            var returnType1 = isTransactionSuccessfulMethodInfo1.ReturnType;
            var returnType2 = isTransactionSuccessfulMethodInfo2.ReturnType;

            // Assert
            parametersOfIsTransactionSuccessful.ShouldNotBeNull();
            responseParser.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            isTransactionSuccessfulMethodInfo1.ShouldNotBeNull();
            isTransactionSuccessfulMethodInfo2.ShouldNotBeNull();
            isTransactionSuccessfulMethodInfo1.ShouldBe(isTransactionSuccessfulMethodInfo2);
            if(isTransactionSuccessfulMethodInfo1.DoesInvokeThrow(responseParser, out invokeException, parametersOfIsTransactionSuccessful))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOfIsTransactionSuccessful), exceptionType: invokeException.GetType());
                Should.Throw(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOfIsTransactionSuccessful), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOfIsTransactionSuccessful));
                Should.NotThrow(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOfIsTransactionSuccessful));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_IsTransactionSuccessful_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOutRanged = {response, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "IsTransactionSuccessful";

            if(responseParser != null)
            {
                // Act
                var isTransactionSuccessfulMethodInfo1 = responseParser.GetType().GetMethod(methodName);
                var isTransactionSuccessfulMethodInfo2 = responseParser.GetType().GetMethod(methodName);
                var returnType1 = isTransactionSuccessfulMethodInfo1.ReturnType;
                var returnType2 = isTransactionSuccessfulMethodInfo2.ReturnType;
                var result1 = isTransactionSuccessfulMethodInfo1.GetResultMethodInfo<ResponseParser, bool>(responseParser, out exception1, parametersOutRanged);
                var result2 = isTransactionSuccessfulMethodInfo2.GetResultMethodInfo<ResponseParser, bool>(responseParser, out exception2, parametersOutRanged);
                var result3 = isTransactionSuccessfulMethodInfo1.GetResultMethodInfo<ResponseParser, bool>(responseParser, out exception3, parametersInDifferentNumber);
                var result4 = isTransactionSuccessfulMethodInfo2.GetResultMethodInfo<ResponseParser, bool>(responseParser, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                responseParser.ShouldNotBeNull();
                isTransactionSuccessfulMethodInfo1.ShouldNotBeNull();
                isTransactionSuccessfulMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => isTransactionSuccessfulMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => isTransactionSuccessfulMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                responseParser.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ResponseParser) => Method (GetReturnMessage) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetReturnMessage_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => ResponseParser.GetReturnMessage(response));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetReturnMessage_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act
            Func<string> getReturnMessage1 = () => ResponseParser.GetReturnMessage(response);
            Func<string> getReturnMessage2 = () => ResponseParser.GetReturnMessage(response);
            var result1 = getReturnMessage1();
            var result2 = getReturnMessage2();
            var target1 = getReturnMessage1.Target;
            var target2 = getReturnMessage2.Target;

            // Assert
            getReturnMessage1.ShouldNotBeNull();
            getReturnMessage2.ShouldNotBeNull();
            getReturnMessage1.ShouldNotBe(getReturnMessage2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getReturnMessage1.Invoke());
            Should.NotThrow(() => getReturnMessage2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetReturnMessage_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOfGetReturnMessage = {response};
            System.Exception exception, invokeException;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetReturnMessage";

            // Act
            var getReturnMessageMethodInfo1 = responseParser.GetType().GetMethod(methodName);
            var getReturnMessageMethodInfo2 = responseParser.GetType().GetMethod(methodName);
            var returnType1 = getReturnMessageMethodInfo1.ReturnType;
            var returnType2 = getReturnMessageMethodInfo2.ReturnType;

            // Assert
            parametersOfGetReturnMessage.ShouldNotBeNull();
            responseParser.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getReturnMessageMethodInfo1.ShouldNotBeNull();
            getReturnMessageMethodInfo2.ShouldNotBeNull();
            getReturnMessageMethodInfo1.ShouldBe(getReturnMessageMethodInfo2);
            if(getReturnMessageMethodInfo1.DoesInvokeThrow(responseParser, out invokeException, parametersOfGetReturnMessage))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOfGetReturnMessage), exceptionType: invokeException.GetType());
                Should.Throw(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOfGetReturnMessage), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOfGetReturnMessage));
                Should.NotThrow(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOfGetReturnMessage));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetReturnMessage_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOutRanged = {response, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetReturnMessage";

            if(responseParser != null)
            {
                // Act
                var getReturnMessageMethodInfo1 = responseParser.GetType().GetMethod(methodName);
                var getReturnMessageMethodInfo2 = responseParser.GetType().GetMethod(methodName);
                var returnType1 = getReturnMessageMethodInfo1.ReturnType;
                var returnType2 = getReturnMessageMethodInfo2.ReturnType;
                var result1 = getReturnMessageMethodInfo1.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception1, parametersOutRanged);
                var result2 = getReturnMessageMethodInfo2.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception2, parametersOutRanged);
                var result3 = getReturnMessageMethodInfo1.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception3, parametersInDifferentNumber);
                var result4 = getReturnMessageMethodInfo2.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                responseParser.ShouldNotBeNull();
                getReturnMessageMethodInfo1.ShouldNotBeNull();
                getReturnMessageMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getReturnMessageMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getReturnMessageMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                responseParser.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ResponseParser) => Method (GetTransactionID) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionID_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => ResponseParser.GetTransactionID(response));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionID_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act
            Func<string> getTransactionId1 = () => ResponseParser.GetTransactionID(response);
            Func<string> getTransactionId2 = () => ResponseParser.GetTransactionID(response);
            var result1 = getTransactionId1();
            var result2 = getTransactionId2();
            var target1 = getTransactionId1.Target;
            var target2 = getTransactionId2.Target;

            // Assert
            getTransactionId1.ShouldNotBeNull();
            getTransactionId2.ShouldNotBeNull();
            getTransactionId1.ShouldNotBe(getTransactionId2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getTransactionId1.Invoke());
            Should.NotThrow(() => getTransactionId2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionID_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOfGetTransactionId = {response};
            System.Exception exception, invokeException;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetTransactionID";

            // Act
            var getTransactionIdMethodInfo1 = responseParser.GetType().GetMethod(methodName);
            var getTransactionIdMethodInfo2 = responseParser.GetType().GetMethod(methodName);
            var returnType1 = getTransactionIdMethodInfo1.ReturnType;
            var returnType2 = getTransactionIdMethodInfo2.ReturnType;

            // Assert
            parametersOfGetTransactionId.ShouldNotBeNull();
            responseParser.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getTransactionIdMethodInfo1.ShouldNotBeNull();
            getTransactionIdMethodInfo2.ShouldNotBeNull();
            getTransactionIdMethodInfo1.ShouldBe(getTransactionIdMethodInfo2);
            if(getTransactionIdMethodInfo1.DoesInvokeThrow(responseParser, out invokeException, parametersOfGetTransactionId))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOfGetTransactionId), exceptionType: invokeException.GetType());
                Should.Throw(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOfGetTransactionId), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOfGetTransactionId));
                Should.NotThrow(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOfGetTransactionId));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionID_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOutRanged = {response, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetTransactionID";

            if(responseParser != null)
            {
                // Act
                var getTransactionIdMethodInfo1 = responseParser.GetType().GetMethod(methodName);
                var getTransactionIdMethodInfo2 = responseParser.GetType().GetMethod(methodName);
                var returnType1 = getTransactionIdMethodInfo1.ReturnType;
                var returnType2 = getTransactionIdMethodInfo2.ReturnType;
                var result1 = getTransactionIdMethodInfo1.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception1, parametersOutRanged);
                var result2 = getTransactionIdMethodInfo2.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception2, parametersOutRanged);
                var result3 = getTransactionIdMethodInfo1.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception3, parametersInDifferentNumber);
                var result4 = getTransactionIdMethodInfo2.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                responseParser.ShouldNotBeNull();
                getTransactionIdMethodInfo1.ShouldNotBeNull();
                getTransactionIdMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getTransactionIdMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getTransactionIdMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                responseParser.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ResponseParser) => Method (GetProfileID) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetProfileID_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => ResponseParser.GetProfileID(response));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetProfileID_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act
            Func<string> getProfileId1 = () => ResponseParser.GetProfileID(response);
            Func<string> getProfileId2 = () => ResponseParser.GetProfileID(response);
            var result1 = getProfileId1();
            var result2 = getProfileId2();
            var target1 = getProfileId1.Target;
            var target2 = getProfileId2.Target;

            // Assert
            getProfileId1.ShouldNotBeNull();
            getProfileId2.ShouldNotBeNull();
            getProfileId1.ShouldNotBe(getProfileId2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getProfileId1.Invoke());
            Should.NotThrow(() => getProfileId2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetProfileID_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOfGetProfileId = {response};
            System.Exception exception, invokeException;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetProfileID";

            // Act
            var getProfileIdMethodInfo1 = responseParser.GetType().GetMethod(methodName);
            var getProfileIdMethodInfo2 = responseParser.GetType().GetMethod(methodName);
            var returnType1 = getProfileIdMethodInfo1.ReturnType;
            var returnType2 = getProfileIdMethodInfo2.ReturnType;

            // Assert
            parametersOfGetProfileId.ShouldNotBeNull();
            responseParser.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getProfileIdMethodInfo1.ShouldNotBeNull();
            getProfileIdMethodInfo2.ShouldNotBeNull();
            getProfileIdMethodInfo1.ShouldBe(getProfileIdMethodInfo2);
            if(getProfileIdMethodInfo1.DoesInvokeThrow(responseParser, out invokeException, parametersOfGetProfileId))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOfGetProfileId), exceptionType: invokeException.GetType());
                Should.Throw(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOfGetProfileId), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOfGetProfileId));
                Should.NotThrow(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOfGetProfileId));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetProfileID_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOutRanged = {response, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetProfileID";

            if(responseParser != null)
            {
                // Act
                var getProfileIdMethodInfo1 = responseParser.GetType().GetMethod(methodName);
                var getProfileIdMethodInfo2 = responseParser.GetType().GetMethod(methodName);
                var returnType1 = getProfileIdMethodInfo1.ReturnType;
                var returnType2 = getProfileIdMethodInfo2.ReturnType;
                var result1 = getProfileIdMethodInfo1.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception1, parametersOutRanged);
                var result2 = getProfileIdMethodInfo2.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception2, parametersOutRanged);
                var result3 = getProfileIdMethodInfo1.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception3, parametersInDifferentNumber);
                var result4 = getProfileIdMethodInfo2.GetResultMethodInfo<ResponseParser, string>(responseParser, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                responseParser.ShouldNotBeNull();
                getProfileIdMethodInfo1.ShouldNotBeNull();
                getProfileIdMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getProfileIdMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getProfileIdMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                responseParser.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ResponseParser) => Method (GetTransactionHistory) (Return Type :  ArrayList) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionHistory_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => ResponseParser.GetTransactionHistory(response));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionHistory_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();

            // Act
            Func<ArrayList> getTransactionHistory1 = () => ResponseParser.GetTransactionHistory(response);
            Func<ArrayList> getTransactionHistory2 = () => ResponseParser.GetTransactionHistory(response);
            var result1 = getTransactionHistory1();
            var result2 = getTransactionHistory2();
            var target1 = getTransactionHistory1.Target;
            var target2 = getTransactionHistory2.Target;

            // Assert
            getTransactionHistory1.ShouldNotBeNull();
            getTransactionHistory2.ShouldNotBeNull();
            getTransactionHistory1.ShouldNotBe(getTransactionHistory2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getTransactionHistory1.Invoke());
            Should.NotThrow(() => getTransactionHistory2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionHistory_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOfGetTransactionHistory = {response};
            System.Exception exception, invokeException;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetTransactionHistory";

            // Act
            var getTransactionHistoryMethodInfo1 = responseParser.GetType().GetMethod(methodName);
            var getTransactionHistoryMethodInfo2 = responseParser.GetType().GetMethod(methodName);
            var returnType1 = getTransactionHistoryMethodInfo1.ReturnType;
            var returnType2 = getTransactionHistoryMethodInfo2.ReturnType;

            // Assert
            parametersOfGetTransactionHistory.ShouldNotBeNull();
            responseParser.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getTransactionHistoryMethodInfo1.ShouldNotBeNull();
            getTransactionHistoryMethodInfo2.ShouldNotBeNull();
            getTransactionHistoryMethodInfo1.ShouldBe(getTransactionHistoryMethodInfo2);
            if(getTransactionHistoryMethodInfo1.DoesInvokeThrow(responseParser, out invokeException, parametersOfGetTransactionHistory))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOfGetTransactionHistory), exceptionType: invokeException.GetType());
                Should.Throw(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOfGetTransactionHistory), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOfGetTransactionHistory));
                Should.NotThrow(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOfGetTransactionHistory));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionHistory_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var response = Fixture.Create<string>();
            Object[] parametersOutRanged = {response, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetTransactionHistory";

            if(responseParser != null)
            {
                // Act
                var getTransactionHistoryMethodInfo1 = responseParser.GetType().GetMethod(methodName);
                var getTransactionHistoryMethodInfo2 = responseParser.GetType().GetMethod(methodName);
                var returnType1 = getTransactionHistoryMethodInfo1.ReturnType;
                var returnType2 = getTransactionHistoryMethodInfo2.ReturnType;
                var result1 = getTransactionHistoryMethodInfo1.GetResultMethodInfo<ResponseParser, ArrayList>(responseParser, out exception1, parametersOutRanged);
                var result2 = getTransactionHistoryMethodInfo2.GetResultMethodInfo<ResponseParser, ArrayList>(responseParser, out exception2, parametersOutRanged);
                var result3 = getTransactionHistoryMethodInfo1.GetResultMethodInfo<ResponseParser, ArrayList>(responseParser, out exception3, parametersInDifferentNumber);
                var result4 = getTransactionHistoryMethodInfo2.GetResultMethodInfo<ResponseParser, ArrayList>(responseParser, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                responseParser.ShouldNotBeNull();
                getTransactionHistoryMethodInfo1.ShouldNotBeNull();
                getTransactionHistoryMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getTransactionHistoryMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getTransactionHistoryMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                responseParser.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (ResponseParser) => Method (GetTransactionStatusByCode) (Return Type :  TransactionStatusEnum) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionStatusByCode_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var statusCode = Fixture.Create<int>();
            Object[] parametersOfGetTransactionStatusByCode = {statusCode};
            System.Exception exception, invokeException;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetTransactionStatusByCode";

            // Act
            var getTransactionStatusByCodeMethodInfo1 = responseParser.GetType().GetMethod(methodName);
            var getTransactionStatusByCodeMethodInfo2 = responseParser.GetType().GetMethod(methodName);
            var returnType1 = getTransactionStatusByCodeMethodInfo1.ReturnType;
            var returnType2 = getTransactionStatusByCodeMethodInfo2.ReturnType;

            // Assert
            parametersOfGetTransactionStatusByCode.ShouldNotBeNull();
            responseParser.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getTransactionStatusByCodeMethodInfo1.ShouldNotBeNull();
            getTransactionStatusByCodeMethodInfo2.ShouldNotBeNull();
            getTransactionStatusByCodeMethodInfo1.ShouldBe(getTransactionStatusByCodeMethodInfo2);
            if(getTransactionStatusByCodeMethodInfo1.DoesInvokeThrow(responseParser, out invokeException, parametersOfGetTransactionStatusByCode))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOfGetTransactionStatusByCode), exceptionType: invokeException.GetType());
                Should.Throw(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOfGetTransactionStatusByCode), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOfGetTransactionStatusByCode));
                Should.NotThrow(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOfGetTransactionStatusByCode));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ResponseParser_GetTransactionStatusByCode_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var statusCode = Fixture.Create<int>();
            Object[] parametersOutRanged = {statusCode, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var responseParser  = CreateAnalyzer.CreateOrReturnStaticInstance<ResponseParser>(Fixture, out exception);
            var methodName = "GetTransactionStatusByCode";

            if(responseParser != null)
            {
                // Act
                var getTransactionStatusByCodeMethodInfo1 = responseParser.GetType().GetMethod(methodName);
                var getTransactionStatusByCodeMethodInfo2 = responseParser.GetType().GetMethod(methodName);
                var returnType1 = getTransactionStatusByCodeMethodInfo1.ReturnType;
                var returnType2 = getTransactionStatusByCodeMethodInfo2.ReturnType;
                var result1 = getTransactionStatusByCodeMethodInfo1.GetResultMethodInfo<ResponseParser, TransactionStatusEnum>(responseParser, out exception1, parametersOutRanged);
                var result2 = getTransactionStatusByCodeMethodInfo2.GetResultMethodInfo<ResponseParser, TransactionStatusEnum>(responseParser, out exception2, parametersOutRanged);
                var result3 = getTransactionStatusByCodeMethodInfo1.GetResultMethodInfo<ResponseParser, TransactionStatusEnum>(responseParser, out exception3, parametersInDifferentNumber);
                var result4 = getTransactionStatusByCodeMethodInfo2.GetResultMethodInfo<ResponseParser, TransactionStatusEnum>(responseParser, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                responseParser.ShouldNotBeNull();
                getTransactionStatusByCodeMethodInfo1.ShouldNotBeNull();
                getTransactionStatusByCodeMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOutRanged));
                    Should.NotThrow(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getTransactionStatusByCodeMethodInfo1.Invoke(responseParser, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getTransactionStatusByCodeMethodInfo2.Invoke(responseParser, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                responseParser.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}