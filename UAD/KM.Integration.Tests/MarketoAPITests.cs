using System;
using System.Collections.Generic;
using KM.Integration.Marketo;
using KM.Integration.Marketo.Process;
using KM.Integration.OAuth2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KM.Integration.Tests
{
	[TestFixture]
	public class MarketoAPITests
	{
		private MarketoRestAPIProcess _marketoRestApiProcess;
		private Authentication _authentication;

		[SetUp]
		public void Setup()
		{


			string baseUrl = "https://786-NQR-020.mktorest.com";
			string clientID = "69b91e6e-2746-4ff4-9fb0-a7b97b1d510a";
			string clientSecret = "PhVkDZ49RSpvIBKtTABJv5supXU8weYk";


			_authentication = new Authentication(baseUrl, clientID, clientSecret);
			_marketoRestApiProcess = new MarketoRestAPIProcess(baseUrl, clientID, clientSecret);

		}

		[TearDown]
		public void TearDown()
		{

		}

		[Test]
		public void TestGetMarketoLists()
		{
			// Arrange

			Token token = _authentication.getToken();

			// Act

			List<MarketoListResponse> marketoLists = _marketoRestApiProcess.GetMarketoLists(token, null, null);

			// Assert
			Assert.IsNotNull(marketoLists);
			Assert.True(marketoLists.Count > 0);
		}
	}
}
