using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ecn.common.classes;

namespace ecn.communicator.classes
{
    public class EmailBlacklist
    {
        int _blastID;
        Hashtable _blackList;
        public EmailBlacklist(int blastId)
        {
            _blastID = blastId;
            _blackList = new Hashtable();

            BlockSentEmailsWithinAWeek();
            BlockMasterSupression();
        }

        public bool IsEmailBlocked(int emailID)
        {
            return _blackList[emailID] == null ? false : true;
        }

        public int BlockEmailsForSampling(int sampleBlastID)
        {
            string sql = string.Format("SELECT EmailID from BlastActivitySends WHERE BlastID = {0}", sampleBlastID);
            return Block(sql);
        }

        public void Add(int emailID)
        {
            if (_blackList[emailID] == null)
            {
                _blackList.Add(emailID, true);
                return;
            }
        }

        #region Private Methods
        private int BlockSentEmailsWithinAWeek()
        {
            //Optimized the sql using JOINS-ashok 6/6/06
            /*string sql = string.Format(@"declare @CustomerID as int;
declare @LayoutID as int;
set @CustomerID =  (select CustomerID from Blast where BlastID = {0})
set @LayoutID =  (select LayoutID from Blast where BlastID = {0})
select EmailID from EmailActivityLog 
where 
(BlastID in (select BlastID from Blast where LayoutID = @LayoutID)) 
AND ActionTypeCode='send' 
AND ActionDate>'{1}'",
                _blastID, theEarliestAllowedDate.ToString());*/

            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(7, 0, 0, 0);
            System.DateTime theEarliestAllowedDate = today.Subtract(duration);

            string sql = string.Format(@"DECLARE @LayoutID AS int;
SET @LayoutID =  (SELECT LayoutID FROM Blast WHERE BlastID = {0})
SELECT EmailID FROM BlastActivitySends eal JOIN Blast b ON eal.BlastID = b.BlastID 
WHERE b.LayoutID = @LayoutID 
AND SendTime>'{1}'",
                _blastID, theEarliestAllowedDate.ToString());
            return Block(sql);
        }

        private int BlockMasterSupression()
        {
            //Optimized the sql using JOINS -ashok 6/6/06
            /*string sql = string.Format(@"declare @CustomerID as int;
set @CustomerID =  (select CustomerID from Blast where BlastID = {0})
select EmailID from Emails where EmailID in (select EmailID from EmailGroups where GroupID=(select GroupID from Groups where CustomerID = @CustomerID and MasterSupression=1))",
            _blastID);*/
            string sql = string.Format(@"DECLARE @CustomerID AS int;
SET @CustomerID =  (SELECT CustomerID FROM Blast WHERE BlastID = {0})
SELECT e.EmailID FROM Emails e JOIN EmailGroups eg ON e.emailID = eg.emailID JOIN Groups g ON eg.groupID = g.groupID
WHERE g.CustomerID = @CustomerID AND g.MasterSupression=1", _blastID);
            return Block(sql);
        }

        private int Block(string sql)
        {
            //Timing out on big queries.. 
            //-ashok 6/6/06
            //DataTable blockEntries = DataFunctions.GetDataTable(sql);
            DataTable blockEntriesDT = null;
            SqlConnection db_connection = new SqlConnection(DataFunctions.GetConnectionString().ToString());
            SqlCommand blockCmd = new SqlCommand(sql, db_connection);
            blockCmd.CommandTimeout = 0;

            SqlDataAdapter blockDA = new SqlDataAdapter(blockCmd);
            DataSet blockDS = new DataSet();
            db_connection.Open();
            blockDA.Fill(blockDS, "blockentries");
            db_connection.Close();
            blockEntriesDT = blockDS.Tables[0];

            foreach (DataRow dr in blockEntriesDT.Rows)
            {
                Add(Convert.ToInt32(dr["EmailID"].ToString()));
            }
            return blockEntriesDT.Rows.Count;
        }
        #endregion
    }
}
