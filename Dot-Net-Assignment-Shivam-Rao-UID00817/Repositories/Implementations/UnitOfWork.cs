using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
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

        /// <summary>
        /// Saves all changes to the database and updates the modification timestamp of changed entities.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The number of database records affected.</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = _db.ChangeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Releases the database context and associated resources.
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
