using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    public class PerformanceReviewMetadata
    {
        [Display(Name = "Performance Review Id")]
        public int PerformanceReviewId { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Location is required")]
        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Required]
        [Display(Name = "Date of Review")]
        [DisplayFormat(NullDisplayText = "* Date of Review is required", DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime? ReviewDate { get; set; }

        [Required]
        [Display(Name = "Rating")]
        [DisplayFormat(NullDisplayText = "* Performance Rating is required")]
        public string PerformanceRating { get; set; }

        [UIHint("Multilinetext")]
        [Display(Name = "Review Notes")]
        public string ReviewDetails { get; set; }
    }

    [MetadataType(typeof(PerformanceReviewMetadata))]
    public partial class PerformanceReview
    {

    }
}
