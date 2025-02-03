using TestTask.Models;

namespace TestTask.Repositories.Interfaces
{
    public interface IDataRepository : IBaseRepository<DataItem>
    {
        Task ClearDataAsync();
        Task<IEnumerable<DataItem>> GetDataAsync(int? codeFilter = null);
    }
}
