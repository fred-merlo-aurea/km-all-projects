using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class BlastSocial : BlastAbstract
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastSocial;

        public override BlastType BlastType
        {
            get
            {
                return BlastType.Social;
            }
        }

        public override bool HasPermission(KMPlatform.Enums.Access rights, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.HasPermission;
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(new ECNError(Entity, Method, "Not Implemented"));
            throw new ECNException(errorList);
        }

        public override string Send(ref ECN_Framework_Entities.Communicator.BlastAbstract blast)
        {
            return "This is a Social Blast";
        }

        public override void Validate(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(new ECNError(Entity, Method, "Not Implemented"));
            throw new ECNException(errorList);
        }

        public override void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(new ECNError(Entity, Method, "Not Implemented"));
            throw new ECNException(errorList);
        }

        internal override int Save(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(new ECNError(Entity, Method, "Not Implemented"));
            throw new ECNException(errorList);
            return blast.BlastID;
        }

        internal override int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(new ECNError(Entity, Method, "Not Implemented"));
            throw new ECNException(errorList);
            return blast.BlastID;
        } 
    }
}
