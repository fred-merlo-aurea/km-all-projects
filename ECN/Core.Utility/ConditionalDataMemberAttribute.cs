﻿using System;
using System.Linq;

namespace Core.Utilities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ConditionalDataMemberAttribute : Attribute
    {
        public bool IsClientServiceVisible { get; set; }
    }
}
