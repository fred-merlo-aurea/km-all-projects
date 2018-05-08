using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_Common.Objects;
using Entities = ECN_Framework_Entities.Communicator;
using KMEntities = KMPlatform.Entity;
using KMEnums = KMPlatform.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public abstract class BlastExtendedAbstract : BlastAbstract
    {
        internal override int Save(Entities.BlastAbstract blast, KMEntities.User user)
        {
            Validate(blast, user);

            if (!HasPermission(KMEnums.Access.Edit, user))
            {
                throw new SecurityException();
            }

            if (!AccessCheck.CanAccessByCustomer(blast, user))
            {
                throw new SecurityException();
            }

            Blast.Save(blast, user);
            if (blast.Fields != null)
            {
                blast.Fields.BlastID = blast.BlastID;
                blast.Fields.CustomerID = blast.CustomerID;
                blast.Fields.CreatedUserID = blast.CreatedUserID;
                blast.Fields.UpdatedUserID = blast.UpdatedUserID;
                BlastFields.Save(blast.Fields, user);
            }

            return blast.BlastID;
        }

        internal override int Save_NoAccessCheck(Entities.BlastAbstract blast, KMEntities.User user)
        {
            Validate_NoAccessCheck(blast, user);

            Blast.Save(blast, user);
            if (blast.Fields != null)
            {
                blast.Fields.BlastID = blast.BlastID;
                blast.Fields.CustomerID = blast.CustomerID;
                blast.Fields.CreatedUserID = blast.CreatedUserID;
                blast.Fields.UpdatedUserID = blast.UpdatedUserID;
                BlastFields.Save_NoAccessCheck(blast.Fields, user);
            }

            return blast.BlastID;
        }
    }
}
