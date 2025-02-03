using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveData([FromBody] IEnumerable<Dictionary<int, string>> data)
        {
            var request = JsonSerializer.Serialize(data);
            await _dataService.SaveDataAsync(data);
            var response = JsonSerializer.Serialize(new { Message = "Data saved successfully" });

            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetData([FromQuery] int? codeFilter = null)
        {
            var data = await _dataService.GetDataAsync(codeFilter);
            var response = JsonSerializer.Serialize(data);

            return Ok(response);
        }
    }
}
