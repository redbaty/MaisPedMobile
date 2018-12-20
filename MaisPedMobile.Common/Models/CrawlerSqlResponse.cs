using System;

namespace MaisPedMobile.Common.Models
{
    public class CrawlerSqlResponse
    {
        public CrawlerSqlResponse()
        {

        }

        public CrawlerSqlResponse(string guid)
        {
            Guid = guid;
        }

        public string Guid { get; set; }

        public bool HasFailed { get; set; }

        public Exception Exception { get; set; }

        public string QueryResult { get; set; }
    }
}