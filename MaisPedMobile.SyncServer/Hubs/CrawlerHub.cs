using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MaisPedMobile.SyncServer.Models;
using MaisPedMobile.SyncServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MaisPedMobile.SyncServer.Hubs
{
    public class CrawlerHub : Hub
    {
        public CrawlerHub(IOptions<AppSettings> appSettings, CrawlerService crawlerService)
        {
            AppSettings = appSettings;
            CrawlerService = crawlerService;
        }

        private IOptions<AppSettings> AppSettings { get; }
        private CrawlerService CrawlerService { get; }

        public override Task OnConnectedAsync()
        {
            return Task.Run(() =>
            {
                var claims = Context.GetHttpContext().GetClaims(AppSettings.Value.Secret);

                if (claims == null)
                {
                    Context.Abort();
                }
                else
                {
                    var id = claims.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value;
                    var @group = $"emp_{id}";
                    Groups.AddToGroupAsync(Context.ConnectionId, @group);
                    CrawlerService.AddTerminal(new CrawlerTerminal(Context.ConnectionId, @group));
                }
            });
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.Run(() =>
            {
                CrawlerService?.RemoveTerminal(Context.ConnectionId);
            });
        }
    }

    public static class HttpContextExtensions
    {
        public static ClaimsPrincipal GetClaims(this HttpContext context, string key)
        {
            string requestHeader;

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                if (context.Request.Query.ContainsKey("access_token"))
                {
                    requestHeader = context.Request.Query["access_token"];
                }

                return null;
            }
            else
            {
                requestHeader = context.Request.Headers["Authorization"].ToString();
            }
       

            if (string.IsNullOrEmpty(requestHeader)) return null;

            var token = requestHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            return handler.ValidateToken(token, validations, out _);
        }
    }
}