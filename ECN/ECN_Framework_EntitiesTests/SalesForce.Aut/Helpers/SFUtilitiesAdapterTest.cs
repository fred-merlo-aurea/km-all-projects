using System;
using System.Net;
using AutoFixture;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Helpers;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_EntitiesTests.SalesForce.Aut.Helpers
{
    [TestFixture]
    public class SFUtilitiesAdapterTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : DelegateMethod

        #region DelegateLikeMethods

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetNextURL_Non_Static_DelegateMethod_With_Parameter_1_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var url = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetNextURL(url));
        	Should.Throw<Exception>(() => SF_Utilities.GetNextURL(url));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => GetNextURL)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetNextURL_Non_Static_DelegateMethod_With_1_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var url = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.GetNextURL(url);
            Action result2 = () => SF_Utilities.GetNextURL(url);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_LogWebException_Non_Static_DelegateMethod_With_Parameter_2_No_Exception_Thrown_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var ex = Fixture.Create<WebException>();
        	var requestURL = Fixture.Create<string>();    

        	// Act , Assert
            Should.NotThrow(() => sFUtilitiesAdapter.LogWebException(ex, requestURL));
            Should.NotThrow(() => SF_Utilities.LogWebException(ex, requestURL));
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateWebRequest_Non_Static_DelegateMethod_With_Parameter_1_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var url = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateWebRequest(url));
        	Should.Throw<Exception>(() => WebRequest.Create(url));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => CreateWebRequest)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateWebRequest_Non_Static_DelegateMethod_With_1_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var url = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.CreateWebRequest(url);
            Action result2 = () => WebRequest.Create(url);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_WriteToLog_Non_Static_DelegateMethod_With_Parameter_1_No_Exception_Thrown_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var text = Fixture.Create<string>();    

        	// Act , Assert
            Should.NotThrow(() => sFUtilitiesAdapter.WriteToLog(text));
            Should.NotThrow(() => SF_Utilities.WriteToLog(text));
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchResults_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var batchId = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId));
        	Should.Throw<Exception>(() => SF_Utilities.GetBatchResults(accessToken, jobId, batchId));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => GetBatchResults)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchResults_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var batchId = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId);
            Action result2 = () => SF_Utilities.GetBatchResults(accessToken, jobId, batchId);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateNewJob_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var sfObject = Fixture.Create<SF_Utilities.SFObject>();
        	var operation = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation));
        	Should.Throw<Exception>(() => SF_Utilities.CreateNewJob(accessToken, sfObject, operation));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => CreateNewJob)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateNewJob_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var sfObject = Fixture.Create<SF_Utilities.SFObject>();
        	var operation = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation);
            Action result2 = () => SF_Utilities.CreateNewJob(accessToken, sfObject, operation);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchState_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var batchId = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId));
        	Should.Throw<Exception>(() => SF_Utilities.GetBatchState(accessToken, jobId, batchId));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => GetBatchState)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchState_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var batchId = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId);
            Action result2 = () => SF_Utilities.GetBatchState(accessToken, jobId, batchId);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_AddBatchToJob_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var xmlString = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString));
        	Should.Throw<Exception>(() => SF_Utilities.AddBatchToJob(accessToken, jobId, xmlString));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => AddBatchToJob)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_AddBatchToJob_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();
        	var xmlString = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString);
            Action result2 = () => SF_Utilities.AddBatchToJob(accessToken, jobId, xmlString);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CloseJob_Non_Static_DelegateMethod_With_Parameter_2_Throw_Exception_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();    

        	// Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CloseJob(accessToken, jobId));
        	Should.Throw<Exception>(() => SF_Utilities.CloseJob(accessToken, jobId));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => CloseJob)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CloseJob_Non_Static_DelegateMethod_With_2_Parameters_Test()
        {
        	// Arrange
            var sFUtilitiesAdapter  = Fixture.Create<SFUtilitiesAdapter>();
        	var accessToken = Fixture.Create<string>();
        	var jobId = Fixture.Create<string>();    

            // Act	
            Action result1 = () => sFUtilitiesAdapter.CloseJob(accessToken, jobId);
            Action result2 = () => SF_Utilities.CloseJob(accessToken, jobId);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
        	result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #endregion

        #endregion

        #endregion
    }
}