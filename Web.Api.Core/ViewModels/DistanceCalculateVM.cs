using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Api.Resources;

namespace Web.Api.Core.ViewModels
{
    public class DistanceCalculateVM
    {
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

    }
}
