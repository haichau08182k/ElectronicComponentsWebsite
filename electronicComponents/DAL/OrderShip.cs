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
    
    public partial class OrderShip
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderShip()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int id { get; set; }
        public Nullable<int> customerID { get; set; }
        public Nullable<System.DateTime> dateOrder { get; set; }
        public Nullable<System.DateTime> dateShip { get; set; }
        public Nullable<int> offer { get; set; }
        public Nullable<bool> isPaid { get; set; }
        public Nullable<bool> isCancel { get; set; }
        public Nullable<bool> isDelete { get; set; }
        public Nullable<bool> isDelivere { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public Nullable<bool> isReceived { get; set; }
        public Nullable<decimal> total { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
