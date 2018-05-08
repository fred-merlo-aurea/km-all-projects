using System;
using KM.Common.Exceptions;
using EntityEnum = FrameworkUAD.Object.Enums.Entity;
using static FrameworkUAD.Object.Enums;

namespace FrameworkUAD.Object
{
    [Serializable]
    public class UADError : EntityError<EntityEnum, Method>
    {
        public UADError()
        {
        }

        public UADError(EntityEnum entity, Method method, string error)
            : base(entity, method, error)
        {
        }
    }
}
