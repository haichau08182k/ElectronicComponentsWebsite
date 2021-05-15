using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace electronicComponents.Models
{
    public class EmployeeDetail
    {
        public int id { get; set; }

        [DisplayName("Tên tài khoản")]
        [Required(ErrorMessage = "Bạn phải nhập tên tài khoản!")]
        [StringLength(50, ErrorMessage = "Ít nhất 3 kí tự và nhiều nhất 50 kí tự", MinimumLength = 3)]
        public string userName { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Ít nhất 3 kí tự và nhiều nhất 100 kí tự", MinimumLength = 3)]
        public string passwordd { get; set; }

        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Bạn phải nhập tên nhân viên")]
        [StringLength(100, ErrorMessage = "Ít nhất 3 kí tự và nhiều nhất 100 kí tự", MinimumLength = 3)]
        public string fullName { get; set; }

        [DisplayName("Địa chỉ")]
        public string addresss { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(10, ErrorMessage = "Ít nhất 3 kí tự và nhiều nhất 10 kí tự", MinimumLength = 3)]
        public string phoneNumber { get; set; }

        [DisplayName("Hình ảnh")]
        public string imagee { get; set; }

        [DisplayName("Loại nhân viên")]
        [Required]
        [Range(1, 50)]
        public Nullable<int> employeeTypeID { get; set; }
    }
    public class EmployeeTyleDetail {

        public int id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên loại nhân viên!")]
        [DisplayName("Tên loại nhân viên")]
        public string name { get; set; }
    }
}