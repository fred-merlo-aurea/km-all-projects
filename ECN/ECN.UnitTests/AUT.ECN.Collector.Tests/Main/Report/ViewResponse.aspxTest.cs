using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.collector.main.report;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Main.Report
{
    [TestFixture]
    public partial class ViewResponseTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ViewResponse_repQuestions_ItemDataBound_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            object[] parametersOutRanged = {sender, null, null};
            object[] parametersInDifferentNumber = {sender};
            var viewResponse  = new ViewResponse();
            var methodName = "repQuestions_ItemDataBound";

            // Act
            var repQuestionsItemDataBoundMethodInfo1 = viewResponse.GetType().GetMethod(methodName);
            var repQuestionsItemDataBoundMethodInfo2 = viewResponse.GetType().GetMethod(methodName);
            var returnType1 = repQuestionsItemDataBoundMethodInfo1.ReturnType;
            var returnType2 = repQuestionsItemDataBoundMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            viewResponse.ShouldNotBeNull();
            repQuestionsItemDataBoundMethodInfo1.ShouldNotBeNull();
            repQuestionsItemDataBoundMethodInfo2.ShouldNotBeNull();
            repQuestionsItemDataBoundMethodInfo1.ShouldBe(repQuestionsItemDataBoundMethodInfo2);
            Should.Throw<Exception>(actual: () => repQuestionsItemDataBoundMethodInfo1.Invoke(viewResponse, parametersOutRanged));
            Should.Throw<Exception>(actual: () => repQuestionsItemDataBoundMethodInfo2.Invoke(viewResponse, parametersOutRanged));
            Should.Throw<Exception>(actual: () => repQuestionsItemDataBoundMethodInfo1.Invoke(viewResponse, parametersInDifferentNumber));
            Should.Throw<Exception>(actual: () => repQuestionsItemDataBoundMethodInfo2.Invoke(viewResponse, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => repQuestionsItemDataBoundMethodInfo1.Invoke(viewResponse, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => repQuestionsItemDataBoundMethodInfo2.Invoke(viewResponse, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => repQuestionsItemDataBoundMethodInfo1.Invoke(viewResponse, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => repQuestionsItemDataBoundMethodInfo2.Invoke(viewResponse, parametersInDifferentNumber));
        }
    }
}