//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeachDigital.Administration.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HeadOffice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HeadOffice()
        {
            this.Circuits = new HashSet<Circuit>();
        }
    
        public long Id { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string Timezone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Circuit> Circuits { get; set; }
    }
}
