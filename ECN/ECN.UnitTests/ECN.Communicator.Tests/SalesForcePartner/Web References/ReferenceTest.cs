using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using Microsoft.QualityTools.Testing.Fakes;
using System.Web.Services.Protocols.Fakes;
using System.Xml;
using ecn.communicator.SalesForcePartner;

namespace ecn.communicator.SalesForcePartner
{
    [TestFixture]
    public class SforceServiceTest
    {
        #region Category : General

        #region Category : WebReferenceInvokeTest

        #region WebService Invoke Test : SforceService => login (Return type :  LoginResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_login_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var username = fixture.Create<string>();
        	var password = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new LoginResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.login(username, password);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => loginAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_loginAsync_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var username = fixture.Create<string>();
        	var password = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.loginAsync(username, password, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeSObject (Return type :  DescribeSObjectResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeSObject_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeSObjectResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeSObject(sObjectType);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeSObjectAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeSObjectAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeSObjectAsync(sObjectType, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeSObjects (Return type :  DescribeSObjectResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeSObjects_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeSObjectResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeSObjects(sObjectType);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeSObjectsAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeSObjectsAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeSObjectsAsync(sObjectType, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeGlobal (Return type :  DescribeGlobalResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeGlobal_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeGlobalResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeGlobal();
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeGlobalAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeGlobalAsync_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeGlobalAsync(userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeDataCategoryGroups (Return type :  DescribeDataCategoryGroupResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeDataCategoryGroups_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeDataCategoryGroupResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeDataCategoryGroups(sObjectType);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeDataCategoryGroupsAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeDataCategoryGroupsAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeDataCategoryGroupsAsync(sObjectType, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeDataCategoryGroupStructures (Return type :  DescribeDataCategoryGroupStructureResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeDataCategoryGroupStructures_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var pairs = fixture.CreateMany<DataCategoryGroupSobjectTypePair>().ToArray();
        	var topCategoriesOnly = fixture.Create<bool>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeDataCategoryGroupStructureResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeDataCategoryGroupStructures(pairs, topCategoriesOnly);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeDataCategoryGroupStructuresAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeDataCategoryGroupStructuresAsync_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var pairs = fixture.CreateMany<DataCategoryGroupSobjectTypePair>().ToArray();
        	var topCategoriesOnly = fixture.Create<bool>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeDataCategoryGroupStructuresAsync(pairs, topCategoriesOnly, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeLayout (Return type :  DescribeLayoutResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeLayout_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var recordTypeIds = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeLayoutResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeLayout(sObjectType, recordTypeIds);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeLayoutAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeLayoutAsync_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var recordTypeIds = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeLayoutAsync(sObjectType, recordTypeIds, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeSoftphoneLayout (Return type :  DescribeSoftphoneLayoutResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeSoftphoneLayout_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeSoftphoneLayoutResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeSoftphoneLayout();
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeSoftphoneLayoutAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeSoftphoneLayoutAsync_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeSoftphoneLayoutAsync(userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeTabs (Return type :  DescribeTabSetResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeTabs_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DescribeTabSetResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeTabs();
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => describeTabsAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_describeTabsAsync_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.describeTabsAsync(userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => create (Return type :  SaveResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_create_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjects = fixture.CreateMany<sObject>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new SaveResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.create(sObjects);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => createAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_createAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjects = fixture.CreateMany<sObject>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.createAsync(sObjects, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => update (Return type :  SaveResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_update_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjects = fixture.CreateMany<sObject>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new SaveResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.update(sObjects);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => updateAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_updateAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjects = fixture.CreateMany<sObject>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.updateAsync(sObjects, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => upsert (Return type :  UpsertResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_upsert_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var externalIDFieldName = fixture.Create<string>();
        	var sObjects = fixture.CreateMany<sObject>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new UpsertResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.upsert(externalIDFieldName, sObjects);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => upsertAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_upsertAsync_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var externalIDFieldName = fixture.Create<string>();
        	var sObjects = fixture.CreateMany<sObject>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.upsertAsync(externalIDFieldName, sObjects, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => merge (Return type :  MergeResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_merge_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var request = fixture.CreateMany<MergeRequest>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new MergeResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.merge(request);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => mergeAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_mergeAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var request = fixture.CreateMany<MergeRequest>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.mergeAsync(request, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => delete (Return type :  DeleteResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_delete_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var ids = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new DeleteResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.delete(ids);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => deleteAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_deleteAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var ids = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.deleteAsync(ids, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => undelete (Return type :  UndeleteResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_undelete_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var ids = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new UndeleteResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.undelete(ids);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => undeleteAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_undeleteAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var ids = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.undeleteAsync(ids, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => emptyRecycleBin (Return type :  EmptyRecycleBinResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_emptyRecycleBin_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var ids = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new EmptyRecycleBinResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.emptyRecycleBin(ids);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => emptyRecycleBinAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_emptyRecycleBinAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var ids = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.emptyRecycleBinAsync(ids, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => retrieve (Return type :  sObject[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_retrieve_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var fieldList = fixture.Create<string>();
        	var sObjectType = fixture.Create<string>();
        	var ids = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new sObject[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.retrieve(fieldList, sObjectType, ids);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => retrieveAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_retrieveAsync_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var fieldList = fixture.Create<string>();
        	var sObjectType = fixture.Create<string>();
        	var ids = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.retrieveAsync(fieldList, sObjectType, ids, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => process (Return type :  ProcessResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_process_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var actions = fixture.CreateMany<ProcessRequest>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new ProcessResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.process(actions);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => processAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_processAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var actions = fixture.CreateMany<ProcessRequest>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.processAsync(actions, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => convertLead (Return type :  LeadConvertResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_convertLead_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var leadConverts = fixture.CreateMany<LeadConvert>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new LeadConvertResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.convertLead(leadConverts);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => convertLeadAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_convertLeadAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var leadConverts = fixture.CreateMany<LeadConvert>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.convertLeadAsync(leadConverts, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => logout (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_logout_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.logout();
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => logoutAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_logoutAsync_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.logoutAsync(userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => invalidateSessions (Return type :  InvalidateSessionsResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_invalidateSessions_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sessionIds = fixture.CreateMany<string>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new InvalidateSessionsResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.invalidateSessions(sessionIds);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => invalidateSessionsAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_invalidateSessionsAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sessionIds = fixture.CreateMany<string>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.invalidateSessionsAsync(sessionIds, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getDeleted (Return type :  GetDeletedResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getDeleted_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var startDate = fixture.Create<System.DateTime>();
        	var endDate = fixture.Create<System.DateTime>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new GetDeletedResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getDeleted(sObjectType, startDate, endDate);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getDeletedAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getDeletedAsync_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var startDate = fixture.Create<System.DateTime>();
        	var endDate = fixture.Create<System.DateTime>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getDeletedAsync(sObjectType, startDate, endDate, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getUpdated (Return type :  GetUpdatedResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getUpdated_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var startDate = fixture.Create<System.DateTime>();
        	var endDate = fixture.Create<System.DateTime>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new GetUpdatedResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getUpdated(sObjectType, startDate, endDate);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getUpdatedAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getUpdatedAsync_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var sObjectType = fixture.Create<string>();
        	var startDate = fixture.Create<System.DateTime>();
        	var endDate = fixture.Create<System.DateTime>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getUpdatedAsync(sObjectType, startDate, endDate, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => query (Return type :  QueryResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_query_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var queryString = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new QueryResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.query(queryString);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => queryAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_queryAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var queryString = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.queryAsync(queryString, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => queryAll (Return type :  QueryResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_queryAll_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var queryString = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new QueryResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.queryAll(queryString);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => queryAllAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_queryAllAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var queryString = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.queryAllAsync(queryString, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => queryMore (Return type :  QueryResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_queryMore_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var queryLocator = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new QueryResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.queryMore(queryLocator);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => queryMoreAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_queryMoreAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var queryLocator = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.queryMoreAsync(queryLocator, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => search (Return type :  SearchResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_search_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var searchString = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new SearchResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.search(searchString);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => searchAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_searchAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var searchString = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.searchAsync(searchString, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getServerTimestamp (Return type :  GetServerTimestampResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getServerTimestamp_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new GetServerTimestampResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getServerTimestamp();
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getServerTimestampAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getServerTimestampAsync_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getServerTimestampAsync(userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => setPassword (Return type :  SetPasswordResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_setPassword_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userId = fixture.Create<string>();
        	var password = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new SetPasswordResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.setPassword(userId, password);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => setPasswordAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_setPasswordAsync_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userId = fixture.Create<string>();
        	var password = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.setPasswordAsync(userId, password, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => resetPassword (Return type :  ResetPasswordResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_resetPassword_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userId = fixture.Create<string>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new ResetPasswordResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.resetPassword(userId);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => resetPasswordAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_resetPasswordAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userId = fixture.Create<string>();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.resetPasswordAsync(userId, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getUserInfo (Return type :  GetUserInfoResult)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getUserInfo_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new GetUserInfoResult()};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getUserInfo();
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => getUserInfoAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_getUserInfoAsync_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.getUserInfoAsync(userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => sendEmail (Return type :  SendEmailResult[])

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_sendEmail_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var messages = fixture.CreateMany<Email>().ToArray();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return new Object[] {new SendEmailResult[] {null}};
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.sendEmail(messages);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => sendEmailAsync (Return type :  void)

        [Test]
        [Author("AUT. Fred Merlo")]
        [Category("AUT WebReferenceInvokeTest")]
        public void SforceService_sendEmailAsync_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var fixture = new Fixture();
        	fixture.Register(() => new XmlDocument().DocumentElement);
        	var messages = fixture.CreateMany<Email>().ToArray();
        	var userState = fixture.Create<object>();

        	using(ShimsContext.Create())
        	{
        		bool invoked = false;
        		var sforceService  = new SforceService();

        		ShimSoapHttpClientProtocol.AllInstances.InvokeStringObjectArray = (service, methodName, parameters) => 
        		{
        			invoked = true;
        			return null;
        		};
        		ShimSoapHttpClientProtocol.AllInstances.InvokeAsyncStringObjectArraySendOrPostCallbackObject = (service, methodName, parameters, callback, state) => 
        		{
        			invoked = true;
        		};

        		// Act, Assert
        		sforceService.sendEmailAsync(messages, userState);
        		Assert.True(invoked);
        	}
        }

        #endregion

        #region WebService Invoke Test : SforceService => CancelAsync (Return type :  void)

        #endregion

        #endregion

        #endregion
    }
}