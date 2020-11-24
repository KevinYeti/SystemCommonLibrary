﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Serialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public sealed class NoSerializeAttribute : System.Attribute
    {

    }
}
