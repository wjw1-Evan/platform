using System.Collections.Generic;
using IServices.Infrastructure;
using Models.SysModels;

namespace IServices.ISysServices
{
    public interface ISysConfigurationSettingService : IRepository<SysConfigurationSetting>
    {
        void AddOrUpdate(ICollection<SysConfigurationSetting> settings);
    }
}
