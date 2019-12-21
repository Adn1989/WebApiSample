using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Api.Resources;

namespace Web.Api.Core.Models
{
    [Table("AppUserLogs")]
    public class AppUserCalculateLog : BaseModel
    {
        [DisplayName("کد کاربر")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        [MaxLength(12)]
        public int AppUserID { get; set; }

        [DisplayName("عرض نقطه اول")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        public double Lat1 { get; set; }

        [DisplayName("طول نقطه اول")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        public double Lon1 { get; set; }

        [DisplayName("عرض نقطه دوم")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        public double Lat2 { get; set; }

        [DisplayName("طول نقطه دوم")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        public double Lon2 { get; set; }

        [DisplayName("مسافت")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), AllowEmptyStrings = false, ErrorMessageResourceName = "Require")]
        public double Distance { get; set; }

        [DisplayName("کاربر")]
        public AppUser AppUser { get; set; }
    }
}
