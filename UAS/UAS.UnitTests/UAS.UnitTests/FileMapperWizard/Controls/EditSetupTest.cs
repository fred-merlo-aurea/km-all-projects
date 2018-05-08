using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileMapperWizard.Controls;
using FileMapperWizard.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAS.UnitTests.FileMapperWizard.Common;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class EditSetupTest : Fakes
    {
        private const string ContainerField = "thisContainer";
        private const string BtnStep1NextClickMethodName = "btnStep1Next_Click";

        private EditSetup _testEntity;
        private PrivateObject _privateTestObj;
        private FMUniversal _container; 

        [SetUp]
        public void SetUp()
        {
            SetupFakes();
            _container = GetShimFMUniversalContainer().Instance;
            _testEntity = new EditSetup(_container);
            _privateTestObj = new PrivateObject(_testEntity);
            
        }
    }
}
