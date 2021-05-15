using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace electronicComponents.Models
{
    public class ImportCoupon
    {
        public int id { get; set; }

        [DisplayName("Ngày")]
        public Nullable<System.DateTime> datee { get; set; }


        [DisplayName("Đã xóa")]

        public Nullable<bool> isDelete { get; set; }

        [DisplayName("Nhân viên")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> employeeID { get; set; }

        [DisplayName("Nhà cung cấp")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> supplierID { get; set; }
    }
    public class ImportCouponDetail
    {
        public int id { get; set; }


        [DisplayName("Phiếu giảm giá")]
        public Nullable<int> importCouponID { get; set; }

        [DisplayName("Sản phẩm")]
        [Range(1,50)]
        [Required]
        public Nullable<int> productID { get; set; }
        [DisplayName("Giá")]
        [Range(typeof(decimal), "1", "200000", ErrorMessage = "Giá không hợp lệ!")]
        public Nullable<decimal> price { get; set; }
        [DisplayName("Số lượng")]
        [Range(typeof(int), "1", "500", ErrorMessage = "Số lượng không hợp lệ!")]
        public Nullable<int> quantity { get; set; }
    }
}