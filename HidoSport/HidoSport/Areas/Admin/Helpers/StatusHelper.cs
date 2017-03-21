using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HidoSport.Areas.Admin.Helpers
{
    public class StatusHelper
    {
        public static string ConverStatus(int? status)
        {
            if (status == 1)
                return "Hoạt động";
            return "Khóa";
        }
        public static string ConverStatusHighlight(int? status)
        {
            if (status == 1)
                return "Nổi bật";
            return "Bình thường";
        }
        public static string ConverStatusCus(int? status)
        {
            if (status == 1)
                return "Đã xử lý";
            return "Chưa xử lý";
        }
    }
}