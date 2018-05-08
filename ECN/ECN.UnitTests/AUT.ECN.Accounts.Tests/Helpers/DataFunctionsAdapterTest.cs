using System;
using System.Data;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using Ecn.Accounts.Helpers;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Helpers
{
    [TestFixture]
    public class DataFunctionsAdapterTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (DataFunctionsAdapter) => Method (GetDataTable) (Return Type :  DataTable) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DataFunctionsAdapter_GetDataTable_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sql = Fixture.Create<string>();
            var connectionStringName = Fixture.Create<string>();
            Object[] parametersOfGetDataTable = {sql, connectionStringName};
            System.Exception exception, invokeException;
            var dataFunctionsAdapter  = CreateAnalyzer.CreateOrReturnStaticInstance<DataFunctionsAdapter>(Fixture, out exception);
            var methodName = "GetDataTable";

            // Act
            var getDataTableMethodInfo1 = dataFunctionsAdapter.GetType().GetMethod(methodName);
            var getDataTableMethodInfo2 = dataFunctionsAdapter.GetType().GetMethod(methodName);
            var returnType1 = getDataTableMethodInfo1.ReturnType;
            var returnType2 = getDataTableMethodInfo2.ReturnType;

            // Assert
            parametersOfGetDataTable.ShouldNotBeNull();
            dataFunctionsAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getDataTableMethodInfo1.ShouldNotBeNull();
            getDataTableMethodInfo2.ShouldNotBeNull();
            getDataTableMethodInfo1.ShouldBe(getDataTableMethodInfo2);
            if(getDataTableMethodInfo1.DoesInvokeThrow(dataFunctionsAdapter, out invokeException, parametersOfGetDataTable))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOfGetDataTable), exceptionType: invokeException.GetType());
                Should.Throw(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOfGetDataTable), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOfGetDataTable));
                Should.NotThrow(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOfGetDataTable));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DataFunctionsAdapter_GetDataTable_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sql = Fixture.Create<string>();
            var connectionStringName = Fixture.Create<string>();
            Object[] parametersOutRanged = {sql, connectionStringName, null};
            Object[] parametersInDifferentNumber = {sql};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var dataFunctionsAdapter  = CreateAnalyzer.CreateOrReturnStaticInstance<DataFunctionsAdapter>(Fixture, out exception);
            var methodName = "GetDataTable";

            if(dataFunctionsAdapter != null)
            {
                // Act
                var getDataTableMethodInfo1 = dataFunctionsAdapter.GetType().GetMethod(methodName);
                var getDataTableMethodInfo2 = dataFunctionsAdapter.GetType().GetMethod(methodName);
                var returnType1 = getDataTableMethodInfo1.ReturnType;
                var returnType2 = getDataTableMethodInfo2.ReturnType;
                var result1 = getDataTableMethodInfo1.GetResultMethodInfo<DataFunctionsAdapter, DataTable>(dataFunctionsAdapter, out exception1, parametersOutRanged);
                var result2 = getDataTableMethodInfo2.GetResultMethodInfo<DataFunctionsAdapter, DataTable>(dataFunctionsAdapter, out exception2, parametersOutRanged);
                var result3 = getDataTableMethodInfo1.GetResultMethodInfo<DataFunctionsAdapter, DataTable>(dataFunctionsAdapter, out exception3, parametersInDifferentNumber);
                var result4 = getDataTableMethodInfo2.GetResultMethodInfo<DataFunctionsAdapter, DataTable>(dataFunctionsAdapter, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                dataFunctionsAdapter.ShouldNotBeNull();
                getDataTableMethodInfo1.ShouldNotBeNull();
                getDataTableMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOutRanged));
                    Should.NotThrow(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOutRanged));
                    Should.NotThrow(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getDataTableMethodInfo1.Invoke(dataFunctionsAdapter, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => getDataTableMethodInfo2.Invoke(dataFunctionsAdapter, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                dataFunctionsAdapter.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}