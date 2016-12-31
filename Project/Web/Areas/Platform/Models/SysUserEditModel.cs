using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Platform.Models
{
    public class SysUserEditModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; }


        [Display(Name = "EnterpriseName")]
        [Required]
        public string[] SysEnterprisesId { get; set; }



        [Required]
        [DataType("SystemId")]
        [Display(Name = "Department")]
        public string DepartmentId { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [MaxLength(100)]
        public string FullName { get; set; }

    

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [RegularExpression("1[34578][0-9]{9}")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "RoleName")]
        [Required]
        public string[] SysRolesId { get; set; }


    }
}