using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using MaisPedMobile.Common.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Pathoschild.Http.Client;
using Serilog;

namespace Mpm.ClientInjections
{
    public class Authentication
    {
        public Authentication(Addresses addresses)
        {
            Addresses = addresses;
        }

        private Addresses Addresses { get; }

        private string GenerateToken()
        {
            var direct = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ??
                                           throw new InvalidOperationException());
            var file = new FileInfo(Path.Combine(direct.FullName, "token.gen"));

            if (file.Exists)
            {
                return file.OpenText().ReadToEnd();
            }

            var newToken = Nanoid.Nanoid.Generate();
            File.WriteAllText(file.FullName, newToken);
            return newToken;
        }

        public async Task<string> GetToken()
        {
            using (var fluentClient = new FluentClient($"{Addresses.ServerUrl}api/"))
            {
                //TODO: Use hash
                return await (await fluentClient.GetAsync($"Authentication/terminal/hash/testing")).As<string>();
            }
        }
    }

    public class SignalRSqlCrawler
    {
        private ILogger Logger { get; }

        private Authentication Authentication { get; }

        public SignalRSqlCrawler(Addresses addresses, IDbConnection dbConnection, Authentication authentication)
        {
            Logger = Log.Logger.ForContext("SourceContext", "SignalR");
            Addresses = addresses;
            DbConnection = dbConnection;
            Authentication = authentication;
            CreateConnection();
        }

        private void CreateConnection()
        {
            Connection = new HubConnectionBuilder().WithUrl($"{Addresses.ServerUrl}hubs/crawler", options =>
            {
                var tok = Authentication.GetToken().Result;
                options.AccessTokenProvider = () => Task.FromResult(tok);
            }).Build();
            Connection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null) throw task.Exception;
                }

                Logger.Information("HubConnection established");

                Connection.On("sql", (CrawlerSqlRequest x) => OnRequestReceived(x));
            });
        }

        private void OnRequestReceived(CrawlerSqlRequest x)
        {
            Logger.Information("Executing {@sql}", x.Statement);
            var response = new CrawlerSqlResponse(x.Guid);

            if (x.Statement.StartsWith("SELECT"))
            {
                try
                {
                    var objects = DbConnection.Query(x.Statement).ToList();
                    response.QueryResult = JsonConvert.SerializeObject(objects, Formatting.None,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                    Logger.Information("Querying {@sql} resulted in {@objectCount} objects", x.Statement,
                        objects.Count);
                }
                catch (Exception e)
                {
                    response.HasFailed = true;
                    response.Exception = e;
                }
            }
            else
            {
                try
                {
                    var affectedRows = DbConnection.Execute(x.Statement);
                    Logger.Information("Querying {@sql} affected {@rowsAffected} rows", x.Statement,
                        affectedRows);
                }
                catch (Exception e)
                {
                    response.HasFailed = true;
                    response.Exception = e;
                }
            }

            using (var client = new HttpClient())
            {
                client.PostAsync($"{Addresses.ServerUrl}api/crawler", new JsonContent(response)).Wait();
            }
        }

        private IDbConnection DbConnection { get; }

        private HubConnection Connection { get; set; }

        private Addresses Addresses { get; }
    }
}