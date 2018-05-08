using System;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public abstract class BlastAbstract : Blast
    {
        public abstract BlastType BlastType { get; }

        public abstract string Send(ref ECN_Framework_Entities.Communicator.BlastAbstract blast);

        public abstract bool HasPermission(KMPlatform.Enums.Access rights, KMPlatform.Entity.User user);

        internal abstract int Save(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user);

        internal abstract int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user);
        //public abstract bool Save(ref ECN_Framework_Entities.Communicator.BlastAbstract blast, ref SqlCommand command);

        public abstract void Validate(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user);

        public abstract void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user);

    }
}
