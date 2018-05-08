//using System;
//using System.Data;
//using ecn.common.classes;
//using System.Data.SqlClient;

//namespace ecn.communicator.classes {
    
//    /// This class handles all of the sample blasing for the A/B blasts. It has no limit on the number of samples that
//    /// can get sent on a blast so it will support A/B/C blasting or beyond if need be.
    
//    public class Samples : DatabaseAccessor,IBlastLike {

//        int _customer_id;
//        int _user_id;
//        string _subject;
//        string _blast_frequency;
//        string _email_from;
//        string _email_from_name;
//        string _reply_to;
//        Layouts _layout;
//        int _filter_id;
//        Groups _group;
//        string _name;
//        string _status_code;
//        string _send_time;
//        string _blastcodeid;
//        string _blastSuppressionlist;
//        bool _optoutPreference= false;
//        int? _blastScheduleID = null;

//        Blasts _master_blast;
		
//        /// Usual ID based constructor
		
//        /// <param name="input_id"> ID in database</param>
//        public Samples(int input_id):base(input_id) {
//        }
		
//        /// String version of ID constructor
		
//        /// <param name="input_id"> ID in database</param>
//        public Samples(string input_id):base(input_id) {
//        }
		
//        /// Nullary Constructor
		
//        public Samples():base() {
//        }
		
//        /// Get the Name of the Sample
		
//        /// <returns>Sample Name</returns>
//        public string Name() {
//            if(ID() != 0) {
//                try {
//                    _name = DataFunctions.ExecuteScalar("Select SampleName from Samples where SampleID=" + ID()).ToString();
//                } catch {

//                }
//            }
//            return _name;
//        }
		
//        /// Set the name of the sample on the object
		
//        /// <param name="new_name"> Sample Name</param>
//        /// <returns>Sample Name</returns>
//        public string Name(string new_name) {
//            _name = new_name;
//            return _name;
//        }
		
//        /// Set the cust id on the object
		
//        /// <param name="cid">Customer ID in DB</param>
//        public void CustomerID(int cid) {
//            _customer_id = cid;
//        }
		
//        /// Set the User ID on the Object
		
//        /// <param name="uid"> User ID in DB</param>
//        public void UserID(int uid) {
//            _user_id = uid;
//        }
		
//        /// Set the Subject on the object for blasting
		
//        /// <param name="sbj">subject to go in db</param>
//        public void Subject(string sbj) {
//            _subject = sbj;
//        }

//        public void BlastFrequency(string bfreq)
//        {
//            _blast_frequency = bfreq;
//        }
		
//        /// sets who the email comes from for blasting 
		
//        /// <param name="eml_frm">Who the email comes from</param>
//        public void EmailFrom(string eml_frm) {
//            _email_from = eml_frm;
//        }
		
//        /// Sets the email from name for blasting
		
//        /// <param name="eml_blastcodeid">from codeid you want on the blast</param>
//        public void BlastCodeID(string eml_blastcodeid) {
//            if (eml_blastcodeid == string.Empty)
//                _blastcodeid = "0";
//            else
//                _blastcodeid = eml_blastcodeid;
//        }
		
//        /// Sets the codeID from blast
		
//        /// <param name="eml_frm_nm">from name you want on the blast</param>
//        public void EmailFromName(string eml_frm_nm) 
//        {
//            _email_from_name = eml_frm_nm;
//        }
		
//        /// Sets the reply to for blasting
		
//        /// <param name="rply">reply to for blasting purposes</param>
//        public void ReplyTo(string rply) {
//            _reply_to = rply;
//        }
		
//        /// Sets which layout to send
		
//        /// <param name="lid"> will send this layout on blast</param>
//        public void Layout(Layouts lid) {
//            _layout = lid;
//        }
		
//        /// Sets any filters if necssary. Warning, how this effects collation has not been determined.
		
//        /// <param name="fid">a fliter to blast on</param>
//        public void FilterID(int fid) {
//            _filter_id = fid;
//        }
		
