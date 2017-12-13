using IServices.Infrastructure;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dataContext;

        public UnitOfWork(DbContext databaseFactory)
        {
            _dataContext = databaseFactory;
        }


        public Task<int> CommitAsync()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}
