using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.channelsmanager;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Channels
{
    [TestFixture]
    public class ChannelsEditorTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (channelseditor) => Method (btnSave_Click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void channelseditor_btnSave_Click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnSaveClick = {sender, e};
            System.Exception exception, invokeException;
            var channelseditor  = CreateAnalyzer.CreateOrReturnStaticInstance<channelseditor>(Fixture, out exception);
            var methodName = "btnSave_Click";

            // Act
            var btnSaveClickMethodInfo1 = channelseditor.GetType().GetMethod(methodName);
            var btnSaveClickMethodInfo2 = channelseditor.GetType().GetMethod(methodName);
            var returnType1 = btnSaveClickMethodInfo1.ReturnType;
            var returnType2 = btnSaveClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnSaveClick.ShouldNotBeNull();
            channelseditor.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnSaveClickMethodInfo1.ShouldNotBeNull();
            btnSaveClickMethodInfo2.ShouldNotBeNull();
            btnSaveClickMethodInfo1.ShouldBe(btnSaveClickMethodInfo2);
            if(btnSaveClickMethodInfo1.DoesInvokeThrow(channelseditor, out invokeException, parametersOfbtnSaveClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOfbtnSaveClick));
                Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOfbtnSaveClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void channelseditor_btnSave_Click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var channelseditor  = CreateAnalyzer.CreateOrReturnStaticInstance<channelseditor>(Fixture, out exception);
            var methodName = "btnSave_Click";

            if(channelseditor != null)
            {
                // Act
                var btnSaveClickMethodInfo1 = channelseditor.GetType().GetMethod(methodName);
                var btnSaveClickMethodInfo2 = channelseditor.GetType().GetMethod(methodName);
                var returnType1 = btnSaveClickMethodInfo1.ReturnType;
                var returnType2 = btnSaveClickMethodInfo2.ReturnType;
                btnSaveClickMethodInfo1.InvokeMethodInfo(channelseditor, out exception1, parametersOutRanged);
                btnSaveClickMethodInfo2.InvokeMethodInfo(channelseditor, out exception2, parametersOutRanged);
                btnSaveClickMethodInfo1.InvokeMethodInfo(channelseditor, out exception3, parametersInDifferentNumber);
                btnSaveClickMethodInfo2.InvokeMethodInfo(channelseditor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                channelseditor.ShouldNotBeNull();
                btnSaveClickMethodInfo1.ShouldNotBeNull();
                btnSaveClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(channelseditor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(channelseditor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                channelseditor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}