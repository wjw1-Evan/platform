using System.Threading.Tasks;

namespace IServices.Infrastructure
{
    public interface IUnitOfWork 
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
