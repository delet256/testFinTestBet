using TestTask.Models;
using TestTask.Repositories.Interfaces;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveDataAsync(IEnumerable<Dictionary<int, string>> data)
        {
            var dataItems = data.Select(d => new DataItem
            {
                Code = d.Keys.First(),
                Value = d.Values.First()
            }).OrderBy(d => d.Code);

            await _repository.ClearDataAsync();
            await _repository.AddRangeAsync(dataItems);
        }

        public async Task<IEnumerable<DataItem>> GetDataAsync(int? codeFilter = null)
        {
            return await _repository.GetDataAsync(codeFilter);
        }
    }
}
