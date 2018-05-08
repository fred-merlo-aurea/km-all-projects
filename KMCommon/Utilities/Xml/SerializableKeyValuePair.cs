﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KM.Common.Utilities.Xml
{
    public class SerializableKeyValuePair<TKey, TValue>
    {
        public SerializableKeyValuePair() { }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key
        { get; set; }

        public TValue Value
        { get; set; }
    }
}
