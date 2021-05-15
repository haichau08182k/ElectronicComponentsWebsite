using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace electronicComponents.Models
{
    public class ProducerDetail
    {
        public int id { get; set; }

        [DisplayName("Tên nhà cung cấp")]
        [Required(ErrorMessage ="Bạn cần phải nhập tên nhà cung cấp!")]
        public string name { get; set; }

        [DisplayName("Thông tin")]
        public string imformation { get; set; }

        [DisplayName("Logo")]
        public string logo { get; set; }

        [DisplayName("Còn hoạt động")]
        public Nullable<bool> isActive { get; set; }

        [DisplayName("Ngày cập nhật cuối cùng")]
        public Nullable<System.DateTime> lastUpdateDate { get; set; }
    }
}