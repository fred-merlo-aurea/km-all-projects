﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace UAD_WS
{
    public static class AuthenticationCache
    {
        public static Dictionary<FrameworkUAS.Service.Authentication, DateTime> AuthObjects { get; set; }
    }
}