using ecn.accounts.Engines;
using ecn.accounts.Engines.Fakes;
using ecn.accounts.includes;
using ecn.accounts.includes.Fakes;
using ecn.common.classes;
using ecn.common.classes.billing;
using ecn.common.classes.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Web.UI.Fakes;

namespace ECN.Accounts.Tests.Engines
{
	public partial class QuoteApprovalTest
	{
		[Test]
		public void PerformAction_ThankYouStep_True()
		{
			// Arrange
			var param = new object[] { (int)QuoteApproveStepsEnum.ThankYou - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeTrue();
		}

		[Test]
		public void PerformAction_ApproveQuoteStep_True()
		{
			// Arrange
			var param = new object[] { (int)QuoteApproveStepsEnum.ApproveQuote - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeTrue();
		}

		[Test]
		public void PerformAction_CreateAdminUserStep_Exception()
		{
			// Arrange
			var param = new object[] { (int)QuoteApproveStepsEnum.CreateAdminUser - 1 };

			// Act 
			Action action = () => _quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			action.ShouldThrow<ApplicationException>();
		}

		[Test]
		public void PerformAction_InputCCInfoStepWithoutAgreeToUseRecurringService_False()
		{
			// Arrange
			InitTest_InputCCInfoStep(agreeToUseRecurringService: false, isValidCard: true);
			var param = new object[] { (int)QuoteApproveStepsEnum.InputCCInfo - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeFalse();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void PerformAction_InputCCInfoStep_TrueIfValidCard(bool isValidCard)
		{
			//Arrange
			InitTest_InputCCInfoStep(agreeToUseRecurringService: true, isValidCard: isValidCard);
			var param = new object[] { (int)QuoteApproveStepsEnum.InputCCInfo - 1 };

			//Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			//Assert
			result.ShouldBe(isValidCard);
		}

		[Test]
		public void PerformAction_CreateCustomerStepNameExist_False()
		{
			// Arrange
			InitTest_CreateCustomerStep(true, out Quote currentQuote);
			var param = new object[] { (int)QuoteApproveStepsEnum.CreateCustomer - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeFalse();
		}

		[Test]
		public void PerformAction_CreateCustomerStepUniqueName_True()
		{
			// Arrange
			InitTest_CreateCustomerStep(false, out Quote currentQuote);
			var param = new object[] { (int)QuoteApproveStepsEnum.CreateCustomer - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeTrue();
			currentQuote.Status.ShouldBe(QuoteStatusEnum.Approved);
			_createBillMethodCallCount.ShouldBe(1);
			_notifyNBDonQuoteStatusMethodCallCount.ShouldBe(1);
			_saveAllMethodCallCount.ShouldBe(1);
			_createLicensesMethodCallCount.ShouldBe(1);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void PerformAction_CreateCustomerStepUniqueNameCreditCard_TrueIfCharged(bool chargeCreditCardResult)
		{
			// Arrange
			ShimQuoteApproval.AllInstances.ChargeCreditCardBill = (q, b) => chargeCreditCardResult;
			InitTest_CreateCustomerStep(false, out Quote currentQuote, "CreditCard");
			var param = new object[] { (int)QuoteApproveStepsEnum.CreateCustomer - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			if (chargeCreditCardResult)
			{
				result.ShouldBeTrue();
				currentQuote.Status.ShouldBe(QuoteStatusEnum.Approved);
				_saveAllMethodCallCount.ShouldBe(1);
				_createLicensesMethodCallCount.ShouldBe(1);
			}
			else
			{
				result.ShouldBeFalse();
				currentQuote.Status.ShouldBe(QuoteStatusEnum.Pending);
				_quoteSaveMethodCallCount.ShouldBe(1);
			}
		}

		[Test]
		public void PerformAction_CreateCustomerStepUniqueNameCreditCardChargeError_False()
		{
			// Arrange
			ShimQuoteApproval.AllInstances.ChargeCreditCardBill = (q, b) => throw new Exception();
			InitTest_CreateCustomerStep(false, out Quote currentQuote, "CreditCard");
			var param = new object[] { (int)QuoteApproveStepsEnum.CreateCustomer - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeFalse();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void PerformAction_ViewQuoteStepNewUser_TrueIfAgreeToTerms(bool agreeToTerms)
		{
			// Arrange
			InitTest_ViewQuoteStep(out Quote currentQuote, out QuoteViewer quoteViewer, out ContactEditor2 contactEditor, agreeToTerms, true);
			var param = new object[] { (int)QuoteApproveStepsEnum.ViewQuote - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldSatisfyAllConditions(() => result.ShouldBe(agreeToTerms));
			currentQuote.ShouldSatisfyAllConditions(
				() => currentQuote.Customer.BillingContact.ShouldBeSameAs(contactEditor.Contact),
				() => currentQuote.Customer.Name.ShouldBe(contactEditor.Company),
				() => currentQuote.Customer.TechContact.ShouldBe(contactEditor.Contact.ContactName),
				() => currentQuote.Customer.TechEmail.ShouldBe(contactEditor.Contact.Email),
				() => currentQuote.Customer.TechPhone.ShouldBe(contactEditor.Contact.Phone),
				() => currentQuote.StartDate.ShouldBe(quoteViewer.StartDate));
		}

		[Test]
		public void PerformAction_ViewQuoteStepUserExist_True()
		{
			// Arrange
			InitTest_ViewQuoteStep(out Quote currentQuote, out QuoteViewer quoteViewer, out ContactEditor2 contactEditor, quoteViewerAgreeToTerms: true, isNewCustomer: false);
			var param = new object[] { (int)QuoteApproveStepsEnum.ViewQuote - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeTrue();
			currentQuote.Status.ShouldBe(QuoteStatusEnum.Approved);
			_createBillMethodCallCount.ShouldBe(1);
			_notifyNBDonQuoteStatusMethodCallCount.ShouldBe(1);
			_saveAllMethodCallCount.ShouldBe(1);
			_createLicensesMethodCallCount.ShouldBe(1);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void PerformAction_ViewQuoteStepUserExistCreditCard_TrueIfCharged(bool chargeCreditCardResult)
		{
			// Arrange
			ShimQuoteApproval.AllInstances.ChargeCreditCardBill = (q, b) => chargeCreditCardResult;
			InitTest_ViewQuoteStep(out Quote currentQuote, out QuoteViewer quoteViewer, out ContactEditor2 contactEditor, quoteViewerAgreeToTerms: true, isNewCustomer: false, billType: "CreditCard");
			var param = new object[] { (int)QuoteApproveStepsEnum.ViewQuote - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			if (chargeCreditCardResult)
			{
				result.ShouldBeTrue();
				currentQuote.Status.ShouldBe(QuoteStatusEnum.Approved);
				_saveAllMethodCallCount.ShouldBe(1);
				_createLicensesMethodCallCount.ShouldBe(1);
			}
			else
			{
				result.ShouldBeFalse();
				currentQuote.Status.ShouldBe(QuoteStatusEnum.Pending);
				_quoteSaveMethodCallCount.ShouldBe(1);
			}
		}

		[Test]
		public void PerformAction_ViewQuoteStepUserExistCreditCardChargeError_False()
		{
			//Arrange
			ShimQuoteApproval.AllInstances.ChargeCreditCardBill = (q, b) => throw new Exception();
			InitTest_ViewQuoteStep(out Quote currentQuote, out QuoteViewer quoteViewer, out ContactEditor2 contactEditor, quoteViewerAgreeToTerms: true, isNewCustomer: false, billType: "CreditCard");
			var param = new object[] { (int)QuoteApproveStepsEnum.ViewQuote - 1 };

			// Act
			bool result = (bool)_quoteApprovalObject.Invoke("PerformAction", param);

			// Assert
			result.ShouldBeFalse();
		}

		private void InitTest_ViewQuoteStep(out Quote quoteInstance, out QuoteViewer quoteViewerInstance, out ContactEditor2 contactEditorInstance, bool quoteViewerAgreeToTerms, bool isNewCustomer, string billType = null)
		{
			var quoteViewer = new QuoteViewer();
			var currentDate = DateTime.Now;
			var contact = new Contact("salutation", "contactName", "title", "phone", "fax", "email", "address", "city", "state", "country", "zip");
			var customerName = "customerName";
            ShimCustomerBase.AllInstances.IsNewGet = (c) => isNewCustomer;
			var quote = new Quote(-1, new Customer());
			quote.BillType = billType;
			ShimQuoteApproval.AllInstances.CurrentQuoteGet = (q) => quote;
			ShimContactEditor2.AllInstances.IsTheSameAsBillingAddressGet = (ce) => true;
			ShimContactEditor2.AllInstances.IsTheSameAsTechContactGet = (ce) => true;
			ShimContactEditor2.AllInstances.GetContact = (ce) => contact;
			ShimContactEditor2.AllInstances.CompanyGet = (ce) => customerName;
			var contactEditor = new ContactEditor2();
			contactEditor.Contact = contact;
			var userInfoEditor = new UserInfoCollector();
			ShimQuoteViewer.AllInstances.ContactEditorGet = (qv) => contactEditor;
			ShimQuoteViewer.AllInstances.UserInfoEditorGet = (qv) => userInfoEditor;
			ShimQuoteViewer.AllInstances.AgreeToTermsAndConditionsGet = (qv) => quoteViewerAgreeToTerms;
			ShimQuoteViewer.AllInstances.StartDateGet = (qv) => currentDate;
			ShimControl.AllInstances.FindControlString = (c, id) =>
			{
				if (id == "uclQuoteViewer")
				{
					return quoteViewer;
				}
				return null;
			};
			quoteInstance = quote;
			contactEditorInstance = contactEditor;
			quoteViewerInstance = quoteViewer;
		}

		private void InitTest_InputCCInfoStep(bool agreeToUseRecurringService, bool isValidCard)
		{
			ShimCCInfoCollector.AllInstances.AgreeToUseRecurringServiceGet = (cc) => agreeToUseRecurringService;
			ShimCCInfoCollector.AllInstances.CreditCardGet = (cc) => null;
			ShimControl.AllInstances.FindControlString = (c, id) =>
			{
				if (id == "uclCCInforCollector")
				{
					return new CCInfoCollector();
				}
				return null;
			};
			Mock<ICreditCardProcessor> creditCardProcessorMock = new Mock<ICreditCardProcessor>();
			creditCardProcessorMock.Setup(x => x.IsCreditCardValid(null)).Returns(isValidCard);
			ShimQuoteApproval.AllInstances.CCHandlerGet = (q) => creditCardProcessorMock.Object;
		}

		private void InitTest_CreateCustomerStep(bool customerNameExit, out Quote quoteInstance, string billType = null)
		{
			ShimCustomerBase.AllInstances.CustomerNameExists = (c) => customerNameExit;
			var quote = new Quote(-1, new Customer());
			quote.BillType = billType;
			ShimCustomerInfoCollector.AllInstances.SetCustomerCustomer = (ci, customer) => { };
			ShimControl.AllInstances.FindControlString = (c, id) =>
			{
				if (id == "uclCustomerInfoCollector")
				{
					return new CustomerInfoCollector();
				}
				return null;
			};
			ShimQuoteApproval.AllInstances.CurrentQuoteGet = (q) => quote;
			quoteInstance = quote;
		}
	}
}
