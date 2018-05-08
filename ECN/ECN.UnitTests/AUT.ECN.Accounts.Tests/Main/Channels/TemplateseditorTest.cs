using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.communicator.channelsmanager;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Channels
{
    [TestFixture]
    public class TemplateseditorTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (templateseditor) => Method (loadBaseChannelDropDown) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templateseditor_loadBaseChannelDropDown_Method_With_No_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            System.Exception exception, exception1, exception2;
            var templateseditor  = CreateAnalyzer.CreateOrReturnStaticInstance<templateseditor>(Fixture, out exception);
            var methodName = "loadBaseChannelDropDown";

            if(templateseditor != null)
            {
                // Act
                var loadBaseChannelDropDownMethodInfo1 = templateseditor.GetType().GetMethod(methodName);
                var loadBaseChannelDropDownMethodInfo2 = templateseditor.GetType().GetMethod(methodName);
                var returnType1 = loadBaseChannelDropDownMethodInfo1.ReturnType;
                var returnType2 = loadBaseChannelDropDownMethodInfo2.ReturnType;
                loadBaseChannelDropDownMethodInfo1.InvokeMethodInfo(templateseditor, out exception1);
                loadBaseChannelDropDownMethodInfo2.InvokeMethodInfo(templateseditor, out exception2);

                // Assert
                templateseditor.ShouldNotBeNull();
                loadBaseChannelDropDownMethodInfo1.ShouldNotBeNull();
                loadBaseChannelDropDownMethodInfo2.ShouldNotBeNull();
                loadBaseChannelDropDownMethodInfo1.ShouldBe(loadBaseChannelDropDownMethodInfo2);
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                if(exception1 == null)
                {
                    Should.NotThrow(() => loadBaseChannelDropDownMethodInfo1.Invoke(templateseditor, null));
                    Should.NotThrow(() => loadBaseChannelDropDownMethodInfo2.Invoke(templateseditor, null));
                }
                else
                {
                    Should.Throw(() => loadBaseChannelDropDownMethodInfo1.Invoke(templateseditor, null), exceptionType: exception1.GetType());
                    Should.Throw(() => loadBaseChannelDropDownMethodInfo2.Invoke(templateseditor, null), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                templateseditor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templateseditor_loadBaseChannelDropDown_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = {null, null};
            System.Exception exception;
            var templateseditor  = CreateAnalyzer.CreateOrReturnStaticInstance<templateseditor>(Fixture, out exception);
            var methodName = "loadBaseChannelDropDown";

            if(templateseditor != null)
            {
                // Act
                var loadBaseChannelDropDownMethodInfo1 = templateseditor.GetType().GetMethod(methodName);
                var loadBaseChannelDropDownMethodInfo2 = templateseditor.GetType().GetMethod(methodName);
                var returnType1 = loadBaseChannelDropDownMethodInfo1.ReturnType;
                var returnType2 = loadBaseChannelDropDownMethodInfo2.ReturnType;

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                templateseditor.ShouldNotBeNull();
                loadBaseChannelDropDownMethodInfo1.ShouldNotBeNull();
                loadBaseChannelDropDownMethodInfo2.ShouldNotBeNull();
                loadBaseChannelDropDownMethodInfo1.ShouldBe(loadBaseChannelDropDownMethodInfo2);
                Should.Throw<System.Exception>(() => loadBaseChannelDropDownMethodInfo1.Invoke(templateseditor, parametersOutRanged));
                Should.Throw<System.Exception>(() => loadBaseChannelDropDownMethodInfo2.Invoke(templateseditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadBaseChannelDropDownMethodInfo1.Invoke(templateseditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadBaseChannelDropDownMethodInfo2.Invoke(templateseditor, parametersOutRanged));
            }
            else
            {
                // Act, Assert
                templateseditor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (templateseditor) => Method (btnSave_click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templateseditor_btnSave_click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnSaveClick = {sender, e};
            System.Exception exception, invokeException;
            var templateseditor  = CreateAnalyzer.CreateOrReturnStaticInstance<templateseditor>(Fixture, out exception);
            var methodName = "btnSave_click";

            // Act
            var btnSaveClickMethodInfo1 = templateseditor.GetType().GetMethod(methodName);
            var btnSaveClickMethodInfo2 = templateseditor.GetType().GetMethod(methodName);
            var returnType1 = btnSaveClickMethodInfo1.ReturnType;
            var returnType2 = btnSaveClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnSaveClick.ShouldNotBeNull();
            templateseditor.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnSaveClickMethodInfo1.ShouldNotBeNull();
            btnSaveClickMethodInfo2.ShouldNotBeNull();
            btnSaveClickMethodInfo1.ShouldBe(btnSaveClickMethodInfo2);
            if(btnSaveClickMethodInfo1.DoesInvokeThrow(templateseditor, out invokeException, parametersOfbtnSaveClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOfbtnSaveClick));
                Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOfbtnSaveClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templateseditor_btnSave_click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var templateseditor  = CreateAnalyzer.CreateOrReturnStaticInstance<templateseditor>(Fixture, out exception);
            var methodName = "btnSave_click";

            if(templateseditor != null)
            {
                // Act
                var btnSaveClickMethodInfo1 = templateseditor.GetType().GetMethod(methodName);
                var btnSaveClickMethodInfo2 = templateseditor.GetType().GetMethod(methodName);
                var returnType1 = btnSaveClickMethodInfo1.ReturnType;
                var returnType2 = btnSaveClickMethodInfo2.ReturnType;
                btnSaveClickMethodInfo1.InvokeMethodInfo(templateseditor, out exception1, parametersOutRanged);
                btnSaveClickMethodInfo2.InvokeMethodInfo(templateseditor, out exception2, parametersOutRanged);
                btnSaveClickMethodInfo1.InvokeMethodInfo(templateseditor, out exception3, parametersInDifferentNumber);
                btnSaveClickMethodInfo2.InvokeMethodInfo(templateseditor, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                templateseditor.ShouldNotBeNull();
                btnSaveClickMethodInfo1.ShouldNotBeNull();
                btnSaveClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(templateseditor, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(templateseditor, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                templateseditor.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}