using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.communicator.main.ECNWizard.Controls;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using Entity = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class WizardContentChampionTest
    {
        private const string AbSampleBlast = "AbSampleBlast";
        private const string LblGroup = "lblGroup";
        private const string LblFilter = "lblFilter";
        private const string LblSuppressionGroups = "lblSuppressionGroups";
        private const string MethodLoadSample = "LoadSample";
        private const string GvFilters = "gvFilters";
        private const string LblAbWinnerType = "lblABWinnerType";
        private const string LblSuppGroupFilters = "lblSuppGroupFilters";
        private const string RptrEnvelope = "rptrEnvelope";
        private const string RptrSample = "rptrSample";
        private const string ChkAorB = "chkAorB";
        private const string ChkLosingCampaign = "chkLosingCampaign";
        private const string RblLosingAction = "rblLosingAction";
        private const string BlastIDColumn = "BlastID";
        private const string GroupIDColumn = "GroupID";
        private const string GroupNameColumn = "GroupName";
        private const string ABWinnerTypeColumn = "ABWinnerType";
        private const string BlastSuppressionColumn = "BlastSuppression";
        private const string EmailFromColumn = "EmailFrom";
        private const string EmailFromNameColumn = "EmailFromName";
        private const string ReplyToColumn = "ReplyTo";

        private WizardContent_Champion _control;
        private ShimECNSession _shimEcnSession;
        private PrivateObject _privateControlObj;
        private readonly NameValueCollection _appSettings = new NameValueCollection();

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _shimEcnSession = new ShimECNSession();
            var fieldCurrentUser = typeof(ECNSession).GetField("CurrentUser");
            var currUser = new User
            {
                CustomerID = 1
            };
            fieldCurrentUser.SetValue(_shimEcnSession.Instance, currUser);
            ShimECNSession.CurrentSession = () => _shimEcnSession.Instance;
            _control = new WizardContent_Champion();
            InitializeAllControls(_control);
            _privateControlObj = new PrivateObject(_control);
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        [TearDown]
        public new void CleanUp()
        {
            base.CleanUp();
            _appSettings.Clear();
        }

        [Test]
        public void LoadSample_GetSampleInfoReturnEmptyTable_EmptyLabels()
        {
            // Arrange
            InitializePageAndControls();
            ShimBlast.GetSampleInfoInt32Int32User = (custId, sId, usr) => new DataTable();

            // Act	
            ReflectionHelper.ExecuteMethod(_control, MethodLoadSample, new object[] { true });

            // Assert
            var lblGroup = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblGroup).GetValue(_control) as Label;
            lblGroup.ShouldNotBeNull();
            lblGroup.Text.ShouldBeNullOrEmpty();
            var lblFilter = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblFilter).GetValue(_control) as Label;
            lblFilter.ShouldNotBeNull();
            lblFilter.Text.ShouldBeNullOrEmpty();
            var lblSuppressionGroups = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblSuppressionGroups).GetValue(_control) as Label;
            lblSuppressionGroups.ShouldNotBeNull();
            lblSuppressionGroups.Text.ShouldBeNullOrEmpty();
        }

        [Test]
        public void LoadSample_CIBFiltersIsEmpty_FillRepeater()
        {
            // Arrange
            InitializePageAndControls();
            SetupFakesIfEmptyCibFilters();

            // Act	
            ReflectionHelper.ExecuteMethod(_control, MethodLoadSample, new object[] { true });

            // Assert
            var lblGroup = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblGroup).GetValue(_control) as Label;
            lblGroup.ShouldNotBeNull();
            lblGroup.Text.ShouldNotBeNullOrEmpty();
            var lblFilter = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblFilter).GetValue(_control) as Label;
            lblFilter.ShouldNotBeNull();
            lblFilter.Text.ShouldBeNullOrEmpty();
            var lblSuppressionGroups = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblSuppressionGroups).GetValue(_control) as Label;
            lblSuppressionGroups.ShouldNotBeNull();
            lblSuppressionGroups.Text.ShouldNotBeNullOrEmpty();
            var gvFilters = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, GvFilters).GetValue(_control) as GridView;
            gvFilters.ShouldNotBeNull();
            gvFilters.Visible.ShouldBeFalse();
            var lblAbWinnerType = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblAbWinnerType).GetValue(_control) as Label;
            lblAbWinnerType.ShouldNotBeNull();
            lblAbWinnerType.Text.ShouldNotBeNullOrEmpty();
            var rptrSample = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, RptrSample).GetValue(_control) as Repeater;
            rptrSample.ShouldNotBeNull();
            rptrSample.DataSource.ShouldNotBeNull();
        }

        [Test]
        public void LoadSample_CIBFiltersIsNotEmpty_FillRepeater()
        {
            // Arrange
            InitializePageAndControls();
            var fieldCurrentCustomer = typeof(ECNSession).GetField("CurrentCustomer");
            var currCustomer = new Customer
            {
                ABWinnerType = "TestString"
            };
            fieldCurrentCustomer.SetValue(_shimEcnSession.Instance, currCustomer);
            SetupFakesIfCibFiltersNotEmpty();

            // Act	
            ReflectionHelper.ExecuteMethod(_control, MethodLoadSample, new object[] { true });

            // Assert
            var lblGroup = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblGroup).GetValue(_control) as Label;
            lblGroup.ShouldNotBeNull();
            lblGroup.Text.ShouldNotBeNullOrEmpty();
            var lblFilter = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblFilter).GetValue(_control) as Label;
            lblFilter.ShouldNotBeNull();
            lblFilter.Text.ShouldBeNullOrEmpty();
            var lblSuppressionGroups = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblSuppressionGroups).GetValue(_control) as Label;
            lblSuppressionGroups.ShouldNotBeNull();
            lblSuppressionGroups.Text.ShouldNotBeNullOrEmpty();
            var gvFilters = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, GvFilters).GetValue(_control) as GridView;
            gvFilters.ShouldNotBeNull();
            gvFilters.Visible.ShouldBeTrue();
            var lblAbWinnerType = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblAbWinnerType).GetValue(_control) as Label;
            lblAbWinnerType.ShouldNotBeNull();
            lblAbWinnerType.Text.ShouldNotBeNullOrEmpty();
            var rptrSample = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, RptrSample).GetValue(_control) as Repeater;
            rptrSample.ShouldNotBeNull();
            rptrSample.DataSource.ShouldNotBeNull();
        }

        [Test]
        [TestCase("")]
        [TestCase("TestString")]
        public void LoadSample_ABWinnerTypeISEmpty_FillRepeater(string deliveredOrOpened)
        {
            // Arrange
            InitializePageAndControls();
            var fieldCurrentCustomer = typeof(ECNSession).GetField("CurrentCustomer");
            var currCustomer = new Customer
            {
                ABWinnerType = null
            };
            _appSettings.Add("KMWinnerTypeDefault", "TestType");
            fieldCurrentCustomer.SetValue(_shimEcnSession.Instance, currCustomer);
            SetupFakesIfEmptyWinnerType(deliveredOrOpened);

            // Act	
            ReflectionHelper.ExecuteMethod(_control, MethodLoadSample, new object[] { true });

            // Assert
            var lblGroup = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblGroup).GetValue(_control) as Label;
            lblGroup.ShouldNotBeNull();
            lblGroup.Text.ShouldNotBeNullOrEmpty();
            var lblFilter = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblFilter).GetValue(_control) as Label;
            lblFilter.ShouldNotBeNull();
            lblFilter.Text.ShouldBeNullOrEmpty();
            var lblSuppressionGroups = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblSuppressionGroups).GetValue(_control) as Label;
            lblSuppressionGroups.ShouldNotBeNull();
            lblSuppressionGroups.Text.ShouldNotBeNullOrEmpty();
            var gvFilters = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, GvFilters).GetValue(_control) as GridView;
            gvFilters.ShouldNotBeNull();
            gvFilters.Visible.ShouldBeTrue();
            var lblAbWinnerType = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, LblAbWinnerType).GetValue(_control) as Label;
            lblAbWinnerType.ShouldNotBeNull();
            lblAbWinnerType.Text.ShouldNotBeNullOrEmpty();
            var rptrSample = ReflectionHelper.GetFieldInfoFromInstanceByName(_control, RptrSample).GetValue(_control) as Repeater;
            rptrSample.ShouldNotBeNull();
            rptrSample.DataSource.ShouldNotBeNull();
        }

        private static void SetupFakesIfEmptyCibFilters()
        {
            ShimBlast.GetSampleInfoInt32Int32User = (custId, sId, usr) =>
            {
                var dt = new DataTable();
                dt.Columns.Add("BlastID", typeof(int));
                dt.Columns.Add("GroupID", typeof(int));
                dt.Columns.Add("GroupName", typeof(string));
                dt.Columns.Add("ABWinnerType", typeof(string));
                dt.Columns.Add("BlastSuppression", typeof(string));
                var row = dt.NewRow();
                row[0] = 1;
                row[1] = 1;
                row[2] = "TestGroupName";
                row[3] = "TestABWinnerType";
                row[4] = "1,2";
                dt.Rows.Add(row);
                return dt;
            };
            ShimCampaignItemBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) => new CampaignItemBlast
            {
                Filters = new List<CampaignItemBlastFilter>(),
                CampaignItemID = 1
            };

            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) => new CampaignItem
            {
                SuppressionList = new List<CampaignItemSuppression>()
                {
                    new CampaignItemSuppression
                    {
                        GroupID = 1,
                        Filters = new List<CampaignItemBlastFilter>
                        {
                            new CampaignItemBlastFilter
                            {
                                FilterID = 0,
                                SmartSegmentID = 1,
                                RefBlastIDs = "1,2"
                            }
                        }
                    }
                }
            };

            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new Entity.Group
            {
                GroupName = "TestString"
            };

            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new Filter
            {
                FilterName = "TestFilter"
            };

            ShimSmartSegment.GetSmartSegmentByIDInt32 = (id) => new SmartSegment
            {
                SmartSegmentName = "TestName"
            };

            ShimBlastActivity.ChampionByProcInt32BooleanUserString = (sId, justWin, usr, type) => new DataTable();
        }

        private static void SetupFakesIfCibFiltersNotEmpty()
        {
            ShimBlast.GetSampleInfoInt32Int32User = (custId, sId, usr) =>
            {
                var dt = new DataTable();
                dt.Columns.Add("BlastID", typeof(int));
                dt.Columns.Add("GroupID", typeof(int));
                dt.Columns.Add("GroupName", typeof(string));
                dt.Columns.Add("ABWinnerType", typeof(string));
                dt.Columns.Add("BlastSuppression", typeof(string));
                var row = dt.NewRow();
                row[0] = 1;
                row[1] = 1;
                row[2] = "TestGroupName";
                row[3] = string.Empty;
                row[4] = "1,2";
                dt.Rows.Add(row);
                return dt;
            };

            ShimCampaignItemBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) => new CampaignItemBlast
            {
                Filters = new List<CampaignItemBlastFilter>
                {
                    new CampaignItemBlastFilter
                    {
                        CampaignItemBlastFilterID = 1,
                        CampaignItemSuppresionID = null,
                        CampaignItemBlastID = 1,
                        IsDeleted = false
                    }
                },
                CampaignItemID = 1
            };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) => new CampaignItem
            {
                SuppressionList = new List<CampaignItemSuppression>()
                {
                    new CampaignItemSuppression
                    {
                        GroupID = 1,
                        Filters = new List<CampaignItemBlastFilter>
                        {
                            new CampaignItemBlastFilter
                            {
                                FilterID = 1,
                                SmartSegmentID = 1,
                                RefBlastIDs = "1,2"
                            }
                        }
                    }
                }
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new Entity.Group
            {
                GroupName = "TestString"
            };

            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new Filter
            {
                FilterName = "TestFilter"
            };

            ShimSmartSegment.GetSmartSegmentByIDInt32 = (id) => new SmartSegment
            {
                SmartSegmentName = "TestName"
            };

            ShimBlastActivity.ChampionByProcInt32BooleanUserString = (sId, justWin, usr, type) => new DataTable();
        }

        private static void SetupFakesIfEmptyWinnerType(string deliveredOrOpened)
        {
            ShimBlast.GetSampleInfoInt32Int32User = (custId, sId, usr) =>
            {
                var dt = new DataTable();
                dt.Columns.Add(BlastIDColumn, typeof(int));
                dt.Columns.Add(GroupIDColumn, typeof(int));
                dt.Columns.Add(GroupNameColumn, typeof(string));
                dt.Columns.Add(ABWinnerTypeColumn, typeof(string));
                dt.Columns.Add(BlastSuppressionColumn, typeof(string));
                dt.Columns.Add(EmailFromColumn, typeof(string));
                dt.Columns.Add(EmailFromNameColumn, typeof(string));
                dt.Columns.Add(ReplyToColumn, typeof(string));
                var row = dt.NewRow();
                row[0] = 1;
                row[1] = 1;
                row[2] = "TestGroupName";
                row[3] = string.Empty;
                row[4] = "1,2";
                row[EmailFromColumn] = SampleEmail;
                row[EmailFromNameColumn] = SampleEmailFromName;
                row[ReplyToColumn] = SampleEmail;
                dt.Rows.Add(row);
                return dt;
            };

            ShimCampaignItemBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) => new CampaignItemBlast
            {
                Filters = new List<CampaignItemBlastFilter>
                {
                    new CampaignItemBlastFilter
                    {
                        CampaignItemBlastFilterID = 1,
                        CampaignItemSuppresionID = null,
                        CampaignItemBlastID = 1,
                        IsDeleted = false
                    }
                },
                CampaignItemID = 1
            };

            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) => new CampaignItem
            {
                SuppressionList = new List<CampaignItemSuppression>()
                {
                    new CampaignItemSuppression
                    {
                        GroupID = 1,
                        Filters = new List<CampaignItemBlastFilter>
                        {
                            new CampaignItemBlastFilter
                            {
                                FilterID = 1,
                                SmartSegmentID = 1,
                                RefBlastIDs = "1,2"
                            }
                        }
                    }
                },
                SampleID = 1
            };

            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new Entity.Group
            {
                GroupName = "TestString"
            };

            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new Filter
            {
                FilterName = "TestFilter"
            };

            ShimSmartSegment.GetSmartSegmentByIDInt32 = (id) => new SmartSegment
            {
                SmartSegmentName = "TestName"
            };

            ShimBlastActivity.ChampionByProcInt32BooleanUserString = (sId, justWin, usr, type) => new DataTable();
            ShimSample.GetBySampleIDInt32User = (id, usr) => new Sample
            {
                DeliveredOrOpened = deliveredOrOpened
            };
        }

        private void InitializePageAndControls()
        {
            var ddlAbSampleBlast = new DropDownList();
            ddlAbSampleBlast.Items.Add(new ListItem("TestString", "1"));
            ddlAbSampleBlast.SelectedIndex = 0;

            ReflectionHelper.SetValue(_control, AbSampleBlast, ddlAbSampleBlast);
            ReflectionHelper.SetValue(_control, LblGroup, new Label());
            ReflectionHelper.SetValue(_control, LblFilter, new Label());
            ReflectionHelper.SetValue(_control, LblSuppressionGroups, new Label());
            ReflectionHelper.SetValue(_control, LblAbWinnerType, new Label());
            ReflectionHelper.SetValue(_control, LblSuppGroupFilters, new Label());
            ReflectionHelper.SetValue(_control, GvFilters, new GridView());
            ReflectionHelper.SetValue(_control, RptrEnvelope, new Repeater());
            ReflectionHelper.SetValue(_control, RptrSample, new Repeater());
            ReflectionHelper.SetValue(_control, ChkAorB, new CheckBox());
            ReflectionHelper.SetValue(_control, ChkLosingCampaign, new CheckBox());
            ReflectionHelper.SetValue(_control, RblLosingAction, new RadioButtonList());
        }
    }
}
