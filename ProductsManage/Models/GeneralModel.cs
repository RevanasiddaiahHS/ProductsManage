using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ProductsManage.Models
{
    public class GeneralModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class Login
    {
        public int id { set; get; }
        public string EmailID { set; get; }

        public string Password { set; get; }
    }
   public class ProductInfo
    {
        public int id { set; get; }
        public int userid { set; get; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalAmount { get; set; }

    }
}
