using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.customersmanager;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Customers
{
    [TestFixture]
    public class LicenseEditTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (LicenseEdit) => Method (btnUpdate_Click) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void LicenseEdit_btnUpdate_Click_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var licenseEdit  = new LicenseEdit();

            // Act, Assert
            Should.Throw<System.Exception>(() => licenseEdit.btnUpdate_Click(null, null));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void LicenseEdit_btnUpdate_Click_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfbtnUpdateClick = {sender, e};
            System.Exception exception, invokeException;
            var licenseEdit  = CreateAnalyzer.CreateOrReturnStaticInstance<LicenseEdit>(Fixture, out exception);
            var methodName = "btnUpdate_Click";

            // Act
            var btnUpdateClickMethodInfo1 = licenseEdit.GetType().GetMethod(methodName);
            var btnUpdateClickMethodInfo2 = licenseEdit.GetType().GetMethod(methodName);
            var returnType1 = btnUpdateClickMethodInfo1.ReturnType;
            var returnType2 = btnUpdateClickMethodInfo2.ReturnType;

            // Assert
            parametersOfbtnUpdateClick.ShouldNotBeNull();
            licenseEdit.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            btnUpdateClickMethodInfo1.ShouldNotBeNull();
            btnUpdateClickMethodInfo2.ShouldNotBeNull();
            btnUpdateClickMethodInfo1.ShouldBe(btnUpdateClickMethodInfo2);
            if(btnUpdateClickMethodInfo1.DoesInvokeThrow(licenseEdit, out invokeException, parametersOfbtnUpdateClick))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOfbtnUpdateClick), exceptionType: invokeException.GetType());
                Should.Throw(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOfbtnUpdateClick), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOfbtnUpdateClick));
                Should.NotThrow(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOfbtnUpdateClick));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void LicenseEdit_btnUpdate_Click_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var licenseEdit  = CreateAnalyzer.CreateOrReturnStaticInstance<LicenseEdit>(Fixture, out exception);
            var methodName = "btnUpdate_Click";

            if(licenseEdit != null)
            {
                // Act
                var btnUpdateClickMethodInfo1 = licenseEdit.GetType().GetMethod(methodName);
                var btnUpdateClickMethodInfo2 = licenseEdit.GetType().GetMethod(methodName);
                var returnType1 = btnUpdateClickMethodInfo1.ReturnType;
                var returnType2 = btnUpdateClickMethodInfo2.ReturnType;
                btnUpdateClickMethodInfo1.InvokeMethodInfo(licenseEdit, out exception1, parametersOutRanged);
                btnUpdateClickMethodInfo2.InvokeMethodInfo(licenseEdit, out exception2, parametersOutRanged);
                btnUpdateClickMethodInfo1.InvokeMethodInfo(licenseEdit, out exception3, parametersInDifferentNumber);
                btnUpdateClickMethodInfo2.InvokeMethodInfo(licenseEdit, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licenseEdit.ShouldNotBeNull();
                btnUpdateClickMethodInfo1.ShouldNotBeNull();
                btnUpdateClickMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOutRanged));
                    Should.NotThrow(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOutRanged));
                    Should.NotThrow(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => btnUpdateClickMethodInfo1.Invoke(licenseEdit, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => btnUpdateClickMethodInfo2.Invoke(licenseEdit, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                licenseEdit.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (LicenseEdit) => Method (Cancel) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void LicenseEdit_Cancel_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfCancel = {sender, e};
            System.Exception exception, invokeException;
            var licenseEdit  = CreateAnalyzer.CreateOrReturnStaticInstance<LicenseEdit>(Fixture, out exception);
            var methodName = "Cancel";

            // Act
            var cancelMethodInfo1 = licenseEdit.GetType().GetMethod(methodName);
            var cancelMethodInfo2 = licenseEdit.GetType().GetMethod(methodName);
            var returnType1 = cancelMethodInfo1.ReturnType;
            var returnType2 = cancelMethodInfo2.ReturnType;

            // Assert
            parametersOfCancel.ShouldNotBeNull();
            licenseEdit.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            cancelMethodInfo1.ShouldNotBeNull();
            cancelMethodInfo2.ShouldNotBeNull();
            cancelMethodInfo1.ShouldBe(cancelMethodInfo2);
            if(cancelMethodInfo1.DoesInvokeThrow(licenseEdit, out invokeException, parametersOfCancel))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOfCancel), exceptionType: invokeException.GetType());
                Should.Throw(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOfCancel), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOfCancel));
                Should.NotThrow(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOfCancel));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void LicenseEdit_Cancel_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var licenseEdit  = CreateAnalyzer.CreateOrReturnStaticInstance<LicenseEdit>(Fixture, out exception);
            var methodName = "Cancel";

            if(licenseEdit != null)
            {
                // Act
                var cancelMethodInfo1 = licenseEdit.GetType().GetMethod(methodName);
                var cancelMethodInfo2 = licenseEdit.GetType().GetMethod(methodName);
                var returnType1 = cancelMethodInfo1.ReturnType;
                var returnType2 = cancelMethodInfo2.ReturnType;
                cancelMethodInfo1.InvokeMethodInfo(licenseEdit, out exception1, parametersOutRanged);
                cancelMethodInfo2.InvokeMethodInfo(licenseEdit, out exception2, parametersOutRanged);
                cancelMethodInfo1.InvokeMethodInfo(licenseEdit, out exception3, parametersInDifferentNumber);
                cancelMethodInfo2.InvokeMethodInfo(licenseEdit, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licenseEdit.ShouldNotBeNull();
                cancelMethodInfo1.ShouldNotBeNull();
                cancelMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOutRanged));
                    Should.NotThrow(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOutRanged));
                    Should.NotThrow(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => cancelMethodInfo1.Invoke(licenseEdit, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => cancelMethodInfo2.Invoke(licenseEdit, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => cancelMethodInfo1.Invoke(licenseEdit, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => cancelMethodInfo2.Invoke(licenseEdit, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => cancelMethodInfo1.Invoke(licenseEdit, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => cancelMethodInfo2.Invoke(licenseEdit, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                licenseEdit.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}