using System;
using SourceMediaPaidPub.Objects;

namespace SourceMediaPaidPub.Process
{
	public class PostDataParams
	{
		public PostDataParams()
		{
		}

		///  <summary>
		///  DefaultPost Params to ECN
		///  </summary>
		///  <param name="groupID"></param>
		///  <param name="term"></param>
		///  <param name="customerID"></param>
		///  <param name="email"></param>
		///  <param name="cardHolderName"></param>
		///  <param name="cardTypeValue"></param>
		///  <param name="creditCardNumber"></param>
		///  <param name="userExpirationMonth"></param>
		///  <param name="userExpirationYear"></param>
		///  <param name="totalAmount"></param>
		///  <param name="taxAmount"></param>
		///  <param name="billingAddress"></param>
		///  <param name="billingAddress2"></param>
		///  <param name="billingCity"></param>
		/// 	<param name="shippingAddress"></param>
		///  <param name="shippingAddress2"></param>
		///  <param name="shippingCity"></param>
		///  <param name="selectedshippingState"></param>
		///  <param name="shippingZip"></param>
		///  <param name="countryCode"></param>
		///  <param name="selectedBillingState"></param>
		///  <param name="billingZip"></param>
		///  <param name="transactionID"></param>
		///  <param name="isSubscriptionRenewal"></param>
		///  <param name="termStartDate"></param>
		///  <param name="termEndDate"></param>
		///  <param name="premiumPubCode"></param>
		///  <param name="taxjar"></param>
		/// <param name="itemPrice"></param>
		/// <param name="isPremium"></param>
		public PostDataParams(string groupID
									, int term
									, string customerID
									, string email
									, string cardHolderName
									, string cardTypeValue
									, string creditCardNumber
									, string userExpirationMonth
									, string userExpirationYear
									, double totalAmount
									, double taxAmount
									, string billingAddress
									, string billingAddress2
									, string billingCity
									, string shippingAddress
									, string shippingAddress2
									, string shippingCity
									, string selectedshippingState
									, string shippingZip
									, string countryCode
									, string selectedBillingState
									, string billingZip
									, string transactionID
									, bool isSubscriptionRenewal
									, DateTime termStartDate
									, DateTime termEndDate
									, double itemPrice
									, bool isPremium)
		{
			GroupId = groupID;
			Term = term;
			CustomerId = customerID;
			Email = email;
			TotalAmount = totalAmount;
			TaxAmount = taxAmount;
			CountryCode = countryCode;
			TransactionID = transactionID;
			IsSubscriptionRenewal = isSubscriptionRenewal;
			GetCreditCarduserInformation(cardHolderName, cardTypeValue, creditCardNumber, userExpirationMonth, userExpirationYear);
			GetBillingInformation(billingAddress, billingAddress2, billingCity, selectedBillingState, billingZip);
			GetShippingInformation(shippingAddress, shippingAddress2, shippingCity, selectedshippingState, shippingZip);
			GetTermStartDateAndEndDate(termStartDate, termEndDate);
			ItemPrice = itemPrice;
			IsPremium = isPremium;
		}

		

		private void GetTermStartDateAndEndDate(DateTime termStartDate, DateTime termEndDate)
		{
			TermStartDate = termStartDate;
			TermEndDate = termEndDate;
		}

		private void GetCreditCarduserInformation(string cardHolderName, string cardTypeValue, string creditCardNumber,
			string userExpirationMonth, string userExpirationYear)
		{
			CardHolderName = cardHolderName;
			CardTypeValue = cardTypeValue;
			CreditCardNumber = creditCardNumber;
			UserExpirationMonth = userExpirationMonth;
			UserExpirationYear = userExpirationYear;
		}

		private void GetBillingInformation(string billingAddress, string billingAddress2, string billingCity,
			string selectedBillingState, string billingZip)
		{
			BillingAddress = billingAddress;
			BillingAddress2 = billingAddress2;
			BillingCity = billingCity;
			SelectedBillingState = selectedBillingState;
			BillingZip = billingZip;
		}

		private void GetShippingInformation(string shippingAddress, string shippingAddress2, string shippingCity,
			string shippingState, string shippingZip)
		{
			ShippingAddress = shippingAddress;
			ShippingAddress2 = shippingAddress2;
			ShippingState = shippingState;
			ShippingZip = shippingZip;
			ShippingCity = shippingCity;
		}

		public bool IsPremium { get; set; }
		public string ShippingCity { get; set; }

		public string ShippingZip { get; set; }

		public string ShippingAddress2 { get; set; }

		public string ShippingAddress { get; set; }

		public string GroupId { get; set; }

		public int Term { get; set; }

		public string CustomerId { get; set; }

		public string Email { get; set; }

		public string CardHolderName { get; set; }

		public string CardTypeValue { get; set; }

		public string CreditCardNumber { get; set; }

		public string UserExpirationMonth { get; set; }

		public string UserExpirationYear { get; set; }

		public double TotalAmount { get; set; }

		public double TaxAmount { get; set; }

		public string BillingAddress { get; set; }

		public string BillingAddress2 { get; set; }

		public string BillingCity { get; set; }

		public string CountryCode { get; set; }

		public string SelectedBillingState { get; set; }

		public string TextBillingState { get; set; }

		public string BillingZip { get; set; }

		public string TransactionID { get; set; }


		public bool IsSubscriptionRenewal { get; set; }

		public string ShippingState { get; set; }

		public DateTime TermStartDate { get; set; }

		public DateTime TermEndDate { get; set; }

		public double ItemPrice { get; set; }
	}
}