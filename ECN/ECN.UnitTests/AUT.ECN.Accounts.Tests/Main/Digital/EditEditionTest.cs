using System;
using System.Reflection;
using System.Web.UI.WebControls;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.main.Digital;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Digital
{
    [TestFixture]
    public class EditEditionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (EditEdition) => Method (dgVersions_DeleteCommand) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditEdition_dgVersions_DeleteCommand_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOfdgVersionsDeleteCommand = { null, null };
            System.Exception exception, invokeException;
            var editEdition = CreateAnalyzer.CreateOrReturnStaticInstance<EditEdition>(Fixture, out exception);
            var methodName = "dgVersions_DeleteCommand";

            // Act
            var dgVersionsDeleteCommandMethodInfo1 = editEdition.GetType().GetMethod(methodName);
            var dgVersionsDeleteCommandMethodInfo2 = editEdition.GetType().GetMethod(methodName);
            var returnType1 = dgVersionsDeleteCommandMethodInfo1.ReturnType;
            var returnType2 = dgVersionsDeleteCommandMethodInfo2.ReturnType;

            // Assert
            parametersOfdgVersionsDeleteCommand.ShouldNotBeNull();
            editEdition.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            dgVersionsDeleteCommandMethodInfo1.ShouldNotBeNull();
            dgVersionsDeleteCommandMethodInfo2.ShouldNotBeNull();
            dgVersionsDeleteCommandMethodInfo1.ShouldBe(dgVersionsDeleteCommandMethodInfo2);
            if (dgVersionsDeleteCommandMethodInfo1.DoesInvokeThrow(editEdition, out invokeException, parametersOfdgVersionsDeleteCommand))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOfdgVersionsDeleteCommand), exceptionType: invokeException.GetType());
                Should.Throw(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOfdgVersionsDeleteCommand), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOfdgVersionsDeleteCommand));
                Should.NotThrow(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOfdgVersionsDeleteCommand));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditEdition_dgVersions_DeleteCommand_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null, null };
            Object[] parametersInDifferentNumber = { null };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var editEdition = CreateAnalyzer.CreateOrReturnStaticInstance<EditEdition>(Fixture, out exception);
            var methodName = "dgVersions_DeleteCommand";

            if (editEdition != null)
            {
                // Act
                var dgVersionsDeleteCommandMethodInfo1 = editEdition.GetType().GetMethod(methodName);
                var dgVersionsDeleteCommandMethodInfo2 = editEdition.GetType().GetMethod(methodName);
                var returnType1 = dgVersionsDeleteCommandMethodInfo1.ReturnType;
                var returnType2 = dgVersionsDeleteCommandMethodInfo2.ReturnType;
                dgVersionsDeleteCommandMethodInfo1.InvokeMethodInfo(editEdition, out exception1, parametersOutRanged);
                dgVersionsDeleteCommandMethodInfo2.InvokeMethodInfo(editEdition, out exception2, parametersOutRanged);
                dgVersionsDeleteCommandMethodInfo1.InvokeMethodInfo(editEdition, out exception3, parametersInDifferentNumber);
                dgVersionsDeleteCommandMethodInfo2.InvokeMethodInfo(editEdition, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                editEdition.ShouldNotBeNull();
                dgVersionsDeleteCommandMethodInfo1.ShouldNotBeNull();
                dgVersionsDeleteCommandMethodInfo2.ShouldNotBeNull();
                if (exception1 != null)
                {
                    Should.Throw(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOutRanged));
                    Should.NotThrow(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOutRanged));
                    Should.NotThrow(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => dgVersionsDeleteCommandMethodInfo1.Invoke(editEdition, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => dgVersionsDeleteCommandMethodInfo2.Invoke(editEdition, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                editEdition.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (EditEdition) => Method (dgVersions_ItemDataBound) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditEdition_dgVersions_ItemDataBound_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOfdgVersionsItemDataBound = { null, null };
            System.Exception exception, invokeException;
            var editEdition = CreateAnalyzer.CreateOrReturnStaticInstance<EditEdition>(Fixture, out exception);
            var methodName = "dgVersions_ItemDataBound";

            // Act
            var dgVersionsItemDataBoundMethodInfo1 = editEdition.GetType().GetMethod(methodName);
            var dgVersionsItemDataBoundMethodInfo2 = editEdition.GetType().GetMethod(methodName);
            var returnType1 = dgVersionsItemDataBoundMethodInfo1.ReturnType;
            var returnType2 = dgVersionsItemDataBoundMethodInfo2.ReturnType;

            // Assert
            parametersOfdgVersionsItemDataBound.ShouldNotBeNull();
            editEdition.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            dgVersionsItemDataBoundMethodInfo1.ShouldNotBeNull();
            dgVersionsItemDataBoundMethodInfo2.ShouldNotBeNull();
            dgVersionsItemDataBoundMethodInfo1.ShouldBe(dgVersionsItemDataBoundMethodInfo2);
            if (dgVersionsItemDataBoundMethodInfo1.DoesInvokeThrow(editEdition, out invokeException, parametersOfdgVersionsItemDataBound))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOfdgVersionsItemDataBound), exceptionType: invokeException.GetType());
                Should.Throw(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOfdgVersionsItemDataBound), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOfdgVersionsItemDataBound));
                Should.NotThrow(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOfdgVersionsItemDataBound));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditEdition_dgVersions_ItemDataBound_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null, null };
            Object[] parametersInDifferentNumber = { null };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var editEdition = CreateAnalyzer.CreateOrReturnStaticInstance<EditEdition>(Fixture, out exception);
            var methodName = "dgVersions_ItemDataBound";

            if (editEdition != null)
            {
                // Act
                var dgVersionsItemDataBoundMethodInfo1 = editEdition.GetType().GetMethod(methodName);
                var dgVersionsItemDataBoundMethodInfo2 = editEdition.GetType().GetMethod(methodName);
                var returnType1 = dgVersionsItemDataBoundMethodInfo1.ReturnType;
                var returnType2 = dgVersionsItemDataBoundMethodInfo2.ReturnType;
                dgVersionsItemDataBoundMethodInfo1.InvokeMethodInfo(editEdition, out exception1, parametersOutRanged);
                dgVersionsItemDataBoundMethodInfo2.InvokeMethodInfo(editEdition, out exception2, parametersOutRanged);
                dgVersionsItemDataBoundMethodInfo1.InvokeMethodInfo(editEdition, out exception3, parametersInDifferentNumber);
                dgVersionsItemDataBoundMethodInfo2.InvokeMethodInfo(editEdition, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                editEdition.ShouldNotBeNull();
                dgVersionsItemDataBoundMethodInfo1.ShouldNotBeNull();
                dgVersionsItemDataBoundMethodInfo2.ShouldNotBeNull();
                if (exception1 != null)
                {
                    Should.Throw(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOutRanged));
                    Should.NotThrow(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOutRanged));
                    Should.NotThrow(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => dgVersionsItemDataBoundMethodInfo1.Invoke(editEdition, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => dgVersionsItemDataBoundMethodInfo2.Invoke(editEdition, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                editEdition.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}