using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketinAutomation.Tests.Models.PostModels
{
    public partial class ControlBaseTest
    {
        private const string CategoryCampaign = "Campaign";
        private const string CategoryNoclick = "Noclick";
        private const string CategoryNoopen = "Noopen";
        private const string CategoryOpennoclick = "Opennoclick";
        private const string CategorySent = "Sent";
        private const string CategoryNotsent = "Notsent";
        private const string CategorySuppressed = "Suppressed";
        private const string CategoryGEclick = "GEclick";
        private const string CategoryGEopen = "GEopen";
        private const string CategoryDEopen = "DEopen";
        private const string CategorySubscribe = "Subscribe";
        private const string CategoryUnsubscribe = "Unsubscribe";
        private const string CategoryDEclick = "DEclick";
        private const string Categoryform = "form";
        private const string CategoryFormsubmit = "Formsubmit";
        private const string CategoryFormabandon = "Formabandon";
        private const string CategoryStart = "Start";
        private const string CategoryEnd = "End";
        private const string CategoryWait = "Wait";
        private const string SampleMessage = "SampleMessage";

        [Test]
        public void Deserialize_WhenCampaignItemControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new CampaignItem
                {
                    SelectedGroups = new List<GroupSelect>(),
                    SelectedGroupFilters = new List<FilterSelect>(),
                    SuppressedGroups = new List<GroupSelect>(),
                    SuppressedGroupFilters = new List<FilterSelect>()
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryCampaign;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(CampaignItem)),
                () => 
                {
                    var campItem = resultControlList[0] as CampaignItem;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.CampaignID.ShouldBe(1),
                        () => campItem.CampaignItemName.ShouldBe("SampleCampaignItem"),
                        () => campItem.CampaignName.ShouldBe("SampleCampaign"),
                        () => campItem.CustomerID.ShouldBe(1),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenCampaignItemControlTypeAndGroupsisNotEmpty_ThrowsNullReferenceException()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new CampaignItem
                {
                    SelectedGroups = new List<GroupSelect> { new GroupSelect { } },
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryCampaign;
            SetDynamicProperties(jsonObject);

            // This is a BUG
            // Line no : 1183 
            // Tries to add groups to an unintialized collection.(The 'SelectedGroups' collection property is not initailzed in constructor)
            // Act and Assert
            Should.Throw<NullReferenceException>(() => ControlBase.Deserialize(jsonObject));
        }

        [Test]
        public void Deserialize_WhenCampaignItemControlTypeAndGroupsFilterisNotEmpty_ThrowsNullReferenceException()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new CampaignItem
                {
                    SelectedGroups = new List<GroupSelect>(),
                    SelectedGroupFilters = new List<FilterSelect> { new FilterSelect { } }
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryCampaign;
            SetDynamicProperties(jsonObject);

            // This is a BUG
            // Line no : 1193 
            // Tries to add groupsFilter to an unintialized collection.(The 'SelectedGroupFilters' collection property is not initailzed in constructor)
            // Act and Assert
            Should.Throw<NullReferenceException>(() => ControlBase.Deserialize(jsonObject));
        }

        [Test]
        public void Deserialize_WhenCampaignItemControlTypeAndSuppressedGroupsisNotEmpty_ThrowsNullReferenceException()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new CampaignItem
                {
                    SelectedGroups = new List<GroupSelect>(),
                    SelectedGroupFilters = new List<FilterSelect>(),
                    SuppressedGroups = new List<GroupSelect> { new GroupSelect { } },
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryCampaign;
            SetDynamicProperties(jsonObject);

            // This is a BUG
            // Line no : 1203
            // Tries to add groups to an unintialized collection.(The 'SuppressedGroups' collection property is not initailzed in constructor)
            // Act and Assert
            Should.Throw<NullReferenceException>(() => ControlBase.Deserialize(jsonObject));
        }

        [Test]
        public void Deserialize_WhenCampaignItemControlTypeAndSuppressedGroupsFilterisNotEmpty_ThrowsNullReferenceException()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new CampaignItem
                {
                    SelectedGroups = new List<GroupSelect>(),
                    SelectedGroupFilters = new List<FilterSelect>(),
                    SuppressedGroups = new List<GroupSelect>(),
                    SuppressedGroupFilters = new List<FilterSelect> { new FilterSelect{ } },
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryCampaign;
            SetDynamicProperties(jsonObject);

            // This is a BUG
            // Line no : 1213 
            // Tries to add groups to an uninitialized collection.(The 'SuppressedGroupFilters' collection property is not initialized in constructor)
            // Act and Assert
            Should.Throw<NullReferenceException>(() => ControlBase.Deserialize(jsonObject));
        }

        [Test]
        public void Deserialize_WhenGroupControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Group
                {
                    ControlID = "1",
                    CustomerName = "SampleCustomer",
                    GroupID = 1,
                    GroupName = "SampleGroupName",
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Group)),
                () =>
                {
                    var campItem = resultControlList[0] as Group;
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.GroupID.ShouldBe(1),
                        () => campItem.GroupName.ShouldBe("SampleGroupName"),
                        () => campItem.CustomerID.ShouldBe(1),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenNoclickControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new NoClick
                {
                    ControlID = "1",
                    CampaignItemName = "SampleCampaignItemName"
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryNoclick;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(NoClick)),
                () =>
                {
                    var campItem = resultControlList[0] as NoClick;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.CampaignItemName.ShouldBe("SampleCampaign"),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenNoOpenControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new NoOpen
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryNoopen;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(NoOpen)),
                () =>
                {
                    var campItem = resultControlList[0] as NoOpen;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenOpenNoClickControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Open_NoClick
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryOpennoclick;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Open_NoClick)),
                () =>
                {
                    var campItem = resultControlList[0] as Open_NoClick;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenSentControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Sent
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategorySent;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Sent)),
                () =>
                {
                    var campItem = resultControlList[0] as Sent;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenNotSentControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new NotSent
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryNotsent;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(NotSent)),
                () =>
                {
                    var campItem = resultControlList[0] as NotSent;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenSuppressedControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Suppressed
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategorySuppressed;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Suppressed)),
                () =>
                {
                    var campItem = resultControlList[0] as Suppressed;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenClickControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Click
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryGEclick;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Click)),
                () =>
                {
                    var campItem = resultControlList[0] as Click;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenOpenControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Open
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryGEopen;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Open)),
                () =>
                {
                    var campItem = resultControlList[0] as Open;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenDirectOpenControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Direct_Open
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryDEopen;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Direct_Open)),
                () =>
                {
                    var campItem = resultControlList[0] as Direct_Open;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenSubscribeControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Subscribe
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategorySubscribe;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Subscribe)),
                () =>
                {
                    var campItem = resultControlList[0] as Subscribe;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenUnsubscribeControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Unsubscribe
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryUnsubscribe;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Unsubscribe)),
                () =>
                {
                    var campItem = resultControlList[0] as Unsubscribe;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenDirectClickControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Direct_Click
                {
                    ControlID = "1",
                    MessageName = SampleMessage
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryDEclick;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Direct_Click)),
                () =>
                {
                    var campItem = resultControlList[0] as Direct_Click;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageName.ShouldBe(SampleMessage),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenFormControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Form
                {
                    ControlID = "1",
                    FormName = "SampleFormName",
                    FormID = 1
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = Categoryform;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Form)),
                () =>
                {
                    var campItem = resultControlList[0] as Form;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.FormID.ShouldBe(1),
                        () => campItem.FormName.ShouldBe("SampleFormName"),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenFormSubmitControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new FormSubmit
                {
                    ControlID = "1",
                    FromName = "SampleFromName",
                    MessageID = 1
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryFormsubmit;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(FormSubmit)),
                () =>
                {
                    var campItem = resultControlList[0] as FormSubmit;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageID.ShouldBe(1),
                        () => campItem.FromName.ShouldBe("SampleFromName"),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenFormAbandonControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new FormAbandon
                {
                    ControlID = "1",
                    FromName = "SampleFromName",
                    MessageID = 1
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryFormabandon;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(FormAbandon)),
                () =>
                {
                    var campItem = resultControlList[0] as FormAbandon;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(1),
                        () => campItem.MessageID.ShouldBe(1),
                        () => campItem.FromName.ShouldBe("SampleFromName"),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenStartControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Start
                {
                    ControlID = "1",
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryStart;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(Start)),
                () =>
                {
                    var campItem = resultControlList[0] as Start;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(-1),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.Text.ShouldBe("Start"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenEndControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new End
                {
                    ControlID = "1",
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryEnd;
            SetDynamicProperties(jsonObject);

            // Act
            var resultControlList = ControlBase.Deserialize(jsonObject) as List<ControlBase>;

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.ShouldSatisfyAllConditions(
                () => resultControlList.Count.ShouldBe(1),
                () => resultControlList[0].ShouldBeOfType(typeof(End)),
                () =>
                {
                    var campItem = resultControlList[0] as End;
                    campItem.ShouldNotBeNull();
                    campItem.ShouldSatisfyAllConditions(
                        () => campItem.ECNID.ShouldBe(-1),
                        () => campItem.ControlID.ShouldBe("1"),
                        () => campItem.Text.ShouldBe("End"),
                        () => campItem.MAControlID.ShouldBe(1),
                        () => campItem.IsDirty.ShouldBeTrue());
                });
        }

        [Test]
        public void Deserialize_WhenWaitControlType_ReturnsDeserializedObject()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                new Wait
                {
                    ControlID = "1",
                    Days = 1,
                    Hours = 0,
                    Minutes = 0
                }
            };
            dynamic jsonObject = GetDynamicJsonObject(controls);
            jsonObject.shapes[0].category = CategoryWait;
            SetDynamicProperties(jsonObject);

            // This is a BUG
            // Line no : 1523
            // Directly assigns dynamic properties which has value of type JValue to Timespan struct.
            // The 'Wait' class properties should be used for it as they are marked with correct Json attributes for conversion
            // Act and Assert
           Should.Throw<RuntimeBinderException>(() => ControlBase.Deserialize(jsonObject));
        }

        private dynamic GetDynamicJsonObject(List<ControlBase> controls)
        {
            var json = ControlBase.Serialize(controls, new List<Connector> { new Connector() }, MAID: 1);
            dynamic jsonObject = JsonConvert.DeserializeObject(json);
            return jsonObject;
        }

        private void SetDynamicProperties(dynamic jsonObject)
        {
            jsonObject.shapes[0].campaign_item_nameID = "1";
            jsonObject.shapes[0].campaign_item = "SampleCampaignItem";
            jsonObject.shapes[0].campaign_item_name = "SampleCampaign";
            jsonObject.shapes[0].customerID = "1";
            jsonObject.shapes[0].isDirty = true;
            jsonObject.shapes[0].ma_controlID = "1";
            jsonObject.shapes[0].campaign_itemID = "1";
        }
    }
}
