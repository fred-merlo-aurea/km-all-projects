using System;
using System.Collections.Generic;
using FrameworkUAS.Service;

namespace WebServiceFramework
{
    public static class AuthenticationCache
    {
        public static Dictionary<Authentication, DateTime> AuthObjects { get; set; }
    }
}
