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
    public class BaseChannelEditorTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (basechanneleditor) => Method (btnSave_Click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void basechanneleditor_btnSave_Click_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var basechanneleditor  = new basechanneleditor();

            // Act, Assert
            Should.Throw<System.Exception>(() => basechanneleditor.btnSave_Click(sender, null));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void basechanneleditor_btnSave_Click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnSaveClick = {sender, e};
            System.Exception exception, invokeException;
            var basechanneleditor  = CreateAnalyzer.CreateOrReturnStaticInstance<basechanneleditor>(Fixture, out exception);
            var methodName = "btnSave_Click";

            // Act
            var btnSaveClickMethodInfo1 = basechanneleditor.GetType().GetMethod(methodName);
            var btnSaveClickMethodInfo2 = basechanneleditor.GetType().GetMethod(methodName);
            var returnType1 = btnSaveClickMethodInfo1.ReturnType;
            var returnType2 = btnSaveClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnSaveClick.ShouldNotBeNull();
            basechanneleditor.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnSaveClickMethodInfo1.ShouldNotBeNull();
            btnSaveClickMethodInfo2.ShouldNotBeNull();
            btnSaveClickMethodInfo1.ShouldBe(btnSaveClickMethodInfo2);
            if(btnSaveClickMethodInfo1.DoesInvokeThrow(basechanneleditor, out invokeException, parametersOfbtnSaveClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOfbtnSaveClick));
                Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOfbtnSaveClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void basechanneleditor_btnSave_Click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var basechanneleditor  = CreateAnalyzer.CreateOrReturnStaticInstance<basechanneleditor>(Fixture, out exception);
            var methodName = "btnSave_Click";

            if(basechanneleditor != null)
            {
                // Act
                var btnSaveClickMethodInfo1 = basechanneleditor.GetType().GetMethod(methodName);
                var btnSaveClickMethodInfo2 = basechanneleditor.GetType().GetMethod(methodName);
                var returnType1 = btnSaveClickMethodInfo1.ReturnType;
                var returnType2 = btnSaveClickMethodInfo2.ReturnType;
                btnSaveClickMethodInfo1.InvokeMethodInfo(basechanneleditor, out exception1, parametersOutRanged);
                btnSaveClickMethodInfo2.InvokeMethodInfo(basechanneleditor, out exception2, parametersOutRanged);
                btnSaveClickMethodInfo1.InvokeMethodInfo(basechanneleditor, out exception3, parametersInDifferentNumber);
                btnSaveClickMethodInfo2.InvokeMethodInfo(basechanneleditor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                basechanneleditor.ShouldNotBeNull();
                btnSaveClickMethodInfo1.ShouldNotBeNull();
                btnSaveClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(basechanneleditor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(basechanneleditor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                basechanneleditor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}