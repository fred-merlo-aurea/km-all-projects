using System.Collections.Generic;
using Shouldly;
using UAS.Web.Models.Circulations;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UAS.Web.Tests.Models.Circulations
{
    [TestFixture]
    public partial class SubscriptionManagerTest
    {
        private const string TestedMethodName_BindValues = "BindValues";
        private const string PrivateField_myProductSubscription = "myProductSubscription";

        [Test]
        public void BindValues_NewSubscriber_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = true;
            _prodSubscription.IsLocked = true;
            _prodSubscription.CountryID = CountryIDUS;
            _prodSubscription.ZipCode = ContryUSFullZip;
            _prodSubscription.Plus4 = ContryUSFullZip;
            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            AddCountries();

            //Act
            Should.NotThrow(() => _smPrivateObject.Invoke(TestedMethodName_BindValues));
            var myProductSubscription = _smPrivateObject.GetFieldOrProperty(PrivateField_myProductSubscription) as ProductSubscription;

            //Assert   
            myProductSubscription.ShouldSatisfyAllConditions(()=> myProductSubscription.FullZip.ShouldNotBeNullOrEmpty(),
                                                            ()=> myProductSubscription.PhoneCode.ShouldNotBeSameAs(_prodSubscription.PhoneCode));
        }

        [Test]
        public void BindValues_ForCananda_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = true;            
            _prodSubscription.CountryID = CountryIDCanada;
            _prodSubscription.ZipCode = ContryUSFullZip;
            _prodSubscription.Plus4 = ContryUSFullZip;
            _prodSubscription.IsLocked = false;
            _product.AllowDataEntry = true;
            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            AddCountries();

            //Act
            Should.NotThrow(() => _smPrivateObject.Invoke(TestedMethodName_BindValues));
            var myProductSubscription = _smPrivateObject.GetFieldOrProperty(PrivateField_myProductSubscription) as ProductSubscription;

            //Assert   
            myProductSubscription.ShouldSatisfyAllConditions(() => myProductSubscription.FullZip.ShouldNotBeNullOrEmpty(),
                                                            () => myProductSubscription.PhoneCode.ShouldNotBeSameAs(_prodSubscription.PhoneCode));
        }

        [Test]
        public void BindValues_ForMexico_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = false;            
            _prodSubscription.CountryID = DummyInt;
            _prodSubscription.ZipCode = CountryFullZip;
            _prodSubscription.Plus4 = CountryFullZip;
            _prodSubscription.IsLocked = false;
            _product.AllowDataEntry = true;
            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            AddCountries();

            //Act
            Should.NotThrow(() => _smPrivateObject.Invoke(TestedMethodName_BindValues));
            var myProductSubscription = _smPrivateObject.GetFieldOrProperty(PrivateField_myProductSubscription) as ProductSubscription;

            //Assert   
            myProductSubscription.ShouldSatisfyAllConditions(() => myProductSubscription.FullZip.ShouldNotBeNullOrEmpty(),
                                                            () => myProductSubscription.PhoneCode.ShouldNotBeSameAs(_prodSubscription.PhoneCode));
        }

        [Test]        
        public void BindValues_NotANewSubscriber_BindCatTransactionValues_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = false;
            _prodSubscription.PubID = 0;
            _prodSubscription.ZipCode = null;
            _prodSubscription.FullZip = null;
            _prodSubscription.SubscriptionStatusID = DummyInt;
            _prodSubscription.ProductMapList = new List<ProductSubscriptionDetail>()
            {
                new ProductSubscriptionDetail()
                {
                    CodeSheetID = DummyInt
                }
            };
            _prodSubscription.IsLocked = false;
            _product.AllowDataEntry = true;
            _sstList.Add(new SubscriptionStatus()
            {
                SubscriptionStatusID = DummyInt,
                StatusCode = FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAFree.ToString()
            });
            
            _categoryCodeList.Add(new CategoryCode()
            {
                 CategoryCodeValue = CategoryValue
            });
            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            AddCountries();

            //Act
            Should.NotThrow(()=>_smPrivateObject.Invoke(TestedMethodName_BindValues));
            var myProductSubscription = _smPrivateObject.GetFieldOrProperty(PrivateField_myProductSubscription) as ProductSubscription;

            //Assert   
            myProductSubscription.ShouldSatisfyAllConditions(() => myProductSubscription.FullZip.ShouldBeEmpty(),
                                                            () => myProductSubscription.ZipCode.ShouldBeEmpty());
        }

        [Test]
        public void BindValues_NewSubscriptionManager_WithSuccess()
        {
            //Act
            var subsctiptionManager = new SubscriptionManager();
            var smPrivateObject = new PrivateObject(subsctiptionManager);
            var sstListReturn = smPrivateObject.GetFieldOrProperty("sstList");

            //Assert   
            sstListReturn.ShouldNotBeNull();            
        }

        private void AddCountries()
        {
            AddCountry(CountryIDUS, FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " "), DummyInt);
            AddCountry(CountryIDCanada, FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " "), CountryPrefix);
            AddCountry(DummyInt, FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.MEXICO.ToString().Replace("_", " "), DummyInt);
        }

    }
}
