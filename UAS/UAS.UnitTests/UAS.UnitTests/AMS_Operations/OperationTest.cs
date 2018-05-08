using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AMS_Operations;
using FrameworkUAS.Entity;
using FrameworkSubGen.Entity;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using FrameworkSubGen.Object;

namespace UAS.UnitTests.AMS_Operations
{
    [TestFixture]
    public partial class OperationTest : Fakes
    {
        private const string SamplePubCode = "SamplePubCode";
        private const string SampleKMPubCode = "SampleKMPubCode";
        private const string SampleFirstName = "SampleFirstName";
        private const string SampleLastName = "SampleLastName";
        private const string SampleCompany = "SampleCompany";
        private const string SampleTitle = "SampleTitle";
        private const string SamplePhone = "123456789";
        private const string SampleEmail = "sample@sample.com";
        private const string SampleSource = "SampleSource";
        private const string SamplePublication = "SamplePublication";
        private const string SampleResponseGroup = "SampleResponseGroup";
        private const string CountryUS = "United States";
        private const string CountryCanada = "Canada";
        private const string CountryBrazil = "Brazil";
        private const string StateUS = "Fresno";
        private const string StateCanada = "Montreal";
        private const string StateBrazil = "Sao Paulo";
        private const string StringYes = "Yes";
        private const string StringNo = "No";
        private const string StringMaybe = "Maybe";
        private const string OtherDimensionField = "demo";
        private const string CustomMapperField = "demo1";
        private const string PublicationsField = "publications";
        private const string SampleDimensionResponse = "123456789";
        private const string FaxDimensionField = "fax";
        private const string ImportErrorField = "importErrors";
        private const string FileName = "SampleFile";
        private const string FileImportFolderKey = "SubGenSubscriberFileImportFolder";
        private const string CreateFileMethodName = "CreateFile";
        private const string ConvertImportSubscriberToSubscriberTransMethodName = "Convert_ImportSubscriber_to_SubscriberTrans";
        private const string HourlyDataSyncMethodName = "HourlyDataSync";
        private const string EmailErrorLogMethodName = "EmailErrorLog";
        private const string LogImportErrorMethodName = "LogImportError";
        private static readonly List<string> DemoFields = new List<string> { "demo31" ,"demo32","demo33", "demo34","demo35", "demo36", };
        private Mocks mocks;                                                 
        private List<ImportError> _importErrors;                                     
        private Operations _operations;                                      
        private Client _client;                                              
        private PrivateObject _privateOperationsObj;
        private bool _isBundleSaved = false;
        private bool _isPublicationSaved = false;
        private bool _isCustomFieldSaved = false;
        private bool _isValueOptionSaved = false;
        private bool _isUserSaved = false;
        private bool _isCustomFieldCreated = false;
        private bool _isExpectionLogged = false;
        private bool _isImportSubscriberUpdated = false;
        private List<ImportSubscriber> _updatedimportSubscriberList;

        [SetUp]
        [ExcludeFromCodeCoverage]
        public void SetUp()
        {
            mocks = new Mocks();
            SetupFakes(mocks);
            _operations = new Operations();
            _client = new Client();
            _privateOperationsObj = new PrivateObject(_operations);
            _privateOperationsObj.SetField(PublicationsField, new List<Publication> {
                new Publication
                {
                    publication_id = 0,
                    KMPubCode = SampleKMPubCode
                }
            });
        }
    }
}
