//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using ECN_Framework.Communicator.Abstract;
//using ECN_Framework.Communicator.Entity;

//namespace ECN_Framework.Communicator.Object
//{
//    public class BlastSocial : BlastAbstract
//    {
//        public BlastNew _Blast;

//        public BlastSocial()
//            : base()
//        {
//            BlastTypeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Social;
//            BlastType = "social";
//            StatusCodeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending;
//            StatusCode = "pending";
//            TestBlast = "n";
//        }

//        public override string Send()
//        {
//            return "This is a Social Blast";
//        }

//        public override bool Validate()
//        {
//            return false;
//        }

        
//        public override bool Save()
//        {
//            return false;
//        }

//        public override bool Save(SqlCommand command)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
