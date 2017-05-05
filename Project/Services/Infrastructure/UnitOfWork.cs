using System.Data.Entity;
using System.Threading.Tasks;
using IServices.Infrastructure;

namespace Services.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dataContext;

        public UnitOfWork(DbContext databaseFactory)
        {
            _dataContext = databaseFactory;
        }

        public int Commit()
        {
            return _dataContext.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}