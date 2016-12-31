using System.ComponentModel.DataAnnotations;

namespace Models.SysModels
{
    /// <summary>
    /// 数据类型，用于后台编辑配置值时验证输入是否正确
    /// </summary>
    public enum SettingDataType
    {
        字符串=1,
        整数 = 2,
        数字 = 3,
        日期 = 4,
        日期时间 = 5

    }

    /// <summary>
    /// 系统设置
    /// </summary>
    public class SysConfigurationSetting: DbSetBase
    {
        [MaxLength(100)]
        public string SystemCode { get; set; }
        [MaxLength(50)]
        public string SettingName { get; set; }
        [MaxLength(200),DataType(DataType.MultilineText)]
        public string SettingDescription { get; set; }
        [MaxLength(50)]
        public string SettingValue { get; set; }
        [MaxLength(50)]
        public string SettingDataType { get; set; }
        public bool Enabled { get; set; }
        
    }


    /// <summary>
    /// 账户参数,默认值 60 ，5，60，5
    /// </summary>
    public class AccountConfiguration
    {
        public AccountConfiguration()
        {
            PhoneCodeExpiration = 60;
            PhoneCodeMaxSendTimesEachday = 5;
            EmailCodeExpiration = 60;
            EmailCodeMaxSendTimesEachday = 5;
            PhonePayCodeExpiration = 2;
            PhonePayCodeMaxSendTimesEachday = 10;
        }
        /// <summary>
        /// 手机验证码有效时长,5到60分钟之间，默认60
        /// </summary>
        [Display(Name = "手机验证码有效时长(分钟数)"), Range(5, 60, ErrorMessage = "{0}必须是{1}到{2}之间的数字")]
        public int PhoneCodeExpiration { get; set; }
        /// <summary>
        /// 手机号获取验证码最多次数，5到20之间，默认5
        /// </summary>
        [Display(Name = "手机号获取验证码最多次数(每天)"), Range(5, 20, ErrorMessage = "{0}必须是{1}到{2}之间的数字")]
        public int PhoneCodeMaxSendTimesEachday { get; set; }
        /// <summary>
        /// 邮件验证码有效时长，30到120之间，默认60
        /// </summary>
        [Display(Name = "邮箱验证码有效时长(分钟数)"), Range(30, 120, ErrorMessage = "{0}必须是{1}到{2}之间的数字")]
        public int EmailCodeExpiration { get; set; }
        /// <summary>
        /// 邮箱获取验证码最多次数，5到20之间 ，默认5
        /// </summary>
        [Display(Name = "邮箱获取验证码最多次数(每天)"), Range(5, 20, ErrorMessage = "{0}必须是{1}到{2}之间的数字")]
        public int EmailCodeMaxSendTimesEachday { get; set; }


        /// <summary>
        /// 手机支付动态码有效时长,2到5分钟之间，默认2
        /// </summary>
        [Display(Name = "手机支付动态码有效时长(分钟数)",Description = "请输入2到5之间（包括2和5）的整数"), Range(2, 5, ErrorMessage = "{0}必须是{1}到{2}之间的数字")]
        public int PhonePayCodeExpiration { get; set; }
        /// <summary>
        /// 手机号获取支付动态码最多次数，5到20之间，默认5
        /// </summary>
        [Display(Name = "手机号获取支付动态码最多次数(每天)"), Range(5, 20, ErrorMessage = "{0}必须是{1}到{2}之间的数字")]
        public int PhonePayCodeMaxSendTimesEachday { get; set; }
    }

    /// <summary>
    /// 用户积分设置
    /// </summary>
    public class UserPointConfiguration
    {
        public UserPointConfiguration()
        {
            EnableUserPointSystem = true;
            RegisterPoint = 30;
            RegisterDescription = "注册成为网站用户";
            SetAvatarPoint = 5;
            SetAvatarDescription = "完成头像设置";
            SetUserNamePoint = 5;
            SetUserNameDescription = "完成用户名设置";
            SetEmailPoint = 5;
            SetEmailDescription = "完成邮箱绑定";
            SetPhonePoint = 5;
            SetPhoneDescription = "完成手机绑定";
            SetSecurityAnswersPoint = 5;
            SetSecurityAnswersDescription = "完成安全问题设置";
        }

        /// <summary>
        /// 全局设置
        /// </summary>
        [Display(Name = "启用积分系统")]
        public bool EnableUserPointSystem { get; set; }

        //功能模块设置
        #region 账户设置操作奖励积分
        [Required, Display(Name = "设置注册奖励积分"), Range(1, int.MaxValue, ErrorMessage = "奖励积分必须大于或等于{1}")]
        public int RegisterPoint { get; set; }

        [Required, MaxLength(200), Display(Name = "注册奖励积分描述")]
        public string RegisterDescription { get; set; }
        [Required,Display(Name = "设置用户名奖励积分"),Range(1, int.MaxValue,ErrorMessage = "奖励积分必须大于或等于{1}")]
        public int SetUserNamePoint { get; set; }

        [Required,MaxLength(200),Display(Name = "设置用户名奖励积分描述")]
        public string SetUserNameDescription { get; set; }
        [Required, Display(Name = "设置头像奖励积分"), Range(1, int.MaxValue, ErrorMessage = "奖励积分必须大于或等于{1}")]
        public int SetAvatarPoint { get; set; }

        [Required, MaxLength(200), Display(Name = "设置头像奖励积分描述")]
        public string SetAvatarDescription { get; set; }

        [Required, Display(Name = "绑定邮箱奖励积分"), Range(1, int.MaxValue, ErrorMessage = "奖励积分必须大于或等于{1}")]
        public int SetEmailPoint { get; set; }
        [Required, MaxLength(200), Display(Name = "绑定邮箱奖励积分描述")]
        public string SetEmailDescription { get; set; }


        [Required, Display(Name = "绑定手机奖励积分"), Range(1, int.MaxValue, ErrorMessage = "奖励积分必须大于或等于{1}")]
        public int SetPhonePoint { get; set; }
        [Required, MaxLength(200), Display(Name = "绑定手机奖励积分描述")]
        public string SetPhoneDescription { get; set; }

        [Required, Display(Name = "设置安全问题奖励积分"), Range(1, int.MaxValue, ErrorMessage = "奖励积分必须大于或等于{1}")]
        public int SetSecurityAnswersPoint { get; set; }

        [Required, MaxLength(200), Display(Name = "设置安全问题奖励积分描述")]
        public string SetSecurityAnswersDescription { get; set; }
        #endregion
    }
}
