//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace electronicComponents.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            this.ImportCoupons = new HashSet<ImportCoupon>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string addresss { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> lastUpdatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportCoupon> ImportCoupons { get; set; }
    }
}
