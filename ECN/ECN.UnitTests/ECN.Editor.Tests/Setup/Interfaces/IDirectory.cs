﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN.Editor.Tests.Setup.Interfaces
{
    public interface IDirectory
    {
        string[] GetFiles(string path, string pattern);
    }
}
