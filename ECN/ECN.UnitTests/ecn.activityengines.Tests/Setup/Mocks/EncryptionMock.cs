using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = KM.Common.Fakes.ShimEncryption;
using ShimEntity = KM.Common.Entity.Fakes.ShimEncryption;
using Moq;
using KM.Common.Entity;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class EncryptionMock : Mock<IEncryption>
    {
        public EncryptionMock()
        {
            SetupShims();
            SetReturnsDefault(string.Empty);
        }

        private void SetupShims()
        {
            ShimEntity.GetCurrentByApplicationIDInt32 = GetCurrentByApplicationId;
            Shim.EncryptStringEncryption = Encrypt;
            Shim.Base64EncryptStringEncryption = Base64Encrypt;
        }

        private string Base64Encrypt(string plainText, Encryption encryption)
        {
            return Object.Base64Encrypt(plainText, encryption);
        }

        private string Encrypt(string plainText, Encryption encryption)
        {
            return Object.Encrypt(plainText, encryption);
        }

        private Encryption GetCurrentByApplicationId(int applicationId)
        {
            return Object.GetCurrentByApplicationId(applicationId);
        }
    }
}
