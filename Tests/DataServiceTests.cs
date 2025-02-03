using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Models;
using TestTask.Repositories.Interfaces;
using TestTask.Services;
using Xunit;

namespace TestTask.Tests.Services
{
    public class DataServiceTests
    {
        private readonly Mock<IDataRepository> _mockRepo;
        private readonly DataService _dataService;

        public DataServiceTests()
        {
            _mockRepo = new Mock<IDataRepository>();
            _dataService = new DataService(_mockRepo.Object);
        }

        [Fact]
        public async Task SaveDataAsync_ClearsAndAddsData()
        {
            // Arrange
            var data = new List<Dictionary<int, string>>
            {
                new Dictionary<int, string> { { 1, "Value1" } },
                new Dictionary<int, string> { { 2, "Value2" } }
            };

            _mockRepo.Setup(repo => repo.ClearDataAsync()).Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<DataItem>>())).Returns(Task.CompletedTask);

            // Act
            await _dataService.SaveDataAsync(data);

            // Assert
            _mockRepo.Verify(repo => repo.ClearDataAsync(), Times.Once);
            _mockRepo.Verify(repo => repo.AddRangeAsync(It.Is<IEnumerable<DataItem>>(items =>
                items.Count() == 2 &&
                items.Any(i => i.Code == 1 && i.Value == "Value1") &&
                items.Any(i => i.Code == 2 && i.Value == "Value2")
            )), Times.Once);
        }

        [Fact]
        public async Task GetDataAsync_ReturnsFilteredData()
        {
            // Arrange
            var dataItems = new List<DataItem>
            {
                new DataItem { Id = 1, Code = 1, Value = "Value1" },
                new DataItem { Id = 2, Code = 2, Value = "Value2" }
            };

            _mockRepo.Setup(repo => repo.GetDataAsync(It.IsAny<int?>())).ReturnsAsync(dataItems);

            // Act
            var result = await _dataService.GetDataAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(repo => repo.GetDataAsync(1), Times.Once);
        }
    }
}