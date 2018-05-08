using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Binbin.Linq;
using ECN_Framework_BusinessLayer.Communicator;
using KMPS_JF_Objects.Objects;
using Newtonsoft.Json;
using SourceMediaPaidPub.Objects;
using Magazine = SourceMediaPaidPub.Objects.Magazine;
using Price = SourceMediaPaidPub.Objects.Price;

namespace SourceMediaPaidPub.Process
{
	public interface ISubscriberProcess
	{
		DataTable GetCountriesTable();
		string SetCountryCode(string countryCode);
		ECNLastYearAmountsTermDatesPremiumChecks GetLastYearAmountAndTermStartDateAndEndDateFromECN(Publication pub, string filter, string connectionString="");
		Objects.Magazine GetMagazineList(string filePath);
		PriceRange GetPricesFromAmountPaidLastYear(SourcemediaPubcode sourcemediaPubcode, double amountPaidLastYear);
		List<Price> GetPricesFromLastYearForCountry(PriceRange lastYearPriceRange, string countryCode);
		List<string> GetStandardTermsFromPriceForCountries(List<Price> pricesForCountries);
		List<string> GetPremiumTermsFromPriceForCountries(List<Price> pricesForCountries);
		Price GetPriceForCountryTerm(List<Price> pricesForCountries,string countryCode, string term, Price.PriceType priceType);
		string GetByCountryCodeByName(string countryName, string filePath);
		Taxjar GetTaxjarInfo(string apiKey, string zipCode, string countryCode);
		GoogleAPI GetGoogleAPIInfo(string zipCode);
		bool ValidateStatesHasTaxesByZipCode(string stateCode, SourcemediaPubcode magazine, string countryCode);
		bool IsValidUSandCanadianZipCodes(string zipCode);
		IEnumerable<ListItem> AddDefaultStates(string countryName);
		StringBuilder DefaultPostData(PostDataParams postDataParams);
		Shipping CopyAddress(string billingAddress, string billingAddress2, string billingCity, string billingZip, string countryName, string billingState = "");
		CalculatedPremiumPubCodes GetPremiumPubCodeCalculation(double premiumPrice, double standardPrice, double taxPercentage);
		Subscription CalculateTotalTaxAmountTermEndDate(Taxjar taxjar, string term, SourcemediaPubcode sourcemediaPubcode, string email, string countryCode, Price.PriceType selectedType, string connectionString, DateTime curentDateTime, string billingZipCode, bool fromUI);
		List<Country> GetCountries(string filePath);


	}

	public interface IDateTime
	{
		DateTime GetCurrentDateTime(DateTime dateTime);
	}

	public class SubscriberProcess : ISubscriberProcess,IDateTime
	{

		public DataTable GetCountriesTable()
		{
			string sql = "SELECT CountryID, CountryName from Country WHERE CountryID NOT IN (174,205) ORDER BY CountryName ASC";
			DataTable dt = PaidPub.Objects.DataFunctions.GetDataTable(sql);
			return dt;
		}

		public string SetCountryCode(string countryCode)
		{
			if (!String.IsNullOrEmpty(countryCode))
			{
				if (countryCode.ToUpper() == Constants.UNITEDSTATESOFAMERICA || countryCode.ToUpper() == "US" || countryCode.ToUpper() == "USA")
				{
					return Constants.UNITEDSTATESOFAMERICA;
				}
				else if (countryCode.ToUpper() == Constants.CANADA || countryCode.ToUpper() == "CA")
				{
					return Constants.CANADA;
				}
			}
			return String.Empty;
		}

		public ECNLastYearAmountsTermDatesPremiumChecks GetLastYearAmountAndTermStartDateAndEndDateFromECN(Publication pub, string filter, string connectionString)
		{
			ECNLastYearAmountsTermDatesPremiumChecks ecnLastYearAmountsTermDatesPremiumChecks = new ECNLastYearAmountsTermDatesPremiumChecks();
			ecnLastYearAmountsTermDatesPremiumChecks.LastYearAmounts = new List<double>();
			ecnLastYearAmountsTermDatesPremiumChecks.TermEndDates = new List<DateTime>();
			
			var dtSubscriber = getSubscriberDataTable(pub, filter, connectionString);


			if (dtSubscriber == null || dtSubscriber.Rows.Count == 0)
				return ecnLastYearAmountsTermDatesPremiumChecks;

			foreach (DataRow dr in dtSubscriber.Rows)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(dr["t_amountpaid"].ToString()))
					{
						ecnLastYearAmountsTermDatesPremiumChecks.LastYearAmounts.Add(0);
					}
					else
					{
						if (Convert.ToDouble(dr["t_amountpaid"].ToString()) > 0)
						{
							ecnLastYearAmountsTermDatesPremiumChecks.LastYearAmounts.Add(Convert.ToDouble(dr["t_amountpaid"].ToString()));
						}
					}

