using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MaisPedMobile.SyncServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MaisPedMobile.SyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly PedMobileContext _context;
        private readonly AppSettings _appSettings;

        public AuthenticationController(PedMobileContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;

            if (!_context.Salesman.Any())
            {
                var salesman = new Salesman
                {
                    Name = "Marcao",
                    Phone = new Phone
                    {
                        Hash = "browser"
                    }
                };
                _context.Salesman.Add(salesman);

                _context.Enterprises.Add(new Enterprise
                {
                    Salesman = new List<Salesman> {salesman},
                    Cnpj = "000",
                    TerminalKey = "testing"
                });

                _context.SaveChanges();
            }
        }

        [HttpGet("terminal/hash/{terminalHash}")]
        public ActionResult<string> GetByTerminalHash(string terminalHash)
        {
            var user = _context.Enterprises.SingleOrDefault(i => i.TerminalKey == terminalHash);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        [HttpGet("phone/hash/{phoneHash}")]
        public ActionResult<string> GetByPhoneHash(string phoneHash)
        {
            var user = _context.Salesman.Include(i => i.Phone).SingleOrDefault(i => i.Phone.Hash == phoneHash);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.MobilePhone, string.Empty)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}