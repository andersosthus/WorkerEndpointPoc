using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using proactima.jsonobject;

namespace Contracts
{
    [DataContract]
    public class ProtoObjSurrogate
    {
        [DataMember(Order = 1)]
        public byte[] JsonObj { get; set; }

        public static implicit operator ProtoObj(ProtoObjSurrogate suggorage)
        {
            return suggorage == null ? null : new ProtoObj
            {
                JsonObj = Deserialize(suggorage.JsonObj)
            };
        }

        public static implicit operator ProtoObjSurrogate(ProtoObj source)
        {
            return source == null ? null : new ProtoObjSurrogate
            {
                JsonObj = Serialize(source.JsonObj)
            };
        }

        private static byte[] Serialize(object o)
        {
            if (o == null)
                return null;

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, o);
                return ms.ToArray();
            }
        }

        private static object Deserialize(byte[] b)
        {
            if (b == null)
                return null;

            using (var ms = new MemoryStream(b))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }
    }
}
