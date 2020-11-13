using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    public class LocationMetadata
    {
        [Required]
        [Display(Name = "Location Id")]
        public int LocationId { get; set; }

        [Required]
        [Display(Name = "Store Number")]
        [DisplayFormat(NullDisplayText = "* Please enter your Store Number")]
        public string StoreNumber { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Please enter your Store's City")]
        public string City { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Please enter your Store's State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Manager ")]
        public string ManagerId { get; set; }
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {

    }
}
