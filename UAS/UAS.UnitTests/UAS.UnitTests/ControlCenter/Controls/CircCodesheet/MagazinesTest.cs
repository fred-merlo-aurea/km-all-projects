using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using ControlCenter.Controls.CircCodesheet.Fakes;
using ControlCenter.Controls.CircCodesheet;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.Service;
using KMPlatform.Entity;
using KMPlatform.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAD_Lookup_WS.Interface;
using UAD_Lookup_WS.Interface.Fakes;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using static FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ControlCenter.Controls.CircCodesheet
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class MagazinesTest
    {
        private const int PubId = 1;
        private IDisposable _shimsContext;
        private Magazines _testEntity;
        private PrivateObject _privateTestEntity;
        private string _messageBoxText;
        private Product _savedProduct;
        private bool _isDataLoaded;
        
        [SetUp]
        public void SetUp()
        {
            _messageBoxText = string.Empty;
            _savedProduct = null;
            _isDataLoaded = false;
            _shimsContext = ShimsContext.Create();
            InitializeCommonFakes();
            _testEntity = new Magazines();
            _privateTestEntity = new PrivateObject(_testEntity);
        }
        [TearDown]
        public void CleanUp()
        {
            _shimsContext.Dispose();
        }

        private void InitializeCommonFakes(ServiceResponseStatusTypes Status = ServiceResponseStatusTypes.Success)
        {
            // Faked it as normal invokation throws exception for 'StaticResource KMLightBlueGradient'
            ShimMagazines.AllInstances.InitializeComponent = (r) => { };
            ShimMagazines.AllInstances.LoadData = (r) => { _isDataLoaded = true; };

            ShimServiceClient.UAD_Lookup_CodeClient = () => new ShimServiceClient<ICode>()
            {
                ProxyGet = () => new StubICode()
            };
            ShimServiceClient.UAD_ProductClient = () => new ShimServiceClient<IProduct>
            {
                ProxyGet = () => new StubIProduct
                {
                    SaveGuidProductClientConnections = (key, product, ccon) =>
                    {
                        _savedProduct = product;
                        return new Response<int>
                        {
                            Result = 1,
                            Status = Status,
                        };
                    }
                }
            };
            ShimServiceClient.UAS_ClientClient = () => new ShimServiceClient<IClient>
            {
                ProxyGet = () => new StubIClient()
            };
            ShimServiceClient.UAD_ResponseGroupClient = () => new ShimServiceClient<IResponseGroup>
            {
                ProxyGet = () => new StubIResponseGroup()
            };
            ShimServiceClient.UAD_FrequencyClient = () => new ShimServiceClient<IFrequency>
            {
                ProxyGet = () => new StubIFrequency()
            };

            ShimUser.AllInstances.CurrentClientGet = (u) => new Client { ClientID = 1 };
        }

        private T Get<T>(string fieldName)
        {
            var field = _testEntity.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new KeyNotFoundException($"Speicified {fieldName} not found in {nameof(_testEntity)}");
            }
            if (field != null && field.GetValue(_testEntity) == null)
            {
                var constructor = field.FieldType.GetConstructor(new Type[0]);
                if (constructor != null)
                {
                    var instance = constructor.Invoke(new object[0]);
                    if (instance != null)
                    {
                        field.SetValue(_testEntity, instance);
                        return (T)instance;
                    }
                }
            }
            return (T)field.GetValue(_testEntity);
        }
    }
}