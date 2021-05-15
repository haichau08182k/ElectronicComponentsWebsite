using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace electronicComponents.Models
{
    public class OrderDetail
    {
        public int id { get; set; }
        [Required]
        public Nullable<int> orderID { get; set; }
        [Required]
        public Nullable<int> productID { get; set; }
        [Required]
        public Nullable<int> quantity { get; set; }
        [Required]
        public Nullable<decimal> price { get; set; }
        public Nullable<bool> isRating { get; set; }
    }

    public class OrderDetailShip
    {
        public int id { get; set; }
        public Nullable<int> customerID { get; set; }
        [Required]
        public Nullable<System.DateTime> dateOrder { get; set; }
        [Required]
        public Nullable<System.DateTime> dateShip { get; set; }
        public Nullable<int> offer { get; set; }
        public Nullable<bool> isPaid { get; set; }
        public Nullable<bool> isCancel { get; set; }
        public Nullable<bool> isDelete { get; set; }
        public Nullable<bool> isDelivere { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public Nullable<bool> isReceived { get; set; }
        [Required]
        public Nullable<decimal> total { get; set; }
    }
}