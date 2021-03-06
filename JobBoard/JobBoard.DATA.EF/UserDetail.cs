//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobBoard.DATA.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDetail()
        {
            this.Applications = new HashSet<Application>();
            this.Locations = new HashSet<Location>();
        }
    
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResumeFileName { get; set; }
        public bool IsOpenToRelocation { get; set; }
        public string Title { get; set; }
        public string EmploymentType { get; set; }
        public string VisaStatus { get; set; }
        public Nullable<System.DateTime> DateOfHire { get; set; }
        public string UserImage { get; set; }
        public Nullable<int> LocationId { get; set; }
        public bool IsOpenToNewOpps { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Applications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Locations { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Location Location { get; set; }
    }
}
