using System.Threading.Tasks;
using MaisPedMobile.Common.Models;
using MaisPedMobile.SyncServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaisPedMobile.SyncServer.Controllers
{
    [Route("api/[controller]")]
    public class CrawlerController : Controller
    {
        private CrawlerService CrawlerService { get; }

        public CrawlerController(CrawlerService crawlerService)
        {
            CrawlerService = crawlerService;
        }

        [HttpPost]
        public void PostResponse([FromBody] CrawlerSqlResponse response)
        {
            CrawlerService.ResponseArrived(response);
        }

        [HttpGet("test/{statement}")]
        [Authorize]
        public async Task<IActionResult> PostTest([FromRoute] string statement)
        {
            return Ok(await CrawlerService.DoSqlRequest(statement, "emp_1"));
        }
    }
}