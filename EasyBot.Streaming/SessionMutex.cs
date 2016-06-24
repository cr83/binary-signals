using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBot.Streaming
{
    public static class SessionMutex
    {
        private static ConcurrentDictionary<string, object> _sessionMutex = new ConcurrentDictionary<string, object>();

        public static object Get(string sessionId)
        {
            var mutex = new object();
            return _sessionMutex.GetOrAdd(sessionId, mutex);
        }

        public static object Remove(string sessionId)
        {
            object mutex;
            _sessionMutex.TryRemove(sessionId, out mutex);
            return mutex;
        }
    }
}
