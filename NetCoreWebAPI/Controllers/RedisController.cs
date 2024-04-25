using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebAPI.Services.Redis;
using NetCoreWebAPI.Services.Redis.Model;
using NetCoreWebAPI.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private IRedisService redisService;
        private const string key = "Transit";
        public RedisController(IRedisService _redisService) {
            redisService = _redisService;
        }

        // GET api/<RedisController>/5
        [HttpGet]
        [Authorize(Policy = "api.read", Roles = "Admin")]
        public async Task<IActionResult> GetPrices()
        {
            var userid = UserUtil.GetUserId(User);
            return Ok(await redisService.Get(userid));
        }

        // POST api/<RedisController>
        [HttpPost]
        [Authorize(Policy = "api.create", Roles = "Admin")]
        public async Task<IActionResult> CreateOrUpdate(List<Price> prices)
        {
            var userid = UserUtil.GetUserId(User);
            foreach (var pr in prices)
            {
                pr.ProviderId = userid;
                pr.LastUpdateBy = userid;
            }
            await redisService.CreateOrUpdate(prices, userid);
            return Ok("Successful");
        }

        // DELETE api/<RedisController>/5
        [HttpDelete]
        [Authorize(Policy = "api.delete", Roles = "Admin")]
        public async Task<IActionResult> DeletePrice()
        {
            var userid = UserUtil.GetUserId(User);
            await redisService.Delete(userid);
            return Ok("delete successful provider " + userid);
        }
    }
}
