﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string GetXamlResourceDictionary();
    }
}