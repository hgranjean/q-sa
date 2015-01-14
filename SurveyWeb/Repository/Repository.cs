using System.Data.Entity;

namespace SurveyWeb.Repository
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