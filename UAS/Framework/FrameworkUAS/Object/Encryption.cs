using System;
using System.Runtime.Serialization;
using CommonEncryption = KM.Common.Encryption;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class Encryption
    {
        public Encryption()
        {
            PassPhrase = "a}MTLboFrVa1Q)V~s|DpyiRccV'<5j*>0umZ4o0m(YbS{,i^w2K'[>WFj:e[-+R";
            var encryption = new CommonEncryption();
            SaltValue = encryption.GetRandomSalt();
            HashAlgorithm = "SHA256";
            PasswordIterations = 2;
            InitVector = "U^.6V#Gy,94@pQU]";
            KeySize = 256;
        }
        public Encryption(string _PassPhrase = "", string _SaltValue = "", string _HashAlgorithm = "", int _PasswordIterations = -1, string _InitVector = "", int _KeySize = -1)
        {
            PassPhrase = _PassPhrase;
            SaltValue = _SaltValue;
            HashAlgorithm = _HashAlgorithm;
            PasswordIterations = _PasswordIterations;
            InitVector = _InitVector;
            KeySize = _KeySize;
        }
        #region Properties

        public string PassPhrase { get; set; }
        [DataMember]
        public string SaltValue { get; set; }
        public string HashAlgorithm { get; set; }
        public int PasswordIterations { get; set; }
        public string InitVector { get; set; }
        public int KeySize { get; set; }
        [DataMember]
        public string PlainText { get; set; }
        [DataMember]
        public string EncryptedText { get; set; }
        #endregion

    }
}