//        /// Sets the status code for updating into the DB
		
//        /// <param name="code"> Usual Blastlike status code</param>
//        public void StatusCode (string code) {
//            _status_code = code;
//        }

		
//        /// Which group we will sample
		
//        /// <param name="gid">Group id in database</param>
//        public void Group(Groups gid) {
//            _group = gid;
//        }

//        public bool OptoutPreference 
//        {
//            get {return _optoutPreference;}
//            set { _optoutPreference = value;}
//        }

//        public string BlastSuppressionlist 
//        {
//            get {return _blastSuppressionlist;}
//            set { _blastSuppressionlist = value;}
//        }

//        public int? BlastScheduleID
//        {
//            get { return _blastScheduleID; }
//            set { _blastScheduleID = value; }
//        }
		
//        /// Gets the group object that this sample uses. COMES FROM THE DB
		
//        /// <returns> Group object that sample uses</returns>
//        public Groups Group() 
//        {
//            return new Groups(Convert.ToInt32(DataFunctions.ExecuteScalar("Select b.GroupID from Samples s, Blasts b where s.SampleID=" + ID() + " AND b.BlastID=s.BlastID")));
//        }
		
//        /// Overide ID function to set the sample ID and create a master Blast object we can attach to.
		
//        /// <param name="new_id">Id of sample</param>
//        new public void ID(int new_id)  {
//            base.ID(new_id);
//            _master_blast = new Blasts(new_id);
//        }

		
//        /// Time you wish to send the blast
		
//        /// <param name="time">SQL server 2000 formatted time stamp</param>
//        public void SendTime(string time) {
//            _send_time = time;
//        }

		
//        /// Instantize the preloaded values in the database and create a master sample blast on our master sampler.
		
//        ////public void Create() {
//        ////    _master_blast = PresetBlast();
//        ////    //_master_blast.BlastFrequency("ONETIME");
//        ////    _master_blast.BlastCodeID(_blastcodeid);
//        ////    _master_blast.CreateMasterSampleBlast();

//        ////    ID(Convert.ToInt32(DataFunctions.ExecuteScalar("Insert Into Samples (CustomerID,SampleName,BlastID,UserID,DateEntered) VALUES (" + _customer_id + " ,'"+_name+"', "+ _master_blast.ID() + "," + _user_id +" , GetDate() );SELECT @@IDENTITY")));

//        ////}

		
//        /// Creates a new blast object with all the blast specific variables that the sample has.
		
//        /// <returns>Blast object not in Database </returns>
//        //private Blasts PresetBlast() {
//        //    Blasts my_blast = new Blasts();
//        //    my_blast.CustomerID(_customer_id);
//        //    my_blast.UserID(_user_id);
//        //    my_blast.Subject(_subject);
//        //    my_blast.EmailFrom(_email_from);
//        //    my_blast.EmailFromName(_email_from_name);
//        //    my_blast.BlastCodeID(_blastcodeid);
//        //    my_blast.ReplyTo(_reply_to);
//        //    my_blast.Layout(new Layouts()); // No layout associated with a Master Sample Blast. It just holds values for recordkeeping.
//        //    my_blast.FilterID(_filter_id);
//        //    my_blast.SendTime(_send_time);
//        //    my_blast.Group(_group);
//        //    my_blast.BlastSuppressionlist = _blastSuppressionlist;
//        //    my_blast.OptoutPreference = _optoutPreference;
//        //    my_blast.BlastScheduleID = _blastScheduleID;

//        //    return my_blast;
//        //}

		
//        /// Given a layout and an amount of people that a customer wants to blast to, create sample blasts and attach them to the 
//        /// appropriate tables
		
//        /// <param name="layout">Which Layout you wish to use, based on the ID field</param>
//        /// <param name="amount"> How many people you want to send to (no bounds checking)</param>
//        /// <returns>The newly created blast</returns>
//        //public Blasts AttachLayoutByCount(Layouts layout,int amount, bool isAmount) {
//        //    if(null == _master_blast) {
//        //        throw new Exception("Sample Needs MasterBlast Obj Created");
//        //    }

