using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace electronicComponents.Models
{
    public class StatisticDetail
    {
        public int id { get; set; }
        [DisplayName("Loại thống kê")]
        [Required]
        public Nullable<int> statisticTypeID { get; set; }
        [DisplayName("Mã nhân viên")]
        [Required]
        public Nullable<int> employeeID { get; set; }
        [DisplayName("Ngày")]
        [Required(ErrorMessage ="Bạn phải chọn ngày thống kê!")]
        public Nullable<System.DateTime> datee { get; set; }
    }
    public class StatisticTypeDetail
    {
        public int id { get; set; }
        [DisplayName("Tên loại thống kê")]
        [Required(ErrorMessage ="Bạn phải nhập tên loại thống kê!")]
        public string name { get; set; }
    }
}