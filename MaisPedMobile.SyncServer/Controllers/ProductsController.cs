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
    public class ProductsController : ControllerBase
    {
        private CrawlerService _crawlerService;

        public ProductsController(CrawlerService crawlerService)
        {
            _crawlerService = crawlerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var res = await _crawlerService.DoSqlRequest("SELECT FIRST 100 * FROM PRODUTOS_EMP;", "emp_1");
            if (res.HasFailed)
            {
                return BadRequest(res.Exception);
            }

            var produtosEmps = JsonConvert.DeserializeObject<List<ProdutosEmp>>(res.QueryResult);
            var toReturn = new List<Product>();

            foreach (var i in produtosEmps)
            {
                var crawlerSqlResponse = _crawlerService
                    .DoSqlRequest($"SELECT DESCRICPRODUTO FROM PRODUTOS WHERE CODIGOPRODUTO={i.Codigoproduto};",
                        "emp_1").Result;

                if (crawlerSqlResponse.HasFailed)
                {
                    return BadRequest(crawlerSqlResponse.Exception);
                }

                var deserializeObject = JsonConvert.DeserializeObject<List<Produtos>>(crawlerSqlResponse.QueryResult).FirstOrDefault();

                toReturn.Add(i.ToProduct(deserializeObject));
            }


            return Ok(toReturn);
        }
    }
}