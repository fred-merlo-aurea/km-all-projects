using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceMediaPaidPub.Objects;
using SourceMediaPaidPub.Process;

namespace SourceMediaPaidPubTest.Tests
{
	[TestFixture]
	public class TestSubscriberProcess
	{
		private SubscriberProcess _subscriberProcess;
		private SourcemediaPubcode _sourcemediaPubcodeAON;

		[SetUp]
		public void Setup()
		{
			_subscriberProcess = new SubscriberProcess();

			var stringFilePath = ConfigurationManager.AppSettings["MagazineJson"].ToString();

			_sourcemediaPubcodeAON = _subscriberProcess.GetMagazineList(stringFilePath).SourcemediaPubcodes.SingleOrDefault(x=>x.PubCode == "AON");
		}

		[TearDown]
		public void TearDown()
		{

		}

		[Test]
		public void GetMagazinesTest()
		{
			// Arrange
			var filePath = ConfigurationManager.AppSettings["MagazineJson"];


			// Act
			var magazine = _subscriberProcess.GetMagazineList(filePath);

			// Assert

			Assert.IsNotNull(magazine);
			Assert.IsNotNull(magazine.SourcemediaPubcodes);
			Assert.IsNotNull(magazine.SourcemediaPubcodes[0].PubCode);
			Assert.IsNotNull(magazine.SourcemediaPubcodes[0].GroupID);
			Assert.IsNotNull(magazine.SourcemediaPubcodes[0].CustomerID);
			Assert.IsNotNull(magazine.SourcemediaPubcodes[0].Title);
			Assert.IsNotNull(magazine.SourcemediaPubcodes[0].PriceRanges);
		}

		[Test]
		public void GetPricesFromAmountPaidLastYearTest()
		{
			// Arrange
			string pubCode = "AON";
			var magazine = new Magazine();
			magazine.SourcemediaPubcodes = new List<SourcemediaPubcode>();
			SourcemediaPubcode sourcemediaPubcode = new SourcemediaPubcode();

			sourcemediaPubcode.PriceRanges = new List<PriceRange>
			{
				new PriceRange
				{
					PaidFrom = 0.01,
					PaidTo = 99.00,
					PriceCollection = new List<Price>
					{
						new Price
						{
							Term = "1",
							Type = Price.PriceType.Standard,
							Country = "US"

						}
					}
				}
			};

			double amountPaidLastYear = 89.0;
			// Act
			var priceRanges = _subscriberProcess.GetPricesFromAmountPaidLastYear(sourcemediaPubcode, amountPaidLastYear);


			// Assert
			Assert.IsNotNull(priceRanges);
		}

		[Test]
		public void GetPricesFromLastYearForCountryTest()
		{
			// Arrange
			var priceRange = new PriceRange
			{
				PaidFrom = 0.01,
				PaidTo = 99.00,
				PriceCollection = new List<Price>
				{
					new Price
					{
						Term = "1",
						Type = Price.PriceType.Standard,
						Country = "US",
						Value = (float) Convert.ToDouble("99.0")

					}
				}
			};

			var countryCode = "US";


			// Act
			var prices = _subscriberProcess.GetPricesFromLastYearForCountry(priceRange, countryCode);


			// Assert
			Assert.AreEqual(prices.Count, 1);
		}

		[Test]
		public void TestCountryCodeByName()
		{
			// Arrange

			string countryName = "UNITED STATES OF AMERICA";
			string filePath = @"C:\sourceMediaPaidPub\SourceMediaPaidPub\countries.json";

			//Act
			var countryCode = _subscriberProcess.GetByCountryCodeByName(countryName, filePath);


			//Assert
			Assert.AreEqual("US", countryCode);
		}

		[Test]
		public void TestGetPricesFromAmountPaidLastYear()
		{
			//Arrange
			var sourceMediaPubCode = new SourcemediaPubcode();
			IList<PriceRange> priceRanges = new List<PriceRange>
			{
				new PriceRange
				{
					PaidFrom = 0.00,
					PaidTo = 0.00,
					PriceCollection = new List<Price>
					{
						new Price
						{
							Term = "1",
							Type = Price.PriceType.Standard,
							Country = "US",
							Value = (float) Convert.ToDouble("99.0")

						}
					}
				}
			};
			sourceMediaPubCode.PriceRanges = priceRanges;

			var amountPaidLastYear = 0.0;

			//Act

			var ranges = _subscriberProcess.GetPricesFromAmountPaidLastYear(sourceMediaPubCode, amountPaidLastYear);

			//Assert
			Assert.IsNotNull(ranges);
		}


