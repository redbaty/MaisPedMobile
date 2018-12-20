using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaisPedMobile.Com;
using MaisPedMobile.SyncServer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGE32.DAL.EnterpriseModels;

namespace MaisPedMobile.SyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private CrawlerService _crawlerService;

        public PeopleController(CrawlerService crawlerService)
        {
            _crawlerService = crawlerService;
        }

        // GET: api/People
        [HttpGet]
        [Produces(typeof(IEnumerable<Person>))]
        public async Task<IActionResult> Get()
        {
            var res = await _crawlerService.DoSqlRequest("SELECT FIRST 100 * FROM PESSOA WHERE PESSOATIPO='C'", "emp_1");
            if (res.HasFailed)
            {
                return BadRequest(res.Exception);
            }

            return Ok(JsonConvert.DeserializeObject<List<Pessoa>>(res.QueryResult).Select(i => i.ToPerson()).OrderBy(i => i.Name).ToList());
        }
    }
}
