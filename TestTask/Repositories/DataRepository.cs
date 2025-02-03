using Microsoft.EntityFrameworkCore;
using TestTask.Models;
using TestTask.Repositories.Interfaces;

namespace TestTask.Repositories
{
    public class DataRepository : BaseRepository<DataItem>, IDataRepository
    {
        private readonly ApplicationDbContext _context;

        public DataRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task ClearDataAsync()
        {
            _context.DataItems.RemoveRange(_context.DataItems);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<DataItem>> GetDataAsync(int? codeFilter = null)
        {
            var query = _context.DataItems.AsQueryable();

            if (codeFilter.HasValue)
            {
                query = query.Where(item => item.Code == codeFilter.Value);
            }

            return await query.ToListAsync();
        }
    }
}
