using System;
using System.Collections.Generic;
using System.Linq;

namespace UAS_WS
{
    public static class AuthenticationCache
    {
        public static Dictionary<FrameworkUAS.Service.Authentication, DateTime> AuthObjects { get; set; }
    }
}
