using System;
using System.Diagnostics.CodeAnalysis;
using ecn.editor.includes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Editor.Tests.editor.filemanager
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class FileManagerTests : BasePageTests
    {
        private const string One = "1";
        private const string QueryStringCustomerId = "cuID";
        private const string QueryStringChannelId = "chID";
        private const string PageLoadMethod = "Page_Load";
        private const string MainGalleryControl = "maingallery";
        private const string ExpectedImageDirectory = "/customers/1/images";
        private ecn.editor.filemanager _entity;
        
        [SetUp]
        public void Setup()
        {
            _entity = new ecn.editor.filemanager();
            InitializePage(_entity);
            InitializeAllControls(_entity);

            QueryString.Clear();
            QueryString.Add(QueryStringCustomerId, One);
            QueryString.Add(QueryStringChannelId, One);
        }
        
        [Test]
        public void Page_Load_Default_ImageDirectoryIsCorrect()
        {
            // Arrange
            
            // Act
            PrivatePage.Invoke(PageLoadMethod, this, EventArgs.Empty);

            // Assert
            var imageGallery = GetField<imageGallery>(MainGalleryControl);
            imageGallery.imageDirectory.ShouldBe(ExpectedImageDirectory);
        }

        [Test]
        public void CustomerId_QueryString1_Returns1()
        {
            // Arrange
            
            // Act
            var result = _entity.CustomerId;

            // Assert
            result.ShouldBe(One);
        }

        [Test]
        public void ChannelId_QueryString1_Returns1()
        {
            // Arrange
            
            // Act
            var result = _entity.ChannelId;

            // Assert
            result.ShouldBe(One);
        }
    }
}
