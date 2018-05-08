using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class TemplateTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int templateID = -1;
            string templateStyleCode = string.Empty;
            string templateName = string.Empty;
            string templateImage = string.Empty;
            string templateDescription = string.Empty;
            string templateSource = string.Empty;
            string templateText = string.Empty;
            string templateSubject = string.Empty;        

            // Act
            Template template = new Template();    

            // Assert
            template.TemplateID.ShouldBe(templateID);
            template.BaseChannelID.ShouldBeNull();
            template.TemplateStyleCode.ShouldBe(templateStyleCode);
            template.TemplateName.ShouldBe(templateName);
            template.TemplateImage.ShouldBe(templateImage);
            template.TemplateDescription.ShouldBe(templateDescription);
            template.SortOrder.ShouldBeNull();
            template.SlotsTotal.ShouldBeNull();
            template.IsActive.ShouldBeNull();
            template.TemplateSource.ShouldBe(templateSource);
            template.TemplateText.ShouldBe(templateText);
            template.TemplateSubject.ShouldBe(templateSubject);
            template.CreatedUserID.ShouldBeNull();
            template.CreatedDate.ShouldBeNull();
            template.UpdatedUserID.ShouldBeNull();
            template.UpdatedDate.ShouldBeNull();
            template.IsDeleted.ShouldBeNull();
            template.Category.ShouldBeNull();
        }
    }
}