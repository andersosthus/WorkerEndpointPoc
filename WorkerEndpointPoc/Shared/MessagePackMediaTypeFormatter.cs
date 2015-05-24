using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MsgPack;
using MsgPack.Serialization;

namespace EndpointPoc
{
    public class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        private const string Mime = "application/x-msgpack";

        private readonly Func<Type, bool> _isAllowedType = t =>
        {
            if (!t.IsAbstract && !t.IsInterface && !t.IsNotPublic)
                return true;

            if (typeof (IEnumerable).IsAssignableFrom(t))
                return true;

            return false;
        };

        public MessagePackMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Mime));
        }

        public override bool CanReadType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return _isAllowedType(type);
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return _isAllowedType(type);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content,
            TransportContext transportContext)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (stream == null) throw new ArgumentNullException("stream");

            var tcs = new TaskCompletionSource<object>();

            var serializer = MessagePackSerializer.Get(type);
            serializer.Pack(stream, value);

            tcs.SetResult(null);
            return tcs.Task;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            var tcs = new TaskCompletionSource<object>();
            if (content.Headers != null && content.Headers.ContentLength == 0) return null;
            try
            {
                var serializer = MessagePackSerializer.Get(type);
                object result;

                using (var unpacker = Unpacker.Create(stream))
                {
                    unpacker.Read();
                    result = serializer.UnpackFrom(unpacker);
                }
                tcs.SetResult(result);
            }
            catch (Exception e)
            {
                if (formatterLogger == null) throw;
                formatterLogger.LogError(String.Empty, e.Message);
                tcs.SetResult(GetDefaultValueForType(type));
            }

            return tcs.Task;
        }
    }
}