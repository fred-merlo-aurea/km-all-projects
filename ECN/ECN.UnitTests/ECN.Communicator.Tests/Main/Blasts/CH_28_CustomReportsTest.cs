using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.blastsmanager;
using ecn.communicator.blastsmanager.Fakes;
using ecn.common.classes.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts
{
	/// <summary>
	///     Unit tests for <see cref="ecn.communicator.blastsmanager.CH_28_CustomReports"/>
	/// </summary>
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class CH_28_CustomReportsTest
	{
		private const int CustomerID = 1;
		private const int UserID = 1;
		private const int SendTotal = 100;
		private const int BounceTotal = 50;
		private const string DistinctEmailCount = "10.10";
		private const string EmailCount = "10";
		private const string DummyString = "dummyString";
		private const string SendQuery = "BlastActivitySends";
		private const string MethodLoadFormData = "LoadFormData";
		private object[] _methodArgs;
		private CH_28_CustomReports _testObject;
		private IDisposable _shimObject;

		[SetUp]
		public void TestInitialize()
		{
			_shimObject = ShimsContext.Create();
		}

		[TearDown]
		public void TestCleanUp()
		{
			_shimObject.Dispose();
		}

		[Test]
		public void LoadFormData_Success_RespectiveFieldsContainData()
		{
			// Arrange
			InitilizeFakes();
			InitializeTestObject();
			InitializePropertiesAndFields();
			
			//Act
			CallMethod(typeof(CH_28_CustomReports), MethodLoadFormData, _methodArgs, _testObject);

			// Assert
			AssertPropertiesAndFields();
		}

		[Test]
		public void LoadFormData_WhenCalled_SuccessRateIsCalculatedAndShown()
		{
			// Arrange
			InitilizeFakes();
			InitializeTestObject();
			InitializePropertiesAndFields();
			ShimDataFunctions.ExecuteScalarStringString = (activity, sqlQuery) =>
			{
				return sqlQuery.Contains(SendQuery) ? SendTotal : BounceTotal;
			};

			//Act
			CallMethod(typeof(CH_28_CustomReports), MethodLoadFormData, _methodArgs, _testObject);

			// Assert
			var expectedSuccessful = "50/100";
			var expectedSuccessfulPercentage = "(50%)";
			var successful = (GetField(_testObject, "Successful") as Label).Text;
			var successfulPercentage = (GetField(_testObject, "SuccessfulPercentage") as Label).Text;
			_testObject.ShouldSatisfyAllConditions(
				() => successful.ShouldNotBeNullOrWhiteSpace(),
				() => successful.ShouldBe(expectedSuccessful),
				() => successfulPercentage.ShouldNotBeNullOrWhiteSpace(),
				() => successfulPercentage.ShouldBe(expectedSuccessfulPercentage));
		}

		private void InitilizeFakes()
		{
			ShimCH_28_CustomReports.AllInstances.MasterGet = (x) => new ecn.communicator.MasterPages.Communicator();
			ShimDataFunctions.GetDataTableString = (x) => GroupNameDataTable();
			ShimDataFunctions.ExecuteScalarStringString = (activity, sqlQuery) =>
			{ 
				return sqlQuery.Contains(SendQuery) ? SendTotal : BounceTotal;
			};
			ShimDataFunctions.GetDataTableStringString = (x, y) => ClicksDataTable();
			ShimECNSession.CurrentSession = () =>
			{
				var dummyCustormer = CreateInstance(typeof(Customer));
				dummyCustormer.CustomerID = CustomerID;
				var dummyUser = CreateInstance(typeof(User));
				dummyUser.UserID = UserID;
				var ecnSession = CreateInstance(typeof(ECNSession));
				SetField(ecnSession, "CurrentUser", dummyUser);
				SetField(ecnSession, "CurrentCustomer", dummyCustormer);
				return ecnSession;
			};
			ShimAuthenticationTicket.getTicket = () =>
			{
				var authTkt = CreateInstance(typeof(AuthenticationTicket));
				SetField(authTkt, "CustomerID", CustomerID);
				return authTkt;
			};
			ShimECNSession.AllInstances.RefreshSession = (item) => { };
			ShimECNSession.AllInstances.ClearSession = (itme) => { };
		}

		private DataTable ClicksDataTable()
		{
			var clicksDataTable = new DataTable
			{
				Columns =
				{
					"DistinctEmailCount",
					"EmailCount",
				}
			};
			clicksDataTable.Rows.Add(DistinctEmailCount, EmailCount);
			return clicksDataTable;
		}

		private DataTable GroupNameDataTable()
		{
			var groupNameDataTable = new DataTable
			{
				Columns =
				{
					"GroupName",
					"FilterName",
					"LayoutName",
					"EmailSubject",
					"EmailFrom",
					"EmailFromName",
					"SendTime",
					"FinishTime",
				}
			};
			groupNameDataTable.Rows.Add(DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString);
			return groupNameDataTable;
		}

		private void InitializePropertiesAndFields()
		{
			SetField(_testObject, "GroupTo", new Label());
			SetField(_testObject, "Filter", new Label());
			SetField(_testObject, "Campaign", new Label());
			SetField(_testObject, "EmailSubject", new Label());
			SetField(_testObject, "EmailFrom", new Label());
			SetField(_testObject, "SendTime", new Label());
			SetField(_testObject, "FinishTime", new Label());
			SetField(_testObject, "ClicksUnique", new Label());
			SetField(_testObject, "ClicksTotal", new Label());
			SetField(_testObject, "OpensUnique", new Label());
			SetField(_testObject, "OpensTotal", new Label());
			SetField(_testObject, "BouncesUnique", new Label());
			SetField(_testObject, "BouncesTotal", new Label());
			SetField(_testObject, "SubscribesUnique", new Label());
			SetField(_testObject, "SubscribesTotal", new Label());
			SetField(_testObject, "ResendsUnique", new Label());
			SetField(_testObject, "ResendsTotal", new Label());
			SetField(_testObject, "ForwardsUnique", new Label());
			SetField(_testObject, "ForwardsTotal", new Label());
			SetField(_testObject, "Successful", new Label());
			SetField(_testObject, "SuccessfulPercentage", new Label());
			SetField(_testObject, "ClicksPercentage", new Label());
			SetField(_testObject, "BouncesPercentage", new Label());
			SetField(_testObject, "OpensPercentage", new Label());
			SetField(_testObject, "SubscribesPercentage", new Label());
			SetField(_testObject, "ResendsPercentage", new Label());
			SetField(_testObject, "ForwardsPercentage", new Label());
		}

		private void AssertPropertiesAndFields()
		{
			var groupTo = (GetField(_testObject, "GroupTo") as Label).Text;
			var filter = (GetField(_testObject, "Filter") as Label).Text;
			var campaign = (GetField(_testObject, "Campaign") as Label).Text;
			var emailSubject = (GetField(_testObject, "EmailSubject") as Label).Text;
			var finishTime = (GetField(_testObject, "FinishTime") as Label).Text;
			_testObject.ShouldSatisfyAllConditions(
				() => groupTo.ShouldNotBeNullOrWhiteSpace(),
				() => groupTo.ShouldBe(DummyString),
				() => filter.ShouldNotBeNullOrWhiteSpace(),
				() => filter.ShouldBe(DummyString),
				() => campaign.ShouldNotBeNullOrWhiteSpace(),
				() => campaign.ShouldBe(DummyString),
				() => emailSubject.ShouldNotBeNullOrWhiteSpace(),
				() => emailSubject.ShouldBe(DummyString),
				() => finishTime.ShouldNotBeNullOrWhiteSpace(),
				() => finishTime.ShouldBe(DummyString));
		}

		private void InitializeTestObject()
		{
			var setBlastID = 1;
			_methodArgs = new object[] { setBlastID };
			_testObject = CreateInstance(typeof(CH_28_CustomReports));
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}

		private dynamic GetField(dynamic obj, string fieldName)
		{
			return ReflectionHelper.GetField(obj, fieldName);
		}

		private void SetSessionVariable(string name, object value)
		{
			HttpContext.Current.Session.Add(name, value);
		}

		private void SetProperty(dynamic instance, string propertyName, dynamic value)
		{
			ReflectionHelper.SetProperty(instance, propertyName, value);
		}

		private dynamic GetProperty(dynamic instance, string propertyName)
		{
			return ReflectionHelper.GetPropertyValue(instance, propertyName);
		}

		private dynamic CreateInstanceWithValues(Type type, dynamic values)
		{
			return ReflectionHelper.CreateInstanceWithValues(type, values);
		}
	}
}
