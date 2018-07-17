using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class OrderPlacedModel
    {
        public IEnumerable<CustomerShoppingCart> CartInfo { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }
        public  IEnumerable<CustomerAddress> CustAdress { get; set; }
        public int CustAddressId { get; set; }
        public IEnumerable<PaymentSystemTable> PaymentSystem { get; set; }
    }
}