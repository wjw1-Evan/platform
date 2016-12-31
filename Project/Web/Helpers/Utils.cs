using System;
using System.Security.Cryptography;
using Models.SysModels;

namespace Common
{
    /// <summary>
    /// 验证码生成器
    /// </summary>
    public static class CommonCodeGenerator
    {
        //private static readonly Random Random = new Random();

        private static Random Random
        {
            get
            {
                var randomNumberBuffer = new byte[10];
                new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
                return new Random(BitConverter.ToInt32(randomNumberBuffer, 0));
            }
        }

        /// <summary>
        /// 获取指定长度验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        public static string Generator(int length = 6)
        {
            var rd = Random.NextDouble();
            return Random.NextDouble().ToString().Substring(4, length);
        }

        /// <summary>
        /// 获取随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInt(int min = 0, int max = int.MaxValue)
        {
            return Random.Next(min, max);
        }

        /// <summary>
        /// 部分显示邮箱
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static string GenerateUnvisibleEmail(string emailAddress)
        {
            return emailAddress.Replace(emailAddress.Substring(1, emailAddress.IndexOf("@") - 2), "***");
        }
        /// <summary>
        /// 部分显示手机号
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static string GenerateUnvisiblePhone(string phoneNumber)
        {
            return phoneNumber.Replace(phoneNumber.Substring(3, 6), "******");
        }

        public static string GenerateNewPassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6) + Generator(3);
        }
    }

    public class CommonUtils
    {
        /// <summary>
        /// 显示用户名，
        /// 1   如果设置全名，则显示全名
        /// 2   没设置全名，显示用户名
        /// 3   未设置用户名，显示绑定手机
        /// 4   未绑定手机，显示绑定邮箱
        /// 5   显示系统默认用户名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserName4Dispaly(SysUser user)
        {
            return string.IsNullOrEmpty(user.FullName)
                ? (user.UserName.Contains("UnSetUserName") ? (user.PhoneNumberConfirmed
                    ? CommonCodeGenerator.GenerateUnvisiblePhone(user.PhoneNumber)
                    : (user.EmailConfirmed ? CommonCodeGenerator.GenerateUnvisibleEmail(user.Email) : "未知")) : user.UserName) : user.FullName;
        }
        /// <summary>
        /// 显示用户名，
        /// 1   如果设置全名，则显示全名
        /// 2   没设置全名，显示用户名
        /// 3   未设置用户名，显示绑定手机
        /// 4   未绑定手机，显示绑定邮箱
        /// 5   显示系统默认用户名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserName4Platform(SysUser user)
        {
            return string.IsNullOrEmpty(user.FullName)
                ? (user.UserName.Contains("UnSetUserName") ? (user.PhoneNumberConfirmed
                    ? user.PhoneNumber
                    : (user.EmailConfirmed ? user.Email : "未知")) : user.UserName) : user.FullName;
        }


    }

}