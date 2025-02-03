using TestTask.Models;

namespace TestTask.Services.Interfaces
{
    public interface IDataService
    {
        Task SaveDataAsync(IEnumerable<Dictionary<int, string>> data);
        Task<IEnumerable<DataItem>> GetDataAsync(int? codeFilter = null);
    }
}
