using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.includes;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Includes
{
    [TestFixture]
    public class ThumbnailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (thumbnail) => Method (newthumbSize) (Return Type :  Size) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void thumbnail_newthumbSize_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var currentwidth = Fixture.Create<int>();
            var currentheight = Fixture.Create<int>();
            var newsize = Fixture.Create<int>();
            Object[] parametersOfnewthumbSize = { currentwidth, currentheight, newsize };
            System.Exception exception, invokeException;
            var thumbnail = CreateAnalyzer.CreateOrReturnStaticInstance<thumbnail>(Fixture, out exception);
            var methodName = "newthumbSize";

            // Act
            var newthumbSizeMethodInfo1 = thumbnail.GetType().GetMethod(methodName);
            var newthumbSizeMethodInfo2 = thumbnail.GetType().GetMethod(methodName);
            var returnType1 = newthumbSizeMethodInfo1.ReturnType;
            var returnType2 = newthumbSizeMethodInfo2.ReturnType;

            // Assert
            parametersOfnewthumbSize.ShouldNotBeNull();
            thumbnail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            newthumbSizeMethodInfo1.ShouldNotBeNull();
            newthumbSizeMethodInfo2.ShouldNotBeNull();
            newthumbSizeMethodInfo1.ShouldBe(newthumbSizeMethodInfo2);
            if (newthumbSizeMethodInfo1.DoesInvokeThrow(thumbnail, out invokeException, parametersOfnewthumbSize))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOfnewthumbSize), exceptionType: invokeException.GetType());
                Should.Throw(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOfnewthumbSize), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOfnewthumbSize));
                Should.NotThrow(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOfnewthumbSize));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void thumbnail_newthumbSize_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var currentwidth = Fixture.Create<int>();
            var currentheight = Fixture.Create<int>();
            var newsize = Fixture.Create<int>();
            Object[] parametersOutRanged = { currentwidth, currentheight, newsize, null };
            Object[] parametersInDifferentNumber = { currentwidth, currentheight };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var thumbnail = CreateAnalyzer.CreateOrReturnStaticInstance<thumbnail>(Fixture, out exception);
            var methodName = "newthumbSize";

            if (thumbnail != null)
            {
                // Act
                var newthumbSizeMethodInfo1 = thumbnail.GetType().GetMethod(methodName);
                var newthumbSizeMethodInfo2 = thumbnail.GetType().GetMethod(methodName);
                var returnType1 = newthumbSizeMethodInfo1.ReturnType;
                var returnType2 = newthumbSizeMethodInfo2.ReturnType;
                newthumbSizeMethodInfo1.InvokeMethodInfo(thumbnail, out exception1, parametersOutRanged);
                newthumbSizeMethodInfo2.InvokeMethodInfo(thumbnail, out exception2, parametersOutRanged);
                newthumbSizeMethodInfo1.InvokeMethodInfo(thumbnail, out exception3, parametersInDifferentNumber);
                newthumbSizeMethodInfo2.InvokeMethodInfo(thumbnail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                thumbnail.ShouldNotBeNull();
                newthumbSizeMethodInfo1.ShouldNotBeNull();
                newthumbSizeMethodInfo2.ShouldNotBeNull();
                if (exception1 != null)
                {
                    Should.Throw(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                    Should.NotThrow(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                    Should.NotThrow(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => newthumbSizeMethodInfo1.Invoke(thumbnail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => newthumbSizeMethodInfo2.Invoke(thumbnail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                thumbnail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (thumbnail) => Method (sendFile) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void thumbnail_sendFile_Method_With_No_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            System.Exception exception, exception1, exception2;
            var thumbnail = CreateAnalyzer.CreateOrReturnStaticInstance<thumbnail>(Fixture, out exception);
            var methodName = "sendFile";

            if (thumbnail != null)
            {
                // Act
                var sendFileMethodInfo1 = thumbnail.GetType().GetMethod(methodName);
                var sendFileMethodInfo2 = thumbnail.GetType().GetMethod(methodName);
                var returnType1 = sendFileMethodInfo1.ReturnType;
                var returnType2 = sendFileMethodInfo2.ReturnType;
                sendFileMethodInfo1.InvokeMethodInfo(thumbnail, out exception1);
                sendFileMethodInfo2.InvokeMethodInfo(thumbnail, out exception2);

                // Assert
                thumbnail.ShouldNotBeNull();
                sendFileMethodInfo1.ShouldNotBeNull();
                sendFileMethodInfo2.ShouldNotBeNull();
                sendFileMethodInfo1.ShouldBe(sendFileMethodInfo2);
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                if (exception1 == null)
                {
                    Should.NotThrow(() => sendFileMethodInfo1.Invoke(thumbnail, null));
                    Should.NotThrow(() => sendFileMethodInfo2.Invoke(thumbnail, null));
                }
                else
                {
                    Should.Throw(() => sendFileMethodInfo1.Invoke(thumbnail, null), exceptionType: exception1.GetType());
                    Should.Throw(() => sendFileMethodInfo2.Invoke(thumbnail, null), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                thumbnail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void thumbnail_sendFile_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null };
            System.Exception exception;
            var thumbnail = CreateAnalyzer.CreateOrReturnStaticInstance<thumbnail>(Fixture, out exception);
            var methodName = "sendFile";

            if (thumbnail != null)
            {
                // Act
                var sendFileMethodInfo1 = thumbnail.GetType().GetMethod(methodName);
                var sendFileMethodInfo2 = thumbnail.GetType().GetMethod(methodName);
                var returnType1 = sendFileMethodInfo1.ReturnType;
                var returnType2 = sendFileMethodInfo2.ReturnType;

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                thumbnail.ShouldNotBeNull();
                sendFileMethodInfo1.ShouldNotBeNull();
                sendFileMethodInfo2.ShouldNotBeNull();
                sendFileMethodInfo1.ShouldBe(sendFileMethodInfo2);
                Should.Throw<System.Exception>(() => sendFileMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<System.Exception>(() => sendFileMethodInfo2.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => sendFileMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => sendFileMethodInfo2.Invoke(thumbnail, parametersOutRanged));
            }
            else
            {
                // Act, Assert
                thumbnail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (thumbnail) => Method (sendError) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void thumbnail_sendError_Method_With_No_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            System.Exception exception, exception1, exception2;
            var thumbnail = CreateAnalyzer.CreateOrReturnStaticInstance<thumbnail>(Fixture, out exception);
            var methodName = "sendError";

            if (thumbnail != null)
            {
                // Act
                var sendErrorMethodInfo1 = thumbnail.GetType().GetMethod(methodName);
                var sendErrorMethodInfo2 = thumbnail.GetType().GetMethod(methodName);
                var returnType1 = sendErrorMethodInfo1.ReturnType;
                var returnType2 = sendErrorMethodInfo2.ReturnType;
                sendErrorMethodInfo1.InvokeMethodInfo(thumbnail, out exception1);
                sendErrorMethodInfo2.InvokeMethodInfo(thumbnail, out exception2);

                // Assert
                thumbnail.ShouldNotBeNull();
                sendErrorMethodInfo1.ShouldNotBeNull();
                sendErrorMethodInfo2.ShouldNotBeNull();
                sendErrorMethodInfo1.ShouldBe(sendErrorMethodInfo2);
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                if (exception1 == null)
                {
                    Should.NotThrow(() => sendErrorMethodInfo1.Invoke(thumbnail, null));
                    Should.NotThrow(() => sendErrorMethodInfo2.Invoke(thumbnail, null));
                }
                else
                {
                    Should.Throw(() => sendErrorMethodInfo1.Invoke(thumbnail, null), exceptionType: exception1.GetType());
                    Should.Throw(() => sendErrorMethodInfo2.Invoke(thumbnail, null), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                thumbnail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void thumbnail_sendError_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null };
            System.Exception exception;
            var thumbnail = CreateAnalyzer.CreateOrReturnStaticInstance<thumbnail>(Fixture, out exception);
            var methodName = "sendError";

            if (thumbnail != null)
            {
                // Act
                var sendErrorMethodInfo1 = thumbnail.GetType().GetMethod(methodName);
                var sendErrorMethodInfo2 = thumbnail.GetType().GetMethod(methodName);
                var returnType1 = sendErrorMethodInfo1.ReturnType;
                var returnType2 = sendErrorMethodInfo2.ReturnType;

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                thumbnail.ShouldNotBeNull();
                sendErrorMethodInfo1.ShouldNotBeNull();
                sendErrorMethodInfo2.ShouldNotBeNull();
                sendErrorMethodInfo1.ShouldBe(sendErrorMethodInfo2);
                Should.Throw<System.Exception>(() => sendErrorMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<System.Exception>(() => sendErrorMethodInfo2.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => sendErrorMethodInfo1.Invoke(thumbnail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => sendErrorMethodInfo2.Invoke(thumbnail, parametersOutRanged));
            }
            else
            {
                // Act, Assert
                thumbnail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}