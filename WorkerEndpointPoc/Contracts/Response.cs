using proactima.jsonobject;

namespace Contracts
{
    public class Response
    {
        public JsonObject Json { get; set; }

        public bool Success
        {
            get { return Status < 300; }
        }

        public int Status { get; set; }

        public static Response Create(JsonObject json)
        {
            return json == null
                ? new Response {Status = 404}
                : new Response {Json = json, Status = 200};
        }
    }
}