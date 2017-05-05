using System.Data.Entity;
using System.Threading.Tasks;
using IServices.Infrastructure;
using EntityFramework.Audit;

namespace Services.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _dataContext = databaseFactory.Get();
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