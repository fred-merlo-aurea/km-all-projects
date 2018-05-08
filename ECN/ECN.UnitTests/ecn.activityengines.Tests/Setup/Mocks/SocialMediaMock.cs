using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using Moq;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class SocialMediaMock : Mock<ISocialMedia>
    {
        public SocialMediaMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimSocialMedia.GetSocialMediaByIDInt32 = GetSocialMediaById;
            ShimSocialMedia.GetSocialMediaCanShare = GetSocialMediaCanShare;
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = GetBySocialMediaAuthID;
            ShimSocialMediaHelper.GetFBUserProfileString = GetFBUserProfile;
        }

        private List<SocialMedia> GetSocialMediaCanShare()
        {
            return Object.GetSocialMediaCanShare();
        }

        private Dictionary<string, string> GetFBUserProfile(string accessToken)
        {
            return Object.GetFBUserProfile(accessToken);
        }

        private SocialMediaAuth GetBySocialMediaAuthID(int socialId)
        {
            return Object.GetBySocialMediaAuthID(socialId);
        }

        private SocialMedia GetSocialMediaById(int id)
        {
            return Object.GetSocialMediaById(id);
        }
    }
}
