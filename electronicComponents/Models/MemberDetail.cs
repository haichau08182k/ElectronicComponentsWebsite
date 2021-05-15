using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace electronicComponents.Models
{
    public class MemberDetail
    {
        public int id { get; set; }

        [DisplayName("Tên loại thành viên")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> memberCategoryID { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn cần phải nhập tên nhà cung cấp!")]
        [StringLength(50,ErrorMessage ="Tối thiểu 3 kí tự và tối đa 50 kí tự",MinimumLength =3)]
        public string userName { get; set; }

        [DisplayName("Mật khẩu")]
        [Required]
        [StringLength(100, ErrorMessage = "Tối thiểu 3 kí tự và tối đa 100 kí tự", MinimumLength = 3)]
        public string passwordd { get; set; }

        [DisplayName("Họ tên")]
        [Required(ErrorMessage ="Bạn phải nhập họ tên!")]
        [StringLength(100, ErrorMessage = "Tối thiểu 3 kí tự và tối đa 100 kí tự", MinimumLength = 3)]
        public string fullName { get; set; }

        [DisplayName("Địa chỉ")]
        [Required]
        public string addresss { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(10, ErrorMessage = "Tối thiểu 3 kí tự và tối đa 10 kí tự", MinimumLength = 3)]
        public string phoneNumber { get; set; }

        [DisplayName("Xác nhận email")]
        public bool emailConfirmed { get; set; }

        [DisplayName("Capcha")]
        public string capcha { get; set; }
    }
    public class MemberCategoryDetaail {
        public int id { get; set; }
        [DisplayName("Tên loại thành viên")]
        [Required(ErrorMessage = "Bạn phải nhập tên loại thành viên!")]
        [StringLength(100, ErrorMessage = "Tối thiểu 3 kí tự và tối đa 100 kí tự", MinimumLength = 3)]
        public string name { get; set; }
    }
    public class MemberDiscountCodeDetail
    {
        public int id { get; set; }

        [DisplayName("Tên loại thành viên")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> memberID { get; set; }
        [DisplayName("Code giảm giá")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> discountCodeDetailID { get; set; }
    }
}