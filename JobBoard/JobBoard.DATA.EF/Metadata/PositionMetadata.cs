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
        [StringLength(30, ErrorMessage = "* Position Title must be less than 30 characters.")]
        [Display(Name = "Position Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        [DisplayFormat(NullDisplayText = "* Please provide a description for your position")]
        [UIHint("Multilinetext")]
        public string JobDescription { get; set; }
    }

    [MetadataType(typeof(PositionMetadata))]
    public partial class Position
    {

    }
}

