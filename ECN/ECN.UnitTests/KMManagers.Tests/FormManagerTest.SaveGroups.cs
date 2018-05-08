using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMDbManagers.Fakes;
using KMEntities;
using KMManagers.Fakes;
using KMModels;
using KMModels.PostModels;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace KMManagers.Tests
{
    public partial class FormManagerTest
    {
        private const string SG_Username = "username";
        private const int SG_FormGroupdId = 100;
        private const int SG_ModelGroupId = 200;
        private const int SG_ModelCustomerId = 300;
        private const string SG_ControlFieldModelName = "SG_ControlFieldModelName";
        private const string SG_FieldLabel = "SG_FieldLabel";
        private const int SG_SimilarControlId = 10;
        private const int SG_ChannelID = 10;
        private const string SG_Key = "key";
        private GroupDataFields _savedGroupDataFields;

        [TestCase(10, SG_ControlFieldModelName)]
        [TestCase(10, "differentFieldName")]
        [TestCase(-10, "differentFieldName")]
        public void SaveGroupsTest_GroupDataFieldInitiliazed(int resultantGroupDataFieldsID, string shortName)
        {
            // Arrange
            var user = new User();
            var form = new Form();
            form.Controls.Add(new Control() { Control_ID = SG_SimilarControlId });
            form.GroupID = SG_FormGroupdId;
            var model = InitChangeGroupPostModel_SaveGroupsTest(SG_SimilarControlId, SG_ControlFieldModelName);
            var controlFieldModel = model.Fields.ToList().First();
            var gdfList = CreateGroupDataFields_SaveGroupsTest(shortName);
            InitTest_SaveGroupsTest(form, gdfList, groupDataFieldsSaveMethodResult: resultantGroupDataFieldsID);

            // Act
            var response = _formManagerInstance.SaveGroups(SG_ChannelID, model, SG_Key, user, out string errors);

            // Assert
            _savedGroupDataFields.ShouldSatisfyAllConditions(
                 () => _groupDataFieldsSaveMethodCallCount.ShouldBe(shortName != SG_ControlFieldModelName ? 1 : 0),
                 () => controlFieldModel.FieldId.ShouldBe(resultantGroupDataFieldsID > 0 ? (int?)resultantGroupDataFieldsID : null),
                 () => response.Keys.Count.ShouldBeGreaterThan(0),
                 () => response.Keys.ShouldContain("result"),
                 () => response["result"].ShouldBe((resultantGroupDataFieldsID > 0).ToString()));

            if (shortName != SG_ControlFieldModelName)
            {
                _savedGroupDataFields.ShouldSatisfyAllConditions(
                    () => _savedGroupDataFields.ShouldNotBeNull(),
                    () => _savedGroupDataFields.CustomerID.ShouldBe(SG_ModelCustomerId),
                    () => _savedGroupDataFields.GroupID.ShouldBe(SG_ModelGroupId),
                    () => _savedGroupDataFields.ShortName.ShouldBe(SG_ControlFieldModelName),
                    () => _savedGroupDataFields.LongName.ShouldBe(SG_ControlFieldModelName),
                    () => _savedGroupDataFields.CreatedUserID.ShouldBe(0),
                    () => _savedGroupDataFields.IsPublic.ShouldBe("Y"),
                    () => _savedGroupDataFields.IsDeleted.HasValue.ShouldBeTrue(),
                    () => _savedGroupDataFields.IsDeleted.Value.ShouldBeFalse(),
                    () => _savedGroupDataFields.IsPrimaryKey.HasValue.ShouldBeTrue(),
                    () => _savedGroupDataFields.IsPrimaryKey.Value.ShouldBeFalse(),
                    () => _savedGroupDataFields.GroupDataFieldsID.ShouldBe(resultantGroupDataFieldsID));
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SaveGroupsTest_Exception_Error(bool ecnException)
        {
            // Arrange
            var user = new User();
            var form = new Form();
            form.Controls.Add(new Control() { Control_ID = SG_SimilarControlId });
            form.GroupID = SG_FormGroupdId;
            var model = InitChangeGroupPostModel_SaveGroupsTest(SG_SimilarControlId, SG_ControlFieldModelName);
            var controlFieldModel = model.Fields.ToList().First();
            var gdfList = CreateGroupDataFields_SaveGroupsTest(SG_ControlFieldModelName);
            InitTest_SaveGroupsTest(form, gdfList);
            ShimFormManager.AllInstances.GetFieldsByCustomerAndGroupIDInt32 = (fm, id) =>
            {
                if (ecnException)
                { throw new ECNException(new List<ECNError>() { new ECNError() { ErrorMessage = "error" } }); }
                throw new Exception();
            };

            // Act
            var response = _formManagerInstance.SaveGroups(SG_ChannelID, model, SG_Key, user, out string errors);

            // Assert
            response.ShouldSatisfyAllConditions(
               () => response.Keys.Count.ShouldBeGreaterThan(0),
               () => response.Keys.ShouldContain("result"),
               () => response["result"].ShouldBe("False"));
        }

        [TestCase(0, 0, "")]
        [TestCase(0, 0, "20")]
        [TestCase(500, 500, "")]
        public void SaveGroupsTest_ChangeFormGroup_NoErrors(int formControlId, int modelControlId, string subscriberLogingOtherIdentification)
        {
            // Arrange
            var user = new User();
            var form = new Form();
            var formControl = new Control() { Control_ID = formControlId };
            form.Controls.Add(formControl);
            var subscriberLogins = new List<SubscriberLogin>();
            subscriberLogins.Add(new SubscriberLogin() { OtherIdentification = subscriberLogingOtherIdentification });
            form.SubscriberLogins = subscriberLogins;
            form.GroupID = SG_FormGroupdId;
            var model = InitChangeGroupPostModel_SaveGroupsTest(modelControlId, SG_ControlFieldModelName);
            var controlFieldModel = model.Fields.ToList().First();
            var gdfList = CreateGroupDataFields_SaveGroupsTest(SG_ControlFieldModelName);
            InitTest_SaveGroupsTest(form, gdfList);

            // Act
            var response = _formManagerInstance.SaveGroups(SG_ChannelID, model, SG_Key, user, out string errors);

            // Assert
            response.ShouldSatisfyAllConditions(
              () => _formDbManagerSaveChangedMethodCallCount.ShouldBe(modelControlId == 0 ? 2 : 1),
              () => formControl.FieldID.ShouldBe(modelControlId != 0 ? controlFieldModel.FieldId : formControl.FieldID));
        }

        [TestCase(0, 0)]
        [TestCase(100, 100)]
        public void SaveGroupsTest_NoChangeFormGroup_NoErrors(int fomrControlId, int modelControlId)
        {
            // Arrange
            var user = new User();
            var form = new Form();
            var expectedResponseKeyLabel = modelControlId == 0 ? "Subscriber Identification" : SG_FieldLabel;
            form.Controls.Add(new Control() { FieldLabel = SG_FieldLabel, Control_ID = fomrControlId });
            form.GroupID = SG_FormGroupdId;
            var model = InitChangeGroupPostModel_SaveGroupsTest(modelControlId, SG_ControlFieldModelName, false);
            var controlFieldModel = model.Fields.ToList().First();
            var gdfList = CreateGroupDataFields_SaveGroupsTest(SG_ControlFieldModelName);
            InitTest_SaveGroupsTest(form, gdfList);

            // Act
            var response = _formManagerInstance.SaveGroups(SG_ChannelID, model,SG_Key, user, out string errors);

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.Keys.Count.ShouldBeGreaterThan(0),
                () => response.Keys.ShouldContain(expectedResponseKeyLabel),
                () => response[expectedResponseKeyLabel].ShouldBe(SG_ControlFieldModelName));
        }

        private void InitTest_SaveGroupsTest(Form form, List<GroupDataFields> gdfList, int groupDataFieldsSaveMethodResult = 200)
        {
            ShimFormDbManager.AllInstances.GetByIDInt32Int32 = (dbm, cId, fId) => form;
            ShimFormManager.AllInstances.GetCustomerByIDInt32 = (fm, id) => new Customer() { CustomerID = SG_ModelCustomerId };
            ShimFormManager.AllInstances.GetFieldsByCustomerAndGroupIDInt32 = (fm, id) => gdfList;
            ShimGroupDataFields.SaveGroupDataFieldsUser = (gdf, u) =>
            {

                _groupDataFieldsSaveMethodCallCount++;
                _savedGroupDataFields = gdf;
                return groupDataFieldsSaveMethodResult;
            };
        }

        private ChangeGroupPostModel InitChangeGroupPostModel_SaveGroupsTest(int controlId, string fieldName, bool changeFormGroup = true)
        {
            var model = new ChangeGroupPostModel();
            model.GroupId = SG_ModelGroupId;
            model.ChangeFormGroup = changeFormGroup;
            model.CustomerId = SG_ModelCustomerId;
            var fieldModels = new List<ControlFieldModel>();
            fieldModels.Add(new ControlFieldModel()
            {
                ControlId = controlId,
                FieldName = fieldName,
            });
            model.Fields = fieldModels;
            return model;
        }

        private List<GroupDataFields> CreateGroupDataFields_SaveGroupsTest(string shortName)
        {
            var gdfList = new List<GroupDataFields>();
            gdfList.Add(new GroupDataFields() { GroupDataFieldsID = 10, ShortName = shortName });
            return gdfList;
        }
    }
}
