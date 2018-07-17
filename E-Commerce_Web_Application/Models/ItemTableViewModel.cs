using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class ItemTableViewModel
    {
        [Required]
        [Display(Name="Enter Product Id*")]
        public int ItemId { get; set; }
        [Required]
        [Display(Name = "Enter Product Name*")]
        public string ItemName { get; set; }
        [Required]
        [Display(Name = "Enter Product Brand Name*")]
        public string ItemBrandName { get; set; }
        [Required]
        [Display(Name = "Enter Product Brand ID*")]
        public Nullable<int> ItemBrandId { get; set; }
        [Required]
        [Display(Name = "Enter Product Type Name*")]
        public string ProductTypeName { get; set; }
        [Required]
        [Display(Name = "Enter Product Type ID*")]
        public Nullable<int> ProductTypeId { get; set; }
        [Required]
        [Display(Name = "Select Image*")]
        public byte[] ItemImage { get; set; }
        [Required]
        [Display(Name = "Enter Image Id*")]
        public Nullable<int> ItemImageId { get; set; }
        [Required]
        [Display(Name = "Enter Product Price*")]
        public Nullable<decimal> ItemPrice { get; set; }
        [Display(Name = "Enter Product Discount %")]
        public Nullable<decimal> ItemDiscountPriceInPercentage { get; set; }
    }
}