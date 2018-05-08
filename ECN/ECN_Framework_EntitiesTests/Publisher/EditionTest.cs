using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class EditionTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int editionID = -1;
            string editionName = string.Empty;
            int publicationID = -1;
            string theme = string.Empty;
            string background = string.Empty;
            string fileName = string.Empty;
            int pages = -1;
            string thumbnailPage = string.Empty;
            ECN_Framework_Common.Objects.Publisher.Enums.Status statusID = ECN_Framework_Common.Objects.Publisher.Enums.Status.Active;
            string status = string.Empty;
            string xmlTOC = string.Empty;        

            // Act
            Edition edition = new Edition();    

            // Assert
            edition.EditionID.ShouldBe(editionID);
            edition.EditionName.ShouldBe(editionName);
            edition.PublicationID.ShouldBe(publicationID);
            edition.EnableDate.ShouldBeNull();
            edition.DisableDate.ShouldBeNull();
            edition.Theme.ShouldBe(theme);
            edition.Background.ShouldBe(background);
            edition.FileName.ShouldBe(fileName);
            edition.Pages.ShouldBe(pages);
            edition.ThumbnailPage.ShouldBe(thumbnailPage);
            edition.StatusID.ShouldBe(statusID);
            edition.Status.ShouldBe(status);
            edition.xmlTOC.ShouldBe(xmlTOC);
            edition.IsSearchable.ShouldBeNull();
            edition.IsLoginRequired.ShouldBeNull();
            edition.OriginalEditionID.ShouldBeNull();
            edition.Version.ShouldBeNull();
            edition.CreatedUserID.ShouldBeNull();
            edition.CreatedDate.ShouldBeNull();
            edition.UpdatedUserID.ShouldBeNull();
            edition.UpdatedDate.ShouldBeNull();
            edition.IsDeleted.ShouldBeNull();
            edition.CustomerID.ShouldBeNull();
            edition.PageList.ShouldBeNull();
        }
    }
}