//        //    Blasts my_blast = PresetBlast();
//        //    my_blast.BlastCodeID(_blastcodeid);
//        //    my_blast.Layout(layout);
//        //    my_blast.CreateSampleBlast();

//        //    string overrideAmount = amount.ToString();
//        //    string overrideIsAmount = "'true'";            
//        //    if (!isAmount)
//        //    {
//        //        overrideIsAmount = "'false'";
//        //    }

//        //    DataFunctions.ExecuteScalar("Insert Into SampleBlasts (SampleID,BlastID,Amount, IsAmount) VALUES (" + ID()
//        //        + " ," + my_blast.ID() + "," + overrideAmount + ", " + overrideIsAmount + ");SELECT @@IDENTITY");

//        //    return my_blast;

//        //}

//        //old
//        //public Blasts AttachLayoutByCount(Layouts layout, int amount)
//        //{
//        //    if (null == _master_blast)
//        //    {
//        //        throw new Exception("Sample Needs MasterBlast Obj Created");
//        //    }

//        //    Blasts my_blast = PresetBlast();
//        //    my_blast.BlastCodeID(_blastcodeid);
//        //    my_blast.Layout(layout);
//        //    my_blast.CreateSampleBlast();

//        //    DataFunctions.ExecuteScalar("Insert Into SampleBlasts (SampleID,BlastID,Amount) VALUES (" + ID()
//        //        + " ," + my_blast.ID() + "," + amount + ");SELECT @@IDENTITY");

//        //    return my_blast;

//        //}
        

		
//        /// Determines the Champion of a blast based on which layout recieved the most clicks.
//        /// It returns the earliest blast id on ties.
		
//        /// <returns>The winning blast</returns>
//        //public Blasts ChampionByClicks() {

//        //    DataTable blasts = DataFunctions.GetDataTable("Select * from SampleBlasts where SampleID=" + ID());
//        //    Decimal current_winner_percent = 0.0M;
//        //    Blasts winning_blast = new Blasts();
//        //    foreach(DataRow dr in blasts.Rows) {
//        //        Blasts current_blast = new Blasts(Convert.ToInt32(dr["BlastID"]));
//        //        // First time through, at least give us a winner by default.
//        //        if( 0 == winning_blast.ID()) {
//        //            winning_blast.ID( current_blast.ID() );
//        //        }

//        //        Decimal my_percent = current_blast.GetClickPercent();

//        //        if(my_percent > current_winner_percent) {
//        //            winning_blast.ID( current_blast.ID());
//        //            current_winner_percent = my_percent;
//        //        }
//        //    }
//        //    //throw new Exception("winning blast = " + winning_blast.ID() + " percent = " + current_winner_percent);
//        //    return winning_blast;
//        //}

//        //public Blasts ChampionByProc()
//        //{
//        //    SqlCommand cmd = new SqlCommand();
//        //    cmd.CommandType = CommandType.StoredProcedure;
//        //    cmd.CommandText = "spGetSampleInfoForChampion";
//        //    cmd.Parameters.AddWithValue("@SampleID", ID());
//        //    cmd.Parameters.AddWithValue("@JustWinningBlastID", true);
//        //    DataTable blasts = DataFunctions.GetDataTable("activity", cmd);
//        //    Blasts winning_blast = new Blasts(Convert.ToInt32(blasts.Rows[0]["BlastID"]));

//        //    return winning_blast;
//        //}


		
//        /// Display helper function. Gets the important info on the sample table for display.
		
//        /// <returns>A datattable of important sample info for display</returns>
//        //public DataTable LoadCustomerSamples() {

