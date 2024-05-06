using Core.Providers.MSSQL.Entity;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebAPI.Services.MSSQL;

namespace NetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MSSQLController : ControllerBase
    {
        private readonly IMSSQLService msSQLService;
        public MSSQLController(IMSSQLService _msSQLService)
        {
            msSQLService = _msSQLService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStoresByName(string name)
        {
            return Ok(msSQLService.GetStoresByName(name));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var customer = msSQLService.GetStoreById(id);
            return Ok(customer == null ? "Store not found" : customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] Store value)
        {
            if (value == null)
            {
                return BadRequest("Store not found");
            }
            msSQLService.CreateStore(value);
            return Ok("create successful");
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Store value)
        {
            if (value == null)
            {
                return BadRequest("Store not found");
            }
            else if (!msSQLService.UpdateStore(value))
            {
                return BadRequest("Store not found");
            }
            return Ok("update successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(msSQLService.DeleteStore(id));
        }
    }
}
