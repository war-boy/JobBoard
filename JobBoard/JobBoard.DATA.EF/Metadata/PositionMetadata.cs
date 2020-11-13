using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    public class PositionMetadata
    {
        [Required]
        [Display(Name = "Position Id")]       
        public int PositionId { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Position Title is required")]
        [Display(Name = "Position Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        [DisplayFormat(NullDisplayText = "* Please provide a description for your position")]
        [StringLength(50, ErrorMessage = "* Description must be less than 50 characters.")]
        [UIHint("Multilinetext")]
        public string JobDescription { get; set; }
    }

    [MetadataType(typeof(PositionMetadata))]
    public partial class Position
    {

    }
}

