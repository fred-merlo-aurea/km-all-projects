using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.main.Notification;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Notification
{
    [TestFixture]
    public class NotificationsSetupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (NotificationsSetup) => Method (btnSave_click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnSave_click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnSaveClick = {sender, e};
            System.Exception exception, invokeException;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnSave_click";

            // Act
            var btnSaveClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
            var btnSaveClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
            var returnType1 = btnSaveClickMethodInfo1.ReturnType;
            var returnType2 = btnSaveClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnSaveClick.ShouldNotBeNull();
            notificationsSetup.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnSaveClickMethodInfo1.ShouldNotBeNull();
            btnSaveClickMethodInfo2.ShouldNotBeNull();
            btnSaveClickMethodInfo1.ShouldBe(btnSaveClickMethodInfo2);
            if(btnSaveClickMethodInfo1.DoesInvokeThrow(notificationsSetup, out invokeException, parametersOfbtnSaveClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnSaveClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnSaveClick));
                Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnSaveClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnSave_click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnSave_click";

            if(notificationsSetup != null)
            {
                // Act
                var btnSaveClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
                var btnSaveClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
                var returnType1 = btnSaveClickMethodInfo1.ReturnType;
                var returnType2 = btnSaveClickMethodInfo2.ReturnType;
                btnSaveClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception1, parametersOutRanged);
                btnSaveClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception2, parametersOutRanged);
                btnSaveClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception3, parametersInDifferentNumber);
                btnSaveClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                notificationsSetup.ShouldNotBeNull();
                btnSaveClickMethodInfo1.ShouldNotBeNull();
                btnSaveClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnSaveClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                notificationsSetup.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (NotificationsSetup) => Method (btnCancel_click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnCancel_click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnCancelClick = {sender, e};
            System.Exception exception, invokeException;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnCancel_click";

            // Act
            var btnCancelClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
            var btnCancelClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
            var returnType1 = btnCancelClickMethodInfo1.ReturnType;
            var returnType2 = btnCancelClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnCancelClick.ShouldNotBeNull();
            notificationsSetup.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnCancelClickMethodInfo1.ShouldNotBeNull();
            btnCancelClickMethodInfo2.ShouldNotBeNull();
            btnCancelClickMethodInfo1.ShouldBe(btnCancelClickMethodInfo2);
            if(btnCancelClickMethodInfo1.DoesInvokeThrow(notificationsSetup, out invokeException, parametersOfbtnCancelClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnCancelClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnCancelClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnCancelClick));
                Should.NotThrow(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnCancelClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnCancel_click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnCancel_click";

            if(notificationsSetup != null)
            {
                // Act
                var btnCancelClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
                var btnCancelClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
                var returnType1 = btnCancelClickMethodInfo1.ReturnType;
                var returnType2 = btnCancelClickMethodInfo2.ReturnType;
                btnCancelClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception1, parametersOutRanged);
                btnCancelClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception2, parametersOutRanged);
                btnCancelClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception3, parametersInDifferentNumber);
                btnCancelClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                notificationsSetup.ShouldNotBeNull();
                btnCancelClickMethodInfo1.ShouldNotBeNull();
                btnCancelClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnCancelClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnCancelClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                notificationsSetup.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (NotificationsSetup) => Method (btnPreview_click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnPreview_click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnPreviewClick = {sender, e};
            System.Exception exception, invokeException;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnPreview_click";

            // Act
            var btnPreviewClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
            var btnPreviewClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
            var returnType1 = btnPreviewClickMethodInfo1.ReturnType;
            var returnType2 = btnPreviewClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnPreviewClick.ShouldNotBeNull();
            notificationsSetup.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnPreviewClickMethodInfo1.ShouldNotBeNull();
            btnPreviewClickMethodInfo2.ShouldNotBeNull();
            btnPreviewClickMethodInfo1.ShouldBe(btnPreviewClickMethodInfo2);
            if(btnPreviewClickMethodInfo1.DoesInvokeThrow(notificationsSetup, out invokeException, parametersOfbtnPreviewClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnPreviewClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnPreviewClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnPreviewClick));
                Should.NotThrow(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnPreviewClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnPreview_click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnPreview_click";

            if(notificationsSetup != null)
            {
                // Act
                var btnPreviewClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
                var btnPreviewClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
                var returnType1 = btnPreviewClickMethodInfo1.ReturnType;
                var returnType2 = btnPreviewClickMethodInfo2.ReturnType;
                btnPreviewClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception1, parametersOutRanged);
                btnPreviewClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception2, parametersOutRanged);
                btnPreviewClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception3, parametersInDifferentNumber);
                btnPreviewClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                notificationsSetup.ShouldNotBeNull();
                btnPreviewClickMethodInfo1.ShouldNotBeNull();
                btnPreviewClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnPreviewClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnPreviewClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                notificationsSetup.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (NotificationsSetup) => Method (btnClose_Click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnClose_Click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnCloseClick = {sender, e};
            System.Exception exception, invokeException;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnClose_Click";

            // Act
            var btnCloseClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
            var btnCloseClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
            var returnType1 = btnCloseClickMethodInfo1.ReturnType;
            var returnType2 = btnCloseClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnCloseClick.ShouldNotBeNull();
            notificationsSetup.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnCloseClickMethodInfo1.ShouldNotBeNull();
            btnCloseClickMethodInfo2.ShouldNotBeNull();
            btnCloseClickMethodInfo1.ShouldBe(btnCloseClickMethodInfo2);
            if(btnCloseClickMethodInfo1.DoesInvokeThrow(notificationsSetup, out invokeException, parametersOfbtnCloseClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnCloseClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnCloseClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOfbtnCloseClick));
                Should.NotThrow(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOfbtnCloseClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void NotificationsSetup_btnClose_Click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var notificationsSetup  = CreateAnalyzer.CreateOrReturnStaticInstance<NotificationsSetup>(Fixture, out exception);
            var methodName = "btnClose_Click";

            if(notificationsSetup != null)
            {
                // Act
                var btnCloseClickMethodInfo1 = notificationsSetup.GetType().GetMethod(methodName);
                var btnCloseClickMethodInfo2 = notificationsSetup.GetType().GetMethod(methodName);
                var returnType1 = btnCloseClickMethodInfo1.ReturnType;
                var returnType2 = btnCloseClickMethodInfo2.ReturnType;
                btnCloseClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception1, parametersOutRanged);
                btnCloseClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception2, parametersOutRanged);
                btnCloseClickMethodInfo1.InvokeMethodInfo(notificationsSetup, out exception3, parametersInDifferentNumber);
                btnCloseClickMethodInfo2.InvokeMethodInfo(notificationsSetup, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                notificationsSetup.ShouldNotBeNull();
                btnCloseClickMethodInfo1.ShouldNotBeNull();
                btnCloseClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                    Should.NotThrow(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnCloseClickMethodInfo1.Invoke(notificationsSetup, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnCloseClickMethodInfo2.Invoke(notificationsSetup, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                notificationsSetup.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}