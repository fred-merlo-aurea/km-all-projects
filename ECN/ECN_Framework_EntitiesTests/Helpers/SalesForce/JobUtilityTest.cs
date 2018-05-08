using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Salesforce.Helpers;
using ECN_Framework_Entities.Salesforce.Interfaces;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    [TestFixture]
    public class JobUtilityTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region Method call test

        #region Method Call Test : JobUtility => InitUtilities (Return type :  void)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_InitUtilities_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var utilities = Fixture.Create<ISFUtilities>();

            // Act, Assert
        	Should.NotThrow(() => JobUtility.InitUtilities(utilities));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_InitUtilities_Static_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();

            // Act
            Action initUtilities = () => JobUtility.InitUtilities(utilities);

            // Assert
            Should.NotThrow(() => initUtilities.Invoke());
            Should.NotThrow(() => JobUtility.InitUtilities(utilities));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_InitUtilities_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();
            Object[] parametersOfInitUtilities = {utilities};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "InitUtilities";

            // Act
            var initUtilitiesMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var initUtilitiesMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = initUtilitiesMethodInfo1.ReturnType;
            var returnType2 = initUtilitiesMethodInfo2.ReturnType;

            // Assert
            parametersOfInitUtilities.ShouldNotBeNull();
            jobUtility.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            initUtilitiesMethodInfo1.ShouldNotBeNull();
            initUtilitiesMethodInfo2.ShouldNotBeNull();
            initUtilitiesMethodInfo1.ShouldBe(initUtilitiesMethodInfo2);
            Should.NotThrow(() => initUtilitiesMethodInfo1.Invoke(jobUtility, parametersOfInitUtilities));
            Should.NotThrow(() => initUtilitiesMethodInfo2.Invoke(jobUtility, parametersOfInitUtilities));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_InitUtilities_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();
            Object[] parametersOutRanged = {utilities, null};
            Object[] parametersInDifferentNumber = {};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "InitUtilities";

            // Act
            var initUtilitiesMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var initUtilitiesMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = initUtilitiesMethodInfo1.ReturnType;
            var returnType2 = initUtilitiesMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            jobUtility.ShouldNotBeNull();
            initUtilitiesMethodInfo1.ShouldNotBeNull();
            initUtilitiesMethodInfo2.ShouldNotBeNull();
            initUtilitiesMethodInfo1.ShouldBe(initUtilitiesMethodInfo2);    
            Should.Throw<Exception>(() => initUtilitiesMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<Exception>(() => initUtilitiesMethodInfo2.Invoke(jobUtility, parametersOutRanged));    
            Should.Throw<Exception>(() => initUtilitiesMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<Exception>(() => initUtilitiesMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo2.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : JobUtility => Create (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Create_Static_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var accessToken = Fixture.Create<string>();
        	var operation = Fixture.Create<string>();
        	var sfObject = Fixture.Create<SF_Utilities.SFObject>();

            // Act, Assert
        	Should.Throw<Exception>(() => JobUtility.Create(accessToken, operation, sfObject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Create_Static_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var operation = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();

            // Act
            Func<string> create = () => JobUtility.Create(accessToken, operation, sfObject);

            // Assert
            Should.Throw<Exception>(() => create.Invoke());
            Should.Throw<Exception>(() => JobUtility.Create(accessToken, operation, sfObject));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Create_Static_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var operation = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();

            // Act
            Func<string> create1 = () => JobUtility.Create(accessToken, operation, sfObject);
            Func<string> create2 = () => JobUtility.Create(accessToken, operation, sfObject);
            var target1 = create1.Target;
            var target2 = create2.Target;

            // Assert
            create1.ShouldNotBeNull();
            create2.ShouldNotBeNull();
            create1.ShouldNotBe(create2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => create1.Invoke());
            Should.Throw<Exception>(() => create2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Create_Static_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var operation = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            Object[] parametersOfCreate = {accessToken, operation, sfObject};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "Create";

            // Act
            var createMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var createMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = createMethodInfo1.ReturnType;
            var returnType2 = createMethodInfo2.ReturnType;
        
            // Assert
            parametersOfCreate.ShouldNotBeNull();
            jobUtility.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createMethodInfo1.ShouldNotBeNull();
            createMethodInfo2.ShouldNotBeNull();
            createMethodInfo1.ShouldBe(createMethodInfo2);
            Should.Throw<Exception>(() => createMethodInfo1.Invoke(jobUtility, parametersOfCreate));
            Should.Throw<Exception>(() => createMethodInfo2.Invoke(jobUtility, parametersOfCreate));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Create_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var operation = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            Object[] parametersOutRanged = {accessToken, operation, sfObject, null};
            Object[] parametersInDifferentNumber = {accessToken, operation};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "Create";

            // Act
            var createMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var createMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = createMethodInfo1.ReturnType;
            var returnType2 = createMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            jobUtility.ShouldNotBeNull();
            createMethodInfo1.ShouldNotBeNull();
            createMethodInfo2.ShouldNotBeNull();
            createMethodInfo1.ShouldBe(createMethodInfo2);    
            Should.Throw<Exception>(() => createMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<Exception>(() => createMethodInfo2.Invoke(jobUtility, parametersOutRanged));    
            Should.Throw<Exception>(() => createMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<Exception>(() => createMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createMethodInfo2.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : JobUtility => Close (Return type :  bool)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Close_Static_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();

            // Act, Assert
        	Should.Throw<Exception>(() => JobUtility.Close(accessToken, jobId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Close_Static_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();

            // Act
            Func<bool> close = () => JobUtility.Close(accessToken, jobId);

            // Assert
            Should.Throw<Exception>(() => close.Invoke());
            Should.Throw<Exception>(() => JobUtility.Close(accessToken, jobId));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Close_Static_Method_With_2_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();

            // Act
            Func<bool> close1 = () => JobUtility.Close(accessToken, jobId);
            Func<bool> close2 = () => JobUtility.Close(accessToken, jobId);
            var target1 = close1.Target;
            var target2 = close2.Target;

            // Assert
            close1.ShouldNotBeNull();
            close2.ShouldNotBeNull();
            close1.ShouldNotBe(close2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => close1.Invoke());
            Should.Throw<Exception>(() => close2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Close_Static_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            Object[] parametersOfClose = {accessToken, jobId};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "Close";

            // Act
            var closeMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var closeMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = closeMethodInfo1.ReturnType;
            var returnType2 = closeMethodInfo2.ReturnType;

            // Assert
            parametersOfClose.ShouldNotBeNull();
            jobUtility.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            closeMethodInfo1.ShouldNotBeNull();
            closeMethodInfo2.ShouldNotBeNull();
            closeMethodInfo1.ShouldBe(closeMethodInfo2);
            Should.Throw<Exception>(() => closeMethodInfo1.Invoke(jobUtility, parametersOfClose));
            Should.Throw<Exception>(() => closeMethodInfo2.Invoke(jobUtility, parametersOfClose));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_Close_Static_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            Object[] parametersOutRanged = {accessToken, jobId, null};
            Object[] parametersInDifferentNumber = {accessToken};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "Close";

            // Act
            var closeMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var closeMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = closeMethodInfo1.ReturnType;
            var returnType2 = closeMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            jobUtility.ShouldNotBeNull();
            closeMethodInfo1.ShouldNotBeNull();
            closeMethodInfo2.ShouldNotBeNull();
            closeMethodInfo1.ShouldBe(closeMethodInfo2);    
            Should.Throw<Exception>(() => closeMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<Exception>(() => closeMethodInfo2.Invoke(jobUtility, parametersOutRanged));    
            Should.Throw<Exception>(() => closeMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<Exception>(() => closeMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => closeMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => closeMethodInfo2.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => closeMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => closeMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : JobUtility => AddBatch (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_AddBatch_Static_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var xmlString = Fixture.Create<string>();

            // Act, Assert
        	Should.Throw<Exception>(() => JobUtility.AddBatch(accessToken, jobId, xmlString));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_AddBatch_Static_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();

            // Act
            Func<string> addBatch = () => JobUtility.AddBatch(accessToken, jobId, xmlString);

            // Assert
            Should.Throw<Exception>(() => addBatch.Invoke());
            Should.Throw<Exception>(() => JobUtility.AddBatch(accessToken, jobId, xmlString));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_AddBatch_Static_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();

            // Act
            Func<string> addBatch1 = () => JobUtility.AddBatch(accessToken, jobId, xmlString);
            Func<string> addBatch2 = () => JobUtility.AddBatch(accessToken, jobId, xmlString);
            var target1 = addBatch1.Target;
            var target2 = addBatch2.Target;

            // Assert
            addBatch1.ShouldNotBeNull();
            addBatch2.ShouldNotBeNull();
            addBatch1.ShouldNotBe(addBatch2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => addBatch1.Invoke());
            Should.Throw<Exception>(() => addBatch2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_AddBatch_Static_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            Object[] parametersOfAddBatch = {accessToken, jobId, xmlString};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "AddBatch";

            // Act
            var addBatchMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var addBatchMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = addBatchMethodInfo1.ReturnType;
            var returnType2 = addBatchMethodInfo2.ReturnType;
           
            // Assert
            parametersOfAddBatch.ShouldNotBeNull();
            jobUtility.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            addBatchMethodInfo1.ShouldNotBeNull();
            addBatchMethodInfo2.ShouldNotBeNull();
            addBatchMethodInfo1.ShouldBe(addBatchMethodInfo2);
            Should.Throw<Exception>(() => addBatchMethodInfo1.Invoke(jobUtility, parametersOfAddBatch));
            Should.Throw<Exception>(() => addBatchMethodInfo2.Invoke(jobUtility, parametersOfAddBatch));
            
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_AddBatch_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            Object[] parametersOutRanged = {accessToken, jobId, xmlString, null};
            Object[] parametersInDifferentNumber = {accessToken, jobId};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "AddBatch";

            // Act
            var addBatchMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var addBatchMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = addBatchMethodInfo1.ReturnType;
            var returnType2 = addBatchMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            jobUtility.ShouldNotBeNull();
            addBatchMethodInfo1.ShouldNotBeNull();
            addBatchMethodInfo2.ShouldNotBeNull();
            addBatchMethodInfo1.ShouldBe(addBatchMethodInfo2);    
            Should.Throw<Exception>(() => addBatchMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<Exception>(() => addBatchMethodInfo2.Invoke(jobUtility, parametersOutRanged));    
            Should.Throw<Exception>(() => addBatchMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<Exception>(() => addBatchMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => addBatchMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => addBatchMethodInfo2.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => addBatchMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => addBatchMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : JobUtility => GetBatchState (Return type :  bool)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchState_Static_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var batchId = Fixture.Create<string>();

            // Act, Assert
        	Should.Throw<Exception>(() => JobUtility.GetBatchState(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchState_Static_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act
            Func<bool> getBatchState = () => JobUtility.GetBatchState(accessToken, jobId, batchId);

            // Assert
            Should.Throw<Exception>(() => getBatchState.Invoke());
            Should.Throw<Exception>(() => JobUtility.GetBatchState(accessToken, jobId, batchId));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchState_Static_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act
            Func<bool> getBatchState1 = () => JobUtility.GetBatchState(accessToken, jobId, batchId);
            Func<bool> getBatchState2 = () => JobUtility.GetBatchState(accessToken, jobId, batchId);
            var target1 = getBatchState1.Target;
            var target2 = getBatchState2.Target;

            // Assert
            getBatchState1.ShouldNotBeNull();
            getBatchState2.ShouldNotBeNull();
            getBatchState1.ShouldNotBe(getBatchState2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getBatchState1.Invoke());
            Should.Throw<Exception>(() => getBatchState2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchState_Static_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOfGetBatchState = {accessToken, jobId, batchId};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "GetBatchState";

            // Act
            var getBatchStateMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var getBatchStateMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = getBatchStateMethodInfo1.ReturnType;
            var returnType2 = getBatchStateMethodInfo2.ReturnType;

            // Assert
            parametersOfGetBatchState.ShouldNotBeNull();
            jobUtility.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getBatchStateMethodInfo1.ShouldNotBeNull();
            getBatchStateMethodInfo2.ShouldNotBeNull();
            getBatchStateMethodInfo1.ShouldBe(getBatchStateMethodInfo2);
            Should.Throw<Exception>(() => getBatchStateMethodInfo1.Invoke(jobUtility, parametersOfGetBatchState));
            Should.Throw<Exception>(() => getBatchStateMethodInfo2.Invoke(jobUtility, parametersOfGetBatchState));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchState_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOutRanged = {accessToken, jobId, batchId, null};
            Object[] parametersInDifferentNumber = {accessToken, jobId};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "GetBatchState";

            // Act
            var getBatchStateMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var getBatchStateMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = getBatchStateMethodInfo1.ReturnType;
            var returnType2 = getBatchStateMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            jobUtility.ShouldNotBeNull();
            getBatchStateMethodInfo1.ShouldNotBeNull();
            getBatchStateMethodInfo2.ShouldNotBeNull();
            getBatchStateMethodInfo1.ShouldBe(getBatchStateMethodInfo2);    
            Should.Throw<Exception>(() => getBatchStateMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<Exception>(() => getBatchStateMethodInfo2.Invoke(jobUtility, parametersOutRanged));    
            Should.Throw<Exception>(() => getBatchStateMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getBatchStateMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo2.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : JobUtility => GetBatchResults (Return type :  Dictionary<string, int>)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchResults_Static_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var batchId = Fixture.Create<string>();

            // Act, Assert
        	Should.Throw<Exception>(() => JobUtility.GetBatchResults(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchResults_Static_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act
            Func<Dictionary<string, int>> getBatchResults = () => JobUtility.GetBatchResults(accessToken, jobId, batchId);

            // Assert
            Should.Throw<Exception>(() => getBatchResults.Invoke());
            Should.Throw<Exception>(() => JobUtility.GetBatchResults(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void JobUtility_GetBatchResults_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOutRanged = {accessToken, jobId, batchId, null};
            Object[] parametersInDifferentNumber = {accessToken, jobId};
            var jobUtility  = Fixture.Create<JobUtility>();
            var methodName = "GetBatchResults";

            // Act
            var getBatchResultsMethodInfo1 = jobUtility.GetType().GetMethod(methodName);
            var getBatchResultsMethodInfo2 = jobUtility.GetType().GetMethod(methodName);
            var returnType1 = getBatchResultsMethodInfo1.ReturnType;
            var returnType2 = getBatchResultsMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            jobUtility.ShouldNotBeNull();
            getBatchResultsMethodInfo1.ShouldNotBeNull();
            getBatchResultsMethodInfo2.ShouldNotBeNull();
            getBatchResultsMethodInfo1.ShouldBe(getBatchResultsMethodInfo2);    
            Should.Throw<Exception>(() => getBatchResultsMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<Exception>(() => getBatchResultsMethodInfo2.Invoke(jobUtility, parametersOutRanged));    
            Should.Throw<Exception>(() => getBatchResultsMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getBatchResultsMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo1.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo2.Invoke(jobUtility, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo1.Invoke(jobUtility, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo2.Invoke(jobUtility, parametersInDifferentNumber));
        }

        #endregion

        #endregion
        
        #endregion

        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_JobUtility_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new JobUtility());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<JobUtility>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #endregion

        #endregion
    }
}