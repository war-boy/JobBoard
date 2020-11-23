using System.ComponentModel.DataAnnotations;

namespace JobBoard.DATA.EF
{
    public class UserDetailMetadata
    {
        [Required]
        [Display(Name = "Employee Id")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Resume")]
        public string ResumeFileName { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Please Choose an Option")]
        [Display(Name = "Open to relocation?")]
        public bool IsOpenToRelocation { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Open to what employment types?")]
        public string EmploymentType { get; set; }

        [Display(Name = "Visa Status")]
        public string VisaStatus { get; set; }

        
        [Display(Name = "Date of Hire")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime? DateOfHire { get; set; }

        [UIHint("Multilinetext")]
        public string Notes { get; set; }

        [Display(Name = "Employee Image")]
        public string UserImage { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "* Please Choose an Option")]
        public bool IsOpenToNewOpps { get; set; }

    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail 
    {
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
