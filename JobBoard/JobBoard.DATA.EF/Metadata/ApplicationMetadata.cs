using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    class ApplicationMetadata
    {
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public int OpenPositionId { get; set; }

        [Display(Name ="Date Submitted")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "Not Provided")]
        public System.DateTime ApplicationDate { get; set; }
       
        public string ManagerNotes { get; set; }

        
        public int ApplicationStatusId { get; set; }

        public string ResumeFilename { get; set; }
    }

    [MetadataType(typeof(ApplicationMetadata))]
    public partial class Application { }
}
