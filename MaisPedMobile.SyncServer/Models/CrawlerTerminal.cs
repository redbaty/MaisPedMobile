namespace MaisPedMobile.SyncServer.Models
{
    public class CrawlerTerminal
    {
        public CrawlerTerminal(string connectionId, string @group)
        {
            ConnectionId = connectionId;
            Group = @group;
        }

        public string ConnectionId { get; }

        public string Group { get; }
    }
}