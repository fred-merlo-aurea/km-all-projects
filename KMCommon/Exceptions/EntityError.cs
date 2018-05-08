using System;

namespace KM.Common.Exceptions
{
    [Serializable]
    public abstract class EntityError<TEntityEnum, TEntityMethodEnum>
        where TEntityEnum : struct
        where TEntityMethodEnum : struct
    {
        public TEntityEnum Entity { get; set; }

        public TEntityMethodEnum Method { get; set; }

        public string ErrorMessage { get; set; }

        protected EntityError()
        {
        }

        protected EntityError(TEntityEnum entity, TEntityMethodEnum method, string error)
        {
            Entity = entity;
            Method = method;
            ErrorMessage = error;
        }
    }
}
