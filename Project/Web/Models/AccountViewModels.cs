using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoginViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Lang), Description = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [Remote("CheckUserAccountExists", "Account", ErrorMessage = "用户账号已存在")] // 远程验证（Ajax）
        public string UserName { get; set; }

        //[Required]
        //[MaxLength(11)]
        //public string PhoneNumber { get; set; }


        //[Required(ErrorMessage = "请输入验证码"), RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        //[Display(Name = "验证码")]
        //public string VerifyCode { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "邮箱")]
        //public string Email { get; set; }  

        //[Required]
        //[Display(Name = "手机号")]
        //[Phone]
        //public string PhoneNumber { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "邮箱")]
        //public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChangePasswordViewModel
    {
      
        /// <summary>
        /// 原密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]

        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入验证码"), RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}
