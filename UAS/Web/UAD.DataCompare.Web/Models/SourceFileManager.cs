using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FrameworkUAS.Entity;

namespace UAD.DataCompare.Web.Models
{
    public class SourceFile : SourceFileBase
    {
        public SourceFile()
        {
            FieldMapping = null;
            ColumnMapping = null;
            sourceFileUploadPath = string.Empty;
            errorMessagesDict = null;
        }

        #region Properties

        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string FileOwner { get; set; }

        [DataMember]
        public List<FrameworkUAS.Entity.FieldMapping> FieldMapping { get; set; }

        [DataMember]
        public List<ColumnMapper> ColumnMapping { get; set; }


        [DataMember]
        public Dictionary<string, string> errorMessagesDict { get; set; }

        [DataMember]
        public string  sourceFileUploadPath { get; set; }

        #endregion
    }

    public class SourceFileManager
    {
        private KMPlatform.BusinessLogic.Client _kmClient;
        private KMPlatform.BusinessLogic.User _kmUser;
        private FrameworkUAS.BusinessLogic.SourceFile _sfWrk;
        
        private SourceFile _sfCurrent;

        public List<SourceFile> getFileListByClient(int CurrentClientID)
        {
            #region Framework Object intialization
            _sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();

            #endregion

            
            //Get List of source file by client
            List<FrameworkUAS.Entity.SourceFile> _sfEntityList = _sfWrk.SelectByFileType(CurrentClientID,FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare, true);

            #region Object Lists
            List<SourceFile> _sfCurrentList = new List<SourceFile>();
            #endregion


            //Mapp required values from to Model
            foreach (FrameworkUAS.Entity.SourceFile sf in _sfEntityList)
            {
                _sfCurrent = new SourceFile();
                _sfCurrent.SourceFileID = sf.SourceFileID;
                _sfCurrent.FileName = sf.FileName;
                _sfCurrent.Extension = sf.Extension;
                _sfCurrent.Delimiter = sf.Delimiter;
                _sfCurrent.DateCreated = sf.DateCreated;
                _sfCurrent.DateUpdated = sf.DateUpdated;
                _sfCurrent.FileOwner = getFileOwner(sf.CreatedByUserID);
                _sfCurrent.ClientName = getClientByClientID(sf.ClientID);

                _sfCurrentList.Add(_sfCurrent);
            }

            return _sfCurrentList;


        }

        private string getClientByClientID(int clientID)
        {
            string clientName = " ";

            try
            {
                _kmClient = new KMPlatform.BusinessLogic.Client();
                var _client = new KMPlatform.Entity.Client();
                _client = _kmClient.Select(clientID);
                clientName = _client.DisplayName;
            }
            catch (Exception ex)
            {
                clientName = "N/A";
            }
            

            return clientName;
        }

        private string getFileOwner(int createdByUserID)
        {
            string userEmail = "";

            try
            {
                _kmUser = new KMPlatform.BusinessLogic.User();
                var _user = new KMPlatform.Entity.User();
                _user = _kmUser.SelectUser(createdByUserID);
                userEmail = _user.EmailAddress;
            }
            catch (Exception ex)
            {
                userEmail = "N/A";
             }

            return userEmail;
        }
    }
}