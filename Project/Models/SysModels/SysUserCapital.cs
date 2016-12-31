using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Models.SysModels
{
    public class SysUserCapital : DbSetBase
    {
        public enum CapitalTypes
        {
            会员费=0
        }

        public SysUserCapital()
        {
            Success = true;
            ExpiryDate = DateTimeLocal.Now.AddYears(1);
            DateOfCollection = DateTimeLocal.Now;
        }

        /// <summary>
        /// 缴费用户ID
        /// </summary>
        [ForeignKey("PayUser")]
        [Display(Name = "PayUser")]
        public string PayUserId { get; set; }

        [ScaffoldColumn(false)]
        public virtual SysUser PayUser { get; set; }

        public CapitalTypes CapitalType { get; set; }

        //订单编号
        [MaxLength(128)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal TotalFee { get; set; }

         /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime DateOfCollection { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

    }
}