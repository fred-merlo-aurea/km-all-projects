using KM.Common;
using CommonEncryption = KM.Common.Encryption;
using CommonEncryptionEntity = KM.Common.Entity.Encryption;
using KmEncryptionEntity = KMPlatform.Object.Encryption;

namespace KMPlatform.BusinessLogic
{
    public class Encryption
    {
        public KmEncryptionEntity Encrypt(KmEncryptionEntity encryptionData)
        {
            Guard.NotNull(encryptionData, nameof(encryptionData));

            var data = GetEncryptionData(encryptionData);
            encryptionData.EncryptedText = CommonEncryption.Encrypt(encryptionData.PlainText, data);

            return encryptionData;
        }

        public KmEncryptionEntity Decrypt(KmEncryptionEntity encryptionData)
        {
            Guard.NotNull(encryptionData, nameof(encryptionData));

            if (string.IsNullOrWhiteSpace(encryptionData.EncryptedText))
            {
                return encryptionData;
            }

            var data = GetEncryptionData(encryptionData);
            encryptionData.PlainText = CommonEncryption.Decrypt(encryptionData.EncryptedText, data);

            return encryptionData;
        }

        private CommonEncryptionEntity GetEncryptionData(KmEncryptionEntity encryptionData)
        {
            Guard.NotNull(encryptionData, nameof(encryptionData));

            return new CommonEncryptionEntity
            {
                SaltValue = encryptionData.SaltValue,
                PassPhrase = encryptionData.PassPhrase,
                KeySize = encryptionData.KeySize,
                PasswordIterations = encryptionData.PasswordIterations,
                HashAlgorithm = encryptionData.HashAlgorithm,
                InitVector = encryptionData.InitVector
            };
        }
    }
}
