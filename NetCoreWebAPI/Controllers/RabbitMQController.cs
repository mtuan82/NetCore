using Microsoft.AspNetCore.Mvc;
using NetCoreWebAPI.Services.RabbitMQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private IRabbitMQService _rabbitMQService;
        public RabbitMQController(IRabbitMQService RabbitMQService) {
            _rabbitMQService = RabbitMQService;
        }

        // POST api/<RabbitMQController>
        [HttpPost]
        public async Task<IActionResult> pushQueue([FromQuery]string queueName, [FromBody] string message)
        {
            _rabbitMQService.pushQueue(queueName, message);
            return Ok("Push Successful");
        }

        [HttpGet]
        public async Task<IActionResult> popQueue([FromQuery] string queueName, [FromQuery] ushort maxMessage)
        {
            List<string> data = _rabbitMQService.popQueue(queueName, maxMessage);
            return Ok(data);
        }

        // DELETE api/<RabbitMQController>
        [HttpDelete]
        public async Task<IActionResult> deleteQueue(string queueName)
        {
            _rabbitMQService.deleteQueue(queueName);
            return Ok("Delete Queue Successful");
        }
    }
}
