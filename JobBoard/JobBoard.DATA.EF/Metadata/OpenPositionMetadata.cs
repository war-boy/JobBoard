using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    public class OpenPositionMetadata
    {
        [Required]
        [Display(Name = "Req Number")]
        public int OpenPositionId { get; set; }

        [Required]
        [Display(Name = "Position")]
        [DisplayFormat(NullDisplayText = "* Please choose a position or click below to create a new position")]
        public int PositionId { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Location is required")]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Remote")]
        public bool? IsRemote { get; set; }

        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; }

    }

    [MetadataType(typeof(OpenPositionMetadata))]
    public partial class OpenPosition
    {
        
    }
}