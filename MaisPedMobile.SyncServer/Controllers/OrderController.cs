using System;
using MaisPedMobile.Com;
using Microsoft.AspNetCore.Mvc;

namespace MaisPedMobile.SyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            throw new NotImplementedException();
        }
    }
}