using System;
using System.Data.Entity;
using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;
using System.Threading.Tasks;


namespace Services.SysServices
{

    public class SysUserService : RepositoryBase<SysUser>, ISysUserService
    {

        public SysUserService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }



    public class UserGradeConfigurationService : RepositoryBase<UserGradeConfiguration>, IUserGradeConfigurationService
    {
        public UserGradeConfigurationService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {

        }

     
    }


    public class UserPointSettingService : RepositoryBase<UserPointSetting>, IUserPointSettingService
    {
        public UserPointSettingService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {

        }
      

        public UserPointSetting GetBySystemId(string systemId)
        {
            return base.GetAll().FirstOrDefault(a => a.SystemId == systemId && a.Enabled);
        }
    }


    public class UserPointService : RepositoryBase<UserPoint>, IUserPointService
    {
        private readonly IUserPointSettingService _iUserPointSettingService;
        private readonly IUserInfo _iUserInfo;
        public UserPointService(IDatabaseFactory databaseFactory, IUserInfo userInfo, IUserPointSettingService iUserPointSettingService)
            : base(databaseFactory, userInfo)
        {
            _iUserPointSettingService = iUserPointSettingService;
            _iUserInfo = userInfo;
        }

        public async Task<int> Sum4Awarded(string userId)
        {
            return await GetAll().Where(a => a.SysUserId == userId && a.Point > 0).Select(a => a.Point).DefaultIfEmpty(0).SumAsync();
        }


        public async Task<int> Balance(string userId)
        {
            return await GetAll().Where(a => a.SysUserId == userId).Select(a => a.Point).DefaultIfEmpty(0).SumAsync();
        }

        /// <summary>
        /// 积分操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public UserPoint RewardPoint(UserPoint item)
        {
            var success = false;
            var setting = _iUserPointSettingService.GetBySystemId(item.PointSystemCode);
            if (setting != null)
            {
                item.Description = setting.Description;
                item.Point = setting.PointValue;

                if (string.IsNullOrEmpty(item.SysUserId))
                    item.SysUserId = _iUserInfo.UserId;

                switch (setting.UserPointRestriction)
                {
                    case UserPointRestriction.只奖励一次:
                        success = RewardPointOnce(item);
                        break;
                    case UserPointRestriction.至多n积分:
                        success = RewardPointPointsCheck(item, setting.UserPointRestrictionValue);
                        break;
                    case UserPointRestriction.每天至多n次:
                        success = RewardPointTimesCheck4EachDay(item, setting.UserPointRestrictionValue);
                        break;
                    case UserPointRestriction.每天至多n积分:
                        success = RewardPointPointsCheck4EachDay(item, setting.UserPointRestrictionValue);
                        break;
                    case UserPointRestriction.没有限制:
                        break;
                    default:
                        Save(null, item);
                        success = true;
                        break;
                }
            }
            return success ? item : null;
        }

        //积分操作类型
        public bool RewardPointOnce(UserPoint item)
        {
            if (!GetAll().Any(a => a.SysUserId == item.SysUserId && a.PointSystemCode == item.PointSystemCode))
            {
                Save(null, item);
                return true;
            }
            return false;
        }

        public bool RewardPointTimesCheck4EachDay(UserPoint item, int maxTimes = 1)
        {
            var today = DateTime.Today;
            if (
                GetAll()
                    .Count(
                        a =>
                            a.SysUserId == item.SysUserId && a.PointSystemCode == item.PointSystemCode &&
                            DbFunctions.DiffDays(a.CreatedDate, today) == 0) < maxTimes)
            {
                Save(null, item);
                return true;
            }
            return false;
        }

        public bool RewardPointPointsCheck4EachDay(UserPoint item, int MaxPoints)
        {
            var today = DateTime.Today;
            if (
                GetAll()
                    .Where(
                        a =>
                            a.SysUserId == item.SysUserId && a.PointSystemCode == item.PointSystemCode &&
                            DbFunctions.DiffDays(a.CreatedDate, today) == 0)
                    .Select(a => a.Point).DefaultIfEmpty(0).Sum() < MaxPoints)
            {
                Save(null, item);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查封顶积分
        /// </summary>
        /// <param name="item"></param>
        /// <param name="MaxPoints"></param>
        /// <returns></returns>
        public bool RewardPointPointsCheck(UserPoint item, int MaxPoints)
        {
            if (
                GetAll()
                    .Where(
                        a =>
                            a.SysUserId == item.SysUserId && a.PointSystemCode == item.PointSystemCode)
                    .Select(a => a.Point).DefaultIfEmpty(0).Sum() < MaxPoints)
            {
                Save(null, item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 撤销积分
        /// </summary>
        /// <param name="pointSystemCode"></param>
        /// <param name="itemId"></param>
        public void CancelRewardPoint(string pointSystemCode, string itemId)
        {
            Delete(a => a.PointSystemCode == pointSystemCode && a.ItemId == itemId);
        }

        public async Task<bool> ConsumePoint(UserPoint item)
        {
            //关联对象和使用积分描述不能为空
            if (string.IsNullOrEmpty(item.SysUserId))
                item.SysUserId = _iUserInfo.UserId;
            if (!(string.IsNullOrEmpty(item.Description) || string.IsNullOrEmpty(item.ItemId)))
            {
                var balance = await Balance(item.SysUserId);
                if (item.Point < 0 && item.Point + balance >= 0) //消费的积分数值必须为空且不能超出可用积分
                {
                    Save(null, item);
                    return true;
                }
            }
            return false;
        }

    }
}