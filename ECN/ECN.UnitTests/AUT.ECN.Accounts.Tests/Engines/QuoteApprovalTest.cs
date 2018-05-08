using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.Engines;
using ecn.common.classes.billing;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Engines
{
    [TestFixture]
    public class QuoteApprovalTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (QuoteApproval) => Method (IsCreditCardValid) (Return Type :  bool) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void QuoteApproval_IsCreditCardValid_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            Object[] parametersOfIsCreditCardValid = {creditCard};
            System.Exception exception, invokeException;
            var quoteApproval  = CreateAnalyzer.CreateOrReturnStaticInstance<QuoteApproval>(Fixture, out exception);
            var methodName = "IsCreditCardValid";

            // Act
            var isCreditCardValidMethodInfo1 = quoteApproval.GetType().GetMethod(methodName);
            var isCreditCardValidMethodInfo2 = quoteApproval.GetType().GetMethod(methodName);
            var returnType1 = isCreditCardValidMethodInfo1.ReturnType;
            var returnType2 = isCreditCardValidMethodInfo2.ReturnType;

            // Assert
            parametersOfIsCreditCardValid.ShouldNotBeNull();
            quoteApproval.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            isCreditCardValidMethodInfo1.ShouldNotBeNull();
            isCreditCardValidMethodInfo2.ShouldNotBeNull();
            isCreditCardValidMethodInfo1.ShouldBe(isCreditCardValidMethodInfo2);
            if(isCreditCardValidMethodInfo1.DoesInvokeThrow(quoteApproval, out invokeException, parametersOfIsCreditCardValid))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOfIsCreditCardValid), exceptionType: invokeException.GetType());
                Should.Throw(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOfIsCreditCardValid), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOfIsCreditCardValid));
                Should.NotThrow(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOfIsCreditCardValid));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void QuoteApproval_IsCreditCardValid_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var creditCard = Fixture.Create<CreditCard>();
            Object[] parametersOutRanged = {creditCard, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var quoteApproval  = CreateAnalyzer.CreateOrReturnStaticInstance<QuoteApproval>(Fixture, out exception);
            var methodName = "IsCreditCardValid";

            if(quoteApproval != null)
            {
                // Act
                var isCreditCardValidMethodInfo1 = quoteApproval.GetType().GetMethod(methodName);
                var isCreditCardValidMethodInfo2 = quoteApproval.GetType().GetMethod(methodName);
                var returnType1 = isCreditCardValidMethodInfo1.ReturnType;
                var returnType2 = isCreditCardValidMethodInfo2.ReturnType;
                var result1 = isCreditCardValidMethodInfo1.GetResultMethodInfo<QuoteApproval, bool>(quoteApproval, out exception1, parametersOutRanged);
                var result2 = isCreditCardValidMethodInfo2.GetResultMethodInfo<QuoteApproval, bool>(quoteApproval, out exception2, parametersOutRanged);
                var result3 = isCreditCardValidMethodInfo1.GetResultMethodInfo<QuoteApproval, bool>(quoteApproval, out exception3, parametersInDifferentNumber);
                var result4 = isCreditCardValidMethodInfo2.GetResultMethodInfo<QuoteApproval, bool>(quoteApproval, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                quoteApproval.ShouldNotBeNull();
                isCreditCardValidMethodInfo1.ShouldNotBeNull();
                isCreditCardValidMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOutRanged));
                    Should.NotThrow(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOutRanged));
                    Should.NotThrow(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => isCreditCardValidMethodInfo1.Invoke(quoteApproval, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => isCreditCardValidMethodInfo2.Invoke(quoteApproval, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                quoteApproval.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}