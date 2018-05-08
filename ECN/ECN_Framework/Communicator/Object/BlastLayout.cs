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
//    public class BlastLayout : BlastAbstract
//    {
//        public BlastNew _Blast;

//        public BlastLayout()
//            : base()
//        {
//            BlastTypeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Layout;
//        }

//        public override string Send()
//        {
//            return "This is a Layout Blast";
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
