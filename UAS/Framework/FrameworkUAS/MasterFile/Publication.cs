using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FrameworkUAS.DataAccess;

namespace FrameworkUAS.MasterFile
{
    [Serializable]
    [DataContract]
    public class Publication
    {
        public Publication() { ResponseTypeList = new List<ResponseType>(); }
        #region Properties
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string PublicationName { get; set; }
        [DataMember]
        public string PublicationCode { get; set; }
        #endregion

        public List<ResponseType> ResponseTypeList { get; set; }

        #region CRUD
        public List<Publication> Select(string clientName)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = Object.DBWorker.GetClientSqlConnection(clientName);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "SELECT PubID as 'PublicationID', PubName as 'PublicationName', PubCode as 'PublicationCode' FROM Pubs With(NoLock)";

                return GetList(cmd);
            }
        }

        private List<Publication> GetList(SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            var retList = MasterFileHelpers.ReadPublicationsList(cmd, true);
            return retList;
        }

        #endregion
    }
}
