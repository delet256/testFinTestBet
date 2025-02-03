using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using TestTask.Controllers;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Tests.Controllers
{
    public class DataControllerTests
    {
        private readonly Mock<IDataService> _mockService;
        private readonly DataController _controller;

        public DataControllerTests()
        {
            _mockService = new Mock<IDataService>();
            _controller = new DataController(_mockService.Object);
        }

        [Fact]
        public async Task SaveData_ReturnsOkResult()
        {
            // Arrange
            var data = new List<Dictionary<int, string>>
            {
                new Dictionary<int, string> { { 1, "Value1" } },
                new Dictionary<int, string> { { 2, "Value2" } }
            };

            _mockService.Setup(service => service.SaveDataAsync(It.IsAny<IEnumerable<Dictionary<int, string>>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SaveData(data);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(okResult.Value.ToString());
            Assert.Equal("Data saved successfully", response["Message"]);
        }

        [Fact]
        public async Task GetData_ReturnsOkResult()
        {
            // Arrange
            var dataItems = new List<DataItem>
            {
                new DataItem { Id = 1, Code = 1, Value = "Value1" },
                new DataItem { Id = 2, Code = 2, Value = "Value2" }
            };

            _mockService.Setup(service => service.GetDataAsync(It.IsAny<int?>())).ReturnsAsync(dataItems);

            // Act
            var result = await _controller.GetData();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<List<DataItem>>(okResult.Value.ToString());
            Assert.Equal(2, response.Count);
        }
    }
}