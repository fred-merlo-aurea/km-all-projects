using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class LayoutTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Layout) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var layoutId = Fixture.Create<int>();
            var templateId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();
            var folderId = Fixture.Create<int?>();
            var layoutName = Fixture.Create<string>();
            var contentSlot1 = Fixture.Create<int?>();
            var contentSlot2 = Fixture.Create<int?>();
            var contentSlot3 = Fixture.Create<int?>();
            var contentSlot4 = Fixture.Create<int?>();
            var contentSlot5 = Fixture.Create<int?>();
            var contentSlot6 = Fixture.Create<int?>();
            var contentSlot7 = Fixture.Create<int?>();
            var contentSlot8 = Fixture.Create<int?>();
            var contentSlot9 = Fixture.Create<int?>();
            var tableOptions = Fixture.Create<string>();
            var displayAddress = Fixture.Create<string>();
            var setupCost = Fixture.Create<string>();
            var outboundCost = Fixture.Create<string>();
            var inboundCost = Fixture.Create<string>();
            var designCost = Fixture.Create<string>();
            var otherCost = Fixture.Create<string>();
            var messageTypeId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var archived = Fixture.Create<bool?>();
            var slot1 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot2 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot3 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot4 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot5 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot6 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot7 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot8 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var slot9 = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var template = Fixture.Create<Template>();
            var folder = Fixture.Create<Folder>();
            var messageType = Fixture.Create<MessageType>();
            var convLinks = Fixture.Create<List<ConversionLinks>>();

            // Act
            layout.LayoutID = layoutId;
            layout.TemplateID = templateId;
            layout.CustomerID = customerId;
            layout.FolderID = folderId;
            layout.LayoutName = layoutName;
            layout.ContentSlot1 = contentSlot1;
            layout.ContentSlot2 = contentSlot2;
            layout.ContentSlot3 = contentSlot3;
            layout.ContentSlot4 = contentSlot4;
            layout.ContentSlot5 = contentSlot5;
            layout.ContentSlot6 = contentSlot6;
            layout.ContentSlot7 = contentSlot7;
            layout.ContentSlot8 = contentSlot8;
            layout.ContentSlot9 = contentSlot9;
            layout.TableOptions = tableOptions;
            layout.DisplayAddress = displayAddress;
            layout.SetupCost = setupCost;
            layout.OutboundCost = outboundCost;
            layout.InboundCost = inboundCost;
            layout.DesignCost = designCost;
            layout.OtherCost = otherCost;
            layout.MessageTypeID = messageTypeId;
            layout.CreatedUserID = createdUserId;
            layout.CreatedDate = createdDate;
            layout.UpdatedUserID = updatedUserId;
            layout.UpdatedDate = updatedDate;
            layout.IsDeleted = isDeleted;
            layout.Archived = archived;
            layout.Slot1 = slot1;
            layout.Slot2 = slot2;
            layout.Slot3 = slot3;
            layout.Slot4 = slot4;
            layout.Slot5 = slot5;
            layout.Slot6 = slot6;
            layout.Slot7 = slot7;
            layout.Slot8 = slot8;
            layout.Slot9 = slot9;
            layout.Template = template;
            layout.Folder = folder;
            layout.MessageType = messageType;
            layout.ConvLinks = convLinks;

            // Assert
            layout.HasValidID.ShouldBeTrue();
            layout.LayoutID.ShouldBe(layoutId);
            layout.TemplateID.ShouldBe(templateId);
            layout.CustomerID.ShouldBe(customerId);
            layout.FolderID.ShouldBe(folderId);
            layout.LayoutName.ShouldBe(layoutName);
            layout.ContentSlot1.ShouldBe(contentSlot1);
            layout.ContentSlot2.ShouldBe(contentSlot2);
            layout.ContentSlot3.ShouldBe(contentSlot3);
            layout.ContentSlot4.ShouldBe(contentSlot4);
            layout.ContentSlot5.ShouldBe(contentSlot5);
            layout.ContentSlot6.ShouldBe(contentSlot6);
            layout.ContentSlot7.ShouldBe(contentSlot7);
            layout.ContentSlot8.ShouldBe(contentSlot8);
            layout.ContentSlot9.ShouldBe(contentSlot9);
            layout.TableOptions.ShouldBe(tableOptions);
            layout.DisplayAddress.ShouldBe(displayAddress);
            layout.SetupCost.ShouldBe(setupCost);
            layout.OutboundCost.ShouldBe(outboundCost);
            layout.InboundCost.ShouldBe(inboundCost);
            layout.DesignCost.ShouldBe(designCost);
            layout.OtherCost.ShouldBe(otherCost);
            layout.MessageTypeID.ShouldBe(messageTypeId);
            layout.CreatedUserID.ShouldBe(createdUserId);
            layout.CreatedDate.ShouldBe(createdDate);
            layout.UpdatedUserID.ShouldBe(updatedUserId);
            layout.UpdatedDate.ShouldBe(updatedDate);
            layout.IsDeleted.ShouldBe(isDeleted);
            layout.Archived.ShouldBe(archived);
            layout.Slot1.ShouldBe(slot1);
            layout.Slot2.ShouldBe(slot2);
            layout.Slot3.ShouldBe(slot3);
            layout.Slot4.ShouldBe(slot4);
            layout.Slot5.ShouldBe(slot5);
            layout.Slot6.ShouldBe(slot6);
            layout.Slot7.ShouldBe(slot7);
            layout.Slot8.ShouldBe(slot8);
            layout.Slot9.ShouldBe(slot9);
            layout.Template.ShouldBe(template);
            layout.Folder.ShouldBe(folder);
            layout.MessageType.ShouldBe(messageType);
            layout.ConvLinks.ShouldBe(convLinks);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Archived) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Archived_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<bool>();

            // Act , Set
            layout.Archived = random;

            // Assert
            layout.Archived.ShouldBe(random);
            layout.Archived.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Archived_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.Archived = null;

            // Assert
            layout.Archived.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Archived_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameArchived = "Archived";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameArchived);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.Archived.ShouldBeNull();
            layout.Archived.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Archived) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ArchivedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameArchived = "ArchivedNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameArchived));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Archived_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameArchived = "Archived";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameArchived);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot1) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot1_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot1 = random;

            // Assert
            layout.ContentSlot1.ShouldBe(random);
            layout.ContentSlot1.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot1_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot1 = null;

            // Assert
            layout.ContentSlot1.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot1_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot1 = "ContentSlot1";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot1);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot1.ShouldBeNull();
            layout.ContentSlot1.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot1 = "ContentSlot1NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot1 = "ContentSlot1";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot2) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot2_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot2 = random;

            // Assert
            layout.ContentSlot2.ShouldBe(random);
            layout.ContentSlot2.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot2_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot2 = null;

            // Assert
            layout.ContentSlot2.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot2_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot2 = "ContentSlot2";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot2);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot2.ShouldBeNull();
            layout.ContentSlot2.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot2 = "ContentSlot2NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot2 = "ContentSlot2";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot3) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot3_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot3 = random;

            // Assert
            layout.ContentSlot3.ShouldBe(random);
            layout.ContentSlot3.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot3_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot3 = null;

            // Assert
            layout.ContentSlot3.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot3_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot3 = "ContentSlot3";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot3);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot3.ShouldBeNull();
            layout.ContentSlot3.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot3 = "ContentSlot3NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot3 = "ContentSlot3";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot4) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot4_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot4 = random;

            // Assert
            layout.ContentSlot4.ShouldBe(random);
            layout.ContentSlot4.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot4_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot4 = null;

            // Assert
            layout.ContentSlot4.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot4_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot4 = "ContentSlot4";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot4);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot4.ShouldBeNull();
            layout.ContentSlot4.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot4 = "ContentSlot4NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot4 = "ContentSlot4";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot5) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot5_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot5 = random;

            // Assert
            layout.ContentSlot5.ShouldBe(random);
            layout.ContentSlot5.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot5_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot5 = null;

            // Assert
            layout.ContentSlot5.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot5_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot5 = "ContentSlot5";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot5);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot5.ShouldBeNull();
            layout.ContentSlot5.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot5 = "ContentSlot5NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot5 = "ContentSlot5";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot6) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot6_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot6 = random;

            // Assert
            layout.ContentSlot6.ShouldBe(random);
            layout.ContentSlot6.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot6_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot6 = null;

            // Assert
            layout.ContentSlot6.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot6_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot6 = "ContentSlot6";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot6);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot6.ShouldBeNull();
            layout.ContentSlot6.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot6) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot6 = "ContentSlot6NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot6 = "ContentSlot6";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot6);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot7) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot7_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot7 = random;

            // Assert
            layout.ContentSlot7.ShouldBe(random);
            layout.ContentSlot7.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot7_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot7 = null;

            // Assert
            layout.ContentSlot7.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot7_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot7 = "ContentSlot7";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot7);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot7.ShouldBeNull();
            layout.ContentSlot7.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot7) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot7NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot7 = "ContentSlot7NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot7));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot7_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot7 = "ContentSlot7";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot7);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot8) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot8_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot8 = random;

            // Assert
            layout.ContentSlot8.ShouldBe(random);
            layout.ContentSlot8.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot8_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot8 = null;

            // Assert
            layout.ContentSlot8.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot8_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot8 = "ContentSlot8";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot8);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot8.ShouldBeNull();
            layout.ContentSlot8.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot8) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot8NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot8 = "ContentSlot8NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot8));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot8_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot8 = "ContentSlot8";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot8);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot9) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot9_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.ContentSlot9 = random;

            // Assert
            layout.ContentSlot9.ShouldBe(random);
            layout.ContentSlot9.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot9_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.ContentSlot9 = null;

            // Assert
            layout.ContentSlot9.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot9_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentSlot9 = "ContentSlot9";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameContentSlot9);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.ContentSlot9.ShouldBeNull();
            layout.ContentSlot9.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ContentSlot9) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ContentSlot9NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSlot9 = "ContentSlot9NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameContentSlot9));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ContentSlot9_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSlot9 = "ContentSlot9";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameContentSlot9);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (ConvLinks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_ConvLinksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConvLinks = "ConvLinksNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameConvLinks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_ConvLinks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConvLinks = "ConvLinks";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameConvLinks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.CreatedUserID = random;

            // Assert
            layout.CreatedUserID.ShouldBe(random);
            layout.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.CreatedUserID = null;

            // Assert
            layout.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.CreatedUserID.ShouldBeNull();
            layout.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.CustomerID = random;

            // Assert
            layout.CustomerID.ShouldBe(random);
            layout.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.CustomerID = null;

            // Assert
            layout.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.CustomerID.ShouldBeNull();
            layout.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (DesignCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_DesignCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.DesignCost = Fixture.Create<string>();
            var stringType = layout.DesignCost.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (DesignCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_DesignCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDesignCost = "DesignCostNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameDesignCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_DesignCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDesignCost = "DesignCost";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameDesignCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (DisplayAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_DisplayAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.DisplayAddress = Fixture.Create<string>();
            var stringType = layout.DisplayAddress.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (DisplayAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_DisplayAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayAddress = "DisplayAddressNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameDisplayAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_DisplayAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayAddress = "DisplayAddress";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameDisplayAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Folder) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Folder_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFolder = "Folder";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameFolder);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Folder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_FolderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolder = "FolderNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameFolder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Folder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolder = "Folder";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameFolder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (FolderID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_FolderID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.FolderID = random;

            // Assert
            layout.FolderID.ShouldBe(random);
            layout.FolderID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_FolderID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.FolderID = null;

            // Assert
            layout.FolderID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_FolderID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameFolderID = "FolderID";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameFolderID);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.FolderID.ShouldBeNull();
            layout.FolderID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (FolderID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_FolderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameFolderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_FolderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameFolderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (HasValidID) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_HasValidID_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var boolType = layout.HasValidID.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (HasValidID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_HasValidIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHasValidID = "HasValidIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameHasValidID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_HasValidID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHasValidID = "HasValidID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameHasValidID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (InboundCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_InboundCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.InboundCost = Fixture.Create<string>();
            var stringType = layout.InboundCost.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (InboundCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_InboundCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInboundCost = "InboundCostNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameInboundCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_InboundCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInboundCost = "InboundCost";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameInboundCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<bool>();

            // Act , Set
            layout.IsDeleted = random;

            // Assert
            layout.IsDeleted.ShouldBe(random);
            layout.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.IsDeleted = null;

            // Assert
            layout.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.IsDeleted.ShouldBeNull();
            layout.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (LayoutID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_LayoutID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.LayoutID = Fixture.Create<int>();
            var intType = layout.LayoutID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (LayoutID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_LayoutIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameLayoutID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_LayoutID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameLayoutID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (LayoutName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_LayoutName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.LayoutName = Fixture.Create<string>();
            var stringType = layout.LayoutName.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (LayoutName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_LayoutNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutNameNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameLayoutName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_LayoutName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutName";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameLayoutName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (MessageType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_MessageType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameMessageType = "MessageType";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameMessageType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (MessageType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_MessageTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessageType = "MessageTypeNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameMessageType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_MessageType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessageType = "MessageType";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameMessageType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (MessageTypeID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_MessageTypeID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.MessageTypeID = random;

            // Assert
            layout.MessageTypeID.ShouldBe(random);
            layout.MessageTypeID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_MessageTypeID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.MessageTypeID = null;

            // Assert
            layout.MessageTypeID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_MessageTypeID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMessageTypeID = "MessageTypeID";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameMessageTypeID);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.MessageTypeID.ShouldBeNull();
            layout.MessageTypeID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (MessageTypeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_MessageTypeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessageTypeID = "MessageTypeIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameMessageTypeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_MessageTypeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessageTypeID = "MessageTypeID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameMessageTypeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (OtherCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_OtherCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.OtherCost = Fixture.Create<string>();
            var stringType = layout.OtherCost.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (OtherCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_OtherCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOtherCost = "OtherCostNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameOtherCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_OtherCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOtherCost = "OtherCost";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameOtherCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (OutboundCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_OutboundCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.OutboundCost = Fixture.Create<string>();
            var stringType = layout.OutboundCost.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (OutboundCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_OutboundCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOutboundCost = "OutboundCostNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameOutboundCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_OutboundCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOutboundCost = "OutboundCost";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameOutboundCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (SetupCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_SetupCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.SetupCost = Fixture.Create<string>();
            var stringType = layout.SetupCost.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (SetupCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_SetupCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSetupCost = "SetupCostNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSetupCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_SetupCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSetupCost = "SetupCost";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSetupCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot1) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot1 = "Slot1";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot1 = "Slot1NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot1 = "Slot1";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot2) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot2 = "Slot2";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot2 = "Slot2NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot2 = "Slot2";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot3) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot3 = "Slot3";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot3 = "Slot3NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot3 = "Slot3";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot4) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot4 = "Slot4";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot4 = "Slot4NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot4 = "Slot4";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot5) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot5 = "Slot5";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot5 = "Slot5NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot5 = "Slot5";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot6) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot6_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot6 = "Slot6";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot6);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot6) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot6 = "Slot6NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot6 = "Slot6";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot6);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot7) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot7_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot7 = "Slot7";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot7);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot7) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot7NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot7 = "Slot7NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot7));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot7_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot7 = "Slot7";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot7);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot8) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot8_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot8 = "Slot8";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot8);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot8) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot8NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot8 = "Slot8NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot8));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot8_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot8 = "Slot8";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot8);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot9) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot9_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot9 = "Slot9";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameSlot9);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Slot9) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_Slot9NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlot9 = "Slot9NotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameSlot9));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Slot9_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlot9 = "Slot9";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameSlot9);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (TableOptions) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_TableOptions_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            layout.TableOptions = Fixture.Create<string>();
            var stringType = layout.TableOptions.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (TableOptions) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_TableOptionsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTableOptions = "TableOptionsNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameTableOptions));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_TableOptions_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTableOptions = "TableOptions";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameTableOptions);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Template) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Template_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplate = "Template";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameTemplate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (Template) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_TemplateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplate = "TemplateNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameTemplate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Template_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTemplate = "Template";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameTemplate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (TemplateID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_TemplateID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.TemplateID = random;

            // Assert
            layout.TemplateID.ShouldBe(random);
            layout.TemplateID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_TemplateID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.TemplateID = null;

            // Assert
            layout.TemplateID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_TemplateID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameTemplateID = "TemplateID";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameTemplateID);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.TemplateID.ShouldBeNull();
            layout.TemplateID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (TemplateID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_TemplateIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplateID = "TemplateIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameTemplateID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_TemplateID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTemplateID = "TemplateID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameTemplateID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var layout = Fixture.Create<Layout>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layout, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();
            var random = Fixture.Create<int>();

            // Act , Set
            layout.UpdatedUserID = random;

            // Assert
            layout.UpdatedUserID.ShouldBe(random);
            layout.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layout = Fixture.Create<Layout>();

            // Act , Set
            layout.UpdatedUserID = null;

            // Assert
            layout.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var layout = Fixture.Create<Layout>();
            var propertyInfo = layout.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(layout, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layout.UpdatedUserID.ShouldBeNull();
            layout.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Layout) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var layout  = Fixture.Create<Layout>();

            // Act , Assert
            Should.NotThrow(() => layout.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Layout_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var layout  = Fixture.Create<Layout>();
            var propertyInfo  = layout.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Layout) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Layout_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Layout());
        }

        #endregion

        #region General Constructor : Class (Layout) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Layout_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLayout = Fixture.CreateMany<Layout>(2).ToList();
            var firstLayout = instancesOfLayout.FirstOrDefault();
            var lastLayout = instancesOfLayout.Last();

            // Act, Assert
            firstLayout.ShouldNotBeNull();
            lastLayout.ShouldNotBeNull();
            firstLayout.ShouldNotBeSameAs(lastLayout);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Layout_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLayout = new Layout();
            var secondLayout = new Layout();
            var thirdLayout = new Layout();
            var fourthLayout = new Layout();
            var fifthLayout = new Layout();
            var sixthLayout = new Layout();

            // Act, Assert
            firstLayout.ShouldNotBeNull();
            secondLayout.ShouldNotBeNull();
            thirdLayout.ShouldNotBeNull();
            fourthLayout.ShouldNotBeNull();
            fifthLayout.ShouldNotBeNull();
            sixthLayout.ShouldNotBeNull();
            firstLayout.ShouldNotBeSameAs(secondLayout);
            thirdLayout.ShouldNotBeSameAs(firstLayout);
            fourthLayout.ShouldNotBeSameAs(firstLayout);
            fifthLayout.ShouldNotBeSameAs(firstLayout);
            sixthLayout.ShouldNotBeSameAs(firstLayout);
            sixthLayout.ShouldNotBeSameAs(fourthLayout);
        }

        #endregion

        #region General Constructor : Class (Layout) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Layout_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var layoutId = -1;
            var layoutName = string.Empty;
            var tableOptions = string.Empty;
            var displayAddress = string.Empty;
            var setupCost = string.Empty;
            var outboundCost = string.Empty;
            var inboundCost = string.Empty;
            var designCost = string.Empty;
            var otherCost = string.Empty;
            var archived = false;

            // Act
            var layout = new Layout();

            // Assert
            layout.LayoutID.ShouldBe(layoutId);
            layout.TemplateID.ShouldBeNull();
            layout.CustomerID.ShouldBeNull();
            layout.FolderID.ShouldBeNull();
            layout.LayoutName.ShouldBe(layoutName);
            layout.ContentSlot1.ShouldBeNull();
            layout.ContentSlot2.ShouldBeNull();
            layout.ContentSlot3.ShouldBeNull();
            layout.ContentSlot4.ShouldBeNull();
            layout.ContentSlot5.ShouldBeNull();
            layout.ContentSlot6.ShouldBeNull();
            layout.ContentSlot7.ShouldBeNull();
            layout.ContentSlot8.ShouldBeNull();
            layout.ContentSlot9.ShouldBeNull();
            layout.TableOptions.ShouldBe(tableOptions);
            layout.DisplayAddress.ShouldBe(displayAddress);
            layout.SetupCost.ShouldBe(setupCost);
            layout.OutboundCost.ShouldBe(outboundCost);
            layout.InboundCost.ShouldBe(inboundCost);
            layout.DesignCost.ShouldBe(designCost);
            layout.OtherCost.ShouldBe(otherCost);
            layout.MessageTypeID.ShouldBeNull();
            layout.Slot1.ShouldBeNull();
            layout.Slot2.ShouldBeNull();
            layout.Slot3.ShouldBeNull();
            layout.Slot4.ShouldBeNull();
            layout.Slot5.ShouldBeNull();
            layout.Slot6.ShouldBeNull();
            layout.Slot7.ShouldBeNull();
            layout.Slot8.ShouldBeNull();
            layout.Slot9.ShouldBeNull();
            layout.Template.ShouldBeNull();
            layout.Folder.ShouldBeNull();
            layout.MessageType.ShouldBeNull();
            layout.ConvLinks.ShouldBeNull();
            layout.CreatedUserID.ShouldBeNull();
            layout.CreatedDate.ShouldBeNull();
            layout.UpdatedUserID.ShouldBeNull();
            layout.UpdatedDate.ShouldBeNull();
            layout.IsDeleted.ShouldBeNull();
            layout.Archived.ShouldBe(archived);
        }

        #endregion

        #endregion

        #endregion
    }
}