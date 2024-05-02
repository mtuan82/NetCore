using Core.Providers.MySQL.Entity;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebAPI.Services.MySQL;

namespace NetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MySQLController : ControllerBase
    {
        private readonly IMySQLService mySQLService;
        public MySQLController(IMySQLService _mySQLService) {
            mySQLService = _mySQLService;
        }

        // GET: api/<MySQLController>
        [HttpGet]
        public async Task<IActionResult> GetCustomersByName(string name)
        {
            return Ok(mySQLService.GetCustomersByName(name));
        }

        // GET api/<MySQLController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = mySQLService.GetCustomerById(id);
            return Ok(customer == null?"Customer not found": customer);
        }

        // POST api/<MySQLController>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer value)
        {
            if (value == null)
            {
                return BadRequest("Customer not found");
            }
            mySQLService.CreateCustomer(value);
            return Ok("create successful");
        }

        // PUT api/<MySQLController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Customer value)
        {
            if (value == null)
            {
                return BadRequest("Customer not found");
            }
            else if (!mySQLService.UpdateCustomer(value))
            {
                return BadRequest("Customer not found");
            }
            return Ok("update successful");
        }

        // DELETE api/<MySQLController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(mySQLService.DeleteCustomer(id));
        }
    }
}
