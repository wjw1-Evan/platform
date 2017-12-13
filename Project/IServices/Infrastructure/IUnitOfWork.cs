using System.Threading.Tasks;

namespace IServices.Infrastructure
{
    public interface IUnitOfWork
    {

        Task<int> CommitAsync();
    }
}
