using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class BlastABMaster
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastABMaster;

        public static void ValidateObjects(ECN_Framework_Entities.Communicator.BlastABMaster abMaster, KMPlatform.Entity.User user)
        {
            //validate
            //BlastAB ab = new BlastAB();
            //ab.Validate(abMaster.BlastA, user);
            //ab.Validate(abMaster.BlastB, user);
        }

        public static void Save(ECN_Framework_Entities.Communicator.BlastABMaster abMaster, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_Entities.Communicator.Sample sample = null;
                if (abMaster.BlastA.SampleID == null || abMaster.BlastB.SampleID == null)
                {
                    sample = new ECN_Framework_Entities.Communicator.Sample();
                    sample.CreatedUserID = user.UserID;
                    sample.CustomerID = user.CustomerID;
                    ECN_Framework_BusinessLayer.Communicator.Sample.Save(sample, user);
                }
                else
                {
                    sample = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID(abMaster.BlastA.SampleID.Value, user);
                }
                abMaster.BlastA.SampleID = sample.SampleID;
                abMaster.BlastB.SampleID = sample.SampleID;
                
                //ValidateObjects(abMaster, user);
                BlastAB ab = new BlastAB();
                ab.Save(abMaster.BlastA, user);
                ab.Save(abMaster.BlastB, user);
                scope.Complete();
            }
        }

        public static void Save_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastABMaster abMaster, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_Entities.Communicator.Sample sample = null;
                if (abMaster.BlastA.SampleID == null || abMaster.BlastB.SampleID == null)
                {
                    sample = new ECN_Framework_Entities.Communicator.Sample();
                    sample.CreatedUserID = user.UserID;
                    sample.CustomerID = user.CustomerID;
                    ECN_Framework_BusinessLayer.Communicator.Sample.Save_NoAccessCheck(sample, user);
                }
                else
                {
                    sample = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID_NoAccessCheck(abMaster.BlastA.SampleID.Value, user);
                }
                abMaster.BlastA.SampleID = sample.SampleID;
                abMaster.BlastB.SampleID = sample.SampleID;

                //ValidateObjects(abMaster, user);
                BlastAB ab = new BlastAB();
                ab.Save_NoAccessCheck(abMaster.BlastA, user);
                ab.Save_NoAccessCheck(abMaster.BlastB, user);
                scope.Complete();
            }
        }
    }
}
