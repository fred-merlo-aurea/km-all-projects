using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules;
using FrameworkServices.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAD_WS.Interface;
using UAS.UnitTests.FileMapperWizard.Common;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class MapColumnsTest : Fakes
    {
        private const string ContainerField = "thisContainer";

        private MapColumns _testEntity;
        private PrivateObject _privateTestObj;
        private FMUniversal _container;
        private bool _isbusyIconBusy = false;

        [SetUp]
        public void SetUp()
        {
            SetupFakes();
            ShimMapColumns.ConstructorFMUniversal = null;
            // This is called in construtor and is our target test public method,so would test it explicitly by calling. 
            ShimMapColumns.AllInstances.LoadData = (m) => { };
            _container = GetShimFMUniversalContainer().Instance;
            _testEntity = new MapColumns(_container);
            _privateTestObj = new PrivateObject(_testEntity);
        }
    }
}
