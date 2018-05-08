using System;
using KM.Common.Exceptions;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_Common.Objects
{
    [Serializable]
    public class ECNError : EntityError<Entity, Method>
    {
        public ECNError()
        {
        }

        public ECNError(Entity entity, Method method, string error)
            : base(entity, method, error)
        {
        }
    }

    [Serializable]
    public partial class ECNWarning
    {
        public Enums.Entity Entity { get; set; }
        public Enums.Method Method { get; set; }
        public string WarningMessage { get; set; }

        public ECNWarning()
        {
        }

        public ECNWarning(Enums.Entity entity, Enums.Method method, string error)
        {
            Entity = entity;
            Method = method;
            WarningMessage = error;
        }
    }

    /* REMOVE -  */
    [Serializable]
    public class ValidationError
    {
        public string FieldName { get; set; }

        public string ErrorMessage { get; set; }

        public ValidationError()
        {
        }

        public ValidationError(string fieldName, string error)
        {
            FieldName = fieldName;

            ErrorMessage = error;
        }
    }
}
