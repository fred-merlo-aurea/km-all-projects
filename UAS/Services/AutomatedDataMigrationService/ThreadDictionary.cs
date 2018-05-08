using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ADMS
{
    public class ThreadDictionary
    {
        public static ConcurrentDictionary<int, string> activeThreads = new ConcurrentDictionary<int, string>();
        public static List<Thread> threads = new List<Thread>();

        public static void Set(int key, string value)
        {
            if (activeThreads.ContainsKey(key))
            {
                activeThreads[key] = value;
            }
            else
            {
                activeThreads.AddOrUpdate(key, value, (k, v) => v);
            }
        }

        public static List<KeyValuePair<int,string>> Get()
        {
            return activeThreads.ToList();
        }

        public static void Remove(int key)
        {
            string output = string.Empty;
            activeThreads.TryRemove(key, out output);
        }

    }
}
