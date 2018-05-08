//using System;
//using System.IO;
//using ecn.common.classes;
//using System.Data.SqlClient;

//namespace ecn.communicator.classes.EmailWriter {
//    public class IISEmailWriter : IEmailWriter {		
//        #region IEmailWriter Members

//        public void Write(string pickupDirecctory, string fileName, EmailMessageProvider emailProvider, string subject, string body,string log) {

//            // Put 5 random characters in front to stop NTFS from balking						
//            using (StreamWriter fw = new StreamWriter(Path.Combine(pickupDirecctory, QuotedPrintable.RandomString(5,true) + fileName))) {
//                fw.Write(body);
//            }
//        }

		

//        #endregion
//    }
//}
