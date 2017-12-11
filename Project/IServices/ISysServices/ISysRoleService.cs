using IServices.Infrastructure;
using Models.SysModels;
using System.Threading.Tasks;

namespace IServices.ISysServices
{
    public interface ISysRoleService : IRepository<SysRole>
    {
        Task<bool> CheckSysUserSysRoleSysControllerSysActions(string userid, string area, string action,
            string controller);

    }
}
