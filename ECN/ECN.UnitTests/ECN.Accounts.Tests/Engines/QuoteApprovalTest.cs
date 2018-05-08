using ecn.accounts.Engines;
using ecn.accounts.Engines.Fakes;
using ecn.accounts.includes.Fakes;
using ecn.common.classes.billing.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;

namespace ECN.Accounts.Tests.Engines
{
	/// <summary>
	///     Unit tests for <see cref="ecn.accounts.Engines.QuoteApproval"/>
	/// </summary>
	[TestFixture, ExcludeFromCodeCoverage]
	public partial class QuoteApprovalTest
	{
		private IDisposable _shimContext;
		private PrivateObject _quoteApprovalObject;
		private QuoteApproval _quoteApprovalInstance;
		private ShimQuoteApproval _shimQuoteApproval;
		private int _createBillMethodCallCount;
		private int _notifyNBDonQuoteStatusMethodCallCount;
		private int _saveAllMethodCallCount;
		private int _createLicensesMethodCallCount;
		private int _quoteSaveMethodCallCount;

		[SetUp]
		public void Setup()
		{
			_createBillMethodCallCount = 0;
			_notifyNBDonQuoteStatusMethodCallCount = 0;
			_saveAllMethodCallCount = 0;
			_createLicensesMethodCallCount = 0;
			_quoteSaveMethodCallCount = 0;
			_shimContext = ShimsContext.Create();
			InitShims();
			_quoteApprovalInstance = new QuoteApproval();
			_shimQuoteApproval = new ShimQuoteApproval(_quoteApprovalInstance);
			_quoteApprovalObject = new PrivateObject(_quoteApprovalInstance);
			_quoteApprovalObject.SetField("phdComponents", BindingFlags.NonPublic | BindingFlags.Instance, new PlaceHolder());
		}

		[TearDown]
		public void TearDown()
		{
			_shimContext.Dispose();
		}

		private HttpSessionState CreateSessionState()
		{
			var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
													  new HttpStaticObjectsCollection(), 10, true,
													  HttpCookieMode.AutoDetect,
													  SessionStateMode.InProc, false);
			var sessionState = typeof(HttpSessionState).GetConstructor(
										  BindingFlags.NonPublic | BindingFlags.Instance,
										  null, CallingConventions.Standard,
										  new[] { typeof(HttpSessionStateContainer) },
										  null)
									.Invoke(new object[] { sessionContainer }) as HttpSessionState;
			return sessionState;
		}

		private void InitShims()
		{
			ShimQuote.AllInstances.Save = (s) => _quoteSaveMethodCallCount++;
			ShimQuoteApproval.AllInstances.StepsGet = (q) => new ArrayList(Enum.GetValues(typeof(QuoteApproveStepsEnum)));
			ShimQuoteApproval.AllInstances.SaveAllBill = (obj, b) => _saveAllMethodCallCount++;
			ShimQuoteApproval.AllInstances.notifyNBDonQuoteStatus = (obj) => _notifyNBDonQuoteStatusMethodCallCount++;
			ShimQuoteApproval.AllInstances.CreateBill = (obj) => { _createBillMethodCallCount++; return null; };
			ShimQuoteApproval.AllInstances.CreateLicensesBill = (obj, bill) => _createLicensesMethodCallCount++;
			ShimContactEditor2.AllInstances.SetContactContact = (ce, arg) => { };
			ShimContactEditor2.AllInstances.CompanySetString = (ce, arg) => { };
			ShimUserInfoCollector.AllInstances.SetUserUser = (ic, user) => { };
			var sessionState = CreateSessionState();
			ShimPage.AllInstances.SessionGet = (c) =>
			{
				return sessionState;
			};
		}
	}
}
