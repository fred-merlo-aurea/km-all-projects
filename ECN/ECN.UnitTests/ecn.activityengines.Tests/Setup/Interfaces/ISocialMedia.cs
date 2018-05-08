using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ISocialMedia
    {
        SocialMedia GetSocialMediaById(int id);

        SocialMediaAuth GetBySocialMediaAuthID(int socialId);

        Dictionary<string, string> GetFBUserProfile(string accessToken);

        List<SocialMedia> GetSocialMediaCanShare();
    }
}
