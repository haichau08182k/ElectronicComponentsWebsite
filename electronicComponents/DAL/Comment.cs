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
    
    public partial class Comment
    {
        public int id { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> memberID { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> datee { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Product Product { get; set; }
    }
}
