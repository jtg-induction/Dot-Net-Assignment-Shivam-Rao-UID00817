using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Restaurant_ManagementContext _db;

        public UnitOfWork(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public async Task<int> SaveChangesAsync()
        {
            var entries = _db.ChangeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
