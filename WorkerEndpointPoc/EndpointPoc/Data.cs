using System.Collections.Concurrent;
using Contracts;
using proactima.jsonobject;

namespace EndpointPoc
{
    public static class Data
    {
        private static readonly ConcurrentDictionary<string, JsonObject> Storage =
            new ConcurrentDictionary<string, JsonObject>();

        public static Response Load(string id)
        {
            return Storage.ContainsKey(id)
                ? Response.Create(Storage[id])
                : Response.Create(null);
        }

        public static JsonObject Store(string id, JsonObject json)
        {
            Storage[id] = json;
            return Storage[id];
        }
    }
}