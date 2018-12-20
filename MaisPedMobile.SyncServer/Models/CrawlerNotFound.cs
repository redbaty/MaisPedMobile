using System;
using System.Runtime.Serialization;

namespace MaisPedMobile.SyncServer.Models
{
    public class CrawlerNotFound : Exception
    {
        public CrawlerNotFound()
        {
        }

        protected CrawlerNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CrawlerNotFound(string message) : base(message)
        {
        }

        public CrawlerNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}