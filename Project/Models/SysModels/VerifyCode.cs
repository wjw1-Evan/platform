using System;
using System.ComponentModel.DataAnnotations;

namespace Models.SysModels
{
    public enum VerifyProvider
    {
        Email = 1,
        Phone = 2
    }

    public enum VerifyCodeUsageType
    {
        动态码 = 1,//用于动态码登录等敏感操作
        验证码 = 2,//绑定，解绑，更换操作验证
        手机支付动态码 = 3//支付用动态码
    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyCode
    {
        public VerifyCode()
        {
            Id = Guid.NewGuid().ToString();
            CreateUtcDateTime = DateTime.UtcNow;
            AbsoluteExpirationUtcDateTime = DateTime.UtcNow.AddMinutes(60);//默认有效时间一小时
            Deleted = false;
        }

        [MaxLength(128),Key]
        public string Id { get; set; }
        /// <summary>
        /// 发送目标
        /// </summary>
        [MaxLength(50)]
        public string Destination { get; set; }
        /// <summary>
        /// 验证方式
        /// </summary>
        public VerifyProvider VerifyProvider { get; set; }
        /// <summary>
        /// 验证码用途
        /// </summary>
        public VerifyCodeUsageType VerifyCodeUsageType { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [MaxLength(6)]
        public string Code { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime AbsoluteExpirationUtcDateTime { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateUtcDateTime { get; set; }
    }
}
