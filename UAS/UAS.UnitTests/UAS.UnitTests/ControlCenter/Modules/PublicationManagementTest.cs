using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Fakes;
using ControlCenter.Modules;
using ControlCenter.Modules.Fakes;
using Core_AMS.Utilities.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAS.Object.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using UAS.UnitTests.Helpers;
using KMEntity = KMPlatform.Entity;
using KMEntityFakes = KMPlatform.Entity.Fakes;

namespace UAS.UnitTests.ControlCenter.Modules
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class PublicationManagementTest
    {
        private PublicationManagement _testEntity;
        private IDisposable _shimsContext;
        private bool _isDataLoaded;
        private bool _isWindowClosed;
        private string _messageBoxText;
        private static readonly Application App = new Application(); // only one instance created per AppDomain.

        [SetUp]
        public void SetUp()
        {
            _shimsContext = ShimsContext.Create();
            InitializeFakes();
            App.MainWindow = new Window();
            ShimApplication.CurrentGet = () => App;
            ShimAppData.CheckParentWindowUidString = (uid) => true;
            ShimAppData.IsKmUser = () => true;
            _isDataLoaded = false;
            _isWindowClosed = false;
            _testEntity = new PublicationManagement();
            _messageBoxText = string.Empty;
        }

        [TearDown]
        public void CleanUp()
        {
            _shimsContext.Dispose();
        }

        private void InitializeFakes()
        {
            // Faked it as the original call throws runtime exception for 'StaticResource HorizontalRadioButtonList'
            ShimPublicationManagement.AllInstances.InitializeComponent = (p) => { };
            ShimPublicationManagement.AllInstances.LoadDataString = (p, id) => { _isDataLoaded = true; };
            ShimPublicationManagement.AllInstances.CloseWindow = (p) => { _isWindowClosed = true; };
            ShimServiceClient.UAS_ClientClient = () => new ShimServiceClient<IClient>
            {
                ProxyGet = () => new StubIClient()
            };
            ShimServiceClient.UAS_UserClient = () => new ShimServiceClient<IUser>
            {
                ProxyGet = () => new StubIUser()
            };
            ShimServiceClient.UAD_ProductClient = () => new ShimServiceClient<IProduct>
            {
                ProxyGet = () => new StubIProduct()
            };
            ShimServiceClient.UAD_ProductTypesClient = () => new ShimServiceClient<IProductTypes>
            {
                ProxyGet = () => new StubIProductTypes()
            };
            ShimServiceClient.UAD_FrequencyClient = () => new ShimServiceClient<IFrequency>
            {
                ProxyGet = () => new StubIFrequency()
            };
            KMEntityFakes.ShimUser.AllInstances.CurrentClientGet = (u) => new KMEntity.Client { ClientID = 1 };
            ShimWPF.MessageErrorString = (msg) => { _messageBoxText += msg; };
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString = (msg, b, i, r, s) => 
            {
                _messageBoxText += msg;
            };
        }

        private static void CallBtnSaveClick(object instance, params object[] args)
        {
            typeof(PublicationManagement).CallMethod(
                    BtnNewSaveClickMethodName,
                    args,
                    instance);
        }

        private T Get<T>(string fieldName)
        {
            var field = _testEntity.
                        GetType().
                        GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new KeyNotFoundException($"Speicified {fieldName} not found in {nameof(_testEntity)}");
            }
            if (field.GetValue(_testEntity) == null)
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
