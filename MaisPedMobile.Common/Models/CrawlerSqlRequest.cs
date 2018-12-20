namespace MaisPedMobile.Common.Models
{
    public class CrawlerSqlRequest
    {
        public CrawlerSqlRequest()
        {
        }

        public CrawlerSqlRequest(string guid, string statement)
        {
            Guid = guid;
            Statement = statement;
        }

        public string Guid { get; set; }

        public string Statement { get; set; }
    }
}