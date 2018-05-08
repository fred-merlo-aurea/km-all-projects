using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using CommonEncryption = KM.Common.Encryption;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class Encryption
    {
        private const string DefaultPassPhrase = "a}MTLboFrVa1Q)V~s|DpyiRccV'<5j*>0umZ4o0m(YbS{,i^w2K'[>WFj:e[-+R";
        private const string DefaultHashAlgorithm = "SHA256";
        private const string DefaultInitVector = "U^.6V#Gy,94@pQU]";
        private const int DefaultKeySize = 256;
        private const int DefaultPasswordIterations = 2;

        private static string _CacheRegion = "Encryption";

        /// <summary>
        /// Initializes a new instance of the <see cref="Encryption"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor should never be removed since this class is dynamicaly
        /// built using <see cref="DynamicBuilder{T}"/> ProcessBuild method
        /// </remarks>
        public Encryption()
        {
        }

        public Encryption(bool useDefaults = false)
        {
            ID = -1;
            PassPhrase = "";
            SaltValue = "";
            HashAlgorithm = "";
            PasswordIterations = -1;
            InitVector = "";
            KeySize = -1;
            IsActive = false;
            IsCurrent = false;

            if (useDefaults)
            {
                var encryption = new CommonEncryption();
                SaltValue = encryption.GetRandomSalt();
                PassPhrase = DefaultPassPhrase;
                HashAlgorithm = DefaultHashAlgorithm;
                PasswordIterations = DefaultPasswordIterations;
                InitVector = DefaultInitVector;
                KeySize = DefaultKeySize;
            }
        }

        #region Properties
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PassPhrase { get; set; }
        [DataMember]
        public string SaltValue { get; set; }
        [DataMember]
        public string HashAlgorithm { get; set; }
        [DataMember]
        public int PasswordIterations { get; set; }
        [DataMember]
        public string InitVector { get; set; }
        [DataMember]
        public int KeySize { get; set; }
        [DataMember]
        public bool IsCurrent { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        #endregion

        #region Data
        #region Select
        public static List<Encryption> GetAll()
        {
            List<Encryption> encryptionList = null;

            //object cacheValue = CacheHelper.GetCurrentCache(CacheHelper.CacheItems.Encryption_List.ToString());
            object cacheValue = CacheHelper.GetCurrentCache("KM.Common.Entity.Encryption_Cache");

            if (cacheValue == null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Encryption_Select";

                SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);
                var builder = DynamicBuilder<Encryption>.CreateBuilder(rdr);
                encryptionList = new List<Encryption>();
                while (rdr.Read())
                {
                    Encryption encryption = builder.Build(rdr);
                    encryptionList.Add(encryption);
                }
                cmd.Connection.Close();
                cacheValue = CacheHelper.AddToCache("KM.Common.Entity.Encryption_Cache", encryptionList);
            }
            return (List<Encryption>)cacheValue;
        }
        public static Encryption GetByID(int id)
        {
            Encryption encryption = GetAll().SingleOrDefault();
            return encryption;
        }

        public static Encryption GetCurrentByApplicationID(int applicationID)
        {
            Encryption encryption = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                try
                {
                    encryption = (Encryption)CacheUtil.GetFromCache(applicationID.ToString(), _CacheRegion);
                }
                catch (Exception)
                {}
                if (encryption == null)
                {
                    encryption = GetAll().SingleOrDefault();
                    if (encryption != null)
                        CacheUtil.AddToCache(applicationID.ToString(), encryption, _CacheRegion);
                }
            }
            else
                encryption = GetAll().SingleOrDefault();
            return encryption;
        }

        public static List<Encryption> GetAllByApplicationID(int applicationID)
        {
            List<Encryption> encryptionList = GetAll();
            return encryptionList;
        }
        #endregion
        #endregion

    }
}
