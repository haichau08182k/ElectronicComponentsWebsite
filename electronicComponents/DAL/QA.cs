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
    
    public partial class QA
    {
        public int id { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> memberID { get; set; }
        public Nullable<bool> statuss { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public Nullable<System.DateTime> dateQuestion { get; set; }
        public Nullable<System.DateTime> dateAnswer { get; set; }
        public Nullable<int> employeeID { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Member Member { get; set; }
        public virtual Product Product { get; set; }
    }
}
