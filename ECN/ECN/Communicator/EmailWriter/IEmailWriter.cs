//using System;
//using ecn.common.classes;
//using System.Data.SqlClient;

//namespace ecn.communicator.classes.EmailWriter {
	
//    /// Interface for writting email to a email server. Need to support two type of email servers:
//    /// 1. IIS, need pickupDirectory, fileName, and body as parameters
//    /// 2. IronPort need emailprovider, subject, and body as parameters.
	
//    public interface IEmailWriter {
//        //void Write(string pickupDirecctory, string fileName, EmailMessageProvider emailProvider, string subject, string body);		
//        void Write(string pickupDirecctory, string fileName, EmailMessageProvider emailProvider, string subject, string body,string log);
		
//    }
//}
