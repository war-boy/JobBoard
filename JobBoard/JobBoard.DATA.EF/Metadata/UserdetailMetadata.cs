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

        [Display(Name = "Open to relocation")]       
        public bool IsOpenToRelocation { get; set; }

        [DisplayFormat(NullDisplayText = "Title not Provided")]
        public string Title { get; set; }

        [Display(Name = "Employment Types")]
        public string EmploymentType { get; set; }

        [Display(Name = "Visa Status")]
        public string VisaStatus { get; set; }

        
        [Display(Name = "Date of Hire")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "Not Provided")]
        public System.DateTime? DateOfHire { get; set; }

        [Display(Name = "Employee Image")]
        public string UserImage { get; set; }

        [Display(Name = "Open to new Opportunities")]
        public bool IsOpenToNewOpps { get; set; }

    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail 
    {
        [Display(Name = "Name")]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
