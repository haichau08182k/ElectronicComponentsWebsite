using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace electronicComponents.Models
{

    public class ProductCategoryDetailParent
    {
        public int id { get; set; }
        [DisplayName("Tên danh mục sản phẩm")]
        [Required(ErrorMessage = "Tên loại sản phẩm phải có")]
        [StringLength(100, ErrorMessage = "Tối thiểu 3 và tối thiểu 5 và tối đa 100 kí tự được cho phép ", MinimumLength = 3)]
        public string name { get; set; }
        [Display(Name = "Được xóa")]
        public Nullable<bool> isDelete { get; set; }
        [Display(Name = "Còn hoạt động")]
        public Nullable<bool> isActive { get; set; }
    }
    public class ProductCategoryDetail
    {

        public int id { get; set; }
        [DisplayName("Tên loại sản phẩm")]
        [Required(ErrorMessage = "Tên loại sản phẩm phải có")]
        [StringLength(100, ErrorMessage = "Tối thiểu 3 và tối thiểu 5 và tối đa 100 kí tự được cho phép ", MinimumLength = 3)]
        public string name { get; set; }
        [Display(Name = "Mô tả")]
        public string descriptionn { get; set; }
        [Display(Name = "Hình ảnh")]
        public string imagee { get; set; }
        [Display(Name = "Đang hoạt động")]
        public Nullable<bool> isActive { get; set; }
        [Display(Name = "Ngày cập nhật cuối cùng")]
        public Nullable<System.DateTime> lastUpdatedDate { get; set; }
        [Required]
        [Range(1, 50)]
        [Display(Name = "Tên danh mục sản phẩm")]
        public Nullable<int> parentID { get; set; }

    }
    public class ProductDetail
    {
        public int id { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Bạn chưa nhập tên sản phẩm!")]
        [StringLength(100, ErrorMessage = "Tối thiểu 3 và tối thiểu 5 và tối đa 100 kí tự", MinimumLength = 3)]
        public string name { get; set; }
        [Display(Name = "Tên loại sản phẩm")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> categoryID { get; set; }

        [Display(Name = "Hình 1")]
        public string image1 { get; set; }
        [Display(Name = "Hình 2")]
        public string image2 { get; set; }
        [Display(Name = "Hình 3")]
        public string image3 { get; set; }
        [Display(Name = "Video Review")]
        public string clipReview { get; set; }
        [Display(Name = "Giá")]
        [Required]
        [Range(typeof(decimal), "1", "200000", ErrorMessage = "Giá không hợp lệ")]
        public Nullable<decimal> price { get; set; }

        public Nullable<int> promotionPrice { get; set; }

        [Required]
        [Range(typeof(int), "1", "500", ErrorMessage = "Số lượng không hợp lệ")]
        public Nullable<int> quantity { get; set; }

        [Display(Name ="Mô tả")]
        public string descriptionn { get; set; }
        [Display(Name = "Home Flag")]
        public Nullable<bool> homeFlag { get; set; }
        [Display(Name = "Hot Flag")]
        public Nullable<bool> hotFlag { get; set; }
        [Display(Name = "Lượt xem")]
        public Nullable<int> viewCount { get; set; }
        [Display(Name = "Lượng Comment")]
        public Nullable<int> commentCount { get; set; }
        [Display(Name = "Số lượng đã bán")]

        public Nullable<int> purchaseCount { get; set; }
        [Display(Name = "Khách hàng")]

        [Required]
        [Range(1, 50)]
        public Nullable<int> supplierID { get; set; }

        [Display(Name = "Tên sản phẩm")]

        [Required]
        [Range(1, 50)]
        public Nullable<int> producerID { get; set; }
        [Display(Name = "Mới")]

        public Nullable<bool> isNew { get; set; }
        [Display(Name = "Đang hoạt động")]

        public Nullable<bool> isActive { get; set; }
        [Display(Name = "Ngày cập nhật cuối cùng")]

        public Nullable<System.DateTime> lastUpdatedDate { get; set; }
        [Display(Name = "Giảm giá")]
        [Range(typeof(int), "1", "100", ErrorMessage = "Giảm giá không hợp lệ")]
        public Nullable<int> discount { get; set; }
    }
}