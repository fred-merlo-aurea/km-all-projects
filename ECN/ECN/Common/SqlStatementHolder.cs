//using System;
//using System.Collections;
//using System.Collections.Specialized;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;

//namespace ecn.common.classes
//{
//    public class SqlStatementHolder
//    {

//        int _blastID = 0;
//        int counter = 0;
//        bool _istestblast = false;

//        StringBuilder sbInsertXML = new StringBuilder();

//        public SqlStatementHolder(int BlastID, bool IsTestBlast) 
//        {
//            this._blastID = BlastID;
//            this._istestblast = IsTestBlast;
//        }

//        public void Add(string EmailID) 
//        {
//            if (Convert.ToInt32(EmailID) > 0)
//            {
//                sbInsertXML.Append("<e>" + EmailID.ToString() + "</e>");
//            }
//            counter++;
//            if (counter > 1000)
//                FinishFile();
//        }
//        public int Count {
//            get { return counter; }
//        }
//        public static void FinalFinish(object myref)
//        {
//            SqlStatementHolder my_list = (SqlStatementHolder)myref;
//            my_list.FinishFile();
//        }
//        public void FinishFile() 
//        {
//            try 
//            {
//                //Console.WriteLine("Dumping Log: "+ DateTime.Now);

//                SqlCommand cmdInsertLog = new SqlCommand("sp_insertLog_Sends");
//                cmdInsertLog.CommandTimeout	= 0;
//                cmdInsertLog.CommandType= CommandType.StoredProcedure;

//                cmdInsertLog.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
//                cmdInsertLog.Parameters["@BlastID"].Value =  this._blastID;

//                cmdInsertLog.Parameters.Add(new SqlParameter("@ActionTypeCode", SqlDbType.VarChar, 25));
//                cmdInsertLog.Parameters["@ActionTypeCode"].Value = _istestblast?"testsend":"send";
				
//                cmdInsertLog.Parameters.Add(new SqlParameter("@xmlDoc", SqlDbType.Text));
//                cmdInsertLog.Parameters["@xmlDoc"].Value = "<XML>" + sbInsertXML.ToString() + "</XML>";

//                DataFunctions.Execute(cmdInsertLog);

//                sbInsertXML = new StringBuilder();
//                counter = 0;
//                //Console.WriteLine("Finished Log: "+DateTime.Now);
//            } 
//            catch (Exception ex) 
//            {
//                //sbInsertXML
//                Console.WriteLine(ex.ToString());
//            }
//        }
//    }
//}
