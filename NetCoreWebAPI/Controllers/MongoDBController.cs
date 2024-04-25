using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NetCoreWebAPI.Services.MongoDB;
using NetCoreWebAPI.Services.MongoDB.Model;
using NetCoreWebAPI.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoDBController : ControllerBase
    {
        private readonly IMongoDBService _mongoDBService;
        public MongoDBController(IMongoDBService mongoDBService) {
            _mongoDBService = mongoDBService;
        }

        [Authorize(Policy = "api.read", Roles = "Admin")]
        [HttpGet]
        public IActionResult FindContainsName(string name)
        {
            var data = _mongoDBService.FindContainsName(name);
            return Ok(data);
        }

        // GET api/<MongoDBController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "api.read", Roles = "User")]
        public async Task<IActionResult> FindById(string id)
        {
            var data = await _mongoDBService.FindById(id);
            return Ok(data);
        }

        // POST api/<MongoDBController>
        [HttpPost]
        [Authorize(Policy = "api.create", Roles = "Admin")]
        public async Task<IActionResult> Post(string name, double price)
        {
            var userid = UserUtil.GetUserId(User);
            await _mongoDBService.InsertOne(new Product()
            {
                UserId = userid,
                Name = name,
                Price = price,
                ExpirationDate = DateTime.Now.AddYears(2),
                LastUpdateBy = userid
            });
            return Ok("Successfull");
        }

        // PUT api/<MongoDBController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "api.update", Roles = "Admin")]
        public async Task<IActionResult> Put(string id, string name, double price,DateTime ExpirationDate)
        {
            var userid = UserUtil.GetUserId(User);
            await _mongoDBService.UpdateOne(new Product()
            {
                Id = new ObjectId(id),
                UserId = userid,
                Name = name,
                Price = price,
                ExpirationDate = ExpirationDate,
                LastUpdateBy = userid
            });
            return Ok("Successfull");
        }

        // DELETE api/<MongoDBController>/5
        [Authorize(Policy = "api.delete", Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mongoDBService.DeleteOne(id);
            return Ok("Successfull");
        }
    }
}
