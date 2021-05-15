using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace electronicComponents.Models
{
    public class SupplierDetail
    {
        public int id { get; set; }
        [DisplayName("Tên nhà cung cấp")]
        [Required(ErrorMessage ="Bạn cần phải nhập tên nhà cung cấp!")]
        public string name { get; set; }
        [DisplayName("Địa chỉ")]
        public string addresss { get; set; }
        [DisplayName("Số điện thoại")]
        [StringLength(10,ErrorMessage ="Tối thiểu 3 và tối đa 10 kí tự!",MinimumLength =3)]
        public string phoneNumber { get; set; }
        [DisplayName("Email")]
        public string email { get; set; }
        [DisplayName("Còn hoạt động")]
        public Nullable<bool> isActive { get; set; }
        [DisplayName("Ngày cập nhật cuối cùng")]
        public Nullable<System.DateTime> lastUpdatedDate { get; set; }
    }
}