		[Test]
		public void TestPriceForCountryTerm()
		{
			// Arrange
			var prices = new List<Price>
			{
				new Price
				{
					Country = "US",
					Term = "1",
					Type = Price.PriceType.Standard,
					Value = (float) Convert.ToDouble("200.0")
				}
			};

			string countryCode = "US";

			string term = "1";

			Price.PriceType priceType = Price.PriceType.Standard;

			// Act
			var price = _subscriberProcess.GetPriceForCountryTerm(prices, countryCode, term, priceType);

			// Assert
			Assert.IsNotNull(price);
			Assert.AreEqual(price.Value, 200.0);
		}

		[Test]
		public void TestGetZipTaxInfo()
		{

			// Arrange

			var apiKey = "583482822573cd1421d8efffdd0368ab";
			var zipCode = "55344";
			var countryCode = "US";

			// Act
			var zipTax = _subscriberProcess.GetTaxjarInfo(apiKey, zipCode, countryCode);

			// Assert
			Assert.IsNotNull(zipTax);

		}

		[Test]
		public void ValidateStatesHasTaxes()
		{
			// Arrange
			string stateCode = "AZ";

			var sourceMediaPubCode = new SourcemediaPubcode
			{
				States = "AZ,CA,DC,MD,NC,NY,SD,TX,001"
			};

			string countryCode = "CA";

			// Act
			bool zipCodeHasTax = _subscriberProcess.ValidateStatesHasTaxesByZipCode(stateCode, sourceMediaPubCode, countryCode);

			//Assert
			Assert.AreEqual(zipCodeHasTax, true);
		}

		[Test]
		public void TestGetGoogleApiInfo()
		{

			// Arrange
			var zipCode = "55344";


			// Act
			var googleApi = _subscriberProcess.GetGoogleAPIInfo(zipCode);

			// Assert
			Assert.IsNotNull(googleApi);

		}

		[Test]
		public void DefaultShipToParamsTestWithUSA()
		{
			// Arrange
			string shippingAddress = "1234 executive lane";
			int term = 1;
			string customerID = "blahblah";
			string shippingAddress2 = "some value";
			string shippingCity = "blahblahblah";
			string shippingZip = "55344";
			string countryCode = "United States Of America";
			string cardHolderName = "Name on card";
			string creditCardnumber = "4012888888881881";
			string groupId = "332649";
			string email = "praneeth.palli@blahblahblha.com";
			string shippingState = "MN";
			DateTime termStartDate = DateTime.Now;
			DateTime termEndDate = termStartDate.AddYears(term);

			var defaultPostDataParams = new PostDataParams
			{
				ShippingAddress = shippingAddress,
				Term = term,
				CustomerId = customerID,
				ShippingAddress2 = shippingAddress2,
				ShippingCity = shippingCity,
				ShippingZip = shippingZip,
				ShippingState = shippingState,
				CountryCode = countryCode,
				CardHolderName = cardHolderName,
				CreditCardNumber = creditCardnumber,
				GroupId = groupId,
				Email = email,
				TermStartDate = termStartDate,
				TermEndDate = termEndDate
			};

			// Act
			StringBuilder stringBuilder = _subscriberProcess.DefaultPostData(defaultPostDataParams);

			// Assert

			Assert.AreEqual(
				"g=332649&user_t_Term=1&e=praneeth.palli@blahblahblha.com&f=html&c=blahblah&user_PaymentStatus=paid&user_PAIDorFREE=PAID&user_t_FirstName=Name&user_t_LastName=on card&user_t_CardType=&user_t_CardNumber=4012********1881&user_t_ExpirationDate=/&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy") + "&user_t_AmountPaid=0.00&user_t_TaxPaid=0.00&user_t_Street=&user_t_Street2=&user_t_City=&user_t_Country=United States Of America&user_t_State=&user_SHIPTO_STATE=MN&user_SHIPTO_ZIP=55344&user_t_Zip=&user_t_TransactionID=&user_t_Renewal=False&user_SHIPTO_ADDRESS1=1234 executive lane&user_SHIPTO_ADDRESS2=some value&user_SHIPTO_CITY=blahblahblah&user_t_TermStartDate=" + termStartDate.Date.ToString("MM/dd/yyyy") + "&user_t_TermEndDate=" + termEndDate.Date.ToString("MM/dd/yyyy") + "&user_t_itemPrice=0&user_t_ispremium=False",
				stringBuilder.ToString());
		}

