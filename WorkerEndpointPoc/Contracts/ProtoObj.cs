using System.Collections.Generic;
using System.IO;
using proactima.jsonobject;
using ProtoBuf;

namespace Contracts
{
    [ProtoContract]
    public class ProtoObj
    {
        public ProtoObj()
        {
        }

        public ProtoObj(JsonObject obj)
        {
            JsonObj = obj;
        }

        [ProtoMember(1)]
        public object JsonObj { get; set; }

        public byte[] Serialize()
        {
            byte[] msgOut;

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                msgOut = stream.GetBuffer();
            }

            return msgOut;
        }

        public static ProtoObj Deserialize(byte[] message)
        {
            ProtoObj msgOut;

            using (var stream = new MemoryStream(message))
            {
                msgOut = Serializer.Deserialize<ProtoObj>(stream);
            }

            return msgOut;
        }
    }
}
