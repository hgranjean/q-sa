using System.Data.Entity;

namespace Atum.Repository.Surveillance
{
    public abstract class RepositoryBase<T>
    {
        public RepositoryBase(DbContext dbContext)
        {
            this.Context = dbContext;
        }

        public DbContext Context { get; private set; }
    }
}