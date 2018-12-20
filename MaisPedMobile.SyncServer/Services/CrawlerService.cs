using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MaisPedMobile.Common.Models;
using MaisPedMobile.SyncServer.Hubs;
using MaisPedMobile.SyncServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace MaisPedMobile.SyncServer.Services
{
    public class CrawlerService
    {
        private IHubContext<CrawlerHub> HubContext { get; }

        private List<CrawlerTerminal> Terminals { get; } = new List<CrawlerTerminal>();

        private Dictionary<string, CrawlerSqlResponse> ResponsesCompleted { get; } =
            new Dictionary<string, CrawlerSqlResponse>();

        private Dictionary<string, ManualResetEvent> WaitingRequests { get; } =
            new Dictionary<string, ManualResetEvent>();

        public CrawlerService(IHubContext<CrawlerHub> hubContext)
        {
            HubContext = hubContext;
        }

        public void AddTerminal(CrawlerTerminal terminal)
        {
            if (Terminals.Any(i => i.ConnectionId == terminal.ConnectionId) &&
                Terminals.Any(i => i.Group == terminal.Group))
            {
                return;
            }

            Terminals.Add(terminal);
        }

        public void RemoveTerminal(string connectionId)
        {
            var x = Terminals.SingleOrDefault(i => i.ConnectionId == connectionId);

            if (x != null)
            {
                Terminals.Remove(x);
            }
        }

        public bool IsTerminalAvailable(string group)
        {
            return Terminals.Any(i => i.Group == group);
        }

        public IEnumerable<CrawlerTerminal> GetTerminals()
        {
            return Terminals.ToList();
        }

        public void ResponseArrived(CrawlerSqlResponse sqlResponse)
        {
            if (WaitingRequests.ContainsKey(sqlResponse.Guid))
            {
                var resetEvent = WaitingRequests[sqlResponse.Guid];
                WaitingRequests.Remove(sqlResponse.Guid);
                ResponsesCompleted.Add(sqlResponse.Guid, sqlResponse);
                resetEvent.Set();
            }
        }

        public Task<CrawlerSqlResponse> DoSqlRequest(string statement, string groupName)
        {
            var guid = Nanoid.Nanoid.Generate();

            if (!IsTerminalAvailable(groupName))
            {
                return Task.FromResult(new CrawlerSqlResponse(guid)
                    {HasFailed = true, Exception = new CrawlerNotFound("No crawlers available")});
            }

            return Task.Run(() =>
            {
                var request = new CrawlerSqlRequest(guid, statement);
                var resetEvent = new ManualResetEvent(false);
                WaitingRequests.Add(guid, resetEvent);

                HubContext.Clients.Group(groupName).SendCoreAsync("sql", new object[] {request});

                resetEvent.WaitOne(TimeSpan.FromMinutes(10));
                resetEvent.Dispose();

                if (ResponsesCompleted.ContainsKey(guid))
                {
                    var crawlerSqlResponse = ResponsesCompleted[guid];
                    ResponsesCompleted.Remove(guid);
                    return crawlerSqlResponse;
                }

                return new CrawlerSqlResponse(guid)
                    {HasFailed = true, Exception = new TimeoutException("The crawler didn't respond in time")};
            });
        }
    }
}