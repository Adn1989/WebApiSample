using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Api.Resources;

namespace Web.Api.Core.Models
{
    [Table("AppUsers")]
    public class AppUser : BaseModel
    {
        [DisplayName("نام")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DisplayName("ایمیل")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "EmailAddressError")]
        public string Email { get; set; }

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("نام کامل")]
        [ScaffoldColumn(false)]
        public string FullName { get { return FirstName + " " + LastName; } }

        public ICollection<AppUserCalculateLog> AppUserCalculateLogs { get; set; }
    }
}
