using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using FileMapperWizard.Controls;
using FileMapperWizard.Modules;
using FrameworkUAS.Object;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAS.UnitTests.FileMapperWizard.Common;

namespace UAS.UnitTests.FileMapperWizard.Controls
{

    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class DataMapTransformationTest : Fakes
    {
        private const string SampleType = "SampleType";
        private const string SampleDesiredData = "SampleDesiredData";
        private const string SampleSourceData = "SampleSourceData";

        private const string ContainerField = "thisContainer";
        private const string ListPubCodeField = "lstPubCode";
        private const string MyListField = "myList";
        private const string SpSingleDataMapField = "spSingleDataMap";
        private const string BtnStep1NextClickMethodName = "btnStep1Next_Click";
        private const string SaveDataMapMethodName = "SaveDataMap";

        private DataMapTransformation _testEntity;
        private PrivateObject _privateTestObj;
        private FMUniversal _container;
        private AppData _appData;
        private int _sourceFileId = 1;
        private Client _client;
        private StackPanel _parent;
        private string _transID = "1";
        private string _mapID = "1";
        private bool _existing = true;

        [SetUp]
        public void SetUp()
        {
            SetupFakes();
            _client = new Client();
            _parent = new StackPanel();
            _appData = new AppData();
            _container = GetShimFMUniversalContainer().Instance;
            _testEntity = new DataMapTransformation(_appData, _container, _sourceFileId, _client, _parent, _transID, _mapID, _existing);
            _privateTestObj = new PrivateObject(_testEntity);

        }
    }
}
