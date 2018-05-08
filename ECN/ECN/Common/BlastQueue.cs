using System;
using System.Data;
using System.Data.SqlClient;

namespace ecn.common.classes
{

    public class BlastQueue
    {
        public BlastQueue()
        {
        }

        private string _Message = string.Empty;
        private string _Processed = string.Empty;
        private int _BlastQueueID = 0;
        private int _BlastID = 0;

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
        public string Processed
        {
            get
            {
                return _Processed;
            }
            set
            {
                _Processed = value;
            }
        }
        public int BlastQueueID
        {
            get
            {
                return _BlastQueueID;
            }
        }
        public int BlastID
        {
            get
            {
                return _BlastID;
            }
            set
            {
                _BlastID = value;
            }
        }

        public int AddMessage()
        {
            int newBlastQueueID;
            try
            {
                String insert = string.Empty;
                insert = "insert into BlastQueue (BlastID, Message, Processed) values (@BlastID, @Message, @Processed);SELECT @@IDENTITY";
                SqlCommand cmd = new SqlCommand(insert);
                cmd.Parameters.Add(new SqlParameter("@BlastID", this._BlastID));
                cmd.Parameters.Add(new SqlParameter("@Message", this._Message));
                cmd.Parameters.Add(new SqlParameter("@Processed", 'n'));
                newBlastQueueID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmd));
                return newBlastQueueID;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
