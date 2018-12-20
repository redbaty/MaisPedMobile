using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Mpm.ClientInjections
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) : this(JsonConvert.SerializeObject(obj))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public JsonContent(string content) : base(content)
        {
        }

        public JsonContent(string content, Encoding encoding) : base(content, encoding)
        {
        }

        public JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
        {
        }
    }
}