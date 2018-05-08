using System.Data;
using System.Collections.Generic;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ECN.Webservice.Tests.Facades.ListFacade
{
    [TestFixture]
    public partial class ListFacadeTest
    {
        private DataTable _dataTable;
        private const string XmlEmails = "<EmailAddresses><EmailAddress>address</EmailAddress></EmailAddresses>";
        private const string MethodBuildFolderReturnXML = "BuildFolderReturnXML";
        private const string MethodBuildGDFReturnXML = "BuildGDFReturnXML";
        private const string MethodBuildFilterReturnXML = "BuildFilterReturnXML";
        private const string MethodBuildGroupReturnXML = "BuildGroupReturnXML";

        [TearDown]
        public void TearDownDataTable()
        {
            _dataTable?.Dispose();
        }

        [Test]
        public void UpdateCustomField_ForGroupField_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new CustomFieldParams();
            InitializeUpdate();

            // Act
            var result = _facade.UpdateCustomField(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void UpdateCustomField_ForNullGroupField_ReturnFailResponse()
        {
            // Arrange
            var parameters = new CustomFieldParams();
            InitializeUpdate();
            ShimGroupDataFields.GetByIDInt32Int32User = (x, y, z) => null;

            // Act
            var result = _facade.UpdateCustomField(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void UnsubscribeSubscriber_ForEmail_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new UnsubscribeSubscriberParams() { XmlEmails = XmlEmails };
            InitializeUnsubscribeSubscriber();

            // Act
            var result = _facade.UnsubscribeSubscriber(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void UnsubscribeSubscriber_ForNullEmail_ReturnFailResponse()
        {
            // Arrange
            var parameters = new UnsubscribeSubscriberParams() { XmlEmails = "<String></String>" };
            InitializeUnsubscribeSubscriber();

            // Act
            var result = _facade.UnsubscribeSubscriber(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void UnsubscribeSubscriber_ForNullGroup_ReturnFailResponse()
        {
            // Arrange
            var parameters = new UnsubscribeSubscriberParams();
            InitializeUnsubscribeSubscriber();
            ShimGroup.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.UnsubscribeSubscriber(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void BuildFolderReturnXML_ForFolderList_ReturnXmlString()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);
            var folderList = new List<Folder>
            {
                new Folder()
                {
                   FolderID = Id,
                   FolderName = Name
                }
            };

            // Act
            var result = privateObject.Invoke(MethodBuildFolderReturnXML, new object[] { folderList }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Id.ToString()));
        }

        [Test]
        public void BuildGDFReturnXML_ForGroupDataFields_ReturnXmlString()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);
            var groupDataFields = new List<GroupDataFields> { new GroupDataFields() { ShortName = Name } };

            // Act
            var result = privateObject.Invoke(MethodBuildGDFReturnXML, new object[] { groupDataFields }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Name));
        }

        [Test]
        public void BuildFilterReturnXML_ForFilterList_ReturnXmlString()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);
            var filterList = new List<Filter> { new Filter() { FilterName = Name } };

            // Act
            var result = privateObject.Invoke(MethodBuildFilterReturnXML, new object[] { filterList }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Name));
        }

        [Test]
        public void BuildGroupReturnXML_ForDataTable_ReturnXmlString()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);
            _dataTable = new DataTable()
            {
                Columns = { "GroupID", "GroupName" },
                Rows = { { Id, Name } }
            };

            // Act
            var result = privateObject.Invoke(MethodBuildGroupReturnXML, new object[] { _dataTable }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Name));
        }

        [Test]
        public void BuildGroupReturnXML_ForGroupList_ReturnXmlString()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);
            var groupList = new List<Group> { new Group() { GroupName = Name } };

            // Act
            var result = privateObject.Invoke(MethodBuildGroupReturnXML, new object[] { groupList }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Name));
        }

        private void InitializeUnsubscribeSubscriber()
        {
            Initialize();
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimEmailGroup.UnsubscribeSubscribersInt32StringUser = (x, y, z) => new EmailGroup();
        }

        private void InitializeUpdate()
        {
            Initialize();
            DataLayerCommunicatorFakes.ShimGroupDataFields.GetByIDInt32 = (x) => new GroupDataFields() { CustomerID = Id };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimGroupDataFields.SaveGroupDataFieldsUser = (x, y) => Id;
        }
    }
}
