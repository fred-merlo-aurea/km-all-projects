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
    public class TemplatesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (templates) => Method (ddlCategoryFilter_IndexChanged) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templates_ddlCategoryFilter_IndexChanged_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfddlCategoryFilterIndexChanged = {sender, e};
            System.Exception exception, invokeException;
            var templates  = CreateAnalyzer.CreateOrReturnStaticInstance<templates>(Fixture, out exception);
            var methodName = "ddlCategoryFilter_IndexChanged";

            // Act
            var ddlCategoryFilterIndexChangedMethodInfo1 = templates.GetType().GetMethod(methodName);
            var ddlCategoryFilterIndexChangedMethodInfo2 = templates.GetType().GetMethod(methodName);
            var returnType1 = ddlCategoryFilterIndexChangedMethodInfo1.ReturnType;
            var returnType2 = ddlCategoryFilterIndexChangedMethodInfo2.ReturnType;

            // Assert
            parametersOfddlCategoryFilterIndexChanged.ShouldNotBeNull();
            templates.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            ddlCategoryFilterIndexChangedMethodInfo1.ShouldNotBeNull();
            ddlCategoryFilterIndexChangedMethodInfo2.ShouldNotBeNull();
            ddlCategoryFilterIndexChangedMethodInfo1.ShouldBe(ddlCategoryFilterIndexChangedMethodInfo2);
            if(ddlCategoryFilterIndexChangedMethodInfo1.DoesInvokeThrow(templates, out invokeException, parametersOfddlCategoryFilterIndexChanged))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOfddlCategoryFilterIndexChanged), exceptionType: invokeException.GetType());
                Should.Throw(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOfddlCategoryFilterIndexChanged), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOfddlCategoryFilterIndexChanged));
                Should.NotThrow(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOfddlCategoryFilterIndexChanged));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templates_ddlCategoryFilter_IndexChanged_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var templates  = CreateAnalyzer.CreateOrReturnStaticInstance<templates>(Fixture, out exception);
            var methodName = "ddlCategoryFilter_IndexChanged";

            if(templates != null)
            {
                // Act
                var ddlCategoryFilterIndexChangedMethodInfo1 = templates.GetType().GetMethod(methodName);
                var ddlCategoryFilterIndexChangedMethodInfo2 = templates.GetType().GetMethod(methodName);
                var returnType1 = ddlCategoryFilterIndexChangedMethodInfo1.ReturnType;
                var returnType2 = ddlCategoryFilterIndexChangedMethodInfo2.ReturnType;
                ddlCategoryFilterIndexChangedMethodInfo1.InvokeMethodInfo(templates, out exception1, parametersOutRanged);
                ddlCategoryFilterIndexChangedMethodInfo2.InvokeMethodInfo(templates, out exception2, parametersOutRanged);
                ddlCategoryFilterIndexChangedMethodInfo1.InvokeMethodInfo(templates, out exception3, parametersInDifferentNumber);
                ddlCategoryFilterIndexChangedMethodInfo2.InvokeMethodInfo(templates, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                templates.ShouldNotBeNull();
                ddlCategoryFilterIndexChangedMethodInfo1.ShouldNotBeNull();
                ddlCategoryFilterIndexChangedMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOutRanged));
                    Should.NotThrow(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOutRanged));
                    Should.NotThrow(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => ddlCategoryFilterIndexChangedMethodInfo1.Invoke(templates, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => ddlCategoryFilterIndexChangedMethodInfo2.Invoke(templates, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                templates.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (templates) => Method (deleteTemplate) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templates_deleteTemplate_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var templateId = Fixture.Create<int>();
            Object[] parametersOfdeleteTemplate = {templateId};
            System.Exception exception, invokeException;
            var templates  = CreateAnalyzer.CreateOrReturnStaticInstance<templates>(Fixture, out exception);
            var methodName = "deleteTemplate";

            // Act
            var deleteTemplateMethodInfo1 = templates.GetType().GetMethod(methodName);
            var deleteTemplateMethodInfo2 = templates.GetType().GetMethod(methodName);
            var returnType1 = deleteTemplateMethodInfo1.ReturnType;
            var returnType2 = deleteTemplateMethodInfo2.ReturnType;

            // Assert
            parametersOfdeleteTemplate.ShouldNotBeNull();
            templates.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            deleteTemplateMethodInfo1.ShouldNotBeNull();
            deleteTemplateMethodInfo2.ShouldNotBeNull();
            deleteTemplateMethodInfo1.ShouldBe(deleteTemplateMethodInfo2);
            if(deleteTemplateMethodInfo1.DoesInvokeThrow(templates, out invokeException, parametersOfdeleteTemplate))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOfdeleteTemplate), exceptionType: invokeException.GetType());
                Should.Throw(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOfdeleteTemplate), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOfdeleteTemplate));
                Should.NotThrow(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOfdeleteTemplate));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void templates_deleteTemplate_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var templateId = Fixture.Create<int>();
            Object[] parametersOutRanged = {templateId, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var templates  = CreateAnalyzer.CreateOrReturnStaticInstance<templates>(Fixture, out exception);
            var methodName = "deleteTemplate";

            if(templates != null)
            {
                // Act
                var deleteTemplateMethodInfo1 = templates.GetType().GetMethod(methodName);
                var deleteTemplateMethodInfo2 = templates.GetType().GetMethod(methodName);
                var returnType1 = deleteTemplateMethodInfo1.ReturnType;
                var returnType2 = deleteTemplateMethodInfo2.ReturnType;
                deleteTemplateMethodInfo1.InvokeMethodInfo(templates, out exception1, parametersOutRanged);
                deleteTemplateMethodInfo2.InvokeMethodInfo(templates, out exception2, parametersOutRanged);
                deleteTemplateMethodInfo1.InvokeMethodInfo(templates, out exception3, parametersInDifferentNumber);
                deleteTemplateMethodInfo2.InvokeMethodInfo(templates, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                templates.ShouldNotBeNull();
                deleteTemplateMethodInfo1.ShouldNotBeNull();
                deleteTemplateMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOutRanged));
                    Should.NotThrow(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOutRanged));
                    Should.NotThrow(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => deleteTemplateMethodInfo1.Invoke(templates, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => deleteTemplateMethodInfo2.Invoke(templates, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => deleteTemplateMethodInfo1.Invoke(templates, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => deleteTemplateMethodInfo2.Invoke(templates, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => deleteTemplateMethodInfo1.Invoke(templates, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => deleteTemplateMethodInfo2.Invoke(templates, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                templates.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}