		[Test]
		public void DefaultShipToParamsTestWithCardHolderNameNoLastName()
		{
			// Arrange
			string shippingAddress = "1234 executive lane";
			int term = 1;
			string customerID = "blahblah";
			string shippingAddress2 = "some value";
			string shippingCity = "blahblahblah";
			string shippingZip = "55344";
			string countryCode = "United States Of America";
			string cardHolderName = "Name";
			string creditCardnumber = "4012888888881881";
			string groupId = "332649";
			string email = "praneeth.palli@blahblahblha.com";
			string shippingState = "MN";
			DateTime termStartDate = DateTime.Now;
			DateTime termEndDate = termStartDate.AddYears(term);

			var defaultPostDataParams = new PostDataParams
			{
				ShippingAddress = shippingAddress,
				Term = term,
				CustomerId = customerID,
				ShippingAddress2 = shippingAddress2,
				ShippingCity = shippingCity,
				ShippingZip = shippingZip,
				ShippingState = shippingState,
				CountryCode = countryCode,
				CardHolderName = cardHolderName,
				CreditCardNumber = creditCardnumber,
				GroupId = groupId,
				Email = email,
				TermStartDate = termStartDate,
				TermEndDate = termEndDate
			};

			// Act
			StringBuilder stringBuilder = _subscriberProcess.DefaultPostData(defaultPostDataParams);

			// Assert

			Assert.AreEqual(
				"g=332649&user_t_Term=1&e=praneeth.palli@blahblahblha.com&f=html&c=blahblah&user_PaymentStatus=paid&user_PAIDorFREE=PAID&user_t_FirstName=Name&user_t_LastName=&user_t_CardType=&user_t_CardNumber=4012********1881&user_t_ExpirationDate=/&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy") + "&user_t_AmountPaid=0.00&user_t_TaxPaid=0.00&user_t_Street=&user_t_Street2=&user_t_City=&user_t_Country=United States Of America&user_t_State=&user_SHIPTO_STATE=MN&user_SHIPTO_ZIP=55344&user_t_Zip=&user_t_TransactionID=&user_t_Renewal=False&user_SHIPTO_ADDRESS1=1234 executive lane&user_SHIPTO_ADDRESS2=some value&user_SHIPTO_CITY=blahblahblah&user_t_TermStartDate=" + termStartDate.Date.ToString("MM/dd/yyyy") + "&user_t_TermEndDate=" + termEndDate.Date.ToString("MM/dd/yyyy") + "&user_t_itemPrice=0&user_t_ispremium=False",
				stringBuilder.ToString());
		}

		[Test]
		[Ignore("Need to validate international countries")]
		public void DefaultShipToParamsTestWithInetrantionCountry()
		{
			// Arrange
			string shippingAddress = "1234 executive lane";
			int term = 1;
			string customerID = "blahblah";
			string shippingAddress2 = "some value";
			string shippingCity = "blahblahblah";
			string shippingZip = "55344";
			string countryCode = "Somalia";
			string cardHolderName = "Name";
			string creditCardnumber = "4012888888881881";
			string groupId = "332649";
			string email = "praneeth.palli@blahblahblha.com";
			string shippingState = "MN";
			DateTime termStartDate = DateTime.Now;
			DateTime termEndDate = termStartDate.AddYears(term);

			var defaultPostDataParams = new PostDataParams
			{
				ShippingAddress = shippingAddress,
				Term = term,
				CustomerId = customerID,
				ShippingAddress2 = shippingAddress2,
				ShippingCity = shippingCity,
				ShippingZip = shippingZip,
				ShippingState = shippingState,
				CountryCode = countryCode,
				CardHolderName = cardHolderName,
				CreditCardNumber = creditCardnumber,
				GroupId = groupId,
				Email = email,
				TermStartDate = termStartDate,
				TermEndDate = termEndDate
			};

			// Act
			StringBuilder stringBuilder = _subscriberProcess.DefaultPostData(defaultPostDataParams);

			// Assert

			Assert.AreEqual(
				"g=332649&user_t_Term=1&e=praneeth.palli@blahblahblha.com&f=html&c=blahblah&user_PaymentStatus=paid&user_PAIDorFREE=PAID&user_t_FirstName=Name&user_t_LastName=&user_t_CardType=&user_t_CardNumber=4012********1881&user_t_ExpirationDate=/&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy") + "&user_t_AmountPaid=0.00&user_t_TaxPaid=0.00&user_t_Street=&user_t_Street2=&user_t_City=&user_t_Country=Somalia&user_t_State=INT&user_SHIPTO_STATE=INT&user_SHIPTO_STATE_INT=INT&user_SHIPTO_ZIP=INT&user_t_Zip=INT&user_t_TransactionID=&user_t_Renewal=False&user_SHIPTO_ADDRESS1=1234 executive lane&user_SHIPTO_ADDRESS2=some value&user_SHIPTO_CITY=blahblahblah&user_t_TermStartDate=" + termStartDate.Date.ToString("MM/dd/yyyy") + "&user_t_TermEndDate=" + termEndDate.Date.ToString("MM/dd/yyyy"),
				stringBuilder.ToString());
		}

		[Test]
		[Ignore("Should fix this test")]
		public void TestPremiumAmountTaxJarInformation()
		{
			//Arrange

			PremiumPubCode premiumPubCode = new PremiumPubCode
			{
				Price = 120
			};

			Taxjar taxjar = new Taxjar
			{
				Rate = new Rate
				{
					CombinedRate = "2"
				}
			};

			string shippingAddress = "1234 executive lane";
			int term = 1;
			string customerID = "blahblah";
			string shippingAddress2 = "some value";
			string shippingCity = "blahblahblah";
			string shippingZip = "55344";
			string countryCode = "United States Of America";
			string cardHolderName = "Name";
			string creditCardnumber = "4012888888881881";
			string groupId = "332649";
			string email = "praneeth.palli@blahblahblha.com";
			string shippingState = "MN";
			DateTime termStartDate = DateTime.Now;
			DateTime termEndDate = termStartDate.AddYears(term);

			var defaultPostDataParams = new PostDataParams
			{
				ShippingAddress = shippingAddress,
				Term = term,
				CustomerId = customerID,
				ShippingAddress2 = shippingAddress2,
				ShippingCity = shippingCity,
				ShippingZip = shippingZip,
				ShippingState = shippingState,
				CountryCode = countryCode,
				CardHolderName = cardHolderName,
				CreditCardNumber = creditCardnumber,
				GroupId = groupId,
				Email = email,
				TermStartDate = termStartDate,
				TermEndDate = termEndDate,
			};

			// Act
			StringBuilder stringBuilder = _subscriberProcess.DefaultPostData(defaultPostDataParams);
			
			//Assert
			
				Assert.AreEqual(
				"g=332649&user_t_Term=1&e=praneeth.palli@blahblahblha.com&f=html&c=blahblah&user_PaymentStatus=paid&user_PAIDorFREE=PAID&user_t_FirstName=Name&user_t_LastName=&user_t_CardType=&user_t_CardNumber=4012********1881&user_t_ExpirationDate=/&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy") + "&user_t_AmountPaid=120.00&user_t_TaxPaid=240.00&user_t_Street=&user_t_Street2=&user_t_City=&user_t_Country=United States Of America&user_t_State=&user_SHIPTO_STATE=MN&user_SHIPTO_ZIP=55344&user_t_Zip=&user_t_TransactionID=&user_t_Renewal=False&user_SHIPTO_ADDRESS1=1234 executive lane&user_SHIPTO_ADDRESS2=some value&user_SHIPTO_CITY=blahblahblah&user_t_TermStartDate=" + termStartDate.Date.ToString("MM/dd/yyyy") + "&user_t_TermEndDate=" + termEndDate.Date.ToString("MM/dd/yyyy"),
				stringBuilder.ToString());

		}

		[Test]
		public void TestCopyAddressClickLink()
		{
			// Arrange

			string billingAddress = "3160 Tejon St";

			string billingAddress2 = String.Empty;

			string billingCity = "Denver";

			string billingZip = "80211";

			string countryName = "United States Of America";

			// Act
			Shipping shipping = _subscriberProcess.CopyAddress(billingAddress, billingAddress2, billingCity, billingZip,countryName);



			//Assert
			Assert.NotNull(shipping, null);
			Assert.AreEqual("3160 Tejon St", shipping.ShippingAddress);
			Assert.AreEqual("", shipping.ShippingAddress2);
			Assert.AreEqual("Denver", shipping.ShippingCity);
			Assert.AreEqual("80211", shipping.ShippingZip);
		}

		[Test]
		public void TestPremiumPubCodeData()
		{
			// Arrange
			double premiumPrice = 3795.00;
			double standardPrice = 2995.00;

			double taxpercentage = 0.081;


			// Act

			var calculatedPremiumPubCodes = _subscriberProcess.GetPremiumPubCodeCalculation(premiumPrice, standardPrice, taxpercentage);

			// Assert
			Assert.AreEqual(800, calculatedPremiumPubCodes.PremiumPubCodePrice);
			Assert.AreEqual(64.8,calculatedPremiumPubCodes.PremiumPubCodeTaxPrice);
			Assert.AreEqual(864.8,calculatedPremiumPubCodes.TotalPremiumPubCodePrice);
		}

		[Test]
		[TestCase(1, "55344", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001","US")]
		[TestCase(2, "55344", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001","US")]
		[TestCase(1, "55344", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001","US")]
		[TestCase(2, "55344", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001","US")]
		public void TestAmountForSendToECNForTermForZipCode55344(int term, string shippingZipCode, Price.PriceType selectedType, string states, string countryCode)
		{
			// Arrange

			Taxjar taxjar = new Taxjar
			{
				Rate = new Rate
				{
					CombinedRate = "0.081"
				}
			};

			_sourcemediaPubcodeAON.States = states;

			string email = " and emails.emailaddress = '" + "jay@jayfeldmaninc.com" + "' ";
			string connectionString = ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;

			
			DateTime dateTime = Convert.ToDateTime("05/28/2016");
			DateTime currentDateTime = _subscriberProcess.GetCurrentDateTime(dateTime);
			

			// Act
			var subscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar,term.ToString(),_sourcemediaPubcodeAON, email, countryCode, selectedType, connectionString, currentDateTime, shippingZipCode, false);

			// Assert

			CheckAssertsForZipCode(subscription, shippingZipCode, term, selectedType);
		}

		
		[Test]
		[TestCase(1, "80001", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		[TestCase(2, "80001", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		[TestCase(1, "80001", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		[TestCase(2, "80001", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		public void TestAmountForSendToECNForTermForZipCode80001(int term, string shippingZipCode, Price.PriceType selectedType, string states, string countryCode)
		{
			// Arrange

			Taxjar taxjar = new Taxjar
			{
				Rate = new Rate
				{
					CombinedRate = "0.081"
				}
			};

			_sourcemediaPubcodeAON.States = states;

			string email = " and emails.emailaddress = '" + "jay@jayfeldmaninc.com" + "' ";
			string connectionString = ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;


			DateTime dateTime = Convert.ToDateTime("05/28/2016");
			DateTime currentDateTime = _subscriberProcess.GetCurrentDateTime(dateTime);


			// Act
			var subscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, term.ToString(), _sourcemediaPubcodeAON, email, countryCode, selectedType, connectionString, currentDateTime, shippingZipCode, false);

			// Assert

			CheckAssertsForZipCode(subscription, shippingZipCode, term, selectedType);
		}


		[Test]
		[TestCase(1, "500009", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "INT")]
		[TestCase(2, "500009", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "INT")]
		[TestCase(1, "500009", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "INT")]
		[TestCase(2, "500009", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "INT")]
		public void TestAmountForSendToECNForTermForZipCode500009(int term, string shippingZipCode, Price.PriceType selectedType, string states, string countryCode)
		{
			// Arrange

			Taxjar taxjar = new Taxjar
			{
				Rate = new Rate
				{
					CombinedRate = "0.081"
				}
			};

			_sourcemediaPubcodeAON.States = states;

			string email = " and emails.emailaddress = '" + "jay@jayfeldmaninc.com" + "' ";
			string connectionString = ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;


			DateTime dateTime = Convert.ToDateTime("05/28/2016");
			DateTime currentDateTime = _subscriberProcess.GetCurrentDateTime(dateTime);


			// Act
			var subscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, term.ToString(), _sourcemediaPubcodeAON, email, countryCode, selectedType, connectionString, currentDateTime, shippingZipCode, false);

			// Assert

			CheckAssertsForZipCode(subscription, shippingZipCode, term, selectedType);
		}

		[Test]
		[TestCase(1, "", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		[TestCase(2, "", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		[TestCase(1, "", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		[TestCase(2, "", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "US")]
		public void TestAmountForSendToECNForTermForZipCodeNone(int term, string shippingZipCode, Price.PriceType selectedType, string states, string countryCode)
		{
			// Arrange

			Taxjar taxjar = new Taxjar
			{
				Rate = null
			};

			_sourcemediaPubcodeAON.States = states;

			string email = " and emails.emailaddress = '" + "jay@jayfeldmaninc.com" + "' ";
			string connectionString = ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;


			DateTime dateTime = Convert.ToDateTime("05/28/2016");
			DateTime currentDateTime = _subscriberProcess.GetCurrentDateTime(dateTime);


			// Act
			var subscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, term.ToString(), _sourcemediaPubcodeAON, email, countryCode, selectedType, connectionString, currentDateTime, shippingZipCode, false);

			// Assert

			CheckAssertsForZipCode(subscription, shippingZipCode, term, selectedType);
		}

		[Test]
		[TestCase(1, "K1P 1J1", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "CA")]
		[TestCase(2, "K1P 1J1", Price.PriceType.Premium, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "CA")]
		[TestCase(1, "K1P 1J1", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "CA")]
		[TestCase(2, "K1P 1J1", Price.PriceType.Standard, "AZ,CO,CT,DC,NC,NY,OH,SD,TX,WI,001", "CA")]
		public void TestAmountForSendToECNForTermForZipCodeCanada(int term, string shippingZipCode, Price.PriceType selectedType, string states, string countryCode)
		{
			// Arrange

			Taxjar taxjar = new Taxjar
			{
				Rate = new Rate
				{
					CombinedRate = "0.081"
				}
			};

			_sourcemediaPubcodeAON.States = states;

			string email = " and emails.emailaddress = '" + "jay@jayfeldmaninc.com" + "' ";
			string connectionString = ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;


			DateTime dateTime = Convert.ToDateTime("05/28/2016");
			DateTime currentDateTime = _subscriberProcess.GetCurrentDateTime(dateTime);


			// Act
			var subscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, term.ToString(), _sourcemediaPubcodeAON, email, countryCode, selectedType, connectionString, currentDateTime, shippingZipCode, false);

			// Assert

			CheckAssertsForZipCode(subscription, shippingZipCode, term, selectedType);
		}



		#region Private Asserts

		private static void CheckAssertsForZipCode(Subscription subscription, string billingZipCode, int term, Price.PriceType selectedType)
		{
			var expectedPremiumPubCodeDTOTRA = new PremiumPubCodeDTO();
			var expectedPremiumPubCodeDTOABP = new PremiumPubCodeDTO();

			if (term == 1 && selectedType == Price.PriceType.Premium)
			{
				switch (billingZipCode)
				{
					case "55344":
						AssertTerm1PremiumZip55344(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
					case "80001":
						AssertTerm1PremiumZip80001(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
					case "K1P 1J1":
						AssertTerm1PremiumZip80001(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
					case "500009":
					AssertTerm1PremiumZipInternational(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
					break;
					default:
						AssertTerm1PremiumZip55344(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
				}

			}
			else if (term == 2 && selectedType == Price.PriceType.Premium)
			{
				switch (billingZipCode)
				{
					case "55344": AssertTerm2PremiumZip55344(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
					case "80001": AssertTerm2PremiumZip80001(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
					case "K1P 1J1": AssertTerm2PremiumZip80001(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
					case "500009":
						AssertTerm2PremiumZipInternational(subscription, expectedPremiumPubCodeDTOABP, expectedPremiumPubCodeDTOABP);
						break;
					default:
						AssertTerm2PremiumZip55344(subscription, expectedPremiumPubCodeDTOTRA, expectedPremiumPubCodeDTOABP);
						break;
				}

			}
			else if (billingZipCode == "55344" && term == 2 && selectedType == Price.PriceType.Standard)
			{
				switch (billingZipCode)
				{
					case "55344": AssertTerm2StandardZip55344(subscription);
						break;
					case "80001": AssertTerm2StandardZip80001(subscription);
						break;
					case "K1P 1J1": AssertTerm2StandardZip80001(subscription);
						break;
					case "500009":
						AssertTerm2StandardZipInternational(subscription);
						break;
					default:
						AssertTerm2StandardZip55344(subscription);
						break;

				}

			}
			else if (billingZipCode == "55344" && term == 1 && selectedType == Price.PriceType.Standard)
			{
				switch (billingZipCode)
				{
					case "55344": AssertTerm1StandardZip55344(subscription);
						break;

					case "80001": AssertTerm1StandardZip80001(subscription); ;
						break;
					case "K1P 1J1": AssertTerm1StandardZip80001(subscription); ;
						break;

					case "500009":
						AssertTerm1StandardZipInternational(subscription);
						break;

					default:
						AssertTerm1StandardZip55344(subscription);
						break;
				}

			}
		}

		private static void AssertTerm1StandardZipInternational(Subscription subscription)
		{
			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(200.00, subscription.TotalAmount);
			Assert.AreEqual(200.00, subscription.ItemPrice);
			Assert.AreEqual(subscription.PremiumPubCodeDTOs.Count, 0);
		}

		private static void AssertTerm2StandardZipInternational(Subscription subscription)
		{
			Assert.AreEqual(subscription.PremiumPubCodeDTOs.Count, 0);
			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(400.00, subscription.TotalAmount);
			Assert.AreEqual(400.00, subscription.ItemPrice);
		}

		private static void AssertTerm2PremiumZipInternational(Subscription subscription, PremiumPubCodeDTO expectedPremiumPubCodeDTOABP, PremiumPubCodeDTO expectedPremiumPubCodeDTOTRA)
		{
			expectedPremiumPubCodeDTOTRA.PubCode = "TRA";
			expectedPremiumPubCodeDTOTRA.TaxPrice = 0;
			expectedPremiumPubCodeDTOTRA.TotalPrice = 60;
			expectedPremiumPubCodeDTOTRA.ItemPrice = 60;

			expectedPremiumPubCodeDTOABP.PubCode = "ABP";
			expectedPremiumPubCodeDTOABP.TaxPrice = 0;
			expectedPremiumPubCodeDTOABP.TotalPrice = 60;
			expectedPremiumPubCodeDTOABP.ItemPrice = 60;

			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(340.00, subscription.TotalAmount);
			Assert.AreEqual(340.00, subscription.ItemPrice);


			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.ItemPrice, subscription.PremiumPubCodeDTOs[330927].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TaxPrice, subscription.PremiumPubCodeDTOs[330927].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TotalPrice, subscription.PremiumPubCodeDTOs[330927].TotalPrice);

			Assert.AreEqual(expectedPremiumPubCodeDTOABP.ItemPrice, subscription.PremiumPubCodeDTOs[332649].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TaxPrice, subscription.PremiumPubCodeDTOs[332649].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TotalPrice, subscription.PremiumPubCodeDTOs[332649].TotalPrice);
		}

		private static void AssertTerm1PremiumZipInternational(Subscription subscription, PremiumPubCodeDTO expectedPremiumPubCodeDTOABP, PremiumPubCodeDTO expectedPremiumPubCodeDTOTRA)
		{
			expectedPremiumPubCodeDTOTRA.PubCode = "TRA";
			expectedPremiumPubCodeDTOTRA.TaxPrice = 0;
			expectedPremiumPubCodeDTOTRA.TotalPrice = 30;
			expectedPremiumPubCodeDTOTRA.ItemPrice = 30;

			expectedPremiumPubCodeDTOABP.PubCode = "ABP";
			expectedPremiumPubCodeDTOABP.TaxPrice = 0;
			expectedPremiumPubCodeDTOABP.TotalPrice = 30;
			expectedPremiumPubCodeDTOABP.ItemPrice = 30;

			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(170.00, subscription.TotalAmount);
			Assert.AreEqual(170.00, subscription.ItemPrice);


			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.ItemPrice, subscription.PremiumPubCodeDTOs[330927].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TaxPrice, subscription.PremiumPubCodeDTOs[330927].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TotalPrice, subscription.PremiumPubCodeDTOs[330927].TotalPrice);

			Assert.AreEqual(expectedPremiumPubCodeDTOABP.ItemPrice, subscription.PremiumPubCodeDTOs[332649].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TaxPrice, subscription.PremiumPubCodeDTOs[332649].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TotalPrice, subscription.PremiumPubCodeDTOs[332649].TotalPrice);
		}


		private static void AssertTerm1StandardZip55344(Subscription subscription)
		{
			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(200.00, subscription.TotalAmount);
			Assert.AreEqual(200.00, subscription.ItemPrice);
			Assert.AreEqual(subscription.PremiumPubCodeDTOs.Count, 0);
		}

		private static void AssertTerm2StandardZip55344(Subscription subscription)
		{
			Assert.AreEqual(subscription.PremiumPubCodeDTOs.Count, 0);
			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(400.00, subscription.TotalAmount);
			Assert.AreEqual(400.00, subscription.ItemPrice);
		}

		private static void AssertTerm2PremiumZip55344(Subscription subscription, PremiumPubCodeDTO expectedPremiumPubCodeDTOTRA, PremiumPubCodeDTO expectedPremiumPubCodeDTOABP)
		{
			expectedPremiumPubCodeDTOTRA.PubCode = "TRA";
			expectedPremiumPubCodeDTOTRA.TaxPrice = 0;
			expectedPremiumPubCodeDTOTRA.TotalPrice = 60;
			expectedPremiumPubCodeDTOTRA.ItemPrice = 60;

			expectedPremiumPubCodeDTOABP.PubCode = "ABP";
			expectedPremiumPubCodeDTOABP.TaxPrice = 0;
			expectedPremiumPubCodeDTOABP.TotalPrice = 60;
			expectedPremiumPubCodeDTOABP.ItemPrice = 60;

			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(340.00, subscription.TotalAmount);
			Assert.AreEqual(340.00, subscription.ItemPrice);


			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.ItemPrice, subscription.PremiumPubCodeDTOs[330927].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TaxPrice, subscription.PremiumPubCodeDTOs[330927].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TotalPrice, subscription.PremiumPubCodeDTOs[330927].TotalPrice);

			Assert.AreEqual(expectedPremiumPubCodeDTOABP.ItemPrice, subscription.PremiumPubCodeDTOs[332649].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TaxPrice, subscription.PremiumPubCodeDTOs[332649].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TotalPrice, subscription.PremiumPubCodeDTOs[332649].TotalPrice);
		}

		private static void AssertTerm1PremiumZip80001(Subscription subscription, PremiumPubCodeDTO expectedPremiumPubCodeDTOTRA, PremiumPubCodeDTO expectedPremiumPubCodeDTOABP)
		{
			expectedPremiumPubCodeDTOTRA.PubCode = "TRA";
			expectedPremiumPubCodeDTOTRA.TaxPrice = 2.43;
			expectedPremiumPubCodeDTOTRA.TotalPrice = 32.43;
			expectedPremiumPubCodeDTOTRA.ItemPrice = 30;

			expectedPremiumPubCodeDTOABP.PubCode = "ABP";
			expectedPremiumPubCodeDTOABP.TaxPrice = 2.43;
			expectedPremiumPubCodeDTOABP.TotalPrice = 32.43;
			expectedPremiumPubCodeDTOABP.ItemPrice = 30;

			Assert.AreEqual(13.77, subscription.TaxAmount);
			Assert.AreEqual(183.77, subscription.TotalAmount);
			Assert.AreEqual(170.00, subscription.ItemPrice);


			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.ItemPrice, subscription.PremiumPubCodeDTOs[330927].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TaxPrice, subscription.PremiumPubCodeDTOs[330927].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TotalPrice, subscription.PremiumPubCodeDTOs[330927].TotalPrice);

			Assert.AreEqual(expectedPremiumPubCodeDTOABP.ItemPrice, subscription.PremiumPubCodeDTOs[332649].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TaxPrice, subscription.PremiumPubCodeDTOs[332649].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TotalPrice, subscription.PremiumPubCodeDTOs[332649].TotalPrice);
		}

		private static void AssertTerm1PremiumZip55344(Subscription subscription, PremiumPubCodeDTO expectedPremiumPubCodeDTOTRA,
			PremiumPubCodeDTO expectedPremiumPubCodeDTOABP)
		{
			expectedPremiumPubCodeDTOTRA.PubCode = "TRA";
			expectedPremiumPubCodeDTOTRA.TaxPrice = 0;
			expectedPremiumPubCodeDTOTRA.TotalPrice = 30;
			expectedPremiumPubCodeDTOTRA.ItemPrice = 30;

			expectedPremiumPubCodeDTOABP.PubCode = "ABP";
			expectedPremiumPubCodeDTOABP.TaxPrice = 0;
			expectedPremiumPubCodeDTOABP.TotalPrice = 30;
			expectedPremiumPubCodeDTOABP.ItemPrice = 30;

			Assert.AreEqual(0, subscription.TaxAmount);
			Assert.AreEqual(170.00, subscription.TotalAmount);
			Assert.AreEqual(170.00, subscription.ItemPrice);


			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.ItemPrice, subscription.PremiumPubCodeDTOs[330927].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TaxPrice, subscription.PremiumPubCodeDTOs[330927].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TotalPrice, subscription.PremiumPubCodeDTOs[330927].TotalPrice);

			Assert.AreEqual(expectedPremiumPubCodeDTOABP.ItemPrice, subscription.PremiumPubCodeDTOs[332649].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TaxPrice, subscription.PremiumPubCodeDTOs[332649].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TotalPrice, subscription.PremiumPubCodeDTOs[332649].TotalPrice);
		}

		private static void AssertTerm1StandardZip80001(Subscription subscription)
		{
			Assert.AreEqual(subscription.PremiumPubCodeDTOs.Count, 0);
			Assert.AreEqual(16.20, subscription.TaxAmount);
			Assert.AreEqual(216.20, subscription.TotalAmount);
			Assert.AreEqual(200.20, subscription.ItemPrice);
		}

		private static void AssertTerm2StandardZip80001(Subscription subscription)
		{
			Assert.AreEqual(subscription.PremiumPubCodeDTOs.Count, 0);
			Assert.AreEqual(32.40, subscription.TaxAmount);
			Assert.AreEqual(216.20, subscription.TotalAmount);
			Assert.AreEqual(200.20, subscription.ItemPrice);
		}

		private static void AssertTerm2PremiumZip80001(Subscription subscription, PremiumPubCodeDTO expectedPremiumPubCodeDTOTRA,
			PremiumPubCodeDTO expectedPremiumPubCodeDTOABP)
		{
			expectedPremiumPubCodeDTOTRA.PubCode = "TRA";
			expectedPremiumPubCodeDTOTRA.TaxPrice = 4.86;
			expectedPremiumPubCodeDTOTRA.TotalPrice = 64.86;
			expectedPremiumPubCodeDTOTRA.ItemPrice = 60;

			expectedPremiumPubCodeDTOABP.PubCode = "ABP";
			expectedPremiumPubCodeDTOABP.TaxPrice = 4.86;
			expectedPremiumPubCodeDTOABP.TotalPrice = 64.86;
			expectedPremiumPubCodeDTOABP.ItemPrice = 60;

			Assert.AreEqual(27.54, subscription.TaxAmount);
			Assert.AreEqual(367.54, subscription.TotalAmount);
			Assert.AreEqual(340.0, subscription.ItemPrice);


			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.ItemPrice, subscription.PremiumPubCodeDTOs[330927].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TaxPrice, subscription.PremiumPubCodeDTOs[330927].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOTRA.TotalPrice, subscription.PremiumPubCodeDTOs[330927].TotalPrice);

			Assert.AreEqual(expectedPremiumPubCodeDTOABP.ItemPrice, subscription.PremiumPubCodeDTOs[332649].ItemPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TaxPrice, subscription.PremiumPubCodeDTOs[332649].TaxPrice);
			Assert.AreEqual(expectedPremiumPubCodeDTOABP.TotalPrice, subscription.PremiumPubCodeDTOs[332649].TotalPrice);
		}

		#endregion
	}
}
