using System.Dynamic;
using System.Web;

namespace KM.Common.Web
{
    public class SessionWrapper : DynamicObject
    {
        private readonly HttpSessionStateBase _session;

        public SessionWrapper(HttpSessionStateBase session)
        {
            _session = session;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_session == null)
            {
                return false;
            }

            _session[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder,
            out object result)
        {
            if (_session[binder.Name] != null)
            {
                result = _session[binder.Name];
            }

            result = null;
            return true;
        }
    }
}
