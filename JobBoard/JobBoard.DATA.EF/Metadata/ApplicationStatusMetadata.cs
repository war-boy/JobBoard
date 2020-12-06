using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    public class ApplicationStatusMetadata
    {
        
        [Display(Name = "Application Status Id")]
        public int ApplicationStatusId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string StatusName { get; set; }

        
        [Display(Name = "Application Status Description")]
        [UIHint("Multilinetext")]
        public string StatusDescription { get; set; }
    }

    [MetadataType(typeof(ApplicationStatusMetadata))]
    public partial class ApplicationStatu
    {

    }



}
