using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SyncMP3App.Data
{
    public class SyncMp3AppContextFactory : IDesignTimeDbContextFactory<SyncMp3AppContext>
    {
        public SyncMp3AppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SyncMp3AppContext>();
            
            // Use the same connection string as runtime
            optionsBuilder.UseSqlite(DatabaseConfig.ConnectionString);
            
            return new SyncMp3AppContext(optionsBuilder.Options);
        }
    }
}
