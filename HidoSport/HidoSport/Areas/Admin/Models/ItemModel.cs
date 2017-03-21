using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace HidoSport.Areas.Admin.Models
{
    public class ItemModel
    {
        public List<DataAccess.Item> LstItem { get; set; }
        public string OriginLink { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
        public List<int> Pagination { get; set; }
    }
    public class ItemEdit
    {
        public DataAccess.Item Item { get; set; }
    }
}