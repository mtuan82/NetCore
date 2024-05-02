using Core.Providers.PostgreSQL.Entity;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebAPI.Services.PostgreSQL;

namespace NetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostgreSQLController : ControllerBase
    {
        private readonly IPostgreSQLService postgreSQLService;
        public PostgreSQLController(IPostgreSQLService _postgreSQLService)
        {
            postgreSQLService = _postgreSQLService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStoresByName(string name)
        {
            return Ok(postgreSQLService.GetStoresByName(name));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var customer = postgreSQLService.GetStoreById(id);
            return Ok(customer == null ? "Store not found" : customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] Store value)
        {
            if (value == null)
            {
                return BadRequest("Store not found");
            }
            postgreSQLService.CreateStore(value);
            return Ok("create successful");
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Store value)
        {
            if (value == null)
            {
                return BadRequest("Store not found");
            }
            else if (!postgreSQLService.UpdateStore(value))
            {
                return BadRequest("Store not found");
            }
            return Ok("update successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(postgreSQLService.DeleteStore(id));
        }
    }
}