//        //    string sqlquery=
//        //        " SELECT b.*, f.FilterName, g.GroupName, s.SampleName " +
//        //        " FROM Blast b, Filter f, Groups g, [Sample] s "+
//        //        " WHERE b.CustomerID="+_customer_id+ 
//        //        " AND b.CreatedUserID=" + _user_id+ 
//        //        " AND b.BlastType='Sample' " +
//        //        " AND sb.BlastID = b.BlastID " +  // should only be in the table once.. may fail if not the case
//        //        " AND s.SampleID = sb.SampleID "+
//        //        " AND b.FilterID*=f.FilterID "+
//        //        " AND b.GroupID=g.GroupID "+
//        //        " ORDER BY b.SendTime DESC";
//        //    DataTable sub_table = DataFunctions.GetDataTable(sqlquery);
//        //    sub_table.Columns.Add(new DataColumn("Clicks"));

//        //    foreach(DataRow dr in sub_table.Rows) {
//        //        Blasts current_blast = new Blasts((int)dr["BlastID"]);
//        //        Decimal my_percent = current_blast.GetClickPercent();
//        //        dr["Clicks"] = my_percent;
//        //    }
//        //    return sub_table;
//        //}

		
//        /// Returns all blasts that have this sampleID
		
//        /// <returns>BlastID and amount in a table</returns>
//        //public DataTable GetAllBlasts() {

//        //    string sqlBlastQuery=
//        //        " SELECT BlastID, OverrideAmount from Blast where BlastType = 'Sample' and SampleID=" + ID();
//        //    return DataFunctions.GetDataTable(sqlBlastQuery);
//        //}

		
//        /// Total count of people to send to
		
//        /// <returns> number of people to send to</returns>
//        //public int SampleCount() 
//        //{
//        //    DataTable dt = GetAllBlasts();
//        //    int sum = 0;
//        //    foreach(DataRow dr in dt.Rows) 
//        //    {
//        //        sum += (int) dr["Amount"];
//        //    }
//        //    return sum;
//        //}

//      /*  public DataTable LoadCustomerSamples() {

//            DataTable sample_ids = DataFunctions.GetDataTable("Select SampleID from Samples where CustomerID=" + _customer_id + " AND UserID=" + _user_id + " ORDER BY DateEntered DESC");
//            DataTable final_output = new DataTable();
//            bool first_pass = true;
//            foreach(DataRow dr in sample_ids.Rows) {
//                ID((int)dr["SampleID"]);
//                DataTable sample_blasts = DataFunctions.GetDataTable("Select BlastID from SampleBlasts where SampleID=" + ID());
//                foreach(DataRow blast in sample_blasts.Rows) {
//                    Blasts current_blast = new Blasts((int)blast["BlastID"]);
//                    Decimal my_percent = current_blast.GetClickPercent();

//                    string sqlquery=
//                        " SELECT b.*, f.FilterName, g.GroupName, '" + Name() + "' AS SampleName," + my_percent.ToString() + " AS Clicks " +
//                        " FROM Blasts b, Filters f, Groups g, Samples s, SampleBlasts sb "+
//                        " WHERE b.BlastID="+current_blast.ID()+
//                        " AND sb.BlastID = b.BlastID " +  // should only be in the table once.. may fail if not the case
//                        " AND s.SampleID = sb.SampleID "+
//                        " AND b.FilterID*=f.FilterID "+
//                        " AND b.GroupID=g.GroupID "+
//                        " ORDER BY b.SendTime DESC";
//                    DataTable sub_table = DataFunctions.GetDataTable(sqlquery);
//                    DataRow output_row = sub_table.Rows[0];
//                    if(first_pass) {
//                        foreach(DataColumn dc in sub_table.Columns) {
//                            final_output.Columns.Add(dc);
//                        }
//                        first_pass = false;
//                    }

//                    final_output.ImportRow(output_row);
//                }
//            }
////            foreach (DataRow dr in final_output.Rows) {
//  //              throw new Exception("blast_id="+ final_output.Rows.Count);
//    //        }
//      //      throw new Exception("end=" + final_output.Rows.Count);
//            return final_output;
//        } */

//    }
//}