					if (string.IsNullOrWhiteSpace(dr["MEDIADEF"].ToString()))
					{
						ecnLastYearAmountsTermDatesPremiumChecks.IsPremium = false;
					}
					else
					{
						ecnLastYearAmountsTermDatesPremiumChecks.IsPremium = dr["MEDIADEF"].ToString().ToUpper() == "R";
					}

					if (string.IsNullOrWhiteSpace(dr["t_TermEndDate"].ToString()))
					{
						ecnLastYearAmountsTermDatesPremiumChecks.TermEndDates.Add(DateTime.Now);
					}
					else
					{
						DateTime termEndDate;
						DateTime.TryParse(dr["t_TermEndDate"].ToString(), out termEndDate);

						ecnLastYearAmountsTermDatesPremiumChecks.TermEndDates.Add(termEndDate);
					}
				}
				catch (Exception ex)
				{
					// ignored
				}
			}
			return ecnLastYearAmountsTermDatesPremiumChecks;
		}

		private static DataTable getSubscriberDataTable(Publication pub, string filter, string connectionString)
		{
			EmailGroupCommunicator emailGroupCommunicator = new EmailGroupCommunicator();
			DataTable dtSubscriber = emailGroupCommunicator.GetGroupEmailProfilesWithUDF(pub.ECNDefaultGroupID, pub.ECNCustomerID,
				filter, "'S','P','U'", connectionString);
			return dtSubscriber;
		}

		public Objects.Magazine GetMagazineList(string filePath)
		{
			Objects.Magazine magazine = new Objects.Magazine();

			try
			{
				using (StreamReader file = File.OpenText(filePath))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.Converters.Add(new BooleanJsonConverter());
					magazine = (Objects.Magazine)serializer.Deserialize(file, typeof(Magazine));
				}
			}
			catch (Exception ex)
			{

			}
			return magazine;
		}

		public PriceRange GetPricesFromAmountPaidLastYear(SourcemediaPubcode sourcemediaPubcode, double amountPaidLastYear)
		{
			var predicate = PredicateBuilder.False<PriceRange>();
			predicate = predicate.Or(p => p.PaidFrom <= amountPaidLastYear && p.PaidTo >= amountPaidLastYear);
			var priceRanges = sourcemediaPubcode.PriceRanges;
			var prices = priceRanges.AsQueryable().Where(predicate);

			if (!prices.Any())
			{
				return priceRanges[0];
			}
			return prices.ToList()[0];
		}

		public List<Price> GetPricesFromLastYearForCountry(PriceRange lastYearPriceRange, string countryCode)
		{
			List<Price> prices = new List<Price>();

			foreach (var price in lastYearPriceRange.PriceCollection)
				{
					if (price.Country == countryCode)
					{
						prices.Add(price);
					}
				}
			
			return prices;
		}

		public List<string> GetStandardTermsFromPriceForCountries(List<Price> pricesForCountries)
		{
			List<string> standardTerms = new List<string>();
			foreach (var price in pricesForCountries)
			{
				if (price.Type == Price.PriceType.Standard)
				{
					standardTerms.Add(price.Term);
				}
			}
			return standardTerms.OrderBy(x=>x).ToList();
		}

		public List<string> GetPremiumTermsFromPriceForCountries(List<Price> pricesForCountries)
		{

			List<string> premiumTerms = new List<string>();
			foreach (var price in pricesForCountries)
			{
				if (price.Type == Price.PriceType.Premium)
				{
					premiumTerms.Add(price.Term);
				}
			}
			return premiumTerms.OrderBy(x=>x).ToList();
		}

		public Price GetPriceForCountryTerm(List<Price> pricesForCountries,string countryCode, string term, Price.PriceType priceType)
		{
			foreach (var price in pricesForCountries)
			{
				if (price.Country == countryCode && price.Type == priceType && price.Term == term)
				{
					return price;
				}
			}
			return null;
		}

		public string GetByCountryCodeByName(string countryName, string filePath)
		{
			using (var syncClient = new WebClient())
			{
				try
				{
					var content = syncClient.DownloadString(filePath);
					var countries = JsonConvert.DeserializeObject<List<Country>>(content);

					var country = countries.SingleOrDefault(x => x.Name.ToUpper().Equals(countryName.ToUpper()));
					if (country != null)
					{
						var countryCode = getCountryCodeByCountry(country);
						return countryCode;
					}
				}
				catch (Exception ex)
				{
					
				}	

			}
			
			return String.Empty;
			
		}

		public Taxjar GetTaxjarInfo(string apiKey, string zipCode, string countryCode)
		{
			Taxjar taxjar = null;
			try
			{
				using (var client = new HttpClient())
				{
					var apiAddress = getApiAddress(zipCode, countryCode);

					var request = new HttpRequestMessage
					{
						RequestUri = new Uri(apiAddress),
						Method = HttpMethod.Get
					};

					request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);


					var task = client.SendAsync(request).ContinueWith((taskwithResponse) =>
					{
						try
						{
							var response = taskwithResponse.Result;
							var jsonString = response.Content.ReadAsStringAsync();
							jsonString.Wait();
							taxjar = JsonConvert.DeserializeObject<Taxjar>(jsonString.Result);
						}
						catch (Exception ex)
						{
							throw ex;
						}
						
					});

					task.Wait();
				}
				return taxjar;

			}
			catch (Exception ex)
			{
				
			}
			return taxjar;
		}


		public GoogleAPI GetGoogleAPIInfo(string zipCode)
		{
			GoogleAPI googleApi = null;
			try
			{
				using (var client = new HttpClient())
				{
					var apiAddress = getGoogleApiAddress(zipCode);

					var request = new HttpRequestMessage
					{
						RequestUri = new Uri(apiAddress),
						Method = HttpMethod.Get
					};

					request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));



					var task = client.SendAsync(request).ContinueWith((taskwithResponse) =>
					{
						try
						{
							var response = taskwithResponse.Result;
							var jsonString = response.Content.ReadAsStringAsync();
							jsonString.Wait();
							googleApi = JsonConvert.DeserializeObject<GoogleAPI>(jsonString.Result);
						}
						catch (Exception ex)
						{
							throw ex;
						}

					});

					task.Wait();
				}
				return googleApi;

			}
			catch (Exception ex)
			{

			}
			return googleApi;
		}


	


		private string getApiAddress(string zipCode, string countryCode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder = stringBuilder.Append(Constants.TaxjarURL);
			stringBuilder = stringBuilder.AppendFormat("{0}",zipCode);
			stringBuilder = stringBuilder.AppendFormat("?country={0}",countryCode);
			return stringBuilder.ToString();

		}

		private string getGoogleApiAddress(string zipCode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder = stringBuilder.Append("https://maps.googleapis.com/maps/api/geocode/json");
			stringBuilder = stringBuilder.AppendFormat("?address={0}&sensor=true", zipCode);
			return stringBuilder.ToString();

		}

		private  string getCountryCodeByCountry(Country country)
		{
			if (country.Name.ToUpper().Equals(Constants.UNITEDSTATESOFAMERICA) || country.Name.ToUpper().Equals(Constants.CANADA))
			{
				return country.ISOCode;
			}
			return "INT";
		}
		public bool ValidateStatesHasTaxesByZipCode(string stateCode, SourcemediaPubcode magazine, string countryCode)
		{
			var states = magazine.States.Split(',').Select(x => x).ToList();

			return states.Contains(stateCode) || countryCode == "CA" || countryCode == "174";

		}

		public bool IsValidUSandCanadianZipCodes(string zipCode)
		{
			bool validZipCode = !((!Regex.Match(zipCode, Constants.USCodePattern).Success) && (!Regex.Match(zipCode, Constants.CAZipCodePattern).Success));
			return validZipCode;

		}

		public IEnumerable<ListItem> AddDefaultStates(string countryName)
		{
			var items = new List<ListItem>();
			if (countryName.ToUpper() == "UNITED STATES OF AMERICA")
			{
				items.Add(addlistitem("", "Select a State", ""));
				items.Add(addlistitem("AK", "Alaska", "USA"));
				items.Add(addlistitem("AL", "Alabama", "USA"));
				items.Add(addlistitem("AR", "Arkansas", "USA"));
				items.Add(addlistitem("AZ", "Arizona", "USA"));
				items.Add(addlistitem("CA", "California", "USA"));
				items.Add(addlistitem("CO", "Colorado", "USA"));
				items.Add(addlistitem("CT", "Connecticut", "USA"));
				items.Add(addlistitem("DC", "Washington D.C.", "USA"));
				items.Add(addlistitem("DE", "Delaware", "USA"));
				items.Add(addlistitem("FL", "Florida", "USA"));
				items.Add(addlistitem("GA", "Georgia", "USA"));
				items.Add(addlistitem("HI", "Hawaii", "USA"));
				items.Add(addlistitem("IA", "Iowa", "USA"));
				items.Add(addlistitem("ID", "Idaho", "USA"));
				items.Add(addlistitem("IL", "Illinois", "USA"));
				items.Add(addlistitem("IN", "Indiana", "USA"));
				items.Add(addlistitem("KS", "Kansas", "USA"));
				items.Add(addlistitem("KY", "Kentucky", "USA"));
				items.Add(addlistitem("LA", "Louisiana", "USA"));
				items.Add(addlistitem("MA", "Massachusetts", "USA"));
				items.Add(addlistitem("MD", "Maryland", "USA"));
				items.Add(addlistitem("ME", "Maine", "USA"));
				items.Add(addlistitem("MI", "Michigan", "USA"));
				items.Add(addlistitem("MN", "Minnesota", "USA"));
				items.Add(addlistitem("MO", "Missouri", "USA"));
				items.Add(addlistitem("MS", "Mississippi", "USA"));
				items.Add(addlistitem("MT", "Montana", "USA"));
				items.Add(addlistitem("NC", "North Carolina", "USA"));
				items.Add(addlistitem("ND", "North Dakota", "USA"));
				items.Add(addlistitem("NE", "Nebraska", "USA"));
				items.Add(addlistitem("NH", "New Hampshire", "USA"));
				items.Add(addlistitem("NJ", "New Jersey", "USA"));
				items.Add(addlistitem("NM", "New Mexico", "USA"));
				items.Add(addlistitem("NV", "Nevada", "USA"));
				items.Add(addlistitem("NY", "New York", "USA"));
				items.Add(addlistitem("OH", "Ohio", "USA"));
				items.Add(addlistitem("OK", "Oklahoma", "USA"));
				items.Add(addlistitem("OR", "Oregon", "USA"));
				items.Add(addlistitem("PA", "Pennsylvania", "USA"));
				items.Add(addlistitem("PR", "Puerto Rico", "USA"));
				items.Add(addlistitem("RI", "Rhode Island", "USA"));
				items.Add(addlistitem("SC", "South Carolina", "USA"));
				items.Add(addlistitem("SD", "South Dakota", "USA"));
				items.Add(addlistitem("TN", "Tennessee", "USA"));
				items.Add(addlistitem("TX", "Texas", "USA"));
				items.Add(addlistitem("UT", "Utah", "USA"));
				items.Add(addlistitem("VA", "Virginia", "USA"));
				items.Add(addlistitem("VT", "Vermont", "USA"));
				items.Add(addlistitem("WA", "Washington", "USA"));
				items.Add(addlistitem("WI", "Wisconsin", "USA"));
				items.Add(addlistitem("WV", "West Virginia", "USA"));
				items.Add(addlistitem("WY", "Wyoming", "USA"));
			}
			else if (countryName.ToUpper() == "CANADA")
			{
				items.Add(addlistitem("", "Select a Province", ""));
				items.Add(addlistitem("AB", "Alberta", "Canada"));
				items.Add(addlistitem("BC", "British Columbia", "Canada"));
				items.Add(addlistitem("MB", "Manitoba", "Canada"));
				items.Add(addlistitem("NB", "New Brunswick", "Canada"));
				items.Add(addlistitem("NF", "New Foundland", "Canada"));
				items.Add(addlistitem("NS", "Nova Scotia", "Canada"));
				items.Add(addlistitem("ON", "Ontario", "Canada"));
				items.Add(addlistitem("PE", "Prince Edward Island", "Canada"));
				items.Add(addlistitem("QC", "Quebec", "Canada"));
				items.Add(addlistitem("SK", "Saskatchewan", "Canada"));
				items.Add(addlistitem("YT", "Yukon Territories", "Canada"));
				//addlistitem("OT", "Other", "Foreign");
			}
			return items;
		}

		private ListItem addlistitem(string value, string text, string group)
		{
			ListItem item = new ListItem(text, value);

			if (group != string.Empty)
				item.Attributes["OptionGroup"] = group;

			return item;
		}

		public StringBuilder DefaultPostData(PostDataParams postDataParams)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("g=" + postDataParams.GroupId);
			stringBuilder.Append("&user_t_Term=" + postDataParams.Term);

			stringBuilder.Append("&e=" + postDataParams.Email);
			stringBuilder.Append("&f=html");
			stringBuilder.Append("&c=" + postDataParams.CustomerId.ToString());
			stringBuilder.Append("&user_PaymentStatus=paid");
			stringBuilder.Append("&user_PAIDorFREE=PAID");
			if (postDataParams.CardHolderName.Trim().Contains(" "))
			{
				string fullName = postDataParams.CardHolderName;
				string firstName = fullName.Split(new char[] {' '})[0];
				string lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);
				stringBuilder.Append("&user_t_FirstName=" + firstName);
				stringBuilder.Append("&user_t_LastName=" + lastName);
			}
			else
			{
				stringBuilder.Append("&user_t_FirstName=" + postDataParams.CardHolderName);
				stringBuilder.Append("&user_t_LastName=" + "");
			}

			stringBuilder.Append("&user_t_CardType=" + postDataParams.CardTypeValue);
			stringBuilder.Append("&user_t_CardNumber=" + "************" + postDataParams.CreditCardNumber.Substring(postDataParams.CreditCardNumber.Trim().Length - 4, 4));
			stringBuilder.Append(String.Format("&user_t_ExpirationDate={0}/{1}", postDataParams.UserExpirationMonth, postDataParams.UserExpirationYear));
			stringBuilder.Append("&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy"));

			
				stringBuilder.Append("&user_t_AmountPaid=" + String.Format("{0:0.00}", postDataParams.TotalAmount));
				stringBuilder.Append("&user_t_TaxPaid=" + String.Format("{0:0.00}", postDataParams.TaxAmount));
			
		
			stringBuilder.Append("&user_t_Street=" + postDataParams.BillingAddress);
			stringBuilder.Append("&user_t_Street2=" + postDataParams.BillingAddress2);
			stringBuilder.Append("&user_t_City=" + postDataParams.BillingCity);

			stringBuilder.Append("&user_t_Country=" + postDataParams.CountryCode);

			if (postDataParams.CountryCode.ToUpper() == "UNITED STATES OF AMERICA" || postDataParams.CountryCode.ToUpper() == "CANADA")
			{
				stringBuilder.Append("&user_t_State=" + postDataParams.SelectedBillingState);
				stringBuilder.Append("&user_SHIPTO_STATE=" + postDataParams.ShippingState);
				stringBuilder.Append("&user_SHIPTO_ZIP=" + postDataParams.ShippingZip);
				stringBuilder.Append("&user_t_Zip=" + postDataParams.BillingZip);
			}
			else
			{
				stringBuilder.Append("&user_t_State=" + "INT");
				stringBuilder.Append("&user_SHIPTO_STATE_INT=" + "INT");
				stringBuilder.Append("&user_SHIPTO_ZIP=" + "INT");
				stringBuilder.Append("&user_t_Zip=" + "INT");
			}
			
			
			stringBuilder.Append("&user_t_TransactionID=" + postDataParams.TransactionID);
			stringBuilder.Append("&user_t_Renewal=" + postDataParams.IsSubscriptionRenewal);

			stringBuilder.Append("&user_SHIPTO_ADDRESS1=" + postDataParams.ShippingAddress);
			stringBuilder.Append("&user_SHIPTO_ADDRESS2=" + postDataParams.ShippingAddress2);
			stringBuilder.Append("&user_SHIPTO_CITY=" + postDataParams.ShippingCity);

			stringBuilder.Append("&user_t_TermStartDate=" + postDataParams.TermStartDate.Date.ToString("MM/dd/yyyy"));
			stringBuilder.Append("&user_t_TermEndDate=" + postDataParams.TermEndDate.Date.ToString("MM/dd/yyyy"));
			stringBuilder.Append("&user_t_itemPrice=" + postDataParams.ItemPrice);
			stringBuilder.Append("&user_t_ispremium=" + postDataParams.IsPremium);
			return stringBuilder;
		}

		public Shipping CopyAddress(string billingAddress, string billingAddress2, string billingCity, string billingZip, string countryName, string billingState = "")
		{
			Shipping shipping = new Shipping();
			shipping.ShippingAddress = billingAddress;
			shipping.ShippingAddress2 = billingAddress2;
			shipping.ShippingCity = billingCity;
			shipping.ShippingZip = billingZip;
			if (!String.IsNullOrWhiteSpace(billingState))
			{
				shipping.ShippingState = billingState;
			}
			return shipping;

		}

		public CalculatedPremiumPubCodes GetPremiumPubCodeCalculation(double premiumPrice, double standardPrice, double taxPercentage)
		{
			var premiumPubCodesPrice = premiumPrice - standardPrice;
			var premiumPubCodeTaxprice = premiumPubCodesPrice*taxPercentage;
			var totalPremiumPubCodePrice = premiumPubCodesPrice + premiumPubCodeTaxprice;
			return new CalculatedPremiumPubCodes
			{
				PremiumPubCodePrice = premiumPubCodesPrice,
				PremiumPubCodeTaxPrice = premiumPubCodeTaxprice,
				TotalPremiumPubCodePrice = totalPremiumPubCodePrice,
			};
		}
		public Subscription CalculateTotalTaxAmountTermEndDate(Taxjar taxjar, string term, SourcemediaPubcode sourcemediaPubcode, string email, string countryCode, Price.PriceType selectedType, string connectionString, DateTime currentDateTime, string billingZipCode, bool fromUI)
		{
			// Get Term Start date and end Dates

			var googleApi = GetGoogleAPIInfo(billingZipCode);
			AddressComponent addressComponent = null;
			foreach (var zipState in googleApi.ZipStates)
			{
				foreach (var adc in zipState.AddressComponents)
				{
					if (adc.types.Contains("administrative_area_level_1") && adc.types.Contains("political"))
					{
						addressComponent = adc;
					}
				}
			}

			var stateCode = String.Empty;

			double combinationRate = 0;

			if (addressComponent != null)
			{
				stateCode = addressComponent.ShortName;
			}

			var isTaxable = ValidateStatesHasTaxesByZipCode(stateCode, sourcemediaPubcode, countryCode);

			Subscription subscription = new Subscription();
			subscription.TermStartDate = currentDateTime;
			subscription.TermEndDate = currentDateTime.AddYears(Convert.ToInt32(term));

			subscription.PubCode = sourcemediaPubcode.PubCode;
			subscription.PremiumPubCodeDTOs = new Dictionary<int,PremiumPubCodeDTO>();

			Publication pub = new Publication
			{
				ECNDefaultGroupID = Convert.ToInt32(sourcemediaPubcode.GroupID),
				ECNCustomerID = Convert.ToInt32(sourcemediaPubcode.CustomerID)
			};

			var ecnLastYearAmountsAndTermDates = GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, email, connectionString);

			var amountPaidLastYear = ecnLastYearAmountsAndTermDates.LastYearAmounts.Count > 0 ? ecnLastYearAmountsAndTermDates.LastYearAmounts.Max() : 0.0;

			subscription.CurrentSubscriptionEndDate = ecnLastYearAmountsAndTermDates.TermEndDates.Count > 0 ? ecnLastYearAmountsAndTermDates.TermEndDates.Max(): DateTime.Now;

			if (subscription.TermStartDate <= subscription.CurrentSubscriptionEndDate)
			{
				subscription.TermStartDate = subscription.CurrentSubscriptionEndDate.AddDays(1);
				subscription.TermEndDate = subscription.TermStartDate.AddYears(Convert.ToInt32(term));
			}

			var priceRange = GetPricesFromAmountPaidLastYear(sourcemediaPubcode, amountPaidLastYear);

			var price = GetPriceForCountryTerm(priceRange.PriceCollection.ToList(), countryCode, term, selectedType);

			if (price == null)
			{
				return subscription;
			}

			subscription.ItemPrice = price.Value;

			if (isTaxable)
			{
				if (taxjar == null)
				{
					return subscription;
				}
				
				if (taxjar.Rate == null)
				{
					combinationRate = 0;
				}
				else
				{
					combinationRate = Convert.ToDouble(taxjar.Rate.CombinedRate);
				}
				subscription.TaxAmount = price.Value*combinationRate;
				subscription.TotalAmount = price.Value + subscription.TaxAmount;}
			else
			{
				subscription.TaxAmount = 0;
				subscription.TotalAmount = price.Value;
			}

			if (sourcemediaPubcode.PremiumPubCodes == null)
			{
				return subscription;
			}

			if (sourcemediaPubcode.PremiumPubCodes.Any() && selectedType == Price.PriceType.Premium && !fromUI)
			{
				var premiumPubCodePrices = sourcemediaPubcode.PremiumPubCodes.Where(x=>x.Term == term).Select(x => x.Price).Sum();
				var actualPrice = price.Value - (double) premiumPubCodePrices;

				if (isTaxable)
				{
					subscription.TaxAmount = actualPrice * combinationRate;
					subscription.TotalAmount = actualPrice + subscription.TaxAmount;
					subscription.ItemPrice = actualPrice;
				}
				else
				{
					subscription.TaxAmount = 0;
					subscription.TotalAmount = actualPrice;
					subscription.ItemPrice = actualPrice;
				}

				var premiumPubCodes = sourcemediaPubcode.PremiumPubCodes.Where(x =>x.Term.Equals(term));

				foreach (var premiumPubCode in premiumPubCodes)
				{
					//Publication publication = Publication.GetPublicationbyID(0, premiumPubCode.PubCode);
				
					PremiumPubCodeDTO premiumPubCodeDto = new PremiumPubCodeDTO
					{
						TaxPrice = isTaxable?  premiumPubCode.Price * Convert.ToDouble(combinationRate) : 0,
						TotalPrice = isTaxable ? premiumPubCode.Price * Convert.ToDouble(combinationRate) + premiumPubCode.Price : premiumPubCode.Price,
						PubCode = premiumPubCode.PubCode,
						ItemPrice = premiumPubCode.Price,
                        CustomerId = int.Parse(premiumPubCode.CustomerID)

					};
                    subscription.PremiumPubCodeDTOs.Add(int.Parse(premiumPubCode.GroupID), premiumPubCodeDto);
				}
			}
			

			
			return subscription;
		}
		

		public List<Country> GetCountries(string filePath)
		{
			using (var syncClient = new WebClient())
			{
				try
				{
					var content = syncClient.DownloadString(filePath);
					var countries = JsonConvert.DeserializeObject<List<Country>>(content);
					return countries;
				}
				catch (Exception ex)
				{

				}

			}
			return new List<Country>();
		}

		public DateTime GetCurrentDateTime(DateTime dateTime)
		{
			return dateTime;
		}
	}

	public class EmailGroupCommunicator
	{
		public DataTable GetGroupEmailProfilesWithUDF(int groupID, int customerID, string filter, string subscribeType, string connectionString, string profFilter = "")
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "sp_GetGroupEmailProfilesWithUDF";

			cmd.Parameters.Add("@GroupID", SqlDbType.Int);
			cmd.Parameters["@GroupID"].Value = groupID;

			cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
			cmd.Parameters["@CustomerID"].Value = customerID;

			cmd.Parameters.Add("@Filter", SqlDbType.VarChar);
			cmd.Parameters["@Filter"].Value = filter;

			if (profFilter.Length > 0)
			{
				cmd.Parameters.Add("@ProfileFilter", SqlDbType.VarChar);
				cmd.Parameters["@ProfileFilter"].Value = profFilter;
			}

			cmd.Parameters.Add("@SubscribeType", SqlDbType.VarChar);
			cmd.Parameters["@SubscribeType"].Value = subscribeType;


			return GetDataTable(connectionString,cmd);
		}


		public static DataTable GetDataTable(string db, SqlCommand cmd)
		{
			SqlConnection conn = new SqlConnection(db);
			cmd.CommandTimeout = 0;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			conn.Open();
			cmd.Connection = conn;
			da.Fill(ds, "spresult");
			conn.Close();
			DataTable dt = ds.Tables[0];
			return dt;
		}



	}

	public class CalculatedPremiumPubCodes
	{
		public double PremiumPubCodePrice { get; set; }
		public double TotalPremiumPubCodePrice { get; set; }

		public double PremiumPubCodeTaxPrice { get; set; }
	}

	public class PremiumPubCodeDTO
	{
		public string PubCode { get; set; }
		public double TaxPrice { get; set; }
		public double TotalPrice { get; set; }
		public double ItemPrice { get; set; }
		public int CustomerId { get; set; }
	}
}