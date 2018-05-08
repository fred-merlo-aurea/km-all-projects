using KM.Common.Entity;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IEncryption
    {
        Encryption GetCurrentByApplicationId(int applicationId);

        string Encrypt(string plainText, Encryption encryption);

        string Base64Encrypt(string plainText, Encryption encryption);
    }
}
