using IServices.Infrastructure;
using Models.SysModels;
using System.Threading.Tasks;

namespace IServices.ISysServices
{
    public interface ISysUserService : IRepository<SysUser>
    {
    }


    /// <summary>
    /// 用户等级配置数据
    /// </summary>
    public interface IUserGradeConfigurationService : IRepository<UserGradeConfiguration>
    {
    }
    /// <summary>
    /// 用户积分规则配置数据
    /// </summary>
    public interface IUserPointSettingService : IRepository<UserPointSetting>
    {
        /// <summary>
        /// 根据系统编码获取积分规则
        /// </summary>
        /// <returns></returns>
        UserPointSetting GetBySystemId(string systemId);
    }

    /// <summary>
    /// 积分数据服务
    /// </summary>
    public interface IUserPointService : IRepository<UserPoint>
    {
        /// <summary>
        /// 获取指定用户的总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> Sum4Awarded(string userId);
        /// <summary>
        /// 获取指定用户的可用积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> Balance(string userId);

        /// <summary>
        /// 积分操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        UserPoint RewardPoint(UserPoint item);

        /// <summary>
        /// 只奖励一次
        /// </summary>
        /// <param name="item"></param>
        bool RewardPointOnce(UserPoint item);

        /// <summary>
        /// 每天至多奖励 MaxTimes次
        /// </summary>
        /// <param name="item"></param>
        /// <param name="maxTimes">从相应配置中获取</param>
        bool RewardPointTimesCheck4EachDay(UserPoint item, int maxTimes = 1);
        /// <summary>
        /// 每天至多奖励 MaxPoints 积分
        /// </summary>
        /// <param name="item"></param>
        /// <param name="maxPoints">从相应配置中获取</param>
        bool RewardPointPointsCheck4EachDay(UserPoint item, int maxPoints);

        /// <summary>
        /// 撤销奖励的积分
        /// </summary>
        /// <param name="pointSystemCode">积分配置系统编号</param>
        /// <param name="itemId">操作对象Id</param>
        void CancelRewardPoint(string pointSystemCode, string itemId);
        /// <summary>
        /// 积分消费
        /// </summary>
        /// <param name="item">UserPoint{积分值必须是负数，如果用户积分不够扣减则扣减失败，返回false，消费描述不能为空，关联对象也不能为空}</param>
        /// <returns></returns>
        Task<bool> ConsumePoint(UserPoint item);
    }



}