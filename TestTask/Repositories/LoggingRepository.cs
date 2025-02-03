using Microsoft.EntityFrameworkCore;
using TestTask.Models;
using TestTask.Repositories.Interfaces;

namespace TestTask.Repositories
{
    public class LoggingRepository : BaseRepository<LogEntryModel>, ILoggingRepository
    {
        private readonly ApplicationDbContext _context;

        public LoggingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
