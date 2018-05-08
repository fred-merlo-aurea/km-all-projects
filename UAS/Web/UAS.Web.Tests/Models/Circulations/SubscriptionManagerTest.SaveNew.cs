using NUnit.Framework;
using Shouldly;
using UAS.Web.Models.Circulations;

namespace UAS.Web.Tests.Models.Circulations
{
    [TestFixture]
    public partial class SubscriptionManagerTest
    {
        private const string TestedMethodName_SaveNew = "SaveNew";                

        [Test]
        public void SaveNew_NewSubscriber_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = true;
            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            //Act
            var returnResult = _smPrivateObject.Invoke(TestedMethodName_SaveNew);

            //Assert
            returnResult.ShouldBe(SaveNewSuccessReturnValue);
        }

        [Test]
        public void SaveNew_CompareSubscriber_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = false;
            _orgSubscription.IsInActiveWaveMailing = true;
            _prodSubscription.FirstName = DummyStringValue;
            _prodSubscription.LastName = DummyStringValue;
            _prodSubscription.Title = DummyStringValue;
            _prodSubscription.Company = DummyStringValue;
            _prodSubscription.Address1 = DummyStringValue;
            _prodSubscription.Address2 = DummyStringValue;
            _prodSubscription.Address3 = DummyStringValue;
            _prodSubscription.AddressTypeCodeId = DummyInt;
            _prodSubscription.City = DummyStringValue;
            _prodSubscription.RegionCode = RegionCode;
            _prodSubscription.RegionID = RegionID;
            _prodSubscription.ZipCode = CountryFullZip;
            _prodSubscription.Plus4 = CountryFullZip;
            _prodSubscription.Country = CountryName;
            _prodSubscription.CountryID = CountryID;
            _prodSubscription.Email = DummyEmailAddress;
            _prodSubscription.Phone = DummyStringValue;
            _prodSubscription.Fax = DummyStringValue;
            _prodSubscription.Mobile = DummyStringValue;
            _prodSubscription.Demo7 = DummyStringValue;
            _prodSubscription.PubCategoryID = DummyInt;
            _prodSubscription.PubTransactionID = DummyInt;
            _prodSubscription.IsSubscribed = true;
            _prodSubscription.SubscriptionStatusID = DummyInt;
            _prodSubscription.Copies = DummyInt;
            _prodSubscription.PhoneExt = DummyStringValue;
            _prodSubscription.IsPaid = true;

            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            //Act
            var returnResult = _smPrivateObject.Invoke(TestedMethodName_SaveNew);

            //Assert
            returnResult.ShouldBe(SaveNewSuccessReturnValue);
        }

        [Test]
        public void SaveNew_NewSubscriber_WithZipCode_WithSuccess()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = true;
            _prodSubscription.FullZip = ContryUSFullZip;

            _user.CurrentClientGroup = CreateClientGroup();

            _saveNewReturnValue = SaveNewSuccessReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            //Act
            var returnResult = _smPrivateObject.Invoke(TestedMethodName_SaveNew);

            //Assert
            returnResult.ShouldBe(SaveNewSuccessReturnValue);
        }

        [Test]
        public void SaveNew_NotANewSubscriber_WithAdHocFieldValue_WithSaveError()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = false;
            _orgSubscription.IsInActiveWaveMailing = true;
            _user.CurrentClientGroup = CreateClientGroup();

            _prodSubscription.AdHocFields.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc()
            {
                Value = ValueForErrorList
            });            
            
            _saveNewReturnValue = SaveNewErrorReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            //Act
            var returnResult = _smPrivateObject.Invoke(TestedMethodName_SaveNew);
            var subscribeVMReturn = _smPrivateObject.GetFieldOrProperty(PrivateField_VM) as SubscriberViewModel;

            //Assert
            returnResult.ShouldSatisfyAllConditions(()=> returnResult.ShouldBe(SaveNewErrorReturnValue),
                                                    ()=> subscribeVMReturn.ErrorList.Count.ShouldBeGreaterThan(0));            
        }

        [Test]
        public void SaveNew_NotANewSubscriber_WithCountryAndRegionValues_WithSaveError()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = false;
            _orgSubscription.IsInActiveWaveMailing = true;

            _prodSubscription.CountryID = CountryID;
            _orgSubscription.CountryID = CountryID;
            _prodSubscription.FullZip = CountryFullZip;
            AddCountry(CountryID, CountryName, CountryPrefix);

            _prodSubscription.RegionID = RegionID;
            _orgSubscription.RegionID = RegionID;
            _regionList.Add(new FrameworkUAD_Lookup.Entity.Region()
            {
                RegionID = RegionID,
                RegionCode = RegionCode
            });

            var subscribeVM = CreateSubscribeVM();
            subscribeVM.MySubscriptionPaid = null;
            subscribeVM.MyPaidBillTo = null;

            _user.CurrentClientGroup = CreateClientGroup();
            _saveNewReturnValue = SaveNewErrorReturnValue;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, subscribeVM);

            //Act
            var returnResult = _smPrivateObject.Invoke(TestedMethodName_SaveNew);
            var subscribeVMReturn = _smPrivateObject.GetFieldOrProperty(PrivateField_VM) as SubscriberViewModel;

            //Assert
            returnResult.ShouldSatisfyAllConditions(() => returnResult.ShouldBe(SaveNewErrorReturnValue),
                                                    () => subscribeVMReturn.ErrorList.Count.ShouldBeGreaterThan(0));
        }

        [Test]
        public void SaveNew_CaughtException_WithSaveError_AndExceptionLogged()
        {
            //Arrange            
            _prodSubscription.IsNewSubscription = false;
            _smPrivateObject.SetFieldOrProperty(PrivateField_VM, CreateSubscribeVM());

            //Act
            var returnResult = _smPrivateObject.Invoke(TestedMethodName_SaveNew);
            var subscribeVMReturn = _smPrivateObject.GetFieldOrProperty(PrivateField_VM) as SubscriberViewModel;

            //Assert
            returnResult.ShouldSatisfyAllConditions(() => returnResult.ShouldBe(SaveNewErrorReturnValue),
                                                    () => _criticalErrorWasLogged.ShouldBeTrue(),
                                                    () => subscribeVMReturn.ErrorList.Count.ShouldBeGreaterThan(0));
        }
    }
}
