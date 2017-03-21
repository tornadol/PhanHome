using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HidoSport.Areas.Admin.Models
{
    public class RequestModel
    {
        public List<DataAccess.Request> LstItem { get; set; }
        public string OriginLink { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
        public List<int> Pagination { get; set; }
    }
}