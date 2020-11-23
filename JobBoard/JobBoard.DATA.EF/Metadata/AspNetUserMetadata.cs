using System.ComponentModel.DataAnnotations;


namespace JobBoard.DATA.EF
{
    class AspNetUserMetadata
    {
        [DisplayFormat(NullDisplayText ="Not Provided")]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
    }

    [MetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser
    {
       
    }
}
