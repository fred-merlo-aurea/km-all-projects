using System.Collections.Generic;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardPreview_SFTest
    {
        [Test]
        public void gvSubscriberShare_RowDataBound_FacebookLink_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdFacebookLike, DisplayName="Like" };
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth()
            {
                ProfileName ="Test profile"
            };
            
            var gvSubscriberShare = _testObject.GetField("gvSubscriberShare") as GridView;
            gvSubscriberShare.DataSource = new List<CampaignItemSocialMedia> {
                new CampaignItemSocialMedia {
                    SocialMediaID = SocialMediaIdFacebookLike,
                    SimpleShareDetailID = 1,
                    SocialMediaAuthID = 1,
                }
            };
            ShimControl.AllInstances.FindControlString = (ctrl, controlName) =>
            {
                if (controlName == "lblSocialMediaName")
                {
                    return _lblAccountName;
                }
                else if (controlName == "lblSocialMedia")
                {
                    return _lblNetworkName;
                }

                return new Label();
            };
            gvSubscriberShare.RowDataBound += TestSubscriberEventHandler;

            // Act
            gvSubscriberShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();

            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("Test profile"));

            _lblNetworkName.ShouldSatisfyAllConditions(
                () => _lblNetworkName.ShouldNotBeNull(),
                () => _lblNetworkName.Text.ShouldBe("Like"));
        }

        private void TestSubscriberEventHandler(object sender, GridViewRowEventArgs e)
        {
            _handlerCalled = true;
            _testObject.Invoke("gvSubscriberShare_RowDataBound", new object[] { sender, e });
        }
    }
}
