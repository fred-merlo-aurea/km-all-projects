using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using KM.Platform.Fakes;
using NUnit.Framework;
using Shouldly;
using EntityFake = KM.Common.Entity.Fakes;

namespace ECN.Communicator.Tests.Main.Blasts
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ManageCampaignsTests : BaseBlastsTest<ManageCampaigns>
    {
        private TextBox _txtGoToPage;
        private LinkButton _lbDeleteCampaign;

        [TearDown]
        public void ManageCampaignsTestsTearDown()
        {
            _txtGoToPage?.Dispose();
            _lbDeleteCampaign?.Dispose();
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
        }

        [Test]
        public void Page_Load_Test()
        {
            //Arrange
            InitilizeTestObjects();

            var loadGrid = false;

            ShimManageCampaigns.AllInstances.LoadGrid = (instance) => { loadGrid = true; };

            //Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            //Assert
            loadGrid.ShouldBeTrue();
        }

        [Test]
        public void GvCampaigns_RowDataBound_TypePager()
        {
            //Arrange
            InitilizeTestObjects();

            var row = GetGridViewRow(DataControlRowType.Pager);
            var arg = new GridViewRowEventArgs(row);

            //Act
            privateObject.Invoke("gvCampaigns_RowDataBound", new object[] { null, arg });

            // Assert
            _txtGoToPage.Text.ShouldBe("1");
        }

        [Test]
        public void GvCampaigns_RowDataBound_TypeDataRow()
        {
            //Arrange
            InitilizeTestObjects();

            var gvCampaigns = privateObject.GetFieldOrProperty("gvCampaigns") as GridView;
            gvCampaigns.DataKeyNames = new string[] { "id" };
            gvCampaigns.DataSource = new DataTable { Columns = { "id" }, Rows = { { "1" } } };
            gvCampaigns.DataBind();

            var row = GetGridViewRow(DataControlRowType.DataRow);
            var arg = new GridViewRowEventArgs(row);

            //Act
            privateObject.Invoke("gvCampaigns_RowDataBound", new object[] { null, arg });

            // Assert
            _lbDeleteCampaign.CommandArgument.ShouldBe("1");
        }

        [Test]
        public void GvCampaigns_RowCommand_Delete_CannotDeleteException()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("deletecampaign", "1"));
            var errorMessage = "";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCIB = new List<CampaignItemBlast>();
                var blastItem = new CampaignItemBlast
                {
                    CampaignItemID = p1,
                    BlastID = p1
                };

                var blast = new BlastAB();
                blastItem.Blast = blast;

                listCIB.Add(blastItem);

                return listCIB;
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            errorMessage.ShouldBe("Cannot delete Campaign because it contains Campaign Items");
        }

        [Test]
        public void GvCampaigns_RowCommand_Delete_Successful()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("deletecampaign", "1"));
            var isGridLoaded = false;

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) => { };

            ShimCampaign.DeleteInt32User = (p1, p2) =>
            {
                isGridLoaded = true;
            };

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCIB = new List<CampaignItemBlast>();

                return listCIB;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            isGridLoaded.ShouldBeTrue();
        }

        [Test]
        public void GvCampaigns_RowCommand_Delete_ECNException()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("deletecampaign", "1"));
            var errorMessage = "";

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            };

            ShimManageCampaigns.AllInstances.setECNErrorECNException = (p1, p2) =>
            {
                errorMessage = p2.ErrorList[0].ErrorMessage;
            };

            ShimCampaign.DeleteInt32User = (p1, p2) => { };

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCIB = new List<CampaignItemBlast>();

                return listCIB;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            errorMessage.ShouldBe("Test Exception");
        }

        [Test]
        public void GvCampaigns_RowCommand_Delete_PreviouslyDeleted()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("deletecampaign", "1"));

            var isGridLoaded = false;

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) => { };

            ShimCampaign.DeleteInt32User = (p1, p2) =>
            {
                isGridLoaded = true;
            };

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCIB = new List<CampaignItemBlast>();

                return listCIB;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            isGridLoaded.ShouldBeTrue();
        }

        [Test]
        public void GvCampaigns_RowCommand_Delete_PreviouslyDeleted_ECNException()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("deletecampaign", "1"));

            var errorMessage = "";

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            };

            ShimManageCampaigns.AllInstances.setECNErrorECNException = (p1, p2) =>
            {
                errorMessage = p2.ErrorList[0].ErrorMessage;
            };

            ShimCampaign.DeleteInt32User = (p1, p2) => { };

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCIB = new List<CampaignItemBlast>();

                return listCIB;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            errorMessage.ShouldBe("Test Exception");
        }

        [Test]
        public void GvCampaigns_RowCommand_Delete_NoPermission()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("deletecampaign", "1"));
            var errorMessage = "";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return false;
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            errorMessage.ShouldBe("You do not have permission to delete campaigns");
        }

        [Test]
        public void GvCampaigns_RowCommand_EditCampaign()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("editcampaign", "1"));
            var btnSaveCampaign = privateObject.GetFieldOrProperty("btnSaveCampaign") as Button;

            ShimCampaign.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var campaign = new Campaign();
                campaign.CampaignID = p1;
                campaign.CampaignName = "TEST_Campaign";

                return campaign;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            btnSaveCampaign.CommandArgument.ShouldBe("1");
        }

        [Test]
        public void GvCampaigns_RowCommand_MoveCampaign()
        {
            //Arrange
            InitilizeTestObjects();

            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("move", "2"));
            var btnMoveCampaign = privateObject.GetFieldOrProperty("btnMoveCampaign") as Button;

            ShimCampaign.GetByCustomerIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCampaigns = new List<Campaign>();

                var campaign = new Campaign();
                campaign.CampaignID = p1;
                campaign.CampaignName = "TEST_Campaign";

                listCampaigns.Add(campaign);

                return listCampaigns;
            };

            // Act
            privateObject.Invoke("gvCampaigns_RowCommand", new object[] { null, eventArgs });

            //Assert
            btnMoveCampaign.CommandArgument.ShouldBe("2");
        }

        [Test]
        public void BtnDeleteConfirm_Click_CannotDeleteException()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnDeleteConfirm = privateObject.GetFieldOrProperty("btnDeleteConfirm") as Button;
            btnDeleteConfirm.CommandArgument = "1";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCIB = new List<CampaignItemBlast>();
                var blastItem = new CampaignItemBlast
                {
                    CampaignItemID = p1,
                    BlastID = p1
                };

                var blast = new BlastAB();
                blastItem.Blast = blast;

                listCIB.Add(blastItem);

                return listCIB;
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("Cannot delete Campaign because it contains Campaign Items");
        }

        [Test]
        public void BtnDeleteConfirm_Click_CampaignItemTestBlast_ExceptioCannotDeleteException()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnDeleteConfirm = privateObject.GetFieldOrProperty("btnDeleteConfirm") as Button;
            btnDeleteConfirm.CommandArgument = "1";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemBlast>();
            };

            ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCITB = new List<CampaignItemTestBlast>();
                var blast = new BlastAB();
                var item = new CampaignItemTestBlast
                {
                    BlastID = p1,
                    Blast = blast
                };

                listCITB.Add(item);

                return listCITB;
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("Cannot delete Campaign because it contains Campaign Items");
        }

        [Test]
        public void BtnDeleteConfirm_Click_Success()
        {
            //Arrange
            InitilizeTestObjects();

            var loadGrid = false;

            var btnDeleteConfirm = privateObject.GetFieldOrProperty("btnDeleteConfirm") as Button;
            btnDeleteConfirm.CommandArgument = "1";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemBlast>();
            };

            ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemTestBlast>(); ;
            };

            ShimCampaign.DeleteInt32User = (p1, p2) => { };
            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                loadGrid = true;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            loadGrid.ShouldBeTrue();
        }

        [Test]
        public void BtnDeleteConfirm_Click_Exception()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnDeleteConfirm = privateObject.GetFieldOrProperty("btnDeleteConfirm") as Button;
            btnDeleteConfirm.CommandArgument = "1";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = false,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemBlast>();
            };

            ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemTestBlast>(); ;
            };

            ShimCampaign.DeleteInt32User = (p1, p2) => { };
            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            };

            ShimManageCampaigns.AllInstances.setECNErrorECNException = (p1, p2) =>
            {
                errorMessage = p2.ErrorList[0].ErrorMessage;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("Test Exception");
        }

        [Test]
        public void BtnDeleteConfirm_Click_DeletedItemsExist_ExceptionOnLoadGrid()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnDeleteConfirm = privateObject.GetFieldOrProperty("btnDeleteConfirm") as Button;
            btnDeleteConfirm.CommandArgument = "1";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemBlast>();
            };

            ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemTestBlast>(); ;
            };

            ShimCampaign.DeleteInt32User = (p1, p2) => { };
            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            };

            ShimManageCampaigns.AllInstances.setECNErrorECNException = (p1, p2) =>
            {
                errorMessage = p2.ErrorList[0].ErrorMessage;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("Test Exception");
        }

        [Test]
        public void BtnDeleteConfirm_Click_DeletedItemsExist_DeleteSuccess()
        {
            //Arrange
            InitilizeTestObjects();

            var loadGrid = false;

            var btnDeleteConfirm = privateObject.GetFieldOrProperty("btnDeleteConfirm") as Button;
            btnDeleteConfirm.CommandArgument = "1";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return true;
            };

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemBlast>();
            };

            ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItemTestBlast>(); ;
            };

            ShimCampaign.DeleteInt32User = (p1, p2) => { };

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                loadGrid = true;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            loadGrid.ShouldBeTrue();
        }

        [Test]
        public void BtnDeleteConfirm_Click_No_Permission()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            ShimUser.IsChannelAdministratorUser = (p1) =>
            {
                return false;
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            // Act
            privateObject.Invoke("btnDeleteConfirm_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("You do not have permission to delete campaigns");
        }

        [Test]
        public void BtnMoveCampaign_Click_MoveSuccess()
        {
            //Arrange
            InitilizeTestObjects();

            bool loadGrid = false;

            var btnMoveCampaign = privateObject.GetFieldOrProperty("btnMoveCampaign") as Button;
            btnMoveCampaign.CommandArgument = "1";

            var ddlCampaigns = privateObject.GetFieldOrProperty("ddlCampaigns") as DropDownList;
            ddlCampaigns.Items.Add("1");
            ddlCampaigns.SelectedValue = "1";

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                loadGrid = true;
            };

            ShimCampaignItem.MoveCampaignItemUser = (p1, p2) => { };

            // Act
            privateObject.Invoke("btnMoveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            loadGrid.ShouldBeTrue();
        }

        [Test]
        public void BtnMoveCampaign_Click_ECNException()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnMoveCampaign = privateObject.GetFieldOrProperty("btnMoveCampaign") as Button;
            btnMoveCampaign.CommandArgument = "1";

            var ddlCampaigns = privateObject.GetFieldOrProperty("ddlCampaigns") as DropDownList;
            ddlCampaigns.Items.Add("1");
            ddlCampaigns.SelectedValue = "1";

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItem.MoveCampaignItemUser = (p1, p2) =>
            {
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            };

            ShimManageCampaigns.AllInstances.setECNErrorECNException = (p1, p2) =>
            {
                errorMessage = p2.ErrorList[0].ErrorMessage;
            };

            // Act
            privateObject.Invoke("btnMoveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("Test Exception");
        }

        [Test]
        public void BtnMoveCampaign_Click_SystemException()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnMoveCampaign = privateObject.GetFieldOrProperty("btnMoveCampaign") as Button;
            btnMoveCampaign.CommandArgument = "1";

            var ddlCampaigns = privateObject.GetFieldOrProperty("ddlCampaigns") as DropDownList;
            ddlCampaigns.Items.Add("1");
            ddlCampaigns.SelectedValue = "1";

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                var listCI = new List<CampaignItem>();
                var item = new CampaignItem
                {
                    CampaignID = p1,
                    IsDeleted = true,
                    CampaignItemID = p1
                };

                listCI.Add(item);

                return listCI;
            };

            ShimCampaignItem.MoveCampaignItemUser = (p1, p2) =>
            {
                throw new Exception("Test Exception");
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            EntityFake.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, w, e, desc, t, y) =>
            {
                return 1;
            };

            // Act
            privateObject.Invoke("btnMoveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("An error has occurred");
        }

        [Test]
        public void BtnMoveCampaign_Click_NoCampaignItems_DoNothing()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnMoveCampaign = privateObject.GetFieldOrProperty("btnMoveCampaign") as Button;
            btnMoveCampaign.CommandArgument = "1";

            ShimCampaignItem.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new List<CampaignItem>();
            };

            // Act
            privateObject.Invoke("btnMoveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("");
        }

        [Test]
        public void BtnSaveCampaign_Click_Success()
        {
            //Arrange
            InitilizeTestObjects();

            var loadGrid = false;

            var btnSaveCampaign = privateObject.GetFieldOrProperty("btnSaveCampaign") as Button;
            btnSaveCampaign.CommandArgument = "1";

            ShimCampaign.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new Campaign();
            };

            ShimCampaign.SaveCampaignUser = (p1, p2) => { return 1; };


            ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
            {
                loadGrid = true;
            };

            // Act
            privateObject.Invoke("btnSaveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            loadGrid.ShouldBeTrue();
        }

        [Test]
        public void BtnSaveCampaign_Click_SystemException()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnSaveCampaign = privateObject.GetFieldOrProperty("btnSaveCampaign") as Button;
            btnSaveCampaign.CommandArgument = "1";

            ShimCampaign.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new Campaign();
            };

            ShimCampaign.SaveCampaignUser = (p1, p2) =>
            {
                throw new Exception();
            };

            EntityFake.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, w, e, desc, t, y) =>
            {
                return 1;
            };

            ShimManageCampaigns.AllInstances.throwECNExceptionString = (p1, p2) =>
            {
                errorMessage = p2;
            };

            // Act
            privateObject.Invoke("btnSaveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("An error has occurred");
        }

        [Test]
        public void BtnSaveCampaign_Click_ECNException()
        {
            //Arrange
            InitilizeTestObjects();

            var errorMessage = "";

            var btnSaveCampaign = privateObject.GetFieldOrProperty("btnSaveCampaign") as Button;
            btnSaveCampaign.CommandArgument = "1";

            ShimCampaign.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
            {
                return new Campaign();
            };

            ShimCampaign.SaveCampaignUser = (p1, p2) =>
            {
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            };

            ShimManageCampaigns.AllInstances.setECNErrorECNException = (p1, p2) =>
            {
                errorMessage = p2.ErrorList[0].ErrorMessage;
            };

            // Act
            privateObject.Invoke("btnSaveCampaign_Click", new object[] { null, EventArgs.Empty });

            //Assert
            errorMessage.ShouldBe("Test Exception");
        }

        [Test]
        public void btnNext_Click()
        {
            //Arrange
            InitilizeTestObjects();

            var gvCampaigns = privateObject.GetFieldOrProperty("gvCampaigns") as GridView;

            ShimGridView.AllInstances.BottomPagerRowGet = (p1) =>
            {
                return GetGridViewRow(DataControlRowType.DataRow);
            };

            ShimGridView.AllInstances.PageIndexGet = (p1) =>
            {
                return 1;
            };

            ShimManageCampaigns.AllInstances.LoadGrid = (p1) => { };

            // Act
            privateObject.Invoke("btnNext_Click", new object[] { null, EventArgs.Empty });

            //Assert
            gvCampaigns.PageIndex.ShouldBe(1);
        }

        [Test]
        public void ChkArchive_CheckedChanged()
        {
            var checkBox = new CheckBox();
            try
            {
                //Arrange
                InitilizeTestObjects();

                var loadGrid = false;
                checkBox.Attributes.Add("CID", "1");

                ShimCampaign.GetByCampaignIDInt32UserBoolean = (p1, p2, p3) =>
                {
                    return new Campaign();
                };

                ShimCampaign.SaveCampaignUser = (p1, p2) => { return 1; };

                ShimManageCampaigns.AllInstances.LoadGrid = (p1) =>
                {
                    loadGrid = true;
                };

                // Act
                privateObject.Invoke("chkArchive_CheckedChanged", new object[] { checkBox, EventArgs.Empty });

                //Assert
                loadGrid.ShouldBeTrue();
            }
            finally
            {
                checkBox.Dispose();
            }
        }

        private GridViewRow GetGridViewRow(DataControlRowType type)
        {
            var row = new GridViewRow(0, 0, type, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            row.Cells[0].Controls.Add(new Label { ID = "lblTotalRecords" });
            row.Cells[0].Controls.Add(new Label { ID = "lblTotalNumberOfPages", Text = "3" });
            row.Cells[0].Controls.Add(new LinkButton { ID = "lbPending" });
            row.Cells[0].Controls.Add(new LinkButton { ID = "lbSent" });
            row.Cells[0].Controls.Add(new LinkButton { ID = "lbSaved" });
            row.Cells[0].Controls.Add(new LinkButton { ID = "lbEditCampaignName" });
            row.Cells[0].Controls.Add(new CheckBox { ID = "chkArchive" });
            row.Cells[0].Controls.Add(new HtmlButton { ID = "liDeleteCampaign" });

            _lbDeleteCampaign = new LinkButton { ID = "lbDeleteCampaign" };
            row.Cells[0].Controls.Add(_lbDeleteCampaign);

            _txtGoToPage = new TextBox { ID = "txtGoToPage" };
            row.Cells[0].Controls.Add(_txtGoToPage);

            var ddlPageSize = new DropDownList();
            ddlPageSize = new DropDownList() { ID = "ddlPageSize" };
            row.Cells[0].Controls.Add(ddlPageSize);

            var table = new DataTable();
            table.Columns.Add("CIPending", typeof(string));
            table.Columns.Add("CISent", typeof(string));
            table.Columns.Add("CISaved", typeof(string));
            table.Columns.Add("IsArchived", typeof(bool));

            var dr = table.NewRow();
            dr["CIPending"] = "0";
            dr["CISent"] = "0";
            dr["CISaved"] = "0";
            dr["IsArchived"] = true;
            table.Rows.Add(dr);

            var drv = table.DefaultView[table.Rows.IndexOf(dr)];
            row.DataItem = drv;
            DataRowView drvCurrent = (DataRowView)row.DataItem;
            return row;
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            ShimPage.AllInstances.MasterGet = (instance) => master;
            InitializeAllControls(master);
            ShimCommunicator.AllInstances.UserSessionGet = (p1) =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1", CustomerID = 1 };
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };

            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1", CustomerID = 1 };
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
        }
    }
}