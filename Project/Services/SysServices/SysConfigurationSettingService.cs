using System.Collections.Generic;
using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{

    public class SysConfigurationSettingService : RepositoryBase<SysConfigurationSetting>, ISysConfigurationSettingService
    {
        public SysConfigurationSettingService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

        public void AddOrUpdate(ICollection<SysConfigurationSetting> settings)
        {
            foreach (var setting in settings)
            {
                base.AddOrUpdate(setting);
            }
        }
        

        public override IQueryable<SysConfigurationSetting> GetAll()
        {
            return base.GetAll().OrderBy(a => a.SystemCode);
        }

    }
}
