using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
using System.Transactions.Fakes;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;
using KMPlatform.Entity;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodSaveChildren = "SaveChildren";
        private const string ParentId = "ParentId";
        private const string AllControlsProperty = "AllControls";
        private const string AllConnectorsProperty = "AllConnectors";

        [Test]
        public void SaveChildren_OnEmptyList_ReachEnd()
        {
            // Arrange, Act, Assert
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                null, new List<ControlBase>(), 0
            });
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem, 1, false, 1, true)]
        [TestCase(MarketingAutomationControlType.CampaignItem, 1, true, 1, true)]
        [TestCase(MarketingAutomationControlType.CampaignItem, 1, true, 1, false)]
        [TestCase(MarketingAutomationControlType.CampaignItem, 0, true, 1, false)]
        [TestCase(MarketingAutomationControlType.CampaignItem, 0, true, 1, true)]
        public void SaveChildren_CampaignItem_ReachEnd(
            MarketingAutomationControlType controlType,
            int mAControlID,
            bool remove,
            int campaignItemID,
            bool createCampaignItem)
        {
            // Arrange
            SetValuesForProperties();
            var parent = new Start
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new CampaignItem
                {
                    CampaignItemID = campaignItemID,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = remove
                    },
                    MAControlID = mAControlID,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true,
                    CreateCampaignItem = createCampaignItem,
                    SubCategory = "Group",
                    ControlID = DummyString
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(campaignItemID);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            // Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        public void SaveChildren_ClickOnException_ThrowException()
        {
            // Arrange
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>());
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>());
            var parent = new Start
            {
                ControlID = DummyString,
                ControlType = MarketingAutomationControlType.Click
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new Click
                {
                    ECNID = 1,
                    ControlType = MarketingAutomationControlType.Click,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = 1,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(1);

            // Act, Assert
            var exception = Should.Throw<Exception>(
                () => _privateObject.Invoke(MethodSaveChildren, new object[]
                {
                    parent, controlBase, 1
                })
            ).InnerException;
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ECNException));
            childrenSaved.ShouldBeFalse();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Click, 1, false, 1, true)]
        [TestCase(MarketingAutomationControlType.Click, 0, false, 1, true)]
        [TestCase(MarketingAutomationControlType.Click, 0, false, 0, true)]
        public void SaveChildren_Click_ReachEnd(
            MarketingAutomationControlType controlType,
            int mAControlID,
            bool remove,
            int ecnId,
            bool createCampaignItem)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Wait
            {
                ControlID = ParentId,
                ControlType = MarketingAutomationControlType.Wait
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new Click
                {
                    ECNID = ecnId,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = remove
                    },
                    MAControlID = mAControlID,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Direct_Click, 1)]
        [TestCase(MarketingAutomationControlType.Direct_Click, 0)]
        public void SaveChildren_Direct_Click_ReachEnd(MarketingAutomationControlType controlType, int ecnId)
        {
            // Arrange 
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>
            {
                new Wait
                {
                    ControlID = ParentId
                }
            });
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>
            {
                new Connector
                {
                    from = new from
                    {
                        shapeId = ParentId,
                    },
                    to = new to
                    {
                        shapeId = DummyString
                    },
                    id = DummyString
                }
            });
            var parent = new Wait
            {
                ControlID = ParentId,
                ControlType = MarketingAutomationControlType.Wait
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new Direct_Click
                {
                    ECNID = ecnId,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = 1,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void SaveChildren_FormAndSimilarCases_ReachEnd(int ecnId)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Wait
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                GetFormObject(ecnId),
                GetSubscribeObject(ecnId),
                GetUnsubscribeObject(ecnId)
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.FormSubmit, 1, true)]
        [TestCase(MarketingAutomationControlType.FormSubmit, 0, false)]
        public void SaveChildren_FormSubmit_ReachEnd(MarketingAutomationControlType controlType, int ecnId, bool formParentIsNull)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Wait
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new FormSubmit
                {
                    ECNID = ecnId,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = 1,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (obj, ctrl) =>
            {
                return formParentIsNull
                    ? null
                    : new Wait()
                    {
                        ControlID = DummyString
                    };
            };

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.FormAbandon, 1, true)]
        [TestCase(MarketingAutomationControlType.FormAbandon, 0, false)]
        public void SaveChildren_FormAbandon_ReachEnd(MarketingAutomationControlType controlType, int ecnId, bool formParentIsNull)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Wait
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new FormAbandon
                {
                    ECNID = ecnId,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = 1,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (obj, ctrl) =>
            {
                return formParentIsNull
                    ? null
                    : new Wait()
                    {
                        ControlID = DummyString
                    };
            };

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Direct_Open, 1)]
        [TestCase(MarketingAutomationControlType.Direct_Open, 0)]
        public void SaveChildren_Direct_Open_ReachEnd(MarketingAutomationControlType controlType, int ecnId)
        {
            // Arrange 
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>
            {
                new Wait
                {
                    ControlID = ParentId
                }
            });
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>
            {
                new Connector
                {
                    from = new from
                    {
                        shapeId = ParentId,
                    },
                    to = new to
                    {
                        shapeId = DummyString
                    },
                    id = DummyString
                }
            });
            var parent = new Wait
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new Direct_Open
                {
                    ECNID = ecnId,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = 1,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen, 1)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen, 0)]
        public void SaveChildren_Direct_NoOpen_ReachEnd(MarketingAutomationControlType controlType, int ecnId)
        {
            // Arrange 
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>
            {
                new Direct_Click
                {
                    ControlID = ParentId
                }
            });
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>
            {
                new Connector
                {
                    from = new from
                    {
                        shapeId = ParentId,
                    },
                    to = new to
                    {
                        shapeId = DummyString
                    },
                    id = DummyString
                }
            });
            var parent = new Direct_Click
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new Direct_NoOpen
                {
                    ECNID = ecnId,
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = 1,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (obj, ctrl) =>
            {
                return new Wait()
                {
                    ControlID = DummyString
                };
            };

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.End, 1)]
        [TestCase(MarketingAutomationControlType.End, 0)]
        public void SaveChildren_End_ReachEnd(MarketingAutomationControlType controlType, int mAControlID)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Direct_Click
            {
                ControlID = ParentId
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new End
                {
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = false
                    },
                    MAControlID = mAControlID,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(mAControlID);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Group, 1, false)]
        [TestCase(MarketingAutomationControlType.Group, 1, true)]
        [TestCase(MarketingAutomationControlType.Group, 0, true)]
        public void SaveChildren_Group_ReachEnd(
            MarketingAutomationControlType controlType,
            int mAControlID,
            bool remove)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Wait
            {
                ControlID = ParentId,
                ControlType = MarketingAutomationControlType.Wait
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                new Group
                {
                    ControlType = controlType,
                    editable =  new shapeEditable
                    {
                        remove = remove
                    },
                    MAControlID = mAControlID,
                    ControlID = DummyString,
                    xPosition = 1,
                    yPosition = 1,
                    IsDirty = true
                }
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(mAControlID);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        [TestCase(1, false, 1)]
        [TestCase(1, true, 1)]
        [TestCase(0, true, 0)]
        [TestCase(1, false, 1)]
        [TestCase(1, true, 1)]
        [TestCase(0, true, 0)]
        public void SaveChildren_MultipleCases_ReachEnd(int mAControlID, bool remove, int ecnId)
        {
            // Arrange 
            SetValuesForProperties();
            var parent = new Wait
            {
                ControlID = ParentId,
                ControlType = MarketingAutomationControlType.Wait
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                GetNoClickObject(mAControlID, remove, ecnId),
                GetNoOpenObject(mAControlID, remove, ecnId),
                GetNotSentObject(mAControlID, remove, ecnId),
                GetOpenObject(mAControlID, remove, ecnId),
                GetOpen_NoClickObject(mAControlID, remove, ecnId),
                GetSentObject(mAControlID, remove, ecnId),
                GetSuppressedObject(mAControlID, remove, ecnId),
                GetWaitObject(mAControlID, remove, ecnId)
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(ecnId);

            // Act
            _privateObject.Invoke(MethodSaveChildren, new object[]
            {
                parent, controlBase, 1
            });

            //Assert
            childrenSaved.ShouldBeTrue();
        }

        [Test]
        public void SaveChildren_MultipleCasesOnException_ThrowException()
        {
            // Arrange, Act, Assert
            AssertForException(GetNoClickObject(1, true, 1));
            AssertForException(GetNoOpenObject(1, true, 1));
            AssertForException(GetNotSentObject(1, true, 1));
            AssertForException(GetOpenObject(1, true, 1));
            AssertForException(GetOpen_NoClickObject(1, true, 1));
            AssertForException(GetSentObject(1, true, 1));
            AssertForException(GetSuppressedObject(1, true, 1));
        }

        private void AssertForException(ControlBase objectToAssert)
        {
            // Arrange
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>());
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>());
            var parent = new Start
            {
                ControlID = DummyString,
                ControlType = MarketingAutomationControlType.NoClick
            };
            var childrenSaved = false;
            var controlBase = new List<ControlBase>
            {
                objectToAssert
            };
            ShimMAControl.SaveMAControl = (ctrl) =>
            {
                childrenSaved = true;
                return 1;
            };
            SetShimsForSaveChildren(1);

            // Act, Assert
            var exception = Should.Throw<Exception>(
                () => _privateObject.Invoke(MethodSaveChildren, new object[]
                {
                    parent, controlBase, 1
                })
            ).InnerException;
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ECNException));
            childrenSaved.ShouldBeFalse();
        }

        private NoOpen GetNoOpenObject(int mAControlID, bool remove, int ecnId)
        {
            return new NoOpen
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.NoOpen,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private NoClick GetNoClickObject(int mAControlID, bool remove, int ecnId)
        {
            return new NoClick
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.NoClick,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private NotSent GetNotSentObject(int mAControlID, bool remove, int ecnId)
        {
            return new NotSent
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.NotSent,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Open GetOpenObject(int mAControlID, bool remove, int ecnId)
        {
            return new Open
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Open,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Open_NoClick GetOpen_NoClickObject(int mAControlID, bool remove, int ecnId)
        {
            return new Open_NoClick
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Open_NoClick,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Sent GetSentObject(int mAControlID, bool remove, int ecnId)
        {
            return new Sent
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Sent,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Form GetFormObject(int ecnId)
        {
            return new Form
            {
                ECNID = ecnId,
                FormID = ecnId,
                ControlType = MarketingAutomationControlType.Form,
                editable = new shapeEditable
                {
                    remove = false
                },
                MAControlID = 1,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Subscribe GetSubscribeObject(int ecnId)
        {
            return new Subscribe
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Subscribe,
                editable = new shapeEditable
                {
                    remove = false
                },
                MAControlID = 1,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Suppressed GetSuppressedObject(int mAControlID, bool remove, int ecnId)
        {
            return new Suppressed
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Suppressed,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Unsubscribe GetUnsubscribeObject(int ecnId)
        {
            return new Unsubscribe
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Unsubscribe,
                editable = new shapeEditable
                {
                    remove = false
                },
                MAControlID = 1,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private Wait GetWaitObject(int mAControlID, bool remove, int ecnId)
        {
            return new Wait
            {
                ECNID = ecnId,
                ControlType = MarketingAutomationControlType.Wait,
                editable = new shapeEditable
                {
                    remove = remove
                },
                MAControlID = mAControlID,
                ControlID = DummyString,
                xPosition = 1,
                yPosition = 1,
                IsDirty = true
            };
        }

        private void SetValuesForProperties()
        {
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>());
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>
            {
                new Connector
                {
                    from = new from
                    {
                        shapeId = ParentId,
                    },
                    to = new to
                    {
                        shapeId = DummyString
                    },
                    id = DummyString
                }
            });
        }

        private void SetShimsForSaveChildren(int campaignItemID)
        {
            ShimMAControl.GetByControlIDStringInt32 = (cid, id) => new Entities::MAControl();
            ShimAutomationBaseController.AllInstances.SaveCampaignItemCampaignItemBoolean = 
                (obj, item, obj1) => campaignItemID;
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (obj, ciObj) => new Wait
            {
                WaitTime = 10,
                ControlID = DummyString
            };
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (obj, ciObj) => new CampaignItem
            {
                SendTime = DateTime.Now
            };
            ShimCampaignItem.UpdateSendTimeInt32DateTime = (id, time) => { };
            ShimMAConnector.SaveMAConnector = (obj) => 1;
            ShimAutomationBaseController.AllInstances.FindFormParentControlBase = (obj, cbase) => null;
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (obj, cbase) => null;
            ShimAutomationBaseController.AllInstances.SaveFormControlTriggerControlBaseControlBaseControlBaseControlBase =
                (obj, frmObj, frmWait, frmParent, prnt) => 1;
            ShimAutomationBaseController.AllInstances.SaveMessageTriggerControlBaseControlBaseControlBase =
                (obj, trigger, parenetWait, parentCI) => 1;
            ShimAutomationBaseController.AllInstances.SaveSmartSegmentEmail_ClickControlBaseControlBaseBooleanBoolean 
                = (controller, arg2, arg3, arg4, arg5) => 1;
            ShimAutomationBaseController.AllInstances.SaveTriggerPlanControlBaseControlBaseControlBaseBoolean =
                (obj, trigger, parenetWait, parentCI, ishome) => 1;
            ShimAutomationBaseController.AllInstances.SaveGroupTriggerControlBaseControlBase =
                (obj, ctrl, prnt) => 1;
        }
    }
}