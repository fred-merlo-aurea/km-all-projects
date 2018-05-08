using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using ControlCenter.Controls;
using ControlCenter.Controls.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.Service;
using KMPlatform.Entity;
using KMPlatform.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAD_Lookup_WS.Interface;
using UAD_Lookup_WS.Interface.Fakes;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using static FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ControlCenter.Controls
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class ResponseGroupsTest
    {
        private const int PubId = 1;
        private IDisposable _shimsContext;
        private ResponseGroups _testEntity;
        private PrivateObject _privateTestEntity;
        private string _messageBoxText;
        private ResponseGroup _savedGroup;
        private bool _isDataLoaded;
        private bool _isDataRefreshed;
        private bool _isSavedMessageBoxShown;
        
        [SetUp]
        public void SetUp()
        {
            _messageBoxText = string.Empty;
            _savedGroup = null;
            _isDataLoaded = false;
            _isDataRefreshed = false;
            _isSavedMessageBoxShown = false;
            _shimsContext = ShimsContext.Create();
            InitializeCommonFakes();
            _testEntity = new ResponseGroups(PubId);
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
            ShimResponseGroups.AllInstances.InitializeComponent = (r) => { };
            ShimResponseGroups.AllInstances.LoadDataInt32 = (r, pubId) => { _isDataLoaded = true; };
            ShimResponseGroups.AllInstances.RefreshData = (r) => { _isDataRefreshed = true; };

            ShimServiceClient.UAD_Lookup_CodeClient = () => new ShimServiceClient<ICode>()
            {
                ProxyGet = () => new StubICode()
            };
            ShimServiceClient.UAD_ProductClient = () => new ShimServiceClient<IProduct>
            {
                ProxyGet = () => new StubIProduct()
            };
            ShimServiceClient.UAD_ResponseGroupClient = () => new ShimServiceClient<IResponseGroup>
            {
                ProxyGet = () => new StubIResponseGroup
                {
                    SaveGuidClientConnectionsResponseGroup = (key,ccon,group) => 
                    {
                        _savedGroup = group;
                        return new Response<int>
                        {
                             Result = 1,
                             Status = Status,
                        };
                    }
                }
            };
            ShimUser.AllInstances.CurrentClientGet = (u) => new Client { ClientID = 1 };
        }

        private T Get<T>(string fieldName)
        {
            var field = _testEntity.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
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

        private static object GetNestedType(string typeName, ResponseGroup group)
        {
            var type = typeof(ResponseGroups).
                GetNestedType(typeName,BindingFlags.NonPublic | BindingFlags.Instance);
            if (type != null)
            {
                var constructor = type.GetConstructor(new Type[] { typeof(ResponseGroup) });
                if (constructor == null)
                {
                    throw new KeyNotFoundException($"Specified constructor not found in {typeName}");
                }
                var instance = constructor.Invoke(new object[] { group });
                return instance;
            }
            return null;
        }

        private void SetCurrentResponseGroupField(object value)
        {
            var currentResponseGroup = typeof(ResponseGroups).
                GetField(CurrentResponseGroupField,BindingFlags.Instance | BindingFlags.NonPublic);
            if (currentResponseGroup == null)
            {
                throw new KeyNotFoundException($"Specified field {CurrentResponseGroupField} not found in {nameof(ResponseGroups)}");
            }
            currentResponseGroup.SetValue(_testEntity, value);
        }

        private void SetResponseGroupsListField(IReadOnlyCollection<ResponseGroup> groups)
        {
            var responseGroups = typeof(ResponseGroups).
                GetField(ResponseGroupsField,BindingFlags.Instance | BindingFlags.NonPublic);
            if(responseGroups == null)
            {
                throw new KeyNotFoundException($"Specified field {ResponseGroupsField} not found in {nameof(ResponseGroups)}");
            }
            responseGroups.SetValue(_testEntity, groups);
        }
    }
}
