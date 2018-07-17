using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class ViewModelCustomerAddressTable
    {
        public string UserName { get; set; }
        
        [Display(Name="Full Address")]
        public string Address { get; set; }
        
        public string State { get; set; }
        public string City { get; set; }
        
        [Display(Name="Pin Code")]
        public Nullable<decimal> PinCode { get; set; }
        
        public string AddressType { get; set; }
        
        public string Name { get; set; }
        
        [Display(Name="Mobile Number")]
        public string ModileNumber { get; set; }
        public string Type { get; set; }
        public List<CustomerAddress> CustomerAddresses { get; set; }
    }
}