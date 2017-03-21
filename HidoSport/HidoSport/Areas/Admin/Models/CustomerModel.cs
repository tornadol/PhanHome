using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace HidoSport.Areas.Admin.Models
{
    public class CustomerModel
    {
        public List<DataAccess.Customer> LstItem { get; set; }
        public string OriginLink { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
        public List<int> Pagination { get; set; }
    }
    public class DetailCustomer
    {
        public DataAccess.Customer Cus { get; set; }
        public List<DataAccess.CustomerBuy> Cusbuy { get; set; }
    }
}