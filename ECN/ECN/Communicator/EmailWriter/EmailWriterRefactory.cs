//using System;

//namespace ecn.communicator.classes.EmailWriter {
	
	
	
//    public class EmailWriterRefactory {
//        public static IEmailWriter GetInstance() {
//            object o = System.Configuration.ConfigurationManager.AppSettings["EmailWriterType"];
//            string type = o == null? string.Empty: Convert.ToString(o);
//            switch(type.ToLower()) {
//                case "ironport":
//                    return IronPortEMailWriterInstance;			
//                default:					
//                    return new IISEmailWriter();
//            }
//        }

//        private static IEmailWriter _ironPortEmailWriter = null;
//        public static IEmailWriter IronPortEMailWriterInstance {
//            get {
//                if (_ironPortEmailWriter == null) {
//                    _ironPortEmailWriter = new IronPortEMailWriter();					
//                }
//                return (_ironPortEmailWriter);
//            }
			
//        }

//    }
//}