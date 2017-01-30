using System.Threading.Tasks;
using IServices.Infrastructure;
using EntityFramework.Audit;

namespace Services.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _dataContext = databaseFactory.Get();
        }

        public int Commit()
        {
            return _dataContext.Commit();
        }

        public Task<int> CommitAsync()
        {
            return _dataContext.CommitAsync();
        }
    }
}