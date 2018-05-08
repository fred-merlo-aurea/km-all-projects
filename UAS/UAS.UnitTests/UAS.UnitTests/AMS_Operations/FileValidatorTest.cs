using System.Diagnostics.CodeAnalysis;
using AMS_Operations;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    ///     Unit Tests for <see cref="FileValidator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FileValidatorTest : Fakes
    {
        private Mocks mocks;
        private SourceFile sourceFile;
        private FileValidator validator;
        private Client client;

        /// <summary>
        ///     Common Setup for <see cref="FileValidator"/> method tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            mocks = new Mocks();
            SetupFakes(mocks);
            sourceFile = new SourceFile();
            validator = new FileValidator();
            client = new Client();
        }
    }
}